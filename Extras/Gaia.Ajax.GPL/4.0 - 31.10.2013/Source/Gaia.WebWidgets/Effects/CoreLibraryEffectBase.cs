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
    /// JQuery core defines a few "Effects" like show, hide, toggle which is implemented in the core library
    /// </summary>
    public abstract class CoreLibraryEffectBase : Effect, IEffect
    {
        string IEffect.GetScript()
        {
            return string.Concat("jQuery('#' + ", GetElementReference(), ").", GetElementMethod(), "()");
        }

        /// <summary>
        /// Override this to provide a custom function name for this Prototype effect. 
        /// Examples include "show", "hide", "toggle" which will serialize a JavaScript call
        /// like Element.show(this.element.id); for example
        /// </summary>
        /// <returns></returns>
        protected abstract string GetElementMethod();
    }
}