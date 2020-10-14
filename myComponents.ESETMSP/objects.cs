using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace myComponents.ESETMSP2
{
    public class AuthenticationData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class CompanyUserDetailsResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }

    public class UserDetailsReponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int Status { get; set; }
        public CompanyUserDetailsResponse Company { get; set; }
    }

    public class UsageReportRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class UsageReportProductResponse
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int LicenseType { get; set; }
        public long SeatDays { get; set; }
    }

    public class UsageReportCompanyResponse
    {
        public UsageReportProductResponse[] Products { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public long TotalUsage { get; set; }
    }

    public class UsageReportResponse
    {
        public UsageReportCompanyResponse[] Companies { get; set; }
        public paging paging { get; set; }
    }

    public class paging
    {
        public int totalCount { get; set; }
    }

    public class Keyword
    {
        public string keyword { get; set; }
        public int[] column { get; set; }
    }
    public class SearchRequest
    {
        public ListDictionary search {get;set;}
        public int skip { get; set; }
        public int take { get; set; }
    }
    public class SearchCompaniesResponse
    {
        public SearchCompanyResponse[] Search { get; set; }
    }

    public class SearchCompanyResponse
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string CompanyId { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }

    public class SelectCompaniesRequest
    {
        public string[] companyIds { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public int skip { get; set; }
        public int take { get; set; }
    }

    public class ProductsResponse
    {
        public string code { get; set; }
        public string name { get; set; }
        public string licenseType { get; set; }
        public int seatDays { get; set; }
        public int seats { get; set; }
    }

    public class SelectedCompanyReportResponse
    {
        public List<ProductsResponse> products { get; set; }
        public string companyId { get; set; }
        public string companyName { get; set; }
        public int totalUsage { get; set; }
        public int totalSeats { get; set; }
    }

    public class SelectedCompaniesReportResponse
    {
        public SelectedCompanyReportResponse[] companies { get; set; }
        public paging paging { get; set; }
    }

}
