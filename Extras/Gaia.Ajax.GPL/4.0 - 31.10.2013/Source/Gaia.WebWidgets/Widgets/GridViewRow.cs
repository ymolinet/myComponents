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
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets 
{
    using HtmlFormatting;

    /// <summary>
    /// Represents an individual row in a <see cref="GridView"/> control.
    /// </summary>
    public class GridViewRow : ASP.GridViewRow, IAjaxContainerControl
    {
        private readonly GridView _owner;
        private EffectControl _effectControl;
        private AjaxContainerControl _instance;
        private AjaxContainerControl _ajaxContainerControl;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridViewRow(GridView owner, int rowIndex, int dataItemIndex, ASP.DataControlRowType rowType, ASP.DataControlRowState rowState) : base(rowIndex, dataItemIndex, rowType, rowState)
        {
            _owner = owner;
        }

        /// <summary>
        /// Returns collection of <see cref="IAspect"/> for the <see cref="GridViewRow"/>.
        /// </summary>
        /// <see cref="IAspectableAjaxControl.Aspects"/>
        [Browsable(false)]
        public AspectCollection Aspects
        {
            get { return AjaxContainerControl.Aspects; }
        }

        /// <summary>
        /// Collection of Effects for the Control. 
        /// </summary>
        [Browsable(false)]
        public EffectCollection Effects { get { return EffectControl.Effects; } }

        private EffectControl EffectControl
        {
            get { return _effectControl ?? (_effectControl = new EffectControl(this)); }
        }

        #region [ -- Overridden base class methods and properties -- ]

        /// <summary>
        /// Gets or sets a value that indicates whether a server control is rendered as UI on the page.
        /// </summary>
        /// <returns>
        /// true if the control is visible on the page; otherwise false.
        /// </returns>
        public override bool Visible
        {
            get
            {
                if (_owner != null)
                {
                    var rowType = RowType;

                    // visibility of header and footer rows depends on
                    // ShowHeader and ShowFooter properties of the owner GridView
                    if (rowType == ASP.DataControlRowType.Header && !_owner.ShowHeader)
                        return false;

                    if (rowType == ASP.DataControlRowType.Footer && !_owner.ShowFooter)
                        return false;
                }

                return base.Visible;
            }
            set { base.Visible = value; }
        }

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
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.GridView.js", typeof(GridView), "Gaia.GridView.loaded");
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

        bool IAjaxControl.InDesigner
        {
            get { return DesignMode; }
        }

        Control IAjaxControl.Control
        {
            get { return this; }
        }

        string IAjaxControl.TagName
        {
            get { return TagName; }
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.GridViewRow", ClientID).AddAspects(AjaxContainerControl.Aspects).AddEffects(Effects).ToString();
        }

        void IAjaxControl.IncludeScriptFiles()
        {
            IncludeScriptFiles();
        }

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerWebControl(this);
        }

        void IAjaxControl.RenderControlHtml(XhtmlTagFactory create)
        {
            RenderControlHtml(create);
        }

        PropertyStateManagerControl IAjaxControl.StateManager
        {
            get { return AjaxContainerControl.StateManager; }
        }

        AjaxControl IAjaxControl.AjaxControl 
        {
            get { return _instance ?? (_instance = new AjaxContainerControl(this)); }
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

        void IAjaxContainerControl.ForceAnUpdate()
        {
            AjaxContainerControl.ForceAnUpdate();
        }

        void IAjaxContainerControl.ForceAnUpdateWithAppending()
        {
            AjaxContainerControl.ForceAnUpdateWithAppending();
        }

        void IAjaxContainerControl.TrackControlAdditions()
        {
            AjaxContainerControl.TrackControlAddition();
        }

        void IAjaxContainerControl.RenderChildrenOnForceAnUpdate(XhtmlTagFactory create)
        {
            RenderChildren(create.GetHtmlTextWriter());
        }

        AjaxContainerControl IAjaxContainerControl.AjaxContainerControl
        {
            get { return (AjaxContainerControl)((IAjaxContainerControl)this).AjaxControl; }
        }

        #endregion

        private AjaxContainerControl AjaxContainerControl
        {
            get { return _ajaxContainerControl ?? (_ajaxContainerControl = ((IAjaxContainerControl)this).AjaxContainerControl); }
        }
    }
}
