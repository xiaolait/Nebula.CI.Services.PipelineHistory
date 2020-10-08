using System;


namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineResourceRef
    {
        public string Name { get; protected set; }

        protected PipelineResourceRef()
        {
        }

        public PipelineResourceRef(string name)
        {
            Name = name;
        }
    }
}
