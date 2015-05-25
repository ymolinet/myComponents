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
using ASP = System.Web.UI;

namespace Gaia.WebWidgets.HtmlFormatting
{
    /// <summary>
    /// The XhtmlTagFactory works on top of the <see cref="XhtmlTextWriter"></see> and provides an easy syntax to build complex 
    /// xhtml structures in code. 
    /// </summary>
    public class XhtmlTagFactory
    {
        private readonly bool _disableValidation;
        private readonly XhtmlTextWriter _writer;

        /// <summary>
        /// Retrieve the underlying ASP.HtmlTextWriter. This operation will flush
        /// the stream.
        /// </summary>
        public ASP.HtmlTextWriter GetHtmlTextWriter()
        {
            return (ASP.HtmlTextWriter) _writer;
        }

        /// <summary>
        /// Retrieve the instance of the XhtmlTextWriter used within the factory
        /// </summary>
        public XhtmlTextWriter GetXhtmlTextWriter()
        {
            return _writer;
        }

        /// <summary>
        /// Constructor, pass in the XhtmlTextWriter stream you wish to write to
        /// </summary>
        /// <param name="writer">XhtmlTextWriter</param>
        public XhtmlTagFactory(XhtmlTextWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// Overridden constructor to make it possible to create a "non-validating" version of the XhtmlTagFactory.
        /// Don't use this unless you *really* have to.
        /// </summary>
        /// <param name="writer">Underlaying writer to write into</param>
        /// <param name="disableValidation">If true will disable validation of XHTML</param>
        public XhtmlTagFactory(XhtmlTextWriter writer, bool disableValidation)
        {
            _writer = writer;
            _disableValidation = disableValidation;
        }

        /// <summary>
        /// Overridden constructor to make it possible to create a "non-validating" version of the XhtmlTagFactory.
        /// Don't use this unless you *really* have to.
        /// </summary>
        /// <param name="writer">Unerlaying TextWriter to write into</param>
        /// <param name="disableValidation">If true will disable validation of XHTML</param>
        public XhtmlTagFactory(TextWriter writer, bool disableValidation)
        {
            _writer = new XhtmlTextWriter(writer);
            _disableValidation = disableValidation;
        }

        /// <summary>
        /// Overloaded constructor taking a TextWriter which you want to write to
        /// </summary>
        /// <param name="writer">Write rto write onto</param>
        public XhtmlTagFactory(TextWriter writer)
        {
            _writer = new XhtmlTextWriter(writer);
        }

        private Tag CreateTag(XhtmlTextWriter writer, XhtmlTextWriterTag tag)
        {
            return new Tag(writer, tag, _disableValidation);
        }

        private static Tag AddCssClass(Tag tag, string cssclass)
        {
            return string.IsNullOrEmpty(cssclass) ? tag : tag.AddAttribute(XhtmlAttribute.CssClass, cssclass);
        }

        #region A Element

        /// <summary>
        /// The a element is used to create a hyperlink. 
        /// The destination of the hyperlink is specified in the href attribute, 
        /// and the text or image for the hyperlink is specified between the opening 
        /// <a> and closing </a> tags.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag A()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.A);
        }

        /// <summary>
        /// The a element is used to create a hyperlink. 
        /// The destination of the hyperlink is specified in the href attribute, 
        /// and the text or image for the hyperlink is specified between the opening 
        /// <a> and closing </a> tags.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag A(string id)
        {
            return A().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The a element is used to create a hyperlink. 
        /// The destination of the hyperlink is specified in the href attribute, 
        /// and the text or image for the hyperlink is specified between the opening 
        /// <a> and closing </a> tags.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class to use for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag A(string id, string cssclass)
        {
            return AddCssClass(A(id), cssclass);
        }

        /// <summary>
        /// The a element is used to create a hyperlink. 
        /// The destination of the hyperlink is specified in the href attribute, 
        /// and the text or image for the hyperlink is specified between the opening 
        /// <a> and closing </a> tags.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class to use for element</param>
        /// <param name="href">Target for URL</param>
        /// <returns>Constructed Tag</returns>
        public Tag A(string id, string cssclass, string href)
        {
            return A(id, cssclass).AddAttribute(XhtmlAttribute.Href, href);
        }
        #endregion

        /// <summary>
        /// The body element contains the contents of a Web page.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Body()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Body);
        }

