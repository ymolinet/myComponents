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
    /// This enum contain all the valid elements form XHTML 1.1
    /// </summary>
    public enum XhtmlTextWriterTag
    {
        // Structure Modul

        /// <summary>
        /// The body element contains the contents of a Web page.
        /// </summary>
        Body,

        /// <summary>
        /// The head element contains information about the current document, 
        /// such as its title, keywords that may be useful to search engines, 
        /// and other data that is not considered to be document content. 
        /// This information is usually not displayed by browsers.
        /// </summary>
        Head,

        /// <summary>
        /// The html element is the root element that contains all other elements. 
        /// It must appear only once and usually follows the document declaration.
        /// </summary>
        Html,

        /// <summary>
        /// The title element is used to identify the document.
        /// </summary>
        Title,

        // Text Module

        /// <summary>
        /// An abbreviation is a shortened form of a word or phrase. 
        /// The abbr element is used to identify an abbreviation, 
        /// and can help assistive technologies to correctly pronounce abbreviated text.
        /// <br />
        /// Inline Element.
        /// </summary>
        Abbr,

        /// <summary>
        /// An acronym is a word formed from the initial letters of a series of words. 
        /// The acronym element identifies acronyms, and can help assistive technologies 
        /// to correctly pronounce the acronym.
        /// <br />
        /// Inline Element.
        /// </summary>
        Acronym,

        /// <summary>
        /// The address element is used to supply contact information. 
        /// This element often appears at the beginning or end of a document.
        /// <br />
        /// Block Level Element.
        /// </summary>
        Address,

        /// <summary>
        /// The blockquote element is used to identify larger amounts of quoted text.
        /// <br />
        /// Block Level Element.
        /// </summary>
        BlockQuote,

        /// <summary>
        /// The br element forcibly breaks (ends) the current line of text. 
        /// Web browsers render these line breaks as hard returns.
        /// <br />
        /// Inline Element.
        /// </summary>
        Br,

        /// <summary>
        /// The cite element contains a citation or reference to another source.
        /// <br />
        /// Inline Element.
        /// </summary>
        Cite,

        /// <summary>
        /// The code element contains a fragment of computer code.
        /// <br />
        /// Inline Element.
        /// </summary>
        Code,

        /// <summary>
        /// The div element offers a generic way of grouping areas of content.
        /// <br />
        /// Block Level Element.
        /// </summary>
        Div,

        /// <summary>
        /// The dfn element contains the defining instance of the enclosed term.
        /// <br />
        /// Inline Element.
        /// </summary>
        Dfn,

        /// <summary>
        /// The em element is used to indicate emphasis.
        /// <br />
        /// Inline Element.
        /// </summary>
        Em,

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        H1,

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        H2,

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        H3,

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        H4,

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        H5,

        /// <summary>
        /// The elements h1 to h6 group the contents of a document 
        /// into sections, and briefly describe the topic of each section. 
        /// There are six levels of headings, h1 being the most important 
        /// and h6 the least important.
        /// <br />
        /// Block Level Element.
        /// </summary>
        H6,

        /// <summary>
        /// The kbd element indicates input to be entered by the user.
        /// <br />
        /// Inline Element.
        /// </summary>
        Kbd,

        /// <summary>
        /// The p element represents a paragraph.
        /// <br />
        /// Block Level Element.
        /// </summary>
        P,

        /// <summary>
        /// The pre element instructs visual Web browsers to render content 
        /// in a pre-formatted fashion. Most Web browsers will render 
        /// pre-formatted content in a monospace font while preserving 
        /// white space (spaces, tabs and hard returns).
        /// <br />
        /// Block Level Element.
        /// </summary>
        Pre,

        /// <summary>
        /// The q is used to identify short quoted text.
        /// <br />
        /// Inline Element.
        /// </summary>
        Q,

        /// <summary>
        /// The samp element is used to designate sample output 
        /// from programs, scripts, etc.
        /// <br />
        /// Block Level Element.
        /// <br />
        /// Inline Element. ALSO.
        /// </summary>
        Samp,

        /// <summary>
        /// The span element offers a generic way of adding structure to content.
        /// <br />
        /// Inline Element.
        /// </summary>
        Span,

        /// <summary>
        /// The strong element is used to indicate stronger emphasis.
        /// <br />
        /// Inline Element.
        /// </summary>
        Strong,

        /// <summary>
        /// The var element is used to indicate an instance of a 
        /// computer code variable or program argument.
        /// <br />
        /// Inline Element.
        /// </summary>
        Var,

        // Hypertext Module

        /// <summary>
        /// The a element is used to create a hyperlink. 
        /// The destination of the hyperlink is specified in the href attribute, 
        /// and the text or image for the hyperlink is specified between the opening 
        /// <a> and closing </a> tags.
        /// <br />
        /// Inline Element.
        /// </summary>
        A,

        // List Module

        /// <summary>
        /// The dl element is used to create a list where each item in the list 
        /// comprises two parts: a term and a description. A glossary of terms 
        /// is a typical example of a definition list, where each item consists 
        /// of the term being defined and a definition of the term.
        /// <br />
        /// Block Level Element.
        /// </summary>
        Dl,

        /// <summary>
        /// The dt element is a definition term for an item in a definition list.
        /// </summary>
        Dt,

        /// <summary>
        /// The dd element is a definition description for an item in a definition list.
        /// </summary>
        Dd,

        /// <summary>
        /// The ol element is used to create ordered lists. An ordered list is a grouping 
        /// of items whose sequence in the list is important. For example, the sequence 
        /// of steps in a recipe is important if the result is to be the intended one.
        /// Every ol element must contain at least one li element.
        /// <br />
        /// Block Level Element.
        /// </summary>
        Ol,

        /// <summary>
        /// The ul element is used to create unordered lists. An unordered list is a 
        /// grouping of items whose sequence in the list is not important. 
        /// For example, the order in which ingredients for a recipe are presented 
        /// will not affect the outcome of the recipe.
        /// Every ul element must contain at least one li element.
        /// <br />
        /// Block Level Element.
        /// </summary>
        Ul,

        /// <summary>
        /// The li element represents a list item in ordered lists and unordered lists.
        /// </summary>
        Li,

        // Object Modul

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
        Object,

        /// <summary>
        /// The param element is used to customize embedded objects that are loaded into a 
        /// Web browser via the object element. The param element is a generic way of passing 
        /// data to embedded objects in the form of name/value pairs. The need for param 
        /// elements and the number of param elements depends on the embedded object.
        /// </summary>
        Param,

        // Presentation Modul

        /// <summary>
        /// The b element renders text in bold style.
        /// <br />
        /// Inline Element.
        /// </summary>
        B,

        /// <summary>
        /// The big element renders text in a large font.
        /// <br />
        /// Inline Element.
        /// </summary>
        Big,

        /// <summary>
        /// The hr element is used to separate sections of content. 
        /// Though the name of the hr element is "horizontal rule", 
        /// most visual Web browsers render hr as a horizontal line.
        /// <br />
        /// Block Level Element.
        /// </summary>
        Hr,

        /// <summary>
        /// The i element renders text in italic style.
        /// <br />
        /// Inline Element.
        /// </summary>
        I,

        /// <summary>
        /// The small element renders text in a small font.
        /// <br />
        /// Inline Element.
        /// </summary>
        Small,

        /// <summary>
        /// The sub element indicates that its contents should be 
        /// regarded as a subscript.
        /// <br />
        /// Inline Element.
        /// </summary>
        Sub,

        /// <summary>
        /// The sup element indicates that its contents should regarded as superscript.
        /// <br />
        /// Inline Element.
        /// </summary>
        Sup,

        /// <summary>
        /// The tt element renders text in a teletype or a monospaced font.
        /// <br />
        /// Inline Element.
        /// </summary>
        Tt,

        // Edit Module

        /// <summary>
        /// The del element is used to mark up modifications made to a document. 
        /// Specifically, the del element is used to indicate that a section 
        /// of content has changed and has therefore been removed.
        /// <br />
        /// Block Level Element.
        /// <br />
        /// Inline Element. ALSO
        /// </summary>
        Del,

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
        Ins,

        // Bidirectional Module

        /// <summary>
        /// Unlike English, which is written from left-to-right (LTR), some languages, 
        /// such as Arabic and Hebrew, are written from right-to-left (RTL). 
        /// When the same paragraph contains both RTL and LTR text, this is known as 
        /// bidirectional text or "bidi" text for short. 
        /// <br />
        /// Inline Element.
        /// </summary>
        Bdo,

        // Forms Module

        /// <summary>
        /// The button element is used to create button controls for forms. 
        /// Buttons created using the button element are similar in functionality 
        /// to buttons created using the input element, but offer greater rendering options.
        /// <br />
        /// Inline Element.
        /// </summary>
        Button,

        /// <summary>
        /// The fieldset element adds structure to forms by grouping together 
        /// related controls and labels.
        /// <br />
        /// Block Level Element.
        /// </summary>
        FieldSet,

        /// <summary>
        /// The form element is used to create data entry forms. 
        /// Data collected in the form is sent to the server for 
        /// processing by server-side scripts such as PHP, ASP, etc.
        /// <br />
        /// Block Level Element.
        /// </summary>
        Form,

        /// <summary>
        /// The input element is a multi-purpose form control. 
        /// The type attribute specifies the type of form control to be created.
        /// <br />
        /// Inline Element.
        /// </summary>
        Input,

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
        Label,

        /// <summary>
        /// The legend element is a caption to a fieldset element.
        /// </summary>
        Legend,

        /// <summary>
        /// The select element is used to create an option selector 
        /// form control which most Web browsers render as a listbox control. 
        /// The list of values for this control is created using option elements. 
        /// These values can be grouped together using the optgroup element.
        /// <br />
        /// Inline Element.
        /// </summary>
        Select,

        /// <summary>
        /// The optgroup element is used to group the choices offered in select form controls. 
        /// Users find it easier to work with long lists if related sections are grouped together.
        /// </summary>
        OptGroup,

        /// <summary>
        /// The option element represents a choice offered by select form controls.
        /// </summary>
        Option,

        /// <summary>
        /// The textarea element is used to create a multi-line text input form control.
        /// <br />
        /// Inline Element.
        /// </summary>
        TextArea,

        // Table Module

        /// <summary>
        /// The caption element creates a caption for a table. If a caption is to be used, 
        /// it should be the first element after the opening table element.
        /// </summary>
        Caption,

        /// <summary>
        /// In XHTML, tables are physically constructed from rows, rather than columns. 
        /// Table rows contain table cells. In visual Web browsers, when cells line up 
        /// beneath each other, they are perceived as columns.
        /// </summary>
        Col,

        /// <summary>
        /// In XHTML, tables are physically constructed from rows, rather than columns. 
        /// Table rows contain table cells. In visual Web browsers, when cells line up 
        /// beneath each other, they are perceived as columns.
        /// </summary>
        ColGroup,

        /// <summary>
        /// The table element is used to define a table. A table is a construct where data 
        /// is organized into rows and columns of cells.
        /// <br />
        /// Block Level Element.
        /// </summary>
        Table,

        /// <summary>
        /// The tbody element can be used to group table data rows. This can be useful 
        /// when a Web browser supports scrolling of table rows in longer tables. 
        /// Multiple tbody elements can be used for independent scrolling.
        /// </summary>
        Tbody,

        /// <summary>
        /// The td element defines a data cell in a table (i.e. cells that are not header cells).
        /// </summary>
        Td,

        /// <summary>
        /// The tfoot element can be used to group table rows that contain table footer information. 
        /// This may be useful when printing longer tables that span several printed pages, 
        /// since the data in tfoot is repeated on each page. The tfoot element should appear before 
        /// tbody elements.
        /// </summary>
        Tfoot,

        /// <summary>
        /// The th element defines a table header cell.
        /// </summary>
        Th,

        /// <summary>
        /// The thead element can be used to group table rows that contain table header information. 
        /// This can be useful when printing long tables that span several printed pages, 
        /// since the data in thead will be repeated on each page.
        /// </summary>
        Thead,

        /// <summary>
        /// The tr element defines a table row.
        /// </summary>
        Tr,

        // Image module

        /// <summary>
        /// The img element is used to define an image.
        /// <br />
        /// Inline Element.
        /// </summary>
        Img,

        // Client side Image Map Module

        /// <summary>
        /// The area element identifies geometric regions of a client-side image map, 
        /// and provides a hyperlink for each region.
        /// </summary>
        Area,

        /// <summary>
        /// The map element specifies a client-side image map that may be referenced 
        /// by elements such as img, select and object.
        /// <br />
        /// Inline Element.
        /// </summary>
        Map,

        // Metainformation Module

        /// <summary>
        /// The meta element is a generic mechanism for specifying metadata for a Web page. 
        /// Some search engines use this information.
        /// </summary>
        Meta,

        // Scripting Module

        /// <summary>
        /// The noscript element allows authors to provide alternate content when a script 
        /// is not executed. This can be because the Web browser is configured not to 
        /// process scripts, or because the given script language is not supported.
        /// <br />
        /// Block Level Element.
        /// </summary>
        NoScript,

        /// <summary>
        /// The script element places a client-side script, such as JavaScript, within a document. 
        /// This element may appear any number of times in the head or body of a Web page. 
        /// The script element may contain a script (called an embedded script) or point via 
        /// the src attribute to a file containing a script (an external script).
        /// <br />
        /// Inline Element.
        /// </summary>
        Script,

        // Stylesheet module

        /// <summary>
        /// The style element can contain CSS rules (called embedded CSS) or a URL that leads 
        /// to a file containing CSS rules (called external CSS).
        /// </summary>
        Style,

        // Link Module

        /// <summary>
        /// The link element conveys relationship information that can be used by Web browsers and 
        /// search engines. You can have multiple link elements that link to different resources 
        /// or describe different relationships. The link elements can be contained in the head element.
        /// </summary>
        Link,

        // Base module

        /// <summary>
        /// To resolve relative URLs, Web browsers will use the base URL from where the Web page 
        /// was downloaded. In some circumstances, it is necessary to instruct the Web browser to 
        /// use a different base URL, in which case the base element is used.
        /// </summary>
        Base,

        // Iframe module

        /// <summary>
        /// The Iframe Module defines an element for the definition of inline frames
        /// </summary>
        Iframe,
        
        // Ruby Annotation Module

        /// <summary>
        /// Ruby is mechanism for adding annotations to characters of East Asian languages such 
        /// as Chinese and Japanese. These annotations typically appear in smaller typeface 
        /// above or to the side of regular text, and are meant to help with pronunciation 
        /// of obscure characters or as a language learning aid.
        /// <br />
        /// Inline Element.
        /// </summary>
        Ruby,

        /// <summary>
        /// The rbc element is a base container for rb elements in cases of complex ruby markup. 
        /// Only one rbc element may appear inside a ruby element.
        /// </summary>
        Rbc,

        /// <summary>
        /// The rtc element is a ruby text container for rt elements in cases of complex ruby markup.
        /// </summary>
        Rtc,

        /// <summary>
        /// The rb element contains base text.
        /// </summary>
        Rb,

        /// <summary>
        /// The rt element contains ruby text annotations.
        /// </summary>
        Rt,

        /// <summary>
        /// The rp element can be used to specify characters that can denote the beginning and 
        /// end of ruby text. The rp element should only be used for simple ruby markup, 
        /// and when Web browsers do not support standard ruby notation.
        /// </summary>
        Rp
    }
}
