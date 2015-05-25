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
    /// Represents a field that is displayed as an image in a data-bound control.
    /// </summary>
    [Designer("Gaia.WebWidgets.Design.ImageFieldDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaWebControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class ImageField : ASP.ImageField, IAjaxDataControlField
    {
        private readonly DataControlFieldImpl<ImageField> _impl;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ImageField()
        {
            _impl = new DataControlFieldImpl<ImageField>(this);
        }

        /// <summary>Returns a new instance of the field class.</summary>
        /// <returns>A new instance of the %ImageField class.</returns>
        protected override ASP.DataControlField CreateField() 
        {
            return new ImageField();
        }

        /// <summary>Initializes the field object.</summary>
        /// <param name="enableSorting">True if sorting is supported; Otherwise, false.</param>
        /// <param name="control">The data control that contains the field.</param>
        /// <returns>Always returns true.</returns>
        public override bool Initialize(bool enableSorting, Control control) 
        {
            _impl.Initialize(enableSorting);
            return base.Initialize(enableSorting, control);
        }

        /// <summary>
        /// Initializes the specified cell object with the specified cell type, row state, and row index.
        /// </summary>
        /// <param name="cell">The cell to initialize.</param>
        /// <param name="cellType">One of the %DataControlCellType values.</param>
        /// <param name="rowState">One of the %DataControlRowState values.</param>
        /// <param name="rowIndex">The zero-based index of the row.</param>
        public override void InitializeCell(ASP.DataControlFieldCell cell, ASP.DataControlCellType cellType, ASP.DataControlRowState rowState, int rowIndex) 
        {
            _impl.InitializeCell(cell, cellType, rowState, rowIndex);
            if (cellType != ASP.DataControlCellType.DataCell) return;
            InitializeDataCell(cell, rowState);
        }

        /// <summary>
        /// Initializes the specified cell object with the specified row state.
        /// </summary>
        /// <param name="cell">The cell to initialize.</param>
        /// <param name="rowState">One of the %DataControlRowState values.</param>
        protected override void InitializeDataCell(ASP.DataControlFieldCell cell, ASP.DataControlRowState rowState) 
        {
            Control dataBindControl = null;
            var isEdit = (rowState & ASP.DataControlRowState.Edit) != ASP.DataControlRowState.Normal;
            if ((isEdit && !ReadOnly) || ((rowState & ASP.DataControlRowState.Insert) != ASP.DataControlRowState.Normal)) 
            {
                var child = new TextBox();
                cell.Controls.Add(child);
                if (DataImageUrlField.Length != 0 && isEdit)
                    dataBindControl = child;
            }
            else if (DataImageUrlField.Length != 0) 
            {
                dataBindControl = cell;
                cell.Controls.Add(new Image());
                cell.Controls.Add(new Label());
            }

            if (dataBindControl == null || !Visible) return;
            dataBindControl.DataBinding += OnDataBindField;
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
    }
}
