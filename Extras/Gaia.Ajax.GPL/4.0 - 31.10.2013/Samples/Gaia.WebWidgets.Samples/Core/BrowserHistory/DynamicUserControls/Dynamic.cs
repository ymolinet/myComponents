namespace Gaia.WebWidgets.Samples.Core.BrowserHistory.DynamicUserControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    [ParseChildren(true)]
    public class DynamicUserControl : GaiaControl
    {
        private readonly Panel _container = new Panel();
        private readonly Gaia.WebWidgets.BrowserHistory _browserHistory = new Gaia.WebWidgets.BrowserHistory();

        private readonly List<IDynamicUserControlEntry> _dynamicUserControls =
            new List<IDynamicUserControlEntry>();

        public event EventHandler Reloading;

        [PersistenceMode(PersistenceMode.InnerProperty),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<IDynamicUserControlEntry> DynamicUserControls
        {
            get { return _dynamicUserControls; }
        }

        private IDynamicUserControlEntry GetSelectedUserControl()
        {
            return GetEntryByIndex(SelectedEntryIndex);
        }

        public int StepsComplete
        {
            get { return (int)(ViewState["sc"] ?? 0); }
            set { ViewState["sc"] = value; }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            int idx = 0;
            foreach (IDynamicUserControlEntry controlEntry in DynamicUserControls)
            {
                if (string.IsNullOrEmpty(controlEntry.UserControlPath))
                    throw new ApplicationException("UserControlPath cannot be empty");

                Control control = Page.LoadControl(controlEntry.UserControlPath);
                control.ID = controlEntry.Token;
                control.Visible = idx++ == SelectedEntryIndex;
                _container.Controls.Add(control);
            }

            Controls.Add(_container);

            _browserHistory.Navigated += BrowserHistoryNavigated;
            Controls.Add(_browserHistory);
        }

        protected override void OnLoad(EventArgs e)
        {
            EnsureChildControls();

            if (!Page.IsPostBack)
                AddHistoryToken();

            base.OnLoad(e);
        }


        void BrowserHistoryNavigated(object sender, Gaia.WebWidgets.BrowserHistory.BrowserHistoryEventArgs e)
        {
            IDynamicUserControlEntry entry = FindEntryByToken(e.Token);
            if (entry == null)
                return;

            SelectedEntryIndex = GetIndexOfEntry(FindEntryByToken(e.Token));
            Reload(); // event should be thrown here ... since it's initiated from browser
        }

        private void AddHistoryToken()
        {
            if (SelectedEntryIndex != -1)
                _browserHistory.AddHistory(GetSelectedUserControl().Token);
        }

        public void DisplayUserControl(IDynamicUserControlEntry controlEntry)
        {
            SelectedEntryIndex = GetIndexOfEntry(controlEntry);
        }

        public Control GetActiveControl()
        {
            for (int idx = 0; idx < _container.Controls.Count; idx++)
                if (_container.Controls[idx].Visible)
                    return _container.Controls[idx];

            throw new ApplicationException("No Active Control was defined");
        }

        public T GetInstanceOf<T>() where T : Control
        {
            return WebUtility.First<T>(_container.Controls);
        }

        public void DisplayUserControlByTypeAndReload<T>() where T : UserControl
        {
            //todo: make it recursive too so that we can go deeper to find our control or model
            for (int idx = 0; idx < _container.Controls.Count; idx++)
                if (_container.Controls[idx] is T)
                {
                    SelectedEntryIndex = idx;
                    Reload();
                }
        }

        public void Reload() { Reload(false); }
        public void Reload(bool fireReload)
        {
            // toggle visibility based on the idx;
            for (int idx = 0; idx < _container.Controls.Count; idx++)
            {
                bool isActiveView = idx == SelectedEntryIndex;
                _container.Controls[idx].Visible = isActiveView;

                if (!isActiveView)
                    continue;

                // Fire ReceiveActiveView if it's the newly loaded view
                DynamicUserControlEntryBase entrybase = _container.Controls[idx] as DynamicUserControlEntryBase;
                if (entrybase != null)
                    entrybase.ReceiveActiveView();
            }

            // we need to do forceUpdate here since we're toggling visibility of a non-gaia control 
            _container.ForceAnUpdate();

            AddHistoryToken();

            if (fireReload && Reloading != null)
                Reloading(this, EventArgs.Empty);
        }

        public int SelectedEntryIndex
        {
            get { return (int)(ViewState["idx"] ?? -1); }
            set { ViewState["idx"] = value; }
        }

        private IDynamicUserControlEntry FindEntryByToken(string token)
        {
            return DynamicUserControls.Find(
                delegate(IDynamicUserControlEntry entry)
                {
                    return entry.Token == token;
                });
        }

        private int GetIndexOfEntry(IDynamicUserControlEntry controlEntry)
        {
            return DynamicUserControls.IndexOf(controlEntry);
        }

        private IDynamicUserControlEntry GetEntryByIndex(int index)
        {
            return DynamicUserControls[index];
        }

    }

    /// <summary>
    /// Dynamic User Control Entries. Info Class for usage in the DynamicUserControl
    /// to define the various entries with their related name, token and usercontrolpath
    /// </summary>
    [Serializable]
    public class DynamicUserControlEntry : IDynamicUserControlEntry
    {
        private string _token;
        private string _userControlPath;
        public string Token { get { return _token; } set { _token = value; } }
        public string UserControlPath { get { return _userControlPath; } set { _userControlPath = value; } }
    }

    /// <summary>
    /// Base UserControl for controls that participate in the DynamicUserControl
    /// </summary>
    public class DynamicUserControlEntryBase : UserControl
    {
        private DynamicUserControl _cachedHost;
        protected DynamicUserControl GetHost()
        {
            return _cachedHost ?? (_cachedHost = FindFirstParent<DynamicUserControl>());
        }

        private T FindFirstParent<T>() where T : Control
        {
            return WebUtility.FindFirstParent<T>(Parent);
        }

        protected T GetInstanceOf<T>() where T : Control
        {
            return GetHost().GetInstanceOf<T>();
        }

        public void NavigateTo<T>() where T : UserControl
        {
            GetHost().DisplayUserControlByTypeAndReload<T>();
        }

        public virtual void ReceiveActiveView() { }
    }

    public interface IDynamicUserControlEntry
    {
        string Token { get; set;}
        string UserControlPath { get; set; }
    }

}