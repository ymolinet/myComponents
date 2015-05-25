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
using System.Drawing;
using System.Web.UI;
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.Scripts.Label.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    using HtmlFormatting;

    /// <summary>
    /// The Gaia Ajax Label displays text on a Web page. 
    /// It inherits from the <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.label.aspx">ASP.NET Label</a>, 
    /// and has built-in Ajax behaviour.
    /// </summary>
    [ToolboxBitmap(typeof(Label), "Resources.Gaia.WebWidgets.Label.bmp")]
    public class Label : ASP.Label, IAjaxContainerControl
    {
        #region [ -- Ajax Control Implementation -- ]

        /// <summary>
        /// Specialized implementation of <see cref="AjaxContainerControl"/> for <see cref="Label"/>.
        /// </summary>
        public class LabelAjaxControl : AjaxContainerControl
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="control">Control which owns this instance.</param>
            public LabelAjaxControl(IAjaxContainerControl control) : base(control) { }

            /// <summary>
            /// Used by derived controls to know if the control should be rendered or not
            /// based on the facts if the ShouldRender was set or control was previously rendered
            /// </summary>
            protected internal override bool ShouldRenderAjaxControl
            {
                get
                {
                    var shouldRender = base.ShouldRenderAjaxControl;

                    if (!shouldRender)
                    {
                        var stateManager = StateManager as PropertyStateManagerLabel;
                        shouldRender = stateManager != null && stateManager.RequiresRerendering;
                    }

                    return shouldRender;
                }
            }
        }

        #endregion

        #region [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="Label"/>.
        /// </summary>
        public class PropertyStateManagerLabel : PropertyStateManagerWebControl
        {
            private string _text;
            private bool _hadControls;

            private readonly Label _label;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerLabel"/> for specified <paramref name="label"/>.
            /// </summary>
            /// <param name="label">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="label"/> is null.</exception>
            public PropertyStateManagerLabel(Label label) : base(label)
            {
                _label = label;
            }

            /// <summary>
            /// Returns true if the associated <see cref="TableCell"/> requires rerendering.
            /// </summary>
            internal bool RequiresRerendering
            {
                get
                {
                    if (_hadControls)
                        return !_label.HasControls();

                    return _label.HasControls();
                }
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);

                var stateManager = source as PropertyStateManagerLabel;
                if (stateManager == null) return;

                _text = stateManager._text;
                _hadControls = stateManager._hadControls;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _text = _label.Text;
                base.TakeSnapshot();
            }

            /// <summary>
            /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void DiffSnapshot()
            {
                if (!_hadControls && !_label.HasControls())
                    RenderChange(_text, _label.Text, "setText", true);

                base.DiffSnapshot();
            }

            /// <summary>
            /// Raised after the state snapshot for the associated <see cref="Control"/> is taken.
            /// </summary>
            /// <returns>
            /// True if event was consumed and the bubbling is cancelled. Otherwise, false.
            /// Default value is false.
            /// </returns>
            /// <remarks>
            /// Bubbles up until stopped by return value of true.
            /// </remarks>
            protected override bool OnSnapshotTakenEvent(object sender, EventArgs eventArgs)
            {
                _hadControls = _hadControls || sender is IAjaxControl;
                return false;
            }
        }

        #endregion

        #region [ -- Private members -- ]

        private AjaxControl _instance;
        private EffectControl _effectControl;
        private AjaxContainerControl _ajaxContainerControl;
        
        #endregion

        #region [ -- Label properties -- ]

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
            get { return AjaxContainerControl.Aspects; }
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

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// See <see cref="AjaxControl.OnInit" /> for documentation of this method
        /// </summary>
        /// <param name="e">The EventArgs passed on from the System</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AjaxContainerControl.OnInit();
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// Forwards to <see cref="AjaxControl.TrackViewState" /> method.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            AjaxContainerControl.TrackViewState();
        }

        /// <summary>
        /// See <see cref="AjaxControl.BeginLoadViewState" /> and <see cref="AjaxControl.EndLoadViewState" /> methods for documentation. 
        /// This method only forwards to those methods.
        /// </summary>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(AjaxContainerControl.BeginLoadViewState(savedState));
            AjaxContainerControl.EndLoadViewState();
        }

        /// <summary>
        /// See <see cref="AjaxControl.BeginLoadControlState" /> and <see cref="AjaxControl.EndLoadControlState" /> methods for documentation.
        /// This method only forwards to those methods.
        /// </summary>
        /// <param name="savedState">Saved control state</param>
        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(AjaxContainerControl.BeginLoadControlState(savedState));
            AjaxContainerControl.EndLoadControlState();
        }

        /// <summary>
        /// See <see cref="AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AjaxContainerControl.OnPreRender();
            EffectControl.OnPreRender();
        }

        /// <summary>
        /// See <see cref="AjaxControl.SaveViewState" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <returns>the saved ViewState</returns>
        protected override object SaveViewState()
        {
            return AjaxContainerControl.SaveViewState(base.SaveViewState());
        }

        /// <summary>
        /// See <see cref="AjaxControl.SaveControlState" /> method for documentation. This method only forward to that method.
        /// </summary>
        /// <returns>Control state to save</returns>
        protected override object SaveControlState()
        {
            return AjaxContainerControl.SaveControlState(base.SaveControlState());
        }

        /// <summary>
        /// Rendering
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to write the control into</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            AjaxContainerControl.RenderControl(writer);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
        /// Forwards to <see cref="AjaxControl.OnUnload"/> method.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data. </param>
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            AjaxContainerControl.OnUnload();
        }

        #endregion

        #region [ -- Protected Methods for Inheritance -- ]

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected virtual void IncludeScriptFiles()
        {
            // Include Label Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Label.js", typeof(Manager), "Gaia.Label.loaded", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(XhtmlTagFactory create)
        {
            base.RenderControl(create.GetHtmlTextWriter());
        }

        #endregion

        #region [ -- IAjaxControl Implementation -- ]

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new LabelAjaxControl(this)); }
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
            return new PropertyStateManagerLabel(this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.Label", ClientID).AddAspects(Aspects).AddEffects(Effects).ToString();
        }

        void IAjaxControl.IncludeScriptFiles()
        {
            IncludeScriptFiles();
        }

        bool IAjaxControl.InDesigner
        {
            get { return DesignMode; }
        }

        /// <summary>
        /// Calls base class RenderControl method
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use to create XHTML</param>
        void IAjaxControl.RenderControlHtml(XhtmlTagFactory create)
        {
            RenderControlHtml(create);
        }

        PropertyStateManagerControl IAjaxControl.StateManager
        {
            get { return AjaxContainerControl.StateManager; }
        }

        #endregion

        #region [ -- IAspectableAjaxControl Implementation -- ]

        AspectableAjaxControl IAspectableAjaxControl.AspectableAjaxControl
        {
            get { return ((IAspectableAjaxControl) this).AspectableAjaxControl; }
        }

        private AjaxContainerControl AjaxContainerControl
        {
            get
            {
                return _ajaxContainerControl ??
                       (_ajaxContainerControl = ((IAjaxContainerControl) this).AjaxContainerControl);
            }
        }

        #endregion

        #region [ -- IAjaxContainerControl Implementation -- ]

        /// <summary>
        /// Forces a re-rendering of the child controls of the widget.
        /// </summary>
        void IAjaxContainerControl.ForceAnUpdate()
        {
            AjaxContainerControl.ForceAnUpdate();
        }

        /// <summary>
        /// Does the exact same thing as ForceAnUpdate except it only re-renders the child controls
        /// that are not previously rendered and it appends all child controls at the back of the list
        /// of DOM elements on the Client.
        /// </summary>
        void IAjaxContainerControl.ForceAnUpdateWithAppending()
        {
            AjaxContainerControl.ForceAnUpdateWithAppending();
        }

        /// <summary>
        /// TrackControlAdditions is used in combination with ForceAnUpdate() or ForceAnUpdateWithAppending().
        /// </summary>
        void IAjaxContainerControl.TrackControlAdditions()
        {
            AjaxContainerControl.TrackControlAddition();
        }

        /// <summary>
        /// Called when ForceAnUpdate is dispatched for a control and the
        /// control needs to re-render its child control collection.
        /// </summary>
        void IAjaxContainerControl.RenderChildrenOnForceAnUpdate(XhtmlTagFactory create)
        {
            RenderContents(create.GetHtmlTextWriter());
        }

        /// <summary>
        /// Retrieves actual AjaxContainerControl associated with the Control
        /// </summary>
        AjaxContainerControl IAjaxContainerControl.AjaxContainerControl
        {
            get { return (AjaxContainerControl)((IAjaxContainerControl)this).AjaxControl; }
        }

        /// <summary>
        /// Returns id of the DOM element which acts as the actual container
        /// for the specified child. Used during dynamic rendering.
        /// </summary>
        /// <param name="child">Child control to get container for</param>
        /// <returns>ID of the DOM element which should contain specified child</returns>
        string IAjaxContainerControl.GetDOMContainer(IAjaxControl child)
        {
            return ClientID;
        }

        #endregion
    }
}
