using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace myComponents.SoftwareLocker.Interfaces.frm
{
    public partial class frmDialog : Form
    {
        private string _Pass;
        private string _Identifier;

        public frmDialog(string identifier, string BaseString,
            string Password, int DaysToEnd, int Runed, string info, string appname)
        {
            InitializeComponent();
            _Identifier = identifier;
            sebBaseString.Text = BaseString;
            _Pass = Password;
            lblDays.Text = DaysToEnd.ToString() + " Day(s)";
            lblTimes.Text = Runed.ToString() + " Time(s)";
            lblText.Text = info;
            if (DaysToEnd <= 0 || Runed <= 0)
            {
                lblDays.Text = "Finished";
                lblTimes.Text = "Finished";
                btnTrial.Enabled = false;
            }

            sebPassword.Select();
            Text = String.Format(Text, appname);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_Pass == sebPassword.Text)
            {
                MessageBox.Show("Password is correct", "Password",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("Password is incorrect", "Password",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void btnTrial_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
        }

        private void btnGetKey_Click(object sender, EventArgs e)
        {
            // On ouvre la fenêtre pour saisir le nom de la société
            frmCompany aForm = new frmCompany();
            if (aForm.ShowDialog() == DialogResult.OK)
            {
                // On fait  appel au webservice pour obtenir la clé de débridage
                // System.Net.ServicePointManager.CertificatePolicy = new myComponents.Crypto.TrustAllCertificatesPolicy();
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                fr.adn_systemes.activation.ActivationService acs = new fr.adn_systemes.activation.ActivationService();
                String value = acs.GetSerialKey(_Identifier, sebBaseString.Text, aForm.Company);
                if (!String.IsNullOrEmpty(value) && (value != "00000-00000-00000-00000-00000"))
                    sebPassword.Text = value;
                else MessageBox.Show("Impossible d'obtenir vos informations d'activation. Veuillez contacter la société DIXINFOR!");
            }
        }
    }
}