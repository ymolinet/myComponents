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
    using System.Collections.Generic;
    using System.Web.UI;


    /// <summary>
    /// This abstract class has similar purpose as <see cref="ControlCollectionWrapper{T}"/> but instead of just creating
    /// a View over the real collection it will internally use a List and thereby speed up access dramatically. 
    /// </summary>
    public abstract class ControlCollectionWrapperCached<T> : ControlCollectionWrapper<T>, ICollection<T> where T : Control
    {
        #region [ -- Private Members -- ]

        private readonly IList<T> _thisCollection;
        
        #endregion

        #region [ -- Constructors -- ]

        ///<summary>
        /// Constructor. Initializes a new instance of <see cref="ControlCollectionWrapperCached{T}"/>
        ///</summary>
        ///<param name="owner">The owner Control</param>
        ///<param name="thisCollection">The collection containing the controls that are part of this collection wrapper</param>
        protected ControlCollectionWrapperCached(T owner, IList<T> thisCollection)
            : base(owner)
        {
            _thisCollection = thisCollection;
        }

        #endregion

        #region [ -- Overidden base class methods -- ]

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public new IEnumerator<T> GetEnumerator()
        {
            return _thisCollection.GetEnumerator();
        }

        /// <summary>
        /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection.
        /// </summary>
        /// <param name="child">The <see cref="T:System.Web.UI.Control"/> to add to the collection. 
        ///                 </param><exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="child"/> parameter does not specify a control. 
        ///                 </exception><exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only. 
        ///                 </exception>
        public override void Add(Control child)
        {
            IsValidControl(child);
            Collection.Add(child);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///                 </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///                 </exception>
        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        /// <summary>
        /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection at the specified index location.
        /// </summary>
        /// <param name="index">The location in the array at which to add the child control. 
        ///                 </param><param name="child">The <see cref="T:System.Web.UI.Control"/> to add to the collection. 
        ///                 </param><exception cref="T:System.ArgumentNullException">The <paramref name="child"/> parameter does not specify a control. 
        ///                 </exception><exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index"/> parameter is less than zero or greater than the <see cref="P:System.Web.UI.ControlCollection.Count"/> property. 
        ///                 </exception><exception cref="T:System.Web.HttpException">The <see cref="T:System.Web.UI.ControlCollection"/> is read-only. 
        ///                 </exception>
        public override void AddAt(int index, Control child)
        {
            IsValidControl(child);

            if (index == -1)
                Owner.Controls.Add(child);
            else
            {
                VerifyIndexInsideBounds(index);
                var idx = TranslateIndex(index);
                Owner.Controls.AddAt(idx, child);
            }
        }


        /// <summary>
        /// Gets a reference to the server control at the specified index location in the <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <returns>
        /// The reference to the control.
        /// </returns>
        /// <param name="index">The location of the server control in the <see cref="T:System.Web.UI.ControlCollection"/>. 
        ///                 </param><exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index"/> parameter is less than zero or greater than or equal to <see cref="P:System.Web.UI.ControlCollection.Count"/>. 
        ///                 </exception>
        public override Control this[int index]
        {
            get { return _thisCollection[index]; }
        }

        /// <summary>
        /// Removes a child control, at the specified index location, from the <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="index">The ordinal index of the server control to be removed from the collection. 
        ///                 </param><exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only. 
        ///                 </exception>
        public override void RemoveAt(int index)
        {
            VerifyIndexInsideBounds(index);
            var idx = TranslateIndex(index);
            Owner.Controls.RemoveAt(idx);
        }

        /// <summary>
        /// Copies the child controls stored in the <see cref="T:System.Web.UI.ControlCollection"/> object to an <see cref="T:System.Array"/> object, beginning at the specified index location in the <see cref="T:System.Array"/>.
        /// </summary>
        /// <param name="array">The <see cref="T:System.Array"/> to copy the child controls to. 
        ///                 </param><param name="index">The zero-based relative index in <paramref name="array"/> where copying begins. 
        ///                 </param><exception cref="T:System.Web.HttpException"><paramref name="array"/> is not null and not one-dimensional. 
        ///                 </exception>
        public override void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            var idx = index;
            foreach (var validControl in _thisCollection)
            {
                array.SetValue(validControl, idx);
                ++idx;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.
        ///                 </param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.
        ///                 </param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.
        ///                 </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.
        ///                 </exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.
        ///                     -or-
        ///                 <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        ///                     -or-
        ///                     The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        ///                     -or-
        ///                     Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        ///                 </exception>
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Determines whether the specified server control is in the parent server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <returns>
        /// true if the specified server control exists in the collection; otherwise, false.
        /// </returns>
        /// <param name="c">The server control to search for in the collection. 
        ///                 </param>
        public override bool Contains(Control c)
        {
            return c as T != null && _thisCollection.Contains((T)c);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///                 </param>
        bool ICollection<T>.Contains(T item)
        {
            return Contains(item);
        }

        /// <summary>
        /// Removes all controls from the current server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        public override void Clear()
        {
            var collectionCopy = new List<T>(_thisCollection);

            foreach (var control in collectionCopy)
                Owner.Controls.Remove(control);
        }

        /// <summary>
        /// Retrieves the index of a specified <see cref="T:System.Web.UI.Control"/> object in the collection.
        /// </summary>
        /// <returns>
        /// The index of the specified server control. If the server control is not currently a member of the collection, it returns -1.
        /// </returns>
        /// <param name="value">The <see cref="T:System.Web.UI.Control"/> for which the index is returned. 
        ///                 </param>
        public override int IndexOf(Control value)
        {
            return _thisCollection.IndexOf((T)value);
        }

        /// <summary>
        /// Gets the number of server controls in the <see cref="ControlCollectionWrapperCached{T}"/> object for the specified ASP.NET server control.
        /// </summary>
        /// <returns>
        /// The number of server controls in the <see cref="ControlCollectionWrapperCached{T}"/>.
        /// </returns>
        public override int Count
        {
            get { return _thisCollection.Count; }
        }

        /// <summary>
        /// Removes the specified server control from the parent server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="value">The server control to be removed. 
        ///                 </param>
        public override void Remove(Control value)
        {
            IsValidControl((value));
            Owner.Controls.Remove(value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        ///                 </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        ///                 </exception>
        bool ICollection<T>.Remove(T item)
        {
            Remove(item);
            return true;
        } 

        #endregion

        #region [ -- Helper functions -- ]

        /// <summary>
        /// Helper function to translate the index of the underlying collection to the true index in the Owner Controls collection.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int TranslateIndex(int index)
        {
            var control = _thisCollection[index];
            var realIndex = Owner.Controls.IndexOf(control);
            return realIndex;
        }

        /// <summary>
        /// Helper function to verify that the index doesn't fall outside the underlying collection.
        /// </summary>
        /// <param name="index"></param>
        void VerifyIndexInsideBounds(int index)
        {
            if (index < 0 || index > _thisCollection.Count - 1)
                throw new IndexOutOfRangeException();
        }

        #endregion
    }
}