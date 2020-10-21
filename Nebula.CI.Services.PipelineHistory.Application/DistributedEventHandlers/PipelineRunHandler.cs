using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineRunHandler : IDistributedEventHandler<PipelineRunEto>, ITransientDependency
    {
        private readonly IRepository<PipelineHistory, int> _pipelineHistoryRepository;

        public PipelineRunHandler(IRepository<PipelineHistory, int> pipelineHistoryRepository)
        {
            _pipelineHistoryRepository = pipelineHistoryRepository;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(PipelineRunEto eventData)
        {
            Console.WriteLine($"recv pipeline :{eventData.PipelineId} run cmd");
            await _pipelineHistoryRepository.InsertAsync(
                new PipelineHistory(
                    eventData.No, 
                    eventData.Diagram, 
                    eventData.PipelineName, 
                    eventData.PipelineId, 
                    eventData.UserId
                    )
                );
        }
    }
}