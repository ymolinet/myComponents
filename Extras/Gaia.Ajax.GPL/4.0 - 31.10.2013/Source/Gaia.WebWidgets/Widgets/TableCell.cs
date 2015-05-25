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
using System.Collections.Generic;
using Gaia.WebWidgets.HtmlFormatting;
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.Scripts.TableCell.js", "text/javascript")]

namespace Gaia.WebWidgets 
{
    /// <summary>
    /// Represents ajaxified cell in the <see cref="ASP.Table"/>.
    /// </summary>
    public class TableCell : ASP.TableCell, IAjaxContainerControl, INamingContainer
    {
        #region [ -- Ajax Control Implementation -- ]
        
        /// <summary>
        /// Specialized implementation of <see cref="AjaxContainerControl"/> for <see cref="TableCell"/>.
        /// </summary>
        public class TableCellAjaxControl : AjaxContainerControl
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="control">Control which owns this instance.</param>
            public TableCellAjaxControl(IAjaxContainerControl control) : base(control) { }

            /// <summary>
            /// Used by derived controls to know if the control should be rendered or not
            /// based on the facts if the ShouldRender was set or control was previously rendered
            /// </summary>
            protected internal override bool ShouldRenderAjaxControl
            {
                get
                {
                    var shouldRender = base.ShouldRenderAjaxControl;

                    if (!shouldRender)
                    {
                        var stateManager = StateManager as PropertyStateManagerTableCell;
                        shouldRender = stateManager != null && stateManager.RequiresRerendering;
                    }

                    return shouldRender;
                }
            }
        }

        #endregion

        #region [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="TableCell"/> control.
        /// </summary>
        public class PropertyStateManagerTableCell : PropertyStateManagerWebControl
        {
            private bool _hadControls;
            private ASP.TableCell _state;
            private readonly ASP.TableCell _cell;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerWebControl"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerTableCell(TableCell control) : this(control, control.ClientID, null) { }

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerWebControl"/> for specified <paramref name="control"/>
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
            public PropertyStateManagerTableCell(TableCell control, string clientId, IExtraPropertyCallbackRenderer extra) : this((ASP.TableCell)control, clientId, extra) { }

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerWebControl"/> for specified <paramref name="control"/>
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
            internal PropertyStateManagerTableCell(ASP.TableCell control, string clientId, IExtraPropertyCallbackRenderer extra)
				: base(control, clientId, extra)
            {
                _cell = control;
            }

            /// <summary>
            /// Gets or sets <see cref="ASP.WebControl.ControlStyle"/> property in the state snapshot.
            /// </summary>
            internal protected override ASP.Style ControlStyle
            {
                get { return base.ControlStyle; }
                set
                {
                    _state.ControlStyle.CopyFrom(value);
                    base.ControlStyle = value;
                }
            }

            /// <summary>
            /// Returns true if the associated <see cref="TableCell"/> requires rerendering.
            /// </summary>
            internal bool RequiresRerendering
            {
                get
                {
                    if (_hadControls)
                        return !_cell.HasControls();

                    return _cell.HasControls();
                }
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);

                var stateManager = source as PropertyStateManagerTableCell;
                if (stateManager == null) return;

                _state = stateManager._state;
                _hadControls = stateManager._hadControls;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _state = new ASP.TableCell
                {
                    Text = _cell.Text,
                    Wrap = _cell.Wrap,
                    RowSpan = _cell.RowSpan,
                    ColumnSpan = _cell.ColumnSpan,
                    VerticalAlign = _cell.VerticalAlign,
                    HorizontalAlign = _cell.HorizontalAlign,
                    AssociatedHeaderCellID = _cell.AssociatedHeaderCellID
                };

                base.TakeSnapshot();
            }

            /// <summary>
            /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void DiffSnapshot()
            {
                RenderChange(_state.Wrap, _cell.Wrap, "setWrap");
                RenderChange(_state.RowSpan, _cell.RowSpan, "setRowSpan");
                RenderChange(_state.ColumnSpan, _cell.ColumnSpan, "setColSpan");
                RenderChange(_state.VerticalAlign, _cell.VerticalAlign, "setVerAlign");
                RenderChange(_state.HorizontalAlign, _cell.HorizontalAlign, "setHorAlign");

                if (!_hadControls && !_cell.HasControls())
                    RenderChange(_state.Text, _cell.Text, "setText", true);

                var initialHeaders = _state.AssociatedHeaderCellID;
                var currentHeaders = _cell.AssociatedHeaderCellID;
                var headersModified = (initialHeaders.Length != 0 || currentHeaders.Length != 0) &&
                                       ((initialHeaders.Length == 0 || currentHeaders.Length == 0) ||
                                        Array.Exists(currentHeaders, id => !Array.Exists(initialHeaders, iid => id == iid)));
                if (headersModified)
                    RenderHeaderChanges(currentHeaders);

                base.DiffSnapshot();
            }

            /// <summary>
            /// Raised after the state snapshot for the associated <see cref="Control"/> is taken.
            /// </summary>
            /// <returns>
            /// True if event was consumed and the bubbling is cancelled. Otherwise, false.
            /// Default value is false.
            /// </returns>
            /// <remarks>
            /// Bubbles up until stopped by return value of true.
            /// </remarks>
            protected override bool OnSnapshotTakenEvent(object sender, EventArgs eventArgs)
            {
                _hadControls = _hadControls || sender is IAjaxControl;
                return false;
            }

            private void RenderHeaderChanges(ICollection<string> currentHeaders)
            {
                var namingContainer = _cell.NamingContainer;
                var headers = new List<string>(currentHeaders.Count);
                
                foreach (var id in currentHeaders)
                {
                    var cell = namingContainer.FindControl(id) as ASP.TableHeaderCell;
                    if (cell == null)
                        throw new InvalidOperationException("TableCell AssociatedHeaderCell NotFound");

                    headers.Add(cell.ClientID);
                }

                Builder.Append(".setHeaders([").Append(string.Join(",", headers.ToArray())).Append("])");
            }
        }

        #endregion

        private TableCellAjaxControl _instance;
        private AjaxContainerControl _ajaxContainerControl;

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
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.TableCell.js", typeof(TableCell), "Gaia.TableCell.loaded");
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create"><see cref="XhtmlTagFactory"/> to use for creating Xhtml compliant markup</param>
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
            return new PropertyStateManagerTableCell(this);
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
            get { return _instance ?? (_instance = new TableCellAjaxControl(this)); }
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
    }
}
