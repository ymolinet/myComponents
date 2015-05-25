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
    ///<summary>
    /// Specialized <see cref="ControlCollection"/> used in HybridControls to differentiate between
    /// the Composition Controls and the custom ChildControls
    ///</summary>
    public class TreeViewItemAwareContainerCollection : ControlCollectionContainerAware<TreeViewItem>
    {
        ///<summary>
        /// Constructor. Initializes a new instance of <see cref="ControlCollectionContainerAware{T}"/>
        ///</summary>
        public TreeViewItemAwareContainerCollection(TreeViewItem owner)
            : base(owner)
        {
            
        }
    }

    /// <summary>
    /// Specialized <see cref="ControlCollection"/> of <see cref="TreeViewItem"/> controls.
    /// </summary>
    public class TreeViewItemCollection : GenericControlList<TreeViewItem>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">Owner of the collection.</param>
        public TreeViewItemCollection(Control owner) : base(owner) { }
    }
}
