/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Web.UI;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Apply attribute to the property the value of which needs to be formatted as an Url.
    /// The value will be formatted using Control.ResolveClientUrl method.
    /// </summary>
    public sealed class AjaxSerializableUrlAttribute : AjaxSerializableAttributeBase
    {
        /// <summary>
        /// The constructor for the AjaxSerializableUrlAttribute.
        /// Uses Control.ResolveClientUrl method to process the value.
        /// </summary>
        /// <param name="jsMethodName">The name of the JS client-side method that implements the property setter</param>
        public AjaxSerializableUrlAttribute(string jsMethodName) : base(jsMethodName)
        {
        }

        /// <summary>
        /// Called by the Gaia framework to format the provided value of the property of the provided control.
        /// </summary>
        /// <param name="control">Property owner</param>
        /// <param name="value">Value of the property to format.</param>
        /// <returns>The formatted value of the control's property.</returns>
        protected internal override object FormatValue(Control control, object value)
        {
            return value == null ? base.FormatValue(control, null) : control.ResolveClientUrl(value.ToString());
        }

        /// <summary>
        /// Returns true if the property value should be optimized.
        /// </summary>
        internal override bool OptimizeValue
        {
            get { return true; }
        }
    }
}
