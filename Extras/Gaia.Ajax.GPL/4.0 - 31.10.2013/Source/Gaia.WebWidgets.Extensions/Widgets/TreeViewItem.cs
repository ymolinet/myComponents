/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.Web.UI;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections.Generic;
using Gaia.WebWidgets.Effects;
using Gaia.WebWidgets.HtmlFormatting;

namespace Gaia.WebWidgets.Extensions
{
    /// <summary>
    /// Represents one item in a <see cref="TreeView"/>.
    /// </summary>
    /// <remarks>
    /// Inside the <see cref="TreeViewItem"/> you can add any <see cref="Control"/> you wish.
    /// </remarks>
    /// <example>
    /// <code title="ASPX Markup for TreeViewItem" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\TreeView\Overview\Default.aspx"/>
    /// </code> 
    /// </example>
    [Themeable(true)]
    [ToolboxItem(false)]
    [ParseChildren(false)]
    [PersistChildren(true)]
    [DefaultProperty("CssClass")]
    [ToolboxBitmap(typeof(TreeViewItem), "Resources.Gaia.WebWidgets.Extensions.TreeViewItem.bmp")]
    public class TreeViewItem : GaiaControl, IAjaxContainerControl, INamingContainer, ISkinControl, IPostBackEventHandler
    {

        #region [ -- Constants -- ]

        private const string SelectedEventArgument = "S";
        private const string ExpandedEventArgument = "E";
        private const string CollapsedEventArgument = "C";

        private const string IdControlRegion = "controls";

        #endregion

        #region [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for <see cref="TreeViewItem"/>.
        /// </summary>
        public class PropertyStateManagerTreeViewItem : PropertyStateManagerControl
        {
            private readonly TreeViewItem _node;

            private bool _isLeaf;
            private bool _allowSelections;

            private string _indention;
            private string _cssClassExpander;
            private string _cssClassContainer;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerTreeViewItem"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerTreeViewItem(TreeViewItem control) : base(control)
            {
                _node = control;
            }

            /// <summary>
            /// Returns true if the owner node was collapsed.
            /// </summary>
            internal bool WasCollapsed { get; set; }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="Control"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _isLeaf = _node.IsLeaf;
                WasCollapsed = _node.Collapsed;
                _cssClassContainer = _node.CssClassContainer;
                _allowSelections = _node._owner.AllowSelections;
                _cssClassExpander = _node.GetCssClassExpander(false);
                _indention = ComposeXhtml.ToString(create => _node.RenderIndention(create, false));

                base.TakeSnapshot();
            }

            /// <summary>
            /// Detects changes between current state and saved state snapshot for the associated <see cref="Control"/>.
            /// </summary>
            protected override void DiffSnapshot()
            {
                RenderChange(_cssClassExpander, _node.GetCssClassExpander(), "setCExp", true);
                RenderChange(_cssClassContainer, _node.CssClassContainer, "setCCnt", true);
                RenderChange(_indention, ComposeXhtml.ToString(create => _node.RenderIndention(create)), "setInd", true);
                RenderChange(WasCollapsed, _node.Collapsed, "setCol");
                RenderChange(_isLeaf, _node.IsLeaf, "setLeaf");
                RenderChange(_allowSelections, _node._owner.AllowSelections, "setSel");

                base.DiffSnapshot();
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);
                
                // check state manager
                var stateManager = source as PropertyStateManagerTreeViewItem;
                if (stateManager == null) return;

