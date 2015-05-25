namespace Gaia.WebWidgets.Samples.BasicControls.GridView.NestedGridView
{
    using System;
    using Gaia.WebWidgets.Effects;

    public partial class OrderGridView : System.Web.UI.UserControl
    {
        public object GridDataSource
        {
            get { return zGvC.DataSource; }
            set { zGvC.DataSource = value; }
        }

        protected void zToggleViewOrders_Click(object sender, EventArgs e)
        {
            zGvC.Visible = !zGvC.Visible;

            zToggleViewOrders.Text = zGvC.Visible ? "Hide Orders" : "Show Orders";

            if (zGvC.Visible)
            {
                zGvC.Style["display"] = "none";
                zGvC.Effects.Add(new EffectSlideDown());
            }
        }
    }
}