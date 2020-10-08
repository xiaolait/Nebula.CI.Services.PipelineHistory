using System;

namespace Nebula.CI.Services.PipelineHistory
{
    public class TaskRef
    {
        public string Name { get; protected set; }

        protected TaskRef()
        {
        }

        public TaskRef(string name)
        {
            Name = name;
        }
    }
}
