namespace Gaia.WebWidgets.Samples.Core.XhtmlTagFactory.Overview
{
    using System;
    using HtmlFormatting;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void btn_OnClick(object sender, EventArgs e)
        {
            const string url = "http://gaiaware.net/";
            lbl.Text = ComposeXhtml.ToString(
                delegate(XhtmlTagFactory create)
                {
                    using (create.B())
                    {
                        using (Tag a = create.A("xhtmlLink", string.Empty, url))
                        {
                            a.WriteContent("Gaiaware Home Page");
                        }
                    }
                });
        }
        #endregion
    }
}