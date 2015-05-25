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
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    using HtmlFormatting;

    /// <summary>
    /// One "sheet" in the <see cref="TabControl"/> widget.
    /// </summary>
    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(TabView), "Resources.Gaia.WebWidgets.Extensions.TabView.bmp")]
    public class TabView : Panel
    {
        /// <summary>
        /// Represents header of the <see cref="TabView"/>.
        /// </summary>
        internal sealed class TabViewHeader
        {
            /// <summary>
            /// Initializes new instance of the <see cref="TabViewHeader"/> class.
            /// </summary>
            public TabViewHeader(TabView owner)
            {
                if (owner == null)
                    throw new ArgumentNullException("owner");

                View = owner;
            }

            #region [ -- Constants -- ]

            private const string CssClassLeft = "tab-left";
            private const string CssClassRight = "tab-right";
            private const string CssClassDisabled = "item-disabled";
            private const string CssClassActive = "tabstrip-active";
            private const string CssClassCaption = "tabstrip-content";
            private const string CssClassInactive = "tabstrip-inactive";
            private const string CssClassCaptionText = "tabstrip-text";

            private const string IdHeader = "LI";
            private const string IdCaptionContent = "content";

            #endregion

            /// <summary>
            /// Owner <see cref="TabView"/>.
            /// </summary>
            public TabView View { get; private set; }

            /// <summary>
            /// True if header should be clickable
            /// </summary>
            public bool Clickable
            {
                get { return !View.Active && View.IsEnabled; }
            }

            /// <summary>
            /// Gets caption for the header.
            /// </summary>
            public string Caption
            {
                get { return View.Caption; }
            }

            /// <summary>
            /// Gets css class for the caption tag.
            /// </summary>
            public string CssClassContent
            {
                get
                {
                    var imageCssClass = View.CaptionImageCssClass;
                    return View.Owner.CombineCssClass(
                        CssClassCaptionText, string.IsNullOrEmpty(imageCssClass) ? string.Empty : " " + imageCssClass);
                }
            }

            /// <summary>
            /// Gets css class for header tag.
            /// </summary>
            public string CssClassHeader
            {
                get
                {
                    return View.IsEnabled
                        ? View.Owner.CombineCssClass(View.Active ? CssClassActive : CssClassInactive)
                        : View.Owner.CombineCssClass(CssClassDisabled);
                }
            }

            /// <summary>
            /// ID of the <see cref="TabViewHeader"/>.
            /// </summary>
            public string Id
            {
                get { return IdHeader + "_" + View.Index.ToString(NumberFormatInfo.InvariantInfo); }
            }

            /// <summary>
            /// Returns true if the <see cref="TabViewHeader"/> is visible and should be rendered.
            /// </summary>
            public bool Visible
            {
                get { return View.HeaderVisible; }
            }

            /// <summary>
            /// Renders <see cref="TabView"/> header using provided <see cref="XhtmlTagFactory"/>.
            /// </summary>
            /// <param name="create">Tag factory to use for rendering.</param>
            public void Render(XhtmlTagFactory create)
            {
                if (!Visible)
                    return;

                var tabControl = View.Owner;
                var index = View.Index.ToString(NumberFormatInfo.InvariantInfo);

                using (var li = create.Li(tabControl.CombineId(Id), CssClassHeader))
                {
                    if (View.DesignMode && View._tabViewHeaderRenderCallback != null)
                        View._tabViewHeaderRenderCallback(li);

                    using (create.A(tabControl.CombineId(index), tabControl.CombineCssClass(CssClassRight)).AddAttribute(XhtmlAttribute.OnClick, "return false;"))
                    {
                        using (create.Em().SetCssClass(tabControl.CombineCssClass(CssClassLeft)))
                        {
                            using (create.Span().SetCssClass(tabControl.CombineCssClass(CssClassCaption)))
                            {
                                using (var contentSpan = create.Span(tabControl.CombineId(IdCaptionContent, index), CssClassContent))
                                {
                                    var caption = Caption;
                                    if (string.IsNullOrEmpty(caption))
                                        contentSpan.WriteNonBreakingSpace();
                                    else
                                        contentSpan.WriteContent(caption);
                                }
                            }
                        }
                    }
                }
            }
        }

        private readonly TabViewHeader _header;
        private Action<Tag> _tabViewHeaderRenderCallback;

        /// <summary>
        /// Initializes new instance of the <see cref="TabView"/> control.
        /// </summary>
        public TabView()
        {
            _header = new TabViewHeader(this);
        }

        #region [ -- Properties -- ]

        /// <summary>
        /// Returns the header of the <see cref="TabView"/>.
        /// </summary>
        internal TabViewHeader Header
        {
            get { return _header; }
        }

        /// <summary>
        /// Returns true if <see cref="TabView"/> is active; otherwise: false.
        /// </summary>
        [AjaxSerializable("setActive")]
        internal bool Active 
        { 
            get
            {
                var owner = Owner;
                return owner != null && owner.ActiveTabViewIndex == Index;
            }
        }

        /// <summary>
        /// Index of <see cref="TabView"/> in the <see cref="TabViewCollection"/> of the owner <see cref="TabControl"/>.
        /// </summary>
        internal int Index { get; set; }

        /// <summary>
        /// Owner <see cref="TabControl"/>.
        /// </summary>
        internal TabControl Owner { get; set; }

        /// <summary>
        /// CssClass of the icon for the <see cref="TabView"/>, defaults to "none".
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [Description("Css Class for the caption image.")]
        public string CaptionImageCssClass
        {
            get { return StateUtil.Get(ViewState, "CaptionImage", string.Empty); }
            set { StateUtil.Set(ViewState, "CaptionImage", value, string.Empty); }
        }

        /// <summary>
        /// Gets or sets the visibility and position of scroll bars in a <see cref="Panel"/> control.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ASP.ScrollBars"/> enumeration values. The default is None.
        /// </returns>
        [DefaultValue(ASP.ScrollBars.Auto)]
        public override ASP.ScrollBars ScrollBars
        {
            get
            {
                if (!ControlStyleCreated || ViewState["ScrollBars"] == null)
                    return ASP.ScrollBars.Auto;

                return base.ScrollBars;
            }
            set { base.ScrollBars = value; }
        }

        /// <summary>
        /// Caption text of the <see cref="TabView"/>.
        /// </summary>
        [Localizable(true)]
        [DefaultValue("&nbsp;")]
        [Category("Appearance")]
        [Description("TabView caption.")]
        public string Caption
        {
            get { return StateUtil.Get(ViewState, "Caption", "&nbsp;"); }
            set { StateUtil.Set(ViewState, "Caption", value, "&nbsp;"); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a server control is rendered as UI on the page.
        /// </summary>
        /// <returns>
        /// True if the control is visible on the page; otherwise false.
        /// </returns>
        public override bool Visible
        {
            get
            {
                // if TabView is explicitly required to be invisible, we do as required
                if (!HeaderVisible)
                    return false;

                var owner = Owner;

                // check if TabView has an owner TabControl, 
                // which requires only active TabView to be rendered.
                if (owner != null && owner.ForceDynamicRendering)
                {
                    var parent = Parent;
                    return Active && (parent == null || parent.Visible);
                }

                return true;
            }
            set { HeaderVisible = value; }
        }

        /// <summary>
        /// Returns true if the <see cref="TabViewHeader"/> associated with this <see cref="TabView"/> is visible.
        /// </summary>
        private bool HeaderVisible
        {
            get { return StateUtil.Get(ViewState, "HeaderVisible", true);  }
            set { StateUtil.Set(ViewState, "HeaderVisible", value, true); }
        }

        #endregion

        /// <summary>
        /// Adds information about the background image, alignment, wrap, and direction to the list of attributes to render.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// The <see cref="P:System.Web.UI.WebControls.Panel.DefaultButton"/> property of the <see cref="T:System.Web.UI.WebControls.Panel"/> control must be the ID of a control of type <see cref="T:System.Web.UI.WebControls.IButtonControl"/>.
        /// </exception>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            // add style attributes based on TabView activity.
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, Active || DesignMode ? "block" : "none");
            base.AddAttributesToRender(writer);
        }

        /// <summary>
        /// Returns control registration object required for registering control on the client.
        /// </summary>
        /// <param name="registerControl">Suggested control registration object.</param>
        /// <returns>Modified or new control registration object.</returns>
        protected override RegisterControl GetScript(RegisterControl registerControl)
        {
            var control = base.GetScript(registerControl);
            control.ControlType = "Gaia.Extensions.TabView";
            return control.AddPropertyIfTrue(Active, "active", 1);
        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            var manager = Manager.Instance;
            manager.AddInclusionOfExtensionsScriptFiles(typeof(TabView));
            manager.AddInclusionOfFileFromResource("Gaia.WebWidgets.Extensions.Scripts.TabControl.js", typeof(TabControl), "Gaia.Extensions.TabControl.loaded", true);
        }

        /// <summary>
        /// Sets design-time data for a control.
        /// </summary>
        /// <param name="data">An <see cref="T:System.Collections.IDictionary"/> containing the design-time data for the control.</param>
        protected override void SetDesignModeState(IDictionary data)
        {
            _tabViewHeaderRenderCallback = data["HeaderRenderCallback"] as Action<Tag>;
        }

        /// <summary>
        /// Returns true if the value of the <see cref="Visible"/> property should be serialized during design-time.
        /// </summary>
        internal bool ShouldSerializeVisible()
        {
            return !HeaderVisible;
        }

        /// <summary>
        /// Resets the <see cref="Visible"/> property to its default value.
        /// </summary>
        internal void ResetVisible()
        {
            HeaderVisible = true;
        }
    }
}
