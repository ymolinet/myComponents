namespace Gaia.WebWidgets.Samples.BasicControls.ListBox.OrderedListBox
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        #region Code
        protected void zUp_Click(object sender, EventArgs e)
        {
            if (zList.SelectedIndex == -1 || zList.SelectedIndex <= 0)
                return;

            int oldIndex = zList.SelectedIndex;
            var itemAbove = zList.Items[zList.SelectedIndex - 1];

            zList.Items.Remove(itemAbove);
            zList.Items.Insert(oldIndex, itemAbove);
            zList.SelectedIndex = oldIndex - 1;

            VerifyWinner();
        }

        protected void zDown_Click(object sender, EventArgs e)
        {
            if (zList.SelectedIndex == -1 ||
                zList.SelectedIndex == zList.Items.Count - 1)
                return;

            int oldIndex = zList.SelectedIndex;
            var itemBelow = zList.Items[zList.SelectedIndex + 1];

            zList.Items.Remove(itemBelow);
            zList.Items.Insert(oldIndex, itemBelow);
            zList.SelectedIndex = oldIndex + 1;

            VerifyWinner();
        }

        void VerifyWinner()
        {
            if (zList.Items[0].Text == "Gaiaware")
                zWindow.Visible = true;
        } 
        #endregion
    }
}
