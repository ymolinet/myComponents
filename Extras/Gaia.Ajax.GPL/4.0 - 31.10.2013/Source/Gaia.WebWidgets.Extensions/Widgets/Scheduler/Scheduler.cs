/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2012 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

[assembly : WebResource("Gaia.WebWidgets.Extensions.Scripts.AspectResizableTask.js", "text/javascript")]

namespace Gaia.WebWidgets.Extensions
{
    /// <summary>
    /// <para>
    /// The scheduler allows you to display multiple tasks pr/day that can be resized and moved around. It also features a MonthView or DayView. In the DayView you can 
    /// select how many days you want to display by setting the <see cref="DayViewConfiguration.NumberOfDays"/> setting for the <see cref="DayView"/> property.
    /// You can also specify how many minutes each Resize "tick" represents and how many minutes each Cell represents. Please look at the Scheduler demos in the Samples project
    /// for valid configuration options. </para>
    /// <para>
    /// Because each of items (including Tasks) are Templatable you have full freedom to design it in whatever way you feel like. The Samples are rather simplistic compared to
    /// the full power of the scheduler. 
    /// </para>
    /// <para>By default the Scheduler binds to any data collection and uses the ID, StartTime and EndTime properties to create the Tasks. If your objects define other names
    /// for these properties you can set these properties in the <see cref="DateStartPropertyName"/>, <see cref="DateEndPropertyName"/> and <see cref="DataKeyPropertyName"/></para>
    /// <para>
    /// Other useful features include the ability to set StartHour, EndHour. You can also <see cref="EnableTimeRangeSelection"/> to allow TimeRanges to be selected by just
    /// selecting the cells with your mouse. This raises the <see cref="TimeRangeSelected"/> event where you can do whatever you want, for example create new tasks. Open Dialogs, etc.
    /// </para>
    /// Oh, and the Scheduler itself is written in pure C# just based on the Gaia Ajax components. Yes, not a single line of JavaScript for this component. IOHO - That rocks!
    /// </summary>
    [ParseChildren(true)]
    [PersistChildren(false)]
    public class Scheduler : ASP.CompositeDataBoundControl
    {
        #region [ -- Private Members -- ]

        private ASP.Unit? _rowHeight;
        private ASP.Style _taskCellStyle;
        private List<SchedulerTask> _tasks;
        private ICollection<DateTime> _days;
        private DayViewConfiguration _dayView;
        private WorkdayConfiguration _workday;
        private ViewMode _view = ViewMode.DayView;
        private MonthViewConfiguration _monthView;
        private bool _enableTimeRangeSelection = true;

        #endregion

        #region [ -- Constants -- ]

        private const int DayPerWeek = 7;
        private const string TaskPanelPrefix = "tp";
        private const string TaskKeyDelimeter = "-";
        private const string TaskContainerCssClass = "tc";
        private const string DefaultDataKeyPropertyName = "ID";
        private const string DefaultDateEndPropertyName = "EndTime";
        private const string DefaultDateStartPropertyName = "StartTime";
        private static readonly ASP.Unit FullDayTaskCellHeight = ASP.Unit.Pixel(25);

        #endregion

        #region [ -- Specialized AspectResizable -- ]
        
        sealed class AspectResizableTask : AspectResizable
        {
            public int SnapSize { get; set; }
            public int BorderSize { get; set; }
            public int CellHeight { get; set; }

            protected override RegisterAspect GetScript(RegisterAspect registerAspect)
            {
                var reg = base.GetScript(registerAspect);
                reg.ControlType = "Gaia.ART";
                return reg.AddProperty("ssz", SnapSize).AddProperty("bsz", BorderSize).AddProperty("clh", CellHeight);
            }

            protected override void IncludeScriptFiles()
            {
                base.IncludeScriptFiles();
                Manager.Instance.AddInclusionOfFileFromResource("Gaia.WebWidgets.Extensions.Scripts.AspectResizableTask.js", typeof(AspectResizableTask), "Gaia.ART");
            }
        } 

        #endregion

        #region [-- Specialized Containers -- ]

        private sealed class TaskPanel : Panel, IDataItemContainer
        {
            private readonly int _index;
            private readonly object _task;

            public TaskPanel(object task, int index)
            {
                _task = task;
                _index = index;
            }

            #region [-- IDataItemContainer Implementation -- ]

            object IDataItemContainer.DataItem
            {
                get { return _task; }
            }

            int IDataItemContainer.DataItemIndex
            {
                get { return _index; }
            }

            int IDataItemContainer.DisplayIndex
            {
                get { return _index; }
            }

            #endregion
        }

        private sealed class TaskContainer : Panel
        {
            private readonly DateTime _startTime;
            private readonly bool _isFullDayTaskContainer;

            public TaskContainer(DateTime startTime, bool isFullDayTaskContainer)
            {
                _startTime = startTime;
                _isFullDayTaskContainer = isFullDayTaskContainer;
            }

            public DateTime StartTime
            {
                get { return _startTime; }
            }

            public bool FullDayTaskContainer
            {
                get { return _isFullDayTaskContainer; }
            }
        }

        sealed class ReportPlaceholder : ASP.PlaceHolder, IReportContainer
        {
            private readonly IEnumerable _items;
            private readonly DateTime _date;
            public ReportPlaceholder(IEnumerable items, DateTime date )
            {
                _items = items;
                _date = date;
            }

            public DateTime Date { get { return _date; } }
            public IEnumerable Items { get { return _items; } }
        }

        sealed class NonBoundDateContainer : Panel, IDateContainer
        {
            public DateTime Value
            {
                get { return ((IDateContainer) BindingContainer).Value; }
            }
        }

        #endregion

        #region [-- Configuration classes --]

