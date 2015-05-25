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
    /// Implemented by controls which need to add custom change serialization to the <see cref="PropertyStateManagerControl"/>.
    /// </summary>
    public interface IExtraPropertyCallbackRenderer
    {
        /// <summary>
        /// Injects additional script code into the specified <paramref name="code"/> builder.
        /// </summary>
        /// <param name="code">An already built StringBuilder which you're supposed to render your data into</param>
        /// <remarks>
        /// The injected script code should have the following form: ".foo(bar)", where
        /// foo should be a method of the client-side Gaia Control object and must return the "this" argument for chaining.
        /// </remarks>
        void InjectPropertyChangesToCallbackResponse(StringBuilder code);
    }
}
