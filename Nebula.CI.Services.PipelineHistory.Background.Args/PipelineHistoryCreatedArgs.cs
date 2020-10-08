using System;
namespace Nebula.CI.Services.PipelineHistory
{
    public class PipelineHistoryCreatedArgs
    {
        public int Id { get; set; }
        public string Diagram { get; set; }
    }
}
