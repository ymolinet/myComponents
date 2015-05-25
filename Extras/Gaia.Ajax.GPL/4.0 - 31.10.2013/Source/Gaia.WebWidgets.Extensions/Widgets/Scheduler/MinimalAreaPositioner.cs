/*******************************************************************
 * Gaia Ajax - Ajax Control Library for ASP.NET  
 * Copyright (C) 2008 - 2009 Gaiaware AS
 * All rights reserved. 
 * This program is distributed under either GPL version 3 
 * as published by the Free Software Foundation or the
 * Gaia Commercial License version 1 as published by Gaiaware AS
 * read the details at http://gaiaware.net/product/dual-licensing 
 ******************************************************************/

namespace Gaia.WebWidgets.Extensions
{
    using System;
    using System.Collections.Generic;
    using ASP = System.Web.UI.WebControls;

    sealed class MinimalAreaPositioner : IPositioner
    {
        sealed class TaskArea
        {
            private readonly List<TaskArea> _overlappings = new List<TaskArea>();

            public double Span { get; internal set; }
            public double Column { get; internal set; }
            public SchedulerTask Task { get; internal set; }

            public IEnumerable<TaskArea> Overlappings
            {
                get { return _overlappings; }
            }

            public int OverlappingsCount
            {
                get { return _overlappings.Count; }
            }

            public void AddOverlapping(TaskArea position)
            {
                _overlappings.Add(position);
            }
        }

        private int _columnCount;
        private List<List<TaskArea>> _positions;

        public TaskPosition GetTaskPosition(SchedulerTask task)
        {
            if (_positions == null)
                throw new InvalidOperationException("DistributeTasks should be called before applying the position.");

            TaskArea position = null;
            
            foreach(var column in _positions)
            {
                position = column.Find(item => item.Task == task);
                if (position == null) continue;
                break;
            }
            
            if (position == null)
                throw new ArgumentOutOfRangeException("task", "Task was not distributed");

            return new TaskPosition
                       {
                           Width = ASP.Unit.Percentage(100*position.Span/_columnCount),
                           Left = ASP.Unit.Percentage(Math.Round((position.Column - 1)*100/_columnCount))
                       };
        }

        public void DistributeTasks(ICollection<SchedulerTask> tasks)
        {
            _columnCount = 0;
            var count = tasks.Count;
            _positions = new List<List<TaskArea>>(count);

            foreach (var task in tasks)
                DistributeTask(task, count);

            JustifyColumns();
        }

        private void JustifyColumns()
        {
            for(var index = _columnCount - 2; index >= 0; --index)
            {
                var column = _positions[index];

                foreach(var position in column)
                {
                    if (position.OverlappingsCount > 0) continue;
                    JustifyColumn(index, position);
                }
            }
        }

        private void JustifyColumn(int index, TaskArea position)
        {
            var resizables = new List<TaskArea> { position };

            for (var idx = index - 1; idx >= 0; --idx)
            {
                var searchColumn = _positions[idx];
                var columnHasResizableTask = false;
                foreach(var overlapping in searchColumn)
                {
                    if (!CanResize(overlapping, resizables)) continue;
                    columnHasResizableTask = true;
                    resizables.Add(overlapping);
                }
                
                if (!columnHasResizableTask) break;
            }

            for (var idx = index + 1; idx < _columnCount; ++idx)
            {
                var searchColumn = _positions[idx];
                foreach(var overlapping in searchColumn)
                {
                    if (position.Task.OverlapsWith(overlapping.Task))
                        break;
                }
            }

            var overlappingIndex = FindOverlappingColumn(index + 1, position.Task);
            var count = resizables.Count;
            var availableSpace = (overlappingIndex == -1 ? _columnCount : overlappingIndex) - index - 1;
            if (availableSpace < 1) return;
            var increase = (double)availableSpace/count;

            for(var idx = 0; idx < count; ++idx)
            {
                var resizable = resizables[idx];
                resizable.Span += increase;
                resizable.Column += (count - idx - 1)*increase;
            }
        }

        private int FindOverlappingColumn(int startIndex, SchedulerTask task)
        {
            for (var idx = startIndex; idx < _columnCount; ++idx)
            {
                var searchColumn = _positions[idx];
                foreach (var overlapping in searchColumn)
                {
                    if (task.OverlapsWith(overlapping.Task))
                        return idx;
                }
            }

            return -1;
        }

        private static bool CanResize(TaskArea overlapping, ICollection<TaskArea> resizables)
        {
            if (overlapping.OverlappingsCount != resizables.Count) return false;
            foreach (var item in overlapping.Overlappings)
            {
                if (!resizables.Contains(item)) return false;
            }
            return true;
        }

        private void DistributeTask(SchedulerTask task, int taskCount)
        {
            // find first non-overlapping column
            int index;
            var overlappings = new List<TaskArea>(taskCount);
            for (index = 0; index < _columnCount; ++index)
            {
                var foundColumn = true;
                var column = _positions[index];

                foreach(var taskPosition in column)
                {
                    if (!task.OverlapsWith(taskPosition.Task)) continue;
                    foundColumn = false;
                    overlappings.Add(taskPosition);
                }

                if (foundColumn) break;
            }

            TaskArea position;

            if (index == _columnCount)
            {
                // all column are occupied by overlapping tasks
                // insert new column
                ++_columnCount;
                position = new TaskArea { Task = task, Column = _columnCount, Span = 1 };
                _positions.Add(new List<TaskArea> { position });
            }
            else
            {
                // found a non-overlapping position in one of the columns
                position = new TaskArea { Task = task, Column = index + 1, Span = 1 };
                _positions[index].Add(position);
            }

            overlappings.ForEach(overlapping => overlapping.AddOverlapping(position));
        }
    }
}