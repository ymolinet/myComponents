namespace Gaia.WebWidgets.Samples.Extensions.ExtendedPanel.Overview
{
    using System;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddButtonsDynamically();
        }

        protected void zToggle_Click(object sender, EventArgs e)
        {
            zExtendedPanel1.Toggle();
        }

        protected void zCanBeToggled_OnCheckedChanged(object sender, EventArgs e)
        {
            zExtendedPanel1.CanBeToggled = zCanBeToggled.Checked;
        }

        private void AddButtonsDynamically()
        {
            for (int i = 1; i < 6; i++)
                zExtendedPanel1.Controls.Add(ButtonFactory(i));
        }

        private Button ButtonFactory(int id)
        {
            Button button = new Button();
            button.ID = "button" + id;
            button.Text = "Button " + id;
            button.Width = new System.Web.UI.WebControls.Unit(120);

            //update caption of ExtendedPanel when clicking the button
            button.Click += delegate { zExtendedPanel1.Caption = "You clicked button " + id; };
            return button;
        }

    }
}