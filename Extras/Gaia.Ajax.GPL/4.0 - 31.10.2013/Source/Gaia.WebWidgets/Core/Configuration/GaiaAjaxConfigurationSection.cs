/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Configuration;

namespace Gaia.WebWidgets
{
    /// <example> 
    ///    The configuration file should contain the following elements: 
    ///    <code escaped="true"> 
    ///        <configSections>
    ///          <section name="GaiaAjaxSection"
    ///                   type="Gaia.WebWidgets.GaiaAjaxConfigurationSection"
    ///                   restartOnExternalChanges="true"
    ///                   ></section>
    ///        
    ///        </configSections>
    ///
    ///        <GaiaAjaxSection
    ///            EnableDefaultTheme="false"
    ///            EnableJavaScriptInclusion="true"
    ///            EnableNestedCssClasses="true">
    ///        </GaiaAjaxSection>
    ///        </code> 
    /// </example> 
    internal sealed class GaiaAjaxConfigurationSection : ConfigurationSection
    {
        private const string ZIndexThresholdPropertyName = "ZIndexThreshold";
        private const string EnableDefaultThemePropertyName = "EnableDefaultTheme";
        private const string EnableNestedCssClassesPropertyName = "EnableNestedCssClasses";
        private const string EnableJavaScriptInclusionPropertyName = "EnableJavaScriptInclusion";
        private const string EnableDynamicScriptLoadingPropertyName = "EnableDynamicScriptLoading";

        [ConfigurationProperty(EnableDefaultThemePropertyName, DefaultValue = true)]
        public bool EnableDefaultTheme
        {
            get { return (bool)this[EnableDefaultThemePropertyName]; }
            set { this[EnableDefaultThemePropertyName] = value; }
        }

        [ConfigurationProperty(EnableNestedCssClassesPropertyName, DefaultValue = true)]
        public bool EnableNestedCssClasses
        {
            get { return (bool)this[EnableNestedCssClassesPropertyName]; }
            set { this[EnableNestedCssClassesPropertyName] = value; }
        }

        [ConfigurationProperty(EnableJavaScriptInclusionPropertyName, DefaultValue = true)]
        public bool EnableJavaScriptInclusion
        {
            get { return (bool)this[EnableJavaScriptInclusionPropertyName]; }
            set { this[EnableJavaScriptInclusionPropertyName] = value; }
        }

        [ConfigurationProperty(EnableDynamicScriptLoadingPropertyName, DefaultValue = false)]
        public bool EnableDynamicScriptLoading
        {
            get { return (bool)this[EnableDynamicScriptLoadingPropertyName]; }
            set { this[EnableDynamicScriptLoadingPropertyName] = value; }
        }

        [ConfigurationProperty(ZIndexThresholdPropertyName, DefaultValue = -1), IntegerValidator(MinValue = -1)]
        public int ZIndexThreshold
        {
            get { return (int)this[ZIndexThresholdPropertyName];  }
            set { this[ZIndexThresholdPropertyName] = value; }
        }
    }
}
