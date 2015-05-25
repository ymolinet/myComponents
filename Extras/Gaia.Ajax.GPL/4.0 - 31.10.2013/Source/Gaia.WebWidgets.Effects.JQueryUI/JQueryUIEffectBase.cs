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
    using System.Globalization;

    /// <summary>
    /// Common ancestor for all JQuery effects. 
    /// </summary>
    public abstract class JQueryUIEffectBase : Effect, IJQueryEffect
    {
        #region [ -- Private Members -- ]

        private decimal _duration = DefaultDuration;
        private Easing _easing = Easing.Linear;
        protected const decimal DefaultDuration = 1.0M;
        private EffectMethod _method = EffectMethod.Effect;

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// The name of the effect implemented in jQuery
        /// </summary>
        protected abstract string EffectType { get; }

        /// <summary>
        /// The method that is used to "invoke" the effect. 
        /// </summary>
        public EffectMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }

        /// <summary>
        /// Set the lifetime of the effect
        /// </summary>
        public decimal Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        /// <summary>
        /// Delay before effect is executed. 
        /// </summary>
        public decimal Delay { get; set; }

        /// <summary>
        /// The "easing" applied to the effect. 
        /// </summary>
        public Easing Easing
        {
            get { return _easing; }
            set { _easing = value; }
        }

        #endregion

        /// <summary>
        /// Override this function to automatically add custom properties to your derivative control/effect
        /// </summary>
        /// <param name="registerEffect">the </param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);

            registerEffect.Delay = Delay;
            registerEffect.EffectMethod = Method.ToString().ToLowerInvariant();

            const decimal jqueryDefaultDuration = 0.4M;

            IJQueryEffect self = this;
            var queue = self.Queue;

            registerEffect.AddPropertyIfTrue(!string.IsNullOrEmpty(EffectType), "effect", EffectType)
                          .AddPropertyIfTrue(Duration != jqueryDefaultDuration, "duration", Duration*1000)
                          .AddPropertyIfTrue(Easing != Easing.Linear, "easing", ToCamelCase(Easing.ToString()))
                          .AddPropertyIfTrue(!string.IsNullOrEmpty(self.AfterFinish), "complete", self.AfterFinish, false)
                          .AddPropertyIfTrue(!string.IsNullOrEmpty(self.AfterUpdate), "step", self.AfterUpdate, false);


            // bug: it seems that only effects that use effect() break when using queues.
            if (queue != null && Method != EffectMethod.Effect)
            {
                registerEffect.AddPropertyIfTrue(!queue.UseQueue, "queue", false)
                              .AddPropertyIfTrue(queue.UseQueue && queue.QueueName != null, "queue", queue.QueueName);
            }
        }

        protected override void IncludeScriptFiles()
        {
            Manager.Instance.AddInclusionOfFileFromResource(
                "Gaia.WebWidgets.Effects.JQueryUI.Scripts.jquery-ui.effects.js", typeof (JQueryUIEffectBase), "");
        }


        private static string ToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var camelCased = input[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            if (input.Length == 1)
                return camelCased;

            return camelCased + input.Substring(1, input.Length - 1);
        }

        JQueryQueueDetails IJQueryEffect.Queue { get; set; }

        string IJQueryEffect.AfterUpdate { get; set; }
        string IJQueryEffect.AfterFinish { get; set; }
    }
}
