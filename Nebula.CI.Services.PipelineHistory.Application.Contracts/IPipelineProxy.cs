using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Nebula.CI.Services.PipelineHistory
{
    public interface IPipelineProxy
    {
        Task<List<int>> GetIdListAsync();

        Task UpdateStatusAsync(int id, string status, string time);
    }
}
