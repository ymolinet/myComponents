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
using System.ComponentModel;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;
using System.Drawing;

[assembly: WebResource("Gaia.WebWidgets.Scripts.MultiView.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    using HtmlFormatting;

    /// <summary>
    ///  The Gaia Ajax MultiView represents a control that acts as a container for a group of View controls. 
    /// It inherits from the <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.multiview.aspx">ASP.NET MultiView</a>, 
    /// and has built-in Ajax behaviour.<br />It is often a better alternative than using panels, because it will help you to only display one View at the time.
    /// </summary>
    [ToolboxBitmap(typeof(MultiView), "Resources.Gaia.WebWidgets.MultiView.bmp")]
    public class MultiView : ASP.MultiView, IAjaxContainerControl, INamingContainer
    {
        #region [ -- Private Members -- ]

        private AjaxContainerControl _instance;
        private AjaxContainerControl _ajaxContainerControl;
        private EffectControl _effectControl;

        #endregion

        #region [ -- Public Methods -- ]

        /// <summary>
        /// Forces the control to redraw and re-instantiate all of it child controls
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
            throw new InvalidOperationException("ForceAnUpdateWithAppending cannot append new Views since you can only have one ActiveView at any time");
        }

        /// <summary>
        /// Signalizes that new controls that never has been rendered will be appended after this method invocation.-
        /// </summary>
        public void TrackControlAdditions()
        {
            AjaxContainerControl.TrackControlAddition();
        }

        #endregion

        #region [ -- Properties -- ]

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
            // Include MultiView Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.MultiView.js", typeof(Manager), "Gaia.MultiView.browserFinishedLoading", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(XhtmlTagFactory create)
        {
            // Wrapper element
            using (create.Div(ClientID))
            {
                // Render active view
                System.Web.UI.WebControls.View activeView = GetActiveView();
                if (activeView != null)
                    activeView.RenderControl(create.GetHtmlTextWriter());
            }
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
            get { return "div"; }
        }

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerControl(this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.MultiView", ClientID)
                .AddFocus(AjaxContainerControl)
                .AddAspects(Aspects).AddEffects(Effects).ToString();
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
        /// <param name="create">XhtmlTagFactory to render into</param>
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
            RenderChildren(create.GetHtmlTextWriter());
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
