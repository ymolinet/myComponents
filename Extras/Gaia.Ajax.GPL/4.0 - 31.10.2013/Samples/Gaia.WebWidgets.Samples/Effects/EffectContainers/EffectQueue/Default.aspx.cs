namespace Gaia.WebWidgets.Samples.Effects.EffectContainers.EffectQueue
{
    using System;
    using System.Drawing;
    using Gaia.WebWidgets.Samples.UI;


    public partial class Default : FxSamplePage
    {
        #region Code
        void FireEffect()
        {
            const int distance = 75;
            const decimal duration = 0.5M;
            const Gaia.WebWidgets.Effects.EffectMove.ModeEnum mode = Gaia.WebWidgets.Effects.EffectMove.ModeEnum.Relative;

            zWindow.Effects.Add(
                new Gaia.WebWidgets.Effects.EffectQueue(
                    new Gaia.WebWidgets.Effects.EffectMove(-distance, 0, duration, 0, mode),
                    new Gaia.WebWidgets.Effects.EffectMove(0, distance, duration, 0, mode),
                    new Gaia.WebWidgets.Effects.EffectMove(distance, 0, duration, 0, mode),
                    new Gaia.WebWidgets.Effects.EffectMove(0, -distance, duration, 0, mode),
                    Gaia.WebWidgets.Extensions.Window.ApplyToContent(new Gaia.WebWidgets.Effects.EffectHighlight(Color.LightGreen))));
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
