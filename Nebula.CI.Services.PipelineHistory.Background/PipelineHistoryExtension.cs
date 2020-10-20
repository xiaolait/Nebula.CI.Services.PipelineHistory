using System;
namespace Nebula.CI.Services.PipelineHistory
{
    public static class PipelineHistoryExtension
    {
        public static bool IsFinish(this PipelineHistory pipelineHistory)
        {
            if (pipelineHistory.Status.IsIn(new string[] { "Succeeded", "Failed" })) return true;
            else return false;
        }
    }
}
