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
using System.Text;
using System.Web.UI;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;

namespace Gaia.WebWidgets.Extensions
{
    using Effects;
    using HtmlFormatting;

    /// <summary>
    /// A toolbar item can be thought of as a combination between a menu items and a toolbar composite control.
    /// Inside the ToolbarItem you can add any controls you wish and all of them will be "initially visible"
    /// except for other ToolbarItems which will be put into another container which will be rendered intially hidden.
    /// Then when you click the ToolbarItem the child controls of type ToolbarItems will be shown
    /// </summary>
    [Themeable(true)]
    [ToolboxItem(false)]
    [DefaultProperty("CssClass")]
    [ToolboxBitmap(typeof(ToolbarItem), "Resources.Gaia.WebWidgets.Extensions.ToolbarItem.bmp")]
    public class ToolbarItem : Panel, IExtraPropertyCallbackRenderer, IAjaxContainerControl, ISkinControl
    {
        /// <summary>
        /// Specialized <see cref="AjaxContainerControl"/> for <see cref="ToolbarItem"/>.
        /// </summary>
        public class ToolbarItemAjaxControl : AjaxContainerControl
        {
            private readonly ToolbarItem _owner;
            
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="control">Control which owns this instance.</param>
            public ToolbarItemAjaxControl(ToolbarItem control) : base(control)
            {
                _owner = control;
            }

            /// <summary>
            /// Returns virtual <see cref="ControlCollection"/> for the specified child <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Child control to get cotnainer for.</param>
            /// <returns>Virtual container for the specified control.</returns>
            protected override ControlCollection GetVirtualContainer(Control control)
            {
                return control is ToolbarItem ? (ControlCollection)_owner.ToolbarItems : _owner.ChildControls;
            }
        }

        private const string IdContent = "content";

        #region [ -- Private Members -- ]

        private bool _hasSetClose;
        private ToolbarItemAjaxControl _impl;
        
        #endregion

        #region [ -- Effect Events -- ]

        /// <summary>
        /// Use this function to apply an effect to the content area of the ToolbarItem
        /// </summary>
        /// <typeparam name="T">Any Effect</typeparam>
        /// <param name="effect">The Effect to apply modifications to</param>
        /// <returns>The effect itself. Don't forget to add the effect to the Window effects collection</returns>
        public static T ApplyToContent<T>(T effect) where T : Effect
        {
            return EffectUtils.AppendElementID("_children", effect);
        }

        /// <summary>
        /// Use this EffectEvent to wire up the effect when the ToolbarItem displays its children
        /// </summary>
        public static AjaxEffectEvent ShowChildren { get { return AjaxEffectEventFactory.CreateWithAfterFinishParameter("gaiashowchildren"); } }

        /// <summary>
        /// Use this EffectEvent to wire up the effect when the Toolbaritem hides its children
        /// </summary>
        public static AjaxEffectEvent HideChildren { get { return AjaxEffectEventFactory.CreateWithAfterFinishParameter("gaiahidechildren"); } }

        #endregion

        #region [ -- Enums -- ]
        /// <summary>
        /// Activity that should trigger a drop-down of the child menu items
        /// </summary>
        public enum DropDownMethod
        {
            /// <summary>
            /// Mouse hovers over
            /// </summary>
            Hover, 

            /// <summary>
            /// Single click with left mouse button
            /// </summary>
            Click
        };

        /// <summary>
        /// How to animate the ToolbarItem when whosing it
        /// </summary>
        public enum AnimationMethod
        {
            /// <summary>
            /// Will only show the ToolbarItem directly without any animations
            /// </summary>
            None,

            /// <summary>
            /// Will fade in and out the ToolbarItem
            /// </summary>
            Fade,

