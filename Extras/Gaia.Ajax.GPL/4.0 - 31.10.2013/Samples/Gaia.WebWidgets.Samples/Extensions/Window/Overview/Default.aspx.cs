namespace Gaia.WebWidgets.Samples.Extensions.Window.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            zCaption.KeyChangeEventsInterval = WebUtility.IsLocalhost ? 50 : 400;
        }

        protected void zMinimizable_CheckedChanged(object sender, EventArgs e)
        {
            zWindow.Minimizable = zMinimizable.Checked;
        }

        protected void zClosable_CheckedChanged(object sender, EventArgs e)
        {
            zWindow.Closable = zClosable.Checked;
        }

        protected void zMaximizable_CheckedChanged(object sender, EventArgs e)
        {
            bool maximize = zMaximizable.Checked;
            if (maximize) zWindow.Maximized = false; 
            zWindow.Maximizable = maximize;
        }

        protected void zShowWindow_Click(object sender, EventArgs e)
        {
            zWindow.Visible = true;
            zShowWindow.Enabled = false;
        }

        protected void zWindowSuccess_Closing(object sender, Gaia.WebWidgets.Extensions.Window.WindowClosingEventArgs e)
        {
            zShowWindow.Enabled = true;
        }

        protected void zCaption_TextChanged(object sender, EventArgs e)
        {
            zWindow.Caption = zCaption.Text;
        }

        protected void zMaximize_Click(object sender, EventArgs e)
        {
            zWindow.Visible = true;
            zMaximizable.Checked = true;
            zWindow.Maximizable = true;
            zWindow.Maximized = true;
        }
    }
}
