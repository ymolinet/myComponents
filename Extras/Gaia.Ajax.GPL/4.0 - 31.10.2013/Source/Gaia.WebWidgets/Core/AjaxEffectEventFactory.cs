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
using System.Collections.Generic;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// The AjaxEffectEvent factory is used to create common Effect events where mostly
    /// the functionName and the afterFinish parameter is in use. This allows you to 
    /// more easily create required Effect Events. 
    /// </summary>
    /// <example>
    /// <code title="Using AjaxEffectEventFactory" lang="C#">
    /// <code source="..\..\src\Gaia.WebWidgets.Extensions\Widgets\TreeViewItem.cs" region="[ -- Effect Events -- ]" />
    /// </code>
    /// </example>
    public static class AjaxEffectEventFactory
    {
        ///<summary>
        /// Creates a simple EffectEvent with only a function name for the event. For example "gaiaminimizing"
        ///</summary>
        public static AjaxEffectEvent Create(string functionName)
        {
            return new SimpleAjaxEffectEvent(functionName);
        }

        /// <summary>
        /// Creates an EffectEvent handler with automatic afterFinish as parameter to the 
        /// event arguments. Useful for some Events which require custom code after the
        /// effect is finished. 
        /// </summary>
        public static AjaxEffectEvent CreateWithAfterFinishParameter(string functionName)
        {
            return new WithAfterFinishParam(functionName);
        }

        private class SimpleAjaxEffectEvent : AjaxEffectEvent
        {
            private readonly string _functionName = String.Empty;
            public override string FunctionName { get { return _functionName; } }
            public SimpleAjaxEffectEvent(string functionName)
            {
                _functionName = functionName;
            }
        }

        private class WithAfterFinishParam : SimpleAjaxEffectEvent
        {
            public WithAfterFinishParam(string functionName) : base(functionName) {}
            public override IEnumerable<KeyValuePair<string, string>> GetParameters()
            {
                yield return new KeyValuePair<string, string>("afterFinish", "afterFinish");
            }
        }
    }


}
