namespace Gaia.WebWidgets.Samples.Combinations.WebApps.DashboardWithWebParts
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        private List<WidgetPosition> _positions;

        // here we specify what widgets to render and set their default container. 
        // You can easily extend the infrastructure by creating your own Control Widgets. 
        private readonly Widget[] widgets = new Widget[]
                                                {
                                                    new SearchWidget("search", "Search", WidgetContainerEnum.Left),
                                                    new AdWidget("ads", "Ad", WidgetContainerEnum.Left),
                                                    new ImageGalleryWidget("gallery", "Image Gallery", WidgetContainerEnum.Center),
                                                    new ScratchPadWidget("scratch", "ScratchPad", WidgetContainerEnum.Center),
                                                    new NewsWidget("news", "News", WidgetContainerEnum.Right),
                                                    new OpenWindowWidget("window", "Open Window", WidgetContainerEnum.Right, "ContentPlaceHolder1.windowHolder")
                                                };

        private class WidgetPosition
        {
            private readonly int _index;

            public int Index
            {
                get { return _index; }
            }

            public WidgetContainerEnum Container { get; set; }

            public int Position { get; set; }

            public WidgetPosition(int index, WidgetContainerEnum container, int position)
            {
                _index = index;
                Container = container;
                Position = position;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            EnsureWidgets();
            base.OnLoad(e);
        }

        /// <summary>
        /// Initialize the ExtendedPanels array dynamically, setup default values and add Aspects for the drop regions
        /// </summary>
        private void InitializeComponent()
        {
            _positions = new List<WidgetPosition>(widgets.Length);
            var counts = new Dictionary<WidgetContainerEnum, int>();
            counts[WidgetContainerEnum.Center] = counts[WidgetContainerEnum.Left] = counts[WidgetContainerEnum.Right] = 0;

            // initialize default positions for widgets
            for (int i = 0; i < widgets.Length; ++i)
            {
                var container = widgets[i].DefaultContainer;
                _positions.Add(new WidgetPosition(i, container, counts[container]));
                ++counts[container];
            }

            StorePositions();

            // make the left, center and right panels into drop regions
            Left.Aspects.Add(new AspectDroppable(dropped, "iGoogleDragged"));
            Center.Aspects.Add(new AspectDroppable(dropped, "iGoogleDragged"));
            Right.Aspects.Add(new AspectDroppable(dropped, "iGoogleDragged"));
        }

        /// <summary>
        /// Since our widgets are dynamic in nature we need to ensure that they are added properly to their
        /// respective placeholders.
        /// </summary>
        private void EnsureWidgets()
        {
            LoadPositions();
            _positions.Sort(PositionComparison);

            foreach (var position in _positions)
                AddWidgetToContainer(widgets[position.Index], position.Container);
        }

        /// <summary>
        /// By utilizing the flexibility of Aspects we have created a custom drag and drop infrastructure for each
        /// Widget. In short this code retrieves the old location and new location and updates the controls collection,
        /// the underlying datasource and then flushes the changes over to the client
        /// </summary>
        protected void dropped(object sender, AspectDroppable.DroppedEventArgs e)
        {
            LoadPositions();

            string[] ids = e.DraggedID.Split('_');
            string droppedWidgetID = ids[ids.Length -2];

            // Finding dragged panel
            Widget dragged = Array.Find(widgets, delegate(Widget item) { return item.ID == droppedWidgetID; });

            // check to see if something else was dropped onto the drop container
            if (dragged == null) return;

            // Removing dragged panel out of its previous parent
            var oldPanel = (Panel)dragged.Parent;

            // finding panel dragged item was dropped onto
            var newPanel = ((AspectDroppable) sender).ParentControl as Panel;

            if (newPanel == oldPanel) return;

            var targetContainer = (WidgetContainerEnum)Enum.Parse(typeof(WidgetContainerEnum), newPanel.ID);
            AddWidgetToContainer(dragged, targetContainer);

            // Updating "database"... ;)
            WidgetPosition position = _positions.Find(delegate(WidgetPosition pos) { return widgets[pos.Index] == dragged; });
            List<WidgetPosition> siblings = _positions.FindAll(delegate(WidgetPosition pos) { return pos.Container == targetContainer; });
            int maxPosition = 0;
            foreach (var sibling in siblings)
                maxPosition = Math.Max(maxPosition, sibling.Position);

            position.Container = targetContainer;
            position.Position = maxPosition + 1;
            StorePositions();
        }

        /// <summary>
        /// Adds the Widget to the corresponding container placeholder
        /// </summary>
        private void AddWidgetToContainer(Widget widget, WidgetContainerEnum container)
        {
            var panelContainer = w.FindControl(container.ToString());
            panelContainer.Controls.Add(widget);
        }

        private void StorePositions()
        {
            _positions.Sort(delegate(WidgetPosition x, WidgetPosition y) { return x.Index.CompareTo(y.Index); });
            var pairs = new List<Pair>(_positions.Count);
        
            foreach(var position in _positions)
                pairs.Add(new Pair(position.Container, position.Position));

            ViewState["pos"] = pairs.ToArray();
        }

        private void LoadPositions()
        {
            var pairs = (Pair[])ViewState["pos"];
            _positions.Clear();
        
            for(int i = 0; i < widgets.Length; ++i)
                _positions.Add(new WidgetPosition(i, (WidgetContainerEnum)pairs[i].First, (int)pairs[i].Second));
        }

        private static int PositionComparison(WidgetPosition x, WidgetPosition y)
        {
            int value = x.Container.CompareTo(y.Container);
            return (value == 0) ? x.Position.CompareTo(y.Position) : value;
        }
    }
}