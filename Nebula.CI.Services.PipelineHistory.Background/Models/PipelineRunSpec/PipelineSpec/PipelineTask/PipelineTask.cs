using System;
using System.Collections.Generic;


namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineTask
    {
        public string Name { get; protected set; }

        public TaskRef TaskRef { get; protected set; }

        protected List<Param> _params = new List<Param>();
        public IReadOnlyCollection<Param> Params => _params.AsReadOnly();

        public PipelineTaskResources Resources { get; protected set; }

        protected List<WorkspacePipelineTaskBinding> _workspaces = new List<WorkspacePipelineTaskBinding>();
        public IReadOnlyCollection<WorkspacePipelineTaskBinding> Workspaces => _workspaces.AsReadOnly();

        protected List<string> _runAfter = new List<string>();
        public IReadOnlyCollection<string> RunAfter => _runAfter.AsReadOnly();

        protected PipelineTask()
        {
        }

        public PipelineTask(string pipelineRunName, string taskName, string task, List<Param> @params, List<PipelineTaskInputResource> inputResources, List<string> runAfter)
        {
            Name = taskName;
            TaskRef = new TaskRef(task);
            @params.ForEach(p => _params.Add(new Param(p.Name, p.Value)));
            Resources = new PipelineTaskResources(inputResources);
            _workspaces.Add(new WorkspacePipelineTaskBinding(pipelineRunName));
            runAfter.ForEach(p => _runAfter.Add(p) );
        }
    }
}
