namespace Gaia.WebWidgets.Samples.BasicControls.DynamicImage.Captcha
{
    using System;
    using System.Drawing;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zSubmit_Click(object sender, EventArgs e)
        {
            if (zCaptcha.IsValid)
            {
                zResult.Text = "You proved to be a human, and your form was submitted successfully " + DateTime.Now;
                zResult.ForeColor = Color.Green;
            }
            else
            {
                zResult.Text = "Your captcha was not correct; we don't know if you are a human or a computer robot.";
                zResult.ForeColor = Color.Red;
            }
        }
    }
}
