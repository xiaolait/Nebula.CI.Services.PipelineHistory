using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace Nebula.CI.Services.PipelineHistory
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreMySQLModule))]
    public class PipelineHistoryEFCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PipelineHistoryDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL();
            });
        }
    }
}
