using System;


namespace Nebula.CI.Services.PipelineHistory
{
    public class PersistentVolumeClaimVolumeSource
    {
        public string ClaimName { get; protected set; }

        public bool ReadOnly { get { return false; } }

        protected PersistentVolumeClaimVolumeSource()
        {
        }

        public PersistentVolumeClaimVolumeSource(string claimName)
        {
            ClaimName = claimName;
        }
    }
}
