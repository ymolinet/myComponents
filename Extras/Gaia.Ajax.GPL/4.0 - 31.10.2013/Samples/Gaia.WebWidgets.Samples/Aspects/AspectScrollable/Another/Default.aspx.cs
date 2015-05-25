namespace Gaia.WebWidgets.Samples.Aspects.AspectScrollable.Another
{
    using System;
    using System.Web.UI;
    using Gaia.WebWidgets;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            // add dummy items to make a long scrollable list
            for (int i = 0; i < 100; i++)
                zPanel.Controls.Add(new LiteralControl("Item " + i + "<br />"));

            var aspectScrollable = new AspectScrollable
                                       {
                                           Mode = AspectScrollable.ScrollModes.Vertical,
                                           OnlyRaiseAtEdge = true
                                       };

            aspectScrollable.Scrolled += aspectScrollable_Scrolled;

            zPanel.Aspects.Add(aspectScrollable);
        }

        void aspectScrollable_Scrolled(object sender, AspectScrollable.ScrollEventArgs e)
        {
            zWindow.Visible = true;
        } 
        #endregion
    }
}
