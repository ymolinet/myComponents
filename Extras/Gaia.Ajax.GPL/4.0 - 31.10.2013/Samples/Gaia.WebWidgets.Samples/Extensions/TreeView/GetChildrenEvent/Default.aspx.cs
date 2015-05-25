namespace Gaia.WebWidgets.Samples.Extensions.TreeView.GetChildrenEvent
{
    using System.Web.UI;
    using Gaia.WebWidgets.Extensions;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        private const int MaximumNodes = 2000;

        /// <summary>
        /// Defines how many nodes will be added on each Node Expansion.
        /// </summary>
        private int SelectedCount { get { return int.Parse(zTreeItemcount.SelectedValue); } }
        
        protected void TreeViewGetChildrenControls(object sender, TreeViewItem.GetChildrenControlsEventArgs e)
        {
            zTreeItemcount.Enabled = false;
            int totaltCount = 0;
            for (int i = 1; i < SelectedCount + 1; i++)
            {
                if (++totaltCount >= MaximumNodes)
                {
                    zWindow.Visible = true;
                    break;
                }

                e.Node.TreeViewItems.Add(CreateItem(i));
            }
        }

        private TreeViewItem CreateItem(int number)
        {
            var newItem = new TreeViewItem();
            newItem.CssClass = StyleSheetTheme;
            newItem.GetChildrenControls += TreeViewGetChildrenControls;
            newItem.ChildControls.Add(new LiteralControl("Item " + number));
            return newItem;
        }
    }
}
