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

#if DEBUG

namespace Gaia.WebWidgets
{

    public sealed partial class Manager
    {

        /// <summary>
        /// Add the inclusion of Common files in debug mode. This will include each script separately and is useful for
        /// debugging purposes. 
        /// </summary>
        public void AddInclusionOfCommonFiles()
        {
            AddInclusionOfFileFromResource("Gaia.WebWidgets.LibraryScripts.jquery.js", typeof(Manager), "");
            AddInclusionOfFileFromResource("Gaia.WebWidgets.LibraryScripts.jsface.js", typeof(Manager), "");
            AddInclusionOfFileFromResource("Gaia.WebWidgets.LibraryScripts.jquery-ui.js", typeof(Manager), "");
            AddInclusionOfStyleSheetFromResource(typeof(Manager), "Gaia.WebWidgets.Resources.jquery.ui.resizable.css");

            AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Control.js", typeof(Manager), "Gaia_Control_browserFinishedLoading");
            AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.WebControl.js", typeof(Manager), "Gaia_WebControl_browserFinishedLoading");
        }

        /// <summary>
        /// Includes a JavaScript file for inclusion in the page. Works in both normal page 
        /// processing and Gaia Ajax Callbacks. Gaia can in asynchronous callback include ScriptFiles on the 
        /// client. This can be a useful construct for reducing the initial bandwidth usage 
        /// </summary>
        /// <param name="fileNamePath">Path and name of file to include</param>
        /// <param name="type">"Unique" type to ensure single inclusion of same file</param>
        /// <param name="fullName">Name of file to include, normally the same as the fileNamePath 
        /// unless this is a "resource file inclusion"</param>
        /// <param name="typeToWaitFor">This should be the LAST "type" or "function" in your JavaScript 
        /// file, this one is used to make the browser WAIT until that type is not "undefined" before 
        /// proceeding to ensure we don't get client side bugs due to JavaScript not being finished 
        /// loading before it's being referenced. Normally this would be a "dummy variable" appended at 
        /// the end of your JavaScript file</param>
        /// <param name="isPartOfCoreFiles">If the file to be included also exists in the core package of javascript files.</param>
        public void AddInclusionOfFile(string fileNamePath, Type type, string fullName, string typeToWaitFor, bool isPartOfCoreFiles)
        {
            if (!GaiaAjaxConfiguration.Instance.EnableJavaScriptInclusion || IsFileRegistered(fileNamePath))
                return;

            GaiaScriptInclusions.Add(new ScriptFileInfo(fileNamePath, typeToWaitFor));
            if (!IsAjaxCallback)
                Page.ClientScript.RegisterClientScriptInclude(type, fullName, fileNamePath);
        }


        /// <summary>
        /// Used by the Extensions project to include the Concatenated ScriptFiles for the Extensions in release mode. This function
        /// does nothing in debug mode. 
        /// </summary>
        /// <param name="type">The type</param>
        public void AddInclusionOfExtensionsScriptFiles(Type type) { }

    }
}

#endif