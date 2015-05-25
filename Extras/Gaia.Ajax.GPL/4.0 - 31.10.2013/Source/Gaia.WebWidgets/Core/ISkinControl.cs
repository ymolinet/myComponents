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
    /// The ISkinControl applies default skins to Gaia Controls if a CssClass or StylesheetTheme is not applied. 
    /// </summary>
    public interface ISkinControl
    {
        /// <summary>
        /// Applies default skin for the Control.
        /// </summary>
        void ApplySkin();

        /// <summary>
        /// Returns true if the default skin is enabled.
        /// </summary>
        bool Enabled { get;}
    }
}
