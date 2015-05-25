 namespace Gaia.WebWidgets.Samples.Extensions.Accordion.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
      
        protected void Accordion1_Toggled(object sender, EventArgs e)
        {
            zMessage.Text = "Accordion toggled at " + DateTime.Now +"<br>";
        }
    }
}