using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace myComponents.ActivationWebService.admin.mods
{
    public partial class mod_software : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnNewSoftwares_OnClick(object sender, EventArgs e)
        {
            BLL.BLLSoftwares bllSoftwares = new BLL.BLLSoftwares();
            Int32 Identifier = Int32.Parse(tbIdentifier.Text);
            if (!bllSoftwares.Insert(tbNewSoftware.Text, Identifier))
            {
                lblInfo.Text = "Une erreur s'est produite: " + bllSoftwares.GetLastError();
                lblInfo.ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                lblInfo.Text = "L'enregistrement a été ajouté!";
                lblInfo.ForeColor = System.Drawing.Color.Green;
                softwaresGridview.DataBind();
                pnlSoftwares.ForceAnUpdate();
            }
        }
    }
}