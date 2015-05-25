/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Web.UI;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// This little State Utility is used on statebags to get and set values. It will by default return
    /// generic default values for structs or you can provide custom default values. When the value is 
    /// set to a default value it will not be persisted in the statebag. 
    /// </summary>
    public static class StateUtil
    {
        /// <summary>
        /// Get a value from the statebag
        /// </summary>
        /// <typeparam name="T">The type of the object in the statebag</typeparam>
        /// <param name="bag">The bag to look into</param>
        /// <param name="key">The key of the item</param>
        /// <returns>The object or the structs default value from the statebag</returns>
        public static T Get<T>(StateBag bag, string key)
        {
            object o = bag[key];
            if (o == null)
                return default(T);
            return (T)o;
        }

        /// <summary>
        /// Get a value from the statebag
        /// </summary>
        /// <typeparam name="T">The type of the object in the statebag</typeparam>
        /// <param name="bag">The bag to look into</param>
        /// <param name="key">The key of the item</param>
        /// <param name="defaultValue">Custom default value</param>
        /// <returns>The object or the structs default value from the statebag</returns>
        public static T Get<T>(StateBag bag, string key, T defaultValue)
        {
            object o = bag[key];
            if (o == null)
                return defaultValue;
            return (T)o;
        }

        /// <summary>
        /// Set a value in the statebag
        /// </summary>
        /// <param name="bag">The statebag to use</param>
        /// <param name="key">Define the key for storage</param>
        /// <param name="value">Set the value for the item in the statebag</param>
        public static void Set(StateBag bag, string key, object value)
        {
            bag[key] = value;
        }

        /// <summary>
        /// Set a value in the statebag
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="bag">The statebag to use</param>
        /// <param name="key">Define the key for storage</param>
        /// <param name="value">Set the value for the item in the statebag</param>
        /// <param name="defaultValue">Provide a custom default value. DefaultValues are not persisted.</param>
        public static void Set<T>(StateBag bag, string key, T value, T defaultValue)
        {
            bag[key] = value;
        }

    }
}
