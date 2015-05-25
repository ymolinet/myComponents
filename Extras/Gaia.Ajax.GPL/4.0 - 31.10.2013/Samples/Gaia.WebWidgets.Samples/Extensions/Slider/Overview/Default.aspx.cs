namespace Gaia.WebWidgets.Samples.Extensions.Slider.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void slider_OnValueChanged(object sender, EventArgs e)
        {
            SyncSliders((Gaia.WebWidgets.Extensions.Slider) sender);
            SetSliderResult();
        }

        private void SyncSliders(Gaia.WebWidgets.Extensions.Slider slider)
        {
            //which slider to update, tell from the sender
            if (slider.DisplayDirection == Gaia.WebWidgets.Extensions.Slider.Direction.Horizontal)
                zSliderVertical.Value = zSliderHorizontal.Value;
            else
                zSliderHorizontal.Value = zSliderVertical.Value;
        }

        protected void slider_OnInit(object sender, EventArgs e)
        {
            SetSliderResult();
        }

        private void SetSliderResult()
        {
            zResult.Text = "Slider value: " + zSliderHorizontal.Value;
        }
    }
}
