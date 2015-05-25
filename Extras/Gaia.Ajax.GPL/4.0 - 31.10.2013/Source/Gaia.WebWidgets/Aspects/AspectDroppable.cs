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

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectDroppable.js", "text/javascript")]
namespace Gaia.WebWidgets
{
    using System.Net.Configuration;

    /// <summary>
    /// Aspect class for making elements droppable. 
    /// Element you attach this Aspect to can "catch" Drop events from other Draggable 
    /// elements that have the same Accept string.
    /// </summary>
    /// <example>
    /// <code title="Passing a CustomID via Drag and Drop (ASPX)" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectDroppable\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Passing a CustomID via Drag and Drop" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectDroppable\Overview\Default.aspx.cs" region="Code" />
    /// </code>
    /// </example>
    public class AspectDroppable : Aspect<AspectDroppable>, IAspect
    {
        #region [ -- EventArgs for Events -- ]

        /// <summary>
        /// Abstract base class for <see cref="EventArgs"/> hierarchy for <see cref="AspectDroppable"/>.
        /// </summary>
        public abstract class DroppableEventArgs : EventArgs
        {
            internal DroppableEventArgs(string draggedId, string idToPass)
            {
                DraggedID = draggedId;
                IdToPass = idToPass;
            }

            /// <summary>
            /// The ID of the element dropped
            /// </summary>
            public string DraggedID { get; private set; }

            /// <summary>
            /// The IdToPass from the AspectDraggable Aspect.
            /// </summary>
            public string IdToPass { get; private set; }
        }

        /// <summary>
        /// Provides data for <see cref="AspectDroppable.Dropping"/> event.
        /// </summary>
        public sealed class DroppingEventArgs : DroppableEventArgs
        {
            /// <summary>
            /// Gets or sets if the <see cref="AspectDroppable.Dropping"/> event should be cancelled.
            /// </summary>
            public bool Cancel { get; set; }

            /// <summary>
            /// Returns drop position.
            /// </summary>
            public Point Position { get; private set; }

            /// <summary>
            /// Initializes new instance of <see cref="DroppingEventArgs"/> class using specified parameters.
            /// </summary>
            public DroppingEventArgs(string draggedId, string idToPass, Point position) : base(draggedId, idToPass)
            {
                Position = position;
            }
        }

        /// <summary>
        /// Provides data for the <see cref="AspectDroppable.Dropped"/> event.
        /// </summary>
        public class DroppedEventArgs : DroppableEventArgs
        {
            /// <summary>
            /// Initializes new instance of <see cref="DroppedEventArgs"/> class.
            /// </summary>
            public DroppedEventArgs(string draggedId, string idToPass) : base(draggedId, idToPass) { }
        }

        #endregion

        #region [ -- Effect Events -- ]
        /// <summary>
        /// Use this EffectEvent to wire up an effect when the dragging item is actually dropped on a drop container. 
        /// </summary>
        public static AjaxEffectEvent EffectEventDropped { get { return AjaxEffectEventFactory.Create("gaiadropped"); } }

        #endregion

        /// <summary>
        /// Event raised when a Draggable element that is dragged with AspectDraggable onto and released on top of the 
        /// AspectDroppable element
        /// </summary>
        public event EventHandler<DroppedEventArgs> Dropped;

        /// <summary>
        /// Event raised after a Draggable element is dragged over an <see cref="AspectDroppable"/> element, 
        /// but before the resizing is handled by the <see cref="AspectDroppable"/>.
        /// </summary>
        public event EventHandler<DroppingEventArgs> Dropping;

        private string _hoverClass;
  
        #region [ -- Constructors -- ]

        /// <summary>
        /// Default constructor, sets no event handler for the Dropped event
        /// </summary>
        public AspectDroppable()
            : this(null)
        { }

        /// <summary>
        /// Constructor taking event handler for the Dropped event
        /// </summary>
        /// <param name="dropped">delegate called when widget with AspectDraggable is dropped onto this widget</param>
        public AspectDroppable(EventHandler<DroppedEventArgs> dropped)
            : this(dropped, null)
        { }

        /// <summary>
        /// Constructor taking event handler for the Dropped event and a string which declares the "hover class" (CSS
        /// class to be added to widget) when an AspectDraggable widget is "on top" of it.
        /// </summary>
        /// <param name="dropped">delegate called when widget with AspectDraggable is dropped onto this widget</param>
        /// <param name="hoverClass">CSS class to modify the Widget by when a draggable that's accepted is dragged 
        /// over it</param>
        public AspectDroppable(EventHandler<DroppedEventArgs> dropped, string hoverClass)
        {
            Dropped += dropped;
            _hoverClass = hoverClass;
        }

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// If set then the widget will change CSS class when a widget which is draggable is on top of it and would
        /// be dropped on top of it if dropped
        /// </summary>
        public string HoverClass
        {
            get { return _hoverClass; }
            set { _hoverClass = value; }
        }

        /// <summary>
        /// Set this to true if you have nested <see cref="AspectDroppable"/> instances and only want the deepest instance to receive the event. 
        /// Defaults to false. 
        /// </summary>
        public bool Greedy { get; set; }

        #endregion

        [Method]
        internal void DraggableDropped(string draggedId, int left, int top)
        {
            // try to find a draggable
            AspectDraggable aspectDraggable = null;
            foreach (var control in Manager.Instance.RegisteredAjaxControls)
            {
                if (control.Control.ClientID != draggedId) continue;
                var aspectableAjaxControl = (control as IAspectableAjaxControl);
                if (aspectableAjaxControl == null) break;
                aspectDraggable = aspectableAjaxControl.Aspects.Find<AspectDraggable>();
                break;
            }

            var idToPass = string.Empty;
            if (aspectDraggable != null)
                idToPass = aspectDraggable.IdToPass;

            var cancel = false;

            if (Dropping != null)
            {
                var eventArgs = new DroppingEventArgs(draggedId, idToPass, new Point(left, top));
                Dropping(GetSender(), eventArgs);
                cancel = eventArgs.Cancel;
            }

            if (aspectDraggable != null)
                cancel = aspectDraggable.HandleDropped(left, top, cancel, true /* droppedOnTargetContainer */);
            
            if (!cancel && Dropped != null)
                Dropped(GetSender(), new DroppedEventArgs(draggedId, idToPass));
        }

        #region [ -- IAspect Implementation -- ]

        string IAspect.GetScript()
        {
            return new RegisterAspect("Gaia.AspectDroppable", ParentControl.Control.ClientID)
                .AddPropertyIfTrue(_hoverClass != null, "hoverclass", _hoverClass)
                .AddPropertyIfTrue(Greedy, "greedy", Greedy)
                .ToString();
        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.LibraryScripts.jquery-ui.js", typeof(Manager), "", true);
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectDroppable.js", typeof(Manager), "Gaia.AspectDroppable.browserFinishedLoading", true);
     
        }

        #endregion

    }
}
