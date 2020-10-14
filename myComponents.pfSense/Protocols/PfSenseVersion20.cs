using System;
using System.Collections.Generic;
using System.Net;

namespace myComponents.pfSense.Protocols
{
    /// <summary>
    /// Implementation of the pfSense protocol for version 2.0
    /// </summary>
    public class PfSenseVersion20 : IPfSenseProtocol
    {
        /// <summary>
        /// Connects with the specified pfSense server using the v2.0 protocol implementation and returns the backup file contents
        /// </summary>
        /// <param name="pfSenseServer">pfSense server details which identifies which pfSense server to connect to</param>
        /// <param name="cookieJar">Cookie container to use through the communication with pfSense</param>
        /// <param name="timeout">Timeout in milliseconds on how long requests to pfSense may take. Default = 60000 = 60 seconds.</param>
        /// <returns>PfSenseBackupFile instance containing the retrieved backup content from pfSense</returns>
        public string Execute(pfSense pfSenseServer, CookieContainer cookieJar)
        {
            // Create a session on the pfSense webserver
            HttpUtility.HttpCreateSession(pfSenseServer.ServerBaseUrl, cookieJar);

            // Authenticate the session
            var authenticationResult = HttpUtility.AuthenticateViaUrlEncodedFormMethod(string.Concat(pfSenseServer.ServerBaseUrl, "index.php"),
                                                                                       new Dictionary<string, string>
                                                                                       {
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

            var downloadArgs = new Dictionary<string, string>
                {
                    { "donotbackuprrd", pfSenseServer.with_rrd ? "" : "on" },
                    { "nopackages", pfSenseServer.with_package_information ? "" : "on" },
                    { "Submit", "Download configuration" }
                };

            return HttpUtility.DownloadBackupFile(string.Concat(pfSenseServer.ServerBaseUrl, "diag_backup.php"),
                                                                            downloadArgs,
                                                                            cookieJar,
                                                                            pfSenseServer.timeout);
        }
    }
}
