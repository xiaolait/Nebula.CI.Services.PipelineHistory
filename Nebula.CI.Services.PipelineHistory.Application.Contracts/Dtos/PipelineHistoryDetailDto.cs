using System;
namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryDetailDto : PipelineHistoryBaseDto
    {
        public string Diagram { get; set; }
        public string Message { get; set; }
        public string Logs { get; set; }
    }
}
