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
    /// <summary>
    /// Specialized <see cref="PropertyStateManagerControl"/> for <see cref="ASP.WebControl"/>.
    /// </summary>
    /// <seealso cref="PropertyStateManagerControl"/>
    public class PropertyStateManagerWebControl : PropertyStateManagerControl
    {
        private bool _enabledHandled;
        private ASP.WebControl _state;
        private readonly ASP.WebControl _webControl;
        private readonly List<string> _styleChanges = new List<string>();

        #region [ -- Constructors -- ]

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerWebControl"/> for specified <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control to track changes for.</param>
        /// <remarks>
        /// Stores state information for the control and serializes changes during callback rendering.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
        public PropertyStateManagerWebControl(ASP.WebControl control) : this(control, control.ClientID) { }

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerWebControl"/> for specified <paramref name="control"/>
        /// using specified <paramref name="clientId"/> for reference.
        /// </summary>
        /// <param name="control">Control to track changes for.</param>
        /// <param name="clientId">The client-side ID of the <paramref name="control"/> to use.</param>
        /// <remarks>
        /// Stores state information for the control and serializes changes during callback rendering.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
        public PropertyStateManagerWebControl(ASP.WebControl control, string clientId) : this(control, clientId, null) { }

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerWebControl"/> for specified <paramref name="control"/>
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
        public PropertyStateManagerWebControl(ASP.WebControl control, string clientId, IExtraPropertyCallbackRenderer extra) : base(control, clientId, extra)
        {
            _webControl = control;
        }

        #endregion

        #region [ -- Enums -- ]

        /// <summary>
        /// Enumeration of supported properties.
        /// </summary>
        /// <seealso cref="PropertyStateManagerWebControl.ClearDirty(Gaia.WebWidgets.PropertyStateManagerWebControl.WebControlProperty[])"/>
        public enum WebControlProperty
        {
            /// <summary>
            /// AccessKey
            /// </summary>
            AccessKey,

            /// <summary>
            /// BackColor
            /// </summary>
            BackColor,

            /// <summary>
            /// BorderColor
            /// </summary>
            BorderColor,

            /// <summary>
            /// BorderStyle
            /// </summary>
            BorderStyle,

            /// <summary>
            /// BorderWidth
            /// </summary>
            BorderWidth,

            /// <summary>
            /// CssClass
            /// </summary>
            CssClass,

            /// <summary>
            /// Enabled
            /// </summary>
            Enabled,

            /// <summary>
            /// ForeColor
            /// </summary>
            ForeColor,

            /// <summary>
            /// Height
            /// </summary>
            Height,

            /// <summary>
            /// TabIndex
            /// </summary>
            TabIndex,

            /// <summary>
            /// ToolTip
            /// </summary>
            ToolTip,

            /// <summary>
            /// Width
            /// </summary>
            Width,

            /// <summary>
            /// Styles
            /// </summary>
            Styles,

            /// <summary>
            /// Attributes
            /// </summary>
            Attributes,

            /// <summary>
            /// FontBold
            /// </summary>
            FontBold,

            /// <summary>
            /// FontItalic
            /// </summary>
            FontItalic,

            /// <summary>
            /// FontSize
            /// </summary>
            FontSize,

            /// <summary>
            /// FontOverline
            /// </summary>
            FontOverline,

            /// <summary>
            /// FontStrikeout
            /// </summary>
            FontStrikeout,

            /// <summary>
            /// FontUnderline
            /// </summary>
            FontUnderline,

            /// <summary>
            /// FontNames
            /// </summary>
            FontNames
        }

        /// <summary>
        /// Clears dirtyness of the specified properties
        /// </summary>
        /// <param name="properties">Properties to clear dirtyness</param>
        public void ClearDirty(params WebControlProperty[] properties)
        {
            foreach (var property in properties)
            {
                switch (property)
                {
                    case WebControlProperty.AccessKey: _state.AccessKey = _webControl.AccessKey; break;
                    case WebControlProperty.BackColor: _state.BackColor = _webControl.BackColor; break;
                    case WebControlProperty.BorderColor: _state.BorderColor = _webControl.BorderColor; break;
                    case WebControlProperty.BorderStyle: _state.BorderStyle = _webControl.BorderStyle; break;
                    case WebControlProperty.BorderWidth: _state.BorderWidth = _webControl.BorderWidth; break;
                    case WebControlProperty.CssClass: _state.CssClass = _webControl.CssClass; break;
                    case WebControlProperty.Enabled: _state.Enabled = Utilities.IsEnabled(_webControl); break;
                    case WebControlProperty.ForeColor: _state.ForeColor = _webControl.ForeColor; break;
                    case WebControlProperty.Height: _state.Height = _webControl.Height; break;
                    case WebControlProperty.TabIndex: _state.TabIndex = _webControl.TabIndex; break;
                    case WebControlProperty.ToolTip: _state.ToolTip = _webControl.ToolTip; break;
                    case WebControlProperty.Width: _state.Width = _webControl.Width; break;
                    case WebControlProperty.Styles: CopyStyles(_webControl.Style, _state.Style); break;
                    case WebControlProperty.Attributes: CopyAttributes(_webControl.Attributes, _state.Attributes); break;
                    case WebControlProperty.FontBold: _state.Font.Bold = _webControl.Font.Bold; break;
                    case WebControlProperty.FontItalic: _state.Font.Italic = _webControl.Font.Italic; break;
                    case WebControlProperty.FontSize: _state.Font.Size = _webControl.Font.Size; break;
                    case WebControlProperty.FontNames: _state.Font.Names = _webControl.Font.Names; break;
                    case WebControlProperty.FontOverline: _state.Font.Overline = _webControl.Font.Overline; break;
                    case WebControlProperty.FontStrikeout: _state.Font.Strikeout = _webControl.Font.Strikeout; break;
                    case WebControlProperty.FontUnderline: _state.Font.Underline = _webControl.Font.Underline; break;
                }
            }
        }

        /// <summary>
        /// Clears dirtyness for the specified Style keys
        /// </summary>
        /// <param name="keys">Style keys to clear dirtyness</param>
        public void ClearDirtyStyle(params HtmlTextWriterStyle[] keys)
        {
            if (keys.Length == 0) return;
            
            var stateStyles = _state.Style;
            var currentStyles = _webControl.Style;
            foreach(var key in keys)
            {
                var value = currentStyles[key];
                if (value == null)
                    stateStyles.Remove(key);
                else
                    stateStyles[key] = currentStyles[key];
            }
        }

        /// <summary>
        /// Clears dirtyness for the specified Style keys as strings
        /// </summary>
        /// <param name="keys">Style keys to clear dirtyness</param>
        public void ClearDirtyStyle(params string[] keys)
        {
            if (keys.Length == 0) return;
            
            var stateStyles = _state.Style;
            var currentStyles = _webControl.Style;
            foreach (var key in keys)
            {
                var value = currentStyles[key];
                if (value == null)
                    stateStyles.Remove(key);
                else
                    stateStyles[key] = currentStyles[key];
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets <see cref="ASP.WebControl.ControlStyle"/> property in the state snapshot.
        /// </summary>
        internal protected virtual ASP.Style ControlStyle
        {
            get { return _state.ControlStyle; }
            set { _state.ControlStyle.CopyFrom(value); }
        }

        /// <summary>
        /// Gets or sets <see cref="ASP.WebControl.ForeColor"/> property in the state snapshot.
        /// </summary>
        internal virtual Color ForeColor
        {
            get { return _state.ForeColor; }
            set { _state.ForeColor = value; }
        }

        /// <summary>
        /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
        protected override void AssignState(PropertyStateManagerControl source)
        {
            base.AssignState(source);

            var sourceManager = source as PropertyStateManagerWebControl;
            if (sourceManager == null) return;

            _state = sourceManager._state;
        }

        /// <summary>
        /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void TakeSnapshot()
        {
            _state = new ASP.WebControl(HtmlTextWriterTag.Unknown);
            _state.CopyBaseAttributes(_webControl);
            _state.AccessKey = _webControl.AccessKey;
            _state.BackColor = _webControl.BackColor;
            _state.BorderColor = _webControl.BorderColor;
            _state.BorderStyle = _webControl.BorderStyle;
            _state.BorderWidth = _webControl.BorderWidth;
            _state.CssClass = _webControl.CssClass;
            _state.Enabled = Utilities.IsEnabled(_webControl);
            _state.ForeColor = _webControl.ForeColor;
            _state.Height = _webControl.Height;
            _state.TabIndex = _webControl.TabIndex;
            _state.ToolTip = _webControl.ToolTip;
            _state.Width = _webControl.Width;

            _state.Font.CopyFrom(_webControl.Font);
            CopyStyles(_webControl.Style, _state.Style);

            base.TakeSnapshot();
        }

        /// <summary>
        /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void DiffSnapshot()
        {
            DiffCommonProperties(_state);
            DiffFonts(_state);
            DiffStyles(_state);
            DiffAttributes(_state);
            Flush();

            base.DiffSnapshot();
        }

        private void DiffCommonProperties(ASP.WebControl state)
        {
            RenderChange(state.AccessKey, _webControl.AccessKey, "setAccessKey");
            RenderChange(state.BackColor, _webControl.BackColor, "setBackColor");
            RenderChange(state.BorderColor, _webControl.BorderColor, "setBorderColor");
            RenderChange(state.BorderStyle, _webControl.BorderStyle, "setBorderStyle");
            RenderChange(state.BorderWidth, _webControl.BorderWidth, "setBorderWidth");
            RenderChange(state.CssClass, _webControl.CssClass, RenderCssClassChanged);

            var value = Utilities.IsEnabled(_webControl);
            var enabledChanged = value != state.Enabled;
            var isEnabledHandled = DetermineEnabledHandled();
            if ((enabledChanged && !isEnabledHandled) || (!enabledChanged && !_webControl.Enabled && isEnabledHandled))
            {
                RenderEnabledChanged(value);
                _enabledHandled = true;
            }

            RenderChange(state.ForeColor, _webControl.ForeColor, "setForeColor");
            RenderChange(state.Height, _webControl.Height, "setHeight");
            RenderChange(state.TabIndex, _webControl.TabIndex, "setTabIndex");
            RenderChange(state.ToolTip, _webControl.ToolTip, "setToolTip");
            RenderChange(state.Width, _webControl.Width, "setWidth");
        }

        private void DiffStyles(ASP.WebControl state)
        {
            var commonNames = new List<string>();
            var currentStyles = _webControl.Style;
            var initialStyles = state.Style;

            foreach(string currentName in currentStyles.Keys)
            {
                var change = true;
                var currentStyle = currentStyles[currentName];

                foreach (string name in initialStyles.Keys)
                {
                    if (currentName != name) continue;
                    commonNames.Add(name);
                    if (currentStyle == initialStyles[currentName])
                        change = false;
                    break;
                }

                if (change)
                    RenderStyleChanged(currentName, currentStyle);
            }

            foreach(string initialName in initialStyles.Keys)
            {
                if (commonNames.Contains(initialName)) continue;
                RenderStyleRemoved(initialName);
            }
        }

        private void DiffFonts(ASP.WebControl state)
        {
            var currentFont = _webControl.Font;
            var initialFont = state.Font;

            RenderChange(initialFont.Bold, currentFont.Bold, "setFontBold");
            RenderChange(initialFont.Size, currentFont.Size, "setFontSize");
            RenderChange(initialFont.Italic, currentFont.Italic, "setFontItalic");
            RenderChange(initialFont.Overline, currentFont.Overline, "setFontOverline");
            RenderChange(initialFont.Strikeout, currentFont.Strikeout, "setFontStrikeout");
            RenderChange(initialFont.Underline, currentFont.Underline, "setFontUnderline");

            var initialNames = initialFont.Names;
            var currentNames = currentFont.Names;
            var wasChanged = (initialNames.Length != currentNames.Length) ||
                             Array.Exists(currentNames, name => !Array.Exists(initialNames, name.Equals));

            if (wasChanged)
                RenderFontNamesChanged(currentNames);
        }

        private void DiffAttributes(ASP.WebControl state)
        {
            var commonAttributeNames = new List<string>();
            var currentAttributes = _webControl.Attributes;
            var initialAttributes = state.Attributes;

            foreach (string attributeName in currentAttributes.Keys)
            {
                if (attributeName.ToLowerInvariant().Equals("style")) continue;

                var changed = true;
                var currentValue = currentAttributes[attributeName];

                foreach (string name in initialAttributes.Keys)
                {
                    if (name != attributeName) continue;
                    commonAttributeNames.Add(name);
                    if (currentValue.Equals(initialAttributes[name]))
                        changed = false;
                    break;
                }

                if (changed)
                    RenderAttributeChanged(attributeName, currentValue);
            }


            foreach (string attributeName in initialAttributes.Keys)
            {
                if (commonAttributeNames.Contains(attributeName) || attributeName.ToLowerInvariant().Equals("style")) continue;
                RenderAttributeRemoved(attributeName);
            }
        }

        private static void CopyStyles(CssStyleCollection source, CssStyleCollection destination)
        {
            destination.Value = source.Value;
        }

        private static void CopyAttributes(AttributeCollection source, AttributeCollection destination)
        {
            destination.Clear();
            foreach (string attributeName in source.Keys)
                destination.Add(attributeName, source[attributeName]);
        }

        private bool DetermineEnabledHandled()
        {
            var enabledHandled = false;
            for (var parent = _webControl.Parent; parent != null && !enabledHandled; parent = parent.Parent)
            {
                var ajaxContainerControl = parent as IAjaxContainerControl;
                if (ajaxContainerControl == null) continue;
                var stateManager = ajaxContainerControl.StateManager as PropertyStateManagerWebControl;
                if (stateManager == null) continue;
                enabledHandled = stateManager._enabledHandled;
            }
            return enabledHandled;
        }

        private void RenderCssClassChanged(string cssClass)
        {
            var isSkinControl = _webControl is ISkinControl;
            Builder.Append(isSkinControl
                               ? string.Concat(".setSkinCssClass('", _state.CssClass, "','", cssClass, "')")
                               : string.Concat(".setCssClass('", cssClass, "')"));
        }

        private void RenderEnabledChanged(bool enabled)
        {
            Builder.Append(string.Concat(".setEnabled(", FormatValue(enabled), ")"));
        }

        /// <summary>
        /// Called when an attribute in <see cref="ASP.WebControl.Attributes" /> collection was modified.
        /// </summary>
        /// <param name="name">Name of the modified attribute.</param>
        /// <param name="value">Value of the modified attribute.</param>
        protected virtual void RenderAttributeChanged(string name, string value)
        {
            Builder.Append(string.Concat(".setAttribute('", name, "',", FormatValue(value), ")"));
        }

        private void RenderAttributeRemoved(string name)
        {
            Builder.Append(string.Concat(".removeAttribute('", name, "')"));
        }

        private void RenderStyleChanged(string name, string value)
        {
            _styleChanges.Add(string.Concat("'", name, "':'", value, "'"));
        }

        private void RenderStyleRemoved(string name)
        {
            _styleChanges.Add(string.Concat("'", name, "':''"));
        }

        private void RenderFontNamesChanged(string[] names)
        {
            Builder.Append(string.Concat(".setFontNames('", string.Join(",", names), "')"));
        }

        private void Flush()
        {
            if (_styleChanges.Count > 0)
                Builder.Append(string.Concat(".setStyle({", string.Join(",", _styleChanges.ToArray()), "})"));
        }
    }
}
