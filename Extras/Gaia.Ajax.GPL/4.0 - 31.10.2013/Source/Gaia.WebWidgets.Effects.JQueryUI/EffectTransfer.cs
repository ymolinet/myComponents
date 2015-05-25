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
    /// <summary>
    /// Transfers the outline of an element to another element. Very useful when trying to visualize interaction between two elements.
    /// The transfer element itself has the class name "ui-effects-transfer", and needs to be styled by you, for example by adding a background or border. 
    /// </summary>
    public class EffectTransfer : JQueryUIEffectBase
    {
        /// <summary>
        /// TargetID of the element to transfer to
        /// </summary>
        public string TargetID { get; set; }

        /// <summary>
        /// Optional class name the transfer element will receive.
        /// </summary>
        public string CssClass { get; set; }

        protected override string EffectType
        {
            get { return "transfer"; }
        }

        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.AddPropertyIfTrue(!string.IsNullOrEmpty(TargetID), "to", "#" + TargetID);
            registerEffect.AddPropertyIfTrue(!string.IsNullOrEmpty(CssClass), "className", CssClass);
        }
    }
}