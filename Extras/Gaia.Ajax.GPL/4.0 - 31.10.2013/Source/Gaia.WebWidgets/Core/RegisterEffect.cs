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
    using System.Collections.Generic;
    using System.Text;
    
    /// <summary>
    /// Script Builder for Effects. Simplifies generating error free JavaScript code for use in Gaia
    /// </summary>
    public class RegisterEffect : RegisterObject<RegisterEffect>
    {
        #region [ -- Private Members -- ]

        private Dictionary<string, string> _inputParameters;
        private readonly List<string> _params = new List<string>(5);

        /// <summary>
        /// Pass in a reference to to collection of parameters that will be applied to the RegisterEffect. This allows properties
        /// to be parametrized when used in a function.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public RegisterEffect SetInputParameters(Dictionary<string, string> parameters)
        {
            _inputParameters = parameters;
            return this;
        }

        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// Constructor. Takes the control-type to construct and its ClientID. 
        /// </summary>
        /// <param name="effectType">controltype (ie. Effect.Show, Effect.Appear)</param>
        /// <param name="elementId">ClientID for the client-side construction</param>
        public RegisterEffect(string effectType, string elementId) : base(effectType, elementId) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterEffect() : this(string.Empty, string.Empty) { }

        /// <summary>
        /// Typeof Effect. For example ( Effect.Appear ) 
        /// </summary>
        public string EffectType
        {
            get { return ObjectType; }
            set { ObjectType = value; }
        }

        /// <summary>
        /// The Element / Object for which to apply the Effect
        /// </summary>
        public string ElementID
        {
            get { return ObjectID; }
            set { ObjectID = value; }
        }

        /// <summary>
        /// AppendID is the appended childnode ID if ApplyToContent() function
        /// was used. AppendID is an internal concept abstracted away in the 
        /// ApplyToContent function. 
        /// </summary>
        internal string AppendID { get; set; }

        /// <summary>
        /// When true will use the old construction mechanism used in Scriptaculous effects in Gaia 3.6 and 3.7
        /// </summary>
        public bool EnableEffectConstructionCompatibility { get; set; }

        /// <summary>
        /// When true will use JQuery.method instead of JQuery(selector).method
        /// </summary>
        public bool UseJQueryStatic { get; set; }

        /// <summary>
        /// defaults to effect. alternatives are show, hide, toggle
        /// </summary>
        public string EffectMethod { get; set; }

        /// <summary>
        /// Delay before Effect is executed. 
        /// </summary>
        public decimal Delay { get; set; }

        #endregion

        #region [ -- Public Methods -- ]

        /// <summary>
        /// Add a simple parameter value that will go directly after construction. For example ( new X('id', paramGoesHere, ....
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public RegisterEffect AddParam(string param)
        {
            _params.Add(param);
            return this;
        }

        /// <summary>
        /// Returns the ElementID based on AppendString and Parameters. Various results can be
        /// 'elementonly',  'element_child', e.target.id + '_children'
        /// based on usage ... 
        /// </summary>
        /// <returns></returns>
        protected string GetElementID()
        {
            var containsElementParameter = _inputParameters != null && _inputParameters.ContainsKey("element");
            if (containsElementParameter)
            {
                var hasAppend = !string.IsNullOrEmpty(AppendID);
                var appendString = (hasAppend ? "+ '" + AppendID + "'" : string.Empty);
                return _inputParameters["element"] + appendString;
            }
            
            return "'" + ElementID + AppendID + "'";
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            if (EnableEffectConstructionCompatibility)
            {
                sb.Append(string.Concat("new ", EffectType, "(", GetElementID()));
            }
            else
            {
                if (UseJQueryStatic)
                {
                    sb.Append("jQuery.");
                }
                else
                {
                    sb.Append("jQuery('#' + ").Append(GetElementID()).Append(").");
                }
                
                // delay is not a parameter like in scriptaculus, but we can mimick it by calling delay as a method
                // due to the function queue semantics in jQuery. also delay is not supported on jquery static
                if (!UseJQueryStatic && Delay > 0)
                    sb.Append("delay(").Append(Delay*1000).Append(").");

                sb.Append(EffectMethod).Append("(");
            }
            
            for (var i = 0; i < _params.Count; i++)
            {
                sb.Append(_params[i] + ", ");
            }

            // determine if there are properties to render
            var hasParams = Properties.Count + (_inputParameters != null ? _inputParameters.Count : 0) > 0;

            if (EnableEffectConstructionCompatibility)
            {
                sb.Append(hasParams ? ", {" : ")");
            }
            else
            {
                sb.Append(hasParams ? "{" : ")");
            }

            var first = true;

            // render the key/value properties
            for (var i = 0; i < Properties.Count; i++)
            {
                if (!first) sb.Append(", ");
                first = false;

                // if key is string.empty, we render neither the key nor the : (semicolon) 
                string key = Properties[i].Key;
                if (!string.IsNullOrEmpty(key))
                    sb.Append(key).Append(":").Append(Properties[i].Value);
                else
                    sb.Append(Properties[i].Value);
            }

            // Here we render the property parameters that are defined on the Effect
            // Parameters are only rendered if an existing property is not defined
            // This allows the user to override the parameters with custom values when needed.
            // For example override the duration, coordinates, etc. 
            if (_inputParameters != null)
            {
                foreach (string key in _inputParameters.Keys)
                {
                    var property = key;

                    if (ContainsProperty(_inputParameters[property]) || property == "element")
                        continue;

                    if (!first) sb.Append(", ");
                    first = false;

                    if (!EnableEffectConstructionCompatibility)
                    {
                        // jquery's uses complete vs. scriptaculous uses afterFinish
                        if (property.Equals("afterFinish")) property = "complete";
                    }

                    sb.Append(property).Append(":").Append("e.memo." + _inputParameters[key]);
                }
            }

            if (hasParams)
                sb.Append("})");

            return sb.ToString();
        }

        private bool ContainsProperty(string property)
        {
            return Properties.Exists(left => left.Key == property);
        }

        #endregion
    }

   

}