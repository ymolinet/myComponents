namespace Gaia.WebWidgets.Samples.CommunityExtensions.FishEye.Overview
{
    using System;
    using System.Collections.Generic;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        private static List<MediaImage> Images
        {
            get { return new List<MediaImage>(MediaUtility.GetImageFiles("scenery")); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            zRepeater1.DataSource = Images;
            zRepeater1.DataBind();
        }

        protected void dynamicImage_Click(object sender, AspectClickable.ClickEventArgs e)
        {
            Image clickedImage = sender as Image;
            if (clickedImage == null)
                return;

            //get item from list of images
            MediaImage image = Images.Find(delegate(MediaImage m) { return m.Text == clickedImage.AlternateText; });

            zWinImage.Visible = true;
            zWinImage.Caption = image.Text;
            zImage.ImageUrl = image.ImageUrl;
            zImage.AlternateText = image.Text;
            zFishEyeMenu.Enabled = false;
        }

        protected void zWinImage_Closing(object sender, Gaia.WebWidgets.Extensions.Window.WindowClosingEventArgs e)
        {
            zFishEyeMenu.Enabled = true;
        }
    }
}