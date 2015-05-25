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
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectDraggable.js", "text/javascript")]
namespace Gaia.WebWidgets
{
    /// <summary>
    /// Aspect class for making elements draggable or movable.
    /// Element you attach this Aspect to can be dragged around on screen with mouse. See AspectDroppable
    /// for how to make it possible to track specific places they are dropped.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for AspectDraggable Example" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectDraggable\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Adding Draggable capabilities to ANY Gaia control" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectDraggable\Overview\Default.aspx.cs" region="Code" />
    /// </code>
    /// </example>
    public class AspectDraggable : Aspect<AspectDraggable>,IAspect
    {
        #region [ -- Effect Events -- ]

        /// <summary>
        /// Use this EffectEvent to wire up an effect at the start of the drag process. 
        /// </summary>
        public static AjaxEffectEvent EffectEventStartDragging { get { return AjaxEffectEventFactory.Create("gaiastartdrag"); } }

        /// <summary>
        /// Use this EffectEvent to wire up an effect at the end of the dragging
        /// </summary>
        public static AjaxEffectEvent EffectEventEndDragging { get { return AjaxEffectEventFactory.Create("gaiaenddrag"); } }

        /// <summary>
        /// Use this EffectEvent to wire up an effect when the element is reverted back to it's original location.
        /// Gaia.WebWidgets.EffectMove is parametrized to provide interesting effects when the item is reverted.
        /// </summary>
        /// <example>
        /// <code title="Use Top and Left in ClickedEventArgs through relative coordinates" lang="C#"> 
        /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\Aspects\Draggable\Default.aspx.cs" region="EffectEventReverting" /> 
        /// </code> 
        /// </example>
        public static AjaxEffectEvent EffectEventReverting { get { return new RevertEffectEvent(); } }

        #endregion

        /// <summary>
        /// Provides data for <see cref="AspectDraggable.Dropping"/> event.
        /// </summary>
        public sealed class DroppingEventArgs : CancellablePositionEventArgs
        {
            /// <summary>
            /// If true, indicates that the element was dragged successfully over an element which was constructed with
            /// AspectDroppable. This can be useful to know in certain circumstances.
            /// </summary>
            public bool DroppedOnTargetContainer { get; private set; }

            /// <summary>
            /// Initializes new instance of <see cref="AspectDraggable.Dropping"/> class.
            /// </summary>
            /// <param name="position">Drop position.</param>
            /// <param name="droppedOnTargetContainer">True if widgets was dropped on target container. Otherwise, false.</param>
            public DroppingEventArgs(Point position, bool droppedOnTargetContainer) : base(position)
            {
                DroppedOnTargetContainer = droppedOnTargetContainer;
            }
        }

        /// <summary>
        /// Snapping configuration for the <see cref="AspectDraggable"/>.
        /// </summary>
        public sealed class SnapConfiguration : SnapConfigurationBase
        {
            /// <summary>
            /// Specifies CSS Selector for the DOM elements.
            /// The Control can be only snapped to the selected DOM elements.
            /// </summary>
            public string TargetSelector { get; set; }

            /// <summary>
            /// Returns string representation of this configuration for registering as client-side option.
            /// </summary>
            /// <returns></returns>
            internal override string ToRegistrationOption()
            {
                var targets = TargetSelector;
                return !string.IsNullOrEmpty(targets) ? "'" + targets + "'" : base.ToRegistrationOption();
            }
        }

        /// <summary>
        /// Defines revert effect.
        /// </summary>
        private sealed class RevertEffectEvent : AjaxEffectEvent
        {
            public override string FunctionName
            {
                get { return "gaiadragreverting"; }
            }

            public override IEnumerable<KeyValuePair<string, string>> GetParameters()
            {
                yield return new KeyValuePair<string, string>("x", "x");
                yield return new KeyValuePair<string, string>("y", "y");
                yield return new KeyValuePair<string, string>("duration", "duration");
            }
        }

        private decimal _effect;
        private SnapConfiguration _snap;

        /// <summary>
        /// Set HitEffect to true if you want to visualize the Element which this item is dropped onto.
        /// When HitEffect is set to true it also has the nice side effect of disabling reverts which 
        /// may be desireable in many dragdrop scenarios. You can specify the HitEffect
        /// with the <see cref="AspectDroppable.EffectEventDropped"/> EffectEvent. 
        /// </summary>
        public bool HitEffect { get; set; }

