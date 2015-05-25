namespace Gaia.WebWidgets.Samples.Extensions.Scheduler.MonthView
{
    using System;

    class Resource
    {
        public string Name { get; set; }
        public string CssClass { get; set; }
    }

    public class AssignedResource
    {
        public string ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CssClass { get; set; }
        public string Name { get; set; }
    }
}