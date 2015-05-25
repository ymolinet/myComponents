/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Collections.Generic;
namespace Gaia.WebWidgets
{
    /// <summary>
    /// An EffectEvent is a client side "event" dispatched by a Gaia Control or native DOM element
    /// For example the Window supports events like Appearing, Closing, Minimizing, etc and these
    /// effect events all have custom properties passed into the event arguments on the client. 
    /// To properly connect an Effect to one of these pure client side events you should add the
    /// effect to the Effect collection of the control and also use an effect event to tie them 
    /// together. The available effect events are commonly made available as static functions on the
    /// control itself. 
    /// </summary>
    /// <example>
    /// <code title="Attaching EffectEvents to Ajax Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\Various\EnhancedWindow\Default.aspx.cs" />
    /// </code>
    /// </example>
    public abstract class AjaxEffectEvent
    {
        /// <summary>
        /// The name of the function/event. For example "gaiaminimizing". The gaia prefix is
        /// used to sort out gaia specific events with native events. 
        /// </summary>
        public abstract string FunctionName { get; }
        
        /// <summary>
        /// Overridden in concrete classes to obtain the list of parameters used in this 
        /// EffectEvent.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<KeyValuePair<string, string>> GetParameters() { yield break; }
        
        /// <summary>
        /// Not all effects can be added to all events. Override this function if you need to 
        /// perform validation on the effect added. 
        /// </summary>
        /// <param name="effect"></param>
        public virtual void VerifyEffect(Effect effect) { }
    }

}