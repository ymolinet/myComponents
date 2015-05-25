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
    /// Generic Controls Collection
    /// </summary>
    /// <typeparam name="T">Type of items</typeparam>
    public class GenericControlCollection<T> : ControlCollection, ICollection<T> where T : Control
    {
        /// <summary>
        /// Specialized <see cref="IEnumerator{T}"/> for <see cref="GenericControlCollection{T}"/>.
        /// </summary>
        private sealed class GenericControlCollectionEnumerator : IEnumerator<T>
        {
            private readonly IDisposable _disposable;
            private readonly IEnumerator _enumerator;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="enumerator">Underlying enumerator.</param>
            public GenericControlCollectionEnumerator(IEnumerator enumerator)
            {
                if (enumerator == null)
                    throw new ArgumentNullException("enumerator");

                _enumerator = enumerator;
                _disposable = enumerator as IDisposable;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_disposable == null) return;
                _disposable.Dispose();
            }

            /// <summary>Advances the enumerator to the next element of the collection.</summary>
            /// <returns>
            /// True if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                _enumerator.Reset();
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public T Current
            {
                get { return _enumerator.Current as T; }
            }

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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="owner">Owner control</param>
        public GenericControlCollection(Control owner) : base(owner) { }

        /// <summary>
        /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection.
        /// </summary>
        /// <param name="child">The <see cref="T:System.Web.UI.Control"/> to add to the collection.</param>
        /// <exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="child"/> parameter does not specify a control.</exception>
        /// <exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only.</exception>
        /// <exception cref="InvalidOperationException">Throw if <paramref name="child"/> is not of expected <typeparamref name="T"/> type.</exception>
        public override void Add(Control child)
        {
            Verify(child);
            base.Add(child);
        }

        /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
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
        /// <exception cref="InvalidOperationException">Throw if <paramref name="child"/> is not of expected <typeparamref name="T"/> type.</exception>
        public override void AddAt(int index, Control child)
        {
            Verify(child);
            base.AddAt(index, child);
        }

        /// <summary>
        /// Retrieves an enumerator that can iterate through the <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <returns>The enumerator to iterate through the collection.</returns>
        public new IEnumerator<T> GetEnumerator()
        {
            return new GenericControlCollectionEnumerator(base.GetEnumerator());
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
        /// Gets a reference to the server control at the specified index location in the <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <returns>The reference to the control.</returns>
        /// <param name="index">
        /// The location of the server control in the <see cref="T:System.Web.UI.ControlCollection"/>. 
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// The <paramref name="index"/> parameter is less than zero or greater than or equal to <see cref="P:System.Web.UI.ControlCollection.Count"/>. 
        /// </exception>
        public new T this[int index]
        {
            get { return base[index] as T; }
        }

        /// <summary>
        /// Checks if specified <paramref name="control"/> is of expected <typeparamref name="T"/> type.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if specified <paramref name="control"/> is not of the expected <typeparamref name="T"/> type.
        /// </exception>
        private static void Verify(Control control)
        {
            if (control is T) return;

            // this generic control collection does not allow other types than the specified one
            throw new InvalidOperationException("The Collection must only contain Controls of type " + typeof(T).FullName);
        }
    }
}