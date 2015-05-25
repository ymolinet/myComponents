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
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    using HtmlFormatting;

    /// <summary>
    /// Validates whether the value of an associated input control matches the pattern specified by a regular expression.
    /// </summary>
    [ToolboxBitmap(typeof(CustomValidator), "Resources.Gaia.WebWidgets.CustomValidator.bmp")]
    public class CustomValidator : ASP.CustomValidator, IAjaxControl, IAjaxValidator
    {
        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for <see cref="CustomValidator"/> control.
        /// </summary>
        public class PropertyStateManagerCustomValidator : PropertyStateManagerBaseValidator
        {
            private bool _validateEmptyText;
            private string _clientFunction;
            private readonly CustomValidator _owner;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerBaseValidator"/> for specified <paramref name="validator"/>.
            /// </summary>
            /// <param name="validator">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="validator"/> is null.</exception>
            public PropertyStateManagerCustomValidator(CustomValidator validator) : base(validator.BaseValidator)
            {
                _owner = validator;
            }

            /// <summary>
            /// Returns true if the associated validator needs to be rerendered.
            /// </summary>
            protected override bool RequiresRerendering
            {
                get
                {
                    return base.RequiresRerendering ||
                           ValueHasChanged(_validateEmptyText, _owner.ValidateEmptyText) ||
                           ValueHasChanged(_clientFunction, _owner.ClientValidationFunction);
                }
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);

                var stateManager = source as PropertyStateManagerCustomValidator;
                if (stateManager == null) return;

                _clientFunction = stateManager._clientFunction;
                _validateEmptyText = stateManager._validateEmptyText;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _validateEmptyText = _owner.ValidateEmptyText;
                _clientFunction = _owner.ClientValidationFunction;

                base.TakeSnapshot();
            }
        }

        #region [ -- Private members -- ]

        private AjaxControl _instance;
        private AjaxControl _ajaxControl;
        private BaseValidator _baseValidator;

        #endregion

        #region [ -- Overridden base class methods and properties -- ]

        /// <summary>
        /// See <see cref="Gaia.WebWidgets.AjaxControl.OnInit" /> for documentation of this method
        /// </summary>
        /// <param name="e">The EventArgs passed on from the System</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AjaxControl.OnInit();
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// Forwards to <see cref="Gaia.WebWidgets.AjaxControl.TrackViewState" /> method.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            AjaxControl.TrackViewState();
        }

        /// <summary>Loads the previously saved state for the control.</summary>
        /// <param name="savedState">An object that contains the saved view state values for the control. </param>
        /// <seealso cref="WebWidgets.AjaxControl.EndLoadViewState"/>
        /// <seealso cref="Gaia.WebWidgets.AjaxControl.BeginLoadViewState"/>
        protected override void LoadViewState(object savedState)
        {
            var state = AjaxControl.BeginLoadViewState(savedState);
            state = BaseValidator.LoadViewState(state);
            base.LoadViewState(state);
            AjaxControl.EndLoadViewState();
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.BeginLoadControlState" /> and <see cref="WebWidgets.AjaxControl.EndLoadControlState" /> methods for documentation.
        /// This method only forwards to those methods.
        /// </summary>
        /// <param name="savedState">Saved control state</param>
        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(AjaxControl.BeginLoadControlState(savedState));
            AjaxControl.EndLoadControlState();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            using (BaseValidator.OnPreRender())
            {
                base.OnPreRender(e);
            }

            AjaxControl.OnPreRender();
        }

        /// <summary>
        /// Saves any state that was modified after the <see cref="Control.TrackViewState"/> method was invoked.
        /// </summary>
        /// <returns>
        /// An object that contains the current view state of the control; otherwise, if there is no view state associated with the control, null.
        /// </returns>
        protected override object SaveViewState()
        {
            return AjaxControl.SaveViewState(BaseValidator.SaveViewState(base.SaveViewState()));
        }

        /// <summary>
        /// See <see cref="WebWidgets.AjaxControl.SaveControlState" /> method for documentation. This method only forward to that method.
        /// </summary>
        /// <returns>Control state to save</returns>
        protected override object SaveControlState()
        {
            return AjaxControl.SaveControlState(base.SaveControlState());
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content. </param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            BaseValidator.BeginRenderValidator(writer);
            AjaxControl.RenderControl(writer);
            BaseValidator.EndRenderValidator(writer);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
        /// Forwards to <see cref="WebWidgets.AjaxControl.OnUnload"/> method.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data. </param>
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            AjaxControl.OnUnload();
        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected virtual void IncludeScriptFiles()
        {
            BaseValidator.IncludeScriptFiles();
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(XhtmlTagFactory create)
        {
            BaseValidator.RenderControlHtml(create);
        }

        #endregion

        #region [ -- IAjaxControl Implementation -- ]

        AjaxControl IAjaxControl.AjaxControl
        {
            get { return _instance ?? (_instance = new AjaxControl(this)); }
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
            return new PropertyStateManagerCustomValidator(this);
        }

        string IAjaxControl.GetScript()
        {
            return BaseValidator.GetScript().ToString();
        }

        void IAjaxControl.IncludeScriptFiles()
        {
            IncludeScriptFiles();
        }

        bool IAjaxControl.InDesigner
        {
            get { return DesignMode; }
        }

        /// <summary>
        /// Calls base class RenderControl method
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use to create XHTML</param>
        void IAjaxControl.RenderControlHtml(XhtmlTagFactory create)
        {
            RenderControlHtml(create);
        }

        PropertyStateManagerControl IAjaxControl.StateManager
        {
            get { return AjaxControl.StateManager; }
        }

        private AjaxControl AjaxControl
        {
            get { return _ajaxControl ?? (_ajaxControl = ((IAjaxControl)this).AjaxControl); }
        }

        #endregion

        #region [ -- IAjaxValidator implementation -- ]

        /// <summary>
        /// Gets a value that indicates whether the control specified by the <see cref="ASP.BaseValidator.ControlToValidate"/> property is a valid control.
        /// </summary>
        bool IAjaxValidator.PropertiesValid
        {
            get { return PropertiesValid; }
        }

        /// <summary>
        /// Gets a value indicating whether the control is enabled.
        /// </summary>
        bool IAjaxValidator.IsEnabled
        {
            get { return IsEnabled; }
        }

        /// <summary>
        /// Gets a value denoting if the client browser supports "upward" rendering.
        /// </summary>
        bool IAjaxValidator.RenderUplevel
        {
            get { return RenderUplevel; }
        }

        /// <summary>
        /// Determines whether the validation control can perform client-side validation.
        /// </summary>
        bool IAjaxValidator.DetermineRenderUplevel()
        {
            return DetermineRenderUplevel();
        }

        /// <summary>
        /// Registers an ECMAScript array declaration using the array name Page_Validators.
        /// </summary>
        void IAjaxValidator.RegisterValidatorDeclaration()
        {
            base.RegisterValidatorDeclaration();
        }

        /// <summary>
        /// Gets the client ID of the control having specified id.
        /// </summary>
        /// <param name="controlToValidate">The name of the control to get the client ID from. </param>
        /// <returns>The client ID of the specified control.</returns>
        string IAjaxValidator.GetControlRenderId(string controlToValidate)
        {
            return GetControlRenderID(controlToValidate);
        }

        /// <summary>
        /// Renders the validator to the specified HTML <paramref name="writer"/> during design time.
        /// </summary>
        void IAjaxValidator.RenderDesignTime(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        #endregion

        /// <summary>
        /// Returns implementation.
        /// </summary>
        private BaseValidator BaseValidator
        {
            get { return _baseValidator ?? (_baseValidator = new BaseValidator(this)); }
        }
    }
}
