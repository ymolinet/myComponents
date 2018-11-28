using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myComponents.OpenERP7
{
    public class OpenERP7Helper
    {
        private OpenERP7Server _erp;
        private readonly string res_partner = "res.partner";

        public OpenERP7Helper(OpenERP7Server erp)
        {
            _erp = erp;
        }

        public OpenERP7Helper(OpenERP7Credentials credentials)
        {
            _erp = new OpenERP7Server(credentials);
        }

        public object[] GetPartners(string field, string value)
        {
            object[] filters = new object[1];
            filters[0] = new object[] { field, "=", value };

            int[] ids = _erp.Search(res_partner, filters);

            if (ids.Length == 0) return null;
            return _erp.Read(res_partner, ids);
        }

        public object[] GetPartner(int id)
        {
            int[] ids = { id };
            return _erp.Read(res_partner, ids);
        }
    }
}
