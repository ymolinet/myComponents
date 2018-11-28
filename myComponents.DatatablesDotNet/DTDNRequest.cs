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
            return AdditionalParameters.Where(a => a.key == key).FirstOrDefault();
        }

        public string GetParameterValue(string key)
        {
            DTDNParameter aParam = GetParameter(key);
            if (aParam != null) return aParam.value;
            else return null;
        }
    }
}