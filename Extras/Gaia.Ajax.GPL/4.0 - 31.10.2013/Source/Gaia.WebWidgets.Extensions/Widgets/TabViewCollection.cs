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

namespace Gaia.WebWidgets.Extensions
{
    /// <summary>
    /// Collection of <see cref="TabView"/> controls for <see cref="TabControl"/>
    /// </summary>
    public class TabViewCollection: GenericControlList<TabView>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">Owner control.</param>
        public TabViewCollection(Control owner) : base(owner) { }
    }
}
