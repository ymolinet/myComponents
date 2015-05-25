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
using System.IO;
using System.Web.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Specialized <see cref="HtmlTextWriter"/> for design-time rendering.
    /// </summary>
    internal class DesignTimeHtmlTextWriter : HtmlTextWriter
    {
        private static Regex _webResourceRegEx;
        private List<string> _renderedStyleSheets;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DesignTimeHtmlTextWriter"/> class that uses a default tab string.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.IO.TextWriter"/> instance that renders the markup content.</param>
        public DesignTimeHtmlTextWriter(TextWriter writer) : base(writer) { }

        /// <summary>
        /// Regular expression to use for matching WebResource references inside embedded stylesheets.
        /// </summary>
        /// <remarks>
        /// Used during design-time rendering for replacing with design-time web resource reference uri.
        /// </remarks>
        private static Regex WebResourceRegEx
        {
            get
            {
                return _webResourceRegEx ??
                       (_webResourceRegEx = new Regex("<%\\s*=\\s*WebResource\\(\"(?<resourceName>[^\"]*)\"\\)\\s*%>",
                                                      RegexOptions.Singleline | RegexOptions.Multiline));
            }
        }

        /// <summary>
        /// Returns true if the embedded stylesheet defined by the specified <paramref name="type"/> 
        /// and <paramref name="resourceName"/> is rendered using this <see cref="HtmlTextWriter"/>.
        /// </summary>
        public bool IsStyleSheetRendered(Type type, string resourceName)
        {
            return _renderedStyleSheets != null && _renderedStyleSheets.Contains(CreateStyleSheetResourceKey(type, resourceName));
        }

        /// <summary>
        /// Renders embedded stylesheet defined by the specified <paramref name="type"/> and <paramref name="resourceName"/>
        /// using the specified <paramref name="clientScriptManager"/>.
        /// </summary>
        public void RenderStyleSheetResource(System.Web.UI.ClientScriptManager clientScriptManager, Type type, string resourceName)
        {
            var resource = type.Assembly.GetManifestResourceStream(resourceName);
            if (resource == null) return;

            if (_renderedStyleSheets == null)
                _renderedStyleSheets = new List<string>();

            _renderedStyleSheets.Add(CreateStyleSheetResourceKey(type, resourceName));

            string content;
            using (var reader = new StreamReader(resource))
            {
                content = reader.ReadToEnd();
            }

            AddAttribute("type", "text/css");
            RenderBeginTag(HtmlTextWriterTag.Style);
            Write(WebResourceRegEx.Replace(content, match => PerformWebResourceSubstitution(match, clientScriptManager, type)));
            RenderEndTag();
        }

        /// <summary>
        /// Performs substitution for the WebResource path during design-time rendering.
        /// </summary>
        private static string PerformWebResourceSubstitution(Match match, System.Web.UI.ClientScriptManager clientScriptManager, Type type)
        {
            var group = match.Groups["resourceName"];
            if (group != null)
            {
                var resourceName = group.ToString();

                // we assume the resource is from the same assembly
                if (resourceName.Length > 0)
                    return clientScriptManager.GetWebResourceUrl(type, resourceName);
            }

            return match.Value;
        }

        private static string CreateStyleSheetResourceKey(Type type, string resourceName)
        {
            return type.Assembly.FullName + resourceName;
        }
    }
}