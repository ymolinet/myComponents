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
using System.Globalization;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

[assembly: WebResource("Gaia.WebWidgets.Scripts.ImageMap.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    using HtmlFormatting;
    
    /// <summary>
    /// The Gaia Ajax Image displays an image on a page. When a hot spot region defined within the ImageMap control is clicked,
    /// the control either generates an Ajax callback (postback) to the server or navigates to a specified URL. It inherits from
    /// the <a href="http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.imagemap.aspx"> ASP.NET ImageMap</a>, and has built-in Ajax behaviour.
    /// </summary>
    [DefaultProperty("HotSpots"), ParseChildren(true, "HotSpots")]
    [ToolboxBitmap(typeof(ImageMap), "Resources.Gaia.WebWidgets.Image.bmp")]
    public class ImageMap : ASP.ImageMap, IAspectableAjaxControl
    {
        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="ImageMap"/>.
        /// </summary>
        public class PropertyStateManagerImageMap : PropertyStateManagerWebControl
        {
            private static readonly Reflection.IGetter<ASP.HotSpot, string> MarkupNameGetter = Reflection.Property<ASP.HotSpot, string>("MarkupName");

            /// <summary>
            /// Keeps state of the <see cref="ASP.HotSpot"/>.
            /// </summary>
            private class HotSpotState : ASP.HotSpot
            {
                private readonly string _coordinates;

                /// <summary>
                /// Initializes new instance of <see cref="HotSpotState"/> with the specified <paramref name="coordinates"/>.
                /// </summary>
                public HotSpotState(string coordinates)
                {
                    _coordinates = coordinates;
                }

                /// <summary>
                /// When overridden in a derived class, returns a string that represents the coordinates of the <see cref="T:System.Web.UI.WebControls.HotSpot"/> region.
                /// </summary>
                /// <returns>
                /// A string that represents the coordinates of the <see cref="T:System.Web.UI.WebControls.HotSpot"/> region.
                /// </returns>
                public override string GetCoordinates()
                {
                    return _coordinates;
                }

                /// <summary>
                /// When overridden in a derived class, gets the string representation for the <see cref="T:System.Web.UI.WebControls.HotSpot"/> object's shape.
                /// </summary>
                /// <returns>
                /// A string that represents the name of the <see cref="T:System.Web.UI.WebControls.HotSpot"/> object's shape.
                /// </returns>
                protected override string MarkupName
                {
                    get { throw new NotSupportedException(); }
                }

                /// <summary>
                /// Gets or sets the shape of the <see cref="ASP.HotSpot"/>.
                /// </summary>
                public string Shape { get; set; }
            }

            private List<HotSpotState> _state;
            private readonly ASP.ImageMap _imageMap;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerImageMap"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerImageMap(ImageMap control) : this(control, control.ClientID, null) { }

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerImageMap"/> for specified <paramref name="control"/>
            /// using specified <paramref name="clientId"/> for reference and specified <see cref="IExtraPropertyCallbackRenderer"/>
            /// for additional state change serialization.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <param name="clientId">The client-side ID of the <paramref name="control"/> to use.</param>
            /// <param name="extra">Provides additional state change rendering during callbacks.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerImageMap(ImageMap control, string clientId, IExtraPropertyCallbackRenderer extra)
                : base(control, clientId, extra)
            {
                _imageMap = control;
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerImageMap"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);

                var stateManager = source as PropertyStateManagerImageMap;
                if (stateManager == null) return;

                _state = stateManager._state;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                var collection = _imageMap.HotSpots;
                _state = new List<HotSpotState>(collection.Count);

                foreach (ASP.HotSpot item in collection)
                {
                    var clone = new HotSpotState(item.GetCoordinates())
                                    {
                                        Shape = GetShape(item),
                                        TabIndex = item.TabIndex,
                                        AccessKey = item.AccessKey,
                                        NavigateUrl = item.NavigateUrl,
                                        Target = GetActiveTarget(item),
                                        HotSpotMode = GetActiveMode(item),
                                        AlternateText = item.AlternateText
                                    };

                    _state.Add(clone);
                }

                base.TakeSnapshot();
            }

            /// <summary>
            /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void DiffSnapshot()
            {
                DiffRender();
                base.DiffSnapshot();
            }

            private void DiffRender()
            {
                var initialCollection = _state;
                var currentCollection = _imageMap.HotSpots;
                var initialCollectionCount = initialCollection.Count;
                var currentCollectionCount = currentCollection.Count;

                if (currentCollectionCount == 0)
                {
                    if (initialCollectionCount > 0)
                        Builder.Append(".clear()");

                    return;
                }

                var min = Math.Min(currentCollectionCount, initialCollectionCount);

                for (var index = 0; index < min; ++index)
                {
                    var current = currentCollection[index];
                    var initial = initialCollection[index];

                    // list item was possibly changed
                    RenderChange(index, current, initial);
                }

                var deletions = 0;
                var additions = 0;

                // check if new items added
                if (currentCollectionCount > initialCollectionCount)
                {
                    for (var index = min; index < currentCollectionCount; ++index)
                    {
                        var current = currentCollection[index];
                        RenderAddition(current);
                        ++additions;
                    }
                }

                // check if initial items removed
                if (currentCollectionCount >= initialCollectionCount) return;
                for (var index = min; index < initialCollectionCount; ++index)
                {
                    var position = index + additions - deletions;
                    RenderDeletion(position);
                    ++deletions;
                }
            }

            private void RenderAddition(ASP.HotSpot current)
            {
                var additions = new List<string>(8 /* property count */);

                AddAddition(additions, "ak", current.AccessKey);
                AddAddition(additions, "at", current.AlternateText);
                AddAddition(additions, "cd", current.GetCoordinates());
                AddAddition(additions, "mn", GetShape(current));
                AddAddition(additions, "nu", _imageMap.ResolveClientUrl(current.NavigateUrl));
                AddAddition(additions, "tg", GetActiveTarget(current));

                string mode;
                switch (GetActiveMode(current))
                {
                    case ASP.HotSpotMode.Inactive: mode = "1"; break;
                    case ASP.HotSpotMode.PostBack: mode = "2"; break;
                    default: mode = null; break;
                }

                AddAddition(additions, "md", mode);

                var tabIndex = current.TabIndex;
                if (tabIndex != 0)
                    AddAddition(additions, "ti", tabIndex.ToString(NumberFormatInfo.InvariantInfo));

                Builder.AppendFormat(".add({{{0}}})", string.Join(",", additions.ToArray()));
            }

            private void RenderDeletion(int position)
            {
                Builder.Append(string.Concat(".remove(", position, ")"));
            }

            private void RenderChange(int position, ASP.HotSpot current, HotSpotState initial)
            {
                var changes = new List<string>(8 /* property count */);

                AddChange(changes, "ak", current.AccessKey, initial.AccessKey);
                AddChange(changes, "at", current.AlternateText, initial.AlternateText);
                AddChange(changes, "cd", current.GetCoordinates(), initial.GetCoordinates());
                AddChange(changes, "mn", GetShape(current), initial.Shape);
                AddChange(changes, "nu", _imageMap.ResolveClientUrl(current.NavigateUrl), _imageMap.ResolveClientUrl(initial.NavigateUrl));
                AddChange(changes, "ti", current.TabIndex.ToString(NumberFormatInfo.InvariantInfo), initial.TabIndex.ToString(NumberFormatInfo.InvariantInfo));
                AddChange(changes, "tg", GetActiveTarget(current), GetActiveTarget(initial));

                var mode = GetActiveMode(current);
                if (changes.Count > 0 || mode != initial.HotSpotMode)
                {
                    string modeString = null;

                    switch (mode)
                    {
                        case ASP.HotSpotMode.Inactive: modeString = "1"; break;
                        case ASP.HotSpotMode.PostBack: modeString = "2"; break;
                    }

                    if (modeString != null)
                        AddKeyValue(changes, "md", modeString);
                }

                if (changes.Count == 0) return;
                Builder.AppendFormat(".change({0},{{{1}}})", position, string.Join(",", changes.ToArray()));
            }

            private static void AddChange(List<string> changes, string key, string currentValue, string initialValue)
            {
                if (currentValue == initialValue) return;
                AddKeyValue(changes, key, currentValue);
            }

            private static void AddKeyValue(List<string> list, string key, string value)
            {
                list.Add(key + ":'" + HtmlFormatter.FormatHtmlForInnerHTML(value) + "'");
            }

            private static void AddAddition(List<string> additions, string key, string currentValue)
            {
                if (string.IsNullOrEmpty(currentValue)) return;
                AddKeyValue(additions, key, currentValue);
            }

            private ASP.HotSpotMode GetActiveMode(ASP.HotSpot hotSpot)
            {
                var mode = hotSpot.HotSpotMode;
                if (mode != ASP.HotSpotMode.NotSet)
                    return mode;

                var mapMode = _imageMap.HotSpotMode;
                return mapMode == ASP.HotSpotMode.NotSet ? ASP.HotSpotMode.Navigate : mapMode;
            }

            private string GetActiveTarget(ASP.HotSpot hotSpot)
            {
                var target = hotSpot.Target;
                return string.IsNullOrEmpty(target) ? _imageMap.Target : target;
            }

            private static string GetShape(ASP.HotSpot hotSpot)
            {
                return MarkupNameGetter.GetValue(hotSpot);
            }
        }

        #region [ -- Private Members -- ]

        private AspectableAjaxControl _instance;
        private AspectableAjaxControl _aspectableAjaxControl;
        private EffectControl _effectControl;

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
        /// Gets or sets the image URL for the control
        /// </summary>
        [AjaxSerializable("setImageUrl")]
        public override string ImageUrl
        {
            get { return base.ImageUrl; }
            set { base.ImageUrl = value; }
        }

        /// <summary>
        /// Sets the ALT text for the image
        /// </summary>
        [AjaxSerializable("setAlternateText")]
        public override string AlternateText
        {
            get { return base.AlternateText; }
            set { base.AlternateText = value; }
        }

        ///<summary>Gets or sets the location to a detailed description for the image.</summary>
        ///<value>
        ///The URL for the file that contains a detailed description for the image.
        ///The default is an empty string ("").
        ///</value>   
        [AjaxSerializable("setLongDesc")]
        public override string DescriptionUrl
        {
            get { return base.DescriptionUrl; }
            set { base.DescriptionUrl = value; }
        }

        ///<summary>
        /// Gets or sets the alignment of the System.Web.UI.WebControls.Image control
        /// in relation to other elements on the Web page.
        ///</summary>
        ///<value>One of the System.Web.UI.WebControls.ImageAlign values. The default is NotSet.</value>
        ///<exception cref="System.ArgumentOutOfRangeException">
        ///The specified value is not one of the System.Web.UI.WebControls.ImageAlign values.
        ///</exception>
        [AjaxSerializable("setImgAlign")]
        public override System.Web.UI.WebControls.ImageAlign ImageAlign
        {
            get { return base.ImageAlign; }
            set { base.ImageAlign = value; }
        }

        /// <summary>
        /// <see cref="IAspectableAjaxControl.Aspects"></see> for documentation for this method
        /// </summary>
        [Browsable(false)]
        public AspectCollection Aspects
        {
            get { return AspectableAjaxControl.Aspects; }
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

        #region [ -- Overridden base class methods and constructor -- ]

        /// <summary>
        /// See <see cref="AjaxControl.OnInit" /> for documentation of this method
        /// </summary>
        /// <param name="e">The EventArgs passed on from the System</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AspectableAjaxControl.OnInit();
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// Forwards to <see cref="AjaxControl.TrackViewState" /> method.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            AspectableAjaxControl.TrackViewState();
        }

        /// <summary>
        /// See <see cref="AjaxControl.BeginLoadViewState" /> and <see cref="AjaxControl.EndLoadViewState" /> methods for documentation. 
        /// This method only forwards to those methods.
        /// </summary>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(AspectableAjaxControl.BeginLoadViewState(savedState));
            AspectableAjaxControl.EndLoadViewState();
        }

        /// <summary>
        /// See <see cref="AjaxControl.BeginLoadControlState" /> and <see cref="AjaxControl.EndLoadControlState" /> methods for documentation.
        /// This method only forwards to those methods.
        /// </summary>
        /// <param name="savedState">Saved control state</param>
        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(AspectableAjaxControl.BeginLoadControlState(savedState));
            AspectableAjaxControl.EndLoadControlState();
        }

        /// <summary>
        /// See <see cref="AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AspectableAjaxControl.OnPreRender();
            EffectControl.OnPreRender();
        }

        /// <summary>
        /// See <see cref="AjaxControl.SaveViewState" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <returns>the saved ViewState</returns>
        protected override object SaveViewState()
        {
            return AspectableAjaxControl.SaveViewState(base.SaveViewState());
        }

        /// <summary>
        /// See <see cref="AjaxControl.SaveControlState" /> method for documentation. This method only forward to that method.
        /// </summary>
        /// <returns>Control state to save</returns>
        protected override object SaveControlState()
        {
            return AspectableAjaxControl.SaveControlState(base.SaveControlState());
        }

        /// <summary>
        /// Rendering
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to write the control into</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            AspectableAjaxControl.RenderControl(writer);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
        /// Forwards to <see cref="AjaxControl.OnUnload"/> method.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data. </param>
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            AspectableAjaxControl.OnUnload();
        }

        #endregion

        #region [ -- Protected Methods for Inheritance -- ]

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected virtual void IncludeScriptFiles()
        {
            // Include Image Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Image.js", typeof(Manager), "Gaia.Image.browserFinishedLoading", true);
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.ImageMap.js", typeof(Manager), "Gaia.ImageMap.browserFinishedLoading", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(XhtmlTagFactory create)
        {
            create.GetHtmlTextWriter().Write(ComposeXhtml.ToString(delegate(System.IO.TextWriter textWriter)
                {
                    using (HtmlTextWriter htmlTextWriter = new ImageMapHtmlWriter(textWriter))
                    {
                        BaseRenderControl(htmlTextWriter);
                    }
                }));
        }

        private void BaseRenderControl(HtmlTextWriter htmlTextWriter)
        {
            base.Render(htmlTextWriter);
        }

        #endregion

        #region [ -- IAjaxControl Implementation -- ]

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new AspectableAjaxControl(this)); }
        }

        Control IAjaxControl.Control
        {
            get { return this; }
        }

        string IAjaxControl.TagName
        {
            get { return TagName; }
        }

        PropertyStateManagerControl IAjaxControl.CreateControlStateManager()
        {
            return new PropertyStateManagerImageMap(this);
        }

        string IAjaxControl.GetScript()
        {
            var hotSpots = GetPostBackHotSpotIndexCollection();

            return new RegisterControl("Gaia.ImageMap", ClientID)
                .AddAspects(Aspects).AddEffects(Effects)
                .AddPropertyIfTrue(hotSpots.Count > 0, "spots", string.Join(",", hotSpots.ToArray())).
                ToString();
        }

        private List<string> GetPostBackHotSpotIndexCollection()
        {
            var hotSpots = new List<string>();
            var collection = HotSpots;

            var count = collection.Count;
            for (var index = 0; index < count; ++index)
            {
                if (collection[index].HotSpotMode == ASP.HotSpotMode.PostBack ||
                    (collection[index].HotSpotMode == ASP.HotSpotMode.NotSet && HotSpotMode == ASP.HotSpotMode.PostBack))
                {
                    hotSpots.Add(index.ToString(NumberFormatInfo.InvariantInfo));
                }
            }

            return hotSpots;
        }

        void IAjaxControl.IncludeScriptFiles()
        {
            IncludeScriptFiles();
        }

        void IAjaxControl.RenderControlHtml(XhtmlTagFactory create)
        {
            RenderControlHtml(create);
        }

        bool IAjaxControl.InDesigner
        {
            get { return DesignMode; }
        }

        PropertyStateManagerControl IAjaxControl.StateManager
        {
            get { return AspectableAjaxControl.StateManager; }
        }

        #endregion

        #region [ -- IAspectableAjaxControl Implementation -- ]

        AspectableAjaxControl IAspectableAjaxControl.AspectableAjaxControl
        {
            get { return (AspectableAjaxControl)((IAspectableAjaxControl)this).AjaxControl; }
        }

        private AspectableAjaxControl AspectableAjaxControl
        {
            get { return _aspectableAjaxControl ?? (_aspectableAjaxControl = ((IAspectableAjaxControl)this).AspectableAjaxControl); }
        }

        #endregion
    }
}
