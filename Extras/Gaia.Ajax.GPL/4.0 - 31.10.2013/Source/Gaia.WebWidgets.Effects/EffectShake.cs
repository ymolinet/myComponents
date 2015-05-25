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
    /// Moves the element slightly to the left, then to the right, repeatedly.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectShake to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectShake\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectShake : ScriptaculousEffectBase
    {
        private int _distance = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShake"/> class.
        /// </summary>
        public EffectShake() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShake"/> class.
        /// </summary>
        /// <param name="distance">The distance.</param>
        public EffectShake(int distance) : this(distance, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShake"/> class.
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <param name="duration">The duration.</param>
        public EffectShake(int distance, decimal duration) : this(distance, duration, 0) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShake"/> class.
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectShake(int distance, decimal duration, decimal delay) : base(duration, delay)
        {
            Distance = distance;
        }

        /// <summary>
        /// Defaults to 20, the number of pixels to move horizontally 
        /// </summary>
        public int Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.EffectType = "Effect.Shake";
            registerEffect.AddPropertyIfTrue(Distance != 20, "distance", Distance);

        }
    }
}