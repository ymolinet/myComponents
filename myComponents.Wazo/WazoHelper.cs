using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace myComponents.Wazo
{
    public class WazoHelper
    {
        private HttpClient _client;
        private AuthenticationData _auth;
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
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public bool Login(string Username, string Password)
        {
            _auth = new AuthenticationData() { username = Username, password = Password };
            HttpResponseMessage response = _client.PostAsJsonAsync("/api/auth/0.1/token", _auth).Result;
            if (response.IsSuccessStatusCode)
            {
                AuthenticationResponse authenticationResponse = response.Content.ReadAsAsync<AuthenticationResponse>().Result;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-Auth-Token", authenticationResponse.token);
                return true;
            } else
                return false;
        }

        public SummaryAccountDetail GetSummaryAccount(string tenant_uuid)
        {
            var response = _client.PostAsJsonAsync("/api/accounts/summary", tenant_uuid).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<SummaryAccountDetail>().Result;
            else
                return null;
        }
    }
}