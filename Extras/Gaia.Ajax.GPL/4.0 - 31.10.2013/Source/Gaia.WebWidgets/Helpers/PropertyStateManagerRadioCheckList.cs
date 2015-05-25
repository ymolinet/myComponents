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
    /// Specialized <see cref="PropertyStateManagerListControl"/> for the <see cref="RadioButtonList"/> and <see cref="CheckBoxList"/> controls.
    /// </summary>
    public abstract class PropertyStateManagerRadioCheckList<T> : PropertyStateManagerListControl, IMayRequireRerendering
        where T : ASP.ListControl, IAjaxRadioCheckListControl
    {
        private int _count;
        private int _columns;
        private ASP.TextAlign _align;
        private ASP.RepeatLayout _layout;
        private ASP.RepeatDirection _direction;

        private readonly T _list;

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerRadioCheckList{T}"/> for specified <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control to track changes for.</param>
        /// <remarks>
        /// Stores state information for the control and serializes changes during callback rendering.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
        protected PropertyStateManagerRadioCheckList(T control) : this(control, control.ClientID, null) { }

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerRadioCheckList{T}"/> for specified <paramref name="control"/>
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
        protected PropertyStateManagerRadioCheckList(T control, string clientId, IExtraPropertyCallbackRenderer extra)
            : base(control, clientId, extra)
        {
            _list = control;
        }

        /// <summary>
        /// Returns true if the <see cref="ASP.ListItem"/> in the <see cref="ASP.ListControl.Items"/> collection
        /// should be removed from client if <see cref="ASP.ListItem.Enabled"/> is false.
        /// </summary>
        protected override bool RemoveDisabled
        {
            get { return false; }
        }

        /// <summary>
        /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
        protected override void AssignState(PropertyStateManagerControl source)
        {
            base.AssignState(source);

            var stateManager = source as PropertyStateManagerRadioCheckList<T>;
            if (stateManager == null) return;

            _count = stateManager._count;
            _align = stateManager._align;
            _layout = stateManager._layout;
            _columns = stateManager._columns;
            _direction = stateManager._direction;
        }

        /// <summary>
        /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void TakeSnapshot()
        {
            _align = _list.TextAlign;
            _count = _list.Items.Count;
            _layout = _list.RepeatLayout;
            _columns = _list.RepeatColumns;
            _direction = _list.RepeatDirection;

            base.TakeSnapshot();
        }

        /// <summary>
        /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void DiffSnapshot()
        {
            RenderChange(_align, _list.TextAlign, "setTextAlign");
            base.DiffSnapshot();
        }

        /// <summary>
        /// Returns true if associated <see cref="Control"/> needs to be rerendered.
        /// </summary>
        bool IMayRequireRerendering.RequiresRerendering
        {
            get
            {
                return _list.Items.Count != _count || _list.RepeatColumns != _columns ||
                       _list.RepeatDirection != _direction || _list.RepeatLayout != _layout;
            }
        }
    }
}
