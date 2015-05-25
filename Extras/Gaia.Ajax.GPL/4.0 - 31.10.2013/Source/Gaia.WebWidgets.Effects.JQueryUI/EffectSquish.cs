/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2013 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets.Effects
{
    /// <summary>
    /// Reduce the element to its top-left corner.
    /// </summary>
    /// <remarks>Works safely with most Block Elements, except tables.</remarks>
    /// <example>
    /// <code title="Adding EffectSquish to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectSquish\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectSquish : EffectScale
    {
        private const int ZeroPercentage = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSquish"/> class.
        /// </summary>
        public EffectSquish() : this(DefaultDuration) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSquish"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectSquish(decimal duration) : this(duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSquish"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectSquish(decimal duration, decimal delay) : base(ZeroPercentage, true, true, true, false, duration, delay)
        {
            Method = EffectMethod.Hide;
        }

    }
}