namespace Gaia.WebWidgets.Samples.BasicControls.CheckBoxList.Overview
{
    using System;
    using System.Linq;
    using ASP = System.Web.UI.WebControls;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void zCheckBoxList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            zLabel.Text = "Items selected: " + GetStatusText();
        }

        private string GetStatusText()
        {
            var selectedValues = zCheckBoxList.Items.OfType<ASP.ListItem>().Where(chb => chb.Selected).Select(chb => chb.Text).ToArray();
            return string.Join(", ", selectedValues);
        }

        #endregion
    }
}
