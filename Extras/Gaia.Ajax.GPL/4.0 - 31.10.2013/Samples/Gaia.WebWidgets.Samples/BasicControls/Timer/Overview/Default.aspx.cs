namespace Gaia.WebWidgets.Samples.BasicControls.Timer.Overview
{
    using Gaia.WebWidgets.Samples.UI;
    using System;
    using System.Drawing;

    public partial class Default : SamplePage
    {
        #region Code
        /// <summary>
        /// The Tick Event is fired if the Timer is enabled at the specified interval
        /// </summary>
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //get CPU usage and set text of label
            int cpuUsage = CpuUsagePercent();
            Label1.Text = string.Format("{0} %", cpuUsage);

            //Set the style of the label depending on CPU usage
            Label1.ForeColor = cpuUsage > 75 ? Color.Red : Color.Green;

            //set value to slider
            //subtract from 100 to make it show the lowest value at the bottom
            slider.Value = 100 - cpuUsage;
        } 
        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            //toggle timer on or off
            Timer1.Enabled = ! Timer1.Enabled;
            
            //set text of button depending on timer status
            Button1.Text = Timer1.Enabled ? "Pause" : "Resume";
        }

        /// <summary>
        /// Returns a random CPU Usage in percent for this sample
        /// </summary>
        private static int CpuUsagePercent()
        {
            Random rand = new Random(DateTime.Now.Second);
            return rand.Next(0, 100);
        }
    }
}