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
using System.Text;
using System.Web.UI;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Base class for <see cref="HtmlTextWriter"/> classes which acts as
    /// </summary>
    internal abstract class ContainerHtmlTextWriterBase : HtmlTextWriter
    {
        private StringBuilder _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerHtmlTextWriterBase"/> class that uses a default tab string.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.IO.TextWriter"/> instance that renders the markup content. 
        /// </param>
        protected ContainerHtmlTextWriterBase(TextWriter writer) : base(writer) { }

        /// <summary>
        /// Renders specified <paramref name="script"/> before doing ForceAnUpdate.
        /// </summary>
        /// <param name="script">Script to render.</param>
        public void RenderForceAnUpdateRequirement(string script)
        {
            if (_builder == null)
                _builder = new StringBuilder();

            _builder.Append(script);
        }

        /// <summary>
        /// Returns required script for ForceAnUpdate.
        /// </summary>
        /// <remarks>
        /// Required scripts can be rendered using <see cref="RenderForceAnUpdateRequirement"/>.
        /// </remarks>
        public string ForceAnUpdateRequirements
        {
            get { return _builder == null ? null : _builder.ToString(); }
        }
    }
}
