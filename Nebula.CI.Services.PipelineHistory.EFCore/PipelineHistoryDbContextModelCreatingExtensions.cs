using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Nebula.CI.Services.PipelineHistory
{
    public static class PipelineHistoryDbContextModelCreatingExtensions
    {
        public static void ConfigurePipelineStore(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<PipelineHistory>(c =>
            {
                c.ToTable("PipelineHistory");
                c.ConfigureByConvention();
            });
        }
    }
}
