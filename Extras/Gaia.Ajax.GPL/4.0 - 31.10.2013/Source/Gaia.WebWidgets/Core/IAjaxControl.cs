/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using ASP = System.Web.UI;
using Gaia.WebWidgets.HtmlFormatting;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Implement this interface on any control where you need to interface with the Gaia Ajax Control Engine.
    /// </summary>
    public interface IAjaxControl
    {
        /// <summary>
        /// Specifies if the Control is in DesignMode
        /// </summary>
        bool InDesigner { get; }

        /// <summary>
        /// Should return the control itself.
        /// </summary>
        ASP.Control Control { get; }

        /// <summary>
        /// Returns the Javascript used to register the control on the client. 
        /// </summary>
        /// <returns></returns>
        string GetScript();

        /// <summary>
        /// Called by the framework, when the required script files for the control
        /// should be added for inclusion
        /// </summary>
        void IncludeScriptFiles();

        /// <summary>
        /// Called by the Gaia framework when the StateManager of the Control needs to be initialized
        /// </summary>
        /// <returns>StateManager for the Control</returns>
        PropertyStateManagerControl CreateControlStateManager();

        /// <summary>
        /// Called by the Gaia framework when the html markup for the Control should be rendered into 
        /// provided writer
        /// </summary>
        /// <param name="create">XhtmlTagFactory instance, which is used to create valid Xhtml markup</param>
        void RenderControlHtml(XhtmlTagFactory create);
        
        /// <summary>
        /// Provides access to the state manager associated with the widget.
        /// Default behaviour in AjaxControl is to instantiate by passing only a reference to the Control.  
        /// Derived classes are allowed to override default behaviour which might be useful if you have
        /// special additional rendering through e.g. usage of the IExtraPropertyCallbackRenderer interface.
        /// </summary>
        PropertyStateManagerControl StateManager { get; }

        /// <summary>
        /// Returns the AjaxControl object associated with this control. 
        /// </summary>
        AjaxControl AjaxControl { get; }

        /// <summary>
        /// Specifies tag which should be used when rendering invisible ajax control placeholder
        /// </summary>
        string TagName { get; }
    }
}
