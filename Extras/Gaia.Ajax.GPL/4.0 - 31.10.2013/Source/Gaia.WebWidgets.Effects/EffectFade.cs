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
    /// Makes an element fade away and takes it out of the document flow when the effect is complete by setting the CSS display property to none. Opposite of Effect.Appear.
    /// </summary>
    /// <remarks>
    /// Works safely with most HTML elements, except table rows, table bodies and table heads. 
    /// On Microsoft Internet Explorer, this effect may display a bold/ghosting artifact on elements that don’t have a defined background. 
    /// It’s unclear if this is a feature or a _bug_. Microsoft Internet Explorer can only set opacity on elements that have a ‘layout’. 
    /// To let an element have a layout, you must set some CSS positional properties, like width or height. 
    /// See Giving Elements Layout.
    /// </remarks>
    /// <example>
    /// <code title="Adding EffectFade to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectFade\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectFade : ScriptaculousEffectWithIntervalBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFade"/> class.
        /// </summary>
        public EffectFade() { }

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
            registerEffect.EffectType = "Effect.Fade";
        }
    }
}