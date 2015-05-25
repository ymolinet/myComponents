namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Controllers
{
    using System;
    using System.Web.UI;

    /// <summary>
    /// This base class offers a few boiler-plate hooks to automatically initialize a view and
    /// then reinitialize it on-demand. 
    /// </summary>
    public abstract class ControllerBoundView : UserControl
    {
        private ControllerBase _activityController;
        ControllerBase Controller
        {
            get { return _activityController ?? (_activityController = CreateController()); }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack) Controller.Initialize();
        }

        protected void ViewChanged(object sender, EventArgs e)
        {
            Controller.ViewChanged();
        }

        public abstract ControllerBase CreateController();
    }
}