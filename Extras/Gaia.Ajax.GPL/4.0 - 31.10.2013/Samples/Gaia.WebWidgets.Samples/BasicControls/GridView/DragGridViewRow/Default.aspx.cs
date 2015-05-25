namespace Gaia.WebWidgets.Samples.BasicControls.GridView.DragGridViewRow
{
    using Utilities;

    using System;
    using System.Linq;
    using System.Web.UI.WebControls;
    
    public partial class Default : System.Web.UI.Page
    {
        private readonly CalendarController _calendarController = new CalendarController(10);

        protected void Page_Load(object sender, EventArgs e)
        {
            zDropArea.Aspects.Add(new AspectDroppable(Dropped) {HoverClass = "hover-drop"});

            if (IsPostBack) return;

            BindData();
        }

        private void BindData()
        {
            zGv.DataSource = _calendarController.CalendarItems;
            zGv.DataBind();
        }

        protected void RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            
            var row = (IAjaxContainerControl) e.Row;
            var dragAspect = new AspectDraggable
                                 {
                                     DeepCopy = true,
                                     MakeGhost = true,
                                     DragCssClass = "container-drag",
                                     IdToPass = e.Row.RowIndex.ToString()
                                 };
            row.Aspects.Add(dragAspect);
        }

        private void Dropped(object sender, AspectDroppable.DroppedEventArgs e)
        {
            var dataItem = _calendarController.CalendarItems.ElementAt(int.Parse(e.IdToPass));
            if (dataItem == null) return;

            _calendarController.DeleteByCalendarItem(dataItem);
            BindData();

            zLbl.Text = string.Format("{0} was removed from list", dataItem.ContactPerson);
        }
    }
}