        /// <summary>
        /// The body element contains the contents of a Web page.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Body(string id)
        {
            return Body().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The head element contains information about the current document, 
        /// such as its title, keywords that may be useful to search engines, 
        /// and other data that is not considered to be document content. 
        /// This information is usually not displayed by browsers.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Head()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Head);
        }

        /// <summary>
        /// The head element contains information about the current document, 
        /// such as its title, keywords that may be useful to search engines, 
        /// and other data that is not considered to be document content. 
        /// This information is usually not displayed by browsers.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Head(string id)
        {
            return Head().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The html element is the root element that contains all other elements. 
        /// It must appear only once and usually follows the document declaration.
        /// </summary>        
        /// <returns>Constructed Tag</returns>
        public Tag Html()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Html);
        }

        /// <summary>
        /// The html element is the root element that contains all other elements. 
        /// It must appear only once and usually follows the document declaration.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Html(string id)
        {
            return Html().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The title element is used to identify the document.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Title()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Title);
        }

        /// <summary>
        /// The title element is used to identify the document.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Title(string id)
        {
            return Title().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// An abbreviation is a shortened form of a word or phrase. 
        /// The abbr element is used to identify an abbreviation, 
        /// and can help assistive technologies to correctly pronounce abbreviated text.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Abbr()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Abbr);
        }

        /// <summary>
        /// An acronym is a word formed from the initial letters of a series of words. 
        /// The acronym element identifies acronyms, and can help assistive technologies 
        /// to correctly pronounce the acronym.
        /// <br />
        /// Inline Element.
        /// </summary>        
        /// <returns>Constructed Tag</returns>
        public Tag Acronym()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Acronym);
        }

        /// <summary>
        /// The address element is used to supply contact information. 
        /// This element often appears at the beginning or end of a document.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Address()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Address);
        }

        /// <summary>
        /// The blockquote element is used to identify larger amounts of quoted text.
        /// <br />
        /// Block Level Element.
        /// </summary>        
        /// <returns>Constructed Tag</returns>
        public Tag BlockQuote()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.BlockQuote);
        }
        
        /// <summary>
        /// The br element forcibly breaks (ends) the current line of text. 
        /// Web browsers render these line breaks as hard returns.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Br()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Br);   
        }

        /// <summary>
        /// The br element forcibly breaks (ends) the current line of text. 
        /// Web browsers render these line breaks as hard returns.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Br(string id)
        {
            return Br().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The cite element contains a citation or reference to another source.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Cite()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Cite);
        }

        /// <summary>
        /// The code element contains a fragment of computer code.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Code()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Code);
        }

        #region Div Element

        /// <summary>
        /// The div element offers a generic way of grouping areas of content.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Div()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Div);
        }


        /// <summary>
        /// The div element offers a generic way of grouping areas of content.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Div(string id)
        {
            return Div().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The div element offers a generic way of grouping areas of content.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Div(string id, string cssclass)
        {
            return AddCssClass(Div(id), cssclass);
        } 
        #endregion

        /// <summary>
        /// The dfn element contains the defining instance of the enclosed term.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Dfn()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Dfn);
        }

        /// <summary>
        /// The em element is used to indicate emphasis.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Em()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Em);
        }

        /// <summary>
        /// The em element is used to indicate emphasis.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Em(string id)
        {
            return Em().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag H1()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.H1);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag H1(string id)
        {
            return H1().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag H2()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.H2);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag H2(string id)
        {
            return H2().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag H3()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.H3);
        }


        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag H3(string id)
        {
            return H3().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag H4()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.H4);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag H4(string id)
        {
            return H4().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag H5()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.H5);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag H5(string id)
        {
            return H5().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag H6()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.H6);
        }

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag H6(string id)
        {
            return H6().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The kbd element indicates input to be entered by the user.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Kbd()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Kbd);
        }

        /// <summary>
        /// The p element represents a paragraph.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag P()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.P);
        }

        /// <summary>
        /// The p element represents a paragraph.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag P(string id)
        {
            return P().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The p element represents a paragraph.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag P(string id, string cssclass)
        {
            return AddCssClass(P(id), cssclass);
        }

        /// <summary>
        /// The pre element instructs visual Web browsers to render content 
        /// in a pre-formatted fashion. Most Web browsers will render 
        /// pre-formatted content in a monospace font while preserving 
        /// white space (spaces, tabs and hard returns).
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Pre()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Pre);
        }

        /// <summary>
        /// The q is used to identify short quoted text.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Q()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Q);
        }

        /// <summary>
        /// The samp element is used to designate sample output 
        /// from programs, scripts, etc.
        /// <br />
        /// Block Level Element.
        /// <br />
        /// Inline Element. ALSO.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Samp()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Samp);
        }

        #region Span Element

        /// <summary>
        /// The span element offers a generic way of adding structure to content.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Span()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Span);
        }

        /// <summary>
        /// The span element offers a generic way of adding structure to content.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Span(string id)
        {
            return Span().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The span element offers a generic way of adding structure to content.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Span(string id, string cssclass)
        {
            return AddCssClass(Span(id), cssclass);
        } 
        #endregion

        /// <summary>
        /// The strong element is used to indicate stronger emphasis.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Strong()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Strong);
        }

        /// <summary>
        /// The var element is used to indicate an instance of a 
        /// computer code variable or program argument.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Var()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Var);
        }

        /// <summary>
        /// The dl element is used to create a list where each item in the list 
        /// comprises two parts: a term and a description. A glossary of terms 
        /// is a typical example of a definition list, where each item consists 
        /// of the term being defined and a definition of the term.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Dl()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Dl);
        }

        /// <summary>
        /// The dt element is a definition term for an item in a definition list.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Dt()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Dt);
        }

        /// <summary>
        /// The dd element is a definition description for an item in a definition list.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Dd()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Dd);
        }

        /// <summary>
        /// The ol element is used to create ordered lists. An ordered list is a grouping 
        /// of items whose sequence in the list is important. For example, the sequence 
        /// of steps in a recipe is important if the result is to be the intended one.
        /// Every ol element must contain at least one li element.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Ol()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Ol);
        }

        /// <summary>
        /// The ul element is used to create unordered lists. An unordered list is a 
        /// grouping of items whose sequence in the list is not important. 
        /// For example, the order in which ingredients for a recipe are presented 
        /// will not affect the outcome of the recipe.
        /// Every ul element must contain at least one li element.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Ul()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Ul);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Ul(string id)
        {
            return Ul().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The li element represents a list item in ordered lists and unordered lists.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Ul(string id, string cssclass)
        {
            return AddCssClass(Ul(id), cssclass);
        }

        /// <summary>
        /// The li element represents a list item in ordered lists and unordered lists.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Li()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Li);
        }

        /// <summary>
        /// The li element represents a list item in ordered lists and unordered lists.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Li(string id)
        {
            return Li().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The li element represents a list item in ordered lists and unordered lists.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Li(string id, string cssclass)
        {
            return AddCssClass(Li(id), cssclass);
        }

        /// <summary>
        /// The object element provides a generic way of embedding objects such as images, 
        /// movies and applications (Java applets, browser plug-ins, etc.) into Web pages. 
        /// param elements contained inside the object element are used to configure the 
        /// embedded object. Besides param elements, the object element can contain 
        /// alternate content which can be text or another object element. Alternate content 
        /// serves as a fall-back mechanism for browsers that are unable to process the embedded object.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Object()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Object);
        }


        /// <summary>
        /// The param element is used to customize embedded objects that are loaded into a 
        /// Web browser via the object element. The param element is a generic way of passing 
        /// data to embedded objects in the form of name/value pairs. The need for param 
        /// elements and the number of param elements depends on the embedded object.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Param()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Param);
        }

        /// <summary>
        /// The b element renders text in bold style.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag B()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.B);
        }

        /// <summary>
        /// The big element renders text in a large font.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Big()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Big);
        }


        /// <summary>
        /// The hr element is used to separate sections of content. 
        /// Though the name of the hr element is "horizontal rule", 
        /// most visual Web browsers render hr as a horizontal line.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Hr()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Hr);
        }

        /// <summary>
        /// The i element renders text in italic style.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag I()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.I);
        }

        /// <summary>
        /// The small element renders text in a small font.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Small()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Small);
        }

        /// <summary>
        /// The sub element indicates that its contents should be 
        /// regarded as a subscript.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Sub()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Sub);
        }

        /// <summary>
        /// The tt element renders text in a teletype or a monospaced font.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Tt()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Tt);
        }

        /// <summary>
        /// The del element is used to mark up modifications made to a document. 
        /// Specifically, the del element is used to indicate that a section 
        /// of content has changed and has therefore been removed.
        /// <br />
        /// Block Level Element.
        /// <br />
        /// Inline Element. ALSO
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Del()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Del);
        }

        /// <summary>
        /// The ins element is used to mark up content that has been inserted 
        /// into the current version of a document. The ins element indicates 
        /// that content in the previous version of the document has been changed, 
        /// and that the changes are found inside the ins element.
        /// <br />
        /// Block Level Element.
        /// <br />
        /// Inline Element. ALSO.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Ins()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Ins);
        }

        /// <summary>
        /// Unlike English, which is written from left-to-right (LTR), some languages, 
        /// such as Arabic and Hebrew, are written from right-to-left (RTL). 
        /// When the same paragraph contains both RTL and LTR text, this is known as 
        /// bidirectional text or "bidi" text for short. 
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Bdo()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Bdo);
        }

        /// <summary>
        /// The button element is used to create button controls for forms. 
        /// Buttons created using the button element are similar in functionality 
        /// to buttons created using the input element, but offer greater rendering options.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Button()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Button);
        }

        /// <summary>
        /// The button element is used to create button controls for forms. 
        /// Buttons created using the button element are similar in functionality 
        /// to buttons created using the input element, but offer greater rendering options.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Button(string id)
        {
            return Button().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The button element is used to create button controls for forms. 
        /// Buttons created using the button element are similar in functionality 
        /// to buttons created using the input element, but offer greater rendering options.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Button(string id, string cssclass)
        {
            return AddCssClass(Button(id), cssclass);
        }

        /// <summary>
        /// The fieldset element adds structure to forms by grouping together 
        /// related controls and labels.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag FieldSet()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.FieldSet);
        }


        /// <summary>
        /// The form element is used to create data entry forms. 
        /// Data collected in the form is sent to the server for 
        /// processing by server-side scripts such as PHP, ASP, etc.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Form()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Form);
        }


        /// <summary>
        /// The input element is a multi-purpose form control. 
        /// The type attribute specifies the type of form control to be created.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Input()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Input);
        }

        /// <summary>
        /// The input element is a multi-purpose form control. 
        /// The type attribute specifies the type of form control to be created.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Input(string id)
        {
            return Input().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The input element is a multi-purpose form control. 
        /// The type attribute specifies the type of form control to be created.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Input(string id, string cssclass)
        {
            return AddCssClass(Input(id), cssclass);
        }

        /// <summary>
        /// The input element is a multi-purpose form control. 
        /// The type attribute specifies the type of form control to be created.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns>Constructed Tag</returns>
        public Tag Input(string id, string cssclass, string type, string value)
        {
            return Input(id, cssclass).AddAttribute(XhtmlAttribute.Type, type)
                                      .AddAttribute(XhtmlAttribute.Value, value);

        }

        /// <summary>
        /// The label element associates a label with form controls such as input, 
        /// textarea, select and object. This association enhances the usability of forms. 
        /// For example, when users of visual Web browsers click in a label, 
        /// focus is automatically set in the associated form control. 
        /// For users of assistive technology, establishing associations between labels 
        /// and controls helps clarify the spatial relationships found in forms and makes 
        /// them easier to navigate.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Label()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Label);
        }

        /// <summary>
        /// The label element associates a label with form controls such as input, 
        /// textarea, select and object. This association enhances the usability of forms. 
        /// For example, when users of visual Web browsers click in a label, 
        /// focus is automatically set in the associated form control. 
        /// For users of assistive technology, establishing associations between labels 
        /// and controls helps clarify the spatial relationships found in forms and makes 
        /// them easier to navigate.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Label(string id)
        {
            return Label().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The label element associates a label with form controls such as input, 
        /// textarea, select and object. This association enhances the usability of forms. 
        /// For example, when users of visual Web browsers click in a label, 
        /// focus is automatically set in the associated form control. 
        /// For users of assistive technology, establishing associations between labels 
        /// and controls helps clarify the spatial relationships found in forms and makes 
        /// them easier to navigate.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Label(string id, string cssclass)
        {
            return AddCssClass(Label(id), cssclass);
        }

        /// <summary>
        /// The legend element is a caption to a fieldset element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Legend()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Legend);
        }

        /// <summary>
        /// The select element is used to create an option selector 
        /// form control which most Web browsers render as a listbox control. 
        /// The list of values for this control is created using option elements. 
        /// These values can be grouped together using the optgroup element.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Select()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Select);
        }


        /// <summary>
        /// The select element is used to create an option selector 
        /// form control which most Web browsers render as a listbox control. 
        /// The list of values for this control is created using option elements. 
        /// These values can be grouped together using the optgroup element.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Select(string id)
        {
            return Select().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The select element is used to create an option selector 
        /// form control which most Web browsers render as a listbox control. 
        /// The list of values for this control is created using option elements. 
        /// These values can be grouped together using the optgroup element.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Select(string id, string cssclass)
        {
            return AddCssClass(Select(id), cssclass);
        }

        /// <summary>
        /// The optgroup element is used to group the choices offered in select form controls. 
        /// Users find it easier to work with long lists if related sections are grouped together.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag OptGroup()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.OptGroup);
        }

        /// <summary>
        /// The option element represents a choice offered by select form controls.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Option()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Option);
        }

        /// <summary>
        /// The option element represents a choice offered by select form controls.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Option(string id)
        {
            return Option().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The textarea element is used to create a multi-line text input form control.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag TextArea()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.TextArea);
        }

        /// <summary>
        /// The textarea element is used to create a multi-line text input form control.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag TextArea(string id)
        {
            return TextArea().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The textarea element is used to create a multi-line text input form control.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag TextArea(string id, string cssclass)
        {
            return AddCssClass(TextArea(id), cssclass);
        }

        /// <summary>
        /// The caption element creates a caption for a table. If a caption is to be used, 
        /// it should be the first element after the opening table element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Caption()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Caption);
        }

        /// <summary>
        /// The caption element creates a caption for a table. If a caption is to be used, 
        /// it should be the first element after the opening table element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Caption(string id)
        {
            return Caption().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The caption element creates a caption for a table. If a caption is to be used, 
        /// it should be the first element after the opening table element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Caption(string id, string cssclass)
        {
            return AddCssClass(Caption(id), cssclass);
        }


        /// <summary>
        /// In XHTML, tables are physically constructed from rows, rather than columns. 
        /// Table rows contain table cells. In visual Web browsers, when cells line up 
        /// beneath each other, they are perceived as columns.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Col()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Col);
        }

        /// <summary>
        /// In XHTML, tables are physically constructed from rows, rather than columns. 
        /// Table rows contain table cells. In visual Web browsers, when cells line up 
        /// beneath each other, they are perceived as columns.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Col(string id)
        {
            return Col().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// In XHTML, tables are physically constructed from rows, rather than columns. 
        /// Table rows contain table cells. In visual Web browsers, when cells line up 
        /// beneath each other, they are perceived as columns.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag ColGroup()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.ColGroup);
        }

        /// <summary>
        /// In XHTML, tables are physically constructed from rows, rather than columns. 
        /// Table rows contain table cells. In visual Web browsers, when cells line up 
        /// beneath each other, they are perceived as columns.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag ColGroup(string id)
        {
            return ColGroup().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The table element is used to define a table. A table is a construct where data 
        /// is organized into rows and columns of cells.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Table()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Table);
        }

        /// <summary>
        /// The table element is used to define a table. A table is a construct where data 
        /// is organized into rows and columns of cells.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Table(string id)
        {
            return Table().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The table element is used to define a table. A table is a construct where data 
        /// is organized into rows and columns of cells.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Table(string id, string cssclass)
        {
            return AddCssClass(Table(id), cssclass);
        }

        /// <summary>
        /// The table element is used to define a table. A table is a construct where data 
        /// is organized into rows and columns of cells.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <param name="cellpadding"></param>
        /// <param name="cellspacing"></param>
        /// <returns>Constructed Tag</returns>
        public Tag Table(string id, string cssclass, string cellpadding, string cellspacing)
        {
            return Table(id, cssclass).AddAttribute(XhtmlAttribute.CellPadding, cellpadding)
                                      .AddAttribute(XhtmlAttribute.CellSpacing, cellspacing);
        }

        /// <summary>
        /// The table element is used to define a table. A table is a construct where data 
        /// is organized into rows and columns of cells.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <param name="cellpadding"></param>
        /// <param name="cellspacing"></param>
        /// <param name="border"></param>
        /// <returns>Constructed Tag</returns>
        public Tag Table(string id, string cssclass, string cellpadding, string cellspacing, string border)
        {
            return Table(id, cssclass, cellpadding, cellspacing).AddAttribute(XhtmlAttribute.Border, border);
        }

        /// <summary>
        /// The tbody element can be used to group table data rows. This can be useful 
        /// when a Web browser supports scrolling of table rows in longer tables. 
        /// Multiple tbody elements can be used for independent scrolling.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Tbody()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Tbody);
        }

        /// <summary>
        /// The tbody element can be used to group table data rows. This can be useful 
        /// when a Web browser supports scrolling of table rows in longer tables. 
        /// Multiple tbody elements can be used for independent scrolling.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Tbody(string id)
        {
            return Tbody().AddAttribute(XhtmlAttribute.Id, id);
        }

        #region Td Element
        
        /// <summary>
        /// The td element defines a data cell in a table (i.e. cells that are not header cells).
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Td()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Td);
        }

        /// <summary>
        /// The td element defines a data cell in a table (i.e. cells that are not header cells).
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Td(string id)
        {
            return Td().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The td element defines a data cell in a table (i.e. cells that are not header cells).
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Td(string id, string cssclass)
        {

            return AddCssClass(Td(id), cssclass);
        }

        /// <summary>
        /// The td element defines a data cell in a table (i.e. cells that are not header cells).
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <param name="title"></param>
        /// <returns>Constructed Tag</returns>
        public Tag Td(string id, string cssclass, string title)
        {
            return Td(id, cssclass).AddAttribute(XhtmlAttribute.Title, title);
        }



        #endregion

        /// <summary>
        /// The tfoot element can be used to group table rows that contain table footer information. 
        /// This may be useful when printing longer tables that span several printed pages, 
        /// since the data in tfoot is repeated on each page. The tfoot element should appear before 
        /// tbody elements.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Tfoot()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Tfoot);
        }

        #region Th Element
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Th()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Th);
        }

        /// <summary>
        /// The th element defines a table header cell.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Th(string id)
        {
            return Th().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The th element defines a table header cell.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Th(string id, string cssclass)
        {
            return AddCssClass(Th(id), cssclass);
        }

        /// <summary>
        /// The th element defines a table header cell.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <param name="title"></param>
        /// <returns>Constructed Tag</returns>
        public Tag Th(string id, string cssclass, string title)
        {
            return Th(id, cssclass).AddAttribute(XhtmlAttribute.Title, title);
        } 
        #endregion

        /// <summary>
        /// The thead element can be used to group table rows that contain table header information. 
        /// This can be useful when printing long tables that span several printed pages, 
        /// since the data in thead will be repeated on each page.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Thead()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Thead);
        }


        /// <summary>
        /// The thead element can be used to group table rows that contain table header information. 
        /// This can be useful when printing long tables that span several printed pages, 
        /// since the data in thead will be repeated on each page.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Thead(string id)
        {
            return Thead().AddAttribute(XhtmlAttribute.Id, id);
        }

        #region Tr Element

        /// <summary>
        /// The tr element defines a table row.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Tr()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Tr);
        }

        /// <summary>
        /// The tr element defines a table row.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Tr(string id)
        {
            return Tr().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The tr element defines a table row.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Tr(string id, string cssclass)
        {
            return AddCssClass(Tr(id), cssclass);
        }

        /// <summary>
        /// The tr element defines a table row.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <param name="title"></param>
        /// <returns>Constructed Tag</returns>
        public Tag Tr(string id, string cssclass, string title)
        {
            return Tr(id, cssclass).AddAttribute(XhtmlAttribute.Title, title);
        }  
        #endregion

        #region Img Element
        
        /// <summary>
        /// The img element is used to define an image.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <param name="cssclass">CSS class for element</param>
        /// <param name="src"></param>
        /// <param name="alt"></param>
        /// <returns>Constructed Tag</returns>
        public Tag Img(string id, string cssclass, string src, string alt)
        {
            return AddCssClass(CreateTag(_writer, XhtmlTextWriterTag.Img)
                                   .AddAttribute(XhtmlAttribute.Id, id)
                                   .AddAttribute(XhtmlAttribute.Src, src)
                                   .AddAttribute(XhtmlAttribute.Alt, alt), cssclass);
        }
        #endregion

        /// <summary>
        /// The area element identifies geometric regions of a client-side image map, 
        /// and provides a hyperlink for each region.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Area()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Area);
        }

        /// <summary>
        /// The map element specifies a client-side image map that may be referenced 
        /// by elements such as img, select and object.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Map()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Map);
        }

        /// <summary>
        /// The map element specifies a client-side image map that may be referenced 
        /// by elements such as img, select and object.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Map(string id)
        {
            return Map().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// The meta element is a generic mechanism for specifying metadata for a Web page. 
        /// Some search engines use this information.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Meta()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Meta);
        }

        /// <summary>
        /// The noscript element allows authors to provide alternate content when a script 
        /// is not executed. This can be because the Web browser is configured not to 
        /// process scripts, or because the given script language is not supported.
        /// <br />
        /// Block Level Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag NoScript()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.NoScript);
        }

        /// <summary>
        /// The script element places a client-side script, such as JavaScript, within a document. 
        /// This element may appear any number of times in the head or body of a Web page. 
        /// The script element may contain a script (called an embedded script) or point via 
        /// the src attribute to a file containing a script (an external script).
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Script()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Script);
        }

        /// <summary>
        /// The style element can contain CSS rules (called embedded CSS) or a URL that leads 
        /// to a file containing CSS rules (called external CSS).
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Style()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Style);
        }

        /// <summary>
        /// The link element conveys relationship information that can be used by Web browsers and 
        /// search engines. You can have multiple link elements that link to different resources 
        /// or describe different relationships. The link elements can be contained in the head element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Link()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Link);
        }

        /// <summary>
        /// To resolve relative URLs, Web browsers will use the base URL from where the Web page 
        /// was downloaded. In some circumstances, it is necessary to instruct the Web browser to 
        /// use a different base URL, in which case the base element is used.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Base()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Base);
        }
        
        /// <summary>
        /// The Iframe Module defines an element for the definition of inline frames
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Iframe()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Iframe);
        }

        /// <summary>
        /// The Iframe Module defines an element for the definition of inline frames
        /// </summary>
        /// <param name="id">XHTML id attribute of element</param>
        /// <returns>Constructed Tag</returns>
        public Tag Iframe(string id)
        {
            return Iframe().AddAttribute(XhtmlAttribute.Id, id);
        }

        /// <summary>
        /// Ruby is mechanism for adding annotations to characters of East Asian languages such 
        /// as Chinese and Japanese. These annotations typically appear in smaller typeface 
        /// above or to the side of regular text, and are meant to help with pronunciation 
        /// of obscure characters or as a language learning aid.
        /// <br />
        /// Inline Element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Ruby()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Ruby);
        }

        /// <summary>
        /// The rbc element is a base container for rb elements in cases of complex ruby markup. 
        /// Only one rbc element may appear inside a ruby element.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Rbc()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Rbc);
        }

        /// <summary>
        /// The rtc element is a ruby text container for rt elements in cases of complex ruby markup.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Rtc()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Rtc);
        }

        /// <summary>
        /// The rb element contains base text.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Rb()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Rb);
        }

        /// <summary>
        /// The rt element contains ruby text annotations.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Rt()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Rt);
        }

        /// <summary>
        /// The rp element can be used to specify characters that can denote the beginning and 
        /// end of ruby text. The rp element should only be used for simple ruby markup, 
        /// and when Web browsers do not support standard ruby notation.
        /// </summary>
        /// <returns>Constructed Tag</returns>
        public Tag Rp()
        {
            return CreateTag(_writer, XhtmlTextWriterTag.Rp);
        }


    }
}
