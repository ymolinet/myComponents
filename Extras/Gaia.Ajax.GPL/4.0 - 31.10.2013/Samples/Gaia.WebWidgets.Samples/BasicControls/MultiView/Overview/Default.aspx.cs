namespace Gaia.WebWidgets.Samples.BasicControls.MultiView.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zButton1_Click(object sender, EventArgs e)
        {
            zMultiView1.SetActiveView(zView1);
        }

        protected void zButton2_Click(object sender, EventArgs e)
        {
            zMultiView1.SetActiveView(zView2);
        }

        protected void zButton3_Click(object sender, EventArgs e)
        {
            zMultiView1.SetActiveView(zView3);
        }
    }
}
