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
using System.Collections.Generic;

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectClickable.js", "text/javascript")]
namespace Gaia.WebWidgets
{
    /// <summary>
    /// Aspect class for making elements clickable.
    /// Element you attach this Aspect to can be clicked with the mouse even though they're not 
    /// "natively" clickable elements. When clicked you will be able to trap that event on the server through the 
    /// Clicked of the DblClicked event. Both single clicks and double clicks are supported, you can choose which
    /// of the two (or both) you wish to trap for the widget you attach this aspect to.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for AspectClickable Example" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectClickable\DoubleClick\Default.aspx" />
    /// </code> 
    /// <code title="Double Click Panel to Fade it Away" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectClickable\DoubleClick\Default.aspx.cs" region="Code" /> 
    /// </code>
    /// <code title="Click Panel to Toggle CssClass" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectClickable\ToggleCssClass\Default.aspx.cs" region="Code" /> 
    /// </code> 
    /// <code title="One Line Construction Of AspectClickable" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectClickable\ShortHandSyntax\Default.aspx.cs" region="Code" /> 
    /// </code> 
    /// </example>
    public class AspectClickable : Aspect<AspectClickable>, IAspect
    {
        #region [ -- EffectEvents -- ]
        /// <summary>
        /// Use this EffectEvent to capture the click event and wire an effect to it. Doesn't require that you 
        /// add AspectClickable itself since it relies on the native click event. 
        /// </summary>
        public static AjaxEffectEvent EffectEventClick { get { return AjaxEffectEventFactory.Create("click"); } }

        /// <summary>
        /// Use this EffectEvent to capture the doubleclick event and wire an effect to it. Doesn't require that you 
        /// add AspectClickable itself since it relies on the native click event. 
        /// </summary>
        public static AjaxEffectEvent EffectEventDoubleClick { get { return AjaxEffectEventFactory.Create("dblclick"); } }

        #endregion

        /// <summary>
        /// When you click the element which this aspect is attached to, you will retrieve the coordinates on the server
        /// By default it will capture the absolute x,y position on the entire viewport, but if you want to only capture
        /// x,y relative to the aspects parent container you can set this value to true. 
        /// </summary>
        /// <example>
        /// <code title="Use Top and Left in ClickedEventArgs through relative coordinates" lang="C#"> 
        /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectClickable\RelativeCoordinates\Default.aspx.cs" region="Code" /> 
        /// </code> 
        /// </example>
        public bool UseRelativeCoordinates { get; set; }

        #region [ -- EventArgs for Events -- ]

        /// <summary>
        /// EventArgs for Clicked and the DblClicked events. Notice here that you do have access to the x and y 
        /// coordinate of the place the mouse cursor was at when the event was raised.
        /// </summary>
        public class ClickEventArgs : MouseEventArgs
        {
            internal ClickEventArgs(int left, int top, int controlKeys)
                : base(left, top, controlKeys) { }
        }

        #endregion

        /// <summary>
        /// Event raised when element is single clicked. In the event handler for this event you will have
        /// access to the x and y coordinates of the mouse when the event was raised.
        /// </summary>
        public event EventHandler<ClickEventArgs> Clicked;

        /// <summary>
        /// Event raised when element is DOUBLE clicked. In the event handler for this event you will have
        /// access to the x and y coordinates of the mouse when the event was raised.
        /// </summary>
        public event EventHandler<ClickEventArgs> DblClicked;

     

        #region [ -- Constructors -- ]

        /// <summary>
        /// Default constructor, doesn't set event handlers for any of the events (Clicked and DblClicked)
        /// </summary>
        public AspectClickable()
            : this(null)
        { }

        /// <summary>
        /// Constructor taking event handler for the Clicked event.
        /// </summary>
        /// <param name="clicked">delegate called when item is clicked</param>
        public AspectClickable(EventHandler<ClickEventArgs> clicked)
            : this(clicked, null)
        { }

        /// <summary>
        /// Constructor taking event handler for the Clicked and the DblClicked event
        /// </summary>
        /// <param name="clicked">delegate called when item is clicked</param>
        /// <param name="dblClicked">delegate called when item is DOUBLE clicked</param>
        public AspectClickable(EventHandler<ClickEventArgs> clicked, EventHandler<ClickEventArgs> dblClicked)
        {
            Clicked += clicked;
            DblClicked += dblClicked;
        }

        #endregion

        [Method]
        internal void ClickMethod(string evt, int left, int top, int offsetLeft, int offsetTop, int controlKeys)
        {
            // here we change the x,y based on wheter we should use absolute or relative coordinates
            if (UseRelativeCoordinates)
            {
                left -= offsetLeft;
                top -= offsetTop;
            }

            var eventArgs = new ClickEventArgs(left, top, controlKeys);

            if (evt == "click" && Clicked != null)
                Clicked(GetSender(), eventArgs);
            else if (evt == "dblclick" && DblClicked != null)
                DblClicked(GetSender(), eventArgs);    
            
        }

        #region [ -- IAspect Implementation -- ]

        string IAspect.GetScript()
        {
            return new RegisterAspect("Gaia.AspectClickable", ParentControl.Control.ClientID)
                .AddProperty("evts", GetJsEvents(), false)
                .ToString();
        }

        private string GetJsEvents()
        {
            var events = new List<string>(2);
            if (Clicked != null) events.Add("'click'");
            if (DblClicked != null) events.Add("'dblclick'");
            return "[" + string.Join(",", events.ToArray()) + "]";
        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectClickable.js", typeof(Manager), "Gaia.AspectClickable.browserFinishedLoading", true);
        }

        #endregion

    }
}
