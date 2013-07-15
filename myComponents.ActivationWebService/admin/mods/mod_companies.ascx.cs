using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace myComponents.ActivationWebService.admin.mods
{
    public partial class mod_companies : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnNewCustomer_OnClick(object sender, EventArgs e)
        {
            BLL.BLLCustomers bllCustomers = new BLL.BLLCustomers();
            if (!bllCustomers.Insert(tbNewCustomer.Text))
            {
                lblInfo.Text = "Une erreur s'est produite: " + bllCustomers.GetLastError();
                lblInfo.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblInfo.Text = "Enregistrement ajouté !";
                lblInfo.ForeColor = System.Drawing.Color.Green;
                customerGridview.DataBind();
                pnlCustomer.ForceAnUpdate();
            }
            
        }
    }
}