using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryUpdatingHandler : ILocalEventHandler<EntityUpdatingEventData<PipelineHistory>>, ITransientDependency
    {

        public async Task HandleEventAsync(EntityUpdatingEventData<PipelineHistory> eventData)
        {
            await Task.Delay(1);
            Console.WriteLine($"updating pipelinehistory:{eventData.Entity.Id}");
        }
    }
}