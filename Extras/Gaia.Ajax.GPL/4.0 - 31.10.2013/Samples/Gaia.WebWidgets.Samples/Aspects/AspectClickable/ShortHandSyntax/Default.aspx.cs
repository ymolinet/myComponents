namespace Gaia.WebWidgets.Samples.Aspects.AspectClickable.ShortHandSyntax
{
    using System;
    using Gaia.WebWidgets.Samples.UI;


    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            // Shorthand for quickly adding Aspects with Event Handler
            zPanel.Aspects.Add(
                new Gaia.WebWidgets.AspectClickable(
                    delegate
                    {
                        zPanel.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight());
                    }));
        } 
        #endregion
    }
}
