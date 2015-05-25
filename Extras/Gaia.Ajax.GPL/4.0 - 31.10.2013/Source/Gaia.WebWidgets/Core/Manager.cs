/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Text;
using System.Web.Configuration;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

// Including all the "common" scripts
[assembly: WebResource("Gaia.WebWidgets.LibraryScripts.jquery.js", "text/javascript")]
[assembly: WebResource("Gaia.WebWidgets.LibraryScripts.jsface.js", "text/javascript")]
[assembly: WebResource("Gaia.WebWidgets.LibraryScripts.jquery-ui.js", "text/javascript")]

[assembly: WebResource("Gaia.WebWidgets.Scripts.Control.js", "text/javascript")]
[assembly: WebResource("Gaia.WebWidgets.Scripts.WebControl.js", "text/javascript")]

[assembly: WebResource("Gaia.WebWidgets.LibraryScripts.all.js", "text/javascript")]

[assembly: WebResource("Gaia.WebWidgets.Resources.jquery.ui.resizable.css", "text/css")]


namespace Gaia.WebWidgets
{
    /// <summary>
    /// This is the "Manager" of Gaia Ajax Widgets.
    /// Basically contains lots of nice to have methods like IsAjaxCallback end so on.
    /// The Manager is also responsible for rendering the updates back to the HTTP Response
    /// Class is a Singleton which means you cannot instantiate object of it but you
    /// can access the only object in existence from the static Instance property of the class.
    /// </summary>
    public sealed partial class Manager
    {
        #region [ -- Private members -- ]

        private bool _pageUnloaded;
        private bool? _isAjaxCallback;
        private List<string> _styleSheets;
        private List<string> _customScripts;
        private bool _hasRenderedDefaultUrl;
        private bool _pagePreRenderComplete;
        private List<ScriptFileInfo> _inclusions;
        private List<IAjaxControl> _ajaxControls;
        private List<string> _previousInclusionHashCodes;
        private Stack<WriterStreamTuple> _stackOfWriters;
       
        private Manager() { }

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Returns the "one and only" instance object of this type for this thread. Singleton accessor property.
        /// </summary>
        public static Manager Instance
        {
            get
            {
                IDictionary items = HttpContext.Current.Items;
                if (!items.Contains("__AjaxManager__"))
                {
                    lock (typeof(Manager))
                    {
                        // double check synchronization pattern
                        if (!items.Contains("__AjaxManager__"))
                            items["__AjaxManager__"] = new Manager();
                    }
                }

                return (Manager)items["__AjaxManager__"];
            }
        }

        /// <summary>
        /// Returns true if the request is a Gaia Ajax Request
        /// </summary>
        public bool IsAjaxCallback
        {
            get
            {
                if (!_isAjaxCallback.HasValue)
                    _isAjaxCallback = HttpContext.Current.Request.Params["GaiaCallback"] == "true";

                return _isAjaxCallback.Value;
            }
        }

        /// <summary>
        /// See <see cref="GaiaAjaxConfiguration.EnableJavaScriptInclusion" /> for documentation on this method
        /// </summary>
        [Obsolete("Use of GaiaAjaxConfiguration.Instance.EnableJavaScriptInclusion is recommended instead")]
        public bool SkipJavaScriptFilesInclusion
        {
            get { return !GaiaAjaxConfiguration.Instance.EnableJavaScriptInclusion; }
            set { GaiaAjaxConfiguration.Instance.EnableJavaScriptInclusion = !value; }
        }


        /// <summary>
        /// Returns the Page that the request was for
        /// </summary>
        public Page Page { get; private set; }

        /// <summary>
        /// The Update Control to use for all your Ajax Requests.
        /// The Update Control will only be visible while there's an active Ajax Callback in action, at 
        /// any other times the whole control will be IN-visible and will not interfer with the page at all. 
        /// Normally you'd use only a Panel with some text or a couple of Labels as the Update Control to 
        /// your page. The Update Control MUST be a control which exists in the Page Controls collection.
        /// If you override the UpdateControl for specific widgets by e.g. using the AspectUpdateControl
        /// then for that control this logic will be overridden.
        /// </summary>
        public Control UpdateControl { get; set; }

