namespace Gaia.WebWidgets.Samples.Extensions.ExtendedButton.Overview
{
    using Gaia.WebWidgets.Samples.UI;
    using System;

    public partial class Default : SamplePage
    {
      
        protected void zButton1_Click(object sender, EventArgs e)
        {
            zButton1.Toggled = !zButton1.Toggled;
            zButton2.Visible = zButton1.Toggled;

        }

        protected void zButton2_Click(object sender, EventArgs e)
        {
            zButton3.Visible = !zButton3.Visible;
            zButton2.Toggled = !zButton2.Toggled;
        }

        protected void zButton3_Click(object sender, EventArgs e)
        {
            zButton3.Enabled = false;
            zButton4.Visible = true;
        }

        protected void zButton4_Click(object sender, EventArgs e)
        {
            zButton3.Enabled = true;
            zButton4.Visible = false;
        }
    }
}
