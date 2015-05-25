/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System.Web;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;

namespace Gaia.WebWidgets.Extensions
{
    using HtmlFormatting;

    /// <summary>
    /// The Gaia Ajax Toolbar (or Menu if you like) is a container with a collection of <see cref="ToolbarItem"/> which
    /// in turn can be as complex or as simple as you want them to be.
    /// You can add up any widget you like inside your Gaia Ajax Toolbar. Including <see cref="Button"/>, <see cref="CheckBox"/>, 
    /// <see cref="DateTimePicker"/> and even <see cref="AutoCompleter"/> and Repeaters if you want to.
    /// And everything is ajaxified automatically. Some of the key features of the Gaia Ajax Toolbar is that it does its work and nothing more. 
    /// It is small, works 100% fine together with search engines and it is highly flexible. 
    /// </summary>
    [Themeable(true)]
    [PersistChildren(false)]
    [DefaultProperty("ToolbarItems")]
    [ParseChildren(typeof(ToolbarItem))]
    [ToolboxData("<{0}:Toolbar runat=\"server\"></{0}:Toolbar>")]
    [ToolboxBitmap(typeof(Toolbar), "Resources.Gaia.WebWidgets.Extensions.Toolbar.bmp")]
    [Designer("Gaia.WebWidgets.Extensions.Design.ToolbarDesigner, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    [ToolboxItem("Gaia.WebWidgets.ComponentModel.Design.GaiaExtensionControlToolboxItem, Gaia.WebWidgets.ComponentModel.4.0.172.1, Version=4.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a")]
    public class Toolbar : Panel, ISkinControl
    {
        #region [ -- Properties -- ]

        /// <summary>
        /// The collection of <see cref="ToolbarItem"/>.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("Gaia.WebWidgets.Extensions.Design.ToolbarItemCollectionEditor, Gaia.WebWidgets.Design, Version=2.0.172.1, Culture=neutral, PublicKeyToken=e3fc7cc7a1b1114a", typeof(UITypeEditor))]
        public ToolbarItemCollection ToolbarItems
        {
            get { return (ToolbarItemCollection) Controls; }
        }

        #endregion

        #region [ -- Overridden base class methods and properties -- ]

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.
        /// </returns>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Ul; }
        }

        /// <summary>
        /// Overridden to get the correct collection type
        /// </summary>
        /// <returns>ToolbarItemCollection</returns>
        protected override ControlCollection CreateControlCollection()
        {
            return new ToolbarItemCollection(this);
        }

        /// <summary>
        /// Notifies the server control that an element, either XML or HTML, was parsed, and adds the element to the server control's <see cref="T:System.Web.UI.ControlCollection"/> object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object"/> that represents the parsed element. </param>
        protected override void AddParsedSubObject(object obj)
        {
            // add ToolbarItems, ignore LiteralControls, and throw on other controls
            if (obj is ToolbarItem)
                base.AddParsedSubObject(obj);
            else if (!(obj is LiteralControl))
                throw new HttpException("Accordion cannot have children of type " + obj.GetType().Name);
        }

        /// <summary>
        /// Called after a child control is added to the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"/> that has been added.</param>
        /// <param name="index">The index of the control in the <see cref="P:System.Web.UI.Control.Controls"/> collection.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="control"/> is a <see cref="T:System.Web.UI.WebControls.Substitution"/>  control.
        /// </exception>
        protected override void AddedControl(Control control, int index)
        {
            var item = (ToolbarItem) control;
            item.Owner = this;
            base.AddedControl(control, index);
        }

        /// <summary>
        /// Called after a child control is removed from the <see cref="P:System.Web.UI.Control.Controls"/> collection of the <see cref="T:System.Web.UI.Control"/> object.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"/> that has been removed.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// The control is a <see cref="T:System.Web.UI.WebControls.Substitution"/> control.
        /// </exception>
        protected override void RemovedControl(Control control)
        {
            base.RemovedControl(control);
            var item = (ToolbarItem) control;
            item.Owner = null;
        }

        /// <summary>
        /// Renders <see cref="Toolbar"/>
        /// </summary>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            using (var ul = create.Ul(ClientID, Css.Combine(CssClass, false, "toolbar")))
            {
                Css.SerializeAttributesAndStyles(this, ul);
                RenderChildren(create.GetHtmlTextWriter());
            }
        }

        #endregion

        #region [ -- ISkinControl Implementation -- ]

        void ISkinControl.ApplySkin()
        {
            // include default skin css file
            ((IAjaxControl)this).AjaxControl.RegisterDefaultSkinStyleSheetFromResource(typeof(Toolbar), Constants.DefaultSkinResource);

            // name of the default skin;
            CssClass = Constants.DefaultSkinCssClass;
        }

        bool ISkinControl.Enabled
        {
            get { return string.IsNullOrEmpty(CssClass) || CssClass.Equals(Constants.DefaultSkinCssClass); }
        }

        #endregion
    }
}
    