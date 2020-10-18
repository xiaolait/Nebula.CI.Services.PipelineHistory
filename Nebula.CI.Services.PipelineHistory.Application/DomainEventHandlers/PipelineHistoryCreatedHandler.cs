using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryCreatedHandler : ILocalEventHandler<EntityCreatedEventData<PipelineHistory>>, ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        public PipelineHistoryCreatedHandler(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<PipelineHistory> eventData)
        {
            Console.WriteLine($"pipelinehistory:{eventData.Entity.Id} is created");

            await _backgroundJobManager.EnqueueAsync(new PipelineHistoryCreatedArgs {
                Id = eventData.Entity.Id,
                Diagram = eventData.Entity.Diagram
            });
        }
    }
}