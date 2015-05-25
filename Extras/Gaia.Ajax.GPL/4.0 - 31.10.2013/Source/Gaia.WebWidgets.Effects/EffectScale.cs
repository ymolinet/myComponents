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
    using System.Globalization;

    /// <summary>
    /// EffectScale will scale the element and optionally it's children to a given percentange. For example 120 denotes to scale something
    /// from 100% to 120% - a 20% increase from it's previous size. 
    /// </summary>
    /// <example>
    /// <code title="Adding EffectScale to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectScale\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectScale : ScriptaculousEffectWithScaleBase
    {
        private int _percent = 120;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScale"/> class.
        /// </summary>
        public EffectScale() : this(120, true, true, true, false, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScale"/> class.
        /// </summary>
        /// <param name="percent">The percent.</param>
        public EffectScale(int percent) : this(percent, true, true, true, false, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScale"/> class.
        /// </summary>
        /// <param name="percent">The percent.</param>
        /// <param name="scaleX">if set to <c>true</c> [scale X].</param>
        /// <param name="scaleY">if set to <c>true</c> [scale Y].</param>
        public EffectScale(int percent, bool scaleX, bool scaleY) : this(percent, scaleX, scaleY, true, false, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScale"/> class.
        /// </summary>
        /// <param name="percent">The percent.</param>
        /// <param name="scaleX">if set to <c>true</c> [scale X].</param>
        /// <param name="scaleY">if set to <c>true</c> [scale Y].</param>
        /// <param name="scaleContent">if set to <c>true</c> [scale content].</param>
        /// <param name="scaleFromCenter">if set to <c>true</c> [scale from center].</param>
        public EffectScale(int percent, bool scaleX, bool scaleY, bool scaleContent, bool scaleFromCenter): this(percent, scaleX, scaleY, scaleContent, scaleFromCenter, 1, 0) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScale"/> class.
        /// </summary>
        /// <param name="percent">The percent.</param>
        /// <param name="scaleX">if set to <c>true</c> [scale X].</param>
        /// <param name="scaleY">if set to <c>true</c> [scale Y].</param>
        /// <param name="scaleContent">if set to <c>true</c> [scale content].</param>
        /// <param name="scaleFromCenter">if set to <c>true</c> [scale from center].</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectScale(int percent, bool scaleX, bool scaleY, bool scaleContent, bool scaleFromCenter, decimal duration, decimal delay)
        {

            Duration = duration;
            Delay = delay;

            ScaleX = scaleX;
            ScaleY = scaleY;
            ScaleContent= scaleContent;
            ScaleFromCenter = scaleFromCenter;
            Percent = percent;
        }

        /// <summary>
        /// Determines the percentage scale increase when effect is applied
        /// </summary>
        public int Percent
        {
            get { return _percent; }
            set { _percent = value; }
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);

            registerEffect.EffectType = "Effect.Scale";
            registerEffect.AddParam(Percent.ToString(CultureInfo.InvariantCulture))
                .AddPropertyIfTrue(!ScaleX, "scaleX", false)
                .AddPropertyIfTrue(!ScaleY, "scaleY", false)
                .AddPropertyIfTrue(ScaleMode != "box", "scaleMode", ScaleMode)
                .AddPropertyIfTrue(!ScaleContent, "scaleContent", false)
                .AddPropertyIfTrue(ScaleFromCenter, "scaleFromCenter", true)
                .AddPropertyIfTrue(ScaleFrom != 100M, "scaleFrom", ScaleFrom); 
        }

    }
}