        /// <summary>
        /// This event is fired immediately when the element is starting the drag operation. It can be useful 
        /// to subscribe to this event to "set the stage" for the draggable operation to take place. 
        /// </summary>
        public event EventHandler BeginDrag;

        /// <summary>
        /// Event raised when element dropped after a dragging operation. If you have created
        /// a container with AspectDroppable, both the Dropped events will be fired in the 
        /// same callback, first this Event and then the Dropped event of AspectDroppable. 
        /// The Left and Top styles reflect the new position of the element after it was 
        /// dropped
        /// </summary>
        public event EventHandler Dropped;
        
        ///<summary>
        /// Raised before the <see cref="ASP.WebControl.Style"/> is updated.
        ///</summary>
        public event EventHandler<DroppingEventArgs> Dropping;

        #region [ -- Constructors -- ]

        /// <summary>
        /// Default constructor, adds no Event Handler for the dropped method, uses all "default properties"
        /// </summary>
        public AspectDraggable() : this(null) { }

        /// <summary>
        /// Constructor taking event handler for the Dropped event
        /// </summary>
        /// <param name="dropped">delegate called when item is dropped on page</param>
        public AspectDraggable(EventHandler dropped) : this(dropped, Rectangle.Empty) { }

        /// <summary>
        /// Constructor taking event handler for the Dropped event in addition to a Rectangle which defines
        /// the boundaries of where the user can drag the item.
        /// </summary>
        /// <param name="dropped">delegate called when item is dropped on page</param>
        /// <param name="boundingRect">Rectangle from which to constrain movements within. To make it only draggable
        /// in one axis set both other axis values to 0.</param>
        public AspectDraggable(EventHandler dropped, Rectangle boundingRect) : this(dropped, boundingRect, false) { }

        /// <summary>
        /// Constructor taking event handler for the Dropped event in addition to a Rectangle which defines
        /// the boundaries of where the user can drag the item and a boolean value which says if the 
        /// dragging operation should be reverted when the user lets go of the widget.
        /// </summary>
        /// <param name="dropped">delegate called when item is dropped on page</param>
        /// <param name="boundingRect">Rectangle from which to constrain movements within</param>
        /// <param name="revert">If true Draggable will "animate" back to the former place after being dropped, default value is false</param>
        public AspectDraggable(EventHandler dropped, 
            Rectangle boundingRect, 
            bool revert)
            : this(dropped, boundingRect, revert, false)
        { }

        /// <summary>
        /// Constructor taking event handler for the Dropped event in addition to a Rectangle which defines
        /// the boundaries of where the user can drag the item, a boolean value which says if the 
        /// dragging operation should be reverted when the user lets go of the widget and a boolean value which if
        /// true says that the logic shouldn't check for hits on AspectDroppable widgets. To set this value
        /// to true might seriously optimize the JavaScript on the client-side.
        /// </summary>
        /// <param name="dropped">delegate called when item is dropped on page</param>
        /// <param name="boundingRect">Rectangle from which to constrain movements within</param>
        /// <param name="revert">If true Draggable will "animate" back to the former place after being dropped</param>
        /// <param name="silent">If true Draggable "silent" meaning it will not look for droppables before dropped 
        /// (and thereby not give visual clues about which droppables it is actually over), the default value of this 
        /// parameter is false</param>
        public AspectDraggable(EventHandler dropped, 
            Rectangle boundingRect, 
            bool revert, 
            bool silent)
            : this(dropped, boundingRect, revert, silent, 1.0M)
        { }

        /// <summary>
        /// Constructor taking event handler for the Dropped event in addition to a Rectangle which defines
        /// the boundaries of where the user can drag the item, a boolean value which says if the 
        /// dragging operation should be reverted when the user lets go of the widget, a boolean value which if
        /// true says that the logic shouldn't check for hits on AspectDroppable widgets and a decimal value
        /// which must be between 0.0 and 1.0 which defines the amount of transparency the widget will have
        /// while being dragged around on the client.
        /// </summary>
        /// <param name="dropped">delegate called when item is dropped on page</param>
        /// <param name="boundingRect">Rectangle from which to constrain movements within</param>
        /// <param name="revert">If true Draggable will "animate" back to the former place after being dropped</param>
        /// <param name="silent">If true Draggable "silent" meaning it will not look for droppables before dropped (and thereby not give visual clues about which droppables it is actually over)</param>
        /// <param name="effect">If not 1.0 then Widget will be semi transparent while being dragged according to the value 
        /// between 0.0 and 1.0 where 0.0 is 100% invisible and 1.0 is 100% visible. Default value of this property
        /// is 1.0M</param>
        public AspectDraggable(EventHandler dropped, 
            Rectangle boundingRect, 
            bool revert, 
            bool silent, 
            decimal effect)
            : this(dropped, boundingRect, revert, silent, effect, string.Empty)
        { }

