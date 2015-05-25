/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Collections.Generic;
using System.Globalization;

namespace Gaia.WebWidgets
{
    using ObjectProperties = List<KeyValuePair<string, string>>;

    /// <summary>
    /// Core Abstract Builder class for clientside object construction. Offers a nice 
    /// convenient syntax for initializing clientside objects like controls, aspects and effects
    /// </summary>
    /// <typeparam name="T">The Concrete Implementation of the Register class</typeparam>
    public abstract class RegisterObject<T> where T: RegisterObject<T>
    {
        private readonly ObjectProperties _properties = new ObjectProperties();

        #region [ -- Protected Properties -- ]

        /// <summary>
        /// Used in derived classes to get or set object id.
        /// </summary>
        protected string ObjectID { get; set; }

        /// <summary>
        /// Used in derived classes to get or set object type.
        /// </summary>
        protected string ObjectType { get; set; }

        /// <summary>
        /// Used in derived classes to get object properties.
        /// </summary>
        protected ObjectProperties Properties
        {
            get { return _properties; }
        }

        #endregion

        /// <summary>
        /// Constructor. Takes the objecttype to construct based on it's objectid
        /// </summary>
        /// <param name="objectType">ObjectType (ie. Gaia.Window, Effect.Show)</param>
        /// <param name="objectId">ObjectID for the client-side construction</param>
        protected RegisterObject(string objectType, string objectId)
        {
            ObjectID = objectId;
            ObjectType = objectType;
        }


        /// <summary>
        /// Add a property to the RegisterObject script. This will render key/value pairs with or withour quotes
        /// around the value depending on wheter you set the third parameter to true.
        /// </summary>
        /// <param name="key">Property Key</param>
        /// <param name="value">Property Value</param>
        /// <param name="addQuotes">If true adds quotes '' around the value</param>
        /// <returns>itself</returns>
        public T AddProperty(string key, string value, bool addQuotes)
        {
            string tempVal = value;

            if (addQuotes)
            {
                tempVal = string.Concat("'", tempVal, "'");
            }
            _properties.Add(new KeyValuePair<string, string>(key, tempVal));
            return (T) this;
        }

        /// <summary>
        /// Add a property with value representation only. This will be rendered as {value here} in the script
        /// </summary>
        /// <param name="value">Property Value</param>
        /// <returns>itself</returns>
        public T AddProperty(string value)
        {
            return AddProperty(string.Empty, value, false);
        }


        /// <summary>
        /// Add a property to the RegisterObject script. This will be rendered as key:'value' in the script.
        /// If you want to remove the quotes '' around the value, please use the overload and set addQuotes to false
        /// </summary>
        /// <param name="key">Property Key</param>
        /// <param name="value">Property Value</param>
        /// <returns>itself</returns>
        public T AddProperty(string key, string value)
        {
            return AddProperty(key, value, true);
        }

        /// <summary>
        /// Add a property to the RegisterObject and convert bool true to the string representation of "true" or "false"
        /// </summary>
        /// <param name="key">Property Key</param>
        /// <param name="value">Property Value</param>
        /// <returns>itself</returns>
        public T AddProperty(string key, bool value)
        {
            string val = (value) ? "true" : "false";
            return AddProperty(key, val, false);
        }

        /// <summary>
        /// Add a property to the RegisterObject script and append quotes '' around the value parameter being an integer
        /// if specified through the addQuotes parameter
        /// </summary>
        /// <param name="key">Property key</param>
        /// <param name="value">Property value</param>
        /// <param name="addQuotes">Wheter to add quotes '' around the value</param>
        /// <returns>itself</returns>
        public T AddProperty(string key, int value, bool addQuotes)
        {
            return AddProperty(key, value.ToString(CultureInfo.InvariantCulture), addQuotes);
        }

        /// <summary>
        /// Add a property to the RegisterObject script and append quotes '' around the value parameter being an integer
        /// </summary>
        /// <param name="key">Property key</param>
        /// <param name="value">Property value</param>
        /// <returns>itself</returns>
        public T AddProperty(string key, int value)
        {
            return AddProperty(key, value.ToString(CultureInfo.InvariantCulture), false);
        }

        /// <summary>
        /// Add a property to the RegisterObject and converts decimal into a format that can be interpreted on the client
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T AddProperty(string key, decimal value)
        {
            return AddProperty(key, value.ToString("0.##", CultureInfo.InvariantCulture), false);
        }

        /// <summary>
        /// Add a property to the RegisterObject script if the first parameter evaluates to true. 
        /// If not the property is omitted alltogheter. 
        /// </summary>
        /// <param name="eval">The evaluation</param>
        /// <param name="key">Property key</param>
        /// <param name="value">Property Value</param>
        /// <returns>itself</returns>
        public T AddPropertyIfTrue(bool eval, string key, string value)
        {
            return eval ? AddProperty(key, value) : (T)this;
        }

        /// <summary>
        /// Add a property to the RegisterObject script if the first parameter evaluates to true. 
        /// If not the property is omitted alltogheter. The bool value parameter is translated
        /// into "true" or "false"
        /// </summary>
        /// <param name="eval">The evaluation</param>
        /// <param name="key">Property key</param>
        /// <param name="value">Property Value</param>
        /// <returns>itself</returns>
        public T AddPropertyIfTrue(bool eval, string key, bool value)
        {
            return eval ? AddProperty(key, value) : (T) this;
        }

        /// <summary>
        /// Add a property to the RegisterObject script if the first parameter evaluates to true. 
        /// If not the property is omitted alltogheter.
        /// </summary>
        /// <param name="eval">The evaluation</param>
        /// <param name="key">Property key</param>
        /// <param name="value">Property Value</param>
        /// <returns>itself</returns>
        public T AddPropertyIfTrue(bool eval, string key, int value)
        {
            return eval ? AddProperty(key, value) : (T) this;
        }

        /// <summary>
        /// Add a property to the RegisterObject script if the first parameter evaluates to true. 
        /// If not the property is omitted alltogheter.
        /// </summary>
        /// <param name="eval">The evaluation</param>
        /// <param name="key">Property key</param>
        /// <param name="value">Property Value</param>
        /// <param name="addQuotes">Specify if you want to append quotes to the value</param>
        /// <returns>itself</returns>
        public T AddPropertyIfTrue(bool eval, string key, string value, bool addQuotes)
        {
            return eval ? AddProperty(key, value, addQuotes) : (T)this;
        }

        /// <summary>
        /// Add a property to the RegisterObject script if the first parameter evaluates to true. 
        /// If not the property is omitted alltogheter.
        /// </summary>
        /// <param name="eval">The evaluation</param>
        /// <param name="key">Property key</param>
        /// <param name="value">Property Value</param>
        /// <returns>itself</returns>
        public T AddPropertyIfTrue(bool eval, string key, decimal value)
        {
            return eval ? AddProperty(key, value.ToString("0.##", CultureInfo.InvariantCulture), false) : (T) this;
        }

    }
}