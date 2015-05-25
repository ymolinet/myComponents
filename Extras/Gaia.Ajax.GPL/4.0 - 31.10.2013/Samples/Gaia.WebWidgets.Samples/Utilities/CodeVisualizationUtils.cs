namespace Gaia.WebWidgets.Samples.Utilities
{
    using System.IO;
    using System.Web.UI.WebControls;
    using ICSharpCode.NRefactory;
    using ICSharpCode.NRefactory.Ast;
    using ICSharpCode.NRefactory.PrettyPrinter;
    using Manoli.Utils.CSharpFormat;

    public static class CodeVisualizationUtils
    {
        static Literal BuildLiteral(string content)
        {
            Literal lit = new Literal();
            lit.Text = content;
            return lit;
        }

        public static Literal BuildCSharpCodeLiteral(string code)
        {
            return BuildCodeLiteral(new CSharpFormat(), "Codebehind (C#/.cs)", code, "c#");
        }

        public static Literal BuildVbCodeLiteral(string code)
        {
            return BuildCodeLiteral(new VisualBasicFormat(), "Codebehind (VB.NET/.vb)", code, "c#");
        }

        public static Literal BuildAspxLiteral(string code)
        {
            return BuildCodeLiteral(new HtmlFormat(), "Markup (ASPX/.aspx)", code, "xml");
        }

        public static Literal BuildCodeLiteral(SourceFormat format, string title, string code, string cssClass)
        {
            string codeBehind = Colorize(format, code);
            return BuildLiteral("<h3>" + title + "</h3><div class=\"" + cssClass + "\">" + codeBehind + "</div><br />");
        }

        public delegate Literal BuildLiteralDelegate(string code);


        public static string ConvertCSharpToVisualBasic(string csharpCode)
        {
            StringReader reader = new StringReader(csharpCode);

            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, reader);
            parser.Parse();

            if (parser.Errors.Count > 0) // print errors
                return parser.Errors.ErrorOutput;

            CompilationUnit cu = parser.CompilationUnit;
            IOutputAstVisitor output = new VBNetOutputVisitor();
            cu.AcceptVisitor(output, null);
            return output.Text;
        }

        public static string Colorize(SourceFormat formatter, string source)
        {
            formatter.Alternate = true;
            formatter.LineNumbers = true;
            return formatter.FormatCode(source);
        }

        public static string ParseCodeRegionFromCodebehind(string csharp, string region)
        {
            string regionString = "#region " + region;
            const string regionEndString = "#endregion";
            int startIndex = csharp.IndexOf(regionString);
            if (startIndex == -1)
                return csharp;

            int endIndex = csharp.IndexOf(regionEndString);
            if (endIndex == -1)
                return csharp;

            startIndex += regionString.Length;
            endIndex -= regionEndString.Length;

            return csharp.Substring(startIndex, endIndex - startIndex);
        }
    }
}