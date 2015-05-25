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
using ASP = System.Web.UI;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Helper class to make registering of ParentControl abstracted while adding Aspects into a Control
    /// </summary>
    public class AspectCollection : IList<IAspect>
    {
        private readonly IAspectableAjaxControl _owner;
        private readonly List<IAspect> _aspects;
        private readonly List<IAspect> _memberBoundAspects;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ctrl">ParentControl of List of aspects</param>
        public AspectCollection(IAspectableAjaxControl ctrl)
        {
            _owner = ctrl;
            _aspects = new List<IAspect>();
            _memberBoundAspects = new List<IAspect>();
        }

        #region IList<IAspect> Members

        /// <summary>
        /// Returns the index of the given item
        /// </summary>
        /// <param name="item">item to return index of</param>
        /// <returns></returns>
        public int IndexOf(IAspect item)
        {
            return _aspects.IndexOf(item);
        }

        /// <summary>
        /// Inserts a new item at the specific index
        /// </summary>
        /// <param name="index">index to insert item at</param>
        /// <param name="item">item to insert</param>
        public void Insert(int index, IAspect item)
        {
            VerifyAspectBeforeAdding(item);
            item.ParentControl = _owner;

            if (item is Aspect)
                (item as Aspect).Owner = this;
            
            _aspects.Insert(index, item);
        }

        private void VerifyAspectBeforeAdding(IAspect item)
        {
            // Checking to see (dirty hack) if the same Aspect is added twice...
            IAspect toRemove = null;
            foreach (IAspect idx in this)
            {
                if (!item.Equals(idx))
                    continue;

                // If the aspect is memberbound, we remove it in favor of custom added aspects which are given less precedence. 
                if (_memberBoundAspects.Contains(idx) && _aspects.Contains(item))
                    toRemove = idx;
                else
                    throw new ApplicationException("You cannot add the same aspect twice for the same widget");

            }

            if (toRemove != null)
                _aspects.Remove(toRemove);

        }

        /// <summary>
        /// Removes an item at given index
        /// </summary>
        /// <param name="index">What index to remove an item from</param>
        public void RemoveAt(int index)
        {
            // We need to retrieve the Aspect to remove it from the memberbound collection too
            IAspect aspect = _aspects[index]; 
            _memberBoundAspects.Remove(aspect);
            _aspects.RemoveAt(index);

        }

        /// <summary>
        /// Returns the Aspect at the given index
        /// </summary>
        /// <param name="index">index of item to return</param>
        /// <returns>IAspect at specific index</returns>
        public IAspect this[int index]
        {
            get { return _aspects[index]; }
            set
            {
                VerifyAspectBeforeAdding(value);
                value.ParentControl = _owner;

                if (value is Aspect)
                    (value as Aspect).Owner = this;

                _aspects[index] = value;
            }
        }

        #endregion

        #region ICollection<IAspect> Members

        /// <summary>
        /// Appends an item to the list of Aspects
        /// </summary>
        /// <param name="item">item to append</param>
        public void Add(IAspect item)
        {
            VerifyAspectBeforeAdding(item);
            item.ParentControl = _owner;

            if (item is Aspect)
                (item as Aspect).Owner = this;

            _aspects.Add(item);
        }

        /// <summary>
        /// Empties the list of Aspects
        /// </summary>
        public void Clear()
        {
            _aspects.Clear();
            _memberBoundAspects.Clear();
        }

        /// <summary>
        /// Returns true if given Aspect exists within the list of Aspects
        /// </summary>
        /// <param name="item">item to query for</param>
        /// <returns>true of item exists in the list of Aspects</returns>
        public bool Contains(IAspect item)
        {
            return _aspects.Contains(item);
        }

        /// <summary>
        /// Returns true if an aspect of a given type exists in the collection. Since most Aspects are
        /// supposed to only exist ONE instance of we created this method to easily be able to spot duplicates.
        /// </summary>
        /// <typeparam name="T">Type to search for</typeparam>
        /// <returns>true if list contains one instance of the given type</returns>
        public bool Contains<T>() where T : IAspect
        {
            foreach (IAspect idx in _aspects)
                if (idx is T)
                    return true;

            return false;
        }

        /// <summary>
        /// Copies the list of Aspects to the given array
        /// </summary>
        /// <param name="array">array to copy into</param>
        /// <param name="arrayIndex">index in array to start copying into</param>
        public void CopyTo(IAspect[] array, int arrayIndex)
        {
            _aspects.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns the number of Aspects in the list of Aspects
        /// </summary>
        public int Count
        {
            get { return _aspects.Count; }
        }

        // Hiding it since it's stupid to expose as part of the "API" or interface of the class.
        bool ICollection<IAspect>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Searches the list of Aspects for the given Aspect type and returns it
        /// </summary>
        /// <typeparam name="T">Type to find</typeparam>
        /// <returns>Aspect of T</returns>
        public T Find<T>() where T : class, IAspect
        {
            foreach (IAspect idx in _aspects)
            {
                if (idx is T)
                    return (T)idx;
            }
            return default(T);
        }

        /// <summary>
        /// Binds an aspect of a particular type and returns it. It's important to use the Bind function instead of the Add/Insert
        /// function if you intend to automatically map properties and events over custom addition of Aspects. The reason for this is
        /// that MemberBound aspects are given less priority than Custom Aspects added to the collection from the user. This makes
        /// it quite easy to override the mapped behavior with custom functionality. 
        /// </summary>
        /// <returns>The Aspect</returns>
        public T Bind<T>() where T : class, IAspect, new()
        {
            var aspect = Find<T>();
            if (aspect != null)
            {
                if (!_memberBoundAspects.Contains(aspect))
                    _memberBoundAspects.Add(aspect);
                return aspect;
            }

            aspect = new T();
            _memberBoundAspects.Add(aspect);
            Add(aspect);
            return aspect;
        }

        /// <summary>
        /// Binds an aspect of a particular type and returns it. It's important to use the Bind function instead of the Add/Insert
        /// function if you intend to automatically map properties and events over custom addition of Aspects. The reason for this is
        /// that MemberBound aspects are given less priority than Custom Aspects added to the collection from the user. This makes
        /// it quite easy to override the mapped behavior with custom functionality. 
        /// </summary>
        /// <param name="addAspect">If set to true will automatically find/create the aspect and mark it as bound. If set to false
        /// it will try to remove the Aspect if it was bound and return the aspect for further cleanup. If the Aspect was not
        /// bound and addAspect is set to false, the aspect is simply returned</param>
        /// <returns>The Aspect</returns>
        public T Bind<T>(bool addAspect) where T : class, IAspect, new()
        {
            // If true we attach the aspect and return it
            if (addAspect)
                return Bind<T>();

            // Here we try to find the aspect in the existing collection
            IAspect aspect = Find<T>();

            // If the aspect wasn't found, do nothing and return nothing
            if (aspect == null)
                return null;

            // If the aspect was member-bound, we remove the aspect
            if (_memberBoundAspects.Contains(aspect))
                Remove(aspect);

            // Here we return the aspect for potential cleanup purposes ... 
            return (T)aspect;

        }

        /// <summary>
        /// Returns true if a typeof Aspect is bound to a property/event
        /// </summary>
        /// <typeparam name="T">The Aspect of a given type</typeparam>
        /// <returns>If bound returns true, otherwise false.</returns>
        public bool IsBound<T>() where T : IAspect
        {
            foreach (IAspect idx in _aspects)
                if (idx is T && _memberBoundAspects.Contains(idx))
                    return true;
            
            return false;
        }

        /// <summary>
        /// Returns true if a particular Aspect is bound to a property/event
        /// </summary>
        /// <param name="item">The aspect instance</param>
        /// <returns>If bound returns true, otherwise false.</returns>
        public bool IsBound(IAspect item)
        {
            return _memberBoundAspects.Contains(item);
        }

        /// <summary>
        /// Removes given item from list of Aspects
        /// </summary>
        /// <param name="item">item to remove</param>
        /// <returns>true if removal was performed</returns>
        public bool Remove(IAspect item)
        {
            _memberBoundAspects.Remove(item);
            return _aspects.Remove(item);
        }

        /// <summary>
        /// Removes a given type from list of Aspects
        /// </summary>
        public bool Remove<T>() where T : IAspect
        {
            IAspect toRemove = null;
            foreach (IAspect idx in _aspects)
            {
                if (idx is T)
                    toRemove = idx;
            }
            return toRemove != null && Remove(toRemove);
        }

        #endregion

        #region IEnumerable<IAspect> Members

        /// <summary>
        /// Returns the enumerator or the list of Aspects
        /// </summary>
        /// <returns>Enumerator for the list of Aspects</returns>
        public IEnumerator<IAspect> GetEnumerator()
        {
            return _aspects.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _aspects.GetEnumerator();
        }

        #endregion
    }
}
