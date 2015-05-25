namespace Gaia.WebWidgets.Samples.UI
{
    using Utilities;
    using HtmlFormatting;

    public class GDoc : GaiaControl
    {
        private bool _includeTitle = true;

        public string Member { get; set; }

        public bool IncludeTitle
        {
            get { return _includeTitle; }
            set { _includeTitle = value; }
        }

        public static string GetLastTypeName(string member)
        {
            int idx = member.LastIndexOf(".") + 1;
            return member.Substring(idx, member.Length - idx);
        }

        protected override void RenderControlHtml(XhtmlTagFactory create)
        {
            if (IncludeTitle)
                using (create.H1().WriteContent("Gaia Ajax: " + GetLastTypeName(Member))) { }

            using (Tag paragraph = create.P())
            {
                paragraph.WriteContent(string.Empty);
                string summary = XmlCodeDocumentation.GetSummary(Member);
                
                create.GetHtmlTextWriter().Write(summary);
            }
            
        }
    }
}