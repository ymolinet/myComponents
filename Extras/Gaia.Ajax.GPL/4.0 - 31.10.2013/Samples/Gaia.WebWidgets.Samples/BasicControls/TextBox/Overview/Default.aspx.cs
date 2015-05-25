namespace Gaia.WebWidgets.Samples.BasicControls.TextBox.Overview
{
    using System;
    using ASP = System.Web.UI.WebControls;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void zBoldness_CheckedChanged(object sender, EventArgs e)
        {
            zTextBox1.Font.Bold = zBoldness.Checked;
        }

        protected void zEnabled_CheckedChanged(object sender, EventArgs e)
        {
            zTextBox1.Enabled = zEnabled.Checked;
        }

        protected void zVisible_CheckedChanged(object sender, EventArgs e)
        {
            zTextBox1.Visible = zVisible.Checked;
        }

        protected void zToggleCssClass_CheckedChanged(object sender, EventArgs e)
        {
            zTextBox1.CssClass = string.IsNullOrEmpty(zTextBox1.CssClass) ? "sample-textBox" : "";
        }

        protected void zSelectAll_Click(object sender, EventArgs e)
        {
            zTextBox1.SelectAll();
        }
    }
}
