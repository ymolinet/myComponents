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
using System.Drawing;
using System.IO;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.Scripts.RadioButton.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    using HtmlFormatting;
    
    /// <summary>
    /// The Gaia Ajax RadioButton represents a single radio button control. It inherits from the <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.radiobutton.aspx">ASP.NET RadioButton</a>, 
    /// and has built-in Ajax behaviour.
    /// </summary>
    [ToolboxBitmap(typeof(RadioButton), "Resources.Gaia.WebWidgets.RadioButton.bmp")]
    public class RadioButton : ASP.RadioButton, IAspectableAjaxControl
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
        /// Gets or sets the Text of the control. The default value is ""
        /// </summary>
        [AjaxSerializable("setText")]
        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// Gets or sets the AutoPostBack property. If this property is true the control will generate an
        /// Ajax Request when it's value changes.
        /// </summary>
        [AjaxSerializable("setAutoPostBack")]
        public override bool AutoPostBack
        {
            get { return base.AutoPostBack; }
            set { base.AutoPostBack = value; }
        }

        /// <summary>
        /// Gets or sets if the RadioButton is checked
        /// </summary>
        [AjaxSerializable("setChecked")]
        public override bool Checked
        {
            get { return base.Checked; }
            set { base.Checked = value; }
        }

        /// <summary>
        /// Gets or sets the text alignment of the text of the CheckBox (right or left)
        /// </summary>
        [AjaxSerializable("setTextAlign")]
        public override ASP.TextAlign TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }

        /// <summary>
        /// Gets or sets the name of the group that the radio button belongs to.
        /// </summary>
        [AjaxSerializable("setGroupName")]
        public override string GroupName
        {
            get { return base.GroupName; }
            set { base.GroupName = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether validation is performed when the <see cref="RadioButton" /> control is clicked.
        /// </summary>
        [AjaxSerializable(Constants.SetCausesValidationFunctionName)]
        public override bool CausesValidation 
        {
            get { return base.CausesValidation; }
            set { base.CausesValidation = value; }
        }

        /// <summary>
        /// Gets or sets the group of controls for which the <see cref="RadioButton" /> control causes validation when it posts back to the server.
        /// </summary>
        [AjaxSerializable(Constants.SetValidationGroupFunctionName)]
        public override string ValidationGroup 
        {
            get { return base.ValidationGroup; }
            set { base.ValidationGroup = value; }
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
                                                             stateManager => stateManager.ClearDirty("Checked"));
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
            // Include RadioButton Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.CheckBox.js", typeof(Manager), "Gaia.CheckBox.browserFinishedLoading", true);
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.RadioButton.js", typeof(Manager), "Gaia.RadioButton.browserFinishedLoading", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(XhtmlTagFactory create)
        {
            if (!DesignMode)
            {
                Attributes["id"] = ClientID;
                Style[HtmlTextWriterStyle.Display] = "inline-block";
            }

            // generate internal markup
            string markup = ComposeXhtml.ToString(
                delegate(TextWriter textWriter)
                {
                    using (HtmlTextWriter htmlTextWriter = new CheckBoxHtmlWriter(textWriter))
                    {
                        bool autoPostBack = AutoPostBack;
                        AutoPostBack = false;
                        BaseRenderControl(htmlTextWriter);
                        AutoPostBack = autoPostBack;
                    }
                });
            create.GetHtmlTextWriter().Write(markup);
        }

        #endregion

        #region [ -- Helpers -- ]

        private void BaseRenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
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
            return new PropertyStateManagerWebControl(this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.RadioButton", ClientID)
                .AddFocus(AspectableAjaxControl)
                .AddValidation(CausesValidation, ValidationGroup)
                .AddPropertyIfTrue(TextAlign == ASP.TextAlign.Left, "textalign", TextAlign.ToString())
                .AddAspects(Aspects).AddEffects(Effects)
                .InvokeIf(AutoPostBack, "setAutoPostBack", false, "true").ToString();
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
            get 
            {
                return _aspectableAjaxControl ??
                       (_aspectableAjaxControl = ((IAspectableAjaxControl) this).AspectableAjaxControl);
            }
        }

        #endregion
    }
}
