namespace Gaia.WebWidgets.Samples.BasicControls.LinkButton.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zLinkButton1_Click(object sender, EventArgs e)
        {
            zLinkButton1.Text = "You clicked the LinkButton at " + DateTime.Now;
        }

        protected void zBold_CheckedChanged(object sender, EventArgs e)
        {
            zLinkButton1.Font.Bold = !zLinkButton1.Font.Bold;
        }

        protected void zCssClass_CheckedChanged(object sender, EventArgs e)
        {
            zLinkButton1.CssClass = string.IsNullOrEmpty(zLinkButton1.CssClass) ? "css-linkbutton" : "";
        }

        protected void zToggleEnabling_CheckedChanged(object sender, EventArgs e)
        {
            zLinkButton1.Enabled = !zLinkButton1.Enabled;
        }
    }
}
