namespace Gaia.WebWidgets.Samples.Combinations.WebApps.SimpleCalendarApplication
{
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Gaia.WebWidgets.Extensions;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class CalendarApplication : UserControl
    {
        private readonly CalendarController _calendarController = new CalendarController(20);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SelectedDate = DateTime.Now.Date;
                BindData();
            }
        }

        #region UI Logic

        private DateTime SelectedDate
        {
            get { return (DateTime)(ViewState["sd"] ?? DateTime.Now.Date); }
            set
            {
                ViewState["sd"] = value;
                BindData();
            }
        }

        protected void zGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var dataKey = zGridView.DataKeys[e.RowIndex];
            if (dataKey == null) return;

            _calendarController.Delete(int.Parse(dataKey.Value.ToString()));
            BindData();
        }

        private void BindData()
        {
            zScheduler.MonthView.Month = SelectedDate;
            zScheduler.DataSource = _calendarController.CalendarItems;
            zScheduler.DataBind();

            zGridView.DataSource = _calendarController.GetByDate(SelectedDate);
            zGridView.DataBind();

            zGvHeader.Text = SelectedDate.ToString("dddd dd", CultureInfo.InvariantCulture).ToLower() +
                             (zGridView.Rows.Count == 0 ? ": No planned items." : "");

            zLblCurrentMonth.Text = SelectedDate.ToString("MMMM yyyy", CultureInfo.InvariantCulture).ToLower();
        }

        protected void zToday_Clicked(object sender, EventArgs e)
        {
            SelectedDate = DateTime.Now.Date;
        }

        protected void next_click(object sender, EventArgs e)
        {
            SelectedDate = SelectedDate.AddMonths(1);
        }

        protected void previous_click(object sender, EventArgs e)
        {
            SelectedDate = SelectedDate.AddMonths(-1);
        }

        protected void newApp_click(object sender, EventArgs e)
        {
            AddAppointment();
        }

        protected void zBtnCancel_Click(object sender, EventArgs e)
        {
            zNewAppWindow.Visible = false;
        }

        protected void LoadClickablePanel(object sender, EventArgs e)
        {
            var clickableControl = (IAspectableAjaxControl) sender;
            var currentCellDate = ((ReportTemplateWrapperPanel) sender).Date;

            var clickable = new AspectClickable();
            clickable.Clicked += delegate
                {
                    SelectedDate = currentCellDate;
                };

            clickable.DblClicked += delegate
                {
                    SelectedDate = currentCellDate;
                    AddAppointment();
                };

            clickableControl.Aspects.Add(clickable);

            var droppable = new AspectDroppable();
            droppable.HoverClass = "drop-hover";
            droppable.Dropped += delegate(object o, AspectDroppable.DroppedEventArgs args)
                {
                    var draggedRowItemIndex = int.Parse(args.IdToPass);
                    var droppedCalendarItem = _calendarController.GetByDate(SelectedDate)[draggedRowItemIndex];

                    var canAddMoreAppointments = CanAddMoreAppointments(currentCellDate);

                    if (!canAddMoreAppointments)
                        return;

                    ChangeDatePart(droppedCalendarItem, currentCellDate);
                    BindData();
                };

            clickableControl.Aspects.Add(droppable);
        }

        protected void GridRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            e.Row.ToolTip = "Drag to new day in scheduler";

            var row = (IAjaxContainerControl)e.Row;
            var dragAspect = new AspectDraggable
            {
                DeepCopy = true,
                MakeGhost = true,
                DragCssClass = "row-drag",
                IdToPass = e.Row.RowIndex.ToString()
            };
            row.Aspects.Add(dragAspect);
        }

        protected string GetDayTemplateCssClass(IReportContainer container)
        {
            return "day-container noselect " +
                   (IsSelectedDay(container.Date) ? " selected" : "") +
                   (IsToday(container.Date) ? " today" : "");
        }

        protected string GetDayCssClass(DateTime value)
        {
            var selected = SelectedDate;
            var isCurrentMonthYear = value.Year == selected.Year && value.Month == selected.Month;

            return "day noselect " + (isCurrentMonthYear ? "" : " other-month");
        }

        #endregion

        #region Business logic
        
        private void AddAppointment()
        {
            var canAddMoreAppointments = CanAddMoreAppointments(SelectedDate);

            if (!canAddMoreAppointments)
                return;

            zNewAppWindow.Visible = true;
            zTxtTaskName.Text = "";
            zTxtTaskName.Focus();
            zTxtPerson.Text = "";
            var date = SelectedDate;
            zDateAppointment.Value = new DateTime(date.Year, date.Month, date.Day, 8, 0, 0);
        }

        private bool CanAddMoreAppointments(DateTime date)
        {
            return GetAppointmentsCountForDay(date) < 10;
        }

        protected void zBtnOk_Click(object sender, EventArgs e)
        {
            var appointmentDate = zDateAppointment.Value.Value;

            var canAddMoreAppointments = CanAddMoreAppointments(appointmentDate);

            if (!canAddMoreAppointments)
                return;

            var newItem = new CalendarItem(zTxtTaskName.Text, appointmentDate, zTxtPerson.Text,
                                           _calendarController.GetNextId(), 0, 0D, 0D);
            _calendarController.Add(newItem);

            zNewAppWindow.Visible = false;
            SelectedDate = appointmentDate;
            BindData();
        }
        
        protected int GetAppointmentsCountForDay(DateTime date)
        {
            return _calendarController.GetByDate(date).Count;
        }

        private static void ChangeDatePart(CalendarItem droppedCalendarItem, DateTime newDate)
        {
            var prevDate = droppedCalendarItem.ActivityDate;
            droppedCalendarItem.ActivityDate = new DateTime(newDate.Year, newDate.Month, newDate.Day, prevDate.Hour,
                                                            prevDate.Minute, 0);
        }

        private bool IsSelectedDay(DateTime dateTime)
        {
            return dateTime.Date == SelectedDate.Date;
        }

        private bool IsToday(DateTime date)
        {
            return date == DateTime.Now.Date;
        }

        #endregion
    }
}