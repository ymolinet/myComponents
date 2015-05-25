namespace Gaia.WebWidgets.Samples.Effects.EffectScrollTo.Overview
{
    using System;

    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : FxSamplePage
    {
        #region Code
        
        protected void zButtonDemo_Click(object sender, EventArgs e)
        {
            zButtonDest.Effects.Add(
                new Gaia.WebWidgets.Effects.EffectScrollTo());
        } 
       
        protected void zButtonDest_Click(object sender, EventArgs e)
        {
            zButtonDemo.Effects.Add(
                new Gaia.WebWidgets.Effects.EffectScrollTo());
        }
        
        #endregion
    }
}
