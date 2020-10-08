using System;
using System.Collections.Generic;


namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineTaskResources
    {
        protected List<PipelineTaskInputResource> _inputs = new List<PipelineTaskInputResource>();
        public IReadOnlyCollection<PipelineTaskInputResource> Inputs => _inputs.AsReadOnly();

        protected PipelineTaskResources()
        {
        }

        public PipelineTaskResources(List<PipelineTaskInputResource> inputResources)
        {
            inputResources.ForEach(r => _inputs.Add(new PipelineTaskInputResource(r.Name, r.Resource)));
        }
    }
}
