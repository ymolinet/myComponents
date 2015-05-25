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
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;

namespace Gaia.WebWidgets.Extensions
{
    using HtmlFormatting;

    /// <summary>
    /// The Gaia Ajax TreeView is our version of the TreeView control made popular in the Common Controls from Windows.
    /// As you can see from the screen shot, it's a pretty flexible and useful control.
    /// The Gaia Ajax TreeView Control is extremely flexible and powerful, yet very intuitive and easy to use. You can 
    /// combine static items with dynamic items as you wish. Static items are rendered as part of the HTML output of the 
    /// page (if the TreeView is visible from the start) and dynamic items are retrieved on a "need basis". This means 
    /// the TreeView will be very useful for both visibility in navigations if you like, while at the same time very 
    /// efficient on bandwidth usage up front. The TreeView is very easy to skin since it uses ul and li elements and 
    /// it is also 100% XHTML compliant. It also has perfect support for accessibility.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for TreeView" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\TreeView\Overview\Default.aspx"/>
    /// </code> 
    /// </example>
    [PersistChildren(false)]
    [DefaultProperty("TreeViewItems")]
    [ParseChildren(typeof(TreeViewItem))]
    [ToolboxData("<{0}:TreeView runat=\"server\"></{0}:TreeView>")]
    [ToolboxBitmap(typeof(TreeView), "Resources.Gaia.WebWidgets.Extensions.TreeView.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.TreeViewDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class TreeView : Panel, ISkinControl
    {
        #region [ -- Private Members -- ]

        private TreeViewItem _selectedNode;
        
        #endregion
        
        #region [ -- Events -- ]

        /// <summary>
        /// Fired when a node is selected in the <see cref="TreeView"/>.
        /// Use the <see cref="SelectedNode"/> property to find out which node was selected.
        /// </summary>
        public event EventHandler NodeSelected;

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Style of <see cref="TreeViewItem"/>.
        /// </summary>
        [Category("Styles")]
        [DefaultValue("treeview-lines")]
        [Description("Style of TreeViewItem items.")]
        public string TreeStyle
        {
            get { return StateUtil.Get(ViewState, "TreeStyle", "treeview-lines"); }
            set { StateUtil.Set(ViewState, "TreeStyle", value, "treeview-lines"); }
        }

        /// <summary>
        /// Gets or sets if the collapse and expansion of child <see cref="TreeViewItem"/> should be animated.
        /// Default value is false.
        /// </summary>
        /// <remarks>
        /// This can be overridden for specific <see cref="TreeViewItem"/> which takes precendence over this property.
        /// </remarks>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Specifies if expanding and collapsing of TreeViewItems should be animated.")]
        public bool AnimateToggling
        {
            get { return StateUtil.Get(ViewState, "AnimateToggling", false); }
            set { StateUtil.Set(ViewState, "AnimateToggling", value, false); }
        }

        /// <summary>
        /// Gets or sets if all child <see cref="TreeViewItem"/> are selectable.
        /// Default value is true.
        /// If true (default) all nodes in the TreeView are made selectable and the  
        /// </summary>
        /// <remarks>
        /// In many of the Gaia Samples <see cref="LinkButton"/> are rendered directly in the <see cref="TreeView"/>and 
        /// <see cref="AllowSelections"/> is set to false to avoid "double click" behavior due to event bubbling.
        /// </remarks>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Allows selection of TreeViewItems.")]
        public bool AllowSelections
        {
            get { return StateUtil.Get(ViewState, "AllowSelections", true); }
            set { StateUtil.Set(ViewState, "AllowSelections", value, true); }
        }

        /// <summary>
        /// The collection of <see cref="TreeViewItem"/>.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("Gaia.WebWidgets.Extensions.Design.TreeViewItemCollectionEditor, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a", typeof(UITypeEditor))]
        public TreeViewItemCollection TreeViewItems
        {
            get { return (TreeViewItemCollection) Controls; }
        }
        
        /// <summary>
        /// Gets or sets selected state of the specified node.
        /// </summary>
        /// <remarks>
        /// Getter may return null if no node was selected.
        /// Setting to null clears previous selection.
        /// </remarks>
        [Browsable(false)]
        public TreeViewItem SelectedNode
        {
            get { return _selectedNode; }
            set
            {
                if (value != null)
                    value.Selected = true;
                else if (_selectedNode != null)
                    _selectedNode.Selected = false;
            }
        }

        /// <summary>
        /// Called when <see cref="TreeViewItem.Selected"/> property of the specified <paramref name="node"/> is set.
        /// </summary>
        internal void OnNodeSelect(TreeViewItem node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            var isSelected = node.Selected;
            var isSelectedNode = ReferenceEquals(node, _selectedNode);

            if (!isSelectedNode && isSelected)
            {
                if (_selectedNode != null)
                    _selectedNode.Selected = false;

                _selectedNode = node;
            }
            else if (isSelectedNode && !isSelected)
                _selectedNode = null;
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Overridden to get the correct collection type
        /// </summary>
        /// <returns>TreeViewItemCollection</returns>
        protected override ControlCollection CreateControlCollection()
        {
            return new TreeViewItemCollection(this);
        }

        /// <summary>
        /// Notifies the server control that an element, either XML or HTML, was parsed, and adds the element to the server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object"/> that represents the parsed element. </param>
        protected override void AddParsedSubObject(object obj)
        {
            // add TreeViewItems, ignore LiteralControls, and throw on other controls
            if (obj is TreeViewItem)
                base.AddParsedSubObject(obj);
            else if (!(obj is LiteralControl))
                throw new HttpException("Accordion cannot have children of type " + obj.GetType().Name);
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
            var item = (TreeViewItem)control;
            item.SetOwner(this);
            base.AddedControl(control, index);
        }

        /// <summary>
        /// Called after a child control is removed from the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"/> that has been removed.</param>
        /// <exception cref="T:System.InvalidOperationException">The control is a <see cref="T:System.Web.UI.WebControls.Substitution"/> control.</exception>
        protected override void RemovedControl(Control control)
        {
            base.RemovedControl(control);
            var item = (TreeViewItem) control;
            item.SetOwner(null);
        }

        /// <summary>
        /// Render TreeView
        /// </summary>
        /// <param name="create"></param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (var ul = create.Ul(ClientID).SetCssClass(CombineCssClass("treeview", "treeview-root-ct", TreeStyle)))
            {
                Css.SerializeAttributesAndStyles(this, ul);
                RenderChildren(create.GetHtmlTextWriter());
            }
        }

        #endregion

        #region [ -- Helpers --]

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
        /// Raises <see cref="NodeSelected"/> event.
        /// </summary>
        /// <param name="node">Node which needs to be selected.</param>
        internal void RaiseNodeSelected(TreeViewItem node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (!ReferenceEquals(node, _selectedNode))
                throw new InvalidOperationException("Node should have been selected before raising NodeSelected event.");
            
            if (NodeSelected != null)
                NodeSelected(this, EventArgs.Empty);
        }

        #endregion

        #region [ -- ISkinControl Implementation -- ]

        void ISkinControl.ApplySkin()
        {
            // include default skin css file
            ((IAjaxControl)this).AjaxControl.RegisterDefaultSkinStyleSheetFromResource(typeof(TreeView), Constants.DefaultSkinResource);

            // name of the default skin;
            CssClass = Constants.DefaultSkinCssClass;
        }

        bool ISkinControl.Enabled
        {
            get { return string.IsNullOrEmpty(CssClass) || CssClass.Equals(Constants.DefaultSkinCssClass); }
        }

        #endregion
    }
}
