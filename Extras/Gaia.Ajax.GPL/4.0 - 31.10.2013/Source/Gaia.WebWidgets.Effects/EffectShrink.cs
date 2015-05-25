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
    /// “Shrinks” an element into a specific direction (see demo for better understanding), hides it when the effect is complete.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectShrink to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectShrink\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectShrink : ScriptaculousEffectBase
    {
        private ScriptaculousDirections _direction = ScriptaculousDirections.Center;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShrink"/> class.
        /// </summary>
        public EffectShrink() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShrink"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public EffectShrink(ScriptaculousDirections direction) : this(direction, 1, 0) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShrink"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="duration">The duration.</param>
        public EffectShrink(ScriptaculousDirections direction, decimal duration) : this(direction, duration, 0) {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectShrink"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectShrink(ScriptaculousDirections direction, decimal duration, decimal delay) : base(duration, delay)
        {
            Direction = direction;
        }

        /// <summary>
        /// The direction to “shrink” the element to
        /// </summary>
        public ScriptaculousDirections Direction
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
            registerEffect.EffectType = "Effect.Shrink";
            registerEffect.AddPropertyIfTrue(Direction != ScriptaculousDirections.Center, "direction",Direction.ToString().ToLower());

        }
    }
}