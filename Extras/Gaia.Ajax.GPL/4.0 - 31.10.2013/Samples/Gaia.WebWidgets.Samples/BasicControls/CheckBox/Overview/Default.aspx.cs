namespace Gaia.WebWidgets.Samples.BasicControls.CheckBox.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {

        #region Code
        protected void cbxSame_CheckedChanged(object sender, EventArgs e)
        {
            txtWork.Text = cbxSame.Checked ? txtHome.Text : "";
        } 
        #endregion

    }
}