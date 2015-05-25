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
using System.Web.UI;
using System.Collections.Generic;

namespace Gaia.WebWidgets.HtmlFormatting
{
    /// <summary>
    /// Class encapsulating an XHTML tag/element. Use in combination with XhtmlTextWriter class to easily create 
    /// XHTML valid markup. Implements the "using pattern" or the "IDisposable pattern" to make sure tag is 
    /// correctly closed. Makes it VERY easy to create XHTML valid markup "typed" in server-side code without 
    /// resorting to string.Format or other "non-best practices methods". In combination with XhtmlTextWriter,
    /// XhtmlTagFactory and ComposeXhtml it can virtually "walk on water" and make your code look AWESOME while
    /// still formatting beautiful XHTML syntax in C#.
    /// </summary>
    public class Tag : AtomicInvoker
    {
        private readonly XhtmlTextWriter _writer;
        private readonly XhtmlTextWriterTag _tag;
        private readonly List<XhtmlAttribute> _attributes = new List<XhtmlAttribute>();
        private readonly List<Tag> _children = new List<Tag>();
        private bool _disableValidation;

        internal bool DisableValidation
        {
            get { return _disableValidation; }
            set { _disableValidation = value; }
        }

        internal Tag(XhtmlTextWriter writer, XhtmlTextWriterTag tag)
            : this(writer, tag, false)
        { }

        internal Tag(XhtmlTextWriter writer, XhtmlTextWriterTag tag, bool disableValidation)
        {
            _disableValidation = disableValidation;

            _writer = writer;
            _tag = tag;
            _writer.Flush();

            _writer.VerifyTagAllowed(this, _disableValidation);

            _writer.Write("<");
            _writer.Write(tag.ToString().ToLowerInvariant());
            _writer.IsFlushed = false;

            Destructor = CloseTag;
        }

        /// <summary>
        /// Sets the "id" attribute for the tag.
        /// If more arguments are given the id parameter is expected to be
        /// a string formatting expression e.g. like; "{0}_SOMEVALUE", ClientID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Tag SetId(string id, params string[] args)
        {
            if (args != null && args.Length > 0)
                id = string.Format(System.Globalization.CultureInfo.InvariantCulture, id, args);
            
            AddAttribute(XhtmlAttribute.Id, id);
            return this;
        }

        /// <summary>
        /// Sets the CssClass ("class" attribute) for the tag
        /// </summary>
        /// <param name="cssclass"></param>
        /// <returns></returns>
        public Tag SetCssClass(string cssclass)
        {
            AddAttribute(XhtmlAttribute.CssClass, cssclass);
            return this;
        }

        /// <summary>
        /// Sets the "style" attribute for the tag
        /// If more arguments are given the id parameter si expected to be
        /// a string formatting expression e.g. like; "{0}_SOMEVALUE", ClientID
        /// </summary>
        /// <param name="style"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Tag SetStyle(string style, params object[] args)
        {
            if (string.IsNullOrEmpty(style))
                return this;
            if (args != null)
                style = string.Format(System.Globalization.CultureInfo.InvariantCulture, style, args);
            
            AddAttribute(XhtmlAttribute.Style, style);
            return this;
        }

        /// <summary>
        /// Returns the Tag enum for the type of element/tag this is
        /// </summary>
        public XhtmlTextWriterTag ElementTag
        {
            get { return _tag; }
        }

        #region [ -- Getters for types of elements -- ]

        internal bool IsInlineElement
        {
            get
            {
                switch (_tag)
                {
                    case XhtmlTextWriterTag.A:
                    case XhtmlTextWriterTag.Abbr:
                    case XhtmlTextWriterTag.Acronym:
                    case XhtmlTextWriterTag.B:
                    case XhtmlTextWriterTag.Bdo:
                    case XhtmlTextWriterTag.Big:
                    case XhtmlTextWriterTag.Br:
                    case XhtmlTextWriterTag.Button:
                    case XhtmlTextWriterTag.Cite:
                    case XhtmlTextWriterTag.Code:
                    case XhtmlTextWriterTag.Del:
                    case XhtmlTextWriterTag.Dfn:
                    case XhtmlTextWriterTag.Em:
                    case XhtmlTextWriterTag.I:
                    case XhtmlTextWriterTag.Img:
                    case XhtmlTextWriterTag.Ins:
                    case XhtmlTextWriterTag.Input:
                    case XhtmlTextWriterTag.Label:
                    case XhtmlTextWriterTag.Map:
                    case XhtmlTextWriterTag.Kbd:
                    case XhtmlTextWriterTag.Object:
                    case XhtmlTextWriterTag.Q:
                    case XhtmlTextWriterTag.Ruby:
                    case XhtmlTextWriterTag.Samp:
                    case XhtmlTextWriterTag.Script:
                    case XhtmlTextWriterTag.Select:
                    case XhtmlTextWriterTag.Small:
                    case XhtmlTextWriterTag.Span:
                    case XhtmlTextWriterTag.Strong:
                    case XhtmlTextWriterTag.Sub:
                    case XhtmlTextWriterTag.Sup:
                    case XhtmlTextWriterTag.TextArea:
                    case XhtmlTextWriterTag.Tt:
                    case XhtmlTextWriterTag.Var:
                        return true;
                    default:
                        return false;
                }
            }
        }

