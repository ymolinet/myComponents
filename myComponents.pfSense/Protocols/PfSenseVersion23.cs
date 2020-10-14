using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace myComponents.pfSense.Protocols
{
    /// <summary>
    /// Implementation of the pfSense protocol for version 2.3
    /// </summary>
    public class PfSenseVersion23 : IPfSenseProtocol
    {
        /// <summary>
        /// Connects with the specified pfSense server using the v2.3 protocol implementation and returns the backup file contents
        /// </summary>
        /// <param name="pfSenseServer">pfSense server details which identifies which pfSense server to connect to</param>
        /// <param name="cookieJar">Cookie container to use through the communication with pfSense</param>
        /// <param name="timeout">Timeout in milliseconds on how long requests to pfSense may take. Default = 60000 = 60 seconds.</param>
        /// <returns>PfSenseBackupFile instance containing the retrieved backup content from pfSense</returns>
        public string Execute(pfSense pfSenseServer, CookieContainer cookieJar)
        {
            // Create a session on the pfSense webserver
            var loginPageContents = HttpUtility.HttpGetLoginPageContents(pfSenseServer.ServerBaseUrl, cookieJar, pfSenseServer.timeout);

            // Check if a response was returned from the login page request
            if (string.IsNullOrEmpty(loginPageContents))
            {
                throw new ApplicationException("Unable to retrieve login page contents");
            }

            // Use a regular expression to fetch the anti cross site scriping token from the HTML
            var xssToken = Regex.Match(loginPageContents, "<input.+?type=['\"]hidden['\"].+?name=['\"]_+?csrf_magic['\"] value=['\"](?<xsstoken>.*?)['\"].+?/>", RegexOptions.IgnoreCase);

            // Verify that the anti XSS token was found
            if (!xssToken.Success)
            {
                xssToken = Regex.Match(loginPageContents, "var.*?csrfMagicToken.*?=.*?\"(?<xsstoken>.*?)\"");
            }

            // Authenticate the session
            var authenticationResult = HttpUtility.AuthenticateViaUrlEncodedFormMethod(string.Concat(pfSenseServer.ServerBaseUrl, "index.php"),
                                                                                       new Dictionary<string, string>
                                                                                       {
                                                                                            {"__csrf_magic", xssToken.Groups["xsstoken"].Value },
                                                                                            { "usernamefld", System.Web.HttpUtility.UrlEncode(pfSenseServer.username) },
                                                                                            { "passwordfld", System.Web.HttpUtility.UrlEncode(pfSenseServer.password) },
                                                                                            { "login", "Login" }
                                                                                       },
                                                                                       cookieJar,
                                                                                       pfSenseServer.timeout);

            // Verify if the username/password combination was valid by examining the server response
            if (authenticationResult.Contains("Username or Password incorrect"))
            {
                throw new ApplicationException("ERROR: Credentials incorrect");
            }

            // Get the backup page contents for the xsrf token
            var backupPageUrl = string.Concat(pfSenseServer.ServerBaseUrl, "diag_backup.php");

            var backupPageContents = HttpUtility.HttpGetLoginPageContents(backupPageUrl, cookieJar, pfSenseServer.timeout);

            // Check if a response was returned from the login page request
            if (string.IsNullOrEmpty(backupPageContents))
            {
                throw new ApplicationException("Unable to retrieve backup page contents");
            }

            // Use a regular expression to fetch the anti cross site scriping token from the HTML
            xssToken = Regex.Match(backupPageContents, "<input.+?type=['\"]hidden['\"].+?name=['\"]_+?csrf_magic['\"] value=['\"](?<xsstoken>.*?)['\"].+?/>", RegexOptions.IgnoreCase);

            // Verify that the anti XSS token was found
            if (!xssToken.Success)
            {
                xssToken = Regex.Match(backupPageContents, "var.*?csrfMagicToken.*?=.*?\"(?<xsstoken>.*?);.*?\"");
            }

            var downloadArgs = new Dictionary<string, string>
                {
                    {"__csrf_magic", xssToken.Groups["xsstoken"].Value },
                    { "backuparea", "" },
                    { "nopackages", pfSenseServer.with_package_information ? "" : "yes" },
                    { "donotbackuprrd", pfSenseServer.with_rrd ? "" : "yes" },
                    { "encrypt", pfSenseServer.encrypt_backup ? "yes" : "" },
                    { "encrypt_password", pfSenseServer.encrypt_password },
                    { "Submit", "Download configuration as XML" },
                    { "restorearea", "" },
                    { "decrypt_password", "" }
                };

            return HttpUtility.DownloadBackupFile(backupPageUrl,
                                                                downloadArgs,
                                                                cookieJar,
                                                                pfSenseServer.timeout,
                                                                backupPageUrl);
        }
    }
}
