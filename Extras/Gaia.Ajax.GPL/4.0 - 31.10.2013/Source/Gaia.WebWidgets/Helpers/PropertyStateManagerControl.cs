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
using System.IO;
using System.Text;
using System.Web.UI;
using System.Drawing;
using System.Globalization;
using System.Collections.Generic;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    using PropertyCache = Reflection.Cache<AjaxSerializableAttributeBase>;

    /// <summary>
    /// Tracks state changes for the associated <see cref="IAjaxControl"/> and
    /// renders found state changes to the client.
    /// </summary>
    public class PropertyStateManagerControl
    {
        private static readonly char[] HexDigits = {
                                                       '0', '1', '2', '3', '4', '5', '6', '7',
                                                       '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
                                                   };

        /// <summary>
        /// Keeps state for a property which has <see cref="AjaxSerializableAttribute"/>.
        /// </summary>
        private sealed class ControlPropertyState
        {
            private readonly List<object> _ajaxSerializableValues = new List<object>();

            public List<object> AjaxSerializableValues
            {
                get { return _ajaxSerializableValues; }
            }
        }

        #region [ -- Private Members -- ]

        private StringBuilder _builder;
        private readonly Control _control;
        private ControlPropertyState _state;
        private List<PropertyCache.CachedProperty> _propertyCache;
        private readonly IExtraPropertyCallbackRenderer _extraRendering;

        #endregion

        #region [ -- Constructors -- ]

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerControl"/> for specified <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control to track changes for.</param>
        /// <remarks>
        /// Stores state information for the control and serializes changes during callback rendering.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
        public PropertyStateManagerControl(Control control) : this(control, control.ClientID, null) { }

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerControl"/> for specified <paramref name="control"/>
        /// using specified <paramref name="clientId"/> for reference.
        /// </summary>
        /// <param name="control">Control to track changes for.</param>
        /// <param name="clientId">The client-side ID of the <paramref name="control"/> to use.</param>
        /// <remarks>
        /// Stores state information for the control and serializes changes during callback rendering.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
        [Obsolete("Explicitly specified clientId is not supported. Control.ClientID will be used instead.")]
        public PropertyStateManagerControl(Control control, string clientId) : this(control, control.ClientID, null) { }

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerControl"/> for specified <paramref name="control"/>
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
        public PropertyStateManagerControl(Control control, string clientId, IExtraPropertyCallbackRenderer extra)
        {
            if (control == null)
                throw new ArgumentNullException("control");

            if (clientId != control.ClientID)
                throw new ArgumentException("Explicitly specified clientId is not supported. Use Control.ClientID instead.", "clientId");

            _control = control;
            _extraRendering = extra;
        }

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// Returns true if changes should be serialized to the client.
        /// </summary>
        public virtual bool ShouldRender
        {
            get { return WasVisible || AjaxControl.Renamed; }
        }

        /// <summary>
        /// True if the control was visible during previous request.
        /// </summary>
        internal bool WasVisible { get; set; }

        /// <summary>
        /// Buffer to render changes into.
        /// </summary>
        protected StringBuilder Builder
        {
            get { return _builder ?? (_builder = new StringBuilder()); }
        }

        /// <summary>
        /// Returns the associated <see cref="AjaxControl"/>.
        /// </summary>
        private AjaxControl AjaxControl
        {
            get { return ((IAjaxControl) _control).AjaxControl; }
        }

        #endregion

        #region [ -- Public Methods -- ]

        /// <summary>
        /// Takes snapshot of the associated <see cref="Control"/> state and raises <see cref="OnSnapshotTakenEvent"/>.
        /// </summary>
        public void TrackChanges()
        {
            if (!Manager.Instance.IsAjaxCallback)
                throw new InvalidOperationException("Tracking is not supported during non Ajax callbacks");

            TakeSnapshot();
            RaiseSnapshotTaken();
        }

        /// <summary>
        /// Detects state changes of the associated <see cref="Control"/> and serialized them back to the client.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/> to write the changes into.</param>
        public void RenderChanges(TextWriter writer)
        {
            if (!Manager.Instance.IsAjaxCallback)
                throw new InvalidOperationException("Rendering is not supported during non Ajax callbacks");

            _builder = null;
            var wasVisible = WasVisible;
            var isVisible = _control.Visible;
            
            if (isVisible)
                DiffSnapshot();

            int index;
            string suffix;
            string clientid;
            var clientReference = AjaxControl.ClientReference;
            if (wasVisible && AjaxControl.Renamed)
            {
                clientid = _control.ClientID;
                suffix = Utilities.GenerateDifference(clientReference, clientid, out index);
                var idScript = index == -1
                                   ? string.Concat(".setID('", clientid, "')")
                                   : string.Concat(".sepID('", suffix, "',", index, ")");

                Builder.Append(idScript);
            }

            if (wasVisible != isVisible)
                RenderVisibleChanged(isVisible);

            if (_extraRendering != null && _control.Visible)
                _extraRendering.InjectPropertyChangesToCallbackResponse(Builder);

            if (Builder.Length > 0)
                writer.Write("$G('{0}'){1};", clientReference, Builder);

            if (wasVisible) return;

            clientid = _control.ClientID;
            suffix = Utilities.GenerateDifference(clientReference, clientid, out index);
            writer.Write(index == -1
                             ? string.Concat(Constants.GaiaClientModule, ".SID('", clientReference, "','", clientid, "');")
                             : string.Concat(Constants.GaiaClientModule, ".SID('", clientReference, "','", suffix, "',", index, ");"));
        }

        /// <summary>
        /// Marks the property with the specified <paramref name="propertyName"/> as clean.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <remarks>
        /// The property must have <see cref="AjaxSerializableAttribute"/> applied to it.
        /// Marking the property as clean prevents detection of change in its state.
        /// </remarks>
        public void ClearDirty(string propertyName)
        {
            var index = _propertyCache.FindIndex(prop => prop.Name == propertyName);
            
            if (index == -1)
                throw new ArgumentException("AjaxSerializable property not found", propertyName);
            
            _state.AjaxSerializableValues[index] = _propertyCache[index].Getter.GetValue(_control);
        }

        /// <summary>
        /// Assigns new state to this <see cref="PropertyStateManagerControl"/> by copying from specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source <see cref="PropertyStateManagerControl"/> to copy state from.</param>
        /// <param name="keepVisible">True if the state of the <see cref="WasVisible"/> property should be kept during assignment.</param>
        internal void AssignState(PropertyStateManagerControl source, bool keepVisible)
        {
            var visible = WasVisible;
            AssignState(source);
            if (!keepVisible) return;
            WasVisible = visible;
        }

        #endregion

        #region [ -- Protected Members -- ]

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
        protected virtual bool OnSnapshotTakenEvent(object sender, EventArgs eventArgs)
        {
            return false;
        }

        /// <summary>
        /// Takes snapshot of the current state of the associated <see cref="Control"/>.
        /// </summary>
        protected virtual void TakeSnapshot()
        {
            WasVisible = _control.Visible;
            _state = new ControlPropertyState();
            _propertyCache = PropertyCache.GetProperties(_control);

            _propertyCache.ForEach(info => _state.AjaxSerializableValues.Add(info.Getter.GetValue(_control)));
        }

        /// <summary>
        /// Detects changes between current state and saved state snapshot for the associated <see cref="Control"/>.
        /// </summary>
        protected virtual void DiffSnapshot()
        {
            var initialValues = _state.AjaxSerializableValues;

            for (var index = 0; index < initialValues.Count; ++index)
            {
                var property = _propertyCache[index];
                
                var initialValue = initialValues[index];
                var currentValue = property.Getter.GetValue(_control);

                if ((initialValue == null && currentValue != null) ||
                    (initialValue != null && currentValue == null) ||
                    (initialValue != null && !initialValue.Equals(currentValue)))
                {
                    RenderAjaxSerializablePropertyChanged(property.Attribute, initialValue, currentValue);
                }
            }

            if (AjaxControl.Focused)
                RenderFocus();
        }

        /// <summary>
        /// Assigns new state to this <see cref="PropertyStateManagerControl"/> by copying from specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source <see cref="PropertyStateManagerControl"/> to copy state from.</param>
        protected virtual void AssignState(PropertyStateManagerControl source) 
        {
            _state = source._state;
            WasVisible = source.WasVisible;
            _propertyCache = source._propertyCache;
        }

        /// <summary>
        /// Executes specified <paramref name="renderAction"/> if the specified <paramref name="initial"/> and <paramref name="current"/> values are not equal.
        /// </summary>
        /// <typeparam name="T">Type of the parameters.</typeparam>
        /// <param name="initial">Initial value.</param>
        /// <param name="current">Current value.</param>
        /// <param name="renderAction">Action to execute if the specified <paramref name="initial"/> and <paramref name="current"/> values are not equal.</param>
        /// <remarks>Equality is determined using <see cref="EqualityComparer{T}.Default"/>.</remarks>
        protected void RenderChange<T>(T initial, T current, Action<T> renderAction)
        {
            var comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(initial, current)) return;
            renderAction(current);
        }

        /// <summary>
        /// Renders client-side Gaia Control method call using specified <paramref name="commandName"/>
        /// if the specified <paramref name="initial"/> and <paramref name="current"/> values are not equal.
        /// </summary>
        /// <typeparam name="T">Type of the parameters.</typeparam>
        /// <param name="initial">Initial value.</param>
        /// <param name="current">Current value.</param>
        /// <param name="commandName">Client-side Gaia Control method name.</param>
        /// <param name="optimizeCommandArgument">True if the client-side method supports argument optimizations. Otherwise, false.</param>
        /// <remarks>
        /// Equality is determined using <see cref="EqualityComparer{T}.Default"/>.
        /// Client-side Gaia Control methods supporting argument optimizations should have the form of "function foo(value, index)".
        /// To get the actual value to use they should call Gaia.Control.decodeArgument and specify initial and current values, as well as the index provided.
        /// </remarks>
        protected void RenderChange<T>(T initial, T current, string commandName, bool optimizeCommandArgument = false)
        {
            var comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(initial, current)) return;
            var commandArgument = FormatValue(initial, current, optimizeCommandArgument);
            Builder.Append(".").Append(commandName).Append("(").Append(commandArgument).Append(")");
        }

        /// <summary>
        /// Formats the specified <paramref name="value"/> as <see cref="String"/> to send to the client.
        /// </summary>
        /// <param name="value">Value to format.</param>
        /// <returns><see cref="String"/> representation of the specified <paramref name="value"/>.</returns>
        protected virtual string FormatValue(object value)
        {
            if (value == null)
                return "null";

            if (value is bool)
                return ((bool)value) ? "1" : "0";

            if (value is Color)
                return FormatColor((Color) value);

            if (value is ASP.Unit)
                return FormatString(((ASP.Unit) value).ToString(NumberFormatInfo.InvariantInfo));

            if (value is ASP.FontUnit)
                return FormatString(((ASP.FontUnit) value).ToString(NumberFormatInfo.InvariantInfo));

            if (value is float || value is double || value is decimal)
                return FormatString(((IFormattable) value).ToString(null, NumberFormatInfo.InvariantInfo));

            if (value is Enum || value is string)
                return FormatString(value.ToString());

            return value.ToString();
        }

        #endregion

        /// <summary>
        /// Renders code for the visibility change on the client-side.
        /// </summary>
        /// <param name="visible">The current value of the <see cref="Control.Visible"/> property.</param>
        private void RenderVisibleChanged(bool visible)
        {
            Builder.AppendFormat(".setVisible({0})", FormatValue(visible));
        }

        /// <summary>
        /// Renders code for the property value change having specified <paramref name="attr"/>.
        /// </summary>
        /// <param name="attr">The attribute of the property.</param>
        /// <param name="initialValue">The initial value of the property.</param>
        /// <param name="currentValue">The current value of the property.</param>
        private void RenderAjaxSerializablePropertyChanged(AjaxSerializableAttributeBase attr, object initialValue, object currentValue)
        {
            var currentFormattedValue = attr.FormatValue(_control, currentValue);
            Builder.AppendFormat(".{0}({1})", attr.JSMethodName, FormatValue(initialValue, currentFormattedValue, attr.OptimizeValue));
        }

        /// <summary>
        /// Renders code for setting focus to the associated <see cref="Control"/>.
        /// </summary>
        private void RenderFocus() 
        {
            Builder.Append(".setFocus()");
        }

        /// <summary>
        /// Formats specified <paramref name="value"/> for using in client-side code as valid string.
        /// </summary>
        private static string FormatString(string value)
        {
            return string.Concat("'", HtmlFormatter.FormatHtmlForInnerHTML(value), "'");
        }

        /// <summary>
        /// Raises <see cref="OnSnapshotTakenEvent"/>.
        /// </summary>
        private void RaiseSnapshotTaken()
        {
            var ajaxControl = _control as IAjaxControl;
            for (var parent = _control.Parent; parent != null; parent = parent.Parent)
            {
                // skip non-ajax parent controls
                var parentAjaxControl = parent as IAjaxControl;
                if (parentAjaxControl == null) continue;

                // skip ajax parent controls which do not have state manager
                var stateManager = parentAjaxControl.StateManager;
                if (stateManager == null) continue;

                // raise the event.
                if (stateManager.OnSnapshotTakenEvent(ajaxControl, EventArgs.Empty))
                    break;
            }
        }

        /// <summary>
        /// Formats specified <paramref name="color"/> as <see cref="String"/> using RRGGBBAA format.
        /// </summary>
        private static string FormatColor(Color color)
        {
            if (color.IsEmpty) return string.Empty;
            return string.Concat("'",
                                 color.R.ToString("X2", null),
                                 color.G.ToString("X2", null),
                                 color.B.ToString("X2", null),
                                 color.A.ToString("X2", null),
                                 "'");
        }

        /// <summary>
        /// Formats the specified <paramref name="current"/> value as <see cref="string"/> for client-side evaluation.
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="initial">Initial value.</param>
        /// <param name="current">Current value.</param>
        /// <param name="optimizeCommandArgument">True to optimize the current value.</param>
        /// <remarks>
        /// If optimization is requested, the difference from the <paramref name="initial"/> value and the difference index is returned.
        /// </remarks>
        /// <seealso cref="RenderChange{T}(T,T,string,bool)"/>.
        private string FormatValue<T>(T initial, T current, bool optimizeCommandArgument)
        {
            if (!optimizeCommandArgument)
                return FormatValue(current);

            var initialString = initial as string;
            var currentString = current as string;

            int index;
            var suffix = Utilities.GenerateDifference(initialString, currentString, out index);

            return index <= 1 ? FormatValue(current) : FormatString(suffix) + "," + index;
        }
    }
}
