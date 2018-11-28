using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

namespace myComponents.ActivationWebService
{
    /// <summary>
    /// Description résumée de ActivationService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class ActivationService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetSerialKey(String AppIdentifier, String CompIdentifier, String CompanyName)
        {
            // Cherche le nombre de clé disponible pour l'entreprise (CompanyName) et vérifier que le nombre maximum de clé n'est pas atteinte.
            BLL.BLLLicences bllLicences = new BLL.BLLLicences();
            if (bllLicences.LicenceAvailable(CompanyName, Int32.Parse(AppIdentifier)))
            {
                // TODO: Vérifier que AppIdentifier et CompIdentifier sont déjà dans la base de données pour la redonner (réinstallation).

                String SerialKey = myComponents.Crypto.Encryption.MakePassword(CompIdentifier, AppIdentifier);
                return SerialKey;
            } return null;
        }
    }
}
