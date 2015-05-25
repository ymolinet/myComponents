namespace Gaia.WebWidgets.Samples.Effects.EffectHighlight.Overview
{
    using System;
    using System.Drawing;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    using Cell = System.Web.UI.WebControls.TableCell;
    using Row = System.Web.UI.WebControls.TableRow;

    public partial class Overview : FxSamplePage
    {
        private const int size = 4;
        protected void Page_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < size; i++)
            {
                Row row = new Row();

                for (int j = 0; j < size; j++)
                {
                    Cell cell = new Cell();

                    Panel panel = new Panel();
                    panel.MouseOver +=
                        delegate
                        {
                            panel.Effects.Add(
                                new Gaia.WebWidgets.Effects.EffectHighlight
                                (
                                    WebUtility.GetRandomColor(), /*StartColor*/
                                    Color.White,      /*EndColor*/
                                    Color.White)      /*RestoreColor*/
                                );
                        };

                    cell.Controls.Add(panel);
                    row.Cells.Add(cell);
                }

                table.Rows.Add(row);
            }
        }
    }
}