using System;


namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineResourceBinding
    {
        public string Name { get; protected set; }

        public PipelineResourceRef ResourceRef { get; protected set; }

        protected PipelineResourceBinding()
        {

        }

        public PipelineResourceBinding(string name)
        {
            Name = name;
            ResourceRef = new PipelineResourceRef(name);
        }

    }
}
