namespace Gaia.WebWidgets.Samples.Combinations.WebUtilities.AsyncLoading
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Gaia.WebWidgets.Samples.Utilities;

    public class CustomBackgroundWorker
    {
        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public void RunTask()
        {
            lock (this)
            {
                if (_isRunning)
                    throw new InvalidOperationException("The task is already running!");

                _isRunning = true;
                new Thread(DoWork).Start();
            }
        }

        private ICollection<CalendarItem> _data;
        public ICollection<CalendarItem> Data
        {
            get { return _data ?? (_data = new List<CalendarItem>()); }
        }

        void DoWork()
        {
            try
            {
                for (int i = 0; i < 15; i++)
                {
                    lock (Data)
                        foreach (CalendarItem item in CalendarController.CreatItems(new Random().Next(1, 3)))
                            Data.Add(item);

                    Thread.Sleep(new Random().Next(300, 2000)); // Random Sleep to simulate variance
                }
            }
            catch  { /* Task failed (suppress exceptions in demo) */ }
            finally { _isRunning = false; }
        }
    }
}
