/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using ASP = System.Web.UI;

[assembly: ASP.WebResource("Gaia.WebWidgets.Scripts.Aspect.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Interface all aspects must implement
    /// </summary>
    public interface IAspect
    {
        /// <summary>
        /// Returns Aspect initialization script
        /// </summary>
        string GetScript();

        /// <summary>
        /// The Control the Aspect is attached to
        /// </summary>
        IAspectableAjaxControl ParentControl { get; set; }

        /// <summary>
        /// Called when it's time to include the JavaScript files for the Aspect
        /// </summary>
        void IncludeScriptFiles();
    }
}
