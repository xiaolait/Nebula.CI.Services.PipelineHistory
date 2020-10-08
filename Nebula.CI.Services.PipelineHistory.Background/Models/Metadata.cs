using System;

namespace Nebula.CI.Services.PipelineHistory
{
    public class Metadata
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        protected Metadata()
        {
        }

        public Metadata(string name, string @namespace)
        {
            Name = name;
            Namespace = @namespace;
        }
    }
}
