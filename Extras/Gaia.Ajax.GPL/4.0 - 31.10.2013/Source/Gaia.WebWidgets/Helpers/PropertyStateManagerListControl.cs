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
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Specialized <see cref="PropertyStateManagerControl"/> for controls inheriting from <see cref="ASP.ListControl"/>.
    /// </summary>
    public class PropertyStateManagerListControl : PropertyStateManagerWebControl
    {
        private bool _autoPostBack;
        private bool _causesValidation;
        private string _validationGroup;
        private List<ASP.ListItem> _state;
        private readonly ASP.ListControl _listControl;
        
        #region [ -- Constructors -- ]

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerListControl"/> for specified <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control to track changes for.</param>
        /// <remarks>
        /// Stores state information for the control and serializes changes during callback rendering.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the specified <paramref name="control"/> is null.</exception>
        public PropertyStateManagerListControl(ASP.ListControl control) : this(control, control.ClientID, null) { }

        /// <summary>
        /// Initializes new instance of <see cref="PropertyStateManagerListControl"/> for specified <paramref name="control"/>
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
        public PropertyStateManagerListControl(ASP.ListControl control, string clientId, IExtraPropertyCallbackRenderer extra)
            : base(control, clientId, extra)
        {
            _listControl = control;
        }

        #endregion

        /// <summary>
        /// Returns true if the <see cref="ASP.ListItem"/> in the <see cref="ASP.ListControl.Items"/> collection
        /// should be removed from client if <see cref="ASP.ListItem.Enabled"/> is false.
        /// </summary>
        protected virtual bool RemoveDisabled
        {
            get { return true; }
        }

        /// <summary>
        /// Marks the <see cref="ASP.ListItem.Selected"/> property as clean
        /// for each item in <see cref="ASP.ListControl.Items"/> collection in the state snapshot.
        /// </summary>
        public void ClearDirtySelection()
        {
            var count = Math.Min(_state.Count, _listControl.Items.Count);
            for (var index = 0; index < count; ++index)
                _state[index].Selected = _listControl.Items[index].Selected;
        }

        /// <summary>
        /// Assigns new state to this <see cref="PropertyStateManagerWebControl"/> by copying from specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source <see cref="PropertyStateManagerWebControl"/> to copy state from.</param>
        protected override void AssignState(PropertyStateManagerControl source)
        {
            base.AssignState(source);

            var stateManagerListControl = source as PropertyStateManagerListControl;
            if (stateManagerListControl == null) return;

            _state = stateManagerListControl._state;
            _autoPostBack = stateManagerListControl._autoPostBack;
            _validationGroup = stateManagerListControl._validationGroup;
            _causesValidation = stateManagerListControl._causesValidation;
        }

        /// <summary>
        /// Takes snapshot of the current state of the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void TakeSnapshot()
        {
            _autoPostBack = _listControl.AutoPostBack;
            _validationGroup = _listControl.ValidationGroup;
            _causesValidation = _listControl.CausesValidation;

            var collection = _listControl.Items;
            _state = new List<ASP.ListItem>(collection.Count);
            foreach (ASP.ListItem item in collection)
            {
                var clone = new ASP.ListItem(item.Text, item.Value, item.Enabled) {Selected = item.Selected};
                _state.Add(clone);
            }

            base.TakeSnapshot();
        }

        /// <summary>
        /// Detects changes between current state and saved state snapshot for the associated <see cref="ASP.WebControl"/>.
        /// </summary>
        protected override void DiffSnapshot()
        {
            DiffRender(_state);

            RenderChange(_autoPostBack, _listControl.AutoPostBack, "setAutoPostBack");
            RenderChange(_validationGroup, _listControl.ValidationGroup, Constants.SetValidationGroupFunctionName);
            RenderChange(_causesValidation, _listControl.CausesValidation, Constants.SetCausesValidationFunctionName);

            base.DiffSnapshot();
        }

        private void DiffRender(IList<ASP.ListItem> initialCollection)
        {
            var currentCollection = _listControl.Items;
            var initialCollectionCount = initialCollection.Count;
            var currentCollectionCount = currentCollection.Count;

            if (currentCollectionCount == 0 )
            {
                if (initialCollectionCount > 0)
                    Builder.Append(".clear()");
                
                return;
            }

            var additions = 0;
            var deletions = 0;
            var disabledCount = 0;
            var removeDisabled = RemoveDisabled;
            var min = Math.Min(currentCollectionCount, initialCollectionCount);

            for (var index = 0; index < min; ++index)
            {
                var current = currentCollection[index];
                var initial = initialCollection[index];
                var position = index + additions - deletions - disabledCount;

                if (removeDisabled && !initial.Enabled)
                    ++disabledCount;

                var enabledStateChanged = current.Enabled != initial.Enabled;

                if (removeDisabled && enabledStateChanged)
                {
                    if (current.Enabled)
                    {
                        RenderAddition(position, current);
                        ++additions;
                    }
                    else
                    {
                        RenderDeletion(position);
                        ++deletions;
                    }
                }
                else if ((!removeDisabled || current.Enabled) &&
                         (current.Selected != initial.Selected || current.Text != initial.Text || 
                          current.Value != initial.Value || enabledStateChanged))
                {
                    // list item was changed
                    RenderChange(position, current, initial);
                }
            }

            // check if new items added
            if (currentCollectionCount > initialCollectionCount)
            {
                for (var index = min; index < currentCollectionCount; ++index)
                {
                    var current = currentCollection[index];
                    if (removeDisabled && !current.Enabled)
                    {
                        ++disabledCount;
                        continue;
                    }
                    var position = index + additions - deletions - disabledCount;
                    RenderAddition(position, current);
                }
            }

            // check if initial items removed
            if (currentCollectionCount >= initialCollectionCount) return;
            for (var index = min; index < initialCollectionCount; ++index)
            {
                var position = index + additions - deletions - disabledCount;
                RenderDeletion(position);
                ++deletions;
            }
        }

        private void RenderAddition(int position, ASP.ListItem current)
        {
            Builder.AppendFormat(".add({0},{{text:{1},value:{2},selected:{3}}})",
                                 position,
                                 FormatValue(current.Text),
                                 FormatValue(current.Value),
                                 FormatValue(current.Selected));
        }

        private void RenderDeletion(int position)
        {
            Builder.Append(string.Concat(".remove(", position, ")"));
        }

        private void RenderChange(int position, ASP.ListItem current, ASP.ListItem initial)
        {
            var changes = new List<string>(4);

            string str = current.Text;
            if (str != initial.Text)
                changes.Add("text:" + FormatValue(str));

            str = current.Value;
            if (str != initial.Value)
                changes.Add("value:" + FormatValue(str));

            bool value = current.Selected;
            if (value != initial.Selected)
                changes.Add("selected:" + FormatValue(value));
            
            if (!RemoveDisabled)
            {
                value = current.Enabled;
                if (value != initial.Enabled)
                    changes.Add("enabled:" + FormatValue(value));
                else if (!value)
                    changes.Add("enabled:false");
            }

            Builder.AppendFormat(".change({0},{{{1}}})", position, string.Join(",", changes.ToArray()));
        }
    }
}
