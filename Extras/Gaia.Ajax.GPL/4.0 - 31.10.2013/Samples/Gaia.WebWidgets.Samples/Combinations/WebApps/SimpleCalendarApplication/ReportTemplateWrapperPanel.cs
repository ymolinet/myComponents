namespace Gaia.WebWidgets.Samples.Combinations.WebApps.SimpleCalendarApplication
{
    using System;
    using System.Collections;
    using Gaia.WebWidgets.Extensions;

    public class ReportTemplateWrapperPanel : Panel, IReportContainer
    {
        public IEnumerable Items
        {
            get { return (Parent as IReportContainer).Items; }
        }

        public DateTime Date
        {
            get { return (Parent as IReportContainer).Date; }
        }
    }
}