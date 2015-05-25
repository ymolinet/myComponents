namespace Gaia.WebWidgets.Samples.BasicControls.GridView.SelectRows
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using ASP = System.Web.UI.WebControls;
    using Gaia.WebWidgets;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;
    using Gaia.WebWidgets.Samples.Utilities.Extensions;

    public partial class Default : SamplePage
    {
        private const string RowSelectedCssClass = "row-selected";
        private readonly CalendarController _calendarController = new CalendarController(10);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            zGrid.DataSource = _calendarController.CalendarItems;
            zGrid.DataBind();
        }

        /// <summary>
        /// Add clickable to row for toggling checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowCreated(object sender, ASP.GridViewRowEventArgs e)
        {
            if (e.Row.RowType != ASP.DataControlRowType.DataRow) return;

            var row = (IAjaxContainerControl)e.Row;
            var clickAspect = new AspectClickable(ClickAspectClicked);
            row.Aspects.Add(clickAspect);
        }

        /// <summary>
        /// If clicked row with Ctrl hold down, we toggle checkbox
        /// </summary>
        private void ClickAspectClicked(object sender, AspectClickable.ClickEventArgs e)
        {
            var gridViewRow = ((AspectClickable)sender).ParentControl.Control as GridViewRow;
            if (gridViewRow == null) return;

            var checkBox = GetFirstCheckBoxInRow(gridViewRow);

            var isRowClick = e.CtrlKey;
            if (isRowClick)
            {
                var setChecked = !gridViewRow.CssClass.Contains(RowSelectedCssClass);
                checkBox.Checked = setChecked;
                UpdateRowHeaderAndStatus(checkBox, setChecked);
            }
            else
            {
                UpdateRowHeaderAndStatus(checkBox, checkBox.Checked);
            }
        }

        protected void cbxHeader_OnCheckedChanged(object sender, EventArgs e)
        {
            //find out if we will select/deselect
            var cbSelectedHeader = (CheckBox)sender;
            var setChecked = cbSelectedHeader.Checked;

            foreach (GridViewRow row in zGrid.Rows)
            {
                //get first instance of CheckBox in row
                var cbRow = GetFirstCheckBoxInRow(row);
                cbRow.Checked = setChecked;

                //select/deselect parent row
                SetSelectionParentRow(cbRow, setChecked);
            }

            SetStatus();
        }

        private void UpdateRowHeaderAndStatus(CheckBox checkBox, bool setChecked)
        {
            //update parent row for this checkbox
            SetSelectionParentRow(checkBox, setChecked);

            //update CheckBox in header with updated value
            UpdateHeaderCheckBox();

            SetStatus();
        }

        private void UpdateHeaderCheckBox()
        {
            var isAllChecked = IsAllChecked();
            zGrid.HeaderRow.Controls.All().OfType<CheckBox>().Single().Checked = isAllChecked;
        }

        private bool IsAllChecked()
        {
            return CountSelectedRows() == zGrid.Rows.Count;
        }

        private int CountSelectedRows()
        {
            return zGrid.Rows.Cast<GridViewRow>().Count(row => row.Controls.All().OfType<CheckBox>().Single().Checked);
        }

        private void SetSelectionParentRow(CheckBox cbx, bool setChecked)
        {
            GetGridViewRowFromChildren(cbx).CssClass = GridViewRowCssClass() + (setChecked
                                                                                    ? " " + RowSelectedCssClass
                                                                                    : null);
        }

        private string GridViewRowCssClass()
        {
            return zGrid.CssClass + "-row";
        }

        private static GridViewRow GetGridViewRowFromChildren(Control cbx)
        {
            return ((GridViewRow) cbx.Parent.Parent);
        }

        private static CheckBox GetFirstCheckBoxInRow(GridViewRow gridViewRow)
        {
            return gridViewRow.Controls.All().OfType<CheckBox>().First();
        }

        private void SetStatus()
        {
            var status = (IsAllChecked() ? "All " : "") + CountSelectedRows() + " checked";

            zStatus.Text = status;
            zStatus.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight());
        }      
    }
}
