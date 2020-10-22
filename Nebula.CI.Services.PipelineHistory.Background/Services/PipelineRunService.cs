using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineRunService : ITransientDependency
    {
        private readonly Kubernetes _client;

        public PipelineRunService(Kubernetes client)
        {
            _client = client;
        }

        public void CreateAsync(PipelineRun pipelineRun)
        {
            var pvc = CreatePVC(pipelineRun.Metadata.Name, pipelineRun.Metadata.Namespace);
            _client.CreateNamespacedPersistentVolumeClaimAsync(pvc, pipelineRun.Metadata.Namespace).Wait();

            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.NullValueHandling = NullValueHandling.Ignore;
            jss.Formatting = Formatting.Indented;
            jss.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            var pr = JObject.Parse(JsonConvert.SerializeObject(pipelineRun,Formatting.None,jss));
            _client.CreateNamespacedCustomObjectAsync(pr, "tekton.dev", "v1beta1", pipelineRun.Metadata.Namespace, "pipelineruns").Wait();
        }

        public void DeleteAsync(int id)
        {
            _client.DeleteNamespacedCustomObjectAsync("tekton.dev", "v1beta1", "ci-nebula", "pipelineruns", id.ToString()).Wait();
            _client.DeleteNamespacedPersistentVolumeClaimAsync(id.ToString(), "ci-nebula").Wait();
        }

        public async Task<PipelineRunStatus> GetStatusAsync(string pipelineRunName)
        {
            var log = new PipelineRunStatus() { Status = "", TaskRunStatusList = new List<TaskRunStatus>() };
            var pipeLine = new JObject();
            try
            {
                pipeLine = await _client.GetNamespacedCustomObjectAsync("tekton.dev", "v1beta1", "ci-nebula", "pipelineruns", pipelineRunName) as JObject;
            }
            catch (Exception)
            {
                return null;
            }

            var pipeLineStatus = pipeLine.Value<JObject>("status");
            //log.Status = pipeLineStatus.Value<JArray>("conditions").First.Value<string>("reason");
            var pStatus = pipeLineStatus.Value<JArray>("conditions").First.Value<string>("status");
            if (pStatus == "True")
            {
                log.Status = "Succeeded";
            }
            else if (pStatus == "False")
            {
                log.Status = "Failed";
            }
            else
            {
                log.Status = pipeLineStatus.Value<JArray>("conditions").First.Value<string>("reason");
            }
            log.CompletionTime = pipeLineStatus.Value<DateTime?>("completionTime")?.ToUniversalTime();
            log.StartTime = pipeLineStatus.Value<DateTime?>("startTime")?.ToUniversalTime();

            var tasks = pipeLineStatus.Value<JObject>("pipelineSpec").Value<JArray>("tasks");
            foreach (var task in tasks)
            {
                var taskRunStatus = new TaskRunStatus();
                taskRunStatus.Task = task.Value<JObject>("taskRef").Value<string>("name");
                taskRunStatus.ShapeId = task.Value<string>("name");
                log.TaskRunStatusList.Add(taskRunStatus);
            }
            
            var taskList = tasks.Select(s => s.Value<string>("name")).ToList();
            var taskRuns = pipeLineStatus.Value<JObject>("taskRuns");

            if (log.Status == "Failed" && taskRuns == null)
            {
                log.Message = pipeLineStatus.Value<JArray>("conditions").First.Value<string>("message");
            }

            if (taskRuns == null) return log;

            foreach (var taskrunstatus in log.TaskRunStatusList)
            {
                foreach (var taskRun in taskRuns)
                {
                    var taskRunContent = taskRun.Value;
                    var taskName = taskRunContent.Value<string>("pipelineTaskName");
                    if (taskName == taskrunstatus.ShapeId)
                    {
                        var taskStatus = taskRunContent.Value<JObject>("status");
                        var podName = taskStatus.Value<string>("podName");
                        var startTime = taskStatus.Value<DateTime?>("startTime")?.ToUniversalTime();
                        var completionTime = taskStatus.Value<DateTime?>("completionTime")?.ToUniversalTime();
                        var containers = taskStatus.Value<JArray>("steps");
                        var conditions = taskStatus.Value<JArray>("conditions");
                        var status = (conditions.First as JObject)?.Value<string>("reason");
                        var taskRunLog = new TaskRunLog
                        {
                            StartTime = startTime,
                            CompletionTime = completionTime,
                            Status = status
                        };
                        foreach (var container in containers)
                        {
                            var containerName = container.Value<string>("container");
                            var logStream = await _client.ReadNamespacedPodLogAsync(podName, "ci-nebula", containerName);
                            taskRunLog.Content += await new StreamReader(logStream).ReadToEndAsync();
                        }
                        taskrunstatus.Log = taskRunLog;
                    }

                }
            }

            return log;
        }

        private V1PersistentVolumeClaim CreatePVC(string name, string namesapce)
        {
            var per = new k8s.Models.V1PersistentVolumeClaim();
            per.ApiVersion = "v1";
            per.Kind = "PersistentVolumeClaim";
            per.Metadata = new k8s.Models.V1ObjectMeta();
            per.Metadata.Name = name;
            per.Metadata.NamespaceProperty = namesapce;
            per.Metadata.Annotations = new Dictionary<string, string>();
            per.Metadata.Annotations.Add("volume.beta.kubernetes.io/storage-class", "managed-nfs-storage");
            per.Spec = new k8s.Models.V1PersistentVolumeClaimSpec();
            per.Spec.AccessModes = new List<string>();
            per.Spec.AccessModes.Add("ReadWriteMany");
            per.Spec.Resources = new k8s.Models.V1ResourceRequirements();
            per.Spec.Resources.Requests = new Dictionary<string, k8s.Models.ResourceQuantity>();
            per.Spec.Resources.Requests.Add("storage", new k8s.Models.ResourceQuantity("1Gi"));
            return per;
        }
    }
}
