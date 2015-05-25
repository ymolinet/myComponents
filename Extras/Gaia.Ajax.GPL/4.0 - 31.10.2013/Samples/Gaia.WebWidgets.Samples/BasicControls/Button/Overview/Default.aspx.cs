using System;
using Gaia.WebWidgets.Samples.UI;

namespace Gaia.WebWidgets.Samples.BasicControls.Button.Overview
{
    public partial class Default : SamplePage
    {
        #region Code
        protected void btn_Click(object sender, EventArgs e)
        {
            lbl.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
        } 
        #endregion
    }
}