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
using System.Drawing.Design;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    using System.Globalization;
    using HtmlFormatting;

    /// <summary>
    /// <para>The Gaia Ajax AutoCompleter allows the user to type in a query into a TextBox and a list of items
    /// that satisfy this query according to your code will appear, from which the user can select an item.
    /// The Gaia Ajax AutoCompleter is extremely rich, flexible and versatile. You can add up complex Ajax Controls 
    /// into its items collection and everything will "just work". The user types in a query into a TextBox then 
    /// you can fill the AutoCompleterItems controls collection with items from a server-side event handler, when the 
    /// user selects a control (item), you will get access to the control through the SelectedValue property.</para>
    /// <para> User selection is being done with the arrow keys in addition to that the user can use Up, Down, Page Up or 
    /// Page Down to scroll 5 items at the time or even Home and End to go to the top or the bottom of the list.</para>
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for AutoCompleter" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\AutoCompleter\Overview\Default.aspx"/>
    /// </code> 
    /// <code title="Codebehind for AutoCompleter" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\AutoCompleter\Overview\Default.aspx.cs"/>
    /// </code> 
    /// </example>
    [Themeable(true)]
    [ParseChildren(true)]
    [PersistChildren(false)]
    [DefaultProperty("Text")]
    [ValidationProperty("Text")]
    [DefaultEvent("GetAutoCompleterItems")]
    [ToolboxBitmap(typeof(AutoCompleter), "Resources.Gaia.WebWidgets.Extensions.AutoCompleter.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.AutoCompleterDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class AutoCompleter : Panel, IAjaxControl
    {
        /// <summary>
        /// Represents and item in the <see cref="AutoCompleter"/>.
        /// </summary>
        [ToolboxItem(false)]
        public class AutoCompleterItem : GaiaControl, IAjaxContainerControl
        {
            private AjaxControl _instance;
            private AjaxContainerControl _ajaxContainerControl;

            /// <summary>
            /// The index in the <see cref="AutoCompleterItemCollection"/> of the owner <see cref="AutoCompleterPicker"/>.
            /// </summary>
            internal int Index { get; set; }

            /// <summary>
            /// Gets or set the owner <see cref="AutoCompleterPicker"/>.
            /// </summary>
            internal ControlPicker Owner { get; set; }

            /// <summary>
            /// Override in inherited class to customize rendering of the control.
            /// </summary>
            /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
            protected override void RenderControlHtml(XhtmlTagFactory create)
            {
                using (create.Div(ClientID, Owner.GetCssClassForItem(Index)))
                {
                    RenderChildren(create.GetHtmlTextWriter());
                }
            }

            #region [ -- IAspectableAjaxControl implementation -- ]

            string IAjaxControl.TagName
            {
                get { return "div"; }
            }

            AjaxControl IAjaxControl.AjaxControl
            {
                get { return _instance ?? (_instance = new AjaxContainerControl(this)); }
            }

            string IAjaxControl.GetScript()
            {
                return new RegisterControl("Gaia.Container", ClientID).AddAspects(AjaxContainerControl.Aspects).ToString();
            }

            /// <summary>
            /// The Aspect collection contains all the Aspects for this control.
            /// </summary>
            AspectCollection IAspectableAjaxControl.Aspects
            {
                get { return AjaxContainerControl.Aspects; }
            }

            /// <summary>
            /// Retrieves the AspectableAjaxControl object associated with the Control
            /// </summary>
            AspectableAjaxControl IAspectableAjaxControl.AspectableAjaxControl
            {
                get { return AjaxContainerControl; }
            }

            #endregion

            #region [ -- IAjaxContainerControl implementation -- ]

            /// <summary>
            /// Forces a re-rendering of the child controls of the widget.
            /// </summary>
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
                RenderChildren(create.GetHtmlTextWriter());
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

            AjaxContainerControl IAjaxContainerControl.AjaxContainerControl
            {
                get { return (AjaxContainerControl)((IAjaxContainerControl)this).AjaxControl; }
            }

            private AjaxContainerControl AjaxContainerControl
            {
                get { return _ajaxContainerControl ?? (_ajaxContainerControl = ((IAjaxContainerControl)this).AjaxContainerControl); }
            }

            #endregion
        }

        /// <summary>
        /// Denotes collection of <see cref="AutoCompleterItem"/> in the <see cref="AutoCompleterPicker"/>.
        /// </summary>
        private sealed class AutoCompleterItemCollection : ControlCollection
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Web.UI.ControlCollection"/> class for the specified parent server control.
            /// </summary>
            public AutoCompleterItemCollection(Control owner) : base(owner) { }

            /// <summary>
            /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection.
            /// </summary>
            public override void Add(Control child)
            {
                base.Add(ToAutoCompleterItem(child));
            }

            /// <summary>
            /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection at the specified index location.
            /// </summary>
            public override void AddAt(int index, Control child)
            {
                base.AddAt(index, ToAutoCompleterItem(child));
            }

            /// <summary>
            /// Determines whether the specified server control is in the parent server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
            /// </summary>
            public override bool Contains(Control c)
            {
                return IndexOf(c) > -1;
            }

            /// <summary>
            /// Retrieves the index of a specified <see cref="T:System.Web.UI.Control"/> object in the collection.
            /// </summary>
            public override int IndexOf(Control value)
            {
                var item = value as AutoCompleterItem;
                if (item != null)
                    return base.IndexOf(item);

                var items = new Control[Count];
                CopyTo(items, 0);
                return Array.FindIndex(items, itm => itm.Controls.Contains(value));
            }

            /// <summary>
            /// Removes the specified server control from the parent server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
            /// </summary>
            public override void Remove(Control value)
            {
                var index = IndexOf(value);
                if (index < 0) return;
                RemoveAt(index);
            }

            /// <summary>
            /// Returns the specified <paramref name="control"/> untouched if it is an <see cref="AutoCompleterItem"/>
            /// or a new <see cref="AutoCompleterItem"/> wrapped around the specified <paramref name="control"/>.
            /// </summary>
            private static AutoCompleterItem ToAutoCompleterItem(Control control)
            {
                var item = control as AutoCompleterItem;
                if (item == null)
                {
                    item = new AutoCompleterItem();
                    item.Controls.Add(control);
                }

                return item;
            }
        }

        /// <summary>
        /// Specialized <see cref="ControlPicker"/> for the <see cref="AutoCompleter"/>.
        /// </summary>
        private sealed class AutoCompleterPicker : ControlPicker, IAjaxControl
        {
            /// <summary>
            /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="AutoCompleterPicker"/>.
            /// </summary>
            private sealed class PropertyStateManagerAutoCompleterPicker : PropertyStateManagerControlPicker
            {
                private int _itemCount;
                private readonly AutoCompleterPicker _owner;

                /// <summary>
                /// Initializes new instance of <see cref="PropertyStateManagerAutoCompleterPicker"/> for specified <paramref name="control"/>.
                /// </summary>
                /// <param name="control">Control to track changes for.</param>
                /// <remarks>
                /// Stores state information for the control and serializes changes during callback rendering.
                /// </remarks>
                /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
                public PropertyStateManagerAutoCompleterPicker(AutoCompleterPicker control) : base(control)
                {
                    _owner = control;
                }

                /// <summary>
                /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
                /// </summary>
                /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
                protected override void AssignState(PropertyStateManagerControl source)
                {
                    base.AssignState(source);

                    var stateManager = source as PropertyStateManagerAutoCompleterPicker;
                    if (stateManager == null) return;

                    _itemCount = stateManager._itemCount;
                }

                /// <summary>
                /// Raised after the state snapshot for the associated <see cref="Control"/> is taken.
                /// </summary>
                /// <returns>
                /// True if event was consumed and the bubbling is cancelled. Otherwise, false.
                /// Default value is false.
                /// </returns>
                /// <remarks>
                /// Bubbles up until stopped by return value of true.
                /// </remarks>
                protected override bool OnSnapshotTakenEvent(object sender, EventArgs eventArgs)
                {
                    if (sender is AutoCompleterItem)
                        ++_itemCount;

                    return false;
                }

                /// <summary>
                /// Detects changes between current state and saved state snapshot for the associated <see cref="ControlPicker"/>.
                /// </summary>
                protected override void DiffSnapshot()
                {
                    var currentCount = _owner.HasControls() ? _owner.Controls.Count : 0;
                    if (currentCount < _itemCount && currentCount > 0)
                        Builder.Append(".checkMark(").Append(currentCount).Append(")");
                    
                    base.DiffSnapshot();
                }
            }

            private readonly AutoCompleter _owner;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="owner">Owner control.</param>
            public AutoCompleterPicker(AutoCompleter owner)
            {
                if (owner == null)
                    throw new ArgumentNullException("owner");

                _owner = owner;
            }

            /// <summary>
            /// Ensures <see cref="CreateChildControls"/> is false.
            /// </summary>
            internal void Invalidate()
            {
                if (!ChildControlsCreated) return;
                ChildControlsCreated = false;
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
            /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation 
            /// to create any child controls they contain in preparation for posting back or rendering.
            /// </summary>
            protected override void CreateChildControls()
            {
                _owner.RaiseGetAutoCompleterItems();
                base.CreateChildControls();
            }

            /// <summary>
            /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
            /// </summary>
            /// <returns>
            /// The CSS class rendered by the Web server control on the client. The default is <see cref="F:System.String.Empty"/>.
            /// </returns>
            public override string CssClass
            {
                get
                {
                    var cssClass = _owner.CssClass;
                    return string.IsNullOrEmpty(cssClass) ? base.CssClass : cssClass;
                }
                set { base.CssClass = value;  }
            }

            /// <summary>
            /// Gets or sets the width of the Web server control.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Web.UI.WebControls.Unit"/> that represents the width of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"/>.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">The width of the Web server control was set to a negative value. </exception>
            public override ASP.Unit Width
            {
                get
                {
                    var ownerWidth = _owner.Width;
                    if (ownerWidth.Type != ASP.UnitType.Pixel)
                        throw new InvalidOperationException("Only pixel values are valid for the AutoCompleterPicker.");

                    return ownerWidth.IsEmpty ? ASP.Unit.Empty : ASP.Unit.Pixel((int)ownerWidth.Value + AdditionalWidth);
                }
                set { throw new NotSupportedException(); }
            }

            /// <summary>
            /// Creates a new <see cref="T:System.Web.UI.ControlCollection"/> object to hold the child controls (both literal and server) of the server control.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Web.UI.ControlCollection"/> object to contain the current server control's child server controls.
            /// </returns>
            protected override ControlCollection CreateControlCollection()
            {
                return new AutoCompleterItemCollection(this);
            }

            /// <summary>
            /// Called after a child control is added to the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
            /// </summary>
            protected override void AddedControl(Control control, int index)
            {
                var item = (AutoCompleterItem) control;
                item.Owner = this;
                item.Index = index;
                item.ID = "i" + index;
                base.AddedControl(control, index);

                var collection = Controls;
                var count = collection.Count;
                for (var i = index + 1; i < count; ++i)
                    ++((AutoCompleterItem) collection[i]).Index;
            }

            /// <summary>
            /// Called after a child control is removed from the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
            /// </summary>
            protected override void RemovedControl(Control control)
            {
                base.RemovedControl(control);
                var item = (AutoCompleterItem) control;
                var index = item.Index;
                item.Index = -1;
                item.Owner = null;

                var aspects = ((IAspectableAjaxControl) item).Aspects;
                aspects.Remove(aspects.Find<AspectHoverable>());

                var collection = Controls;
                var count = collection.Count;
                for (var i = index; i < count; ++i)
                    --((AutoCompleterItem) collection[i]).Index;
            }

            internal override void RenderChildrenInternal(XhtmlTagFactory create)
            {
                foreach (AutoCompleterItem control in Controls)
                    control.RenderControl(create.GetHtmlTextWriter());
            }

            /// <summary>
            /// Returns control registration object required for registering control on the client.
            /// </summary>
            /// <param name="registerControl">Suggested control registration object.</param>
            /// <returns>Modified or new control registration object.</returns>
            protected override RegisterControl GetScript(RegisterControl registerControl)
            {
                return base.GetScript(registerControl).InvokeIf(HasControls(), "markItem", false, 0);
            }

            PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
            {
                return new PropertyStateManagerAutoCompleterPicker(this);
            }
        }

        /// <summary>
        /// Specialized <see cref="TextBox"/> for the <see cref="AutoCompleter"/>.
        /// </summary>
        private sealed class AutoCompleterTextBox : TextBox
        {
            private readonly AutoCompleter _owner;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="owner">Owner control.</param>
            public AutoCompleterTextBox(AutoCompleter owner)
            {
                if (owner == null)
                    throw new ArgumentNullException("owner");

                _owner = owner;
            }

            /// <summary>
            /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
            /// </summary>
            /// <returns>
            /// The CSS class rendered by the Web server control on the client. The default is <see cref="F:System.String.Empty"/>.
            /// </returns>
            public override string CssClass
            {
                get { return _owner.CssClass; }
                set { throw new NotSupportedException(); }
            }

            /// <summary>
            /// Gets or sets the width of the Web server control.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Web.UI.WebControls.Unit"/> that represents the width of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"/>.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">The width of the Web server control was set to a negative value. </exception>
            public override ASP.Unit Width
            {
                get { return _owner.Width; }
                set { throw new NotSupportedException(); }
            }

            public override short TabIndex
            {
                get { return _owner.TabIndex; }
                set { throw new NotSupportedException(); }
            }

            protected override void AddAttributesToRender(HtmlTextWriter writer)
            {
                base.AddAttributesToRender(writer);

                if (TabIndex != 0)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Tabindex,
                                        TabIndex.ToString(NumberFormatInfo.InvariantInfo), false);
                }
            }

            /// <summary>
            /// Renders the <see cref="T:System.Web.UI.WebControls.TextBox"/> control to the specified <see cref="T:System.Web.UI.HtmlTextWriter"/> object.
            /// </summary>
            /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> that receives the rendered output.</param>
            protected override void Render(HtmlTextWriter writer)
            {
                // make sure owner properties are copied to the ControlStyle for rendering
                if (ControlStyle.IsEmpty)
                {
                    ControlStyle.Width = Width;
                    ControlStyle.CssClass = CssClass;
                }

                base.Render(writer);
            }
        }

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for <see cref="AutoCompleter"/>
        /// </summary>
        private sealed class PropertyStateManagerAutoCompleter : PropertyStateManagerWebControl
        {
            public PropertyStateManagerAutoCompleter(ASP.WebControl control, string clientId)
                : base(control, clientId)
            { }

            protected override void DiffSnapshot()
            {
                ClearDirty(WebControlProperty.CssClass, WebControlProperty.TabIndex);
                base.DiffSnapshot();
            }
        }

        #region [ -- Private Members -- ]

        private TextBox _input;
        private ClientUtils _clientUtils;
        private AutoCompleterPicker _picker;

        private const int AdditionalWidth = 2; // extra space for the control picker
        
        #endregion

        #region [ -- Events and EventArgs-- ]

        /// <summary>
        /// Passed when the Gaia AutoCompleter is being closed
        /// </summary>
        public class AutoCompleterClosingEventArgs : EventArgs
        {
            internal AutoCompleterClosingEventArgs()
            {
                ShouldClose = true;
            }

            /// <summary>
            /// Check to see if you want to deny the user closing the AutoCompleter
            /// </summary>
            public bool ShouldClose { get; set; }
        }

        /// <summary>
        /// Passed when the user selects an item in the AutoCompleter. 
        /// Contains the SelectedValue.
        /// </summary>
        public class AutoCompleterSelectionChangedEventArgs : EventArgs
        {
            private readonly Control _selectedValue;

            internal AutoCompleterSelectionChangedEventArgs(Control selectedValue)
            {
                _selectedValue = selectedValue;
            }

            /// <summary>
            /// The Control that the user selected in the AutoCompleter.
            /// May contain null values.
            /// </summary>
            public Control SelectedValue
            {
                get { return _selectedValue; }
            }
        }


        /// <summary>
        /// This event is fired just before the Autocompleter will be closed.
        /// The consumer can decide wheter or not to close the window
        /// </summary>
        public event EventHandler<AutoCompleterClosingEventArgs> Closing;

        /// <summary>
        /// This event is fired when the AutoCompleter needs to be populated
        /// Populate the AutoCompleterItems collection ...
        /// </summary>
        public event EventHandler GetAutoCompleterItems;

        /// <summary>
        /// This event is fired when the user selects something. Access
        /// the value from the SelectedValue property. 
        /// note:  Since the AutoCompleter relies on dynamic control creation, you should store
        /// the SelectedValue in your own code
        /// </summary>
        public event EventHandler<AutoCompleterSelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// This event is fired when the user clicks into the AutoCompleter
        /// Can be useful if you need to clear the input textbox or do something else
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        /// Event fires when the AutoCompleter input textbox looses focus
        /// </summary>
        public event EventHandler Blur;

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
        /// Gets or sets a value indicating whether validation is performed for the <see cref="TextBox"/> in the <see cref="AutoCompleter"/> control.
        /// </summary>
        [Themeable(false)]
        [DefaultValue(false)]
        [Category("Behavior")]
        public virtual bool CausesValidation
        {
            get
            {
                EnsureChildControls();
                return _input.CausesValidation;
            }
            set
            {
                EnsureChildControls();
                _input.CausesValidation = value;
            }
        }

        /// <summary>
        /// Gets or sets the group of controls for which this control causes validation when it posts back to the server.
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Category("Behavior")]
        public virtual string ValidationGroup
        {
            get
            {
                EnsureChildControls();
                return _input.ValidationGroup;
            }
            set
            {
                EnsureChildControls();
                _input.ValidationGroup = value;
            }
        }
 
        /// <summary>
        /// AutoCompleterItems ControlCollection. 
        /// </summary>
        [Browsable(false)]
        public ControlCollection AutoCompleterItems
        {
            
            get
            {
                EnsureChildControls();
                return _picker.Controls;
            }
        }

        /// <summary>
        /// Returns the SelectedValue that the user selected. Is only available immediately 
        /// after the user selects something from the AutoCompleter.
        /// </summary>
        private Control SelectedValue
        {
            get
            {
                EnsureChildControls();
                foreach (AutoCompleterItem control in _picker.SelectedValues)
                {
                    var container = control.Controls;
                    return container[0];
                }
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [DefaultValue("")]
        [Localizable(true)]
        [Category("Appearance")]
        [Description("Text of the AutoCompleter.")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor,System.Design", typeof(UITypeEditor))]
        public string Text
        {
            get
            {
                EnsureChildControls();
                return _input.Text;
            }
            set
            {
                EnsureChildControls();
                _input.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets if the mouse is over any <see cref="AutoCompleterItem"/>.
        /// </summary>
        private bool IsMouseOverItem
        {
            get
            {
                EnsureChildControls();
                return _picker.IsMouseOverItem;
            }
        }

        /// <summary>
        /// Height of the Autocompleter
        /// </summary>
        public override ASP.Unit Height
        {
            get
            {
                EnsureChildControls();
                return _picker.Height;
            }
            set
            {
                EnsureChildControls();
                _picker.Height = value;
            }
        }

        /// <summary>
        /// Gets or sets the interval in milliseconds when a callback to the server will occur after key events.
        /// </summary>
        [DefaultValue(1000)]
        [Category("Behavior")]
        [Description("Key event callback interval.")]
        public int KeyChangeEventsInterval
        {
            get
            {
                EnsureChildControls();
                return _input.KeyChangeEventsInterval;
            }
            set
            {
                EnsureChildControls();
                _input.KeyChangeEventsInterval = value;
            }
        }

        /// <summary>
        /// Gets or sets if <see cref="AutoCompleter"/> was able to close the <see cref="AutoCompleterPicker"/>.
        /// </summary>
        private bool CouldClose
        {
            get { return StateUtil.Get(ViewState, "CouldClose", true); }
            set { StateUtil.Set(ViewState, "CouldClose", value, true); }
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Gives focus to the autocompleter input box
        /// </summary>
        public override void Focus()
        {
            EnsureChildControls();
            _input.Focus();
        }

        /// <summary>
        /// Selects all text in the textbox
        /// </summary>
        public void SelectAll()
        {
            EnsureChildControls();
            _input.SelectAll();
        }

        /// <summary>
        /// We override OnLoad to make sure the children are repopulated properly. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            EnsureChildControls();
            base.OnLoad(e);
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation
        /// to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            _input = CreateTextBox();
            _picker = CreatePicker();
            _clientUtils = new ClientUtils {ID = "c"};

            Controls.Add(_input);
            Controls.Add(_picker);
            Controls.Add(_clientUtils);

            base.CreateChildControls();
        }

        /// <summary>
        /// See <see cref="AjaxControl.RenderControl" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <param name="writer">The <see cref="HtmlTextWriter"/> to write the control into.</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (DesignMode)
                EnsureChildControls();

            _picker.InputElementForKeyPressNavigation = _input.ClientID;
            base.RenderControl(writer);
        }

        /// <summary>
        /// Render the Control itself
        /// </summary>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (var root = create.Div(ClientID))
            {
                Css.SerializeAttributesAndStyles(this, root);
                RenderChildren(create.GetHtmlTextWriter());
            }
        }

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerAutoCompleter(this, ClientID);
        }

        #endregion

        /// <summary>
        /// Creates <see cref="TextBox"/> control for <see cref="Text"/> input.
        /// </summary>
        private TextBox CreateTextBox()
        {
            var textbox = new AutoCompleterTextBox(this)
                              {
                                  ID = "i",
                                  KeyChangeEvents = true,
                                  CausesValidation = false,
                                  KeyChangeEventsInterval = 1000,
                                  AutoCompleteType = ASP.AutoCompleteType.Disabled
                              };

            textbox.Blur += OnBlur;
            textbox.TextChanged += InputTextChanged;

            // if the autocompleter is clicked and it has items, we display these ...
            textbox.Click += delegate
                                 {
                                     // fire the clicked event if the user needs to do something
                                     if (Clicked != null)
                                         Clicked(this, EventArgs.Empty);

                                     if (!string.IsNullOrEmpty(Text))
                                         DisplayPicker();
                                 };
            return textbox;
        }

        /// <summary>
        /// Creates <see cref="AutoCompleterPicker"/> for <see cref="AutoCompleterItem"/> display.
        /// </summary>
        private AutoCompleterPicker CreatePicker()
        {
            var picker = new AutoCompleterPicker(this)
                             {
                                 ID = "p",
                                 Visible = false,
                                 HoverSelection = true,
                                 SelectionMode = ControlPicker.ItemSelectionMode.Single
                             };

            picker.Style.Add(HtmlTextWriterStyle.Position, "absolute");
            picker.SelectedIndexChanged += PickerSelectedIndexChanged;
            picker.Aspects.Add(new AspectHoverable(null, PickerMouseOut));

            return picker;
        }
        
        #region [ -- Event handlers -- ]

        private void PickerMouseOut(object sender, EventArgs e)
        {
            if (CouldClose) return;
            CouldClose = true;
            OnBlur(this, EventArgs.Empty);
        }

        private void PickerSelectedIndexChanged(object sender, EventArgs e)
        {
            _picker.Invalidate();
            
            if (SelectionChanged != null)
                SelectionChanged(this, new AutoCompleterSelectionChangedEventArgs(SelectedValue));
                
            TryHidingPicker();
        }

        private void InputTextChanged(object sender, EventArgs e)
        {
            DisplayPicker();
        }

        #endregion

        #region [ -- Helpers -- ]

        private void RaiseGetAutoCompleterItems()
        {
            if (GetAutoCompleterItems == null) return;
            GetAutoCompleterItems(this, EventArgs.Empty);
        }

        private void DisplayPicker()
        {
            // when text_changed is run, we invalidate selectedIndex
            _picker.SelectedIndexes = null;
            _picker.Invalidate();

            // store the old visibility value
            var oldVisible = _picker.Visible;

            // hide the control-picker if nothing is typed into the textbox ... 
            _picker.Visible = _input.Text.Length > 0 && AutoCompleterItems.Count > 0;

            // we only serialize clone position if it's visible for the first time.
            if (_picker.Visible && !oldVisible) 
                _clientUtils.ClonePosition(_input, _picker);
        }

        private void TryHidingPicker()
        {
            var shouldClose = true;
            if (Closing != null)
            {
                var eventArgs = new AutoCompleterClosingEventArgs();
                Closing(this, eventArgs);
                shouldClose = eventArgs.ShouldClose;
            }

            _picker.Visible = !shouldClose;
        }

        /// <summary>
        /// Tries to hide the <see cref="AutoCompleterPicker"/> and raises <see cref="Blur"/> event.
        /// </summary>
        private void OnBlur(object sender, EventArgs e)
        {
            if (!IsMouseOverItem)
            {
                TryHidingPicker();

                if (Blur != null)
                    Blur(this, EventArgs.Empty);
            }
            else if (CouldClose)
                CouldClose = false;
        }

        #endregion
    }
}
