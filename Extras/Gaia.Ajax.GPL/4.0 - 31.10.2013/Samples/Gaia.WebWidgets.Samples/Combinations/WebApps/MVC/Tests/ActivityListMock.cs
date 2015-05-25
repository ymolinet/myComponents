namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Tests
{
    using System.Collections.Generic;
    using Models;
    using Gaia.WebWidgets.Samples.Utilities;

    class ActivityListMock : IActivityList
    {
        private List<CalendarItem> _contacts;

        public List<CalendarItem> CalendarItems
        {
            get { return _contacts ?? (_contacts = new List<CalendarItem>()); }
        }

        public void View(IEnumerable<CalendarItem> data)
        {
            CalendarItems.Clear();
            CalendarItems.AddRange(data);
        }
    }
}