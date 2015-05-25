﻿/*******************************************************************
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
    /// This effect simulates a window blind, where the contents of the affected elements stay in place.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectBlindDown to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectBlindDown\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectBlindDown : EffectBlind
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectBlindDown"/> class.
        /// </summary>
        public EffectBlindDown() : this(DefaultDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectBlindDown"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectBlindDown(decimal duration) : this(duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectBlindDown"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectBlindDown(decimal duration, decimal delay)
        {
            Method = EffectMethod.Show;
            Duration = duration;
            Delay = delay;
        }

    }
}