namespace Gaia.WebWidgets.Samples.Aspects.AspectModal.Overview
{
    using System;
    using System.Drawing;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            Color color = Color.Beige;
            const double opacity = 0.7D;
            var aspectModal = new Gaia.WebWidgets.AspectModal(color, opacity);
            zWindow2.Aspects.Add(aspectModal);
        }

        protected void zOpenWindow1_Click(object sender, EventArgs e)
        {
            zWindow1.Visible = true;
        }

        protected void zOpenWindow2_Click(object sender, EventArgs e)
        {
            zWindow2.Visible = true;
        }
        #endregion
    }
}
