namespace Gaia.WebWidgets.Samples.BasicControls.Label.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zBold_CheckedChanged(object sender, EventArgs e)
        {
            zLabel1.Font.Bold = !zLabel1.Font.Bold;
        }

        protected void zCssClass_CheckedChanged(object sender, EventArgs e)
        {
            zLabel1.CssClass = string.IsNullOrEmpty(zLabel1.CssClass) ? "css-span-label" : "";
        }
    }
}
