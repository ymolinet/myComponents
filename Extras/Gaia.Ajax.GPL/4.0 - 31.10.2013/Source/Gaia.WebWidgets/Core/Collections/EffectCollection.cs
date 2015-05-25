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
  
    /// <summary>
    /// Collection of Effects. 
    /// </summary>
    public class EffectCollection : ICollection<Effect>
    {
        private readonly List<Effect> _effects = new List<Effect>();
        private readonly Dictionary<string, Effect> _functions = new Dictionary<string, Effect>();

        internal List<Effect> Effects
        {
            get { return _effects; }
        }

        internal Dictionary<string, Effect> Functions
        {
            get { return _functions; }
        }

        /// <summary>
        /// Add an Effect to the collection of effects
        /// </summary>
        /// <param name="effect"></param>
        public void Add(Effect effect)
        {
            _effects.Add(effect);
        }
     
        /// <summary>
        /// Use this function when you want to Add a function to a specific event on the widget. The Event blocks can typically be
        /// found on each widget as static getters for each event. 
        /// </summary>
        /// <param name="effectEvent">The event to attach the effect to </param>
        /// <param name="effect">The effect to attach</param>
        public void Add(AjaxEffectEvent effectEvent, Effect effect)
        {   
            if (IsDefined(effectEvent))
                return;

            effectEvent.VerifyEffect(effect);

            IEffect e = effect;

            // an element parameter is not defined, create it! 
            if (!e.PropertyParameters.ContainsKey("element"))
                (effect as IEffect).PropertyParameters.Add("element", "jQuery(e.target).attr('id')");

            foreach (var prm in effectEvent.GetParameters())
                e.PropertyParameters.Add(prm.Key, prm.Value);

            EffectUtilsInternal.RecursivelyCopyPropertyParameters(effect); // copies all the parameters to the children

            _functions.Add(effectEvent.FunctionName, effect);
        }

        /// <summary>
        /// Returns true if the EffectEvent is defined in the Effect collection. 
        /// </summary>
        /// <param name="effectEvent">the Effectevent to use</param>
        /// <returns>True if defined in the collection</returns>
        public bool IsDefined(AjaxEffectEvent effectEvent)
        {
            return _functions.ContainsKey(effectEvent.FunctionName);
        }

        /// <summary>
        /// Clear all the effects (including EffectEvents) for this collection
        /// </summary>
        public void Clear()
        {
            _effects.Clear();
            _functions.Clear();
        }

        /// <summary>
        /// Determine if the given Effect exists in the collection, either as a direct
        /// effect or as an EffectEvent. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(Effect item)
        {
            return _effects.Contains(item) || _functions.ContainsValue(item);
        }

        void ICollection<Effect>.CopyTo(Effect[] array, int arrayIndex)
        {
            var all = new List<Effect>();
            all.AddRange(ListAll());
            all.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes a given Effect from the collection
        /// </summary>
        public bool Remove(Effect item)
        {
            return _effects.Remove(item) || TryRemoveEffectFromDictionary(item);
        }

        private bool TryRemoveEffectFromDictionary(Effect effectBase)
        {
            string key = string.Empty;
            foreach (var pair in _functions)
            {
                if (!pair.Value.Equals(effectBase)) 
                    continue;
                
                key = pair.Key;
                break;
            }

            return !string.IsNullOrEmpty(key) && _functions.Remove(key);
        }

        bool ICollection<Effect>.IsReadOnly
        {
            get { return false; }
        }
    
        /// <summary>
        /// Returns the number of effects in the collection, including EffectEvents
        /// </summary>
        public int Count
        {
            get { return _effects.Count + _functions.Count; }
        }

        #region Enumerable

        private IEnumerable<Effect> ListAll()
        {
            foreach (Effect eff in _effects)
            {
                yield return eff;
            }

            foreach (KeyValuePair<string, Effect> pair in _functions)
            {
                yield return pair.Value;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Effect> GetEnumerator()
        {
            return (IEnumerator<Effect>) ListAll();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}