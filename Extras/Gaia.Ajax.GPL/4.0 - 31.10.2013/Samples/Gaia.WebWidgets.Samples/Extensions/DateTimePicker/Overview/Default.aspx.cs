
namespace Gaia.WebWidgets.Samples.Extensions.DateTimePicker.Overview
{
    using System;
    using ASP = System.Web.UI.WebControls;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                zDateTimePicker1.Value = DateTime.Now;
        }

        protected void zDateTimePicker1_SelectedDateChanged(object sender, EventArgs e)
        {
            zResult.Text = "Date and time selected: " + (zDateTimePicker1.Value.HasValue
                                                             ? zDateTimePicker1.Value.Value.ToString(zFormat.Text)
                                                             : "No date set");
        }

        protected void zFirstDayOfWeek_OnInit(object sender, EventArgs e)
        {
            zFirstDayOfWeek.SelectedIndex = -1;
            zFirstDayOfWeek.Items.FindByValue(zDateTimePicker1.FirstDayOfWeek.ToString()).Selected = true;
        }

        protected void zFirstDayOfWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            zDateTimePicker1.FirstDayOfWeek = (DayOfWeek) Enum.Parse(typeof (DayOfWeek), zFirstDayOfWeek.SelectedValue);
        }

        protected void zFormat_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                zDateTimePicker1.Format = zFormat.Text;
            }
            catch (Exception ex)
            {
                zFormatErrorMsg.Text = "An error occured: " + ex.Message;
            }
        }

        protected void zHasTimePart_Checked(object sender, EventArgs e)
        {
            zDateTimePicker1.HasTimePart = zHasTimePart.Checked;
        }
    }
}
