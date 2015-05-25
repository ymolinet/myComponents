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
using System.Text;
using System.Web.UI;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    using Effects;
    using HtmlFormatting;

    /// <summary>
    /// The Gaia Ajax Window basically mimics the behavior of a normal desktop Window. You can set it to be 
    /// Modal (through AspectModal), Closable, Movable, Resizable and mostly all other properties you'd expect 
    /// to get from a normal desktop Window.
    /// The Gaia Ajax %Window is a very rich Ajax Control. You can set properties for enabling and disabling resizing, 
    /// moving, closing, maximizing, minimizing and modality
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for Window" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\Window\Overview\Default.aspx"/>
    /// </code> 
    /// <code title="Codebehind for Window" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\Window\Overview\Default.aspx.cs"/>
    /// </code> 
    /// <code title="Recursively create Windows programatically" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\Window\Nested\Default.aspx.cs" region="Code"/>
    /// </code> 
    /// <code title="Enhance Window with Custom Effects" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Effects\Various\EnhancedWindow\Default.aspx.cs" region="Code"/>
    /// </code> 
    /// </example>
    [ParseChildren(false)]
    [PersistChildren(true)]
    [ToolboxData("<{0}:Window runat=\"server\"></{0}:Window>")]
    [ToolboxBitmap(typeof(Window), "Resources.Gaia.WebWidgets.Extensions.Window.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.WindowDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class Window : HybridPanelBase, ISkinControl, IExtraPropertyCallbackRenderer
    {
        private const string CssClassCaption = "window-top-text";
        private const string CssClassCloseAction = "item-close";
        private const string CssClassRestoreAction = "item-restore";
        private const string CssClassMinimizeAction = "item-minimize";
        private const string CssClassMaximizeAction = "item-maximize";
        
        private const string CloseCommandName = "c";
        private const string MaximizeOrRestoreCommandName = "mx";
        private const string MinimizeOrRestoreCommandName = "mn";

        private const string CloseId = "w-cs";
        private const string CaptionId = "w-cn";
        private const string ContentId = "content";
        private const string MinimizeId = "w-mn";
        private const string MaximizeId = "w-mx";

        #region [ -- Effect Events -- ]

        /// <summary>
        /// Use this function to apply an effect to the content area of the Window. 
        /// </summary>
        /// <typeparam name="T">Any Effect</typeparam>
        /// <param name="effect">The Effect to apply modifications to</param>
        /// <returns>The effect itself. Don't forget to add the effect to the Window effects collection</returns>
        public static T ApplyToContent<T>(T effect) where T : Effect
        {
            return EffectUtils.AppendElementID("_" + ContentId, effect);
        }

        /// <summary>
        /// Minimize rules for hooking effects to the Window.Minimize event. 
        /// <example>
        /// Effects.Add(Window.Minimize, Window.ApplyToContent(new EffectBlindUp()));
        /// </example>
        /// </summary>
        public static AjaxEffectEvent EffectEventMinimize { get { return AjaxEffectEventFactory.Create("gaiaminimizing"); } }

        /// <summary>
        /// Appear Effect rules for adding effects when the Window appear.  
        /// <example>
        /// Effects.Add(Window.Appear, new EffectGrow(ScriptaculousDirections.Center, ScriptaculousTransitions.Spring));
        /// </example>
        /// </summary>
        /// <remarks>
        /// Doesn't currently support EffectContainers like EffectQueue, EffectParallel. Use others instead
        /// </remarks>
        public static AjaxEffectEvent EffectEventAppear { get { return new WindowAppear(); } }

        /// <summary>
        /// Effect rules for applying an effect to the Window when it's restored after being minimized. 
        /// </summary>
        /// <example>
        /// Effects.Add(Window.RestoreAfterMinimize, Window.ApplyToContent(new EffectBlindDown()));
        /// </example>
        /// <remarks>
        /// Effect is renderes only when restoring after minimization.
        /// This effect will not fire when the <see cref="Window"/> is restored after being maximized.
        /// </remarks>
        public static AjaxEffectEvent EffectEventRestoreAfterMinimize { get { return new WindowRestoreAfterMinimize(); } }

        /// <summary>
        /// Close Effect rules for adding effects to the Window when it's being Closed/Visible false. 
        /// <example>
        /// Effects.Add(Window.Close, new EffectShrink());
        /// </example>
        /// </summary>
        /// <remarks>
        /// Doesn't currently support EffectContainers like EffectQueue, EffectParallel. Use others instead
        /// </remarks>
        public static AjaxEffectEvent EffectEventClose { get { return new WindowClose(); } }

        private class WindowAppear : AjaxEffectEvent
        {
            public override string FunctionName { get { return "gaiaappearing"; } }
            public override void VerifyEffect(Effect effect)
            {
                if (effect is IEffectContainer)
                    throw new ApplicationException("Window Appear Effect cannot be an EffectContainer");
            }
        }

        private class WindowClose : AjaxEffectEvent
        {
            public override string FunctionName { get { return "gaiaclosing"; } }

            public override IEnumerable<KeyValuePair<string, string>> GetParameters()
            {
                yield return new KeyValuePair<string, string>("afterFinish", "afterFinish");
            }

            public override void VerifyEffect(Effect effect)
            {
                if (effect is IEffectContainer)
                    throw new ApplicationException("Window Close Effect cannot be an EffectContainer");
            }
        }

        private  class WindowRestoreAfterMinimize : AjaxEffectEvent
        {
            public override string FunctionName
            {
                get { return "gaiarestoreafterminimize"; }
            }

            public override IEnumerable<KeyValuePair<string, string>> GetParameters()
            {
                yield return new KeyValuePair<string, string>("width", "w");
                yield return new KeyValuePair<string, string>("height", "h");
                yield return new KeyValuePair<string, string>("top", "t");
                yield return new KeyValuePair<string, string>("left", "l");
                yield return new KeyValuePair<string, string>("afterFinish", "afterFinish");

            }
        }

        #endregion

        #region [ -- Private members -- ]

        private bool _bringToFront;

        private Label _caption;
        private LinkButton _closer;
        private LinkButton _minimizer;
        private LinkButton _maximizer;
        
        private Action<Tag> _contentRenderCallback;
        private Action<ASP.WebControl> _minimizerCreatedCallback;
        private Action<ASP.WebControl> _maximizerCreatedCallback;

        #endregion

        #region [ -- Events and EventArgs -- ]
        
         /// <summary>
        /// Passed when a Gaia Window is being closed
        /// </summary>
        public class WindowClosingEventArgs : EventArgs
        {
             internal WindowClosingEventArgs()
             {
                 ShouldClose = true;
             }

             /// <summary>
             /// Set this one to false if you want to deny the user to actually close the window
             /// </summary>
             public bool ShouldClose { get; set; }
        }

        /// <summary>
        /// Event fired when window is being closed.
        /// Return false in WindowClosingEventArgs.ShouldClose if you need to stop the window from being closed.
        /// </summary>
        public event EventHandler<WindowClosingEventArgs> Closing;

        /// <summary>
        /// Event fired when window has been resized.
        /// </summary>
        public event EventHandler Resized;

        /// <summary>
        /// Event fired when window has been moved.
        /// </summary>
        public event EventHandler Moved;

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
        /// </summary>
        /// <returns>
        /// The CSS class rendered by the Web server control on the client. The default is <see cref="F:System.String.Empty"/>.
        /// </returns>
        public override string CssClass
        {
            get { return base.CssClass; }
            set
            {
                var initial = base.CssClass;
                base.CssClass = value;
                if (initial == value) return;
                RequiresRecomposition();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether validation is performed when the button controls are clicked.
        /// </summary>
        [Browsable(false)]
        [Themeable(false)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Obsolete("There is no operation which can cause validation in ExtendedPanel.")]
        public virtual bool CausesValidation
        {
            get { return StateUtil.Get(ViewState, "CausesValidation", false); }
            set { StateUtil.Set(ViewState, "CausesValidation", value, false); }
        }

        /// <summary>
        /// Gets or sets the group of controls for which this control causes validation when it posts back to the server.
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Category("Behavior")]
        [Obsolete("There is no operation which can cause validation in ExtendedPanel.")]
        public virtual string ValidationGroup
        {
            get { return StateUtil.Get(ViewState, "ValidationGroup", string.Empty); }
            set { StateUtil.Set(ViewState, "ValidationGroup", value, string.Empty); }
        }

        /// <summary>
        /// Gets or sets the duration of the animation when the <see cref="Window"/> is shown on the client.
        /// </summary>
        [DefaultValue(-1)]
        [Category("Behavior")]
        [Description("Window appearance animation duration in milliseconds.")]
        public int Animation
        {
            get { return StateUtil.Get(ViewState, "AnimationDuration", -1); }
            set { StateUtil.Set(ViewState, "AnimationDuration", value, -1); }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Window"/> should be modal.
        /// </summary>
        /// <remarks>
        /// More control over modality can be achieved by manually adding <see cref="AspectModal"/>
        /// to the Aspects collection.
        /// </remarks>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Modality of the Window.")]
        public bool Modal
        {
            get { return Aspects.Contains<AspectModal>(); }
            set { Aspects.Bind<AspectModal>(value); }
        }

        /// <summary>
        /// Gets or sets the transparency of the <see cref="Window"/> when it is being dragged.
        /// </summary>
        /// <remarks>
        /// Value of 0 means fully invisibility and 1 means full visibility.
        /// </remarks>
        [Category("Behavior")]
        [DefaultValue(typeof(decimal), "0.5")]
        [Description("Amount of opacity when Window is dragged.")]
        public decimal OpacityWhenMoved
        {
            get { return StateUtil.Get(ViewState, "OpacityWhenMoved", 0.5M); }
            set
            {
                if (value < 0.0M || value > 1.0M)
                    throw new ArgumentException("OpacityWhenMoved property out of range, must be between 0.0 and 1.0", "value");
                StateUtil.Set(ViewState, "OpacityWhenMoved", value, 0.5M);
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Window"/> is minimized.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        [AjaxSerializable("setMinimized")]
        [Description("Minimizes or restores the Window.")]
        public bool Minimized
        {
            get { return StateUtil.Get(ViewState, "Minimized", false); }
            set
            {
                SetStateValue("Minimized", value, false);
                if (!value || !Maximized) return;
                Maximized = false;
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Window"/> is maximized.
        /// </summary>
        /// <remarks>
        /// If the <see cref="Window"/> is nested inside other <see cref="IAjaxContainerControl"/>
        /// it will be maximized to occupy the whole content area of the outer container.
        /// Otherwise, it will be maximized to occupy all the client area.
        /// </remarks>
        [DefaultValue(false)]
        [Category("Behavior")]
        [AjaxSerializable("setMaximized")]
        [Description("Maximizes or restores the Window.")]
        public bool Maximized
        {
            get { return StateUtil.Get(ViewState, "Maximized", false); }
            set
            {
                SetStateValue("Maximized", value, false);
                if (!value || !Minimized) return;
                Minimized = false;
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Window"/> can be minimized.
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Enables minimizing of the Window.")]
        public bool Minimizable
        {
            get { return StateUtil.Get(ViewState, "Minimizable", true); }
            set { SetStateValue("Minimizable", value, true); }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Window"/> can be maximized.
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Enables maximizing of the Window.")]
        public bool Maximizable
        {
            get { return StateUtil.Get(ViewState, "Maximizable", true); }
            set { SetStateValue("Maximizable", value, true); }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Window"/> is centered on the form when it is being shown.
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Center the Window on the Form when initially shown.")]
        public bool CenterInForm
        {
            get { return StateUtil.Get(ViewState, "CenterInForm", true); }
            set { StateUtil.Set(ViewState, "CenterInForm", value, true); }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Window"/> can be closed.
        /// </summary>
        /// <seealso cref="Closing"/>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Enables closing of the Window.")]
        public bool Closable
        {
            get { return StateUtil.Get(ViewState, "Closable", true); }
            set { SetStateValue("Closable", value, true); }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Window"/> can be dragged.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Enables dragging of the Window.")]
        public bool Draggable
        {
            get { return StateUtil.Get(ViewState, "Draggable", false); }
            set
            {
                StateUtil.Set(ViewState, "Draggable", value, false);

                // This logic might look funny, with the whole OnLoad/Aspect/Initializing logic
                // However it is needed since first of all the Aspects must be RE-created BEFORE 
                // "Loading is finished" to fire events, secondly they also can be REMOVED in 
                // Event handlers (where Window is not being made visible before that Ajaxs Request)
                // in which case they should NOT be rendered and we need to remove them from the 
                // rendering. So I think this is the "way to do it"...
                if (!value)
                    Aspects.Remove<AspectDraggable>();
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Window"/> should be resizable.
        /// </summary>
        /// <seealso cref="MinWidth"/>
        /// <seealso cref="MinHeight"/>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Enables resizing of the Window.")]
        public bool Resizable
        {
            get { return StateUtil.Get(ViewState, "Resizable", false); }
            set
            {
                StateUtil.Set(ViewState, "Resizable", value, false);

                // This logic might look funny, with the whole OnLoad/Aspect/Initializing logic
                // However it is needed since first of all the Aspects must be RE-created BEFORE 
                // "Loading is finisihed" to fire events, secondly they also can be REMOVED in 
                // Event handlers (where Window is not being made visible before that Ajaxs Request)
                // in which case they should NOT be rendered and we need to remove them from the 
                // rendering. So I think this is the "way to do it"...
                if (!value)
                    Aspects.Remove<AspectResizable>();
            }
        }

         /// <summary>
         /// Gets or sets the minimum width the <see cref="Window"/> can be resized to.
         /// </summary>
         /// <remarks>
         /// <see cref="MinWidth"/> is ignored if <see cref="Resizable"/> property is set to false.
         /// </remarks>
         /// <seealso cref="Resizable"/>
        [Category("Behavior")]
        [DefaultValue(typeof(ASP.Unit), "200px")]
        [Description("Mimimum possible width of Window.")]
        public ASP.Unit MinWidth
        {
            get { return StateUtil.Get(ViewState, "MinWidth", ASP.Unit.Pixel(200)); }
            set { StateUtil.Set(ViewState, "MinWidth", value); }
        }

        /// <summary>
        /// Gets or sets the minimum height the <see cref="Window"/> can be resized to.
        /// </summary>
        /// <remarks>
        /// <see cref="MinHeight"/> is ignored if <see cref="Resizable"/> property is set to false.
        /// </remarks>
        /// <seealso cref="Resizable"/>
        [Category("Behavior")]
        [DefaultValue(typeof(ASP.Unit), "200px")]
        [Description("Minimum possible height of Window.")]
        public ASP.Unit MinHeight
        {
            get { return StateUtil.Get(ViewState, "MinHeight", ASP.Unit.Pixel(200)); }
            set { StateUtil.Set(ViewState, "MinHeight", value); }
        }

        /// <summary>
        /// Gets or sets the caption of the <see cref="Window"/>.
        /// </summary>
        [DefaultValue("")]
        [Localizable(true)]
        [Category("Appearance")]
        [Description("The caption of the Window.")]
        public string Caption
        {
            get { return StateUtil.Get(ViewState, "Caption", string.Empty); }
            set { SetStateValue("Caption", value, string.Empty); }
        }

        /// <summary>
        /// Gets or sets the css class for the icon in the title bar, next to the <see cref="Caption"/>.
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [Description("Css Class of the icon in the title bar.")]
        public string IconCssClass
        {
            get { return StateUtil.Get(ViewState, "IconCssClass", string.Empty); }
            set { StateUtil.Set(ViewState, "IconCssClass", value, string.Empty); }
        }

        ///<summary>
        ///Gets or sets the visibility and position of scroll bars in the window
        ///</summary>
        [DefaultValue(ASP.ScrollBars.Auto)]
        public override ASP.ScrollBars ScrollBars
        {
            get
            {
                if (!ControlStyleCreated || ViewState["ScrollBars"] == null)
                    return ASP.ScrollBars.Auto;

                return base.ScrollBars;
            }
            set { base.ScrollBars = value; }
        }

        /// <summary>
        /// Gets or sets the width of the Web server control.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.UI.WebControls.Unit"/> that represents the width of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The width of the Web server control was set to a negative value.</exception>
        [DefaultValue(typeof(ASP.Unit), "300px")]
        public override ASP.Unit Width
        {
            get
            {
                var baseWidth = base.Width;
                return baseWidth.IsEmpty ? ASP.Unit.Pixel(300) : baseWidth;
            }
            set { base.Width = value; }
        }

        /// <summary>
        /// Css class name for the header DOM element.
        /// </summary>
        [AjaxSerializable("setCCH")]
        internal string HeaderCssClass
        {
            get
            {
                var draggableCssClass = Draggable ? "window-draggable" : string.Empty;
                var iconCssClass = string.IsNullOrEmpty(IconCssClass) ? string.Empty : "window-icon " + IconCssClass;
                return CombineCssClass("window-top", "noselect", draggableCssClass, iconCssClass);
            }
        }

        #endregion

        #region [ -- Public Methods -- ]

        /// <summary>
        /// Programatically Minimize the Window
        /// </summary>
        [Obsolete("Use Maximized property instead.", true)]
        public void Minimize()
        {
            Maximized = false;
            Minimized = true;
        }

        /// <summary>
        /// Programatically Maximize the Window
        /// </summary>
        [Obsolete("Use Maximized property instead.", true)]
        public void Maximize()
        {
            Maximized = true;
            Minimized = false;
        }

        /// <summary>
        /// Programatically Restore the Window from Minimized or Maximized state
        /// </summary>
        public void Restore()
        {
            if (Maximized)
                Maximized = false;

            if (Minimized)
                Minimized = false;
        }

        /// <summary>
        /// Programmatically bring the window to the front. 
        /// </summary>
        public void BringToFront()
        {
            _bringToFront = true;
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
            if (!CompositionControlsCreated) return;
            CompositionControlsCreated = false;
        }

        #region [ -- Overridden Base class methods -- ]

        /// <summary>
        /// Overridden to create the AspectDraggable if needed (if Window is Draggable)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (Draggable)
            {
                Aspects.Add(new AspectDraggable(
                                delegate { RaiseMovedEvent(); },
                                Rectangle.Empty,
                                false,
                                true,
                                OpacityWhenMoved,
                                CombineId("_header")));
            }

            if (Resizable)
            {
                Aspects.Add(new AspectResizable(
                                delegate { RaiseResizedEvent(); },
                                AspectResizable.ResizeModes.RightBorder |
                                AspectResizable.ResizeModes.BottomBorder,
                                (int) MinWidth.Value,
                                (int) MinHeight.Value,
                                Int32.MaxValue,
                                Int32.MaxValue));
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// Making sure we add up our "special controls" like Close button, minimize, maximize etc.
        /// </summary>
        protected override void CreateCompositionControls()
        {
            _caption = CreateCaption(CaptionId, CssClassCaption, Caption);
            _closer = CreateActionButton(CloseId, CssClassCloseAction, Closable, CloseCommandName);
            
            _minimizer = CreateActionButton(MinimizeId, Minimized ? CssClassRestoreAction : CssClassMinimizeAction,
                                            Minimizable && !Maximized, MinimizeOrRestoreCommandName);
            if (DesignMode && _minimizerCreatedCallback != null)
                _minimizerCreatedCallback(_minimizer);

            _maximizer = CreateActionButton(MaximizeId, Maximized ? CssClassRestoreAction : CssClassMaximizeAction,
                                            Maximizable && !Minimized, MaximizeOrRestoreCommandName);
            if (DesignMode && _maximizerCreatedCallback != null)
                _maximizerCreatedCallback(_maximizer);

            CompositionControls.Add(_closer);
            CompositionControls.Add(_maximizer);
            CompositionControls.Add(_minimizer);
            CompositionControls.Add(_caption);
        }

        /// <summary>
        /// Determines whether the event for the server control is passed up the page's UI server control hierarchy.
        /// </summary>
        /// <returns>
        /// True if the event has been canceled; otherwise, false. The default is false.
        /// </returns>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var commandArguments = args as ASP.CommandEventArgs;
            if (commandArguments != null)
            {
                var commandName = commandArguments.CommandName;
                
                if (string.CompareOrdinal(CloseCommandName, commandName) == 0)
                    return HandleCloseCommand();

                if (string.CompareOrdinal(MinimizeOrRestoreCommandName, commandName) == 0)
                {
                    Minimized = !Minimized;
                    RaiseResizedEvent();
                    return true;
                }

                if (string.CompareOrdinal(MaximizeOrRestoreCommandName, commandName) == 0)
                {
                    Maximized = !Maximized;
                    RaiseResizedEvent();
                    return true;
                }
            }

            return base.OnBubbleEvent(source, args);
        }

        /// <summary>
        /// Last minute changing of attributes relevant to the client
        /// </summary>
        /// <param name="e">See base implementation</param>
        protected override void OnPreRender(EventArgs e)
        {
            SetPreRenderDefaults();

            foreach (var aspect in Aspects)
            {
                var draggable = aspect as AspectDraggable;
                var resizable = aspect as AspectResizable;
                if (draggable != null)
                {
                    draggable.Handle = CombineId("_header");

                    // Add some opacity when the window is being dragged around
                    if (OpacityWhenMoved != 1M)
                    {
                        Effects.Add(AspectDraggable.EffectEventStartDragging, new EffectOpacity(1M, OpacityWhenMoved, 0.3M));
                        Effects.Add(AspectDraggable.EffectEventEndDragging, new EffectOpacity(OpacityWhenMoved, 1M, 0.3M));
                    }
                }
                
                if (resizable != null)
                    resizable.SiblingElement = BodyID;
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// See <see cref="AjaxControl.RenderControl" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to write the control into</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            EnsureChildControls();
            VerifyWindowState();
            base.RenderControl(writer);
        }

        /// <summary>
        /// Renders the HTML for the Gaia Window into the given writer
        /// </summary>
        /// <param name="create">HtmlTextWriter to render into</param>
        /// <returns>HTML for a Gaia Window</returns>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            if (!DesignMode)
            {
                var styleCollection = Style;
                styleCollection["visibility"] = "hidden"; // will be made visible by the client code
                styleCollection["position"] = "absolute";
            }

            // ScrollBars are not serialized on the root element since we want to have
            // them on the content area. Therefore we temporary disable them here.
            var oldScrollbars = ScrollBars;
            ScrollBars = ASP.ScrollBars.None;

            // render root element 
            using (var div = create.Div(ClientID, Css.Combine(CssClass, false, "window")))
            {
                Css.SerializeAttributesAndStyles(this, div);
                ScrollBars = oldScrollbars;
                
                RenderControlContents(create);
            }
        }

        /// <summary>
        /// Renders Window's content
        /// </summary>
        /// <param name="create">XhtmlTagFactory for easy rendering</param>
        protected override void RenderControlContents(XhtmlTagFactory create)
        {
            RenderWindowHeader(create);

            using (var contentWrapper = create.Div().SetCssClass(CombineCssClass("window-contentwrapper"))) // window body wrapper
            {
                if (ScrollBars != ASP.ScrollBars.None)
                    contentWrapper.SetStyle("overflow: hidden;");

                RenderWindowCenter(create);
                RenderWindowBottom(create);
            }
        }

        /// <summary>
        /// Include Window Javascript files
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            var manager = Manager.Instance;
            manager.AddInclusionOfExtensionsScriptFiles(typeof(Window));
            manager.AddInclusionOfFileFromResource("Gaia.WebWidgets.Extensions.Scripts.Window.js", typeof(Window), "Gaia.Extensions.Window.browserFinishedLoading", true);
        }

        /// <summary>
        /// Returns script required for registering control on the client.
        /// </summary>
        /// <param name="registerControl">Suggested control registration object.</param>
        /// <returns>Control registration script.</returns>
        protected override RegisterControl GetScript(RegisterControl registerControl)
        {
            var control = base.GetScript(registerControl);
            control.ControlType = "Gaia.Extensions.Window";

            var properties = GetProperties();
            var nestingIndex = GetNestingIndex();

            if (CenterInForm)
                Manager.Instance.AddScriptForClientSideEval("$G('" + ClientID + "').center();");

            if (Maximized)
                Manager.Instance.AddScriptForClientSideEval("$G('" + ClientID + "').setMaximized(1,1);");
            
            return control.AddProperty("className", CssClass).
                           AddPropertyIfTrue(Animation != -1, "animateAppearance", Animation).
                           AddPropertyIfTrue(nestingIndex != -1, "nested", nestingIndex).
                           AddPropertyIfTrue(properties != 0, "p", properties);
        }

        /// <summary>
        /// Sets design-time data for a control.
        /// </summary>
        /// <param name="data">
        /// An <see cref="T:System.Collections.IDictionary"/> containing the design-time data for the control. 
        /// </param>
        protected override void SetDesignModeState(IDictionary data)
        {
            _contentRenderCallback = data["ContentRenderCallback"] as Action<Tag>;
            _minimizerCreatedCallback = data["MinimizerCreatedCallback"] as Action<ASP.WebControl>;
            _maximizerCreatedCallback = data["MaximizerCreatedCallback"] as Action<ASP.WebControl>;
        }

        /// <summary>
        /// Sets defaults for composition controls
        /// </summary>
        [Obsolete("Use OnPreRender() or CreateChildControl() or CreateCompositionControls() instead.")]
        protected virtual void SetPreRenderDefaults() { }

        #endregion

        /// <summary>
        /// Creates action <see cref="LinkButton"/> with the specified properties.
        /// </summary>
        private LinkButton CreateActionButton(string id, string cssClass, bool visible, string commandName)
        {
            return new LinkButton
                       {
                           ID = id,
                           Visible = visible,
                           CausesValidation = false,
                           CommandName = commandName,
                           CssClass = CombineCssClass("item", cssClass)
                       };
        }

        /// <summary>
        /// Creates caption <see cref="Label"/> with the specified properties.
        /// </summary>
        private Label CreateCaption(string id, string cssClass, string text)
        {
            return new Label { ID = id, CssClass = CombineCssClass(cssClass), Text = text };
        }

        #region [ -- Rendering methods -- ]

        /// <summary>
        /// Renders <see cref="Window"/> header.
        /// </summary>
        private void RenderWindowHeader(XhtmlTagFactory create)
        {
            using (create.Div(CombineId("_header")).SetCssClass(CombineCssClass("window-tl")))
            {
                using (create.Div().SetCssClass(CombineCssClass("window-tr")))
                {
                    using (create.Div().SetCssClass(CombineCssClass("window-tc")))
                    {
                        // the list below should be tuned based on what properties are set, ie. draggable, icon, etc
                        using (create.Div(CombineId("_h")).SetCssClass(HeaderCssClass))
                        {
                            RenderActionButtons(create);
                            RenderCaption(create);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Renders <see cref="Window"/> caption.
        /// </summary>
        private void RenderCaption(XhtmlTagFactory create)
        {
            _caption.RenderControl(create.GetHtmlTextWriter());
        }

        /// <summary>
        /// Renders <see cref="Window"/> action button.
        /// </summary>
        private void RenderActionButtons(XhtmlTagFactory create)
        {
            _closer.RenderControl(create.GetHtmlTextWriter());
            _maximizer.RenderControl(create.GetHtmlTextWriter());
            _minimizer.RenderControl(create.GetHtmlTextWriter());
        }

        /// <summary>
        /// Renders <see cref="Window"/> middle part.
        /// </summary>
        private void RenderWindowCenter(XhtmlTagFactory create)
        {
            using (create.Div(CombineId("_middle")).SetCssClass(CombineCssClass("window-ml")))
            {
                using (create.Div().SetCssClass(CombineCssClass("window-mr")))
                {
                    using (create.Div().SetCssClass(CombineCssClass("window-mc")))
                    {
                        var overflow = Css.SerializeScrollingAttribute(ScrollBars);

                        using (var div = create.Div(BodyID, CombineCssClass("window-content")).SetStyle(overflow)) // window body
                        {
                            if (DesignMode && _contentRenderCallback != null)
                                _contentRenderCallback(div);

                            RenderChildren(create.GetHtmlTextWriter());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Renders <see cref="Window"/> bottom parts.
        /// </summary>
        /// <param name="create"></param>
        private void RenderWindowBottom(XhtmlTagFactory create)
        {
            using (create.Div(CombineId("_bottom")).SetCssClass(CombineCssClass("window-bl"))) // no footer
            {
                using (create.Div().SetCssClass(CombineCssClass("window-br")))
                {
                    using (create.Div().SetCssClass(CombineCssClass("window-bc"))) { }
                }
            }
        }

        #endregion

        #region [ -- Render helper methods -- ]

        /// <summary>
        /// Makes sure <see cref="Window"/> properties are in consistent state.
        /// </summary>
        private void VerifyWindowState()
        {
            // Some logic to avoid the most common pitfalls
            if (!Maximizable && Maximized)
                throw new InvalidOperationException("Window cannot be Maximized when Maximizable property is false.");

            if (!Minimizable && Minimized)
                throw new InvalidOperationException("Window cannot be Minimized when Minimizable property is false.");

            if (Maximized && Minimized)
                throw new ApplicationException("Window cannot be both Maximized and Minimized at the same time.");
        }

        /// <summary>
        /// Returns bit-field of property values for client.
        /// </summary>
        private int GetProperties()
        {
            var shouldBringToFront = Aspects.Find<AspectModal>() == null;
            return (Minimized ? 1 : 0) | (Maximized ? 1 << 1 : 0) |
                   (shouldBringToFront ? 1 << 2 : 0);
        }

        /// <summary>
        /// Returns the length of the <see cref="Control.ClientID"/> of the closest outer <see cref="IAjaxContainerControl"/>.
        /// </summary>
        private int GetNestingIndex()
        {
            for (var parent = Parent; parent != null; parent = parent.Parent)
            {
                var container = parent as IAjaxContainerControl;
                if (container != null)
                    return container.Control.ClientID.Length;
            }

            return -1;
        }

        private string CombineCssClass(params string[] cssclass)
        {
            return Css.Combine(CssClass, cssclass);
        }

        private string CombineId(string append)
        {
            return string.Concat(ClientID, append);
        }

        #endregion

        #region [ -- IExtraPropertyCallbackRenderer Implementation -- ]

        /// <summary>
        /// Implementation of your custom rendering. 
        /// </summary>
        /// <param name="code">An already built StringBuilder which you're supposed to render your data into</param>
        void IExtraPropertyCallbackRenderer.InjectPropertyChangesToCallbackResponse(StringBuilder code)
        {
            if (!_bringToFront || !Visible) return;
            code.Append(".bringWindowToFront()");
        }

        #endregion

        #region [ -- ISkinControl Implementation -- ]

        void ISkinControl.ApplySkin()
        {
            // include default skin css file
            ((IAjaxControl)this).AjaxControl.RegisterDefaultSkinStyleSheetFromResource(typeof(Window), Constants.DefaultSkinResource);

            // name of the default skin;
            CssClass = Constants.DefaultSkinCssClass;
        }

        bool ISkinControl.Enabled
        {
            get { return string.IsNullOrEmpty(CssClass) || CssClass.Equals(Constants.DefaultSkinCssClass); }
        }

        #endregion

        #region [ -- Window Command handlers -- ]

        /// <summary>
        /// Handles Close Window command.
        /// </summary>
        private bool HandleCloseCommand()
        {
            var shouldClose = true;

            if (Closing != null)
            {
                var evtArgs = new WindowClosingEventArgs();
                Closing(this, evtArgs);
                shouldClose = evtArgs.ShouldClose;
            }

            if (shouldClose && Visible)
                Visible = false;

            return true;
        }

        /// <summary>
        /// Raises <see cref="Resized"/> event.
        /// </summary>
        private void RaiseResizedEvent()
        {
            if (Resized == null) return;
            Resized(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="Moved"/> event.
        /// </summary>
        private void RaiseMovedEvent()
        {
            if (Moved == null) return;
            Moved(this, EventArgs.Empty);
        }

        #endregion
    }
}


