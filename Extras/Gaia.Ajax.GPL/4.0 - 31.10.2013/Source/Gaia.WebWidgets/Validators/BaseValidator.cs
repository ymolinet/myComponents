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
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    using HtmlFormatting;

    /// <summary>
    /// Common implementation details for the ajaxified validator controls.
    /// </summary>
    internal class BaseValidator
    {
        /// <summary>
        /// Specifies the display rendering mode for text and error messages in the validator.
        /// </summary>
        internal enum ValidatorDisplayMode
        {
            /// <summary>
            /// Validator content is never displayed inline.
            /// </summary>
            None,

            /// <summary>
            /// Validator content is rendered and is part of page layout.
            /// </summary>
            Static,

            /// <summary>
            /// Validator content is rendered, but not as part of page layout.
            /// </summary>
            Dynamic,

            /// <summary>
            /// Validator content is rendered and displayed.
            /// </summary>
            Visible
        } ;

        private bool _markupRendered;
        private bool _clientScriptUpdated;
        private readonly IAjaxValidator _accessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseValidator"/> class.
        /// </summary>
        public BaseValidator(ASP.BaseValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            _accessor = validator as IAjaxValidator;
            if (_accessor == null)
                throw new ArgumentException("Validator should implement IAjaxValidator interface");
            
            Validator = validator;
        }

        /// <summary>
        /// Returns true if the validator was valid.
        /// </summary>
        internal bool WasValid { get; private set; }

        /// <summary>
        /// Returns the owner of this implementation.
        /// </summary>
        internal ASP.BaseValidator Validator { get; private set; }

        /// <summary>Loads the previously saved state for the control.</summary>
        /// <param name="savedState">An object that contains the saved view state values for the control. </param>
        /// <seealso cref="WebWidgets.AjaxControl.EndLoadViewState"/>
        /// <seealso cref="Gaia.WebWidgets.AjaxControl.BeginLoadViewState"/>
        public object LoadViewState(object savedState)
        {
            var pair = (Pair) savedState;
            WasValid = (bool) pair.First;
            return pair.Second;
        }
        
        /// <summary>
        /// Called during PreRender stage.
        /// </summary>
        /// <returns>Disposable object.</returns>
        public IDisposable OnPreRender()
        {
            if (!_accessor.DetermineRenderUplevel())
                return null;

            return Manager.Instance.ClientScript.TrackScriptRegistrations(typeof (BaseValidator), "ValidatorCommonScript");
        }

        /// <summary>
        /// Saves any state that was modified after the <see cref="Control.TrackViewState"/> method was invoked.
        /// </summary>
        /// <returns>
        /// An object that contains the current view state of the control; otherwise, if there is no view state associated with the control, null.
        /// </returns>
        public object SaveViewState(object savedState)
        {
            return new Pair(Validator.IsValid, savedState);
        }

        /// <summary>
        /// Begin rendering associated <see cref="Validator"/> using the specified <paramref name="writer"/>.
        /// </summary>
        public void BeginRenderValidator(HtmlTextWriter writer)
        {
        }

        /// <summary>
        /// Finish rendering associated <see cref="Validator"/> using the specified <paramref name="writer"/>.
        /// </summary>
        public void EndRenderValidator(HtmlTextWriter writer)
        {
            var ajaxControl = ((IAjaxControl)Validator);
            
            // we need to render even during callbacks so that registration scripts are rendered.
            if (ajaxControl.InDesigner || _markupRendered) return;
            _clientScriptUpdated = true;
            RenderControlHtml(new XhtmlTagFactory(writer));
        }

        /// <summary>
        /// Renders HTML markup for the validator using the specified <see cref="XhtmlTagFactory"/>.
        /// </summary>
        public void RenderControlHtml(XhtmlTagFactory create)
        {
            var writer = create.GetHtmlTextWriter();
            var ajaxControl = ((IAjaxControl)Validator);

            if (ajaxControl.InDesigner)
            {
                _accessor.RenderDesignTime(writer);
                return;
            }

            if (Validator.Page != null)
                Validator.Page.VerifyRenderingInServerForm(Validator);

            if (_accessor.RenderUplevel)
                _accessor.RegisterValidatorDeclaration();

            SetDisplayStyle(GetDisplayMode(Validator.IsValid, _accessor.RenderUplevel));

            // Gaia controls should always render a root element);
            using (writer)
            {
                Validator.RenderBeginTag(writer);

                var text = GetValidationText(Validator.IsValid, _accessor.RenderUplevel);
                if (text != null)
                    writer.Write(text);

                Validator.RenderEndTag(writer);
            }

            // check if ajax core is rendering markup, and force rerendering scripts.
            _markupRendered = true;
            EnsureClientScriptUpdated();
        }

        /// <summary>
        /// Includes required script files.
        /// </summary>
        public void IncludeScriptFiles()
        {
            var manager = Manager.Instance;

            // todo: this is not a good way of including of files!
            manager.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Container.js", typeof(Manager), "Gaia.Container.browserFinishedLoading", true);
            manager.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.Label.js", typeof(Manager), "Gaia.Label.loaded", true);
        }

        /// <summary>
        /// Returns validator client-side registration object.
        /// </summary>
        public RegisterControl GetScript()
        {
            return new RegisterControl("Gaia.Label", Validator.ClientID);
        }

        /// <summary>
        /// Ensures client scripts are updated.
        /// </summary>
        internal void EnsureClientScriptUpdated()
        {
            if (_clientScriptUpdated) return;
            _clientScriptUpdated = true;

            var clientScript = Manager.Instance.ClientScript;
            clientScript.UpdateArrayDeclaration("Page_Validators");
            clientScript.UpdateExpandoAttributes(Validator.ClientID);
            clientScript.UpdateScriptRegistrations(typeof(BaseValidator), "ValidatorCommonScript");
        }

        /// <summary>
        /// Sets display styles based on the value of the specified <paramref name="displayMode"/> parameter.
        /// </summary>
        internal void SetDisplayStyle(ValidatorDisplayMode displayMode)
        {
            switch (displayMode)
            {
                case ValidatorDisplayMode.Static:
                    Validator.Style[HtmlTextWriterStyle.Display] = "inline";
                    Validator.Style[HtmlTextWriterStyle.Visibility] = "hidden";
                    break;

                case ValidatorDisplayMode.None:
                case ValidatorDisplayMode.Dynamic:
                    Validator.Style[HtmlTextWriterStyle.Display] = "none";
                    Validator.Style[HtmlTextWriterStyle.Visibility] = "visible";
                    break;

                case ValidatorDisplayMode.Visible:
                    Validator.Style[HtmlTextWriterStyle.Display] = "inline";
                    Validator.Style[HtmlTextWriterStyle.Visibility] = "visible";
                    break;
            }
        }

        /// <summary>
        /// Computes display mode for rendering based on the value of <paramref name="isValid"/> parameter.
        /// </summary>
        internal ValidatorDisplayMode GetDisplayMode(bool isValid, bool renderUplevel)
        {
            var display = Validator.Display;
            if (_accessor.PropertiesValid && display != ASP.ValidatorDisplay.None)
            {
                if (!isValid && _accessor.IsEnabled)
                    return ValidatorDisplayMode.Visible;

                var isDisplayStatic = display == ASP.ValidatorDisplay.Static;
                if (renderUplevel || isDisplayStatic)
                    return isDisplayStatic ? ValidatorDisplayMode.Static : ValidatorDisplayMode.Dynamic;
            }

            return ValidatorDisplayMode.None;
        }

        /// <summary>
        /// Returns the validation text to render.
        /// </summary>
        internal string GetValidationText(bool isValid, bool renderUplevel)
        {
            var display = Validator.Display;
            if (_accessor.PropertiesValid && display != ASP.ValidatorDisplay.None)
            {
                if (renderUplevel || (!isValid && _accessor.IsEnabled))
                {
                    var text = Validator.Text;
                    return string.IsNullOrEmpty(text) ? Validator.ErrorMessage : text;
                }

                if (display == ASP.ValidatorDisplay.Static)
                    return "&nbsp";
            }

            return null;
        }
    }
}