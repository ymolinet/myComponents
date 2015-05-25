namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC
{
    using System;
    using System.Drawing;
    using Models;
    using Tests;
    using Gaia.WebWidgets.Samples.UI;
    using Gaia.WebWidgets.Samples.Utilities;

    using ASP = System.Web.UI.WebControls;

    /// <summary>
    /// This page does the following: 
    /// a) Populates the zAvailableItems DDL with available Views 
    /// b) Instruments the ActivityView with the selected View 
    /// </summary>
    public partial class Default : SamplePage
    {
        #region Code
        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            InitializeAvailableViewsList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            SetDefaultView();
        }

        private void SetDefaultView()
        {
            if (string.IsNullOrEmpty(zActs.SelectedView))
                zActs.SelectedView = zAvailableViews.SelectedValue;
        }

        protected void zAvailableViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            zActs.SelectedView = zAvailableViews.SelectedValue;
            zActs.TriggerViewChanged();
        }

        private void InitializeAvailableViewsList()
        {
            const string viewsFolder = "Views";
            foreach (ASP.ListItem item in WebUtility.GetUserControlViews<IActivityList>(Page, viewsFolder))
                zAvailableViews.Items.Add(item);
        }

        protected void zRunTests_Click(object sender, EventArgs e)
        {
            try
            {
                // Run the test-suite manually, fail on blowup! 
                ActivityTests tests = new ActivityTests();
                tests.Test_ActivityController_TestInitialize();
                tests.Test_ActivityController_TestMockObject();
                tests.Test_ActivityController_TestFilter();

                zRunTests.Text = "All Tests passed ... ";
                zRunTests.ForeColor = Color.Green;
            }
            catch
            {
                zRunTests.Text = "One or more tests failed ...";
                zRunTests.ForeColor = Color.Red;
            }

        } 
        #endregion
    }
}
