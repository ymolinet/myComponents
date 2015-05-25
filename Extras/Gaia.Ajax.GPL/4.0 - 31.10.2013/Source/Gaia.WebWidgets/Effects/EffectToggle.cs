/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets.Effects
{
    /// <summary>
    /// Toggles the AjaxControl's visibility
    /// </summary>
    public class EffectToggle : CoreLibraryEffectBase
    {
        /// <summary>
        /// Returns the function name for this Effect
        /// </summary>
        /// <returns></returns>
        protected override string GetElementMethod() { return "toggle"; }
    }
}