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

[assembly: WebResource("Gaia.WebWidgets.Scripts.TextBox.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    using HtmlFormatting;
    
    /// <summary>
    /// The Gaia Ajax TextBox displays a text box control for user input. It inherits from the 
    /// <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.textbox.aspx">
    /// ASP.NET TextBox</a>, and has built-in Ajax behaviour.
    /// </summary>
    [ToolboxBitmap(typeof(TextBox), "Resources.Gaia.WebWidgets.TextBox.bmp")]
    public class TextBox : ASP.TextBox, IAspectableAjaxControl
    {
        #region [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="TextBox"/>.
        /// </summary>
        public class PropertyStateManagerTextBox : PropertyStateManagerWebControl, IMayRequireRerendering
        {
            private ASP.TextBox _state;
            private bool _keyChangeEvents;
            private int _keyChangeInterval;

            private readonly TextBox _owner;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerWebControl"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerTextBox(TextBox control) : base(control)
            {
                _owner = control;
            }

            /// <summary>
            /// Returns true if the associated <see cref="TextBox"/> needs to be rerendered.
            /// </summary>
            bool IMayRequireRerendering.RequiresRerendering
            {
                get { return _state.TextMode != _owner.TextMode; }
            }

            /// <summary>
            /// Clears dirtiness of the <see cref="ITextControl.Text"/> property.
            /// </summary>
            public void ClearDirtyText()
            {
                _state.Text = _owner.Text;
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);

                var stateManager = source as PropertyStateManagerTextBox;
                if (stateManager == null) return;

                _state = stateManager._state;
                _keyChangeEvents = stateManager._keyChangeEvents;
                _keyChangeInterval = stateManager._keyChangeInterval;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _keyChangeEvents = _owner.KeyChangeEvents;
                _keyChangeInterval = _owner.KeyChangeEventsInterval;

                _state = new ASP.TextBox
                             {
                                 Text = _owner.Text,
                                 ReadOnly = _owner.ReadOnly,
                                 TextMode = _owner.TextMode,
                                 AutoPostBack = _owner.AutoPostBack,
                                 ValidationGroup = _owner.ValidationGroup,
                                 CausesValidation = _owner.CausesValidation
                             };

                base.TakeSnapshot();
            }

            /// <summary>
            /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void DiffSnapshot()
            {
                RenderChange(_state.Text, _owner.Text, "setText");
                RenderChange(_state.ReadOnly, _owner.ReadOnly, "setROnly");
                RenderChange(_state.AutoPostBack, _owner.AutoPostBack, "setAutoPostBack");
                RenderChange(_keyChangeEvents, _owner.KeyChangeEvents, "setKeyChangeEvents");
                RenderChange(_keyChangeInterval, _owner.KeyChangeEventsInterval, "setKeyChangeEventsInterval");
                RenderChange(_state.ValidationGroup, _owner.ValidationGroup, Constants.SetValidationGroupFunctionName);
                RenderChange(_state.CausesValidation, _owner.CausesValidation, Constants.SetCausesValidationFunctionName);

                base.DiffSnapshot();

                // this should be done after rendering all changes, including style changes.
                RenderChange(false, _owner.SelectAllText, "setSelectAll");
            }
        }

        #endregion

        #region [ -- Private Members -- ]

        private AjaxControl _instance;
        private EffectControl _effectControl;
        private AspectableAjaxControl _aspectableAjaxControl;
        
        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Event is triggered when text is selected
        /// </summary>
        [Description("Fires when text is selected")]
        public event EventHandler Select
        {
            add { Aspects.Bind<AspectGeneric>().Select += value; }
            remove { Aspects.Bind<AspectGeneric>().Select -= value; }
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
        /// Event is fired when the control is clicked. Use the eventArgs to retrieve the mouse coordinates for the 
        /// click event
        /// </summary>
        [Description("Event is fired when the control is clicked")]
        public event EventHandler<AspectClickable.ClickEventArgs> Click
        {
            add { Aspects.Bind<AspectClickable>().Clicked += value; }
            remove { Aspects.Bind<AspectClickable>().Clicked -= value; }
        }

        /// <summary>
        /// Event is fired when the control is double clicked. Use the eventArgs to retrieve the mouse coordinates for the 
        /// click event
        /// </summary>
        [Description("Event is fired when the control is double clicked")]
        public event EventHandler<AspectClickable.ClickEventArgs> DoubleClick
        {
            add { Aspects.Bind<AspectClickable>().DblClicked += value; }
            remove { Aspects.Bind<AspectClickable>().DblClicked -= value; }
        }

        /// <summary>
        /// Gets or sets if key events on the <see cref="TextBox"/> should trigger a callback.
        /// </summary>
        /// <seealso cref="KeyChangeEventsInterval"/>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Key event callback triggering.")]
        public bool KeyChangeEvents
        {
            get { return StateUtil.Get(ViewState, "KeyChangeEvents", false); }
            set { StateUtil.Set(ViewState, "KeyChangeEvents", value, false); }
        }

        /// <summary>
        /// Gets or sets the interval in milliseconds when a callback to the server will occur after key events.
        /// </summary>
        /// <remarks>
        /// The value of this property is used only when the <see cref="KeyChangeEvents"/> property is set to True.
        /// </remarks>
        /// <seealso cref="KeyChangeEventsInterval"/>
        [DefaultValue(500)]
        [Category("Behavior")]
        [Description("Key event callback interval.")]
        public int KeyChangeEventsInterval
        {
            get { return StateUtil.Get(ViewState, "KeyChangeEventsInterval", 500); }
            set { StateUtil.Set(ViewState, "KeyChangeEventsInterval", value, 500); }
        }

        private bool SelectAllText { get; set; }

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

        /// <summary>
        /// Selects all text in the textbox
        /// </summary>
        public void SelectAll()
        {
            SelectAllText = true;
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
        /// See <see cref="AjaxControl.ExecuteLoadPostData" /> method for documentation. This method only forwards to that method.
        /// </summary>
        protected override bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            return AspectableAjaxControl.ExecuteLoadPostData(base.LoadPostData, postDataKey, postCollection,
                                                             stateManager => ((PropertyStateManagerTextBox) stateManager).ClearDirtyText());
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
        /// See <see cref="AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
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
        /// Rendering
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

        #endregion

        #region [ -- Protected Methods for Inheritance -- ]

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected virtual void IncludeScriptFiles()
        {
            // Add TextBox Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.TextBox.js", typeof(Manager), "Gaia.TextBox.loaded", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(XhtmlTagFactory create)
        {
            // Since the AutoPostBack if true renders this incredible stupid JavaScript in 
            // the "onclick" property, we basically "hack" it out right here
            var hadAutoPostBack = AutoPostBack;

            using (new AtomicInvoker(
            delegate
            {
                AutoPostBack = false;
            },
            delegate
            {
                AutoPostBack = hadAutoPostBack;
            }))
            {
                base.RenderControl(create.GetHtmlTextWriter());
            }
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
            return new PropertyStateManagerTextBox(this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.TextBox", ClientID)
               .AddProperty("keyChangeEvents", KeyChangeEvents)
               .AddPropertyIfTrue(KeyChangeEvents, "keyChangeEventsInterval", KeyChangeEventsInterval.ToString(System.Globalization.NumberFormatInfo.InvariantInfo), false)
               .AddFocus(AspectableAjaxControl)
               .AddValidation(CausesValidation, ValidationGroup)
               .AddAspects(Aspects).AddEffects(Effects)
               .InvokeIf(SelectAllText, "setSelectAll", false, 1)
               .InvokeIf(AutoPostBack, "setAutoPostBack", false, "true").ToString();
        }

        bool IAjaxControl.InDesigner
        {
            get { return DesignMode; }
        }

        void IAjaxControl.IncludeScriptFiles()
        {
            IncludeScriptFiles();
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

        #region IAspectableAjaxControl Members

        AspectableAjaxControl IAspectableAjaxControl.AspectableAjaxControl
        {
            get { return (AspectableAjaxControl)((IAspectableAjaxControl)this).AjaxControl; }
        }

        private AspectableAjaxControl AspectableAjaxControl
        {
            get 
            {
                return _aspectableAjaxControl ??
                       (_aspectableAjaxControl = ((IAspectableAjaxControl) this).AspectableAjaxControl);
            }
        }

        #endregion
    }
}
