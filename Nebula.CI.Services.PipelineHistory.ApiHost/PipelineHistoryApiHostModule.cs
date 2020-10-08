using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Nebula.CI.Services.PipelineHistory
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(PipelineHistoryApplicationModule))]
    [DependsOn(typeof(PipelineHistoryEFCoreModule))]
    [DependsOn(typeof(PipelineHistoryEFCoreDbMigrationsModule))]
    [DependsOn(typeof(PipelineHistoryBackgroundModule))]
    public class PipelineHistoryApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(PipelineHistoryApplicationModule).Assembly);
            });

            configureSwaggerService(context);
        }

        
        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var migrator = app.ApplicationServices.GetRequiredService<EFCorePipelineHistoryDbSchemaMigrator>();
            migrator.MigrateAsync().Wait();
        }
        

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors(option =>
            {
                option.AllowAnyHeader();
                option.AllowAnyMethod();
                option.AllowAnyOrigin();
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseConfiguredEndpoints();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Nebula.CI Service API");
            });
        }

        private void configureSwaggerService(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Bugdigger Service API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
        }
    }
}
