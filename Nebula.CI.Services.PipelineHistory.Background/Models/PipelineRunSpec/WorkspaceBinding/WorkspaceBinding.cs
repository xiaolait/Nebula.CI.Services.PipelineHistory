using System;


namespace Nebula.CI.Services.PipelineHistory
{
    public class WorkspaceBinding
    {
        public string Name { get; protected set; }

        public PersistentVolumeClaimVolumeSource PersistentVolumeClaim { get; protected set; }

        protected WorkspaceBinding()
        {
        }

        public WorkspaceBinding(string name)
        {
            Name = name;
            PersistentVolumeClaim = new PersistentVolumeClaimVolumeSource(name);
        }
    }
}
