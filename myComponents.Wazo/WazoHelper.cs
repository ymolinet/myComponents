using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace myComponents.Wazo
{
    public class WazoHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private HttpClient _client;
        private AuthenticationCredentials _auth;
        public WazoHelper(string BaseURL)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(BaseURL);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //_client.DefaultRequestHeaders.Add("Keep-Alive", "false");
            _client.Timeout = TimeSpan.FromMinutes(20);
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public bool Login(string Username, string Password)
        {
            _auth = new AuthenticationCredentials() { username = Username, password = Password };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _auth.authenticationString);
            AuthenticationRequest _req = new AuthenticationRequest() { expiration = 900 };
 
            HttpResponseMessage response = _client.PostAsJsonAsync("/api/auth/0.1/token", _req).Result;
            
            if (response.IsSuccessStatusCode)
            {
                string message = response.Content.ReadAsStringAsync().Result;
                logger.Debug(message);
                AuthenticationResponse authenticationResponse = response.Content.ReadAsAsync<AuthenticationResponse>().Result;
                _client.DefaultRequestHeaders.Add("X-Auth-Token", authenticationResponse.data.token);
                return true;
            }
            else
            {
                string message = response.Content.ReadAsStringAsync().Result;
                logger.Error(message);
                return false;
            }
        }

        /// <summary>
        /// Ref API : /api/confd/1.0/accounts/summary
        /// </summary>
        /// <param name="tenant_uuid">Pass in web header</param>
        /// <returns></returns>
        public SummaryAccountDetail GetSummaryAccount(string tenant_uuid)
        {
            // var response = _client.GetAsync("/api/confd/1.0/accounts/summary").Result;
            // _client.DefaultRequestHeaders.Remove("Wazo-Tenant");
            HttpRequestMessage httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_client.BaseAddress + "/api/confd/1.0/accounts/summary")
            };
            httpRequest.Headers.Add("Wazo-Tenant", tenant_uuid);
            var response = _client.SendAsync(httpRequest).Result;
            if (response.IsSuccessStatusCode)
            {
                string message = response.Content.ReadAsStringAsync().Result;
                logger.Debug(message);
                return response.Content.ReadAsAsync<SummaryAccountDetail>().Result;
            }
            else
            {
                string message = response.Content.ReadAsStringAsync().Result;
                logger.Error(message);
                return null;
            }
        }

        /// <summary>
        ///  Ref API : /api/confd/1.0/accounts
        /// </summary>
        /// <returns></returns>
        public AccountsListReponse GetAccounts()
        {
            var response = _client.GetAsync("/api/confd/1.0/accounts").Result;
            if (response.IsSuccessStatusCode)
            {
                string message = response.Content.ReadAsStringAsync().Result;
                logger.Debug(message);
                return response.Content.ReadAsAsync<AccountsListReponse>().Result; ;
            }
            else
            {
                string message = response.Content.ReadAsStringAsync().Result;
                logger.Error(message);
                return null;
            }
        }
    }
}