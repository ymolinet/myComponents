/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets.Effects
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text.RegularExpressions;

    /// <summary>
    /// This effect changes the CSS properties of an element.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectMorph to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectMorph\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectMorph : ScriptaculousEffectBase
    {
        private string _cssStyle;

        // underlying this is the animate ... 

        /// <summary>
        /// The CssStyle to morph into. For example (background:#080; color:#fff;);
        /// note: keys should be javascript names (camel-cased), rather than CSS ones
        /// <example>
        /// backgroundColor rather than background-color):
        /// </example>
        /// </summary>
        public string CssStyle
        {
            get { return _cssStyle; }
            set { _cssStyle = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMorph"/> class.
        /// </summary>
        public EffectMorph() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMorph"/> class.
        /// </summary>
        /// <param name="cssStyle">The CSS style.</param>
        public EffectMorph(string cssStyle) : this(cssStyle, 1) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMorph"/> class.
        /// </summary>
        /// <param name="cssStyle">The CSS style.</param>
        /// <param name="duration">The duration.</param>
        public EffectMorph(string cssStyle, decimal duration) : base(duration, 0)
        {
            _cssStyle = cssStyle;
        }

        /// <summary>
        /// GetStyleProperties will check if CustomParameters are added to this Effect. If none are specified, the CssStyle string is returned
        /// and no calculations are performed. If however custom parameters are defined we swap out the provided values with the parameters 
        /// instead. 
        /// </summary>
        /// <returns></returns>
        private string GetStyleProperties()
        {
            IEffect e = this;
            if (e.PropertyParameters.Count == 0)
                return string.Concat("'", CssStyle, "'");

            Match match;
            HybridDictionary styleProperties = new HybridDictionary(true);

            if (!string.IsNullOrEmpty(CssStyle) && (match = RegexStyles.Match(CssStyle, 0)).Success)
            {
                CaptureCollection keys = match.Groups["key"].Captures;
                CaptureCollection values = match.Groups["val"].Captures;
                for (int i = 0; i < keys.Count; i++)
                    styleProperties[keys[i].ToString()] = values[i].ToString();
            }

            
            List<string> cssParams = new List<string>(styleProperties.Count);
            foreach (DictionaryEntry entry in styleProperties)
            {
                bool keyIsParam = e.PropertyParameters.ContainsKey(entry.Key.ToString());
                if (keyIsParam)
                    continue;

                cssParams.Add(string.Concat(entry.Key, ":'", entry.Value, "'"));
                
            }

            foreach (KeyValuePair<string, string> pair in e.PropertyParameters)
            {
                bool valid = Array.IndexOf(SupportedStyles, pair.Key) > -1;
                if (valid)
                    cssParams.Add(string.Concat(pair.Key, ":", "e.memo." + pair.Value)); // property params take the o.val notation where o=options input
            }

            return string.Concat("{", string.Join(",", cssParams.ToArray()), "}");

        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            registerEffect.AddProperty("style", GetStyleProperties(), false);
            registerEffect.EffectType = "Effect.Morph";
        }
        
        private static readonly Regex RegexStyles = new Regex(@"\G(\s*(;\s*)*(?<key>[^:]+?)\s*:\s*(?<val>[^;]*))*\s*(;\s*)*$", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Multiline);
        private static readonly string[] SupportedStyles = {
            "background",
            "backgroundcolor",
            "bordercolor",
            "border",
            "borderwidth",
            "color",
            "fontsize",
            "font",
            "height",
            "width",
            "left",
            "margin",
            "marginbottom",
            "marginleft",
            "marginright",
            "margintop",
            "padding",
            "paddingbottom",
            "paddingleft",
            "paddingright",
            "paddingtop",
            "top",
            "zindex"
}; 
    }
}