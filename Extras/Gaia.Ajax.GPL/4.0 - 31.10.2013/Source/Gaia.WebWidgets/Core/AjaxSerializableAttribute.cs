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
    /// <summary>
    /// Attribute class for telling the Gaia Core Runtime that a property is serializable. Usage is to submit the
    /// JavaScript "setter method" as JSMethodName part of the Attribute. Then when the property is changed
    /// during an Ajax Callback the framework itself will handle serializing the new value back to the client.
    /// </summary>
    /// <example>
    /// <code title="Using the AjaxSerializableAttribute attribute" lang="C#"> 
    /// <code source="..\..\src\Gaia.WebWidgets.Extensions\Widgets\ExtendedButton.cs" region="[ -- Properties -- ]" />
    /// </code>
    /// </example>
    public sealed class AjaxSerializableAttribute : AjaxSerializableAttributeBase
    {
        private readonly bool _optimizeValue;

        /// <summary>
        /// Initializes new instance of the <see cref="AjaxSerializableAttribute"/> class.
        /// </summary>
        /// <param name="jsMethodName">The name of the JS client-side method that implements the property setter.</param>
        /// <param name="optimizeValue">True to optimize the property value if supported.</param>
        public AjaxSerializableAttribute(string jsMethodName, bool optimizeValue = false) : base(jsMethodName)
        {
            _optimizeValue = optimizeValue;
        }

        /// <summary>
        /// Returns true if the property value should be optimized.
        /// </summary>
        internal override bool OptimizeValue
        {
            get { return _optimizeValue; }
        }
    }
}
