namespace Gaia.WebWidgets.Samples.Extensions.Scheduler.Overview
{
    using System;
    using System.Globalization;
    using System.Linq;
    using ASP = System.Web.UI.WebControls;
    using Gaia.WebWidgets.Effects;
    
    public partial class Default : DataSamplePage
    {
        private int SelectedResourceIndex
        {
            get { return (int)(ViewState["resourceIndex"] ?? 0); }
            set { ViewState["resourceIndex"] = value; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            BuildResourceBoxes();

            if (!IsPostBack)
            {
                Tasks.Clear();
                DataBindScheduler();
            }
        }

        protected void OnTaskModified(object sender, WebWidgets.Extensions.Scheduler.TaskModifiedEventArgs e)
        {
            var task = FindTaskByID(e.TaskKey);
            
            task.StartTime = e.TaskStart;
            task.EndTime = e.TaskEnd;
            
            DataBindScheduler();

        }

        private const string NewTaskName = "[Click to edit]";

        protected void OnTimeRangeSelected(object sender, WebWidgets.Extensions.Scheduler.TimeRangeSelectedEventArgs e)
        {
            var timespan = (e.TaskEnd - e.TaskStart);
            
            if (!ValidateNewTask(timespan))return;
            
            var resources = new[] { Person1, Person2, Person3 };
            
            var newTask = new Task
                              {
                                  StartTime = e.TaskStart,
                                  EndTime = e.TaskEnd,
                                  ID = (NextID()).ToString(CultureInfo.InvariantCulture),
                                  Owner = resources[SelectedResourceIndex],
                                  Title = NewTaskName
                              };
            Tasks.Add(newTask);

            DataBindScheduler();

        }

        void BuildResourceBoxes()
        {
            var resources = new[] { Person1, Person2, Person3 };
            for (int i = 0; i < resources.Length; i++)
            {
                var resource = resources[i];
                var resourceClass = GetResourceCssClass(resource);
                var index = i;

                var resourcePanel = new Panel
                {
                    ID = resourceClass,
                    CssClass = "resourceBox " + resourceClass + (i == SelectedResourceIndex ? " selected" : "")
                };

                resourcePanel.Aspects.Add(new AspectClickable(delegate
                {
                    SelectedResourceIndex = index;
                    zResources.Controls.Clear();
                    BuildResourceBoxes();
                }));

                resourcePanel.Controls.Add(new Label
                {
                    Text = resource
                });

                zResources.Controls.Add(resourcePanel);
            }

        }

        protected object GetTaskCssClass(object dataItem)
        {
            var task = (Task) dataItem;
            return "task-container " + GetResourceCssClass(task.Owner);
        }

        private string GetResourceCssClass(string owner)
        {
            if (owner == Person1) return "person1";
            if (owner == Person2) return "person2";
            if (owner == Person3) return "person3";
            return "unknown";
            
        }

        protected void DeleteTask(object sender, ASP.CommandEventArgs e)
        {
            var id = (string) e.CommandArgument;
            Tasks.Remove(FindTaskByID(id));
            DataBindScheduler();
        }

        void DataBindScheduler()
        {
            zScheduler.DataSource = Tasks;
            zScheduler.DataBind();
        }

        protected void ClearTasks(object sender, EventArgs e)
        {
            Tasks.Clear();
            DataBindScheduler();
        }

        protected void ReloadSampleData(object sender, EventArgs e)
        {
            LoadSampleData();
            DataBindScheduler();
        }

        bool ValidateNewTask(TimeSpan timeSpan)
        {
            if (timeSpan.Days >= 1 || timeSpan.TotalHours > 10)
            {
                msg.Text = "Please add a task within the same day that last's less than 10 hours. " +
                           "This is only a limitation of the sample and not the scheduler component.";

                msg.Effects.Add(new EffectHighlight());
                return false;
            }

            msg.Text = string.Empty;
            return true;

        }

        protected void TaskTitleChanged(object sender, EventArgs e)
        {
            var inPlaceEdit = (WebWidgets.Extensions.InPlaceEdit) sender;

            // we stored the id here ... 
            var id = inPlaceEdit.Parent.Controls.OfType<LinkButton>().Single().CommandArgument; 

            // update the *db* 
            FindTaskByID(id).Title = inPlaceEdit.Text;
        }
    }
}