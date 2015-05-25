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
    /// Makes an element drop and fade out at the same time.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectDropOut to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectDropOut\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectDropOut : JQueryUIEffectBase
    {
        private const decimal DefaultDropOutDuration = 0.3M;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectDropOut"/> class.
        /// </summary>
        public EffectDropOut() : this(DefaultDropOutDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectDropOut"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectDropOut(decimal duration) : this(duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectDropOut"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectDropOut(decimal duration, decimal delay)
        {
            Method = EffectMethod.Hide;
            Duration = duration;
            Delay = delay;
        }

        protected override string EffectType
        {
            get { return "drop"; }
        }

        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.AddProperty("direction", "down");
        }
    }
}