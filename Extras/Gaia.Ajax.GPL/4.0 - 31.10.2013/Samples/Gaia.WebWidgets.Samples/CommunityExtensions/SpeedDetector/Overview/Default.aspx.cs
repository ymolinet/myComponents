namespace Gaia.WebWidgets.Samples.CommunityExtensions.SpeedDetector.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.CommunityExtensions;

    public partial class Default : SamplePage
    {
        protected void zButtonStart_Click(object sender, EventArgs e)
        {
            zButtonStart.Enabled = false;
            
            zResult.Text = string.Format(
                "Please wait while downloading {0} MB of gibberish",
                zSpeedDetector.DownloadSize/1024/1024);
            
            zSpeedDetector.Start();
        }

        protected void zSpeedDetector_SpeedDetectionComplete(
            object sender, SpeedDetector.DetectionCompleteEventArgs e)
        {
            zResult.Text = string.Format(
                "Your bandwith is meassured to be " +
                "approximately {0} kbps", GetKpbs(e.TimeSpent));
            
            zButtonStart.Enabled = true;
        }

        private double GetKpbs(TimeSpan timeSpent)
        {
            double fileSize = zSpeedDetector.DownloadSize;
            double rate = fileSize / timeSpent.TotalSeconds;
            double kbps = (rate * 8) / 1024;
            return kbps;
        }
    }
}
