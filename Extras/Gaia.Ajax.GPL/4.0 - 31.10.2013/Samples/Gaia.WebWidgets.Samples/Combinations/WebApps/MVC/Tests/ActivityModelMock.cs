namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Tests
{
    using Models;

    class ActivityModelMock : IActivityModel
    {
        private readonly IActivityFilter _filter = new ActivityFilterMock();
        private readonly IActivityList _list = new ActivityListMock();

        public IActivityFilter Filter
        {
            get { return _filter; }
        }

        public IActivityList ViewResults
        {
            get { return _list; }
        }
    }
}