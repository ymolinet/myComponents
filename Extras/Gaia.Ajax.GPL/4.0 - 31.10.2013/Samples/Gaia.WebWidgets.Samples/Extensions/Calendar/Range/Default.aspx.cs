namespace Gaia.WebWidgets.Samples.Extensions.Calendar.Range
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Calendars_Changed(object sender, EventArgs e)
        {
            var a = zCalendarStart.SelectedDate;
            var b = zCalendarEnd.SelectedDate;

            if (a == DateTime.MinValue || b == DateTime.MinValue || a >= b)
            {
                zTimeSpan.Text = "Range must be positive";
            }
            else
            {
                var period = b.Date - a.Date;

                const string txt = "You have selected a range of {0} days";
                zTimeSpan.Text = string.Format(txt, period.TotalDays);
            }
        }
    }
}
