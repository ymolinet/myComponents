/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.IO;
using System.Web.UI;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Specialized <see cref="HtmlTextWriter"/> for ForceAnUpdate scenarios.
    /// </summary>
    internal class ContainerHtmlTextWriter : ContainerHtmlTextWriterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerHtmlTextWriter"/> class that uses a default tab string.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.IO.TextWriter"/> instance that renders the markup content. 
        /// </param>
        public ContainerHtmlTextWriter(TextWriter writer) : base(writer) { }
    }
}
