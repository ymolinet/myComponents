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
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    using Widgets.DataControls;

    /// <summary>
    /// Implementation details for ajaxified <see cref="System.Web.UI.WebControls.DataControlField"/> derived classes.
    /// </summary>
    internal class DataControlFieldImpl<T> where T : ASP.DataControlField, IAjaxDataControlField
    {
        private readonly T _field;
        private bool _sortingEnabled;

        /// <summary>
        /// Initializes new instace of the <see cref="DataControlFieldImpl{T}"/> class.
        /// </summary>
        /// <param name="field">Owner field.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="field"/> is null.</exception>
        public DataControlFieldImpl(T field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            _field = field;
        }

        /// <summary>
        /// Initializes the associated <see cref="ASP.DataControlField"/> object.
        /// </summary>
        /// <param name="enableSorting">True if sorting is supported; otherwise, false.</param>
        public virtual void Initialize(bool enableSorting)
        {
            _sortingEnabled = enableSorting;
        }

        /// <summary>
        /// Initializes the specified <see cref="ASP.DataControlFieldCell"/> object to the specified row state. 
        /// </summary>
        /// <param name="cell">The <see cref="ASP.DataControlFieldCell"/> to initialize.</param>
        /// <param name="cellType">One of the <see cref="ASP.DataControlCellType"/> values.</param>
        /// <param name="rowState">One of the <see cref="ASP.DataControlRowState"/> values.</param>
        /// <param name="rowIndex">The zero-based index of the row.</param>
        public virtual void InitializeCell(ASP.DataControlFieldCell cell, ASP.DataControlCellType cellType, ASP.DataControlRowState rowState, int rowIndex)
        {
            switch (cellType)
            {
                case ASP.DataControlCellType.Header:
                    {
                        InitializeHeaderCell(cell);
                        break;
                    }
                case ASP.DataControlCellType.Footer:
                    {
                        var footerText = _field.FooterText;
                        cell.Text = string.IsNullOrEmpty(footerText) ? "&nbsp;" : footerText;
                        break;
                    }
            }
        }

        /// <summary>
        /// Initializes specified <paramref name="cell"/> as a header cell.
        /// </summary>
        private void InitializeHeaderCell(ASP.TableCell cell)
        {
            var headerText = _field.HeaderText;
            var headerImageUrl = _field.HeaderImageUrl;
            var sortExpression = _field.SortExpression;
            var container = _field.Control as ASP.IPostBackContainer;
            var canSort = _sortingEnabled && sortExpression.Length > 0;

            if (string.IsNullOrEmpty(headerImageUrl))
            {
                if (canSort)
                {
                    var sortButton = DataControlFactory.LinkButton(headerText, "Sort", sortExpression, false, container);
                    cell.Controls.Add(sortButton);
                }
                else
                    cell.Text = string.IsNullOrEmpty(headerText) ? "&nbsp;" : headerText;
            }
            else if (!canSort)
            {
                var image = new Image {ImageUrl = headerImageUrl, AlternateText = headerText};
                cell.Controls.Add(image);
            }
            else
            {
                var sortButton = DataControlFactory.ImageButton(headerText, headerImageUrl, "Sort", sortExpression,
                                                                false, container);
                cell.Controls.Add(sortButton);
            }
        }
    }
}
