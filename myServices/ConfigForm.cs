using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using Microsoft.Win32;
using myComponents.Object;

namespace myServices
{
    public partial class ConfigForm : Form
    {
        private Boolean _changemade = false;
        private ArrayList _services;

        public ConfigForm(ArrayList services)
        {
            InitializeComponent();
            _services = services;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadServicesSelected();
            LoadServicesInstalled();
            LoadStartWithWindows();
        }

        private void LoadStartWithWindows()
        {
            RegistryKey aKey = Registry.CurrentUser.OpenSubKey("Software\\myServices");
            if (aKey == null) Registry.CurrentUser.CreateSubKey("Software\\myServices");

            object val = aKey.GetValue("StartWithWindows");

            cbLoadOnStart.Checked = Convert.ToBoolean(val);
        }

        private void LoadServicesSelected()
        {
            lbServicesSelected.Items.Clear();
            foreach (ServiceController aService in ServiceController.GetServices())
            {
                if (_services.Contains(aService.ServiceName))
                {
                    StringItem anItem = new StringItem(aService.DisplayName + " (" + aService.ServiceName + ")", aService.ServiceName);
                    lbServicesSelected.Items.Add(anItem);
                }
            }
            lbServicesSelected.Sorted = true;
        }

        private void LoadServicesInstalled()
        {
            lbServicesInstalled.Items.Clear();
            foreach (ServiceController aService in ServiceController.GetServices())
            {
                StringItem anItem = new StringItem(aService.DisplayName + " (" + aService.ServiceName + ")", aService.ServiceName);
                if (!lbServicesSelected.Items.Contains(anItem))
                    lbServicesInstalled.Items.Add(anItem);
            }
            lbServicesInstalled.Sorted = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!lbServicesSelected.Items.Contains(lbServicesInstalled.SelectedItem))
            {
                lbServicesSelected.Items.Add(lbServicesInstalled.SelectedItem);
                LoadServicesInstalled();
                btnApply.Enabled = true;
                _changemade = true;
            }
            else { MessageBox.Show("Ce service est déjà dans la liste", "Service", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            lbServicesSelected.Items.Remove(lbServicesSelected.SelectedItem);
            LoadServicesInstalled();
            btnApply.Enabled = true;
            _changemade = true;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            RegistryKey aKey = Registry.CurrentUser.OpenSubKey(@"Software\myServices", true);
            if (aKey == null) Registry.CurrentUser.CreateSubKey(@"Software\myServices");

            _services.Clear();
            foreach (StringItem anItem in lbServicesSelected.Items)
            {
                _services.Add(anItem.Value);
            }

            aKey.SetValue("StartWithWindows", cbLoadOnStart.Checked, RegistryValueKind.DWord);
            aKey.SetValue("ServicesSelected", _services.ToArray(typeof(String)), RegistryValueKind.MultiString);


            // Paramètre de démarrage HKLM\Software\Microsoft\Windows\CurrentVersion\Run

            RegistryKey RunKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (cbLoadOnStart.Checked)
                RunKey.SetValue("myServices", Application.ExecutablePath, RegistryValueKind.String);
            else
            {
                if (RunKey.GetValue("myServices") != null)
                    RunKey.DeleteValue("myServices");
            }

            btnApply.Enabled = false;
            _changemade = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_changemade)
            {
                if (MessageBox.Show("Des modifications ont été apportées, voulez-vous les enregistrer ?", "Modification des paramètres", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    btnApply_Click(sender, e);
            }
            this.Close();
        }

        private void cbLoadOnStart_CheckedChanged(object sender, EventArgs e)
        {
            _changemade = true;
            btnApply.Enabled = true;
        }
    }
}
