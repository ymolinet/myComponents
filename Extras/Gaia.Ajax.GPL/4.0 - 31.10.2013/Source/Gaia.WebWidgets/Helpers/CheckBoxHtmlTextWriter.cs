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
    /// Controls CheckBox/RadioButton markup generation 
    /// when base.RenderControl() is called
    /// </summary>
    internal class CheckBoxHtmlWriter : HtmlTextWriter
    {
        public CheckBoxHtmlWriter(TextWriter writer)
            : base(writer)
        { }

        public override void AddAttribute(HtmlTextWriterAttribute key, string value)
        {
            switch (key)
            {
                case HtmlTextWriterAttribute.Id:
                case HtmlTextWriterAttribute.For:
                    base.AddAttribute(key, value + "_ctl");
                    break;
                default:
                    base.AddAttribute(key, value);
                    break;
            }
        }

        protected override bool OnAttributeRender(string name, string value, HtmlTextWriterAttribute key)
        {
            return (TagKey != HtmlTextWriterTag.Span || key != HtmlTextWriterAttribute.Disabled) &&
                   base.OnAttributeRender(name, value, key);
        }
    }
}
