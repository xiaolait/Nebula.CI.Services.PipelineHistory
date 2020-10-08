using System;
namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryCreateDto
    {
        public int No { get; set; }
        public string Diagram { get; set; }
        public string PipelineName { get; set; }
        public int PipelineId { get; set; }
    }
}
