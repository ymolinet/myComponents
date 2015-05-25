namespace Gaia.WebWidgets.Samples.BasicControls.GridView.RichInlineEditing
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;

    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    public partial class Default : SamplePage
    {
        private readonly CalendarController _calendarController = new CalendarController(10);

        protected void Page_Load(object sender, EventArgs e)
        {
            Manager.Instance.UpdateControl = updateControl;
        }

        /// <summary>
        /// Updates the Status Label and the DataSource when the user changes Activity Name
        /// </summary>
        protected void ActivityNameTextChanged(object sender, EventArgs e)
        {
            var inPlaceEdit = sender as Gaia.WebWidgets.Extensions.InPlaceEdit;
            int currentRowIndex = GetCurrentRowIndex(inPlaceEdit);

            var calendarItems = _calendarController.CalendarItems;

            status.Text = string.Format("Item No. {0} changed ActivityName from '{1}' to '{2}'",
                                        currentRowIndex,
                                        calendarItems[currentRowIndex].ActivityName,
                                        inPlaceEdit.Text);
            FlashStatusField();
            calendarItems[currentRowIndex].ActivityName = inPlaceEdit.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable ListAsDataTable()
        {
            return _calendarController.ListAsDataTable();
        }

        /// <summary>
        /// Updates the Status Label and the DataSource when the user changes Contact Name
        /// </summary>
        protected void ContactPersonTextChanged(object sender, EventArgs e)
        {
            var edit = sender as Gaia.WebWidgets.Extensions.InPlaceEdit;
            int currentRowIndex = GetCurrentRowIndex(edit);

            var calendarItems = _calendarController.CalendarItems;

            status.Text = string.Format("Item No. {0} changed ContactPerson from '{1}' to '{2}'",
                                        currentRowIndex,
                                        calendarItems[currentRowIndex].ContactPerson,
                                        edit.Text);

            FlashStatusField();

            calendarItems[currentRowIndex].ContactPerson = edit.Text;
        }

        /// <summary>
        /// Updates the Status Label and the DataSource when the user changes Activity Status
        /// </summary>
        protected void StatusChanged(object sender, EventArgs e)
        {
            var radio = sender as Gaia.WebWidgets.RadioButtonList;
            int currentRowIndex = GetCurrentRowIndex(radio);

            var calendarItems = _calendarController.CalendarItems;

            status.Text = string.Format("Item No. {0} changed Status from '{1}' to '{2}'",
                                        currentRowIndex,
                                        calendarItems[currentRowIndex].Status,
                                        radio.SelectedItem.Value);

            FlashStatusField();

            calendarItems[currentRowIndex].Status = Convert.ToInt32(radio.SelectedItem.Value);
        }

        protected void IsCheckedChanged(object sender, EventArgs e)
        {
            var check = sender as Gaia.WebWidgets.CheckBox;
            int currentRowIndex = GetCurrentRowIndex(check);

            List<CalendarItem> data = _calendarController.CalendarItems;

            status.Text = string.Format("Item No. {0} changed IsChecked from '{1}' to '{2}'",
                                        currentRowIndex,
                                        data[currentRowIndex].IsChecked,
                                        check.Checked);

            FlashStatusField();

            data[currentRowIndex].IsChecked = check.Checked;
        }

        /// <summary>
        /// Updates the Status Label and the DataSource when the user changes Activity Date
        /// </summary>
        protected void DateChanged(object sender, EventArgs e)
        {
            var date = sender as Gaia.WebWidgets.Extensions.DateTimePicker;
            int currentRowIndex = GetCurrentRowIndex(date);

            var calendarItems = _calendarController.CalendarItems;

            status.Text = string.Format("Item No. {0} changed Date from '{1}' to '{2}'",
                                        currentRowIndex,
                                        calendarItems[currentRowIndex].ActivityDate.ToString("dd.MMM yy", null),
                                        date.Value.Value.ToString("dd.MMM yy", null));

            FlashStatusField();

            calendarItems[currentRowIndex].ActivityDate = date.Value.Value;
        }

        private static int GetCurrentRowIndex(Control control)
        {
            return ((GridViewRow)control.Parent.Parent).RowIndex;
        }

        /// <summary>
        /// Highlights the Status Label
        /// </summary>
        private void FlashStatusField()
        {
            status.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight());
        }
    }
}