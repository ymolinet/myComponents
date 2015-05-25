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
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Specialized <see cref="PropertyStateManagerControl"/> for compare validator controls.
    /// </summary>
    public class PropertyStateManagerBaseCompareValidator : PropertyStateManagerBaseValidator
    {
        private bool _invariantValues;
        private ASP.ValidationDataType _type;

        private readonly BaseValidator _impl;
        private readonly ASP.BaseCompareValidator _compareValidator;

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerBaseCompareValidator"/> for specified <paramref name="baseValidator"/>.
        /// </summary>
        /// <param name="baseValidator">Control to track changes for.</param>
        /// <remarks>
        /// Stores state information for the control and serializes changes during callback rendering.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="baseValidator"/> is null.</exception>
        internal PropertyStateManagerBaseCompareValidator(BaseValidator baseValidator) : base(baseValidator)
        {
            _impl = baseValidator;
            _compareValidator = (ASP.BaseCompareValidator) _impl.Validator;
        }

        /// <summary>
        /// Returns true if the associated validator needs to be rerendered.
        /// </summary>
        protected override bool RequiresRerendering
        {
            get
            {
                return base.RequiresRerendering ||
                       ValueHasChanged(_type, _compareValidator.Type) ||
                       ValueHasChanged(_invariantValues, _compareValidator.CultureInvariantValues);
            }
        }

        /// <summary>
        /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
        protected override void AssignState(PropertyStateManagerControl source)
        {
            base.AssignState(source);

            var stateManager = source as PropertyStateManagerBaseCompareValidator;
            if (stateManager == null) return;

            _type = stateManager._type;
            _invariantValues = stateManager._invariantValues;
        }

        /// <summary>
        /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void TakeSnapshot()
        {
            _type = _compareValidator.Type;
            _invariantValues = _compareValidator.CultureInvariantValues;

            base.TakeSnapshot();
        }
    }
}
