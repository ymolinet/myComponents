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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base class for Effects. 
    /// note: This class has not been marked abstract for backwards compatibility. Ideally you should not use the ctor, but rather construct
    /// each individual effect descendant directly. For example new EffectShow, new EffectHide, 
    /// </summary>
    public partial class Effect : IEffect
    {
        private const string ObsoleteText = @"This constructor is obsolete. The Effect API was changed in Gaia Ajax 3.6 and you should use the
Effects found in the Gaia.WebWidgets.Effects assembly and add the Effects you need to the Effects Collection of the Control. If you need to 
apply effects to non Gaia Controls you can use the static Effect.Show method and pass in the clientID there. ";

        /// <summary>
        /// AppendID is appended to the ElementID and is useful if  you need to apply an effect to an inner element.
        /// </summary>
        protected internal string AppendID { get; set; }

        string IEffect.ElementID { get; set; }

        string IEffect.GetScript()
        {
            var registerEffect = new RegisterEffect();
            registerEffect.SetInputParameters(((IEffect) this).PropertyParameters);
            PopulateProperties(registerEffect);
            return registerEffect.ToString();
        }

        private Dictionary<string, string> _propertyParameters;

        Dictionary<string, string> IEffect.PropertyParameters
        {
            get { return _propertyParameters ?? (_propertyParameters = new Dictionary<string, string>()); }
        }

        /// <summary>
        /// Override this function to automatically add custom properties to your derivative control/effect
        /// </summary>
        /// <param name="registerEffect">the </param>
        protected virtual void PopulateProperties(RegisterEffect registerEffect)
        {
            if (registerEffect == null)
                throw new ArgumentNullException("registerEffect");

            registerEffect.ElementID = ((IEffect) this).ElementID;
            registerEffect.AppendID = AppendID;
            registerEffect.EnableEffectConstructionCompatibility = false;
        }

        /// <summary>
        /// Override in derived classes to include custom javascript files for 
        /// automatic inclusion.
        /// </summary>
        protected virtual void IncludeScriptFiles() {}
        
        void IEffect.IncludeScriptFiles()
        {
            IncludeScriptFiles();
        }

        internal bool HasIncludedScriptFiles;

        /// <summary>
        /// Returns the id of the associated DOM element or
        /// client-side script which returns the id of the associated DOM element.
        /// </summary>
        public string GetElementReference()
        {
            var appendid = AppendID;
            var elementid = ((IEffect) this).ElementID;
            var shouldAppend = !string.IsNullOrEmpty(appendid);
            
            if (string.IsNullOrEmpty(elementid))
            {
                const string referenceScript = "jQuery(e.target).attr('id')";
                return shouldAppend ? string.Concat(referenceScript, "+'", appendid,"'") : referenceScript;
            }

            return string.Concat("'", elementid, appendid, "'");
        }

        /// <summary>
        /// Utilize an effect on an element 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="effect"></param>
        public static void Show(string elementId, Effect effect)
        {
            // We cannot successfully attach the scripts if it's too early in the lifecycle
            if (Manager.Instance.Page == null)
                return;

            // Write the EffectScript
            EffectUtilsInternal.RecursivelySetElementId(effect, elementId);
            Manager.Instance.AddScriptForClientSideEval(EffectUtilsInternal.ToDelayedFunctionInvocation(effect));

            // if the effect is added in a "fire and forget" fashion in a callback we need to make
            // sure that the effect includes its related script files
            if (effect.HasIncludedScriptFiles) 
                return;

            // If it's not an ajax callback we assume that the page is fully reloaded and we must attach the effect scripts after
            // prerender has fired. If not we can assume that everything is in place. 
            if (!Manager.Instance.IsAjaxCallback)
                Manager.Instance.Page.PreRenderComplete += delegate { IncludeScriptFilesForEffect(effect); };
            else
                IncludeScriptFilesForEffect(effect);
        }

        private static void IncludeScriptFilesForEffect(Effect effect)
        {
            effect.HasIncludedScriptFiles = true;
            (effect as IEffect).IncludeScriptFiles();
        }
    }
}