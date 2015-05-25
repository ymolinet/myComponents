/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Drawing;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Defines snapping configuration.
    /// </summary>
    public abstract class SnapConfigurationBase
    {
        /// <summary>
        /// Specifies delta values for horizontal and vertical snap.
        /// </summary>
        public Point Delta { get; set; }

        /// <summary>
        /// Specifies a function that receives the proposed new top left coordinate pair and 
        /// returns the coordinate pair to actually be used.
        /// </summary>
        /// <remarks>
        /// Function signature should be function(x, y, element).
        /// If both <see cref="Delta"/> and <see cref="ClientFunctionName"/> are specified, <see cref="ClientFunctionName"/> takes precedence.
        /// </remarks>
        public string ClientFunctionName { get; set; }

        /// <summary>
        /// Returns string representation of this configuration for registering as client-side option.
        /// </summary>
        /// <returns></returns>
        internal virtual string ToRegistrationOption()
        {
            var clientFunctionName = ClientFunctionName;
            if (!string.IsNullOrEmpty(clientFunctionName))
                return clientFunctionName;

            var delta = Delta;
            return !delta.IsEmpty ? string.Concat("[", delta.X, ",", delta.Y, "]") : null;
        }
    }
}