namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Views
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using Models;

    public partial class ActivityFilterView : UserControl, IActivityFilter
    {
        public event EventHandler FilterChanged;

        private void InvokeSelectionChanged(EventArgs e)
        {
            EventHandler changed = FilterChanged;
            if (changed != null) changed(this, e);
        }

        protected void ViewChanged(object sender, EventArgs e)
        {
            InvokeSelectionChanged(EventArgs.Empty);
        }

        public string SelectedPerson
        {
            get { return zPersons.SelectedValue; }
        }

        public DateTime When
        {
            get { return zWhen.Value.Value; }
            set { zWhen.Value = value; }
        }

        void IActivityFilter.BindPersons(IEnumerable<string> persons)
        {
            foreach (string person in persons) zPersons.Items.Add(person);
        }
    }
}