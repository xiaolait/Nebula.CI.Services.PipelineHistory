using System;


namespace Nebula.CI.Services.PipelineHistory
{
    public class Param
    {
        public string Name { get; protected set; }
        public string Value { get; protected set; }

        protected Param()
        {
        }

        public Param(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
