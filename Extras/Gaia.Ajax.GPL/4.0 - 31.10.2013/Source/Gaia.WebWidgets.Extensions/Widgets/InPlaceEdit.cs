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
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    using Effects;

    /// <summary>
    /// The Gaia InPlaceEdit Control is normally displayed as a Label, but when the user clicks on it, the %Label 
    /// turns into a TextBox, to edit the displayed text.
    /// The InPlaceEdit is simply a Label that turns into a TextBox when the user clicks on it. When the user is done 
    /// editing the text, he/she can press enter or click somewhere else on the page to turn it back into a Label that displays 
    /// the new text. As it turns back into a Label it is animated, this animation, as well as other aspects of this control,
    /// is configurable. The InPlaceEdit is often used for inplace editing of fields meant to be readable at first but easily 
    /// editable, like for instance, in-place editing of a GridView or DataGrid cells.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for InPlaceEdit" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\InPlaceEdit\Matrix\Default.aspx"/>
    /// </code> 
    /// <code title="Codebehind for InPlaceEdit" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\InPlaceEdit\Matrix\Default.aspx.cs"/>
    /// </code> 
    /// </example>
    [Themeable(true)]
    [PersistChildren(false)]
    [SupportsEventValidation]
    [ValidationProperty("Text")]
    [DefaultEvent("TextChanged")]
    [ParseChildren(true, "Text")]
    [ToolboxData("<{0}:InPlaceEdit runat=\"server\" />")]
    [ToolboxBitmap(typeof(InPlaceEdit), "Resources.Gaia.WebWidgets.Extensions.InPlaceEdit.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.InPlaceEditDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class InPlaceEdit : Label, IAjaxControl, INamingContainer
    {
        #region  [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerWebControl"/> for <see cref="InPlaceEdit"/>.
        /// </summary>
        public class PropertyStateManagerInPlaceEdit : PropertyStateManagerLabel, IMayRequireRerendering
        {
            private readonly InPlaceEdit _owner;

            private bool _blink;
            private bool _hadButtons;
            private MouseClickMode _clickMode;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerInPlaceEdit"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerInPlaceEdit(InPlaceEdit control) : base(control)
            {
                _owner = control;
            }

            /// <summary>
            /// Returns true if associated <see cref="InPlaceEdit"/> needs to be rerendered.
            /// </summary>
            bool IMayRequireRerendering.RequiresRerendering
            {
                get
                {
                    return _blink != _owner.Blink || _hadButtons != _owner.HasButtons ||
                           _clickMode != _owner.MouseClicksToTriggerEditMode;
                }
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);
                
                var stateManager = source as PropertyStateManagerInPlaceEdit;
                if (stateManager == null) return;

                _blink = stateManager._blink;
                _clickMode = stateManager._clickMode;
                _hadButtons = stateManager._hadButtons;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _owner.RequiresRecomposition();
                _owner.SetClickHandler();

                _blink = _owner.Blink;
                _hadButtons = _owner.HasButtons;
                _clickMode = _owner.MouseClicksToTriggerEditMode;

                base.TakeSnapshot();
            }
        }

        #endregion

        private TextBox _editor;
        private bool _enteredEditMode;

        private const string ExitEditModeCommandName = "ex";
        private const string EnterEditModeCommandName = "en";

        /// <summary>
        /// Text for empty content.
        /// </summary>
        private const string EmptyContentText = "&nbsp;";

        #region [ -- Effect Events -- ]
        
        /// <summary>
        /// Use this EffectEvent to wire up an effect to the highlight event of the control
        /// </summary>
        public static AjaxEffectEvent Highlight
        {
            get { return AspectHoverable.EffectEventMouseOver; }
        }

        #endregion

        #region [ -- Enums -- ]
        
        /// <summary>
        /// Used in accordance with the MouseClicksToTriggerEditMode property to specify 
        /// SingleClick or DoubleClick for triggering the edit mode for the InPlaceEdit
        /// </summary>
        public enum MouseClickMode
        {
            /// <summary>
            /// Specifies that the editing field will be visible through single-clicking the control
            /// </summary>
            SingleClick,

            /// <summary>
            /// Specifies that the editing field will be visible through double-clicking the control.
            /// </summary>
            /// <remarks>
            /// This one DOES NOT work in Opera out of the box, the user must go to: <br />
            /// Tools > Preferences > Advanced > Toolbars and uncheck "Double-click text to display context menu".
            /// </remarks>
            DoubleClick
        }
        
        #endregion

        #region [ -- Events -- ]
        
        /// <summary>
        /// Raised when <see cref="Text"/> is changed.
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Raised when the edit mode is cancelled.
        /// </summary>
        public event EventHandler Cancel;

        #endregion

        #region [ -- Properties -- ]
        
        /// <summary>
        /// Gets or sets if <see cref="InPlaceEdit"/> will blink during hovering or other operations.
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Blink state during hovering and other operations.")]
        public bool Blink
        {
            get { return StateUtil.Get(ViewState, "Blink", true); }
            set { StateUtil.Set(ViewState, "Blink", value, true); }
        }

        /// <summary>
        /// Gets or sets if the OK and Cancel buttons are rendered.
        /// </summary>
        /// <remarks>
        /// If this is false then changes will be persisted on Blur event or Enter key event
        /// and will be cancelled on ESC key event.
        /// </remarks>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Visibility state of the OK and Cancel buttons.")]
        public bool HasButtons
        {
            get { return StateUtil.Get(ViewState, "HasButtons", false); }
            set { SetStateValue("HasButtons", value, false); }
        }

        /// <summary>
        /// Gets or sets if single or multi-line editing should be enabled.
        /// </summary>
        /// <remarks>
        /// If the property value is set to True, pressing Enter key will persist the changes, while in multi-line edit mode
        /// pressing Enter will result in new-line.
        /// </remarks>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Single or Multi-Line edit mode.")]
        public bool SingleLine
        {
            get { return StateUtil.Get(ViewState, "SingleLine", true); }
            set { SetStateValue("SingleLine", value, true); }
        }

        /// <summary>
        /// Gets or sets the mouse click mode for triggering editing.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(MouseClickMode.SingleClick)]
        [Description("Edit triggering mouse click mode.")]
        public MouseClickMode MouseClicksToTriggerEditMode
        {
            get { return StateUtil.Get(ViewState, "TriggerMode", MouseClickMode.SingleClick); }
            set { StateUtil.Set(ViewState, "TriggerMode", value, MouseClickMode.SingleClick); }
        }

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
        /// Gets or sets the Text of the control. The default value is " "
        /// </summary>
        public override string Text
        {
            get { return StateUtil.Get(ViewState, "Text", string.Empty); }
            set { SetStateValue("Text", value, string.Empty); }
        }

        /// <summary>
        /// Display value of the <see cref="Text"/> property.
        /// </summary>
        private string DisplayText
        {
            get
            {
                var text = Text;
                var effectiveText = string.IsNullOrEmpty(text) ? EmptyContentText : text;
                return SingleLine ? effectiveText : NewlineToBreak(effectiveText);
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
                var initial = base.Enabled;
                if (initial == value) return;
                
                base.Enabled = value;
                RequiresRecomposition();
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="InPlaceEdit"/> is in edit mode.
        /// </summary>
        private bool InEditMode
        {
            get { return StateUtil.Get(ViewState, "EditMode", false); }
            set { SetStateValue("EditMode", value, false); }
        }

        /// <summary>
        /// Returns true if both <see cref="InEditMode"/> and <see cref="ASP.WebControl.Enabled"/> are true.
        /// </summary>
        private bool EditModeEnabled
        {
            get { return InEditMode && IsEnabled; }
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

        #region [ -- Overriden Base class methods -- ]

        /// <summary>
        /// Notifies the control that an element was parsed and adds the element to the <see cref="T:System.Web.UI.WebControls.Label"/> control.
        /// </summary>
        /// <param name="obj">An object that represents the parsed element.</param>
        protected override void AddParsedSubObject(object obj)
        {
            var literalControl = obj as LiteralControl;
            if (literalControl == null)
                throw new NotSupportedException("InPlaceEdit supports only LiteralControls in the markup.");

            Text = literalControl.Text;
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation 
        /// to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            var editor = CreateEditor();
            _editor = editor;

            var okButton = CreateActionButton("ok", "OK");
            okButton.Click += delegate { RaiseExitEditModeCommand(editor.Text); };

            var cancelButton = CreateActionButton("cb", "Cancel");
            cancelButton.Click += delegate { RaiseExitEditModeCommand(null); };

            Controls.Add(CreateLabel());
            Controls.Add(editor);
            Controls.Add(okButton);
            Controls.Add(cancelButton);

            base.CreateChildControls();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            EnsureChildControls();
            base.OnLoad(e);
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            // Add EffectHighlight as default effect for Highlight event
            if (Blink)
            {
                Aspects.Add(new AspectHoverable());
                Effects.Add(Highlight, CreateHighlightEffect());
            }

            SetClickHandler();
            PrepareTextBox();
            base.OnPreRender(e);
        }

        /// <summary>
        /// Renders the contents of the <see cref="T:System.Web.UI.WebControls.Label"/> into the specified writer.
        /// </summary>
        /// <param name="writer">The output stream that renders HTML content to the client. </param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (DesignMode)
                EnsureChildControls();
            
            base.RenderContents(writer);
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
            var commandArgument = args as ASP.CommandEventArgs;
            if (commandArgument != null)
            {
                var commandName = commandArgument.CommandName;

                if (string.CompareOrdinal(EnterEditModeCommandName, commandName) == 0)
                    return HandleEnterEditModeCommand();

                if (string.CompareOrdinal(ExitEditModeCommandName, commandName) == 0)
                    return HandleExitEditModeCommand(commandArgument.CommandArgument as string);
            }

            return base.OnBubbleEvent(source, args);
        }

        #endregion

        /// <summary>
        /// Remove redundant aspect and event handlers from the <see cref="TextBox"/>.
        /// </summary>
        private void PrepareTextBox()
        {
            EnsureChildControls();
            
            if (HasButtons)
            {
                var aspects = _editor.Aspects;
                var aspect = aspects.Find<AspectKey>();
                aspects.Remove(aspect);

                _editor.Blur -= OnEditorBlur;
            }

            if (!EditModeEnabled || !_enteredEditMode) return;
            _editor.Focus();
            _editor.SelectAll();
        }

        /// <summary>
        /// Activates one of <see cref="Label.Click"/> or <see cref="Label.DoubleClick"/> events.
        /// </summary>
        private void SetClickHandler()
        {
            Click -= OnMouseEvent;
            DoubleClick -= OnMouseEvent;
            switch (MouseClicksToTriggerEditMode)
            {
                case MouseClickMode.SingleClick: Click += OnMouseEvent; break;
                case MouseClickMode.DoubleClick: DoubleClick += OnMouseEvent; break;
            }
        }

        /// <summary>
        /// Creates Label for displaying <see cref="Text"/>.
        /// </summary>
        private Label CreateLabel()
        {
            return new Label {ID = "l", Text = DisplayText, Visible = !EditModeEnabled};
        }

        /// <summary>
        /// Creates the editor <see cref="TextBox"/>.
        /// </summary>
        private TextBox CreateEditor()
        {
            var editor = new TextBox
                             {
                                 ID = "t",
                                 Columns = SingleLine ? Text.Length : 18,
                                 Text = SingleLine ? Text : BreakToNewline(Text),
                                 TextMode = SingleLine ? ASP.TextBoxMode.SingleLine : ASP.TextBoxMode.MultiLine
                             };

            // todo: Preferably we should have used editor.Visible = EditModeEnabled, but that makes impossible to do client-side validation.
            // todo: Can be fixed by introducing a special method in AjaxControl which specifies how to render invisible control. 
            editor.Style[HtmlTextWriterStyle.Display] = EditModeEnabled ? null : "none";

            // always set event handlers, will remove during OnPreRender if redundant.
            editor.Blur += OnEditorBlur;

            var aspectKey = new AspectKey();
            aspectKey.KeyDown += OnEditorKeyDown;
            aspectKey.AddFilter(new AspectKey.KeyFilter(AspectKey.KeyCode.ESC, false, false, false, true, true));

            if (SingleLine)
                aspectKey.AddFilter(new AspectKey.KeyFilter(AspectKey.KeyCode.RETURN, false, false, false, true, true));

            editor.Aspects.Add(aspectKey);
            
            return editor;
        }

        /// <summary>
        /// Creates OK or Cancel button.
        /// </summary>
        private Button CreateActionButton(string id, string text)
        {
            return new Button { ID = id, Text = text, Visible = EditModeEnabled && HasButtons };
        }

        /// <summary>
        /// Called when one of the <see cref="AspectKey.KeyCode.ESC"/> or <see cref="AspectKey.KeyCode.RETURN"/>
        /// keys is down on the <see cref="TextBox"/>.
        /// </summary>
        private void OnEditorKeyDown(object sender, AspectKey.KeyEventArgs e)
        {
            switch (e.Key.KeyCode)
            {
                case AspectKey.KeyCode.ESC:
                    RaiseExitEditModeCommand(null);
                    break;
                case AspectKey.KeyCode.RETURN:
                    var textBox = (ITextControl)((AspectKey)sender).ParentControl;
                    RaiseExitEditModeCommand(NewlineToBreak(textBox.Text));
                    break;
                default:
                    throw new InvalidOperationException("Invalid key encountered.");
            }
        }

        /// <summary>
        /// Called when the editor is blurred.
        /// </summary>
        private void OnEditorBlur(object sender, EventArgs e)
        {
            var editor = (ITextControl) sender;
            RaiseExitEditModeCommand(NewlineToBreak(editor.Text));
        }

        /// <summary>
        /// Called when either clicked or double clicked on the <see cref="Label"/>.
        /// </summary>
        private void OnMouseEvent(object sender, EventArgs e)
        {
            OnBubbleEvent(this, new ASP.CommandEventArgs(EnterEditModeCommandName, null));
        }

        /// <summary>
        /// Handles enter edit mode command.
        /// </summary>
        private bool HandleEnterEditModeCommand()
        {
            if (!InEditMode && IsEnabled)
            {
                InEditMode = true;
                _enteredEditMode = true;
            }

            return true;
        }

        /// <summary>
        /// Handles exit edit mode command.
        /// </summary>
        /// <param name="commandArgument">
        /// Text to set before exiting edit mode or null if cancelling is requested.
        /// </param>
        private bool HandleExitEditModeCommand(string commandArgument)
        {
            if (commandArgument != null)
            {
                var value = Text;
                if (value != commandArgument)
                {
                    Text = commandArgument;
                    RaiseEvent(TextChanged);
                }
            }
            else
                RaiseEvent(Cancel);

            var shouldHighlight = InEditMode && Blink;
            InEditMode = false;
            if (shouldHighlight)
                Effects.Add(CreateHighlightEffect());

            return true;
        }

        /// <summary>
        /// Raises the specified event <paramref name="handler"/> if it's not null.
        /// </summary>
        private void RaiseEvent(EventHandler handler)
        {
            if (handler == null) return;
            handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises exit edit command with the specified <paramref name="commandArgument"/>.
        /// </summary>
        private void RaiseExitEditModeCommand(string commandArgument)
        {
            OnBubbleEvent(this, new ASP.CommandEventArgs(ExitEditModeCommandName, commandArgument));
        }

        /// <summary>
        /// Creates <see cref="EffectHighlight"/>.
        /// </summary>
        private Effect CreateHighlightEffect()
        {
            var backColor = BackColor;
            var defaultColor = backColor.IsEmpty ? Color.White : backColor;
            return new EffectHighlight(Color.FromArgb(255, 255, 153), defaultColor, defaultColor, true);
        }

        /// <summary>
        /// Converts newlines in the specified <paramref name="value"/> to a <br/> elements.
        /// </summary>
        private static string NewlineToBreak(string value)
        {
            return new Regex("\r?\n").Replace(value, "<br/>");
        }

        /// <summary>
        /// Converts <br/> elements in the specified <paramref name="value"/> to newlines.
        /// </summary>
        private static string BreakToNewline(string value)
        {
            return new Regex("<br[^>]*>").Replace(value, "\n");
        }

        #region [ -- IAjaxControl implementation -- ]

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerInPlaceEdit(this);
        }

        #endregion
    }
}
