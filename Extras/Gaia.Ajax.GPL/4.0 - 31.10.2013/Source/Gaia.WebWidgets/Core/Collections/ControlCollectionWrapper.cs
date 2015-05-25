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
using System.Collections;
using System.Collections.Generic;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// An abstract control collection wrapper that implements <see cref="ICollection{T}"/> over a normal <see cref="ControlCollection"/>. 
    /// The wrapper will only work on top of an existing <see cref="ControlCollection"/> and you can create a custom implementation in a derived class.     
    /// </summary>
    /// <example>
    /// Examples include <see cref="ControlCollectionOf{T}"/> and <see cref="ControlCollectionExcept{T}"/>.
    /// </example>
    /// <typeparam name="T">Generic <see cref="Control"/> if specialization is needed on the collection wrapper</typeparam>
    public abstract class ControlCollectionWrapper<T> : ControlCollection, ICollection<T> where T : Control
    {
        #region [ -- Enumerator definition -- ]

        /// <summary>
        /// Specialized <see cref="IEnumerator{T}"/> for wrapper.
        /// </summary>
        private sealed class WrapperEnumerator : IEnumerator<T>
        {
            private readonly IEnumerator _enumerator;
            private readonly ControlCollectionWrapper<T> _owner;

            /// <summary>
            /// Returns position of the <see cref="Current"/> element
            /// in the underlying collection of the wrapper.
            /// </summary>
            public int Position { get; private set; }

            /// <summary>
            /// Constructor
            /// </summary>
            public WrapperEnumerator(ControlCollectionWrapper<T> owner)
            {
                if (owner == null)
                    throw new ArgumentNullException("owner");

                _owner = owner;
                _enumerator = _owner.Collection.GetEnumerator();
                Reset();
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose() { }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// True if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public bool MoveNext()
            {
                while (_enumerator.MoveNext())
                {
                    ++Position;
                    var control = _enumerator.Current as T;
                    
                    if (control == null || !_owner.IsValidControl(control)) continue;
                    Current = control;
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                Position = -1;
                Current = null;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public T Current { get; private set; }

            /// <summary>Gets the current element in the collection.</summary>
            /// <returns>The current element in the collection.</returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// The enumerator is positioned before the first element of the collection or after the last element.
            /// </exception>
            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        #endregion

        private readonly T _ownerControl;
        private ControlCollection _collection;

        /// <summary>
        /// Collection which is wrapped.
        /// </summary>
        protected ControlCollection Collection
        {
            get { return _collection ?? (_collection = Owner.Controls); }
        }

        /// <summary>
        /// The control that owns the underlying collection.
        /// </summary>
        [Obsolete("Use ControlCollectionWrapper<T>.Owner instead.", true)]
        protected T OwnerControl
        {
            get { return _ownerControl; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ownerControl">The owner of the controls collection</param>
        protected ControlCollectionWrapper(T ownerControl)
            : base(ownerControl)
        {
            _ownerControl = ownerControl;
        }

        /// <summary>
        /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection.
        /// </summary>
        /// <param name="child">The <see cref="T:System.Web.UI.Control"/> to add to the collection.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="child"/> parameter does not specify a control.</exception>
        /// <exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the control is not of type <typeparamref name="T"/> or is not valid for this container.</exception>
        public override void Add(Control child)
        {
            VerifyControl(child);
            Collection.Add(child);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        /// <seealso cref="Add"/>
        void ICollection<T>.Add(T item)
        {
            Add(item);
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
        /// <exception cref="InvalidOperationException">Thrown if the control is not of type <typeparamref name="T"/> or is not valid for this container.</exception>
        public override void AddAt(int index, Control child)
        {
            VerifyControl(child);

            var position = -1;
            if (index < 0 && index != -1)
                throw new ArgumentOutOfRangeException("index");

            if (index != -1)
            {
                var idx = -1;

                var enumerator = (WrapperEnumerator) GetEnumerator();
                while (enumerator.MoveNext() && idx != index)
                {
                    ++idx;
                    position = enumerator.Position;
                }

                if (idx != index)
                {
                    if (idx == -1 || index != idx + 1)
                        throw new ArgumentOutOfRangeException("index");

                    // index == idx + 1, so need to add at the next position.
                    ++position;
                }
            }

            Collection.AddAt(position, child);
        }

        /// <summary>
        /// Removes all controls from the current server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        public override void Clear()
        {
            var indices = new Queue<int>();

            var removedCount = 0;
            var enumerator = (WrapperEnumerator) GetEnumerator();
            while (enumerator.MoveNext())
            {
                indices.Enqueue(enumerator.Position - removedCount);
                ++removedCount;
            }

            while (indices.Count > 0)
                Collection.RemoveAt(indices.Dequeue());
        }

        /// <summary>
        /// Determines whether the specified server control is in the parent server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <returns>
        /// True if the specified server control exists in the collection; otherwise, false.
        /// </returns>
        /// <param name="control">The server control to search for in the collection.</param>
        public override bool Contains(Control control)
        {
            return IndexOf(control) != -1;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// True if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        bool ICollection<T>.Contains(T item)
        {
            return Contains(item);
        }

        /// <summary>
        /// Copies the child controls stored in the <see cref="T:System.Web.UI.ControlCollection"/> object to an <see cref="T:System.Array"/> object, beginning at the specified index location in the <see cref="T:System.Array"/>.
        /// </summary>
        /// <param name="array">The <see cref="T:System.Array"/> to copy the child controls to.</param>
        /// <param name="index">The zero-based relative index in <paramref name="array"/> where copying begins.</param>
        /// <exception cref="T:System.Web.HttpException"><paramref name="array"/> is not null and not one-dimensional.</exception>
        public override void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            var idx = index;
            foreach(var validControl in this)
            {
                array.SetValue(validControl, idx);
                ++idx;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. 
        /// The <see cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.
        /// -or-
        /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// -or-
        /// Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of server controls in the <see cref="T:System.Web.UI.ControlCollection"/> object for the specified ASP.NET server control.
        /// </summary>
        /// <returns>
        /// The number of server controls in the <see cref="T:System.Web.UI.ControlCollection"/>.
        /// </returns>
        public override int Count
        {
            get
            {
                var count = 0;
                
                var enumerator = GetEnumerator();
                while(enumerator.MoveNext())
                    ++count;
                
                return count;
            }
        }

        /// <summary>
        /// Retrieves the index of a specified <see cref="T:System.Web.UI.Control"/> object in the collection.
        /// </summary>
        /// <returns>
        /// The index of the specified server control. If the server control is not currently a member of the collection, it returns -1.
        /// </returns>
        /// <param name="value">The <see cref="T:System.Web.UI.Control"/> for which the index is returned.</param>
        public override int IndexOf(Control value)
        {
            var index = -1;
            
            var control = value as T;
            if (control != null)
            {
                var comparer = EqualityComparer<T>.Default;
                foreach (T validControl in this)
                {
                    ++index;
                    if (comparer.Equals(validControl, control))
                        return index;
                }
            }

            return index;
        }

        /// <summary>
        /// Retrieves an enumerator that can iterate through the <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <returns>
        /// The enumerator to iterate through the collection.
        /// </returns>
        public new IEnumerator<T> GetEnumerator()
        {
            return new WrapperEnumerator(this);
        }

        /// <summary>
        /// Removes the specified server control from the parent server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="value">The server control to be removed.</param>
        public override void Remove(Control value)
        {
            Collection.Remove(value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// True if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        bool ICollection<T>.Remove(T item)
        {
            Remove(item);
            return true;
        }

        /// <summary>
        /// Removes a child control, at the specified index location, from the <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="index">The ordinal index of the server control to be removed from the collection.</param>
        /// <exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only.</exception>
        public override void RemoveAt(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");

            var idx = 0;
            var enumerator = (WrapperEnumerator) GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (idx != index)
                {
                    Collection.RemoveAt(enumerator.Position);
                    break;
                }

                ++idx;
            }

            throw new ArgumentOutOfRangeException("index");
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
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");

                var idx = 0;
                foreach(T validControl in this)
                {
                    if (idx == index) return validControl;
                    ++idx;
                }

                throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// True if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public new bool IsReadOnly
        {
            get { return Collection.IsReadOnly; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// True if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<T>.IsReadOnly
        {
            get { return Collection.IsReadOnly; }
        }

        /// <summary>
        /// Return true if specified <paramref name="control"/> is valid for this wrapped container.
        /// </summary>
        /// <param name="control">Control to check for validity.</param>
        /// <returns>True if specified <paramref name="control"/> is valid.</returns>
        /// <remarks>
        /// Used by derived classes to specify which controls are valid for this wrapped container.
        /// </remarks>
        /// <example>
        /// Concrete implementations can be found in <see cref="ControlCollectionOf{T}"/> and <see cref="ControlCollectionExcept{T}"/> classes.
        /// </example>
        protected abstract bool IsValidControl(Control control);

        /// <summary>
        /// Ensures <paramref name="control"/> is of type <typeparamref name="T"/> and is valid.
        /// </summary>
        private void VerifyControl(Control control)
        {
            if (control is T && IsValidControl(control)) return;
            throw new InvalidOperationException("Specified control is not valid for the container.");
        }
    }
}
