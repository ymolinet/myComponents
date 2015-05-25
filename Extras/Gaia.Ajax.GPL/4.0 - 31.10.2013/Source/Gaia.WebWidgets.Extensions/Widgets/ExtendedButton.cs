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
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using Gaia.WebWidgets.HtmlFormatting;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    /// <summary>
    /// The Gaia Ajax ExtendedButton is based on Gaia Button and adds support for Toggling, Image and 
    /// various size and position settings. It is highly skinnable, flexible and renders very lightweight XHTML
    /// and conforms to all accepted web standards like XHTML and so on. 
    /// <br/>
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for ExtendedButton" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\ExtendedButton\Overview\Default.aspx"/>
    /// </code> 
    /// <code title="Codebehind for ExtendedButton" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\ExtendedButton\Overview\Default.aspx.cs"/>
    /// </code> 
    /// </example>
    [DefaultProperty("Text")]
    [ParseChildren(true)]
    [PersistChildren(false)]
    [ToolboxBitmap(typeof(ExtendedButton), "Resources.Gaia.WebWidgets.Extensions.ExtendedButton.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.ExtendedButtonDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class ExtendedButton : Button, ISkinControl
    {
        #region [ -- Properties -- ]

        /// <summary>
        /// Gets or sets if the <see cref="ExtendedButton"/> is toggled.
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [AjaxSerializable("setToggle")]
        [Description("The toggled state of the button.")]
        public bool Toggled
        {
            get { return StateUtil.Get(ViewState, "Toggled", false); }
            set { StateUtil.Set(ViewState, "Toggled", value, false); }
        }

        /// <summary>
        /// Gets or sets Css Class for the <see cref="ExtendedButton"/> image.
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [Description("Css Class for the button image.")]
        public string ImageCssClass
        {
            get { return StateUtil.Get(ViewState, "ImageCssClass", string.Empty); }
            set { StateUtil.Set(ViewState, "ImageCssClass", value, string.Empty); }
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Include ExtendedButton Javascript files
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            // Include ExtendedButton Javascript stuff
            var manager = Manager.Instance;
            manager.AddInclusionOfExtensionsScriptFiles(typeof(ExtendedButton));
            manager.AddInclusionOfFileFromResource("Gaia.WebWidgets.Extensions.Scripts.ExtendedButton.js", typeof(ExtendedButton), "Gaia.Extensions.ExtendedButton.browserFinishedLoading", true);
        }

        /// <summary>
        /// Renders the ExtendedButton
        /// </summary>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (var div = create.Span(ClientID, GetCssClass())) // root element
            {
                Css.SerializeAttributesAndStyles(this, div);

                using (create.Span(null, "first-child"))
                {
                    using (var button = create.Button(CombineId("_btn")).SetStyle("width: 100%;"))
                    {
                        if (!string.IsNullOrEmpty(ImageCssClass)) 
                            button.SetCssClass(ImageCssClass);

                        if (!Enabled) 
                            button.AddAttribute(XhtmlAttribute.Disabled, "disabled");

                        // add the OnClientClick for the ExtendedButton
                        button.AddAttribute(XhtmlAttribute.OnClick, EnsureEndWithSemiColon(OnClientClick));

                        // the text must be put into an inner span because button.innerHTML is readonly in IE
                        using (create.Span(CombineId("_content")).WriteContent(Text)) { }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.
        /// </returns>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Button; }
        }

        /// <summary>
        /// Returns control registration object required for registering control on the client.
        /// </summary>
        /// <param name="registerControl">Suggested control registration object.</param>
        /// <returns>Modified or new control registration object.</returns>
        protected override RegisterControl GetScript(RegisterControl registerControl)
        {
            registerControl.ControlType = "Gaia.Extensions.ExtendedButton";
            return base.GetScript(registerControl).AddProperty("className", CssClass);
        }

        #endregion
 
        #region [ -- Helpers -- ]
        
        private string CombineId(string append)
        {
            return string.Concat(ClientID, append);
        } 

        #endregion

        #region [ -- Css Helpers -- ]

        private static string EnsureEndWithSemiColon(string value)
        {
            if (!string.IsNullOrEmpty(value) && !value.EndsWith(";", StringComparison.InvariantCultureIgnoreCase))
                return string.Concat(value, ";");

            return value;
        }

        private string CombineCssClass(params string[] cssclass)
        {
            return Css.Combine(CssClass, cssclass);
        }

        /// <summary>
        /// Returns full CssClass for the root DOM element.
        /// </summary>
        private string GetCssClass()
        {
            var cssClasses = new List<string> { "button" };

            if (Toggled) cssClasses.Add("button-checked");
            if (!Enabled) cssClasses.Add("button-disabled");

            // signal if we have defined an icon ... 
            if (!string.IsNullOrEmpty(ImageCssClass))
                cssClasses.Add("button-icon");

            return CombineCssClass(cssClasses.ToArray());
        }

        #endregion

        #region [ -- ISkinControl Implementation -- ]

        void ISkinControl.ApplySkin()
        {
            // include default skin css file
            ((IAjaxControl)this).AjaxControl.RegisterDefaultSkinStyleSheetFromResource(typeof(ExtendedButton), Constants.DefaultSkinResource);

            // name of the default skin;
            CssClass = Constants.DefaultSkinCssClass;
        }

        bool ISkinControl.Enabled
        {
            get { return string.IsNullOrEmpty(CssClass) || CssClass.Equals(Constants.DefaultSkinCssClass); }
        }

        #endregion
    }
}
