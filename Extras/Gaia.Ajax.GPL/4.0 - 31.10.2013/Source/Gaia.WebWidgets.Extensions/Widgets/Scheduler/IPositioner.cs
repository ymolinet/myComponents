/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2009 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets.Extensions
{
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    
    interface IPositioner
    {
        TaskPosition GetTaskPosition(SchedulerTask task);
        void DistributeTasks(ICollection<SchedulerTask> tasks);
    }

    sealed class TaskPosition
    {
        public int? ZIndex { get; internal set; }
        public Unit? Left { get; internal set; }
        public Unit? Width { get; internal set; }
    }
}