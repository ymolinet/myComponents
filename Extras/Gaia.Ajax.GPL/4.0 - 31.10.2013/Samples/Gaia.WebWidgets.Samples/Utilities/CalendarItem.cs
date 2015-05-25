namespace Gaia.WebWidgets.Samples.Utilities
{
    using System;

    public class CalendarItem
    {
        public CalendarItem(string name, DateTime date, string person, int id, int status, double percentCompleted, double totalTaskCost)
        {
            ActivityName = name;
            ActivityDate = date;
            ContactPerson = person;
            Id = id;
            Status = status;
            PercentCompleted = percentCompleted;
            TotalTaskCost = totalTaskCost;
        }

        public bool IsDeleted { get; set; }

        public bool IsChecked { get; set; }

        public int Status { get; set; }

        public int Id { get; set; }

        public double PercentCompleted { get; set; }

        public double TotalTaskCost { get; set; }

        public string ContactPerson { get; set; }

        public DateTime ActivityDate { get; set; }

        public string ActivityName { get; set; }
    }
}