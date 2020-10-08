using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryStatusCheckerWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly List<int> _pipelineHistoryIdList = new List<int>();
        private readonly PipelineRunService _pipelineRunService;
        private readonly IRepository<PipelineHistory, int> _pipelineHistoryRepository;

        public PipelineHistoryStatusCheckerWorker(
                AbpTimer timer,
                IServiceScopeFactory serviceScopeFactory,
                PipelineRunService pipelineRunService
            ) : base(
                timer,
                serviceScopeFactory)
        {
            Timer.Period = 5000; //5 seconds
            _pipelineRunService = pipelineRunService;
        }

        protected override async Task DoWorkAsync(
            PeriodicBackgroundWorkerContext workerContext)
        {
            for(int i= _pipelineHistoryIdList.Count-1; i>=0; i--)
            {
                var pipelineHistoryId = _pipelineHistoryIdList[i];
                var pipelineRunStatus = await _pipelineRunService.GetStatusAsync(pipelineHistoryId.ToString());

                var pipelineHistory = await _pipelineHistoryRepository.GetAsync(pipelineHistoryId);
                var nodeDic = Digram.CreateInstance(pipelineHistory.Diagram).NodeList.ToDictionary(t => t.Id);
                pipelineRunStatus.TaskRunStatusList.ForEach(t => {
                    t.TaskAnnoName = nodeDic[t.ShapeId].AnnoName;
                    if (t.Log == null) return;
                    if (t.Log.StartTime == null) t.Log.ExecTime = null;
                    else if (t.Log.CompletionTime == null) t.Log.ExecTime = DateTime.Now - t.Log.StartTime;
                    else t.Log.ExecTime = t.Log.CompletionTime - t.Log.StartTime;
                });
                pipelineHistory
                    .SetStatus(
                        pipelineRunStatus.Status,
                        pipelineRunStatus.StartTime,
                        pipelineRunStatus.CompletionTime,
                        pipelineRunStatus.Percent,
                        pipelineRunStatus.Logs);

                await _pipelineHistoryRepository.UpdateAsync(pipelineHistory);

                if (pipelineHistory.Status == "Succeeded" || pipelineHistory.Status == "Failed")
                {
                    _pipelineHistoryIdList.RemoveAt(i);
                }

                //Console.WriteLine(JsonConvert.SerializeObject(logs));
            }

            /*
            var userRepository = workerContext
                .ServiceProvider
                .GetRequiredService<IUserRepository>();

            //Do the work
            await userRepository.UpdateInactiveUserStatusesAsync();
            */
        }

        public void AddPipelineHistoryId(int id)
        {
            _pipelineHistoryIdList.Add(id);
        }

        public void RemovePipelineHistoryId(int id)
        {
            _pipelineHistoryIdList.Remove(id);
        }
    }
}