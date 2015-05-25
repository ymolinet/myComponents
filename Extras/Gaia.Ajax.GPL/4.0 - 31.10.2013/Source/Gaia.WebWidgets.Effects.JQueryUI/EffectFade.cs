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
    /// Makes an element fade away and takes it out of the document flow when the effect is complete by setting the CSS display property to none. Opposite of <see cref="EffectAppear"/>
    /// </summary>
    /// <example>
    /// <code title="Adding EffectFade to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectFade\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectFade : JQueryUIEffectBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFade"/> class.
        /// </summary>
        public EffectFade() : this(DefaultDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFade"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectFade(decimal duration) : this(duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFade"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectFade(decimal duration, decimal delay)
        {
            Method = EffectMethod.Hide;
            Duration = duration;
            Delay = delay;
        }

        protected override string EffectType
        {
            get { return "fade"; }
        }
    }
}