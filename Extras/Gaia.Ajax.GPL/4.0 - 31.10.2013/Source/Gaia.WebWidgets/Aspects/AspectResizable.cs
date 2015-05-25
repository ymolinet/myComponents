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
using System.Drawing;
using System.Globalization;
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectResizable.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Aspect class for making elements resizable. With this aspect you can make ANY widget 
    /// resizable by dragging its borders. Events will be raised (if subscribed to) on the server
    /// when the Widget has been resized by the user.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for AspectResizable" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectResizable\Overview\Default.aspx"  />
    /// </code> 
    /// <code title="Adding Support for Resizing your Control with AspectResizable" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectResizable\Overview\Default.aspx.cs" region="Code"  />
    /// </code>
    /// <code title="Specifying Maximum Height and Width during Resize" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectResizable\MaxHeightWidth\Default.aspx.cs" region="Code"  />
    /// </code>
    /// <code title="Add Support for Resizing only certain corners of your Control" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectResizable\SpecificBorders\Default.aspx.cs" region="Code"  />
    /// </code>
    /// </example>
    public class AspectResizable : Aspect<AspectResizable>, IAspect
    {
        #region [ -- Effect Events -- ]
        /// <summary>
        /// Add this EffectEvent to a control to add an effect when element is resized
        /// </summary>
        public static AjaxEffectEvent EffectEventResized { get { return AjaxEffectEventFactory.Create("gaiaresized"); } }

        #endregion

        #region [ -- Enumerations -- ]

        /// <summary>
        /// Denotes through which borders or their combination(s)
        /// control can be resized.
        /// </summary>
        [Flags]
        public enum ResizeModes
        {
            /// <summary>
            /// Disables resizing
            /// </summary>
            None         = 0,

            /// <summary>
            /// Resize using left border
            /// </summary>
            LeftBorder   = 2,

            /// <summary>
            /// Resize using right border
            /// </summary>
            RightBorder  = 4,

            /// <summary>
            /// Resize using top border
            /// </summary>
            TopBorder    = 8,

            /// <summary>
            /// Resize using bottom border
            /// </summary>
            BottomBorder = 16,

            /// <summary>
            /// Resize using all borders
            /// </summary>
            All = LeftBorder | RightBorder | TopBorder | BottomBorder
        }

        #endregion

        /// <summary>
        /// Snapping configuration for the <see cref="AspectResizable"/>.
        /// </summary>
        public sealed class SnapConfiguration : SnapConfigurationBase { }

        /// <summary>
        /// Raised when the associated <see cref="ASP.WebControl"/> has been resized,
        /// but after the resizing is handled by the <see cref="AspectResizable"/>.
        /// </summary>
        public event EventHandler Resized;

        /// <summary>
        /// Raised when the associated <see cref="ASP.WebControl"/> has been resized,
        /// but before the resizing is handled by the <see cref="AspectResizable"/>.
        /// </summary>
        public event EventHandler<ResizingEventArgs> Resizing;

        /// <summary>
        /// Provides data for <see cref="AspectResizable.Resizing"/> event.
        /// </summary>
        public sealed class ResizingEventArgs : CancellablePositionEventArgs
        {            
            private readonly Size _dimensions;

            /// <summary>
            /// Returns new dimensions of the resized element.
            /// </summary>
            public Size Dimensions
            {
                get { return _dimensions; }
            }

            internal ResizingEventArgs(Point position, Size dimensions) : base(position)
            {
                _dimensions = dimensions;
            }
        }

        private SnapConfiguration _snap;

        #region [ -- Constructors -- ]

        /// <summary>
        /// Constructor defaults to resizing in all directions
        /// </summary>
        public AspectResizable()
            : this(null)
        { }

        /// <summary>
        /// Constructor taking event handler for Resized event and defaults the resizing to all directions.
        /// </summary>
        /// <param name="onResized">Event handler for Resized event</param>
        public AspectResizable(EventHandler onResized)
            : this(onResized, ResizeModes.All)
        { }

        /// <summary>
        /// Constructor taking resize mode and Resized event handler
        /// </summary>
        /// <param name="resizeMode">Which borders to let the widget be resized at</param>
        /// <param name="onResized">Event handler to call when widget has been resized</param>
        public AspectResizable(EventHandler onResized, ResizeModes resizeMode)
            : this(onResized, resizeMode, 0, 0)
        { }

        /// <summary>
        /// Constructor taking resize mode, resized event handler and min width and height
        /// </summary>
        /// <param name="resizeMode">Which borders to let the widget be resized at</param>
        /// <param name="onResized">Event handler for OnResized event</param>
        /// <param name="minWidth">Minimum width of widget</param>
        /// <param name="minHeight">Minimum height of widget</param>
        public AspectResizable(EventHandler onResized, 
            ResizeModes resizeMode, 
            int minWidth, 
            int minHeight)
            : this(onResized, resizeMode, minWidth, minHeight, -1, -1)
        { }

        /// <summary>
        /// Constructor taking resize mode, resized event handler and min size
        /// </summary>
        /// <param name="resizeMode">Which borders to let the widget be resized at</param>
        /// <param name="onResized">Event handler for OnResized event</param>
        /// <param name="minSize">Minimum size of widget</param>
        public AspectResizable(EventHandler onResized,
            ResizeModes resizeMode,
            Size minSize)
            : this(onResized, resizeMode, minSize.Width, minSize.Height, -1, -1)
        { }

        /// <summary>
        /// Constructor taking resize mode, OnResized event handler, minimum height and width and maximum 
        /// height and width
        /// </summary>
        /// <param name="resizeMode">Which borders to let the widget be resized by</param>
        /// <param name="onResized">Event handler to call when Widget is resized</param>
        /// <param name="minWidth">Minimum width of widget</param>
        /// <param name="minHeight">Minimum height of widget</param>
        /// <param name="maxWidth">Maximum width of widget</param>
        /// <param name="maxHeight">Maximum height of widget</param>
        public AspectResizable(EventHandler onResized,
            ResizeModes resizeMode,
            int minWidth,
            int minHeight,
            int maxWidth,
            int maxHeight)
            : this(onResized, resizeMode, minWidth, minHeight, maxWidth, maxHeight, string.Empty)
        { }

        /// <summary>
        /// Constructor taking resize mode, OnResized event handler, minimum size and maximum size
        /// </summary>
        /// <param name="resizeMode">Which borders to let the widget be resized by</param>
        /// <param name="onResized">Event handler to call when Widget is resized</param>
        /// <param name="minSize">Minimum size of widget</param>
        /// <param name="maxSize">Maximum size of widget</param>
        public AspectResizable(EventHandler onResized,
            ResizeModes resizeMode,
            Size minSize,
            Size maxSize)
            : this(onResized, resizeMode, minSize.Width, minSize.Height, maxSize.Width, maxSize.Height, string.Empty)
        { }
                
        /// <summary>
        /// Constructor taking resize mode, OnResized event handler, minimum size and maximum size 
        /// and handle DOM element ID from which the widget should be resizable from.
        /// </summary>
        /// <param name="resizeMode">Which borders to let the widget be resized by</param>
        /// <param name="onResized">Event handler to call when Widget is resized</param>
        /// <param name="minSize">Minimum size of widget</param>
        /// <param name="maxSize">Maximum size of widget</param>
        /// <param name="handleId">The client ID of the DOM element from which to resize the Widget</param>
        public AspectResizable(EventHandler onResized,
            ResizeModes resizeMode,
            Size minSize,
            Size maxSize,
            string handleId)
            : this(onResized, resizeMode, minSize.Width, minSize.Height, maxSize.Width, maxSize.Height, handleId)
        { }

        /// <summary>
        /// Constructor taking resize mode, OnResized event handler, minimum height and width and maximum height 
        /// and width and handle DOM element ID from which the widget should be resizable from.
        /// </summary>
        /// <param name="resizeMode">Which borders to let the widget be resized by</param>
        /// <param name="onResized">Event handler to call when Widget is resized</param>
        /// <param name="minWidth">Minimum width of widget</param>
        /// <param name="minHeight">Minimum height of widget</param>
        /// <param name="maxWidth">Maximum width of widget</param>
        /// <param name="maxHeight">Maximum height of widget</param>
        /// <param name="handleId">The client ID of the DOM element from which to resize the Widget</param>
        public AspectResizable(EventHandler onResized,
            ResizeModes resizeMode,
            int minWidth,
            int minHeight,
            int maxWidth,
            int maxHeight,
            string handleId)
        {
            Mode = resizeMode;
            Resized += onResized;
            MinWidth = minWidth;
            MinHeight = minHeight;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
            HandleID = handleId;
        }

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Which corners the Widget can be resized at
        /// </summary>
        public ResizeModes Mode { get; set; }

        /// <summary>
        /// Minimum width that widget can be resized to
        /// </summary>
        public int MinWidth { get; set; }

        /// <summary>
        /// Maximum width widget can be resized to
        /// </summary>
        public int MaxWidth { get; set; }

        /// <summary>
        /// Minimum height widget can be resized to
        /// </summary>
        public int MinHeight { get; set; }

        /// <summary>
        /// Maximum height widget can be resized to
        /// </summary>
        public int MaxHeight { get; set; }

        /// <summary>
        /// ID to DOM element from which the widget will be resizable from
        /// </summary>
        public string HandleID { get; set; }

        /// <summary>
        /// Id of the DOM element which also will be resized with the exact same delta if widget is resized. This
        /// is useful if you have another element (or control) which needs to be resized with the exact same delta as the
        /// widget the resizing occurs to.
        /// </summary>
        public string SiblingElement { get; set; }

        /// <summary>
        /// Specifies snap configuration.
        /// </summary>
        public SnapConfiguration Snap
        {
            get { return _snap ?? (_snap = new SnapConfiguration()); }
        }

        /// <summary>
        /// Specifies the bounding rectangle where the control resizing will be constrainted to.
        /// </summary>
        public Rectangle BoundingRectangle { get; set; }

        #endregion

        [Method]
        internal void ResizedMethod(int left, int top, int width, int height)
        {
            var parentControl = ParentControl as ASP.WebControl;
            if (parentControl == null) return;

            string originalTop = null;
            string originalLeft = null;
            var originalWidth = ASP.Unit.Empty;
            var originalHeight = ASP.Unit.Empty;
            
            bool cancel = false;
            if (Resizing != null)
            {
                originalWidth = parentControl.Width;
                originalHeight = parentControl.Height;
                
                originalTop = parentControl.Style[HtmlTextWriterStyle.Top];
                originalLeft = parentControl.Style[HtmlTextWriterStyle.Left];

                var evtArgs = new ResizingEventArgs(new Point(left, top), new Size(width, height));
                Resizing(GetSender(), evtArgs);
                cancel = evtArgs.Cancel;
            }

            parentControl.Width = ASP.Unit.Pixel(width);
            parentControl.Height = ASP.Unit.Pixel(height);

            parentControl.Style[HtmlTextWriterStyle.Top] = ASP.Unit.Pixel(top).ToString(NumberFormatInfo.InvariantInfo);
            parentControl.Style[HtmlTextWriterStyle.Left] = ASP.Unit.Pixel(left).ToString(NumberFormatInfo.InvariantInfo);

            var aspectableAjaxControl = (IAspectableAjaxControl)parentControl;
            var stateManager = aspectableAjaxControl.StateManager as PropertyStateManagerWebControl;
            if (stateManager != null)
            {
                stateManager.ClearDirty(PropertyStateManagerWebControl.WebControlProperty.Width,
                                        PropertyStateManagerWebControl.WebControlProperty.Height);

                stateManager.ClearDirtyStyle(HtmlTextWriterStyle.Top, HtmlTextWriterStyle.Left);
            }

            if (cancel)
            {
                parentControl.Width = originalWidth;
                parentControl.Height = originalHeight;
                parentControl.Style[HtmlTextWriterStyle.Top] = originalTop;
                parentControl.Style[HtmlTextWriterStyle.Left] = originalLeft;
            }
            else if (Resized != null)
                Resized(GetSender(), EventArgs.Empty);
        }

        #region [ -- IAspect Implementation -- ]

        string IAspect.GetScript()
        {
            var snap = Snap.ToRegistrationOption();
            var bounds = SerializeBoundingRectangle();

            return GetScript(new RegisterAspect("Gaia.AspectResizable", ParentControl.Control.ClientID)
                                 .AddPropertyIfTrue(Mode != ResizeModes.All, "resizeMode", (int) Mode)
                                 .AddPropertyIfTrue(MinWidth > 0, "minWidth", MinWidth)
                                 .AddPropertyIfTrue(MinHeight > 0, "minHeight", MinHeight)
                                 .AddPropertyIfTrue(MaxWidth > -1, "maxWidth", MaxWidth)
                                 .AddPropertyIfTrue(MaxHeight > -1, "maxHeight", MaxHeight)
                                 .AddPropertyIfTrue(Resized == null && Resizing == null, "hasEvent", false)
                                 .AddPropertyIfTrue(!string.IsNullOrEmpty(bounds), "rect", bounds, false)
                                 .AddPropertyIfTrue(!string.IsNullOrEmpty(HandleID), "handle", HandleID)
                                 .AddPropertyIfTrue(!string.IsNullOrEmpty(SiblingElement), "sibling", SiblingElement)
                                 .AddPropertyIfTrue(!string.IsNullOrEmpty(snap), "sp", snap, false)).ToString();
        }

        /// <summary>
        /// Used by derived classes to modify the aspect registration script.
        /// </summary>
        /// <param name="registerAspect">Initial aspect registration object.</param>
        /// <returns>Modified aspect registration object.</returns>
        protected virtual RegisterAspect GetScript(RegisterAspect registerAspect)
        {
            return registerAspect;
        }

        #endregion

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectResizable.js", typeof(Manager), "Gaia.AspectResizable.browserFinishedLoading", true);
        }

        /// TODO: Consider moving into Utility or member method for both
        /// TODO: AspectDraggable and AspectResizable
        private string SerializeBoundingRectangle()
        {
            var rectangle = BoundingRectangle;
            if (rectangle.IsEmpty) return null;

            return "[" + string.Join(",", new[]
                                              {
                                                  rectangle.Left.ToString(NumberFormatInfo.InvariantInfo),
                                                  rectangle.Top.ToString(NumberFormatInfo.InvariantInfo),
                                                  rectangle.Right.ToString(NumberFormatInfo.InvariantInfo),
                                                  rectangle.Bottom.ToString(NumberFormatInfo.InvariantInfo)
                                              }) + "]";
        }
    }
}
