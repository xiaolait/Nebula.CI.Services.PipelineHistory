using System;
using System.Collections.Generic;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineRun
    {
        public string ApiVersion { get { return "tekton.dev/v1beta1"; } }

        public string Kind { get { return "PipelineRun"; } }

        public Metadata Metadata { get; protected set; }

        public PipelineRunSpec Spec { get; protected set; }

        protected PipelineRun()
        {

        }

        public PipelineRun(string name, string @namespace)
        {
            Metadata = new Metadata(name, @namespace);
            Spec = new PipelineRunSpec(name);
        }

        public PipelineRun AddTask(string taskName, string task, List<Param> @params, List<PipelineTaskInputResource> inputResources, List<string> runAfter)
        {
            Spec.PipelineSpec.AddTask(Metadata.Name, taskName, task, @params, inputResources, runAfter);

            return this;
        }
    }
}
