namespace Gaia.WebWidgets.Samples.Combinations.WebUtilities.AsyncLoading
{
    using System;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;


    public partial class Default : SamplePage
    {
        const string CollapsedText = "Click here for more details ...";
        const string ExpandedText = "Click here to hide again";

        protected void Page_Init(object sender, EventArgs e)
        {
            SetTimerPollingBasedOnNetworkLatency();
            zViewResponse.Text = CollapsedText;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BackgroundTask = null;
        }

        protected void zButton_Click(object sender, EventArgs e)
        {
            if (BackgroundTask.IsRunning) return;
            BackgroundTask.Data.Clear(); 
            BackgroundTask.RunTask();
            ActivateUiTaskRunning();
            DataBindGridViewToProcessedItems();
        }

        protected void zTimer_Tick(object sender, EventArgs e)
        {
            DataBindGridViewToProcessedItems();
            if (!BackgroundTask.IsRunning) DeactiveUiTaskRunning();
        }

        private void DataBindGridViewToProcessedItems()
        {
            zGrid.DataSource = BackgroundTask.Data;
            zGrid.DataBind();
        }

        private void SetTimerPollingBasedOnNetworkLatency()
        {
            zTimer.Milliseconds = WebUtility.IsLocalhost ? 500 : 1000;
        }

        private void DeactiveUiTaskRunning()
        {
            zImageLoader.Visible = false;
            zTimer.Enabled = false;
            zButton.Enabled = true;
        }

        private void ActivateUiTaskRunning()
        {
            zTimer.Enabled = true;
            zButton.Enabled = false;
            zImageLoader.Visible = true;
        }

        private CustomBackgroundWorker BackgroundTask
        {
            get
            {
                return Session["worker"] as CustomBackgroundWorker ??
                    (Session["worker"] = new CustomBackgroundWorker()) 
                    as CustomBackgroundWorker;
            }
            set { Session["worker"] = value; }
        }

        protected void zViewResponse_Click(object sender, EventArgs e)
        {
            /* some effects for show-off */

            bool show = zViewResponse.Text == CollapsedText;
            zViewResponse.Text = show ? ExpandedText : CollapsedText;

            if (show)
                zCodeResponse.Effects.Add(
                    new Gaia.WebWidgets.Effects.EffectParallel(
                        new Gaia.WebWidgets.Effects.EffectMorph("width: 650px; height: 450px;", 0.5M),
                            new Gaia.WebWidgets.Effects.EffectAppear(0.5M)));
            else
                zCodeResponse.Effects.Add(
                    new Gaia.WebWidgets.Effects.EffectParallel(
                        new Gaia.WebWidgets.Effects.EffectMorph("width: 0px; height: 0px;", 0.5M),
                            new Gaia.WebWidgets.Effects.EffectFade(0.5M)));
        }
    }
}
