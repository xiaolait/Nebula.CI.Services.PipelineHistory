using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryEFCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PipelineHistoryDbMigrationsContext>();
        }
    }
}
