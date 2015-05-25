namespace Gaia.WebWidgets.Samples.BasicControls.GridView.NestedGridView
{
    using System;
    using System.Globalization;

    public partial class OrderLineGridView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public object GridDataSource
        {
            get { return zGvCC.DataSource; }
            set { zGvCC.DataSource = value; }
        }

        /// <summary>
        /// Calculate linetotal and total price and put on bottom
        /// </summary>
        decimal _priceTotal;
        protected void zGvCC_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                // add the UnitPrice and QuantityTotal to the running total variables
                decimal lineTotal =
                    decimal.Parse(((Gaia.WebWidgets.Label)e.Row.Cells[2].FindControl("price")).Text, CultureInfo.InvariantCulture) *
                    decimal.Parse(((Gaia.WebWidgets.Label)e.Row.Cells[1].FindControl("quantity")).Text, CultureInfo.InvariantCulture);

                _priceTotal += lineTotal;

                ((Gaia.WebWidgets.Label)e.Row.Cells[3].FindControl("linetotal")).Text = lineTotal.ToString("0.00",
                                                                                           CultureInfo.InvariantCulture);
            }
            else if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.Footer)
            {
                ((Gaia.WebWidgets.Label)e.Row.Cells[3].FindControl("footer")).Text = _priceTotal.ToString("0.00", CultureInfo.InvariantCulture);
            }
        }
    }
}