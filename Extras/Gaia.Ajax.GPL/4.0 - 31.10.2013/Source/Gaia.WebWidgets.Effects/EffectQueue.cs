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
    using System;

    /// <summary>
    /// Summary description for EffectQueue
    /// </summary>
    /// <example>
    /// <code title="Adding EffectQueue to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectContainers\EffectQueue\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectQueue : ScriptaculousEffectContainer, IEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectQueue"/> class.
        /// </summary>
        public EffectQueue() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectQueue"/> class.
        /// </summary>
        /// <param name="effects">The effects.</param>
        public EffectQueue(params ScriptaculousEffectBase[] effects) : base(effects) { }

        string IEffect.GetScript()
        {
            string script = string.Empty;
            string scope = Guid.NewGuid().ToString().Substring(0, 5);
        
            for (int i = 0; i < Effects.Count; i++)
            {
                IScriptaculousEffect effect = Effects[i] as IScriptaculousEffect;

                if (effect == null)
                    throw new ApplicationException("Only effects of type IScriptaculousEffect can be added to the EffectQueue Effects collection");

                InitQueueRecursive(effect, scope);
                script += effect.GetScript();
                if (i != Effects.Count - 1)
                    script += ChildSeparatorChar; 
            }

            return script;

        }


        private static void InitQueueRecursive(IScriptaculousEffect effect, string scope)
        {
            if (effect.ScriptaculousQueue == null)
                effect.ScriptaculousQueue = new ScriptaculousQueueDetails();

            if (string.IsNullOrEmpty(effect.ScriptaculousQueue.Scope))
                effect.ScriptaculousQueue.Scope = scope;

            EffectQueue effectQueue = effect as EffectQueue;
            if (effectQueue == null) 
                return;

            foreach (ScriptaculousEffectBase childEffect in effectQueue.Effects)
                InitQueueRecursive(childEffect, scope);
            
        }
    }
}