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
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets.Extensions
{
    using System.Threading;
    using HtmlFormatting;

    /// <summary>
    /// The TabControl extends <see cref="Panel"/> and maintains a collection of <see cref="TabView"/> where only one <see cref="TabView"/> is visible at the time.
    /// The Gaia Ajax TabControl is extremely flexible in regards to skinning and usage. You can use it completely without 
    /// any JavaScript knowledge at all. All you need to do is create it declaratively in your ASP.NET .ASPX code file, choose
    /// your skin and start adding code for its event handlers.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for TabControl" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\TabControl\Overview\Default.aspx"/>
    /// </code> 
    /// </example>
    [Themeable(true)]
    [DefaultProperty("CssClass")]
    [ParseChildren(typeof(TabView))]
    [DefaultEvent("ActiveTabViewChanged")]
    [ToolboxData("<{0}:TabControl runat=\"server\"></{0}:TabControl>")]
    [ToolboxBitmap(typeof(TabControl), "Resources.Gaia.WebWidgets.Extensions.TabControl.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.TabControlDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class TabControl : Panel, ISkinControl, IAjaxContainerControl
    {
        #region [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="TabControl"/> control.
        /// </summary>
        public class PropertyStateManagerTabControl : PropertyStateManagerWebControl
        {
            /// <summary>
            /// Keeps state info for the TabViewHeader
            /// </summary>
            sealed class TabViewHeaderState
            {
                /// <summary>
                /// Caption
                /// </summary>
                public string Caption { get; set; }

                /// <summary>
                /// True if clickable
                /// </summary>
                public bool Clickable { get; set; }

                /// <summary>
                /// Content css class
                /// </summary>
                public string CssClassContent { get; set; }

                /// <summary>
                /// Header css class
                /// </summary>
                public string CssClassHeader { get; set;}

                /// <summary>
                /// Gets or sets if the <see cref="TabView.TabViewHeader"/> was visible.
                /// </summary>
                public bool Visible { get; set; }
            }

            private string _contentWidth;
            private string _contentHeight;
            private readonly TabControl _tabControl;
            private readonly List<TabViewHeaderState> _headers;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerTabControl"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerTabControl(TabControl control) : base(control)
            {
                _tabControl = control;
                _headers = new List<TabViewHeaderState>();
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);
                
                // check state manager
                var stateManager = source as PropertyStateManagerTabControl;
                if (stateManager == null) return;
                
                // assign state
                _contentWidth = stateManager._contentWidth;
                _contentHeight = stateManager._contentHeight;
                
                _headers.Clear();
                _headers.AddRange(stateManager._headers);
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _contentWidth = _tabControl.ContentWidth;
                _contentHeight = _tabControl.ContentHeight;

                base.TakeSnapshot();
            }

            /// <summary>
            /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void DiffSnapshot()
            {
                DiffTabControl();
                DiffTabViewHeaders();
                base.DiffSnapshot();
            }

            /// <summary>
            /// Raised after the state snapshot for the associated <see cref="Control"/> is taken.
            /// </summary>
            /// <returns>
            /// True if event was consumed and the bubbling is cancelled. Otherwise, false.
            /// Default value is false.
            /// </returns>
            /// <remarks>
            /// Bubbles up until stopped by return value of true.
            /// </remarks>
            protected override bool OnSnapshotTakenEvent(object sender, EventArgs eventArgs)
            {
                return HandleTabView(sender);
            }

            /// <summary>
            /// Finds and serializes changes for <see cref="TabControl"/>.
            /// </summary>
            private void DiffTabControl()
            {
                RenderChange(_contentWidth, _tabControl.ContentWidth, "setContentWidth");
                RenderChange(_contentHeight, _tabControl.ContentHeight, "setContentHeight");
            }

            /// <summary>
            /// Find and serialize changes for <see cref="TabView.Header"/>.
            /// </summary>
            private void DiffTabViewHeaders()
            {
                var views = _tabControl.TabViews;
                var current = new List<TabView.TabViewHeader>(views.Count);
                foreach (var view in views)
                    current.Add(view.Header);

                var currentCount = current.Count;
                var initialCount = _headers.Count;

                // no changes in the empty collection
                if (currentCount == 0 && initialCount == 0)
                    return;

                // all headers were removed
                if (currentCount == 0 && initialCount != 0)
                {
                    Builder.Append(".clear()");
                    return;
                }

                // apply changes to existing headers
                var lastVisibleTabViewIndex = -1;
                var commonCount = Math.Min(currentCount, initialCount);
                for (var index = 0; index < commonCount; ++index)
                {
                    var tabView = current[index];
                    DiffTabViewHeader(index, tabView, lastVisibleTabViewIndex);
                    if (!tabView.Visible) continue;
                    lastVisibleTabViewIndex = index;
                }
                
                if (currentCount > commonCount)
                {
                    // render markup for newly added headers
                    var markup = ComposeXhtml.ToString(create => RenderTabViewHeaders(create, current, commonCount));
                    Builder.Append(string.Concat(".add(", FormatValue(markup), ",", lastVisibleTabViewIndex + 1, ")"));
                }
                else if (initialCount > commonCount)
                {
                    // remove redundant headers
                    Builder.Append(string.Concat(".clear(", commonCount, ",", initialCount, ")"));
                }
            }

            /// <summary>
            /// Renders <paramref name="headers"/> starting from <paramref name="startIndex"/> into <paramref name="create"/>.
            /// </summary>
            private static void RenderTabViewHeaders(XhtmlTagFactory create, IList<TabView.TabViewHeader> headers, int startIndex)
            {
                for(var index = startIndex; index < headers.Count; ++index)
                    headers[index].Render(create);
            }

            /// <summary>
            /// Find and serialize changes for <paramref name="header"/> from its state at <paramref name="index"/>.
            /// </summary>
            private void DiffTabViewHeader(int index, TabView.TabViewHeader header, int lastVisibleTabViewIndex)
            {
                var state = _headers[index];
                var isVisible = header.Visible;
                var wasVisible = state.Visible;

                if (wasVisible && isVisible)
                {
                    // fill in arguments for one-shot client-side update of tab header.
                    // sequence of arguments is important.
                    var changeOptions = new List<string>();

                    var value = header.Clickable;
                    if (value != state.Clickable)
                        changeOptions.Add("active:" + (value ? "1" : "0"));

                    var str = header.Caption;
                    if (str != state.Caption)
                        changeOptions.Add("caption:" + FormatValue(str));

                    str = header.CssClassContent;
                    if (str != state.CssClassContent)
                        changeOptions.Add("classContent:" + FormatValue(str));

                    str = header.CssClassHeader;
                    if (str != state.CssClassHeader)
                        changeOptions.Add("classHeader:" + FormatValue(str));

                    if (changeOptions.Count == 0) return;
                    Builder.Append(string.Concat(".update(", index,
                                                 ",{", string.Join(",", changeOptions.ToArray()), "})"));
                }
                else if (!wasVisible && isVisible)
                {
                    var markup = ComposeXhtml.ToString(header.Render);
                    Builder.Append(string.Concat(".add(", FormatValue(markup), ",", lastVisibleTabViewIndex + 1, ")"));
                }
                else if (wasVisible)
                    Builder.Append(string.Concat(".clear(", index, ")"));
            }

            /// <summary>
            /// Checks if <paramref name="sender"/> is a <see cref="TabView"/>
            /// and takes snapshot of its header state.
            /// </summary>
            /// <returns>True if bubbling should be stopped, otherwise: false.</returns>
            private bool HandleTabView(object sender)
            {
                // check TabView
                var tabView = sender as TabView;
                if (tabView == null) return false;
                TakeSnapshotHeader(tabView.Header);
                return true;
            }

            /// <summary>
            /// Takes snapshot of <paramref name="header"/> state.
            /// </summary>
            private void TakeSnapshotHeader(TabView.TabViewHeader header)
            {
                var state = new TabViewHeaderState
                                {
                                    Caption = header.Caption,
                                    Visible = header.Visible,
                                    CssClassContent = header.CssClassContent,
                                    CssClassHeader = header.CssClassHeader,
                                    Clickable = header.Clickable
                                };

                _headers.Add(state);
            }
        }

        #endregion

        #region [ -- Constants -- ]

        private const string IdViewContainer = "content";

        /// <summary>
        /// Command name associated with changing the active <see cref="TabView "/> control in a <see cref="TabControl"/> based on a specified index.
        /// </summary>
        public static readonly string SwitchViewByIndexCommandName = "SwitchViewByIndex";

        #endregion

        private Action<Tag> _viewContainerRenderCallback;

        #region [ -- Events -- ]
        /// <summary>
        /// The ActiveViewIndexChanged fires when the user selects a different tab in the TabControl
        /// </summary>
        public event EventHandler<EventArgs> ActiveTabViewIndexChanged;
        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Gets or sets the index of currently active <see cref="TabView"/>.
        /// </summary>
        [DefaultValue(0)]
        [Category("Behavior")]
        [Description("Active TabView index.")]
        public int ActiveTabViewIndex
        {
            get { return StateUtil.Get(ViewState, "ActiveViewIndex", 0); }
            set { StateUtil.Set(ViewState, "ActiveViewIndex", value, 0); }
        }

        /// <summary>
        /// If set to true, inactive <see cref="TabView"/> will be not part of DOM.
        /// Otherwise, it will have "display:none" style.
        /// </summary>
        /// <remarks>
        /// Setting <see cref="ForceDynamicRendering"/> to true affects the <see cref="TabView.Visible"/> property.
        /// </remarks>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Render TabViews on the client as hidden DOM elements.")]
        public bool ForceDynamicRendering
        {
            get { return StateUtil.Get(ViewState, "ForceDynamicRendering", false); }
            set { StateUtil.Set(ViewState, "ForceDynamicRendering", value, false); }
        }

        /// <summary>
        /// The collection of <see cref="TabView"/>.
        /// </summary>
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("Gaia.WebWidgets.Extensions.Design.TabViewCollectionEditor, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a", typeof(UITypeEditor))]
        public TabViewCollection TabViews
        {
            get { return (TabViewCollection)Controls; }
        }

        /// <summary>
        /// Height of the body
        /// </summary>
        private string ContentHeight
        {
            get { return Height.Type == ASP.UnitType.Percentage ? "auto" : Height.ToString(NumberFormatInfo.InvariantInfo); }
        }

        /// <summary>
        /// Width of the body
        /// </summary>
        private string ContentWidth
        {
            get
            {
                if (!Width.IsEmpty)
                    return Width.Type == ASP.UnitType.Percentage ? "100%" : Width.ToString(NumberFormatInfo.InvariantInfo);

                return "auto";
            }
        }

        /// <summary>
        /// Returns ClientID for the content element.
        /// </summary>
        private string ViewContainerClientId
        {
            get { return CombineId(IdViewContainer); }
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Determines whether the event for the server control is passed up the page's UI server control hierarchy.
        /// </summary>
        /// <returns>
        /// True if the event has been canceled; otherwise, false. The default is false.
        /// </returns>
        /// <param name="source">The source of the event.</param>
        /// <param name="args">An <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            // check if it's a activeviewindex change command
            var commandArgs = args as ASP.CommandEventArgs;
            if (commandArgs != null && commandArgs.CommandName == SwitchViewByIndexCommandName)
            {
                var commandArgument = commandArgs.CommandArgument;
                const string exceptionMessage = "TabControl SwitchViewByIndex command expects valid TabView index.";

                if (commandArgument == null)
                    throw new ArgumentNullException("args", exceptionMessage);

                int index;
                if (!int.TryParse(commandArgument.ToString(), out index))
                    throw new ArgumentException(exceptionMessage, "args");

                VerifyTabViewIndex(index);
                ActiveTabViewIndex = index;
                
                // raise selected index changed event
                if (ActiveTabViewIndexChanged != null)
                    ActiveTabViewIndexChanged(this, EventArgs.Empty);
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Notifies the server control that an element, either XML or HTML, was parsed, and adds the element to the server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object"/> that represents the parsed element. </param>
        protected override void AddParsedSubObject(object obj)
        {
            // add TabViews, ignore LiteralControls, and throw on other controls
            if (obj is TabView)
                base.AddParsedSubObject(obj);
            else if (!(obj is LiteralControl))
                throw new HttpException("Accordion cannot have children of type " + obj.GetType().Name);
        }

        /// <summary>
        /// Called after a child control is added to the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"/> that has been added.</param>
        /// <param name="index">The index of the control in the <see cref="P:System.Web.UI.Control.Controls"/> collection.</param>
        /// <exception cref="T:System.InvalidOperationException"><paramref name="control"/> is a <see cref="T:System.Web.UI.WebControls.Substitution"/> control.</exception>
        protected override void AddedControl(Control control, int index)
        {
            var view = (TabView) control;
            view.Owner = this;
            view.Index = index;
            base.AddedControl(control, index);

            var collection = TabViews;
            var count = collection.Count;
            for (var i = index + 1; i < count; ++i)
                ++collection[i].Index;
        }

        /// <summary>
        /// Called after a child control is removed from the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"/> that has been removed.</param>
        /// <exception cref="T:System.InvalidOperationException">The control is a <see cref="T:System.Web.UI.WebControls.Substitution"/> control.</exception>
        protected override void RemovedControl(Control control)
        {
            base.RemovedControl(control);
            var view = (TabView) control;
            var index = view.Index;
            view.Index = -1;
            view.Owner = null;

            var collection = TabViews;
            var count = collection.Count;
            for (var i = index; i < count; ++i)
                --collection[i].Index;
        }

        /// <summary>
        /// Create the TabControl controls collection
        /// </summary>
        protected override ControlCollection CreateControlCollection()
        {
            return new TabViewCollection(this);
        }

        /// <summary>
        /// Last minute settings before rendering
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            SetPreRenderDefaults();
            base.OnPreRender(e);
        }

        /// <summary>
        /// Base implementation does nothing.
        /// specifies default values for setting during OnPreRender event
        /// </summary>
        protected virtual void SetPreRenderDefaults() { }

        /// <summary>
        /// See <see cref="AjaxControl.RenderControl" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <param name="writer">The <see cref="HtmlTextWriter"/> to write the control into.</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            VerifyTabViewIndex(ActiveTabViewIndex);
            base.RenderControl(writer);
        }

        /// <summary>
        /// Renders the HTML TabControl
        /// </summary>
        /// <param name="create">Writer to render HTML into</param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            RenderTabControl(create);
        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            var manager = Manager.Instance;
            manager.AddInclusionOfExtensionsScriptFiles(typeof(TabControl));
            manager.AddInclusionOfFileFromResource("Gaia.WebWidgets.Extensions.Scripts.TabControl.js", typeof(TabControl), "Gaia.Extensions.TabControl.loaded", true);
        }

        /// <summary>
        /// Returns control registration object required for registering control on the client.
        /// </summary>
        /// <param name="registerControl">Suggested control registration object.</param>
        /// <returns>Modified or new control registration object.</returns>
        protected override RegisterControl GetScript(RegisterControl registerControl)
        {
            var control = base.GetScript(registerControl);
            control.ControlType = "Gaia.Extensions.TabControl";

            var inactives = GetInactiveHeaders();
            return control.AddPropertyIfTrue(inactives != null, "inactive",
                                             string.Concat("[", string.Join(",", inactives), "]"), false);
        }

        /// <summary>
        /// Sets design-time data for a control.
        /// </summary>
        /// <param name="data">
        /// An <see cref="T:System.Collections.IDictionary"/> containing the design-time data for the control. 
        /// </param>
        protected override void SetDesignModeState(IDictionary data)
        {
            _viewContainerRenderCallback = data["ViewContainerRenderCallback"] as Action<Tag>;
        }

        #endregion

        #region [ -- Rendering methods -- ]

        private void RenderTabHeader(XhtmlTagFactory create)
        {
            using (var headerDiv = create.Div()) // header
            {
                headerDiv.SetCssClass(CombineCssClass("tabcontrol-top", "noselect")) // , "tabcontrol-top-plain" ( no background );
                         .SetStyle("-moz-user-select; {0}", GetStyleContentWidth());

                using (create.Div().SetCssClass(CombineCssClass("tabstrip-wrapper"))) // wrapper
                {
                    using (create.Ul().SetCssClass(CombineCssClass("tabstrip", "tabstrip-top"))) // list
                    {
                        foreach (var view in TabViews)
                            view.Header.Render(create);

                        using (create.Li(CombineId("_clear")).SetCssClass(CombineCssClass("clear"))) { } // clear the floats
                    }
                }
                using (create.Div().SetCssClass(CombineCssClass("tabstrip-spacer"))) { }
            }
        }

        private void RenderTabContent(XhtmlTagFactory create)
        {
            using (create.Div().SetCssClass(CombineCssClass("tabcontrol-body-wrapper"))) // body wrapper
            {
                using (create.Div().SetCssClass(CombineCssClass("tabcontrol-body"))) // body
                {
                    using (create.Div()) // panel
                    {
                        using (create.Div()) // panel body wrap
                        {
                            using (var div = create.Div(ViewContainerClientId, CombineCssClass("tabcontrol-body-content"))) // body itself 
                            {
                                div.SetStyle(GetStyleContentHeight());

                                if (DesignMode && _viewContainerRenderCallback != null)
                                    _viewContainerRenderCallback(div);

                                RenderChildren(create.GetHtmlTextWriter());
                            }
                        }
                    }
                }
            }
        }

        private void RenderTabControl(XhtmlTagFactory create)
        {
            using (var tabControl = create.Div(ClientID, Css.Combine(CssClass, false, "tabcontrol"))) // tab control 
            {
                Css.SerializeAttributesAndStyles(this, tabControl);

                RenderTabHeader(create);
                RenderTabContent(create);
            }
        }

        #endregion

        #region [ -- Helpers -- ]

        /// <summary>
        /// Returns indices of inactive headers
        /// </summary>
        /// <returns></returns>
        private string[] GetInactiveHeaders()
        {
            var views = TabViews;
            if (views == null || views.Count == 0) return null;

            var inactives = new List<string>(views.Count);
            for (var index = 0; index < views.Count; ++index)
            {
                var header = views[index].Header;
                if (header.Clickable) continue;
                inactives.Add(index.ToString(NumberFormatInfo.InvariantInfo));
            }

            return inactives.ToArray();
        }

        /// <summary>
        /// Checks if provided <see cref="TabView"/> index is between 0 and TabViews.Count.
        /// </summary>
        /// <param name="index"><see cref="TabView"/> index to check.</param>
        private void VerifyTabViewIndex(int index)
        {
            if (index >= 0 && index < TabViews.Count) return;

            throw new ArgumentOutOfRangeException("index",
                                                  "ActiveTabViewIndex should be between 0 and TabViews.Count.");
        }

        /// <summary>
        /// Combines provided css classes with the <see cref="ASP.WebControl.CssClass"/> property.
        /// </summary>
        /// <param name="cssclasses">Css classes to combine with.</param>
        /// <returns>Combined css classes.</returns>
        internal string CombineCssClass(params string[] cssclasses)
        {
            string combined = Css.Combine(CssClass, cssclasses);
            return string.IsNullOrEmpty(combined) ? combined : combined.TrimEnd();
        }

        /// <summary>
        /// Returns <see cref="ContentHeight"/> for using in style attribute of a tag
        /// </summary>
        private string GetStyleContentHeight()
        {
            return "height:" + ContentHeight + ";";
        }

        /// <summary>
        /// Returns <see cref="ContentWidth"/> for using in style attribute of a tag
        /// </summary>
        private string GetStyleContentWidth()
        {
            return "width:" + ContentWidth + ";";
        }

        /// <summary>
        /// Combines provided suffixes with the <see cref="Control.ClientID"/> property.
        /// </summary>
        /// <param name="suffixes">Suffixes to combine with.</param>
        /// <returns>Combined id.</returns>
        internal string CombineId(params string[] suffixes)
        {
            return string.Concat(ClientID, "_", string.Join("_", suffixes));
        }

        #endregion

        /// <summary>
        /// Returns id of the DOM element which acts as the actual container
        /// for the specified child. Used during dynamic rendering.
        /// </summary>
        /// <param name="child">Child control to get container for</param>
        /// <returns>ID of the DOM element which should contain specified child</returns>
        string IAjaxContainerControl.GetDOMContainer(IAjaxControl child)
        {
            return ViewContainerClientId;
        }

        /// <summary>
        /// Called by the Gaia framework when the StateManager of the Control needs to be initialized
        /// </summary>
        /// <returns>StateManager for the Control</returns>
        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerTabControl(this);
        }

        /// <summary>
        /// Callback method, which is called when <see cref="TabView "/> header is clicked.
        /// </summary>
        [Method]
        internal void TabViewHeaderClicked(int index)
        {
            OnBubbleEvent(this, new ASP.CommandEventArgs(SwitchViewByIndexCommandName, index));
        }

        #region [ -- ISkinControl Implementation -- ]

        void ISkinControl.ApplySkin()
        {
            // include default skin css file
            ((IAjaxControl)this).AjaxControl.RegisterDefaultSkinStyleSheetFromResource(typeof(TabControl), Constants.DefaultSkinResource);

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
