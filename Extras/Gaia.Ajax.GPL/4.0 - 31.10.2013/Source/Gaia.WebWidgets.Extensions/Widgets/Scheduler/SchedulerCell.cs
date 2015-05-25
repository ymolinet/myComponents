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
    using System.Web.UI;
    using System.Collections;
    using ASP = System.Web.UI.WebControls;

    public abstract class SchedulerCell : TableCell
    {    
    }

    public interface IDateContainer : INamingContainer
    {
        DateTime Value { get; }
    }

    public interface ITimeContainer : INamingContainer
    {
        TimeSpan Value { get; }
    }

    public interface IReportContainer: INamingContainer
    {
        IEnumerable Items { get; }
        DateTime Date { get; }
    }

    abstract class SchedulerDateTimeCell : SchedulerCell, IDataItemContainer
    {
        protected SchedulerDateTimeCell(int index, object date)
        {
            DataItem = date;
            DataItemIndex = index;
        }

        #region [-- IDataItemContainer implementation -- ]

        /// <summary>
        /// When implemented, gets an object that is used in simplified data-binding operations.
        /// </summary>
        /// <returns>
        /// An object that represents the value to use when data-binding operations are performed.
        /// </returns>
        public object DataItem { get; private set; }

        /// <summary>
        /// When implemented, gets the index of the data item bound to a control.
        /// </summary>
        /// <returns>
        /// An Integer representing the index of the data item in the data source.
        /// </returns>
        public int DataItemIndex { get; private set; }

        /// <summary>
        /// When implemented, gets the position of the data item as displayed in a control.
        /// </summary>
        /// <returns>
        /// An Integer representing the position of the data item as displayed in a control.
        /// </returns>
        public int DisplayIndex
        {
            get { return DataItemIndex; }
        }

        #endregion
    }

    class SchedulerDateCell : SchedulerDateTimeCell, IDateContainer 
    {
        private readonly DateTime _date;
        
        public SchedulerDateCell(int index, DateTime date) : base(index, date)
        {
            _date = date;
        }

        #region [-- ISchedulerHeaderCell Implementation --]

        DateTime IDateContainer.Value
        {
            get { return _date; }
        }

        #endregion
    }

    class SchedulerTimeCell : SchedulerDateTimeCell, ITimeContainer
    {
        private readonly TimeSpan _time;

        public SchedulerTimeCell(int index, TimeSpan time) : base(index, time)
        {
            _time = time;
        }

        #region [-- ISchedulerHeaderCell Implementation --]

        TimeSpan ITimeContainer.Value
        {
            get { return _time; }
        }

        #endregion
    }

    class SchedulerTaskCell : SchedulerCell { }
}
