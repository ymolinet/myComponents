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
    /// EffectOpacity changes the opacity gradually from a given value to a new value. Valid values are between 0.0M and 1.0M
    /// </summary>
    /// <example>
    /// <code title="Adding EffectOpacity to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectOpacity\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectOpacity : ScriptaculousEffectWithIntervalBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectOpacity"/> class.
        /// </summary>
        public EffectOpacity() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectOpacity"/> class.
        /// </summary>
        /// <param name="fromOpacity">From opacity.</param>
        /// <param name="toOpacity">To opacity.</param>
        public EffectOpacity(decimal fromOpacity, decimal toOpacity) : this(fromOpacity, toOpacity, 1, 0) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectOpacity"/> class.
        /// </summary>
        /// <param name="fromOpacity">From opacity.</param>
        /// <param name="toOpacity">To opacity.</param>
        /// <param name="duration">The duration.</param>
        public EffectOpacity(decimal fromOpacity, decimal toOpacity, decimal duration):this(fromOpacity, toOpacity, duration, 0) {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectOpacity"/> class.
        /// </summary>
        /// <param name="fromOpacity">From opacity.</param>
        /// <param name="toOpacity">To opacity.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectOpacity(decimal fromOpacity, decimal toOpacity, decimal duration, decimal delay)
        {
            Duration = duration;
            Delay = delay;
            From = fromOpacity;
            To = toOpacity;
        }



        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.EffectType = "Effect.Opacity";
        }
    }

}