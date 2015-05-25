namespace Gaia.WebWidgets.Samples.Core.Events.MouseOverOut
{
    using System;
    using System.Drawing;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void zPanel_MouseOut(object sender, EventArgs e)
        {
            zPanel.BackColor = Color.LightBlue;
            zPanelOutput.Text = "OnMouseOut";
        }

        protected void zPanel_MouseOver(object sender, AspectHoverable.HoverEventArgs e)
        {
            Random rnd = new Random();
            zPanel.BackColor = Color.FromArgb(
                    rnd.Next(100, 255),
                    rnd.Next(100, 255),
                    rnd.Next(100, 255));
            zPanelOutput.Text = "OnMouseOver";
        }
    }
}
