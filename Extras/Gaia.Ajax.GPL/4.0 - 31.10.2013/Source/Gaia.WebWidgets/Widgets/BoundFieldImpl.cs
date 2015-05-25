/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Web;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{    
    /// <summary>
    /// Implementation details for the <see cref="BoundField"/> and derived classes.
    /// </summary>
    internal class BoundFieldImpl<T> : DataControlFieldImpl<T> where T : ASP.BoundField, IAjaxBoundField
    {
        private readonly T _field;
        private bool _encodeHeaderTextAndSuppressChange;

        /// <summary>
        /// Initializes new instance of the <see cref="BoundFieldImpl{T}"/> class.
        /// </summary>
        /// <param name="field">Owner field.</param>
        public BoundFieldImpl(T field) : base(field)
        {
            _field = field;
        }

        /// <summary>
        /// Gets or sets the text that is displayed in the header of a data control.
        /// </summary>
        public string HeaderText
        {
            get
            {
                var value = StateUtil.Get(_field.ViewState, "HeaderText", string.Empty);
                return _encodeHeaderTextAndSuppressChange ? HttpUtility.HtmlEncode(value) : value;
            }
            set
            {
                if (value == HeaderText) return;
                StateUtil.Set(_field.ViewState, "HeaderText", value, string.Empty);
                if (_encodeHeaderTextAndSuppressChange) return;
                _field.OnFieldChanged();
            }
        }

        /// <summary>
        /// Initializes the specified <see cref="ASP.DataControlFieldCell"/> object to the specified row state. 
        /// </summary>
        /// <param name="cell">The <see cref="ASP.DataControlFieldCell"/> to initialize.</param>
        /// <param name="cellType">One of the <see cref="ASP.DataControlCellType"/> values.</param>
        /// <param name="rowState">One of the <see cref="ASP.DataControlRowState"/> values.</param>
        /// <param name="rowIndex">The zero-based index of the row.</param>
        public override void InitializeCell(System.Web.UI.WebControls.DataControlFieldCell cell, ASP.DataControlCellType cellType, ASP.DataControlRowState rowState, int rowIndex)
        {
            var headerText = _field.HeaderText;

            if (((cellType == ASP.DataControlCellType.Header) && _field.SupportsHtmlEncode) && _field.HtmlEncode && !string.IsNullOrEmpty(headerText))
                _encodeHeaderTextAndSuppressChange = true;

            base.InitializeCell(cell, cellType, rowState, rowIndex);

            _encodeHeaderTextAndSuppressChange = false;

            if (cellType != ASP.DataControlCellType.DataCell) return;
            _field.InitializeDataCell(cell, rowState);
        }
    }
}
