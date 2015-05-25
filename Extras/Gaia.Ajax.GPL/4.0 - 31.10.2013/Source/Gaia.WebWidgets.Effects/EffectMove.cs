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
    /// This effect moves an element by modifying its position attributes.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectMove to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectMove\PuzzleGame\Default.aspx.cs" />
    /// </code> 
    /// </example>
    public class EffectMove : ScriptaculousEffectBase
    {
        private int _x;
        private int _y;
        private ModeEnum _mode = ModeEnum.Relative;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMove"/> class.
        /// </summary>
        public EffectMove() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMove"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public EffectMove(int x, int y) : this(x, y, 1.0M, 0M, ModeEnum.Absolute) {}

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
        public EffectMove(int x, int y, decimal duration, decimal delay, ModeEnum mode) : this(x,y,duration, delay, mode, ScriptaculousTransitions.Sinoidal) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMove"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="transitions">The transitions.</param>
        public EffectMove(int x, int y, decimal duration, decimal delay, ModeEnum mode, ScriptaculousTransitions transitions)
        {
            Duration = duration;
            Delay = delay;
            Mode = mode;
            _y = y;
            _x = x;
            Transition = transitions;
        }

        /// <summary>
        /// Move to (X) coordinate
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Move to (Y) coordinate 
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

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
        /// Set wheter you want to use relative/absolute coordinates when moving an element. Absolute is default
        /// </summary>
        public ModeEnum Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);

            registerEffect.EffectType = "Effect.Move";
            registerEffect.AddPropertyIfTrue(X != 0, "x", X) 
                          .AddPropertyIfTrue(Y != 0, "y", Y)
                          .AddPropertyIfTrue(Mode == ModeEnum.Absolute, "mode", "absolute");
        }

       
    }
}