using System.Web.UI;

namespace Gaia.WebWidgets
{
    internal class ImageMapHtmlWriter : HtmlTextWriter
    {
        public ImageMapHtmlWriter(System.IO.TextWriter writer)
            : base(writer)
        { }

        public override void AddAttribute(HtmlTextWriterAttribute key, string value)
        {
            if (key == HtmlTextWriterAttribute.Href && value.Contains("__doPostBack"))
                base.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
            else
                base.AddAttribute(key, value);
        }
    }
}
