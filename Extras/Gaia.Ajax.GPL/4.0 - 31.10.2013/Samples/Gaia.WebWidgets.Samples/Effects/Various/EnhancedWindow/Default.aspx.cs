namespace Gaia.WebWidgets.Samples.Effects.Various.EnhancedWindow
{
    using Gaia.WebWidgets.Samples.UI;
    using System;


    public partial class Default : FxSamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            /* Special appear effect */
            window.Effects.Add(
                Gaia.WebWidgets.Extensions.Window.EffectEventAppear,
                new Gaia.WebWidgets.Effects.EffectGrow());

            // Shrink for closing the window ... 
            window.Effects.Add(
                Gaia.WebWidgets.Extensions.Window.EffectEventClose,
                new Gaia.WebWidgets.Effects.EffectShrink());

            /* Special minimize effect */
            window.Effects.Add(
                Gaia.WebWidgets.Extensions.Window.EffectEventMinimize,
                new Gaia.WebWidgets.Effects.EffectQueue(
                    Gaia.WebWidgets.Extensions.Window.ApplyToContent(new Gaia.WebWidgets.Effects.EffectSlideUp(0.4M)),
                    new Gaia.WebWidgets.Effects.EffectMorph("width: 175px;")));

            /* EffectMorph has the advantage of being able to morph the 
               widget back into it's old state because of it's support for
               parameters */
            window.Effects.Add(
                Gaia.WebWidgets.Extensions.Window.EffectEventRestoreAfterMinimize,
                new Gaia.WebWidgets.Effects.EffectParallel(
                   Gaia.WebWidgets.Extensions.Window.ApplyToContent(new Gaia.WebWidgets.Effects.EffectSlideDown(0.4M)),
                    new Gaia.WebWidgets.Effects.EffectMorph()));
        } 
        #endregion

        protected void buttonOpen_Click(object sender, EventArgs e)
        {
            window.Visible = true;
        }
    }
}
