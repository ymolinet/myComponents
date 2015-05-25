namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Views
{
    using System.Web.UI;
    using System.Collections.Generic;
    using Models;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class ActivityListRepeater : UserControl, IActivityList
    {
        void IActivityList.View(IEnumerable<CalendarItem> data)
        {
            zView.DataSource = data;
            zView.DataBind();
        }
    }
}