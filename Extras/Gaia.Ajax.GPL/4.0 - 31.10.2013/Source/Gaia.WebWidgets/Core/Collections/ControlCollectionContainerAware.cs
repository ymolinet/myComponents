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
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.UI;

    ///<summary>
    /// This specialized ControlCollection serves the same purpose as <see cref="ControlCollectionOf{T}"/> 
    /// and <see cref="ControlCollectionExcept{T}"/> combined but also will internally use Lists as a "cache" to avoid
    /// enumerating again and again over large lists. This is especially important in DRIMR scenarios.
    ///</summary>
    ///<typeparam name="T">The Container Type</typeparam>
    public abstract class ControlCollectionContainerAware<T> : ControlCollection where T : Control
    {
        #region [ -- Private Members -- ] 

        private readonly List<T> _containers;
        private readonly List<Control> _childControls; 

        #endregion

        #region [ -- Public Forwarder Collections -- ]

        /// <summary>
        /// In this Collection you will find all Controls of type <typeparamref name="T"></typeparamref>. 
        /// </summary>
        public ControlCollectionOf<T> Containers { get; private set; }

        /// <summary>
        /// In this Collection you will all other Controls except of type <typeparamref name="T"></typeparamref>
        /// </summary>
        public ControlCollectionExcept<T> ChildControls { get; private set; } 
        
        #endregion

        #region [ -- Constructors -- ]

        ///<summary>
        /// Constructor. Initializes a new instance of <see cref="ControlCollectionContainerAware{T}"/>
        ///</summary>
        protected ControlCollectionContainerAware(T owner)
            : base(owner)
        {
            _containers = new List<T>();
            _childControls = new List<Control>();

            Containers = new ControlCollectionOf<T>(owner, _containers);
            ChildControls = new ControlCollectionExcept<T>(owner, _childControls);

        } 

        #endregion

        #region [ -- Overriden base class functions -- ]

        /// <summary>
        /// Adds the specified <see cref="T:System.Web.UI.Control"/> object to the collection.
        /// </summary>
        /// <param name="child">The <see cref="T:System.Web.UI.Control"/> to add to the collection. 
        ///                 </param><exception cref="T:System.ArgumentNullException">Thrown if the <paramref name="child"/> parameter does not specify a control. 
        ///                 </exception><exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only. 
        ///                 </exception>
        public override void Add(Control child)
        {
            base.Add(child);
            ResolveList(child).Add(child);
        }

        /// <summary>
        /// Removes all controls from the current server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            _containers.Clear();
            _childControls.Clear();
        }

        /// <summary>
        /// Removes the specified server control from the parent server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="child">The server control to be removed. 
        ///                 </param>
        public override void Remove(Control child)
        {
            base.Remove(child);
            ResolveList(child).Remove(child);
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
            return ResolveList(c).Contains(c);
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
            base.AddAt(index, child);
            bool isContainer = child is T;
            IList list = isContainer ? (IList)_containers : _childControls;
            var newIndex = ResolveIndexInCollectionWrapper(list, index, isContainer);
            list.Insert(newIndex, child);
        }

        /// <summary>
        /// Removes a child control, at the specified index location, from the <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="index">The ordinal index of the server control to be removed from the collection. 
        ///                 </param><exception cref="T:System.Web.HttpException">Thrown if the <see cref="T:System.Web.UI.ControlCollection"/> is read-only. 
        ///                 </exception>
        public override void RemoveAt(int index)
        {
            var control = this[index];
            ResolveList(control).Remove(control);
            base.RemoveAt(index);
        } 
        #endregion

        #region [ -- Helper functions -- ]

        /// <summary>
        /// Helper function to resolve the correct underlying list of controls based on the <paramref name="control"/>.
        /// </summary>
        IList ResolveList(Control control)
        {
            return control is T ? (IList)_containers : _childControls;
        }

        /// <summary>
        /// Helper function to find the correct index in the correct collectionwrapper list. 
        /// </summary>
        int ResolveIndexInCollectionWrapper(IList collection, int index, bool includeContainer)
        {
            while (--index >= 0)
            {
                var isContainer = this[index] is T;

                bool canRetrieveIndex = isContainer ^ !includeContainer;
                if (canRetrieveIndex)
                    return collection.IndexOf(this[index]) + 1;
            }

            return 0;
        } 

        #endregion

    }
}