/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Text;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Renders a clientside representation of the aspect with construction and options/parameters.
    /// It has a nice syntax and allow you to do chaining of 
    /// function calls.
    /// Use this one if you create your own Ajax Extension Controls.
    /// </summary>
    public class RegisterAspect : RegisterObject<RegisterAspect>
    {
        #region [ -- Constructors -- ]

        /// <summary>
        /// Constructor. Takes the control-type to construct and its ClientID. 
        /// </summary>
        /// <param name="controltype">controltype (ie. Gaia.Window, Gaia.Button)</param>
        /// <param name="clientid">ClientID for the client-side construction</param>
        public RegisterAspect(string controltype, string clientid) : base(controltype, clientid) { }

        #endregion

        /// <summary>
        /// ClientID of the control to construct
        /// </summary>
        public string ClientId
        {
            get { return ObjectID;  }
            set { ObjectID = value; }
        }

        /// <summary>
        /// Type of Control to construct
        /// </summary>
        public string ControlType
        {
            get { return ObjectType; }
            set { ObjectType = value; }
        } 

        #region [ -- Public Methods -- ]

        /// <summary>
        /// Render the RegisterAspect script that is to be sent to the client 
        /// </summary>
        /// <returns>RegisterAspect script</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(string.Concat("new ", ObjectType, "('", ObjectID, "'"));

            // determine if there are properties to render
            sb.Append(Properties.Count == 0 ? ")" : ", {");

            // render the key/value properties
            for (int i = 0; i < Properties.Count; i++)
            {
                if (i != 0) sb.Append(", ");

                // if key is string.empty, we render neither the key nor the : (semicolon) 
                if (!string.IsNullOrEmpty(Properties[i].Key))
                    sb.Append(Properties[i].Key).Append(":");

                sb.Append(Properties[i].Value);
            }

            if (Properties.Count != 0)
                sb.Append("})");

            return sb.ToString();
        }

        #endregion
    }
}
