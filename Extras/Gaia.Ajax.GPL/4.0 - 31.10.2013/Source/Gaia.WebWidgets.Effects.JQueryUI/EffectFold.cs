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
    /// Reduce the element to its top then to left to make it disappear.
    /// </summary>
    /// <remarks>
    /// Works safely with most Block Elements, except tables.
    /// </remarks>
    /// <example>
    /// <code title="Adding EffectFold to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectFold\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectFold : JQueryUIEffectBase
    {
        private const decimal DefaultFoldDuration = 1.5M;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFold"/> class.
        /// </summary>
        public EffectFold(): this(DefaultFoldDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFold"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectFold(decimal duration) : this(duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFold"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectFold(decimal duration, decimal delay)
        {
            Method = EffectMethod.Hide;
            Duration = duration;
            Delay = delay;
        }

        protected override string EffectType
        {
            get { return "fold"; }
        }
    }
}