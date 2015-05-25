/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2011 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets.Extensions
{
    using System;

    [Serializable]
    sealed class SchedulerTask
    {
        public int Order { get; private set; }
        public string ID { get; private set; }
        public DateTime EndTime { get; private set; }
        public DateTime StartTime { get; private set; }
        public object DataSource { get; private set; }

        public bool OverlapsWith(SchedulerTask task)
        {
            return StartTime >= task.StartTime && StartTime < task.EndTime;
        }

        public static SchedulerTask Create(int index, string id, DateTime start, DateTime end, object dataSource)
        {
            return new SchedulerTask {Order = index, ID = id, StartTime = start, EndTime = end, DataSource = dataSource};
        }
    }
}
