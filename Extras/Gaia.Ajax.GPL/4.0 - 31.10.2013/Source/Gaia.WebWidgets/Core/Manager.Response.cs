/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2012 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gaia.WebWidgets
{
    public partial class Manager
    {
        private string _pageContent;
        private List<string> _scriptHashes;
        private StringBuilder _scriptBlocks;
        private bool _processedArrayRegistrations;
        private bool _processedExpandoRegistrations;
        private ClientScriptManager _clientScriptManager;
        
        private const string ScriptBlockKeyPrefix = "s";
        private const string ArrayRegistrationKeyPrefix = "a";
        private const string ExpandoRegistrationKeyPrefix = "e";

        private const string ViewStateFieldName = "__VIEWSTATE";
        private const string FileInclusionFieldName = "__GAIA_FILES";
        private const string ViewStateEncryptedFieldName = "__VIEWSTATEENCRYPTED";
        
        internal static readonly string ExpandoAttributeSectionMarker = "ExpandoAttributeSectionMarker";
        internal static readonly string ArrayRegistrationSectionMarker = "ArrayRegistrationSectionMarker";

        #region [ -- Regular Expressions -- ]

        private static Regex _arrayRegistrationRegex;
        private static Regex _expandoRegistrationRegex;

        private static Regex ArrayRegistrationRegex
        {
            get
            {
                return _arrayRegistrationRegex ??
                       (_arrayRegistrationRegex =
                        new Regex(@"^\s*var\s+(?<variableName>\S+)\s", RegexOptions.ExplicitCapture | RegexOptions.Compiled));
            }
        }

        private static Regex ExpandoRegistrationRegex
        {
            get
            {
                return _expandoRegistrationRegex ??
                       (_expandoRegistrationRegex =
                        new Regex(@"^\s*var\s+(?<variableName>.+?)\s", RegexOptions.ExplicitCapture | RegexOptions.Compiled));
            }
        }

        private static readonly Regex ScriptTagRegex = new Regex(
            @"<script((\s+(?<attrName>\w+)(\s*=\s*(?<attrValue>"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)>(?<content>[\S\s]*?)</script>",
            RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        #endregion

        /// <summary>
        /// Returns <see cref="ClientScriptManager"/> object used for handling <see cref="System.Web.UI.ClientScriptManager"/> during Ajax callbacks.
        /// </summary>
        public ClientScriptManager ClientScript
        {
            get { return _clientScriptManager ?? (_clientScriptManager = new ClientScriptManager(this)); }
        }

        /// <summary>
        /// Returns list of hashes of registered scripts.
        /// </summary>
        private List<string> RegisteredScriptHashes
        {
            get { return _scriptHashes ?? (_scriptHashes = new List<string>()); }
        }

        /// <summary>
        /// Post processes <paramref name="pageContent"/> and returns the result to send to the client.
        /// </summary>
        internal string ProcessPage(string pageContent)
        {
            _pageContent = pageContent;
            
            if (IsAjaxCallback)
                ProcessAjaxCallback();
            else
                ProcessNonAjaxCallback();

            return _pageContent;
        }

        /// <summary>
        /// Processes <see cref="HttpResponse"/> when the <see cref="IsAjaxCallback"/> is false.
        /// </summary>
        private void ProcessNonAjaxCallback()
        {
            ProcessScripts(false);

            string gaiaAdditions = null;
            if (HasVisibleGaiaControl)
            {
                // Write custom scripts
                var customScripts = string.Join("", CustomScripts.ToArray());

                // Write file inclusion directives
                string inclusions = null;
                var hashCodes = GetFileInclusionHashCodes();
                if (hashCodes != null)
                {
                    using (var stringWriter = new StringWriter())
                    {
                        SerializeFieldChanges(FileInclusionFieldName, hashCodes, null, stringWriter);
                        stringWriter.Flush();
                        inclusions = stringWriter.ToString();
                    }
                }

                // Write custom JavaScript
                var tuple = CurrentWriterStreamTuple;
                var stream = tuple.Stream;
                
                tuple.Writer.Flush();
                stream.Position = 0;

                string registration;
                using (var reader = new StreamReader(stream))
                {
                    registration = reader.ReadToEnd();
                }

                gaiaAdditions =
                    // todo: reconsider how we execute the loading
                    string.Format(CultureInfo.InvariantCulture,
                                  @"<script type=""text/javascript"">
                                    //<![CDATA[
                                        // Gaia engine loading
                                        Gaia.load(function() {{ {0}{1}{2} }});
                                    //]]>
                                    </script>",
                                  inclusions, registration, customScripts);
            }

            if (gaiaAdditions == null) return;

            _pageContent = _pageContent.
                Replace("</form>", "</form>" + gaiaAdditions).
                Replace("</FORM>", "</FORM>" + gaiaAdditions);
        }

        /// <summary>
        /// Processes <see cref="HttpResponse"/> when the <see cref="IsAjaxCallback"/> is true.
        /// </summary>
        private void ProcessAjaxCallback()
        {
            ProcessScripts(true);

            var data = new Dictionary<string, object>();
            
            var returnValue = MethodDispatcher.ReturnValue;
            if (returnValue != null)
                data["rv"] = returnValue;

            var scripts = Array.ConvertAll(GaiaScriptInclusions.ToArray(), incl => incl.FileName);
            if (scripts.Length > 0)
                data["js"] = scripts;

            if (StyleSheets.Count > 0)
                data["css"] = StyleSheets;

            using (var writer = new StringWriter())
            {
                if (data.Count > 0)
                {
                    var json = Json.SimpleJson.SerializeObject(data);
                    writer.Write(json);
                    writer.Write(";;;;");
                }

                // Writing out commands to persist script and stylesheet inclusion hash codes to page
                WriteFileInclusionPersistenceDirective(writer);

                // Then we need to FLUSH the Writer object
                Writer.Flush();

                // Writing custom JavaScript to output
                WriteCustomJavaScriptToAjaxCallbackStream(writer);

                // Reading complete HTML
                ExtractAndWriteFormStateElements(writer);

                // Then the "append at back" content
                writer.Write(string.Join("", CustomScripts.ToArray()));

                writer.Flush();
                _pageContent = writer.ToString();
            }
        }

        /// <summary>
        /// Processes HTML script tags on the rendered page.
        /// </summary>
        private void ProcessScripts(bool isCallback)
        {
            var matches = ScriptTagRegex.Matches(_pageContent);

            foreach (Match match in matches)
            {
                var attrNames = match.Groups["attrName"];
                var attrValues = match.Groups["attrValue"];
                var scriptContent = match.Groups["content"];

                var hasSource = false;
                if (attrNames.Success && attrValues.Success)
                {
                    var nameCaptures = attrNames.Captures;
                    var valueCaptures = attrValues.Captures;

                    for (var index = 0; index < nameCaptures.Count; ++index)
                    {
                        var attrName = nameCaptures[index].Value;
                        var attrValue = valueCaptures[index].Value;

                        hasSource = attrName.ToLowerInvariant() == "src" && !string.IsNullOrEmpty(attrValue);
                        if (!hasSource) continue;

                        ProcessScriptSource(attrValue, isCallback);
                        break;
                    }
                }

                if (hasSource || !scriptContent.Success) continue;
                ProcessScriptBlock(match, scriptContent.Value.Trim(), isCallback);
            }

            if (_scriptBlocks != null)
                ExecuteScript(_scriptBlocks.ToString());

            // remove markers during postback
            if (isCallback) return;

            var markerStart = _pageContent.LastIndexOf(ArrayRegistrationSectionMarker, StringComparison.Ordinal);
            if (markerStart > -1)
            {
                var lineStart = _pageContent.LastIndexOf(Environment.NewLine, markerStart, StringComparison.Ordinal);
                var lineEnd = _pageContent.IndexOf(Environment.NewLine, markerStart, StringComparison.Ordinal);
                _pageContent = _pageContent.Remove(lineStart, lineEnd - lineStart + 1);
            }

            markerStart = _pageContent.LastIndexOf(ExpandoAttributeSectionMarker, StringComparison.Ordinal);
            if (markerStart > -1)
            {
                var lineEnd = _pageContent.IndexOf(Environment.NewLine, markerStart, StringComparison.Ordinal);

                markerStart = _pageContent.LastIndexOf(ExpandoAttributeSectionMarker, markerStart, StringComparison.Ordinal);
                var lineStart = _pageContent.LastIndexOf(Environment.NewLine, markerStart, StringComparison.Ordinal);
                
                _pageContent = _pageContent.Remove(lineStart, lineEnd - lineStart + 1);
            }
        }

        /// <summary>
        /// Processes script tag with src attribute equal to the specified <paramref name="fileName"/>.
        /// </summary>
        private void ProcessScriptSource(string fileName, bool isCallback)
        {
            if (IsFileRegistered(fileName))
                return;

            if (isCallback)
            {
                var path = fileName.Trim('"');
                AddInclusionOfFile(path, typeof (Manager), path, "self");
            }
            else
                GaiaScriptInclusions.Add(new ScriptFileInfo(fileName, "self"));
        }

        /// <summary>
        /// Processes specified <paramref name="blockCapture"/>.
        /// </summary>
        private void ProcessScriptBlock(Capture blockCapture, string scriptContent, bool isCallback)
        {
            if (string.IsNullOrEmpty(scriptContent) || scriptContent.Contains("// Gaia control registration"))
                return;

            if (!_processedArrayRegistrations && ProcessArrayRegistrations(scriptContent, isCallback))
            {
                _processedArrayRegistrations = true;
                return;
            }

            if (!_processedExpandoRegistrations && ProcessExpandoRegistrations(scriptContent, isCallback))
            {
                _processedExpandoRegistrations = true;
                return;
            }

            var key = CreateScriptBlockIncludeKey(blockCapture.Value);
            if (RegisteredScriptHashes.Contains(key))
                return;

            if (isCallback && (!IsPreviouslyRegisteredKey(key) || IsUpdatableScriptBlock(blockCapture)))
            {
                var lines = scriptContent.Split(new[] {"\n", "\r", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                var scriptBlock = string.Join("\n", lines);

                if (_scriptBlocks == null)
                    _scriptBlocks = new StringBuilder(scriptBlock);
                else
                    _scriptBlocks.Append(scriptBlock);
            }

            RegisteredScriptHashes.Add(key);
        }

        /// <summary>
        /// Processes array registrations inside specified <paramref name="scriptContent"/>.
        /// </summary>
        /// <returns>True if specified <paramref name="scriptContent"/> had array registrations and they were processed successfully.</returns>
        private bool ProcessArrayRegistrations(string scriptContent, bool isCallback)
        {
            if (!scriptContent.Contains(ArrayRegistrationSectionMarker))
                return false;

            var builder = new StringBuilder();
            var registrations = scriptContent.Split(new[] { "\n", "\r\n", "\r" },
                                                    StringSplitOptions.RemoveEmptyEntries);

            foreach (var registration in registrations)
            {
                var match = ArrayRegistrationRegex.Match(registration);
                if (!match.Success)
                    continue;

                var arrayName = match.Groups["variableName"].Value;
                if (arrayName == ArrayRegistrationSectionMarker)
                    continue;

                var registerArray = true;
                var registrationKey = CreateArrayDeclarationKey(registration);
                if (isCallback)
                {
                    if (IsPreviouslyRegisteredKey(registrationKey))
                        registerArray = false;

                    if (IsUpdatableArrayRegistrations(arrayName))
                        builder.Append(registration).Append("\n");
                }

                if (registerArray)
                    AddScriptForClientSideEval(string.Concat(Constants.GaiaClientModule, @".RA(""", registrationKey,
                                                             @""",""", arrayName, @""");"));

                RegisteredScriptHashes.Add(registrationKey);
            }

            // remove previously declared and currently removed arrays
            foreach(var registrationKey in PreviousInclusionHashCodes)
            {
                if (!registrationKey.StartsWith(ArrayRegistrationKeyPrefix) || RegisteredScriptHashes.Contains(registrationKey))
                    continue;

                AddScriptForClientSideEval(string.Concat(Constants.GaiaClientModule, @".DA(""", registrationKey, @""");"));
            }

            if (builder.Length > 0)
                ExecuteScript(builder.ToString());

            return true;
        }

        /// <summary>
        /// Processes expando attribute registrations inside specified <paramref name="scriptContent"/>.
        /// </summary>
        /// <returns>True if specified <paramref name="scriptContent"/> had expando attribute registrations and they were processed successfully.</returns>
        private bool ProcessExpandoRegistrations(string scriptContent, bool isCallback)
        {
            if (!scriptContent.Contains(ExpandoAttributeSectionMarker))
                return false;

            var blockName = (string) null;
            var builder = new StringBuilder();
            var appendLineToBlockBuilder = false;
            var blockBuilder = new StringBuilder();
            var lines = scriptContent.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (line.StartsWith("//"))
                    continue;

                var match = ExpandoRegistrationRegex.Match(line);
                if (match.Success)
                {
                    ProcessExpandoRegistration(blockName, blockBuilder, builder, isCallback);
                    
                    blockBuilder = new StringBuilder();
                    blockName = match.Groups["variableName"].Value;
                    appendLineToBlockBuilder = blockName != ExpandoAttributeSectionMarker;
                }

                if (appendLineToBlockBuilder)
                    blockBuilder.Append(line).Append("\n");
            }

            ProcessExpandoRegistration(blockName, blockBuilder, builder, isCallback);

            if (builder.Length > 0)
                ExecuteScript(builder.ToString());

            return true;
        }

        /// <summary>
        /// Processes expando registration of a control having specified <paramref name="blockName"/>.
        /// </summary>
        /// <param name="blockName">Name of the control which has expando attributes.</param>
        /// <param name="blockBuilder">Builder containing expando attribute definitions.</param>
        /// <param name="builder">Builder containing all expando attribute registrations.</param>
        /// <param name="isCallback">Defines if the method is called during an ajax callback.</param>
        private void ProcessExpandoRegistration(string blockName, StringBuilder blockBuilder, StringBuilder builder, bool isCallback)
        {
            if (blockBuilder.Length <= 0)
                return;

            var block = blockBuilder.ToString();

            var registrationKey = CreateScriptBlockIncludeKey(block);
            RegisteredScriptHashes.Add(registrationKey);

            if (!isCallback || (IsPreviouslyRegisteredKey(registrationKey) && !IsUpdatableExpandoControlId(blockName)))
                return;

            builder.Append(block);
        }

        /// <summary>
        /// Returns true if the <paramref name="key"/> is contained in the <see cref="PreviousInclusionHashCodes"/>.
        /// </summary>
        private bool IsPreviouslyRegisteredKey(string key)
        {
            return PreviousInclusionHashCodes.Contains(key);
        }

        /// <summary>
        /// Return true if the expando attribute registrations for the <paramref name="controlid"/> need to be updated.
        /// </summary>
        private bool IsUpdatableExpandoControlId(string controlid)
        {
            return _clientScriptManager != null && _clientScriptManager.IsUpdatableExpandoControlId(controlid);
        }

        /// <summary>
        /// Return true if the specified <paramref name="blockCapture"/> needs to be updated.
        /// </summary>
        private bool IsUpdatableScriptBlock(Capture blockCapture)
        {
            return _clientScriptManager != null &&
                   _clientScriptManager.IsInsideUpdatableScriptBlock(_pageContent, blockCapture);
        }

        /// <summary>
        /// Return true if the array registration with the specified <paramref name="arrayName"/> needs to be updated.
        /// </summary>
        private bool IsUpdatableArrayRegistrations(string arrayName)
        {
            return _clientScriptManager != null && _clientScriptManager.IsUpdatableArrayName(arrayName);
        }

        /// <summary>
        /// Executes specified <paramref name="scriptContent"/>.
        /// </summary>
        private void ExecuteScript(string scriptContent)
        {
            AddScriptForClientSideEval(Constants.GaiaClientModule + ".ES('" +
                                       HtmlFormatter.FormatHtmlForInnerHTML(scriptContent) + "');");
        }

        /// <summary>
        /// Returns key for identifying an array declaration.
        /// </summary>
        private static string CreateArrayDeclarationKey(string arrayDeclaration)
        {
            return ArrayRegistrationKeyPrefix + arrayDeclaration.GetHashCode().ToString(NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Returns a key used for identifying a script block inclusion.
        /// </summary>
        private static string CreateScriptBlockIncludeKey(string scriptBlock)
        {
            return ScriptBlockKeyPrefix + scriptBlock.GetHashCode().ToString(NumberFormatInfo.InvariantInfo);
        }        

        /// <summary>
        /// Returns concatenated string representation of hash codes of included files.
        /// </summary>
        private string GetFileInclusionHashCodes()
        {
            var hashCodes = new List<string>();

            hashCodes.AddRange(
                StyleSheets.ConvertAll(
                    styleSheet => styleSheet.GetHashCode().ToString(NumberFormatInfo.InvariantInfo)));

            hashCodes.AddRange(
                GaiaScriptInclusions.ConvertAll(
                    scriptFileInfo =>
                    scriptFileInfo.FileName.GetHashCode().ToString(NumberFormatInfo.InvariantInfo)));

            hashCodes.AddRange(
                RegisteredScriptHashes.ConvertAll(
                    scriptHash => scriptHash.ToString(NumberFormatInfo.InvariantInfo)));

            return hashCodes.Count == 0 ? null : string.Join("$", hashCodes.ToArray());
        }

        private static void SerializeFieldChanges(string fieldName, string currentState, string initialState, TextWriter writer)
        {
            if (initialState == currentState) return;

            int index;
            var suffix = Utilities.GenerateDifference(initialState, currentState, out index);

            string arg;
            if (index != -1)
                arg = string.Concat(",'", suffix, "',", index);
            else if (currentState != null)
                arg = string.Concat(",'", currentState, "'");
            else
                arg = string.Empty;

            writer.Write(string.Concat("\r\n$FCg('", fieldName, "'", arg, ");"));
        }

        private void WriteFileInclusionPersistenceDirective(TextWriter writer)
        {
            var hashCodes = GetFileInclusionHashCodes();
            if (hashCodes == null) return;

            var initialValue = HttpContext.Current.Request.Params[FileInclusionFieldName];
            var initialValues = initialValue.Split('$');
            var initialHashes = new List<string>(initialValues.Length);
            
            // remove previously registered script block keys
            foreach (var value in initialValue.Split('$'))
            {
                if (value.StartsWith(ScriptBlockKeyPrefix) ||
                    value.StartsWith(ArrayRegistrationKeyPrefix) ||
                    value.StartsWith(ExpandoRegistrationKeyPrefix))
                    continue;
                
                initialHashes.Add(value);
            }

            var fieldValue = string.Join("$", initialHashes.ToArray()) + "$" + hashCodes;
            SerializeFieldChanges(FileInclusionFieldName, fieldValue, initialValue, writer);
        }

        private void WriteCustomJavaScriptToAjaxCallbackStream(TextWriter writer)
        {
            var stream = CurrentWriterStreamTuple.Stream;
            
            stream.Position = 0;
            using (TextReader reader = new StreamReader(stream))
            {
                writer.Write(reader.ReadToEnd());
            }
        }

        private void ExtractAndWriteFormStateElements(TextWriter writer)
        {
            var parameters = HttpContext.Current.Request.Params;
            var initialViewState = parameters[ViewStateFieldName];
            var initialEncryptedViewState = parameters[ViewStateEncryptedFieldName];

            // Writing updated StateUtil, EventValidation and so on...
            ExtractAndSerializeFieldChanges(_pageContent, ViewStateFieldName, initialViewState, writer);
            ExtractAndSerializeFieldChanges(_pageContent, ViewStateEncryptedFieldName, initialEncryptedViewState, writer);
            ExtractAndSerializeFieldChanges(_pageContent, "__EVENTVALIDATION", null, writer);
            ExtractAndSerializeFieldChanges(_pageContent, "__PREVIOUSPAGE", null, writer);
        }

        private static void ExtractAndSerializeFieldChanges(string pageContent, string fieldName, string initialState, TextWriter writer)
        {
            var stateValue = ExtractStateElement(pageContent, fieldName);
            if ((stateValue != null && stateValue != initialState) ||
                (stateValue == null && initialState != null))
            {
                SerializeFieldChanges(fieldName, stateValue, initialState, writer);
            }
        }

        private static string ExtractStateElement(string completeHtml, string whatToLookFor)
        {
            var stateStart = string.Format(CultureInfo.InvariantCulture,
                                              "<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"", whatToLookFor);
            var index = completeHtml.IndexOf(stateStart, StringComparison.Ordinal);
            if (index != -1)
            {
                const string stateEnd = "\" />";
                var state = completeHtml.Substring(index + stateStart.Length);
                return state.Substring(0, state.IndexOf(stateEnd, StringComparison.Ordinal));
            }

            return null;
        }
    }
}