        /// <summary>
        /// Constructor taking event handler for the Dropped event in addition to a Rectangle which defines
        /// the boundaries of where the user can drag the item, a boolean value which says if the 
        /// dragging operation should be reverted when the user lets go of the widget, a boolean value which if
        /// true says that the logic shouldn't check for hits on AspectDroppable widgets, a decimal value
        /// which must be between 0.0 and 1.0 which defines the amount of transparency the widget will have
        /// while being dragged around on the client, a boolean value stating if the widget should "animate" when
        /// dropped on an accepting AspectDroppable and a "handle" which if non-null defines a DOM element ID by which 
        /// the widget will be draggable from.
        /// </summary>
        /// <param name="dropped">delegate called when item is dropped on page</param>
        /// <param name="boundingRect">Rectangle from which to constrain movements within</param>
        /// <param name="revert">If true Draggable will "animate" back to the former place after being dropped</param>
        /// <param name="silent">If true Draggable "silent" meaning it will not look for droppables before dropped (and thereby not give visual clues about which droppables it is actually over)</param>
        /// <param name="effect">If not 1.0 then Widget will be semi transparent while being dragged according to the value 
        /// between 0.0 and 1.0 where 0.0 is 100% invisible and 1.0 is 100% visible. Default value of this property
        /// is 1.0M</param>
        /// <param name="handle">Specifies the handle element id for dragging</param>
        public AspectDraggable(EventHandler dropped, 
            Rectangle boundingRect, 
            bool revert, 
            bool silent, 
            decimal effect, 
            string handle)
            : this(dropped, boundingRect, revert, silent, effect, handle, null)
        { }

        /// <summary>
        /// Constructor taking event handler for the Dropped event in addition to a Rectangle which defines
        /// the boundaries of where the user can drag the item, a boolean value which says if the 
        /// dragging operation should be reverted when the user lets go of the widget, a boolean value which if
        /// true says that the logic shouldn't check for hits on AspectDroppable widgets, a decimal value
        /// which must be between 0.0 and 1.0 which defines the amount of transparency the widget will have
        /// while being dragged around on the client, a boolean value stating if the widget should "animate" when
        /// dropped on an accepting AspectDroppable, a "handle" which if non-null defines a DOM element ID by which 
        /// the widget will be draggable from and an "ID" to pass into the Dropped event handler when widget is dropped.
        /// </summary>
        /// <param name="dropped">delegate called when item is dropped on page</param>
        /// <param name="boundingRect">Rectangle from which to constrain movements within</param>
        /// <param name="revert">If true Draggable will "animate" back to the former place after being dropped</param>
        /// <param name="silent">If true Draggable "silent" meaning it will not look for droppables before dropped (and thereby not give visual clues about which droppables it is actually over)</param>
        /// <param name="effect">If not 1.0 then Widget will be semi transparent while being dragged according to the value 
        /// between 0.0 and 1.0 where 0.0 is 100% invisible and 1.0 is 100% visible. Default value of this property
        /// is 1.0M</param>
        /// <param name="handle">Specifies the handle element id for dragging</param>
        /// <param name="idToPass">Id which will be passed into the Dropped event handler if one is defined. This can be very useful
        /// for figuring out which item was being dropped when you pair the AspectDraggable together with an AspectDroppable.</param>
        public AspectDraggable(EventHandler dropped, 
            Rectangle boundingRect, 
            bool revert, 
            bool silent, 
            decimal effect, 
            string handle, 
            string idToPass)
        {
            if (effect < 0.0M || effect > 1.0M)
                throw new ArgumentException("Value of effect parameter can't be less than 0 or more than 1");
            Dropped += dropped;
            Rectangle = boundingRect;
            Revert = revert;
            Silent = silent;
            _effect = effect;
            Handle = handle;
            IdToPass = idToPass;
        }

