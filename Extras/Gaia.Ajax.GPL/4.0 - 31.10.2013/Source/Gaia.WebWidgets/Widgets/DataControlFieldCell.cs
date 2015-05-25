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
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets 
{
    using HtmlFormatting;

    /// <summary>
    /// Represents a cell in the rendered table of a tabular ASP.NET data-bound control, such as <see cref="GridView"/>.
    /// </summary>
    class DataControlFieldCell : ASP.DataControlFieldCell, IAjaxContainerControl, IDataItemContainer
    {
        #region [ -- Private fields -- ]

        private TableCell.TableCellAjaxControl _instance;
        private AjaxContainerControl _ajaxContainerControl;
        private IDataItemContainer _parentDataItemContainer;

        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Gaia.WebWidgets.DataControlFieldCell"/> class, setting the specified <see cref="T:System.Web.UI.WebControls.DataControlField"/> object as the cell's container.
        /// </summary>
        /// <param name="containingField">The <see cref="T:System.Web.UI.WebControls.DataControlField"/> that contains the current cell.</param>
        public DataControlFieldCell(ASP.DataControlField containingField) : base(containingField)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Gaia.WebWidgets.DataControlFieldCell"/> class, setting the specified <see cref="T:System.Web.UI.WebControls.DataControlField"/> object as the cell's container.
        /// </summary>
        /// <param name="tagKey">
        /// An <see cref="T:System.Web.UI.HtmlTextWriterTag"/> that specifies the HTML tag to render for the cell. The default tag used by the <see cref="T:System.Web.UI.WebControls.TableCell"/> class is <see cref="F:System.Web.UI.HtmlTextWriterTag.Td"/>.
        /// </param>
        /// <param name="containingField">The <see cref="T:System.Web.UI.WebControls.DataControlField"/> that contains the current cell.</param>
        protected DataControlFieldCell(HtmlTextWriterTag tagKey, ASP.DataControlField containingField) : base(tagKey, containingField)
        {
        }

        #endregion

        #region [ -- Overridden base class methods and properties -- ]

        /// <summary>
        /// Gets or sets a value that indicates whether a server control is rendered as UI on the page.
        /// </summary>
        /// <returns>
        /// True if the control is visible on the page; otherwise false.
        /// </returns>
        public override bool Visible
        {
            get
            {
                // Visibility of DataControlFieldCell depends on the visibility of the containing field.
                // Check if there is a containing field registered.
                var containingField = ContainingField;
                return (containingField == null || containingField.Visible) && base.Visible;
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
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.TableCell.js", typeof(GridView), "Gaia.TableCell.loaded");
        }

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
            return new RegisterControl("Gaia.TableCell", ClientID).ToString();
        }

        void IAjaxControl.IncludeScriptFiles()
        {
            IncludeScriptFiles();
        }

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new TableCell.PropertyStateManagerTableCell(this, ClientID, null);
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
            get { return _instance ?? (_instance = new TableCell.TableCellAjaxControl(this)); }
        }

        #endregion

        #region [ -- IAspectableAjaxControl Implementation -- ]

        AspectCollection IAspectableAjaxControl.Aspects
        {
            get { return AjaxContainerControl.Aspects; }
        }

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

        #region [ -- IDataItemContainer Implementation -- ]

        /// <summary>
        /// Delegating <see cref="IDataItemContainer"/> interface members to Parent.
        /// Assuming Parent implements <see cref="IDataItemContainer"/> interface.
        /// </summary>
        object IDataItemContainer.DataItem
        {
            get { return ParentDataItemContainer.DataItem; }
        }

        /// <summary>
        /// Delegating <see cref="IDataItemContainer"/> interface members to Parent.
        /// Assuming Parent implements <see cref="IDataItemContainer"/> interface.
        /// </summary>
        int IDataItemContainer.DataItemIndex
        {
            get { return ParentDataItemContainer.DataItemIndex; }
        }

        /// <summary>
        /// Delegating <see cref="IDataItemContainer"/> interface members to Parent.
        /// Assuming Parent implements <see cref="IDataItemContainer"/> interface.
        /// </summary>
        int IDataItemContainer.DisplayIndex
        {
            get { return ParentDataItemContainer.DisplayIndex; }
        }

        private IDataItemContainer ParentDataItemContainer
        {
            get { return _parentDataItemContainer ?? (_parentDataItemContainer = Parent as IDataItemContainer); }
        }

        #endregion
    }
}
