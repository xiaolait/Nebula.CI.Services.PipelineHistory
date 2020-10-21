using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryUpdatedHandler : ILocalEventHandler<EntityUpdatedEventData<PipelineHistory>>, ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;

        public PipelineHistoryUpdatedHandler(IDistributedEventBus distributedEventBus)
        {
            _distributedEventBus = distributedEventBus;
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<PipelineHistory> eventData)
        {
            Console.WriteLine($"pipelinehistory:{eventData.Entity.Id} id updated");
            if (eventData.Entity.IsFinish()) {
                await _distributedEventBus.PublishAsync(new PipelineUpdateStatusEto(){
                    Id = eventData.Entity.PipelineId,
                    Status = eventData.Entity.Status,
                    Time = ((DateTime)(eventData.Entity.CompletionTime)).AddHours(8).ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
        }
    }
}