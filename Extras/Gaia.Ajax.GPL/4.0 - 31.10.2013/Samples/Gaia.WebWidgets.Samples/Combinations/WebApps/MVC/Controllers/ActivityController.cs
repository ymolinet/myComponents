namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using Gaia.WebWidgets.Samples.Utilities;
    using Data;
    using Models;

    /// <summary>
    /// This is the Controller in the MVC pattern and it acts as the "hub" and is responsible for
    /// working on the models and implementing business rules, DAL communication, etc
    /// </summary>
    public class ActivityController : ControllerBase
    {
        private readonly IActivityModel _activityModel;

        public ActivityController(IActivityModel activityModel)
        {
            _activityModel = activityModel;
        }

        public override void Initialize()
        {
            _activityModel.Filter.BindPersons(ActivityDataLayer.GetContacts());
            _activityModel.Filter.When = DateTime.Now.Date;

            ViewChanged();
        }

        IEnumerable<CalendarItem> GetItems()
        {
            return ActivityDataLayer.GetByDateAndPerson(_activityModel.Filter.When, _activityModel.Filter.SelectedPerson);
        }

        public override void ViewChanged()
        {
            _activityModel.ViewResults.View(GetItems());
        }
  
    }
}