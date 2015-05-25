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
    using System.Web.UI;
    
    /// <summary>
    /// Composite builder block for Ajax Effect Controls keeping track of the Effects collection. Don't forget to forward calls to the
    /// OnPreRender method() for script inclusion. 
    /// </summary>
    public class EffectControl
    {
        private readonly Control _owner;
        private EffectCollection _effects;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">Owner Control</param>
        public EffectControl(Control owner) { _owner = owner; }

        /// <summary>
        /// Effect Collection
        /// </summary>
        public EffectCollection Effects
        {
            get { return _effects ?? (_effects = new EffectCollection()); }
        }

        /// <summary>
        /// Forward calls to this method from the OnPreRender() in your control. It will make sure that required scripts are
        /// added and that custom effects in callbacks are being executed properly. 
        /// </summary>
        public void OnPreRender()
        {
            foreach (var effect in Effects.Effects)
            {
                ((IEffect)effect).IncludeScriptFiles();

                // note : this sets the id properly, but we execute the effect twice since it is also rendered as part of the control ... 
                // bug : find another place to put this ... 
                EffectUtilsInternal.RecursivelySetElementId(effect, _owner.ClientID);
                Manager.Instance.AddScriptForClientSideEval(EffectUtilsInternal.ToDelayedFunctionInvocation(effect));
            }

            // Effects can be added as functions and we still need to include their required script files
            foreach (var effect in Effects.Functions.Values)
                ((IEffect) effect).IncludeScriptFiles();
        }

    }
}
