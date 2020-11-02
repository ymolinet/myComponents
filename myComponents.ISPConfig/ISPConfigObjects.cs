using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace myComponents.ISPConfig
{
    public abstract class ISPConfigBaseObject
    {
        protected XElement _xml;
        public ISPConfigBaseObject(XElement xmlReturn)
        {
            _xml = xmlReturn;
        }
        protected string GetValue(string propertyName)
        {
            //Remove "get_"
            string key = propertyName;
            if (propertyName.StartsWith("get_"))
                key = propertyName.Substring(4, propertyName.Length - 4);
            XElement xKey = _xml.Descendants().Where(e => e.Name.LocalName == "key" && e.Value == key).FirstOrDefault();
            return xKey.Parent.Elements("value").First().Value;
        }

        protected void SetValue(string propertyName, string value)
        {
            // Remove set_
            string key = propertyName;
            if (propertyName.StartsWith("set_"))
                key = propertyName.Substring(4, propertyName.Length - 4);
            XElement xKey = _xml.Descendants().Where(e => e.Name.LocalName == "key" && e.Value == key).FirstOrDefault();
            xKey.Parent.Elements("value").First().Value = value;
        }

        public string sys_userid
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string sys_groupid
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string sys_perm_user
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string sys_perm_group
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string sys_perm_other
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
    }

    public class ISPConfigClientDetails : ISPConfigBaseObject
    {
        public ISPConfigClientDetails(XElement xml) : base(xml) { }

        public string client_id
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }

        public string company_name
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string company_id
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string gender
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string contact_firstname
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string contact_name
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string customer_no
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string vat_id
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string street
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string zip
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string city
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string state
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string country
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string telephone
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string mobile
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string fax
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string email
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string internet
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string icq
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string notes
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string bank_account_owner
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string bank_account_number
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string bank_code
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string bank_name
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string bank_account_iban
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string bank_account_swift
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string paypal_email
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string default_mailserver
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string mail_servers
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_maildomain
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_mailbox
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_mailalias
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_mailaliasdomain
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_mailforward
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_mailcatchall
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_mailrouting
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_mailfilter
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_fetchmail
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_mailquota
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_spamfilter_wblist
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_spamfilter_user
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_spamfilter_policy
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string default_webserver
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string web_servers
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_web_ip
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_web_domain
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_web_quota
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string web_php_options
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_cgi
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_ssi
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_perl
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_ruby
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_python
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string force_suexec
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_hterror
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_wildcard
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_ssl
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_ssl_letsencrypt
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_web_subdomain
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_web_aliasdomain
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_ftp_user
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_shell_user
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string ssh_chroot
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_webdav_user
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_backup
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_directive_snippets
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_aps
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string default_dnsserver
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string dns_servers
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_dns_zone
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string default_slave_dnsserver
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_dns_slave_zone
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_dns_record
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string default_dbserver
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string db_servers
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_database
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_database_user
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_database_quota
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_cron
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_cron_type
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_cron_frequency
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_traffic_quota
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_client
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_mailmailinglist
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_openvz_vm
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_openvz_vm_template_id
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string parent_client_id
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string username
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string password
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string language
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string usertheme
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string template_master
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string template_additional
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string created_at
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string locked
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string canceled
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string can_use_api
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string tmp_data
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string id_rsa
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string ssh_rsa
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string customer_no_template
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string customer_no_start
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string customer_no_counter
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string added_date
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string added_by
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_domainmodule
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string default_xmppserver
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string xmpp_servers
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_xmpp_domain
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_xmpp_user
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_xmpp_muc
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_xmpp_anon
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_xmpp_auth_options
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_xmpp_vjud
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_xmpp_proxy
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_xmpp_status
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_xmpp_pastebin
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string limit_xmpp_httparchive
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
    }

    public class ISPConfigMailDomainDetails : ISPConfigBaseObject
    {
        public ISPConfigMailDomainDetails(XElement xml) : base(xml) { }

        public string domain_id
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }


        public string server_id
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string domain
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string dkim
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string dkim_selector
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string dkim_private
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string dkim_public
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string active
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
    }

    public class ISPConfigMailUserDetails : ISPConfigBaseObject
    {
        public ISPConfigMailUserDetails(XElement xml) : base(xml) { }

        public string mailuser_id
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string server_id
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string email
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string login
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string password
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string name
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string uid
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string gid
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string maildir
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string maildir_format
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string quota
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string cc
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string sender_cc
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string homedir
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string autoresponder
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string autoresponder_start_date
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string autoresponder_end_date
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string autoresponder_subject
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string autoresponder_text
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string move_junk
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string custom_mailfilter
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string postfix
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string greylisting
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string access
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string disableimap
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string disablepop3
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string disabledeliver
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string disablesmtp
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string disablesieve
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string disablesieve_filter
        {
            get
            {
                return GetValue("disablesieve-filter");
            }
            set
            {
                SetValue("disablesieve-filter", value);
            }
        }
        public string disablelda
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string disablelmtp
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string disabledoveadm
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string last_quota_notification
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string backup_interval
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
        public string backup_copies
        {
            get
            {
                return GetValue(System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            set
            {
                SetValue(System.Reflection.MethodBase.GetCurrentMethod().Name, value);
            }
        }
    }

}