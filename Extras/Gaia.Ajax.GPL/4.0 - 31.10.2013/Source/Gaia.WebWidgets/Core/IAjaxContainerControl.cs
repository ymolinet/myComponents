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
using Gaia.WebWidgets.HtmlFormatting;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// This interface defines that a Gaia Control is a container widget which means it can contain 
    /// child controls. Examples of such Controls are Panel, MultiView etc.
    /// </summary>
    public interface IAjaxContainerControl : IAspectableAjaxControl
    {
        /// <summary>
        /// Forces a re-rendering of the child controls of the widget. If you call this method
        /// the entire HTML markup of the widget will re-render. Note that this will be very
        /// bandwidth intensive compared to just updating the specifically changed controls instead
        /// so use this with CAUTION. Gaia tries to do "partial rendering" as seldom as possible
        /// but by calling this method you can "force" Gaia into doing a complete partial re-rendering
        /// of the widget which might inevitable occasionally.
        /// </summary>
        void ForceAnUpdate();

        /// <summary>
        /// Does the exact same thing as ForceAnUpdate except it only re-renders the child controls
        /// that are not previously rendered and it appends all child controls at the back of the list
        /// of DOM elements on the Client. Used in e.g. the AspectScrollable to create LiveScrolling and
        /// similar functionality. Note that this will APPEND all the non-rendered control back to the
        /// client which means that if you insert widgets at specific places in the Controls collection
        /// you might get surprised since those widgets will appear APPENDED on the client.
        /// </summary>
        [Obsolete("Use of TrackControlAdditions()/ForceAnUpdate() is recommended instead")]
        void ForceAnUpdateWithAppending();

        /// <summary>
        /// TrackControlAdditions is used in combination with ForceAnUpdate() or ForceAnUpdateWithAppending()
        /// It basically signals that all controls added after calling this method will be appended to the container
        /// control. Useful for minimizing network traffic when you only need to append one new control at the bottom
        /// of the control, for example TreeViewItems, Panels and similar. 
        /// </summary>
        void TrackControlAdditions();

        /// <summary>
        /// Called when ForceAnUpdate is dispatched for a control and the
        /// control needs to re-render its child control collection.
        /// </summary>
        void RenderChildrenOnForceAnUpdate(XhtmlTagFactory create);

        /// <summary>
        /// Retrieves actual AjaxContainerControl associated with the Control
        /// </summary>
        AjaxContainerControl AjaxContainerControl { get; }

        /// <summary>
        /// Returns id of the DOM element which acts as the actual container
        /// for the specified child. Used during dynamic rendering.
        /// </summary>
        /// <param name="child">Child control to get container for</param>
        /// <returns>ID of the DOM element which should contain specified child</returns>
        string GetDOMContainer(IAjaxControl child);
    }
}
