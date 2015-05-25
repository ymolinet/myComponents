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

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Specialized interface for such <see cref="PropertyStateManagerControl"/> 
    /// which may require rerendering of associated <see cref="Control"/> instead of serializing changes.
    /// </summary>
    public interface IMayRequireRerendering
    {
        /// <summary>
        /// Returns true if associated <see cref="Control"/> needs to be rerendered.
        /// </summary>
        bool RequiresRerendering { get; }
    }
}
