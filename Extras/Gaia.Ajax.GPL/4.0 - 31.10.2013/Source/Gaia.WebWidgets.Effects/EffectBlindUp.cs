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
    /// This effect simulates a window blind, where the contents of the affected elements stay in place.
    /// </summary>
    /// <remarks>Works safely with most Block Elements, except table rows, table bodies and table heads. The opposite of Effect.BlindUp is Effect.BlindDown.</remarks>
    /// <example>
    /// <code title="Adding EffectBlindUp to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectBlindUp\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectBlindUp : ScriptaculousEffectSlideAndBlindBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectBlindUp"/> class.
        /// </summary>
        public EffectBlindUp() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectBlindUp"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectBlindUp(decimal duration) : this(duration, 0) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectBlindUp"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectBlindUp(decimal duration, decimal delay)
        {
            Duration = duration;
            Delay = delay;
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.EffectType = "Effect.BlindUp";
        }
    }
}