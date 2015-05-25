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
    /// Shake effect
    /// </summary>
    /// <example>
    /// <code title="Adding EffectShake to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectShake\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectShake : JQueryUIEffectBase
    {
        private const int DefaultDistance = 20;
        private const decimal DefaultShakeDuration = 0.5M;
        private int _distance = DefaultDistance;
        private ShakeDirection _direction = ShakeDirection.Left;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShake"/> class.
        /// </summary>
        public EffectShake() : this(DefaultDistance) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShake"/> class.
        /// </summary>
        /// <param name="distance">The distance.</param>
        public EffectShake(int distance) : this(distance, DefaultShakeDuration) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShake"/> class.
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <param name="duration">The duration.</param>
        public EffectShake(int distance, decimal duration) : this(distance, duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShake"/> class.
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectShake(int distance, decimal duration, decimal delay)
        {
            Duration = duration;
            Delay = delay;
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

        public enum ShakeDirection
        {
            Left,

            Up, 

            Down, 

            Right
        }

        /// <summary>
        /// The Direction for the Shake effect
        /// </summary>
        public ShakeDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }


        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.AddPropertyIfTrue(Distance != DefaultDistance, "distance", Distance);
            registerEffect.AddPropertyIfTrue(Direction != ShakeDirection.Left, "direction", Direction.ToString().ToLowerInvariant());

        }

        protected override string EffectType
        {
            get { return "shake"; }
        }
    }
}