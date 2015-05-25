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
    /// Base class for all scriptaculous effects with from and to intervals
    /// </summary>
    public abstract class ScriptaculousEffectWithIntervalBase : ScriptaculousEffectBase
    {
        private decimal _from;
        private decimal _to = 1.0M;

        /// <summary>
        /// Sets the starting point of the transition, a float between 0.0 and 1.0. Defaults to 0.0.
        /// </summary>
        public decimal From
        {
            get { return _from; }
            set { _from = value; }
        }

        /// <summary>
        /// Sets the end point of the transition, a float between 0.0 and 1.0. Defaults to 1.0.
        /// </summary>
        public decimal To
        {
            get { return _to; }
            set { _to = value; }
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);

            registerEffect.AddPropertyIfTrue(From != 0.0M, "from", From);
            registerEffect.AddPropertyIfTrue(To != 1.0M, "to", To);
        }

    }
}