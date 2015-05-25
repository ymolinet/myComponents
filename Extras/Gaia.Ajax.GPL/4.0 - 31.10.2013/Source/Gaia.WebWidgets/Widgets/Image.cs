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
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.Scripts.Image.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Gaia Ajax Image displays an image on a Web page. It inherits from the 
    /// <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.image.aspx">
    /// ASP.NET Image</a>, and has built-in Ajax behaviour.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for Image" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\Image\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Codebehind for Image" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\Image\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    [ToolboxBitmap(typeof(Image), "Resources.Gaia.WebWidgets.Image.bmp")]
    public class Image : ASP.Image, IAspectableAjaxControl
    {
        #region [ -- Private Members -- ]

        private AspectableAjaxControl _instance;
        private AspectableAjaxControl _aspectableAjaxControl;
        private EffectControl _effectControl;

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
        /// Gets or sets the image URL for the control
        /// </summary>
        [AjaxSerializableUrl("setImageUrl")]
        public override string ImageUrl
        {
            get { return base.ImageUrl; }
            set { base.ImageUrl = value; }
        }

        /// <summary>
        /// Sets the ALT text for the image
        /// </summary>
        [AjaxSerializable("setAlternateText")]
        public override string AlternateText
        {
            get { return base.AlternateText; }
            set { base.AlternateText = value; }
        }

        ///<summary>Gets or sets the location to a detailed description for the image.</summary>
        ///<value>
        ///The URL for the file that contains a detailed description for the image.
        ///The default is an empty string ("").
        ///</value>   
        [AjaxSerializable("setLongDesc")]
        public override string DescriptionUrl
        {
            get { return base.DescriptionUrl;  }
            set { base.DescriptionUrl = value; }
        }

        ///<summary>
        /// Gets or sets the alignment of the System.Web.UI.WebControls.Image control
        /// in relation to other elements on the Web page.
        ///</summary>
        ///<value>One of the System.Web.UI.WebControls.ImageAlign values. The default is NotSet.</value>
        ///<exception cref="System.ArgumentOutOfRangeException">
        ///The specified value is not one of the System.Web.UI.WebControls.ImageAlign values.
        ///</exception>
        [AjaxSerializable("setImgAlign")]
        public override System.Web.UI.WebControls.ImageAlign ImageAlign
        {
            get { return base.ImageAlign;  }
            set { base.ImageAlign = value; }
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


        #endregion

        #region [ -- Overridden base class methods and constructor -- ]

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
            // Include Image Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Image.js", typeof(Manager), "Gaia.Image.browserFinishedLoading", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(HtmlFormatting.XhtmlTagFactory create)
        {
            // ensure "alt" attribute is rendered
            bool hasAlternateText = !string.IsNullOrEmpty(AlternateText);

            // remove accesskey and tabindex for xhtml compliant rendering
            short tabIndex = TabIndex;
            string accessKey = AccessKey;

            using (new AtomicInvoker(
            delegate
            {
                TabIndex = 0;
                AccessKey = string.Empty;

                if (!hasAlternateText)
                    AlternateText = ID;
            },
            delegate
            {
                //restore properties
                TabIndex = tabIndex;
                AccessKey = accessKey;

                if (!hasAlternateText)
                    AlternateText = string.Empty;
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
            return new PropertyStateManagerWebControl(this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.Image", ClientID).AddAspects(Aspects).AddEffects(Effects).ToString();
        }

        void IAjaxControl.IncludeScriptFiles()
        {
            IncludeScriptFiles();
        }

        void IAjaxControl.RenderControlHtml(HtmlFormatting.XhtmlTagFactory create)
        {
            RenderControlHtml(create);
        }

        bool IAjaxControl.InDesigner
        {
            get { return DesignMode; }
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
