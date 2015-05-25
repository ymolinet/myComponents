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

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectMouseMove.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Aspect class for trapping MouseMove Events on elements. You would think that this is *impossible* online since it would 
    /// result in something similar to a DOS attack on the server, however the polling time can be configured. 
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for AspectMouseMove" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectMouseMove\Overview\Default.aspx"  />
    /// </code> 
    /// <code title="Adding AspectModal in CodeBehind" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectMouseMove\Overview\Default.aspx.cs" region="Code" />
    /// </code>
    /// </example>
    public class AspectMouseMove : Aspect<AspectMouseMove>, IAspect
    {
        #region [ -- Effect Events -- ]
        /// <summary>
        /// Add this EffectEvent to a control to add an effect when the mouse is moving over the control.
        /// </summary>
        public static AjaxEffectEvent EffectMouseMove { get { return AjaxEffectEventFactory.Create("gaiamoving"); } }

        #endregion

        /// <summary>
        /// When you click the element which this aspect is attached to, you will retrieve the coordinates on the server
        /// By default it will capture the absolute x,y position on the entire viewport, but if you want to only capture
        /// x,y relative to the aspects parent container you can set this value to true. 
        /// </summary>
        /// <example>
        /// <code title="Demonstrates usage of RelativeCoordinates" lang="C#"> 
        /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectClickable\RelativeCoordinates\Default.aspx.cs" region="Code" /> 
        /// </code> 
        /// </example>
        public bool UseRelativeCoordinates { get; set; }

        #region [ -- EventArgs for Events -- ]

        /// <summary>
        /// EventArgs for the MouseMove Event
        /// </summary>
        public class MouseMoveEventArgs : MouseEventArgs
        {
            internal MouseMoveEventArgs(int left, int top, int controlKeys) : 
                base(left, top, controlKeys) { }
        }

        #endregion

        /// <summary>
        /// Event raised when mouse is moved on the element's surface. X and y coords are available in the 
        /// MouseMoveEventArgs passed.
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> MouseMove;

        private int _millisecondsInterval;
        
        #region [ -- Constructors -- ]

        /// <summary>
        /// Default constructor
        /// </summary>
        public AspectMouseMove()
            : this(null)
        { }

        /// <summary>
        /// Constructor taking event handler for the MouseMoved Event
        /// </summary>
        /// <param name="mouseMove">delegate called when mouse is moved</param>
        public AspectMouseMove(EventHandler<MouseMoveEventArgs> mouseMove)
            :this(mouseMove, 100)
        { }

        /// <summary>
        /// Constructor taking event handler for the MouseMoved Event and number of milliseconds between
        /// events the client should wait before firing the next event when mouse is continuously moved.
        /// </summary>
        /// <param name="mouseMove">delegate called when mouse is moved</param>
        /// <param name="millisecondsInterval">How often the MouseMove event will raise, not if this value is too small it will completely eat up all the bandwidth for both the server and the client. Default value is 100</param>
        public AspectMouseMove(EventHandler<MouseMoveEventArgs> mouseMove, int millisecondsInterval)
        {
            MouseMove += mouseMove;
            _millisecondsInterval = millisecondsInterval;
        }

        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// Since to call the server on every mousemove event is impossible due to bandwidth usage
        /// you can set at what interval the server will be called if the mouse is moved to reduce
        /// the bandwidth usage. This value is set in milliseconds meaning 1000 == 1 second.
        /// The default value for this property is 100. Be CAREFUL if you set this value to a "low" value
        /// since this might seriously increase the bandwidth traffic for your users (and your servers)
        /// </summary>
        public int MillisecondsInterval
        {
            get { return _millisecondsInterval; }
            set { _millisecondsInterval = value; }
        }

        #endregion

        [Method]
        internal void MouseMoveMethod(int left, int top, int offsetLeft, int offsetTop, int controlKeys)
        {
            // here we change the x,y based on wheter we should use absolute or relative coordinates
            if (UseRelativeCoordinates)
            {
                left -= offsetLeft;
                top -= offsetTop;
            }

            if (MouseMove != null)
            {
                MouseMove(GetSender(), new MouseMoveEventArgs(left, top, controlKeys));
            }
        }

        #region [ -- IAspect Implementation -- ]

        string IAspect.GetScript()
        {
            return new RegisterAspect("Gaia.AspectMouseMove", ParentControl.Control.ClientID)
                .AddProperty("interval", _millisecondsInterval)
                .AddPropertyIfTrue(MouseMove != null, "hasEvent", true)
                .ToString();
        }

    
        #endregion


        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            const bool isPartOfCoreJsFiles = true;
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.LibraryScripts.dragdrop.js", typeof(Manager), "Droppables.browserFinishedLoading", isPartOfCoreJsFiles);
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectMouseMove.js", typeof(Manager), "Gaia.AspectMouseMove.browserFinishedLoading", isPartOfCoreJsFiles);
        }

     
    }
}
