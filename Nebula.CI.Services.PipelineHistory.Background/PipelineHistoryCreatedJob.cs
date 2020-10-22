using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryCreatedJob : BackgroundJob<PipelineHistoryCreatedArgs>, ITransientDependency
    {
        private readonly PipelineHistoryStatusCheckerWorker _pipelineHistoryStatusCheckerWorker;
        private readonly PipelineRunService _pipelineRunService;

        public PipelineHistoryCreatedJob(PipelineHistoryStatusCheckerWorker pipelineHistoryStatusCheckerWorker, PipelineRunService pipelineRunService)
        {
            _pipelineHistoryStatusCheckerWorker = pipelineHistoryStatusCheckerWorker;
            _pipelineRunService = pipelineRunService;
        }

        public override void Execute(PipelineHistoryCreatedArgs args)
        {
            Console.WriteLine($"PipelineRun:{args.Id} is being Created in background");

            var diagram = Digram.CreateInstance(args.Diagram);
            var pipelineRun = new PipelineRun(args.Id.ToString(), "ci-nebula");
            foreach (var node in diagram.NodeList)
            {
                var @params = new List<Param>();
                var inputResources = new List<PipelineTaskInputResource>();
                var runAfter = new List<string>();
                node.Property?.Params.ForEach(r => @params.Add(new Param(r.Name, r.Value)));
                node.Property?.Resources?.Inputs.ForEach(r => inputResources.Add(new PipelineTaskInputResource(r.Name, r.Resource)));
                node.Source.ForEach(r => runAfter.Add(r));
                pipelineRun.AddTask(node.Id, node.Name, @params, inputResources, runAfter);
            }
            _pipelineRunService.CreateAsync(pipelineRun);

            Console.WriteLine($"PipelineRun:{args.Id} is Created in background");

            _pipelineHistoryStatusCheckerWorker.AddPipelineHistoryId(args.Id);

            Console.WriteLine($"PipelineRun:{args.Id} status check id added to background");
        }
    }
}
