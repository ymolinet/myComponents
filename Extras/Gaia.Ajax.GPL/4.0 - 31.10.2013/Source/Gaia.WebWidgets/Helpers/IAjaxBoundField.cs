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
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Interface implemented by <see cref="BoundField"/> and derived controls for accessing protected members.
    /// </summary>
    internal interface IAjaxBoundField : IAjaxDataControlField
    {
        /// <summary>
        /// Gets a dictionary of state information that allows you to save and 
        /// restore the view state of a <see cref="BoundField"/> object across multiple requests for the same page.
        /// </summary>
        StateBag ViewState { get; }

        /// <summary>
        /// Gets or sets a value indicating whether HTML encoding is supported by a <see cref="BoundField"/> object.
        /// </summary>
        bool SupportsHtmlEncode { get; }

        /// <summary>
        /// Raises the FieldChanged event.
        /// </summary>
        void OnFieldChanged();

        /// <summary>
        /// Initializes the specified <see cref="ASP.DataControlFieldCell"/> object to the specified row state.
        /// </summary>
        /// <param name="cell">The <see cref="ASP.DataControlFieldCell"/> to initialize.</param>
        /// <param name="rowState">One of the <see cref="ASP.DataControlRowState"/> values.</param>
        void InitializeDataCell(ASP.DataControlFieldCell cell, ASP.DataControlRowState rowState);
    }
}