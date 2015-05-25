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
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.Scripts.Button.js", "text/javascript")]
[assembly: WebResource("Gaia.WebWidgets.Scripts.ButtonControl.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    using HtmlFormatting;
    
    /// <summary>
    /// The Gaia Ajax Button displays a push button control on the Web
    /// page. It inherits from the <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.button.aspx">ASP.NET Button</a>, 
    /// and has built-in Ajax behaviour.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for Button" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\Button\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Codebehind for Button" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\Button\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    [ToolboxBitmap(typeof(Button), "Resources.Gaia.WebWidgets.Button.bmp")]
    public class Button : ASP.Button, IAspectableAjaxControl, IAjaxButtonControl
    {
        #region [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="Button"/> control.
        /// </summary>
        public class PropertyStateManagerButton : PropertyStateManagerButtonControl<Button>
        {
            private bool _useSubmitBehavior;
            private readonly Button _owner;
            
            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerButton"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerButton(Button control) : this(control, control.ClientID, null) { }

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerButton"/> for specified <paramref name="control"/>
            /// using specified <paramref name="clientId"/> for reference and specified <see cref="IExtraPropertyCallbackRenderer"/>
            /// for additional state change serialization.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <param name="clientId">The client-side ID of the <paramref name="control"/> to use.</param>
            /// <param name="extra">Provides additional state change rendering during callbacks.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerButton(Button control, string clientId, IExtraPropertyCallbackRenderer extra) : base(control, clientId, extra)
            {
                _owner = control;
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerButtonControl{T}"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerButtonControl{T}"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);

                var stateManager = source as PropertyStateManagerButton;
                if (stateManager == null) return;

                _useSubmitBehavior = stateManager._useSubmitBehavior;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _useSubmitBehavior = _owner.UseSubmitBehavior;
                base.TakeSnapshot();
            }

            /// <summary>
            /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void DiffSnapshot()
            {
                RenderChange(_useSubmitBehavior, _owner.UseSubmitBehavior, "setSubmit");
                base.DiffSnapshot();
            }
        }

        #endregion

        #region [ -- Private Members -- ]

        private EffectControl _effectControl;
        private AspectableAjaxControl _instance;
        private AspectableAjaxControl _aspectableAjaxControl;

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Gets or sets if the click even should bubble on the client.
        /// </summary>
        [DefaultValue(false), Category("Behavior"), Description("True if click event should bubble on the client.")]
        public virtual bool EnableBubbling
        {
            get { return StateUtil.Get(ViewState, "EnableBubbling", false); }
            set { StateUtil.Set(ViewState, "EnableBubbling", value, false); }
        }

        /// <summary>
        /// Event is triggered when the control looses focus
        /// </summary>
        [Description("Fires when the control looses focus")]
        public event EventHandler Blur
        {
            add { Aspects.Bind<AspectGeneric>().Blur += value; }
            remove { Aspects.Bind<AspectGeneric>().Blur -= value; }
        }

        /// <summary>
        /// Event is triggered when the control gets focus
        /// </summary>
        [Description("Fires when the control gets focus")]
        public event EventHandler Focused
        {
            add { Aspects.Bind<AspectGeneric>().Focus += value; }
            remove { Aspects.Bind<AspectGeneric>().Focus -= value; }
        }

         /// <summary>
        /// Event is triggered when the Mouse cursor is moved over the control
        /// </summary>
        [Description("Fires when the mouse cursor moves over the control")]
        public event EventHandler<AspectHoverable.HoverEventArgs> MouseOver
        {
            add { Aspects.Bind<AspectHoverable>().MouseOver += value; }
            remove { Aspects.Bind<AspectHoverable>().MouseOver -= value; }
        }

        /// <summary>
        /// Event is triggered when the Mouse cursor is moved outside of the control
        /// </summary>
        [Description("Fires when the mouse cursor is moved outside the control")]
        public event EventHandler MouseOut
        {
            add { Aspects.Bind<AspectHoverable>().MouseOut += value; }
            remove { Aspects.Bind<AspectHoverable>().MouseOut -= value; }
        }

        /// <summary>
        /// <see cref="IAspectableAjaxControl.Aspects"></see> for documentation for this method
        /// </summary>
        [Browsable(false)]
        public AspectCollection Aspects
        {
            get { return AspectableAjaxControl.Aspects; }
        }

        private EffectControl EffectControl
        {
            get { return _effectControl ?? (_effectControl = new EffectControl(this)); }
        }

        /// <summary>
        /// Collection of Effects for the Control. 
        /// </summary>
        [Browsable(false)]
        public EffectCollection Effects { get { return EffectControl.Effects; } }

        /// <summary>
        /// Sets input focus to a control.
        /// </summary>
        public override void Focus()
        {
            base.Focus();
            AspectableAjaxControl.Focus();
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// See <see cref="AjaxControl.OnInit" /> for documentation of this method
        /// </summary>
        /// <param name="e">The EventArgs passed on from the System</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AspectableAjaxControl.OnInit();
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// Forwards to <see cref="AjaxControl.TrackViewState" /> method.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            AspectableAjaxControl.TrackViewState();
        }

        /// <summary>
        /// See <see cref="AjaxControl.BeginLoadViewState" /> and <see cref="AjaxControl.EndLoadViewState" /> methods for documentation. 
        /// This method only forwards to those methods.
        /// </summary>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(AspectableAjaxControl.BeginLoadViewState(savedState));
            AspectableAjaxControl.EndLoadViewState();
        }

        /// <summary>
        /// See <see cref="AjaxControl.BeginLoadControlState" /> and <see cref="AjaxControl.EndLoadControlState" /> methods for documentation.
        /// This method only forwards to those methods.
        /// </summary>
        /// <param name="savedState">Saved control state</param>
        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(AspectableAjaxControl.BeginLoadControlState(savedState));
            AspectableAjaxControl.EndLoadControlState();
        }
       
        /// <summary>
        /// See also <see cref="AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            // Cannot bubble event if the button is in submit mode.
            if (UseSubmitBehavior && EnableBubbling)
                EnableBubbling = false;

            base.OnPreRender(e);
            AspectableAjaxControl.OnPreRender();
            EffectControl.OnPreRender();
        }

        /// <summary>
        /// See <see cref="AjaxControl.SaveViewState" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <returns>the saved ViewState</returns>
        protected override object SaveViewState()
        {
            return AspectableAjaxControl.SaveViewState(base.SaveViewState());
        }

        /// <summary>
        /// See <see cref="AjaxControl.SaveControlState" /> method for documentation. This method only forward to that method.
        /// </summary>
        /// <returns>Control state to save</returns>
        protected override object SaveControlState()
        {
            return AspectableAjaxControl.SaveControlState(base.SaveControlState());
        }

        /// <summary>
        /// See <see cref="AjaxControl.RenderControl" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to write the control into</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            AspectableAjaxControl.RenderControl(writer);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
        /// Forwards to <see cref="AjaxControl.OnUnload"/> method.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data. </param>
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            AspectableAjaxControl.OnUnload();
        }

        /// <summary>
        /// Creates a <see cref="PostBackOptions"/> object that represents the <see cref="Button" /> control's postback behavior.
        /// </summary>
        /// <returns>A <see cref="PostBackOptions"/> that represents the <see cref="Button" /> control's postback behavior.</returns>
        protected override PostBackOptions GetPostBackOptions() 
        {
            var options = base.GetPostBackOptions();
            options.ClientSubmit = false;

            if (CausesValidation)
            {
                options.PerformValidation = true;
                options.ValidationGroup = ValidationGroup;
            }
            
            return options;
        }

        #endregion

        #region [ -- Protected Methods for Inheritance -- ]

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected virtual void IncludeScriptFiles()
        {
            var manager = Manager.Instance;

            // Include Button Javascript stuff
            manager.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.ButtonControl.js", typeof(Manager), "Gaia.ButtonControl.loaded", true);
            manager.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Button.js", typeof(Manager), "Gaia.Button.browserFinishedLoading", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(XhtmlTagFactory create)
        {
            using (var writer = new ButtonControlWriter<Button>(create.GetHtmlTextWriter(), this))
            {
                base.RenderControl(writer);
            }
            
        }

        /// <summary>
        /// Returns control registration object required for registering control on the client.
        /// </summary>
        /// <param name="registerControl">Suggested control registration object.</param>
        /// <returns>Modified or new control registration object.</returns>
        protected virtual RegisterControl GetScript(RegisterControl registerControl)
        {
            return registerControl.AddFocus(AspectableAjaxControl)
                .AddPostBackOptions(this, GetPostBackOptions())
                .AddAspects(Aspects).AddEffects(Effects)
                .ObserveClick(EnableBubbling);
        }

        #endregion

        #region [ -- IAjaxControl Implementation -- ]

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new AspectableAjaxControl(this)); }
        }

        Control IAjaxControl.Control
        {
            get { return this; }
        }

        string IAjaxControl.TagName
        {
            get { return TagName; }
        }

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerButton(this);
        }

        string IAjaxControl.GetScript()
        {
            return GetScript(new RegisterControl("Gaia.Button", ClientID)).ToString();
        }

        void IAjaxControl.IncludeScriptFiles()
        {
            IncludeScriptFiles();
        }

        bool IAjaxControl.InDesigner
        {
            get { return DesignMode; }
        }

        void IAjaxControl.RenderControlHtml(XhtmlTagFactory create)
        {
            RenderControlHtml(create);
        }

        PropertyStateManagerControl IAjaxControl.StateManager
        {
            get { return AspectableAjaxControl.StateManager; }
        }

        #endregion

        #region [ -- IAspectableAjaxControl Implementation -- ]

        AspectableAjaxControl IAspectableAjaxControl.AspectableAjaxControl
        {
            get { return (AspectableAjaxControl)((IAspectableAjaxControl)this).AjaxControl; }
        }

        private AspectableAjaxControl AspectableAjaxControl
        {
            get { return _aspectableAjaxControl ?? (_aspectableAjaxControl = ((IAspectableAjaxControl) this).AspectableAjaxControl); }
        }

        #endregion

        #region [ -- IAjaxButtonControl implementation -- ]

        /// <summary>
        /// Returns a <see cref="PostBackOptions"/> object that represents the control's postback behavior.
        /// </summary>
        /// <returns>A <see cref="PostBackOptions"/> that represents the control's postback behavior.</returns>
        PostBackOptions IAjaxButtonControl.GetPostBackOptions()
        {
            return GetPostBackOptions();
        }

        #endregion
    }
}
