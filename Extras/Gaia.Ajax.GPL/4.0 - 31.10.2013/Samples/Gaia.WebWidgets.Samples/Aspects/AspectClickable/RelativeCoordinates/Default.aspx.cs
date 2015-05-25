namespace Gaia.WebWidgets.Samples.Aspects.AspectClickable.RelativeCoordinates
{
    using System;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;
    using ASP = System.Web.UI.WebControls;

    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Load(object sender, EventArgs e)
        {
            InitAspectClickableUsingTopLeft();

            var innerPanel = new Panel();
            innerPanel.Style["position"] = "absolute";
            innerPanel.Style["border"] = "1px solid black";
            innerPanel.Style["width"] = "25px";
            innerPanel.Style["height"] = "25px";
            innerPanel.Visible = false;
            zPanel.Controls.Add(innerPanel);
        }

        private void InitAspectClickableUsingTopLeft()
        {
            var aspectClickable = new Gaia.WebWidgets.AspectClickable {UseRelativeCoordinates = true};

            aspectClickable.Clicked += clickable_Clicked;
            zPanel.Aspects.Add(aspectClickable);
        }

        void clickable_Clicked(object sender, Gaia.WebWidgets.AspectClickable.ClickEventArgs e)
        {
            // note: left+top is not persisted in viewstate and therefore gets reset after each callback
            var innerPanel = WebUtility.First<Panel>(zPanel.Controls);
            innerPanel.Style["left"] = e.Left + "px";
            innerPanel.Style["top"] = e.Top + "px";
            innerPanel.Visible = true;

        }  
        #endregion
    }
}
