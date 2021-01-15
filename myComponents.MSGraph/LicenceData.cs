using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace myComponents.MSGraph
{
    public class TenantLicenceData
    {
        public TenantLicenceData(SubscribedSku sku)
        {
            nb_used = (int)sku.ConsumedUnits;
            nb_total = (int)sku.PrepaidUnits.Enabled;
            skuId = sku.SkuId.ToString();
            skuPartNumber = sku.SkuPartNumber;
        }

        // "prepaidUnits": "enabled"
        public int nb_total;
        // "consumedUnits"
        public int nb_used;
        // "skuId"
        public string skuId;
        // skuPartNumber
        public string skuPartNumber;
    }

    public class UserLicenceData
    {
        public UserLicenceData(Microsoft.Graph.User user, IUserLicenseDetailsCollectionPage licenseDetails)
        {
            AffectedSKU = new List<string>();
            userId = user.Id;
            userPrincipalName = user.UserPrincipalName;
            Department = user.Department;

            if (licenseDetails != null)
            {
                foreach (Microsoft.Graph.LicenseDetails licenceDetail in licenseDetails)
                {
                    AffectedSKU.Add(licenceDetail.SkuPartNumber);
                }
            }
        }

        public string userPrincipalName;
        public string userId;
        public List<string> AffectedSKU;
        public string Department;
    }
}
