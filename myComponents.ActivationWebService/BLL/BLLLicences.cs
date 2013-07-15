using System;
using System.Collections.Generic;
using System.Web;

namespace myComponents.ActivationWebService.BLL
{
    [System.ComponentModel.DataObject]
    public class BLLLicences
    {
        // Fields
        private MySql.Data.MySqlClient.MySqlConnection _cnx = null;
        private String _lasterror = String.Empty;

        // Properties
        protected MySql.Data.MySqlClient.MySqlConnection Connexion
        {
            get
            {
                if (this._cnx == null)
                {
                    this._cnx = new MySql.Data.MySqlClient.MySqlConnection(Properties.Settings.Default.BDD);
                }
                return this._cnx;
            }
        }

        public String GetLastError()
        {
            return this._lasterror;
        }

        public Boolean LicenceAvailable(String CompanyName, Int32 AppIdentifier)
        {
            try
            {
                String Commande = "SELECT (licences.LICENCES_NB - COUNT(activation_keys.ACTIVATION_KEY)) AS LICENCE_FREE FROM licences INNER JOIN softwares ON licences.SOFTWARE_ID = softwares.ID INNER JOIN customers ON licences.CUSTOMER_ID = customers.ID LEFT OUTER JOIN activation_keys ON activation_keys.CUSTOMER_ID = customers.ID AND activation_keys.SOFTWARE_ID = softwares.ID WHERE softwares.identifier=" + AppIdentifier + " AND customers.nom='" + CompanyName + "'";
                Connexion.Open();
                MySql.Data.MySqlClient.MySqlCommand sqlCmd = new MySql.Data.MySqlClient.MySqlCommand(Commande, Connexion);
                Int32 FreeLicence = (Int32)sqlCmd.ExecuteScalar();
                return FreeLicence > 0;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                _lasterror = ex.Message;
                return false;
            }
        }

        public Boolean Insert(Int32 CustomerID, Int32 SoftwareID, Int32 NBLicences)
        {
            try
            {
                String Commande = "INSERT INTO licences (CUSTOMER_ID, SOFTWARE_ID, LICENCES_NB) VALUES (" + CustomerID + "," + SoftwareID + "," + NBLicences + ")";
                Connexion.Open();
                MySql.Data.MySqlClient.MySqlCommand sqlCmd = new MySql.Data.MySqlClient.MySqlCommand(Commande, Connexion);
                sqlCmd.ExecuteNonQuery();
                Connexion.Close();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                _lasterror = ex.Message;
                return false;
            }
        }


    }
}