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
    using System.Globalization;

    /// <summary>
    /// This effect moves an element by modifying its position attributes.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectMove to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectMove\PuzzleGame\Default.aspx.cs" />
    /// </code> 
    /// </example>
    public class EffectMove : JQueryUIEffectBase
    {
        private ModeEnum _mode = ModeEnum.Relative;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMove"/> class.
        /// </summary>
        public EffectMove() : this(0,0) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMove"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public EffectMove(int x, int y) : this(x, y, DefaultDuration,0M, ModeEnum.Absolute) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMove"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="duration">The duration.</param>
        public EffectMove(int x, int y, decimal duration) : this(x,y,duration, 0,ModeEnum.Absolute) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMove"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="mode">The mode.</param>
        public EffectMove(int x, int y, decimal duration, decimal delay, ModeEnum mode) : this(x,y,duration, delay, mode, Easing.EaseOutElastic) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMove"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="transitions">The transitions.</param>
        public EffectMove(int x, int y, decimal duration, decimal delay, ModeEnum mode, Easing transitions)
        {
            Method = EffectMethod.Animate;
            Duration = duration;
            Delay = delay;
            Mode = mode;
            Y = y;
            X = x;
            Easing = transitions;
        }

        /// <summary>
        /// Move to (X) coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Move to (Y) coordinate 
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Enum that defines how the coordinates will be applied to the effect. 
        /// </summary>
        public enum ModeEnum
        {
            /// <summary>
            /// Move to relative coordinates
            /// </summary>
            Relative,
            
            /// <summary>
            /// Move to absolute coordinates (default)
            /// </summary>
            Absolute
        }

        /// <summary>
        /// Set whether you want to use relative/absolute coordinates when moving an element. Absolute is default
        /// </summary>
        public ModeEnum Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        protected override string EffectType
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            
            string top, left;
            if (_mode == ModeEnum.Relative)
            {
                top = Y.ToString("+=#;-=#;+=0", NumberFormatInfo.InvariantInfo);
                left = X.ToString("+=#;-=#;+=0", NumberFormatInfo.InvariantInfo);
            }
            else
            {
                top = Y.ToString(NumberFormatInfo.InvariantInfo);
                left = X.ToString(NumberFormatInfo.InvariantInfo);
            }
            
            var propertyParameters = (this as IEffect).PropertyParameters;
            string propX, propY;
            var hasLeftProperty = propertyParameters.TryGetValue("x", out propX);
            var hasTopProperty = propertyParameters.TryGetValue("y", out propY);
            
            if (hasLeftProperty)
            {
                propertyParameters.Remove("x");
                left = "'+=' + e.memo." + propX;
            }

            if (hasTopProperty)
            {
                propertyParameters.Remove("y");
                top = "'+=' + e.memo." + propY;
            }

            registerEffect.AddParam(string.Concat("{left:", ToValue(left, !hasLeftProperty),
                                                  ",top:", ToValue(top, !hasTopProperty), "}"));
        }

        private static string ToValue(string str, bool addQuotes)
        {
            return addQuotes ? "'" + str + "'" : str;
        }
    }
}