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
    /// Pulsates the element, loops over five times over fading out and in.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectPulsate to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectPulsate\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectPulsate : ScriptaculousEffectWithIntervalBase
    {
        private int _pulses = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPulsate"/> class.
        /// </summary>
        public EffectPulsate() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPulsate"/> class.
        /// </summary>
        /// <param name="pulses">The pulses.</param>
        public EffectPulsate(int pulses) : this(pulses, 0, 1) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPulsate"/> class.
        /// </summary>
        /// <param name="pulses">The pulses.</param>
        /// <param name="from">From.</param>
        public EffectPulsate(int pulses, decimal from) : this(pulses, from, 1) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPulsate"/> class.
        /// </summary>
        /// <param name="pulses">The pulses.</param>
        /// <param name="from">From.</param>
        /// <param name="duration">The duration.</param>
        public EffectPulsate(int pulses, decimal from, decimal duration)
        {
            From = from;
            Duration = duration;
            _pulses = pulses;
        }

        /// <summary>
        /// The amount of pulses within the duration time 
        /// </summary>
        public int Pulses
        {
            get { return _pulses; }
            set { _pulses = value; }
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.EffectType = "Effect.Pulsate";
            registerEffect.AddPropertyIfTrue(Pulses != 5, "pulses", Pulses);

        }

    }
}