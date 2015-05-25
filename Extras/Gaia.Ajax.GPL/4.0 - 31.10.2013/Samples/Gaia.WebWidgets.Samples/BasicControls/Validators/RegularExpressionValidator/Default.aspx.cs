namespace Gaia.WebWidgets.Samples.BasicControls.Validators.RegularExpressionValidator
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            zResult.Text = "The email address you submitted was valid";
        }
    }
}