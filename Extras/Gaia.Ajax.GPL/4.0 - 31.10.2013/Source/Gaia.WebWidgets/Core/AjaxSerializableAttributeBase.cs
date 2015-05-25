/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.Web.UI;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Abstract base class for all AjaxSerializable attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class AjaxSerializableAttributeBase : Attribute
    {
        private readonly string _jsMethodName;

        /// <summary>
        /// The constructor for the AjaxSerializableAttribute
        /// </summary>
        /// <param name="jsMethodName">The name of the JS client-side method that implements the property setter</param>
        protected AjaxSerializableAttributeBase(string jsMethodName)
        {
            _jsMethodName = jsMethodName;
        }

        /// <summary>
        /// Returns the JS client-side property setter name
        /// </summary>
        public string JSMethodName
        {
            get { return _jsMethodName; }
        }

        /// <summary>
        /// Called by the Gaia framework to format the provided value of the property of the provided control.
        /// </summary>
        /// <param name="control">Property owner</param>
        /// <param name="value">Value of the property to format.</param>
        /// <returns>The formatted value of the control's property.</returns>
        protected internal virtual object FormatValue(Control control, object value)
        {
            return value;
        }

        /// <summary>
        /// Returns true if the property value should be optimized.
        /// </summary>
        internal virtual bool OptimizeValue
        {
            get { return false; }
        }
    }
}