        /// <summary>
        /// Constructor taking event handler for the Dropped event in addition to a Rectangle which defines
        /// the boundaries of where the user can drag the item, a boolean value which says if the 
        /// dragging operation should be reverted when the user lets go of the widget, a boolean value which if
        /// true says that the logic shouldn't check for hits on AspectDroppable widgets, a decimal value
        /// which must be between 0.0 and 1.0 which defines the amount of transparency the widget will have
        /// while being dragged around on the client, a boolean value stating if the widget should "animate" when
        /// dropped on an accepting AspectDroppable and a "handle" which if non-null defines a Control by which 
        /// the widget will be draggable from.
        /// </summary>
        /// <param name="dropped">delegate called when item is dropped on page</param>
        /// <param name="boundingRect">Rectangle from which to constrain movements within</param>
        /// <param name="revert">If true Draggable will "animate" back to the former place after being dropped</param>
        /// <param name="silent">If true Draggable "silent" meaning it will not look for droppables before dropped (and thereby not give visual clues about which droppables it is actually over)</param>
        /// <param name="effect">If not 1.0 then Widget will be semi transparent while being dragged according to the value 
        /// between 0.0 and 1.0 where 0.0 is 100% invisible and 1.0 is 100% visible. Default value of this property
        /// is 1.0M</param>
        /// <param name="hitEffect">If set to true will add up an effect on the target droppable</param>
        /// <param name="handleControl">Specifies the handle Control for dragging</param>
        public AspectDraggable(EventHandler dropped, 
            Rectangle boundingRect, 
            bool revert, 
            bool silent, 
            decimal effect,
            bool hitEffect,
            Control handleControl)
        {
            if (effect < 0.0M || effect > 1.0M)
                throw new ArgumentException("Value of effect parameter can't be less than 0 or more than 1");
            Dropped += dropped;
            HitEffect = hitEffect;
            Rectangle = boundingRect;
            Revert = revert;
            Silent = silent;
            _effect = effect;
            HandleControl = handleControl;
        }

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Rectangular constraints for how much and where the widget can be dragged around. To set it to 
        /// only draggable horizontally for instance set this value to; new Rectangle(-4000, 0, 4000, 0)
        /// </summary>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// If true then when the widget is dropped the widget will be reverted back to its original position
        /// </summary>
        public bool Revert { get; set; }

        /// <summary>
        /// If true then while dragged the dragging logic will avoid to check for intersections against 
        /// widgets with the AspectDroppable aspect. This is a big optimalization, so use it if you don't have
        /// an AspectDroppable attached which is listening in on Dropped events.
        /// </summary>
        public bool Silent { get; set; }

        /// <summary>
        /// Defines the amount of transparency the widget will have when being dragged on the surface. 
        /// 0.0 means completely in-visible and 1.0 means completely visible (no transparency)
        /// You can also use the EffectEvents StartDragging + EndDragging to have a custom start effect
        /// and custom end effect which offers much more granularity on the draggable effect.
        /// </summary>
        public decimal Effect
        {
            get { return _effect; }
            set
            {
                if (value < 0.0M || value > 1.0M)
                    throw new ArgumentException("Value can't be less than 0 or more than 1");
                _effect = value;
            }
        }

        /// <summary>
        /// When set to true a deep copy of the element will be created to be dragged when used either with
        /// <see cref="UseDocumentBody"></see> or <see cref="MakeGhost"></see> properties. 
        /// Does not have an effect if standard drag and drop behavior is used. When set to false, will only 
        /// create a shallow copy of the draggable element and that means only the root element is copied. 
        /// Can be used together with <see cref="DragCssClass"></see> for interesting effect. 
        /// </summary>
        public bool DeepCopy { get; set; }

        /// <summary>
        /// When set to true will create a ghost by cloning the draggable element. This creates the interesting
        /// effect of keeping the draggable control in it's place, while you drag around a proxy. Can be used 
        /// together with <see cref="UseDocumentBody"></see> and <see cref="DeepCopy"></see> for various effects. 
        /// </summary>
        /// <example>
        /// <code title="Ghosting" lang="C#"> 
        /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectDraggable\Ghosting\Default.aspx.cs" region="Code" />
        /// </code>
        /// </example>
        public bool MakeGhost { get; set; }

        /// <summary>
        /// When set to true, will add the element to the Body of the Document while dragging. This 
        /// fixes some browser issues and also makes it easier to drag and drop between different Gaia controls
        /// on the page since it doesn't care about different overflow settings, stacking contexts, etc.
        /// </summary>
        /// <value><c>true</c> if [use document body]; otherwise, <c>false</c>.</value>
        /// <example>
        /// <code title="Using the Document Body for Drag and Drop" lang="C#"> 
        /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\TreeView\DragAndDrop\Default.aspx.cs" region="Make Draggable" />
        /// </code>
        /// </example>
        public bool UseDocumentBody { get; set; }

