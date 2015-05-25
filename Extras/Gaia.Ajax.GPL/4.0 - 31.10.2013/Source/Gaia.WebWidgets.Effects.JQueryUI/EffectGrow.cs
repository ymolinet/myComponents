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
    /// “Grows” an element into a specific direction 
    /// </summary>
    /// <example>
    /// <code title="Adding EffectGrow to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectGrow\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectGrow : EffectScale
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectGrow"/> class.
        /// </summary>
        public EffectGrow()
        {
            Method = EffectMethod.Show;
            ScaleFromCenter = true;
            Percent = 100;
        }
    }
}