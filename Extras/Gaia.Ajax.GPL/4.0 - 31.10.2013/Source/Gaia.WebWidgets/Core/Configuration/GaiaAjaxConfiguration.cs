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
    using System.Web.Configuration;

    /// <summary>
    /// The Gaia Ajax Configuration class 
    /// </summary>
    /// <example> 
    ///    The configuration file should contain the following elements: 
    ///    <code escaped="true"> 
    ///        <configSections>
    ///          <section name="GaiaAjaxSection"
    ///                   type="Gaia.WebWidgets.GaiaAjaxConfigurationSection"
    ///                   restartOnExternalChanges="true"></section>
    ///        </configSections>
    ///
    ///        <GaiaAjaxSection
    ///            EnableDefaultTheme="false"
    ///            EnableJavaScriptInclusion="true"
    ///            EnableNestedCssClasses="true">
    ///        </GaiaAjaxSection>
    ///        </code> 
    /// </example> 
    public class GaiaAjaxConfiguration
    {
        /// <summary>
        /// Helper class for creating thread-safe singletons.
        /// </summary>
        // ReSharper disable ClassNeverInstantiated.Local
        private sealed class SingletonHelper
        // ReSharper restore ClassNeverInstantiated.Local
        {
            static SingletonHelper()
            {
                var section = WebConfigurationManager.GetSection("GaiaAjaxSection") as GaiaAjaxConfigurationSection;
                GaiaAjaxConfigurationInstance = new GaiaAjaxConfiguration(section ?? new GaiaAjaxConfigurationSection());
            }

            internal static readonly GaiaAjaxConfiguration GaiaAjaxConfigurationInstance;
        }

        private readonly GaiaAjaxConfigurationSection _config;

        private GaiaAjaxConfiguration(GaiaAjaxConfigurationSection section)
        {
            _config = section;
        }

        /// <summary>
        /// Entrance to the Gaia Ajax Configuration Provider. 
        /// </summary>
        public static GaiaAjaxConfiguration Instance
        {
            get { return SingletonHelper.GaiaAjaxConfigurationInstance; }
        }

        /// <summary>
        /// If you do not provide a CssClass property, Gaia will automatically enable some default
        /// styles for the widgets so they work out-of-the-box. If you don't want this behavior
        /// set this value to true
        /// </summary>
        public bool EnableDefaultTheme
        {
            get { return _config.EnableDefaultTheme; }
            set { _config.EnableDefaultTheme = value; }
        }

        /// <summary>
        /// The DOM model that Gaia introduces presets a lot of different css classnames on elements
        /// and child elements. ie. Gaiax-window, Gaiax-window-bodywrapper, Gaiax-window-content, etc.
        /// If you want to only set your custom css class on the root element and stop propagation of the
        /// Gaia DOM structure, set this property to true. If you want to skin Gaia manually you must
        /// use Selectors manually like this. .gaiax-window div div span for example to find the proper element
        /// to set its styles on. 
        /// </summary>
        public bool EnableNestedCssClasses
        {
            get { return _config.EnableNestedCssClasses; }
            set { _config.EnableNestedCssClasses = value; }
        }

        /// <summary>
        /// Gaia automatically includes the required JavaScript files pr/widget. If you don't want
        /// this behavior and want to manually include the JavaScript files yourself, you can set
        /// this property to false and Gaia will stop sending the files. Set this value with caution
        /// as it will break your entire application if you don't include the files properly. 
        /// </summary>
        public bool EnableJavaScriptInclusion
        {
            get { return _config.EnableJavaScriptInclusion; }
            set { _config.EnableJavaScriptInclusion = value; }            
        }

        /// <summary>
        /// Gaia automatically includes all JavaScript files in 1 file which is rendered from the WebResource 
        /// handler. If you set this value to true, Gaia will automatically only render the required JavaScript
        /// files needed pr/widget. Even in callbacks it will deliver .js files over the network and include them
        /// note: The dynamic loading of JavaScript has known issues and may break some applications. 
        /// This functionality is not supported by Gaiaware
        /// </summary>
        public bool EnableDynamicScriptLoading
        {
            get { return _config.EnableDynamicScriptLoading; }
            set { _config.EnableDynamicScriptLoading = value; }
        }

        /// <summary>
        /// Specifies the threshold that will be used in routines which compute z-index.
        /// All the stacking contexts with z-index greater then the specified threshold will be ignored.
        /// </summary>
        public int ZIndexThreshold
        {
            get { return _config.ZIndexThreshold; }
            set { _config.ZIndexThreshold = value; }
        }

        internal string CssClassHiddenControl
        {
            get { return "plh__gaia"; }
        }
    }
}
