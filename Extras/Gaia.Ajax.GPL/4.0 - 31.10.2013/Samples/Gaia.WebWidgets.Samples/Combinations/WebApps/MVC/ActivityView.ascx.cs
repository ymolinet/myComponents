namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC
{
    using System;
    using System.IO;
    using System.Web.UI;
    using Controllers;
    using Models;
    using Gaia.WebWidgets.Samples.Utilities;

    /// <summary>
    /// The ActivityView implements the IActivityModel interface and offers
    /// dynamic loading of various Views which implement this interface
    /// </summary>
    public partial class ActivityView : ControllerBoundView, IActivityModel
    {
        public string SelectedView
        {
            get { return (ViewState["sv"] ?? string.Empty).ToString(); }
            set { ViewState["sv"] = value; }
        }

        public void TriggerViewChanged()
        {
            LoadSelectedView();
            ViewChanged(this, EventArgs.Empty);    
        }

        protected void Page_Load(object sender, EventArgs e)
        { 
            LoadSelectedView();
        }

        void LoadSelectedView()
        {
            Control selectedView = Page.LoadControl(Path.Combine("Views", SelectedView + ".ascx"));
            zView.Controls.Clear();
            zView.Controls.Add(selectedView);   
        }

        public override ControllerBase CreateController()
        {
            return new ActivityController(this);
        }

        #region IActivityModel Implementation

        public IActivityFilter Filter { get { return zFilter;} }
        public IActivityList ViewResults { get { return WebUtility.First<IActivityList>(zView.Controls); } }
        
        #endregion

    }
}