        /// <summary>
        /// Specify a CssClass to be applied while the Element is being dragged. The CssClass will be added
        /// regardless of Draggable configuration and removed immediately on drop. 
        /// </summary>
        public string DragCssClass { get; set; }

        /// <summary>
        /// Specifies client ID of the element in the DOM tree which will serve as a handle for dragging
        /// </summary>
        public string Handle { get; set; }

        /// <summary>
        /// Specifies Control which will serve as an handle for dragging. The Handle control must 
        /// be a child of Draggable control
        /// </summary>
        public Control HandleControl { get; set; }

        /// <summary>
        /// Special ID to pass into AspectDroppable event handler if one is defined
        /// </summary>
        /// <example>
        /// <code title="Passing a custom ID when the Gaia control is dropped" lang="C#"> 
        /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectDroppable\Overview\Default.aspx.cs" region="Code" />
        /// </code>
        /// </example>
        public string IdToPass { get; set; }

        /// <summary>
        /// Specifies Snap configuration to use during dragging.
        /// </summary>
        public SnapConfiguration Snap
        {
            get { return _snap ?? (_snap = new SnapConfiguration());  }
        }

        #endregion

        [Method]
        internal void DroppedMethodRel(double left, double top)
        {
            const bool droppedOnTargetContainer = false;
            const bool droppingCancelled = false;
            HandleDropped(left, top, droppingCancelled,droppedOnTargetContainer, ASP.UnitType.Percentage);
        }

        [Method]
        internal void DroppedMethod(int left, int top)
        {
            const bool droppedOnTargetContainer = false;
            const bool droppingCancelled = false;
            HandleDropped(left, top, droppingCancelled, droppedOnTargetContainer);
        }

