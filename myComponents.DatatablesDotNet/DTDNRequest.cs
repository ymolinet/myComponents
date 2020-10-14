using System;
using System.Collections.Generic;
using System.Linq;

namespace myComponents.DatatablesDotNet
{
    public class DTDNRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<DTDNColumn> Columns { get; private set; }
        public DTDNSearch Search { get; private set; }
        public List<DTDNOrder> Orders { get; private set; }
        public List<DTDNParameter> AdditionalParameters { get; private set; }

        public DTDNRequest()
        {
            Columns = new List<DTDNColumn>();
            Search = new DTDNSearch();
            Orders = new List<DTDNOrder>();
            AdditionalParameters = new List<DTDNParameter>();
        }

        public bool ParameterExist(string key)
        {
            return (GetParameter(key) != null);
        }

        public DTDNParameter GetParameter(string key)
        {
            return AdditionalParameters.FirstOrDefault(a => a.key == key);
        }

        public string GetParameterValueAsString(string key)
        {
            DTDNParameter aParam = GetParameter(key);
            if (aParam != null) return aParam.value;
            else return null;
        }

        public long GetParameterValueAsLong(string key)
        {
            DTDNParameter aParam = GetParameter(key);
            if (aParam != null) return String.IsNullOrEmpty(aParam.value) ? -1 : Convert.ToInt64(aParam.value);
            else return -1;
        }

        public bool GetParameterValueAsBool(string key)
        {
            DTDNParameter aParam = GetParameter(key);
            if (aParam != null) return String.IsNullOrEmpty(aParam.value) ? false : Convert.ToBoolean(aParam.value);
            else return false;
        }
    }
}