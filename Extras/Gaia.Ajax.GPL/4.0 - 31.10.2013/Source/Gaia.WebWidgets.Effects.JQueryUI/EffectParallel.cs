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
    using System;

    /// <summary>
    /// This is a special effect which allows to combine more than one core effect into a parallel effect. 
    /// It’s the only effect that doesn't take an element as first parameter, but an array of sub-effects.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectParallel to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectContainers\EffectParallel\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public sealed class EffectParallel : JQueryEffectContainerBase, IEffect
    {
        private const string EffectScriptSeparator = ";";

        string IEffect.GetScript()
        {
            IJQueryEffect self = this;

            string onCompleteCallback = null;
            var hasOnCompleteCallback = string.IsNullOrEmpty(self.AfterFinish);
            if (hasOnCompleteCallback)
                onCompleteCallback = self.AfterFinish;

            if (Effects.Count < 1)
            {
                if (!hasOnCompleteCallback)
                    return string.Empty;

                return onCompleteCallback + "();";
            }

            string result;
            var queue = self.Queue;

            if (queue != null && queue.UseQueue && !string.IsNullOrEmpty(queue.QueueName)) // inside sequential queue
            {
                // make the parallel effect with max duration call the next method in queue.

                decimal maxDuration = -1;
                IJQueryEffect effectWithMaxDuration = null;
                foreach (var effect in Effects)
                {
                    var uiEffect = effect as JQueryUIEffectBase;
                    if (uiEffect != null && uiEffect.Duration >= maxDuration)
                    {
                        maxDuration = uiEffect.Duration;
                        effectWithMaxDuration = uiEffect;
                    }
                }

                if (effectWithMaxDuration == null) // continue from the last effect in parallel queue
                    effectWithMaxDuration = (IJQueryEffect) Effects[Effects.Count - 1];

                effectWithMaxDuration.AfterFinish = "next";
            }

            result = string.Join(EffectScriptSeparator, Effects.ConvertAll(ConvertEffectToString).ToArray());

            if (queue == null || queue.UseQueue) // default or sequential queue
            {
                return "(function(){" + result + "})()";
            }

            // inside parallel queue
            return result;
        }

        private static string ConvertEffectToString(Effect effect)
        {
            var eff = effect as IJQueryEffect;
            if (eff == null)
                throw new ApplicationException(
                    "Only effects of type IJQueryEffect can be added to the EffectQueue.Effects collection");

            eff.Queue = new JQueryQueueDetails(false);
            return eff.GetScript();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectQueue"/> class.
        /// </summary>
        /// <param name="effects">The effects.</param>
        public EffectParallel(params Effect[] effects)
        {
            Effects.AddRange(effects);
        }
    }
}