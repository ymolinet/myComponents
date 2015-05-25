namespace Gaia.WebWidgets.Samples.Effects.EffectOpacity.Overview
{
    using System;
    using System.Globalization;
    using Gaia.WebWidgets.Samples.UI;


    public partial class Default : FxSamplePage
    {
        #region Code
        protected void zOpacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal toOpacity = decimal.Parse(zOpacity.SelectedValue,
                CultureInfo.InvariantCulture);

            zPanel.Effects.Add(
                new Gaia.WebWidgets.Effects.EffectOpacity(Opacity, toOpacity));

            // Note that effects themselves does not persist style changes
            // back to the server. This is to avoid having a lot of dirty 
            // items in the ViewState. That's why we persist the previous 
            // opacity in this private field. 
            Opacity = toOpacity;
        }

        private decimal Opacity
        {
            get { return (decimal)(ViewState["opacity"] ?? 1M); }
            set { ViewState["opacity"] = value; }
        } 
        #endregion
    }
}
