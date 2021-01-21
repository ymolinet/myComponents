using Newtonsoft.Json;
using System;

namespace myComponents.Wazo
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

    public class LicenceDetail
    {
        public Int32 total { get; set; }
    }
    public class SummaryAccountDetail
    {
        [JsonProperty("1")]
        public LicenceDetail Voice { get; set; }
        [JsonProperty("2")]
        public LicenceDetail UnifiedCommunication { get; set; }
        [JsonProperty("3")]
        public LicenceDetail Collaboration { get; set; }
        [JsonProperty("4")]
        public LicenceDetail CustomerRelationShip { get; set; }
    }
}
