using System;
using System.Collections.Generic;

namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineRunSpec
    {
        public PipelineSpec PipelineSpec { get; protected set; }

        protected List<WorkspaceBinding> _workspaces = new List<WorkspaceBinding>();
        public IReadOnlyCollection<WorkspaceBinding> Workspaces => _workspaces.AsReadOnly();

        protected List<PipelineResourceBinding> _resources = new List<PipelineResourceBinding>();
        public IReadOnlyCollection<PipelineResourceBinding> Resources => _resources.AsReadOnly();

        protected PipelineRunSpec()
        {
        }

        public PipelineRunSpec(string name)
        {
            PipelineSpec = new PipelineSpec(name);
            _workspaces.Add(new WorkspaceBinding(name));
        }

        public PipelineRunSpec AddTask(string pipelineRunName, string taskName, string task, List<Param> @params, List<PipelineTaskInputResource> inputResources, List<string> runAfter)
        {
            PipelineSpec.AddTask(pipelineRunName, taskName, task, @params, inputResources, runAfter);
            inputResources.ForEach(r => _resources.Add(new PipelineResourceBinding(r.Resource)));

            return this;
        }
    }
}
