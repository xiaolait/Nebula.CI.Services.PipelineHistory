using System;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryDeletedJob : BackgroundJob<PipelineHistoryDeletedArgs>, ITransientDependency
    {
        private readonly PipelineHistoryStatusCheckerWorker _pipelineHistoryStatusCheckerWorker;
        private readonly PipelineRunService _pipelineRunService;

        public PipelineHistoryDeletedJob(PipelineHistoryStatusCheckerWorker pipelineHistoryStatusCheckerWorker, PipelineRunService pipelineRunService)
        {
            _pipelineHistoryStatusCheckerWorker = pipelineHistoryStatusCheckerWorker;
            _pipelineRunService = pipelineRunService;
        }

        public override void Execute(PipelineHistoryDeletedArgs args)
        {
            _pipelineRunService.DeleteAsync(args.Id).Wait();

            _pipelineHistoryStatusCheckerWorker.RemovePipelineHistoryId(args.Id);
        }
    }
}
