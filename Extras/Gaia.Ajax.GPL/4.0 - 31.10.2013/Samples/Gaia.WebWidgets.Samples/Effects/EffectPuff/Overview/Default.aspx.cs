namespace Gaia.WebWidgets.Samples.Effects.EffectPuff.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;


    public partial class Default : FxSamplePage
    {
        #region Code
        protected void zButtonDemo_Click(object sender, EventArgs e)
        {
            zPanel.Effects.Add(new Gaia.WebWidgets.Effects.EffectPuff());
        } 
        #endregion

        protected void zButtonReset_Click(object sender, EventArgs e)
        {
            zPanel.Effects.Add(new Gaia.WebWidgets.Effects.EffectShow());
        }
    }
}
