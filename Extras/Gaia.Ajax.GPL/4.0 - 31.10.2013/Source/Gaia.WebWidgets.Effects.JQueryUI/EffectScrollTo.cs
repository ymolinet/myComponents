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
    /// Scrolls to a specific place in the page.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectScrollTo to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectScrollTo\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectScrollTo :JQueryUIEffectBase
    {
        /// <summary>
        /// Offset
        /// </summary>
        public int Offset { get; set; }

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
        public EffectScrollTo(int offset, decimal duration, decimal delay)
        {
            Duration = duration;
            Delay = delay;
            Offset = offset;
        }

        protected override string EffectType
        {
            get { return string.Empty; }
        }

        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Effects.JQueryUI.Scripts.jquery.scrollTo.js", typeof(EffectScrollTo), "jQueryScrollTo_browserFinishedLoading");         
        }

        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.EffectMethod = "scrollTo";
            registerEffect.UseJQueryStatic = true;
            registerEffect.AddParam("'#' + " + GetElementReference());
            registerEffect.AddPropertyIfTrue(Offset != 0, "offset", Offset.ToString(CultureInfo.InvariantCulture), false);
        }

    }
}