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
    /// Defines required members for Ajax Button controls.
    /// </summary>
    internal interface IAjaxButtonControl : ASP.IButtonControl
    {
        /// <summary>
        /// Return the client-side script that executes when a Button control's Click event is raised.
        /// </summary>
        string OnClientClick { get; }

        /// <summary>
        /// Returns true if the click even should bubble on the client.
        /// </summary>
        bool EnableBubbling { get; }

        /// <summary>
        /// Returns a <see cref="PostBackOptions"/> object that represents the control's postback behavior.
        /// </summary>
        /// <returns>A <see cref="PostBackOptions"/> that represents the control's postback behavior.</returns>
        PostBackOptions GetPostBackOptions();
    }
}
