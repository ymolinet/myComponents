using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace myComponents.ActivationWebService.admin.mods
{
    public partial class mod_licences : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnNewLicence_OnClick(object sender, EventArgs e)
        {
            BLL.BLLLicences bllLicences = new BLL.BLLLicences();
            if (!bllLicences.Insert(Int32.Parse(ddlClients.SelectedValue), Int32.Parse(ddlSoftware.SelectedValue), Int32.Parse(tbQuantite.Text)))
            {
                lblInfo.Text = "Une erreur s'est produite: " + bllLicences.GetLastError();
                lblInfo.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblInfo.Text = "L'enregistrement a été ajouté!";
                lblInfo.ForeColor = System.Drawing.Color.Green;
                licencesGridview.DataBind();
                pnlLicences.ForceAnUpdate();
            }
        }
    }
}