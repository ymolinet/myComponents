namespace Gaia.WebWidgets.Samples.Utilities
{
    using System;
    using System.Web;
    using System.Linq;
    using System.Data;
    using System.Collections.Generic;
    
    /// <summary>
    /// Summary description for CalendarController
    /// </summary>
    public class CalendarController
    {
        private const string ConstDataDridItemsName = "DataGridItems";
        private readonly int _recordCount;

        public CalendarController() : this(25) { }

        public CalendarController(int numberOfRecords)
        {
            Filter = string.Empty;
            _recordCount = numberOfRecords;
        }

        public string Filter { get; set; }

        private List<CalendarItem> CalendarItemsData
        {
            get
            {
                if (HttpContext.Current.Session[ConstDataDridItemsName] == null)
                    HttpContext.Current.Session[ConstDataDridItemsName] = CreatItems(_recordCount);

                return HttpContext.Current.Session[ConstDataDridItemsName] as List<CalendarItem>;
            }
        }

        public void Add(CalendarItem item)
        {
            CalendarItemsData.Add(item);
        }

        public List<CalendarItem> List(int startRow, int pageSize)
        {
            return CalendarItems.GetRange(startRow, pageSize);
        }

        public List<CalendarItem> List()
        {
            return CalendarItems;
        }

        public DataTable ListAsDataTable()
        {
            return GenericsUtility.ToDataTable(List());
        }

        public List<CalendarItem> CalendarItems
        {
            get
            {
                return CalendarItemsData.FindAll(delegate(CalendarItem item)
                                                     {
                                                         var hasFilter = !string.IsNullOrEmpty(Filter) &&
                                                                          Filter.ToLower() != "search";

                                                         if (hasFilter)
                                                         {
                                                             return
                                                                 (item.ActivityName.ToLower().IndexOf(Filter.ToLower()) !=
                                                                  -1 ||
                                                                  item.ContactPerson.ToLower().IndexOf(Filter.ToLower()) !=
                                                                  -1) &&
                                                                 !item.IsDeleted;
                                                         }

                                                         return !item.IsDeleted;
                                                     });
            }
        }

        public int GetCount()
        {
            return CalendarItems.Count;
        }

        public int GetNextId()
        {
            var maxId = CalendarItemsData.Aggregate(0, (current, calendarItem) => Math.Max(current, calendarItem.Id));

            return ++maxId;
        }

        public List<CalendarItem> GetDeletedItems()
        {
            return CalendarItemsData.Where(c => c.IsDeleted).ToList();
        }

        public List<CalendarItem> GetByDate(DateTime date)
        {
            return CalendarItems.Where(c => c.ActivityDate.Day == date.Day &&
                                            c.ActivityDate.Month == date.Month &&
                                            c.ActivityDate.Year == date.Year).ToList();
        }

        public CalendarItem GetById(int id)
        {
            return CalendarItemsData.Find(index => index.Id == id);
        }

        private void SetDeleteStatus(int id, bool deleted)
        {
            var found = GetById(id);

            if (found != null)
                found.IsDeleted = deleted;
        }

        public void Delete(int id)
        {
            SetDeleteStatus(id, true);
        }
        
        public void DeleteByCalendarItem(CalendarItem item)
        {
            Delete(item.Id);
        }

        public void UnDelete(int id)
        {
            SetDeleteStatus(id, false);
        }

        public void Update(int id, string contactPerson, string activityName, bool isChecked)
        {
            var item = GetById(id);
            item.ContactPerson = contactPerson;
            item.ActivityName = activityName;
            item.IsChecked = isChecked;
        }

        public void SortByActivityName(bool ascending)
        {
            CalendarItemsData.Sort(
                (left, right) => ascending
                                     ? left.ActivityName.CompareTo(right.ActivityName)
                                     : right.ActivityName.CompareTo(left.ActivityName));
        }

        public void SortByActivityDate(bool ascending)
        {
            CalendarItemsData.Sort(
                (left, right) => ascending
                                     ? left.ActivityDate.CompareTo(right.ActivityDate)
                                     : right.ActivityDate.CompareTo(left.ActivityDate));
        }

        public void SortByContactPerson(bool ascending)
        {
            CalendarItemsData.Sort(
                (left, right) => ascending
                                     ? left.ContactPerson.CompareTo(right.ContactPerson)
                                     : right.ContactPerson.CompareTo(left.ContactPerson));
        }

        public void Sort(bool ascending, string sortBy)
        {
            switch (sortBy.ToLower())
            {
                case "task":
                    SortByActivityName(ascending);
                    break;

                case "due":
                    SortByActivityDate(ascending);
                    break;

                case "who":
                    SortByContactPerson(ascending);
                    break;

                case "done":
                    SortByStatus(ascending);
                    break;

                case "is difficult":
                    SortByChecked(ascending);
                    break;
            }
        }

        public void SortByStatus(bool ascending)
        {
            CalendarItemsData.Sort(
                (left, right) => ascending ? left.Status.CompareTo(right.Status) : right.Status.CompareTo(left.Status));
        }

        public void SortByChecked(bool ascending)
        {
            CalendarItemsData.Sort(
                (left, right) => ascending
                                     ? left.IsChecked.CompareTo(right.IsChecked)
                                     : right.IsChecked.CompareTo(left.IsChecked));

        }

        public static void Reset()
        {
            HttpContext.Current.Session[ConstDataDridItemsName] = null;
        }

        private static DateTime GetRandomDateTime()
        {
            return DateTime.Now.AddDays(Rnd.Next(-3, 3)).AddHours(Rnd.Next(-8, 8)).AddMinutes(Rnd.Next(-60, 60));
        }

        private static readonly Random Rnd = new Random();

        public static List<CalendarItem> CreatItems(int count)
        {
            return CreateItems(count, Math.Max(count, 3) / 3);
        }

        public static List<CalendarItem> CreateItems(int count, int personCount)
        {
            var firstnames = new[] { "Tom", "Erik", "Bill", "Gary", "Lucy", "Cindy", "Jenna", "Joel", "Scott", "Myron", "Nina", "Isabelle", "Bob" };
            var lastnames = new[] { "Wilkins", "Farraday", "Nielsen", "Olsen", "Meyers", "Croft", "Anderson", "Fitzgerald", "Diaz", "Hopkins", "Scholes" };

            //int personCount = Math.Max(count, 3) / 3;
            var names = new string[personCount];
            for (var i = 0; i < personCount; i++)
                names[i] = firstnames[Rnd.Next(0, firstnames.Length)] + " " +
                           lastnames[Rnd.Next(0, lastnames.Length)];

            var tasks = new[]
                            {
                                "Code Review", "Attend Seminar", "Fix Bugs", "Refactor Code", "Deliver speech",
                                "Morning meeting",
                                "Interview", "Business lunch", "Staff meeting", "Design Architecture",
                                "Create Unit Tests", "Write Blog", "Customer meeting",
                                "Development", "Development", "Development", "Development", "Quality Assurance",
                                "Read newspapers", "Sales Presentation"
                            };

            var id = 0;
            var tmp = new List<CalendarItem>();

            for (var i = 0; i < count; i++)
                tmp.Add(new CalendarItem(tasks[Rnd.Next(0, tasks.Length)], GetRandomDateTime(),
                                         names[Rnd.Next(0, names.Length)], ++id, Rnd.Next(1, 2),
                                         Rnd.NextDouble(),
                                         Rnd.NextDouble()*Rnd.Next(0, 100000)));

            return tmp;
        }

    }
}