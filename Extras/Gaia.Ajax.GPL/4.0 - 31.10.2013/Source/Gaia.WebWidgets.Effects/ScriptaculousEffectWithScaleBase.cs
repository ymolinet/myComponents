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
    /// Base class for all scriptaculous effects which includes scaling
    /// </summary>
    public abstract class ScriptaculousEffectWithScaleBase: ScriptaculousEffectBase
    {
        private bool _scaleX = true;
        private bool _scaleY = true;
        private bool _scaleContent = true;
        private decimal _scaleFrom = 100M;
        private string _scaleMode = "box";
        
        /// <summary>
        /// Either ‘box’ (default, scales the visible area of the element) or ‘contents’ (scales the complete element, that is parts normally only visible byscrolling are taken into account). You can also precisely control the size the element will become by assigning the originalHeight and originalWidth variables to scaleMode. Example: scaleMode: { originalHeight: 400, originalWidth: 200 }
        /// </summary>
        public string ScaleMode
        {
            get { return _scaleMode; }
            set { _scaleMode = value; }
        }

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
        /// Sets whether content scaling should be enabled, defaults to true. 
        /// </summary>
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
        public decimal ScaleFrom
        {
            get { return _scaleFrom; }
            set { _scaleFrom = value; }
        }

    }
}