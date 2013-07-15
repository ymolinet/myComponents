using System;
using System.Collections.Generic;
using System.Web;

namespace myComponents.ActivationWebService.BLL
{
    [System.ComponentModel.DataObject]
    public class BLLActivation
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

        public String GetExistingKey(Int32 CustomerID, Int32 SoftwareID, String ComputerKey)
        {
            return null;
        }

        public Boolean Insert(Int32 CustomerID, Int32 SoftwareID, String ComputerKey, String SerialKey)
        {
            try
            {
                String Commande = String.Empty;
                // String Commande = "INSERT INTO licences (CUSTOMER_ID, SOFTWARE_ID, LICENCES_NB) VALUES (" + CustomerID + "," + SoftwareID + "," + NBLicences + ")";
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