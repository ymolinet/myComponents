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
    using System.Collections.Generic;

    /// <summary>
    /// Abstract class for effects which are being constructed with child effects.
    /// EffectParallel and EffectQueue are examples of this. 
    /// </summary>
    public abstract class ScriptaculousEffectContainer : ScriptaculousEffectBase, IEffectContainer
    {
        private char _childSeparatorChar = ';';
        private List<Effect> _effects;

        /// <summary>
        /// Container Effects can contain children which needs to be separated differently. 
        /// Used by EffectParallel to comma  separate children in the constructor
        /// </summary>
        public char ChildSeparatorChar
        {
            get { return _childSeparatorChar; }
            set { _childSeparatorChar = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptaculousEffectContainer"/> class.
        /// </summary>
        /// <param name="effects">The effects.</param>
        protected ScriptaculousEffectContainer(params ScriptaculousEffectBase[] effects) { Effects.AddRange(effects); }

        /// <summary>
        /// Collection of Child Effects
        /// </summary>
        public List<Effect> Effects
        {
            get{ return _effects ?? (_effects = new List<Effect>()); }
        }
    }
}