        [Method]
        internal void BeginDragMethod()
        {
            EventHandler handler = BeginDrag;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        internal bool HandleDropped(double left, double top, bool droppingCancelled, bool droppedOnTargetContainer, ASP.UnitType unitType = ASP.UnitType.Pixel)
        {
            var parentControl = ParentControl as ASP.WebControl;
            if (parentControl == null) return droppingCancelled;

            var cancel = droppingCancelled;
            var styleCollection = parentControl.Style;

            // hereafter we use both string and HtmlTextWriterStyle keys
            // because of inconsistent behavior of CssStyleCollection.

            string originalTop = null;
            string originalTopUsingKey = null;
            
            string originalLeft = null;
            string originalLeftUsingKey = null;

            if (!cancel && Dropping != null)
            {
                originalTop = styleCollection["top"];
                originalTopUsingKey = styleCollection[HtmlTextWriterStyle.Top];

                originalLeft = styleCollection["left"];
                originalLeftUsingKey = styleCollection[HtmlTextWriterStyle.Left];

                var evtArgs = new DroppingEventArgs(new Point((int)left, (int)top), droppedOnTargetContainer);
                Dropping(GetSender(), evtArgs);
                cancel = evtArgs.Cancel;
            }

            if (unitType != ASP.UnitType.Percentage)
            {
                var topValue = new ASP.Unit(top, unitType).ToString(NumberFormatInfo.InvariantInfo);
                styleCollection["top"] = topValue;
                styleCollection[HtmlTextWriterStyle.Top] = topValue;

                var leftValue = new ASP.Unit(left, unitType).ToString(NumberFormatInfo.InvariantInfo);
                styleCollection["left"] = leftValue;
                styleCollection[HtmlTextWriterStyle.Left] = leftValue;
            }
            else
                UpdatePercentageStyles(styleCollection, left, top);

            var aspectableAjaxControl = (IAspectableAjaxControl)parentControl;
            var stateManager = aspectableAjaxControl.StateManager as PropertyStateManagerWebControl;
            if (stateManager != null)
            {
                if (unitType == ASP.UnitType.Percentage)
                {
                    stateManager.ClearDirtyStyle("left", "top", "right", "bottom");
                    stateManager.ClearDirtyStyle(HtmlTextWriterStyle.Left, HtmlTextWriterStyle.Top);
                }

                stateManager.ClearDirtyStyle("left", "top");
                stateManager.ClearDirtyStyle(HtmlTextWriterStyle.Left, HtmlTextWriterStyle.Top);
            }

            if (cancel)
            {
                styleCollection["top"] = originalTop;
                styleCollection[HtmlTextWriterStyle.Top] = originalTopUsingKey;

                styleCollection["left"] = originalLeft;
                styleCollection[HtmlTextWriterStyle.Left] = originalLeftUsingKey;
            }
            else if (Dropped != null)
                Dropped(GetSender(), EventArgs.Empty);

            return cancel;
        }

        /// <summary>
        /// Sets specified <paramref name="left"/> and <paramref name="top"/> into the specified <paramref name="styleCollection"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="CssStyleCollection"/> keys used may be either left/right or top/bottom depending of the parameter values.
        /// </remarks>
        private static void UpdatePercentageStyles(CssStyleCollection styleCollection, double left, double top)
        {
            if (left <= 50D)
            {
                var leftValue = ASP.Unit.Percentage(left).ToString(NumberFormatInfo.InvariantInfo);
                styleCollection["left"] = leftValue;
                styleCollection[HtmlTextWriterStyle.Left] = leftValue;

                styleCollection.Remove("right");
            }
            else
            {
                styleCollection["right"] = ASP.Unit.Percentage(100D - left).ToString(NumberFormatInfo.InvariantInfo);
                
                styleCollection.Remove("left");
                styleCollection.Remove(HtmlTextWriterStyle.Left);
            }

            if (top <= 50D)
            {
                var topValue = ASP.Unit.Percentage(top).ToString(NumberFormatInfo.InvariantInfo);
                styleCollection["top"] = topValue;
                styleCollection[HtmlTextWriterStyle.Top] = topValue;

                styleCollection.Remove("bottom");
            }
            else
            {
                styleCollection["bottom"] = ASP.Unit.Percentage(100D - top).ToString(NumberFormatInfo.InvariantInfo);
                
                styleCollection.Remove("top");
                styleCollection.Remove(HtmlTextWriterStyle.Top);
            }
        }

        #region [ -- IAspect Implementation -- ]

        int GetProperties()
        {
            return
                (HasDropEvent ? 1 : 0) |
                (Revert ? 2 : 0) |
                (DeepCopy ? 4 : 0) |
                (MakeGhost ? 8 : 0) |
                (UseDocumentBody ? 16 : 0) |
                (HitEffect ? 32 : 0) |
                (Silent ? 64 : 0) |
                (HasBeginDragEvent ? 128 : 0);
        }

        bool HasDropEvent { get { return Dropped != null || Dropping != null; } }
        bool HasBeginDragEvent { get { return BeginDrag != null; } }

        /// <summary>
        /// Returns aspect registration object required for registering aspect on the client.
        /// </summary>
        /// <param name="registerAspect">Suggested aspect registration object.</param>
        /// <returns>Modified or new aspect registration object.</returns>
        protected virtual RegisterAspect GetScript(RegisterAspect registerAspect)
        {
            VerifyHandle(HandleControl);
            var snap = Snap.ToRegistrationOption();
            var handle = HandleControl == null ? Handle : HandleControl.ClientID;

            return registerAspect
                .AddProperty("p", GetProperties())
                .AddPropertyIfTrue(!string.IsNullOrEmpty(DragCssClass), "dragClass", DragCssClass)
                .AddPropertyIfTrue(_effect != 1.0M, "opacity", _effect.ToString("0.##", CultureInfo.InvariantCulture))
                .AddPropertyIfTrue(!string.IsNullOrEmpty(handle), "handle", handle)
                .AddPropertyIfTrue(!string.IsNullOrEmpty(snap), "sp", snap, false)
                .AddPropertyIfTrue(!Rectangle.IsEmpty, "rect",
                                   string.Concat("{top:", Rectangle.Top, ",left:", Rectangle.Left,
                                                 ",right:", Rectangle.Right, ",bottom:", Rectangle.Bottom, "}"), false);

        }

        string IAspect.GetScript()
        {
            return GetScript(new RegisterAspect("Gaia.AspectDraggable", ParentControl.Control.ClientID)).ToString();
        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.LibraryScripts.jquery-ui.js", typeof(Manager), "Droppables.browserFinishedLoading", true);
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectDraggable.js", typeof(Manager), "Gaia.AspectDraggable.browserFinishedLoading", true);
        }

        #endregion

        private void VerifyHandle(Control handle)
        {
            if (handle == null) return;
            
            var parent = handle.Parent;
            while (parent != null)
            {
                if (parent == ParentControl)
                    return;

                parent = parent.Parent;
            }

            throw new Exception("Handle Control should be child of Draggable Control");
        }
    }
}