        /// <summary>
        /// The ErrorHandler is a JavaScript function you declare yourself on the page that will be called with
        /// 3 parameters, the first being the STATUS from the server, e.g. 500 or 404 etc.
        /// The second one will be the string message from the server and the third one
        /// will be the complete response (HTTP "body") of the response.
        /// IF there is a connection problem (the server was unavailable) the second parameter will
        /// be; "CONNECTION_PROBLEM" and the first parameter will be; -1.
        /// If you throw an exception yourself the first will be 500 and the second will be the Exception Message, 
        /// the third will be the complete response from the server.
        /// If you return true from your error handler the execution of the page will HALT and the entire Ajax
        /// Engine will STOP and not continue raising events and requests to the server, if you return false, 
        /// null or nothing the execution of the Ajax Engine on the page will continue.
        /// If you want to silently catch all exceptions and errors you can define your own ErrorHandler that 
        /// does nothing and just returns nothing, null or false.
        /// </summary>
        /// <example>
        /// <code title="Defining a custom Error Handler (ASPX)" lang="C#"> 
        /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Core\Manager\CustomErrors\Default.aspx" />
        /// </code> 
        /// <code title="Defining a custom Error Handler (Codebehind)" lang="C#"> 
        /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Core\Manager\CustomErrors\Default.aspx.cs" region="Code" />
        /// </code> 
        /// </example>
        public string ErrorHandler { get; set; }

        /// <summary>
        /// This can override the Callback URL which your page will call when Gaia Ajax Requests are being created.
        /// Often this might be useful due to HttpHandlers or Url Rewriting Modules or similar overrides the URL 
        /// of your page and it can sometimes stop working due to this. If this is the case you often have the 
        /// possibility of retrieving those "rewritten" URLs and explicitly set the URL to postback to here.
        /// This is often useful if using Gaia in combination with e.g. CMS systems or application systems like
        /// SharePoint, DotNetNuke or EPIServer.
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Helper getter (and setter for extension control developers) to determine if there 
        /// are visible Gaia controls on the page.
        /// Should be change in or after PreRender event
        /// </summary>
        public bool HasVisibleGaiaControl { get; set; }

        /// <summary>
        /// This is the actual stream written out when the request is a Gaia Ajax Request and NOT the 
        /// Page.Response.Writer! If you need to add custom stuff into the response then THIS is the 
        /// TextWriter you should use! Note you'd probably want to use the AddScriptForClientSideEval 
        /// method instead of this one directly. Note also that no matter which one of those you use 
        /// you should be VERY careful when including your own stuff into the response since if 
        /// there's a Syntax Error on the client after returning the Response the application will 
        /// basically crash!
        /// </summary>
        public TextWriter Writer
        {
            get { return CurrentWriterStreamTuple.Writer; }
        }

        #endregion

        #region [ -- Public Methods -- ]

        /// <summary>
        /// Add a stylesheet to the page.
        /// </summary>
        /// <param name="styleSheet">The link to the stylesheet</param>
        public void AddInclusionOfStyleSheet(string styleSheet)
        {
            var hashCode = styleSheet.GetHashCode().ToString(CultureInfo.InvariantCulture);
            var isPreviouslyRegistered = IsPreviouslyRegisteredKey(hashCode);
            if (StyleSheets.Contains(styleSheet) || (isPreviouslyRegistered && IsAjaxCallback)) return;
            StyleSheets.Add(styleSheet);
        }

        /// <summary>
        /// Use this function to include a Stylesheet from an embedded resource. Useful if you include your 
        /// css files as part of the assembly as an embedded resource. 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="resourceName"></param>
        public void AddInclusionOfStyleSheetFromResource(Type type, string resourceName)
        {
            AddInclusionOfStyleSheet(Page.ClientScript.GetWebResourceUrl(type, resourceName));
        }

