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
using System.Web.UI;
using System.Collections.Generic;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Specialized <see cref="ControlCollection"/> for partitioning owner's Control collection
    /// into composition and non-composition child controls.
    /// </summary>
    /// <typeparam name="T">Type of child controls.</typeparam>
    public class HybridControlCollection<T> : ControlCollectionWrapper<T> where T : Control
    {
        private readonly ICollection<Control> _skipTheseControls;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ownerControl">Owner control.</param>
        /// <param name="skipTheseControls">Collection of controls to skip.</param>
        public HybridControlCollection(T ownerControl, ICollection<Control> skipTheseControls) : base(ownerControl)
        {
            if (skipTheseControls == null)
                throw new ArgumentNullException("skipTheseControls");

            _skipTheseControls = skipTheseControls;
        }

        /// <summary>
        /// Return true if specified <paramref name="control"/> is valid for this wrapped container.
        /// </summary>
        /// <param name="control">Control to check for validity.</param>
        /// <returns>True if specified <paramref name="control"/> is valid.</returns>
        protected override bool IsValidControl(Control control)
        {
            return !_skipTheseControls.Contains(control);
        }
    }
}
