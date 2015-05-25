/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets
{
    using System;

    /// <summary>
    /// Base EventArgs for Mouse related events
    /// </summary>
    public abstract class MouseEventArgs : EventArgs
    {
        private readonly int _top;
        private readonly int _left;
        private readonly bool _controlKeyPressed;
        private readonly bool _shiftKeyPressed;
        private readonly bool _altKeyPressed;

        /// <summary>
        /// Returns True if the Control key was pressed during the click event
        /// </summary>
        public bool CtrlKey { get { return _controlKeyPressed; } }

        /// <summary>
        /// Returns True if the Shift key was pressed during the click event
        /// </summary>
        public bool ShiftKey { get { return _shiftKeyPressed; } }

        /// <summary>
        /// Returns true if the Alt key was pressed during the click event
        /// </summary>
        public bool AltKey { get { return _altKeyPressed; } }

        /// <summary>
        /// Constructor to be used with derived classes.
        /// </summary>
        /// <param name="left">Left position</param>
        /// <param name="top">Top position</param>
        /// <param name="controlKeys">Encoded control keys (Shift/Control/Alt)</param>
        protected MouseEventArgs(int left, int top, int controlKeys)
        {
            _top = top;
            _left = left; 

             _shiftKeyPressed = (controlKeys & 1) == 1;
             _controlKeyPressed = (controlKeys & 2) == 2;
             _altKeyPressed = (controlKeys & 4) == 4;
        }

        /// <summary>
        /// Offset from top of browser view
        /// </summary>
        public int Top
        {
            get { return _top; }
        }

        /// <summary>
        /// Offset from left of browser view
        /// </summary>
        public int Left
        {
            get { return _left; }
        }
    }
}