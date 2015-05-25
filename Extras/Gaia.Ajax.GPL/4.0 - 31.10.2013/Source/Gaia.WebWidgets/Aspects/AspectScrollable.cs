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

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectScrollable.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Aspect class for tracking "scroll events" on widgets. Widgets you attach this aspect to
    /// can raise the Scroll event when the user scrolls the widget. Either when scrolling to the bottom
    /// or when scrolling at all.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for AspectScrollable" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectScrollable\Another\Default.aspx" />
    /// </code> 
    /// <code title="Codebehind for AspectScrollable" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectScrollable\Another\Default.aspx.cs" />
    /// </code>
    /// </example>
    public class AspectScrollable : Aspect<AspectScrollable>, IAspect
    {
        #region [ -- EventArgs for Events -- ]

        /// <summary>
        /// EventArgs for the Scroll Event
        /// </summary>
        public class ScrollEventArgs : EventArgs
        {
            private readonly int _height;
            private readonly int _width;
            private readonly int _left;
            private readonly int _top;

            internal ScrollEventArgs(int left, int top, int width, int height)
            {
                _top = top;
                _left = left;
                _width = width;
                _height = height;
            }

            /// <summary>
            /// Distance between element top and current viewable content top
            /// </summary>
            public int Top
            {
                get { return _top; }
            }

            /// <summary>
            /// Distance between element left and current viewable content left
            /// </summary>
            public int Left
            {
                get { return _left; }
            }

            /// <summary>
            /// Full scrollable width of the element content,
            /// which is NOT the actual viewable width
            /// </summary>
            public int Width
            {
                get { return _width; }
            }

            /// <summary>
            /// Full scrollable height of the element content,
            /// which is NOT the actual viewable height
            /// </summary>
            public int Height
            {
                get { return _height; }
            }
        }

        #endregion

        #region [ -- Enumerations -- ]

        /// <summary>
        /// Specifies in what direction or combination of directions scrolling should be trapped.
        /// </summary>
        [Flags]
        public enum ScrollModes
        {
            /// <summary>
            /// Doesn't trap scroll events at all
            /// </summary>
            None = 0,

            /// <summary>
            /// Traps vertical scrolling events
            /// </summary>
            Vertical = 2,

            /// <summary>
            /// Traps horizontal scrolling events
            /// </summary>
            Horizontal = 4,

            /// <summary>
            /// Traps all scrolling events in all directions
            /// </summary>
            All = Horizontal | Vertical
        }

        #endregion

        /// <summary>
        /// Event raised when element is scrolled
        /// </summary>
        public event EventHandler<ScrollEventArgs> Scrolled;

        private ScrollModes _scrollMode;
        private bool _onlyRaiseAtEdge;

        #region [ -- Constructors -- ]

        /// <summary>
        /// Default constructor
        /// </summary>
        public AspectScrollable()
            : this(null)
        { }

        /// <summary>
        /// Constructor taking event handler for the OnScrolled event
        /// </summary>
        /// <param name="scrolled">delegate called when item is scrolled</param>
        public AspectScrollable(EventHandler<ScrollEventArgs> scrolled)
            : this(scrolled, ScrollModes.All)
        { }

        /// <summary>
        /// Constructor taking event handler of the OnScrolled event and also the directions to raise the event for
        /// </summary>
        /// <param name="scrolled">Event handler for the scrolled event</param>
        /// <param name="mode">Which axis to raise the event for</param>
        public AspectScrollable(EventHandler<ScrollEventArgs> scrolled, ScrollModes mode)
            : this(scrolled, mode, false)
        { }

        /// <summary>
        /// Constructor taking event handler for the Scroll event,
        /// the sides to raise the event for and whether or not the event should only 
        /// be raised when you're at "EOF" of the element.
        /// </summary>
        /// <param name="scrolled">Event handler for thescrolled event</param>
        /// <param name="mode">Which axis to raise the event for</param>
        /// <param name="onlyRaiseAtEdge">If true event handler will only be called when you're at the bottom or far right of the element</param>
        public AspectScrollable(EventHandler<ScrollEventArgs> scrolled, ScrollModes mode, bool onlyRaiseAtEdge)
        {
            if (scrolled != null)
                Scrolled += scrolled;

            _scrollMode = mode;
            _onlyRaiseAtEdge = onlyRaiseAtEdge;
        }

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Which direction to trap scroll events for
        /// </summary>
        public ScrollModes Mode
        {
            get { return _scrollMode; }
            set { _scrollMode = value; }
        }

        /// <summary>
        /// If true then scroll events will only be raised at "edges" meaning when maximum scrolling to
        /// some direction have occured
        /// </summary>
        public bool OnlyRaiseAtEdge
        {
            get { return _onlyRaiseAtEdge; }
            set { _onlyRaiseAtEdge = value; }
        }

        #endregion

        [Method]
        internal void ScrollMethod(int left, int top, int width, int height)
        {
            if (Scrolled != null)
                Scrolled(GetSender(), new ScrollEventArgs(left, top, width, height));
        }

        #region [ -- IAspect Implementation -- ]

        string IAspect.GetScript()
        {
            return new RegisterAspect("Gaia.AspectScrollable", ParentControl.Control.ClientID)
                .AddProperty("scrollMode", (int)_scrollMode)
                .AddPropertyIfTrue(_onlyRaiseAtEdge, "onlyRaiseAtEdges", _onlyRaiseAtEdge)
                .ToString();
        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectScrollable.js", typeof(Manager), "Gaia.AspectScrollable.browserFinishedLoading", true);
        }

        #endregion

    }
}
