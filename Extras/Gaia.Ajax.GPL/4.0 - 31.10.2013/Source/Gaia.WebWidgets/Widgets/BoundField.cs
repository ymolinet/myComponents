/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Web.UI;
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets 
{
    /// <summary>
    /// Represents a field that is displayed as text in a data-bound control.
    /// </summary>
    [Designer("Gaia.WebWidgets.Design.BoundFieldDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaWebControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class BoundField : ASP.BoundField, IAjaxBoundField
    {
        private readonly BoundFieldImpl<BoundField> _impl;

        /// <summary>
        /// Gets or sets the text that is displayed in the header of a data control.
        /// </summary>
        public override string HeaderText
        {
            get { return _impl.HeaderText;  }
            set { _impl.HeaderText = value; }
        }

        /// <summary>
        /// Initializes new instance of the <see cref="BoundField"/> class.
        /// </summary>
        public BoundField()
        {
            _impl = new BoundFieldImpl<BoundField>(this);
        }

        /// <summary>
        /// Initializes the <see cref="BoundField"/> object.
        /// </summary>
        /// <param name="enableSorting">True if sorting is supported; otherwise, false.</param>
        /// <param name="control">The data control that owns the <see cref="BoundField"/>.</param>
        /// <returns>False in all cases</returns>
        public override bool Initialize(bool enableSorting, Control control) 
        {
            _impl.Initialize(enableSorting);
            return base.Initialize(enableSorting, control);
        }

        /// <summary>
        /// Initializes the specified <see cref="ASP.DataControlFieldCell"/> object to the specified row state. 
        /// </summary>
        /// <param name="cell">The <see cref="ASP.DataControlFieldCell"/> to initialize.</param>
        /// <param name="cellType">One of the <see cref="ASP.DataControlCellType"/> values.</param>
        /// <param name="rowState">One of the <see cref="ASP.DataControlRowState"/> values.</param>
        /// <param name="rowIndex">The zero-based index of the row.</param>
        public override void InitializeCell(ASP.DataControlFieldCell cell, ASP.DataControlCellType cellType, ASP.DataControlRowState rowState, int rowIndex) 
        {
            _impl.InitializeCell(cell, cellType, rowState, rowIndex);
        }

        /// <summary>
        /// Initializes the specified <see cref="ASP.DataControlFieldCell"/> object to the specified row state.
        /// </summary>
        /// <param name="cell">The <see cref="ASP.DataControlFieldCell"/> to initialize.</param>
        /// <param name="rowState">One of the <see cref="ASP.DataControlRowState"/> values.</param>
        protected override void InitializeDataCell(ASP.DataControlFieldCell cell, ASP.DataControlRowState rowState)
        {
            Control child = null;
            Control control = null;
            var isEdit = (rowState & ASP.DataControlRowState.Edit) != ASP.DataControlRowState.Normal;
            if ((isEdit && !ReadOnly) || ((rowState & ASP.DataControlRowState.Insert) != ASP.DataControlRowState.Normal)) 
            {
                var box = new TextBox {ToolTip = HeaderText};
                child = box;
                if (DataField.Length != 0 && isEdit)
                    control = box;
            } 
            else if (DataField.Length != 0)
                control = cell;

            if (child != null)
                cell.Controls.Add(child);

            if (control == null || !Visible) return;
            control.DataBinding += OnDataBindField;
        }

        /// <summary>
        /// Creates an empty <see cref="BoundField"/> object.
        /// </summary>
        /// <returns>An empty <see cref="BoundField"/>.</returns>
        protected override ASP.DataControlField CreateField() 
        {
            return new BoundField();
        }

        #region [ -- IAjaxDataControlField implementation -- ]

        /// <summary>
        /// Gets a reference to the data control that the <see cref="ASP.DataControlField"/> object is associated with.
        /// </summary>
        Control IAjaxDataControlField.Control
        {
            get { return Control; }
        }

        #endregion

        #region [ -- IAjaxBoundField implementation -- ]

        /// <summary>
        /// Gets a dictionary of state information that allows you to save and 
        /// restore the view state of a <see cref="BoundField"/> object across multiple requests for the same page.
        /// </summary>
        StateBag IAjaxBoundField.ViewState
        {
            get { return ViewState; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether HTML encoding is supported by a <see cref="BoundField"/> object.
        /// </summary>
        bool IAjaxBoundField.SupportsHtmlEncode
        {
            get { return SupportsHtmlEncode; }
        }

        /// <summary>
        /// Raises the FieldChanged event.
        /// </summary>
        void IAjaxBoundField.OnFieldChanged()
        {
            OnFieldChanged();
        }

        /// <summary>
        /// Initializes the specified <see cref="ASP.DataControlFieldCell"/> object to the specified row state.
        /// </summary>
        /// <param name="cell">The <see cref="ASP.DataControlFieldCell"/> to initialize.</param>
        /// <param name="rowState">One of the <see cref="ASP.DataControlRowState"/> values.</param>
        void IAjaxBoundField.InitializeDataCell(ASP.DataControlFieldCell cell, ASP.DataControlRowState rowState)
        {
            InitializeDataCell(cell, rowState);
        }

        #endregion
    }
}
