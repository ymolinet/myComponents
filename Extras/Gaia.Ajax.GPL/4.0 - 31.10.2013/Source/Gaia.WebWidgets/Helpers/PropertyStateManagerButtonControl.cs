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
    /// <summary>
    /// Specialized <see cref="PropertyStateManagerControl"/> for Button controls.
    /// </summary>
    /// <typeparam name="T">Concrete button type.</typeparam>
    public abstract class PropertyStateManagerButtonControl<T> : PropertyStateManagerWebControl where T: ASP.WebControl, ASP.IButtonControl, new()
    {
        private bool _enableBubbling;
        private bool _causesValidation;

        private string _text;
        private string _target;
        private string _argument;
        private string _postBackUrl;
        private string _validationGroup;
        private string _clientClickHandler;
        
        private readonly T _button;
        private readonly IAjaxButtonControl _buttonControl;

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerButtonControl{T}"/> for specified <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control to track changes for.</param>
        /// <remarks>
        /// Stores state information for the control and serializes changes during callback rendering.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
        protected PropertyStateManagerButtonControl(T control) : this(control, control.ClientID, null) { }

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerButtonControl{T}"/> for specified <paramref name="control"/>
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
        protected PropertyStateManagerButtonControl(T control, string clientId, IExtraPropertyCallbackRenderer extra) : base(control, clientId, extra)
        {
            _button = control;
            _buttonControl = (IAjaxButtonControl) control;
        }

        /// <summary>
        /// Assigns new state to this <see cref="PropertyStateManagerButtonControl{T}"/> by copying from specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source <see cref="PropertyStateManagerButtonControl{T}"/> to copy state from.</param>
        protected override void AssignState(PropertyStateManagerControl source)
        {
            base.AssignState(source);
            
            var sourceManager = source as PropertyStateManagerButtonControl<T>;
            if (sourceManager == null) return;

            _text = sourceManager._text;
            _target = sourceManager._target;
            _argument = sourceManager._argument;
            _postBackUrl = sourceManager._postBackUrl;
            _enableBubbling = sourceManager._enableBubbling;
            _validationGroup = sourceManager._validationGroup;
            _causesValidation = sourceManager._causesValidation;
            _clientClickHandler = sourceManager._clientClickHandler;
        }

        /// <summary>
        /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void TakeSnapshot()
        {
            var postBackOptions = _buttonControl.GetPostBackOptions();

            _text = _buttonControl.Text;
            _argument = postBackOptions.Argument;
            _postBackUrl = postBackOptions.ActionUrl;
            _clientClickHandler = GetOnClickHandler();
            _enableBubbling = _buttonControl.EnableBubbling;
            _validationGroup = postBackOptions.ValidationGroup;
            _causesValidation = postBackOptions.PerformValidation;

            // we assume TargetControl is part of control tree, so that UniqueID is valid.
            _target = GetCallbackName(postBackOptions);

            base.TakeSnapshot();
        }

        /// <summary>
        /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void DiffSnapshot()
        {
            var postBackOptions = _buttonControl.GetPostBackOptions();

            RenderChange(_text, _button.Text, "setText", true);
            RenderChange(_clientClickHandler, GetOnClickHandler(), "setOnClick");
            RenderChange(_target, GetCallbackName(postBackOptions), "setTgt", true);
            RenderChange(_enableBubbling, _buttonControl.EnableBubbling, "setBubble");
            RenderChange(_argument, postBackOptions.Argument, Constants.SetArgumentFunctionName);
            RenderChange(_postBackUrl, postBackOptions.ActionUrl, Constants.SetPostBackUrlFunctionName, true);
            RenderChange(_validationGroup, postBackOptions.ValidationGroup, Constants.SetValidationGroupFunctionName);
            RenderChange(_causesValidation, postBackOptions.PerformValidation, Constants.SetCausesValidationFunctionName);

            base.DiffSnapshot();
        }

        /// <summary>
        /// Called when an attribute in <see cref="ASP.WebControl.Attributes" /> collection was modified.
        /// </summary>
        /// <param name="name">Name of the modified attribute.</param>
        /// <param name="value">Value of the modified attribute.</param>
        protected override void RenderAttributeChanged(string name, string value)
        {
            if (name == "onclick") return;
            base.RenderAttributeChanged(name, value);
        }

        /// <summary>
        /// Returns DOM 0 click event handler for the associated <see cref="IAjaxButtonControl"/>.
        /// </summary>
        private string GetOnClickHandler()
        {
            var onClientClick = _buttonControl.OnClientClick;
            
            string onClickAttr = null;
            if (_button.HasAttributes)
                onClickAttr = _button.Attributes["onclick"];

            var handler = string.IsNullOrEmpty(onClientClick) ? string.Empty : EnsureSemicolonSuffix(onClientClick);

            if (!string.IsNullOrEmpty(onClickAttr))
                handler += EnsureSemicolonSuffix(onClickAttr);

            return handler.Replace("javascript:", string.Empty);
        }

        /// <summary>
        /// Ensures specified <paramref name="str"/> ends with semicolon.
        /// </summary>
        private static string EnsureSemicolonSuffix(string str)
        {
            return str.EndsWith(";") ? str : str + ";";
        }

        /// <summary>
        /// Returns the callbackName based on the specified <paramref name="options"/>.
        /// </summary>
        private string GetCallbackName(PostBackOptions options)
        {
            var targetControl = options.TargetControl;
            var target = targetControl == null ? _button.UniqueID : targetControl.UniqueID;
            return Utilities.GetCallbackName(target, _button.ClientID);
        }
    }
}
