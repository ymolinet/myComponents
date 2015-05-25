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
    /// This collection serves similar purpose as <see cref="ControlCollectionExcept{T}"/>, but offers higher performance by internally
    /// using Lists of valid controls instead of just creating a View over an existing collection. 
    /// </summary>
    /// <typeparam name="T">The type that cannot be part of this Controls Collection</typeparam>
    public class ControlCollectionExcept<T> : ControlCollectionWrapperCached<Control> 
    {
        ///<summary>
        /// Constructor. Initializes a new instance of <see cref="ControlCollectionWrapperCached{T}"/>
        ///</summary>
        ///<param name="owner">The owner Control</param>
        ///<param name="thisCollection">The collection containing the controls that are part of this collection wrapper</param>
        public ControlCollectionExcept(Control owner, IList<Control> thisCollection) : base(owner, thisCollection) {}

        /// <summary>
        /// Abstract function used to Verify that a control matches the requirements for derived collections. 
        /// </summary>
        /// <param name="control">The control to verify</param>
        protected override bool IsValidControl(Control control)
        {
            if (control is T)
                throw new ArgumentException("Controls of type cannot be added to this collection", "control");
            return true;
        }
    }
}