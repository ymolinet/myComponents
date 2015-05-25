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
    /// This effect simulates a window blind, where the contents of the affected elements scroll up and down accordingly
    /// </summary>
    /// <remarks>
    /// Include a second div element, wrapping the contents of the outer div. Your element must look like this:
    /// <example>
    /// <div id="x">
    ///  <div>
    ///    contents
    ///  </div>
    ///</div>
    ///</example>
    /// The target element should not have padding set, otherwise you’ll see the effect “bouncing”. 
    /// Because of a _bug in Internet Explorer 6 (overflow not correctly hidden), an additional wrapper div is needed if you want to use 
    /// these effects on absolutely positioned elements (wrapper is the absolutely positioned element, x has position:relative set):
    ///<example>
    /// <div id="wrapper">
    ///  <div id="x">
    ///    <div>
    ///      contents
    ///    </div>
    ///  </div>
    ///</div>
    /// </example> 
    /// Works only on block elements. In Internet Explorer 6.0 there’s a problem where floated block level elements don’t animate. 
    /// If you add a position: relative to the element it all works though.The opposite of Effect.SlideDown is Effect.SlideUp.
    /// </remarks>
    /// <example>
    /// <code title="Adding EffectSlideDown to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectSlideDown\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectSlideDown : ScriptaculousEffectSlideAndBlindBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSlideDown"/> class.
        /// </summary>
        public EffectSlideDown() {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSlideDown"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectSlideDown(decimal duration) : this(duration, 0) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSlideDown"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectSlideDown(decimal duration, decimal delay)
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
            registerEffect.EffectType = "Effect.SlideDown";

        }

    }
}