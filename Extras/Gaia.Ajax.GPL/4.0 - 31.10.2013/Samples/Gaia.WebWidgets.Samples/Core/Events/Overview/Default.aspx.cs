namespace Gaia.WebWidgets.Samples.Core.Events.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zInput_Blur(object sender, EventArgs e)
        {
            zInputInfo.Text = "Lost focus";
        }

        protected void zInput_TextSelected(object sender, EventArgs e)
        {
            zInputInfo.Text = "Text selected";
        }

        protected void zInput_TextChanged(object sender, EventArgs e)
        {
            zInputInfo.Text = "You typed: " + zInput.Text;
        }

        protected void zInput_DoubleClick(object sender, AspectClickable.ClickEventArgs e)
        {
            zInputInfo.Text = "Double Clicked";
        }

        protected void zInput_Focus(object sender, EventArgs e)
        {
            zInputInfo.Text = "Got focus";
        }
    }
}
