namespace Gaia.WebWidgets.Samples.BasicControls.ListBox.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void zListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            zLabel.Text = "You selected '" + zListBox.SelectedValue + "' from the ListBox";
            zButtonClearSelection.Enabled = true;
        }

        protected void zButtonClearSelection_Click(object sender, EventArgs e)
        {
            zListBox.SelectedIndex = -1;
            zButtonClearSelection.Enabled = false;
            zLabel.Text = string.Empty;
        }
        #endregion
    }
}
