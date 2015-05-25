namespace Gaia.WebWidgets.Samples.Extensions.InPlaceEdit.Matrix
{
    using System;
    using System.Drawing;
    using System.Linq;

    using UI;
    using Utilities;

    using Cell = System.Web.UI.WebControls.TableCell;
    using Row = System.Web.UI.WebControls.TableRow;
    using Table = System.Web.UI.WebControls.Table;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                MyMatrix = Helper.GenerateMatrixNumbers(MatrixSize);

            BuildMatrixTable();

            if (!IsPostBack)
                UpdateTotal();
        }

        private void BuildMatrixTable()
        {
            zWrap.Controls.Clear();
            
            var table = new Table {CssClass = "matrix"};

            foreach (var t in MyMatrix)
            {
                var row = new Row();

                foreach (var t1 in t)
                {
                    var cell = new Cell();

                    var edit = new WebWidgets.Extensions.InPlaceEdit {Blink = false, Text = t1.ToString(), CssClass = "editor"};

                    edit.TextChanged += 
                        delegate
                            {
                                edit.Effects.Add(new WebWidgets.Effects.EffectHighlight(Color.Green));
                                UpdateTotal();
                            };

                    cell.Controls.Add(edit);
                    row.Cells.Add(cell);
                }

                table.Rows.Add(row);
            }
            zWrap.Controls.Add(table);
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            var allEditControls = WebUtility.All(zWrap.Controls).OfType<WebWidgets.Extensions.InPlaceEdit>();
            
            foreach (var inPlaceEdit in allEditControls)
            {
                decimal number;
                var success = decimal.TryParse(inPlaceEdit.Text, out number);

                // 0 is set if conversion failed
                if (!success)
                    number = 0;

                // Numbers larger than 100 is rejected
                if (number >= 100)
                    number = 99;

                inPlaceEdit.Text = number + "";
                total += number;
            }
            zResult.Text = string.Format("Total: {0}", total);
        }

        protected void ZMatrixSizeSelectedIndexChanged(object sender, EventArgs e)
        {
            MyMatrix = Helper.GenerateMatrixNumbers(MatrixSize);
            BuildMatrixTable();
            zWrap.ForceAnUpdate();
            UpdateTotal();
        }

        private int[][] MyMatrix
        {
            get { return (int[][])Session["numbers"] ?? new int[0][]; }
            set { Session["numbers"] = value; }
        }

        private int MatrixSize
        {
            get { return int.Parse(zMatrixSize.SelectedValue); }
        }
    }
}
