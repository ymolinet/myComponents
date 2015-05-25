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

#if !DEBUG
namespace Gaia.WebWidgets
{
    public sealed partial class Manager
    {
        private bool _commonFilesAreIncluded;
        private bool _extensionsScriptFilesIncluded;

        /// <summary>
        /// Include the script files required for Gaia to function. If dynamic script loading is enabled we
        /// include the minimum core set of javascript for Gaia to function. If not, we include a single 
        /// ScriptFile that includes all the javascript minified for this version. 
        /// </summary>
        public void AddInclusionOfCommonFiles()
        {
            if (GaiaAjaxConfiguration.Instance.EnableDynamicScriptLoading)
            {
                AddInclusionOfFileFromResource("Gaia.WebWidgets.LibraryScripts.common.js", typeof(Manager), "Gaia_CommonInclude_browserFinishedLoading");
                return;
            }

            if (_commonFilesAreIncluded) return;
             
            AddInclusionOfFileFromResource("Gaia.WebWidgets.LibraryScripts.all.js", typeof(Manager), "Gaia_AllInclude_browserFinishedLoading", false);
            _commonFilesAreIncluded = true;
        }

        /// <summary>
        /// Includes a JavaScript file for inclusion in the page. Works in both normal page 
        /// processing and Gaia Ajax Callbacks. Gaia can in asynchronous callback include ScriptFiles on the 
        /// client. This can be a useful construct for reducing the initial bandwidth usage. 
        /// If GaiaAjaxConfiguration.Instance.EnableDynamicScriptLoading is turned off most of the core files
        /// will be included in a single ScriptFile included. This is also quite useful. 
        /// </summary>
        /// <param name="fileNamePath">Path and name of file to include</param>
        /// <param name="type">"Unique" type to ensure single inclusion of same file</param>
        /// <param name="fullName">Name of file to include, normally the same as the fileNamePath 
        /// unless this is a "resource file inclusion"</param>
        /// <param name="typeToWaitFor">This should be the LAST "type" or "function" in your JavaScript</param>
        /// <param name="isPartOfCoreFiles">If the file to be included also exists in the core package of javascript files.
        /// file, this one is used to make the browser WAIT until that type is not "undefined" before 
        /// proceeding to ensure we don't get client side bugs due to JavaScript not being finished 
        /// loading before it's being referenced. Normally this would be a "dummy variable" appended at 
        /// the end of your JavaScript file</param>
        /// 
        public void AddInclusionOfFile(string fileNamePath, Type type, string fullName, string typeToWaitFor, bool isPartOfCoreFiles)
        {
            if (!GaiaAjaxConfiguration.Instance.EnableJavaScriptInclusion)
                return;

            if (!GaiaAjaxConfiguration.Instance.EnableDynamicScriptLoading && isPartOfCoreFiles)
                return;

            if (IsFileRegistered(fileNamePath)) 
                return;

            GaiaScriptInclusions.Add(new ScriptFileInfo(fileNamePath, typeToWaitFor));
            if (!IsAjaxCallback)
                Page.ClientScript.RegisterClientScriptInclude(type, fullName, fileNamePath);
        }

        
        /// <summary>
        /// Used by the Extensions project to include the Concatenated ScriptFiles for the Extensions. 
        /// </summary>
        /// <param name="type">The type</param>
        public void AddInclusionOfExtensionsScriptFiles(Type type)
        {
            if (_extensionsScriptFilesIncluded || GaiaAjaxConfiguration.Instance.EnableDynamicScriptLoading)
                return;

            AddInclusionOfFileFromResource("Gaia.WebWidgets.Extensions.Scripts.extensions.js", type, "Gaia_ExtensionsInclude_browserFinishedLoading");

            _extensionsScriptFilesIncluded = true;
        }

    }
}

#endif