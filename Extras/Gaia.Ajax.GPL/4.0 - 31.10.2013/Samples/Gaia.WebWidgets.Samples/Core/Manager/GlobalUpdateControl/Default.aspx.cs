namespace Gaia.WebWidgets.Samples.Core.Manager.GlobalUpdateControl
{
    using System;
    using System.Threading;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Gaia.WebWidgets.Manager.Instance.UpdateControl = updateControl;
        }

        protected void zButton_Click(object sender, EventArgs e)
        {
            LengthyProcess();
        }

        protected void zLinkButton_Click(object sender, EventArgs e)
        {
            LengthyProcess();
        }

        protected void zDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LengthyProcess();
        }

        void LengthyProcess()
        {
            Thread.Sleep(1000);
        }
    }
}
