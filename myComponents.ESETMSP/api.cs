﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace myComponents.ESETMSP2
{
    // https://help.eset.com/ema/2/api/en-US/
    public class EsetHelper
    {
        private static HttpClient client = new HttpClient();
        private static string BaseURL = "https://mspapi.eset.com/";
        private AuthenticationData _auth;
        public EsetHelper(string Username, string Password)
        {
            _auth = new AuthenticationData() { Username = Username, Password = Password };
            client.BaseAddress = new Uri(BaseURL);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AuthenticationResponse authenticationResponse = Login();
            if (authenticationResponse != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResponse.AccessToken);
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }

        private AuthenticationResponse Login()
        {
            HttpResponseMessage response = client.PostAsJsonAsync("/api/Token/Get", _auth).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<AuthenticationResponse>().Result;
            else
                return null;
        }

        // https://help.eset.com/ema/2/api/en-US/billing_divisions.html?allcompanies_products.html

        public UserDetailsReponse GetCurrentUserDetails()
        {
            var response = client.GetAsync("/api/User/Current").Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<UserDetailsReponse>().Result;
            else
                return null;
        }

        public UsageReportResponse GetUsagePagingReport(DateTime from, DateTime to, int skip = 0, int take = 100)
        {
            string strFrom = from.ToString("yyyy-MM-dd");
            string strTo = to.ToString("yyyy-MM-dd");

            var usagereportRequest = new UsageReportRequest { From = strFrom, To = strTo, Skip = skip, Take = take };
            var response = client.PostAsJsonAsync("/api/UsageReport/AllCompanies/products", usagereportRequest).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<UsageReportResponse>().Result;
            else return null;
        }

        public SearchCompanyResponse SearchCompanyByName(string value)
        {
            SearchRequest searchRequest = new SearchRequest()
            {
                take = 1,
                skip = 0,
                search = new ListDictionary()
            };
            searchRequest.search.Add(value, new int[] { 0 });

            var response = client.PostAsJsonAsync("/api/Search/Companies", searchRequest).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<SearchCompaniesResponse>().Result;
                if (result.Search.Count() == 1)
                    return result.Search[0];
                else return null;
            }
            else return null;
        }

        public SelectedCompaniesReportResponse GetSelectedCompaniesProductsPagingReport(string[] CompanyIds, DateTime from, DateTime to, int skip = 0, int take = 100)
        {
            string strFrom = from.ToString("yyyy-MM-dd");
            string strTo = to.ToString("yyyy-MM-dd");

            var request = new SelectCompaniesRequest { companyIds = CompanyIds, from = strFrom, to = strTo, skip = skip, take = take };
            var response = client.PostAsJsonAsync("/api/UsageReport/SelectedCompanies/products", request).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<SelectedCompaniesReportResponse>().Result;
            else return null;
        }

        public List<ProductsResponse> GetCompanyProductsReport(string CompanyId, DateTime from, DateTime to)
        {
            List<ProductsResponse> globalReport = new List<ProductsResponse>();
            int take = 20;

            SelectedCompaniesReportResponse report = GetSelectedCompaniesProductsPagingReport(new string[] { CompanyId }, from, to, 0, take);

            if (report != null)
            {
                globalReport.AddRange(report.companies[0].products);

                if (report.paging.totalCount > 1)
                {
                    for (int x = 1; x <= report.paging.totalCount; x++)
                    {
                        report = GetSelectedCompaniesProductsPagingReport(new string[] { CompanyId }, from, to, take * x, take);
                        if (report != null)
                            globalReport.AddRange(report.companies[0].products);
                    }
                }
            }
            return globalReport;
        }


    }
}