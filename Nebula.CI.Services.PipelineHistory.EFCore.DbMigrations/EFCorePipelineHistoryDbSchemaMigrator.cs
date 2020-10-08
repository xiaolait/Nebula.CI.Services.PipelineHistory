using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Nebula.CI.Services.PipelineHistory
{
    public class EFCorePipelineHistoryDbSchemaMigrator : ITransientDependency
    {
        private readonly PipelineHistoryDbMigrationsContext _dbContext;

        public EFCorePipelineHistoryDbSchemaMigrator(PipelineHistoryDbMigrationsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}

