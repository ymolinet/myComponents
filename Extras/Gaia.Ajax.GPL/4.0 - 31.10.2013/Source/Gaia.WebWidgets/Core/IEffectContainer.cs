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
    /// Interface applied to Effect Containers for accessing the Child Effects
    /// </summary>
    public interface IEffectContainer
    {
        /// <summary>
        /// List of Child Effects
        /// </summary>
        List<Effect> Effects { get;}
    }
}