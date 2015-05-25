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
    /// These are all the attributes in the XHTML 1.1 standard, not that of course not all of them
    /// can be added to all of the elements. The Gaia XHTML Serializer will throw an exception
    /// if you try to add the wrong attribute to an element which doesn't support it though.
    /// </summary>
    public enum XhtmlAttribute
    {
        /// <summary>
        /// URI. This attribute specifies the location of a Web resource.
        /// </summary>
        Href,

        /// <summary>
        /// This maps to the class attribute
        /// This attribute assigns a class name or set of class names to an element. 
        /// Any number of elements may be assigned the same class name or set of class names. 
        /// Multiple class names must be separated by white space characters.
        /// </summary>
        CssClass,

        /// <summary>
        /// This attribute assigns an ID to an element. This ID must be unique in a document.
        /// </summary>
        Id,

        /// <summary>
        /// This attribute offers advisory information. Some Web browsers will display this 
        /// information as tooltips.
        /// </summary>
        Title,

        /// <summary>
        /// This maps to the xml:lang attribute.
        /// This attribute specifies the base language of an element's attribute values and text content.
        /// </summary>
        XmlLang,

        /// <summary>
        /// This attribute specifies the base direction of text.
        /// Must contain either ltr or rtl.
        /// </summary>
        Dir,

        // Common event handler attributes

        /// <summary>
        /// A client-side script event that occurs when a pointing device button is clicked over an element.
        /// </summary>
        OnClick,

        /// <summary>
        /// A client-side script event that occurs when a pointing device button is double-clicked 
        /// over an element.
        /// </summary>
        OnDblClick,

        /// <summary>
        /// A client-side script event that occurs when a pointing device button is pressed down 
        /// over an element.
        /// </summary>
        OnMouseDown,

        /// <summary>
        /// A client-side script event that occurs when a pointing device button is released 
        /// over an element.
        /// </summary>
        OnMouseUp,

        /// <summary>
        /// A client-side script event that occurs when a pointing device is moved onto an element.
        /// </summary>
        OnMouseOver,

        /// <summary>
        /// A client-side script event that occurs when a pointing device is moved within an element.
        /// </summary>
        OnMouseMove,

        /// <summary>
        /// A client-side script event that occurs when a pointing device is moved away from an element.
        /// </summary>
        OnMouseOut,

        /// <summary>
        /// A client-side script event that occurs when a key is pressed down over an element then released.
        /// </summary>
        OnKeyPress,

        /// <summary>
        /// A client-side script event that occurs when a key is pressed down over an element.
        /// </summary>
        OnKeyDown,

        /// <summary>
        /// A client-side script event that occurs when a key is released over an element.
        /// </summary>
        OnKeyUp,

        // Body special event handler attributes

        /// <summary>
        /// A client-side script event that occurs when the document has been loaded.
        /// </summary>
        OnLoad,

        /// <summary>
        /// A client-side script event that occurs when the document has been removed.
        /// </summary>
        OnUnload,

        // Form special event handler attributes

        /// <summary>
        /// A client-side script event that occurs when a form is reset (all form 
        /// controls are set to their initial values).
        /// </summary>
        OnReset,

        /// <summary>
        /// A client-side script event that occurs when a form is submitted 
        /// (form data is sent to server-side scripts for processing).
        /// </summary>
        OnSubmit,

        /// <summary>
        /// This attribute specifies formatting style information for the current element. 
        /// The content of this attribute is called inline CSS. The style attribute is 
        /// deprecated (considered outdated), because it fuses together content and 
        /// formatting.
        /// </summary>
        Style,

        /// <summary>
        /// This attribute specifies the intended destination medium for style information. 
        /// It may be a single media descriptor or a comma-separated list. 
        /// The default value for this attribute is screen.
        /// </summary>
        Media,

        /// <summary>
        /// For the link tag;
        /// Style sheet language. For example: text/css.
        /// </summary>
        Type,

        /// <summary>
        /// Specifies the primary language of the resource designated 
        /// by href and may only be used when href is specified.
        /// </summary>
        HrefLang,

        /// <summary>
        /// Describes the forward relationship from the current document to 
        /// the resource specified by the href attribute. The value of this 
        /// attribute is a space-separated list of link types.
        /// </summary>
        Rel,

        /// <summary>
        /// Describes the reverse relationship back to the current document, 
        /// to the resource specified by the href attribute. The value of this 
        /// attribute is a space-separated list of link types.
        /// </summary>
        Rev,

        /// <summary>
        /// Form control name.
        /// </summary>
        Name,

        /// <summary>
        /// Specifies a property's value in the META element
        /// </summary>
        Content,

        /// <summary>
        /// This attribute may be used in place of the name attribute of the META element. HTTP 
        /// servers use this attribute to gather information for HTTP response message headers.
        /// </summary>
        HttpEquiv,

        /// <summary>
        /// Names a scheme to be used to interpret the property's value in the META element
        /// </summary>
        Scheme,

        /// <summary>
        /// This required attribute specifies the location of the image source.
        /// </summary>
        Src,

        /// <summary>
        /// When set on SCRIPT element, this boolean attribute provides a hint to 
        /// the user agent that the script is not going to generate any document 
        /// content (e.g., no "document.write" in javascript) and thus, the user 
        /// agent can continue parsing and rendering.
        /// </summary>
        Defer,

        /// <summary>
        /// The special attribute xml:space may be attached to an element to 
        /// signal an intention that in that element, white space should be 
        /// preserved by applications. In valid documents, this attribute, 
        /// like any other, MUST be declared if it is used. When declared, it 
        /// MUST be given as an enumerated type whose values are one or both 
        /// of "default" and "preserve".
        /// </summary>
        XmlSpace,

        /// <summary>
        /// URI. This attribute is intended to supply information about 
        /// the source from which the quotation was borrowed.
        /// </summary>
        Cite,

        /// <summary>
        /// This is used to indicate the date and time when the content change was made.
        /// </summary>
        DateTime,

        /// <summary>
        /// Specifies the location of the server-side script used to process data collected in the form.
        /// </summary>
        Action,

        /// <summary>
        /// Specifies the type of HTTP method used to send data to the server.
        /// Possible values are get and post
        /// </summary>
        Method,

        /// <summary>
        /// This attribute specifies a comma-separated list of content types that a server 
        /// processing the form will handle correctly.
        /// </summary>
        Accept,

        /// <summary>
        /// This attribute specifies the list of character encodings for input data that 
        /// are accepted by the server processing the form.
        /// </summary>
        AcceptCharset,

        /// <summary>
        /// This attribute specifies the content type used to send form data to the server 
        /// when the value of method is post. The default value for this attribute is 
        /// application/x-www-form-urlencoded. If a form contains a file upload control 
        /// (input element with type value of file), then this attribute value should be 
        /// multipart/form-data.
        /// </summary>
        EncType,

        /// <summary>
        /// This attributes specifies the width (in pixels) of the border around table cells.
        /// </summary>
        Border,

        /// <summary>
        /// This attribute specifies the amount of space between the border of the cell 
        /// and its contents.
        /// </summary>
        CellPadding,

        /// <summary>
        /// This attribute specifies the amount of space between the border of the cell 
        /// and the table frame or other cells.
        /// </summary>
        CellSpacing,

        /// <summary>
        /// This attribute provides a summary of the table's purpose and structure, 
        /// for devices rendering to non-visual media such as speech and Braille.
        /// </summary>
        Summary,

        /// <summary>
        /// This attribute specifies the desired width.
        /// </summary>
        Width,

        /// <summary>
        /// This attribute specifies which sides of the frame surrounding a table will be visible.
        /// Possible values:<br />
        /// void, above, below, hsides, vsides, lhs, rhs, box, border
        /// </summary>
        Frame,

        /// <summary>
        /// This attribute specifies which rules (lines) will appear between cells within a table.
        /// Possible values:<br />
        /// none, groups, rows, cols, all
        /// </summary>
        Rules,

        /// <summary>
        /// Accessibility key character. Normally in combination with ALT the element will receive focus.
        /// </summary>
        AccessKey,

        /// <summary>
        /// Specifies the character encoding of the resource designated by the link (A element)
        /// </summary>
        Charset,

        /// <summary>
        /// Specifies the position and shape on the screen. The number and order of values depends 
        /// on the shape being defined.
        /// </summary>
        Coords,

        /// <summary>
        /// Event occuring when element looses focus
        /// </summary>
        OnBlur,

        /// <summary>
        /// Element occuring when wlement gains focus
        /// </summary>
        OnFocus,

        /// <summary>
        /// Specifies the shape of a region
        /// </summary>
        Shape,

        /// <summary>
        /// Numeric value describing the order controls will receive focus if TAB is clicked
        /// </summary>
        TabIndex,

        /// <summary>
        /// Value of form element input field.
        /// </summary>
        Value,

        /// <summary>
        /// Disables a form control. Possible value is only disabled.
        /// </summary>
        Disabled,

        /// <summary>
        /// Alternative text for element
        /// </summary>
        Alt,

        /// <summary>
        /// Height of element
        /// </summary>
        Height,

        /// <summary>
        /// Boolean attribute, must be set for server-side image maps for IMG and INPUT (of type image) elements
        /// </summary>
        IsMap,

        /// <summary>
        /// For FRAME and IFRAME elements, specifies a link to a long description of the frame. This 
        /// description should supplement the short description provided using the title attribute. For IMG 
        /// element, Specifies a link to a long description of the image. This description should supplement 
        /// the short description provided using the alt attribute. When the image has an associated image map, 
        /// this attribute should provide information about the image map's contents. This is particularly important 
        /// for server-side image maps.
        /// </summary>
        LongDesc,

        /// <summary>
        /// Associates an image map with an element. The image map is defined by a MAP element. The value of usemap 
        /// must match the value of the name attribute of the associated MAP element.
        /// </summary>
        UseMap,

        /// <summary>
        /// When the type attribute of an input field has the value radio or checkbox, 
        /// this attribute specifies that the radio/checkbox is selected.
        /// </summary>
        Checked,

        /// <summary>
        /// Max length of value of an input field
        /// </summary>
        MaxLength,

        /// <summary>
        /// This attribute tells the Web browser the initial width of the control. 
        /// The width is given in pixels except when the type attribute has the value 
        /// text or password. In such cases, its value is the number of characters.
        /// </summary>
        Size,

        /// <summary>
        /// Event occuring when input form element changes its value
        /// </summary>
        OnChange,

        /// <summary>
        /// Event occuring when input form element selects a different value
        /// </summary>
        OnSelect,

        /// <summary>
        /// If present, this attribute prohibits changes to the value in the control. 
        /// Possible value is readonly.
        /// </summary>
        ReadOnly,

        /// <summary>
        /// This attribute explicitly associates the label with a form control.
        /// </summary>
        For,

        /// <summary>
        /// Border of frame of Iframe
        /// </summary>
        FrameBorder,

        /// <summary>
        /// Height of margin between iframe and surroundings
        /// </summary>
        MarginHeight,

        /// <summary>
        /// Width of margin between iframe and surroundings
        /// </summary>
        MarginWidth,
        
        /// <summary>
        /// Whether iframe should have scrolling enabled or not
        /// </summary>
        Scrolling,

        /// <summary>
        /// May be used to specify the location of an object's implementation via a URI. It may be used together 
        /// with, or as an alternative to the data attribute, depending on the type of object involved.
        /// </summary>
        ClassId,

        /// <summary>
        /// For OBJECT element, specifies the base path used to resolve relative URIs specified by the 
        /// classid, data, and archive attributes. When absent, its default value is the base URI of 
        /// the current document. For the APPLET element, specifies the base URI for the applet. If this 
        /// attribute is not specified, then it defaults the same base URI as for the current document.
        /// </summary>
        CodeBase,

        /// <summary>
        /// For APPLET element, specifies a comma-separated list of URIs for archives containing classes 
        /// and other resources that will be "preloaded". For OBJECT element, specifies a space-separated 
        /// list of URIs for archives containing resources relevant to the object, which may include the 
        /// resources specified by the classid and data attributes.
        /// </summary>
        Archive,

        /// <summary>
        /// Specifies the content type of data expected when downloading the object specified by classid.
        /// </summary>
        CodeType,

        /// <summary>
        /// Specifies the location of the object's data, for instance image data for objects defining 
        /// images, or more generally, a serialized form of an object which can be used to recreate it.
        /// </summary>
        Data,

        /// <summary>
        /// This boolean attribute (if present) makes the current OBJECT definition a declaration 
        /// only. The object must be instantiated by a subsequent OBJECT definition referring to this declaration.
        /// </summary>
        Declare,

        /// <summary>
        /// Specifies a message that a user agent may render while loading the object's implementation and data.
        /// </summary>
        StandBy,

        /// <summary>
        /// If set, this boolean attribute allows multiple selections. If not set, the SELECT element only 
        /// permits single selections.
        /// </summary>
        Multiple,

        /// <summary>
        /// For TEXTAREA element, specifies the visible width in average character widths. For FRAMESET 
        /// element, specifies the layout of vertical frames. It is a comma-separated list of 
        /// pixels, percentages, and relative lengths. The default value is 100%, meaning one column.
        /// </summary>
        Cols,

        /// <summary>
        /// For TEXTAREA element, specifies the number of visible text lines. For FRAMESET element
        /// specifies the layout of horizontal frames. It is a comma-separated list of pixels, 
        /// percentages, and relative lengths. The default value is 100%, meaning one row.
        /// </summary>
        Rows,

        /// <summary>
        /// For the CAPTION element, specifies the position of the caption with respect to the table. For
        /// IMG, OBJECT and APPLET elements, specifies the position of the element with respect to its context.
        /// For LEGEND and FIELDSET elements, specifies the position of the legend with respect to the fieldset.
        /// For the TABLE element, specifies the position of the table with respect to the document. For the 
        /// HR element, specifies the horizontal alignment of the rule with respect to the surrounding context.
        /// For block level elements, specifies the horizontal alignment of its element with respect to the 
        /// surrounding context. For different table elements, specifies the alignment of data and the 
        /// justification of text in a cell.
        /// </summary>
        Align,

        /// <summary>
        /// For the COL element Specifies the number of columns "spanned" by the COL element. For the 
        /// COLGROUP element, specifies the number of columns in a column group.
        /// </summary>
        Span,

        /// <summary>
        /// Specifies the vertical position of data within a cell.
        /// </summary>
        Valign,

        /// <summary>
        /// Specifies a single character within a text fragment to act as an axis for alignment.
        /// </summary>
        Char,

        /// <summary>
        /// For alignment, specifies the offset to the first occurrence of the alignment character on each line.
        /// </summary>
        CharOff,

        /// <summary>
        /// Specifies the list of header cells that provide header information for the current data cell.
        /// </summary>
        Headers,

        /// <summary>
        /// Specifies the number of columns spanned by the current cell.
        /// </summary>
        ColSpan,

        /// <summary>
        /// Specifies the number of rows spanned by the current cell.
        /// </summary>
        RowSpan,

        /// <summary>
        /// Used to place a cell into conceptual categories that can be considered to form axes in an n-dimensional space.
        /// </summary>
        Axis,

        /// <summary>
        /// Used to provide an abbreviated form of the cell's content
        /// </summary>
        Abbr,

        /// <summary>
        /// Specifies the set of data cells for which the current header cell provides header information.
        /// </summary>
        Scope,

        /// <summary>
        /// Specifies the type of the value attribute of PARAM element.
        /// </summary>
        ValueType,

        /// <summary>
        /// In complex ruby markup, the rbspan attribute allows an rt element to span multiple rb elements.
        /// </summary>
        RbSpan
    }
}
