namespace Gaia.WebWidgets.Samples.Extensions.Scheduler.MonthView
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Gaia.WebWidgets.Extensions;
    using Gaia.WebWidgets.Samples.UI;

    public partial class Default : SamplePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BuildResourceControls();

            if (!IsPostBack)
            {
                SelectResourceByIndex(0);
                AssignedResources.Clear();
                DataBindScheduler();
            }
        }

        void DataBindScheduler()
        {
            zScheduler.DataSource = AssignedResources;
            zScheduler.DataBind();
        }

        protected void MonthCellsCollected(object sender, ControlCollector.CollectedEventArgs e)
        {
            var selectedDays = e.Controls.Cast<IDateContainer>().Select(ct => ct.Value.Date);
            DoAssignResources(selectedDays);
            DataBindScheduler();
        }

        /// <summary>
        /// This function assigns all the selected resources to the selected days. 
        /// </summary>
        void DoAssignResources(IEnumerable<DateTime> selectedDays)
        {
            var resources = GetResources().ToArray();

            foreach (var selectedDay in selectedDays)
            {
                foreach (var resourceIndex in GetSelectedResourceIndices())
                {
                    var resource = resources[resourceIndex];
                    if (AssignedResources.Any(a=> a.Name == resource.Name && a.StartTime.Date == selectedDay.Date))
                        continue;

                    var assignedResource = new AssignedResource
                    {
                        ID = NextID().ToString(CultureInfo.InvariantCulture),
                        StartTime = selectedDay.Date.AddHours(8),
                        EndTime = selectedDay.Date.AddHours(16),
                        Name = resource.Name,
                        CssClass = resource.CssClass,
                    };

                    AssignedResources.Add(assignedResource);
                }

            }

            DataBindScheduler();
        }

        protected void ClearResourcesForSelectedResource(object sender, EventArgs e)
        {
            var linkButton = (LinkButton) sender;
            var id = linkButton.Parent.Controls.OfType<HiddenField>().Single().Value;

            RemoveFromAssignedResources(a => a.ID == id);

            DataBindScheduler();
        }

        protected void ClearResourcesForSelectedDay(object sender, EventArgs e)
        {
            var linkButton = (LinkButton)sender;
            var dateTime = DateTime.Parse(linkButton.CommandArgument);

            RemoveFromAssignedResources(a => a.StartTime.Date == dateTime.Date);
            DataBindScheduler();
        }

        private void RemoveFromAssignedResources(Func<AssignedResource, bool> removePredicate)
        {
            var removals = AssignedResources.Where(removePredicate).ToArray();

            foreach (var assignedResource in removals)
                AssignedResources.Remove(assignedResource);
        }

        protected bool MakeClearButtonVisible(IReportContainer container)
        {
            return container.Items.GetEnumerator().MoveNext();
        }

        protected void ClearAllAssignedResources(object sender, EventArgs e)
        {
            AssignedResources.Clear();
            DataBindScheduler();
        }

        /// <summary>
        /// Each Resource ( icon, checkbox & label) is grouped into it's own ResourceControl and created here. 
        /// </summary>
        private void BuildResourceControls()
        {
            int idx = 0;
            foreach (var resource in GetResources())
            {
                zResourcesPlaceholder.Controls.Add(new ResourceControl
                {
                    ID = "r" + idx,
                    Index = idx++,
                    Resource = resource
                });
            }
        }

        #region [ -- Sample Database -- ]

        private char NextID()
        {
            var nextId = AssignedResources.Any() ? AssignedResources.Select(task => task.ID[0]).Max() : 'a';
            return ++nextId;
        }

        /// <summary>
        /// You should probably use a database instead :-) 
        /// </summary>
        IList<AssignedResource> AssignedResources
        {
            get
            {
                return Session["assignedResources"] as IList<AssignedResource> ??
                       (IList<AssignedResource>) (Session["assignedResources"] = new List<AssignedResource>());
            }
        }

        private void SelectResourceByIndex(int index)
        {
            var result = GetResourceControls().SingleOrDefault(r => r.Index == index);
            if (result != null)
                result.Selected = true;
        }

        private IEnumerable<int> GetSelectedResourceIndices()
        {
            return from result in GetResourceControls()
                   where result.Selected
                   select result.Index;
        }

        private IEnumerable<ResourceControl> GetResourceControls()
        {
            return zResourcesPlaceholder.Controls.OfType<ResourceControl>();
        }

        static IEnumerable<Resource> GetResources()
        {
            yield return new Resource { Name = "Donnie Baloney", CssClass = "user-red" };
            yield return new Resource { Name = "Mickey Hickey", CssClass = "user-orange" };
            yield return new Resource { Name = "Johnny Donkey", CssClass = "user-green" };
            yield return new Resource { Name = "Tanja Manjana", CssClass = "user-female" };
            yield return new Resource { Name = "Cliff Diff", CssClass = "user-suit" };
            yield return new Resource { Name = "Bull Dozer", CssClass = "user-blue" };
        }

        #endregion
    }
}