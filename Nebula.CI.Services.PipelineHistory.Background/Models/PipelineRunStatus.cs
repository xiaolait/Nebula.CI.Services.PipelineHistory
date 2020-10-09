using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nebula.CI.Services.PipelineHistory
{
    public class TaskRunStatus
    {
        public string Task { get; set; }//taskref
        public string TaskAnnoName { get; set; }
        public string ShapeId { get; set; }//taskname
        public string ConfigUrl { get; set; }
        public string ResultUrl { get; set; }
        public TaskRunLog Log { get; set; }
        public List<string> NextShapes { get; set; } = new List<string>();
        
    }

    public class TaskRunLog
    {

        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public TimeSpan? ExecTime { get; set; }
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
                return (taskSuccessedNum * 100) / TaskRunStatusList.Count;
            } }

        public string Logs { get
            {
                return JsonConvert.SerializeObject(TaskRunStatusList);
            }
        }
    }


}
