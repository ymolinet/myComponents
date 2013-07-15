using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using Microsoft.Win32;
using System.ServiceProcess;


namespace myServices
{
    public class Systray : Form
    {
        private NotifyIcon _trayIcon;
        private ContextMenu _trayMenu;
        private ArrayList _services = new ArrayList();

        public Systray()
        {
            GetServices();
            _trayMenu = new ContextMenu();

            LoadMenuServices();

            _trayIcon = new NotifyIcon();
            _trayIcon.Text = "myServices";
            _trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            _trayIcon.ContextMenu = _trayMenu;
            _trayIcon.Visible = true;
        }

        private void LoadMenuServices()
        {
            _trayMenu.MenuItems.Clear();

            MenuItem StartedServices = new MenuItem("Démarrer un service");
            AddSubMenuStartedServices(StartedServices);
            _trayMenu.MenuItems.Add(StartedServices);

            MenuItem StoppedServices = new MenuItem("Arreter un service");
            AddSubMenuStoppedServices(StoppedServices);
            _trayMenu.MenuItems.Add(StoppedServices);

            _trayMenu.MenuItems.Add("Rafraichir l'état des services", OnRefreshState);
            _trayMenu.MenuItems.Add("Configurer", OnConfigure);
            _trayMenu.MenuItems.Add("Quitter", OnExit);


        }

        private void OnRefreshState(object sender, EventArgs e)
        {
            GetServices();
            LoadMenuServices();
        }

        private void AddSubMenuStartedServices(MenuItem aMenu)
        {
            aMenu.MenuItems.Clear();
            foreach (ServiceController aService in ServiceController.GetServices())
            {
                if ((_services.Contains(aService.ServiceName)) && (aService.Status == ServiceControllerStatus.Stopped))
                {
                    MenuItem anItem = new MenuItem(aService.DisplayName + " (" + aService.ServiceName + ")", OnStartService);
                    anItem.Tag = aService.ServiceName;
                    aMenu.MenuItems.Add(anItem);
                }
            }
            if (aMenu.MenuItems.Count == 0)
            {
                MenuItem anItem = new MenuItem("Aucun service a démarrer");
                anItem.Enabled = false;
                aMenu.MenuItems.Add(anItem);
            }
        }

        private void AddSubMenuStoppedServices(MenuItem aMenu)
        {
            aMenu.MenuItems.Clear();
            foreach (ServiceController aService in ServiceController.GetServices())
            {
                if ((_services.Contains(aService.ServiceName)) && (aService.Status == ServiceControllerStatus.Running))
                {
                    MenuItem anItem = new MenuItem(aService.DisplayName + " (" + aService.ServiceName + ")", OnStopService);
                    anItem.Tag = aService.ServiceName;
                    aMenu.MenuItems.Add(anItem);
                }
            }
            if (aMenu.MenuItems.Count == 0)
            {
                MenuItem anItem = new MenuItem("Aucun service a arrêter");
                anItem.Enabled = false;
                aMenu.MenuItems.Add(anItem);
            }
        }

        private void OnStartService(object sender, EventArgs e)
        {
            ServiceController aService = new ServiceController((String)((MenuItem)sender).Tag);
            if (aService.Status == ServiceControllerStatus.Stopped) aService.Start();
            TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
            aService.WaitForStatus(ServiceControllerStatus.Running, timeout);
            LoadMenuServices();
        }

        private void OnStopService(object sender, EventArgs e)
        {
            ServiceController aService = new ServiceController((String)((MenuItem)sender).Tag);
            if (aService.Status == ServiceControllerStatus.Running) aService.Stop();
            TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
            aService.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            LoadMenuServices();

        }

        private void GetServices()
        {
            _services.Clear();
            RegistryKey aKey = Registry.CurrentUser.OpenSubKey("Software\\myServices");
            if (aKey == null) aKey = Registry.CurrentUser.CreateSubKey("Software\\myServices");

            object svc = aKey.GetValue("ServicesSelected");
            if (svc != null) _services.AddRange((String[])svc);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
            base.OnLoad(e);
        }

        private void OnConfigure(object sender, EventArgs e)
        {
            ConfigForm aForm = new ConfigForm(_services);
            aForm.ShowDialog();
            LoadMenuServices();
        }

        private void OnExit(object sender, EventArgs e)
        {
            if (MessageBox.Show("Etes-vous sûr de vouloir quitter ?", "Quitter l'application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                _trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
