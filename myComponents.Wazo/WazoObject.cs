using Newtonsoft.Json;
using System;
using System.Text;

namespace myComponents.Wazo
{
    public class AuthenticationCredentials
    {
        public string username { get; set; }
        public string password { get; set; }

        public string authenticationString { get { return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")); } }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AuthenticationRequest
    {
        public string access_type { get; set; }
        public string backend { get; set; }
        public int expiration { get; set; }

        public string client_id { get; set; }

        public string refresh_token { get; set; }
    }


    public class AuthenticationResponse
    {
        public AuthenticationData data { get; set; }
    }

    public class AuthenticationData
    {
        public string[] acl { get; set; }
        public string[] acls { get; set; }
        public string auth_id { get; set; }
        public string expires_at { get; set; }
        public string issued_at { get; set; }
        // public string metadata { get; set; }
        public string session_uuid { get; set; }
        public string token { get; set; }
        public string utc_expires_at { get; set; }
        public string utc_issued_at { get; set; }
        public string xivo_user_uuid { get; set; }
        public string xivo_uuid { get; set; }
    }

    public class LicenceDetail
    {
        public Int32 total { get; set; }
    }

    public class SummaryAccountDetail
    {
        [JsonProperty("subscriptions")]
        public Subscriptions Subscriptions { get; set; }
    }
    public class Subscriptions
    {
        [JsonProperty("0")]
        public LicenceDetail Voice { get; set; }
        [JsonProperty("1")]
        public LicenceDetail UnifiedCommunication { get; set; }
        [JsonProperty("2")]
        public LicenceDetail Collaboration { get; set; }
        [JsonProperty("3")]
        public LicenceDetail CustomerRelationShip { get; set; }
    }

    public class AccountsListReponse
    {
        public Account[] items { get; set; }
        public int filtered { get; set; }
        public int total { get; set; }
    }
    public class Account
    {
        public string created_at { get; set; }
        public string modified_at { get; set; }
        public int subscription { get; set; }
        public string tenant_uuid { get; set; }
        public string uuid { get; set; }

        public customer customer { get; set; }
    }

    public class customer
    {
        public string name { get; set; }
        public string uuid { get; set; }
    }
}
