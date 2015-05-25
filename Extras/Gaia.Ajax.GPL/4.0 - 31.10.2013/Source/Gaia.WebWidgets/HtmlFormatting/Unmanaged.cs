/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets.HtmlFormatting
{
    /// <summary>
    /// Helper class for encapsulating a section where you don't want validation on the XhtmlTextWriter. This
    /// is useful if you *must* break XHTML compliance or add up "unsafe attributes" to a Tag etc. Don't use
    /// this class unless you REALLY have to since it stops all validation towards XHTML compliance if you do.
    /// Implements the IDisposable pattern which makes it very easy by using the "using" statement to turn
    /// it "off" and "on" on a very specific scope.
    /// </summary>
    public class Unmanaged : AtomicInvoker
    {
        private readonly Tag _tag;
        private readonly bool _wasDisabled;

        /// <summary>
        /// Constructor, will set the underlying XhtmlTextWriter (or tag) in "non-validating" mode.
        /// Will restore the initial mode after disposal
        /// </summary>
        /// <param name="tag">Tag or XhtmlTextWriter to set to "non-validating" mode</param>
        public Unmanaged(Tag tag)
        {
            // initialization
            _tag = tag;
            _wasDisabled = tag.DisableValidation;
            tag.DisableValidation = true;

            // destruction
            Destructor = delegate { _tag.DisableValidation = _wasDisabled; };
        }
    }
}