        /// <summary>
        /// This one basically adds up extra script values you would want to run on the client after 
        /// the response returns. Note you should be VERY careful when including your own
        /// stuff into the response since if there's a Syntax Error on the client after returning the 
        /// Response the application will basically crash!
        /// </summary>
        /// <param name="script">JavaScript to execute when the response returns to the client. Note that
        /// you should NOT add up &lt;script...&gt; elements around it since that will be done
        /// automatically by the Framework</param>
        public void AddScriptForClientSideEval(string script)
        {
            CustomScripts.Add(script);
        }

        /// <summary>
        /// Includes a JavaScript file for inclusion in the page. Works in both normal page 
        /// processing and Gaia Ajax Callbacks. Gaia can in asynchronous callback include ScriptFiles on the 
        /// client. This can be a useful construct for reducing the initial bandwidth usage 
        /// </summary>
        /// <param name="fileNamePath">Path and name of file to include</param>
        /// <param name="type">"Unique" type to ensure single inclusion of same file</param>
        /// <param name="fullName">Name of file to include, normally the same as the fileNamePath 
        /// unless this is a "resource file inclusion"</param>
        /// <param name="typeToWaitFor">This should be the LAST "type" or "function" in your JavaScript 
        /// file, this one is used to make the browser WAIT until that type is not "undefined" before 
        /// proceeding to ensure we don't get client side bugs due to JavaScript not being finished 
        /// loading before it's being referenced. Normally this would be a "dummy variable" appended at 
        /// the end of your JavaScript file</param>
        public void AddInclusionOfFile(string fileNamePath, Type type, string fullName, string typeToWaitFor)
        {
            AddInclusionOfFile(fileNamePath, type, fullName, typeToWaitFor, false);
        }

        /// <summary>
        /// Includes a file from a resource full name (complete namespace and name of file)
        /// </summary>
        /// <param name="fullName">Full name of the file (namespace and so on)</param>
        /// <param name="type">A type from the assembly to guard against multiple inclusions</param>
        /// <param name="typeToWaitFor">This should be the LAST "type" or "function" in your JavaScript 
        /// file, this one is used to make the browser WAIT until that type is not "undefined" before 
        /// proceeding to ensure we don't get client side bugs due to JavaScript not being finished 
        /// loading before it's being referenced. Normally this would be a "dummy variable" appended at 
        /// the end of your JavaScript file</param>
        public void AddInclusionOfFileFromResource(string fullName, Type type, string typeToWaitFor)
        {
            AddInclusionOfFile(Page.ClientScript.GetWebResourceUrl(type, fullName), type, fullName, typeToWaitFor, false);
        }

        ///<summary>
        ///</summary>
        /// <param name="fullName">Full name of the file (namespace and so on)</param>
        /// <param name="type">A type from the assembly to guard against multiple inclusions</param>
        /// <param name="typeToWaitFor">This should be the LAST "type" or "function" in your JavaScript</param>
        /// <param name="isPartOfCoreFiles">If the file to be included also exists in the core package of javascript files.
        /// file, this one is used to make the browser WAIT until that type is not "undefined" before 
        /// proceeding to ensure we don't get client side bugs due to JavaScript not being finished 
        /// loading before it's being referenced. Normally this would be a "dummy variable" appended at 
        /// the end of your JavaScript file</param>
        public void AddInclusionOfFileFromResource(string fullName, Type type, string typeToWaitFor, bool isPartOfCoreFiles)
        {
            AddInclusionOfFile(Page.ClientScript.GetWebResourceUrl(type, fullName), type, fullName, typeToWaitFor, isPartOfCoreFiles);
        }

        #endregion

        #region [ -- Manager Writer related methods and classes -- ]
        
        private WriterStreamTuple CurrentWriterStreamTuple
        {
            get
            {
                if (_stackOfWriters == null)
                    _stackOfWriters = new Stack<WriterStreamTuple>();

                if (_stackOfWriters.Count == 0)
                    _stackOfWriters.Push(new WriterStreamTuple());

                return _stackOfWriters.Peek();
            }
        }

