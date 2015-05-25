namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Data
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Gaia.WebWidgets.Samples.Utilities;

    /// <summary>
    /// Simulated DAL for Gaia.WebWidgets.Samples
    /// In a real world scenario this is can be your 
    /// a) DB components
    /// b) ORM layer
    /// c) Something else which provides data storage
    /// </summary>
    public static class ActivityDataLayer
    {
        public static List<CalendarItem> GetByDate(DateTime date)
        {
            return CalendarItemsData.FindAll(delegate(CalendarItem index)
                                                 {
                                                     return index.ActivityDate.Day == date.Day &&
                                                            index.ActivityDate.Month == date.Month;
                                                 });
        }

        public static List<CalendarItem> GetByDateAndPerson(DateTime date, string contactPerson)
        {
            return GetByDate(date).FindAll(delegate(CalendarItem idx) { return idx.ContactPerson == contactPerson; });
        }


        public static IEnumerable<string> GetContacts()
        {
            List<string> people = new List<string>();
            foreach (CalendarItem calendarItem in CalendarItemsData)
            {
                if (people.Contains(calendarItem.ContactPerson)) continue;
                people.Add(calendarItem.ContactPerson);
            }

            return people.ToArray();
        }

        /// <summary>
        /// In this property we "simulate" the database by creating a list of 100 tasks that are split across 5 people. 
        /// </summary>
        private static List<CalendarItem> CalendarItemsData
        {
            get
            {
                if (HttpContext.Current.Session["DataGridItems"] == null)
                    HttpContext.Current.Session["DataGridItems"] = CalendarController.CreateItems(100, 5);

                return HttpContext.Current.Session["DataGridItems"] as List<CalendarItem>;
            }
        }
    }
}