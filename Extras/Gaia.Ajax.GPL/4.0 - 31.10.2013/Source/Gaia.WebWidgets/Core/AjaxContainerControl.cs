/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.IO;
using System.Web.UI;
using ASP = System.Web.UI;
using System.Collections.Generic;

namespace Gaia.WebWidgets
{
    using HtmlFormatting;

    /// <summary>
    /// Common implementation for Controls implementing IAjaxContainerControl interface. Usage is to implement
    /// IAjaxContainerControl and forward every method in that interface to this class. 
    /// All AjaxContainer Controls supports partial rendering by supporting the ForceAnUpdate method and
    /// related method TrackControlAdditions which means that the user can only render new controls that are 
    /// appended on the control when needed. Examples of controls using this class is Panel, HybridPanelBase and MultiView
    /// </summary>
    /// <seealso cref="IAjaxContainerControl"/>
    /// <seealso cref="AjaxControl"/>
    /// <seealso cref="IAjaxControl"/>
    /// <seealso cref="AspectableAjaxControl"/>
    /// <seealso cref="IAspectableAjaxControl"/>
    public class AjaxContainerControl : AspectableAjaxControl
    {
        #region [ -- Private Members -- ]

        private bool _forceAnUpdate;
        private List<Control> _initialControls;
        private readonly IAjaxContainerControl _ajaxContainerControl;

        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control">Control which owns this instance</param>
        public AjaxContainerControl(IAjaxContainerControl control)
            : base(control)
        {
            _ajaxContainerControl = control;
        }

        #endregion

        #region [ -- Public Methods -- ]

        /// <summary>
        /// See <see cref="IAjaxContainerControl.ForceAnUpdate "/> for documentation on this method
        /// </summary>
        public void ForceAnUpdate()
        {
            if (!Manager.Instance.IsAjaxCallback)
                return;

            _forceAnUpdate = true;
        }

        /// <summary>
        /// See <see cref="IAjaxContainerControl.TrackControlAdditions" /> for documentation on this method
        /// </summary>
        public void ForceAnUpdateWithAppending()
        {
            ForceAnUpdate();
        }

        /// <summary>
        /// Call this function to start tracking control additions. All controls added to the controls collection
        /// after this will be rendered during ForceAnUpdate() or ForceAnUpdateWithAppending(). This is very useful
        /// when you are just adding new content like for example new TreeViewItems in the TreeView or adding new
        /// stuff in for example AspectScrollable. 
        /// </summary>
        public void TrackControlAddition()
        {
            IsTrackingControlAdditions = true;

            var controls = _ajaxContainerControl.Control.Controls;
            _initialControls = new List<Control>(controls.Count);
            foreach(Control ctrl in controls)
                _initialControls.Add(ctrl);
        }

        /// <summary>
        /// Returns true if the Container Control is tracking control additions. See TrackControlAddition() for 
        /// more information. 
        /// </summary>
        public bool IsTrackingControlAdditions { get; private set; }

        #endregion

        #region [ -- Overridden Base Methods -- ]

        /// <summary>
        /// Overridden to ensure inclusion of common JavaScript files
        /// </summary>
        public override void OnPreRender()
        {
            base.OnPreRender();

            if (IsTrackingControlAdditions)
            {
                foreach (Control ctrl in _ajaxContainerControl.Control.Controls)
                {
                    var ajaxControl = ctrl as IAjaxControl;
                    if (ajaxControl == null) continue;
                    if (!_initialControls.Contains(ctrl))
                        ajaxControl.AjaxControl.ShouldRender = true;
                }
            }

            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Container.js", typeof(Manager), "Gaia.Container.browserFinishedLoading", true);
        }

        /// <summary>
        /// Used by derived controls to know if the control should be rendered or not
        /// based on the facts if the ShouldRender was set or control was previously rendered
        /// </summary>
        internal protected override bool ShouldRenderAjaxControl 
        {
            get { return base.ShouldRenderAjaxControl || CanDoForceAnUpdate; }
        }

