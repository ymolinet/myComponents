/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    /// <summary>
    /// This <see cref="ControlCollection"/> serves similar purpose as <see cref="ControlCollectionOf{T}"/> but offers higher
    /// performance by using Lists to keep track of which controls are part of this Collection instead of just creating a View
    /// over the Collection. 
    /// </summary>
    /// <typeparam name="T">The Type which is allowed in this collection</typeparam>
    public class ControlCollectionOf<T> : ControlCollectionWrapperCached<T> where T: Control
    {
        ///<summary>
        /// Constructor. Initializes a new instance of <see cref="ControlCollectionWrapperCached{T}"/>
        ///</summary>
        ///<param name="owner">The owner Control</param>
        ///<param name="thisCollection">The collection containing the controls that are part of this collection wrapper</param>
        public ControlCollectionOf(T owner, IList<T> thisCollection) : base(owner, thisCollection) { }

        /// <summary>
        /// Abstract function used to Verify that a control matches the requirements for derived collections. 
        /// </summary>
        /// <param name="control">The control to verify</param>
        protected override bool IsValidControl(Control control)
        {
            if (!(control is T))
                throw new ArgumentException("Only controls of type T can be added to this collection");
            return true;
        }
    }
}