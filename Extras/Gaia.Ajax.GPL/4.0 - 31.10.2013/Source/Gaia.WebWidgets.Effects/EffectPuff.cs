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
    /// Gives the illusion of the element puffing away (like a in a cloud of smoke).
    /// </summary>
    /// <example>
    /// <code title="Adding EffectPuff to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectPuff\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectPuff : ScriptaculousEffectWithIntervalBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPuff"/> class.
        /// </summary>
        public EffectPuff() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPuff"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public EffectPuff(decimal from, decimal to) : this(from, to, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPuff"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectPuff(decimal from, decimal to, decimal duration, decimal delay)
        {
            From = from;
            To = to;
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
            registerEffect.EffectType = "Effect.Puff";
        }

    }
}