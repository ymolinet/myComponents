namespace Gaia.WebWidgets.Samples.Extensions.InPlaceEdit.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zInPlaceEdit4_OnTextChanged(object sender, EventArgs e)
        {
            zResultTextChanged.Text = "Updated value of the InPlaceEdit control: " + zInPlaceEdit4.Text;
        }
    }
}
