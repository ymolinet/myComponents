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
    /// Base class for all Scriptaculous effects
    /// </summary>
    public abstract class ScriptaculousEffectBase : Effect, IScriptaculousEffect
    {
        private ScriptaculousTransitions _transition = ScriptaculousTransitions.Sinoidal;

        bool IScriptaculousEffect.Sync { get; set; }
        string IScriptaculousEffect.AfterUpdate { get; set; }
        string IScriptaculousEffect.AfterFinish { get; set; }
        ScriptaculousQueueDetails IScriptaculousEffect.ScriptaculousQueue { get; set; }

        decimal _duration = 1.0M;

        /// <summary>
        /// Set the lifetime of the Parallel execution
        /// </summary>
        public decimal Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptaculousEffectBase"/> class.
        /// </summary>
        protected ScriptaculousEffectBase() { }

        /// <summary>
        /// Constructor. Specify Duration and Delay
        /// </summary>
        /// <param name="duration">The duration </param>
        /// <param name="delay"></param>
        protected ScriptaculousEffectBase(decimal duration, decimal delay)
        {
            Delay = delay;
            _duration = duration;
        }

        /// <summary>
        /// Set the delay before the Parallel execution commence. 
        /// </summary>
        public decimal Delay { get; set; }

        /// <summary>
        /// Includes the script files.
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

#if DEBUG
            // Include ScriptAculous Effects here since this script file contains all effects ...
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Effects.Scripts.prototype.js", typeof(ScriptaculousEffectBase), "Prototype.browserFinishedLoading");         
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Effects.Scripts.effects.js", typeof(ScriptaculousEffectBase), "Effect.browserFinishedLoading");         
#elif RELEASE
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Effects.Scripts.effects.bundle.js", typeof (ScriptaculousEffectBase), "");
#endif
        }

        /// <summary>
        /// Sets a function that modifies the current point of the animation, which is between 0 and 1. Following transitions are supplied:
        /// </summary>
        public ScriptaculousTransitions Transition
        {
            get { return _transition; }
            set
            {
                    _transition = value;
            }
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);

            registerEffect.EnableEffectConstructionCompatibility = true;

            IScriptaculousEffect effect = this;

            registerEffect.AddPropertyIfTrue(Duration != 1.0M, "duration", Duration);
            registerEffect.AddPropertyIfTrue(Delay != 0.0M, "delay", Delay);

            registerEffect.AddPropertyIfTrue(effect.Sync, "sync", true) // false is default     
                .AddPropertyIfTrue(Transition != ScriptaculousTransitions.Sinoidal, "transition",
                                   "Effect.Transitions." + Transition.ToString().ToLower(), false);

            registerEffect.AddPropertyIfTrue(!string.IsNullOrEmpty(effect.AfterUpdate), "afterUpdate",effect.AfterUpdate, false);
            registerEffect.AddPropertyIfTrue(!string.IsNullOrEmpty(effect.AfterFinish), "afterFinish", effect.AfterFinish, false);

            if (effect.ScriptaculousQueue != null)
                registerEffect.AddProperty("queue", effect.ScriptaculousQueue.GetScript(), false);

        }

    }
}