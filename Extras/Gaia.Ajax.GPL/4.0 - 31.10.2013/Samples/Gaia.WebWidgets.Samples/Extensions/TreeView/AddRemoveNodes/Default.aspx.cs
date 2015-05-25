namespace Gaia.WebWidgets.Samples.Extensions.TreeView.AddRemoveNodes
{
    using System;
    using System.Web.UI;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < ItemCount; i++)
                root.TreeViewItems.Add(CreateItem((i + 1)));
        }

        protected void zButtonAdd_Click(object sender, EventArgs e)
        {
            if (ItemCount == 10)
            {
                window.Visible = true;
                return;
            }

            root.TreeViewItems.Add(CreateItem((++ItemCount)));
        }

        private int ItemCount
        {
            get { return (int)(ViewState["items"] ?? 0); }
            set { ViewState["items"] = value; }
        }

        private Gaia.WebWidgets.Extensions.TreeViewItem CreateItem(int number)
        {
            var newItem = new Gaia.WebWidgets.Extensions.TreeViewItem
                              {
                                  CssClass = StyleSheetTheme,
                                  IsLeaf = true,
                                  IconCssClass = "noicon"
                              };

            var backgroundImage = new Label {CssClass = "tree-play-icon"};

            newItem.ChildControls.Add(backgroundImage);
            newItem.ChildControls.Add(new LiteralControl("Item " + number));
            return newItem;
        }

        protected void zButtonClear_Click(object sender, EventArgs e)
        {
            ItemCount = 0;
            root.TreeViewItems.Clear();
        }
    }
}
