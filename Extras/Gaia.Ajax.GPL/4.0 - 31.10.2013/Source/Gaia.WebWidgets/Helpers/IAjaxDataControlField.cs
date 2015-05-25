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
    /// Interface implemented by ajaxified <see cref="ASP.DataControlField"/> derived controls for accessing protected members.
    /// </summary>
    internal interface IAjaxDataControlField
    {
        /// <summary>
        /// Gets a reference to the data control that the <see cref="ASP.DataControlField"/> object is associated with.
        /// </summary>
        Control Control { get; }
    }
}