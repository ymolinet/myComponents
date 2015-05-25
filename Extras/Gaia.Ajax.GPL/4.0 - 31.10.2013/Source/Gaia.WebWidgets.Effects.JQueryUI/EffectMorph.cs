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
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// This effect changes the CSS properties of an element.
    /// </summary>
    /// <example>
    /// <code title="Adding EffectMorph to Gaia Controls" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\EffectMorph\Overview\Default.aspx.cs" region="Code" />
    /// </code> 
    /// </example>
    public class EffectMorph : JQueryUIEffectBase
    {
        /// <summary>
        /// The CssStyle to morph into. For example (backgroundColor:#080; color:#fff;);
        /// note: keys should be javascript names (camel-cased), rather than CSS ones
        /// <example>
        /// backgroundColor rather than background-color):
        /// </example>
        /// </summary>
        public string CssStyle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMorph"/> class.
        /// </summary>
        public EffectMorph() : this(string.Empty, DefaultDuration) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMorph"/> class.
        /// </summary>
        /// <param name="cssStyle">The CSS style.</param>
        public EffectMorph(string cssStyle) : this(cssStyle, DefaultDuration) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectMorph"/> class.
        /// </summary>
        /// <param name="cssStyle">The CSS style.</param>
        /// <param name="duration">The duration.</param>
        public EffectMorph(string cssStyle, decimal duration)
        {
            Method = EffectMethod.Animate;
            Duration = duration;
            CssStyle = cssStyle;
        }

        HybridDictionary GetStyles()
        {
            Match match;
            var styleProperties = new HybridDictionary(true);

            if (!string.IsNullOrEmpty(CssStyle) && (match = RegexStyles.Match(CssStyle, 0)).Success)
            {
                var keys = match.Groups["key"].Captures;
                var values = match.Groups["val"].Captures;
                for (var i = 0; i < keys.Count; i++)
                    styleProperties[keys[i].ToString()] = values[i].ToString();
            }

            return styleProperties;
        }

        /// <summary>
        /// The name of the effect implemented in jQuery
        /// </summary>
        protected override string EffectType
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Populates the properties.
        /// </summary>
        /// <param name="registerEffect">The register effect class</param>
        protected override void PopulateProperties(RegisterEffect registerEffect)
        {
            base.PopulateProperties(registerEffect);
            SerializeStyleArray(registerEffect);
        }

        private void SerializeStyleArray(RegisterEffect registerEffect)
        {
            var styleArray = new List<string>();
            var styles = GetStyles();

            foreach (var styleKey in styles.Keys)
            {
                // this translates background-color into backgroundColor etc ...
                var key = ToCamelCase(styleKey.ToString());
                
                // here we check if we support a morph on the style
                if (Array.IndexOf(SupportedStyles, key.ToLowerInvariant()) == -1)
                    continue;

                styleArray.Add(string.Format("{0}:'{1}'", key, styles[styleKey]));
            }
            if (styleArray.Count > 0)
                registerEffect.AddParam("{" + string.Join(",", styleArray.ToArray()) + "}");
        }

        private static readonly Regex RegexStyles = new Regex(@"\G(\s*(;\s*)*(?<key>[^:]+?)\s*:\s*(?<val>[^;]*))*\s*(;\s*)*$", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Multiline);

        private static readonly string[] SupportedStyles =
            {
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

        static string ToCamelCase(string input)
        {
            return GetFriendlyFunctionName(input, false);
        }

        /// <summary>
        /// Takes an input string that may look like get_product and converts it
        /// into GetProduct or getProduct (Pascal/camelCase) 
        /// </summary>
        /// <returns></returns>
        static string GetFriendlyFunctionName(string input, bool usePascalCasing)
        {
            var result = string.Empty;
            var skippedUnderscore = false;
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (i == 0 && usePascalCasing)
                    result += c.ToString(CultureInfo.InvariantCulture).ToUpper();
                else if (skippedUnderscore)
                {
                    skippedUnderscore = false;
                    result += c.ToString(CultureInfo.InvariantCulture).ToUpper();
                }
                else if (c == '_' || c == '.' || c == '-')
                    skippedUnderscore = true;
                else
                    result += c;
            }
            return result;
        }
    }
}