using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Nebula.CI.Services.PipelineHistory
{
    [ConnectionStringName("PipelineHistory")]
    public class PipelineHistoryDbMigrationsContext : AbpDbContext<PipelineHistoryDbMigrationsContext>
    {
        public PipelineHistoryDbMigrationsContext(DbContextOptions<PipelineHistoryDbMigrationsContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */
            builder.ConfigurePipelineStore();
        }
    }
}