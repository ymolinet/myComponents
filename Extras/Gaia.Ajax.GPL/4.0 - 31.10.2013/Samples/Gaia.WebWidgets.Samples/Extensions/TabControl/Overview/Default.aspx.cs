namespace Gaia.WebWidgets.Samples.Extensions.TabControl.Overview
{
    using System;
    using Gaia.WebWidgets.Extensions;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            for (int i = 5; i < ExtraTabs ; i++)
            {
                zTabControl1.Controls.Add(AddTabView(i.ToString()));
            }
        }

        protected void zAddTabView_Click(object sender, EventArgs e)
        {
            zTabControl1.Controls.Add(AddTabView(ExtraTabs.ToString()));

            //set added as visible tabview
            zTabControl1.ActiveTabViewIndex = (short)(zTabControl1.Controls.Count - 1);

            //store added TabView
            ExtraTabs++;
        }

        private static TabView AddTabView(string id)
        {
            var tabView = new TabView {Caption = id};
            var lbl = new Label {Text = string.Format("<p>Hello TabView with ID: {0}</p>", id)};
            tabView.Controls.Add(lbl);

            return tabView;
        }

        protected void zHiddenGem_Click(object sender, EventArgs e)
        {
            zWindowSuccess.Visible = true;
        }

        public int ExtraTabs
        {
            get { return (int)(ViewState["ExtraTabs"] ?? 5); }
            set { ViewState["ExtraTabs"] = value; }
        }
    }
}
