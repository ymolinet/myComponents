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
    /// Base class for all SlideAndBlind Scriptaculous effects
    /// </summary>
    public abstract class ScriptaculousEffectSlideAndBlindBase : ScriptaculousEffectWithScaleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptaculousEffectSlideAndBlindBase"/> class.
        /// </summary>
        protected ScriptaculousEffectSlideAndBlindBase()
        {
            ScaleX = false;
        }

        /// <summary>
        /// Sets the starting percentage for scaling, defaults to 100.0. 
        /// </summary>
        public decimal ScaleTo { get; set; }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);

            registerEffect.AddPropertyIfTrue(ScaleX, "scaleX", true)
                .AddPropertyIfTrue(!ScaleY, "scaleY", false)
                .AddPropertyIfTrue(ScaleMode != "box", "scaleMode", ScaleMode)
                .AddPropertyIfTrue(!ScaleContent, "scaleContent", false)
                .AddPropertyIfTrue(ScaleFromCenter, "scaleFromCenter", true)
                .AddPropertyIfTrue(ScaleFrom != 100M, "scaleFrom", ScaleFrom)
                .AddPropertyIfTrue(ScaleTo != 0, "scaleTo", ScaleTo);
        }

    }
}