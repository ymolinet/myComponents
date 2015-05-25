namespace Gaia.WebWidgets.Samples.Aspects.AspectClickable.ToggleCssClass
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            InitAspectClickable();
        }

        private void InitAspectClickable()
        {
            var aspectClickable = new Gaia.WebWidgets.AspectClickable();
            aspectClickable.Clicked += aspectClickable_Clicked;
            zPanel.Aspects.Add(aspectClickable);
        }

        void aspectClickable_Clicked(object sender, EventArgs e)
        {
            // Toggle CssClass for Panel on Selection
            zPanel.CssClass = (zPanel.CssClass == "panel") ?
                "panel-selected" :
                "panel";
        } 
        #endregion
    }
}
