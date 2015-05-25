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

    // bug : http://bugs.jqueryui.com/ticket/4316

    /// <summary>
    /// EffectScale will scale the element and optionally it's children to a given percentange. For example 120 denotes to scale something
    /// from 100% to 120% - a 20% increase from it's previous size. 
    /// </summary>
    /// <example>
    /// <code title="Adding EffectScale to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectScale\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectScale: JQueryUIEffectBase
    {
        #region [ -- Private Members -- ]
        
        private string _scaleMode = "both";
        private const int DefaultPercentage = 120; 
        private int _percent = DefaultPercentage; 
        private bool _scaleX = true;
        private bool _scaleY = true;

        private bool _scaleContent = true;
        private decimal _scaleFrom = 100M;

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Sets whether the element should be scaled horizontally, defaults to true. 
        /// </summary>
        public bool ScaleX
        {
            get { return _scaleX; }
            set { _scaleX = value; }
        }

        /// <summary>
        /// Sets whether the element should be scaled vertically, defaults to true. 
        /// </summary>
        public bool ScaleY
        {
            get { return _scaleY; }
            set { _scaleY = value; }
        }

        /// <summary>
        /// The percentage to scale to.
        /// </summary>
        public int Percent
        {
            get { return _percent; }
            set { _percent = value; }
        }

        /// <summary>
        /// Which areas of the element will be resized: 'both', 'box', 'content'.
        /// Box resizes the border and padding of the element Content resizes any content inside of the element.
        /// </summary>
        public string ScaleMode
        {
            get { return _scaleMode; }
            set { _scaleMode = value; }
        }

        /// <summary>
        /// Sets whether content scaling should be enabled, defaults to true. 
        /// </summary>
        [Obsolete("ScaleContent property is ignored. Use ScaleMode property instead.")]
        public bool ScaleContent
        {
            get { return _scaleContent; }
            set { _scaleContent = value; }
        }

        /// <summary>
        /// If true, scale the element in a way that the center of the element stays on the same position on the screen, defaults to false. 
        /// </summary>
        public bool ScaleFromCenter { get; set; }

        /// <summary>
        /// Sets the starting percentage for scaling, defaults to 100.0. 
        /// </summary>
        [Obsolete("ScaleFrom property is ignored. Scale the element before applying the effect.")]
        public decimal ScaleFrom
        {
            get { return _scaleFrom; }
            set { _scaleFrom = value; }
        }

        /// <summary>
        /// The name of the effect implemented in jQuery
        /// </summary>
        protected override string EffectType
        {
            get { return "scale"; }
        } 
        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScale"/> class.
        /// </summary>
        public EffectScale() : this(DefaultPercentage, true, true, true, false, DefaultDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScale"/> class.
        /// </summary>
        /// <param name="percent">The percent.</param>
        public EffectScale(int percent) : this(percent, true, true, true, false, DefaultDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScale"/> class.
        /// </summary>
        /// <param name="percent">The percent.</param>
        /// <param name="scaleX">if set to <c>true</c> [scale X].</param>
        /// <param name="scaleY">if set to <c>true</c> [scale Y].</param>
        public EffectScale(int percent, bool scaleX, bool scaleY) : this(percent, scaleX, scaleY, true, false, DefaultDuration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScale"/> class.
        /// </summary>
        /// <param name="percent">The percent.</param>
        /// <param name="scaleX">if set to <c>true</c> [scale X].</param>
        /// <param name="scaleY">if set to <c>true</c> [scale Y].</param>
        /// <param name="scaleContent">if set to <c>true</c> [scale content].</param>
        /// <param name="scaleFromCenter">if set to <c>true</c> [scale from center].</param>
        public EffectScale(int percent, bool scaleX, bool scaleY, bool scaleContent, bool scaleFromCenter) : this(percent, scaleX, scaleY, scaleContent, scaleFromCenter, DefaultDuration, 0) { }

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
            ScaleContent = scaleContent;
            ScaleFromCenter = scaleFromCenter;
            Percent = percent;
        } 

        #endregion

        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.AddPropertyIfTrue(ScaleMode != "both", "scale", ScaleMode);
            registerEffect.AddPropertyIfTrue(Percent != -1, "percent", Percent); // 0 is jquery default scale percent
            registerEffect.AddPropertyIfTrue(!ScaleFromCenter, "origin", "['top','left']", false);

            // if both x and y is false, we assume default behavior which is both true
            registerEffect.AddPropertyIfTrue(ScaleX ^ ScaleY, "direction", ScaleX ? "horizontal" : "vertical");
        }
    }
}