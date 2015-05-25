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
using System.Globalization;
using System.ComponentModel;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    /// <summary>
    /// Extended <see cref="Calendar"/> with a time picker.
    /// </summary>
    [ToolboxItem(false)]
    public class CalendarWithTimePart : Calendar
    {
        #region [ -- Time Picker Command Names -- ]

        private const string AddHourCommandName = "ah";
        private const string SubtractHourCommandName = "suh";
        private const string SetHourCommandName = "sh";
        private const string SetMinuteCommandName = "sm";
        private const string AddMinuteCommandName = "am";
        private const string SubtractMinuteCommandName = "sum";
        private const string ToggleAmPmCommandName = "tc";
        private const int TimePartHeight = 20;
        private const int TimePickerInPlaceEditWidth = 20;

        #endregion

        #region [ -- Events -- ]

        /// <summary>
        /// Raised when selected time is changed.
        /// </summary>
        public event EventHandler SelectedTimeChanged;

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Gets or sets if the time picker is rendered.
        /// </summary>
        [DefaultValue(false)]
        public bool HasTimePart
        {
            get { return StateUtil.Get(ViewState, "HasTimePart", false); }
            set { SetStateValue("HasTimePart", value, false); }
        }

        private bool UseMilitaryFormat
        {
            get { return Format.Contains("HH"); }
        }

        #endregion

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
                
                if (string.CompareOrdinal(AddHourCommandName, commandName) == 0)
                    return HandleAddHourCommand(1);

                if (string.CompareOrdinal(SubtractHourCommandName, commandName) == 0)
                    return HandleAddHourCommand(-1);

                if (string.CompareOrdinal(SetHourCommandName, commandName) == 0)
                    return HandleSetTimeCommand(GetHour(commandArguments.CommandArgument.ToString()), -1);

                if (string.CompareOrdinal(SetMinuteCommandName, commandName) == 0)
                    return HandleSetTimeCommand(-1, GetMinute(commandArguments.CommandArgument.ToString()));

                if (string.CompareOrdinal(AddMinuteCommandName, commandName) == 0)
                    return HandleAddMinuteCommand(1);

                if (string.CompareOrdinal(SubtractMinuteCommandName, commandName) == 0)
                    return HandleAddMinuteCommand(-1);

                if (string.CompareOrdinal(ToggleAmPmCommandName, commandName) == 0)
                {
                    var selectedHour = SelectedDate.Hour;
                    var hours = selectedHour > 0 && selectedHour < 13 ? 12 : -12;
                    return HandleAddHourCommand(hours);
                }
            }

            return base.OnBubbleEvent(source, args);
        }

        /// <summary>
        /// Used in derived classes to add additional <see cref="ASP.TableRow"/> to the <see cref="Calendar"/>.
        /// </summary>
        /// <param name="placeholder"><see cref="ASP.Table"/> to add additional rows to.</param>
        protected override void RenderTableBottomRows(ASP.Table placeholder)
        {
            var cell = new ASP.TableCell { CssClass = CombineCssClass("calendar-current-month"), ColumnSpan = 5 };
            cell.Attributes["align"] = "center";
            cell.Controls.Add(CreateTimePickerTable());

            var row = new ASP.TableRow {Visible = HasTimePart};
            row.Cells.Add(cell);
            placeholder.Rows.Add(row);
        }

        #endregion

        #region [ -- Time Picker Creation Methods -- ]

        /// <summary>
        /// Creates <see cref="ASP.Table"/> for Time picker.
        /// </summary>
        private ASP.Table CreateTimePickerTable()
        {
            var row = new ASP.TableRow();
            var cells = row.Cells;

            cells.Add(CreateTimePickerTimeAddCell("+", AddHourCommandName));
            cells.Add(CreateTimePickerTimeAddCell("-", SubtractHourCommandName));
            cells.Add(CreateTimePickerTimeEditCell(UseMilitaryFormat ? "HH" : "hh", SetHourCommandName));
            cells.Add(CreateTimePickerSeparatorCell());
            cells.Add(CreateTimePickerTimeEditCell("mm", SetMinuteCommandName));
            cells.Add(CreateTimePickerAmPmToggleCell());
            cells.Add(CreateTimePickerTimeAddCell("+", AddMinuteCommandName));
            cells.Add(CreateTimePickerTimeAddCell("-", SubtractMinuteCommandName));

            var pickerTable = new ASP.Table { CellSpacing = 0, CellPadding = 0, BorderWidth = 0 };
            pickerTable.Rows.Add(row);
            return pickerTable;
        }

        /// <summary>
        /// Creates <see cref="ASP.TableCell"/> for adding hours.
        /// </summary>
        /// <returns></returns>
        private ASP.TableCell CreateTimePickerTimeAddCell(string text, string commandName)
        {
            /* add hours */
            var cell = CreateCell("calendar-withtime-nav");
            cell.Controls.Add(CreateLinkButton(text, commandName));
            return cell;
        }

        /// <summary>
        /// Creates <see cref="ASP.TableCell"/> for editing hour or minute.
        /// </summary>
        private ASP.TableCell CreateTimePickerTimeEditCell(string format, string commandName)
        {
            var editor = new InPlaceEdit
            {
                Width = TimePickerInPlaceEditWidth,
                Text = SelectedDate.ToString(format, Culture),
                CssClass = CombineCssClass("calendar-withtime-part")
            };
            editor.TextChanged += delegate { OnBubbleEvent(this, new ASP.CommandEventArgs(commandName, editor.Text)); };

            var cell = CreateCell("calendar-withtime-nav");
            cell.Controls.Add(editor);
            return cell;
        }

        /// <summary>
        /// Creates <see cref="ASP.TableCell"/> which acts as a separator between hour and minute parts.
        /// </summary>
        private ASP.TableCell CreateTimePickerSeparatorCell()
        {
            var cell = CreateCell("calendar-withtime-nav");
            cell.Width = 2;
            cell.Controls.Add(new ASP.Label {Text = ":"});
            return cell;
        }

        /// <summary>
        /// Returns AM/PM Designator.
        /// </summary>
        private string GetDesignator()
        {
            var selectedHour = SelectedDate.Hour;
            var designator = selectedHour > 0 && selectedHour < 13
                                 ? Culture.DateTimeFormat.AMDesignator
                                 : Culture.DateTimeFormat.PMDesignator;
            return designator.ToLowerInvariant();
        }

        /// <summary>
        /// Creates <see cref="ASP.TableCell"/> for toggling AM/PM parts of the time.
        /// </summary>
        private ASP.TableCell CreateTimePickerAmPmToggleCell()
        {
            var button = CreateLinkButton(GetDesignator(), ToggleAmPmCommandName);
            button.CssClass = CombineCssClass("calendar-month");

            var cell = CreateCell("calendar-withtime-nav");
            cell.Visible = UseMilitaryFormat;
            cell.Controls.Add(button);
            return cell;
        }

        /// <summary>
        /// Creates <see cref="LinkButton"/> with the specified properties.
        /// </summary>
        private static LinkButton CreateLinkButton(string text, string commandName)
        {
            return new LinkButton {Text = text, CommandName = commandName};
        }

        /// <summary>
        /// Creates <see cref="ASP.TableCell"/> with <see cref="ASP.WebControl.CssClass"/> combined
        /// with the specified <paramref name="cssClass"/>.
        /// </summary>
        private ASP.TableCell CreateCell(string cssClass)
        {
            return new ASP.TableCell {CssClass = CombineCssClass(cssClass)};
        }

        #endregion

        #region [ -- Time Picker Command Handlers -- ]

        /// <summary>
        /// Handles add or subtract hour commands.
        /// </summary>
        private bool HandleAddHourCommand(int hours)
        {
            var selectedDate = SelectedDate;
            SelectedDate = selectedDate == DateTime.MinValue
                               ? DateTime.Today.AddHours(hours)
                               : selectedDate.AddHours(hours);

            Validate();
            RaiseSelectedTimeChanged();
            return true;
        }

        /// <summary>
        /// Handles add or subtract minute commands.
        /// </summary>
        private bool HandleAddMinuteCommand(int minutes)
        {
            var selectedDate = SelectedDate;
            SelectedDate = selectedDate == DateTime.MinValue
                               ? DateTime.Today.AddMinutes(minutes)
                               : selectedDate.AddMinutes(minutes);

            Validate();
            RaiseSelectedTimeChanged();
            return true;
        }

        /// <summary>
        /// Handles setting hour or minute commands.
        /// </summary>
        private bool HandleSetTimeCommand(int hour, int minute)
        {
            if (minute != -1 || hour != -1)
            {
                var selectedDate = SelectedDate;
                var startingPoint = selectedDate == DateTime.MinValue ? DateTime.Now : selectedDate;

                var newHour = hour != -1 ? hour : startingPoint.Hour;
                var newMinute = minute != -1 ? minute : startingPoint.Minute;

                SelectedDate = new DateTime(startingPoint.Year, startingPoint.Month, startingPoint.Day,
                                            newHour, newMinute, 0);

                Validate();
                RaiseSelectedTimeChanged();
            }

            return true;
        }

        #endregion

        #region [ -- Helpers -- ]

        /// <summary>
        /// Used in derived classes to specify the height of the MonthYearPicker.
        /// </summary>
        /// <returns>Height of the MonthYearPicker.</returns>
        protected override int GetMonthYearPickerHeight()
        {
            return base.GetMonthYearPickerHeight() + (HasTimePart ? TimePartHeight : 0); // add pixels for the time picker part ... 
        }

        /// <summary>
        /// Parses specified <paramref name="text"/> as an hour and returns the result.
        /// </summary>
        private int GetHour(string text)
        {
            int hour;
            var success = int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out hour);
            var maxHours = UseMilitaryFormat ? 23 : 12;
            return !success || hour > maxHours || hour < 0 ? -1 : hour;
        }

        /// <summary>
        /// Parses specified <paramref name="text"/> as a minute and returns the result.
        /// </summary>
        private static int GetMinute(string text)
        {
            int minute;
            var success = int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out minute);
            return !success || minute > 59 || minute < 0 ? -1 : minute;
        }

        /// <summary>
        /// Raises <see cref="SelectedTimeChanged"/> event.
        /// </summary>
        private void RaiseSelectedTimeChanged()
        {
            if (SelectedTimeChanged == null) return;
            SelectedTimeChanged(this, EventArgs.Empty);
        }

        #endregion  
    }
}

