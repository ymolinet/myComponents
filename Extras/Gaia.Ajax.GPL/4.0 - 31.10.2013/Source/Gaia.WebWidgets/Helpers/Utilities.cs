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
using System.Text;
using System.Web.UI;
using System.Text.RegularExpressions;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    using HtmlFormatting;

    /// <summary>
    /// Commonly used utility methods.
    /// </summary>
    static class Utilities
    {
        private static Regex _tagMatchRegex;

        /// <summary>
        /// Returns true if <see cref="Control.ViewState"/> is enabled for the specified <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control to check the ViewState for.</param>
        /// <returns>True if viewstate is enabled, false otherwise.</returns>
        public static bool IsViewStateEnabled(Control control)
        {
            var ctrl = control;
            for (; ctrl != null && ctrl.EnableViewState; ctrl = ctrl.Parent) {}
            return ctrl == null;
        }

        /// <summary>
        /// Returns true if specified <paramref name="webControl"/> is enabled.
        /// Checks recursively.
        /// </summary>
        /// <param name="webControl">WebControl to check.</param>
        /// <returns>True if <see cref="ASP.WebControl"/> is enabled.</returns>
        public static bool IsEnabled(ASP.WebControl webControl) 
        {
            for (Control ctrl = webControl; ctrl != null; ctrl = ctrl.Parent) 
            {
                var webCtrl = ctrl as ASP.WebControl;
                if (webCtrl != null && !webCtrl.Enabled) return false;
            }
            return true;
        }

        internal static string GenerateDifference(string source, string destination, out int index)
        {
            index = -1;
            if (source == null || destination == null)
                return null;

            var sourceLength = source.Length;
            var destinationLength = destination.Length;
            if (sourceLength == 0 || destinationLength == 0)
                return null;

            // in case if the source string contained HTML tag look-alikes
            // we don't try to serialize a change, because it can be different when applied to innerHTML.
            if (TagMatchRegex.IsMatch(source))
                return null;

            var length = Math.Min(sourceLength, destinationLength);
            for (index = 0; index < length && source[index] == destination[index]; ++index) { }
            return index < destinationLength ? destination.Substring(index) : string.Empty;
        }

        /// <summary>
        /// Returns callback name for the specified <paramref name="control"/>.
        /// </summary>
        internal static string GetCallbackName(Control control)
        {
            return GetCallbackName(control.UniqueID, control.ClientID);
        }

        /// <summary>
        /// Gets callback name for the control based on the specified <paramref name="uniqueid"/> and <paramref name="clientid"/>.
        /// </summary>
        internal static string GetCallbackName(string uniqueid, string clientid)
        {
            return clientid.Replace('_', '$') != uniqueid ? uniqueid : null;
        }

        private static Regex TagMatchRegex
        {
            get { return _tagMatchRegex ?? (_tagMatchRegex = new Regex("<[^>]+>")); }
        }
    }

    /// <summary>
    /// The ComposeXhtml is a helper class for creating xhtml compliant markup as string values. The ToString()
    /// and Write() functions take either an XhtmlTagFactory or XhtmlTextWriter input parameter in a delegate.
    /// </summary>
    /// <example>
    /// <code title="Using the ComposeXhtml.ToString() function" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Core\XhtmlTagFactory\Overview\Default.aspx.cs" region="Code" />
    /// </code>
    /// </example>
    public static class ComposeXhtml
    {
        /// <summary>
        /// The RenderStringDelegateFactory is used as input parameter to functions where it's useful to
        /// have access to the XhtmlTagFactory
        /// </summary>
        public delegate void RenderStringDelegateFactory(XhtmlTagFactory create);

        /// <summary>
        /// The RenderStringDelegateTextWriter is used as input parameter to functions where you need access
        /// to the TextWriter. Used in Gaia.WebWidgets.AjaxContainerControl
        /// </summary>
        public delegate void RenderStringDelegateTextWriter(TextWriter stringWriter);

        /// <summary>
        /// The RenderStringDelegateStream is used as input parameter to functions where you need access
        /// to the Stream. Used in Gaia.WebWidgets.AjaxControl
        /// </summary>
        public delegate void RenderStringDelegateStream(Stream stream);

        /// <summary>
        /// Takes a delegate and returns a string based on the usage of the XhtmlTagfactory. 
        /// Allows for nice syntax when using the factory to provide markup that can be easily serialized
        /// </summary>
        /// <param name="use">delegate to a method taking XhtmlTagFactory as only parameter</param>
        /// <param name="disableValidation">Specifies if the <see cref="XhtmlTagFactory"/> should be validating or not.</param>
        /// <returns>XHTML string representation</returns>
        public static string ToString(RenderStringDelegateFactory use, bool disableValidation = false)
        {
            return ToString(delegate(TextWriter textWriter)
                         {
                             var writer = new HtmlTextWriter(textWriter);
                             var create = new XhtmlTagFactory(writer, disableValidation);
                             use(create);
                         });
        }

        /// <summary>
        /// Takes a delegate and returns a string based on the usage of the TextWriter.
        /// Allows for nice syntax when using the factory to provide markup that can be easily serialized
        /// </summary>
        public static string ToString(RenderStringDelegateTextWriter use)
        {
            string rendered;

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(memoryStream))
                {
                    use(writer);
                    writer.Flush();
                    memoryStream.Position = 0;
                    using (var reader = new StreamReader(memoryStream))
                    {
                        rendered = reader.ReadToEnd();
                    }
                }
            }

            return rendered;
        }

        /// <summary>
        /// Just writes directly to memory stream. 
        /// Use if you need to use XhmltTagFactory and to throw memory stream away
        /// </summary>
        public static void Write(RenderStringDelegateFactory use)
        {
            using (var stream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    var create = new XhtmlTagFactory(new HtmlTextWriter(writer));
                    use(create);
                    writer.Flush();
                }
            }
        }

        /// <summary>
        /// Just writes directly to memory stream. 
        /// Use if you need to use TextWriter and to throw memory stream away
        /// </summary>
        public static void Write(RenderStringDelegateTextWriter use)
        {
            using (var memoryStream = new MemoryStream())
            {
                using(TextWriter textWriter = new StreamWriter(memoryStream))
                {
                    use(textWriter);
                    textWriter.Flush();
                }
            }
        }

        /// <summary>
        /// Just writes directly to memory stream. 
        /// Use if you need to use Stream and to throw away
        /// </summary>
        public static void Write(RenderStringDelegateStream use)
        {
            using(var memoryStream = new MemoryStream())
            {
                use(memoryStream);
                memoryStream.Flush();
            }
        }
    }

    /// <summary>
    /// Css Helper class
    /// </summary>
    public static class Css
    {
        /// <summary>
        /// Specify a root CssClass (ie. Gaia) and then subsequent combinations like (ie "panel", "header" )
        /// and it will return a list of combinations, like ("Gaia-panel Gaia-header") that can be passed to
        /// the CssClass property of any control
        /// </summary>
        /// <param name="cssClass">the root CssClass</param>
        /// <param name="cssClasses">combinations</param>
        /// <returns>the combined string</returns>
        public static string Combine(string cssClass, params string[] cssClasses)
        {
            return Combine(cssClass, true, cssClasses);
        }

        /// <summary>
        /// Specify a root CssClass (ie. Gaia) and then subsequent combinations like (ie "panel", "header" )
        /// and it will return a list of combinations, like ("Gaia-panel Gaia-header") that can be passed to
        /// the CssClass property of any control
        /// </summary>
        /// <param name="cssClass">The Root CssClass</param>
        /// <param name="skipIfDisabled">If set to true, will skip the process if Nesting of Classes is disabled</param>
        /// <param name="cssClasses">The additional cssclasses to append</param>
        /// <returns>a complete string</returns>
        public static string Combine(string cssClass, bool skipIfDisabled, params string[] cssClasses)
        {
            if (string.IsNullOrEmpty(cssClass))
                return null;

            if (!GaiaAjaxConfiguration.Instance.EnableNestedCssClasses)
                return (skipIfDisabled) ? null : cssClass;
            
            var val = new StringBuilder();
            for (int i = 0; i < cssClasses.Length; ++i)
            {
                if (string.IsNullOrEmpty(cssClasses[i]))
                    continue;

                val.Append(string.Concat(cssClass, "-", cssClasses[i], " "));
            }

            // The return value of this method should always contain an "extra space" since several controls
            // asserts that there are when they add up "extra classes" on the concatenated results of
            // this method.
            return val.ToString(); 
        }

        /// <summary>
        /// Helper method to render Attributes and Styles for controls inheriting from Gaia Panel. Examples include
        /// TabControl and ExtendedPanel. We override the style serialization to render valid xhtml markup and gaia
        /// compatible properties. Extra properties included in Panel include HorizontalAlign, Scrollbars, Wrap, etc.
        /// </summary>
        /// <param name="panel">Panel to parse attributes and styles for</param>
        /// <param name="tag">The Tag to render values into</param>
        public static void SerializeAttributesAndStyles(Panel panel, Tag tag)
        {
            InitializeStyleSerialization(tag, panel);
            var builder = new StringBuilder();

            SerializeStylesWebControl(panel, builder);
            SerializeStylesPanel(panel, builder);

            if (panel.Direction == ASP.ContentDirection.LeftToRight)
                tag.AddAttribute(XhtmlAttribute.Dir, "ltr");
            else if (panel.Direction == ASP.ContentDirection.RightToLeft)
                tag.AddAttribute(XhtmlAttribute.Dir, "rtl");

            FinalizeStyleSerialization(tag, panel, builder);
        }

        /// <summary>
        /// Helper method to render Attributes and Styles for a WebControl. The WebControl hierarchy unfortunately have a lot
        /// of overlapping properties from ASP.NET WebControl. E.g. the BorderWidth which also maps to the Style["border-width"]
        /// and so on. This method renders all attributes, properties and Style values to the given tag. If you create
        /// your own Ajax Controls with Gaia then use this method on your "root element" to make sure you get all those
        /// values into the Root element of your widget.
        /// </summary>
        /// <param name="webControl">Control to parse attributes for</param>
        /// <param name="tag">Tag to render values into</param>
        public static void SerializeAttributesAndStyles(ASP.WebControl webControl, Tag tag)
        {
            InitializeStyleSerialization(tag, webControl);
            var builder = new StringBuilder();
            
            SerializeStylesWebControl(webControl, builder);

            // serialize tooltip
            if (!string.IsNullOrEmpty(webControl.ToolTip))
                tag.AddAttribute(XhtmlAttribute.Title, webControl.ToolTip);
       
            FinalizeStyleSerialization(tag, webControl, builder);
        }

        private static void InitializeStyleSerialization(Tag tag, ASP.WebControl webControl)
        {
            // serialize custom attributes
            using (new Unmanaged(tag))
            {
                tag.AddAttributes(webControl.Attributes);
            }
        }

        private static void FinalizeStyleSerialization(Tag tag, ASP.WebControl webControl, StringBuilder builder)
        {
            // serialize style
            string style = webControl.Style.Value;
            if (!string.IsNullOrEmpty(style))
            {
                tag.SetStyle(style.EndsWith(";", StringComparison.Ordinal)
                                 ? string.Concat(style, builder)
                                 : string.Concat(style, ";", builder));
            }
            else
                tag.SetStyle(builder.ToString());
        }

        private static void SerializeStylesPanel(ASP.Panel panel, StringBuilder builder)
        {
            if ( panel.BackImageUrl.Trim().Length > 0)
                builder.Append(string.Concat("background-image: url(", panel.ResolveClientUrl(panel.BackImageUrl), ");"));
                
            if (panel.ScrollBars != ASP.ScrollBars.None)
                builder.Append(SerializeScrollingAttribute(panel.ScrollBars));

            var horizontalAlign = panel.HorizontalAlign;
            if (horizontalAlign != ASP.HorizontalAlign.NotSet)
                builder.Append(string.Concat("text-align: ", horizontalAlign.ToString().ToLowerInvariant(), ";"));

            if (!panel.Wrap)
                builder.Append("white-space: nowrap;");
        }

        private static void SerializeStylesWebControl(ASP.WebControl webControl, StringBuilder builder)
        {
            // serialize colors
            builder.Append(SerializeColor(webControl.BackColor, "background-color"));
            builder.Append(SerializeColor(webControl.BorderColor, "border-color"));
            builder.Append(SerializeColor(webControl.ForeColor, "color"));

            // serialize units
            builder.Append(SerializeUnit(webControl.BorderWidth, "border-width"));

            builder.Append(SerializeUnit(webControl.Width, "width"));
            builder.Append(SerializeUnit(webControl.Height, "height"));

            // serialize font
            builder.Append(SerializeFont(webControl.Font));

            // serialize border style
            if (webControl.BorderStyle != ASP.BorderStyle.NotSet)
                builder.Append(string.Concat("border-style:", webControl.BorderStyle.ToString().ToLowerInvariant(), ";"));
        }

        private static string SerializeFont(ASP.FontInfo font)
        {
            var serialized = new StringBuilder();

            if (font.Bold)
                serialized.Append("font-weight: bold;");

            if (font.Italic)
                serialized.Append("font-style: italic;");

            // serialize text-decoration
            if (font.Underline || font.Overline || font.Strikeout)
                serialized.Append("text-decoration:");
            
            if (font.Underline)
                serialized.Append(" underline");

            if (font.Overline)
                serialized.Append(" overline");
            
            if (font.Strikeout)
                serialized.Append(" line-through");

            if (font.Underline || font.Overline || font.Strikeout)
                serialized.Append(";");

            serialized.Append(SerializeUnit(font.Size.Unit, "font-size"));

            // serialize font family
            if (!string.IsNullOrEmpty(font.Name))
                serialized.Append(string.Concat("font-family: ", font.Name));

            if (font.ShouldSerializeNames())
            {
                if (!string.IsNullOrEmpty(font.Name))
                    serialized.Append("," + string.Join(",", font.Names));
                else
                    serialized.Append(string.Concat("font-family: ", string.Join(",", font.Names), ";"));
            }

            if (!string.IsNullOrEmpty(font.Name))
                serialized.Append(";");

            return serialized.ToString();
        }

        private static string SerializeUnit(ASP.Unit unit, string styleName)
        {
            string serialized = string.Empty;

            if (!unit.IsEmpty)
                serialized = string.Concat(styleName, ":", unit, ";");

            return serialized;
        }

        private static string SerializeColor(System.Drawing.Color color, string styleName)
        {
            string serialized = string.Empty;

            if (!color.IsEmpty)
            {
                serialized = color.IsNamedColor ? 
                    string.Concat(styleName, ":", color.Name, ";") : 
                    string.Concat(styleName, ":#", color.Name.Substring(2), ";");
            }

            return serialized;
        }

        /// <summary>
        /// Use this function to retrieve the overflow based on the scrollbars of your control.
        /// This is a useful function if you need to use the overflow on a nested element in your
        /// composition control. 
        /// </summary>
        /// <param name="scrollBars">Scrollbars</param>
        /// <returns>Overflow string</returns>
        public static string SerializeScrollingAttribute(ASP.ScrollBars scrollBars)
        {
            switch (scrollBars)
            {
                case ASP.ScrollBars.Horizontal:
                    return "overflow-x: scroll;";

                case ASP.ScrollBars.Vertical:
                    return "overflow-y: scroll;";

                case ASP.ScrollBars.Both:
                    return "overflow: scroll;";

                case ASP.ScrollBars.Auto:
                    return "overflow: auto;";
            }

            return string.Empty;
        }
    }
}
