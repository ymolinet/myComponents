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
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;

namespace Gaia.WebWidgets.Helpers
{
    /// <summary>
    /// Specialized <see cref="HtmlTextWriter"/> used during callback rendering
    /// to detect if a control does not render anything itself and just forwards
    /// rendering to its children.
    /// </summary>
    class CallbackWriter : HtmlTextWriter
    {
        /// <summary>
        /// Read-only collection of sibling controls.
        /// </summary>
        sealed class SiblingCollection : ControlCollection
        {
            private readonly IList<Control> _collection;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Web.UI.ControlCollection"/> class for the specified parent server control.
            /// </summary>
            /// <param name="owner">The ASP.NET server control that the control collection is created for. </param>
            /// <param name="collection">Collection of controls to wrap around.</param>
            /// <exception cref="T:System.ArgumentNullException">Occurs if the <paramref name="owner"/> or <paramref name="collection"/> parameters are null. </exception>
            public SiblingCollection(Control owner, IList<Control> collection) : base(owner)
            {
                if (collection == null)
                    throw new ArgumentNullException("collection");

                _collection = collection;
            }

            /// <summary>
            /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection.
            /// </summary>
            /// <param name="child">The <see cref="T:System.Web.UI.Control"/> to add to the collection. </param>
            /// <exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="child"/> parameter does not specify a control. </exception>
            /// <exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only. </exception>
            public override void Add(Control child)
            {
                ThrowCollectionReadOnly();
            }

            /// <summary>
            /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection at the specified index location.
            /// </summary>
            /// <param name="index">The location in the array at which to add the child control.</param>
            /// <param name="child">The <see cref="T:System.Web.UI.Control"/> to add to the collection.</param>
            /// <exception cref="T:System.ArgumentNullException">The <paramref name="child"/> parameter does not specify a control.</exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            /// The <paramref name="index"/> parameter is less than zero or greater than the <see cref="P:System.Web.UI.ControlCollection.Count"/> property.
            /// </exception><exception cref="T:System.Web.HttpException">The <see cref="T:System.Web.UI.ControlCollection"/> is read-only.</exception>
            public override void AddAt(int index, Control child)
            {
                ThrowCollectionReadOnly();
            }

            /// <summary>
            /// Removes all controls from the current server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
            /// </summary>
            public override void Clear()
            {
                ThrowCollectionReadOnly();
            }

            /// <summary>
            /// Determines whether the specified server control is in the parent server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
            /// </summary>
            /// <returns>
            /// true if the specified server control exists in the collection; otherwise, false.
            /// </returns>
            /// <param name="c">The server control to search for in the collection. </param>
            public override bool Contains(Control c)
            {
                return _collection.Contains(c);
            }

            /// <summary>
            /// Retrieves the index of a specified <see cref="T:System.Web.UI.Control"/> object in the collection.
            /// </summary>
            /// <returns>
            /// The index of the specified server control. If the server control is not currently a member of the collection, it returns -1.
            /// </returns>
            /// <param name="value">The <see cref="T:System.Web.UI.Control"/> for which the index is returned. </param>
            public override int IndexOf(Control value)
            {
                return _collection.IndexOf(value);
            }

            /// <summary>
            /// Retrieves an enumerator that can iterate through the <see cref="T:System.Web.UI.ControlCollection"/> object.
            /// </summary>
            /// <returns>
            /// The enumerator to iterate through the collection.
            /// </returns>
            public override IEnumerator GetEnumerator()
            {
                return _collection.GetEnumerator();
            }

            /// <summary>
            /// Copies the child controls stored in the <see cref="T:System.Web.UI.ControlCollection"/> object to an <see cref="T:System.Array"/> object, beginning at the specified index location in the <see cref="T:System.Array"/>.
            /// </summary>
            /// <param name="array">The <see cref="T:System.Array"/> to copy the child controls to.</param>
            /// <param name="index">The zero-based relative index in <paramref name="array"/> where copying begins.</param>
            /// <exception cref="T:System.Web.HttpException"><paramref name="array"/> is not null and not one-dimensional.</exception>
            public override void CopyTo(Array array, int index)
            {
                _collection.CopyTo((Control[])array, index);
            }

            /// <summary>
            /// Removes a child control, at the specified index location, from the <see cref="T:System.Web.UI.ControlCollection"/> object.
            /// </summary>
            /// <param name="index">The ordinal index of the server control to be removed from the collection.</param>
            /// <exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only.</exception>
            public override void RemoveAt(int index)
            {
                ThrowCollectionReadOnly();
            }

            /// <summary>
            /// Removes the specified server control from the parent server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
            /// </summary>
            /// <param name="value">The server control to be removed. </param>
            public override void Remove(Control value)
            {
                ThrowCollectionReadOnly();
            }

            /// <summary>
            /// Gets the number of server controls in the <see cref="T:System.Web.UI.ControlCollection"/> object for the specified ASP.NET server control.
            /// </summary>
            /// <returns>
            /// The number of server controls in the <see cref="T:System.Web.UI.ControlCollection"/>.
            /// </returns>
            public override int Count
            {
                get { return _collection.Count; }
            }

            /// <summary>
            /// Gets a reference to the server control at the specified index location in the <see cref="T:System.Web.UI.ControlCollection"/> object.
            /// </summary>
            /// <returns>
            /// The reference to the control.
            /// </returns>
            /// <param name="index">The location of the server control in the <see cref="T:System.Web.UI.ControlCollection"/>.</param>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            /// The <paramref name="index"/> parameter is less than zero or greater than or equal to <see cref="P:System.Web.UI.ControlCollection.Count"/>.
            /// </exception>
            public override Control this[int index]
            {
                get { return _collection[index]; }
            }

