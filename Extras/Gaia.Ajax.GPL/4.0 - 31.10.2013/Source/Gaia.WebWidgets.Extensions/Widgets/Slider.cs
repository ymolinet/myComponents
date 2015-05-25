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
using System.ComponentModel;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    using HtmlFormatting;

    /// <summary>
    /// The Gaia Ajax Slider allows you to drag a handle and drop it to set the sliders value. 
    /// The Gaia Ajax Slider is used to represent a numeric value in the range from 0 to 100. It has a handle that
    /// you can drag back and forth then drop to set the Value of the %Slider.
    /// </summary>
    [Themeable(true)]
    [DefaultProperty("Value")]
    [ValidationProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [ToolboxData("<{0}:Slider runat=\"server\" />")]
    [ToolboxBitmap(typeof(Slider), "Resources.Gaia.WebWidgets.Extensions.Slider.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.SliderDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class Slider : GaiaWebControl, INamingContainer, ISkinControl, IAjaxContainerControl
    {
        #region [ -- Specialized classes -- ]

        /// <summary>
        /// Specialized <see cref="AspectDraggable"/> for the <see cref="Slider"/> control.
        /// </summary>
        /// <remarks>
        /// Uses undocumented feature of <see cref="AspectDraggable"/>, which enables:
        /// 1. getting and setting top and left coordinates in percentage units relative to offset parent.
        /// 2. use percentage units relative to offset parent for the bounding rectangle.
        /// </remarks>
        private sealed class AspectDraggableSlider : AspectDraggable
        {
            private readonly Slider _owner;

            /// <summary>
            /// Constructor taking event handler for the Dropped event.
            /// </summary>
            /// <param name="owner">Owner control.</param>
            /// <param name="dropped">Called when item is dropped on page.</param>
            public AspectDraggableSlider(Slider owner, EventHandler dropped) : base(dropped)
            {
                _owner = owner;
            }

            /// <summary>
            /// Returns aspect registration object required for registering aspect on the client.
            /// </summary>
            /// <param name="registerAspect">Suggested aspect registration object.</param>
            /// <returns>Modified or new aspect registration object.</returns>
            protected override RegisterAspect GetScript(RegisterAspect registerAspect)
            {
                return base.GetScript(registerAspect).
                    AddProperty("rel", "1", false).AddProperty("rect", "parent", true);
            }
        }

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="Slider"/>.
        /// </summary>
        public class PropertyStateManagerSlider : PropertyStateManagerWebControl, IMayRequireRerendering
        {
            private readonly Slider _slider;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerSlider"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerSlider(Slider control) : base(control)
            {
                _slider = control;
            }

            /// <summary>
            /// Gets or sets if the <see cref="Slider"/> was initially enabled.
            /// </summary>
            private bool Enabled { get; set; }

            /// <summary>
            /// <see cref="Direction"/> which the <see cref="Slider"/> was rendered with.
            /// </summary>
            internal Direction DisplayedDirection { get; set; }

            /// <summary>
            /// Returns true if associated <see cref="Slider"/> needs to be rerendered.
            /// </summary>
            bool IMayRequireRerendering.RequiresRerendering
            {
                get { return DisplayedDirection != _slider.DisplayDirection || Enabled != _slider.Enabled; }
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);

                var stateManager = source as PropertyStateManagerSlider;
                if (stateManager == null) return;

                Enabled = _slider.Enabled;
                DisplayedDirection = stateManager.DisplayedDirection;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                Enabled = _slider.Enabled;
                DisplayedDirection = _slider.DisplayDirection;
                base.TakeSnapshot();
            }
        }

        #endregion

        #region [ -- Enumerations -- ]

        /// <summary>
        /// Enumeration for direction the Slider is being rendered
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// Horizontal direction
            /// </summary>
            Horizontal,

            /// <summary>
            /// Vertical direction
            /// </summary>
            Vertical
        };

        #endregion

        private Label _label;
        private AjaxControl _instance;
        private AjaxContainerControl _ajaxContainerControl;

        #region [ -- Events and EventHandlers -- ]
        
        /// <summary>
        /// Raised when the <see cref="Value"/> property is changed.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Gets a <see cref="T:System.Web.UI.ControlCollection"/> object that represents the child controls for a specified server control in the UI hierarchy.
        /// </summary>
        /// <returns>
        /// The collection of child controls for the specified server control.
        /// </returns>
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Web server control is enabled.
        /// </summary>
        /// <returns>
        /// True if control is enabled; otherwise, false. The default is true.
        /// </returns>
        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                RequiresRecomposition();
            }
        }

        /// <summary>
        /// Gets or sets the rendering direction for the <see cref="Slider"/>.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(Direction.Horizontal)]
        [Description("Rendering direction.")]
        public Direction DisplayDirection
        {
            get { return StateUtil.Get(ViewState, "DisplayDirection", Direction.Horizontal); }
            set { SetStateValue("DisplayDirection", value, Direction.Horizontal); }
        }

        /// <summary>
        /// Override this if you've got skins that have another width of the "outer parts" to help the
        /// dragger know which offset values are its borders
        /// </summary>
        [Browsable(false)]
        [Description("Sets the width or height of the borders.")]
        [Obsolete("Does not affect Slider functionality anymore.")]
        public int BorderSize { get; set; }

        /// <summary>
        /// Gets or sets the value of the <see cref="Slider"/>.
        /// </summary>
        [DefaultValue(0D)]
        [Description("Slider value.")]
        public double Value
        {
            get { return StateUtil.Get(ViewState, "Value", 0D); }
            set
            {
                if (value < 0D || value > 100D)
                    throw new ArgumentOutOfRangeException("value", "Should be between 0 and 100");

                SetStateValue("Value", value, 0D);
            }
        }

        /// <summary>
        /// Gets or sets the width of the Web server control.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.UI.WebControls.Unit"/> that represents the width of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The width of the Web server control was set to a negative value.</exception>
        [DefaultValue(typeof(ASP.Unit), "200px")]
        public override ASP.Unit Width
        {
            get
            {
                if (DisplayDirection == Direction.Vertical)
                    return ASP.Unit.Empty;

                var baseWidth = base.Width;
                return baseWidth.IsEmpty ? ASP.Unit.Pixel(200) : baseWidth;
            }
            set { base.Width = value; }
        }

        /// <summary>
        /// Gets or sets the height of the Web server control.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.UI.WebControls.Unit"/> that represents the height of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The height was set to a negative value.</exception>
        [DefaultValue(typeof(ASP.Unit), "100px")]
        public override ASP.Unit Height
        {
            get
            {
                if (DisplayDirection == Direction.Horizontal)
                    return ASP.Unit.Empty;

                var baseHeight= base.Height;
                return baseHeight.IsEmpty ? ASP.Unit.Pixel(100) : baseHeight;
            }
            set { base.Height = value; }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.
        /// </returns>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        #endregion

        /// <summary>
        /// Sets specified <paramref name="value"/> into the <see cref="Control.ViewState"/>
        /// using specified <paramref name="key"/>.
        /// </summary>
        /// <remarks>
        /// The value is set if it's different from the current value.
        /// If the value if updated, <see cref="RequiresRecomposition"/> is called.
        /// </remarks>
        private void SetStateValue<T>(string key, T value, T defaultValue)
        {
            var initial = StateUtil.Get(ViewState, key, defaultValue);

            var comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(initial, value)) return;

            StateUtil.Set(ViewState, key, value, defaultValue);
            RequiresRecomposition();
        }

        /// <summary>
        /// Ensures <see cref="Control.ChildControlsCreated"/> is false.
        /// </summary>
        private void RequiresRecomposition()
        {
            if (!ChildControlsCreated) return;
            ChildControlsCreated = false;
        }

        #region [ -- Overridden base methods -- ]

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation
        /// to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            var wrapper = new ASP.Panel {CssClass = CombineCssClass("slider-wrapper")};

            _label = CreateLabel();
            wrapper.Controls.Add(_label);

            var closer = new ASP.Panel {CssClass = CombineCssClass("slider-closer")};
            closer.Controls.Add(wrapper);

            Controls.Add(closer);
            base.CreateChildControls();
        }

        /// <summary>
        /// Overridden to ensure ChildControls in the composition control. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            EnsureChildControls();
            base.OnLoad(e);
        }

        /// <summary>
        /// See <see cref="AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (!Enabled)
            {
                EnsureChildControls();
                _label.Aspects.Remove<AspectDraggableSlider>();
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Renders the Sliders HTML properly. 
        /// </summary>
        /// <param name="create">XhtmlTagFactory passed from caller</param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            if (DesignMode)
                EnsureChildControls();

            var directionCssClass = DisplayDirection == Direction.Horizontal ? "slider-horizontal" : "slider-vertical";
            using (var div = create.Div(ClientID,  Css.Combine(CssClass, false, "slider", directionCssClass))) // root element
            {
                Css.SerializeAttributesAndStyles(this, div);
                RenderChildren(create.GetHtmlTextWriter());
            }
        }

        #endregion

        #region [ -- Helpers -- ]

        /// <summary>
        /// Creates the draggable <see cref="Label"/>.
        /// </summary>
        private Label CreateLabel()
        {
            var cssClass = Enabled ? CombineCssClass("slider-handle") : CombineCssClass("slider-disabled");
            var label = new Label {ID = "h", CssClass = cssClass};
            SetLabelStyles(label.Style);
            label.Aspects.Add(new AspectDraggableSlider(this, DraggableDropped));
            return label;
        }

        /// <summary>
        /// Sets <see cref="Label"/> positional styles.
        /// </summary>
        private void SetLabelStyles(CssStyleCollection styleCollection)
        {
            var value = Value;
            var unit = value <= 50D ? ASP.Unit.Percentage(value) : ASP.Unit.Percentage(100D - value);

            if (DisplayDirection == Direction.Horizontal)
            {
                var key = value <= 50D ? "left" : "right";

                styleCollection[HtmlTextWriterStyle.Top] = "0%";
                styleCollection[key] = unit.ToString(NumberFormatInfo.InvariantInfo);
            }
            else
            {
                var key = value <= 50D ? "top" : "bottom";

                styleCollection[HtmlTextWriterStyle.Left] = "0%";
                styleCollection[key] = unit.ToString(NumberFormatInfo.InvariantInfo);
            }
        }

        /// <summary>
        /// Called when the <see cref="Label"/> is dropped.
        /// </summary>
        private void DraggableDropped(object sender, EventArgs e)
        {
            var label = (Label)((AspectDraggable)sender).ParentControl;
            
            var stateManager = (PropertyStateManagerSlider)AjaxContainerControl.StateManager;
            var displayDirection = stateManager.DisplayedDirection;
            var key = displayDirection == Direction.Horizontal ? "left" : "top";

            double value;
            var styleCollection = label.Style;
            var styleValue = styleCollection[key];
            
            if (styleValue == null)
            {
                key = DisplayDirection == Direction.Horizontal ? "right" : "bottom";
                value = 100 - ASP.Unit.Parse(label.Style[key], CultureInfo.InvariantCulture).Value;
            }
            else
                value = ASP.Unit.Parse(label.Style[key], CultureInfo.InvariantCulture).Value;

            if (Math.Abs(Value - value) < double.Epsilon) return;

            Value = value;
            if (ValueChanged == null) return;
            ValueChanged(this, EventArgs.Empty);
        }

        private string CombineCssClass(params string[] cssclass)
        {
            return Css.Combine(CssClass, cssclass);
        } 

        #endregion

        #region [ -- ISkinControl Implementation -- ]

        void ISkinControl.ApplySkin()
        {
            // include default skin css file
            AjaxContainerControl.RegisterDefaultSkinStyleSheetFromResource(typeof(Slider), Constants.DefaultSkinResource);

            // name of the default skin;
            CssClass = Constants.DefaultSkinCssClass;
        }

        bool ISkinControl.Enabled
        {
            get { return string.IsNullOrEmpty(CssClass) || CssClass.Equals(Constants.DefaultSkinCssClass); }
        }

        #endregion

        #region [ -- IAjaxControl and IAjaxContainerControl implementation -- ]

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerSlider(this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.ContainerWebControl", ClientID).AddAspects(Aspects).AddEffects(Effects).ToString();
        }

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new AjaxContainerControl(this)); }
        }

        private AjaxContainerControl AjaxContainerControl
        {
            get { return _ajaxContainerControl ?? (_ajaxContainerControl = ((IAjaxContainerControl)this).AjaxContainerControl); }
        }

        void IAjaxContainerControl.ForceAnUpdate()
        {
            AjaxContainerControl.ForceAnUpdate();
        }

        void IAjaxContainerControl.ForceAnUpdateWithAppending()
        {
            throw new NotSupportedException();
        }

        void IAjaxContainerControl.TrackControlAdditions()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Called when ForceAnUpdate is dispatched for a control and the
        /// control needs to re-render its child control collection.
        /// </summary>
        void IAjaxContainerControl.RenderChildrenOnForceAnUpdate(XhtmlTagFactory create)
        {
            RenderContents(create.GetHtmlTextWriter());
        }

        /// <summary>
        /// Retrieves actual AjaxContainerControl associated with the Control
        /// </summary>
        AjaxContainerControl IAjaxContainerControl.AjaxContainerControl
        {
            get { return (AjaxContainerControl)((IAjaxContainerControl)this).AjaxControl; }
        }

        /// <summary>
        /// Returns id of the DOM element which acts as the actual container
        /// for the specified child. Used during dynamic rendering.
        /// </summary>
        /// <param name="child">Child control to get container for</param>
        /// <returns>ID of the DOM element which should contain specified child</returns>
        string IAjaxContainerControl.GetDOMContainer(IAjaxControl child)
        {
            return ClientID;
        }

        #endregion
    }
}
