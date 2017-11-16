using System;
using XenAPI;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using XenAdmin.SettingsPanels;
using XenAdmin.Actions.VMActions;
using XenAdmin.Core;
using XenAdmin.Commands;
using XenAdmin.Actions;

namespace XenAdmin.Dialogs.WarningDialogs
{
    public partial class HasRunningVMsWarningDialog :Form
    {
        private List<VM> running_vms;
        private List<SR> sr_need_reattach;
        private List<XenRef<VBD>> allVBDs;
        private List<VM> no_tools_vms = new List<VM>();
        private List<AsyncAction> s_actions = new List<AsyncAction>();
        private List<AsyncAction> r_actions = new List<AsyncAction>();
        public HasRunningVMsWarningDialog()
        {
            InitializeComponent();
        }
        public HasRunningVMsWarningDialog(List<VM> vms,List<SR> srs,List<XenRef<VBD>> vbds)
        {
            running_vms = vms;
            sr_need_reattach = srs;
            allVBDs = vbds;
            InitializeComponent();
            ComboBoxInit();
        }
        public void ComboBoxInit()
        {
            foreach (VM v in running_vms)
            {
                if (v.is_control_domain)
                {
                    continue;
                }
                comboBox1.Items.Add(v.name_label);
            }
            comboBox1.SelectedIndex = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            foreach (VM v in running_vms)
            {
                if (v.is_control_domain)
                {
                    continue;
                }
                if (v.allowed_operations.Contains(vm_operations.suspend))
                {
                    continue;
                }
                else 
                {
                    no_tools_vms.Add(v);
                }
             }
            if (no_tools_vms.Count!=0)
            {
                new NeedToolsWarningDialog(no_tools_vms).ShowDialog();
            }
             /*           if (no_tools_vms.Count == 0)
                        { 
                            foreach (VM v in running_vms)
                            {
                                s_actions.Add(new VMSuspendAction(v));
                            }
                            MultipleAction suspend_actions = new MultipleAction(running_vms[0].Connection,"","","",s_actions,true);
                            suspend_actions.RunExternal(running_vms[0].Connection.Session);
                            foreach (SR sr in sr_need_reattach)
                            {
                                if (sr.PBDs.Count < 1)
                                    continue;

                                //CA-176935, CA-173497 - we need to run Unplug for the master last - creating a new list of hosts where the master is always the last
                                var allPBDRefsToNonMaster = new List<XenRef<PBD>>();
                                var allPBDRefsToMaster = new List<XenRef<PBD>>();

                                var master = Helpers.GetMaster(sr.Connection);

                                foreach (var pbdRef in sr.PBDs)
                                {
                                    var pbd = sr.Connection.Resolve(pbdRef);
                                    if (pbd != null)
                                    {
                                        if (pbd.host != null)
                                        {
                                            var host = sr.Connection.Resolve(pbd.host);
                                            if (master != null && host != null && host.uuid == master.uuid)
                                            {
                                                allPBDRefsToMaster.Add(pbdRef);
                                            }
                                            else
                                            {
                                                allPBDRefsToNonMaster.Add(pbdRef);
                                            }
                                        }
                                    }
                                }

                                var allPBDRefs = new List<XenRef<PBD>>();
                                allPBDRefs.AddRange(allPBDRefsToNonMaster);
                                allPBDRefs.AddRange(allPBDRefsToMaster);

                                foreach (XenRef<PBD> pbd in allPBDRefs)
                                {
                                    XenAPI.PBD.unplug(sr.Connection.Session, pbd.opaque_ref);
                                }
                                foreach (XenRef<PBD> pbd in allPBDRefs)
                                {
                                    XenAPI.PBD.plug(sr.Connection.Session, pbd.opaque_ref);
                                }
                            }
                            foreach (VM v in running_vms)
                            {
                                r_actions.Add(new VMResumeAction(v,VMOperationCommand.WarningDialogHAInvalidConfig, VMOperationCommand.StartDiagnosisForm));
                                // XenAPI.VM.async_resume(v.Connection.Session,v.opaque_ref,true,true);
                            }
                            MultipleAction resume_actions = new MultipleAction(running_vms[0].Connection, "", "", "", r_actions, true);
                            resume_actions.RunAsync();
                        }
                        else
                        {
                            foreach (SR sr in sr_need_reattach)
                            {
                                XenAPI.SR.remove_from_other_config(sr.Connection.Session, sr.opaque_ref, "scheduler");
                            }
                            foreach (XenRef<VBD> v in allVBDs)
                            {
                                if (running_vms[0].Connection.Resolve<VBD>(v).type == vbd_type.CD)
                                {
                                    continue;
                                }
                                if (XenAPI.VBD.get_qos_algorithm_params(running_vms[0].Connection.Session, v.opaque_ref).ContainsKey("class"))
                                {
                                    XenAPI.VBD.remove_from_qos_algorithm_params(running_vms[0].Connection.Session, v.opaque_ref, "class");
                                }
                                if (XenAPI.VBD.get_qos_algorithm_params(running_vms[0].Connection.Session, v.opaque_ref).ContainsKey("sched"))
                                {
                                    XenAPI.VBD.remove_from_qos_algorithm_params(running_vms[0].Connection.Session, v.opaque_ref, "sched");
                                }
                                if (XenAPI.VBD.get_qos_algorithm_type(running_vms[0].Connection.Session, v.opaque_ref) != null)
                                {
                                    XenAPI.VBD.set_qos_algorithm_type(running_vms[0].Connection.Session, v.opaque_ref, "");
                                }
                            }
                            new NeedToolsWarningDialog(no_tools_vms).ShowDialog();
                        }*/

        }
        private void button2_Click(object sender, EventArgs e)
        {
            /*            foreach (SR sr in sr_need_reattach)
                        {
                            XenAPI.SR.remove_from_other_config(sr.Connection.Session, sr.opaque_ref, "scheduler");
                        }
                        foreach (XenRef<VBD> v in allVBDs)
                        {
                            if (running_vms[0].Connection.Resolve<VBD>(v).type == vbd_type.CD)
                            {
                                continue;
                            }
                            if (XenAPI.VBD.get_qos_algorithm_params(running_vms[0].Connection.Session, v.opaque_ref).ContainsKey("class"))
                            {
                                XenAPI.VBD.remove_from_qos_algorithm_params(running_vms[0].Connection.Session, v.opaque_ref, "class");
                            }
                            if (XenAPI.VBD.get_qos_algorithm_params(running_vms[0].Connection.Session, v.opaque_ref).ContainsKey("sched"))
                            {
                                XenAPI.VBD.remove_from_qos_algorithm_params(running_vms[0].Connection.Session, v.opaque_ref, "sched");
                            }
                            if (XenAPI.VBD.get_qos_algorithm_type(running_vms[0].Connection.Session, v.opaque_ref) != null)
                            {
                                XenAPI.VBD.set_qos_algorithm_type(running_vms[0].Connection.Session, v.opaque_ref, "");
                            }
                        }*/
            this.Close();
        }
    }
}
