namespace Gaia.WebWidgets.Samples.BasicControls.Panel.Overview
{
    using System;
    using System.Drawing;
    using ASP = System.Web.UI.WebControls;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void zRed_ValueChanged(object sender, EventArgs e)
        {
            zLabelRed.Text = RedValue.ToString();
            SetPanelBackgroundColor();
        }

        protected void zGreen_ValueChanged(object sender, EventArgs e)
        {
            zLabelGreen.Text = GreenValue.ToString();
            SetPanelBackgroundColor();
        }

        protected void zBlue_ValueChanged(object sender, EventArgs e)
        {
            zLabelBlue.Text = BlueValue.ToString();
            SetPanelBackgroundColor();
        }

        private void SetPanelBackgroundColor()
        {
            zPanel.BackColor = Color.FromArgb(RedValue, GreenValue, BlueValue);
        }

        private static int ConvertToRGB(double sliderFactor)
        {
            return Convert.ToInt32(Math.Round(sliderFactor * 2.55, 0));
        }

        public int RedValue
        {
            get { return ConvertToRGB(zRed.Value); }
        }

        public int GreenValue
        {
            get { return ConvertToRGB(zGreen.Value); }
        }

        public int BlueValue
        {
            get { return ConvertToRGB(zBlue.Value); }
        }

        protected void zBorderStyle_OnInit(object sender, EventArgs e)
        {
            zBorderStyle.DataSource = Enum.GetNames(typeof (ASP.BorderStyle));
            zBorderStyle.DataBind();

            zBorderStyle.SelectedIndex = -1;

            ASP.ListItem selectedItem = zBorderStyle.Items.FindByValue(zPanel.BorderStyle.ToString());
            if (selectedItem != null)
                selectedItem.Selected = true;
        }

        protected void zBorderStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            zPanel.BorderStyle = (ASP.BorderStyle) Enum.Parse(typeof (ASP.BorderStyle), zBorderStyle.SelectedValue);
        }

        protected void zBorderWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            zPanel.BorderWidth = new ASP.Unit(int.Parse(zBorderWidth.SelectedValue));
        }

        protected void zVisibility_OnCheckedChanged(object sender, EventArgs e)
        {
            zPanel.Visible = !zPanel.Visible;
        }
    }
}
