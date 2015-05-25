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
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;

[assembly: WebResource("Gaia.WebWidgets.Scripts.AspectKey.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Aspect class for making elements trap the key clicking events.
    /// Element you attach this Aspect will raise the KeyPressed Event when a key is clicked while widget
    /// have focus.
    /// <br />
    /// Making key shortcuts available for your end users can increase 
    /// the productivity typical of repeating operations. Currently the key events listen on any Gaia input control like TextBox, 
    /// DropDownList that currently have focus. 
    /// </summary>
    /// <example>
    /// <code title="ASPX Markup for AspectKey Example" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectKey\Overview\Default.aspx"  />
    /// </code> 
    /// <code title="Using AspectKey to trap individual keys" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\Aspects\AspectKey\Overview\Default.aspx.cs" />
    /// </code>
    /// </example>
    public class AspectKey : Aspect<AspectKey>, IAspect
    {
        #region [ -- Private Members -- ]

        private readonly List<string> _filters = new List<string>();
        
        #endregion

        #region [ -- Enums -- ]

        /// <summary>
        /// Common KeyCodes. 
        /// </summary>
        public enum KeyCode
        {
            /// <summary>
            /// Undefined is an enum for a not defined KeyCode. Only the most common ones are included here
            /// </summary>
            UNDEFINED = -1,

            /// <summary>
            /// Backspace
            /// </summary>
            BACKSPACE = 8,

            /// <summary>
            /// Tab
            /// </summary>
            TAB = 9,

            /// <summary>
            /// Return/Enter Key
            /// </summary>
            RETURN = 13,

            /// <summary>
            /// Esc
            /// </summary>
            ESC = 27,

            /// <summary>
            /// Space
            /// </summary>
            SPACE = 32,

            /// <summary>
            /// Left
            /// </summary>
            LEFT = 37,

            /// <summary>
            /// Up
            /// </summary>
            UP = 38,

            /// <summary>
            /// Right
            /// </summary>
            RIGHT = 39,

            /// <summary>
            /// Down
            /// </summary>
            DOWN = 40,

            /// <summary>
            /// Delete
            /// </summary>
            DELETE = 46,

            /// <summary>
            /// Home
            /// </summary>
            HOME = 36,

            /// <summary>
            /// End
            /// </summary>
            END = 35,

            /// <summary>
            /// Page Up
            /// </summary>
            PAGEUP = 33,

            /// <summary>
            /// Page Down
            /// </summary>
            PAGEDOWN = 34,

            /// <summary>
            /// Insert
            /// </summary>
            INSERT = 45,

            /// <summary>
            /// Scroll
            /// </summary>
            SCROLL = 145,

            /// <summary>
            /// Capslock
            /// </summary>
            CAPSLOCK  = 20,

            /// <summary>
            /// Numlock
            /// </summary>
            NUMLOCK = 144,

            /// <summary>
            /// Pause
            /// </summary>
            PAUSE = 19,

            /// <summary>
            /// F1
            /// </summary>
            F1 = 112,

            /// <summary>
            /// F2
            /// </summary>
            F2 = 113,

            /// <summary>
            /// F3
            /// </summary>
            F3 = 114,

            /// <summary>
            /// F4
            /// </summary>
            F4 = 115,

            /// <summary>
            /// F5
            /// </summary>
            F5 = 116,

            /// <summary>
            /// F6
            /// </summary>
            F6 = 117,

            /// <summary>
            /// F7
            /// </summary>
            F7 = 118,

            /// <summary>
            /// F8
            /// </summary>
            F8 = 119,

            /// <summary>
            /// F9
            /// </summary>
            F9 = 120,

            /// <summary>
            /// F10
            /// </summary>
            F10 = 121,

            /// <summary>
            /// F11
            /// </summary>
            F11 = 122,

            /// <summary>
            /// F12
            /// </summary>
            F12 = 123,

            /// <summary>
            /// F13
            /// </summary>
            F13 = 0x7c,

            /// <summary>
            /// F14
            /// </summary>
            F14 = 0x7d,
            
            ///<summary>
            /// F15
            ///</summary>
            F15 = 0x7e,
            
            ///<summary>
            /// F16
            ///</summary>
            F16 = 0x7f,
            
            ///<summary>
            /// F17
            ///</summary>
            F17 = 0x80,
            
            ///<summary>
            /// F18
            ///</summary>
            F18 = 0x81,

            ///<summary>
            /// F19
            ///</summary>
            F19 = 130,
            
            ///<summary>
            /// F20
            ///</summary>
            F20 = 0x83,
            
            ///<summary>
            /// F21
            ///</summary>
            F21 = 0x84,
            
            ///<summary>
            /// F22
            ///</summary>
            F22 = 0x85,

            ///<summary>
            /// F23
            ///</summary>
            F23 = 0x86,
            
            /// <summary>
            /// F24
            /// </summary>
            F24 = 0x87,

            /// <summary>
            /// Slash /
            /// </summary>
            SLASH = 0x60,

            /// <summary>
            /// Hyhpen -
            /// </summary>
            HYPHEN = 0x6D,

            /// <summary>
            /// ADD (+)
            /// </summary>
            ADD = 0x6b,

            /// <summary>
            /// CONTROL
            /// </summary>
            CONTROL = 0x11,

            /// <summary>
            /// ALT
            /// </summary>
            ALT = 0x12,

            /// <summary>
            /// SHIFT
            /// </summary>
            SHIFT = 0x10,

            /// <summary>
            /// PrintScreen
            /// </summary>
            PRINTSCREEN = 0x2c,

            /// <summary>
            /// 0
            /// </summary>
            D0 = 0x30,
            
            /// <summary>
            /// 1
            /// </summary>
            D1 = 0x31,

            /// <summary>
            /// 2
            /// </summary>
            D2 = 50,

            /// <summary>
            /// 3
            /// </summary>
            D3 = 0x33,
            
            /// <summary>
            /// 4
            /// </summary>
            D4 = 0x34,
            
            /// <summary>
            /// 5
            /// </summary>
            D5 = 0x35,
            
            /// <summary>
            /// 6
            /// </summary>
            D6 = 0x36,
            
            /// <summary>
            /// 7
            /// </summary>
            D7 = 0x37,
            
            /// <summary>
            /// 8
            /// </summary>
            D8 = 0x38,
            
            /// <summary>
            /// 9
            /// </summary>
            D9 = 0x39,

            /// <summary>
            /// Numpad 0
            /// </summary>
            NUMPAD0 = 0x60,

            /// <summary>
            /// Numpad 1
            /// </summary>
            NUMPAD1 = 0x61,
            
            /// <summary>
            /// Numpad 2
            /// </summary>
            NUMPAD2 = 0x62,

            /// <summary>
            /// Numpad 3
            /// </summary>
            NUMPAD3 = 0x63,
            
            /// <summary>
            /// Numpad 4
            /// </summary>
            NUMPAD4 = 100,
            
            /// <summary>
            /// Numpad 5
            /// </summary>
            NUMPAD5 = 0x65,

            /// <summary>
            /// Numpad 6
            /// </summary>
            NUMPAD6 = 0x66,

            /// <summary>
            /// Numpad 7
            /// </summary>
            NUMPAD7 = 0x67,

            /// <summary>
            /// Numpad 8
            /// </summary>
            NUMPAD8 = 0x68,

            /// <summary>
            /// Numpad 9
            /// </summary>
            NUMPAD9 = 0x69,

            /// <summary>
            /// A
            /// </summary>
            A = 0x41,

            /// <summary>
            /// B
            /// </summary>
            B = 0x42,

            /// <summary>
            /// C
            /// </summary>
            C = 0x43,
            
            /// <summary>
            /// D
            /// </summary>
            D = 0x44,

            /// <summary>
            /// E
            /// </summary>
            E = 0x45,
            
            /// <summary>
            /// F
            /// </summary>
            F = 70,
            
            /// <summary>
            /// G
            /// </summary>
            G = 0x47,

            /// <summary>
            /// H
            /// </summary>
            H = 0x48,

            /// <summary>
            /// I
            /// </summary>
            I = 0x49,

            /// <summary>
            /// J
            /// </summary>
            J = 0x4a,

            /// <summary>
            /// K
            /// </summary>
            K = 0x4b,

            /// <summary>
            /// L
            /// </summary>
            L = 0x4c,

            /// <summary>
            /// M
            /// </summary>
            M = 0x4d,

            /// <summary>
            /// N
            /// </summary>
            N = 0x4e,

            /// <summary>
            /// O
            /// </summary>
            O = 0x4f,

            /// <summary>
            /// P
            /// </summary>
            P = 80,

            /// <summary>
            /// Q
            /// </summary>
            Q = 0x51,

            /// <summary>
            /// R
            /// </summary>
            R = 0x52,

            /// <summary>
            /// S
            /// </summary>
            S = 0x53,

            /// <summary>
            /// T
            /// </summary>
            T = 0x54,

            /// <summary>
            /// U
            /// </summary>
            U = 0x55,

            /// <summary>
            /// V
            /// </summary>
            V = 0x56,

            /// <summary>
            /// W
            /// </summary>
            W = 0x57,

            /// <summary>
            /// X
            /// </summary>
            X = 0x58,

            /// <summary>
            /// Y
            /// </summary>
            Y = 0x59,
            
            ///<summary>
            /// Z
            ///</summary>
            Z = 90
        }

        /// <summary>
        /// Represents a Key with its modifiers (shift, control, etc...)
        /// </summary>
        public class Key
        {
            internal Key(int key, int modifiers)
            {
                KeyValue = key;
                ShiftKey = (modifiers & 0x1) == 0x1;
                CtrlKey = (modifiers & 0x10) == 0x10;
                AltKey = (modifiers & 0x100) == 0x100;
            }

            internal Key(KeyCode key, bool shiftKey, bool ctrlKey, bool altKey)
            {
                KeyValue = (int)key;
                ShiftKey = shiftKey;
                CtrlKey = ctrlKey;
                AltKey = altKey;
            }

            /// <summary>
            /// Key value
            /// </summary>
            public int KeyValue { get; private set; }

            internal int Modifiers
            {
                get { return (ShiftKey ? 0x1 : 0) | (CtrlKey ? 0x10 : 0) | (AltKey ? 0x100 : 0); }
            }

            /// <summary>
            /// If a common Key was pressed ( Enter, Esc, UpArrow, etc ) you can retrieve the value as an enum here
            /// If any other character was pressed you can retrieve the character from the Key property. 
            /// </summary>
            public KeyCode KeyCode
            {
                get { return Enum.IsDefined(typeof(KeyCode), KeyValue) ? (KeyCode)Enum.Parse(typeof(KeyCode), KeyValue.ToString(NumberFormatInfo.InvariantInfo)) : KeyCode.UNDEFINED; }
            }

            /// <summary>
            /// True if Shift Key was pressed
            /// </summary>
            public bool ShiftKey { get; private set; }

            /// <summary>
            /// True if Control Key was pressed
            /// </summary>
            public bool CtrlKey { get; private set; }

            /// <summary>
            /// True if Alt Key was pressed
            /// </summary>
            public bool AltKey { get; private set; }
        }

        /// <summary>
        /// Key filter interface
        /// </summary>
        public interface IFilter
        {
            /// <summary>
            /// Filter registration javascript code
            /// </summary>
            string RegistrationText { get; }
        }

        /// <summary>
        /// Base class for all filters which support key event suppression.
        /// Suppression may include default action and/or event propogation preventions.
        /// </summary>
        public abstract class KeySuppressionFilter
        {
            private readonly bool _suppressDefaultAction;
            private readonly bool _suppressEventPropagation;

            internal bool SuppressDefaultAction
            {
                get { return _suppressDefaultAction; }
            }

            internal bool SuppressEventPropagation
            {
                get { return _suppressEventPropagation; }
            }

            internal KeySuppressionFilter(bool suppressDefaultAction, bool suppressEventPropagation)
            {
                _suppressDefaultAction = suppressDefaultAction;
                _suppressEventPropagation = suppressEventPropagation;
            }
        }

        /// <summary>
        /// Key code filters used with KeyUp and/or KeyDown events.
        /// </summary>
        public class KeyFilter : KeySuppressionFilter, IFilter
        {
            private readonly Key _key;

            /// <summary>
            /// Constructor for keycode filter.
            /// </summary>
            /// <param name="key">Allowed <see cref="KeyCode"/></param>
            public KeyFilter(KeyCode key) : this(key, false) { }

            /// <summary>
            /// Constructor for keycode filter.
            /// </summary>
            /// <param name="key">Allowed <see cref="KeyCode"/></param>
            /// <param name="shiftKey">True if shift key should also be up/down; otherwise false.</param>
            public KeyFilter(KeyCode key, bool shiftKey) : this(key, shiftKey, false) { }

            /// <summary>
            /// Constructor for keycode filter.
            /// </summary>
            /// <param name="key">Allowed <see cref="KeyCode"/></param>
            /// <param name="shiftKey">True if shift key should also be up/down; otherwise false.</param>
            /// <param name="ctrlKey">True if control key should also be up/down; otherwise false.</param>
            public KeyFilter(KeyCode key, bool shiftKey, bool ctrlKey) : this(key, shiftKey, ctrlKey, false) { }

            /// <summary>
            /// Constructor for keycode filter.
            /// </summary>
            /// <param name="key">Allowed <see cref="KeyCode"/></param>
            /// <param name="shiftKey">True if shift key should also be up/down; otherwise false.</param>
            /// <param name="ctrlKey">True if control key should also be also up/down; otherwise false.</param>
            /// <param name="altKey">True if alt key should also be up/down; otherwise false.</param>
            public KeyFilter(KeyCode key, bool shiftKey, bool ctrlKey, bool altKey) : this(key, shiftKey, ctrlKey, altKey, false) { }

            /// <summary>
            /// Constructor for keycode filter.
            /// </summary>
            /// <param name="key">Allowed <see cref="KeyCode"/></param>
            /// <param name="shiftKey">True if shift key should also be up/down; otherwise false.</param>
            /// <param name="ctrlKey">True if control key should also be also up/down; otherwise false.</param>
            /// <param name="altKey">True if alt key should also be up/down; otherwise false.</param>
            /// <param name="suppressDefaultAction">True if default action on key up/down should be prevented; otherwise, false.</param>
            public KeyFilter(KeyCode key, bool shiftKey, bool ctrlKey, bool altKey, bool suppressDefaultAction) 
                : this(key, shiftKey, ctrlKey, altKey, suppressDefaultAction, false) 
            { }

            /// <summary>
            /// Constructor for keycode filter.
            /// </summary>
            /// <param name="key">Allowed <see cref="KeyCode"/></param>
            /// <param name="shiftKey">True if shift key should also be up/down; otherwise false.</param>
            /// <param name="ctrlKey">True if control key should also be also up/down; otherwise false.</param>
            /// <param name="altKey">True if alt key should also be up/down; otherwise false.</param>
            /// <param name="suppressDefaultAction">True if default action on key up/down should be prevented; otherwise, false.</param>
            /// <param name="suppressEventPropagation">True if event propagaton on key up/down should be stopped; otherwise; false.</param>
            public KeyFilter(KeyCode key, bool shiftKey, bool ctrlKey, bool altKey, bool suppressDefaultAction, bool suppressEventPropagation) 
                : base(suppressDefaultAction, suppressEventPropagation)
            {
                _key = new Key(key, shiftKey, ctrlKey, altKey);
            }

            string IFilter.RegistrationText
            {
                get
                {
                    var suppress = (SuppressDefaultAction ? 0x1 : 0) | (SuppressEventPropagation ? 0x10 : 0);
                    var key = string.Join("#", new[] { "0", _key.KeyValue.ToString(NumberFormatInfo.InvariantInfo), _key.Modifiers.ToString(NumberFormatInfo.InvariantInfo) });
                    var keyText = string.Concat("'", key, "'");
                    return suppress == 0 ? keyText : string.Concat("[", keyText, ",", suppress, "]");
                }
            }
        }

        /// <summary>
        /// Character filter used with KeyPress event.
        /// </summary>
        public class KeyPressFilter : KeySuppressionFilter, IFilter
        {
            private readonly char _character;

            /// <summary>
            /// Constructor for character filter.
            /// </summary>
            /// <param name="character">Allowed character.</param>
            public KeyPressFilter(char character) : this(character, false, false) { }

            /// <summary>
            /// Constructor for character filter.
            /// </summary>
            /// <param name="character">Allowed character.</param>
            /// <param name="suppressDefaultAction">True if default action on key up/down should be prevented; otherwise, false.</param>
            public KeyPressFilter(char character, bool suppressDefaultAction) : this(character, suppressDefaultAction, false) { }

            /// <summary>
            /// Constructor for character filter.
            /// </summary>
            /// <param name="character">Allowed character.</param>
            /// <param name="suppressDefaultAction">True if default action on key up/down should be prevented; otherwise, false.</param>
            /// <param name="suppressEventPropagation">True if event propagaton on key up/down should be stopped; otherwise; false.</param>
            public KeyPressFilter(char character, bool suppressDefaultAction, bool suppressEventPropagation) : base(suppressDefaultAction, suppressEventPropagation)
            {
                _character = character;
            }

            string IFilter.RegistrationText
            {
                get
                {
                    var suppress = (SuppressDefaultAction ? 0x1 : 0) | (SuppressEventPropagation ? 0x10 : 0);
                    var key = string.Join("#", new[] {"1", ((int) _character).ToString(NumberFormatInfo.InvariantInfo), "0"});
                    var keyText = string.Concat("'", key, "'");
                    return suppress == 0 ? keyText : string.Concat("[", keyText, ",", suppress, "]");
                }
            }
        }

        #endregion

        #region [ -- EventArgs for Events -- ]

        /// <summary>
        /// EventArgs for the KeyDown and KeyUp Events.
        /// </summary>
        public class KeyEventArgs : EventArgs
        {
            internal KeyEventArgs(Key key)
            {
                Key = key;
            }

            /// <summary>
            /// Key pressed
            /// </summary>
            public Key Key { get; private set; }
        }

        /// <summary>
        /// EventArgs for the KeyPressed Event.
        /// </summary>
        public class KeyPressedEventArgs : EventArgs
        {
            internal KeyPressedEventArgs(char key)
            {
                Key = key;
            }

            /// <summary>
            /// Key pressed as char
            /// </summary>
            public char Key { get; private set; }
        }

        #endregion

        #region [ -- Properties -- ]

        /// <summary>
        /// If it is set to True the event propagation will stop.
        /// Otherwise key event will propagate to the parent(s).
        /// Default value is False.
        /// </summary>
        public bool SuppressEventPropagation { get; set; }

        /// <summary>
        /// If it is set to True the default actions
        /// will be stopped. Default value is False.
        /// </summary>
        public bool SuppressDefaultAction { get; set; }

        #endregion

        #region [ -- Events -- ]

        /// <summary>
        /// Listen to the actual keypress
        /// The events are done in the sequence: keydown, keypress, keyup
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        /// <summary>
        /// Listen to the keyup event occuring after the key is released
        /// The events are done in the sequence: keydown, keypress, keyup
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyUp;

        /// <summary>
        /// Listen to the keydown event occuring immediately when a key is pushed.
        /// The events are done in the sequence: keydown, keypress, keyup
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyDown;

        #endregion

        /// <summary>
        /// If you only want AspectKey to dispatch events on specific keys, you can specift them as an array here. 
        /// </summary>
        /// <param name="keyCodes">The KeyCodes to listen to</param>
        [Obsolete("Use AddFilter() with KeyFilter instance instead.")]
        public void SetKeyListeners(KeyCode[] keyCodes)
        {
            Array.ForEach(keyCodes, code => AddFilter(new KeyFilter(code)));
        }

        /// <summary>
        /// If you only want AspectKey to dispatch events based on specific characters typed in. 
        /// </summary>
        /// <param name="chars">The Chars to listen to</param>
        [Obsolete("Use AddFilter() with KeyPressFilter instance instead.")]
        public void SetKeyListeners(char[] chars)
        {
            Array.ForEach(chars, code => AddFilter(new KeyPressFilter(code)));
        }

        /// <summary>
        /// Here you can set a combination of KeyCodes and Chars to listen to. It will only trigger server events when
        /// a valid char or keycode is trapped. 
        /// </summary>
        /// <param name="keyCodes">The keycodes to listen to</param>
        /// <param name="chars">The chars to listen to</param>
        [Obsolete("Use AddFilter() with either KeyFilter or KeyPressFilter instance instead.")]
        public void SetKeyListeners(KeyCode[] keyCodes, char[] chars) {
            SetKeyListeners(keyCodes);
            SetKeyListeners(chars);
        }

        /// <summary>
        /// Adds speficied <see cref="IFilter"/>
        /// </summary>
        /// <param name="filters"><see cref="IFilter"/> to add</param>
        public void AddFilter(params IFilter[] filters)
        {
            Array.ForEach(filters, filter => _filters.Add(filter.RegistrationText));
        }

        #region [ -- Constructors -- ]

        /// <summary>
        /// Default constructor
        /// </summary>
        public AspectKey() : this(null) { }

        /// <summary>
        /// Constructor taking event handler for the KeyPressed event
        /// </summary>
        /// <param name="keyPressed">delegate called when key is clicked</param>
        public AspectKey(EventHandler<KeyPressedEventArgs> keyPressed)
        {
            KeyPressed += keyPressed;
        }

        #endregion

        [Method]
        internal void KeyMethod(string evt, int key, int modifiers)
        {
            switch (evt)
            {
                case "keydown":
                    if (KeyDown != null)
                        KeyDown(GetSender(), new KeyEventArgs(new Key(key, modifiers)));
                    break;

                case "keypress":
                    if (KeyPressed != null)
                        KeyPressed(GetSender(), new KeyPressedEventArgs((char)key));
                    break;

                case "keyup":
                    if (KeyUp != null)
                        KeyUp(GetSender(), new KeyEventArgs(new Key(key, modifiers)));
                    break;
            }
        }


        #region [ -- IAspect Implementation -- ]
        
        string IAspect.GetScript()
        {
            var keyup = KeyUp != null;
            var keydown = KeyDown != null;
            var keypress = KeyPressed != null;
            var hasEvents = keyup || keydown || keypress;

            var registerAspect = new RegisterAspect("Gaia.AspectKey", ParentControl.Control.ClientID);

            // Here we extract the keycodes from the enum and the char[] array and convert them into ints serialized to the client.
            // Since the keyup event is a low level keyboard event we convert the chars to upper case so they get handled anyway. 
            if (hasEvents && _filters.Count > 0)
                registerAspect.AddProperty("filters", "[" + string.Join(",", _filters.ToArray()) + "]", false);

            // denote which events to listen to clientside
            var events = new List<string>(3);
            if (keyup) events.Add("'keyup'");
            if (keydown) events.Add("'keydown'");
            if (keypress) events.Add("'keypress'");
            registerAspect.AddProperty("evts", "[" + string.Join(",", events.ToArray()) + "]", false);

            var suppress = (SuppressDefaultAction ? 0x1 : 0) | (SuppressEventPropagation ? 0x10 : 0);
            registerAspect.AddPropertyIfTrue(suppress != 0, "suppress", suppress);

            return registerAspect.ToString();            
        }

        /// <summary>
        /// Override in inherited classes to include javascript files.
        /// Do not forget to call base.IncludeScriptFiles()
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.AspectKey.js", typeof(Manager), "Gaia.AspectKey.browserFinishedLoading", true);
        }

        #endregion
    }
}
