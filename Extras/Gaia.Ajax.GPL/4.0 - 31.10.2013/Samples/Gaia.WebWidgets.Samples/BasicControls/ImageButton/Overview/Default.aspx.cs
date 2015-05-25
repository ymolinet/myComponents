using System;
namespace Gaia.WebWidgets.Samples.BasicControls.ImageButton.Overview
{
    using System.Web.UI;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zPlay_Click(object sender, ImageClickEventArgs e)
        {
            zState.Text = "Play pressed";
            SwitchPlayPause();
        }

        private void SwitchPlayPause()
        {
            zPlay.Enabled = zPause.Enabled;

            zPlay.ImageUrl = zPlay.Enabled
                                 ? "~/media/icons/play_gray.png"
                                 : "~/media/icons/play_gray_disabled.png";

            zPause.Enabled = !zPlay.Enabled;

            zPause.ImageUrl = zPause.Enabled
                                 ? "~/media/icons/pause_gray.png"
                                 : "~/media/icons/pause_gray_disabled.png";
        }

        protected void zPause_Click(object sender, ImageClickEventArgs e)
        {
            zState.Text = "Pause pressed";
            SwitchPlayPause();
        }

        protected void zNext_Click(object sender, ImageClickEventArgs e)
        {
            zState.Text = "Next pressed";
        }

        protected void zPrev_Click(object sender, ImageClickEventArgs e)
        {
            zState.Text = "Previous pressed";
        }
    }
}
