using Volo.Abp.EventBus;

namespace Nebula.CI.Services.PipelineHistory
{
    [EventName("PipelineRun")]
    public class PipelineRunEto
    {
        public int No { get; set; }
        public string Diagram { get; set; }
        public string PipelineName { get; set; }
        public int PipelineId { get; set; }
        public string UserId { get; set; }
    }
}