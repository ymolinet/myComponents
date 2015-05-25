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

[assembly: WebResource("Gaia.WebWidgets.Scripts.ListControl.js", "text/javascript")]
[assembly: WebResource("Gaia.WebWidgets.Scripts.ListBox.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    ///  The Gaia Ajax ListBox creates a single-selection or multiselection list box. It inherits from the 
    /// <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.listbox.aspx" target="_blank">ASP.NET ListBox</a>, 
    /// and has built-in Ajax behaviour.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for ListBox" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\ListBox\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Codebehind for ListBox" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\ListBox\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// <code title="Ordering Items in a ListBox (ASPX)" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\ListBox\OrderedListBox\Default.aspx" />
    /// </code> 
    /// <code title="Ordering Items in a ListBox (Code)" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\ListBox\OrderedListBox\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    [ToolboxBitmap(typeof(ListBox), "Resources.Gaia.WebWidgets.ListBox.bmp")]
    public class ListBox : ASP.ListBox, IAspectableAjaxControl
    {
        #region [ -- Private Members -- ]

        private EffectControl _effectControl;
        private AspectableAjaxControl _instance;
        private AspectableAjaxControl _aspectableAjaxControl;

        #endregion

        #region [ -- Properties -- ]

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
        /// Gets or sets the number of rows displayed in the <see cref="T:System.Web.UI.WebControls.ListBox"/> control.
        /// </summary>
        /// <returns>
        /// The number of rows displayed in the <see cref="T:System.Web.UI.WebControls.ListBox"/> control. The default value is 4.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The specified number of rows is less than one or greater than 2000.</exception>
        [AjaxSerializable("setRows")]
        public override int Rows
        {
            get { return base.Rows;  }
            set { base.Rows = value; }
        }

        /// <summary>
        /// Gets or sets the selection mode of the <see cref="T:System.Web.UI.WebControls.ListBox"/> control.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Web.UI.WebControls.ListSelectionMode"/> values. The default value is Single.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// The specified selection mode is not one of the <see cref="T:System.Web.UI.WebControls.ListSelectionMode"/> values. 
        /// </exception>
        [AjaxSerializable("setMode")]
        public override ASP.ListSelectionMode SelectionMode
        {
            get { return base.SelectionMode;  }
            set { base.SelectionMode = value; }
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
        /// See <see cref="AjaxControl.ExecuteLoadPostData" /> method for documentation. This method only forwards to that method.
        /// </summary>
        protected override bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            return AspectableAjaxControl.ExecuteLoadPostData(base.LoadPostData, postDataKey, postCollection,
                                                             stateManager => ((PropertyStateManagerListControl) stateManager).ClearDirtySelection());
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
            // Include ListBox Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.ListControl.js", typeof(Manager), "Gaia.ListControl.browserFinishedLoading", true);
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.ListBox.js", typeof(Manager), "Gaia.ListBox.browserFinishedLoading", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(HtmlFormatting.XhtmlTagFactory create)
        {
            // Since the AutoPostBack if true renders this incredible stupid JavaScript in the "onclick" property,
            // we basically "hack" it out right here...!!
            var hadAutoPostBack = AutoPostBack;

            // xhtml strict does not support accesskey on the "select" element,
            // so we remove it before rendering
            var accessKey = AccessKey;

            using (new AtomicInvoker(
            delegate
            {
                AutoPostBack = false;
                AccessKey = string.Empty;
            },
            delegate
            {
                AccessKey = accessKey;
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
            return new PropertyStateManagerListControl(this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.ListBox", ClientID)
                .AddFocus(AspectableAjaxControl)
                .AddValidation(CausesValidation, ValidationGroup)
                .AddAspects(Aspects).AddEffects(Effects)
                .InvokeIf(AutoPostBack, "setAutoPostBack", false, 1).ToString();
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
    }
}
