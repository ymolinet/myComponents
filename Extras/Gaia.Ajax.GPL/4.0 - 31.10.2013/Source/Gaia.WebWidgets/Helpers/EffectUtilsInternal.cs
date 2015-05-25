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
    internal static class EffectUtilsInternal
    {
        public static string ToDelayedFunctionInvocation(Effect effect)
        {
            return string.Format("setTimeout({0}, 10);", ToFunction(effect));
        }

        public static string ToAnonymousFunction(Effect effect)
        {
            return string.Format("({0})();", ToFunction(effect)); //todo: howto invoke with arguments ??? 
        }

        public static string ToFunction(Effect effect)
        {
            return "function(e){" + ToScript(effect) + "}";
        }

        public static string ToScript(Effect effect)
        {
            return ((IEffect) effect).GetScript() + ";";
        }

        public static void RecursivelyCopyPropertyParameters(Effect parent)
        {
            var container = parent as IEffectContainer;
            if (container == null)
                return;

            foreach (var child in container.Effects)
            {
                foreach (var pair in ((IEffect)parent).PropertyParameters)
                    ((IEffect)child).PropertyParameters.Add(pair.Key, pair.Value);

                RecursivelyCopyPropertyParameters(child);
            }
        }

        public static void RecursivelySetElementId(Effect effect, string elementId)
        {
            ((IEffect) effect).ElementID = elementId;

            var container = effect as IEffectContainer;
            if (container == null)
                return;

            foreach (var childEffect in container.Effects)
            {
                ((IEffect)childEffect).ElementID = elementId;
                RecursivelySetElementId(childEffect, elementId);
            }
        }
    }
}