        internal TextWriter PushWriter()
        {
            var topWriter = CurrentWriterStreamTuple.Writer;
            _stackOfWriters.Push(new WriterStreamTuple());
            return topWriter;
        }

        internal void PopWriter()
        {
            var tuple = _stackOfWriters.Pop();
            using (tuple.Writer)
            {
                tuple.Writer.Flush();
                tuple.Stream.Position = 0;
                using (TextReader reader = new StreamReader(tuple.Stream))
                {
                    Writer.Write(reader.ReadToEnd());
                }
            }
        }

        internal void ClearWriter()
        {
            using(CurrentWriterStreamTuple) {}
            _stackOfWriters.Pop();
            _stackOfWriters.Push(new WriterStreamTuple());
        }

        private class WriterStreamTuple : AtomicInvoker
        {
            private readonly TextWriter _writer;
            private readonly MemoryStream _stream;

            public TextWriter Writer
            {
                get { return _writer; }
            }

            public MemoryStream Stream
            {
                get { return _stream; }
            }

            public WriterStreamTuple()
            {
                _stream = new MemoryStream();
                _writer = new StreamWriter(_stream);

                Destructor = delegate
                {
                    _writer.Dispose();
                    _stream.Dispose();
                };
            }
        }

        #endregion

        #region [ -- Page Event handlers and helpers -- ]

        internal void RenderGlobalSettings()
        {
            if (IsAjaxCallback || _hasRenderedDefaultUrl) return;
            _hasRenderedDefaultUrl = true;

            string callbackUrl;
            if (string.IsNullOrEmpty(CallbackUrl))
            {
                var url = Page.Request.Url;
                callbackUrl = string.Concat(url.Scheme, "://", Page.Request.Headers["Host"],
                                            Page.Response.ApplyAppPathModifier(url.PathAndQuery));
            }
            else
                callbackUrl = CallbackUrl;

            Writer.WriteLine();
            Writer.WriteLine(Constants.GaiaClientModule + "._defaultUrl='{0}';", HtmlFormatter.FormatHtmlForInnerHTML(callbackUrl));

            var config = GaiaAjaxConfiguration.Instance;
            var zindexThreshold = config.ZIndexThreshold;
            if (zindexThreshold > -1)
                Writer.Write("Gaia.Control.ZIndexThreshold=" + zindexThreshold.ToString(NumberFormatInfo.InvariantInfo) + ";");
        }

        private void PagePreRenderComplete(object sender, EventArgs e)
        {
            EnsurePreviousInclusionHashCodesLoaded();
            ClientScript.TrackArrayAndExpandoRegistrations();

            IncludeScriptFiles();
            if (!IsAjaxCallback)
            {
                IncludeStyleSheets();
                GaiaLicenseProvider.Instance.Validate();
            }

            _pagePreRenderComplete = true;
        }

        internal IEnumerable<IAjaxControl> RegisteredAjaxControls
        {
            get { return _ajaxControls; }
        }

        private void IncludeScriptFiles()
        {
            _ajaxControls.ForEach(delegate(IAjaxControl ajaxControl)
                                      {
                                          // include script files only for controls 
                                          // which are part of current control tree
                                          // and are visible
                                          if (!ajaxControl.AjaxControl.PartOfControlTree || !ajaxControl.Control.Visible) return;
                                          ajaxControl.IncludeScriptFiles();
                                      });
        }

        private void IncludeStyleSheets()
        {
            StyleSheets.ForEach(styleSheet =>
                                    {
                                        var link = new System.Web.UI.HtmlControls.HtmlLink { Href = styleSheet };
                                        link.Attributes.Add("rel", "stylesheet");
                                        link.Attributes.Add("type", "text/css");
                                        Page.Header.Controls.Add(link);
                                    });
        }

        #endregion

