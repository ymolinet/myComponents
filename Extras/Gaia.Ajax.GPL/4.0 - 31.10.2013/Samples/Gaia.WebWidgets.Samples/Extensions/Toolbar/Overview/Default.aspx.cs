namespace Gaia.WebWidgets.Samples.Extensions.Toolbar.Overview
{
    using System;
    using Gaia.WebWidgets.Effects;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void ItemCommand(object sender, EventArgs e)
        {
            var linkButton = sender as LinkButton;
            if (linkButton == null) return;
            zMessage.Text = "You selected " + linkButton.Text;
            zMessage.Effects.Add(new EffectHighlight());
        }
    }
}
