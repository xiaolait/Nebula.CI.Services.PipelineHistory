using System;
using System.Collections.Generic;


namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineSpec
    {
        protected List<PipelineWorkspaceDeclaration> _workspaces = new List<PipelineWorkspaceDeclaration>();
        public IReadOnlyCollection<PipelineWorkspaceDeclaration> Workspaces => _workspaces.AsReadOnly();

        protected List<PipelineDeclaredResource> _resources = new List<PipelineDeclaredResource>();
        public IReadOnlyCollection<PipelineDeclaredResource> Resources => _resources.AsReadOnly();

        protected List<PipelineTask> _tasks = new List<PipelineTask>();
        public IReadOnlyCollection<PipelineTask> Tasks => _tasks.AsReadOnly();
        

        protected PipelineSpec()
        {
        }

        public PipelineSpec(string name)
        {
            _workspaces.Add(new PipelineWorkspaceDeclaration(name));
        }

        public PipelineSpec AddTask(string pipelineRunName, string taskName, string task, List<Param> @params, List<PipelineTaskInputResource> inputResources, List<string> runAfter)
        {
            _tasks.Add(new PipelineTask(pipelineRunName, taskName, task, @params, inputResources,runAfter));
            inputResources.ForEach(r => _resources.Add(new PipelineDeclaredResource(r.Resource)));

            return this;
        }
    }
}
