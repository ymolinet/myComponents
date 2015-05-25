namespace Gaia.WebWidgets.Samples.Utilities
{
    using System.Xml;

    public class XmlUtility
    {
        /// <summary>
        /// Returns an 0-based index of where the nodename is in the list
        /// </summary>
        /// <param name="nodeName">Name to find index for</param>
        /// <param name="xmlNode">Node to search in</param>
        /// <returns></returns>
        public static int GetIndexOfXmlNode(string nodeName, XmlNode xmlNode)
        {
            if (xmlNode != null)
                foreach (XmlNode node in xmlNode.SelectNodes("//Directories/Directory"))
                {
                    if (node.Attributes["Name"].Value == nodeName)
                        return node.SelectNodes("preceding-sibling::*").Count;

                }
            return -1;
        }

        public static string GetAttributeValue(XmlNode currentXmlNode, string attributeName)
        {
            if (currentXmlNode != null && currentXmlNode.Attributes != null)
            {
                XmlAttribute urlAttribute = currentXmlNode.Attributes[attributeName];
                if (urlAttribute != null && !string.IsNullOrEmpty(urlAttribute.Value))
                    return urlAttribute.Value;
            }
            return "";
        }
    }
}