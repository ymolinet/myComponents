namespace Gaia.WebWidgets.Samples.BasicControls.Validators.CompareValidator
{
    using System;
    using System.Drawing;
    using System.Web.UI.WebControls;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zValidate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                zResult.Text = "Result: Valid!";
                zResult.ForeColor = Color.Green;
            }
            else
            {
                zResult.Text = "Result: Not valid!";
                zResult.ForeColor = Color.Red;
            }

        }

        protected void zOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            zCompareValidator1.Operator = (ValidationCompareOperator)zOperator.SelectedIndex;
            zCompareValidator1.Validate();
        }

        protected void zDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            zCompareValidator1.Type = (ValidationDataType)zDataType.SelectedIndex;
            zCompareValidator1.Validate();
        }
    }
}
