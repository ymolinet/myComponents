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
using System.Threading;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    using HtmlFormatting;

    /// <summary>
    /// The DateTimePicker allows the user to select a certain date and time from within a TextBox
    /// The Gaia Ajax DateTimePicker is extremely easy and flexible to use. For example, localization is done on the server, 
    /// meaning you have every localization option you have in the .NET Framework. It is also very lightweight and has very 
    /// little custom JavaScript written to make it work. It integrates perfectly with ASP.NET and is of course a 100% Ajax Citizen.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for DateTimePicker" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\DateTimePicker\Overview\Default.aspx"/>
    /// </code> 
    /// <code title="Codebehind for DateTimePicker" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\DateTimePicker\Overview\Default.aspx.cs"/>
    /// </code> 
    /// </example>
    [Themeable(true)]
    [ParseChildren(true)]
    [PersistChildren(false)]
    [ValidationProperty("Value")]
    [DefaultProperty("SelectedDate")]
    [DefaultEvent("SelectedDateChanged")]
    [ToolboxBitmap(typeof(DateTimePicker), "Resources.Gaia.WebWidgets.Extensions.DateTimePicker.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.DateTimePickerDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class DateTimePicker : GaiaControl, INamingContainer, IAjaxContainerControl
    {
        /// <summary>
        /// Specialized <see cref="DateTimeConverter"/> for the nullable <see cref="DateTime"/> values.
        /// </summary>
        public class NullableDateTimeConverter : DateTimeConverter
        {
            /// <summary>
            /// Converts the given value object to a <see cref="T:System.DateTime"/>.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Object"/> that represents the converted <paramref name="value"/>.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
            /// <param name="culture">An optional <see cref="T:System.Globalization.CultureInfo"/>. If not supplied, the current culture is assumed.</param>
            /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
            /// <exception cref="T:System.FormatException"><paramref name="value"/> is not a valid value for the target type.</exception>
            /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed.</exception>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var str = value as string;
                return str != null && str.Length == 0 ? null : base.ConvertFrom(context, culture, value);
            }
        }

        /// <summary>
        /// Specialized <see cref="ASP.Table"/> for <see cref="DateTimePicker"/> rendering.
        /// </summary>
        private sealed class DateTimePickerTable : ASP.Table
        {
            private readonly DateTimePicker _owner;

            /// <summary>
            /// Initializes a new instance of the <see cref="DateTimePickerTable"/> class.
            /// </summary>
            public DateTimePickerTable(DateTimePicker owner)
            {
                _owner = owner;
            }

            /// <summary>
            /// Adds HTML attributes and styles that need to be rendered to the specified <see cref="T:System.Web.UI.HtmlTextWriter"/>.
            /// </summary>
            /// <param name="writer">The output stream that renders HTML content to the client.</param>
            protected override void AddAttributesToRender(HtmlTextWriter writer)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID);
                base.AddAttributesToRender(writer);
            }
        }

        #region [ -- Constants -- ]

        private const string ToggleButtonId = "db";

        #region [ -- DateTimePicker Command Names -- ]

        private const string TogglePickerCommandName = "tp";
        private const string SetValueCommandName = "s";
        private const string SetValueAndCloseCommandName = "svc";
        private const string SetValueAndKeepOpenCommandName = "sv";

        #endregion

        private Button _toggleButton;

        #endregion

        #region [ -- Private Members -- ]

        private bool _draggable = true;
        private AjaxContainerControl _instance;
        private AjaxContainerControl _ajaxContainerControl;
        
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
        /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
        /// </summary>
        [Bindable(true)]
        [DefaultValue("")]
        [Category("Appearance")]
        [Description("Css Class name for the control.")]
        public string CssClass
        {
            get { return StateUtil.Get(ViewState, "CssClass", string.Empty); }
            set { SetStateValue("CssClass", value, string.Empty); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether validation is performed when the Buttons in the Calendar control is set to validate when a postback occurs.
        /// </summary>
        [Themeable(false)]
        [DefaultValue(false)]
        [Category("Behavior")]
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
        public virtual string ValidationGroup
        {
            get { return StateUtil.Get(ViewState, "ValidationGroup", string.Empty); }
            set { StateUtil.Set(ViewState, "ValidationGroup", value, string.Empty); }
        }        
        
        /// <summary>
        /// Get or Set the Width of the TextBox displaying the value
        /// </summary>
        [Category("Layout")]
        [Description("Width of the control.")]
        [DefaultValue(typeof(ASP.Unit), "200px")]
        public ASP.Unit Width
        {
            get { return StateUtil.Get(ViewState, "Width", ASP.Unit.Pixel(200)); }
            set { SetStateValue("Width", value, ASP.Unit.Pixel(200)); }
        }

        /// <summary>
        /// Gets or sets the tab index of the Web server control.
        /// </summary>
        [DefaultValue((short)0)]
        [Category("Accessibility")]
        [Description("Tab index of DateTimePicker.")]
        public short TabIndex
        {
            get { return StateUtil.Get(ViewState, "TabIndex", (short) 0); }
            set { SetStateValue("TabIndex", value, (short)0); }
        }
        
        /// <summary>
        /// Gets or sets if the <see cref="Calendar"/> inside the <see cref="DateTimePicker"/> is draggable.
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Specifies if the Calendar is draggable.")]
        public bool Draggable
        {
            get { return _draggable;  }
            set { _draggable = value; }
        }

        /// <summary>
        /// Gets or sets the width of the <see cref="Calendar"/>.
        /// </summary>
        [Category("Layout")]
        [Description("Width of the Calendar.")]
        [DefaultValue(typeof(ASP.Unit), "200px")]
        public ASP.Unit CalendarWidth
        {
            get { return StateUtil.Get(ViewState, "CalendarWidth", ASP.Unit.Pixel(200)); }
            set { SetStateValue("CalendarWidth", value, ASP.Unit.Pixel(200)); }
        }

        /// <summary>
        /// Gets or sets value indicating if the <see cref="DateTimePicker"/> is enabled.
        /// </summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Enabled state of the control.")]
        public bool Enabled
        {
            get { return StateUtil.Get(ViewState, "Enabled", true); }
            set { SetStateValue("Enabled", value, true); }
        }

        /// <summary>
        /// Gets or sets the transparency of the <see cref="Calendar"/> when it is being dragged.
        /// </summary>
        /// <remarks>
        /// Value of 0 means fully invisibility and 1 means full visibility.
        /// </remarks>
        [Category("Behavior")]
        [DefaultValue(typeof(decimal), "1")]
        [Description("Amount of opacity when DateTimePicker is dragged.")]
        public decimal OpacityWhenMoved
        {
            get { return StateUtil.Get(ViewState, "OpacityWhenMoved", 1M); }
            set
            {
                if (value < 0.0M || value > 1.0M)
                    throw new ArgumentException("OpacityWhenMoved property out of range, must be between 0.0 and 1.0");
                StateUtil.Set(ViewState, "OpacityWhenMoved", value, 1M);
            }
        }

        /// <summary>
        /// Get or Set the Selected DateTime. Not that to set this in .ASPX page
        /// you must use the ISO format and the Format property of the %DateTimePicker
        /// will not be used at all. Meaning use; "yyyy.MM.dd HH:mm" when setting
        /// this property. The Format property is only used for DISPLAYING the value back
        /// to the client.
        /// </summary>
        [DefaultValue(typeof(DateTime?), null)]
        [TypeConverter(typeof(NullableDateTimeConverter))]
        [Description("The currently selected date and time.")]
        [Editor("System.ComponentModel.Design.DateTimeEditor,System.Design", typeof(UITypeEditor))]
        public DateTime? Value
        {
            get { return StateUtil.Get(ViewState, "SelectedDate", (DateTime?)null); }
            set
            {
                var initial = Value;
                if (initial == value) return;

                if (value != null)
                {
                    var date = value.Value;
                    VisibleDate = date;
                    ViewState["SelectedDate"] = date;
                }
                else
                    ViewState["SelectedDate"] = null;

                RequiresRecomposition();
            }
        }

        /// <summary>
        /// The VisibleDate property is used to set the which month and year that will be visible initally if
        /// not the value is set.
        /// </summary>
        [DefaultValue(typeof(DateTime), "1/1/0001")]
        [Description("Currently visible date and time.")]
        public DateTime VisibleDate
        {
            get { return StateUtil.Get(ViewState, "VisibleDate", DateTime.MinValue); }
            set
            {
                var visibleDate = VisibleDate;
                var minValue = DateTime.MinValue;
                if (visibleDate != minValue && visibleDate.Month == value.Month && visibleDate.Year == value.Year) return;
                SetStateValue("VisibleDate", value.Date, minValue);
            }
        }

        /// <summary>
        /// The effective value of the <see cref="VisibleDate"/> property.
        /// </summary>
        private DateTime EffectiveVisibleDate
        {
            get
            {
                var value = VisibleDate;
                var minValue = DateTime.MinValue;
                return value == minValue ? DateTime.Today : value;
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Calendar"/> is rendered with a time part.
        /// </summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Visibility state of the Calendar time part.")]
        public bool HasTimePart
        {
            get { return StateUtil.Get(ViewState, "HasTimePart", false); }
            set { SetStateValue("HasTimePart", value, false); }
        }

        /// <summary>
        /// Basically the format from which the control should render its date in.
        /// It basically only supports yyyy for years, MM for months, dd for days, HH for hours in military format and mm for minutes.
        /// You cannot use long date formats like February or similar.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("MM/dd/yyyy HH:mm")]
        [Description("Date format string for displaying.")]
        public string Format
        {
            get { return StateUtil.Get(ViewState, "Format", "MM/dd/yyyy HH:mm"); }
            set { SetStateValue("Format", value, "MM/dd/yyyy HH:mm"); }
        }

        /// <summary>
        /// Gets or set the Culture to be used in the DateTimePicker
        /// </summary>
        [Category("Appearance")]
        [Description("Culture to use for formatting.")]
        public CultureInfo Culture
        {
            get
            {
                return ViewState["Culture"] == null
                           ? Thread.CurrentThread.CurrentUICulture
                           : new CultureInfo(ViewState["Culture"].ToString());
            }
            set
            {
                var initial = Culture;
                if (initial == value) return;

                ViewState["Culture"] = value.Name;
                RequiresRecomposition();
            }
        }

        /// <summary>
        /// Specify which <see cref="DayOfWeek"/> will be the first day displayed in the Calendar.
        /// </summary>
        /// <remarks>
        /// Default value is <see cref="DayOfWeek.Sunday"/>.
        /// </remarks>
        [Category("Appearance")]
        [DefaultValue(DayOfWeek.Sunday)]
        [Description("Which day of the week is displayed first.")]
        public DayOfWeek FirstDayOfWeek
        {
            get { return StateUtil.Get(ViewState, "DayOfWeek", DayOfWeek.Sunday); }
            set { SetStateValue("DayOfWeek", value, DayOfWeek.Sunday); }
        }

        /// <summary>
        /// Gets or sets if a <see cref="Button"/> for toggling visibility 
        /// of the <see cref="Calendar"/> inside the <see cref="DateTimePicker"/> is rendered.
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        [Description("Visibility state of the Calendar toggling button.")]
        public bool HasDropDownButton
        {
            get { return StateUtil.Get(ViewState, "HasDropDownButton", true); }
            set { SetStateValue("HasDropDownButton", value, true); }
        }

        /// <summary>
        /// Gets or sets if the <see cref="CalendarWithTimePart"/> is rendered.
        /// </summary>
        private bool CalendarVisible
        {
            get { return StateUtil.Get(ViewState, "CalendarVisible", false); }
            set { SetStateValue("CalendarVisible", value, false); }
        }

        /// <summary>
        /// Gets or sets if the <see cref="TextBox"/> is focused.
        /// </summary>
        private bool EditorFocused
        {
            get { return StateUtil.Get(ViewState, "Focused", false); }
            set { SetStateValue("Focused", value, false); }
        }

        #endregion

        #region [ -- Events -- ]

        /// <summary>
        /// Raised when the <see cref="Value"/> property is modified.
        /// </summary>
        public event EventHandler SelectedDateChanged;

        /// <summary>
        /// Raised when a user clicks a specific day in the <see cref="Calendar"/> and before <see cref="Value"/> is modified.
        /// </summary>
        public event EventHandler<Calendar.CalendarDayClickingEventArgs> DayClicking;

        /// <summary>
        /// Raised when a user clicks a specific day in the <see cref="Calendar"/> and after <see cref="Value"/> is modified.
        /// </summary>
        public event EventHandler DayClicked;

        /// <summary>
        /// Raised when new day cell is created and allows changing <see cref="ASP.WebControl.CssClass"/>.
        /// </summary>
        public event EventHandler<Calendar.DayRenderEventArgs> DayRender;

        #endregion

        /// <summary>
        /// Sets specified <paramref name="value"/> into the <see cref="Control.ViewState"/>
        /// using specified <paramref name="key"/>.
        /// </summary>
        /// <remarks>
        /// The value is set if it's different from the current value.
        /// If the value if updated, <see cref="RequiresRecomposition"/> is called.
        /// </remarks>
        internal void SetStateValue<T>(string key, T value, T defaultValue)
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

        /// <summary>
        /// Give focus to the input textbox
        /// </summary>
        public override void Focus()
        {
            EditorFocused = true;
        }

        /// <summary>
        /// Toggles visibility of associated Calendar
        /// </summary>
        public void TogglePicker()
        {
            if (CalendarVisible)
                HidePicker();
            else
                ShowPicker();
        }

        #region [ -- Overridden base class methods -- ]

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

                if (string.CompareOrdinal(TogglePickerCommandName, commandName) == 0)
                {
                    TogglePicker();
                    return true;
                }

                if (string.CompareOrdinal(SetValueAndCloseCommandName, commandName) == 0)
                    return HandleSetValueCommand(commandArguments.CommandArgument as string, true, true);

                if (string.CompareOrdinal(SetValueAndKeepOpenCommandName, commandName) == 0)
                    return HandleSetValueCommand(commandArguments.CommandArgument as string, true, false);

                if (string.CompareOrdinal(SetValueCommandName, commandName) == 0)
                    return HandleSetValueCommand(commandArguments.CommandArgument as string, false, false);
            }

            return base.OnBubbleEvent(source, args);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            EnsureChildControls();
            base.OnLoad(e);
        }

        /// <summary>
        /// Last minute changing of attributes relevant to the client
        /// </summary>
        /// <param name="e">See base implementation</param>
        protected override void OnPreRender(EventArgs e)
        {
            SetPreRenderDefaults();
            base.OnPreRender(e);
        }

        /// <summary>
        /// Sets the Text property of the TextBox which displays the Value to the user if Value is not NULL
        /// </summary>
        [Obsolete("Use OnPreRender() or CreateChildControl() instead.")]
        protected virtual void SetPreRenderDefaults() { }        

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            BuildDateTimePicker();
            base.CreateChildControls();
        }

        /// <summary>
        /// Render the DateTimePicker Control HTML
        /// </summary>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            if (DesignMode)
                EnsureChildControls();

            RenderChildren(create.GetHtmlTextWriter());
        }

        /// <summary>
        /// Gets design-time data for a control.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IDictionary"/> containing the design-time data for the control.
        /// </returns>
        protected override IDictionary GetDesignModeState()
        {
            EnsureChildControls();
            return new System.Collections.Specialized.HybridDictionary
                       {
                           {"ToggleButton", _toggleButton},
                           {"CalendarVisible", CalendarVisible}
                       };
        }

        /// <summary>
        /// Sets design-time data for a control.
        /// </summary>
        /// <param name="data">An <see cref="T:System.Collections.IDictionary"/> containing the design-time data for the control.</param>
        protected override void SetDesignModeState(IDictionary data)
        {
            var value = data["CalendarVisible"];
            if (value != null && value is bool)
                CalendarVisible = (bool) value;
        }

        /// <summary>
        /// Returns true if the value of the <see cref="VisibleDate"/> property should be serialized during design-time.
        /// </summary>
        internal bool ShouldSerializeVisibleDate()
        {
            return ViewState["VisibleDate"] != null;
        }

        /// <summary>
        /// Resets the <see cref="VisibleDate"/> property to its default value.
        /// </summary>
        internal void ResetVisibleDate()
        {
            ViewState.Remove("VisibleDate");
        }

        /// <summary>
        /// Returns true if the value of the <see cref="Culture"/> property should be serialized during design-time.
        /// </summary>
        internal bool ShouldSerializeCulture()
        {
            return ViewState["Culture"] != null;
        }

        /// <summary>
        /// Resets the <see cref="Culture"/> property to its default value.
        /// </summary>
        internal void ResetCulture()
        {
            ViewState.Remove("Culture");
        }

        #endregion

        #region [ -- Control creation methods -- ]

        /// <summary>
        /// Builds <see cref="DateTimePicker"/> control hierarchy.
        /// </summary>
        private void BuildDateTimePicker()
        {
            var headerRow = new ASP.TableRow();
            headerRow.Cells.Add(CreateTextBoxCell());
            headerRow.Cells.Add(CreateButtonCell());

            var calendarRow = new ASP.TableRow();
            calendarRow.Cells.Add(CreateCalendarCell());

            var table = new DateTimePickerTable(this) { CellPadding = 0, CellSpacing = 0, BorderWidth = 0 };
            table.Rows.Add(headerRow);
            table.Rows.Add(calendarRow);
            Controls.Add(table);
        }

        /// <summary>
        /// Creates <see cref="ASP.TableCell"/> containing <see cref="TextBox"/>.
        /// </summary>
        private ASP.TableCell CreateTextBoxCell()
        {
            var textBoxCell = new ASP.TableCell();
            textBoxCell.Controls.Add(CreateTextBox());
            return textBoxCell;
        }

        /// <summary>
        /// Creates <see cref="ASP.TableCell"/> containing <see cref="ExtendedButton"/>.
        /// </summary>
        /// <returns></returns>
        private ASP.TableCell CreateButtonCell()
        {
            var buttonContainer = new HtmlGenericControl("span");
            buttonContainer.Attributes.Add("class", CombineCssClass("datetimepicker-button"));

            _toggleButton = CreateButton();
            buttonContainer.Controls.Add(_toggleButton);

            var buttonCell = new ASP.TableCell();
            buttonCell.Style[HtmlTextWriterStyle.PaddingLeft] = "4px";
            buttonCell.Controls.Add(buttonContainer);
            return buttonCell;
        }

        /// <summary>
        /// Creates <see cref="ASP.TableCell"/> containing <see cref="CalendarWithTimePart"/>.
        /// </summary>
        /// <returns></returns>
        private ASP.TableCell CreateCalendarCell()
        {
            var calendarCell = new ASP.TableCell { ColumnSpan = 2 };
            calendarCell.Style[HtmlTextWriterStyle.ZIndex] = "1000";
            calendarCell.Style[HtmlTextWriterStyle.Position] = "absolute";
            calendarCell.Controls.Add(CreateCalendar());
            return calendarCell;
        }

        /// <summary>
        /// Creates <see cref="CalendarWithTimePart"/>.
        /// </summary>
        private CalendarWithTimePart CreateCalendar()
        {
            var calendar = new CalendarWithTimePart
                               {
                                   ID = "c",
                                   Format = Format,
                                   Culture = Culture,
                                   CssClass = CssClass,
                                   Width = CalendarWidth,
                                   ShowCloseButton = true,
                                   CausesValidation = false,
                                   HasTimePart = HasTimePart,
                                   Visible = CalendarVisible,
                                   FirstDayOfWeek = FirstDayOfWeek,
                                   VisibleDate = EffectiveVisibleDate,
                                   SelectedDate = Value.HasValue ? Value.Value : DateTime.MinValue
                               };

            if (_draggable)
            {
                var aspect = new AspectDraggable(null, Rectangle.Empty, false, true, OpacityWhenMoved);
                (calendar as IAspectableAjaxControl).Aspects.Add(aspect);
            }

            calendar.DayRender += DayRender;
            calendar.CalendarDayClicked += DayClicked;
            calendar.CalendarDayClicking += DayClicking;
            calendar.CloseCalendarClicked += delegate { HidePicker(); };

            // If the TimePart is updated we simply set the Value, 
            // but we don't fire SelectedDateChanged; 
            calendar.SelectedTimeChanged += delegate { RaiseSetValueCommand(calendar.SelectedDate.ToString(Format, Culture), false, false); };

            // We accept a selection when a particular day or the Today button is clicked
            calendar.TodayButtonClicked += delegate { RaiseSetValueCommand(calendar.SelectedDate.ToString(Format, Culture), true, !HasTimePart); };
            calendar.CalendarDayClicked += delegate { RaiseSetValueCommand(calendar.SelectedDate.ToString(Format, Culture), true, !HasTimePart); };

            return calendar;
        }

        /// <summary>
        /// Creates <see cref="TextBox"/> for date and time input.
        /// </summary>
        private TextBox CreateTextBox()
        {
            var editor = new TextBox
                             {
                                 ID = "i",
                                 Width = Width,
                                 Enabled = Enabled,
                                 TabIndex = TabIndex,
                                 AutoPostBack = true,
                                 CausesValidation = false,
                                 CssClass = CombineCssClass("calendar-input-text"),
                                 Text = Value.HasValue ? Value.Value.ToString(Format, Culture) : string.Empty
                             };

            if (EditorFocused)
                editor.Focus();

            var aspectKey = new AspectKey();
            aspectKey.AddFilter(new AspectKey.KeyFilter(AspectKey.KeyCode.TAB, false, false, false, false, false));
            aspectKey.AddFilter(new AspectKey.KeyFilter(AspectKey.KeyCode.RETURN, false, false, false, true, false));
            aspectKey.KeyDown += delegate { };
            editor.Aspects.Add(aspectKey);

            // add up AspectClickable to the InputTextBox to open Calendar if it hasn't got a dropdown
            // we also monitor textchanged on loose focus to remove the panel calendar if it has changes);
            if (!HasDropDownButton)
                editor.Click += delegate { ShowPicker(); };

            editor.TextChanged += delegate { RaiseSetValueCommand(editor.Text, true, true); };

            return editor;
        }

        /// <summary>
        /// Creates <see cref="ExtendedButton"/> for toggling <see cref="CalendarWithTimePart"/>.
        /// </summary>
        private ExtendedButton CreateButton()
        {
            var button = new ExtendedButton
                             {
                                 ID = ToggleButtonId,
                                 Text = "...",
                                 Enabled = Enabled,
                                 CssClass = CssClass,
                                 CausesValidation = false,
                                 Toggled = CalendarVisible,
                                 Width = ASP.Unit.Pixel(40),
                                 Visible = HasDropDownButton,
                                 CommandName = TogglePickerCommandName
                             };

            return button;
        }

        /// <summary>
        /// Raises one of the supported value setting commands
        /// based on specified parameters.
        /// </summary>
        private void RaiseSetValueCommand(string value, bool raiseChangeEvent, bool closePicker)
        {
            string commandName;
            if (raiseChangeEvent && closePicker)
                commandName = SetValueAndCloseCommandName;
            else if (raiseChangeEvent)
                commandName = SetValueAndKeepOpenCommandName;
            else
                commandName = SetValueCommandName;

            OnBubbleEvent(this, new ASP.CommandEventArgs(commandName, value));
        }

        #endregion

        #region [ -- DateTimePicker Command Handlers -- ]

        /// <summary>
        /// Handles command having the name of <see cref="SetValueCommandName"/>.
        /// </summary>
        private bool HandleSetValueCommand(string argument, bool raiseChangeEvent, bool canClose)
        {
            if (argument != null)
            {
                if (argument.Trim().Length > 0)
                {
                    DateTime newValue;
                    var succeeded = DateTime.TryParseExact(argument, Format, Culture, DateTimeStyles.None, out newValue);
                    if (succeeded)
                    {
                        Value = newValue;

                        if (canClose)
                            HidePicker();
                    }
                }
                else
                    Value = null;

                Validate();
                if (raiseChangeEvent && SelectedDateChanged != null)
                    SelectedDateChanged(this, EventArgs.Empty);
            }

            return true;
        }

        /// <summary>
        /// Validates page if required.
        /// </summary>
        private void Validate()
        {
            var page = Page;
            if (!CausesValidation || (page == null)) return;
            page.Validate(ValidationGroup);
        }

        #endregion

        #region [ -- IAjaxControl and IAjaxContainerControl implementation -- ]

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new AjaxContainerControl(this)); }
        }

        string IAjaxControl.TagName
        {
            get { return "table"; }
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.Container", ClientID).AddAspects(AjaxContainerControl.Aspects).ToString();
        }

        AspectCollection IAspectableAjaxControl.Aspects
        {
            get { return AjaxContainerControl.Aspects; }
        }

        private AjaxContainerControl AjaxContainerControl
        {
            get { return _ajaxContainerControl ?? (_ajaxContainerControl = ((IAjaxContainerControl)this).AjaxContainerControl); }
        }

        /// <summary>
        /// Retrieves the AspectableAjaxControl object associated with the Control
        /// </summary>
        AspectableAjaxControl IAspectableAjaxControl.AspectableAjaxControl
        {
            get { return AjaxContainerControl; }
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
            throw new NotSupportedException();
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

        #region [ -- Helpers -- ]

        private void ShowPicker()
        {
            CalendarVisible = true;
        }

        private void HidePicker()
        {
            CalendarVisible = false;
        }

        /// <summary>
        /// Combines and returns variable number of css classes.
        /// </summary>
        /// <param name="cssclasses">Css classes to combine</param>
        /// <returns>Combined string of provided css classes.</returns>
        private string CombineCssClass(params string[] cssclasses)
        {
            return Css.Combine(CssClass, cssclasses);
        }

        #endregion
    }
}
