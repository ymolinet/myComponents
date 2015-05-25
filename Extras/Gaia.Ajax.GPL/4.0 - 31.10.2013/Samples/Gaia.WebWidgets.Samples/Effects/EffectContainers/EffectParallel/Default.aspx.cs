namespace Gaia.WebWidgets.Samples.Effects.EffectContainers.EffectParallel
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : FxSamplePage
    {
        #region Code
        void FireEffect()
        {
            // In this parallel effect we fire three different simultaneously
            // To affect the body of the Window we need to add the Effect to 
            // the Window.ApplyToContent function. 
            zWindow.Effects.Add(
                    new Gaia.WebWidgets.Effects.EffectParallel(
                        new Gaia.WebWidgets.Effects.EffectShake(),
                        Gaia.WebWidgets.Extensions.Window.ApplyToContent(new Gaia.WebWidgets.Effects.EffectHighlight()),
                        new Gaia.WebWidgets.Effects.EffectMorph("width: 640px;")));
        }

        protected void zFireEffect_Click(object sender, EventArgs e)
        {
            FireEffect();
        }

        protected void zButtonFire_Click(object sender, EventArgs e)
        {
            FireEffect();
        } 
        #endregion
    }
}
