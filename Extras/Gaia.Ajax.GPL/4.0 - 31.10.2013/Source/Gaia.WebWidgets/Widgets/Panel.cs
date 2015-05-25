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

[assembly: WebResource("Gaia.WebWidgets.Scripts.Container.js", "text/javascript")]
[assembly: WebResource("Gaia.WebWidgets.Scripts.Panel.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    using HtmlFormatting;
    
    /// <summary>
    /// The Gaia Ajax Panel represents a control that acts as a container for other controls. It inherits from the 
    /// <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.panel.aspx"> ASP.NET Panel</a>, 
    /// and has built-in Ajax behavior.
    /// </summary>
    [ToolboxBitmap(typeof(Panel), "Resources.Gaia.WebWidgets.Panel.bmp")]
    public class Panel : ASP.Panel, IAjaxContainerControl, INamingContainer
    {
        #region [ -- Private Members -- ]

        private EffectControl _effectControl;
        private AjaxContainerControl _instance;
        private AjaxContainerControl _ajaxContainerControl;

        #endregion

        #region [ -- Public Methods -- ]
        
        /// <summary>
        /// Forces the control to redraw and re-instantiate all of it child controls
        /// See <see cref="IAjaxContainerControl.ForceAnUpdate" /> for documentation on this method
        /// </summary>
        public void ForceAnUpdate()
        {
            AjaxContainerControl.ForceAnUpdate();
        }

        /// <summary>
        /// Forces the control to redraw and append new chlid controls to existing ones
        /// </summary>
        public void ForceAnUpdateWithAppending()
        {
            AjaxContainerControl.ForceAnUpdateWithAppending();
        }

        /// <summary>
        /// TrackControlAdditions is used in combination with ForceAnUpdate() or ForceAnUpdateWithAppending()
        /// It basically signals that all controls added after calling this method will be appended to the container
        /// control. Useful for minimizing network traffic when you only need to append one new control at the bottom
        /// of the control, for example TreeViewItems, Panels and similar. 
        /// </summary>
        public void TrackControlAdditions()
        {
            AjaxContainerControl.TrackControlAddition();
        }

        #endregion

        #region [ -- Properties -- ]

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

        /// <summary>
        /// Sets input focus to a control.
        /// </summary>
        public override void Focus()
        {
            base.Focus();
            AjaxContainerControl.Focus();
        }
       
        /// <summary>
        /// Gets or sets the direction in which to display controls that include text
        /// in a System.Web.UI.WebControls.Panel control.
        /// </summary>
        /// <value>
        /// One of the System.Web.UI.WebControls.ContentDirection enumeration values.
        /// The default is NonSet.
        /// </value>
        [AjaxSerializable("setDirection")]
        public override ASP.ContentDirection Direction
        {
            get { return base.Direction; }
            set { base.Direction = value; }
        }

        /// <summary>
        /// Gets or sets the caption for the group of controls that is contained in the panel control.
        /// </summary>
        /// <value>
        /// The caption text for the child controls contained in the panel control.
        /// The default is empty string.
        /// </value>
        [AjaxSerializable("setGroupingText")]
        public override string GroupingText
        {
            get { return base.GroupingText; }
            set { base.GroupingText = value; }
        }

        /// <summary>
        /// Gets or sets the visibility and position of scroll bars in a <see cref="Panel"/> control.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ASP.ScrollBars"/> enumeration values. The default is None.
        /// </returns>
        [AjaxSerializable("setBars")]
        public override ASP.ScrollBars ScrollBars
        {
            get { return base.ScrollBars; }
            set { base.ScrollBars = value; }
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
        /// See also <see cref="AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
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
        /// See <see cref="AjaxControl.RenderControl" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <param name="writer">The <see cref="HtmlTextWriter"/> to write the control into.</param>
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
            // Include Panel Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Panel.js", typeof(Manager), "Gaia.Panel.browserFinishedLoading", true);
        }

        /// <summary>
        /// Renders HTML markup for the control into the specified <see cref="XhtmlTagFactory"/> instance.
        /// </summary>
        /// <param name="create">Used to create HTML markup for the control.</param>
        protected virtual void RenderControlHtml(XhtmlTagFactory create)
        {
            // for xhtml compliant markup accesskey and tabindex should be disabled
            var tabIndex = TabIndex;
            var accesskey = AccessKey;
            var defaultButton = DefaultButton;
            
            using (new AtomicInvoker(
            delegate
            {
                TabIndex = 0;
                AccessKey = string.Empty;
                DefaultButton = string.Empty;
            },
            delegate
            {
                TabIndex = tabIndex;
                AccessKey = accesskey;
                DefaultButton = defaultButton;
            }))
            {
                base.RenderControl(create.GetHtmlTextWriter());
            }
        }

        /// <summary>
        /// Render the content (ie. Children) of the Panel
        /// </summary>
        /// <param name="create">XhtmlTagFactory</param>
        protected virtual void RenderControlContents(XhtmlTagFactory create)
        {
            RenderChildren(create.GetHtmlTextWriter());
        }

        /// <summary>
        /// Used by derived classes to register default button in <see cref="GetScript"/>.
        /// </summary>
        /// <param name="registerControl">Control registration object.</param>
        /// <returns>Control registration object with possibly modified attributes for chaining.</returns>
        protected RegisterControl RegisterDefaultButton(RegisterControl registerControl)
        {
            string defaultButton = string.Empty;
            if (!DesignMode && Page != null && DefaultButton.Length > 0)
            {
                Control button = FindControl(DefaultButton);
                if (!(button is ASP.IButtonControl))
                    throw new InvalidOperationException(string.Concat("Only IButtonControl Can Be Default Button For '", ID, "'"));

                defaultButton = button.ClientID;
            }

            registerControl.AddPropertyIfTrue(defaultButton.Length > 0, "db", defaultButton, true);
            return registerControl;
        }

        /// <summary>
        /// Returns control registration object required for registering control on the client.
        /// </summary>
        /// <param name="registerControl">Suggested control registration object.</param>
        /// <returns>Modified or new control registration object.</returns>
        protected virtual RegisterControl GetScript(RegisterControl registerControl)
        {
            return RegisterDefaultButton(registerControl)
                .AddFocus(AjaxContainerControl)
                .AddAspects(Aspects).AddEffects(Effects);
        }

        #endregion

        #region [ -- IAjaxControl Implementation -- ]

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new AjaxContainerControl(this)); }
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
            return new PropertyStateManagerWebControl(this, ClientID);
        }

        string IAjaxControl.GetScript()
        {
            return GetScript(new RegisterControl("Gaia.Panel", ClientID)).ToString();
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
            get { return AjaxContainerControl.StateManager; }
        }

        #endregion

        #region [ -- IAspectableAjaxControl Implementation -- ]

        AspectableAjaxControl IAspectableAjaxControl.AspectableAjaxControl
        {
            get { return AjaxContainerControl; }
        }

        #endregion

        #region [ -- IAjaxContainerControl Implementation -- ]

        string IAjaxContainerControl.GetDOMContainer(IAjaxControl child)
        {
            return ClientID;
        }

        void IAjaxContainerControl.RenderChildrenOnForceAnUpdate(XhtmlTagFactory create)
        {
            if (AjaxContainerControl.IsTrackingControlAdditions)
                RenderChildren(create.GetHtmlTextWriter());
            else
                RenderControlContents(create);
        }

        AjaxContainerControl IAjaxContainerControl.AjaxContainerControl
        {
            get { return (AjaxContainerControl)((IAjaxContainerControl)this).AjaxControl; }
        }

        private AjaxContainerControl AjaxContainerControl
        {
            get { return _ajaxContainerControl ?? (_ajaxContainerControl = ((IAjaxContainerControl)this).AjaxContainerControl); }
        }

        #endregion
    }
}
