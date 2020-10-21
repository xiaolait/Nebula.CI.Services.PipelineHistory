using System;
using Volo.Abp.Domain.Entities;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistory : AggregateRoot<int>
    {
        public int No { get; protected set; }
        public string Diagram { get; protected set; }
        public string Status { get; protected set; }
        public DateTime? StartTime { get; protected set; }
        public DateTime? CompletionTime { get; protected set; }
        public int Percent { get; protected set; }
        public string PipelineName { get; protected set; }
        public int PipelineId { get; protected set; }
        public string UserId { get; protected set; }

        protected PipelineHistory()
        {
        }

        public PipelineHistory(int no, string diagram, string pipelineName, int pipelineId, string userId)
        {
            No = no;
            Diagram = diagram;
            PipelineName = pipelineName;
            PipelineId = pipelineId;
            UserId = userId;
        }

        public PipelineHistory SetStatus(string status, DateTime? startTime, DateTime? completionTime, int percent, string log)
        {
            Status = status;
            StartTime = startTime;
            CompletionTime = completionTime;
            Percent = percent;
            ExtraProperties["Logs"] = log;
            return this;
        }

        public string GetLogs()
        {
            var property = "Logs";
            if (ExtraProperties.ContainsKey(property)) return ExtraProperties[property] as string;
            else return null;
        }
    }
}
