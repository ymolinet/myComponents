namespace Gaia.WebWidgets.Samples.Effects.Various.ShowMethod
{
    using System;

    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : FxSamplePage
    { 
        protected void zFireEffectButton_Click(object sender, EventArgs e)
        {
            Effect effect = ElementVisible ? (Effect)
                new Gaia.WebWidgets.Effects.EffectDropOut() :
                new Gaia.WebWidgets.Effects.EffectAppear();

            Effect.Show("div", effect);
            zFireEffectButton.Text = !ElementVisible ? "Drop Out!" : "Appear";
            ElementVisible = !ElementVisible;
        }

        private bool ElementVisible
        {
            get { return (bool)(ViewState["ElementVisible"] ?? true); }
            set { ViewState["ElementVisible"] = value; }
        }

    }
}