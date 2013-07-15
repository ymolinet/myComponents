using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
using System.Collections;

namespace myComponents.Components.Web.Grids
{
    public class aGrid : Gaia.WebWidgets.GridView
    {
        // Fields
        protected DataControlField[] _fields;
        protected GridViewRow _footerRow2;
        protected GridViewRow _headerRow2;
        private IContainer components;

        // Methods
        public aGrid()
        {
            this.components = null;
            this.InitializeComponent();
            this.PrepareGrid();
        }

        public aGrid(IContainer container)
        {
            this.components = null;
            container.Add(this);
            this.InitializeComponent();
            this.PrepareGrid();
        }

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            int numrows = base.CreateChildControls(dataSource, dataBinding);
            if ((numrows == 0) && this.ShowWhenEmpty)
            {
                this.CreateEmptyRows();
            }
            if ((dataSource != null) && (dataSource.GetType() == typeof(DataView)))
            {
                this.ViewState["myGrid_TotalRowsCount"] = ((DataView)dataSource).Count;
                this.ViewState["myGrid_DataTable"] = ((DataView)dataSource).ToTable();
            }
            return numrows;
        }

        private void CreateEmptyRows()
        {
            if (this.Controls.Count == 0)
            {
                this.Controls.Add(new Table());
            }
            else
            {
                this.Controls[0].Controls.Clear();
            }
            DataControlField[] fields = new DataControlField[this.Columns.Count];
            this.Columns.CopyTo(fields, 0);
            if (this.ShowHeader)
            {
                this._headerRow2 = base.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
                this.InitializeRow(this._headerRow2, fields);
                this.Controls[0].Controls.Add(this._headerRow2);
            }
            GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
            TableCell cell = new TableCell();
            cell.ColumnSpan = this.Columns.Count;
            cell.Width = Unit.Percentage(100.0);
            if (!string.IsNullOrEmpty(this.EmptyDataText))
            {
                cell.Controls.Add(new LiteralControl(this.EmptyDataText));
            }
            if (this.EmptyDataTemplate != null)
            {
                this.EmptyDataTemplate.InstantiateIn(cell);
            }
            emptyRow.Cells.Add(cell);
            this.Controls[0].Controls.Add(emptyRow);
            if (this.ShowFooter)
            {
                this._footerRow2 = base.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);
                this.InitializeRow(this._footerRow2, fields);
                this.Controls[0].Controls.Add(this._footerRow2);
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private void PrepareGrid()
        {
            this._fields = new DataControlField[this.Columns.Count];
            this.Columns.CopyTo(this._fields, 0);
        }

        // Properties
        public DataTable CurrentRecords
        {
            get
            {
                return (DataTable)this.ViewState["myGrid_DataTable"];
            }
        }

        public override GridViewRow FooterRow
        {
            get
            {
                GridViewRow f = base.FooterRow;
                if (f != null)
                {
                    return f;
                }
                return this._footerRow2;
            }
        }

        public override GridViewRow HeaderRow
        {
            get
            {
                GridViewRow h = base.HeaderRow;
                if (h != null)
                {
                    return h;
                }
                return this._headerRow2;
            }
        }

        [Description("Set True/False to show the datagrid if empty datasource is provided"), DefaultValue(false), Category("Extended GridView Properties"), Bindable(true)]
        public bool ShowWhenEmpty
        {
            get
            {
                if (this.ViewState["ShowWhenEmpty"] == null)
                {
                    this.ViewState["ShowWhenEmpty"] = false;
                }
                return (bool)this.ViewState["ShowWhenEmpty"];
            }
            set
            {
                this.ViewState["ShowWhenEmpty"] = value;
            }
        }

        public int TotalRowsCount
        {
            get
            {
                if (this.ViewState["myGrid_TotalRowsCount"] != null)
                {
                    return Convert.ToInt32(this.ViewState["myGrid_TotalRowsCount"]);
                }
                return 0;
            }
        }
    }
}
