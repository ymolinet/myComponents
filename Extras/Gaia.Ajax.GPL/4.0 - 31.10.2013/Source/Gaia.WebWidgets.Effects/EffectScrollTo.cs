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
    /// Scrolls to a specific place in the page.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectScrollTo to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectScrollTo\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectScrollTo :ScriptaculousEffectBase
    {
        private int _offset;
        
        /// <summary>
        /// Offset
        /// </summary>
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScrollTo"/> class.
        /// </summary>
        public EffectScrollTo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScrollTo"/> class.
        /// </summary>
        /// <param name="offset">The offset.</param>
        public EffectScrollTo(int offset) : this(offset, 1, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScrollTo"/> class.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="duration">The duration.</param>
        public EffectScrollTo(int offset, decimal duration) : this(offset, duration, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectScrollTo"/> class.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        public EffectScrollTo(int offset, decimal duration, decimal delay) : base(duration, delay)
        {
            Offset = offset;
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.EffectType = "Effect.ScrollTo";
            registerEffect.AddPropertyIfTrue(Offset != 0, "offset", Offset);
        }

    }
}