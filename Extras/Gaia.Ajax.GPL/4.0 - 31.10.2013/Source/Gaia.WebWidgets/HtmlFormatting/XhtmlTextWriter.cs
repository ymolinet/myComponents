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

namespace Gaia.WebWidgets.HtmlFormatting
{
    /// <summary>
    /// Class wrapping a TextWriter (e.g. HtmlTextWriter which is given during Render method of 
    /// System.Web.UI.WebControl.Control) for helping creating XHTML valid markup. It uses the 
    /// "using pattern" or the "IDisposable pattern" to make sure the HTML tags are opened and 
    /// closed at the correct place. Very useful in combination with XhtmlTagFactory to very easily
    /// create markup yourself either for your own Gaia Extension Controls or for any other places 
    /// where you need XHTML valid HTML markup and you want to create it with as few lines of code 
    /// as possible.
    /// </summary>
    public class XhtmlTextWriter
    {
        private bool _isFlushed;
        private readonly System.Collections.Generic.Stack<Tag> _tag = new System.Collections.Generic.Stack<Tag>();
        private readonly TextWriter _writer;

        /// <summary>
        /// Constructor, normally you would pass in your HtmlTextWriter here and then use the XhtmlTextWriter
        /// for easy creation of XHTML markup later.
        /// </summary>
        /// <param name="writer">Underlaying writer to write into</param>
        public XhtmlTextWriter(TextWriter writer)
        {
            _writer = writer;
            _isFlushed = true;
        }

        internal bool IsFlushed
        {
            get { return _isFlushed; }
            set { _isFlushed = value; }
        }

        internal void VerifyTagAllowed(Tag tag)
        {
            VerifyTagAllowed(tag, false);
        }

        internal void VerifyTagAllowed(Tag tag, bool disableValidation)
        {
            if (!disableValidation)
            {
                CheckForCommonErrors(tag);
                CheckForSpecialErrors(tag);
            }

            if (_tag.Count > 0)
            {
                Tag parentTag = _tag.Peek();
                parentTag.AddChild(tag);
            }

            _tag.Push(tag);
        }

        /// <summary>
        /// Casts the instance object to a TextWriter object, use when calling base and you need to "pass in" 
        /// the (Html)TextWriter object to base class implementation. It ensures that the writer is "flushed" 
        /// before casting which is necessary due to otherwise the stream might be "in the middle" of a element 
        /// string (not added the ending > bracket)
        /// </summary>
        /// <param name="rhs">XhtmlTextWriter to cast</param>
        /// <returns>Underlaying TextWriter (or HtmlTextWriter in fact normally)</returns>
        public static implicit operator TextWriter(XhtmlTextWriter rhs)
        {
            rhs.Flush();
            return rhs._writer;
        }

        internal void Write(string content)
        {
            _writer.Write(content);
        }

        private void CheckForSpecialErrors(Tag tag)
        {
            // Some tags requires special treatment...
            switch (tag.ElementTag)
            {
                case XhtmlTextWriterTag.A:
                    foreach (Tag idx in _tag)
                    {
                        if (idx.ElementTag == XhtmlTextWriterTag.A)
                            throw new ApplicationException("You can't have an a element inside another a element at any level");
                    }
                    break;
                case XhtmlTextWriterTag.Img:
                case XhtmlTextWriterTag.Object:
                case XhtmlTextWriterTag.Big:
                case XhtmlTextWriterTag.Small:
                case XhtmlTextWriterTag.Sub:
                case XhtmlTextWriterTag.Sup:
                    foreach (Tag idx in _tag)
                    {
                        switch (idx.ElementTag)
                        {
                            case XhtmlTextWriterTag.Pre:
                                throw new ApplicationException("You can't have an img, object, big, small, sub or sup element inside a pre element at any level");
                        }
                    }
                    break;
                case XhtmlTextWriterTag.Input:
                case XhtmlTextWriterTag.Select:
                case XhtmlTextWriterTag.TextArea:
                case XhtmlTextWriterTag.Label:
                case XhtmlTextWriterTag.Button:
                case XhtmlTextWriterTag.Form:
                case XhtmlTextWriterTag.FieldSet:
                    foreach (Tag idx in _tag)
                    {
                        switch (idx.ElementTag)
                        {
                            case XhtmlTextWriterTag.Button:
                                throw new ApplicationException("You can't have an input, select, textarea, label, button, form or fieldset element inside a button element at any level");
                        }
                        if (tag.ElementTag == XhtmlTextWriterTag.Form && idx.ElementTag == XhtmlTextWriterTag.Form)
                        {
                            throw new ApplicationException("You can't have form element inside another form element at any level");
                        }
                        if (tag.ElementTag == XhtmlTextWriterTag.Label && idx.ElementTag == XhtmlTextWriterTag.Label)
                        {
                            throw new ApplicationException("You can't have form element inside another form element at any level");
                        }
                    }
                    break;
            }
            foreach (var idx in _tag) { }
        }

        private void CheckForCommonErrors(Tag tag)
        {
            if (_tag.Count <= 0)
                return;

            if (_tag.Peek().IsInlineElement && tag.IsBlockLevelElement)
            {
                throw new ApplicationException("Can't have Block Level elements inside of inline elements");
            }
            if (tag.IsHeaderElement && _tag.Peek().ElementTag != XhtmlTextWriterTag.Head)
            {
                throw new ApplicationException("Can't have header elements outside of 'head' element");
            }
            if (tag.ElementTag == XhtmlTextWriterTag.Head && _tag.Peek().ElementTag != XhtmlTextWriterTag.Html)
            {
                throw new ApplicationException("Can't have 'head' element within any other elements than HTML");
            }
            if (tag.ElementTag == XhtmlTextWriterTag.Body && _tag.Peek().ElementTag != XhtmlTextWriterTag.Html)
            {
                throw new ApplicationException("Can't have 'body' element within any other elements than HTML");
            }

        }

        internal void Flush()
        {
            if (_isFlushed) 
                return;
            
            Tag current = _tag.Peek();
            current.VerifyAllMustBeAttributesPresent();
            _writer.Write(">");
            _isFlushed = true;
        }

        internal void PopTag()
        {
            _tag.Pop();
        }

        internal void VerifyTagIsCurrent(Tag tag)
        {
            Tag tag2 = _tag.Peek();

            // Doing an OBJECT reference comparison intentionally
            object t1 = tag, t2 = tag2;
            if (t1 != t2)
                throw new ApplicationException("Trying to add attributes to a tag after opening a new tag");
        }

        internal void WriteNonBreakingSpace()
        {
            Flush();
            _writer.Write("&nbsp;");
        }
    }
}
