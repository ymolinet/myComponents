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
    /// This effect simulates a window blind, where the contents of the affected elements scroll up and down accordingly
    /// </summary>
    /// <example>
    /// <code title="Adding EffectSlideUp to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectSlideUp\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectSlideUp : EffectSlide
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSlideUp"/> class.
        /// </summary>
        public EffectSlideUp() : this(DefaultDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSlideUp"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectSlideUp(decimal duration) : this(duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSlideUp"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectSlideUp(decimal duration, decimal delay)
        {
            Method = EffectMethod.Hide;
            Direction = SlideDirection.Up;
            Duration = duration;
            Delay = delay;
        }

    }
}