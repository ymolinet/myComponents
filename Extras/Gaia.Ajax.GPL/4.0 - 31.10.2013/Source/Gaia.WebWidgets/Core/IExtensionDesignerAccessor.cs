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
    /// This interface is used by ExtensionControlDesigner to explicitly 
    /// ask the control implementing it, to set it pre render defaults when appropriate
    /// </summary>
    public interface IExtensionDesignerAccessor
    {
        /// <summary>
        /// Called by the ExtensionControlDesigner, when default values should be set
        /// before actually rendering the control on the design surface
        /// </summary>
        void SetPreRenderDefaults();
    }
}
