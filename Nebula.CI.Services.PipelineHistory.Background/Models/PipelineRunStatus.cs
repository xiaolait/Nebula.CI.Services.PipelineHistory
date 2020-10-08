using System;
using System.Collections.Generic;

namespace Nebula.CI.Services.PipelineHistory
{
    public class TaskRunStatus
    {
        public string Task { get; set; }//taskref
        public string TaskAnnoName { get; set; } = "";
        public string ShapeId { get; set; }//taskname
        public TaskRunLog Log { get; set; } = null;
    }

    public class TaskRunLog
    {

        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? CompletionTime { get; set; }
    }

    public class PipelineRunStatus
    {
        public string Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? CompletionTime { get; set; }

        public List<TaskRunStatus> TaskRunStatusList { get; set; }

        public int Percent { get
            {
                int taskSuccessedNum = 0;
                TaskRunStatusList.ForEach(t => {
                    if (t.Log.Status == "Succeeded") taskSuccessedNum++;
                });
                return (int)((taskSuccessedNum * 1.0) / TaskRunStatusList.Count);
            } }
    }


}
