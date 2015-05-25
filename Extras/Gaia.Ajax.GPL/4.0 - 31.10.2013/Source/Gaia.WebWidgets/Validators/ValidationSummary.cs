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
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    using HtmlFormatting;

    /// <summary>
    /// Displays a summary of all validation errors inline on a Web page.
    /// </summary>
    [ToolboxBitmap(typeof(ValidationSummary), "Resources.Gaia.WebWidgets.ValidationSummary.bmp")]
    public class ValidationSummary : ASP.ValidationSummary, IAjaxControl
    {
        /// <summary>
        /// Specialized <see cref="PropertyStateManagerControl"/> for the <see cref="ValidationSummary"/> control.
        /// </summary>
        public class PropertyStateManagerValidationSummary : PropertyStateManagerWebControl, IMayRequireRerendering
        {
            private ASP.ValidationSummary _state;
            private readonly ValidationSummary _owner;

            /// <summary>
            /// Initializes new instance of <see cref="PropertyStateManagerValidationSummary"/> for specified <paramref name="control"/>.
            /// </summary>
            /// <param name="control">Control to track changes for.</param>
            /// <remarks>
            /// Stores state information for the control and serializes changes during callback rendering.
            /// </remarks>
            /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
            public PropertyStateManagerValidationSummary(ValidationSummary control) : base(control)
            {
                _owner = control;
            }

            /// <summary>
            /// Returns true if the associated <see cref="ValidationSummary"/> needs rerendering.
            /// </summary>
            bool IMayRequireRerendering.RequiresRerendering
            {
                get
                {
                    return
                        ValueHasChanged(_state.Enabled, _owner.IsEnabled) ||
                        ValueHasChanged(_state.HeaderText, _owner.HeaderText) ||
                        ValueHasChanged(_state.DisplayMode, _owner.DisplayMode) ||
                        ValueHasChanged(_state.ShowSummary, _owner.ShowSummary) ||
                        ValueHasChanged(_state.ShowMessageBox, _owner.ShowMessageBox) ||
                        ValueHasChanged(_state.ValidationGroup, _owner.ValidationGroup) ||
                        ValueHasChanged(_owner._initialValidation, _owner._savedValidation) ||
                        ValueHasChanged(_state.EnableClientScript, _owner.EnableClientScript);
                }
            }

            /// <summary>
            /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
            /// </summary>
            /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
            protected override void AssignState(PropertyStateManagerControl source)
            {
                base.AssignState(source);

                var stateManager = source as PropertyStateManagerValidationSummary;
                if (stateManager == null) return;

                _state = stateManager._state;
            }

            /// <summary>
            /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
            /// </summary>
            protected override void TakeSnapshot()
            {
                _state = new ASP.ValidationSummary
                             {
                                 Enabled = _owner.IsEnabled,
                                 HeaderText = _owner.HeaderText,
                                 ShowSummary = _owner.ShowSummary,
                                 DisplayMode = _owner.DisplayMode,
                                 ShowMessageBox = _owner.ShowMessageBox,
                                 ValidationGroup = _owner.ValidationGroup,
                                 EnableClientScript = _owner.EnableClientScript
                             };
                
                base.TakeSnapshot();
            }

            /// <summary>
            /// Returns true if <paramref name="initial"/> value is equal to the <paramref name="current"/> value.
            /// </summary>
            private static bool ValueHasChanged<T>(T initial, T current)
            {
                return !EqualityComparer<T>.Default.Equals(initial, current);
            }
        }

        #region [ -- Private members -- ]

        private List<string> _errors;
        private AjaxControl _instance;
        private AjaxControl _ajaxControl;

        private int _savedValidation;
        private int _initialValidation;

        private bool _markupRendered;
        private bool _clientScriptUpdated;

        private static bool _isRunningOnMono;
        private static bool _hasVerifiedMono;

        #endregion

        #region [ -- Overridden base class methods and properties -- ]

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.
        /// </returns>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

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

        /// <summary>
        /// See <see cref="Gaia.WebWidgets.AjaxControl.BeginLoadViewState" /> and <see cref="Gaia.WebWidgets.AjaxControl.EndLoadViewState" /> methods for documentation. 
        /// This method only forwards to those methods.
        /// </summary>
        protected override void LoadViewState(object savedState)
        {
            var state = (Pair) AjaxControl.BeginLoadViewState(savedState);
            _initialValidation = (int) state.First;
            base.LoadViewState(state.Second);
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
        /// See <see cref="Gaia.WebWidgets.AjaxControl.OnPreRender" /> method for documentation. This method also calls into that method.
        /// </summary>
        /// <param name="e">EventArgs passed on from the system</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AjaxControl.OnPreRender();

            // bug: this is a workaround for the bug in mono:
            // bug: https://bugzilla.novell.com/show_bug.cgi?id=652973
            // todo: should be removed whenever the bug is fixed in mono.
            if (!_hasVerifiedMono)
            {
                _isRunningOnMono = Type.GetType("Mono.Runtime", false, false) != null;
                _hasVerifiedMono = true;
            }

            if (!_isRunningOnMono || !EnableClientScript || !IsEnabled) return;
            Page.ClientScript.RegisterArrayDeclaration("Page_ValidationSummaries",
                                                       string.Concat(@"jQuery(""#", ClientID, @""")[0]"));
        }

        /// <summary>
        /// See <see cref="Gaia.WebWidgets.AjaxControl.SaveViewState" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <returns>the saved ViewState</returns>
        protected override object SaveViewState()
        {
            _savedValidation = SaveValidationState();
            return AjaxControl.SaveViewState(new Pair(_savedValidation, base.SaveViewState()));
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
        /// See <see cref="Gaia.WebWidgets.AjaxControl.RenderControl" /> method for documentation. This method only forwards to that method.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to write the control into</param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            AjaxControl.RenderControl(writer);

            if (DesignMode || _markupRendered) return;
            _clientScriptUpdated = true;
            RenderControlHtml(new XhtmlTagFactory(writer));
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
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Label.js",
                                                            typeof (RequiredFieldValidator), "Gaia.Label.loaded", true);
        }

        /// <summary>
        /// Override in inherited class to customize rendering of the control.
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for creating Xhtml compliant markup</param>
        protected virtual void RenderControlHtml(XhtmlTagFactory create)
        {
            var writer = create.GetHtmlTextWriter();

            if (DesignMode)
            {
                base.Render(writer);
                return;
            }

            var canShow = IsEnabled && ((ShowSummary && GetValidationErrors().Count > 0) || EnableClientScript);
            Style[HtmlTextWriterStyle.Display] = canShow ? "block" : "none";

            using(writer)
            {
                RenderBeginTag(writer);
                RenderContents(writer);
                RenderEndTag(writer);
            }

            _markupRendered = true;
            
            if (_clientScriptUpdated) return;
            _clientScriptUpdated = true;
            
            var clientScript = Manager.Instance.ClientScript;
            clientScript.UpdateExpandoAttributes(ClientID);
            clientScript.UpdateArrayDeclaration("Page_ValidationSummaries");
            clientScript.UpdateScriptRegistrations(typeof(BaseValidator), "ValidatorCommonScript");
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">
        /// A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client. 
        /// </param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            var errors = GetValidationErrors();

            if (!IsEnabled || !ShowSummary || errors.Count <= 0) return;

            var headerText = HeaderText;
            var hasHeader = !string.IsNullOrEmpty(headerText);

            if (hasHeader)
                writer.Write(headerText);

            switch (DisplayMode)
            {
                case ASP.ValidationSummaryDisplayMode.BulletList:
                    {
                        writer.Write("<ul>");
                        foreach (var error in errors)
                        {
                            writer.Write("<li>");
                            writer.Write(error);
                            writer.Write("</li>");
                        }
                        writer.Write("</ul>");

                        break;
                    }

                case ASP.ValidationSummaryDisplayMode.List:
                    {
                        if (hasHeader)
                            writer.Write("<br/>");

                        foreach (var error in errors)
                        {
                            writer.Write(error);
                            writer.Write("<br/>");
                        }

                        break;
                    }

                case ASP.ValidationSummaryDisplayMode.SingleParagraph:
                    {
                        if (hasHeader)
                            writer.Write(" ");

                        foreach (var error in errors)
                            writer.Write(error + " ");

                        writer.Write("<br/>");
                        break;
                    }
            }
        }

        /// <summary>
        /// Override in derived classes to change registration script for this control.
        /// </summary>
        /// <param name="registerControl">Initial RegisterControl object to modify.</param>
        /// <returns>The modified RegisterControl object.</returns>
        protected virtual RegisterControl GetScript(RegisterControl registerControl)
        {
            return registerControl;
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
            return new PropertyStateManagerValidationSummary(this);
        }

        string IAjaxControl.GetScript() 
        {
            return GetScript(new RegisterControl("Gaia.Label", ClientID)).ToString();
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

        /// <summary>
        /// Returns validation state to save.
        /// </summary>
        private int SaveValidationState()
        {
            var errors = GetValidationErrors();
            return string.Join("~", errors.ToArray()).GetHashCode();
        }

        /// <summary>
        /// Returns <see cref="List{String}"/> of validator error messages.
        /// </summary>
        /// <returns></returns>
        private List<string> GetValidationErrors()
        {
            if (_errors == null)
            {
                var validators = Page.GetValidators(ValidationGroup);
                _errors = new List<string>(validators.Count);

                foreach (IValidator validator in validators)
                {
                    if (validator.IsValid) continue;

                    var errorMessage = validator.ErrorMessage;
                    if (errorMessage == null) continue;

                    _errors.Add(errorMessage);
                }
            }

            return _errors;
        }
    }
}
