namespace Gaia.WebWidgets.Samples.Extensions.Scheduler.Overview
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Gaia.WebWidgets.Samples.UI;

    public class DataSamplePage : SamplePage
    {
        protected IList<Task> Tasks
        {
            get { return Session["myTasks"] as IList<Task> ?? (IList<Task>)(Session["myTasks"] = new List<Task>()); }
        }

        protected virtual void LoadSampleData()
        {
            Tasks.Clear();
            foreach (var defaultTask in InitializeDefaultTasks())
                Tasks.Add(defaultTask);
        }

        protected Task FindTaskByID(string id)
        {
            return Tasks.Single(task => task.ID == id);
        }

        protected char NextID()
        {
            var nextId = Tasks.Any() ? Tasks.Select(task => task.ID[0]).Max() : 'a';
            return ++nextId;
        }

        protected const string Person1 = "David Lorimer";
        protected const string Person2 = "Barbie Diaz";
        protected const string Person3 = "Bill Olsen";

        protected const string TaskName1 = "Code";
        protected const string TaskName2 = "Meeting";
        protected const string TaskName3 = "QA";

        IEnumerable<Task> InitializeDefaultTasks()
        {

            var startDate = DateTime.Now.Date;
            char id = 'a';

            yield return new Task
                             {
                                 StartTime = startDate.AddHours(8),
                                 EndTime = startDate.AddHours(12),
                                 ID = (id++).ToString(CultureInfo.InvariantCulture),
                                 Owner = Person1,
                                 Title = TaskName1
                             };

            yield return new Task
                             {
                                 StartTime = startDate.AddHours(12),
                                 EndTime = startDate.AddHours(16),
                                 ID = (id++).ToString(CultureInfo.InvariantCulture),
                                 Owner = Person1,
                                 Title = TaskName3
                             };


            yield return new Task
                             {
                                 StartTime = startDate.AddHours(9),
                                 EndTime = startDate.AddHours(11),
                                 ID = (id++).ToString(CultureInfo.InvariantCulture),
                                 Owner = Person2,
                                 Title = TaskName1
                             };

            yield return new Task
                             {
                                 StartTime = startDate.AddHours(12),
                                 EndTime = startDate.AddHours(15),
                                 ID = (id++).ToString(CultureInfo.InvariantCulture),
                                 Owner = Person2,
                                 Title = TaskName2
                             };

            var tomorrow = startDate.AddDays(1);

            yield return new Task
                             {
                                 StartTime = tomorrow.AddHours(8),
                                 EndTime = tomorrow.AddHours(16),
                                 ID = (id++).ToString(CultureInfo.InvariantCulture),
                                 Owner = Person3,
                                 Title = TaskName1
                             };

            yield return new Task
                             {
                                 StartTime = tomorrow.AddHours(9),
                                 EndTime = tomorrow.AddHours(15),
                                 ID = (id++).ToString(CultureInfo.InvariantCulture),
                                 Owner = Person2,
                                 Title = TaskName1
                             };

            tomorrow = tomorrow.AddDays(1);

            yield return new Task
                             {
                                 StartTime = tomorrow.AddHours(7),
                                 EndTime = tomorrow.AddHours(10),
                                 ID = (id++).ToString(CultureInfo.InvariantCulture),
                                 Owner = Person2,
                                 Title = TaskName1
                             };

            yield return new Task
                             {
                                 StartTime = tomorrow.AddHours(8),
                                 EndTime = tomorrow.AddHours(11),
                                 ID = (id++).ToString(CultureInfo.InvariantCulture),
                                 Owner = Person3,
                                 Title = TaskName3
                             };

            yield return new Task
                             {
                                 StartTime = tomorrow.AddHours(8),
                                 EndTime = tomorrow.AddHours(16),
                                 ID = (id++).ToString(CultureInfo.InvariantCulture),
                                 Owner = Person1,
                                 Title = TaskName2
                             };

        }



    }
}