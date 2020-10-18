using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryUpdatedHandler : ILocalEventHandler<EntityUpdatedEventData<PipelineHistory>>, ITransientDependency
    {

        public async Task HandleEventAsync(EntityUpdatedEventData<PipelineHistory> eventData)
        {
            Console.WriteLine($"pipelinehistory:{eventData.Entity.Id} id updated");
        }
    }
}