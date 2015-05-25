namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Models
{
    using System.Collections.Generic;
    using Gaia.WebWidgets.Samples.Utilities;

    /// <summary>
    /// Model that allows binding of results to a View
    /// </summary>
    public interface IActivityList
    {
        void View(IEnumerable<CalendarItem> data);
    }
}