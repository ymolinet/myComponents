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
using System.ComponentModel;
using System.IO;
using System.Drawing;
using Gaia.WebWidgets.HtmlFormatting;
using System.Web.UI;

[assembly: WebResource("Gaia.WebWidgets.Scripts.DynamicImage.js", "text/javascript")]

namespace Gaia.WebWidgets
{
    /// <summary>
    /// A Gaia Ajax DynamicImage can be thought of like a "normal" image except that it has no src attribute. Instead
    /// it has the capability of "dynamically render" its image through a publically available event.
    /// The DynamicImage has an event called RetrieveImage which will be called when it's time
    /// to "update" the image control with new "image data". From this event you can construct a System.Drawing.Bitmap object
    /// which will be rendered onto the surface of your DynamicImage. This makes it very easy to e.g. create a
    /// <a href="http://en.wikipedia.org/wiki/Captcha">Captcha Control</a> or render images which only exist 
    /// in your database etc.
    /// </summary>
    ///<remarks>
    /// In order to raise the RetrieveImage to re-render the Image you need to set its ImageId property to a new unique
    /// value. Unique to make sure the browser doesn't reuse a previously fetched Image from its cache. This is easy to achieve by
    /// using e.g. "Guid.NewGuid().ToString();".
    /// </remarks>
    /// <example>
    /// <code title="ASPX Markup for DynamicImage" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\DynamicImage\Overview\Default.aspx" />
    /// </code> 
    /// <code title="Codebehind for DynamicImage Retrieve Event" lang="C#"> 
    /// <code source="..\..\samples\Gaia.WebWidgets.Samples\BasicControls\DynamicImage\Overview\Default.aspx.cs" region="DynamicImageRetrieveEvent" />
    /// </code> 
    /// </example>
    [DefaultEvent("RetrieveImage")]
    [ToolboxData("<{0}:DynamicImage runat=\"server\" />")]
    [ToolboxBitmap(typeof(DynamicImage), "Resources.Gaia.WebWidgets.DynamicImage.bmp")]
    public class DynamicImage : GaiaWebControl, IAjaxControl
    {
        #region [ -- EventArgs nested classes -- ]

        /// <summary>
        /// Passed to RetrieveImage event when an Image is needed for you DynamicImage object
        /// </summary>
        public class RetrieveImageEventArgs : EventArgs
        {
            /// <summary>
            /// How to flush image back to Client
            /// </summary>
            public enum ImageType
            {
                /// <summary>
                /// JPEG
                /// </summary>
                Jpeg,

                /// <summary>
                /// PNG (use this is you can)
                /// </summary>
                Png,

                /// <summary>
                /// GIF
                /// </summary>
                Gif
            };

            private readonly string _idOfImage;
            private int _minutesCached = 60 * 24 * 365 * 3;

            /// <summary>
            /// How to render image, JPEG, GIF or PNG
            /// </summary>
            public ImageType RenderType { get; set; }

            /// <summary>
            /// How many minutes the browser should cache this image. Set this to a very high
            /// value if the images you're using with the same ImageId always will be the same.
            /// If you set it to -1 the browser will not cache this at all.
            /// Defaults to three years.
            /// </summary>
            public int MinutesCached
            {
                get { return _minutesCached; }
                set { _minutesCached = value; }
            }

            /// <summary>
            /// This is the actually image to display into the Control
            /// </summary>
            public System.Drawing.Image Image { get; set; }

            /// <summary>
            /// This is the Id of the Image, use this one to be able to reuse the same event handler for more than one DynamicImage control
            /// </summary>
            public string IdOfImage
            {
                get { return _idOfImage; }
            }

            internal RetrieveImageEventArgs(string idOfImage)
            {
                _idOfImage = idOfImage;
            }
        }

        #endregion

        #region [ -- Properties and Events -- ]

    
        /// <summary>
        /// Event is fired when the control is clicked. Use the eventArgs to retrieve the mouse coordinates for the 
        /// click event
        /// </summary>
        [Description("Event is fired when the control is clicked")]
        public event EventHandler<AspectClickable.ClickEventArgs> Click
        {
            add { Aspects.Bind<AspectClickable>().Clicked += value; }
            remove { Aspects.Bind<AspectClickable>().Clicked -= value; }
        }

        /// <summary>
        /// Event is fired when the control is double clicked. Use the eventArgs to retrieve the mouse coordinates for the 
        /// click event
        /// </summary>
        [Description("Event is fired when the control is double clicked")]
        public event EventHandler<AspectClickable.ClickEventArgs> DoubleClick
        {
            add { Aspects.Bind<AspectClickable>().DblClicked += value; }
            remove { Aspects.Bind<AspectClickable>().DblClicked -= value; }
        }


        /// <summary>
        /// Handle this event to populate the image with actual data.
        /// The most important member is the Image property which contains the actual image that
        /// will be rendered
        /// </summary>
        public event EventHandler<RetrieveImageEventArgs> RetrieveImage;

