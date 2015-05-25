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
    /// Gives the illusion of a TV-style switch off.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectSwitchOff to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectSwitchOff\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectSwitchOff : JQueryUIEffectBase
    {
        private const int DefaultSwitchOffDelay = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSwitchOff"/> class.
        /// </summary>
        public EffectSwitchOff() : this(DefaultDuration) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSwitchOff"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectSwitchOff(decimal duration) : this(duration, DefaultSwitchOffDelay) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectSwitchOff"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectSwitchOff(decimal duration, decimal delay)
        {
            Method = EffectMethod.Hide;
            Easing = Easing.EaseOutElastic;
            Duration = duration;
            Delay = delay;
        }

        protected override string EffectType
        {
            get { return "fade"; }
        }
    }
}