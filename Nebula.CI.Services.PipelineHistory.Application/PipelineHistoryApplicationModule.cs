using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Nebula.CI.Services.PipelineHistory
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(PipelineHistoryBackgroundArgsModule))]
    public class PipelineHistoryApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            
            var configuration = context.Services.GetConfiguration();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<PipelineHistoryApplicationAutoMapperProfile>(validate: true);
            });
            
        }
    }
}