            /// <summary>
            /// Will blind down and up the ToolbarItem
            /// </summary>
            Blind
        }

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Owner <see cref="Toolbar"/> for this item.
        /// </summary>
        internal Toolbar Owner { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="ToolbarItem"/> should close when it is clicked.        
        /// </summary>
        /// <remarks>
        /// For child controls of the <see cref="ToolbarItem"/> which require click handling for their own
        /// functionality, such as <see cref="Calendar"/>, <see cref="AutoCompleter"/>, etc... controls,
        /// value of the <see cref="CloseIfClicked"/> property should be set to false.
        /// </remarks>
        [DefaultValue(true)]
        [Category("Behavior")]
        [AjaxSerializable("setCloseOnClick")]
        [Description("Should close the ToolbarItem when clicked.")]
        public bool CloseIfClicked
        {
            get { return StateUtil.Get(ViewState, "CloseIfClicked", true); }
            set { StateUtil.Set(ViewState, "CloseIfClicked", value, true); }
        }

        /// <summary>
        /// Gets or sets how the <see cref="ToolbarItem"/> should show its child <see cref="ToolbarItem"/> controls.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(DropDownMethod.Click)]
        [AjaxSerializable("setDropDownMethod")]
        [Description("Child ToolbarItem show method.")]
        public DropDownMethod DropMethod
        {
            get { return StateUtil.Get(ViewState, "DropMethod", DropDownMethod.Click); }
            set { StateUtil.Set(ViewState, "DropMethod", value, DropDownMethod.Click); }
        }

        /// <summary>
        /// Gets or sets the animation method for child <see cref="ToolbarItem"/>.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(AnimationMethod.None)]
        [Description("Animation method for child ToolbarItems.")]
        public AnimationMethod Animation
        {
            get { return StateUtil.Get(ViewState,"Animation", AnimationMethod.None); }
            set { StateUtil.Set(ViewState, "Animation", value, AnimationMethod.None); }
        }

        /// <summary>
        /// Gets or sets a CSS class used for displaying an icon for the <see cref="ToolbarItem"/>.
        /// </summary>
        [DefaultValue("")]
        [Category("Behavior")]
        [Description("Css class for the ToolbarItem icon.")]
        public string IconCssClass
        {
            get { return StateUtil.Get(ViewState,"IconCssClass", string.Empty); }
            set { StateUtil.Set(ViewState, "IconCssClass", value, string.Empty); }
        }

        /// <summary>
        /// Full Css class for the icon.
        /// </summary>
        [AjaxSerializable("setIconCssClass")]
        internal string ToolbarItemIconCssClass
        {
            get { return CombineCssClass("toolbar-item-icon", IconCssClass) + "span-for-image"; }
        }

        /// <summary>
        /// Returns true if the item is nested into another <see cref="ToolbarItem"/>.
        /// </summary>
        [AjaxSerializable("setNested")]
        internal bool Nested
        {
            get { return Owner != null; }
        }

        /// <summary>
        /// Returns CssClass of the root DOM element.
        /// </summary>
        [AjaxSerializable("setCssClassRoot")]
        internal string CssClassRoot
        {
            get { return Css.Combine(CssClass, false, "toolbar-item", Nested ? "toolbar-top-item" : "toolbar-child-item"); }
        }

        #endregion

        #region [ -- Public Methods -- ]

        /// <summary>
        /// Closes the <see cref="ToolbarItem"/> if it is expanded
        /// </summary>
        public void Close()
        {
            _hasSetClose = true;
        }

        #endregion

        #region [ -- Overridden base class methods and properties -- ]

        /// <summary>
        /// Gets the HtmlTextWriterTag value for the GridView control.
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Li; }
        }

        /// <summary>
        /// Creates a new <see cref="T:System.Web.UI.ControlCollection"/> object to hold the child controls (both literal and server) of the server control.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.UI.ControlCollection"/> object to contain the current server control's child server controls.
        /// </returns>
        protected override ControlCollection CreateControlCollection()
        {
            return new ToolbarItemAwareContainerCollection(this);
        }

