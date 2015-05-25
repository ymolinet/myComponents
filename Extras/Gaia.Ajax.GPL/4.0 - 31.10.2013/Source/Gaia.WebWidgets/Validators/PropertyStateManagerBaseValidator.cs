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
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Specialized <see cref="PropertyStateManagerControl"/> for validator controls.
    /// </summary>
    public class PropertyStateManagerBaseValidator : PropertyStateManagerWebControl, IMayRequireRerendering
    {
        private string _validationText;
        private ASP.BaseValidator _state;
        private string _controlToValidate;
        private BaseValidator.ValidatorDisplayMode _displayMode;

        private readonly BaseValidator _impl;
        private readonly IAjaxValidator _accessor;
        private readonly ASP.BaseValidator _validator;

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerBaseValidator"/> for specified <paramref name="baseValidator"/>.
        /// </summary>
        /// <param name="baseValidator">Control to track changes for.</param>
        /// <remarks>
        /// Stores state information for the control and serializes changes during callback rendering.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="baseValidator"/> is null.</exception>
        internal PropertyStateManagerBaseValidator(BaseValidator baseValidator) : base(baseValidator.Validator)
        {
            _impl = baseValidator;
            _validator = baseValidator.Validator;
            _accessor = (IAjaxValidator) _validator;
        }

        /// <summary>
        /// Returns true if associated <see cref="Control"/> needs to be rerendered.
        /// </summary>
        bool IMayRequireRerendering.RequiresRerendering
        {
            get { return RequiresRerendering; }
        }

        /// <summary>
        /// Returns true if the associated <see cref="IValidator"/> needs to be rerendered.
        /// </summary>
        protected virtual bool RequiresRerendering
        {
            get
            {
                return
                    ValueHasChanged(_state.Display, _validator.Display) ||
                    ValueHasChanged(_state.ErrorMessage, _validator.ErrorMessage) ||
                    ValueHasChanged(_state.ValidationGroup, _validator.ValidationGroup) ||
                    ValueHasChanged(_state.SetFocusOnError, _validator.SetFocusOnError) ||
                    ValueHasChanged(_state.EnableClientScript, _validator.EnableClientScript) ||
                    ValueHasChanged(_controlToValidate, _accessor.GetControlRenderId(_validator.ControlToValidate)) ||
                    ValueHasChanged(_state.Enabled, Utilities.IsEnabled(_validator)) ||
                    ValueHasChanged(_impl.WasValid, _validator.IsValid);
            }
        }

        /// <summary>
        /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
        protected override void AssignState(PropertyStateManagerControl source)
        {
            base.AssignState(source);

            var stateManager = source as PropertyStateManagerBaseValidator;
            if (stateManager == null) return;

            _state = stateManager._state;
            _displayMode = stateManager._displayMode;
            _validationText = stateManager._validationText;
            _controlToValidate = stateManager._controlToValidate;
        }

        /// <summary>
        /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void TakeSnapshot()
        {
            _state = new RequiredFieldValidator
                         {
                             Display = _validator.Display,
                             ErrorMessage = _validator.ErrorMessage,
                             Enabled = Utilities.IsEnabled(_validator),
                             ValidationGroup = _validator.ValidationGroup,
                             SetFocusOnError = _validator.SetFocusOnError,
                             EnableClientScript = _validator.EnableClientScript
                         };

            _controlToValidate = _accessor.GetControlRenderId(_validator.ControlToValidate);
            _displayMode = _impl.GetDisplayMode(_impl.WasValid, _accessor.DetermineRenderUplevel());
            _validationText = _impl.GetValidationText(_impl.WasValid, _accessor.DetermineRenderUplevel());
                
            base.TakeSnapshot();
        }

        /// <summary>
        /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void DiffSnapshot()
        {
            var isValid = _validator.IsValid;
            
            RenderChange(_validationText, _impl.GetValidationText(isValid, _accessor.RenderUplevel), "setText", true);
            RenderChange(_displayMode, _impl.GetDisplayMode(isValid, _accessor.RenderUplevel), _impl.SetDisplayStyle);

            base.DiffSnapshot();
        }

        /// <summary>
        /// Returns true if the specified values are not equal.
        /// </summary>
        internal static bool ValueHasChanged<T>(T initial, T current)
        {
            return !EqualityComparer<T>.Default.Equals(initial, current);
        }
    }
}