        /// <summary>
        /// The Id of the Image.
        /// Use this one to be able to reuse the same RetrieveImage event handler for multiple objects
        /// </summary>
        [DefaultValue("")]
        [Description("Use this one to be able to re-use the same RetrieveImage event handler for several DynamicImages")]
        [AjaxSerializable("setImageId")]
        public string ImageId
        {
            get { return StateUtil.Get(ViewState, "ImageId", string.Empty); }
            set { StateUtil.Set(ViewState, "ImageId", value, string.Empty); }
        }

        /// <summary>
        /// The ALT text of the Image.
        /// </summary>
        [DefaultValue("")]
        [Description("Becomes the ALT text of the image")]
        [AjaxSerializable("setAltText")]
        public string AltText
        {
            get { return StateUtil.Get(ViewState, "AltText", string.Empty); }
            set { StateUtil.Set(ViewState, "AltText", value, string.Empty); }
        }

        /// <summary>
        /// Returns the IMG HTML tag key to the system
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Img; }
        }

        #endregion

        #region [ -- Overridden base class methods -- ]

        /// <summary>
        /// Retrieving the image data for the control.
        /// </summary>
        /// <param name="e">The EventArgs passed on from the System</param>
        protected override void OnInit(EventArgs e)
        {
            if (!HandleImageRetrieval())
                base.OnInit(e);
        }

        /// <summary>
        /// Include dynamic image javascript files
        /// </summary>
        protected override void IncludeScriptFiles()
        {
            base.IncludeScriptFiles();

            // DynamicImage Javascript stuff
            Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Scripts.DynamicImage.js", typeof(Manager), "Gaia.DynamicImage.browserFinishedLoading", true);
        }

        /// <summary>
        /// Render xhtml compliant markup for the dynamic image
        /// </summary>
        /// <param name="create">XhtmlTagFactory to use for xhtml compliant markup generation</param>
        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
 	        using (Tag img = create.Img(ClientID, CssClass, GenerateImageSource(), AltText))
            {
                Css.SerializeAttributesAndStyles(this, img);
            }
        }

        #endregion

        #region [ -- Helpers -- ]

        private bool HandleImageRetrieval()
        {
            // Checking to see if this is a retrieval of the Image
            if (DesignMode || Page.Request.Params["Gaia.WebWidgets.DynamicImage.GetImage"] != ClientID)
                return false;

            try
            {
                Page.Response.ClearContent();
                if (RetrieveImage != null)
                {
                    var e2 = new RetrieveImageEventArgs(Page.Request.Params["ImageId"]);
                    RetrieveImage(this, e2);
                    if (e2.MinutesCached != -1)
                    {
                        DateTime expires = DateTime.Now.AddMinutes(e2.MinutesCached);
                        Page.Response.Cache.SetExpires(expires);
                        Page.Response.Cache.SetValidUntilExpires(true);
                        Page.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
                    }

                    // Now rendering actually IMAGE
                    switch (e2.RenderType)
                    {
                        case RetrieveImageEventArgs.ImageType.Gif:
                            Page.Response.ContentType = "image/gif";
                            e2.Image.Save(Page.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case RetrieveImageEventArgs.ImageType.Jpeg:
                            Page.Response.ContentType = "image/jpeg";
                            e2.Image.Save(Page.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case RetrieveImageEventArgs.ImageType.Png:
                            {
                                // Saving PNGs requires a seekable stream and therefore we must
                                // "go by" a MemoryStream...
                                Page.Response.ContentType = "application/octet-stream";
                                var m = new MemoryStream();
                                e2.Image.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                                m.WriteTo(Page.Response.OutputStream);
                            }
                            break;
                    }
                    Page.Response.Flush();
                    e2.Image.Dispose();
                }
                else
                {
                    var def = new Bitmap(100, 100);
                    var g = Graphics.FromImage(def);
                    g.DrawString("No image", new Font(FontFamily.GenericSansSerif, 12F), Brushes.Red, 0F, 0F);
                    def.Save(Page.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    def.Dispose();
                }
                Page.Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
                return true; // Do nothing...!!
            }

            return false;
        }

        private string GenerateImageSource()
        {
            if (DesignMode)
                return string.Empty;

            // Rendering URL of actual Image
            string imgSrc = Page.Request.Url.ToString();
            string andOrQuestion = imgSrc.IndexOf("?", StringComparison.Ordinal) == -1 ? "?" : "&amp;";

            imgSrc = string.Concat(imgSrc, andOrQuestion, "Gaia.WebWidgets.DynamicImage.GetImage=",
                System.Web.HttpUtility.UrlEncode(ClientID), "&amp;ImageId=", System.Web.HttpUtility.UrlEncode(ImageId));

            return imgSrc;
        }

        #endregion

        #region [ -- Overridden IAjaxControl Methods -- ]

        string IAjaxControl.GetScript()
        {
            return new RegisterControl("Gaia.DynamicImage", ClientID).AddAspects(Aspects).AddEffects(Effects).ToString();
        }

        #endregion
    }
}
