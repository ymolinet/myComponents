/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.IO;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

namespace Gaia.WebWidgets
{
    /// <summary>
    /// Specialized <see cref="HtmlTextWriter"/> for Button controls
    /// which removes postback references from the onclick attribute.
    /// </summary>
    /// <typeparam name="T">Button control type.</typeparam>
    internal sealed class ButtonControlWriter<T> : HtmlTextWriter where T : ASP.WebControl, IAjaxButtonControl
    {
        private readonly T _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonControlWriter{T}"/> class that uses a default tab string.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.IO.TextWriter"/> instance that renders the markup content.</param>
        /// <param name="owner">Owner button control.</param>
        public ButtonControlWriter(TextWriter writer, T owner)
            : base(writer)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            _owner = owner;
        }

        /// <summary>
        /// Adds the markup attribute and the attribute value to the opening tag of the element that the <see cref="T:System.Web.UI.HtmlTextWriter"/> object 
        /// creates with a subsequent call to the <see cref="HtmlTextWriter.RenderBeginTag(string)"/> method.
        /// </summary>
        /// <param name="key">
        /// An <see cref="T:System.Web.UI.HtmlTextWriterAttribute"/> that represents the markup attribute to add to the output stream. 
        /// </param>
        /// <param name="value">A string containing the value to assign to the attribute.</param>
        public override void AddAttribute(HtmlTextWriterAttribute key, string value)
        {
            var attributeValue = value;

            if (key == HtmlTextWriterAttribute.Onclick)
            {
                var postBackOptions = _owner.GetPostBackOptions();
                var postBackReference = _owner.Page.ClientScript.GetPostBackEventReference(postBackOptions, false);

                if (postBackReference != null)
                {
                    attributeValue = value.Replace(postBackReference, String.Empty);
                    if (attributeValue == "javascript:") return;
                }
            }

            base.AddAttribute(key, attributeValue);
        }
    }
}
