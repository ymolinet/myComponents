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
    /// <summary>
    /// Defines constants used in the assembly
    /// </summary>
    static class Constants
    {
        /// <summary>
        /// Function name which handles CausesValidation property.
        /// </summary>
        public const string SetCausesValidationFunctionName = "setVal";

        /// <summary>
        /// Function name which handles ValidationGroup property.
        /// </summary>
        public const string SetValidationGroupFunctionName = "setValGrp";

        /// <summary>
        /// Function name which handles PostBackUrl and PostBackOptions.ActionUrl property.
        /// </summary>
        public const string SetPostBackUrlFunctionName = "setPostUrl";

        /// <summary>
        /// Function name which handles PostBackOptions.Argument property.
        /// </summary>
        public const string SetArgumentFunctionName = "setArg";

        /// <summary>
        /// Client-side reference to an object which acts as entry point for client callback methods.
        /// </summary>
        public const string GaiaClientModule = "$$";
    }
}
