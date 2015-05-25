namespace Gaia.WebWidgets.Samples.BasicControls.DropDownList.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void zDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            zLabel.Text = "You selected: " + zDropDownList.SelectedValue;
        } 
        #endregion
    }
}
