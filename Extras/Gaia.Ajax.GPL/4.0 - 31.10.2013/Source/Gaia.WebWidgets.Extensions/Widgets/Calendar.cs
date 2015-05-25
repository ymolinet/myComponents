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
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    using Effects;
    using HtmlFormatting;

    /// <summary>
    /// The Calendar allows the user to select a certain date and time
    /// The Gaia Ajax Calendar is similar to the DateTimePicker except it is directly embedded in the page (no drop down). It is
    /// extremely easy and flexible to use. For example, localization is done on the server, meaning you have every localization 
    /// option you have in the .NET Framework. It is also very lightweight and has very little custom JavaScript written to make 
    /// it work. It integrates perfectly with ASP.NET and is of course a 100% Ajax Citizen.
    /// <br/>
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for Calendar" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\Calendar\Overview\Default.aspx"/>
    /// </code> 
    /// <code title="Codebehind for Calendar" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\Calendar\Overview\Default.aspx.cs"/>
    /// </code> 
    /// </example>
    [Themeable(true)]
    [ParseChildren(true)]
    [PersistChildren(false)]
    [DefaultProperty("SelectedDate")]
    [ValidationProperty("SelectedDate")]
    [DefaultEvent("SelectedDateChanged")]
    [ToolboxBitmap(typeof(Calendar), "Resources.Gaia.WebWidgets.Extensions.Calendar.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.CalendarDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class Calendar : GaiaControl, ISkinControl, INamingContainer, IAjaxContainerControl
    {
        private AjaxContainerControl _instance;
        private AjaxContainerControl _ajaxContainerControl;

        #region [ -- Constants -- ]

        private const int WeekCount = 6;
        private const int DaysPerWeek = 7;
        
        private const string IdRootPanel = "c";
        private const string IdRootPicker = "p";
        private const string IdRootTable = "t";
        private const string IdDayCellPrefix = "dc";
        private const string IdDayButtonPrefix = "db";

        private const int PickerYearChangeOffset = 4;
        
        #endregion

        #region [ -- Calendar Command Names -- ]

        private const string NextYearCommandName = "ny";
        private const string PreviousYearCommandName = "py";

        private const string NextMonthCommandName = "nm";
        private const string PreviousMonthCommandName = "pm";

        private const string DayClickCommandName = "dc";
        private const string MonthClickCommandName = "mc";

        private const string YearChangeCommandName = "y";
        private const string TodayClickCommandName = "t";
        private const string MonthChangeCommandName = "m";
        private const string CloseCalendarCommandName = "cc";

        private const string PickerCloseCommandName = "pcc";
        private const string PickerNextYearCommandName = "pny";
        private const string PickerPreviousYearCommandName = "ppy";

        #endregion

        #region [ -- Events and EventArgs -- ]

        /// <summary>
        /// Raised when the close button is clicked.
        /// </summary>
        /// <remarks>
        /// This button is visible if the <see cref="ShowCloseButton"/> is set to true. 
        /// </remarks>
        internal event EventHandler CloseCalendarClicked;

        /// <summary>
        /// Raised when the <see cref="SelectedDate"/> is changed. 
        /// </summary>
        public event EventHandler SelectedDateChanged;

        /// <summary>
        /// Raised when the <see cref="VisibleDate"/> is changed.
        /// </summary>
        public event EventHandler ActiveDateViewChanged;

        /// <summary>
        /// Raised when a user clicks a specific day in the <see cref="Calendar"/> and before <see cref="SelectedDate"/> is modified.
        /// </summary>
        /// <remarks>
        /// <see cref="SelectedDate"/> is not modified if <see cref="CalendarDayClickingEventArgs.AllowSelection"/> is set to false.
        /// </remarks>
        public event EventHandler<CalendarDayClickingEventArgs> CalendarDayClicking;

        /// <summary>
        /// Raised when a user clicks a specific day in the <see cref="Calendar"/> and after <see cref="SelectedDate"/> is modified.
        /// </summary>
        public event EventHandler CalendarDayClicked;

        /// <summary>
        /// Raised when new day cell is created and allows changing <see cref="ASP.WebControl.CssClass"/>.
        /// </summary>
        public event EventHandler<DayRenderEventArgs> DayRender;

        /// <summary>
        /// Raised when Today button is clicked.
        /// </summary>
        public event EventHandler TodayButtonClicked;

        /// <summary>
        /// Specialized <see cref="EventArgs"/> for the <see cref="CalendarDayClicking"/> event. 
        /// </summary>
        public sealed class CalendarDayClickingEventArgs : EventArgs
        {
            private readonly DateTime _clickedDate;

            /// <summary>
            /// Constructor
            /// </summary>
            internal CalendarDayClickingEventArgs(DateTime date)
            {
                _clickedDate = date;
                AllowSelection = true;
            }

            /// <summary>
            /// The clicked day in the <see cref="Calendar"/>.
            /// </summary>
            public DateTime ClickedDate
            {
                get { return _clickedDate; }
            }

            /// <summary>
            /// Gets or sets if the <see cref="Calendar.SelectedDate"/> can be modified.
            /// </summary>
            public bool AllowSelection { get; set; }
        }

        /// <summary>
        /// Specialized <see cref="EventArgs"/> for <see cref="Calendar.DayRender"/> event.
        /// </summary>
        public class DayRenderEventArgs : EventArgs
        {
            private readonly DateTime _calendarDay;

            /// <summary>
            /// Constructor
            /// </summary>
            internal DayRenderEventArgs(DateTime day)
            {
                _calendarDay = day;
            }

            /// <summary>
            /// Gets or set CssClass which should be used for day cell.
            /// </summary>
            public string DayCssClass { get; set; }

            /// <summary>
            /// Gets associated day.
            /// </summary>
            public DateTime CalendarDay { get { return _calendarDay; } }
        }

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
        /// Gets or sets a value indicating whether validation is performed when the Buttons in the Calendar control is set to validate when a postback occurs.
        /// </summary>
        [Themeable(false)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Whether causes validation to fire.")]
        public virtual bool CausesValidation
        {
            get { return StateUtil.Get(ViewState, "CausesValidation", false); }
            set { StateUtil.Set(ViewState, "CausesValidation", value, false); }
        }

        /// <summary>
        /// Width of the <see cref="Calendar"/>.
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
        /// Specify which Date will be the first day displayed in the <see cref="Calendar"/>.
        /// By default it is Sunday. 
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(DayOfWeek.Sunday)]
        [Description("Which day of the week is displayed first.")]
        public DayOfWeek FirstDayOfWeek
        {
            get { return StateUtil.Get(ViewState, "DayOfWeek", DayOfWeek.Sunday); }
            set { SetStateValue("DayOfWeek", value, DayOfWeek.Sunday); }
        }

        /// <summary>
        /// Format string used for displaying date.
        /// </summary>
        /// <remarks>
        /// Only yyyy for years, MM for months, dd for days, HH for hours in military format and mm for minutes are supported.
        /// You cannot use long date formats like February or similar.
        /// </remarks>
        [Category("Appearance")]
        [DefaultValue("yyyy.MM.dd HH:mm")]
        [Description("Date format string for displaying.")]
        public string Format
        {
            get { return StateUtil.Get(ViewState, "Format", "yyyy.MM.dd HH:mm"); }
            set { SetStateValue("Format", value, "yyyy.MM.dd HH:mm"); }
        }

        /// <summary>
        /// Duration of animation when shifting from expanded to collapsed mode.
        /// </summary>
        [DefaultValue(0)]
        [Category("Behavior")]
        [Description("Duration of animation.")]
        public int AnimationDuration
        {
            get { return StateUtil.Get(ViewState, "AnimationDuration", 0); }
            set { SetStateValue("AnimationDuration", value, 0); }
        }

        /// <summary>
        /// Gets or sets <see cref="DateTime"/> for rendering the month and year view.
        /// Does not change <see cref="SelectedDate"/>.
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
        /// Gets or sets the selected <see cref="DateTime"/> for the <see cref="Calendar"/> control.
        /// </summary>
        /// <remarks>
        /// Default value is <see cref="DateTime.MinValue"/>.
        /// Setting to <see cref="DateTime.MinValue"/> clear selected date.
        /// </remarks>
        [DefaultValue(typeof(DateTime), "1/1/0001")]
        [Description("The currently selected date and time.")]
        public DateTime SelectedDate
        {
            get { return StateUtil.Get(ViewState, "SelectedDate", DateTime.MinValue); }
            set
            {
                var initial = SelectedDate;
                if (initial == value) return;

                // Setting the date to MinValue is the same as *clearing* the date. It will result in redrawing the surface 
                // as any previously selected day no get's cleared out ... 
                if (value != DateTime.MinValue)
                {
                    VisibleDate = value;
                    ViewState["SelectedDate"] = value;
                }
                else
                    ViewState["SelectedDate"] = null;

                RequiresRecomposition();
            }
        }

        private int StartYearInMonthYearPicker
        {
            get
            {
                var value = ViewState["Year"];
                if (value == null)
                {
                    value = Math.Max(EffectiveVisibleDate.Year - 4, DateTime.MinValue.Year);
                    ViewState["Year"] = value;
                    ViewState.SetItemDirty("Year", true);
                }
                return (int) value;
            }
            set { SetStateValue("Year", value, -1); }
        }

        /// <summary>
        /// Gets or sets if month picker is visible.
        /// </summary>
        private bool MonthPickerVisible
        {
            get { return StateUtil.Get(ViewState, "dp", false); }
            set { SetStateValue("dp", value, false); }
        }

        /// <summary>
        /// Gets or sets the group of controls for which this control causes validation when it posts back to the server.
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        [Category("Behavior")]
        [Description("The group that should be validated when the control causes a postback.")]
        public virtual string ValidationGroup
        {
            get { return StateUtil.Get(ViewState, "ValidationGroup", string.Empty); }
            set { StateUtil.Set(ViewState, "ValidationGroup", value, string.Empty); }
        }

        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [Description("Css Class name for the control.")]
        public virtual string CssClass
        {
            get { return StateUtil.Get(ViewState, "CssClass", string.Empty); }
            set { SetStateValue("CssClass", value, string.Empty); }
        }

        /// <summary>
        /// Gets or sets if the close button should be rendered or not.
        /// </summary>
        /// <remarks>
        /// Clicking the close button raises <see cref="CloseCalendarClicked"/> event and 
        /// does not automatically close the <see cref="Calendar"/>.
        /// </remarks>
        [DefaultValue(false)]
        [Category("DateTimePicker custom properties")]
        [Description("Specify wheter the Close Calendar button is displayed or not.")]
        internal bool ShowCloseButton
        {
            get { return StateUtil.Get(ViewState, "ShowCloseButton", false); }
            set { SetStateValue("ShowCloseButton", value, false); }
        }

        /// <summary>
        /// Gets or sets the current <see cref="CultureInfo"/> that will be used for formatting values.
        /// Default value is <see cref="Thread.CurrentUICulture"/>.
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

        #endregion

        #region [-- Overriden base class and rendering --]

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation 
        /// to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            BuildCalendar();
            base.CreateChildControls();
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
                
                if (string.CompareOrdinal(NextYearCommandName, commandName) == 0)
                    return HandleVisibleDateCommand(EffectiveVisibleDate.AddYears(1));
                
                if (string.CompareOrdinal(NextMonthCommandName, commandName) == 0)
                    return HandleVisibleDateCommand(EffectiveVisibleDate.AddMonths(1));

                if (string.CompareOrdinal(PreviousYearCommandName, commandName) == 0)
                    return HandleVisibleDateCommand(EffectiveVisibleDate.AddYears(-1));

                if (string.CompareOrdinal(PreviousMonthCommandName, commandName) == 0)
                    return HandleVisibleDateCommand(EffectiveVisibleDate.AddMonths(-1));

                if (string.CompareOrdinal(MonthClickCommandName, commandName) == 0)
                    return HandleMonthClickCommand();

                if (string.CompareOrdinal(DayClickCommandName, commandName) == 0)
                    return HandleDayClickCommand(int.Parse(commandArguments.CommandArgument.ToString()));

                if (string.CompareOrdinal(CloseCalendarCommandName, commandName) == 0)
                    return HandleCloseCalendarCommand();

                if (string.CompareOrdinal(TodayClickCommandName, commandName) == 0)
                    return HandleTodayClickCommand();

                if (string.CompareOrdinal(PickerNextYearCommandName, commandName) == 0)
                    return HandlePickerYearCommand(PickerYearChangeOffset);

                if (string.CompareOrdinal(PickerPreviousYearCommandName, commandName) == 0)
                    return HandlePickerYearCommand(-PickerYearChangeOffset);

                if (string.CompareOrdinal(PickerCloseCommandName, commandName) == 0)
                    return HandlePickerCloseCommand();

                if (string.CompareOrdinal(MonthChangeCommandName, commandName) == 0)
                {
                    var month = int.Parse(commandArguments.CommandArgument.ToString());
                    var visibleDate = EffectiveVisibleDate;
                    return HandleVisibleDateCommand(visibleDate.AddMonths(month - visibleDate.Month));
                }

                if (string.CompareOrdinal(YearChangeCommandName, commandName) == 0)
                {
                    var year = int.Parse(commandArguments.CommandArgument.ToString());
                    var visibleDate = EffectiveVisibleDate;
                    return HandleVisibleDateCommand(visibleDate.AddYears(year - visibleDate.Year));
                }
            }
            return base.OnBubbleEvent(source, args);
        }

        /// <summary>
        /// Render the Calendar Control Html
        /// </summary>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            if (DesignMode)
                EnsureChildControls();

            using (create.Div(ClientID))
            {
                RenderChildren(create.GetHtmlTextWriter());
            }
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
        /// Builds the calendar.
        /// </summary>
        private void BuildCalendar()
        {
            var container = new Panel
                                {
                                    Width = Width,
                                    ID = IdRootPanel,
                                    CssClass = CombineCssClass("calendar-container", "noselect")
                                };
            
            container.Controls.Add(CreateCalendarTable());
            container.Controls.Add(CreateMonthAndYearPicker());

            Controls.Add(container);
        }

        #region [ -- Calendar table creation methods -- ]

        /// <summary>
        /// Creates calendar table.
        /// </summary>
        private ASP.Table CreateCalendarTable()
        {
            var table = new ASP.Table {ID = IdRootTable, Width = ASP.Unit.Percentage(100), CellSpacing = 0};

            // build the three core rows and invoke virtual for CalendarWithTimePart
            table.Rows.Add(CreateTableHeaderRow());
            table.Rows.Add(CreateTableCenterRow());
            RenderTableBottomRows(table);
            table.Rows.Add(CreateTableFooterRow());

            return table;
        }

        #region [ -- Calendar table header row creation methods -- ]

        /// <summary>
        /// Creates header row for the calendar table.
        /// </summary>
        private ASP.TableRow CreateTableHeaderRow()
        {
            var row = new ASP.TableRow();
            var cells = row.Cells;

            var date = EffectiveVisibleDate;
            var year = date.Year;
            var month = date.Month;

            var calendar = Culture.Calendar;
            var minYear = calendar.MinSupportedDateTime.Year;
            var maxYear = calendar.MaxSupportedDateTime.Year;
            
            /* previous year */
            cells.Add(CreateHeaderCell("calendar-previous-year",
                                       CreateHeaderCellNavigationLinkButton("py", PreviousYearCommandName, year - 1 >= minYear)));

            /* previous month */
            cells.Add(CreateHeaderCell("calendar-previous-month",
                                       CreateHeaderCellNavigationLinkButton("pm", PreviousMonthCommandName, year != minYear || month >= 2)));

            /* current month */
            cells.Add(CreateHeaderCell("calendar-current-month",
                                       CreateHeaderCellMonthLinkButton("cm"), true));

            /* next month */
            cells.Add(CreateHeaderCell("calendar-next-month",
                                       CreateHeaderCellNavigationLinkButton("nm", NextMonthCommandName, year != maxYear || month <= 11)));

            /* next year */
            cells.Add(CreateHeaderCell("calendar-next-year",
                                       CreateHeaderCellNavigationLinkButton("ny", NextYearCommandName, year + 1 <= maxYear)));

            return row;
        }

        /// <summary>
        /// Creates header cell with a <see cref="LinkButton"/> inside.
        /// </summary>
        private ASP.TableCell CreateHeaderCell(string cssClass, Control button, bool alignCenter = false)
        {
            var cell = new ASP.TableCell {CssClass = CombineCssClass(cssClass)};
            if (alignCenter)
                cell.Attributes["align"] = "center";

            cell.Controls.Add(button);
            return cell;
        }

        /// <summary>
        /// Creates <see cref="LinkButton"/> for header cell year and month navigation.
        /// </summary>
        private LinkButton CreateHeaderCellNavigationLinkButton(string id, string commandName, bool enabled)
        {
            var button = CreateLinkButton(id, "noselect", commandName);
            button.Enabled = enabled;
            button.Text = " ";
            return button;
        }

        /// <summary>
        /// Creates <see cref="LinkButton"/> for current month.
        /// </summary>
        private LinkButton CreateHeaderCellMonthLinkButton(string id)
        {
            var button = CreateLinkButton(id, "calendar-month", MonthClickCommandName);
            button.Text = GetMonthButtonText();
            return button;
        }

        /// <summary>
        /// Returns the localized name for the <see cref="VisibleDate"/> month and year.
        /// </summary>
        private string GetMonthButtonText()
        {
            var visibleDate = EffectiveVisibleDate;
            return string.Concat(GetLocalizedMonthName(visibleDate.Month), " ", visibleDate.Year);
        }

        #endregion

        /// <summary>
        /// Creates <see cref="LinkButton"/> using specified properties.
        /// </summary>
        private LinkButton CreateLinkButton(string id, string cssClass, string commandName)
        {
            return new LinkButton
            {
                ID = id,
                CausesValidation = false,
                CommandName = commandName,
                CssClass = CombineCssClass(cssClass)
            };
        }

        /// <summary>
        /// Creates table center row.
        /// </summary>
        private ASP.TableRow CreateTableCenterRow()
        {
            var row = new ASP.TableRow();
            var cell = new ASP.TableCell { ColumnSpan = 5 };
            cell.Controls.Add(CreateWeekDayTable());
            row.Cells.Add(cell);

            return row;
        }

        #region [ -- Week and Day table creation methods -- ]

        /// <summary>
        /// Creates table of weeks and days.
        /// </summary>
        private ASP.Table CreateWeekDayTable()
        {
            var table = new ASP.Table {CssClass = CombineCssClass("calendar-daywrapper"), CellSpacing = 0};

            // header row
            table.Rows.Add(CreateWeekDayTableHeaderRow());
            var rows = table.Rows;

            var startMonth = GetFirstDayOfMonth(EffectiveVisibleDate);
            var startDay = GetNumberOfDaysToSubtractFromStartDayOfMonth(startMonth);

            var cellNumber = 0;
            for (var i = 0; i < WeekCount; ++i)
            {
                var row = new ASP.TableRow();
                var cells = row.Cells;

                for (var y = 0; y < DaysPerWeek; ++y, ++cellNumber, ++startDay)
                    cells.Add(CreateWeekDayTableCell(cellNumber, startMonth, startDay));

                rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// Creates week and day table cell.
        /// </summary>
        private ASP.TableCell CreateWeekDayTableCell(int index, DateTime startMonth, int startDay)
        {
            LinkButton button = null;
            var cellCssClass = string.Empty;

            if (IsValidWeekDay(startMonth, startDay))
            {
                var date = startMonth.AddDays(startDay);

                button = CreateLinkButton(IdDayButtonPrefix + index, "calendar-day", DayClickCommandName);
                button.Text = date.Day.ToString(Culture);
                button.CommandArgument = index.ToString(NumberFormatInfo.InvariantInfo);

                cellCssClass = GetDayCellCssClass(date);

                // We fire the DayRender event to perform cssClass changes on the day which is rendered
                // This makes the calendar more extensible since you can add custom logic for each day
                // which is rendered ... //todo: pass in the TableCell too?  
                if (DayRender != null)
                {
                    var eventArgs = new DayRenderEventArgs(date);
                    DayRender(this, eventArgs);
                    if (!string.IsNullOrEmpty(eventArgs.DayCssClass))
                        cellCssClass += eventArgs.DayCssClass;
                }
            }

            var cell = new TableCell {ID = IdDayCellPrefix + index, CssClass = cellCssClass};
            if (button != null)
                cell.Controls.Add(button);
            return cell;
        }

        /// <summary>
        /// Creates row with the names of week days.
        /// </summary>
        private ASP.TableRow CreateWeekDayTableHeaderRow()
        {
            var row = new ASP.TableRow();
            var cells = row.Cells;

            for (var dayOfWeek = (int)FirstDayOfWeek; dayOfWeek < (int)FirstDayOfWeek + DaysPerWeek; ++dayOfWeek)
            {
                var cell = new ASP.TableHeaderCell();
                var label = new Label {ID = "l" + dayOfWeek, Text = GetLocalizedShortestDayName((DayOfWeek) (dayOfWeek%DaysPerWeek))};
                cell.Controls.Add(label);
                cells.Add(cell);
            }

            return row;
        }

        /// <summary>
        /// Gets localized shortest name of the specified <paramref name="day"/> of week.
        /// </summary>
        private string GetLocalizedShortestDayName(DayOfWeek day)
        {
            return Culture.DateTimeFormat.GetShortestDayName(day);
        }

        /// <summary>
        /// Returns <see cref="ASP.WebControl.CssClass"/> for day <see cref="ASP.TableCell"/> control.
        /// </summary>
        private string GetDayCellCssClass(DateTime cellDate)
        {
            string cssClass;
            var visibleDate = EffectiveVisibleDate;
            if (cellDate.Month < visibleDate.Month) // in previous month
                cssClass = CombineCssClass("calendar-previous-day");
            else if (cellDate.Month > visibleDate.Month) // in next month
                cssClass = CombineCssClass("calendar-next-day");
            else if (cellDate.Date == SelectedDate.Date) // is selected day
                cssClass = CombineCssClass("calendar-active", "calendar-selected");
            else // in current month
                cssClass = CombineCssClass("calendar-active");

            return cssClass;
        }

        #endregion

        /// <summary>
        /// Used in derived classes to add additional <see cref="ASP.TableRow"/> to the <see cref="Calendar"/>.
        /// </summary>
        /// <param name="placeholder"><see cref="ASP.Table"/> to add additional rows to.</param>
        /// <remarks>
        /// The content will be added just above the row with the buttons for closing and setting the today's date. 
        /// </remarks>
        protected virtual void RenderTableBottomRows(ASP.Table placeholder) { }

        #region [ -- Calendar table footer row creation methods -- ]

        /// <summary>
        /// Creates Calendar table footer row.
        /// </summary>
        private ASP.TableRow CreateTableFooterRow()
        {
            var cell = new ASP.TableCell {CssClass = CombineCssClass("calendar-bottom"), ColumnSpan = 5};
            cell.Attributes["align"] = "center";
            cell.Controls.Add(CreateFooterTable());

            var row = new ASP.TableRow();
            row.Cells.Add(cell);

            return row;
        }

        /// <summary>
        /// Creates footer table for the footer row.
        /// </summary>
        private ASP.Table CreateFooterTable()
        {
            var button = CreateCommandButton("cb", "CloseCalendar", CloseCalendarCommandName);
            button.Visible = ShowCloseButton;

            var closeCalendarCell = new ASP.TableCell();
            closeCalendarCell.Controls.Add(button);

            button = CreateCommandButton("tb", "Today", TodayClickCommandName);
            var todayCalendarCell = new ASP.TableCell();
            todayCalendarCell.Controls.Add(button);

            var row = new ASP.TableRow();
            row.Cells.Add(closeCalendarCell);
            row.Cells.Add(todayCalendarCell);

            var table = new ASP.Table { CellSpacing = 0, CellPadding = 0, BorderWidth = 0 };
            table.Rows.Add(row);

            return table;
        }

        /// <summary>
        /// Creates <see cref="ExtendedButton"/> with the specialized properties.
        /// </summary>
        private ExtendedButton CreateCommandButton(string id, string key, string commandName)
        {
            return new ExtendedButton
                       {
                           ID = id,
                           CssClass = CssClass,
                           Text = GetValue(key),
                           Width = GetButtonWidth(),
                           CausesValidation = false,
                           CommandName = commandName
                       };
        }

        /// <summary>
        /// Returns the width of the command button, such as close or today buttons.
        /// </summary>
        private ASP.Unit GetButtonWidth()
        {
            return ASP.Unit.Parse(GetValue("ButtonWidth"), CultureInfo.InvariantCulture);
        }

        #endregion

        #endregion

        #region [ -- Calendar Month And Year Picker creation methods -- ]

        /// <summary>
        /// Creates Month and Year Picker.
        /// </summary>
        private Panel CreateMonthAndYearPicker()
        {
            var root = new Panel
                           {
                               ID = IdRootPicker,
                               Visible = MonthPickerVisible,
                               Height = GetMonthYearPickerHeight(),
                               CssClass = CombineCssClass("calendar-ympicker")
                           };
            
            if (AnimationDuration > 0)
                root.Style["display"] = "none";

            root.Controls.Add(CreatePickerTable());
            return root;
        }

        /// <summary>
        /// Creates Calendar Month And Year Picker table.
        /// </summary>
        /// <returns></returns>
        private ASP.Table CreatePickerTable()
        {
            var pickerTable = new ASP.Table
                                  {
                                      CellSpacing = 0,
                                      CellPadding = 0,
                                      Width = ASP.Unit.Percentage(100),
                                      Height = ASP.Unit.Percentage(100),
                                      BorderWidth = 0
                                  };
            var rows = pickerTable.Rows;

            var calendar = Culture.Calendar;
            var minYear = calendar.MinSupportedDateTime.Year;
            var maxYear = calendar.MaxSupportedDateTime.Year;
            var startYear = StartYearInMonthYearPicker;

            for (int i = 1, y = DaysPerWeek; i < DaysPerWeek; ++i, ++y)
            {
                var row = new ASP.TableRow();
                var cells = row.Cells;

                for (int j = 0, month = i; j < 2; ++j, month = y)
                    cells.Add(CreatePickerMonthCell(month));

                if (i == 1)
                {
                    // previous year
                    var button = CreateLinkButton("pyb", "calendar-ympicker-previous", PickerPreviousYearCommandName);
                    button.Enabled = startYear - PickerYearChangeOffset >= minYear;
                    cells.Add(CreatePickerCell("pyc", "calendar-ympicker-yearbutton", button));

                    // next year
                    button = CreateLinkButton("nyb", "calendar-ympicker-next", PickerNextYearCommandName);
                    button.Enabled = startYear + PickerYearChangeOffset <= maxYear;
                    cells.Add(CreatePickerCell("nyc", "calendar-ympicker-yearbutton", button));
                }
                else
                {
                    for (int j = 0, year = i; j < 2; ++j, ++startYear, year = y)
                        cells.Add(CreatePickerYearCell(year, startYear, startYear >= minYear && startYear <= maxYear));
                }

                rows.Add(row);
            }

            rows.Add(CreatPickerTableFooterRow());
            return pickerTable;
        }

        /// <summary>
        /// Creates Month And Year Picker month cell.
        /// </summary>
        private ASP.TableCell CreatePickerMonthCell(int month)
        {
            var cell = new TableCell {ID = "mc" + month, CssClass = GetPickerMonthCellCssClass(month)};
            cell.Controls.Add(CreatePickerLinkButton("m" + month, GetLocalizedMonthName(month), MonthChangeCommandName,
                                                     month.ToString(NumberFormatInfo.InvariantInfo)));
            return cell;
        }

        /// <summary>
        /// Creates Month And Year Picker year cell.
        /// </summary>
        private ASP.TableCell CreatePickerYearCell(int index, int year, bool isValid)
        {
            var cell = new TableCell {ID = "yc" + index};

            if (isValid)
            {
                cell.CssClass = GetPickerYearCellCssClass(year);
                cell.Controls.Add(CreatePickerLinkButton("yl" + index, year.ToString(Culture), YearChangeCommandName,
                                                         year.ToString(NumberFormatInfo.InvariantInfo)));
            }

            return cell;
        }

        /// <summary>
        /// Creates a <see cref="LinkButton"/> for the Month and Year Picker.
        /// </summary>
        private LinkButton CreatePickerLinkButton(string id, string text, string commandName, string commandArgument)
        {
            return new LinkButton
                       {
                           ID = id,
                           Text = text,
                           CausesValidation = false,
                           CommandName = commandName,
                           CommandArgument = commandArgument
                       };
        }

        /// <summary>
        /// Creates Month And Year Picker cell.
        /// </summary>
        private ASP.TableCell CreatePickerCell(string id, string cssClass, Control button)
        {
            var cell = new ASP.TableCell {ID = id, CssClass = CombineCssClass(cssClass)};
            cell.Attributes["align"] = "center";
            cell.Controls.Add(button);
            return cell;
        }

        /// <summary>
        /// Creates Month And Year Picker table footer row.
        /// </summary>
        /// <returns></returns>
        private ASP.TableRow CreatPickerTableFooterRow()
        {
            // Adding controls
            var cell = new ASP.TableCell {CssClass = CombineCssClass("calendar-bottom"), ColumnSpan = 4};
            cell.Attributes["align"] = "center";
            cell.Controls.Add(CreateCommandButton("b", "Close", PickerCloseCommandName));

            // adding the last Close picker row
            var row = new ASP.TableRow();
            row.Controls.Add(cell);
            return row;
        }

        /// <summary>
        /// Returns <see cref="ASP.WebControl.CssClass"/> for the specified <paramref name="month"/> cell.
        /// </summary>
        private string GetPickerMonthCellCssClass(int month)
        {
            return EffectiveVisibleDate.Month == month
                       ? CombineCssClass("calendar-ympicker-month", "calendar-ympicker-selected")
                       : CombineCssClass("calendar-ympicker-month");
        }

        /// <summary>
        /// Returns <see cref="ASP.WebControl.CssClass"/> for the specified <paramref name="year"/> cell.
        /// </summary>
        private string GetPickerYearCellCssClass(int year)
        {
            return EffectiveVisibleDate.Year == year
                       ? CombineCssClass("calendar-ympicker-year", "calendar-ympicker-selected")
                       : CombineCssClass("calendar-ympicker-year");
        }

        #endregion

        #region [ -- Calendar Command handlers -- ]

        /// <summary>
        /// Handles <see cref="VisibleDate"/> change commands and raises <see cref="ActiveDateViewChanged"/> event.
        /// </summary>
        /// <param name="date">New <see cref="VisibleDate"/> to set.</param>
        private bool HandleVisibleDateCommand(DateTime date)
        {
            VisibleDate = date;

            if (ActiveDateViewChanged != null)
                ActiveDateViewChanged(this, EventArgs.Empty);

            return true;
        }

        /// <summary>
        /// Handles moth click command.
        /// </summary>
        private bool HandleMonthClickCommand()
        {
            MonthPickerVisible = true;
            if (AnimationDuration > 0)
                FireMonthPickerEffect();

            return true;
        }

        /// <summary>
        /// Handles day click command.
        /// </summary>
        /// <param name="index">Index of the day.</param>
        private bool HandleDayClickCommand(int index)
        {
            var newDate = GetDayByIndex(index);

            var selectedDate = SelectedDate;
            if (selectedDate.Date != newDate.Date)
            {
                var canSelect = true;

                // simple way to keep the timepart
                var timeSpan = new TimeSpan(selectedDate.Hour, selectedDate.Minute, selectedDate.Second);

                // We fire the CalendarDayClicking event to check if the user wants to allow selection of this date
                // Set AllowSelection to false 
                if (CalendarDayClicking != null)
                {
                    //use firemethod instead
                    var eventArgs = new CalendarDayClickingEventArgs(newDate);
                    CalendarDayClicking(this, eventArgs);
                    canSelect = eventArgs.AllowSelection;
                }

                if (canSelect)
                {
                    SelectedDate = newDate.Add(timeSpan);
                    Validate();
                    RaiseSelectedDateChanged();

                    if (CalendarDayClicked != null) //todo: use fireMethod instead
                        CalendarDayClicked(this, EventArgs.Empty);
                }
            }

            return true;
        }

        /// <summary>
        /// Handles <see cref="Calendar"/> close command.
        /// </summary>
        private bool HandleCloseCalendarCommand()
        {
            if (CloseCalendarClicked != null)
                CloseCalendarClicked(this, EventArgs.Empty);

            return true;
        }

        /// <summary>
        /// Handles click command for the today button.
        /// </summary>
        private bool HandleTodayClickCommand()
        {
            SelectedDate = DateTime.Now;
            Validate();
            RaiseSelectedDateChanged();

            if (TodayButtonClicked != null)
                TodayButtonClicked(this, EventArgs.Empty);

            return true;
        }

        /// <summary>
        /// Handles MonthAndYear Picker year change command.
        /// </summary>
        /// <param name="offset">Year offset to set.</param>
        private bool HandlePickerYearCommand(int offset)
        {
            StartYearInMonthYearPicker += offset;
            return true;
        }

        /// <summary>
        /// Handles MonthAndYear Picker close command.
        /// </summary>
        /// <returns></returns>
        private bool HandlePickerCloseCommand()
        {
            MonthPickerVisible = false;
            return true;
        }

        /// <summary>
        /// Raises <see cref="SelectedDateChanged"/> event.
        /// </summary>
        private void RaiseSelectedDateChanged()
        {
            if (SelectedDateChanged == null) return;
            SelectedDateChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Runs page validation if required.
        /// </summary>
        internal void Validate()
        {
            var page = Page;
            if (!CausesValidation || page == null) return;
            page.Validate(ValidationGroup);
        }

        #endregion

        #region [-- Helpers --]

        /// <summary>
        /// Used in derived classes to specify the height of the MonthYearPicker.
        /// </summary>
        /// <returns>Height of the MonthYearPicker.</returns>
        protected virtual int GetMonthYearPickerHeight()
        {
            return MonthYearPickerHeight;
        }

        private void FireMonthPickerEffect()
        {
            var pickerPanel = (Panel)FindControl(IdRootPanel + ":" + IdRootPicker);
            pickerPanel.Effects.Add(new EffectSlideDown(AnimationDuration / 1000M));
        }

        private ResourceManager _resourceManager;
        const string ResourceClass = "Gaia.WebWidgets.Extensions.Resources.Calendar";
        private const int MonthYearPickerHeight = 188;

        ResourceManager GetResourceManager()
        {
            return _resourceManager ?? (_resourceManager = new ResourceManager(ResourceClass, Assembly.GetExecutingAssembly()));
        }

        private string GetValue(string key)
        {
            var val = HttpContext.GetGlobalResourceObject(ResourceClass, key, Culture);
            return val != null ? val.ToString() : GetResourceManager().GetString(key, Culture);
        }

        private string GetLocalizedMonthName(int month)
        {
            var monthName = Culture.DateTimeFormat.GetAbbreviatedMonthName(month);
            return string.Concat(monthName[0].ToString().ToUpperInvariant(), monthName.Substring(1, monthName.Length - 1));
        }

        /// <summary>
        /// Returns first day of the month of the specified <paramref name="date"/>.
        /// </summary>
        private static DateTime GetFirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        private DateTime GetDayByIndex(int idx)
        {
            var startMonth = GetFirstDayOfMonth(EffectiveVisibleDate);
            var startDay = GetNumberOfDaysToSubtractFromStartDayOfMonth(startMonth);

            for (var i = 0; i < DaysPerWeek * WeekCount; ++i, ++startDay)
            {
                if (i == idx)
                    return startMonth.AddDays(startDay);
            }

            throw new ArgumentOutOfRangeException("idx");
        }

        private int GetNumberOfDaysToSubtractFromStartDayOfMonth(DateTime startMonth)
        {
            var startingIndex = (startMonth.DayOfWeek - FirstDayOfWeek);
            if (startingIndex < 0)
                startingIndex += DaysPerWeek;

            return -startingIndex;
        }

        /// <summary>
        /// Returns true if the specified <paramref name="dayOffset"/> can be added to the specified
        /// <paramref name="startDate"/> to create representable <see cref="DateTime"/>.
        /// </summary>
        private bool IsValidWeekDay(DateTime startDate, int dayOffset)
        {
            var date = startDate.Date;
            var year = date.Year;
            var month = date.Month;

            var calendar = Culture.Calendar;
            var minDate = calendar.MinSupportedDateTime.Date;
            if (minDate.Year == year && minDate.Month == month)
                return date.Day + dayOffset >= 1;

            var maxDate = calendar.MaxSupportedDateTime.Date;
            var maxYear = maxDate.Year;
            var maxMonth = maxDate.Month;

            if (maxYear == year && maxMonth == month)
                return date.Day + dayOffset <= DateTime.DaysInMonth(maxYear, maxMonth);

            return true;
        }

        /// <summary>
        /// Combines and returns variable number of css classes.
        /// </summary>
        /// <param name="cssclasses">Css classes to combine</param>
        /// <returns>Combined string of provided css classes.</returns>
        protected string CombineCssClass(params string[] cssclasses)
        {
            return Css.Combine(CssClass, cssclasses);
        }

        #endregion

        #region [ -- IAjaxControl and IAjaxContainerControl implementation -- ]

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new AjaxContainerControl(this)); }
        }

        string IAjaxControl.TagName
        {
            get { return "div"; }
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
            RenderChildren(create.GetHtmlTextWriter());
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

        #region [-- ISkinControl Implementation -- ]

        void ISkinControl.ApplySkin()
        {
            if (!string.IsNullOrEmpty(CssClass)) return;

            // include default skin css file
            AjaxContainerControl.RegisterDefaultSkinStyleSheetFromResource(typeof(Calendar), Constants.DefaultSkinResource);

            // name of the default skin;
            CssClass = Constants.DefaultSkinCssClass;
        }

        bool ISkinControl.Enabled
        {
            get { return string.IsNullOrEmpty(CssClass) || CssClass.Equals(Constants.DefaultSkinCssClass); }
        }

        #endregion
    }
}
