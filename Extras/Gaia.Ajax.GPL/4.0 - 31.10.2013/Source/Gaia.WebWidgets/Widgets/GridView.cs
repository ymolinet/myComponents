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
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;

[assembly:WebResource("Gaia.WebWidgets.Scripts.GridView.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    using HtmlFormatting;
    using Widgets.DataControls;
    
    /// <summary>
    /// The Gaia Ajax GridView displays the values of a data source
    /// in a table where each column represents a field and each 
    /// row represents a record. It inherits from the
    /// <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.gridview.aspx">
    /// ASP.NET GridView</a>, and has built-in Ajax behaviour.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for GridView" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\GridView\Overview\Default.aspx" />
    /// </code> 
    /// </example>
    [ToolboxBitmap(typeof(GridView), "Resources.Gaia.WebWidgets.GridView.bmp")]
    public class GridView : ASP.GridView, IAjaxContainerControl
    {
        #region [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="GridView"/>.
        /// </summary>
        public class PropertyStateManagerGridView : PropertyStateManagerWebControl
        {
            private ASP.GridView _state;
            private readonly GridView _gridView;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerWebControl"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerGridView(GridView control) : this(control, control.ClientID, null) { }

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
            public PropertyStateManagerGridView(GridView control, string clientId, IExtraPropertyCallbackRenderer extra) : base(control, clientId, extra)
            {
                _gridView = control;
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
            /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);
                
                var stateManager = source as PropertyStateManagerGridView;
                if (stateManager == null) return;
                
                _state = stateManager._state;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _state = new ASP.GridView();
                
                _state.RowStyle.CopyFrom(_gridView.RowStyle);
                _state.PagerStyle.CopyFrom(_gridView.PagerStyle);
                _state.HeaderStyle.CopyFrom(_gridView.HeaderStyle);
                _state.FooterStyle.CopyFrom(_gridView.FooterStyle);
                _state.EditRowStyle.CopyFrom(_gridView.EditRowStyle);
                _state.SelectedRowStyle.CopyFrom(_gridView.SelectedRowStyle);
                _state.EmptyDataRowStyle.CopyFrom(_gridView.EmptyDataRowStyle);
                _state.AlternatingRowStyle.CopyFrom(_gridView.AlternatingRowStyle);

                _state.Caption = _gridView.Caption;
                _state.GridLines = _gridView.GridLines;
                _state.ShowHeader = _gridView.ShowHeader;
                _state.ShowFooter = _gridView.ShowFooter;
                _state.CellPadding = _gridView.CellPadding;
                _state.CellSpacing = _gridView.CellSpacing;
                _state.CaptionAlign = _gridView.CaptionAlign;
                _state.BackImageUrl = _gridView.BackImageUrl;
                _state.SelectedIndex = _gridView.SelectedIndex;
                _state.HorizontalAlign = _gridView.HorizontalAlign;

                base.TakeSnapshot();
            }

            /// <summary>
            /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void DiffSnapshot()
            {
                RenderChange(_state.Caption, _gridView.Caption, "setCaption");
                RenderChange(_state.GridLines, _gridView.GridLines, "setRules");
                RenderChange(_state.CellPadding, _gridView.CellPadding, "setPadding");
                RenderChange(_state.CellSpacing, _gridView.CellSpacing, "setSpacing");
                RenderChange(_state.BackImageUrl, _gridView.BackImageUrl, "setBackImgUrl");
                RenderChange(_state.CaptionAlign, _gridView.CaptionAlign, "setCaptionAlign");
                RenderChange(_state.HorizontalAlign, _gridView.HorizontalAlign, "setHorAlign");

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
                return HandleGridViewRow(sender) || HandleGridViewCell(sender) || HandleGridViewCellControl(sender);
            }

            /// <summary>
            /// Tries handling the specified object as a <see cref="GridViewRow"/> and
            /// applying correct state information.
            /// </summary>
            /// <param name="sender">The object the state snapshot of which is being taken.</param>
            /// <returns>
            /// True if the provided sender is a <see cref="GridViewRow"/> and further event bubbling should be stopped; otherwise, false.
            /// </returns>
            private bool HandleGridViewRow(object sender)
            {
                var row = sender as GridViewRow;
                if (row != null)
                {
                    // try setting control style
                    var stateManager = ((IAjaxControl)row).StateManager as PropertyStateManagerWebControl;
                    if (stateManager != null)
                    {
                        stateManager.ControlStyle = GetRowStyle(row);
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Tries handling the specified object as a <see cref="DataControlFieldCell"/> and
            /// applying correct state information.
            /// </summary>
            /// <param name="sender">The object the state snapshot of which is being taken.</param>
            /// <returns>
            /// True if the provided sender is a <see cref="DataControlFieldCell"/> and further event bubbling should be stopped; otherwise, false.
            /// </returns>
            private static bool HandleGridViewCell(object sender)
            {
                // check cell
                var cell = sender as DataControlFieldCell;
                if (cell == null) return false;

                // check row type
                var row = cell.Parent as GridViewRow;
                if (row == null || row.RowType == ASP.DataControlRowType.Pager || row.RowType == ASP.DataControlRowType.EmptyDataRow) return false;

                // check containing field
                var containingField = cell.ContainingField;
                if (containingField == null || !containingField.Visible) return false;

                // check state manager
                var stateManager = ((IAjaxControl)cell).StateManager as TableCell.PropertyStateManagerTableCell;
                if (stateManager == null) return false;

                // apply styles
                ASP.Style style;
                switch (row.RowType)
                {
                    case ASP.DataControlRowType.Header:
                        style = containingField.HeaderStyle;
                        break;

                    case ASP.DataControlRowType.Footer:
                        style = containingField.FooterStyle;
                        break;

                    default:
                        style = containingField.ItemStyle;
                        break;
                }
                stateManager.ControlStyle = style;
                return true;
            }

            /// <summary>
            /// Tries handling the specified object as a <see cref="ASP.WebControl"/> and
            /// applying correct state information.
            /// </summary>
            /// <param name="sender">The object the state snapshot of which is being taken.</param>
            /// <returns>
            /// True if the provided sender is a <see cref="ASP.WebControl"/> and further event bubbling should be stopped; otherwise, false.
            /// </returns>
            private static bool HandleGridViewCellControl(object sender)
            {
                // check webcontrol
                var webControl = sender as ASP.WebControl;
                if (webControl == null) return false;

                // check cell control
                var cell = webControl.Parent as DataControlFieldCell;
                if (cell == null || !cell.Visible) return false;

                // check row and its type
                var row = cell.Parent as GridViewRow;
                if (row == null || row.RowType != ASP.DataControlRowType.DataRow) return false;

                var stateManager = ((IAjaxControl)webControl).StateManager as PropertyStateManagerWebControl;
                if (stateManager != null)
                {
                    stateManager.ControlStyle = cell.ContainingField.ControlStyle;
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Returns <see cref="ASP.Style"/> with which the <see cref="GridViewRow"/> was rendered.
            /// </summary>
            private ASP.Style GetRowStyle(ASP.GridViewRow gridViewRow)
            {
                var rowStyle = new ASP.TableItemStyle();
                rowStyle.CopyFrom(gridViewRow.ControlStyle);

                switch (gridViewRow.RowType)
                {
                    case ASP.DataControlRowType.Header:
                        if (_state.ShowHeader)
                            rowStyle.MergeWith(_state.HeaderStyle);
                        break;
                    case ASP.DataControlRowType.Footer:
                        if (_state.ShowFooter)
                            rowStyle.MergeWith(_state.FooterStyle);
                        break;
                    case ASP.DataControlRowType.Pager:
                        if (gridViewRow.Visible)
                            rowStyle.MergeWith(_state.PagerStyle);
                        break;
                    case ASP.DataControlRowType.EmptyDataRow:
                        rowStyle.MergeWith(_state.EmptyDataRowStyle);
                        break;
                    case ASP.DataControlRowType.DataRow:
                        var dataRowStyle = new ASP.TableItemStyle();

                        var alternatingRowState = _state.AlternatingRowStyle;
                        if (!alternatingRowState.IsEmpty && (CheckState(gridViewRow.RowState, ASP.DataControlRowState.Alternate) || (gridViewRow.RowIndex % 2 != 0)))
                            dataRowStyle.CopyFrom(alternatingRowState);
                        else
                            dataRowStyle.CopyFrom(_state.RowStyle);

                        if (CheckState(gridViewRow.RowState, ASP.DataControlRowState.Selected) || (gridViewRow.RowIndex == _state.SelectedIndex))
                            dataRowStyle.CopyFrom(_state.SelectedRowStyle);

                        if (CheckState(gridViewRow.RowState, ASP.DataControlRowState.Edit))
                            dataRowStyle.CopyFrom(_state.EditRowStyle);

                        rowStyle.MergeWith(dataRowStyle);
                        break;
                }

                return rowStyle;
            }

            /// <summary>
            /// Checks if the specified state satisfies expected state.
            /// </summary>
            /// <param name="state">State to check.</param>
            /// <param name="expectedState">Expected state.</param>
            /// <returns>True if state satisfies expected state; otherwise, false.</returns>
            private static bool CheckState(ASP.DataControlRowState state, ASP.DataControlRowState expectedState)
            {
                return (state & expectedState) != ASP.DataControlRowState.Normal;
            }
        }

        #endregion

        #region [ -- Child Table -- ]

        /// <summary>
        /// Specialized child <see cref="ASP.Table"/> for the <see cref="GridView"/>.
        /// </summary>
        private sealed class GridViewChildTable : ASP.Table
        {
            private readonly GridView _gridView;

            /// <summary>
            /// Initializes new instance of <see cref="GridViewChildTable"/>.
            /// </summary>
            /// <param name="owner">Owner control</param>
            public GridViewChildTable(GridView owner)
            {
                if (owner == null)
                    throw new ArgumentNullException("owner");

                _gridView = owner;
            }

            /// <summary>
            /// Adds HTML attributes and styles that need to be rendered to the specified <see cref="T:System.Web.UI.HtmlTextWriter"/>.
            /// </summary>
            /// <param name="writer">The output stream that renders HTML content to the client. </param>
            protected override void AddAttributesToRender(HtmlTextWriter writer)
            {
                base.AddAttributesToRender(writer);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, _gridView.ClientID);
            }
        }

        #endregion

        #region [ -- Private Members -- ]

        private EffectControl _effectControl;
        private AjaxContainerControl _instance;
        private AjaxContainerControl _ajaxContainerControl;

        #endregion

        #region [ -- Overridden base class methods and properties -- ]

        /// <summary>
        /// Gets or sets a value indicating whether client-side callbacks are used for sorting and paging operations.
        /// </summary>
        public override bool EnableSortingAndPagingCallbacks
        {
            set { /* should be always false */ }
        }

        /// <summary>
        /// Collection of Effects for the Control. 
        /// </summary>
        [Browsable(false)]
        public EffectCollection Effects { get { return EffectControl.Effects; } }

        /// <summary>Creates a new child table.</summary>
        /// <returns>Always returns a new <see cref="ASP.Table"/> that represents the child table.</returns>
        protected override ASP.Table CreateChildTable()
        {
            return new GridViewChildTable(this);
        }

        /// <summary>
        /// Creates the control hierarchy used to render the <see cref="T:Gaia.WebWidgets.GridView"/> control using the specified data source.
        /// </summary>
        /// <returns>The number of rows created.</returns>
        /// <param name="dataSource">An <see cref="T:System.Collections.IEnumerable"/> that contains the data source for the <see cref="T:Gaia.WebWidgets.GridView"/> control.</param>
        /// <param name="dataBinding">true to indicate that the child controls are bound to data; otherwise, false.</param>
        /// <exception cref="T:System.Web.HttpException">
        /// <paramref name="dataSource"/> returns a null <see cref="T:System.Web.UI.DataSourceView"/>.
        /// -or-
        /// <paramref name="dataSource"/> does not implement the <see cref="T:System.Collections.ICollection"/> interface and cannot return a <see cref="P:System.Web.UI.DataSourceSelectArguments.TotalRowCount"/>. 
        /// -or-
        /// <see cref="P:System.Web.UI.WebControls.GridView.AllowPaging"/> is true and <paramref name="dataSource"/> does not implement the <see cref="T:System.Collections.ICollection"/> interface and cannot perform data source paging.
        /// -or-
        /// <paramref name="dataSource"/> does not implement the <see cref="T:System.Collections.ICollection"/> interface and <paramref name="dataBinding"/> is set to false.
        /// </exception>
        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            int rowCount = base.CreateChildControls(dataSource, dataBinding);
            EnsureChildTable();
            return rowCount;
        }

        /// <summary>
        /// Creates the control hierarchy that is used to render a composite data-bound control based on the values that are stored in view state.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnsureChildTable();
        }

        /// <summary>
        /// Creates a row in the GridView control.
        /// </summary>
        /// <param name="rowIndex">The index of the row to create.</param>
        /// <param name="dataSourceIndex">The index of the data source item to bind to the row.</param>
        /// <param name="rowType">One of the DataControlRowType values.</param>
        /// <param name="rowState">One of the DataControlRowState values.</param>
        /// <returns>A ICollection that contains the fields used to build the control hierarchy.</returns>
        protected override ASP.GridViewRow CreateRow(int rowIndex, int dataSourceIndex, ASP.DataControlRowType rowType, ASP.DataControlRowState rowState) 
        {
            return new GridViewRow(this, rowIndex, dataSourceIndex, rowType, rowState);
        }

        /// <summary>
        /// Creates the set of column fields used to build the control hierarchy.
        /// </summary>
        /// <param name="dataSource">A PagedDataSource that represents the data source.</param>
        /// <param name="useDataSource">True to use the data source specified by the dataSource parameter; Otherwise, false.</param>
        /// <returns></returns>
        protected override ICollection CreateColumns(ASP.PagedDataSource dataSource, bool useDataSource) 
        {
            var list = base.CreateColumns(dataSource, useDataSource);
            var copies = new ArrayList(list.Count);

            var autoGenerateCommandField = AutoGenerateEditButton || AutoGenerateSelectButton ||
                                           AutoGenerateDeleteButton;

            foreach (ASP.DataControlField field in list) 
            {
                if (autoGenerateCommandField)
                {
                    var commandField = field as ASP.CommandField;
                    if (commandField != null && !(field is CommandField)) 
                    {
                        var copy = new CommandField
                                       {
                                           ButtonType = ASP.ButtonType.Link,
                                           ShowEditButton = commandField.ShowEditButton,
                                           ShowDeleteButton = commandField.ShowDeleteButton,
                                           ShowSelectButton = commandField.ShowSelectButton
                                       };
                        copies.Add(copy);
                        continue;
                    }

                    autoGenerateCommandField = false;
                }
                
                var autoGeneratedField = field as ASP.AutoGeneratedField;
                // note: we don't need to check for gaia AutoGeneratedField here, because it does not inherit from ASP.AutoGeneratedField
                if (autoGeneratedField != null) 
                {
                    var copy = new AutoGeneratedField();
                    ((IStateManager)copy).TrackViewState();
                    string text = autoGeneratedField.HeaderText;
                    copy.HeaderText = text;
                    copy.SortExpression = text;
                    copy.ReadOnly = autoGeneratedField.ReadOnly;
                    copy.DataType = autoGeneratedField.DataType;
                    copy.DataField = autoGeneratedField.DataField;
                    copies.Add(copy);
                    continue;
                }

                copies.Add(field);
            }
            
            return copies;
        }

        /// <summary>
        /// Initializes a row in the GridView control.
        /// </summary>
        /// <param name="row">A GridViewRow that represents the row to initialize.</param>
        /// <param name="fields">An array of DataControlField objects that represent the column fields in the GridView control.</param>
        protected override void InitializeRow(ASP.GridViewRow row, ASP.DataControlField[] fields)
        {
            ASP.DataControlRowType rowType = row.RowType;

            if (rowType == ASP.DataControlRowType.EmptyDataRow)
            {
                InitializeTemplateRow(row, fields.Length);
                return;
            }

            int rowIndex = row.RowIndex;
            string rowHeaderColumn = RowHeaderColumn;
            ASP.TableCellCollection cells = row.Cells;
            ASP.DataControlRowState rowState = row.RowState;

            for (int i = 0; i < fields.Length; ++i) 
            {
                ASP.DataControlFieldCell cell;
                ASP.DataControlCellType header;

                if ((rowType == ASP.DataControlRowType.Header) && UseAccessibleHeader)
                {
                    var headerCell = new DataControlFieldHeaderCell(fields[i])
                                         {
                                             Scope = ASP.TableHeaderScope.Column,
                                             AbbreviatedText = fields[i].AccessibleHeaderText
                                         };
                    cell = headerCell;
                }
                else
                {
                    var field = fields[i] as BoundField;
                    if ((rowHeaderColumn.Length > 0) && (field != null) && (field.DataField == rowHeaderColumn))
                    {
                        var headerCell = new DataControlFieldHeaderCell(fields[i]) {Scope = ASP.TableHeaderScope.Row};
                        cell = headerCell;
                    } 
                    else
                        cell = new DataControlFieldCell(fields[i]);
                }

                switch (rowType)
                {
                    case ASP.DataControlRowType.Header:
                        header = ASP.DataControlCellType.Header;
                        break;

                    case ASP.DataControlRowType.Footer:
                        header = ASP.DataControlCellType.Footer;
                        break;

                    default:
                        header = ASP.DataControlCellType.DataCell;
                        break;
                }

                fields[i].InitializeCell(cell, header, rowState, rowIndex);
                cells.Add(cell);
            }
        }

        /// <summary>
        /// Initializes the pager row displayed when the paging feature is enabled.
        /// </summary>
        /// <param name="row">A GridViewRow that represents the pager row to initialize.</param>
        /// <param name="columnSpan">The number of columns the pager row should span.</param>
        /// <param name="pagedDataSource">A PagedDataSource that represents the data source.</param>
        protected override void InitializePager(ASP.GridViewRow row, int columnSpan, ASP.PagedDataSource pagedDataSource) 
        {
            if (PagerTemplate != null)
            {
                InitializeTemplateRow(row, columnSpan);
                return;
            }

            var cell = new TableCell();
            if (columnSpan > 1) cell.ColumnSpan = columnSpan;

            var table = new ASP.Table();
            var tableRow = new ASP.TableRow();
            switch (PagerSettings.Mode) 
            {
                case ASP.PagerButtons.NextPrevious:
                    CreateNextPrevPager(tableRow, pagedDataSource, false);
                    break;
                case ASP.PagerButtons.NextPreviousFirstLast:
                    CreateNextPrevPager(tableRow, pagedDataSource, true);
                    break;
                case ASP.PagerButtons.Numeric:
                    CreateNumericPager(tableRow, pagedDataSource, false);
                    break;
                case ASP.PagerButtons.NumericFirstLast:
                    CreateNumericPager(tableRow, pagedDataSource, true);
                    break;
            }

            table.Rows.Add(tableRow);
            cell.Controls.Add(table);
            row.Cells.Add(cell);
        }

        /// <summary>
        /// Initializes Template Row in the %GridView.
        /// </summary>
        /// <param name="row">GridViewRow to initialize the template in.</param>
        /// <param name="columnSpan">ColumnSpan of the Template Row.</param>
        protected virtual void InitializeTemplateRow(ASP.GridViewRow row, int columnSpan) 
        {
            ITemplate template = null;
            TableCell container = null;

            switch (row.RowType)
            {
                case ASP.DataControlRowType.Pager:
                    ITemplate pagerTemplate = PagerTemplate;
                    if (pagerTemplate != null)
                    {
                        template = pagerTemplate;
                        container = new TableCell();
                    }
                    break;

                case ASP.DataControlRowType.EmptyDataRow:
                    container = new TableCell();
                    ITemplate emptyDataTemplate = EmptyDataTemplate;
                    if (emptyDataTemplate == null)
                    {
                        string emptyDataText = EmptyDataText;
                        if (!string.IsNullOrEmpty(emptyDataText))
                            container.Text = emptyDataText;
                    }
                    else
                        template = emptyDataTemplate;
                    break;
            }

            if (container == null) return;
            if (columnSpan > 1)
                container.ColumnSpan = columnSpan;
            if (template != null)
                template.InstantiateIn(container);
            row.Cells.Add(container);
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
            if (!DesignMode && Manager.Instance.IsAjaxCallback) PrepareControlHierarchy();
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
            return new RegisterControl("Gaia.GridView", ClientID).AddEffects(Effects).ToString();
        }

        void IAjaxControl.IncludeScriptFiles()
        {
            IncludeScriptFiles();
        }

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerGridView(this);
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

        private AjaxContainerControl AjaxContainerControl
        {
            get { return _ajaxContainerControl ?? (_ajaxContainerControl = ((IAjaxContainerControl)this).AjaxContainerControl); }
        }

        #endregion

        private void CreateNextPrevPager(ASP.TableRow row, ASP.PagedDataSource pagedDataSource, bool addFirstLast)
        {
            var pagerSettings = PagerSettings;
            var isFirstPage = pagedDataSource.IsFirstPage;
            var isLastPage = pagedDataSource.IsLastPage;
            
            if (addFirstLast && !isFirstPage) 
                CreateButton(row, pagerSettings.FirstPageImageUrl, pagerSettings.FirstPageText, "First");
            
            if (!isFirstPage)
                CreateButton(row, pagerSettings.PreviousPageImageUrl, pagerSettings.PreviousPageText, "Prev");
            
            if (!isLastPage)
                CreateButton(row, pagerSettings.NextPageImageUrl, pagerSettings.NextPageText, "Next");
            
            if (!addFirstLast || isLastPage) return;
            CreateButton(row, pagerSettings.LastPageImageUrl, pagerSettings.LastPageText, "Last");
        }

        private void CreateNumericPager(ASP.TableRow row, ASP.PagedDataSource pagedDataSource, bool addFirstLast)
        {
            var pagerSettings = PagerSettings;
            var pageCount = pagedDataSource.PageCount;
            var pageButtonCount = pagerSettings.PageButtonCount;
            var currentPageIndex = pagedDataSource.CurrentPageIndex + 1;
            var groupSize = Math.Min(pageCount,pageButtonCount);
            var group = (currentPageIndex - 1) / groupSize;
            var start = 1;
            var end = pageCount;

            if (pageButtonCount < pageCount)
            {
                start = 1 + group * groupSize;
                end = start + groupSize - 1;
                if (end > pageCount) 
                {
                    int delta = end - pageCount;
                    end -= delta;
                    start -= delta;
                }

                if (LastGroupRendered)
                {
                    int lastGroupEnd = pageCount;
                    int lastGroupStart = pageCount - groupSize + 1;
                    
                    if (currentPageIndex >= lastGroupStart && currentPageIndex <= lastGroupEnd)
                    {
                        group = pageCount/pageButtonCount;
                        end = lastGroupEnd;
                        start = lastGroupStart;
                    }
                }
            }

            if (group != 0)
            {
                if (addFirstLast)
                    CreateButton(row, pagerSettings.FirstPageImageUrl, pagerSettings.FirstPageText, "First");

                var previous = start - 1;
                CreateButton(row, string.Empty, "...", previous.ToString(NumberFormatInfo.InvariantInfo));
            }

            for (int index = start; index <= end; ++index) 
            {
                string arg = index.ToString(NumberFormatInfo.InvariantInfo);

                if (currentPageIndex == index) 
                {
                    var label = new Label {Text = arg};
                    var cell = new TableCell();
                    cell.Controls.Add(label);
                    row.Cells.Add(cell);
                } 
                else
                    CreateButton(row, string.Empty, arg, arg);
            }

            LastGroupRendered = end >= pageCount;
            if (LastGroupRendered) return;
            var next = end + 1;
            CreateButton(row, string.Empty, "...", next.ToString(NumberFormatInfo.InvariantInfo));
            if (!addFirstLast) return;
            CreateButton(row, pagerSettings.LastPageImageUrl, pagerSettings.LastPageText, "Last");
        }

        private bool LastGroupRendered 
        {
            get { return ViewState["LastGroupRendered"] != null; }
            set 
            {
                if (value)
                    ViewState["LastGroupRendered"] = 0;
                else
                    ViewState.Remove("LastGroupRendered");
            }
        }

        private void CreateButton(ASP.TableRow row, string imageUrl, string text, string argument)
        {
            var control = imageUrl.Length > 0
                              ? DataControlFactory.ImageButton(text, imageUrl, "Page", argument, false, this)
                              : (Control) DataControlFactory.PagerLinkButton(text, "Page", argument, false, this);

            var cell = new TableCell();
            cell.Controls.Add(control);
            row.Cells.Add(cell);
        }

        private EffectControl EffectControl
        {
            get { return _effectControl ?? (_effectControl = new EffectControl(this)); }
        }

        private void EnsureChildTable()
        {
            if (HasControls()) return;
            Controls.Add(CreateChildTable());
        }
    }
}