        public abstract class StyleConfigurationCommon
        {
            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.TableItemStyle EmptyCellStyle { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.TableItemStyle DefaultCellStyle { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.TableItemStyle HeaderCellStyle { get; set; }
        }

        public sealed class MonthViewStyleConfiguration : StyleConfigurationCommon
        {
            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.Style ContainerStyle { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.TableItemStyle CellStyle { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.TableItemStyle CellOutOfRangeStyle { get; set;}
        }

        public sealed class DayViewStyleConfiguration : StyleConfigurationCommon
        {
            public class PanelStyle : ASP.Style
            {
                public string CssClassDrag
                {
                    get { return StateUtil.Get<string>(ViewState, "dcc"); }
                    set { StateUtil.Set(ViewState, "dcc", value); }
                }
            }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.TableItemStyle TaskCellStyle { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public PanelStyle TaskPanelStyle { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.Style TaskContainerStyle { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.Style TaskContainerSelectedStyle { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.TableItemStyle FullDayTaskPanelStyle { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ASP.TableItemStyle TimeCellStyle { get; set; }
        }

        public abstract class TemplatesConfigurationCommon
        {
            [TemplateInstance(TemplateInstance.Single)]
            [TemplateContainer(typeof(IDateContainer))]
            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ITemplate HeaderTemplate { get; set; }
        }

        public sealed class DayViewTemplatesConfiguration : TemplatesConfigurationCommon
        {
            [TemplateInstance(TemplateInstance.Single)]
            [TemplateContainer(typeof(IDataItemContainer))]
            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ITemplate TaskTemplate { get; set; }

            [TemplateInstance(TemplateInstance.Single)]
            [TemplateContainer(typeof(ITimeContainer))]
            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ITemplate TimeCellTemplate { get; set; }
        }

        public sealed class MonthViewTemplatesConfiguration : TemplatesConfigurationCommon
        {
            [TemplateInstance(TemplateInstance.Single)]
            [TemplateContainer(typeof(IDateContainer))]
            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ITemplate DayTemplate { get; set; }

            [TemplateInstance(TemplateInstance.Single)]
            [TemplateContainer(typeof(IReportContainer))]
            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ITemplate ReportTemplate { get; set; }
        }

        public sealed class WorkdayConfiguration : IStateManager
        {
            private bool _marked;
            private int _markedEndHour;
            private int _markedStartHour;
            private int _startHour = 8;
            private int _endHour = 16;

            /// <summary>
            /// Workday start hour (between 00 and 24)
            /// </summary>
            public int StartHour
            {
                get { return _startHour; }
                set
                {
                    CheckHour(value);
                    _startHour = value;
                }
            }

            /// <summary>
            /// Workday end hour (between 00 and 24)
            /// </summary>
            public int EndHour
            {
                get { return _endHour; }
                set
                {
                    CheckHour(value);
                    _endHour = value;
                }
            }

            private static void CheckHour(int hour)
            {
                if (hour >= 0 && hour <= 23) return;
                throw new ArgumentOutOfRangeException("hour");
            }

            #region [-- IStateManager Implementation --]

            bool IStateManager.IsTrackingViewState
            {
                get { return _marked; }
            }

            void IStateManager.LoadViewState(object state)
            {
                if (state == null) return;
                var pair = (Pair) state;
                StartHour = (int)pair.First;
                EndHour = (int)pair.Second;
            }

            object IStateManager.SaveViewState()
            {
                if (!_marked || (_markedStartHour == StartHour && _markedEndHour == EndHour)) return null;
                return new Pair(StartHour, EndHour);
            }

            void IStateManager.TrackViewState()
            {
                if (_marked) return;
                _marked = true;
                _markedEndHour = EndHour;
                _markedStartHour = StartHour;
            }

            #endregion

            internal WorkdayConfiguration VerifiedClone()
            {
                return new WorkdayConfiguration
                           {
                               EndHour = EndHour,
                               StartHour = StartHour
                           };
            }
        }

        public sealed class TimelineConfiguration : IStateManager
        {
            private StateBag _viewState;
            private bool _isTrackingViewState;
            private const string GranularityKey = "gr";

            /// <summary>
            /// The granularity of displaying hours in minutes.
            /// Should be value between 1 and 60.
            /// </summary>
            public int Granularity 
            {
                get { return StateUtil.Get(ViewState, GranularityKey, 15); }
                set
                {
                    if (value < 1 || value > 60)
                        throw new ArgumentOutOfRangeException("value");

                    StateUtil.Set(ViewState, GranularityKey, value);
                }
            }

            /// <summary>
            /// Timeline hour template
            /// </summary>
            [TemplateContainer(typeof(GridViewRow))]
            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ITemplate HourTemplate { get; set; }

            /// <summary>
            /// Timeline minute template
            /// </summary>
            [TemplateContainer(typeof(GridViewRow))]
            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public ITemplate MinuteTemplate { get; set; }

            private StateBag ViewState
            {
                get
                {
                    if(_viewState == null)
                    {
                        _viewState = new StateBag();
                        if (_isTrackingViewState)
                            ((IStateManager)_viewState).TrackViewState();
                    }

                    return _viewState;
                }
            }

            #region [-- IStateManager Implementation --]

            bool IStateManager.IsTrackingViewState
            {
                get { return _isTrackingViewState; }
            }

            void IStateManager.LoadViewState(object state)
            {
                if (state == null) return;
                ViewState[GranularityKey] = (int) state;
            }

            object IStateManager.SaveViewState()
            {
                return ViewState.IsItemDirty(GranularityKey) ? ViewState[GranularityKey] : null;
            }

            void IStateManager.TrackViewState()
            {
                _isTrackingViewState = true;
                if (_viewState == null) return;
                ((IStateManager)_viewState).TrackViewState();
            }

            #endregion
        }

        public sealed class DayViewConfiguration : IStateManager
        {
            private StateBag _viewState;
            private bool _isTrackingViewState;
            private TimelineConfiguration _timeline;

            internal DayViewConfiguration VerifiedClone()
            {
                return new DayViewConfiguration
                           {
                               NumberOfDays = NumberOfDays,
                               ResizeMinutes = ResizeMinutes,
                               StartDate = StartDate,
                               Styles = Styles ?? new DayViewStyleConfiguration(),
                               Timeline = Timeline ?? new TimelineConfiguration(),
                               Templates = Templates ?? new DayViewTemplatesConfiguration()
                           };
            }

            public int ResizeMinutes
            {
                get { return StateUtil.Get(ViewState, "rm", 5); }
                set
                {
                    if (value < 1)
                        throw new ArgumentOutOfRangeException("value");

                    StateUtil.Set(ViewState, "rm", value);
                }
            }

            public int NumberOfDays
            {
                get { return StateUtil.Get(ViewState, "nd", 3); }
                set { StateUtil.Set(ViewState, "nd", value); }
            }

            public DateTime StartDate
            {
                get { return StateUtil.Get(ViewState, "sd", DateTime.Today); }
                set { StateUtil.Set(ViewState, "sd", value); }
            }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public TimelineConfiguration Timeline
            {
                get { return _timeline; }
                set
                {
                    _timeline = value;
                    if (!_isTrackingViewState) return;
                    ((IStateManager)_timeline).TrackViewState();
                }
            }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public DayViewStyleConfiguration Styles { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public DayViewTemplatesConfiguration Templates { get; set; }

            private StateBag ViewState
            {
                get
                {
                    if (_viewState == null)
                    {
                        _viewState = new StateBag();
                        if (_isTrackingViewState)
                            ((IStateManager)_viewState).TrackViewState();
                    }

                    return _viewState;
                }
            }

            #region [-- IStateManager Implementation --]

            bool IStateManager.IsTrackingViewState
            {
                get { return _isTrackingViewState; }
            }

            void IStateManager.LoadViewState(object state)
            {
                if (state == null) return;
                
                object savedState;
                var pair = state as Pair;
                
                if (pair != null)
                {
                    savedState = pair.First;

                    if (_timeline == null)
                        _timeline = new TimelineConfiguration();

                    ((IStateManager) _timeline).LoadViewState(pair.Second);
                }
                else
                    savedState = state;

                ((IStateManager)ViewState).LoadViewState(savedState);
            }

            object IStateManager.SaveViewState()
            {
                object timelineState = null;
                if (_timeline != null)
                    timelineState = ((IStateManager) _timeline).SaveViewState();

                var state = ((IStateManager) ViewState).SaveViewState();
                return _timeline != null ? new Pair(state, timelineState) : state;
            }

            void IStateManager.TrackViewState()
            {
                _isTrackingViewState = true;
                if (_viewState != null)
                    ((IStateManager)_viewState).TrackViewState();
                if (_timeline != null)
                    ((IStateManager)_timeline).TrackViewState();
            }

            #endregion
        }

        public sealed class MonthViewConfiguration : IStateManager
        {
            private StateBag _viewState;
            private bool _isTrackingViewState;

            public DateTime Month 
            {
                get { return StateUtil.Get(ViewState, "m", DateTime.Today);  }
                set { StateUtil.Set(ViewState, "m", value); }
            }

            public ASP.FirstDayOfWeek FirstDayOfWeek
            {
                get { return StateUtil.Get(ViewState, "f", ASP.FirstDayOfWeek.Default); }
                set
                {
                    if (value < ASP.FirstDayOfWeek.Sunday || value > ASP.FirstDayOfWeek.Default)
                        throw new ArgumentOutOfRangeException("value");
                    StateUtil.Set(ViewState, "f", value);
                }
            }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public MonthViewStyleConfiguration Styles { get; set; }

            [PersistenceMode(PersistenceMode.InnerProperty)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public MonthViewTemplatesConfiguration Templates { get; set; }

            internal MonthViewConfiguration VerifiedClone()
            {
                return new MonthViewConfiguration
                           {
                               Month = Month,
                               FirstDayOfWeek = FirstDayOfWeek,
                               Styles = Styles ?? new MonthViewStyleConfiguration(),
                               Templates = Templates ?? new MonthViewTemplatesConfiguration()
                           };
            }

            private StateBag ViewState
            {
                get
                {
                    if (_viewState == null)
                    {
                        _viewState = new StateBag();
                        if (_isTrackingViewState)
                            ((IStateManager)_viewState).TrackViewState();
                    }

                    return _viewState;
                }
            }

            #region [-- IStateManager Implementation -- ]

            void IStateManager.LoadViewState(object state)
            {
                ((IStateManager)ViewState).LoadViewState(state);
            }

            object IStateManager.SaveViewState()
            {
                return ((IStateManager)ViewState).SaveViewState();
            }

            void IStateManager.TrackViewState()
            {
                _isTrackingViewState = true;
                if (_viewState == null) return;
                ((IStateManager)_viewState).TrackViewState();
            }

            bool IStateManager.IsTrackingViewState
            {
                get { return _isTrackingViewState; }
            }

            #endregion
        }

        public enum ViewMode
        {
            DayView,
            MonthView
        } ;

        #endregion

        #region [ -- Event Arguments -- ]

        /// <summary>
        /// EventArgs passed when the <see cref="Scheduler.TimeRangeSelected"/> event is raised. 
        /// </summary>
        public sealed class TimeRangeSelectedEventArgs : EventArgs
        {
            private readonly  DateTime _taskEnd;
            private readonly DateTime _taskStart;

            /// <summary>
            /// DateTime object that represents the Start of the SelectedRange
            /// </summary>
            public DateTime TaskStart
            {
                get { return _taskStart; }
            }

            /// <summary>
            /// DateTime object that represents the End of the SelectedRange
            /// </summary>
            public DateTime TaskEnd
            {
                get { return _taskEnd; }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="taskStart">DateTime object that represents the Start of the SelectedRange</param>
            /// <param name="taskEnd">DateTime object that represents the End of the SelectedRange</param>
            public TimeRangeSelectedEventArgs(DateTime taskStart, DateTime taskEnd)
            {
                _taskEnd = taskEnd;
                _taskStart = taskStart;
            }
        }

        /// <summary>
        /// EventArgs that is passed when the <see cref="Scheduler.TaskModified"/> event is raised. 
        /// </summary>
        public sealed class TaskModifiedEventArgs : CancellableEventArgs
        {
            private readonly string _taskKey;
            private readonly DateTime _taskEnd;
            private readonly DateTime _taskStart;

            /// <summary>
            /// The Unique ID/Key for this Task. 
            /// </summary>
            public string TaskKey
            {
                get { return _taskKey; }
            }

            /// <summary>
            /// DateTime object that represents the Start of this Task.
            /// </summary>
            public DateTime TaskStart
            {
                get { return _taskStart; }
            }

            /// <summary>
            /// DateTime object that represents the End of this Task.
            /// </summary>
            public DateTime TaskEnd
            {
                get { return _taskEnd; }
            }


            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="taskKey">The Unique ID/Key for this Task. </param>
            /// <param name="taskStart">DateTime object that represents the Start of this Task.</param>
            /// <param name="taskEnd">DateTime object that represents the End of this Task.</param>
            public TaskModifiedEventArgs(string taskKey, DateTime taskStart, DateTime taskEnd)
            {
                _taskKey = taskKey;
                _taskEnd = taskEnd;
                _taskStart = taskStart;
            }
        }

        /// <summary>
        /// EventArgs that is passed when the <see cref="Scheduler.CellCreated"/> event is fired. 
        /// </summary>
        public sealed class SchedulerCellEventArgs : EventArgs
        {
            private readonly SchedulerCell _cell;
            private readonly bool _isHeader;

            /// <summary>
            /// True if this Cell is a HeaderCell
            /// </summary>
            public bool IsHeader
            {
                get { return _isHeader; }
            }

            /// <summary>
            /// The SchedulerCell
            /// </summary>
            public SchedulerCell Cell
            {
                get { return _cell; }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="cell">The SchedulerCell</param>
            /// <param name="isHeader">True if this Cell is a HeaderCell</param>
            public SchedulerCellEventArgs(SchedulerCell cell, bool isHeader)
            {
                _cell = cell;
                _isHeader = isHeader;
            }
        }

        /// <summary>
        /// EventArgs that is passed when the <see cref="Scheduler.TaskCreating"/> event is fired. 
        /// </summary>
        public sealed class TaskCreatingEventArgs : EventArgs
        {
            private readonly string _taskKey;
            private bool _allowMove = true;
            private bool _allowResize = true;

            /// <summary>
            /// The Unique ID/Key for the Task
            /// </summary>
            public string TaskKey
            {
                get { return _taskKey; }
            }

            /// <summary>
            /// Set this property to true if you want to provide the user the ability to move the Task around
            /// </summary>
            [DefaultValue(true)]
            public bool AllowMove
            {
                get { return _allowMove; }
                set { _allowMove = value; }
            }

            /// <summary>
            /// Set this property to true if you want to provide the user the ability to resize the Task. 
            /// </summary>
            [DefaultValue(true)]
            public bool AllowResize
            {
                get { return _allowResize; }
                set { _allowResize = value; }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            internal TaskCreatingEventArgs(string taskKey)
            {
                _taskKey = taskKey;
            }

        }

        #endregion

        #region [ -- Events -- ]

        public event EventHandler<SchedulerCellEventArgs> CellCreated;

        public event EventHandler<SchedulerCellEventArgs> CellDataBound;
        
        public event EventHandler<TimeRangeSelectedEventArgs> TimeRangeSelected;
        
        public event EventHandler<TaskModifiedEventArgs> TaskModified;
        
        public event EventHandler<TaskCreatingEventArgs> TaskCreating;
        
        #endregion
        
        #region [ -- Properties -- ]

        /// <summary>
        /// Set this property to true to provide the user the ability to select a TimeRange in the Scheduler. This in turn will raise the
        /// <see cref="TimeRangeSelected"/> event where you can do things like for example creating a new task for that selection. 
        /// </summary>
        [DefaultValue(true)]
        public bool EnableTimeRangeSelection
        {
            get { return _enableTimeRangeSelection; }
            set { _enableTimeRangeSelection = value; }
        }

        /// <summary>
        /// The Scheduler supports a day/week view represented by the default <see cref="ViewMode.DayView"/> value or you can use the <see cref="ViewMode.MonthView"/>
        /// to provide a regular month view. 
        /// </summary>
        [DefaultValue(ViewMode.DayView)]
        public ViewMode View
        {
            get { return _view; }
            set { _view = value; }
        }

        /// <summary>
        /// Configuration options for the DayView
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DayViewConfiguration DayView
        {
            get { return _dayView; }
            set 
            { 
                _dayView = value;
                if (IsTrackingViewState)
                    ((IStateManager)_dayView).TrackViewState();
            }
        }

        /// <summary>
        /// Configuration options for the MonthView
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public MonthViewConfiguration MonthView
        {
            get { return _monthView; }
            set
            {
                _monthView = value;
                if (IsTrackingViewState)
                    ((IStateManager)_monthView).TrackViewState();
            }
        }
        
        /// <summary>
        /// WorkDay configuration
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public WorkdayConfiguration Workday
        {
            get { return _workday; }
            set
            {
                _workday = value;
                if (IsTrackingViewState)
                    ((IStateManager)_workday).TrackViewState();
            }
        }

        /// <summary>
        /// The scheduler requires some knowledge about the data you bind against and by default it will look for a property called StartDate to represent the Start
        /// of the Task. If your object has some other name for that property you can override the defaults by specifying that value here. ie. TaskStart
        /// </summary>
        [PersistenceMode(PersistenceMode.Attribute)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public virtual string DateStartPropertyName { get; set; }

        /// <summary>
        /// The scheduler requires some knowledge about the data you bind against and by default it will look for a property called EndDate to represent the End
        /// of the Task. If your object has some other name for that property you can override the defaults by specifying that value here. ie. TaskEnd
        /// </summary>
        [PersistenceMode(PersistenceMode.Attribute)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public virtual string DateEndPropertyName { get; set; }

        /// <summary>
        /// The scheduler requires some knowledge about the data you bind against and by default it will look for a property called ID to represent the Unique
        /// ID of the Task. If your object has some other name for that property you can override the defaults by specifying that value here. ie. UniqueTaskID
        /// </summary>
        [PersistenceMode(PersistenceMode.Attribute)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public virtual string DataKeyPropertyName { get; set; }

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.
        /// </returns>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        #endregion

        #region [-- Creation Methods --]

        private ASP.TableCell CreateTimeCell(TimeSpan timeSpan, int rangeIndex, ASP.Unit rowHeight)
        {
            var timeCell = new SchedulerTimeCell(rangeIndex, timeSpan)
            {
                ID = "tl",
                Width = ASP.Unit.Percentage(1)
            };

            var styles = VerifiedDayView.Styles;
            timeCell.ControlStyle.CopyFrom(styles.TimeCellStyle);
            timeCell.ControlStyle.MergeWith(styles.DefaultCellStyle);
            var prefix = string.IsNullOrEmpty(timeCell.CssClass) ? "" : " ";

            var timeCellTemplate = VerifiedDayView.Templates.TimeCellTemplate;
            if (timeCellTemplate == null)
            {
                var panel = new ASP.Panel { Height = rowHeight };

                // first cell
                if (timeSpan.Minutes == 0)
                {
                    var date = DateTime.Today.Add(timeSpan);
                    timeCell.CssClass += prefix + "first";

                    var label = new Label { ID = "tx", Text = date.ToString("h "), /*Font = { Bold = true }*/ };
                    panel.Controls.Add(label);

                    label = new Label
                    {
                        ID = "ap",
                        Text = date.ToString("tt", CultureInfo.InvariantCulture)
                    };
                    
                    panel.Controls.Add(label);
                }

                if (timeSpan.Add(new TimeSpan(0, VerifiedDayView.Timeline.Granularity, 0)).Hours >= timeSpan.Hours + 1)
                    timeCell.CssClass += prefix + "last";

                timeCell.Controls.Add(panel);
            }
            else
                timeCellTemplate.InstantiateIn(timeCell);

            return timeCell;
        }

        #endregion

        #region [-- Task management methods --]

        private ICollection<SchedulerTask> GetTasks(IEnumerable dataSource, bool normalize)
        {
            var tasks = new List<SchedulerTask>();
            if (dataSource == null) return tasks;
            var timelineEndHour = VerifiedWorkday.EndHour;
            var timelineStartHour = VerifiedWorkday.StartHour;

            foreach (var source in dataSource)
            {
                var task = source as SchedulerTask;
                if (task == null)
                {
                    var value = DataBinder.GetPropertyValue(source, DateStartPropertyName ?? DefaultDateStartPropertyName);
                    if (value == null || !(value is DateTime))
                        throw new ArgumentException("Specified DataSource item does not provide a DateStart property");
                    var startDate = (DateTime)value;

                    value = DataBinder.GetPropertyValue(source, DateEndPropertyName ?? DefaultDateEndPropertyName);
                    if (value == null || !(value is DateTime))
                        throw new ArgumentException("Specified DataSource item does not provide a DateEnd property");
                    var endDate = (DateTime)value;

                    value = DataBinder.GetPropertyValue(source, DataKeyPropertyName ?? DefaultDataKeyPropertyName);
                    if (value == null)
                        throw new ArgumentException("Specified DataSource item does not provide a DataKey property");
                    var id = value.ToString();

                    var end = NormalizeEndDate(endDate, timelineStartHour, timelineEndHour);
                    var start = NormalizeStartDate(startDate, timelineStartHour, timelineEndHour);
                    var duration = end - start;

                    if (duration.TotalMinutes <= 0) continue;

                    foreach (var normalizedTask in NormalizeTask(id, start, end, source, normalize))
                        tasks.Add(normalizedTask);
                }
                else
                    tasks.Add(task);
            }

            return tasks;
        }

        private IEnumerable<SchedulerTask> NormalizeTask(string id, DateTime start, DateTime end, object dataSource, bool normalize)
        {
            var index = 0;

            if (normalize)
            {
                while (start.Date < end.Date)
                {
                    yield return SchedulerTask.Create(index, id + TaskKeyDelimeter + index, start, start.Date.AddHours(VerifiedWorkday.EndHour), dataSource);
                    start = start.Date.AddDays(1).AddHours(VerifiedWorkday.StartHour);
                    ++index;
                }
            }
            
            if (start > end) yield break;
            yield return SchedulerTask.Create(index, id + TaskKeyDelimeter + index, start, end, dataSource);
        }

        private static DateTime NormalizeStartDate(DateTime start, int timelineStartHour, int timelineEndHour)
        {
            var startHour = start.Hour;
            if (startHour < timelineStartHour) return start.Date.AddHours(timelineStartHour);
            return startHour <= timelineEndHour ? start : start.Date.AddDays(1).AddHours(timelineStartHour);
        }

        private static DateTime NormalizeEndDate(DateTime end, int timelineStartHour, int timelineEndHour)
        {
            var endHour = end.Hour;
            if (endHour > timelineEndHour) return end.Date.AddHours(timelineEndHour);
            return endHour >= timelineStartHour ? end : end.Date.AddDays(-1).AddHours(timelineEndHour);
        }

        private bool IsTaskInRange(SchedulerTask task, DateTime cellTime)
        {
            var diff = task.StartTime - cellTime;
            return diff.Days == 0 && diff.Hours == 0 && diff.Minutes >= 0 &&
                   diff.Minutes < VerifiedDayView.Timeline.Granularity;
        }

        #endregion

        protected virtual ASP.Table CreateChildTable()
        {
            return new ASP.Table();
        }

        protected virtual SchedulerRow CreateSchedulerRow()
        {
            return new SchedulerRow();
        }

        private void InitializeChildTable(ASP.Table table)
        {
            table.CellSpacing = table.CellPadding = 0;
            table.ControlStyle.CopyFrom(ControlStyle);
        }

        private void InitializeEmptyCell(ASP.TableCell cell)
        {
            cell.RowSpan = 2;
            var emptyCellStyle = cell.ControlStyle;
            emptyCellStyle.Width = ASP.Unit.Percentage(1);
            emptyCellStyle.MergeWith(VerifiedDayView.Styles.DefaultCellStyle);
            emptyCellStyle.MergeWith(VerifiedDayView.Styles.EmptyCellStyle);

            if (EnableTimeRangeSelection)
            {
                var selector = new ControlCollector
                {
                    Filter = "." + TaskContainerCssClass,
                    FilterEventSource = "." + TaskContainerCssClass,
                };
                var taskContainerSelectedStyle = VerifiedDayView.Styles.TaskContainerSelectedStyle;
                if (taskContainerSelectedStyle != null)
                    selector.CssClassCollected = taskContainerSelectedStyle.CssClass;
                selector.Collected += ContainersSelected;
                cell.Controls.Add(selector);
   
            }

        }

        private void ContainersSelected(object sender, ControlCollector.CollectedEventArgs e)
        {
            var end = DateTime.MinValue;
            var start = DateTime.MaxValue;
            var granularity = VerifiedDayView.Timeline.Granularity;

            foreach(var control in e.Controls)
            {
                var container = control as TaskContainer;
                if (container == null) continue;
                if (start > container.StartTime)
                    start = container.StartTime;

                var containerEndTime = container.StartTime.AddMinutes(granularity);
                if (end < containerEndTime)
                    end = containerEndTime;
            }


            if (start > end) return;

            OnTimeRangeSelected(new TimeRangeSelectedEventArgs(start, end));
        }


        private void InitializeMonthViewHeaderCell(ASP.TableCell cell, DateTime date)
        {
            var localizedDay = CultureInfo.CurrentUICulture.DateTimeFormat.GetDayName(date.DayOfWeek);
            InitializeHeaderCell(cell, localizedDay, VerifiedMonthView.Styles, VerifiedMonthView.Templates.HeaderTemplate);
        }

        private void InitializeDayViewHeaderCell(ASP.TableCell cell, DateTime day)
        {
            InitializeHeaderCell(cell, day.ToLongDateString(), VerifiedDayView.Styles, VerifiedDayView.Templates.HeaderTemplate);
        }

        private static void InitializeHeaderCell(ASP.TableCell cell, string text, StyleConfigurationCommon styles, ITemplate headerTemplate)
        {
            var cellStyle = cell.ControlStyle;
            cellStyle.CopyFrom(styles.HeaderCellStyle);
            cellStyle.MergeWith(styles.DefaultCellStyle);

            if (headerTemplate == null)
                cell.Text = text;
            else
                headerTemplate.InstantiateIn(cell);
        }

        private void InitializeHeaderRow(ASP.TableRow row, ASP.TableRowCollection rows, IEnumerable<DateTime> days, bool dataBinding)
        {
            row.ID = "hdr";
            rows.Add(row);

            var emptyCell = new ASP.TableCell();
            InitializeEmptyCell(emptyCell);
            row.Cells.Add(emptyCell);

            var index = 0;

            foreach (var day in days)
            {
                var cell = new SchedulerDateCell(index, day) { ID = "c" + index };
                InitializeDayViewHeaderCell(cell, day);
                var eventArgs = new SchedulerCellEventArgs(cell, isHeader:true);
                OnCellCreated(eventArgs);
                row.Cells.Add(cell);
                ++index;
                if (!dataBinding) continue;
                cell.DataBind();
                OnCellDataBound(eventArgs);
            }
        }

        private void InitializeTaskPanel(Panel panel, DateTime? cellTime, TaskPanelConfiguration configuration)
        {
            var styles = VerifiedDayView.Styles;
            if (!cellTime.HasValue)
            {
                panel.ControlStyle.CopyFrom(styles.FullDayTaskPanelStyle);
                if (string.IsNullOrEmpty(panel.CssClass))
                {
                    panel.Style[HtmlTextWriterStyle.Margin] = ASP.Unit.Pixel(5).ToString(NumberFormatInfo.InvariantInfo);
                }
            }
            else
            {
                InitializeRegularTaskPanel(panel, cellTime.Value, configuration);
            }

            if (configuration.AllowTaskMove)
            {
                var draggable = new AspectDraggable { IdToPass = configuration.Task.ID };
                draggable.Snap.TargetSelector = "." + TaskContainerCssClass;
                var panelStyle = styles.TaskPanelStyle;
                if (panelStyle != null) draggable.DragCssClass = panelStyle.CssClassDrag;
                panel.Aspects.Add(draggable);
            }
            
        }

        private void InitializeRegularTaskPanel(Panel panel, DateTime cellTime, TaskPanelConfiguration configuration)
        {
            var containerHeight = configuration.TaskContainerHeight;
            var taskCellBorderWidth = configuration.TaskCellBorderWidth;
            var taskContainerBorderWidth = configuration.TaskContainerBorderWidth;

            VerifyUnitType(taskCellBorderWidth, containerHeight);
            VerifyUnitType(taskCellBorderWidth, taskContainerBorderWidth);

            var task = configuration.Task;
            var position = configuration.Position;

            var endTime = task.EndTime;
            var startTime = task.StartTime;
            var cellTimeValue = cellTime;
            var offset = (startTime - cellTimeValue).TotalMinutes;
            var granularity = VerifiedDayView.Timeline.Granularity;
            var minuteSize = containerHeight.Value / granularity;

            panel.Style[HtmlTextWriterStyle.Position] = "absolute";
            if (position.Width.HasValue)
            {
                panel.Width = position.Width.Value;
            }

            if (position.Left.HasValue)
            {
                panel.Style[HtmlTextWriterStyle.Left] = position.Left.Value.ToString(NumberFormatInfo.InvariantInfo);
            }

            if (position.ZIndex.HasValue)
            {
                panel.Style[HtmlTextWriterStyle.ZIndex] = position.ZIndex.Value.ToString(CultureInfo.InvariantCulture);
            }

            panel.Style[HtmlTextWriterStyle.Top] = new ASP.Unit(offset * minuteSize, containerHeight.Type).ToString(NumberFormatInfo.InvariantInfo);
            panel.ControlStyle.MergeWith(VerifiedDayView.Styles.TaskPanelStyle);
            panel.Height = new ASP.Unit(ComputeHeight(startTime, endTime, cellTimeValue, minuteSize, taskCellBorderWidth.Value, taskContainerBorderWidth.Value, panel.BorderWidth.Value), containerHeight.Type);

            if (configuration.AllowTaskResize)
            {
                var timelineEndTime = cellTimeValue.Date.AddHours(VerifiedWorkday.EndHour);
                var timelineStartTime = cellTimeValue.Date.AddHours(VerifiedWorkday.StartHour);
                var top = -(int)Math.Round(ComputeHeight(timelineStartTime, startTime, timelineStartTime, minuteSize, taskCellBorderWidth.Value, taskContainerBorderWidth.Value, panel.BorderWidth.Value));
                var bottom = (int)Math.Round(ComputeHeight(timelineStartTime, timelineEndTime, timelineStartTime.AddMinutes(1), minuteSize, taskCellBorderWidth.Value, taskContainerBorderWidth.Value, panel.BorderWidth.Value));

                var resizable = new AspectResizableTask();
                var borderHeight = taskCellBorderWidth.Value + taskContainerBorderWidth.Value * 2;
                resizable.Resizing += (sender, evtArgs) => TaskResizing(panel, evtArgs, task.ID, minuteSize);
                resizable.BoundingRectangle = new Rectangle(0, top, 0, bottom);
                resizable.Mode = AspectResizable.ResizeModes.TopBorder | AspectResizable.ResizeModes.BottomBorder;
                resizable.SnapSize = (int)Math.Round(minuteSize * VerifiedDayView.ResizeMinutes);
                resizable.CellHeight = (int)containerHeight.Value;
                resizable.BorderSize = (int)borderHeight;
                panel.Aspects.Add(resizable);
            }

        }
        
        /// <summary>
        /// Invoked/Fired when each Cell in the <see cref="Scheduler"/> is created. Either subscribe to the event or override this in your derived class
        /// if you want to provide custom behavior, change the Cell or merely add your own <see cref="Control"/>s to it.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCellCreated(SchedulerCellEventArgs e)
        {
            if (CellCreated == null) return;
            CellCreated(this, e);
        }

        /// <summary>
        /// Invoked during <see cref="System.Web.UI.WebControls.BaseDataBoundControl.DataBind"/> for each Cell
        /// </summary>
        protected virtual void OnCellDataBound(SchedulerCellEventArgs e)
        {
            if (CellDataBound == null) return;
            CellDataBound(this, e);
        }

        /// <summary>
        /// Invoked for each Task that is created by the <see cref="Scheduler"/>. Either subscribe to the event or override this in your derived class
        /// if you want to specify the <see cref="TaskCreatingEventArgs.AllowMove"/> or <see cref="TaskCreatingEventArgs.AllowResize"/>.
        /// </summary>
        protected virtual void OnTaskCreating(TaskCreatingEventArgs e)
        {
            if (TaskCreating == null) return;
            TaskCreating(this, e);
        }

        /// <summary>
        /// Invoked/Fired when the user has selected a timerange. You can either subscribe to the event or override this method in your derived class.
        /// </summary>
        protected virtual void OnTimeRangeSelected(TimeRangeSelectedEventArgs e)
        {
            if (TimeRangeSelected == null) return;
            TimeRangeSelected(this, e);
        }

        /// <summary>
        /// Invoked/Fired when a user modifies a Task in the Scheduler. This can be either a Move or Resize operation, but you are required to handle the actual result. 
        /// If you want to cancel/invalidate the operation simply set the <see cref="CancellableEventArgs.Cancel"/> property to true.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTaskModified(TaskModifiedEventArgs e)
        {
            if (TaskModified == null) return;
            TaskModified(this, e);
        }

        
        private double ComputeHeight(DateTime startTime, DateTime endTime, DateTime cellTime, double minuteSize, double taskCellBorderWidth, double taskContainerBorderWidth, double taskPanelBorderWidth)
        {
            var minuteCount = (endTime - startTime).TotalMinutes + 1;
            var cellCount = Math.Floor((endTime - cellTime).TotalMinutes / VerifiedDayView.Timeline.Granularity);
            return minuteCount * minuteSize + cellCount * taskCellBorderWidth + cellCount * 2 * taskContainerBorderWidth - 2 * taskPanelBorderWidth;
        }

        private sealed class TaskPanelConfiguration
        {
            private bool _allowTaskMove = true;
            private bool _allowTaskResize = true;

            public SchedulerTask Task { get; set; }
            
            public TaskPosition Position { get; set; }
            
            public ASP.Unit TaskCellBorderWidth { get; set; }
            
            public ASP.Unit TaskContainerHeight { get; set; }
            
            public ASP.Unit TaskContainerBorderWidth { get; set; }
            
            public bool AllowTaskMove
            {
                get { return _allowTaskMove; }
                set { _allowTaskMove = value; }
            }
            
            public bool AllowTaskResize
            {
                get { return _allowTaskResize; }
                set { _allowTaskResize = value; }
            }
        }

        private ASP.WebControl CreateTaskPanel(DateTime? cellTime, TaskPanelConfiguration configuration)
        {
            var task = configuration.Task;
            var taskKey = configuration.Task.ID;
            taskKey = taskKey.Substring(0, taskKey.LastIndexOf(TaskKeyDelimeter, StringComparison.Ordinal));
            
            var args = new TaskCreatingEventArgs(taskKey);
            OnTaskCreating(args);
            configuration.AllowTaskMove = args.AllowMove;
            configuration.AllowTaskResize = args.AllowResize;
            
            SaveTask(task);
            
            var taskPanel = new TaskPanel(task.DataSource, -1) {ID = TaskPanelPrefix + task.ID};
            InitializeTaskPanel(taskPanel, cellTime, configuration);

            var taskTemplate = VerifiedDayView.Templates.TaskTemplate;
            if (taskTemplate == null)
            {
                InstantiateDefaultTaskPanelTemplate(cellTime, task, taskPanel);
            }
            else
            {
                taskTemplate.InstantiateIn(taskPanel);
            }
            
            return taskPanel;
        }

        private static void InstantiateDefaultTaskPanelTemplate(DateTime? cellTime, SchedulerTask task, TaskPanel taskPanel)
        {
            var leftBorder = new Label
                                 {
                                     ID = "l",
                                     Width = ASP.Unit.Pixel(3),
                                     Height = ASP.Unit.Percentage(100),
                                     BackColor = Color.FromKnownColor(KnownColor.Blue)
                                 };

            leftBorder.Style["float"] = "left";
            taskPanel.Controls.Add(leftBorder);

            var label = new Label
                            {
                                ID = "t", 
                                Text = task.StartTime.ToShortTimeString() + " - " + task.EndTime.ToShortTimeString()
                            };

            if (!cellTime.HasValue)
            {
                label.Style[HtmlTextWriterStyle.VerticalAlign] = "middle";
                label.Style["line-height"] = taskPanel.Height.ToString(NumberFormatInfo.InvariantInfo);
            }

            taskPanel.Controls.Add(label);
        }

        private void InitializeFullDayTaskRow(ASP.TableRow row, ASP.TableRowCollection rows, IDictionary<DateTime, Partition> partitions, bool dataBinding)
        {
            row.ID = "fdt";
            rows.Add(row);
            
            var days = GetDays();
            var fullDayTaskExists = false;
            var list = new List<Partition>(days.Count);
            
            foreach (var day in days)
            {
                Partition partition;
                if (partitions.TryGetValue(day, out partition))
                    fullDayTaskExists = fullDayTaskExists || (partition.FullDayTasks.Count > 0);
                list.Add(partition);
            }

            var index = 0;
            foreach(var day in days)
            {
                var partition = list[index];
                var cell = CreateTaskCell(index);

                var fullHeight = ASP.Unit.Percentage(100);
                var tasksExist = partition != null && partition.FullDayTasks.Count > 0;

                if (!tasksExist)
                {
                    if (fullDayTaskExists)
                        cell.Height = fullHeight;
                    else if (cell.Height.IsEmpty)
                        cell.Height = FullDayTaskCellHeight;
                }
                
                var eventArgs = new SchedulerCellEventArgs(cell, isHeader:false); // note : consider using an enum to differentiate what kind of Cell it is 
                OnCellCreated(eventArgs);
                row.Cells.Add(cell);

                var taskContainer = CreateTaskContainer(fullHeight, day.AddHours(VerifiedWorkday.StartHour), true);
                cell.Controls.Add(taskContainer);

                if (tasksExist)
                {
                    foreach (var task in partition.FullDayTasks)
                    {
                        var configuration = new TaskPanelConfiguration {Task = task};
                        var panel = CreateTaskPanel(null, configuration);
                        taskContainer.Controls.Add(panel);
                        if (!dataBinding) continue;
                        panel.DataBind();
                    }
                }

                ++index;
            }
        }

        private ASP.Style TaskCellStyle
        {
            get
            {
                if (_taskCellStyle == null)
                {
                    _taskCellStyle = new ASP.TableItemStyle();
                    var styles = VerifiedDayView.Styles;
                    _taskCellStyle.CopyFrom(styles.TaskCellStyle);
                    _taskCellStyle.MergeWith(styles.DefaultCellStyle);
                }
                
                return _taskCellStyle;
            }
        }

        
        private ASP.Unit RowHeight
        {
            get
            {
                if (!_rowHeight.HasValue)
                {
                    var style = VerifiedDayView.Styles.TaskContainerStyle;
                    _rowHeight = GetEffectiveUnit(TaskCellStyle.Height, style == null ? ASP.Unit.Empty : style.Height);
                }

                return _rowHeight.Value;
            }
        }

        private ASP.Unit GetEffectiveUnit(ASP.Unit first, ASP.Unit second)
        {
            if (!first.IsEmpty && !second.IsEmpty)
            {
                VerifyUnitType(first, second);
                return new ASP.Unit(first.Value + second.Value, first.Type);
            }
            
            if (first.IsEmpty && second.IsEmpty)
            {
                const int minuteHeight = 3;
                return ASP.Unit.Pixel(VerifiedDayView.Timeline.Granularity * minuteHeight);
            }

            return !first.IsEmpty ? first : second;
        }

        private static void VerifyUnitType(ASP.Unit first, ASP.Unit second)
        {
            if (first.Type == second.Type) return;
            throw new ArgumentException("Units should have same type.");
        }

        private SchedulerCell CreateTaskCell(int index)
        {
            var cell = new SchedulerTaskCell { ID = "c" + index };

            var days = GetDays();
            const double maxWidth = 99d;
            var columnWidth = ASP.Unit.Percentage(maxWidth / days.Count);

            var cellStyle = cell.ControlStyle;
            cellStyle.Width = columnWidth;
            cellStyle.MergeWith(TaskCellStyle);

            return cell;
        }

        private ASP.WebControl CreateTaskContainer(ASP.Unit rowHeight, DateTime startTime, bool isFullDayTaskContainer)
        {
            var taskContainer = new TaskContainer(startTime, isFullDayTaskContainer) {ID = "tc"};
            ApplyContainerStyle(taskContainer.ControlStyle, rowHeight);
            
            // adjust CssClass
            if (string.IsNullOrEmpty(taskContainer.CssClass))
                taskContainer.CssClass = TaskContainerCssClass;
            else
                taskContainer.CssClass += " " + TaskContainerCssClass;

            var droppable = new AspectDroppable();
            droppable.Dropping += TaskDropping;
            taskContainer.Aspects.Add(droppable);

            return taskContainer;
        }

        private void ApplyContainerStyle(ASP.Style taskContainerStyle, ASP.Unit height)
        {
            taskContainerStyle.Height = height;
            taskContainerStyle.MergeWith(VerifiedDayView.Styles.TaskContainerStyle);
        }

        private void TaskResizing(ASP.WebControl panel, AspectResizable.ResizingEventArgs e, string taskKey, double minuteSize)
        {
            var originalTop = int.Parse(panel.Style[HtmlTextWriterStyle.Top].Replace("px", string.Empty));
            var startDiff = Math.Round((e.Position.Y - originalTop) / minuteSize);
            
            var originalBottom = panel.Height.Value + originalTop;
            var endDiff = Math.Round((e.Position.Y + e.Dimensions.Height - originalBottom - 2*(int) panel.BorderWidth.Value) / minuteSize);

            var resizedTask = GetTaskById(taskKey);

            var key = taskKey.Substring(0, taskKey.LastIndexOf(TaskKeyDelimeter, StringComparison.Ordinal));
            var end = resizedTask.EndTime.AddMinutes(endDiff);
            var start = resizedTask.StartTime.AddMinutes(startDiff);

            var eventArgs = new TaskModifiedEventArgs(key, start, end);
                                
            OnTaskModified(eventArgs);
            e.Cancel = eventArgs.Cancel;
        }

        private SchedulerTask GetTaskById(string id)
        {
            return _tasks.Find(task => task.ID == id);
        }

        private void TaskDropping(object sender, AspectDroppable.DroppingEventArgs eventArgs)
        {
            var id = eventArgs.DraggedID;
            
            var idx = id.LastIndexOf(TaskPanelPrefix, StringComparison.Ordinal);
            if (idx == -1) return;

            var draggedTask = GetTaskById(id.Substring(idx + 2));
            if (draggedTask == null) return;

            var target = (TaskContainer)((AspectDroppable) sender).ParentControl.Control;
            var taskStart = target.StartTime;
            var duration = draggedTask.EndTime - draggedTask.StartTime;
            var taskEnd = target.FullDayTaskContainer
                              ? taskStart.Date.AddHours(VerifiedWorkday.EndHour)
                              : taskStart.AddMinutes(duration.TotalMinutes);

            var taskKey = draggedTask.ID;

            var key = taskKey.Substring(0, taskKey.LastIndexOf(TaskKeyDelimeter, StringComparison.Ordinal));
            var evtArgs = new TaskModifiedEventArgs(key, taskStart, taskEnd);
            
            OnTaskModified(evtArgs);
            eventArgs.Cancel = evtArgs.Cancel;
        }

        private bool IsAllDayTask(DateTime start, DateTime end)
        {
            return start.Date == end.Date &&
                   start.Hour == VerifiedWorkday.StartHour &&
                   end.Hour == VerifiedWorkday.EndHour;
        }

        private sealed class Partition
        {
            public IPositioner Positioner { get; set; }
            public ICollection<SchedulerTask> RegularTasks { get; set; }
            public ICollection<SchedulerTask> FullDayTasks { get; set; }
        }

        private IDictionary<DateTime, Partition> PartitionTasks(IEnumerable<SchedulerTask> tasks)
        {
            var partitions = new SortedDictionary<DateTime, Partition>(Comparer<DateTime>.Default);

            foreach (var task in tasks)
            {
                var endTime = task.EndTime;
                var startTime = task.StartTime;
                var date = startTime.Date;
                Partition partition;
                if (!partitions.TryGetValue(date, out partition))
                {
                    partition = new Partition
                    {
                        Positioner = new MinimalAreaPositioner(),
                        FullDayTasks = new List<SchedulerTask>(),
                        RegularTasks = new List<SchedulerTask>()
                    };
                    partitions.Add(date, partition);
                }

                if (IsAllDayTask(startTime, endTime))
                    partition.FullDayTasks.Add(task);
                else
                    partition.RegularTasks.Add(task);
            }

            foreach(var partition in partitions.Values)
            {
                ((List<SchedulerTask>)partition.RegularTasks).Sort((t1, t2) =>
                {
                    var result = t1.StartTime.CompareTo(t2.StartTime);
                    return result == 0 ? t1.Order.CompareTo(t2.Order) : result;
                });

                partition.Positioner.DistributeTasks(partition.RegularTasks);
            }

            return partitions;
        }

        private void CreateMonthView(IEnumerable dataSource, bool dataBinding)
        {
            ICollection<SchedulerTask> tasks;
            if (dataBinding)
            {
                ClearState();
                tasks = GetTasks(dataSource, false);
            }
            else
            {
                tasks = _tasks;
                _tasks = null;
            }

            var table = CreateTable();
            Controls.Add(table);

            var rows = table.Rows;
            var headerRowCreated = false;

            var rowIndex = 0;
            SchedulerRow row;
            var styles = VerifiedMonthView.Styles;
            var month = VerifiedMonthView.Month.Date;
            var templates = VerifiedMonthView.Templates;
            var workdayHours = VerifiedWorkday.EndHour = VerifiedWorkday.StartHour;
            foreach(var start in GetStartDays())
            {
                var day = start;
                var days = new List<DateTime>(DayPerWeek);
                while (day < start.AddDays(DayPerWeek))
                {
                    days.Add(day);
                    day = day.AddDays(1);
                }

                if (!headerRowCreated)
                {
                    row = CreateSchedulerRow();
                    row.ID = "hdr";
                    rows.Add(row);

                    var index = 0;
                    foreach (var date in days)
                    {
                        var cell = new SchedulerDateCell(index, date) { ID = "c" + index };
                        InitializeMonthViewHeaderCell(cell, date);
                        var eventArgs = new SchedulerCellEventArgs(cell, isHeader: true);
                        OnCellCreated(eventArgs);
                        row.Cells.Add(cell);
                        ++index;
                        if (!dataBinding) continue;
                        cell.DataBind();
                        OnCellDataBound(eventArgs);
                    }
                    headerRowCreated = true;
                }

                row = CreateSchedulerRow();
                row.ID = "r" + rowIndex;
                rows.Add(row);

                var cellIndex = 0;
                foreach (var date in days)
                {
                    var cell = new SchedulerDateCell(cellIndex, date) {ID = "c" + cellIndex};
                    cell.ControlStyle.CopyFrom(date.Month == month.Month ? styles.CellStyle : styles.CellOutOfRangeStyle);
                    cell.ControlStyle.MergeWith(styles.DefaultCellStyle);

                    var eventArgs = new SchedulerCellEventArgs(cell, isHeader:false);
                    OnCellCreated(eventArgs);

                    row.Cells.Add(cell);
                    if (dataBinding)
                    {
                        cell.DataBind();
                        OnCellDataBound(eventArgs);
                    }

                    var container = new NonBoundDateContainer { ID = "c" };
                    container.ControlStyle.CopyFrom(styles.ContainerStyle);
                    container.ControlStyle.MergeWith(styles.DefaultCellStyle);
                    cell.Controls.Add(container);

                    var template = VerifiedMonthView.Templates.DayTemplate;
                    if (template == null)
                    {
                        var label = new Label {ID="dl", CssClass="day", Text = date.ToString("MMMM dd", CultureInfo.CurrentUICulture)};
                        container.Controls.Add(label);
                    }
                    else
                    {
                        var placeholder = new ASP.PlaceHolder();
                        template.InstantiateIn(placeholder);
                        container.Controls.Add(placeholder);
                        if (dataBinding)
                            placeholder.DataBind();
                    }

                    template = templates.ReportTemplate;
                    
                    if (template == null)
                    {
                        var hours = workdayHours - ComputeUsedHours(tasks, date, workdayHours);
                        var label = new Label { ID = "rl", CssClass="report", Text = hours + " available hours" };
                        container.Controls.Add(label);
                    }
                    else
                    {
                        var placeholder = new ReportPlaceholder(FilterReportItemsByDate(tasks, date), date);
                        template.InstantiateIn(placeholder);
                        container.Controls.Add(placeholder);
                        if (dataBinding)
                            placeholder.DataBind();
                    }

                    ++cellIndex;
                }

                ++rowIndex;
            }
        }

        private static IEnumerable FilterReportItemsByDate(IEnumerable<SchedulerTask> tasks, DateTime date)
        {
            foreach (var task in tasks)
            {
                var end = task.EndTime;
                var start = task.StartTime;
                if (date < start.Date || date > end.Date) continue;
                yield return task.DataSource;
            }
        }

        private static double ComputeUsedHours(IEnumerable<SchedulerTask> tasks, DateTime date, double workdayHours)
        {
            double count = 0;
            foreach(var task in tasks)
            {
                var end = task.EndTime;
                var start = task.StartTime;
                if (date < start.Date || date > end.Date) continue;
                count += Math.Min((end - start).TotalHours, workdayHours);
            }
            return count;
        }

        /// <summary>
        /// Returns the StartDays based on the <see cref="Scheduler"/> configuration.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DateTime> GetStartDays()
        {
            var monthView = VerifiedMonthView;
            var date = monthView.Month.Date;
            var first = new DateTime(date.Year, date.Month, 1);
            var last = first.AddMonths(1).AddDays(-1);
            var startOfWeek = DayOfWeekToNumber(monthView.FirstDayOfWeek);
            var endOfWeek = (startOfWeek + (DayPerWeek - 1)) % DayPerWeek;

            var end = last.AddDays(GetOffset(endOfWeek, (int)last.DayOfWeek)).AddDays(1-DayPerWeek);
            var start = first.AddDays(-GetOffset((int)first.DayOfWeek, startOfWeek));
            
            while (start <= end)
            {
                yield return start;
                start = start.AddDays(DayPerWeek);
            }
        }

        private static int GetOffset(int dayOfWeek, int startOfWeek)
        {
            return (dayOfWeek + (DayPerWeek - startOfWeek)%DayPerWeek)%DayPerWeek;
        }

        private static int DayOfWeekToNumber(ASP.FirstDayOfWeek dayOfWeek)
        {
            if (dayOfWeek == ASP.FirstDayOfWeek.Default)
                return (int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;

            return (int) dayOfWeek;
        }

        private ASP.Table CreateTable()
        {
            var table = CreateChildTable();
            InitializeChildTable(table);
            return table;
        }

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            VerifyConfiguration();

            if (View == ViewMode.DayView)
                CreateDayView(dataSource, dataBinding);
            else
                CreateMonthView(dataSource, dataBinding);

            return 0;
        }

        private void CreateDayView(IEnumerable dataSource, bool dataBinding)
        {
            ICollection<SchedulerTask> tasks;
            if (dataBinding)
            {
                ClearState();
                tasks = GetTasks(dataSource, true);
            }
            else
            {
                tasks = _tasks;
                _tasks = null;
            }

            var partitions = PartitionTasks(tasks);

            var table = CreateTable();
            Controls.Add(table);

            var rows = table.Rows;
            var days = GetDays();

            InitializeHeaderRow(CreateSchedulerRow(), rows, days, dataBinding);
            InitializeFullDayTaskRow(CreateSchedulerRow(), rows, partitions, dataBinding);

            var index = -1;
            foreach (var range in GetTimelineRange())
            {
                ++index;
                var row = CreateSchedulerRow();
                row.ID = "r" + index;
                rows.Add(row);

                var rowHeight = RowHeight;
                var timeCell = CreateTimeCell(range, index, rowHeight); 
                row.Cells.Add(timeCell);
                if (dataBinding) timeCell.DataBind();

                var dayIndex = 0;
                foreach(var day in days)
                {
                    var cellTime = day.AddHours(range.Hours).AddMinutes(range.Minutes);
                    var cell = CreateTaskCell(dayIndex);
                    row.Cells.Add(cell);

                    var taskContainer = CreateTaskContainer(rowHeight, cellTime, false);
                    cell.Controls.Add(taskContainer);

                    Partition partition;
                    if (partitions.TryGetValue(day, out partition))
                    {
                        var positioner = partition.Positioner;
                        foreach (var task in partition.RegularTasks)
                        {
                            if (!IsTaskInRange(task, cellTime)) continue;
                            var taskPanel = CreateTaskPanel(cellTime,
                                                            new TaskPanelConfiguration
                                                                {
                                                                    Task = task,
                                                                    TaskCellBorderWidth = cell.BorderWidth,
                                                                    TaskContainerHeight = taskContainer.Height,
                                                                    Position = positioner.GetTaskPosition(task),
                                                                    TaskContainerBorderWidth = taskContainer.BorderWidth
                                                                });
                            taskContainer.Controls.Add(taskPanel);
                            if (!dataBinding) continue;
                            taskPanel.DataBind();
                        }
                    }

                    ++dayIndex;
                }
            }
        }

        #region [-- State Management --]

        private void ClearState()
        {
            _days = null;
            _tasks = null;
        }

        private void SaveTask(SchedulerTask task)
        {
            if (_tasks == null)
                _tasks = new List<SchedulerTask>();

            _tasks.Add(task);
        }

        protected override void LoadViewState(object savedState)
        {
            var pair = (Pair) savedState;
            var list = (ArrayList)pair.Second;

            if (_dayView == null)
                DayView = new DayViewConfiguration();

            if (_workday == null)
                Workday = new WorkdayConfiguration();

            if (_monthView == null)
                MonthView = new MonthViewConfiguration();

            ((IStateManager)DayView).LoadViewState(list[0]);
            ((IStateManager)Workday).LoadViewState(list[1]);
            ((IStateManager)MonthView).LoadViewState(list[2]);
            
            var count = list.Count;
            if (count > 3)
            {
                var savedTasks = list.GetRange(3, count - 3);
                count = savedTasks.Count;
                _tasks = new List<SchedulerTask>(count / 3);
                for (var index = 0; index + 2 < count; index += 3)
                    _tasks.Add(SchedulerTask.Create(index, savedTasks[index].ToString(), (DateTime)savedTasks[index + 1], (DateTime)savedTasks[index + 2], null));
            }
            else
                _tasks = new List<SchedulerTask>();

            base.LoadViewState(pair.First);
        }

        protected override object SaveViewState()
        {
            var hasTasks = _tasks != null;
            var list = hasTasks ? new ArrayList(_tasks.Count + 3) : new ArrayList();

            list.Add(_dayView != null ? ((IStateManager)_dayView).SaveViewState() : null);
            list.Add(_workday != null ? ((IStateManager)_workday).SaveViewState() : null);
            list.Add(_monthView != null ? ((IStateManager)_monthView).SaveViewState() : null);

            if (hasTasks)
            {
                foreach(var task in _tasks)
                {
                    list.Add(task.ID);
                    list.Add(task.StartTime);
                    list.Add(task.EndTime);
                }
            }
            
            return new Pair(base.SaveViewState(), list);
        }

        protected override void TrackViewState()
        {
            if (_dayView != null)
                ((IStateManager)_dayView).TrackViewState();

            if (_workday != null)
                ((IStateManager)_workday).TrackViewState();

            if (_monthView != null)
                ((IStateManager)_monthView).TrackViewState();

            base.TrackViewState();
        }

        #endregion

        
        private ICollection<DateTime> GetDays()
        {
            if (_days == null)
            {
                var numberOfDays = VerifiedDayView.NumberOfDays;
                _days = new List<DateTime>(numberOfDays);

                var startDate = VerifiedDayView.StartDate.Date;
                for (var index = 0; index < numberOfDays; ++index)
                    _days.Add(startDate.AddDays(index));
            }

            return _days;
        }

        private IEnumerable<TimeSpan> GetTimelineRange()
        {
            var workday = VerifiedWorkday;
            var granularity = VerifiedDayView.Timeline.Granularity;
            var endHour = new TimeSpan(workday.EndHour, 0, 0);
            var startHour = new TimeSpan(workday.StartHour, 0, 0);
            var timeSpan = new TimeSpan(0, granularity, 0);

            while(startHour < endHour)
            {
                yield return startHour;
                startHour = startHour.Add(timeSpan);
            }
        }

        private void VerifyConfiguration()
        {
            VerifyWorkdayConfiguration();
            if (View == ViewMode.DayView)
                VerifyDayViewConfiguration();
            else
                VerifyMonthViewConfiguration();
        }

        private void VerifyWorkdayConfiguration()
        {
            VerifiedWorkday = Workday != null ? Workday.VerifiedClone() : new WorkdayConfiguration();
        }

        private void VerifyDayViewConfiguration()
        {
            VerifiedDayView = DayView != null ? DayView.VerifiedClone() : new DayViewConfiguration();
        }

        private void VerifyMonthViewConfiguration()
        {
            VerifiedMonthView = MonthView != null ? MonthView.VerifiedClone() : new MonthViewConfiguration();
        }

        private DayViewConfiguration VerifiedDayView { get; set; }
        private WorkdayConfiguration VerifiedWorkday { get; set; }
        private MonthViewConfiguration VerifiedMonthView { get; set; }
    }
}
