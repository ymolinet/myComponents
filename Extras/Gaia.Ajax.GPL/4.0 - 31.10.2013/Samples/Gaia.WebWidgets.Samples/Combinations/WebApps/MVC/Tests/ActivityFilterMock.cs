namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Tests
{
    using System;
    using System.Collections.Generic;
    using Models;

    class ActivityFilterMock : IActivityFilter
    {
        private DateTime _when;
        private List<string> _contacts;

        public List<string> Contacts
        {
            get { return _contacts ?? (_contacts = new List<string>()); }
        }

        public void BindPersons(IEnumerable<string> persons)
        {
            Contacts.AddRange(persons);
        }

        // mock always returns first item in the list
        public string SelectedPerson
        {
            get { return Contacts.Count > 0 ? Contacts[0] : string.Empty; }
        }

        public DateTime When
        {
            get { return _when; }
            set { _when = value; }
        }
    }
}