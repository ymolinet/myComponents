namespace Gaia.WebWidgets.Samples.Extensions.Calendar.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                zInfo.Text = "No date is currently selected";
        }

        protected void zCalendar_TodayButtonClicked(object sender, EventArgs e)
        {
            SetDateAndText("The Today button was clicked. The Date is :");
        }

        protected void zCalendar_CalendarDayClicked(object sender, EventArgs e)
        {
           SetDateAndText("You selected");
        }

        protected void zCalendar_ActiveDateViewChanged(object sender, EventArgs e)
        {
            zInfo.Text = "The ActiveView changed to " + 
                zCalendar.VisibleDate.Date.Month + 
                "/" + 
                zCalendar.VisibleDate.Date.Year
                ;
        }

        void SetDateAndText(string text)
        {
            zInfo.Text = text + " " + 
                           zCalendar.SelectedDate.ToShortDateString();
            zClearDate.Enabled = true;
            
        }

        protected void zClearDate_Click(object sender, EventArgs e)
        {
            // We clear the selected date by setting the MinValue 
            zCalendar.SelectedDate = DateTime.MinValue;
            zClearDate.Enabled = false;
            zInfo.Text = "Selected Date cleared"; 
        }
    }
}
