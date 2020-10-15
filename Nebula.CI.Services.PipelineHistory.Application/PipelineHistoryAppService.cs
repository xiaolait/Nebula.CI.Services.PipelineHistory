using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryAppService : ApplicationService, IPipelineHistoryAppService
    {
        private readonly IRepository<PipelineHistory, int> _pipelineHistoryRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public IPipelineProxy PipelineProxy { get; set; }

        public PipelineHistoryAppService(IRepository<PipelineHistory, int> pipelineHistoryRepository, IBackgroundJobManager backgroundJobManager)
        {
            _pipelineHistoryRepository = pipelineHistoryRepository;
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task CreateAsync(PipelineHistoryCreateDto input)
        {
            var pipelineHistory = await _pipelineHistoryRepository.InsertAsync(new PipelineHistory(input.No, input.Diagram, input.PipelineId));

            await UnitOfWorkManager.Current.SaveChangesAsync();

            await _backgroundJobManager.EnqueueAsync(new PipelineHistoryCreatedArgs {
                Id = pipelineHistory.Id,
                Diagram = pipelineHistory.Diagram
            });
        }

        public async Task DeleteAsync(int id)
        {
            await _pipelineHistoryRepository.DeleteAsync(id);
        }

        public async Task<PipelineHistoryDetailDto> GetDetailAsync(int id)
        {
            var pipelineHistory = await _pipelineHistoryRepository.GetAsync(id);

            return ObjectMapper.Map<PipelineHistory, PipelineHistoryDetailDto>(pipelineHistory);
        }

        public async Task<List<PipelineHistoryBaseDto>> GetListAsync(int pipelineId)
        {
            var pipelineHistories = await _pipelineHistoryRepository.Where(s => s.PipelineId == pipelineId).ToListAsync();

            return ObjectMapper.Map<List<PipelineHistory>, List<PipelineHistoryBaseDto>>(pipelineHistories);
        }

        [Authorize]
        public async Task<List<PipelineHistoryBaseDto>> GetRunningListAsync()
        {
            var pipelineIdList = await PipelineProxy?.GetIdListAsync() ?? new List<int>();
            var pipelineHistories = await _pipelineHistoryRepository.Where(s => (pipelineIdList.Contains(s.PipelineId)) && (s.StartTime != null) && (s.CompletionTime == null)).ToListAsync();
            return ObjectMapper.Map<List<PipelineHistory>, List<PipelineHistoryBaseDto>>(pipelineHistories??new List<PipelineHistory>());
        }
    }
}
