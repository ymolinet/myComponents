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
    /// Interface implemented by ajaxified validators to access protected members.
    /// </summary>
    internal interface IAjaxValidator
    {
        /// <summary>
        /// Gets a value that indicates whether the control specified by the <see cref="ASP.BaseValidator.ControlToValidate"/> property is a valid control.
        /// </summary>
        bool PropertiesValid { get; }

        /// <summary>
        /// Gets a value indicating whether the control is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets a value denoting if the client browser supports "upward" rendering.
        /// </summary>
        bool RenderUplevel { get; }

        /// <summary>
        /// Determines whether the validation control can perform client-side validation.
        /// </summary>
        bool DetermineRenderUplevel();

        /// <summary>
        /// Registers an ECMAScript array declaration using the array name Page_Validators.
        /// </summary>
        void RegisterValidatorDeclaration();

        /// <summary>
        /// Gets the client ID of the control having specified id.
        /// </summary>
        /// <param name="controlToValidate">The name of the control to get the client ID from. </param>
        /// <returns>The client ID of the specified control.</returns>
        string GetControlRenderId(string controlToValidate);

        /// <summary>
        /// Renders the validator to the specified HTML <paramref name="writer"/> during design time.
        /// </summary>
        void RenderDesignTime(HtmlTextWriter writer);
    }
}