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
    using System.Collections.Generic;

    /// <summary>
    /// Denotes queue of <see cref="IJQueryEffect"/>s.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectQueue to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectContainers\EffectQueue\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectQueue : JQueryEffectContainerBase, IEffect
    {
        private const string EffectScriptSeparator = ",";

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

            if (queue != null && queue.UseQueue && !string.IsNullOrEmpty(queue.QueueName)) // run sequentially inside another queue
            {
                result = CombineEffectScripts(Effects);
                if (hasOnCompleteCallback)
                    result += EffectScriptSeparator + onCompleteCallback;

                return result;
            }

            // use default queue or run parallel

            var effects = Effects;
            var first = effects[0] as IJQueryEffect;
            if (first == null)
                throw new ApplicationException(
                    "Only effects of type IJQueryEffect can be added to the EffectQueue.Effects collection");

            if (queue != null && !queue.UseQueue) // run parallel, make the first one start in parallel, and then continue sequential
                first.Queue = new JQueryQueueDetails(false);

            first.AfterFinish = "next";
            result = @"jQuery({}).queue('q',[" + GetWrappedScript(first);

            if (effects.Count > 1)
            {
                effects = effects.GetRange(1, effects.Count - 1);
                result += EffectScriptSeparator + CombineEffectScripts(effects);
            }

            if (hasOnCompleteCallback)
                result += EffectScriptSeparator + onCompleteCallback;

            result += "]).dequeue('q');";
            return result;
        }

        private static string CombineEffectScripts(List<Effect> effects)
        {
            var scripts = string.Join(EffectScriptSeparator, effects.ConvertAll(GetEffectScript).ToArray());
            return scripts;
        }

        private static string GetEffectScript(Effect effect)
        {
            var eff = effect as IJQueryEffect;
            if (eff == null)
                throw new ApplicationException(
                    "Only effects of type IJQueryEffect can be added to the EffectQueue.Effects collection");

            var container = eff as IEffectContainer;
            eff.Queue = container != null ? new JQueryQueueDetails("q") : null;

            eff.AfterFinish = "next";
            return GetWrappedScript(eff);
        }

        private static string GetWrappedScript(IEffect effect)
        {
            return "function(next){" + effect.GetScript() + "}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectQueue"/> class.
        /// </summary>
        /// <param name="effects">The effects.</param>
        public EffectQueue(params Effect[] effects)
        {
            ((IEffectContainer) this).Effects.AddRange(effects);
        }
    }
}
