using System;

namespace Nebula.CI.Services.PipelineHistory
{
    public class WorkspacePipelineTaskBinding
    {
        public string Name { get { return "local"; } }

        public string Workspace { get; protected set; }

        protected WorkspacePipelineTaskBinding()
        {
        }

        public WorkspacePipelineTaskBinding(string workspace)
        {
            Workspace = workspace;
        }
    }
}
