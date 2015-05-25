
namespace Gaia.WebWidgets.Samples.BasicControls.HiddenField.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void zButton_Click(object sender, EventArgs e)
        {
            zLabel.Text = "Value of HiddenField: " + zHiddenField.Value;
        } 
        #endregion
    }
}
