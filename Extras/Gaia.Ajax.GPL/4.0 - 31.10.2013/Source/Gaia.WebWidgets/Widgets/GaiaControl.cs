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

namespace Gaia.WebWidgets
{
   
    /// <summary>
    /// Gaia Base Control, useful if you need to create your own Ajax Control and don't want to 
    /// start from scratch. If inheriting from this class you get a LOT for free when creating your own "custom 
    /// extension widgets".
    /// This class inherits from the System.Web.UI.Control and is useful if you create "invisible" Ajax Controls like
    /// for instance our Timer control which is inheriting from this class. It might also be a useful base class 
    /// for your extension controls if you don't need the overhead from the System.Web.UI.WebControls.WebControl class
    /// like style collection, Border, BackgroundColor and so on. It is our most lightweight Ajax Extension Control
    /// base class.
    /// 
    /// Very useful class if you need to implement your own Ajax Extensions Controls with as few lines of code 
    /// as possible.
    /// </summary>
    public abstract class GaiaControl : Control, IAjaxControl
    {
        #region [ -- Private Members -- ]

        private AjaxControl _instance;
        private AjaxControl _ajaxControl;

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
        { }

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
            get { return "span"; }
        }

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerControl(this);
        }

        string IAjaxControl.GetScript()
        {
            return string.Empty;
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
            get { return _ajaxControl ?? (_ajaxControl = ((IAjaxControl)this).AjaxControl); }
        }

        #endregion
    }
}
