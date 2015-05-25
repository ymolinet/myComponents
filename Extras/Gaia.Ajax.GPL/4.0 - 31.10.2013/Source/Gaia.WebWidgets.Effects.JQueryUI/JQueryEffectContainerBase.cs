namespace Gaia.WebWidgets.Effects
{
    using System.Collections.Generic;

    public abstract class JQueryEffectContainerBase : Effect, IJQueryEffect, IEffectContainer
    {
        private List<Effect> _effects;

        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            foreach (IEffect effect in Effects)
                effect.IncludeScriptFiles();
        }

        /// <summary>
        /// Collection of Child Effects
        /// </summary>
        public List<Effect> Effects
        {
            get { return _effects ?? (_effects = new List<Effect>()); }
        }

        JQueryQueueDetails IJQueryEffect.Queue { get; set; }

        string IJQueryEffect.AfterUpdate { get; set; }
        string IJQueryEffect.AfterFinish { get; set; }
    }
}