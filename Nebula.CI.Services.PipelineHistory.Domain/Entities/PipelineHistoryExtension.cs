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

        public static bool IsRunning(this PipelineHistory pipelineHistory)
        {
            if ((pipelineHistory.StartTime != null) && (pipelineHistory.CompletionTime == null)) return true;
            else return false;
        }

        public static string GetLogs(this PipelineHistory pipelineHistory)
        {
            return pipelineHistory.GetProperty<string>("Logs");
        }

        public static PipelineHistory SetLogs(this PipelineHistory pipelineHistory, string logs)
        {
            return pipelineHistory.SetProperty<string>("Logs", logs);
        }

        public static string GetMessage(this PipelineHistory pipelineHistory)
        {
            return pipelineHistory.GetProperty<string>("Message");
        }

        public static PipelineHistory SetMessage(this PipelineHistory pipelineHistory, string logs)
        {
            return pipelineHistory.SetProperty<string>("Message", logs);
        }
    }
}