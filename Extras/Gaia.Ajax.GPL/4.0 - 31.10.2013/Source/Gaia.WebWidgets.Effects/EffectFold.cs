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
    /// Reduce the element to its top then to left to make it disappear.
    /// </summary>
    /// <remarks>
    /// Works safely with most Block Elements, except tables.
    /// </remarks>
    /// <example>
    /// <code title="Adding EffectFold to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectFold\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectFold : ScriptaculousEffectBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFold"/> class.
        /// </summary>
        public EffectFold() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFold"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectFold(decimal duration) : this(duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFold"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectFold(decimal duration, decimal delay)
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
            registerEffect.EffectType = "Effect.Fold";
        }
    }
}