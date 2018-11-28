using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace myComponents.OpenERP7
{
    public class OpenERP7Server
    {
        private OpenERP7Credentials _cred;
        private WebProxy _proxy;
        private bool _sslvalidation;
        private IOpenERP7Object _objectRpc;

        public OpenERP7Server(OpenERP7Credentials credentials, WebProxy proxy = null, bool sslvalidation = false, bool immediatelogin = true)
        {
            _cred = credentials;
            _proxy = proxy;
            _sslvalidation = sslvalidation;
            if (immediatelogin) Connect();
        }

        // https://stackoverflow.com/questions/25628216/fault-response-contains-a-string-value-where-an-integer-was-expected
        public bool Connect()
        {
            IOpenERP7Login loginRpc = XmlRpcProxyGen.Create<IOpenERP7Login>();
            loginRpc.Url = _cred.CommonUrl;
            loginRpc.NonStandard = XmlRpcNonStandard.AllowStringFaultCode;
            if (_proxy != null) loginRpc.Proxy = _proxy;
            if (_sslvalidation) ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            _cred.UserId = loginRpc.login(_cred.DbName, _cred.DbUser, _cred.DbPassword);
            _objectRpc = XmlRpcProxyGen.Create<IOpenERP7Object>();
            _objectRpc.Url = _cred.ObjectUrl;
            _objectRpc.NonStandard = XmlRpcNonStandard.AllowStringFaultCode;
            return isConnected;
        }

        public bool isConnected { get { return _cred.UserId != -1; } }

        public void Disconnect()
        {
            _cred.UserId = -1;
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // Implement Server Certificate validation logic here, if needed.
            return true;
        }

        public int[] Search(string model, object[] filters)
        {
            if (!isConnected) throw new Exception("Not connected");
            return _objectRpc.search(_cred.DbName, _cred.UserId, _cred.DbPassword, model, "search", filters);
        }

        public object[] Read(string model, int[] ids)
        {
            return this.Read(model, ids, null);
        }

        public object[] Read(string model, int[] ids, object[] fields)
        {
            if (!isConnected) throw new Exception("Not connected");
            if (fields == null)
                return _objectRpc.read(_cred.DbName, _cred.UserId, _cred.DbPassword, model, "read", ids);
            else return _objectRpc.read(_cred.DbName, _cred.UserId, _cred.DbPassword, model, "read", ids, fields);
        }

        public bool Write(string model, int[] ids, XmlRpcStruct fieldValues)
        {
            if (!isConnected) throw new Exception("Not connected");
            return _objectRpc.write(_cred.DbName, _cred.UserId, _cred.DbPassword, model, "write", ids, fieldValues);
        }

        public bool Remove(string model, int[] ids)
        {
            if (!isConnected) throw new Exception("Not connected");
            return _objectRpc.unlink(_cred.DbName, _cred.UserId, _cred.DbPassword, model, "unlink", ids);
        }

        public bool Execute_Workflow(string model, string action, int id)
        {
            if (!isConnected) throw new Exception("Not connected");
            return _objectRpc.exec_workflow(_cred.DbName, _cred.UserId, _cred.DbPassword, model, action, id);
        }
    }
}
