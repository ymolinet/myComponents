namespace Gaia.WebWidgets.Samples.BasicControls.Validators.RangeValidator
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zSubmit_Click(object sender, EventArgs e)
        {
            zResult.Text = Page.IsValid ? "Page is valid." : "Page is not valid!!";
        }
    }
}
