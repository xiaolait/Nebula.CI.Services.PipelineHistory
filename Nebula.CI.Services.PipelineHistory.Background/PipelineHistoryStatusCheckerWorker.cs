using System;
using System.Collections.Concurrent;
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
        public ConcurrentQueue<int> PipelineHistoryCreatedQueue = new ConcurrentQueue<int>();
        public ConcurrentQueue<int> PipelineHistoryTobeDeleteQueue = new ConcurrentQueue<int>();
        private HashSet<int> _pipelineHistoryTobeCheckSet = new HashSet<int>();
        //private List<int> _pipelineHistoryIdList = new List<int>();
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

            int pipelineHistoryId = 0;
            while(PipelineHistoryCreatedQueue.TryDequeue(out pipelineHistoryId)) {
                _pipelineHistoryTobeCheckSet.Add(pipelineHistoryId);
            }

            var pipelineHistoryService = workerContext.ServiceProvider
                .GetRequiredService<PipelineHistoryService>();
            var pipelineHistoryTobeUncheckSet = new HashSet<int>();
            foreach(var pipelineHistoryTobeCheck in  _pipelineHistoryTobeCheckSet)
            {
                try{
                    var pipelineHistory = await UpdatePipelineHistoryAsync(pipelineHistoryTobeCheck, workerContext);
                    if (pipelineHistory.IsFinish()) {
                        pipelineHistoryService.DeletePipelinerun(pipelineHistory.Id);
                        pipelineHistoryTobeUncheckSet.Add(pipelineHistoryTobeCheck);
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            _pipelineHistoryTobeCheckSet.RemoveWhere(p => pipelineHistoryTobeUncheckSet.Contains(p));

            while(PipelineHistoryTobeDeleteQueue.TryDequeue(out pipelineHistoryId)) {
                if (_pipelineHistoryTobeCheckSet.Contains(pipelineHistoryId)) {
                    pipelineHistoryService.DeletePipelinerun(pipelineHistoryId);
                    _pipelineHistoryTobeCheckSet.Remove(pipelineHistoryId);
                }
                pipelineHistoryService.DeletePVC(pipelineHistoryId);
            }


            /*
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
            */
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
            var pipelineHistoryService = workerContext.ServiceProvider
                .GetRequiredService<PipelineHistoryService>();

            var pipelineRunStatus = await pipelineHistoryService.GetStatusAsync(pipelineHistoryId.ToString());
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
        
        /*
        public void AddPipelineHistoryId(int id)
        {
            _pipelineHistoryIdList.Add(id);
        }

        public void RemovePipelineHistoryId(int id)
        {
            _pipelineHistoryIdList.Remove(id);
        }
        */
    }
}