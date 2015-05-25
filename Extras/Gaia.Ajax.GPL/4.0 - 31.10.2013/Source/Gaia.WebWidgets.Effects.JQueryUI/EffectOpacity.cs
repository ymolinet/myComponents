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
    using System.Globalization;

    /// <summary>
    /// EffectOpacity changes the opacity gradually from a given value to a new value. Valid values are between 0.0M and 1.0M
    /// </summary>
    /// <example>
    /// <code title="Adding EffectOpacity to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectOpacity\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectOpacity : JQueryUIEffectBase
    {
        private decimal _from;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectOpacity"/> class.
        /// </summary>
        public EffectOpacity() : this(0, 1) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectOpacity"/> class.
        /// </summary>
        public EffectOpacity(decimal opacity) : this(0, opacity) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectOpacity"/> class.
        /// </summary>
        /// <param name="fromOpacity">From opacity.</param>
        /// <param name="toOpacity">To opacity.</param>
        [Obsolete("fromOpacity parameter is ignored. Set initial opacity before applying the effect.")]
        public EffectOpacity(decimal fromOpacity, decimal toOpacity) : this(fromOpacity, toOpacity, DefaultDuration, 0) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectOpacity"/> class.
        /// </summary>
        /// <param name="fromOpacity">From opacity.</param>
        /// <param name="toOpacity">To opacity.</param>
        /// <param name="duration">The duration.</param>
        [Obsolete("fromOpacity parameter is ignored. Set initial opacity before applying the effect.")]
        public EffectOpacity(decimal fromOpacity, decimal toOpacity, decimal duration):this(fromOpacity, toOpacity, duration, 0) {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectOpacity"/> class.
        /// </summary>
        /// <param name="fromOpacity">From opacity.</param>
        /// <param name="toOpacity">To opacity.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        [Obsolete("fromOpacity parameter is ignored. Set initial opacity before applying the effect.")]
        public EffectOpacity(decimal fromOpacity, decimal toOpacity, decimal duration, decimal delay)
        {
            Method = EffectMethod.Animate;   
            Duration = duration;
            Delay = delay;
            From = fromOpacity;
            Opacity = toOpacity;
        }

        /// <summary>
        /// Set the value of the opacity you want. It should be a value between 0.0 and 1.0
        /// </summary>
        public decimal Opacity { get; set; }

        [Obsolete("From property is ignored. Set initial opacity before applying the effect.")]
        public decimal From
        {
            get { return _from; }
            set { _from = value; }
        }

        [Obsolete("Please use Opacity property instead.")]
        public decimal To
        {
            get { return Opacity; }
            set { Opacity = value; }
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.AddParam("{opacity:" + Opacity.ToString(NumberFormatInfo.InvariantInfo) + "}");
        }

        protected override string EffectType
        {
            get { return string.Empty; }
        }
    }

}