namespace Gaia.WebWidgets.Samples.Aspects.AspectUpdateControl.Modal
{
    using System;
    using System.Threading;
    using Gaia.WebWidgets;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            zButton1.Aspects.Add(new AspectUpdateControl(zModalUpdateControl));
        }

        protected void zButton1_Click(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
        }
    }
}
