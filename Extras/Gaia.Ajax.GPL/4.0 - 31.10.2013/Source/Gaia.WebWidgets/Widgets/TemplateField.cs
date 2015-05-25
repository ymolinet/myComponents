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
    /// Represents a field that displays custom content in a data-bound control.
    /// </summary>
    [Designer("Gaia.WebWidgets.Design.TemplateFieldDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaWebControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class TemplateField : ASP.TemplateField, IAjaxDataControlField
    {
        private readonly DataControlFieldImpl<TemplateField> _impl;

        /// <summary>
        /// Initializes new instance of the <see cref="TemplateField"/> control.
        /// </summary>
        public TemplateField()
        {
            _impl = new DataControlFieldImpl<TemplateField>(this);
        }

        /// <summary>Returns a new instance of the field class.</summary>
        /// <returns>A new instance of the %TemplateField class.</returns>
        protected override ASP.DataControlField CreateField()
        {
            return new TemplateField();
        }

        /// <summary>Initializes the field object.</summary>
        /// <param name="sortingEnabled">True if sorting is supported; Otherwise, false.</param>
        /// <param name="control">The data control that contains the field.</param>
        /// <returns>Always returns true.</returns>
        public override bool Initialize(bool sortingEnabled, Control control)
        {
            _impl.Initialize(sortingEnabled);
            return base.Initialize(sortingEnabled, control);
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
            ITemplate template = null;
            switch (cellType)
            {
                case ASP.DataControlCellType.Header:
                    template = HeaderTemplate;
                    _impl.InitializeCell(cell, cellType, rowState, rowIndex);
                    break;
                case ASP.DataControlCellType.Footer:
                    template = FooterTemplate;
                    _impl.InitializeCell(cell, cellType, rowState, rowIndex);
                    break;
                default:
                    base.InitializeCell(cell, cellType, rowState, rowIndex);
                    break;
            }
    
            if (template == null) return;
            
            cell.Text = string.Empty;
            template.InstantiateIn(cell);
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
