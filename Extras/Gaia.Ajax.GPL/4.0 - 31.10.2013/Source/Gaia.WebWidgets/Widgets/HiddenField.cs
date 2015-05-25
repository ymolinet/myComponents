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
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.Scripts.HiddenField.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// The Gaia Ajax HiddenField represents a hidden field used to store a non-displayed value. It inherits from the ASP.NET Button,
    /// and has built-in Ajax behaviour.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for HiddenField" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\HiddenField\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Codebehind for HiddenField" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\HiddenField\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    [ToolboxBitmap(typeof(HiddenField), "Resources.Gaia.WebWidgets.HiddenField.bmp")]
    public class HiddenField : ASP.HiddenField, IAjaxControl
    {
        #region [ -- Private Members -- ]

        private AjaxControl _instance;
        private AjaxControl _ajaxControl;

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// The value of the hidden field in string representation.
        /// </summary>
        [AjaxSerializable("setValue")]
        public override string Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.OnInit" /> for documentation of this method
        /// </summary>
        /// <param name="e">The EventArgs passed on from the System</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AjaxControl.OnInit();
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.ExecuteLoadPostData" /> method for documentation. This method only forwards to that method.
        /// </summary>
        protected override bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            return AjaxControl.ExecuteLoadPostData(base.LoadPostData, postDataKey, postCollection,
                                                   stateManager => stateManager.ClearDirty("Value"));
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// Forwards to <see cref="Gaia.WebWidgets.AjaxControl.TrackViewState" /> method.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            AjaxControl.TrackViewState();
        }

        /// <summary>
        /// See <see cref="Gaia.WebWidgets.AjaxControl.BeginLoadViewState" /> and <see cref="Gaia.WebWidgets.AjaxControl.EndLoadViewState" /> methods for documentation. 
        /// This method only forwards to those methods.
        /// </summary>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(AjaxControl.BeginLoadViewState(savedState));
            AjaxControl.EndLoadViewState();
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.BeginLoadControlState" /> and <see cref="WebWidgets.AjaxControl.EndLoadControlState" /> methods for documentation.
        /// This method only forwards to those methods.
        /// </summary>
        /// <param name="savedState">Saved control state</param>
        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(AjaxControl.BeginLoadControlState(savedState));
            AjaxControl.EndLoadControlState();
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AjaxControl.OnPreRender();
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.SaveViewState" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <returns>the saved ViewState</returns>
        protected override object SaveViewState()
        {
            return AjaxControl.SaveViewState(base.SaveViewState());
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.SaveControlState" /> method for documentation. This method only forward to that method.
        /// </summary>
        /// <returns>Control state to save</returns>
        protected override object SaveControlState()
        {
            return AjaxControl.SaveControlState(base.SaveControlState());
        }

        /// <summary>
        /// Rendering
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to write the control into</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            AjaxControl.RenderControl(writer);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
        /// Forwards to <see cref="WebWidgets.AjaxControl.OnUnload"/> method.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data. </param>
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            AjaxControl.OnUnload();
        }

        #endregion

        #region [ -- Protected Methods for Inheritance -- ]

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected virtual void IncludeScriptFiles()
        {
            // Include HiddenField Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.HiddenField.js", typeof(Manager), "Gaia.HiddenField.browserFinishedLoading", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(HtmlFormatting.XhtmlTagFactory create)
        {
            base.RenderControl(create.GetHtmlTextWriter());
        }

        #endregion

        #region [ -- IAjaxControl Implementation -- ]

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new AjaxControl(this)); }
        }

        Control IAjaxControl.Control
        {
            get { return this; }
        }

        string IAjaxControl.TagName
        {
            get { return "input"; }
        }

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerControl(this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.HiddenField", ClientID).ToString();
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
            get { return AjaxControl.StateManager; }
        }

        private AjaxControl AjaxControl
        {
            get { return _ajaxControl ?? (_ajaxControl = ((IAjaxControl) this).AjaxControl); }
        }

        #endregion
    }
}
