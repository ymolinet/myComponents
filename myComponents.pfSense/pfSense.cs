using myComponents.pfSense.Protocols;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace myComponents.pfSense
{
    public class pfSense
    {
        private string _strIPValidation = @"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){4}";
        private Regex _checkIPAddressv4;

        public pfSense()
        {
            this._checkIPAddressv4 = new Regex(_strIPValidation);
        }

        private string _address;
        public string Address {
            get { return _address; }
            set {
                if (_checkIPAddressv4.IsMatch(value))
                    _address = value;
                else throw new Exception("IPv4 address not valid");
            }
        }

        public bool use_ssl { get; set; }
        public int port { get; set; }
        public int timeout { get; set; }
        public string version { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool with_rrd { get; set; }
        public bool with_package_information { get; set; }
        public bool encrypt_backup { get; set; }
        public string encrypt_password { get; set; }

        public string ServerBaseUrl
        {
            get
            {
                if (port == 0)
                    port = use_ssl ? 443 : 80;

                return string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/", use_ssl ? "https" : "http", Address, port);
            }
        }

        public string Backup()
        {
            if (string.IsNullOrEmpty(password))
                throw new ApplicationException("Password could not be empty");

            if (string.IsNullOrEmpty(username))
                throw new ApplicationException("Username could not be empty");

            // Ignore all certificate related errors
            if (use_ssl)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            }

            // Create a cookie container to hold the session cookies
            var cookieJar = new CookieContainer();

            // Define the protocol implementation to use to communicate with pfSense
            IPfSenseProtocol pfSenseProtocol = null;
            switch (this.version)
            {
                case "1.2":
                    pfSenseProtocol = new PfSenseVersion12();
                    break;

                case "2.0":
                    pfSenseProtocol = new PfSenseVersion20();
                    break;

                case "2.1":
                case "2.2":
                    pfSenseProtocol = new PfSenseVersion21();
                    break;

                case "2.3":
                case "2.3.1":
                case "2.3.2":
                    pfSenseProtocol = new PfSenseVersion23();
                    break;

                case "2.3.3":
                case "2.3.4":
                case "2.4.0":
                case "2.4.1":
                case "2.4.2":
                case "2.4.3":
                case "2.4.4":
                case "2.4.5":
                    pfSenseProtocol = new PfSenseVersion233();
                    break;

                default:
                    throw new Exception("Version de pfSense non supportée: " + version);
            }

            string result = pfSenseProtocol.Execute(this, cookieJar);

            return result;
        }


    }
}
