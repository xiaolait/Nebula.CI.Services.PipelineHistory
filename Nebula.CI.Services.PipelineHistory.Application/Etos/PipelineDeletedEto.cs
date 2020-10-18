using Volo.Abp.EventBus;

namespace Nebula.CI.Services.PipelineHistory
{
    [EventName("PipelineDeleted")]
    public class PipelineDeletedEto
    {
        public int Id { get; set; }
    }
}