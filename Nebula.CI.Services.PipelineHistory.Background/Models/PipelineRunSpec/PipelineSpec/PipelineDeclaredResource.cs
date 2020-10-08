using System;
namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineDeclaredResource
    {
        public string Name { get; protected set; }

        public bool Optional { get { return false; } }

        protected PipelineDeclaredResource()
        {
        }

        public PipelineDeclaredResource(string name)
        {
            Name = name;
        }
    }
}
