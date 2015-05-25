namespace Gaia.WebWidgets.Samples.Extensions.TreeView.MediaLibrary
{
    using UI;
    using Utilities;
    using System;
    using System.Drawing;
    using System.Web.UI;
    
    public partial class Default : SamplePage
    {
        private const string DropClass = "gaiax-treeview-drag-append";

        protected void Page_Load(object sender, EventArgs e)
        {
            // The TreeNodes beneath Images are made into drop locations by adding aspects
            foreach (var item in new[] { beautiful, people, food })
                item.Aspects.Add(new AspectDroppable(ItemDropped, DropClass));
        }

        /// <summary>
        /// This event will dynamically populate the children of the clicked TreeViewNode. This allows
        /// for great flexibility when building dynamic treeviews populated from a DataSource 
        /// </summary>
        protected void RetrieveTreeViewItems(object sender, WebWidgets.Extensions.TreeViewItem.GetChildrenControlsEventArgs e)
        {
            var id = e.Node.ID;
            var draggable = id == "beautiful" || id == "people" || id == "food";

            foreach (var media in new MediaController().GetMediaByCategory(e.Node.ID))
                e.Node.Controls.Add(CreateSimpleTreeViewItem(media.Text, media.Id, draggable));
        }

        /// <summary>
        /// Here's an example of using a helper function to create a TreeViewNode. Below you can see some
        /// more advanced samples of building composite TreeViewNodes with different functionality
        /// </summary>
        private WebWidgets.Extensions.TreeViewItem CreateAdvancedNode(string id)
        {
            return new WebWidgets.Extensions.TreeViewItem {ID = id, CssClass = StyleSheetTheme};
        }

        /// <summary>
        /// Another helper function to create a TreeViewNode
        /// </summary>
        private WebWidgets.Extensions.TreeViewItem CreateAdvancedNodeLeaf(WebWidgets.Extensions.TreeViewItem node, string id)
        {
            var ti = new WebWidgets.Extensions.TreeViewItem
                         {
                             ID = id,
                             IconCssClass = "folder",
                             CssClass = StyleSheetTheme
                         };
            node.Controls.Add(ti);
            return ti;
        }

        /// <summary>
        /// Here we create a TreeViewNode by using different Gaia Controls that are "composed" together. 
        /// Notice how we use labels, linkbuttons and aspects to create a powerful TreeViewNode for usage
        /// in this sample. It's also nice to be able to delegate the responsibility of creating these
        /// different TreeViewNodes to their own functions. 
        /// </summary>
        private WebWidgets.Extensions.TreeViewItem CreateSimpleTreeViewItem(string value, string id, bool hasDragger)
        {
            var treeViewItem = new WebWidgets.Extensions.TreeViewItem
                                   {
                                       ID = id,
                                       IsLeaf = true,
                                       IconCssClass = "noicon",
                                       CssClass = StyleSheetTheme
                                   };

            // the label renders a simple span that will hold our icon as a background image
            var backgroundImage = new Label {ID = id + "lbl", CssClass = "tv-item-background-image"};

            treeViewItem.Controls.Add(backgroundImage);

            // the linkbutton will receive click events and display relevant information
            var linkButton = new LinkButton
                                 {
                                     Text = value,
                                     ID = id + "linkBtn",
                                     CssClass = "tv-item-unclicked",
                                     ToolTip = "Click me (the green text) to see an image or a video"
                                 };

            linkButton.Click += delegate
                                    {
                                        linkButton.CssClass = "tv-item-clicked";
                                        resultsPanel.Caption = treeViewItem.ID;
                                        viewer.View(treeViewItem.ID, treeViewItem.Parent.ID);
                                    };

            treeViewItem.Controls.Add(linkButton);

            if (hasDragger)
            {
                // here we create an AspectDraggable and set the id of the item as IdToPass. 
                var aspectDraggable = new AspectDraggable(null, Rectangle.Empty, true, false, 0.5M) {IdToPass = id};
                backgroundImage.Aspects.Add(aspectDraggable);
                backgroundImage.ToolTip += " or DRAG me to another folder";
                backgroundImage.Style["cursor"] = "move";
            }

            return treeViewItem;
        }

        /// <summary>
        /// The Drag and Drop functionality was hand-made using the powerful Aspects AspectDroppable
        /// and AspectDraggable. It requires some code to make the plumbing work, but this can easily
        /// be abstracted away in helper classes. See function: CreateSimpleTreeViewItem for simplification 
        /// of creating draggable nodes. Because we are using Aspects you can implement dragdrop between
        /// treeviews and other widgets. 
        /// </summary>
        protected void ItemDropped(object sender, AspectDroppable.DroppedEventArgs e)
        {
            if (sender == null) return;

            var dragId = e.IdToPass;

            var dropLocation = (WebWidgets.Extensions.TreeViewItem) ((IAspect) sender).ParentControl;
            var dragged = (WebWidgets.Extensions.TreeViewItem) WebUtility.FindControl(images, dragId);
            var draggedPreviousParent = (WebWidgets.Extensions.TreeViewItem) dragged.Parent;

            if (draggedPreviousParent == dropLocation) return;

            // swap the dragged TreeViewItem and re-render each container with the updated content
            draggedPreviousParent.Controls.Remove(dragged);

            if (!dropLocation.Collapsed)
                dropLocation.Controls.Add(dragged);

            // here we update the underlying data. we use the id of the container control as category
            var item = new MediaController().GetMediaById(dragId);
            item.Category = dropLocation.ID;

            //notify
            resultsPanel.Caption = string.Format("Element {0} was successfully moved to {1}", dragId, dropLocation.ID);
        }

        /// <summary>
        /// Here's a quick example of dynamically creating different controls. We add a editable label, 
        /// linkbutton and checkbox to our sample. We also add another dynamic control which in turns
        /// retrieves a huge dynamic list of elements
        /// </summary>
        protected void RetrieveAdvancedItems(object sender, WebWidgets.Extensions.TreeViewItem.GetChildrenControlsEventArgs e)
        {
            // InPlaceEdit
            var inplaceEdit = new WebWidgets.Extensions.InPlaceEdit
                                  {
                                      Text = "Click me",
                                      ID = "inpledit",
                                      Width = System.Web.UI.WebControls.Unit.Pixel(80),
                                      Height = System.Web.UI.WebControls.Unit.Pixel(16)
                                  };
            inplaceEdit.TextChanged += InplaceEditTextChanged;

            var inplaceEditTreeViewNode = CreateAdvancedNode("inplaceTree");
            inplaceEditTreeViewNode.IconCssClass = "file";
            inplaceEditTreeViewNode.ID = "inPlTreeViewItem";
            inplaceEditTreeViewNode.IsLeaf = true;
            inplaceEditTreeViewNode.Controls.Add(inplaceEdit);
            advanced.Controls.Add(inplaceEditTreeViewNode);

            // CheckBox
            var checkbox = new CheckBox
                               {
                                   ID = "chkBox",
                                   Text = "Check me!",
                                   AutoPostBack = true,
                                   Height = System.Web.UI.WebControls.Unit.Pixel(16)
                               };
            checkbox.CheckedChanged += CheckboxCheckedChanged;

            var checkboxTreeViewItem = CreateAdvancedNode("checkbox");
            checkboxTreeViewItem.IsLeaf = true;
            checkboxTreeViewItem.IconCssClass = "file";
            checkboxTreeViewItem.ID = "chkTreeItem";
            checkboxTreeViewItem.Controls.Add(checkbox);
            advanced.Controls.Add(checkboxTreeViewItem);

            // HyperLink
            var hyperlink = new LiteralControl
                                {
                                    ID = "googleLink",
                                    Text =
                                        "<a href=\"http://www.google.com/search?hl=en&q=gaia+ajax&btnG=Google+Search\" style=\"color:Blue;\" title=\"Google for Gaia\">Google for Gaia</a>"
                                };

            var hyperlinkTreeViewItem = CreateAdvancedNode("hyperlink");
            hyperlinkTreeViewItem.IsLeaf = true;
            hyperlinkTreeViewItem.ID = "hplLnkTreeItem";
            hyperlinkTreeViewItem.IconCssClass = "file";
            hyperlinkTreeViewItem.Controls.Add(hyperlink);
            advanced.Controls.Add(hyperlinkTreeViewItem);

            var huge = CreateAdvancedNode("hugeTreeItem");
            advanced.Controls.Add(huge);

            var literalControl = new LiteralControl {ID = "litBig", Text = "Large collection"};
            huge.GetChildrenControls += HugeGetChildrenControls;
            huge.Aspects.Add(new AspectUpdateControl(updateProgressControl));
            huge.Controls.Add(literalControl);
        }

        void HugeGetChildrenControls(object sender, WebWidgets.Extensions.TreeViewItem.GetChildrenControlsEventArgs e)
        {
            // here we render 100 dynamically created TreeViewItems
            for (var idx = 0; idx < 100; idx++)
            {
                var ti = CreateAdvancedNodeLeaf(e.Node, "child" + (idx + 1));
                ti.IconCssClass = "file";
                ti.IsLeaf = true;

                var lit = new LiteralControl {Text = "Item; " + (idx + 1), ID = "hugeLit" + idx};
                ti.Controls.Add(lit);
            }
        }


        /// <summary>
        /// Simple Event Handler. See how easy it is to write Ajax with Gaia?
        /// </summary>
        void CheckboxCheckedChanged(object sender, EventArgs e)
        {
            var cb = (CheckBox) sender;
            resultsPanel.Caption = cb.Parent.ID + " changed to: " + cb.Checked;
        }

        /// <summary>
        /// Simple Event Handler. See how easy it is to write Ajax with Gaia?
        /// </summary>
        void InplaceEditTextChanged(object sender, EventArgs e)
        {
            var ie = (WebWidgets.Extensions.InPlaceEdit) sender;
            resultsPanel.Caption = ie.Parent.ID + " changed to: " + ie.Text;
        }
    }
}