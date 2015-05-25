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
using System.Drawing;
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.Scripts.LinkButton.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// The Gaia Ajax LinkButton displays a hyperlink-style button control on a Web page. It inherits from the 
    /// <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.linkbutton.aspx"> ASP.NET LinkButton</a>, 
    /// and has built-in Ajax behaviour.
    /// </summary>
    [ToolboxBitmap(typeof(LinkButton), "Resources.Gaia.WebWidgets.LinkButton.bmp")]
    public class LinkButton : ASP.LinkButton, IAspectableAjaxControl, IAjaxButtonControl
    {
        /// <summary>
        /// Specialized <see cref="HtmlTextWriter"/> for <see cref="LinkButton"/>
        /// which removes postback references from the href attribute.
        /// </summary>
        private sealed class LinkButtonWriter : HtmlTextWriter
        {
            private readonly LinkButton _owner;

            /// <summary>
            /// Initializes a new instance of the <see cref="LinkButtonWriter"/> class that uses a default tab string.
            /// </summary>
            /// <param name="writer">The <see cref="T:System.IO.TextWriter"/> instance that renders the markup content.</param>
            /// <param name="owner">Owner button control.</param>
            public LinkButtonWriter(TextWriter writer, LinkButton owner) : base(writer)
            {
                if (owner == null)
                    throw new ArgumentNullException("owner");

                _owner = owner;
            }

            /// <summary>
            /// Adds the markup attribute and the attribute value to the opening tag of the element that the <see cref="T:System.Web.UI.HtmlTextWriter"/> object 
            /// creates with a subsequent call to the <see cref="HtmlTextWriter.RenderBeginTag(string)"/> method.
            /// </summary>
            /// <param name="key">
            /// An <see cref="T:System.Web.UI.HtmlTextWriterAttribute"/> that represents the markup attribute to add to the output stream. 
            /// </param>
            /// <param name="value">A string containing the value to assign to the attribute.</param>
            public override void AddAttribute(HtmlTextWriterAttribute key, string value)
            {
                var attributeValue = value;

                if (key == HtmlTextWriterAttribute.Href)
                {
                    var postBackOptions = _owner.GetPostBackOptions();
                    var postBackReference = _owner.Page.ClientScript.GetPostBackEventReference(postBackOptions, false);

                    if (postBackReference != null)
                        attributeValue = value.Replace(postBackReference, "javascript:void(0)");
                }

                base.AddAttribute(key, attributeValue);
            }
        }

        #region [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="LinkButton"/> control.
        /// </summary>
        public class PropertyStateManagerLinkButton : PropertyStateManagerButtonControl<LinkButton>
        {
            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerLinkButton"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerLinkButton(LinkButton control) : this(control, control.ClientID, null) { }

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerLinkButton"/> for specified <paramref name="control"/>
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
            public PropertyStateManagerLinkButton(LinkButton control, string clientId, IExtraPropertyCallbackRenderer extra) : base(control, clientId, extra) { }
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
            get { return StateUtil.Get(ViewState, "EnableBubbling", true); }
            set { StateUtil.Set(ViewState, "EnableBubbling", value, true); }
        }

        /// <summary>
        /// Raised when the control looses focus.
        /// </summary>
        [Description("Raised when the control looses focus.")]
        public event EventHandler Blur
        {
            add { Aspects.Bind<AspectGeneric>().Blur += value; }
            remove { Aspects.Bind<AspectGeneric>().Blur -= value; }
        }

        /// <summary>
        /// Raised when the control gets focus.
        /// </summary>
        [Description("Raised when the control gets focus.")]
        public event EventHandler Focused
        {
            add { Aspects.Bind<AspectGeneric>().Focus += value; }
            remove { Aspects.Bind<AspectGeneric>().Focus -= value; }
        }

        /// <summary>
        /// Raised when the mouse is moved over the control.
        /// </summary>
        [Description("Raised when the mouse cursor moves over the control.")]
        public event EventHandler<AspectHoverable.HoverEventArgs> MouseOver
        {
            add { Aspects.Bind<AspectHoverable>().MouseOver += value; }
            remove { Aspects.Bind<AspectHoverable>().MouseOver -= value; }
        }

        /// <summary>
        /// Raised when the mouse cursor is moved outside the control.
        /// </summary>
        [Description("Raised when the mouse cursor is moved outside the control.")]
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

        #region [ -- Overriden base class methods -- ]

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
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object and 
        /// stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
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
        /// Creates a <see cref="T:System.Web.UI.PostBackOptions"/> object that represents the <see cref="T:System.Web.UI.WebControls.LinkButton"/> control's postback behavior.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.UI.PostBackOptions"/> that represents the <see cref="T:System.Web.UI.WebControls.LinkButton"/> control's postback behavior.
        /// </returns>
        protected override PostBackOptions GetPostBackOptions()
        {
            var options = base.GetPostBackOptions();

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
            // LinkButton Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.ButtonControl.js", typeof(Manager), "Gaia.ButtonControl.loaded", true);
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.LinkButton.js", typeof(Manager), "Gaia.LinkButton.browserFinishedLoading", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(HtmlFormatting.XhtmlTagFactory create)
        {
            using (var writer = new LinkButtonWriter(create.GetHtmlTextWriter(), this))
            {
                base.RenderControl(writer);
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
            return new PropertyStateManagerLinkButton(this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.LinkButton", ClientID)
                .AddFocus(AspectableAjaxControl)
                .AddPostBackOptions(this, GetPostBackOptions())
                .AddAspects(Aspects).AddEffects(Effects)
                .InvokeIf(!IsEnabled, "setEnabled", false, 0)
                .ObserveClick(EnableBubbling).ToString();
        }

        void IAjaxControl.IncludeScriptFiles()
        {
            IncludeScriptFiles();
        }

        bool IAjaxControl.InDesigner
        {
            get { return DesignMode; }
        }

        void IAjaxControl.RenderControlHtml(HtmlFormatting.XhtmlTagFactory create)
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
            get 
            {
                return _aspectableAjaxControl ??
                       (_aspectableAjaxControl = ((IAspectableAjaxControl) this).AspectableAjaxControl);
            }
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
