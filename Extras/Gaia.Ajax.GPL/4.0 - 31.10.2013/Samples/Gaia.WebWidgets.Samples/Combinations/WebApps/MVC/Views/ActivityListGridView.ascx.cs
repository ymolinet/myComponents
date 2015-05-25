namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Views
{
    using System.Web.UI;
    using System.Collections.Generic;
    using Models;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class ActivityListGridView : UserControl, IActivityList
    {
        void IActivityList.View(IEnumerable<CalendarItem> data)
        {
            zGrid.DataSource = data;
            zGrid.DataBind();
        }

    }
}