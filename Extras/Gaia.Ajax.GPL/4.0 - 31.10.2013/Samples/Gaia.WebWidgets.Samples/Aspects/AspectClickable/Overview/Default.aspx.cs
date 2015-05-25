namespace Gaia.WebWidgets.Samples.Aspects.AspectClickable.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            zPanelClickable.Aspects.Add(new Gaia.WebWidgets.AspectClickable(PanelClicked));
        }

        private void PanelClicked(object sender, Gaia.WebWidgets.AspectClickable.ClickEventArgs e)
        {
            //update a label with the position of the mouse click, and run a highlight on the label
            zCoordinates.Text = string.Format("Document position of your click: {0}px and top: {1}px", e.Left, e.Top);
            zCoordinates.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight());
        } 
        #endregion
    }
}
