namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Views
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using Models;
    using Gaia.WebWidgets.Samples.Utilities;
    using ASP = System.Web.UI.WebControls;
    
    public partial class ActivityListListBox : UserControl, IActivityList
    {
        void IActivityList.View(IEnumerable<CalendarItem> data)
        {            
            zList.Items.Clear();
            foreach (CalendarItem calendarItem in data)
            {
                ASP.ListItem item = new ASP.ListItem();
                item.Value = calendarItem.Id.ToString();
                item.Text = calendarItem.ContactPerson + ":" + calendarItem.ActivityName;
                zList.Items.Add(item);
            }
        }

        protected void zList_SelectedIndexChanged(object sender, EventArgs e)
        {
            zWindow.Caption = zList.SelectedValue + " - " + zList.SelectedItem.Text; 
            zWindow.Visible = true;

        }
    }
}