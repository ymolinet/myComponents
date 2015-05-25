namespace Gaia.WebWidgets.Samples.Core.DynamicControls.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AddedTimer)
                CreateTimer();
        }

        /// <summary>
        /// This function creates the timer and adds it to the Container Panel
        /// </summary>
        private void CreateTimer()
        {
            var timer = new Timer();
            timer.Enabled = true;
            timer.Milliseconds = 1000;
            timer.Tick += delegate { zMessage.Text = DateTime.Now.ToString(); };

            zContainer.Controls.Add(timer);
            AddedTimer = true;
        }

        protected void createTimer_Click(object sender, EventArgs e)
        {
            //if already added, then skip
            if (AddedTimer)
                return;

            CreateTimer();
        }

        /// <summary>
        /// Flag to indicate that the Timer should be added to the page on 
        /// subsequent callbacks
        /// </summary>
        private bool AddedTimer
        {
            get { return ViewState["TimerAdded"] != null && (bool)ViewState["TimerAdded"]; }
            set { ViewState["TimerAdded"] = value; }
        }
    }
}