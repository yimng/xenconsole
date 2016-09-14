namespace XenAdmin.Wizards.ReplicationWizard_Pages
{
    public class ReplicatedVmState
    {
        public ReplicatedVmState(string uuid, bool isrunning)
        {
            this.VmUuid = uuid;
            this.IsVMRunning = isrunning;
        }

        public string VmUuid;
        public bool IsVMRunning;
    }
}