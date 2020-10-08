using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Nebula.CI.Services.PipelineHistory
{
    public interface IPipelineHistoryAppService : IApplicationService
    {
        Task CreateAsync(PipelineHistoryCreateDto input);

        Task<List<PipelineHistoryBaseDto>> GetListAsync(int pipelineId);

        Task<PipelineHistoryDetailDto> GetDetailAsync(int id);

        Task DeleteAsync(int id);
    }
}