        internal bool IsBlockLevelElement
        {
            get
            {
                switch (_tag)
                {
                    case XhtmlTextWriterTag.Address:
                    case XhtmlTextWriterTag.BlockQuote:
                    case XhtmlTextWriterTag.Del:
                    case XhtmlTextWriterTag.Div:
                    case XhtmlTextWriterTag.Dl:
                    case XhtmlTextWriterTag.FieldSet:
                    case XhtmlTextWriterTag.Form:
                    case XhtmlTextWriterTag.H1:
                    case XhtmlTextWriterTag.H2:
                    case XhtmlTextWriterTag.H3:
                    case XhtmlTextWriterTag.H4:
                    case XhtmlTextWriterTag.H5:
                    case XhtmlTextWriterTag.H6:
                    case XhtmlTextWriterTag.Hr:
                    case XhtmlTextWriterTag.Ins:
                    case XhtmlTextWriterTag.NoScript:
                    case XhtmlTextWriterTag.Ol:
                    case XhtmlTextWriterTag.P:
                    case XhtmlTextWriterTag.Pre:
                    case XhtmlTextWriterTag.Script:
                    case XhtmlTextWriterTag.Table:
                    case XhtmlTextWriterTag.Ul:
                    case XhtmlTextWriterTag.Iframe:
                        return true;
                    default:
                        return false;
                }
            }
        }

        internal bool IsHeaderElement
        {
            get
            {
                switch (_tag)
                {
                    case XhtmlTextWriterTag.Base:
                    case XhtmlTextWriterTag.Meta:
                    case XhtmlTextWriterTag.Title:
                        return true;
                    default:
                        return false;
                }
            }
        }

        #endregion

        #region [ -- Attribute manipulation -- ]

        /// <summary>
        /// Flushes the XhtmlTextWriter/Tag and writes "content" to it. Note that content will be escaped according 
        /// to w3c rules meaning that angle brackets will not be rendered as angle brackets.
        /// </summary>
        /// <param name="content">Content to write to XhtmlTextWriter</param>
        /// <returns>this Tag</returns>
        public Tag WriteContent(string content)
        {
            if (CanBeClosedImmediately)
                throw new ApplicationException("You can't add content (PCDATA) to this element");
            _writer.Flush();
            content = content.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
            _writer.Write(content);
            return this;
        }

