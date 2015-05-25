namespace Gaia.WebWidgets.Samples.Effects.Aspects.Hoverable
{
    using System;
    using Gaia.WebWidgets.Samples.UI;


    public partial class Default :SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            // Gotcha: Must add AspectHoverable since it contains 
            //special handling for MouseOver/MouseOut correctly
            zImage.Aspects.Add(new Gaia.WebWidgets.AspectHoverable());

            const decimal opacity = 0.6M;
            const decimal duration = 0.4M;

            zImage.Effects.Add(
                AspectHoverable.EffectEventMouseOver,
                new Gaia.WebWidgets.Effects.EffectOpacity(1,opacity, duration)
                );

            zImage.Effects.Add(
                AspectHoverable.EffectEventMouseOut,
                new Gaia.WebWidgets.Effects.EffectOpacity(opacity, 1, duration));

        }
        #endregion
    }

   
}
