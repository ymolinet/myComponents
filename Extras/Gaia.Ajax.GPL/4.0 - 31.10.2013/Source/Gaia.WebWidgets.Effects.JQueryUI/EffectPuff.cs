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
    using System;

    /// <summary>
    /// Gives the illusion of the element puffing away (like a in a cloud of smoke).
    /// </summary>
    /// <example>
    /// <code title="Adding EffectPuff to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectPuff\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectPuff : JQueryUIEffectBase
    {
        private const int DefaultPercentage = 150;
        private int _percent = DefaultPercentage;
        
        /// <summary>
        /// The percentage to scale to.
        /// </summary>
        public int Percent
        {
            get { return _percent; }
            set { _percent = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPuff"/> class.
        /// </summary>
        public EffectPuff() : this(DefaultPercentage, DefaultDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPuff"/> class.
        /// </summary>
        public EffectPuff(int percent) : this(percent, DefaultDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPuff"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        
        [Obsolete("Obsolete. Use .ctor(int) instead")]
        public EffectPuff(decimal from, decimal to) : this(0, to, DefaultDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPuff"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        [Obsolete("Obsolete. Use .ctor(int) instead")]
        public EffectPuff(decimal from, decimal to, decimal duration, decimal delay) : this((int)to, duration, delay) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectPuff"/> class.
        /// </summary>
        public EffectPuff(int percent, decimal duration, decimal delay)
        {
            Percent = percent;
            Duration = duration;
            Delay = delay;
        }

        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.AddPropertyIfTrue(Percent != DefaultPercentage, "percent", Percent);
        }

        protected override string EffectType
        {
            get { return "puff"; }
        }
    }
}