using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineDeletedHandler : IDistributedEventHandler<PipelineDeletedEto>, ITransientDependency
    {
        private readonly IRepository<PipelineHistory, int> _pipelineHistoryRepository;

        public PipelineDeletedHandler(IRepository<PipelineHistory, int> pipelineHistoryRepository)
        {
            _pipelineHistoryRepository = pipelineHistoryRepository;
        }

        public async Task HandleEventAsync(PipelineDeletedEto eventData)
        {
            Console.WriteLine($"recv pipeline:{eventData.Id} is deleted");
            await _pipelineHistoryRepository.DeleteAsync(s => s.PipelineId == eventData.Id);
        }
    }
}