            /// <summary>
            /// Throws <see cref="HttpException"/>.
            /// </summary>
            private static void ThrowCollectionReadOnly()
            {
                throw new HttpException("Collection is read-only.");
            }
        }

        private long _position;
        private readonly Stream _stream;
        private readonly Control _control;
        private readonly List<Control> _siblings;
        private readonly IAjaxControl _ajaxControl;
        private readonly ControlCollection _controls;
        private Dictionary<Control, int> _replacements;
        private List<IAjaxControl> _unresolvedAjaxControls;
        private readonly SiblingCollection _readOnlySiblings;

        /// <summary>Initializes a new instance of the <see cref="T:Gaia.WebWidgets.Helpers.CallbackWriter"/> class that uses a default tab string.</summary>
        /// <param name="writer">The <see cref="T:System.IO.TextWriter"/> instance that renders the markup content.</param>
        /// <param name="writerStream">The <see cref="T:System.IO.Stream"/> instance used in <see cref="T:System.IO.TextWriter"/></param>
        /// <param name="ajaxControl">The <see cref="T:Gaia.WebWidgets.IAjaxControl"/> which created this writer</param>
        public CallbackWriter(TextWriter writer, Stream writerStream, IAjaxControl ajaxControl) : base(writer)
        {
            _stream = writerStream;
            _ajaxControl = ajaxControl;
            _position = _stream.Position;
            _control = _ajaxControl.Control;
            
            _controls = _control.Controls;
            _siblings = new List<Control>(_controls.Count);
            foreach (Control control in _controls)
                _siblings.Add(control);

            _readOnlySiblings = new SiblingCollection(_control, _siblings);
        }

        /// <summary>
        /// Owner ajax control
        /// </summary>
        public IAjaxControl AjaxControl
        {
            get { return _ajaxControl; }
        }

        /// <summary>
        /// Returns true if there was something written 
        /// to this writer.
        /// </summary>
        /// <returns></returns>
        public bool HasContent()
        {
            Flush();
            return _stream.Position > _position && !HasEmptyContent();
        }

        public ControlCollection Siblings
        {
            get { return _readOnlySiblings; }
        }

        public IEnumerable<IAjaxControl> UnresolvedAjaxControls
        {
            get { return _unresolvedAjaxControls; }
        }

        /// <summary>
        /// Resets writer
        /// </summary>
        public void Reset()
        {
            Flush();
            _position = _stream.Position;
        }

        /// <summary>
        /// Registers <paramref name="ajaxControl"/> as unresolved,
        /// meaning DRIMR logic could not find any way to render this control.
        /// </summary>
        public void RegisterUnresolved(IAjaxControl ajaxControl)
        {
            if (_unresolvedAjaxControls == null)
                _unresolvedAjaxControls = new List<IAjaxControl>();

            _unresolvedAjaxControls.Add(ajaxControl);
        }

        /// <summary>
        /// Registers <paramref name="ajaxControl"/> with this writer
        /// and returns its position.
        /// </summary>
        /// <returns>Position of <paramref name="ajaxControl"/>.</returns>
        public int Add(IAjaxControl ajaxControl)
        {
            if (_replacements == null)
                _replacements = new Dictionary<Control, int>();

            var replacement = ajaxControl.Control;
            var placeholder = replacement;
            
            // find the parent control which acts as a placeholder,
            // meaning it does not generate any markup and delegates
            // rendering to child control such as control being processed.
            var parent = placeholder.Parent;
            while (!ReferenceEquals(parent, _control))
            {
                placeholder = parent;
                parent = parent.Parent;
            }

            // control should assume being rendered in place of placeholder
            // so to find its position, we need to find its siblings and 
            // the position of the placeholder 
            var siblingCount = 0;
            var placeholderPosition = _controls.IndexOf(placeholder);
            var position = placeholderPosition;

            if (_replacements.Count > 0)
            {
                // if there are replacements for other controls
                // in the controls collection, offset should be added
                // to our position
                for (var index = 0; index <= placeholderPosition; ++index)
                {
                    int count;
                    if (!_replacements.TryGetValue(_controls[index], out count)) continue;
                    
                    if (index == placeholderPosition)
                        siblingCount += count;
                    
                    position += count;
                }
            }
            
            // try replacing the number of replacements for the placeholder
            if (!_siblings.Remove(placeholder))
            {
                ++position;
                _replacements[placeholder] = siblingCount + 1;
            }
            else
                _replacements[placeholder] = 0;
                
            // the control may be somewhere between siblings
            if (position < _siblings.Count)
                _siblings.Insert(position, replacement);
            else
                _siblings.Add(replacement);
            
            return position;
        }

        /// <summary>
        /// Checks if the content written to this writer
        /// is just whitespace which can be safely ignored.
        /// </summary>
        /// <returns>True if only whitespace was written to this writer.</returns>
        private bool HasEmptyContent()
        {
            long bufferSize = _stream.Position - _position;
            var buffer = new byte[bufferSize];
            _stream.Position = _position;
            _stream.Read(buffer, 0, (int)bufferSize);
            return System.Text.Encoding.Default.GetString(buffer).Trim().Length == 0;
        }
    }
}
