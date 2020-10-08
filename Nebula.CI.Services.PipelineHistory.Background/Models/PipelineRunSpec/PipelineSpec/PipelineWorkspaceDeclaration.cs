using System;


namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineWorkspaceDeclaration
    {
        public string Name { get; protected set; }

        protected PipelineWorkspaceDeclaration()
        {
        }

        public PipelineWorkspaceDeclaration(string name)
        {
            Name = name;
        }
    }
}
