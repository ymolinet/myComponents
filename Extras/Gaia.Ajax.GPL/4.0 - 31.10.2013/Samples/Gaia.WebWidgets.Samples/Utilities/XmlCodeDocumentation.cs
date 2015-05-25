using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Gaia.WebWidgets.Samples.Utilities
{
    /// <summary>
    /// The XmlCodeDocumentation loads the xml file with documentation 
    /// </summary>
    public class XmlCodeDocumentation
    {
        private static Dictionary<string, string> _summaryCache;
        private static readonly List<string> LoadedDocuments = new List<string>();

        public static Dictionary<string, string> SummaryCache
        {
            get { return _summaryCache ?? (_summaryCache = new Dictionary<string, string>()); }
        }

        public static void EnsureDocumentLoaded(Assembly assembly)
        {
            if (LoadedDocuments.Contains(assembly.FullName))
                return;

            var path = assembly.CodeBase.Replace("file:///", "");
            var docPath = Path.ChangeExtension(path, ".xml");

            if (File.Exists(docPath))
            {
                CacheXmlFile(docPath);
            }

            LoadedDocuments.Add(assembly.FullName);
        }


        public static string GetSummary(Type type)
        {
            return SummaryCache["T:" + type.FullName];
        }

        public static string GetSummary(string type)
        {
            return SummaryCache.ContainsKey(type) ? SummaryCache[type] : string.Empty;
        }

        static void CacheXmlFile(string file)
        {
            var document = new XPathDocument(file);
            var navigator = document.CreateNavigator();

            navigator.MoveToRoot();
            var members = navigator.SelectDescendants("member", string.Empty, false);
            while (members.MoveNext())
            {
                var member = members.Current;
                if (member == null) continue;

                var memberName = member.GetAttribute("name", string.Empty);
                if (string.IsNullOrEmpty(memberName) || SummaryCache.ContainsKey(memberName)) continue;

                var summaryNode = member.SelectSingleNode("summary");
                if (summaryNode == null) continue;

                SummaryCache.Add(memberName, ProcessSummary(summaryNode.CreateNavigator()));
            }
        }

        private static string ProcessSummary(XPathNavigator summaryNavigator)
        {
            var builder = new StringBuilder();

            if (summaryNavigator.HasChildren && summaryNavigator.MoveToFirstChild())
            {
                do
                {
                    string text;
                    switch (summaryNavigator.LocalName)
                    {
                        case "see":
                            text = GetLastTypeName(summaryNavigator.GetAttribute("cref", string.Empty));
                            break;
                        case "a":
                        case "pre":
                            text = summaryNavigator.OuterXml;
                            break;
                        default:
                            text = summaryNavigator.Value;
                            break;
                    }

                    builder.Append(text);
                } while (summaryNavigator.MoveToNext());
            }

            return builder.ToString();
        }

        private static string GetLastTypeName(string member)
        {
            var idx = member.LastIndexOf(".") + 1;
            return member.Substring(idx, member.Length - idx);
        }
    }
}
