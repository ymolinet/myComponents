/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets
{
    internal static class HtmlFormatter
    {
        /// <summary>
        /// Formats the specified <paramref name="value"/> to use for innerHtml assignment.
        /// </summary>
        public static string FormatHtmlForInnerHTML(string value)
        {
            return value == null
                       ? null
                       : value.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n");
        }

        /// <summary>
        /// Formats the specified <paramref name="value"/> to use for attribute assignment.
        /// </summary>
        public static string FormatHtmlForInnerAttribute(string value)
        {
            return value == null
                       ? null
                       : value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
        }
    }
}
