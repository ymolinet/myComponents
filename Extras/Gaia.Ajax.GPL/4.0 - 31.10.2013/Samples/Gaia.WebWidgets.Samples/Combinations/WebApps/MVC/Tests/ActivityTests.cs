namespace Gaia.WebWidgets.Samples.Combinations.WebApps.MVC.Tests
{
    using System;
    using Controllers;
    using Data;

    /// <summary>
    /// This is just a "dummy" test-suite to demonstrate that it's easy to write
    /// TDD - Test Driven Development against an MVC application
    /// </summary>
    public class ActivityTests
    {
        public void Test_ActivityController_TestInitialize()
        {
            ActivityModelMock mock = new ActivityModelMock();
            new ActivityController(mock).Initialize();

            bool dateInitialized = mock.Filter.When == DateTime.Now.Date;
            DummyAssert.IsTrue(dateInitialized);
        }

        public void Test_ActivityController_TestFilter()
        {
            ActivityModelMock mock = new ActivityModelMock();
            new ActivityController(mock).Initialize();

            ActivityListMock view = mock.ViewResults as ActivityListMock;

            bool sameData = ActivityDataLayer.GetByDateAndPerson(
                mock.Filter.When, mock.Filter.SelectedPerson).Count== view.CalendarItems.Count;
            DummyAssert.IsTrue(sameData, "Different data based on same filter");
        }

        public void Test_ActivityController_TestMockObject()
        {
            ActivityModelMock mock = new ActivityModelMock();
            new ActivityController(mock).Initialize();

            ActivityListMock view = mock.ViewResults as ActivityListMock;
            DummyAssert.IsTrue(view != null, "Unable to get test implementation");
        }

    }
}