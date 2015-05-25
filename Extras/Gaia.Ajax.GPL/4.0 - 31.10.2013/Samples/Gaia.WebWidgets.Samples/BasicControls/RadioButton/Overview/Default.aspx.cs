namespace Gaia.WebWidgets.Samples.BasicControls.RadioButton.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;


    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void zButtonConfirm_Click(object sender, EventArgs e)
        {
            if (zRadioButton1.Checked)
                zLabelContinent.Text = "You live in " + zRadioButton1.Text;
            else if (zRadioButton2.Checked)
                zLabelContinent.Text = "You live in " + zRadioButton2.Text;
            else if (zRadioButton3.Checked)
                zLabelContinent.Text = "You live in " + zRadioButton3.Text;
            else if (zRadioButton4.Checked)
                zLabelContinent.Text = "You live in " + zRadioButton4.Text;
            else if (zRadioButton5.Checked)
                zLabelContinent.Text = "You live in " + zRadioButton5.Text;

            //fire effect
            zLabelContinent.Effects.Add(new Gaia.WebWidgets.Effects.EffectHighlight());
        }
    }
}
