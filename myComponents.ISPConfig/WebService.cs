using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace myComponents.ISPConfig
{
    /// <summary>
    /// From : https://www.diogonunes.com/blog/calling-a-web-method-in-c-without-a-service-reference/
    /// This class is an alternative when you can't use Service References. It allows you to invoke Web Methods on a given Web Service URL.
    /// Based on the code from http://stackoverflow.com/questions/9482773/web-service-without-adding-a-reference
    /// </summary>
    public class SOAPWebService
    {
        public string Url { get; private set; }
        public XDocument SOAPResponse;

        private Cursor InitialCursorState;

        public SOAPWebService()
        {
            Url = String.Empty;
        }
        public SOAPWebService(string baseUrl)
        {
            Url = baseUrl;
        }
        // Public API

        /// <summary>
        /// Using the base url, invokes the WebMethod with the given name
        /// </summary>
        /// <param name="methodName">Web Method name</param>
        public void Invoke(string methodName, ListDictionary parameters)
        {
            PreInvoke();
            Invoke(methodName, parameters, true);
            PosInvoke();
        }

        /// <summary>
        /// Cleans all internal data used in the last invocation, except the WebService's URL.
        /// This avoids creating a new WebService object when the URL you want to use is the same.
        /// </summary>
        public void CleanLastInvoke()
        {
            SOAPResponse = null;
        }

        #region Helper Methods

        /// <summary>
        /// Checks if the WebService's URL and the WebMethod's name are valid. If not, throws ArgumentNullException.
        /// </summary>
        /// <param name="methodName">Web Method name (optional)</param>
        private void AssertCanInvoke(string methodName = "")
        {
            if (Url == String.Empty)
                throw new ArgumentNullException("You tried to invoke a webservice without specifying the WebService's URL.");
        }

        /// <summary>
        /// Invokes a Web Method, with its parameters encoded or not.
        /// </summary>
        /// <param name="methodName">Name of the web method you want to call (case sensitive)</param>
        /// <param name="encode">Do you want to encode your parameters? (default: true)</param>
        private void Invoke(string methodName, ListDictionary parameters, bool encode)
        {
            AssertCanInvoke(methodName);
            string soapStr =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                   xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                   xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <soap:Body>
                    <{0} xmlns=""http://tempuri.org/"">
                      {1}
                    </{0}>
                  </soap:Body>
                </soap:Envelope>";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Headers.Add("SOAPAction", "\"http://tempuri.org/" + methodName + "\"");
            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.Method = "POST";

            using (Stream stm = req.GetRequestStream())
            {
                string methodParameters = FormatParameters(parameters, encode);

                soapStr = string.Format(soapStr, methodName, methodParameters);
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(soapStr);
                }
            }

            try
            {
                using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    string result = responseReader.ReadToEnd();
                    SOAPResponse = XDocument.Parse(result);
                }
            }
            catch (WebException we)
            {
                SOAPResponse = XDocument.Parse("<error>" + we.Message + "</error>");
            }
        }

        internal string FormatParameters(ListDictionary parameters, bool encode)
        {
            string methodParameters = "";
            foreach (var param in parameters)
            {
                DictionaryEntry entry = (DictionaryEntry)param;

                String valueTypeName = entry.Value.GetType().FullName;
                if (entry.Value.GetType().FullName == "System.String")
                {
                    if (encode) methodParameters += string.Format("<{0}>{1}</{0}>", HttpUtility.HtmlEncode(entry.Key), HttpUtility.HtmlEncode(entry.Value));
                    else methodParameters += string.Format("<{0}>{1}</{0}>", entry.Key, entry.Value);
                } 
                if (entry.Value.GetType().FullName == "System.Collections.Specialized.ListDictionary")
                {
                    string subParameters = FormatParameters(entry.Value as ListDictionary, encode);
                    if (encode) methodParameters += string.Format("<{0}>{1}</{0}>", HttpUtility.HtmlEncode(entry.Key), subParameters);
                    else methodParameters += string.Format("<{0}>{1}</{0}>", entry.Key, subParameters);
                }
            }
            return methodParameters;
        }

        /// <summary>
        /// This method should be called before each Invoke().
        /// </summary>
        internal void PreInvoke()
        {
            CleanLastInvoke();
            InitialCursorState = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            // feel free to add more instructions to this method
        }

        /// <summary>
        /// This method should be called after each (successful or unsuccessful) Invoke().
        /// </summary>
        internal void PosInvoke()
        {
            Cursor.Current = InitialCursorState;
            // feel free to add more instructions to this method
        }

        #endregion
    }
}