                _isLeaf = stateManager._isLeaf;
                _indention = stateManager._indention;
                WasCollapsed = stateManager.WasCollapsed;
                _allowSelections = stateManager._allowSelections;
                _cssClassExpander = stateManager._cssClassExpander;
                _cssClassContainer = stateManager._cssClassContainer;
            }
        }

        #endregion

        /// <summary>
        /// Specialized <see cref="AjaxControl"/> for <see cref="TreeViewItem"/>.
        /// </summary>
        public class TreeViewItemAjaxControl : AjaxContainerControl
        {
            private readonly TreeViewItem _owner;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="owner">Control which owns this instance</param>
            public TreeViewItemAjaxControl(TreeViewItem owner) : base(owner)
            {
                if (owner == null)
                    throw new ArgumentNullException("owner");

                _owner = owner;
            }

            /// <summary>
            /// Returns virtual <see cref="ControlCollection"/> for the specified child <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Child control to get container for.</param>
            /// <returns>Virtual container for the specified control.</returns>
            protected override ControlCollection GetVirtualContainer(Control control)
            {
                return control is TreeViewItem ? (ControlCollection) _owner.TreeViewItems : _owner.ChildControls;
            }
        }

        #region [ -- Effect Events -- ]

        /// <summary>
        /// Use this function to apply an effect to the content area of the TreeViewItem
        /// </summary>
        /// <typeparam name="T">Any Effect</typeparam>
        /// <param name="effect">The Effect to apply modifications to</param>
        /// <returns>The effect itself. Don't forget to add the effect to the Window effects collection</returns>
        public static T ApplyToContent<T>(T effect) where T : Effect
        {
            return EffectUtils.AppendElementID("_children", effect);
        }
        /// <summary>
        /// Use this EffectEvent to wire up the effect when the TreeViewItem is expanding
        /// </summary>
        public static AjaxEffectEvent EffectEventExpand { get { return AjaxEffectEventFactory.CreateWithAfterFinishParameter("gaiaexpanding"); } }

        /// <summary>
        /// Use this EffectEvent to wire up the effect when the TreeViewItem is collapsing
        /// </summary>
        public static AjaxEffectEvent EffectEventCollapse { get { return AjaxEffectEventFactory.CreateWithAfterFinishParameter("gaiacollapsing"); } }

        #endregion

        #region [ -- Private Members -- ]

        private TreeView _owner;
        private TreeViewItem _ownerNode;
        private EffectControl _effectControl;
        private AspectableAjaxControl _instance;
        private bool _requiresSelectionNotification;
        private AjaxContainerControl _ajaxContainerControl;
        
        #endregion
        
        #region [ -- Events and EventArgs -- ]

        /// <summary>
        /// EventArgs fired when TreeViewItem needs to fetch the TreeViewItem children
        /// </summary>
        public class GetChildrenControlsEventArgs : EventArgs
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="node">Associated node.</param>
            internal GetChildrenControlsEventArgs(TreeViewItem node)
            {
                Node = node;
            }

            /// <summary>
            /// The <see cref="TreeViewItem"/> node the children request is for
            /// </summary>
            public TreeViewItem Node { get; set; }
        }

        /// <summary>
        /// Fired when node needs to display its children and those are not yet loaded to the client
        /// </summary>
        public event EventHandler<GetChildrenControlsEventArgs> GetChildrenControls;

        /// <summary>
        /// Fired when node it collapsed or expanded.
        /// </summary>
        /// <remarks>
        /// <see cref="Collapsed"/> property defines if the node was collapsed or expanded.
        /// </remarks>
        public event EventHandler Toggled;

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Event is triggered when the Mouse cursor is moved over the control
        /// </summary>
        [Description("Fires when the mouse cursor moves over the control")]
        public event EventHandler<AspectHoverable.HoverEventArgs> MouseOver
        {
            add { Aspects.Bind<AspectHoverable>().MouseOver += value; }
            remove { Aspects.Bind<AspectHoverable>().MouseOver -= value; }
        }

        /// <summary>
        /// Event is triggered when the Mouse cursor is moved outside of the control
        /// </summary>
        [Description("Fires when the mouse cursor is moved outside the control")]
        public event EventHandler MouseOut
        {
            add { Aspects.Bind<AspectHoverable>().MouseOut += value; }
            remove { Aspects.Bind<AspectHoverable>().MouseOut -= value; }
        }

        /// <summary>
        /// Gets or sets if this node is selected.
        /// </summary>
        [DefaultValue(false)]
        internal bool Selected
        {
            get { return StateUtil.Get(ViewState, "Selected", false); }
            set
            {
                StateUtil.Set(ViewState, "Selected", value, false);
                NotifyOwnerAboutSelection();
            }
        }

        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [Description("CSS class of the TreeViewItem.")]
        public string CssClass
        {
            get { return StateUtil.Get(ViewState, "CssClass", string.Empty); }
            set { StateUtil.Set(ViewState, "CssClass", value, string.Empty); }
        }

        /// <summary>
        /// CssClass for the Icon of the TreeView
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [Description("CSS class for the TreeViewItem icon.")]
        public string IconCssClass
        {
            get { return StateUtil.Get(ViewState, "IconCssClass", string.Empty); }
            set { StateUtil.Set(ViewState, "IconCssClass", value, string.Empty); }
        }

        /// <summary>
        /// Gets or sets if node is collapsed or expanded.
        /// </summary>
        /// <remarks>
        /// If true, collapsed node still renders all content including children, which might be useful
        /// for large <see cref="TreeView"/>, for e.g. SEO purposes and similar cases.
        /// </remarks>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Collapsed or expanded state of the TreeViewItem.")]
        public bool Collapsed
        {
            get { return StateUtil.Get(ViewState, "Collapsed", true); }
            set { StateUtil.Set(ViewState, "Collapsed", value, true); }

        }

        /// <summary>
        /// Gets or sets if the <see cref="TreeViewItem"/> is a leaf node.
        /// </summary>
        /// <remarks>
        /// Leaf <see cref="TreeViewItem"/> cannot be expanded or collapsed.
        /// </remarks>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Specifies if the TreeViewItem is a leaf node.")]
        public bool IsLeaf
        {
            get { return StateUtil.Get(ViewState, "IsLeaf", false); }
            set { StateUtil.Set(ViewState, "IsLeaf", value, false); }
        }

        //todo: re-implement animate toggling by adding effects in on_pre_render ...
        //todo: find a way to add effects on the treeview which gets added to all children
        //todo: 

        /// <summary>
        /// If true (defaults to true) then nodes will be "animated" up and down on the client-side
        /// when expanded and collapsed. If this value is not explicitly set for the %TreeViewItem
        /// then the default set value of the TreeView will be used. The default of both of these
        /// values are true.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Specifies if expanding and collapsing of TreeViewItems should be animated.")]
        [Obsolete("AnimateToggling is no longer supported. If you want custom animation please add custom Effects to the effects collection")]
        public bool AnimateToggling
        {
            get { return StateUtil.Get(ViewState, "AnimateToggling", false); }
            set { StateUtil.Set(ViewState, "AnimateToggling", value, false); }
        }

        /// <summary>
        /// This collection contains all the Child TreeViewItems in this TreeViewItem. Use this collection instead
        /// of the <see cref="Control.Controls"/> collection to access your TreeViewItems. It's safe to call AddAt, RemoveAt and Clear on 
        /// this controls collection. If you need to access your custom user defined controls on the TreeViewItem
        /// use the ChildControls collection instead. 
        /// </summary>
        [Editor("Gaia.WebWidgets.Extensions.Design.TreeViewItemCollectionEditor, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a", typeof(UITypeEditor))]
        public ControlCollectionOf<TreeViewItem> TreeViewItems
        {
            get { return ((TreeViewItemAwareContainerCollection) Controls).Containers ; } 
        }

        /// <summary>
        /// This collection contains all your user defined controls and not the child TreeViewItems. This control works as 
        /// an abstraction on top of the Controls collection, but you should use this collection instead for secure access.
        /// If you need to access your TreeViewItems please use the <see cref="TreeViewItems"/> collection.  
        /// </summary>
        [Browsable(false)]
        public ControlCollectionExcept<TreeViewItem> ChildControls
        {
            get { return ((TreeViewItemAwareContainerCollection)Controls).ChildControls; }
        }

        private bool HasRetrievedChildren
        {
            get { return StateUtil.Get(ViewState, "HasRetrievedChildren", false); }
            set { StateUtil.Set(ViewState, "HasRetrievedChildren", value, false); }
        }

        /// <summary>
        /// Returns CssClass for the container DOM element.
        /// </summary>
        [Browsable(false)]
        private string CssClassContainer
        {
            get
            {
                var selectedCssClas = Selected ? "treeview-selected" : string.Empty;
                var collapsedCssClass = Collapsed ? "treeview-item-collapsed" : "treeview-item-expanded";
                var leafCssClass = IsLeaf ? "treeview-item-leaf" : collapsedCssClass;
                var iconCssClass = string.IsNullOrEmpty(IconCssClass) ? string.Empty : IconCssClass;

                return CombineCssClass("treeview-item-el", "noselect", selectedCssClas, leafCssClass) + iconCssClass;
            }
        }

        /// <summary>
        /// Returns CssClass for the expander DOM element.
        /// </summary>
        /// <param name="useIsEndNode">
        /// If true uses <see cref="IsEndNode"/> for state management.
        /// Otherwise <see cref="WasEndNode"/> is used.
        /// </param>
        private string GetCssClassExpander(bool useIsEndNode = true)
        {
            var isLast = useIsEndNode ? IsEndNode : WasEndNode;
            var tailCssClass = isLast ? "-last" : string.Empty;
            var collapsedCssClass = tailCssClass + (Collapsed ? "-expanded" : "-collapsed");
            var leafCssClass = "treeview-branch" + (IsLeaf ? tailCssClass : collapsedCssClass);

            return CombineCssClass("treeview-ec-icon", leafCssClass) + "span-for-image";
        }

        /// <summary>
        /// Return true if this item is the last in its owner's node collection.
        /// </summary>
        private bool IsEndNode
        {
            get
            {
                ControlCollection container = null;

                if (_ownerNode != null)
                    container = _ownerNode.TreeViewItems;
                else if (_owner != null)
                    container = _owner.TreeViewItems;

                return container != null && ReferenceEquals(container[container.Count - 1], this);
            }
        }

        /// <summary>
        /// Accessor to the value of <see cref="IsEndNode"/> property stored in the <see cref="Control.ViewState"/>.
        /// Used by <see cref="PropertyStateManagerTreeViewItem"/> when taking snapshot of the state.
        /// </summary>
        /// <seealso cref="SaveViewState"/>
        private bool WasEndNode
        {
            get { return StateUtil.Get(ViewState, "WasEnd", false); }
            set
            {
                if (value)
                    StateUtil.Set(ViewState, "WasEnd", true);
                else
                    ViewState.Remove("WasEnd");
            }
        }

        /// <summary>
        /// See AspectableAjaxControl.Aspects for documentation for this method
        /// </summary>
        [Browsable(false)]
        public AspectCollection Aspects
        {
            get { return AjaxContainerControl.Aspects; }
        }

        private EffectControl EffectControl
        {
            get { return _effectControl ?? (_effectControl = new EffectControl(this)); }
        }

        /// <summary>
        /// Collection of Effects for the Control. 
        /// </summary>
        [Browsable(false)]
        public EffectCollection Effects { get { return EffectControl.Effects; } }        

        #endregion

        #region [ -- Public Methods -- ]

        /// <summary>
        /// Collapses this node. 
        /// </summary>
        /// <remarks>
        /// This function is not recursive meaning that it will only apply to this particular item. 
        /// </remarks>
        [Obsolete("Use Collapsed property instead.")]
        public void Collapse()
        {
            Collapsed = true;
        }

        /// <summary>
        /// Expands this node.
        /// </summary>
        /// <remarks>
        /// This function is not recursive meaning that it will only apply to this particular item. 
        /// </remarks>
        [Obsolete("Use Collapsed property instead.")]
        public void Expand()
        {
            Collapsed = false;
        }

        /// <summary>
        /// Implementation of ForceAnUpdate (which re-renderes every child control of TreeViewItem)
        /// </summary>
        [Obsolete("Consider relying on DRIMR or using IAjaxContainerControl.ForceAnUpdate().")]
        public void ForceAnUpdate()
        {
            AjaxContainerControl.ForceAnUpdate();
        }

        /// <summary>
        /// Implementation of ForceAnUpdateWithAppending which basically just appends newly added controls 
        /// into the TreeViewItem
        /// </summary>
        [Obsolete("Consider relying on DRIMR or using IAjaxContainerControl.ForceAnUpdate().")]
        public void ForceAnUpdateWithAppending()
        {
            AjaxContainerControl.ForceAnUpdateWithAppending();
        }

        /// <summary>
        /// Start Tracking New Controls to the Control Collection. These will be rendered during ForceAnUpdate or
        /// ForceAnUpdateWithAppending
        /// </summary>
        [Obsolete("Consider relying on DRIMR or using IAjaxContainerControl.ForceAnUpdate().")]
        public void TrackControlAdditions()
        {
            AjaxContainerControl.TrackControlAddition();
        }

        /// <summary>
        /// Sets the specified <paramref name="owner"/> for this node.
        /// </summary>
        /// <remarks>
        /// Propogates the owner to the child nodes and handles selection notification.
        /// </remarks>
        internal void SetOwner(TreeView owner)
        {
            _owner = owner;

            if (_owner != null && _requiresSelectionNotification)
                _owner.OnNodeSelect(this);

            foreach (var child in TreeViewItems)
                child.SetOwner(owner);
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Creates a new <see cref="T:System.Web.UI.ControlCollection"/> object to hold the child controls (both literal and server) of the server control.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.UI.ControlCollection"/> object to contain the current server control's child server controls.
        /// </returns>
        protected override ControlCollection CreateControlCollection()
        {
            return new TreeViewItemAwareContainerCollection(this);
        }

        /// <summary>
        /// Called after a child control is added to the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"/> that has been added.</param>
        /// <param name="index">The index of the control in the <see cref="P:System.Web.UI.Control.Controls"/> collection.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="control"/> is a <see cref="T:System.Web.UI.WebControls.Substitution"/>  control.
        /// </exception>
        protected override void AddedControl(Control control, int index)
        {
            var node = control as TreeViewItem;
            if (node != null)
            {
                node._ownerNode = this;
                if (_owner != null)
                    node.SetOwner(_owner);
            }

            base.AddedControl(control, index);
        }

        /// <summary>
        /// Called after a child control is removed from the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"/> that has been removed.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// The control is a <see cref="T:System.Web.UI.WebControls.Substitution"/> control.
        /// </exception>
        protected override void RemovedControl(Control control)
        {
            base.RemovedControl(control);
            
            var node = control as TreeViewItem;
            if (node == null) return;
            
            node._ownerNode = null;
            node.SetOwner(null);
        }

        /// <summary>
        /// Load ViewState for this control. Also Initializes Dynamic Control Children.
        /// </summary>
        /// <param name="savedState"></param>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            if (Selected)
                NotifyOwnerAboutSelection();

            if (!HasRetrievedChildren) return;
            RaiseGetChildren();
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            EnsureChildren();
            base.OnPreRender(e);

            // We keep this code for backwards compatibility. If AnimateToggling is defined
            // We add custom SlideUp/SlideDown effects to Expand/Collapse. These effects
            // are not added if other effects are added instead
            var shouldAnimate = AnimateToggling || _owner.AnimateToggling;
            if (shouldAnimate)
            {
                Effects.Add(EffectEventExpand, ApplyToContent(new EffectBlindDown(0.5M)));
                Effects.Add(EffectEventCollapse, ApplyToContent(new EffectBlindUp(0.5M)));
            }

            EffectControl.OnPreRender();
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.SaveViewState" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <returns>the saved ViewState</returns>
        protected override object SaveViewState()
        {
            WasEndNode = IsEndNode;
            return base.SaveViewState();
        }

        /// <summary>
        /// Include TreeViewItem Javascript files
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            // Include TreeViewItem JavaScript stuff
            var manager = Manager.Instance;
            manager.AddInclusionOfExtensionsScriptFiles(typeof(TreeViewItem));
            manager.AddInclusionOfFileFromResource("Gaia.WebWidgets.Extensions.Scripts.TreeViewItem.js", typeof(TreeViewItem), "Gaia.Extensions.TreeViewItem.browserFinishedLoading", true);
        }

        /// <summary>
        /// Renders this node using specified <paramref name="create"/> factory.
        /// </summary>
        /// <param name="create"><see cref="XhtmlTagFactory"/> to render into.</param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (create.Li(ClientID, Css.Combine(CssClass, false, "treeview-item")))
            {
                RenderControlContainer(create);
                RenderNodeContainer(create);
            }
        }

        #endregion

        #region [ -- Rendering methods -- ]

        private Action<Tag> _expanderRenderCallback;
        private Action<Tag> _childContainerRenderCallback;
        

        /// <summary>
        /// Renders <see cref="ChildControls"/> container DOM element using specified <paramref name="create"/> factory.
        /// </summary>
        private void RenderControlContainer(XhtmlTagFactory create)
        {
            using (create.Div(ClientID + "_container", CssClassContainer)) // Wrapping the entire TreeViewItem "line"
            {
                using (create.Span(null, CombineCssClass("treeview-item-indent span-for-indent")))
                {
                    RenderIndention(create);
                }

                // render expand/collapse icon.
                using (var span = create.Span(ClientID + "_expander", GetCssClassExpander()))
                {
                    if (DesignMode && _expanderRenderCallback != null)
                        _expanderRenderCallback(span);
                }

                // The "Folder" or "File" icon
                using (create.Span(null, CombineCssClass("treeview-item-icon") + "span-for-image")) { }

                using (create.A(null, CombineCssClass("treeview-item-anchor")).
                    AddAttribute(XhtmlAttribute.Href, "javascript:void(0);"))
                {
                    using (var span = create.Span(ClientID + "_" + IdControlRegion))
                    {
                        if (DesignMode && _childContainerRenderCallback != null)
                            _childContainerRenderCallback(span);

                        RenderChildControls(create);
                    }
                }
            }
        }

        /// <summary>
        /// Renders <see cref="TreeViewItems"/> container DOM element using specified <paramref name="create"/> factory.
        /// </summary>
        private void RenderNodeContainer(XhtmlTagFactory create)
        {
            if (DesignMode && (Collapsed || TreeViewItems.Count == 0)) return;

            using (var ul = create.Ul(ClientID + "_children", CombineCssClass("treeview-item-ct")))
            {
                if (Collapsed)
                    ul.SetStyle("display:none;position:static;left:auto;top:auto;z-index:auto;");

                RenderChildTreeViewItems(create);
            }
        }

        /// <summary>
        /// Renders indents using specified <paramref name="create"/> factory.
        /// </summary>
        /// <param name="create">Factory to use for rendering.</param>
        /// <param name="useIsEndNode">
        /// If true uses <see cref="IsEndNode"/> for state management.
        /// Otherwise <see cref="WasEndNode"/> is used.
        /// </param>
        private void RenderIndention(XhtmlTagFactory create, bool useIsEndNode = true)
        {
            foreach (var node in WalkDownFromRoot)
            {
                var isLast = useIsEndNode ? node.IsEndNode : node.WasEndNode;
                var cssClass = CombineCssClass(isLast ? "treeview-icon" : "treeview-branch-line") + "span-for-image";
                using (create.Span(null, cssClass).WriteNonBreakingSpace()) { }
            }
        }

        /// <summary>
        /// Renders child nodes using specified <paramref name="create"/> factory.
        /// </summary>
        private void RenderChildTreeViewItems(XhtmlTagFactory create)
        {
            var nodes = TreeViewItems;

            if (nodes.Count > 0)
            {
                foreach (var node in nodes)
                    node.RenderControl(create.GetHtmlTextWriter());
            }
            else
            {
                // make sure we have valid html.
                using (create.Li()) { }
            }
        }

        /// <summary>
        /// Renders child controls using specified <paramref name="create"/> factory.
        /// </summary>
        private void RenderChildControls(XhtmlTagFactory create)
        {
            foreach (Control child in ChildControls)
                child.RenderControl(create.GetHtmlTextWriter());
        }

        /// <summary>
        /// Sets design-time data for a control.
        /// </summary>
        /// <param name="data">
        /// An <see cref="T:System.Collections.IDictionary"/> containing the design-time data for the control.
        /// </param>
        protected override void SetDesignModeState(IDictionary data)
        {
            _expanderRenderCallback = data["ExpanderRenderCallback"] as Action<Tag>;
            _childContainerRenderCallback = data["ChildContainerRenderCallback"] as Action<Tag>;
        }

        #endregion

        #region [ -- Overridden IAjaxControl methods -- ]

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new TreeViewItemAjaxControl(this)); }
        }

        string IAjaxControl.TagName
        {
            get { return "li"; }
        }

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerTreeViewItem(this);
        }

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.TreeItem", ClientID)
                .AddProperty("p", GetProperties())
                .AddPropertyIfTrue(UniqueID != ClientID.Replace('_', '$'), "callbackName", UniqueID)
                .AddAspects(Aspects).AddEffects(Effects)
                .ToString();
        }

        /// <summary>
        /// Returns bit-field set state for the client initialization.
        /// </summary>
        private int GetProperties()
        {
            return
                (IsEndNode ? 1 : 0) |
                (IsLeaf ? 2 : 0) |
                (_owner.AllowSelections ? 4 : 0) |
                (TreeViewItems.Count > 0 ? 8 : 0) |
                (Collapsed ? 16 : 0);
        }

        #endregion

        #region [ -- IAspectableAjaxControl Implementation -- ]

        AspectableAjaxControl IAspectableAjaxControl.AspectableAjaxControl
        {
            get { return AjaxContainerControl; }
        }

        #endregion

        #region [ -- IAjaxContainerControl Implementation -- ]

        string IAjaxContainerControl.GetDOMContainer(IAjaxControl child)
        {
            return (child.Control is TreeViewItem) ? ClientID + "_children" : ClientID + "_" + IdControlRegion;
        }

        void IAjaxContainerControl.RenderChildrenOnForceAnUpdate(XhtmlTagFactory create)
        {
            if (AjaxContainerControl.IsTrackingControlAdditions)
            {
                // ForceAnUpdateWithAppending only appends new TreeViewItems
                foreach (var treeViewItem in TreeViewItems) 
                    treeViewItem.RenderControl(create.GetHtmlTextWriter());

                // Here we make sure that we serialize property changes for all other controls
                foreach (var childControl in ChildControls)
                {
                    var ajaxControl = childControl as IAjaxControl;
                    if (ajaxControl != null && ajaxControl.StateManager != null)
                        ajaxControl.StateManager.RenderChanges(Manager.Instance.Writer); 
                }
            }
            else
            {
                RenderControlContainer(create);
                RenderNodeContainer(create);
            }
        }

        AjaxContainerControl IAjaxContainerControl.AjaxContainerControl
        {
            get { return (AjaxContainerControl)((IAjaxContainerControl)this).AjaxControl; }
        }

        private AjaxContainerControl AjaxContainerControl
        {
            get { return _ajaxContainerControl ?? (_ajaxContainerControl = ((IAjaxContainerControl) this).AjaxContainerControl); }
        }

        #endregion
        
        #region [ -- Helpers -- ]

        /// <summary>
        /// Walks from the root <see cref="TreeViewItem"/> to this node.
        /// </summary>
        private IEnumerable<TreeViewItem> WalkDownFromRoot
        {
            get
            {
                var stack = new Stack<TreeViewItem>();
                for (var parent = Parent as TreeViewItem; parent != null; parent = parent.Parent as TreeViewItem)
                    stack.Push(parent);

                while (stack.Count > 0)
                    yield return stack.Pop();
            }
        }

        /// <summary>
        /// Combines provided css classes with the <see cref="System.Web.UI.WebControls.WebControl.CssClass"/> property.
        /// </summary>
        /// <param name="cssclasses">Css classes to combine with.</param>
        /// <returns>Combined css classes.</returns>
        private string CombineCssClass(params string[] cssclasses)
        {
            return Css.Combine(CssClass, cssclasses);
        }

        /// <summary>
        /// Notifies owner <see cref="TreeView"/> about change of <see cref="Selected"/> property.
        /// </summary>
        /// <remarks>
        /// If the owner <see cref="TreeView"/> is not available, the action will be delayed until <see cref="SetOwner"/> is called.
        /// </remarks>
        /// <seealso cref="SetOwner"/>
        private void NotifyOwnerAboutSelection()
        {
            _requiresSelectionNotification = true;
            if (_owner == null) return;
            _owner.OnNodeSelect(this);
            _requiresSelectionNotification = false;
        }
        
        #endregion        

        #region [ -- ISkinControl Implementation -- ]

        void ISkinControl.ApplySkin()
        {
            if (!string.IsNullOrEmpty(CssClass)) return;

            // include default skin css file
            AjaxContainerControl.RegisterDefaultSkinStyleSheetFromResource(typeof(TreeViewItem), Constants.DefaultSkinResource);

            // name of the default skin;
            CssClass = Constants.DefaultSkinCssClass;
        }

        bool ISkinControl.Enabled
        {
            get { return string.IsNullOrEmpty(CssClass) || CssClass.Equals(Constants.DefaultSkinCssClass); }
        }

        #endregion

        #region [ -- Postback management methods -- ]

        /// <summary>
        /// Raises events for the <see cref="TreeViewItem"/> when it posts back to the server.
        /// </summary>
        /// <param name="eventArgument">The argument for the event.</param>
        protected virtual void RaisePostBackEvent(string eventArgument)
        {
            if (string.CompareOrdinal(eventArgument, SelectedEventArgument) == 0)
                RaiseSelected();
            else if (string.CompareOrdinal(eventArgument, CollapsedEventArgument) == 0)
                RaiseToggled(true);
            else if (string.CompareOrdinal(eventArgument, ExpandedEventArgument) == 0)
                RaiseToggled(false);
        }

        /// <summary>
        /// When implemented by a class, enables a server control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">
        /// A <see cref="T:System.String"/> that represents an optional event argument to be passed to the event handler. 
        /// </param>
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            RaisePostBackEvent(eventArgument);
        }

        /// <summary>
        /// Raises <see cref="TreeView.NodeSelected"/> event.
        /// </summary>
        private void RaiseSelected()
        {
            if (Selected) return;
            Selected = true;
            _owner.RaiseNodeSelected(this);
        }

        /// <summary>
        /// Raises <see cref="Toggled"/> event.
        /// </summary>
        private void RaiseToggled(bool collapsed)
        {
            Collapsed = collapsed;
            if (Toggled != null)
                Toggled(this, EventArgs.Empty);
        }

        #endregion

        #region [ -- Children retrieval related methods --]

        /// <summary>
        /// Ensures child nodes and controls are retrieved if needed.
        /// </summary>
        private void EnsureChildren()
        {
            if (HasRetrievedChildren || !RequiresChildrenRetrieval) return;
            RaiseGetChildren();
            HasRetrievedChildren = true;
        }

        /// <summary>
        /// Returns true if retrieval of child nodes and controls is required.
        /// </summary>
        /// <seealso cref="EnsureChildren"/>
        private bool RequiresChildrenRetrieval
        {
            get
            {
                // should be expanded
                if (Collapsed) return false;

                // not in postback
                if (!Page.IsPostBack) return true;

                // or previously collapsed
                var stateManager = AjaxContainerControl.StateManager as PropertyStateManagerTreeViewItem;
                return stateManager == null || stateManager.WasCollapsed;
            }
        }

        /// <summary>
        /// Raises <see cref="GetChildrenControls"/> event.
        /// </summary>
        private void RaiseGetChildren()
        {
            if (GetChildrenControls == null) return;
            GetChildrenControls(this, new GetChildrenControlsEventArgs(this));
        }

        #endregion
    }
}
