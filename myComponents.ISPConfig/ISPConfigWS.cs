using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace myComponents.ISPConfig
{
    public class ISPConfigWS : WebService
    {
        public XElement XmlResult;
        public string StringResult;
        public bool RequestSuccess;
        public Exception LastException;

        public ISPConfigWS(string ISPConfigURL) : base(ISPConfigURL) { }

        public new void Invoke(string methodName, Dictionary<string, string> parameters)
        {
            try
            {
                base.Invoke(methodName, parameters);
                ExtractResult(methodName);
                RequestSuccess = true;
            }
            catch (Exception ex)
            {
                LastException = ex;
                RequestSuccess = false;
            }
        }

        private new void CleanLastInvoke()
        {
            base.CleanLastInvoke();
            XmlResult = null;
            StringResult = String.Empty;
        }

        private void ExtractResult(string methodName)
        {
            // Selects just the elements with namespace http://tempuri.org/ (i.e. ignores SOAP namespace)
            XmlNamespaceManager namespMan = new XmlNamespaceManager(new NameTable());
            namespMan.AddNamespace("ns", "http://schemas.xmlsoap.org/soap/envelope/");
            namespMan.AddNamespace("ns1", "/remote/index.php");

            XElement webMethodResult = SOAPResponse.XPathSelectElement("//ns1:" + methodName + "Response", namespMan);
            // If the result is an XML, return it and convert it to string
            if (webMethodResult.FirstNode.NodeType == XmlNodeType.Element)
            {
                XmlResult = webMethodResult.FirstNode as XElement;
                StringResult = XmlResult.Value;

                //if (XmlResult.HasElements)
                //{
                //    // https://stackoverflow.com/questions/64077278/deserialize-key-value-xml-data-to-c-sharp-object
                //    //XmlSerializer xmlSerializer = new XmlSerializer(typeof(ISPConfigItems));
                //    //DataResult = (ISPConfigItems)xmlSerializer.Deserialize(XmlResult.CreateReader());
                //    //DataResult.Items.AddRange((from element in XmlResult.Descendants("return").Elements("item")
                //    //              select new
                //    //              {
                //    //                  key = element.Elements("key").First().Value,
                //    //                  value = element.Elements("value").First().Value,
                //    //              }).ToList());

                //    DataResult = new ISPConfigItems();
                //    foreach (XElement element in XmlResult.Elements())
                //    {
                //        ISPConfigItem anItem = new ISPConfigItem()
                //        {
                //            Key = element.Elements("key").First().Value,
                //            Value = element.Elements("value").First().Value,
                //        };
                //        DataResult.Items.Add(anItem);
                //    }
                // }
            }
            // If the result is a string, return it and convert it to XML (creating a root node to wrap the result)
            else
            {
                StringResult = webMethodResult.FirstNode.ToString();
                XmlResult = null;
            }
        }


    }
}
