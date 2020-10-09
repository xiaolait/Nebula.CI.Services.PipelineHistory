using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Nebula.CI.Services.PipelineHistory
{
    [ConnectionStringName("PipelineHistory")]
    public class PipelineHistoryDbContext : AbpDbContext<PipelineHistoryDbContext>
    {
        public DbSet<PipelineHistory> PipelineHistories { get; set; }

        public PipelineHistoryDbContext(DbContextOptions<PipelineHistoryDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePipelineStore();
        }
    }
}

