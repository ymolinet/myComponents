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

namespace Gaia.WebWidgets.Helpers
{
    /// <summary>
    /// Specialized <see cref="CallbackWriter"/> used when the control is moved during callback.
    /// </summary>
    class MovedContainerHtmlTextWriter : CallbackWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Gaia.WebWidgets.Helpers.MovedContainerHtmlTextWriter"/> class that uses a default tab string.
        /// </summary>
        public MovedContainerHtmlTextWriter(TextWriter writer, Stream writerStream, IAjaxControl ajaxControl) : base(writer, writerStream, ajaxControl)
        {
        }
    }
}