        internal void RegisterAjaxControl(Page page, Control control)
        {
            var initial = Page == null;
            
            if (initial)
            {
                Page = page;
                page.Unload += OnPageUnload;
                page.PreRenderComplete += PagePreRenderComplete;
            }

            if (_ajaxControls == null)
                _ajaxControls = new List<IAjaxControl>();

            _ajaxControls.Add(control as IAjaxControl);

            // can't dispatch if too late in the lifecycle...
            if (_pageUnloaded) return;

            // Now we're checking to see if this is a call of a method
            // Checking to see WHAT type of method it is and for WHAT CONTROL (if any) the method is for!
            string paramsString = page.Request.Params["gaiaParams"];
            if (paramsString == null) return;

            // Splitting the parameters
            string[] parameters = paramsString.Split(',');

            // Now the first entry in the array contains the name of the method and the second contains the TYPE of method
            string typeOfMethod = parameters[1];

            switch (typeOfMethod)
            {
                case "PageMethod":
                    if (initial)
                    {
                        page.LoadComplete +=
                            delegate
                                {
                                    MethodDispatcher.DispatchMethodCall(
                                        new MethodDispatcher.DispatchContext(Page, parameters));
                                };
                    }
                    break;
                case "AspectMethod":
                case "ControlMethod":
                    if (control != null)
                    {
                        // Figuring out if this is for the current Control...!!
                        if (control.ClientID == parameters[2]) // Some controls have "funny ids"...!!
                        {
                            // Then dispatching Load event for CONTROL
                            page.LoadComplete += delegate
                                                     {
                                                         MethodDispatcher.DispatchMethodCall(
                                                             new MethodDispatcher.DispatchContext(control, parameters));
                                                     };
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Called during Unload phase of <see cref="Page"/> lifecycle.
        /// </summary>
        private void OnPageUnload(object sender, EventArgs e)
        {
            _pageUnloaded = true;
            
            if (_pagePreRenderComplete)
            {
                RegisterGlobalUpdateControl();
                RegisterErrorHandler();
            }

            var filterAjaxCallbackResponse = false;
            var response = HttpContext.Current.Response;

            if (IsAjaxCallback)
            {
                DisableTracingIfRequired();

                response.ContentEncoding = Encoding.UTF8;
                response.ContentType = "application/x-javascript";
                response.Cache.SetCacheability(HttpCacheability.NoCache);

                // when the page is unloaded we try to catch if it was a response.redirect and if so
                // we serialize location as window.location for client side evaluation. 
                var location = response.RedirectLocation;
                if (response.StatusCode == 302 || !string.IsNullOrEmpty(location))
                {
                    var href = HtmlFormatter.FormatHtmlForInnerHTML(location);

                    response.Clear();
                    response.StatusCode = 200;
                    response.RedirectLocation = string.Empty;
                    response.Write("window.location='" + href + "';");
                    response.End();
                }
                else if (_pagePreRenderComplete)
                {
                    SerializeAjaxControlRemovals();
                    filterAjaxCallbackResponse = true;
                }
            }

            var context = HttpContext.Current;
            ShouldApplyResponseFilter = filterAjaxCallbackResponse ||
                                        (_pagePreRenderComplete &&
                                         context.Request.Params["HTTP_X_MICROSOFTAJAX"] == null && !Page.IsCallback);
            
            if (!ShouldApplyResponseFilter) return;
            
            var ajaxModuleFound = false;
            var httpModules = context.ApplicationInstance.Modules;
            foreach (var moduleName in httpModules.AllKeys)
            {
                ajaxModuleFound = httpModules[moduleName] is AjaxModule;
                if (ajaxModuleFound) break;
            }

            if (!ajaxModuleFound)
                response.Filter = new HttpResponseFilter(response.Filter);
        }

        /// <summary>
        /// Returns whether <see cref="HttpResponse.Filter"/> should be chained.
        /// </summary>
        internal bool ShouldApplyResponseFilter { get; set; }

        /// <summary>
        /// Disables trace if it writes to page.
        /// </summary>
        private static void DisableTracingIfRequired()
        {
            var traceContext = HttpContext.Current.Trace;
            if (!traceContext.IsEnabled) return;
           
            var traceSection = (TraceSection)WebConfigurationManager.GetSection("system.web/trace");
            if (traceSection.PageOutput)
                traceContext.IsEnabled = false;
        }

        /// <summary>
        /// Registers <see cref="ErrorHandler"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="ErrorHandler"/> is registered if it's not null and is not stored.
        /// </remarks>
        private void RegisterErrorHandler()
        {
            if (ErrorHandler == null) return;

            var errorHandler = ErrorHandler;
            var argument = string.IsNullOrEmpty(errorHandler) ? string.Empty : errorHandler;
            CustomScripts.Add(Constants.GaiaClientModule + ".SE(" + argument + ");");
        }

        /// <summary>
        /// Registers <see cref="UpdateControl"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="UpdateControl"/> is being registered during every callback and is not stored.
        /// </remarks>
        private void RegisterGlobalUpdateControl()
        {
            if (!HasVisibleGaiaControl) return;

            var updateControl = UpdateControl;
            var argument = updateControl != null && updateControl.Visible
                               ? "'" + updateControl.ClientID + "'"
                               : string.Empty;

            CustomScripts.Add(Constants.GaiaClientModule + ".SU(" + argument + ");");
        }

        private void SerializeAjaxControlRemovals()
        {
            var removedAjaxControls = _ajaxControls.FindAll(ajaxControl => ajaxControl.AjaxControl.ShouldRemove);
            var removableReferences = new List<string>(removedAjaxControls.Count);
            removedAjaxControls.ForEach(delegate(IAjaxControl ajaxControl)
                            {
                                var shouldSkip = false;
                                var control = ajaxControl.Control;
                                for (var parent = control.Parent; parent != null && !shouldSkip; parent = parent.Parent)
                                {
                                    var parentAjaxControl = parent as IAjaxControl;
                                    if (parentAjaxControl == null) continue;
                                    shouldSkip = removedAjaxControls.Contains(parentAjaxControl);
                                }
                                if (shouldSkip) return;
                                removableReferences.Add("'" + ajaxControl.AjaxControl.ClientReference + "'");
                            });
            if (removableReferences.Count == 0) return;
            CustomScripts.Add(Constants.GaiaClientModule + ".DR(" +
                              string.Join(",", removableReferences.ToArray()) + ");");
        }        

        #region [ -- Script Inclusion related methods and classes -- ]

        private List<string> StyleSheets
        {
            get { return _styleSheets ?? (_styleSheets = new List<string>()); }
        }

        private List<string> CustomScripts
        {
            get { return _customScripts ?? (_customScripts = new List<string>()); }
        }

        private class ScriptFileInfo
        {
            private readonly string _fileName;
            private readonly string _typeToWaitFor;

            public ScriptFileInfo(string fileName, string waitType)
            {
                _fileName = fileName;
                _typeToWaitFor = waitType;
            }

            public string TypeToWaitFor
            {
                get { return _typeToWaitFor; }
            }

            public string FileName
            {
                get { return _fileName; }
            }
        }

        private List<ScriptFileInfo> GaiaScriptInclusions
        {
            get { return _inclusions ?? (_inclusions = new List<ScriptFileInfo>()); }
        }

        private bool IsFileRegistered(string fileName)
        {
            var isNewlyRegistered = GaiaScriptInclusions.Exists(idx => fileName == idx.FileName);
            var hash = fileName.GetHashCode().ToString(CultureInfo.InvariantCulture);
            var isPreviouslyRegistered = IsPreviouslyRegisteredKey(hash);
            return isNewlyRegistered || (isPreviouslyRegistered && IsAjaxCallback);
        }

        private void EnsurePreviousInclusionHashCodesLoaded()
        {
            if (_previousInclusionHashCodes != null) return;
            _previousInclusionHashCodes = new List<string>();

            var files = Page.Request.Params[FileInclusionFieldName];
            if (string.IsNullOrEmpty(files)) return;

            _previousInclusionHashCodes.AddRange(files.Split(new[] {'$'}, StringSplitOptions.RemoveEmptyEntries));
        }

        private List<string> PreviousInclusionHashCodes
        {
            get
            {
                EnsurePreviousInclusionHashCodesLoaded();
                return _previousInclusionHashCodes;
            }
        }

        #endregion
    }
}
