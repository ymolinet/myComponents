using System;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Samples.BasicControls.Button.ButtonInRepeater
{
    using UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rep.DataSource = GetItems();
                rep.DataBind();
            }
        }

        private IEnumerable<CustomItem> GetItems()
        {
            for (int i = 0; i < int.Parse(ddl.SelectedValue); i++)
                yield return new CustomItem("Index: " + i);
        }

        protected void ddl_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            rep.DataSource = GetItems();
            rep.DataBind();
        }

        protected void rep_OnItemCommand(object source, ASP.RepeaterCommandEventArgs e)
        {
            lblMsg.Text = @"You clicked item with " + e.CommandArgument;
        }
    }

    public class CustomItem
    {
        public CustomItem(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}