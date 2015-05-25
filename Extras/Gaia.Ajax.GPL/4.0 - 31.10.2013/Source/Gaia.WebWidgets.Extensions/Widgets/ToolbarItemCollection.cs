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
    /// Specialized <see cref="ControlCollection"/> used in HybridControls to differentiate between
    /// the Composition Controls and the custom ChildControls
    /// </summary>
    public class ToolbarItemAwareContainerCollection : ControlCollectionContainerAware<ToolbarItem>
    {
        ///<summary>
        /// Constructor. Initializes a new instance of <see cref="ControlCollectionContainerAware{T}"/>
        ///</summary>
        public ToolbarItemAwareContainerCollection(ToolbarItem owner) : base(owner)
        {
        }
    }

    /// <summary>
    /// Specialized <see cref="ControlCollection"/> for <see cref="ToolbarItem"/> controls.
    /// </summary>
    public class ToolbarItemCollection : GenericControlList<ToolbarItem>
    {
        /// <summary>
        /// Creates an instance of the collection class
        /// </summary>
        /// <param name="owner">Owner control.</param>
        public ToolbarItemCollection(Control owner) : base(owner) { }
    }
}
