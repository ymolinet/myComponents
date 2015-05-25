namespace Gaia.WebWidgets.Samples.Aspects.AspectUpdateControl.Overview
{
    using System;
    using System.Threading;
    using Gaia.WebWidgets;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            zButtonLengthy.Aspects.Add(new AspectUpdateControl(zUpdateControlButton));
            zCheckBoxLengthy.Aspects.Add(new AspectUpdateControl(zUpdateControlCheckBox));
            zLinkButtonLengthy.Aspects.Add(new AspectUpdateControl(zUpdateControlLinkButton));
        }
        #endregion

        protected void zButtonNotLengthy_Click(object sender, EventArgs e)
        {

        }

        protected void zButtonLengthy_Click(object sender, EventArgs e)
        {
            PerformLengthyOperation();
        }

        protected void zCheckBoxNotLengthy_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void zCheckBoxLengthy_CheckedChanged(object sender, EventArgs e)
        {
            PerformLengthyOperation();
        }

        protected void zLinkButtonNotLengthy_Click(object sender, EventArgs e)
        {

        }

        protected void zLinkButtonLengthy_Click(object sender, EventArgs e)
        {
            PerformLengthyOperation();
        }

        private void PerformLengthyOperation()
        {
            Thread.Sleep(int.Parse(zDelay.SelectedValue));
        }
    }
}
