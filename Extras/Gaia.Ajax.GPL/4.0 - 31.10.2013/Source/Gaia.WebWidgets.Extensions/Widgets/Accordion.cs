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
using System.Drawing.Design;
using System.ComponentModel;

namespace Gaia.WebWidgets.Extensions
{
    using HtmlFormatting;

    /// <summary>
    /// The Accordion is a container which groups <see cref="ExtendedPanel"/> controls so that only one can be visible at one time if required.
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for Accordion" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Extensions\Accordion\Overview\Default.aspx"/>
    /// </code> 
    /// </example>
    [Themeable(true)]
    [ParseChildren(typeof(ExtendedPanel))]
    [ToolboxBitmap(typeof(Accordion), "Resources.Gaia.WebWidgets.Extensions.Accordion.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.AccordionDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class Accordion : Panel
    {
        /// <summary>
        /// Raised when <see cref="Accordion"/> is toggled.
        /// </summary>
        public event EventHandler Toggled;

        #region [ -- Properties -- ]

        /// <summary>
        /// Duration of animation when shifting from expanded to collapsed state.
        /// </summary>
        [Bindable(true)]
        [DefaultValue(0)]
        [Category("Behavior")]
        [Description("Duration of collapse or expand animation.")]
        public int AnimationDuration
        {
            get { return StateUtil.Get(ViewState, "AnimationDuration", 0); }
            set { StateUtil.Set(ViewState, "AnimationDuration", value, 0); }
        }

        /// <summary>
        /// Gets or sets if only one <see cref="ExtendedPanel"/> may be visible at once.
        /// Default value is true.
        /// </summary>
        /// <remarks>
        /// If true <see cref="Accordion"/> will always have one open view even though you programatically close all yourself. 
        /// If false then you are in total control, but also this requires more gluing work on your behalf.
        /// </remarks>
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Forces only one ExtendedPanel to be expanded.")]
        public bool ForceOnlyOne
        {
            get { return StateUtil.Get(ViewState, "ForceOnlyOne", true); }
            set { StateUtil.Set(ViewState, "ForceOnlyOne", value, true); }
        }

        #endregion

        #region [ -- View Collection -- ]

        /// <summary>
        /// The collection of <see cref="ExtendedPanel"/> controls.
        /// </summary>
        [Browsable(false)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("Gaia.WebWidgets.Extensions.Design.AccordionViewCollectionEditor, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a", typeof(UITypeEditor))]
        public ExtendedPanelCollection ExtendedPanels
        {
            get { return (ExtendedPanelCollection) Controls; }
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Notifies the server control that an element, either XML or HTML, was parsed, and adds the element to the server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object"/> that represents the parsed element. </param>
        protected override void AddParsedSubObject(object obj)
        {
            if (obj is ExtendedPanel)
                base.AddParsedSubObject(obj);
            else if (!(obj is LiteralControl))
                throw new HttpException("Accordion cannot have children of type " + obj.GetType().Name);
        }

        /// <summary>
        /// Called after a child control is added to the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"/> that has been added.</param>
        /// <param name="index">The index of the control in the <see cref="P:System.Web.UI.Control.Controls"/> collection.</param>
        /// <exception cref="T:System.InvalidOperationException"><paramref name="control"/> is a <see cref="T:System.Web.UI.WebControls.Substitution"/>  control.</exception>
        protected override void AddedControl(Control control, int index)
        {
            var view = (ExtendedPanel) control;
            view.Toggled += PanelToggled;
            base.AddedControl(control, index);
        }

        /// <summary>
        /// Called after a child control is removed from the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"/> that has been removed. </param>
        /// <exception cref="T:System.InvalidOperationException">The control is a <see cref="T:System.Web.UI.WebControls.Substitution"/> control.</exception>
        protected override void RemovedControl(Control control)
        {
            base.RemovedControl(control);
            var view = (ExtendedPanel)control;
            view.Toggled -= PanelToggled;
        }

        /// <summary>
        /// Creates a new <see cref="T:System.Web.UI.ControlCollection"/> object to hold the child controls (both literal and server) of the server control.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.UI.ControlCollection"/> object to contain the current server control's child server controls.
        /// </returns>
        protected override ControlCollection CreateControlCollection()
        {
            return new ExtendedPanelCollection(this);
        }

        /// <summary>
        /// See also <see cref="AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            SetPreRenderDefaults();
            base.OnPreRender(e);
        }

        /// <summary>
        /// Initializes the <see cref="ExtendedPanel"/> with common properties and hooks the events for the toggling.
        /// </summary>
        protected virtual void SetPreRenderDefaults()
        {
            if (!ForceOnlyOne) return;

            var views = ExtendedPanels;
            ExtendedPanel expandedView = null;

            foreach (var view in views)
            {
                if (view.DependsOnOwnerProperties)
                    view.RequiresRecomposition();

                var isCollapsed = view.Collapsed;

                if (!isCollapsed && expandedView != null)
                {
                    isCollapsed = true;
                    view.Collapsed = true;
                }

                if (isCollapsed)
                {
                    if (!view.CanBeToggled)
                        view.CanBeToggled = true;
                }
                else
                    expandedView = view;
            }

            if (expandedView == null && views.Count > 0)
                expandedView = views[0];

            if (expandedView == null) return;

            if (expandedView.Collapsed)
                expandedView.Collapsed = false;

            if (expandedView.CanBeToggled)
                expandedView.CanBeToggled = false;
        }

        /// <summary>
        /// Renders HTML markup for the control into the specified <see cref="XhtmlTagFactory"/> instance.
        /// </summary>
        /// <param name="create">Used to create HTML markup for the control.</param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (var div = create.Div(ClientID, CssClass))
            {
                Css.SerializeAttributesAndStyles(this, div);
                RenderChildren(create.GetHtmlTextWriter());
            }
        }

        #endregion

        #region [ -- Toggled Handler -- ]

        /// <summary>
        /// Raises <see cref="Toggled"/> event and ensure only one view is open if <see cref="ForceOnlyOne"/> is set to True.
        /// </summary>
        private void PanelToggled(object sender, EventArgs e)
        {
            // We must check to see if this panel is Collapsed because if it is
            // it means this is the "second time" around due to the recursiveness
            // of the Event Handlers of Toggled for all ExtendedPanels inside of the Accordion
            var toggledPanel = (ExtendedPanel)sender;
            if (toggledPanel.Collapsed) return;

            if (ForceOnlyOne)
            {
                foreach (var view in ExtendedPanels)
                {
                    if (ReferenceEquals(view, toggledPanel) || view.Collapsed) continue;
                    view.Toggle();
                }
            }

            if (Toggled != null)
                Toggled(this, EventArgs.Empty);
        }

        #endregion
    }
}