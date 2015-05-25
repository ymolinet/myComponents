namespace Gaia.WebWidgets.Samples.UI
{
    using System;
    using System.Web.UI;
    using System.IO;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class CodeViewerControl : UserControl
    {
        public string CodeFile { get; set; }

        public string Format { get; set; }

        public string CodeFileName
        {
            set
            {
                zWindow.Caption = value;
                zExpandCode.Text = value;
            }
            get
            {
                return zWindow.Caption;
            }
        }
       
        protected void zExpandCode_Click(object sender, EventArgs e)
        {
            // Read the code, convert it, add it to the page
            string content = File.ReadAllText(Server.MapPath(CodeFile));

            CodeVisualizationUtils.BuildLiteralDelegate builder;
            switch (Format.ToLowerInvariant())
            {
                case "vb":  builder = CodeVisualizationUtils.BuildVbCodeLiteral; break;
                case "aspx": builder = CodeVisualizationUtils.BuildAspxLiteral; break;
                default: builder = CodeVisualizationUtils.BuildCSharpCodeLiteral; break;
            }
            zCode.Controls.Add(builder(content));
            
            zWindow.Visible = true;
            zCode.Visible = true;
        }
    }
}