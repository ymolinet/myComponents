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
    /// <remarks>Works safely with most Block Elements, except tables. You can define different durations for several div elements, and place them in a row in order to make them appear one after another.</remarks>
    /// <example>
    /// <code title="Adding EffectGrow to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectGrow\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectGrow : ScriptaculousEffectBase
    {
        /// <summary>
        /// specifying the origin from which to “grow” the element 
        /// </summary>
        public ScriptaculousDirections Direction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectGrow"/> class.
        /// </summary>
        public EffectGrow() : this(ScriptaculousDirections.Center, ScriptaculousTransitions.Sinoidal, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectGrow"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public EffectGrow(ScriptaculousDirections direction) : this(direction, ScriptaculousTransitions.Sinoidal, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectGrow"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="transition">The transition.</param>
        public EffectGrow(ScriptaculousDirections direction, ScriptaculousTransitions transition) : this(direction, transition, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectGrow"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="transition">The transition.</param>
        /// <param name="duration">The duration.</param>
        public EffectGrow(ScriptaculousDirections direction, ScriptaculousTransitions transition, decimal duration) : this(direction, transition, duration, 0) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectGrow"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="transition">The transition.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectGrow(ScriptaculousDirections direction, ScriptaculousTransitions transition, decimal duration, decimal delay) : base(duration, delay)
        {
            Transition = transition;
            Direction = direction;
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.EffectType = "Effect.Grow";
            registerEffect.AddPropertyIfTrue(Direction != ScriptaculousDirections.Center, "direction", Direction.ToString().ToLower());
        }
    }
}