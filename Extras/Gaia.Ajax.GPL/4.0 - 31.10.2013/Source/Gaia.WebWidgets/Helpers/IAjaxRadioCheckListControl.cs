/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Defines members common for <see cref="RadioButtonList"/> and <see cref="CheckBoxList"/> controls.
    /// </summary>
    public interface IAjaxRadioCheckListControl
    {
        /// <summary>
        /// Gets the direction in which the radio buttons within the group are displayed.
        /// </summary>
        ASP.RepeatDirection RepeatDirection { get; }

        /// <summary>
        /// Gets a value that specifies whether the list will be rendered by using a table element, a ul element, an ol element, or a span element.
        /// </summary>
        ASP.RepeatLayout RepeatLayout { get; }

        /// <summary>
        /// Gets the number of columns to display in the control.
        /// </summary>
        int RepeatColumns { get; }

        /// <summary>
        /// Gets the text alignment for the items within the group.
        /// </summary>
        ASP.TextAlign TextAlign { get; }
    }
}
