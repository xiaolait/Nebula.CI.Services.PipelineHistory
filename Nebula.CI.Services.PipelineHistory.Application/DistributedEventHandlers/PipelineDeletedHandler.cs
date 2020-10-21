using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineDeletedHandler : IDistributedEventHandler<PipelineDeletedEto>, ITransientDependency
    {
        private readonly IRepository<PipelineHistory, int> _pipelineHistoryRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PipelineDeletedHandler(IRepository<PipelineHistory, int> pipelineHistoryRepository, IServiceScopeFactory serviceScopeFactory)
        {
            _pipelineHistoryRepository = pipelineHistoryRepository;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(PipelineDeletedEto eventData)
        {
            Console.WriteLine($"recv pipeline:{eventData.Id} is deleted");
            await _pipelineHistoryRepository.DeleteAsync(s => s.PipelineId == eventData.Id, true);
        }
    }
}