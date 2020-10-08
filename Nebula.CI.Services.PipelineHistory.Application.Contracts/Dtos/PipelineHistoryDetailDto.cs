using System;
namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryDetailDto
    {
        public int Id { get; set; }
        public int No { get; set; }
        public string Diagram { get; set; }
        public string Status { get; set; }
        public string StartTime { get; set; }
        public string CompletionTime { get; set; }
        public int Percent { get; set; }
        public string PipelineName { get; set; }
        public int PipelineId { get; set; }
        public string Logs { get; set; }
    }
}
