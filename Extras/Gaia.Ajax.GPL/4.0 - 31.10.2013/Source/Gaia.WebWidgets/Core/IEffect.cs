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
    using System.Collections.Generic;

    /// <summary>
    /// Implemented on Effect explicitly to make a cleaner API when working with effects. 
    /// </summary>
    public interface IEffect
    {
        /// <summary>
        /// a string containing the id of the element
        /// </summary>
        string ElementID { get; set;}
        
       
        /// <summary>
        /// retrieves the script that represents the effect
        /// </summary>
        /// <returns></returns>
        string GetScript();

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, string> PropertyParameters { get;}


        /// <summary>
        /// Include script files
        /// </summary>
        void IncludeScriptFiles();

    }
}