        /// <summary>
        /// Overridden version of AddAttribute
        /// </summary>
        /// <param name="attribute">Tag attribute name</param>
        /// <param name="value">Attribute value</param>
        /// <returns></returns>
        public virtual Tag AddAttribute(XhtmlAttribute attribute, string value)
        {
            // Easy check
            if (value == null)
                return this;

            // Verifying we don't add up attributes AFTER added other tags
            if (_writer.IsFlushed)
                throw new ApplicationException("Writer was flushed, cannot add attributes to it");

            _writer.VerifyTagIsCurrent(this);

            string name = ConvertAttributeToString(attribute);

            if (!_disableValidation)
            {
                VerifyLegalAttributeUnderTag(_tag, name, value);
                VerifyLegalAttributeValue(attribute, value);
                VerifyNoExtraSpacesAndLineBreaksWithinValue(value);
            }

            _writer.Write(string.Concat(" ", name, @"=""", value, @""""));
            _attributes.Add(attribute);
            return this;
        }

        /// <summary>
        /// Overridden version that can also do string.Format of extra parameters
        /// </summary>
        /// <param name="attribute">Attribute type</param>
        /// <param name="value">Attribute content</param>
        /// <param name="args">arguments which will be "formated" into the content parameter. 
        /// See String.Format for reference on this</param>
        /// <returns></returns>
        public virtual Tag AddAttribute(XhtmlAttribute attribute, string value, params object[] args)
        {
            return AddAttribute(attribute, string.Format(System.Globalization.CultureInfo.InvariantCulture, value, args));
        }

        /// <summary>
        /// Overridden version ONLY usable if you have previously called; "DisableValidation" on the tag. Usable in 
        /// combination with Unmanaged class to make sure you're closing the scope where you do not want validation 
        /// of the underlaying XhtmlTextWriter object.
        /// </summary>
        /// <param name="name">Name of attribute</param>
        /// <param name="value">Value of attribute</param>
        /// <returns>this Tag</returns>
        public virtual Tag AddAttribute(string name, string value)
        {
            if (!_disableValidation)
                throw new ApplicationException("Validating writer does not permit adding string attributes directly. Use Unmanaged construct.");

            // Easy check
            if (value == null)
                return this;

            // Verifying we don't add up attributes AFTER added other tags
            if (_writer.IsFlushed)
                throw new ApplicationException("Writer was flushed, cannot add attributes to it");

            _writer.VerifyTagIsCurrent(this);
            _writer.Write(string.Concat(" ", name, @"=""", value, @""""));
            return this;
        }

        /// <summary>
        /// Overridden version ONLY usable if you have previously called; "DisableValidation" on the tag. 
        /// Usable in combination with Unmanaged class to make sure you're closing the scope where you do 
        /// not want validation of the underlaying XhtmlTextWriter object.
        /// </summary>
        /// <param name="attributes">Collection of attributes to add</param>
        /// <returns>this Tag</returns>
        public virtual Tag AddAttributes(AttributeCollection attributes)
        {
            if (!_disableValidation)
                throw new ApplicationException("Validating writer does not permit adding string attributes directly. Use Unmanaged construct.");

            if (attributes == null)
                return this;

            // Verifying we don't add up attributes AFTER added other tags
            if (_writer.IsFlushed)
                throw new ApplicationException("Writer was flushed, cannot add attributes to it");

            _writer.VerifyTagIsCurrent(this);

            foreach (string name in attributes.Keys)
            {
                if (name.ToLowerInvariant().Equals("style")) // ignore style attribute
                    continue;

                string content = attributes[name];
                if (!string.IsNullOrEmpty(content))
                    _writer.Write(string.Concat(" ", name, @"=""", content, @""""));
            }
            return this;
        }

        /// <summary>
        /// Indexer for adding attributes with some syntactic sugar.
        /// </summary>
        /// <param name="attribute">Attribute type</param>
        /// <returns></returns>
        public string this[XhtmlAttribute attribute]
        {
            set { AddAttribute(attribute, value); }
        }

        private void VerifyNoExtraSpacesAndLineBreaksWithinValue(string content)
        {
            if (content.IndexOf("  ", StringComparison.Ordinal) != -1 || 
                content.IndexOf("\n", StringComparison.Ordinal) != -1)
                throw new ApplicationException("Can't have linebreaks or double-spaces within attributes of any elements");
        }

        private void VerifyLegalAttributeValue(XhtmlAttribute attr, string content)
        {
            switch (attr)
            {
                case XhtmlAttribute.Abbr:
                case XhtmlAttribute.Accept:
                case XhtmlAttribute.AcceptCharset:
                case XhtmlAttribute.AccessKey:
                case XhtmlAttribute.Action:
                case XhtmlAttribute.Alt:
                case XhtmlAttribute.Archive:
                case XhtmlAttribute.Axis:
                case XhtmlAttribute.Charset:
                case XhtmlAttribute.Cite:
                case XhtmlAttribute.ClassId:
                case XhtmlAttribute.CodeBase:
                case XhtmlAttribute.CodeType:
                case XhtmlAttribute.Content:
                case XhtmlAttribute.Coords: // TODO: Implement
                case XhtmlAttribute.CssClass: // TODO: Implement
                case XhtmlAttribute.Data:
                case XhtmlAttribute.EncType:
                case XhtmlAttribute.For:
                case XhtmlAttribute.Headers:
                case XhtmlAttribute.Href:
                case XhtmlAttribute.HrefLang:
                case XhtmlAttribute.HttpEquiv:
                case XhtmlAttribute.Id: // TODO: Implement unique
                case XhtmlAttribute.IsMap:
                case XhtmlAttribute.LongDesc:
                case XhtmlAttribute.Media:
                case XhtmlAttribute.Name:
                case XhtmlAttribute.OnBlur:
                case XhtmlAttribute.OnChange:
                case XhtmlAttribute.OnClick:
                case XhtmlAttribute.OnDblClick:
                case XhtmlAttribute.OnFocus:
                case XhtmlAttribute.OnKeyDown:
                case XhtmlAttribute.OnKeyPress:
                case XhtmlAttribute.OnKeyUp:
                case XhtmlAttribute.OnLoad:
                case XhtmlAttribute.OnMouseDown:
                case XhtmlAttribute.OnMouseMove:
                case XhtmlAttribute.OnMouseOut:
                case XhtmlAttribute.OnMouseOver:
                case XhtmlAttribute.OnMouseUp:
                case XhtmlAttribute.OnReset:
                case XhtmlAttribute.OnSelect:
                case XhtmlAttribute.OnSubmit:
                case XhtmlAttribute.OnUnload:
                case XhtmlAttribute.RbSpan:
                case XhtmlAttribute.Scheme:
                case XhtmlAttribute.Src:
                case XhtmlAttribute.StandBy:
                case XhtmlAttribute.Style: // TODO: Implement
                case XhtmlAttribute.Summary:
                case XhtmlAttribute.Title:
                case XhtmlAttribute.UseMap: // TODO: Implement IDReference
                case XhtmlAttribute.Value:
                case XhtmlAttribute.XmlLang: // TODO: Implement, NameToken
                    // Impossible to verify
                    break;


                case XhtmlAttribute.XmlSpace:
                    if (content != "preserve")
                        throw new ApplicationException("Bad value of xml:space attribute");
                    break;
                case XhtmlAttribute.ValueType:
                    switch (content)
                    {
                        case "data":
                        case "ref":
                        case "object":
                            break;
                        default:
                            throw new ApplicationException("Bad value of valign attribute");
                    }
                    break;
                case XhtmlAttribute.Valign:
                    switch (content)
                    {
                        case "top":
                        case "middle":
                        case "bottom":
                        case "baseline":
                            break;
                        default:
                            throw new ApplicationException("Bad value of valign attribute");
                    }
                    break;
                case XhtmlAttribute.Type:
                    if(ElementTag == XhtmlTextWriterTag.Input)
                    {
                        switch (content)
                        {
                            case "text":
                            case "password":
                            case "checkbox":
                            case "radio":
                            case "submit":
                            case "image":
                            case "reset":
                            case "button":
                            case "hidden":
                            case "file":
                                break;
                            default:
                                throw new ApplicationException("Bad value of type attribute");
                        }
                    }
                    break;
                case XhtmlAttribute.Shape:
                    switch (content)
                    {
                        case "default":
                        case "rect":
                        case "circle":
                        case "poly":
                            break;
                        default:
                            throw new ApplicationException("Bad value of rules attribute");
                    }
                    break;
                case XhtmlAttribute.Scope:
                    switch (content)
                    {
                        case "row":
                        case "col":
                        case "rowgroup":
                        case "colgroup":
                            break;
                        default:
                            throw new ApplicationException("Bad value of rules attribute");
                    }
                    break;
                case XhtmlAttribute.Rules:
                    switch (content)
                    {
                        case "none":
                        case "groups":
                        case "rows":
                        case "cols":
                        case "all":
                            break;
                        default:
                            throw new ApplicationException("Bad value of rules attribute");
                    }
                    break;
                case XhtmlAttribute.Rev:
                case XhtmlAttribute.Rel:
                    switch (content.ToLowerInvariant())
                    {
                        case "alternate":
                        case "stylesheet":
                        case "start":
                        case "next":
                        case "prev":
                        case "contents":
                        case "index":
                        case "glossary":
                        case "copyright":
                        case "chapter":
                        case "section":
                        case "subsection":
                        case "appendix":
                        case "help":
                        case "bookmark":
                            break;
                        default:
                            throw new ApplicationException("Bad value of rel attribute");
                    }
                    break;
                case XhtmlAttribute.ReadOnly:
                    if (content != "readonly")
                        throw new ApplicationException("Bad value of attribute readonly");
                    break;
                case XhtmlAttribute.Multiple:
                    if (content != "multiple")
                        throw new ApplicationException("Bad value of attribute multiple");
                    break;
                case XhtmlAttribute.Method:
                    if (content != "get" && content != "post")
                        throw new ApplicationException("Wrong value of attribute method");
                    break;
                case XhtmlAttribute.Height:
                case XhtmlAttribute.Width:
                    if (content.IndexOf("px", StringComparison.Ordinal) == -1 &&
                        content.IndexOf("%", StringComparison.Ordinal) == -1)
                        throw new ApplicationException("Bad value of height attribute");
                    int.Parse(content.Replace("px", "").Replace("%", ""), System.Globalization.CultureInfo.InvariantCulture); // Will throw
                    break;
                case XhtmlAttribute.FrameBorder:
                    if (content != "1" && content != "0")
                        throw new ApplicationException("Bad content of frameborder attribute");
                    break;
                case XhtmlAttribute.Frame:
                    if (content != "void" &&
                        content != "above" &&
                        content != "below" &&
                        content != "hsides" &&
                        content != "vsides" &&
                        content != "lhs" &&
                        content != "rhs" &&
                        content != "box" &&
                        content != "border")
                        throw new ApplicationException("Bad content of frame attribute");
                    break;
                case XhtmlAttribute.Disabled:
                    if (content != "disabled")
                        throw new ApplicationException("Bad value of disabled attribute");
                    break;
                case XhtmlAttribute.Dir:
                    if (content != "ltr" && content != "rtl")
                        throw new ApplicationException("Bad value of dir attribute");
                    break;
                case XhtmlAttribute.Defer:
                    if (content != "defer")
                        throw new ApplicationException("Bad value of defer attribute");
                    break;
                case XhtmlAttribute.Declare:
                    if (content != "declare")
                        throw new ApplicationException("Bad value of declare attribute");
                    break;
                case XhtmlAttribute.DateTime:
                    DateTime.ParseExact(content, "YYYY-MM-DDThh:mm:ss.", null); // Throws if error
                    break;
                case XhtmlAttribute.Checked:
                    if (content != "checked")
                        throw new ApplicationException("Garbage value of checked attribute");
                    break;
                case XhtmlAttribute.Char:
                    if (content.Length != 1)
                        throw new ApplicationException("Expected char attribute content of no more than one length");
                    break;
                case XhtmlAttribute.Scrolling:
                    if (content != "yes" && content != "no" && content != "auto")
                        throw new ApplicationException("Bad value of scrolling attribute");
                    break;
                case XhtmlAttribute.ColSpan:
                case XhtmlAttribute.Cols:
                case XhtmlAttribute.CharOff:
                case XhtmlAttribute.CellPadding:
                case XhtmlAttribute.CellSpacing:
                case XhtmlAttribute.Border:
                case XhtmlAttribute.MaxLength:
                case XhtmlAttribute.Rows:
                case XhtmlAttribute.RowSpan:
                case XhtmlAttribute.Size:
                case XhtmlAttribute.Span:
                case XhtmlAttribute.TabIndex:
                case XhtmlAttribute.MarginHeight:
                case XhtmlAttribute.MarginWidth:
                    int.Parse(content, System.Globalization.CultureInfo.InvariantCulture); // Will throw if not integer value
                    break;
                case XhtmlAttribute.Align:
                    if( content != "left" && 
                        content != "center" && 
                        content != "right" && 
                        content != "justify" && 
                        content != "char" )
                        throw new ApplicationException("Wrong value of align attribute found.");
                    break;
            }
        }

        #endregion

        private string ConvertAttributeToString(XhtmlAttribute attr)
        {
            switch (attr)
            {
                case XhtmlAttribute.AcceptCharset:
                    return "accept-charset";
                case XhtmlAttribute.CssClass:
                    return "class";
                case XhtmlAttribute.HttpEquiv:
                    return "http-equiv";
                case XhtmlAttribute.XmlLang:
                    return "xml:lang";
                case XhtmlAttribute.XmlSpace:
                    return "xml:space";
                default:
                    return attr.ToString().ToLowerInvariant();
            }
        }

        private static bool IsCoreAttribute(string attributeName, string attributeContent)
        {
            if (attributeName == "xml:space")
            {
                if (attributeContent != "default" && attributeContent != "preserve")
                {
                    throw new ApplicationException("Wrong value for xml:space");
                }
                return true;
            }
            if (attributeName == "class")
            {
                // TODO: Verify against NMTOKENS
                // see; http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_core_collection
                return true;
            }
            if (attributeName == "id")
            {
                // TODO: Verify against uniqueness
                // see; http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_core_collection
                return true;
            }
            if (attributeName == "title")
                return true;
            return false;
        }

        private static bool IsI18NAttribute(string attributeName, string attributeValue)
        {
            return attributeName == "xml:lang" || attributeName == "dir";
        }

        private static bool IsEventsAttribute(string attributeName, string attributeValue)
        {
            switch (attributeName)
            {
                case "onclick":
                case "ondblclick":
                case "onmousedown":
                case "onmouseup":
                case "onmouseover":
                case "onmousemove":
                case "onmouseout":
                case "onkeypress":
                case "onkeydown":
                case "onkeyup":
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsStyleAttribute(string attributeName, string attributeContent)
        {
            // TODO: Verify content...?
            return attributeName == "style";
        }

        private static bool IsCommon(string attributeName, string attributeContent)
        {
            return IsCoreAttribute(attributeName, attributeContent) ||
                IsEventsAttribute(attributeName, attributeContent) ||
                IsI18NAttribute(attributeName, attributeContent) ||
                IsStyleAttribute(attributeName, attributeContent);
        }

        private static void VerifyLegalAttributeUnderTag(XhtmlTextWriterTag tag, string attributeName, string attributeContent)
        {
            switch (tag)
            {
                case XhtmlTextWriterTag.Abbr:
                case XhtmlTextWriterTag.Acronym:
                case XhtmlTextWriterTag.Address:
                case XhtmlTextWriterTag.Caption:
                case XhtmlTextWriterTag.Cite:
                case XhtmlTextWriterTag.Dfn:
                case XhtmlTextWriterTag.Div:
                case XhtmlTextWriterTag.Dd:
                case XhtmlTextWriterTag.Code:
                case XhtmlTextWriterTag.Br:
                case XhtmlTextWriterTag.Big:
                case XhtmlTextWriterTag.B:
                case XhtmlTextWriterTag.Sub:
                case XhtmlTextWriterTag.Tt:
                case XhtmlTextWriterTag.Ul:
                case XhtmlTextWriterTag.Var:
                case XhtmlTextWriterTag.Sup:
                case XhtmlTextWriterTag.Small:
                case XhtmlTextWriterTag.Span:
                case XhtmlTextWriterTag.Strong:
                case XhtmlTextWriterTag.Rb:
                case XhtmlTextWriterTag.Rbc:
                case XhtmlTextWriterTag.Rp:
                case XhtmlTextWriterTag.Rt:
                case XhtmlTextWriterTag.Rtc:
                case XhtmlTextWriterTag.Ruby:
                case XhtmlTextWriterTag.Samp:
                case XhtmlTextWriterTag.P:
                case XhtmlTextWriterTag.Ol:
                case XhtmlTextWriterTag.NoScript:
                case XhtmlTextWriterTag.Map:
                case XhtmlTextWriterTag.Li:
                case XhtmlTextWriterTag.Kbd:
                case XhtmlTextWriterTag.I:
                case XhtmlTextWriterTag.Hr:
                case XhtmlTextWriterTag.H1:
                case XhtmlTextWriterTag.H2:
                case XhtmlTextWriterTag.H3:
                case XhtmlTextWriterTag.H4:
                case XhtmlTextWriterTag.H5:
                case XhtmlTextWriterTag.H6:
                case XhtmlTextWriterTag.Dl:
                case XhtmlTextWriterTag.Dt:
                case XhtmlTextWriterTag.Em:
                case XhtmlTextWriterTag.FieldSet:
                    if (!IsCommon(attributeName, attributeContent))
                        throw new ApplicationException("Attribute not found in element");
                    break;
                case XhtmlTextWriterTag.A:
                    switch (attributeName)
                    {
                        case "href":
                        case "accesskey":
                        case "charset":
                        case "coords":
                        case "hreflang":
                        case "onblur":
                        case "onfocus":
                        case "rel":
                        case "rev":
                        case "shape":
                        case "tabindex":
                        case "type":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Area:
                    switch (attributeName)
                    {
                        case "alt":
                        case "coords":
                        case "href":
                        case "shape":
                        case "accesskey":
                        case "onblur":
                        case "onfocus":
                        case "nohref":
                        case "tabindex":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Base:
                    if (attributeName != "href")
                        throw new ApplicationException("Attribute not found in element");
                    break;
                case XhtmlTextWriterTag.Bdo:
                    if (!IsCoreAttribute(attributeName, attributeName) && 
                        attributeName != "dir")
                        throw new ApplicationException("Attribute not found in element");
                    break;
                case XhtmlTextWriterTag.BlockQuote:
                    if (!IsCommon(attributeName, attributeContent) &&
                        attributeName != "cite")
                        throw new ApplicationException("Attribute not found in element");
                    break;
                case XhtmlTextWriterTag.Body:
                    if (!IsCommon(attributeName, attributeContent) &&
                        attributeName != "onunload" &&
                        attributeName != "onload")
                        throw new ApplicationException("Attribute not found in element");
                    break;
                case XhtmlTextWriterTag.Button:
                    switch (attributeName)
                    {
                        case "name":
                        case "type":
                        case "value":
                        case "accesskey":
                        case "disabled":
                        case "onblur":
                        case "onfocus":
                        case "tabindex":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Col:
                    switch (attributeName)
                    {
                        case "align":
                        case "span":
                        case "width":
                        case "char":
                        case "charoff":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.ColGroup:
                    switch (attributeName)
                    {
                        case "align":
                        case "span":
                        case "valign":
                        case "char":
                        case "charoff":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Del:
                    if (!IsCommon(attributeName, attributeContent) &&
                        attributeName != "cite" &&
                        attributeName != "datetime")
                        throw new ApplicationException("Attribute not found in element");
                    break;
                case XhtmlTextWriterTag.Form:
                    switch (attributeName)
                    {
                        case "action":
                        case "method":
                        case "accept":
                        case "accept-charsets":
                        case "enctype":
                        case "onreset":
                        case "onsubmit":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Q:
                    switch (attributeName)
                    {
                        case "cite":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Head:
                    switch (attributeName)
                    {
                        case "profile":
                        case "xml:lang":
                        case "dir":
                            break;
                        default:
                            throw new ApplicationException("Attribute not found in element");
                    }
                    break;
                case XhtmlTextWriterTag.Html:
                    switch (attributeName)
                    {
                        case "xmlns":
                        case "version":
                        case "xml:lang":
                        case "dir":
                            break;
                        default:
                            throw new ApplicationException("Attribute not found in element");
                    }
                    break;
                case XhtmlTextWriterTag.Iframe:
                    switch (attributeName)
                    {
                        case "frameborder":
                        case "height":
                        case "longdesc":
                        case "marginheight":
                        case "marginwidth":
                        case "scrolling":
                        case "src":
                        case "width":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Img:
                    switch (attributeName)
                    {
                        case "alt":
                        case "height":
                        case "src":
                        case "width":
                        case "ismap":
                        case "longdesc":
                        case "usemap":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Input:
                    switch (attributeName)
                    {
                        case "alt":
                        case "checked":
                        case "maxlength":
                        case "name":
                        case "size":
                        case "type":
                        case "value":
                        case "accept":
                        case "accesskey":
                        case "disabled":
                        case "ismap":
                        case "onblur":
                        case "onchange":
                        case "onfocus":
                        case "onselect":
                        case "readonly":
                        case "src":
                        case "tabindex":
                        case "usemap":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Ins:
                    switch (attributeName)
                    {
                        case "cite":
                        case "datetime":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Label:
                    switch (attributeName)
                    {
                        case "for":
                        case "accesskey":
                        case "onblur":
                        case "onfocus":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Legend:
                    switch (attributeName)
                    {
                        case "accesskey":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Link:
                    switch (attributeName)
                    {
                        case "href":
                        case "media":
                        case "type":
                        case "charset":
                        case "hreflang":
                        case "rel":
                        case "rev":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Meta:
                    switch (attributeName)
                    {
                        case "content":
                        case "name":
                        case "http-equiv":
                        case "scheme":
                        case "xml:lang":
                        case "dir":
                            break;
                        default:
                            throw new ApplicationException("Attribute not found in element");
                    }
                    break;
                case XhtmlTextWriterTag.Object:
                    switch (attributeName)
                    {
                        case "classid":
                        case "codebase":
                        case "height":
                        case "name":
                        case "type":
                        case "width":
                        case "archive":
                        case "codetype":
                        case "data":
                        case "declare":
                        case "standby":
                        case "tabindex":
                        case "usemap":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.OptGroup:
                    switch (attributeName)
                    {
                        case "label":
                        case "disabled":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Option:
                    switch (attributeName)
                    {
                        case "selected":
                        case "value":
                        case "disabled":
                        case "label":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Param:
                    switch (attributeName)
                    {
                        case "name":
                        case "value":
                        case "id":
                        case "type":
                        case "valuetype":
                            break;
                        default:
                            throw new ApplicationException("Attribute not found in element");
                    }
                    break;
                case XhtmlTextWriterTag.Pre:
                    switch (attributeName)
                    {
                        case "xml:space":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Script:
                    switch (attributeName)
                    {
                        case "src":
                        case "type":
                        case "charset":
                        case "defer":
                        case "xml:space":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Select:
                    switch (attributeName)
                    {
                        case "multiple":
                        case "name":
                        case "size":
                        case "disabled":
                        case "onblur":
                        case "onchange":
                        case "onfocus":
                        case "tabindex":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Style:
                    switch (attributeName)
                    {
                        case "media":
                        case "title":
                        case "type":
                        case "xml:space":
                        case "xml:lang":
                        case "dir":
                            break;
                        default:
                            throw new ApplicationException("Attribute not found in element");
                    }
                    break;
                case XhtmlTextWriterTag.Table:
                    switch (attributeName)
                    {
                        case "border":
                        case "cellpadding":
                        case "cellspacing":
                        case "summary":
                        case "width":
                        case "frame":
                        case "rules":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Tbody:
                    switch (attributeName)
                    {
                        case "align":
                        case "valign":
                        case "char":
                        case "charoff":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Td:
                    switch (attributeName)
                    {
                        case "align":
                        case "colspan":
                        case "headers":
                        case "rowspan":
                        case "valign":
                        case "axis":
                        case "char":
                        case "charoff":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.TextArea:
                    switch (attributeName)
                    {
                        case "cols":
                        case "name":
                        case "rows":
                        case "accesskey":
                        case "disabled":
                        case "onblur":
                        case "onchange":
                        case "onfocus":
                        case "onselect":
                        case "readonly":
                        case "tabindex":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Tfoot:
                    switch (attributeName)
                    {
                        case "align":
                        case "valign":
                        case "char":
                        case "charoff":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Th:
                    switch (attributeName)
                    {
                        case "abbr":
                        case "align":
                        case "colspan":
                        case "rowspan":
                        case "valign":
                        case "axis":
                        case "char":
                        case "charoff":
                        case "scope":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Thead:
                    switch (attributeName)
                    {
                        case "align":
                        case "valign":
                        case "char":
                        case "charoff":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
                case XhtmlTextWriterTag.Title:
                    switch (attributeName)
                    {
                        case "xml:lang":
                        case "dir":
                            break;
                        default:
                            throw new ApplicationException("Attribute not found in element");
                    }
                    break;
                case XhtmlTextWriterTag.Tr:
                    switch (attributeName)
                    {
                        case "align":
                        case "valign":
                        case "char":
                        case "charoff":
                            break;
                        default:
                            if (!IsCommon(attributeName, attributeContent))
                                throw new ApplicationException("Attribute not found in element");
                            break;
                    }
                    break;
            }
        }

        private bool CanBeClosedImmediately
        {
            get
            {
                switch (_tag)
                {
                    case XhtmlTextWriterTag.Br:
                    case XhtmlTextWriterTag.Hr:
                    case XhtmlTextWriterTag.Input:
                    case XhtmlTextWriterTag.Col:
                    case XhtmlTextWriterTag.Img:
                    case XhtmlTextWriterTag.Area:
                    case XhtmlTextWriterTag.Param:
                    case XhtmlTextWriterTag.Meta:
                    case XhtmlTextWriterTag.Link:
                    case XhtmlTextWriterTag.Base:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Just makes sure the "end element" string is written to the underlaying XhtmlTextWriter.
        /// </summary>
        private void CloseTag()
        {
            // Must verify that all mandatory child elements are present
            if (!_disableValidation)
                VerifyAllMustBeChildElementsExists();

            // Just writing the end element to our Writer
            if (!_writer.IsFlushed && CanBeClosedImmediately)
            {
                if (!_disableValidation)
                    VerifyAllMustBeAttributesPresent();

                _writer.IsFlushed = true;
                _writer.Write(" />");
            }
            else
            {
                bool writerWasFlushed = _writer.IsFlushed;
                _writer.Flush();

                // Making sure every element has at least something inside of it
                if (!writerWasFlushed && IsBlockLevelElement)
                    _writer.Write("&nbsp;");
                _writer.Write("</");
                _writer.Write(_tag.ToString().ToLowerInvariant());
                _writer.Write(">");
            }
            _writer.PopTag();
        }

        internal void VerifyAllMustBeAttributesPresent()
        {
            switch (_tag)
            {
                case XhtmlTextWriterTag.Param:
                    VerifyAllAttributesHere(XhtmlAttribute.Name);
                    break;
                case XhtmlTextWriterTag.Bdo:
                    VerifyAllAttributesHere(XhtmlAttribute.Dir);
                    break;
                case XhtmlTextWriterTag.Form:
                    VerifyAllAttributesHere(XhtmlAttribute.Action);
                    break;
                case XhtmlTextWriterTag.TextArea:
                    VerifyAllAttributesHere(XhtmlAttribute.Cols, XhtmlAttribute.Rows);
                    break;
                case XhtmlTextWriterTag.Img:
                    VerifyAllAttributesHere(XhtmlAttribute.Alt, XhtmlAttribute.Src);
                    break;
                case XhtmlTextWriterTag.Area:
                    VerifyAllAttributesHere(XhtmlAttribute.Alt);
                    break;
                case XhtmlTextWriterTag.Map:
                    VerifyAllAttributesHere(XhtmlAttribute.Id);
                    break;
                case XhtmlTextWriterTag.Meta:
                    VerifyAllAttributesHere(XhtmlAttribute.Content);
                    break;
                case XhtmlTextWriterTag.Script:
                case XhtmlTextWriterTag.Style:
                    VerifyAllAttributesHere(XhtmlAttribute.Type);
                    break;
                case XhtmlTextWriterTag.Base:
                    VerifyAllAttributesHere(XhtmlAttribute.Href);
                    break;
            }
        }

        private void VerifyAllAttributesHere(params XhtmlAttribute[] attrs)
        {
            foreach (XhtmlAttribute idx in attrs)
            {
                bool found = false;
                foreach (XhtmlAttribute idxThis in _attributes)
                {
                    if (idxThis == idx)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new ApplicationException("Missing attribute in tag");
                }
            }
        }

        internal void AddChild(Tag tag)
        {
            _children.Add(tag);
        }

        private void VerifyAtLeastOneExistsUnderneath(params XhtmlTextWriterTag[] attrs)
        {
            if (_writer.IsFlushed)
                return;
            foreach (Tag idx in _children)
            {
                foreach (XhtmlTextWriterTag idxTag in attrs)
                {
                    if (idxTag == idx.ElementTag)
                        return;
                }
            }
            throw new ApplicationException("Mandatory tag not found");
        }

        internal void VerifyAllMustBeChildElementsExists()
        {
            switch (_tag)
            {
                case XhtmlTextWriterTag.Body:
                    VerifyAtLeastOneExistsUnderneath(
                        /* Header */
                        XhtmlTextWriterTag.H1,
                        XhtmlTextWriterTag.H2,
                        XhtmlTextWriterTag.H3,
                        XhtmlTextWriterTag.H4,
                        XhtmlTextWriterTag.H5,
                        XhtmlTextWriterTag.H6,

                        /* Block */
                        XhtmlTextWriterTag.Address,
                        XhtmlTextWriterTag.BlockQuote,
                        XhtmlTextWriterTag.Div,
                        XhtmlTextWriterTag.P,
                        XhtmlTextWriterTag.Pre,

                        /* List */
                        XhtmlTextWriterTag.Dl,
                        XhtmlTextWriterTag.Ol,
                        XhtmlTextWriterTag.Ul);
                    break;
                case XhtmlTextWriterTag.Head:
                    VerifyAtLeastOneExistsUnderneath(XhtmlTextWriterTag.Title);
                    break;
                case XhtmlTextWriterTag.Html:
                    VerifyAtLeastOneExistsUnderneath(XhtmlTextWriterTag.Body);
                    VerifyAtLeastOneExistsUnderneath(XhtmlTextWriterTag.Head);
                    break;
                case XhtmlTextWriterTag.BlockQuote:
                    VerifyAtLeastOneExistsUnderneath(
                        /* Header */
                        XhtmlTextWriterTag.H1,
                        XhtmlTextWriterTag.H2,
                        XhtmlTextWriterTag.H3,
                        XhtmlTextWriterTag.H4,
                        XhtmlTextWriterTag.H5,
                        XhtmlTextWriterTag.H6,

                        /* Block */
                        XhtmlTextWriterTag.Address,
                        XhtmlTextWriterTag.BlockQuote,
                        XhtmlTextWriterTag.Div,
                        XhtmlTextWriterTag.P,
                        XhtmlTextWriterTag.Pre,

                        /* List */
                        XhtmlTextWriterTag.Dl,
                        XhtmlTextWriterTag.Ol,
                        XhtmlTextWriterTag.Ul);
                    break;
                case XhtmlTextWriterTag.Dl:
                    VerifyAtLeastOneExistsUnderneath(
                        XhtmlTextWriterTag.Dt, 
                        XhtmlTextWriterTag.Dd);
                    break;
                case XhtmlTextWriterTag.Ol:
                case XhtmlTextWriterTag.Ul:
                    VerifyAtLeastOneExistsUnderneath(XhtmlTextWriterTag.Li);
                    break;
                case XhtmlTextWriterTag.Form:
                    VerifyAtLeastOneExistsUnderneath(
                        /* Header */
                        XhtmlTextWriterTag.H1,
                        XhtmlTextWriterTag.H2,
                        XhtmlTextWriterTag.H3,
                        XhtmlTextWriterTag.H4,
                        XhtmlTextWriterTag.H5,
                        XhtmlTextWriterTag.H6,

                        /* Block */
                        XhtmlTextWriterTag.Address,
                        XhtmlTextWriterTag.BlockQuote,
                        XhtmlTextWriterTag.Div,
                        XhtmlTextWriterTag.P,
                        XhtmlTextWriterTag.Pre,

                        /* List */
                        XhtmlTextWriterTag.Dl,
                        XhtmlTextWriterTag.Ol,
                        XhtmlTextWriterTag.Ul,

                        /* Form */
                        XhtmlTextWriterTag.Button,
                        XhtmlTextWriterTag.Input,
                        XhtmlTextWriterTag.Select,
                        XhtmlTextWriterTag.Option,
                        XhtmlTextWriterTag.TextArea,
                        XhtmlTextWriterTag.FieldSet);
                    break;
                case XhtmlTextWriterTag.Select:
                    VerifyAtLeastOneExistsUnderneath(
                        XhtmlTextWriterTag.Option, 
                        XhtmlTextWriterTag.OptGroup);
                    break;
                    // TODO: Button
                case XhtmlTextWriterTag.OptGroup:
                    VerifyAtLeastOneExistsUnderneath(XhtmlTextWriterTag.Option);
                    break;
                case XhtmlTextWriterTag.Table:
                    VerifyAtLeastOneExistsUnderneath(
                        XhtmlTextWriterTag.Tr,
                        XhtmlTextWriterTag.Tbody);
                    break;
                case XhtmlTextWriterTag.Tr:
                    VerifyAtLeastOneExistsUnderneath(
                        XhtmlTextWriterTag.Td, 
                        XhtmlTextWriterTag.Th);
                    break;
                case XhtmlTextWriterTag.ColGroup:
                    VerifyAtLeastOneExistsUnderneath(XhtmlTextWriterTag.Col);
                    break;
                case XhtmlTextWriterTag.Tbody:
                    VerifyAtLeastOneExistsUnderneath(XhtmlTextWriterTag.Tr);
                    break;
                case XhtmlTextWriterTag.Thead:
                    VerifyAtLeastOneExistsUnderneath(XhtmlTextWriterTag.Tr);
                    break;
                case XhtmlTextWriterTag.Tfoot:
                    VerifyAtLeastOneExistsUnderneath(XhtmlTextWriterTag.Tr);
                    break;
                    // TODO: Map
                case XhtmlTextWriterTag.NoScript:
                    VerifyAtLeastOneExistsUnderneath(
                        /* Header */
                        XhtmlTextWriterTag.H1,
                        XhtmlTextWriterTag.H2,
                        XhtmlTextWriterTag.H3,
                        XhtmlTextWriterTag.H4,
                        XhtmlTextWriterTag.H5,
                        XhtmlTextWriterTag.H6,

                        /* Block */
                        XhtmlTextWriterTag.Address,
                        XhtmlTextWriterTag.BlockQuote,
                        XhtmlTextWriterTag.Div,
                        XhtmlTextWriterTag.P,
                        XhtmlTextWriterTag.Pre,

                        /* List */
                        XhtmlTextWriterTag.Dl,
                        XhtmlTextWriterTag.Ol,
                        XhtmlTextWriterTag.Ul);
                    break;
            }
        }

        /// <summary>
        /// Writes a non-breaking-space (&amp;nbsp;) to the XhtmlTextWriter
        /// </summary>
        /// <returns></returns>
        public Tag WriteNonBreakingSpace()
        {
            _writer.WriteNonBreakingSpace();
            return this;
        }
    }
}
