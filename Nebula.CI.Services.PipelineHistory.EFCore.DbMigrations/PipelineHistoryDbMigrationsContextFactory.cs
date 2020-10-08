using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryDbMigrationsContextFactory : IDesignTimeDbContextFactory<PipelineHistoryDbMigrationsContext>
    {
        public PipelineHistoryDbMigrationsContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<PipelineHistoryDbMigrationsContext>()
                .UseMySql(configuration.GetConnectionString("mysql"));

            return new PipelineHistoryDbMigrationsContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
