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
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gaia.WebWidgets.Extensions
{
    using HtmlFormatting;

    /// <summary>
    /// The ControlPicker is a flexible control that allows you to render any <see cref="Control"/> and
    /// select one/multiple instances of the control from client-side.
    /// </summary>
    [ToolboxItem(false)]
    [DefaultProperty("CssClass")]
    [DefaultEvent("SelectedIndexChanged")]
    public class ControlPicker : Panel, IAjaxContainerControl, ISkinControl, IPostBackDataHandler
    {
        #region [ -- State Manager -- ]

        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="ControlPicker"/> control.
        /// </summary>
        public class PropertyStateManagerControlPicker : PropertyStateManagerWebControl
        {
            private bool _hoverSelection;
            private List<int> _selection;

            private readonly ControlPicker _owner;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerControlPicker"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerControlPicker(ControlPicker control) : base(control)
            {
                _owner = control;
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);

                var stateManager = source as PropertyStateManagerControlPicker;
                if (stateManager == null) return;

                _selection = stateManager._selection;
                _hoverSelection = stateManager._hoverSelection;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ControlPicker"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _hoverSelection = _owner.HoverSelection;
                _selection = new List<int>(_owner.SelectedIndexes);

                base.TakeSnapshot();
            }

            /// <summary>
            /// Detects changes between current state and saved state snapshot for the associated <see cref="ControlPicker"/>.
            /// </summary>
            protected override void DiffSnapshot()
            {
                var currentSelection = new List<int>(_owner.SelectedIndexes);

                var selected = currentSelection.FindAll(index => !_selection.Contains(index)).
                    ConvertAll(index => index.ToString(NumberFormatInfo.InvariantInfo));

                var deselected = _selection.FindAll(index => !currentSelection.Contains(index)).
                    ConvertAll(index => index.ToString(NumberFormatInfo.InvariantInfo));

                if (selected.Count > 0)
                {
                    Builder.Append(".setSel([").Append(string.Join(",", selected.ToArray())).Append("],").
                        Append(FormatValue(_owner.CssClassForSelectedItem)).Append(")");
                }

                if (deselected.Count > 0)
                {
                    Builder.Append(".setSel([").Append(string.Join(",", deselected.ToArray())).Append("],").
                        Append(FormatValue(_owner.CssClassForItem)).Append(")");
                }

                RenderChange(_hoverSelection, _owner.HoverSelection, "setHoverSelection");
                base.DiffSnapshot();
            }
        }

        #endregion

        #region [ -- Enums -- ]

        /// <summary>
        /// Enum used to Control the Selection Behaviour of the ControlPicker. 
        /// Can be set to either single/multiple selection mode ...
        /// </summary>
        public enum ItemSelectionMode
        {
            /// <summary>
            /// Only one Item can be Selected at any given time ...
            /// </summary>
            Single,

            /// <summary>
            /// Multiple Items can be selected/deselected at any given time
            /// </summary>
            Multiple
        }
 
        #endregion

        private static readonly int[] EmptyArray = new int[0];

        #region [ -- Properties -- ]
        
        /// <summary>
        /// Gets or sets if the <see cref="Control"/> items in the <see cref="ControlPicker"/>
        /// should be highlighted when hovered.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Highlight controls when hovered.")]
        public bool HoverSelection
        {
            get { return StateUtil.Get(ViewState, "HoverSelection", false); }
            set { StateUtil.Set(ViewState, "HoverSelection", value, false); }
        }

        /// <summary>
        /// Gets or sets <see cref="Control"/> items selection mode.
        /// </summary>
        [Category("Behavior")]
        [AjaxSerializable("setSelectionMode")]
        [DefaultValue(ItemSelectionMode.Single)]
        [Description("Controls selection mode.")]
        public ItemSelectionMode SelectionMode
        {
            get { return StateUtil.Get(ViewState, "SelectionMode", ItemSelectionMode.Single); }
            set { StateUtil.Set(ViewState, "SelectionMode", value, ItemSelectionMode.Single); }
        }

        /// <summary>
        /// Gets or sets the ID of the DOM input element which should be used for tracking key events for navigation.
        /// </summary>
        /// <remarks>
        /// <see cref="HoverSelection"/> should be set to True for the tracking to work.
        /// </remarks>
        [DefaultValue("")]
        [Description("ID of the DOM input element for key navigation.")]
        public string InputElementForKeyPressNavigation
        {
            get { return StateUtil.Get(ViewState, "InputElementForKeyPressNavigation", string.Empty); }
            set { StateUtil.Set(ViewState, "InputElementForKeyPressNavigation", value, string.Empty); }
        }

        /// <summary>
        /// Currently selected indexes
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int[] SelectedIndexes
        {
            get { return StateUtil.Get(ViewState, "SelectedIndex", EmptyArray); }
            set { StateUtil.Set(ViewState, "SelectedIndex", value, EmptyArray); }
        }

        /// <summary>
        /// Enumerates over the list of Selected Controls in the ControlPicker
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<Control> SelectedValues
        {
            get
            {
                var index = 0;
                foreach (Control control in Controls)
                {
                    if (IndexIsSelected(index))
                        yield return control;
                    
                    ++index;
                }
            }
        }

        /// <summary>
        /// Returns true if the mouse is over an item.
        /// </summary>
        internal bool IsMouseOverItem { get; private set; }

        #endregion

        #region [ -- Events and EventArgs -- ]
        
        /// <summary>
        /// Raised when the <see cref="SelectedIndexes"/> are changed.
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        /// <summary>
        /// EventArgs forwarded on every rendering of a Control in the ControlPicker
        /// </summary>
        public class ControlPickerRenderControlItemEventArgs : EventArgs
        {
            private readonly XhtmlTagFactory _factory;
            private readonly Control _control;

            /// <summary>
            /// The XhtmlTagFactory for writing xhtml compliant code
            /// </summary>
            public XhtmlTagFactory XhtmlTagFactory
            {
                get { return _factory; }
            }

            /// <summary>
            /// The currently rendering control
            /// </summary>
            public Control Control
            {
                get { return _control; }
            }


            internal ControlPickerRenderControlItemEventArgs(XhtmlTagFactory factory, Control control)
            {
                _factory = factory;
                _control = control;
            }
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Overridden to make sure ComboList is rendered
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            RenderComboList(new XhtmlTagFactory(writer));
        }

        private void RenderComboList(XhtmlTagFactory create)
        {
            using (var list = create.Div(CombineId("_list"), CombineCssClass("layer", "controlpicker")))
            {
                var dimensions = string.Empty;
                if (!Width.IsEmpty)
                    dimensions = "width:" + Width.ToString(NumberFormatInfo.InvariantInfo);

                if (!Height.IsEmpty)
                    dimensions = "height:" + Height.ToString(NumberFormatInfo.InvariantInfo);

                if (dimensions.Length > 0)
                    list.SetStyle(dimensions);

                using (var wrapper = create.Div(CombineId("_c")).SetCssClass(CombineCssClass("controlpicker-wrapper")))
                {
                    if (dimensions.Length > 0)
                        wrapper.SetStyle(dimensions);

                    RenderChildrenInternal(create);
                }
            }
        }

        internal virtual void RenderChildrenInternal(XhtmlTagFactory create)
        {
            var idx = 0;
            foreach (Control control in Controls)
            {
                using (create.Div().SetCssClass(GetCssClassForItem(idx)))
                {
                    control.RenderControl(create.GetHtmlTextWriter());
                }

                ++idx;
            }
        }

        /// <summary>
        /// Render ControlPicker
        /// </summary>
        /// <param name="create"></param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (var div = create.Div(ClientID, CssClass)) // root div
            {
                Css.SerializeAttributesAndStyles(this, div);
                RenderChildren(create.GetHtmlTextWriter());
            }
        }

        /// <summary>
        /// Include ControlPicker Javascript files
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            // Include ControlPicker Javascript stuff
            Manager.Instance.AddInclusionOfExtensionsScriptFiles(typeof(ControlPicker));
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Extensions.Scripts.ControlPicker.js", typeof(ControlPicker), "Gaia.Extensions.ControlPicker.browserFinishedLoading", true);
            
        }

        /// <summary>
        /// Returns control registration object required for registering control on the client.
        /// </summary>
        /// <param name="registerControl">Suggested control registration object.</param>
        /// <returns>Modified or new control registration object.</returns>
        protected override RegisterControl GetScript(RegisterControl registerControl)
        {
            return new RegisterControl("Gaia.Extensions.ControlPicker", ClientID)
                .AddProperty("className", CssClass)
                .AddProperty("hoverSelection", HoverSelection)
                .AddProperty("selectionMode", SelectionMode.ToString())
                .AddAspects(Aspects).AddEffects(Effects)
                .AddPropertyIfTrue(!string.IsNullOrEmpty(InputElementForKeyPressNavigation), "input",
                                   InputElementForKeyPressNavigation);

        }

        #endregion

        #region [ -- Overridden IAjaxContainerControl Methods -- ]

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerControlPicker(this);
        }

        string IAjaxContainerControl.GetDOMContainer(IAjaxControl child)
        {
            return CombineId("_c");
        }

        #endregion

        #region [ -- Helpers -- ]
        
        private string CombineId(string append)
        {
            return string.Concat(ClientID, append);
        }

        internal string GetCssClassForItem(int index)
        {
            return IndexIsSelected(index) ? CssClassForSelectedItem : CssClassForItem;
        }

        private string CssClassForSelectedItem
        {
            get { return CombineCssClass("controlpicker-item", "controlpicker-selected"); }
        }

        private string CssClassForItem
        {
            get { return CombineCssClass("controlpicker-item"); }
        }

        private string CombineCssClass(params string[] cssclass)
        {
            return Css.Combine(CssClass, cssclass);
        }

        private bool IndexIsSelected(int index)
        {
            return Array.Exists(SelectedIndexes, idx => idx == index);
        }

        #endregion

        #region [ -- ISkinControl Implementation -- ]

        void ISkinControl.ApplySkin()
        {
            // include default skin css file
            ((IAjaxControl)this).AjaxControl.RegisterDefaultSkinStyleSheetFromResource(typeof(ControlPicker), Constants.DefaultSkinResource);

            // name of the default skin;
            CssClass = Constants.DefaultSkinCssClass;
        }

        bool ISkinControl.Enabled
        {
            get { return string.IsNullOrEmpty(CssClass) || CssClass.Equals(Constants.DefaultSkinCssClass); }
        }

        #endregion

        #region [ -- IPostBackDataHandler implementation -- ]

        /// <summary>
        /// Processes the postback data for the <see cref="ControlPicker"/> control.
        /// </summary>
        /// <param name="postDataKey">The index within the posted collection that references the content to load.</param>
        /// <param name="postCollection">The collection posted to the server.</param>
        /// <returns>True if the posted content is different from the last posting; otherwise, false.</returns>
        protected virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            var postArgument = postCollection[postDataKey];
            
            var hasPostArgument = postArgument != null;
            IsMouseOverItem = hasPostArgument && postArgument.StartsWith("M");

            if (hasPostArgument && postArgument.StartsWith("S"))
            {
                int index;
                var argument = postArgument.Substring(1);

                if (int.TryParse(argument, out index) && index >= 0 && index < Controls.Count)
                {
                    var wasSelected = IndexIsSelected(index);
                    var selection = new List<int>(SelectedIndexes);

                    if (SelectionMode == ItemSelectionMode.Single)
                        selection.Clear();

                    if (wasSelected)
                        selection.Remove(index);
                    else
                        selection.Add(index);

                    SelectedIndexes = selection.ToArray();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Invokes the <see cref="OnSelectedIndexChanged"/> method when the posted data for the <see cref="ControlPicker"/> control has changed.
        /// </summary>
        protected virtual void RaisePostDataChangedEvent()
        {
            OnSelectedIndexChanged();
        }

        /// <summary>
        /// Raises <see cref="SelectedIndexChanged"/> event.
        /// </summary>
        protected virtual void OnSelectedIndexChanged()
        {
            if (SelectedIndexChanged == null) return;
            SelectedIndexChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// When implemented by a class, processes postback data for an ASP.NET server control.
        /// </summary>
        /// <returns>
        /// True if the server control's state changes as a result of the postback; otherwise, false.
        /// </returns>
        /// <param name="postDataKey">The key identifier for the control.</param>
        /// <param name="postCollection">The collection of all incoming name values.</param>
        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            return LoadPostData(postDataKey, postCollection);
        }

        /// <summary>
        /// When implemented by a class, signals the server control to notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            RaisePostDataChangedEvent();
        }

        #endregion
    }
}