        private bool CanDoForceAnUpdate
        {
            get { return Rendered && _forceAnUpdate && _ajaxContainerControl.Control.Visible; }
        }

        /// <summary>
        /// When the ajax control is replaced, this method is called to check
        /// if the move script needs to serialized, or if it is handled by the replacement script
        /// </summary>
        protected override bool DetermineRequiresMoveSerialization()
        {
            return base.DetermineRequiresMoveSerialization() || !base.ShouldRenderAjaxControl;
        }

        /// <summary>
        /// Called when the control is being rerendered during ajax callback.
        /// </summary>
        protected override void RenderControlFirstTimeOnAjaxCallback()
        {
            Rerendered = true;
            if (!base.ShouldRenderAjaxControl && CanDoForceAnUpdate)
                RenderCallbackOnForceAnUpdate();
            else
                RenderCallbackOnNotForceAnUpdate();
        }

        /// <summary>
        /// Called during ajax callback to render control state changes.
        /// </summary>
        protected override void RenderControlChangesOnAjaxCallback()
        {
            var visible = _ajaxContainerControl.Control.Visible;
            if (visible) Manager.PushWriter();

            base.RenderControlChangesOnAjaxCallback();

            if (!visible) return;
            var rerender = CanDoForceAnUpdate;
            if (rerender) Manager.ClearWriter();
            Manager.PopWriter();
            if (!rerender) return;
            RenderControlFirstTimeOnAjaxCallback();
        }

        /// <summary>
        /// Called when container control is being rerendered, but ForceAnUpdate was not called.
        /// </summary>
        protected virtual void RenderCallbackOnNotForceAnUpdate()
        {
            base.RenderControlFirstTimeOnAjaxCallback();
        }

        /// <summary>
        /// Called when container control is being rerendered and ForceAnUpdate was called.
        /// </summary>
        protected void RenderCallbackOnForceAnUpdate()
        {
            TextWriter writer = null;
            using (new AtomicInvoker(() => writer = Manager.PushWriter(), () => Manager.Instance.PopWriter()))
            {
                string requirements = null;
                var childMarkup = ComposeXhtml.ToString(
                    delegate(TextWriter stream)
                    {
                        var childWriter = IsTrackingControlAdditions
                                              ? new ContainerHtmlTextWriterPartialForce(stream)
                                              : (ContainerHtmlTextWriterBase)
                                                new ContainerHtmlTextWriter(stream);

                        _ajaxContainerControl.RenderChildrenOnForceAnUpdate(new XhtmlTagFactory(childWriter));
                        requirements = childWriter.ForceAnUpdateRequirements;
                    });

                // In addition we render the changes to the Gaia container control too...
                // (style and attribute stuff etc...!)
                _ajaxContainerControl.StateManager.RenderChanges(Manager.Writer);

                var operation = IsTrackingControlAdditions ?
                    "$G('{0}').appendHtml('{1}');" :
                    "$G('{0}').setContent('{1}');";

                var actions = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                            operation, ClientReference,
                                            HtmlFormatter.FormatHtmlForInnerHTML(childMarkup));

                if (requirements != null)
                    writer.Write(requirements);

                if (!IsTrackingControlAdditions)
                    writer.Write(@"$G('{0}').forceAnUpdate();", ClientReference);

                writer.Write(actions);

                if (!IsTrackingControlAdditions)
                    writer.Write(@"$G('{0}').reInit();", ClientReference);
            }
        }

        /// <summary>
        /// Called when the control for some reason (was invisible previously e.g.) should render for the "first" time
        /// but it is still an Ajax Callback
        /// </summary>
        /// <returns>Value to return back to client for initializing control</returns>
        protected override string GetMarkup()
        {
            return ComposeXhtml.ToString(delegate(TextWriter stream)
            {
                var create = IsTrackingControlAdditions ?
                    new XhtmlTagFactory(new ContainerHtmlTextWriterPartialForce(stream)) :
                    new XhtmlTagFactory(new ContainerHtmlTextWriter(stream));

                _ajaxContainerControl.RenderControlHtml(create);
            });
        }

        #endregion
    }
}
