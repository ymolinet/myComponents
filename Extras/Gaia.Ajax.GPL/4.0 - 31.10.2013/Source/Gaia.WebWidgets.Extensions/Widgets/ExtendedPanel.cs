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
using System.ComponentModel;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    using Effects;
    using HtmlFormatting;

    /// <summary>
    /// The Gaia Ajax ExtendedPanel is specialized container like <see cref="Panel"/>, which can be more easily skinned, 
    /// has caption and supports dragging, collapsing or expanding.
    /// </summary>
    [DefaultEvent("Toggled")]
    [DefaultProperty("CssClass")]
    [ToolboxData("<{0}:ExtendedPanel runat=\"server\"></{0}:ExtendedPanel>")]
    [ToolboxBitmap(typeof(ExtendedPanel), "Resources.Gaia.WebWidgets.Extensions.ExtendedPanel.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.ExtendedPanelDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class ExtendedPanel : HybridPanelBase, ISkinControl
    {
        /// <summary>
        /// Specialized <see cref="ASP.Panel"/> for the body part.
        /// </summary>
        private sealed class ExtendedPanelBody : ASP.Panel
        {
            private readonly ExtendedPanel _owner;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExtendedPanelBody"/> class.
            /// </summary>
            public ExtendedPanelBody(ExtendedPanel owner)
            {
                if (owner == null)
                    throw new ArgumentNullException("owner");

                _owner = owner;
            }

            /// <summary>
            /// Outputs the content of a server control's children to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
            /// </summary>
            /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the rendered content.</param>
            protected override void RenderChildren(HtmlTextWriter writer)
            {
                if (!_owner.CanRenderBody) return;
                _owner.RenderChildren(writer);
            }
        }

        /// <summary>
        /// Name of the <see cref="ExtendedPanel"/> toggling command.
        /// </summary>
        private const string ToggleCommandName = "t";

        private const string HeaderId = "e-hr";
        private const string CaptionId = "e-cn";
        private const string ToggleToolId = "e-tt";
        private const string ContentId = "content";

        private bool _toggled;
        private Action<ASP.WebControl> _contentCreatedCallback;
        private Action<ASP.WebControl> _captionCreatedCallback;
        private Action<ASP.WebControl> _toggleToolCreatedCallback;

        #region [ -- Effect Events -- ]

        /// <summary>
        /// Applies an effect to the content area of the <see cref="ExtendedPanel"/>. 
        /// </summary>
        /// <typeparam name="T">Type of <see cref="Effect"/>.</typeparam>
        /// <param name="effect">The <see cref="Effect"/> to modify.</param>
        /// <returns>The effect to be add the <see cref="Panel.Effects"/> collection.</returns>
        public static T ApplyToContent<T>(T effect) where T : Effect
        {
            return EffectUtils.AppendElementID("_" + ContentId, effect);
        }

        /// <summary>
        /// Gets <see cref="AjaxEffectEvent"/> used during minimization.
        /// <example>
        /// Effects.Add(ExtendedPanel.Minimize, ExtendedPanel.ApplyToContent(new EffectBlindUp()));
        /// </example>
        /// </summary>
        public static AjaxEffectEvent Minimize { get { return AjaxEffectEventFactory.Create("gaiaminimizing"); } }

        /// <summary>
        /// Gets <see cref="AjaxEffectEvent"/> used during restoration.
        /// <example>
        /// Effects.Add(ExtendedPanel.RestoreAfterMinimize, ExtendedPanel.ApplyToContent(new EffectBlindDown()));
        /// </example>
        /// </summary>
        public static AjaxEffectEvent Restore { get { return AjaxEffectEventFactory.Create("gaiarestoreafterminimize"); } }

        #endregion

        #region [ -- Events -- ]

        /// <summary>
        /// Raised when <see cref="ExtendedPanel"/> is moved.
        /// </summary>
        public event EventHandler Moved;

        /// <summary>
        /// Raised when <see cref="ExtendedPanel"/> is toggled, ie. collapsed or expanded.
        /// </summary>
        public event EventHandler Toggled;

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Owner <see cref="Accordion"/>.
        /// </summary>
        /// NOTE: Intergrating AccordionView functionality into the ExtendedPanel is done only for keeping backwards compatibility.
        internal Accordion OwnerAccordion { get; set; }

        /// <summary>
        /// Returns true if there exists a property value of which depends on one of the <see cref="OwnerAccordion"/> properties.
        /// </summary>
        internal bool DependsOnOwnerProperties
        {
            get { return !HasOwnCssClass || !HasOwnWidth || !HasOwnAnimationDuraion; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether validation is performed when the button controls are clicked.
        /// </summary>
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Obsolete("There is no operation which can cause validation in ExtendedPanel.")]
        public virtual bool CausesValidation
        {
            get { return StateUtil.Get(ViewState, "CausesValidation", false); }
            set { StateUtil.Set(ViewState, "CausesValidation", value, false); }
        }

        /// <summary>
        /// Gets or sets the group of controls for which this control causes validation when it posts back to the server.
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Category("Behavior")]
        [Obsolete("There is no operation which can cause validation in ExtendedPanel.")]
        public virtual string ValidationGroup
        {
            get { return StateUtil.Get(ViewState, "ValidationGroup", string.Empty); }
            set { StateUtil.Set(ViewState, "ValidationGroup", value, string.Empty); }
        }

        /// <summary>
        /// Caption for the panel.
        /// </summary>
        [Themeable(true)]
        [Category("Appearance")]
        [DefaultValue("&nbsp;")]
        public virtual string Caption
        {
            get { return StateUtil.Get(ViewState, "Caption", "&nbsp;"); }
            set { SetStateValue("Caption", value, "&nbsp;"); }
        }

        /// <summary>
        /// Duration of animation when shifting from expanded to collapsed mode
        /// </summary>
        [Bindable(true)]
        [DefaultValue(0)]
        [Category("Behavior")]
        public virtual int AnimationDuration
        {
            get { return StateUtil.Get(ViewState, "AnimationDuration", DefaultAnimationDuration); }
            set { SetStateValue("AnimationDuration", value, DefaultAnimationDuration); }
        }

        /// <summary>
        /// Returns the value of the <see cref="AnimationDuration"/> property
        /// when it is not available from the <see cref="Control.ViewState"/>.
        /// </summary>
        private int DefaultAnimationDuration
        {
            get { return OwnerAccordion == null ? 0 : OwnerAccordion.AnimationDuration; }
        }

        /// <summary>
        /// Returns true if <see cref="AnimationDuration"/> is defined.
        /// </summary>
        private bool HasOwnAnimationDuraion
        {
            get { return ViewState["AnimationDuration"] != null; }
        }

        /// <summary>
        /// True if it should be possible to move the panel around by dragging its bar
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public virtual bool Draggable
        {
            get { return OwnerAccordion == null ? StateUtil.Get(ViewState, "Draggable", false) : false; }
            set { SetStateValue("Draggable", value, false); }
        }

        /// <summary>
        /// Decides wheter the panel has an "expand/collapse" icon to expand and collapse
        /// the surface of the Panel.
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        public virtual bool CanBeToggled
        {
            get { return StateUtil.Get(ViewState, "CanBeToggled", true); }
            set { SetStateValue("CanBeToggled", value, true); }
        }

        /// <summary>
        /// Should the ExtendedPanel be collapsed or expanded initially. 
        /// </summary>
        [Category("Behavior")]
        public virtual bool Collapsed
        {
            get { return StateUtil.Get(ViewState, "Collapsed", OwnerAccordion != null); }
            set { SetStateValue("Collapsed", value); }
        }

        /// <summary>
        /// Set the CSS class to use for the icon which would normally be up and left on the widget
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        public string IconCssClass
        {
            get { return StateUtil.Get(ViewState, "IconCssClass", string.Empty); }
            set { SetStateValue("IconCssClass", value, string.Empty); }
        }

        /// <summary>
        /// Enable Scrollbars on the ExtendedPanel
        /// </summary>
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
        /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
        /// </summary>
        /// <returns>
        /// The CSS class rendered by the Web server control on the client. The default is <see cref="F:System.String.Empty"/>.
        /// </returns>
        public override string CssClass
        {
            get { return HasOwnCssClass ? base.CssClass : OwnerAccordion.CssClass; }
            set
            {
                var initial = base.CssClass;
                base.CssClass = value;
                if (initial == value) return;
                RequiresRecomposition();
            }
        }

        /// <summary>
        /// Returns true if <see cref="CssClass"/> is defined.
        /// </summary>
        private bool HasOwnCssClass
        {
            get { return OwnerAccordion == null || !string.IsNullOrEmpty(base.CssClass); }
        }

        /// <summary>
        /// Gets or sets the width of the Web server control.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.UI.WebControls.Unit"/> that represents the width of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The width of the Web server control was set to a negative value.</exception>
        public override ASP.Unit Width
        {
            get { return HasOwnWidth ? base.Width : OwnerAccordion.Width; }
            set { base.Width = value; }
        }

        /// <summary>
        /// Returns true if <see cref="Width"/> is defined.
        /// </summary>
        private bool HasOwnWidth
        {
            get { return OwnerAccordion == null || !base.Width.IsEmpty; }
        }

        /// <summary>
        /// Gets or sets value denoting if the child <see cref="ExtendedPanelBody"/> 
        /// can render its owner's children.
        /// </summary>
        private bool CanRenderBody { get; set; }
       
        #endregion

        #region [ -- Public Methods -- ]

        /// <summary>
        /// Toggles the visibility of the content panel. Uses animations if AnimationDuration is specified.
        /// </summary>
        public void Toggle()
        {
            _toggled = !_toggled;
            Collapsed = !Collapsed;

            if (Toggled != null)
                Toggled(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Sets specified <paramref name="value"/> into the <see cref="Control.ViewState"/>
        /// using specified <paramref name="key"/>.
        /// </summary>
        /// <remarks>
        /// The value is set if it's different from the current value.
        /// If the value if updated, <see cref="RequiresRecomposition"/> is called.
        /// </remarks>
        internal void SetStateValue<T>(string key, T value, T defaultValue)
        {
            var initial = StateUtil.Get(ViewState, key, defaultValue);

            var comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(initial, value)) return;

            StateUtil.Set(ViewState, key, value, defaultValue);
            RequiresRecomposition();
        }

        /// <summary>
        /// Sets specified <paramref name="value"/> into the <see cref="Control.ViewState"/>
        /// using specified <paramref name="key"/>.
        /// </summary>
        /// <remarks>
        /// The value is set if it's different from the current value.
        /// If the value if updated, <see cref="RequiresRecomposition"/> is called.
        /// </remarks>
        internal void SetStateValue<T>(string key, T value)
        {
            var stateValue = ViewState[key];
            if (stateValue != null)
            {
                var initial = (T) stateValue;
                var comparer = EqualityComparer<T>.Default;
                if (comparer.Equals(initial, value)) return;
            }
            
            StateUtil.Set(ViewState, key, value);
            RequiresRecomposition();
        }

        /// <summary>
        /// Ensures <see cref="Control.ChildControlsCreated"/> is false.
        /// </summary>
        internal void RequiresRecomposition()
        {
            if (!CompositionControlsCreated) return;
            CompositionControlsCreated = false;
        }

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Raises the <see cref="Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            CreateDraggable();
            base.OnLoad(e);
        }

        /// <summary>
        /// Determines whether the event for the server control is passed up the page's UI server control hierarchy.
        /// </summary>
        /// <returns>
        /// True if the event has been canceled; otherwise, false. The default is false.
        /// </returns>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var commandArguments = args as ASP.CommandEventArgs;
            if (commandArguments != null)
            {
                var commandName = commandArguments.CommandName;
                if (string.CompareOrdinal(ToggleCommandName, commandName) == 0)
                {
                    Toggle();
                    return true;
                }
            }

            return base.OnBubbleEvent(source, args);
        }

        /// <summary>
        /// Called by the framework to notify server controls that use composition-based 
        /// implementation to create Composition Controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateCompositionControls()
        {
            CompositionControls.Add(CreateHeader());
            CompositionControls.Add(CreateContentWrapper());
            base.CreateCompositionControls();
        }

        /// <summary>
        /// The last step where we can manipulate the rendering of the control(s)/page before rendering occurs.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            // Add default minimize and maximize effect events. Will not be added if user has defined custom effects
            // Effects will not be added if user defined effects has been added previously. 
            if (AnimationDuration > 0)
            {
                Effects.Add(Minimize, ApplyToContent(new EffectBlindUp(AnimationDuration / 1000M)));
                Effects.Add(Restore,  ApplyToContent(new EffectBlindDown(AnimationDuration / 1000M)));
            }
            else
            {
                Effects.Add(Minimize, ApplyToContent(new EffectHide()));
                Effects.Add(Restore, ApplyToContent(new EffectShow()));
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Set defaults for composition controls.
        /// </summary>
        [Obsolete("Use OnPreRender() or CreateChildControl() or CreateCompositionControls() instead.")]
        protected virtual void SetPreRenderDefaults() { }

        /// <summary>
        /// See <see cref="AjaxControl.RenderControl" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to write the control into</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (_toggled)
            {
                var eventName = Collapsed ? "minimizing" : "restoreafterminimize";
                var js = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                       "jQuery('#{0}').trigger('gaia:{1}');",
                                       ClientID, eventName);

                Manager.Instance.AddScriptForClientSideEval(js);
            }

            base.RenderControl(writer);
        }

        /// <summary>
        /// Renders the HTML of the panel
        /// </summary>
        /// <param name="create">Writer to render HTML into</param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            // no need to store these values so we set them here during render
            // todo: should make sure the style["position"] is set back during callback into the PSM.
            // todo: remove hard-coded style.
            if (Draggable && !DesignMode)
                Style["position"] = "absolute";
            
            // move the scrollbar settings to the inner panel for correct behavior
            var previousScrollbars = ScrollBars;
            ScrollBars = ASP.ScrollBars.None;

            using (var div = create.Div(ClientID, Css.Combine(CssClass, false, "extendedpanel")))
            {
                // serialize common attributes and style
                Css.SerializeAttributesAndStyles(this, div);
                ScrollBars = previousScrollbars;

                RenderControlContents(create);
            }
        }

        /// <summary>
        /// Renders ExtendedPanel's content.
        /// Used during initial rendering and ForceAnUpdate
        /// </summary>
        /// <param name="create">XhtmlTagFactory for easy rendering</param>
        protected override void RenderControlContents(XhtmlTagFactory create)
        {
            CanRenderBody = true;
            var writer = create.GetHtmlTextWriter();

            foreach (var control in CompositionControls)
                control.RenderControl(writer);

            CanRenderBody = false;
        }

        /// <summary>
        /// Gets design-time data for a control.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IDictionary"/> containing the design-time data for the control.
        /// </returns>
        protected override IDictionary GetDesignModeState()
        {
            return new System.Collections.Specialized.HybridDictionary {{"OwnerAccordion", OwnerAccordion}};
        }

        /// <summary>
        /// Sets design-time data for a control.
        /// </summary>
        /// <param name="data">An <see cref="T:System.Collections.IDictionary"/> containing the design-time data for the control. </param>
        protected override void SetDesignModeState(IDictionary data)
        {
            var ownerAccordion = data["OwnerAccordion"] as Accordion;
            if (ownerAccordion != null)
                OwnerAccordion = ownerAccordion;

            _contentCreatedCallback = data["ContentCreatedCallback"] as Action<ASP.WebControl>;
            _captionCreatedCallback = data["CaptionCreatedCallback"] as Action<ASP.WebControl>;
            _toggleToolCreatedCallback = data["ToggleToolCreatedCallback"] as Action<ASP.WebControl>;
        }

        /// <summary>
        /// Returns true if the <see cref="CssClass"/> property should be serialized during desing-time.
        /// </summary>
        internal bool ShouldSerializeCssClass()
        {
            return HasOwnCssClass;
        }

        /// <summary>
        /// Resets the <see cref="CssClass"/> property to the default state.
        /// </summary>
        internal void ResetCssClass()
        {
            base.CssClass = string.Empty;
        }

        /// <summary>
        /// Returns true if the <see cref="Width"/> property should be serialized during design-time.
        /// </summary>
        internal bool ShouldSerializeWidth()
        {
            return HasOwnWidth;
        }

        /// <summary>
        /// Resets the <see cref="Width"/> property to the default state.
        /// </summary>
        internal void ResetWidth()
        {
            base.Width = ASP.Unit.Empty;
        }

        /// <summary>
        /// Return true if the <see cref="AnimationDuration"/> property should be serialized during design-time.
        /// </summary>
        internal bool ShouldSerializeAnimationDuration()
        {
            return HasOwnAnimationDuraion;
        }

        /// <summary>
        /// Resets the <see cref="AnimationDuration"/> property to the default state.
        /// </summary>
        internal void ResetAnimationDuration()
        {
            ViewState.Remove("AnimationDuration");
        }

        /// <summary>
        /// Returns true if the <see cref="Collapsed"/> property should be serialized during design-time.
        /// </summary>
        internal bool ShouldSerializeCollapsed()
        {
            return ViewState["Collapsed"] != null && (bool) ViewState["Collapsed"];
        }

        /// <summary>
        /// Resets the <see cref="Collapsed"/> property to the default state.
        /// </summary>
        internal void ResetCollapsed()
        {
            ViewState.Remove("Collapsed");
        }

        #endregion

        #region [ -- ExtendedPanel creation methods -- ]

        /// <summary>
        /// Creates <see cref="ExtendedPanel"/> header.
        /// </summary>
        private ASP.Panel CreateHeader()
        {
            var cssClass = CombineCssClass("extendedpanel-top", "noselect", Draggable ? "draggable" : string.Empty);
            var panel = new ASP.Panel { ID = HeaderId, CssClass = cssClass };

            var caption = CreateCaption();
            if (DesignMode && _captionCreatedCallback != null)
                _captionCreatedCallback(caption);

            var toggleTool = CreateToggleTool();
            if (DesignMode && _toggleToolCreatedCallback != null)
                _toggleToolCreatedCallback(toggleTool);

            panel.Controls.Add(caption);
            panel.Controls.Add(toggleTool);

            return panel;
        }

        /// <summary>
        /// Creates <see cref="ExtendedPanel"/> content wrapper.
        /// </summary>
        private ASP.Panel CreateContentWrapper()
        {
            var wrapper = new ASP.Panel { CssClass = CombineCssClass("extendedpanel-contentwrapper") };

            var content = CreateContent();
            if (DesignMode && _contentCreatedCallback != null)
                _contentCreatedCallback(content);

            wrapper.Controls.Add(content);
            return wrapper;
        }

        /// <summary>
        /// Create <see cref="ExtendedPanel"/> content area.
        /// </summary>
        private ASP.Panel CreateContent()
        {
            var panel = new ExtendedPanelBody(this) { ID = ContentId, CssClass = CombineCssClass("extendedpanel-content"), ScrollBars = ScrollBars };
            if (Collapsed) panel.Style["display"] = "none";
            return panel;
        }

        /// <summary>
        /// Creates <see cref="ExtendedPanel"/> caption control.
        /// </summary>
        private LinkButton CreateCaption()
        {
            var caption = CreateLinkButton(CaptionId, GetCssClassForCaption(), ToggleCommandName, Caption);
            caption.Enabled = CanBeToggled;
            return caption;
        }

        /// <summary>
        /// Creates <see cref="ExtendedPanel"/> toggle tool control.
        /// </summary>
        private LinkButton CreateToggleTool()
        {
            var toggleTool = CreateLinkButton(ToggleToolId, GetCssClassForToggleTool(), ToggleCommandName, string.Empty);
            toggleTool.Visible = CanBeToggled;
            return toggleTool;
        }

        /// <summary>
        /// Creates <see cref="LinkButton"/> with the specified properties.
        /// </summary>
        private static LinkButton CreateLinkButton(string id, string cssClass, string commandName, string text)
        {
            return new LinkButton
                       {
                           ID = id,
                           Text = text,
                           CssClass = cssClass,
                           CommandName = commandName,
                           CausesValidation = false
                       };
        }

        #endregion

        #region [ -- Helpers -- ]

        /// <summary>
        /// Creates <see cref="AspectDraggable"/> for drag dupport.
        /// </summary>
        private void CreateDraggable()
        {
            if (!Draggable) return;

            var draggable = new AspectDraggable {Handle = HeaderClientId, Effect = 0.5M};

            if (Moved != null)
            {
                draggable.Dropped += delegate(object sender, EventArgs evt)
                                         {
                                             if (Moved == null) return;
                                             Moved(sender, EventArgs.Empty);
                                         };
            }

            Aspects.Add(draggable);
        }

        /// <summary>
        /// Returns Id of the DOM element used for dragging.
        /// </summary>
        private string HeaderClientId
        {
            get { return string.Concat(ClientID, "_", HeaderId); }
        }

        /// <summary>
        /// Returns CssClass for the caption control.
        /// </summary>
        private string GetCssClassForCaption()
        {
            var iconCssClass = !string.IsNullOrEmpty(IconCssClass) ? "extendedpanel-icon" + IconCssClass : string.Empty;
            return CombineCssClass("extendedpanel-top-text", iconCssClass);
        }

        /// <summary>
        /// Returns CssClass for the toggle tool control.
        /// </summary>
        /// <returns></returns>
        private string GetCssClassForToggleTool()
        {
            return CombineCssClass("item", Collapsed ? "item-collapsed" : "item-expanded");
        }

        /// <summary>
        /// Combines provided css classes with the <see cref="ASP.WebControl.CssClass"/> property.
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
            ((IAjaxControl)this).AjaxControl.RegisterDefaultSkinStyleSheetFromResource(typeof(ExtendedPanel), Constants.DefaultSkinResource);

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
