/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Contains Utility methods for working with Effects in your application. 
    /// </summary>
    public static class EffectUtils
    {
        /// <summary>
        /// This Function will apply a postfix to the ElementID which is retrieved from the the AjaxControl the effect is attached to.
        /// This is a useful function if you need to apply an Effect to the Body of a Control instead of the entire control itself. Examples
        /// include Window and ExtendedPanel which define their own content area.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childId"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static T AppendElementID<T>(string childId, T effect) where T : Effect
        {
            effect.AppendID = childId;

            var effectContainer = effect as IEffectContainer;
            if (effectContainer != null)
            {
                foreach (var child in effectContainer.Effects)
                    AppendElementID(childId, (T)child);
            }

            return effect;
        }
    }
}