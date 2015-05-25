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
    /// Make an element appear. If the element was previously set to display:none inside the style attribute of the element, 
    /// the effect will automatically show the element. This means that display must be set within the style attribute of an object, 
    /// and not in the CSS in the head of the document or a linked file. 
    /// In other words, this Effect will not work if display:none is set within style tag or linked CSS file. 
    /// Alternatively, display:none can be set using a document.getElementById script even if no style is set in the style attribute.
    /// </summary>
    /// <remarks>
    /// Works safely with most HTML elements, except table rows, table bodies and table heads. 
    /// On Microsoft Internet Explorer, this effect may display a bold/ghosting artifact on elements that don’t have a defined background. 
    /// It’s unclear if this is a feature or a _bug. Microsoft Internet Explorer can only set opacity on elements that have a ‘layout’. 
    /// To let an element have a layout, you must set some CSS positional properties, like width or height. 
    /// 
    /// See Giving Elements Layout. The opposite of Effect.Appear is Effect.Fade. 
    /// </remarks>
    /// <example>
    /// <code title="Adding EffectAppear to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectAppear\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectAppear : ScriptaculousEffectWithIntervalBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectAppear"/> class.
        /// </summary>
        public EffectAppear() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectAppear"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public EffectAppear(decimal duration) : this(duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectAppear"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectAppear(decimal duration, decimal delay)
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
            registerEffect.EffectType = "Effect.Appear";
        }
    }
}