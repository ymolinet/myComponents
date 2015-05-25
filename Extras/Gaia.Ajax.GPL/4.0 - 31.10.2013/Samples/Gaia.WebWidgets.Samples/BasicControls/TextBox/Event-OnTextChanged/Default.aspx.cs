namespace Gaia.WebWidgets.Samples.BasicControls.TextBox.Event_OnTextChanged
{
    using System;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        private readonly CalendarController _calendarController = new CalendarController(50);

        protected void Page_Load(object sender, EventArgs e)
        {  
            if (!IsPostBack)
                BindCalendarItems();
        }

        private void BindCalendarItems()
        {
            zRepeater.DataSource = _calendarController.CalendarItems;
            zRepeater.DataBind();
        }

        protected void zFilter_TextChanged(object sender, EventArgs e)
        {
            //set value to filter the datasource, can be any datasource
            _calendarController.Filter = zFilter.Text;
            BindCalendarItems();
        }
    }
}
