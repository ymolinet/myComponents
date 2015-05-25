namespace Gaia.WebWidgets.Samples.Core.Manager.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            zLabel.Text = Gaia.WebWidgets.Manager.Instance.IsAjaxCallback
                              ? "Gaia Ajax callback was fired"
                              : "Not a Gaia Ajax callback (it was the initial Page_Load)";
        }
    }
}