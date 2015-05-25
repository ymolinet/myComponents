namespace Gaia.WebWidgets.Samples.Effects.EffectAppear.Overview
{
    using Gaia.WebWidgets.Samples.UI;
    using System;


    public partial class Default : FxSamplePage
    {
        #region Code
        protected void zButtonDemo_Click(object sender, EventArgs e)
        {
            zPanel.Effects.Add(new Gaia.WebWidgets.Effects.EffectAppear());
        } 
        #endregion

        protected void zButtonReset_Click(object sender, EventArgs e)
        {
            zPanel.Effects.Add(new Gaia.WebWidgets.Effects.EffectHide());
        }
    }
}
