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
using System.Web.UI;

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectHoverable.js", "text/javascript")]
namespace Gaia.WebWidgets
{
    /// <summary>
    /// AspectHoverable can dispatch server events on true MouseOver/MouseOut events for a control.
    /// It automatically takes care of the bubbling problem and only dispatches the event if you have subscribed to it and if 
    /// it actually is a true MouseOver of that Element.
    /// <br />
    /// Also notice that the MouseOver and MouseOut events for many of the Gaia Ajax controls actually use AspectHoverable under the 
    /// hood by using AspectBindings. In many cases it's more appropriate to use those events directly instead of adding the Aspect. 
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for AspectHoverable" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectHoverable\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Listening to OnMouseOver and OnMouseOut with AspectHoverable" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectHoverable\Overview\Default.aspx.cs" region="Code" />
    /// </code>
    /// </example>
    public class AspectHoverable : Aspect<AspectHoverable>, IAspect
    {
        #region [ -- Effect Events -- ]
        /// <summary>
        /// Add this EffectEvent to a control to add an effect when the mouse is moved over the control
        /// </summary>
        public static AjaxEffectEvent EffectEventMouseOver { get { return AjaxEffectEventFactory.Create("gaiamouseover"); } }

        /// <summary>
        /// Add this EffectEvent to a control to add an effect when the mouse is moved out of the control
        /// </summary>
        public static AjaxEffectEvent EffectEventMouseOut { get { return AjaxEffectEventFactory.Create("gaiamouseout"); } }

        #endregion

        /// <summary>
        /// When you click the element which this aspect is attached to, you will retrieve the coordinates on the server
        /// By default it will capture the absolute x,y position on the entire viewport, but if you want to only capture
        /// x,y relative to the aspects parent container you can set this value to true. 
        /// </summary>
        public bool UseRelativeCoordinates { get; set; }

        #region [ -- EventArgs for Events -- ]

        /// <summary>
        /// EventArgs for the Hover Event
        /// </summary>
        public class HoverEventArgs : MouseEventArgs
        {
            internal HoverEventArgs(int left, int top, int controlKeys)
                : base(left, top, controlKeys) { }
        }

        #endregion

        /// <summary>
        /// Event raised when mouse passes over the element
        /// </summary>
        public event EventHandler<HoverEventArgs> MouseOver;

        /// <summary>
        /// Event raised when mouse passes OUT of the element's surface
        /// </summary>
        public event EventHandler MouseOut;

        #region [ -- Constructors -- ]

        /// <summary>
        /// Default constructor
        /// </summary>
        public AspectHoverable()
            : this(null)
        { }

        /// <summary>
        /// Constructor taking event handler for the MouseOver Event
        /// </summary>
        /// <param name="mouseOver">delegate called when item is Hovered over (mouse positioned above it)</param>
        public AspectHoverable(EventHandler<HoverEventArgs> mouseOver)
            :this(mouseOver, null)
        { }

        /// <summary>
        /// Constructor taking event handler for the MouseOver and the MouseOut Event
        /// </summary>
        /// <param name="mouseOver">delegate called when item is Hovered over (mouse positioned above it)</param>
        /// <param name="mouseOut">delegate called when item is Hovered out of (mouse removed from element)</param>
        public AspectHoverable(EventHandler<HoverEventArgs> mouseOver, EventHandler mouseOut)
        {
            MouseOver += mouseOver;
            MouseOut += mouseOut;
        }

        #endregion

        [Method]
        internal void MouseOverMethod(int left, int top, int offsetLeft, int offsetTop, int controlKeys)
        {
            // here we change the x,y based on wheter we should use absolute or relative coordinates
            if (UseRelativeCoordinates)
            {
                left -= offsetLeft;
                top -= offsetTop;
            }

            if (MouseOver != null)
                MouseOver(GetSender(), new HoverEventArgs(left, top, controlKeys));
        }

        [Method]
        internal void MouseOutMethod()
        {
            if (MouseOut != null)
                MouseOut(GetSender(), EventArgs.Empty);
        }

        #region [ -- IAspect Implementation -- ]

        string IAspect.GetScript()
        {
            return new RegisterAspect("Gaia.AspectHoverable", ParentControl.Control.ClientID)
                .AddProperty("mouseOver", MouseOver != null)
                .AddProperty("mouseOut", MouseOut != null)
                .ToString();
        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectHoverable.js", typeof(Manager), "Gaia.AspectHoverable.browserFinishedLoading", true);
        }

        #endregion

    }
}
