using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryDeletedHandler : ILocalEventHandler<EntityDeletedEventData<PipelineHistory>>, ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        public PipelineHistoryDeletedHandler(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task HandleEventAsync(EntityDeletedEventData<PipelineHistory> eventData)
        {
            Console.WriteLine($"pipelinehistory:{eventData.Entity.Id} is deleted");
            await _backgroundJobManager.EnqueueAsync(new PipelineHistoryDeletedArgs {
                Id = eventData.Entity.Id
            });
        }
    }
}