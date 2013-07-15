using System;
using System.Security;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace myComponents.Exchange
{
    public static class Admin
    {
        // http://blog.pedroliska.com/2011/07/28/creating-an-exchange-2010-mailbox-from-a-remote-c-program/
        // http://technet.microsoft.com/en-us/library/dd298084.aspx
        // http://devblog.rayonnant.net/2011/04/exchange-2010-enable-mailbox-through-c.html

        public static Boolean NewMailbox(String userName, SecureString userPassword, String userPrincipalName, String organizationalUnit, String MailboxDatabaseName, String ExchangeServerName, String AdminName, SecureString AdminPassword, AuthenticationMechanism Auth, ref StringBuilder errorList)
        {
            PSCredential credentials = new PSCredential(AdminName, AdminPassword);

            // Create the runspace where the command will be executed
            Runspace runspace = RunspaceFactory.CreateRunspace(GetConnInfo(credentials, Auth, ExchangeServerName));
            return NewMailbox(userName, userPassword, userPrincipalName, organizationalUnit, MailboxDatabaseName, runspace, ref errorList);
        }

        public static WSManConnectionInfo GetConnInfo(AuthenticationMechanism Auth, String ExchangeServerRef)
        {
            return GetConnInfo(PSCredential.Empty, Auth, ExchangeServerRef);
        }

        public static WSManConnectionInfo GetConnInfo(String Username, SecureString UserPassword, AuthenticationMechanism Auth, String ExchangeServerRef)
        {
            PSCredential credentials = new PSCredential(Username, UserPassword);
            return GetConnInfo(credentials, Auth, ExchangeServerRef);
        }

        public static WSManConnectionInfo GetConnInfo(PSCredential credentials, AuthenticationMechanism Auth, String ExchangerServerRef)
        {
            // Prepare the connection
            WSManConnectionInfo connInfo = new WSManConnectionInfo(
                new Uri("https://" + ExchangerServerRef + "/PowerShell"),
                "http://schemas.microsoft.com/powershell/Microsoft.Exchange",
                credentials);

            connInfo.AuthenticationMechanism = Auth;
            connInfo.SkipCACheck = true;
            connInfo.SkipCNCheck = true;
            connInfo.SkipRevocationCheck = true;

            return connInfo;
        }

        public static Boolean NewMailbox(String userName, SecureString userPassword, String userPrincipalName, String organizationalUnit, String MailboxDatabaseName, String ExchangeServerName, AuthenticationMechanism Auth, ref StringBuilder errorList)
        {
            // Create the runspace where the command will be executed
            Runspace runspace = RunspaceFactory.CreateRunspace(GetConnInfo(Auth, ExchangeServerName));
            return NewMailbox(userName, userPassword, userPrincipalName, organizationalUnit, MailboxDatabaseName, runspace, ref errorList);
        }


        public static Boolean NewMailbox(String userName, SecureString userPassword, String userPrincipalName, String organizationalUnit, String MailboxDatabaseName, Runspace runspace, ref StringBuilder errorList)
        {
            try
            {
                // runspace.Open();
            }
            catch (Exception ex)
            {
                errorList.Append(ex.Message);
                errorList.Append(Environment.NewLine);
                return false;
            }

            Pipeline pipeLine = runspace.CreatePipeline();
            if (!MailboxExist(userPrincipalName, runspace))
            {
                try
                {
                    Command createMailbox = new Command("New-Mailbox");
                    createMailbox.Parameters.Add("Name", userName);
                    createMailbox.Parameters.Add("userPrincipalName", userPrincipalName);
                    createMailbox.Parameters.Add("Password", userPassword);
                    createMailbox.Parameters.Add("Database", MailboxDatabaseName);
                    createMailbox.Parameters.Add("OrganizationalUnit", organizationalUnit);
                    pipeLine.Commands.Add(createMailbox);
                    pipeLine.Invoke();
                }
                catch (Exception ex)
                {
                    errorList.Append(ex.Message);
                    errorList.Append(System.Environment.NewLine);
                    pipeLine.Dispose();
                    return false;
                }

                if (pipeLine.Error != null && pipeLine.Error.Count > 0)
                {
                    try
                    {
                        foreach (object item in pipeLine.Error.ReadToEnd())
                        {
                            errorList.Append(item.ToString());
                            errorList.Append(System.Environment.NewLine);
                            pipeLine.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        errorList.Append(ex.Message);
                        errorList.Append(System.Environment.NewLine);
                        pipeLine.Dispose();
                    }
                    return false;
                }
                pipeLine.Dispose();
                return true;
            }
            else
            {
                errorList.Append("Mailbox already exist");
                errorList.Append(Environment.NewLine);
                pipeLine.Dispose();
                return false;
            }
        }

        public static Boolean EnableMailbox(String userMail, String MailboxDatabaseName, String ExchangeServerName, String AdminName, String AdminPassword, ref StringBuilder errorList)
        {
            return EnableMailbox(userMail, MailboxDatabaseName, ExchangeServerName, AdminName, myComponents.Utilities.myConvert.ToSecureString(AdminPassword), ref errorList);
        }

        public static Boolean EnableMailbox(String userMail, String MailboxDatabaseName, String ExchangeServerName, String AdminName, SecureString AdminPassword, ref StringBuilder errorList)
        {
            PSCredential credentials = new PSCredential(AdminName, AdminPassword);
            // Prepare the connection
            WSManConnectionInfo connInfo = new WSManConnectionInfo(
                new Uri("http://" + ExchangeServerName + "/PowerShell"),
                "http://schemas.microsoft.com/powershell/Microsoft.Exchange",
                credentials);

            connInfo.AuthenticationMechanism = AuthenticationMechanism.Basic;

            // Create the runspace where the command will be executed
            Runspace runspace = RunspaceFactory.CreateRunspace(connInfo);

            return EnableMailbox(userMail, MailboxDatabaseName, runspace, ref errorList);
        }

        public static Boolean EnableMailbox(String userMail, String MailboxDatabaseName, Runspace runspace, ref StringBuilder errorList)
        {
            // runspace.Open();
            Pipeline pipeLine = runspace.CreatePipeline();
            if (!MailboxExist(userMail, runspace))
            {
                Command createMailbox = new Command("Enable-Mailbox");
                createMailbox.Parameters.Add("identity", userMail);
                createMailbox.Parameters.Add("database", MailboxDatabaseName);
                pipeLine.Commands.Add(createMailbox);
                pipeLine.Invoke();
                if (pipeLine.Error != null && pipeLine.Error.Count > 0)
                {
                    foreach (object item in pipeLine.Error.ReadToEnd())
                    {
                        errorList.Append(item.ToString());
                        errorList.Append(System.Environment.NewLine);
                    }
                    pipeLine.Dispose();
                    return false;
                }
                pipeLine.Dispose();
                return true;
            }
            else
            {
                errorList.Append("Mailbox already exist");
                errorList.Append(System.Environment.NewLine);
                pipeLine.Dispose();
                return false;
            }
        }

        public static Boolean MailboxExist(String userMail, Runspace runspace)
        {
            // if (runspace.RunspaceStateInfo.State != RunspaceState.Opened) runspace.Open();

            Pipeline pipeLine = runspace.CreatePipeline();
            Command getMailBox = new Command("Get-User");
            getMailBox.Parameters.Add("identity", userMail);
            pipeLine.Commands.Add(getMailBox);
            Collection<PSObject> user = pipeLine.Invoke();

            if (user.Count == 0) { pipeLine.Dispose(); return false; }

            PSMemberInfo item = user[0].Properties.Where(property => property.Name == "RecipientType").SingleOrDefault();
            if (item != null)
            {
                if (String.Equals(item.Value.ToString(), "UserMailbox", StringComparison.OrdinalIgnoreCase))
                {
                    pipeLine.Dispose();
                    return true;
                }
                else
                {
                    pipeLine.Dispose();
                    return false;
                }
            }
            else
            {
                pipeLine.Dispose();
                return false;
            }
        }

        public static void ClearSession(Runspace runspace)
        { 
            // if (runspace.RunspaceStateInfo.State != RunspaceState.Opened) runspace.Open();

            Pipeline pipeLine = runspace.CreatePipeline();
            Command removeSession = new Command("Remove-PSSession");
            removeSession.Parameters.Add("$<session>");
            pipeLine.Commands.Add(removeSession);
            Collection<PSObject> user = pipeLine.Invoke();
            pipeLine.Dispose();
        }
    }
}
