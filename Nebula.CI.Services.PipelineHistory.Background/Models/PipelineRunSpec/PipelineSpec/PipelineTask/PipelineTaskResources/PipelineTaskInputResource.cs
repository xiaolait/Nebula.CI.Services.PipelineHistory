using System;


namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineTaskInputResource
    {
        public string Name { get; protected set; }

        public string Resource { get; protected set; }

        protected PipelineTaskInputResource()
        {
        }

        public PipelineTaskInputResource(string name, string resource)
        {
            Name = name;
            Resource = resource;
        }
    }
}
