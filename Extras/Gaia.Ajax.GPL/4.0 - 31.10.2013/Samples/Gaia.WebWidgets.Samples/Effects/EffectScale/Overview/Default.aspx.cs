namespace Gaia.WebWidgets.Samples.Effects.EffectScale.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;


    public partial class Default : FxSamplePage
    {
        #region Code
        protected void zButtonDemo_Click(object sender, EventArgs e)
        {
            const int percentGrowth = 200;
            zButtonDemo.Effects.Add(
                new Gaia.WebWidgets.Effects.EffectScale(percentGrowth));
        } 
        #endregion
    }
}
