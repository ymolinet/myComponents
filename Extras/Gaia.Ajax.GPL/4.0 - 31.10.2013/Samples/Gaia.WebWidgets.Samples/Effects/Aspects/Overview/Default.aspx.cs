namespace Gaia.WebWidgets.Samples.Effects.Aspects.Overview
{
    using Gaia.WebWidgets.Samples.UI;
    using System;


    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Resizable is a custom Gaia event so we need to add 
            // AspectResizable for proper handling of the clientside
            // Resized Effect
            zPanel.Aspects.Add(new Gaia.WebWidgets.AspectResizable());
            zPanel.Effects.Add(
                AspectResizable.EffectEventResized,
                new Gaia.WebWidgets.Effects.EffectPuff());

            // DoubleClick is a native event and doesn't require that you
            // add AspectClickable to the Aspects collection

            Effect effect = new Gaia.WebWidgets.Effects.EffectParallel(
                new Gaia.WebWidgets.Effects.EffectShake(25),
                new Gaia.WebWidgets.Effects.EffectMorph("background-color: #f00;")
                );

            //effect = new Gaia.WebWidgets.Effects.EffectMorph("background-color: #f00;"); 
            zPanel2.Effects.Add(AspectClickable.EffectEventDoubleClick, effect);
        }
    }

}
