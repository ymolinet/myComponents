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
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// This is a special effect which allows to combine more than one core effect into a parallel effect. 
    /// It’s the only effect that doesn’t take an element as first parameter, but an array of subeffects.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectParallel to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectContainers\EffectParallel\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectParallel : ScriptaculousEffectContainer, IEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParallel"/> class.
        /// </summary>
        public EffectParallel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParallel"/> class.
        /// </summary>
        /// <param name="effects">The effects.</param>
        public EffectParallel(params ScriptaculousEffectBase[] effects) : base(effects) { }

        string IEffect.GetScript()
        {
            string script = @"new Effect.Parallel([";
            for (int i = 0, count = Effects.Count; i < count; i++)
            {
                IScriptaculousEffect e = Effects[i] as IScriptaculousEffect;

                if (e == null)
                    throw new ApplicationException("Only effects of type IScriptaculousEffect can be added to the EffectParallel Effects collection");

                e.Sync = true;

                if (e is ScriptaculousEffectContainer)
                    (e as ScriptaculousEffectContainer).ChildSeparatorChar = ',';

                script += e.GetScript();

                if (i != count - 1) 
                    script += ",";

            }

            //bug #xxx : When applied to an event on for example window, 
            // the children's afterFinish is not properly executen because
            // it's not in the correct context ... 

            IScriptaculousEffect effect = this;

            string queue = (effect.ScriptaculousQueue == null) ? "" : "queue:" + effect.ScriptaculousQueue.GetScript() + ", ";
            string duration = Duration == 1.0M ? "" : "duration:" + Duration.ToString("0.##", CultureInfo.InvariantCulture);
            string delay = Delay == 0 ? "" : "delay:" + Delay.ToString("0.##", CultureInfo.InvariantCulture);

            string concatParams = ConcatParams(queue, duration, delay);
            string parameters = string.IsNullOrEmpty(concatParams) ? "" : string.Format(", {{ {0} }}", concatParams);

            script += "]" + parameters + ")";

            return script;
        }


        private static string ConcatParams(params string[] parameters)
        {
            List<string> prms = new List<string>(parameters.Length);
            foreach(string p in parameters)
                if (!string.IsNullOrEmpty(p)) prms.Add(p);
            return string.Join(",", prms.ToArray());
        }
    }
}