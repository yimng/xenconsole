using System;
using XenAPI;
using XenAdmin.Core;
using System.Collections.Generic;

namespace XenAdmin.Actions.VMActions
{
    public class ResetVirtualDiskAction : AsyncAction
    {

        public ResetVirtualDiskAction(Host host, VM vm)
            : base(vm.Connection, string.Format(Messages.ACTION_VM_RESET_TITLE, vm.Name))
        {
            this.Description = Messages.ACTION_PREPARING;
            this.Host = host;
            this.VM = vm;
        }

        protected override void Run()
        {
            this.Description = Messages.ACTION_VM_RESETING_VDI;

            try
            {
                List<VBD> vbds = VM.Connection.ResolveAll(this.VM.VBDs);
                
                foreach (VBD vbd in vbds)
                {
                    if (vbd.IsCDROM || vbd.empty)
                    {
                        continue;
                    }
                    Dictionary<String, String> args = new Dictionary<string, string>();
                    string vdiuuid = VM.Connection.Resolve<VDI>(vbd.VDI).uuid;
                    args.Add("vdiuuid", vdiuuid);
                    RelatedTask = XenAPI.Host.async_call_plugin(Host.Connection.Session, this.Host.opaque_ref, "ResetVDI.py", "ResetVDI", args);
                    PollToCompletion();
                }
            }
            catch (CancelledException)
            {
                this.Description = Messages.ACTION_VM_RESET_POWER_STATE_CANCEL;
                throw;
            }


            this.Description = Messages.ACTION_VM_RESETED_VDI;
        }
    }
}