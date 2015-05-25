namespace Gaia.WebWidgets.Samples.Aspects.AspectClickable.DoubleClick
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            var aspectClickable =
                   new Gaia.WebWidgets.AspectClickable();

            aspectClickable.DblClicked +=
                delegate
                {
                    zPanel.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight());
                };

            zPanel.Aspects.Add(aspectClickable);

        } 
        #endregion
    }
}