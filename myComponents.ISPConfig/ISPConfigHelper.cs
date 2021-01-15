using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace myComponents.ISPConfig
{
    public class ISPConfigHelper
    {
        private string _session_id;
        private ISPConfigWS ISPConfigAPI;

        public Exception LastSOAPError
        {
            get
            {
                return ISPConfigAPI.LastException;
            }
        }
        public ISPConfigHelper(string ISPConfigAPIURL)
        {
            ISPConfigAPI = new ISPConfigWS(ISPConfigAPIURL);
        }

        public bool login(string username, string password)
        {
            ListDictionary parameters = new ListDictionary();
            parameters.Add("username", username);
            parameters.Add("password", password);

            ISPConfigAPI.Invoke("login", parameters);

            if (ISPConfigAPI.RequestSuccess)
                _session_id = ISPConfigAPI.StringResult;
            return ISPConfigAPI.RequestSuccess;
        }


        public bool logout()
        {
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                ISPConfigAPI.Invoke("logout", parameters);
                return ISPConfigAPI.RequestSuccess;
            }
            return false;
        }

        public string get_function_list()
        {
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                ISPConfigAPI.Invoke("get_function_list", parameters);

                if (ISPConfigAPI.RequestSuccess)
                    return ISPConfigAPI.StringResult;
                else return null;
            }
            else return null;
        }
        public ISPConfigClientDetails client_get(Int32 client_id)
        {
            if (client_id == -1) throw new Exception("-1 return an array not a single object, use clients_getall() instead");
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                parameters.Add("client_id", client_id.ToString());
                ISPConfigAPI.Invoke("client_get", parameters);

                if (ISPConfigAPI.RequestSuccess)
                    return new ISPConfigClientDetails(ISPConfigAPI.XmlResult);
                else return null;
            }
            else return null;
        }

        public ISPConfigClientDetails client_get_by_customerno(string customer_no)
        {
            if (String.IsNullOrEmpty(customer_no)) throw new Exception("customer_no could no be null or empty");
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                ListDictionary primary_id = new ListDictionary();
                primary_id.Add("customer_no", customer_no);
                parameters.Add("primary_id", primary_id);
                ISPConfigAPI.Invoke("client_get", parameters);

                if (ISPConfigAPI.RequestSuccess)
                {
                    if (ISPConfigAPI.XmlResult.HasElements)
                        return new ISPConfigClientDetails(ISPConfigAPI.XmlResult.Elements().First());
                    else return null;
                }
                else return null;
            }
            else return null;
        }

        public List<ISPConfigClientDetails> clients_getall()
        {
            List<ISPConfigClientDetails> result = new List<ISPConfigClientDetails>();
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                parameters.Add("client_id", "-1");
                ISPConfigAPI.Invoke("client_get", parameters);

                if (ISPConfigAPI.RequestSuccess)
                {
                    foreach (XElement item in ISPConfigAPI.XmlResult.Elements())
                    {
                        result.Add(new ISPConfigClientDetails(item));
                    }
                    return result;
                }
                else return result;
            }
            else return result;
        }

        public ISPConfigMailDomainDetails mail_domain_get(string primary_id)
        {
            if (primary_id == "-1") throw new Exception("-1 return an array not a single object, use mail_domains_getall() instead");
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                parameters.Add("primary_id", primary_id);
                ISPConfigAPI.Invoke("mail_domain_get", parameters);
                if (ISPConfigAPI.RequestSuccess)
                    return new ISPConfigMailDomainDetails(ISPConfigAPI.XmlResult);
                else return null;
            }
            else return null;
        }

        public List<ISPConfigMailDomainDetails> mail_domains_getall()
        {
            List<ISPConfigMailDomainDetails> result = new List<ISPConfigMailDomainDetails>();
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                parameters.Add("primary_id", "-1");
                ISPConfigAPI.Invoke("mail_domain_get", parameters);
                if (ISPConfigAPI.RequestSuccess)
                {
                    foreach (XElement item in ISPConfigAPI.XmlResult.Elements())
                    {
                        result.Add(new ISPConfigMailDomainDetails(item));
                    }
                    return result;
                }
                else return result;
            }
            else return result;
        }

        public ISPConfigMailDomainDetails mail_domain_get_by_domain(string domain)
        {
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                parameters.Add("domain", domain);
                ISPConfigAPI.Invoke("mail_domain_get_by_domain", parameters);
                if (ISPConfigAPI.RequestSuccess)
                    return new ISPConfigMailDomainDetails(ISPConfigAPI.XmlResult);
                else return null;
            }
            else return null;
        }

        public ISPConfigMailUserDetails mail_user_get(string primary_id)
        {
            if (primary_id == "-1") throw new Exception("-1 return an array not a single object, use mail_users_getall() instead");
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                parameters.Add("primary_id", primary_id);
                ISPConfigAPI.Invoke("mail_user_get", parameters);
                if (ISPConfigAPI.RequestSuccess)
                    return new ISPConfigMailUserDetails(ISPConfigAPI.XmlResult);
                else return null;
            }
            else return null;
        }

        public void mail_user_update(ISPConfigMailUserDetails mail_user)
        {
            if (_session_id != null)
            {
               // TODO
            }
        }
        public List<ISPConfigMailUserDetails> mail_users_getall()
        {
            List<ISPConfigMailUserDetails> result = new List<ISPConfigMailUserDetails>();
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                parameters.Add("primary_id", "-1");
                ISPConfigAPI.Invoke("mail_user_get", parameters);
                if (ISPConfigAPI.RequestSuccess)
                {
                    foreach (XElement item in ISPConfigAPI.XmlResult.Elements())
                    {
                        result.Add(new ISPConfigMailUserDetails(item));
                    }
                    return result;
                }
                else return result;
            }
            else return result;
        }

        public List<ISPConfigMailUserDetails> mail_users_get_by_domain(string domain)
        {
            List<ISPConfigMailUserDetails> result = new List<ISPConfigMailUserDetails>();
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                ListDictionary primary_id = new ListDictionary();
                primary_id.Add("email", domain);
                parameters.Add("primary_id", primary_id);
                ISPConfigAPI.Invoke("mail_user_get", parameters);
                if (ISPConfigAPI.RequestSuccess)
                { 
                    foreach(XElement item in ISPConfigAPI.XmlResult.Elements())
                    {
                        result.Add(new ISPConfigMailUserDetails(item));
                    }
                    return result;
                }
                else return result;
            }
            else return result;
        }

        public List<ISPConfigMailUserDetails> mail_users_get_by_sysgroupid(Int32 sys_groupid)
        {
            if (sys_groupid == 0) throw new Exception("sys_groupid could not be 0.");
            List<ISPConfigMailUserDetails> result = new List<ISPConfigMailUserDetails>();
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                ListDictionary primary_id = new ListDictionary();
                primary_id.Add("sys_groupid", sys_groupid.ToString());
                parameters.Add("primary_id", primary_id);
                ISPConfigAPI.Invoke("mail_user_get", parameters);
                if (ISPConfigAPI.RequestSuccess)
                {
                    foreach (XElement item in ISPConfigAPI.XmlResult.Elements())
                    {
                        result.Add(new ISPConfigMailUserDetails(item));
                    }
                    return result;
                }
                else return result;
            }
            else return result;
        }

        public Int32 client_get_groupid(Int32 clientid)
        {
            if (_session_id != null)
            {
                ListDictionary parameters = new ListDictionary();
                parameters.Add("session_id", _session_id);
                parameters.Add("primary_id", clientid.ToString());
                ISPConfigAPI.Invoke("client_get_groupid", parameters);
                if (ISPConfigAPI.RequestSuccess)
                {
                    Int32 result = 0;
                    Int32.TryParse(ISPConfigAPI.StringResult, out result);
                    return result;
                }
                else return 0;
            }
            else return 0;
        }

    }
}
