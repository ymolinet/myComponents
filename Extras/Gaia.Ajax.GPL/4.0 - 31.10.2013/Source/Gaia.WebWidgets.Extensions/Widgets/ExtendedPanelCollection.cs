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

namespace Gaia.WebWidgets.Extensions
{
    /// <summary>
    /// Represents a collection of <see cref="ExtendedPanel"/>.
    /// </summary>
    public class ExtendedPanelCollection : GenericControlList<ExtendedPanel>
    {
        private readonly Accordion _ownerAccordion;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">Owner control.</param>
        public ExtendedPanelCollection(Control owner) : base(owner)
        {
            _ownerAccordion = owner as Accordion;
        }

        /// <summary>
        /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection.
        /// </summary>
        /// <param name="child">The <see cref="T:System.Web.UI.Control"/> to add to the collection.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="child"/> parameter does not specify a control.</exception>
        /// <exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only.</exception>
        /// <exception cref="InvalidOperationException">Throw if <paramref name="child"/> is not of expected <see cref="ExtendedPanel"/> type.</exception>
        public override void Add(Control child)
        {
            SetOwner(child);
            base.Add(child);
        }

        /// <summary>
        /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection at the specified index location.
        /// </summary>
        /// <param name="index">The location in the array at which to add the child control.</param>
        /// <param name="child">The <see cref="T:System.Web.UI.Control"/> to add to the collection.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="child"/> parameter does not specify a control.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// The <paramref name="index"/> parameter is less than zero or greater than the <see cref="P:System.Web.UI.ControlCollection.Count"/> property.
        /// </exception>
        /// <exception cref="T:System.Web.HttpException">The <see cref="T:System.Web.UI.ControlCollection"/> is read-only.</exception>
        /// <exception cref="InvalidOperationException">Throw if <paramref name="child"/> is not of expected <see cref="ExtendedPanel"/> type.</exception>
        public override void AddAt(int index, Control child)
        {
            SetOwner(child);
            base.AddAt(index, child);
        }

        /// <summary>
        /// Removes the specified server control from the parent server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="value">The server control to be removed. </param>
        public override void Remove(Control value)
        {
            RemoveOwner(value);
            base.Remove(value);
        }

        /// <summary>
        /// Removes a child control, at the specified index location, from the <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="index">The ordinal index of the server control to be removed from the collection. </param>
        /// <exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only. </exception>
        public override void RemoveAt(int index)
        {
            Control child = null;
            if (index >= 0 && index < Count)
                child = this[index];

            base.RemoveAt(index);

            if (child == null)
                RemoveOwner(child);
        }

        /// <summary>
        /// Sets <see cref="ExtendedPanel.OwnerAccordion"/> for the specified <paramref name="panel"/>.
        /// </summary>
        private void SetOwner(Control panel)
        {
            if (_ownerAccordion == null) return;
            
            var accordionView = panel as ExtendedPanel;
            if (accordionView == null) return;

            accordionView.OwnerAccordion = _ownerAccordion;
        }

        /// <summary>
        /// Unsets <see cref="ExtendedPanel.OwnerAccordion"/> of the specified <paramref name="panel"/>
        /// </summary>
        private void RemoveOwner(Control panel)
        {
            if (_ownerAccordion == null) return;

            var accordionView = panel as ExtendedPanel;
            if (accordionView == null) return;

            accordionView.OwnerAccordion = null;
        }
    }
}
