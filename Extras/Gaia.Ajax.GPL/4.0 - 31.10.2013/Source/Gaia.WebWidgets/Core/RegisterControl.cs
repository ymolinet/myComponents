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
using System.Text;
using System.Web.UI;
using System.Collections.Generic;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Renders a clientside representation of the control with construction, options/parameters, event 
    /// observations and rendering of aspects. It has a nice syntax and allow you to do chaining of 
    /// function calls. Use this one if you create your own Ajax Extension Controls.
    /// </summary>
    public class RegisterControl : RegisterObject<RegisterControl>
    {
        #region [ -- Enums -- ]

        /// <summary>
        /// Event Enumerations for the Observe function. 
        /// </summary>
        public enum ObserveEvent
        {
            /// <summary>
            /// Click Event
            /// </summary>
            Click,

            /// <summary>
            /// Change Event
            /// </summary>
            Change,
        }

        #endregion

        #region [ -- Public Properties -- ]
        /// <summary>
        /// ClientID of the control to construct
        /// </summary>
        public string ClientId
        {
            get { return ObjectID; }
            set { ObjectID = value; }
        }

        /// <summary>
        /// Type of Control to construct
        /// </summary>
        public string ControlType
        {
            get { return ObjectType; }
            set { ObjectType = value; }
        } 
        #endregion

        #region [ -- Private Members -- ]

        private List<string> _invoke; 
        
        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// Constructor. Takes the control-type to construct and it's clientside id. 
        /// </summary>
        /// <param name="controltype">controltype (ie. Gaia.Window, Gaia.Button)</param>
        /// <param name="clientid">ClientID for the client-side construction</param>
        public RegisterControl(string controltype, string clientid) : base(controltype, clientid) { }

        /// <summary>
        /// Constructor. 
        /// </summary>
        public RegisterControl() : this("undefined", "undefined") {  }

        #endregion

        #region [ -- Public Functions -- ]

        /// <summary>
        /// Add .observe() function call to the RegisterControl script. 
        /// </summary>
        /// <param name="observeEvent">The event to be observed</param>
        /// <param name="args">optional parameteres to the observe function call</param>
        /// <returns>this</returns>
        public RegisterControl Observe(string observeEvent, params object[] args)
        {
            var hasArgs = args != null;
            var argCount = hasArgs ? args.Length : 0;
            
            var invokeArguments = new object[argCount + 1];
            invokeArguments[0] = observeEvent;

            if (hasArgs && argCount > 0)
                args.CopyTo(invokeArguments, 1);

            return Invoke("observe", invokeArguments);
        }

        /// <summary>
        /// Generates a function call invocation like ie ( .setAutoPostBack('true') );
        /// If you specify addQuotes = false, then the invocation will look like ( .setAutoPostBack(true) );
        /// </summary>
        /// <param name="function">Function name to invoke</param>
        /// <param name="args">the arguments passed to the function</param>
        /// <param name="addQuotes">If you want to remove the quotes around the parameter </param>
        /// <returns>itself</returns>
        public RegisterControl Invoke(string function, bool addQuotes, params object[] args)
        {
            if (_invoke == null)
                _invoke = new List<string>();

            var invoke = new StringBuilder();
            invoke.Append(".").Append(function).Append("(");

            if (args != null)
            {
                for (var i = 0; i < args.Length; ++i)
                {
                    if (args[i] is Array) // arrays are rendered as System.Object[] so we omit those
                        continue;

                    if (i != 0) invoke.Append(", ");

                    if (addQuotes)
                        invoke.Append("'");

                    invoke.Append(args[i].ToString());

                    if (addQuotes)
                        invoke.Append("'");
                }
            }

            invoke.Append(")");
            _invoke.Add(invoke.ToString());
            return this;
        }

        /// <summary>
        /// Generates a function call invocation like ie ( .setAutoPostBack('true') );
        /// If you want to remove the quotes, please use the overload with addQuotes to false instead
        /// </summary>
        /// <param name="function">Function name to invoke</param>
        /// <param name="args">the arguments passed to the function</param>
        /// <returns>itself</returns>
        public RegisterControl Invoke(string function, params object[] args)
        {
            return Invoke(function, true, args);
        }

        /// <summary>
        /// Generates a function call invocation like ie ( .setAutoPostBack('true') );
        /// You can ommit the quotes by specifying addquotes = false. Also the function argument
        /// is only returned if the first eval parameter evaluates to true. 
        /// </summary>
        /// <param name="eval">Boolean Evaluation</param>
        /// <param name="function">Function name to invoke</param>
        /// <param name="addQuotes">Wheter to add quotes around the parameters</param>
        /// <param name="args">the arguments passed to the function</param>
        /// <returns>itself</returns>
        public RegisterControl InvokeIf(bool eval, string function, bool addQuotes, params object[] args)
        {
            return eval ? Invoke(function, addQuotes, args) : this;
        }

        /// <summary>
        /// Add .observe() function call to the RegisterControl script. 
        /// </summary>
        /// <param name="evt">The event to be observed</param>
        /// <param name="args">optional parameteres to the observe function call</param>
        /// <returns>this</returns>
        public RegisterControl Observe(ObserveEvent evt, params object[] args)
        {
            return Observe(evt.ToString().ToLowerInvariant(), args);
        }

        private static void RenderAspects(StringBuilder builder, ICollection<IAspect> aspects)
        {
            if (aspects.Count <= 0) return;
            var scripts = new List<string>(aspects.Count);

            foreach (var aspect in aspects)
                scripts.Add(aspect.GetScript());

            builder.Append(
                string.Concat("aspects:[", string.Join(",", scripts.ToArray()), "]"));
        }

        /// <summary>
        /// Add Aspect Collection to the RegisterControl script
        /// </summary>
        /// <param name="aspects">List of Aspects</param>
        /// <returns>this</returns>
        public RegisterControl AddAspects(AspectCollection aspects)
        {
            var sb = new StringBuilder();
            RenderAspects(sb, aspects);
            var aspectRegistration = sb.ToString();

            return !string.IsNullOrEmpty(aspectRegistration) ? AddProperty(aspectRegistration) : this;
        }

        /// <summary>
        /// Will add effects that will be rendered as properties to the control. 
        /// </summary>
        /// <param name="effects"></param>
        /// <returns></returns>
        public RegisterControl AddEffects(EffectCollection effects)
        {
            if (effects.Functions.Count == 0)
                return this;

            var effectRegistration = "{";
            var first = true;
            foreach (var pair in effects.Functions)
            {
                if (!first)
                    effectRegistration += ",";

                first = false;

                // setting elementid to null will make sure we use Event.element(e).id instead of ClientId
                // which may change during DRIMR operations.
                EffectUtilsInternal.RecursivelySetElementId(pair.Value, null);
                effectRegistration += string.Format("\"{0}\":{1}", pair.Key, EffectUtilsInternal.ToFunction(pair.Value));
            }

            effectRegistration += "}";
            AddProperty("effects", effectRegistration, false);

            return this;
        }

        /// <summary>
        /// Render the RegisterControl script that is to be sent to the client 
        /// </summary>
        /// <returns>RegisterControl script</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("$RC(new ")
               .Append(ControlType).Append("('").Append(ClientId).Append("'");

            // determine if there are properties to render
            builder.Append(Properties.Count == 0 ? "))" : ", {");

            // render the key/value properties
            for (int i = 0; i < Properties.Count; i++)
            {
                if (i != 0) builder.Append(", ");

                // if key is string.empty, we render neither the key nor the : (semicolon) 
                if (!string.IsNullOrEmpty(Properties[i].Key))
                    builder.Append(Properties[i].Key).Append(":");

                builder.Append(Properties[i].Value);
            }

            if (Properties.Count != 0)
                builder.Append("}))");

            // add observe functions 
            if (_invoke != null)
            {
                foreach (string observe in _invoke)
                    builder.Append(observe);
            }

            builder.Append(";"); // your lucky semicolon
            return builder.ToString();
        } 
        #endregion

        #region [ -- Specific registration methods -- ]

        /// <summary>
        /// Adds focus registration script and returns registration object.
        /// </summary>
        internal RegisterControl AddFocus(AjaxControl impl)
        {
            return InvokeIf(impl.Focused, "setFocus", false);
        }

        /// <summary>
        /// Adds validation registration script and returns registration object.
        /// </summary>
        internal RegisterControl AddValidation(bool causesValidation, string validationGroup)
        {
            return InvokeIf(!causesValidation, Constants.SetCausesValidationFunctionName, false, 0).
                InvokeIf(causesValidation && !string.IsNullOrEmpty(validationGroup),
                         Constants.SetValidationGroupFunctionName, true, validationGroup);
        }

        /// <summary>
        /// Adds validation registration script and returns registration object.
        /// </summary>
        internal RegisterControl AddPostBackOptions(Control owner, PostBackOptions options)
        {
            var postUrl = options.ActionUrl;
            var argument = options.Argument;
            var target = options.TargetControl == null ? owner.UniqueID : options.TargetControl.UniqueID;
            var callbackName = Utilities.GetCallbackName(target, owner.ClientID);

            return AddValidation(options.PerformValidation, options.ValidationGroup).
                InvokeIf(!string.IsNullOrEmpty(postUrl), Constants.SetPostBackUrlFunctionName, true, postUrl).
                InvokeIf(!string.IsNullOrEmpty(argument), Constants.SetArgumentFunctionName, true, argument).
                AddPropertyIfTrue(callbackName != null, "callbackName", callbackName);
        }

        /// <summary>
        /// Observe the "click" event and enables or stops bubbling based on the <paramref name="enableBubbling"/> parameter.
        /// </summary>
        internal RegisterControl ObserveClick(bool enableBubbling)
        {
            return InvokeIf(enableBubbling, "observe", false, "'click'", "1").
                InvokeIf(!enableBubbling, "observe", false, "'click'");
        }

        #endregion
    }
}
