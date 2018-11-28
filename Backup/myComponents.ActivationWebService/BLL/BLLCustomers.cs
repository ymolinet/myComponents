using System;
using System.Collections.Generic;
using System.Web;

namespace myComponents.ActivationWebService.BLL
{
    [System.ComponentModel.DataObject]
    public class BLLCustomers
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

        public Boolean Insert(String NomClient)
        {
            try
            {
                String Commande = "INSERT INTO customers (NOM, ACTIF) VALUES ('" + NomClient + "',true)";
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