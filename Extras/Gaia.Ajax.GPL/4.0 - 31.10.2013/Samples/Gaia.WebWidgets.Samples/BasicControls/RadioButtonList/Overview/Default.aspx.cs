namespace Gaia.WebWidgets.Samples.BasicControls.RadioButtonList.Overview
{
    using System;
    using ASP = System.Web.UI.WebControls;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zRadioButtonList1_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            zLabel1.Text = "You selected " + zRadioButtonList1.SelectedItem.Text;
        }

        protected void zRepeatDirection_CheckedChanged(object sender, EventArgs e)
        {
            zRadioButtonList1.RepeatDirection = zRepeatDirection.Checked
                                                    ? ASP.RepeatDirection.Horizontal
                                                    : ASP.RepeatDirection.Vertical;
        }

        protected void zRepeatLayout_OnCheckedChanged(object sender, EventArgs e)
        {
            zRadioButtonList1.RepeatLayout = zRepeatLayout.Checked
                                                 ? ASP.RepeatLayout.Table
                                                 : ASP.RepeatLayout.Flow;
        }
    }
}
