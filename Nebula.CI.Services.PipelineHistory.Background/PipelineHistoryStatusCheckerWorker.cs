using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryStatusCheckerWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private List<int> _pipelineHistoryIdList = new List<int>();
        private bool _isInited = false;

        public PipelineHistoryStatusCheckerWorker(
                AbpTimer timer,
                IServiceScopeFactory serviceScopeFactory
                ) : base(
                timer,
                serviceScopeFactory)
        {
            Timer.Period = 10000; //5 seconds
        }

        protected override async Task DoWorkAsync(
            PeriodicBackgroundWorkerContext workerContext)
        {
            /*
            if (!_isInited)
            {
                await InitAsync(workerContext);
                _isInited = true;
            }
            */
            for (int i = _pipelineHistoryIdList.Count - 1; i >= 0; i--)
            {
                try{
                    var pipelineHistory = await UpdatePipelineHistoryAsync(_pipelineHistoryIdList[i], workerContext);
                    if (pipelineHistory.IsFinish()) _pipelineHistoryIdList.RemoveAt(i);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }
        /*
        private async Task InitAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            var pipelineHistoryRepository = workerContext.ServiceProvider
                .GetRequiredService<IRepository<PipelineHistory, int>>();

            _pipelineHistoryIdList = await pipelineHistoryRepository.Where(p => !p.()).Select(p => p.Id).ToListAsync();
        }
        */

        private async Task<PipelineHistory> UpdatePipelineHistoryAsync(int pipelineHistoryId, PeriodicBackgroundWorkerContext workerContext)
        {
            var pipelineHistoryRepository = workerContext.ServiceProvider
                .GetRequiredService<IRepository<PipelineHistory, int>>();
            var pipelineRunService = workerContext.ServiceProvider
                .GetRequiredService<PipelineRunService>();

            var pipelineRunStatus = await pipelineRunService.GetStatusAsync(pipelineHistoryId.ToString());
            var pipelineHistory = await pipelineHistoryRepository.GetAsync(pipelineHistoryId);
            var nodeDic = Digram.CreateInstance(pipelineHistory.Diagram).NodeList.ToDictionary(t => t.Id);
            pipelineRunStatus.TaskRunStatusList.ForEach(t => {
                t.TaskAnnoName = nodeDic[t.ShapeId].AnnoName;
                t.ConfigUrl = nodeDic[t.ShapeId].ConfigUrl;
                t.ResultUrl = nodeDic[t.ShapeId].ResultUrl;
                nodeDic[t.ShapeId].Destination.ForEach(id => t.NextShapes.Add(id));

                if (t.Log == null) return;
                if (t.Log.StartTime == null) t.Log.ExecTime = null;
                else if (t.Log.CompletionTime == null) t.Log.ExecTime = DateTime.Now - t.Log.StartTime;
                else t.Log.ExecTime = t.Log.CompletionTime - t.Log.StartTime;
            });
            /*pipelineHistory
                .SetStatus(
                    pipelineRunStatus.Status,
                    pipelineRunStatus.StartTime,
                    pipelineRunStatus.CompletionTime,
                    pipelineRunStatus.Percent,
                    pipelineRunStatus.Logs)*/
            pipelineHistory
                .SetStatus(pipelineRunStatus.Status)
                .SetStartTime(pipelineRunStatus.StartTime)   
                .SetCompletionTime(pipelineRunStatus.CompletionTime)    
                .SetPercent(pipelineRunStatus.Percent) 
                .SetLogs(pipelineRunStatus.Logs)
                .SetMessage(pipelineRunStatus.Message);

            await pipelineHistoryRepository.UpdateAsync(pipelineHistory);
            return pipelineHistory;
        }

        private async Task DoPeriodWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            for (int i = _pipelineHistoryIdList.Count - 1; i >= 0; i--)
            {
                var pipelineHistoryId = _pipelineHistoryIdList[i];

                var pipelineHistoryRepository = workerContext.ServiceProvider
                    .GetRequiredService<IRepository<PipelineHistory, int>>();
                /*var pipelineProxy = workerContext.ServiceProvider
                    .GetRequiredService<IPipelineProxy>();*/
                var pipelineRunService = workerContext.ServiceProvider
                    .GetRequiredService<PipelineRunService>();


                var pipelineRunStatus = await pipelineRunService.GetStatusAsync(pipelineHistoryId.ToString());
                var pipelineHistory = await pipelineHistoryRepository.GetAsync(pipelineHistoryId);
                var nodeDic = Digram.CreateInstance(pipelineHistory.Diagram).NodeList.ToDictionary(t => t.Id);
                pipelineRunStatus.TaskRunStatusList.ForEach(t => {
                    t.TaskAnnoName = nodeDic[t.ShapeId].AnnoName;
                    t.ConfigUrl = nodeDic[t.ShapeId].ConfigUrl;
                    t.ResultUrl = nodeDic[t.ShapeId].ResultUrl;
                    nodeDic[t.ShapeId].Destination.ForEach(id => t.NextShapes.Add(id));

                    if (t.Log == null) return;
                    if (t.Log.StartTime == null) t.Log.ExecTime = null;
                    else if (t.Log.CompletionTime == null) t.Log.ExecTime = DateTime.Now - t.Log.StartTime;
                    else t.Log.ExecTime = t.Log.CompletionTime - t.Log.StartTime;
                });
                /*pipelineHistory
                    .SetStatus(
                        pipelineRunStatus.Status,
                        pipelineRunStatus.StartTime,
                        pipelineRunStatus.CompletionTime,
                        pipelineRunStatus.Percent,
                        pipelineRunStatus.Logs)*/
                pipelineHistory
                    .SetStatus(pipelineRunStatus.Status)
                    .SetStartTime(pipelineRunStatus.StartTime)   
                    .SetCompletionTime(pipelineRunStatus.CompletionTime)    
                    .SetPercent(pipelineRunStatus.Percent) 
                    .SetLogs(pipelineRunStatus.Logs)
                    .SetMessage(pipelineRunStatus.Message);

                await pipelineHistoryRepository.UpdateAsync(pipelineHistory);

                if (pipelineHistory.IsFinish())
                {
                    _pipelineHistoryIdList.RemoveAt(i);
                    /*
                    await pipelineProxy.UpdateStatusAsync(
                        pipelineHistory.PipelineId,
                        pipelineHistory.Status,
                        ((DateTime)(pipelineHistory.CompletionTime)).AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"));
                    */
                }

                //Console.WriteLine(JsonConvert.SerializeObject(logs));
            }
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