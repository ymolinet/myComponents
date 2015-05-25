namespace Gaia.WebWidgets.Samples.Effects.EffectMorph.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;


    public partial class Default : FxSamplePage
    {
        #region Code
        protected void zMorph_Click(object sender, EventArgs e)
        {
            zMorphPanel.Effects.Add(
                new Gaia.WebWidgets.Effects.EffectMorph(Morphed ? Morph : Morph2));
            
            Morphed = !Morphed;
        }

        private const string Morph = "left: 0px; border-width: 0px;" +
                                     "border-color: #fff;" +
                                     "background-color:#ccc";

        const string Morph2 = "left: 400px; border-width: 3px;" +
                             "border-color: #a91;" +
                             "background-color: #f33;";


        private bool Morphed
        {
            get { return (bool) (ViewState["morphed"] ?? false); }
            set { ViewState["morphed"] = value;}
        }
        #endregion

    }
}
