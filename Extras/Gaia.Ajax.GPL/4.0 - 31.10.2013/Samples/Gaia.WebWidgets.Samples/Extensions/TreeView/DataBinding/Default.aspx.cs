namespace Gaia.WebWidgets.Samples.Extensions.TreeView.DataBinding
{
    using System;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        private readonly CalendarController _calendarController = new CalendarController(25);

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void zTreeRoot_OnGetChildrenControls(object sender, Gaia.WebWidgets.Extensions.TreeViewItem.GetChildrenControlsEventArgs e)
        {
            _calendarController.SortByActivityDate(true);

            foreach (CalendarItem calendarItem in _calendarController.CalendarItems)
                e.Node.TreeViewItems.Add(CreateItem(calendarItem));
        }

        private Gaia.WebWidgets.Extensions.TreeViewItem CreateItem(CalendarItem item)
        {
            var newItem = new Gaia.WebWidgets.Extensions.TreeViewItem();
            newItem.CssClass = StyleSheetTheme;
            newItem.IconCssClass = "noicon";
            newItem.IsLeaf = true;
            newItem.ChildControls.Add(CreateLinkButton(item));
            return newItem;
        }

        private LinkButton CreateLinkButton(CalendarItem item)
        {
            var btn = new LinkButton();
            btn.ID = item.Id.ToString();
            btn.Text = item.ActivityDate + ": " + item.ActivityName;
            btn.Click += btn_Click;
            return btn;
        }

        void btn_Click(object sender, EventArgs e)
        {
            var linkButton = sender as LinkButton;

            if (linkButton == null) return;

            var selectedItem = _calendarController.GetById(int.Parse(linkButton.ID));

            if (selectedItem == null) return;

            zActivityDate.Text = selectedItem.ActivityDate.ToString();
            zActivityName.Text = selectedItem.ActivityName;
            zMoreInfo.Text = string.Format("Contact person:  {0}<br />Confirmed? {1}<br />Deleted? {2}",
                                           selectedItem.ContactPerson,
                                           selectedItem.IsChecked ? "Yes" : "No",
                                           selectedItem.IsDeleted ? "Yes" : "No");
        }
    }
}
