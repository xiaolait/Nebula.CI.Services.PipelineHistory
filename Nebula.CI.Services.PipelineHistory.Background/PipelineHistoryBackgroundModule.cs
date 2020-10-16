using System;
using k8s;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;

namespace Nebula.CI.Services.PipelineHistory
{
    [DependsOn(typeof(AbpBackgroundJobsModule))]
    [DependsOn(typeof(AbpBackgroundWorkersModule))]
    public class PipelineHistoryBackgroundModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var k8sServer = configuration["K8sServer"];

            KubernetesClientConfiguration config;
            if(k8sServer != null && k8sServer != string.Empty)
            {
                config = new KubernetesClientConfiguration { Host = k8sServer };
            }
            else
            {
                config = KubernetesClientConfiguration.BuildDefaultConfig();
            }

            context.Services.AddTransient(typeof(Kubernetes), provider => {
                //var config = new KubernetesClientConfiguration { Host = "http://172.18.67.167:8001/" };
                //var config = KubernetesClientConfiguration.BuildDefaultConfig();
                return new Kubernetes(config);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.AddBackgroundWorker<PipelineHistoryStatusCheckerWorker>();
        }
    }
}
