using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

                var logs =  JsonConvert.SerializeObject(TaskRunStatusList);
                var taskRunStatusList = JsonConvert.DeserializeObject<List<TaskRunStatus>>(logs);
                taskRunStatusList.ForEach(t => {
                    if (t?.Log?.StartTime != null) t.Log.StartTime = ((DateTime)t.Log.StartTime).AddHours(8);
                    if (t?.Log?.CompletionTime != null) t.Log.CompletionTime = ((DateTime)t.Log.CompletionTime).AddHours(8);
                });
                return JsonConvert.SerializeObject(taskRunStatusList, Formatting.Indented, 
                    new IsoDateTimeConverter(){DateTimeFormat = "yyyy-MM-dd HH:mm:ss"}
                );
            }
        }
    }


}
