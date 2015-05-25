namespace Gaia.WebWidgets.Samples.Extensions.TreeView.DragAndDrop
{
    using System;
    using System.Linq;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        private readonly CalendarController _calendarController = new CalendarController(10);

        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeTreeViewOnFirstLoad();

            zTreeRight.Aspects.Add(new Gaia.WebWidgets.AspectDroppable(Dropped, "gaiax-hover-drop"));
        }

        private void InitializeTreeViewOnFirstLoad()
        {
            if (!IsPostBack)
            {
                //prepare data for left tree
                _calendarController.CalendarItems.ForEach(delegate(CalendarItem item) { item.IsChecked = false; });
            }
        }

        protected void Dropped(object sender, AspectDroppable.DroppedEventArgs e)
        {
            var id = int.Parse(e.IdToPass);
            CalendarItem calendarItem = null;
            foreach (var idx in _calendarController.CalendarItems)
            {
                if (idx.Id != id) continue;
                idx.IsChecked = true;
                calendarItem = idx;
                break;
            }

            if (calendarItem == null) return;

            // Forcing re-rendering of TreeViews
            zLeftRoot.TreeViewItems.Clear();
            zRightRoot.TreeViewItems.Clear();

            //// Re-fetching items (we're just "faking" the Event GetTreeViewItems here...
            getTreeItemsLeft(null, EventArgs.Empty);
            getTreeItemsRight(null, EventArgs.Empty);
        }

        protected void getTreeItemsLeft(object sender, EventArgs e)
        {
            foreach (var item in _calendarController.CalendarItems.Where(item => !item.IsChecked))
            {
                zLeftRoot.Controls.Add(CreateTreeViewItem(item, !item.IsChecked));
            }
        }

        protected void getTreeItemsRight(object sender, EventArgs e)
        {
            foreach (var idx in _calendarController.CalendarItems.Where(idx => idx.IsChecked))
            {
                zRightRoot.Controls.Add(CreateTreeViewItem(idx, !idx.IsChecked));
            }
        }

        private Gaia.WebWidgets.Extensions.TreeViewItem CreateTreeViewItem(CalendarItem idx, bool makeDraggable)
        {
            var treeViewItem = new WebWidgets.Extensions.TreeViewItem();
            treeViewItem.ID = idx.Id.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
            treeViewItem.CssClass = StyleSheetTheme;
            treeViewItem.IsLeaf = true;
            treeViewItem.IconCssClass = "noicon";

            var label = new Label
                            {
                                Text = idx.ActivityName, 
                                CssClass = "gaiax-dragcontainer"
                            };

            #region Make Draggable 
            if (makeDraggable)
            {
                var draggable = new AspectDraggable
                                    {
                                        IdToPass = idx.Id + "",
                                        Revert = true,
                                        UseDocumentBody = true,
                                        DeepCopy = true,
                                        MakeGhost = true,
                                        DragCssClass = "gaiax-drag"
                                    };

                label.Aspects.Add(draggable);
            } 
            #endregion

            treeViewItem.Controls.Add(label);

            return treeViewItem;
        }
    }
}