        /// <summary>
        /// Include ToolbarItem Javascript files
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            // Include ToolbarItem JavaScript stuff
            Manager.Instance.AddInclusionOfExtensionsScriptFiles(typeof(ToolbarItem));
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Extensions.Scripts.ToolbarItem.js", typeof(ToolbarItem), "Gaia.Extensions.ToolbarItem.browserFinishedLoading", true);
        }

        /// <summary>
        /// This collection contains all the Child ToolbarItems in this ToolbarItem. Use this collection instead
        /// of the Controls collection to access your ToolbarItems. It's safe to call AddAt, RemoveAt and Clear on 
        /// this controls collection. If you need to access your custom user defined controls on the ToolbarItem
        /// use the ChildControls collection instead. 
        /// </summary>
        [Editor("Gaia.WebWidgets.Extensions.Design.ToolbarItemCollectionEditor, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a", typeof(UITypeEditor))]
        public ControlCollectionOf<ToolbarItem> ToolbarItems
        {
            get { return ((ToolbarItemAwareContainerCollection)Controls).Containers; }
        }

        /// <summary>
        /// This collection contains all your user defined controls and not the child ToolbarItems. This control works as 
        /// an abstraction on top of the Controls collection, but you should use this collection instead for secure access.
        /// If you need to access your ToolbarItems please use the ToolbarItems collection.  
        /// </summary>
        [Browsable(false)]
        public ControlCollectionExcept<ToolbarItem> ChildControls
        {
            get { return ((ToolbarItemAwareContainerCollection)Controls).ChildControls; }
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            // Here we add default animation for the ToolbarItems
            switch (Animation)
            {
                case AnimationMethod.Fade:
                    Effects.Add(ShowChildren, ApplyToContent(new EffectAppear()));
                    Effects.Add(HideChildren, ApplyToContent(new EffectFade()));
                    break;
                case AnimationMethod.Blind:
                    Effects.Add(ShowChildren, ApplyToContent(new EffectBlindDown(0.4M)));
                    Effects.Add(HideChildren, ApplyToContent(new EffectBlindUp(0.4M)));
                    break;
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Overridden from Panel to render the specific ToolbarItem HTML
        /// </summary>
        /// <param name="create">XhtmlTagFactory to render into</param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (var li = create.Li(ClientID, CssClassRoot))
            {
                Css.SerializeAttributesAndStyles(this, li);

                using (create.Div().SetCssClass(CombineCssClass("toolbar-item-el")))
                {
                    using (create.Div(null, ToolbarItemIconCssClass)) { }
                    using (create.Div(ClientID + "_" + IdContent, CombineCssClass("toolbar-item-content")))
                    {
                        RenderChildControls(create);
                    }
                }

                RenderChildToolbarItems(create);
            }
        }

        #endregion

        #region [ -- Overridden IAjaxControl methods -- ]

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerWebControl(this, ClientID, this);
        }

        private void RenderChildToolbarItems(XhtmlTagFactory create)
        {
            var items = ToolbarItems;
            if (items.Count == 0) return;

            using (create.Ul(ClientID + "_children", CombineCssClass("toolbar-child-container")).
                AddAttribute(XhtmlAttribute.Style, "display:none;"))
            {
                foreach (var item in items)
                    item.RenderControl(create.GetHtmlTextWriter());
            }
        }

        private void RenderChildControls(XhtmlTagFactory create)
        {
            foreach (var control in ChildControls)
                control.RenderControl(create.GetHtmlTextWriter());
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.Extensions.ToolbarItem", ClientID)
                .AddPropertyIfTrue(Nested, "isTop", 1)
                .AddPropertyIfTrue(!CloseIfClicked, "closeOnClick", 0)
                .AddProperty("dropMethod", DropMethod.ToString(), true)
                .AddAspects(Aspects).AddEffects(Effects).ToString();
        }

        #endregion

        #region [ -- Overridden IAjaxContainerControl methods -- ]

        string IAjaxContainerControl.GetDOMContainer(IAjaxControl child)
        {
            return (child is ToolbarItem) ? ClientID + "_children" : ClientID + "_" + IdContent;
        }

        /// <summary>
        /// Returns the AjaxControl object associated with this control. 
        /// </summary>
        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _impl ?? (_impl = new ToolbarItemAjaxControl(this)); }
        }

        #endregion

        #region [ -- IExtraPropertyCallbackRenderer Implementation -- ]

        void IExtraPropertyCallbackRenderer.InjectPropertyChangesToCallbackResponse(StringBuilder code)
        {
            if (!_hasSetClose) return;
            code.Append(".setClose()");
        }

        #endregion

        #region [ -- Helpers -- ]

        /// <summary>
        /// Combines provided css classes with the <see cref="System.Web.UI.WebControls.WebControl.CssClass"/> property.
        /// </summary>
        /// <param name="cssclasses">Css classes to combine with.</param>
        /// <returns>Combined css classes.</returns>
        private string CombineCssClass(params string[] cssclasses)
        {
            return Css.Combine(CssClass, cssclasses);
        }

        #endregion

        #region [ -- ISkinControl Implementation -- ]

        void ISkinControl.ApplySkin()
        {
            // include default skin css file
            ((IAjaxControl)this).AjaxControl.RegisterDefaultSkinStyleSheetFromResource(typeof(ToolbarItem), Constants.DefaultSkinResource);

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
