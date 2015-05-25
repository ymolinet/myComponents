namespace Gaia.WebWidgets.Samples.Extensions.Scheduler.Overview
{
    using System;

    public class Task
    {
        public string ID { get; set; }
        
        public string Owner { get; set; }
        
        public string Title { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
    }
}