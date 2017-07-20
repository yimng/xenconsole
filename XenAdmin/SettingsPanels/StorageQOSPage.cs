using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XenAdmin.Actions;
using XenAPI;
using XenAdmin.Core;
using XenAdmin.Dialogs.WarningDialogs;

namespace XenAdmin.SettingsPanels
{
    public partial class StorageQOSPage : UserControl, IEditPage
    {
        private VM vm;
        private bool origin_level_checked;
        private bool origin_rate_checked;
        private int origin_limit_level = 8;
        private int limit_level;
        private Double origin_limit_rate;
        private Double limit_rate;
        private bool _ValidToSave = true;
        bool _ValidToSave1, _ValidToSave2,_ValidToSave3=true,_ValidToSave4=true;
        public StorageQOSPage()
        {
            InitializeComponent();
            Text = Messages.STORAGEQOSPAGE_TEXT;
        }
        private void Limit_Level_Init()
        {
            VM vm = this.vm;
            comboBox1.Items.Add("0");
            comboBox1.Items.Add("1");
            comboBox1.Items.Add("2");
            comboBox1.Items.Add("3");
            comboBox1.Items.Add("4");
            comboBox1.Items.Add("5");
            comboBox1.Items.Add("6");
            comboBox1.Items.Add("7");
            List<XenRef<VBD>> allVBDs = new List<XenRef<VBD>>();
            allVBDs = vm.VBDs;
            foreach (XenRef<VBD> v in allVBDs)
            {
                if (vm.Connection.Resolve<VBD>(v).type==vbd_type.CD)
                {
                    continue;
                }
                if (XenAPI.VBD.get_qos_algorithm_params(vm.Connection.Session,v.opaque_ref).ContainsKey("class"))
                {
                    label4.Visible = false;
                    for (int i = comboBox1.Items.Count - 1; i >= 0; i--)
                    {
                        if (comboBox1.Items[i].ToString().Equals(XenAPI.VBD.get_qos_algorithm_params(vm.Connection.Session, v.opaque_ref)["class"]))
                        {
                            comboBox1.SelectedIndex = i;
                            checkBox2.Checked = true;
                            origin_level_checked = checkBox2.Checked;
                            origin_limit_level = i;
                        }
                    }
                }
                else
                {
                    label4.Visible = true;
                    checkBox2.Checked = false;
                    origin_level_checked = checkBox2.Checked;
                }
            }
        }
        private void Limit_Rate_Init()
        {
            VM vm = this.vm;
            IEnumerable<SR> allsrs = vm.SRs;
            int i = 0;//记录所有sr中非iso类型的sr个数
            int j = 0;//记录iso类型的sr个数
            foreach (SR sr in allsrs)
            {
                if (sr.GetSRType(true) ==XenAPI.SR.SRTypes.iso )
                {
                    continue;
                }
                if (sr.GetSRType(true)==SR.SRTypes.nfs||sr.GetSRType(true)==SR.SRTypes.ext)
                {
                    i++;
                }
                if (i == allsrs.Count() - j && i!=0)
                {
                    checkBox1.Enabled = false;
                    textBox1.Enabled = false;
                    label8.Visible = true;
                }
                if(i>0&&i<allsrs.Count()-j)
                {
                    label9.Visible = true;
                }
            }
            if (vm.other_config.ContainsKey("io_limit"))
            {
                if (vm.other_config["io_limit"] == "0")
                {
                    checkBox1.Checked = false;
                    origin_rate_checked = false;
                    label5.Visible = true;
                    textBox1.Text = "";
                }
                else
                {
                    checkBox1.Checked = true;
                    origin_rate_checked = true;
                    label5.Visible = false;
                    textBox1.Text = vm.other_config["io_limit"];
                }               
            }
            else
            {
                checkBox1.Checked = false;
                origin_rate_checked = false;
                label5.Visible = true;
            }
        }
        public bool HasChanged
        {
            get { return HasLimitLevelChanged()||HasLimitRateChanged(); }
        }
        public bool HasLimitLevelChanged()
        {
            if (checkBox2.Checked== false && origin_level_checked == true)
            {
                return true;
            }
            if (comboBox1.Text!=""&&comboBox1.Text!=null)
            {
                limit_level = int.Parse(comboBox1.Text);
                if ((!origin_limit_level.Equals(limit_level)) && checkBox2.Checked == true)
                {
                    return true;
                }
            }
            return false;    
       }
        public bool HasLimitRateChanged()
        {
            if (checkBox1.Checked==false && origin_rate_checked == true)
            {
                return true;
            }
            if (vm.other_config.ContainsKey("io_limit"))
            {
                if (vm.other_config["io_limit"] == "" || vm.other_config["io_limit"] == null)
                {
                    origin_limit_rate = 0;
                }
                else
                {
                    origin_limit_rate = Double.Parse(vm.other_config["io_limit"]);
                }
                if (textBox1.Text!=""&& textBox1.Text!=null)
                {
                    limit_rate = Double.Parse(textBox1.Text);
                }
                if (!origin_limit_rate.Equals(limit_rate) && checkBox1.Checked == true)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (textBox1.Text != null && textBox1.Text != "" && checkBox1.Checked == true)
                {
                    return true;
                }
                return false;
            }
        }

        public Image Image
        {
            get
            {
                return Properties.Resources._000_MigrateVM_h32bit_16;
            }
        }

        public string SubText
        {
            get
            {
                return Messages.STORAGEQOSPAGE_SUBTEXT;
            }
        }

        public bool ValidToSave
        {
            get
            {
                if (checkBox1.Checked == true && (textBox1.Text == null || textBox1.Text == ""))
                {
                    _ValidToSave1 = false;
                    label2.Visible = true;
                }
                else
                {
                    _ValidToSave1 = true;
                }
                if (checkBox2.Checked == true && (comboBox1.Text == null || comboBox1.Text == ""))
                {
                    _ValidToSave2 = false;
                    label2.Visible = true;
                }
                else
                {
                    _ValidToSave2 = true;
                }
                _ValidToSave=_ValidToSave1 && _ValidToSave2 && _ValidToSave3 && _ValidToSave4;
                return _ValidToSave;
            }
        }

        public void Cleanup() { }

        public AsyncAction SaveSettings()
        {
            Dictionary<String, String> args = new Dictionary<string, string>();
            args.Add("vm_uuid", vm.uuid);
            if (checkBox1.Checked == false)
            {
                if (vm.other_config.ContainsKey("io_limit"))
                {
                    XenAPI.VM.remove_from_other_config(vm.Connection.Session, vm.opaque_ref, "io_limit");
                }
                XenAPI.VM.add_to_other_config(vm.Connection.Session, vm.opaque_ref, "io_limit", "0");
                args.Add("mbps", "0");
            }
            else if (HasLimitRateChanged()&&checkBox1.Checked==true)
            {
                if (vm.other_config.ContainsKey("io_limit"))
                {
                    XenAPI.VM.remove_from_other_config(vm.Connection.Session, vm.opaque_ref, "io_limit");
                }
                XenAPI.VM.add_to_other_config(vm.Connection.Session, vm.opaque_ref, "io_limit", textBox1.Text);
                args.Add("mbps", textBox1.Text); 
            }
            if (checkBox2.Checked==false && origin_level_checked == true)
            {
                List<XenRef<VBD>> allVBDs = new List<XenRef<VBD>>();
                allVBDs = vm.VBDs;                   
                foreach (XenRef<VBD> v in allVBDs)
                {
                    if (vm.Connection.Resolve<VBD>(v).type == vbd_type.CD)
                    {
                        continue;
                    }
                    if (XenAPI.VBD.get_qos_algorithm_params(vm.Connection.Session, v.opaque_ref).ContainsKey("class"))
                    {
                        XenAPI.VBD.remove_from_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "class");
                        XenAPI.VBD.add_to_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "class", "0");
                    }
                    if (XenAPI.VBD.get_qos_algorithm_params(vm.Connection.Session, v.opaque_ref).ContainsKey("sched"))
                    {
                        XenAPI.VBD.remove_from_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "sched");
                    }
                    if (XenAPI.VBD.get_qos_algorithm_type(vm.Connection.Session, v.opaque_ref) != null)
                    {
                        XenAPI.VBD.set_qos_algorithm_type(vm.Connection.Session, v.opaque_ref,"");
                    }
                }                              
            }
            else if (checkBox2.Checked == true)
            {
                List<XenRef<VBD>> allVBDs = new List<XenRef<VBD>>();
                IEnumerable<SR> allSRs;
                allVBDs = vm.VBDs;
                allSRs = vm.SRs;
                List<SR> sr_need_reattach = new List<SR>();
                List<VM> running_vms = new List<VM>();
                allVBDs =vm.VBDs;
                foreach (SR sr in allSRs)
                {
                    if (sr.GetSRType(true) == XenAPI.SR.SRTypes.iso)
                    {
                        continue;
                    }
                    if (IsSrCfq(sr))
                    {
                        continue;
                    }
                    else
                    {
                        if(XenAPI.SR.get_other_config(sr.Connection.Session,sr.opaque_ref).ContainsKey("scheduler"))
                        {
                            XenAPI.SR.remove_from_other_config(sr.Connection.Session, sr.opaque_ref, "scheduler");
                            XenAPI.SR.add_to_other_config(sr.Connection.Session, sr.opaque_ref, "scheduler", "cfq");
                        }
                        else
                        {
                            XenAPI.SR.add_to_other_config(sr.Connection.Session, sr.opaque_ref, "scheduler", "cfq");                           
                        }
                        sr_need_reattach.Add(sr);
                    }
                }
                if (sr_need_reattach.Count != 0)
                {
                    foreach (SR sr in sr_need_reattach)
                    {
                        foreach (VDI vdi in sr.Connection.ResolveAll(sr.VDIs))
                        {
                            foreach (VBD vbd in sr.Connection.ResolveAll(vdi.VBDs))
                            {
                                VM v = sr.Connection.Resolve(vbd.VM);
                                if (v == null)
                                    continue;
                                // PR-1223: ignore control domain VM on metadata VDIs, so that the SR can be detached if there are no other running VMs
                                if (vdi.type == vdi_type.metadata && v.is_control_domain)
                                    continue;
                                if (v.power_state == vm_power_state.Running && !running_vms.Contains(v))
                                    running_vms.Add(v);
                            }
                        }
                    }
                    if (running_vms.Count == 0)
                    {
                        foreach (XenRef<VBD> v in allVBDs)
                        {
                            if (vm.Connection.Resolve<VBD>(v).type == vbd_type.CD)
                            {
                                continue;
                            }
                            if (XenAPI.VBD.get_qos_algorithm_params(vm.Connection.Session, v.opaque_ref).ContainsKey("class"))
                            {
                                XenAPI.VBD.remove_from_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "class");
                            }
                            if (XenAPI.VBD.get_qos_algorithm_params(vm.Connection.Session, v.opaque_ref).ContainsKey("sched"))
                            {
                                XenAPI.VBD.remove_from_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "sched");
                            }
                            if (XenAPI.VBD.get_qos_algorithm_type(vm.Connection.Session, v.opaque_ref) != null)
                            {
                                XenAPI.VBD.set_qos_algorithm_type(vm.Connection.Session, v.opaque_ref, "");
                            }
                            XenAPI.VBD.set_qos_algorithm_type(vm.Connection.Session, v.opaque_ref, "ionice");
                            XenAPI.VBD.add_to_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "sched", "rt");
                            XenAPI.VBD.add_to_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "class", comboBox1.Text);
                        }
/*                        foreach (SR sr in sr_need_reattach)
                        {
                            new SrReattachAction(sr,sr.opaque_ref,null,null);
                        }
                        */
                        foreach (SR sr in sr_need_reattach)
                        {
                            if (sr.PBDs.Count < 1)
                                return null;

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
                                //XenAPI.PBD.async_unplug(sr.Connection.Session, pbd.opaque_ref);
                                XenAPI.PBD.unplug(sr.Connection.Session, pbd.opaque_ref);
                            }
                            foreach (XenRef<PBD> pbd in allPBDRefs)
                            {
                                //XenAPI.PBD.async_plug(sr.Connection.Session, pbd.opaque_ref);
                                XenAPI.PBD.plug(sr.Connection.Session, pbd.opaque_ref);
                            }
                        }
                    }
                    else
                    {
                        foreach (SR sr in sr_need_reattach)
                        {
                            if (XenAPI.SR.get_other_config(sr.Connection.Session, sr.opaque_ref).ContainsKey("scheduler"))
                            {
                                XenAPI.SR.remove_from_other_config(sr.Connection.Session, sr.opaque_ref, "scheduler");
                            }
                        }
                        _ValidToSave4 = false;
                        // new HasRunningVMsWarningDialog(running_vms,sr_need_reattach,allVBDs).ShowDialog();
                        new HasRunningVMsWarningDialog(running_vms, sr_need_reattach, allVBDs).ShowDialog();
                        return null;
                    }
                }
                else
                {
                    foreach (XenRef<VBD> v in allVBDs)
                    {
                        if (vm.Connection.Resolve<VBD>(v).type == vbd_type.CD)
                        {
                            continue;
                        }
                        if (XenAPI.VBD.get_qos_algorithm_params(vm.Connection.Session, v.opaque_ref).ContainsKey("class"))
                        {
                            XenAPI.VBD.remove_from_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "class");
                        }
                        if (XenAPI.VBD.get_qos_algorithm_params(vm.Connection.Session, v.opaque_ref).ContainsKey("sched"))
                        {
                            XenAPI.VBD.remove_from_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "sched");
                        }
                        if (XenAPI.VBD.get_qos_algorithm_type(vm.Connection.Session, v.opaque_ref) != null)
                        {
                            XenAPI.VBD.set_qos_algorithm_type(vm.Connection.Session, v.opaque_ref, "");
                        }
                        XenAPI.VBD.set_qos_algorithm_type(vm.Connection.Session, v.opaque_ref, "ionice");
                        XenAPI.VBD.add_to_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "sched", "rt");
                        XenAPI.VBD.add_to_qos_algorithm_params(vm.Connection.Session, v.opaque_ref, "class", comboBox1.Text);
                    }
                }
            }
            if (!args.ContainsKey("mbps"))
            {
                if (vm.other_config["io_limit"] == null || vm.other_config["io_limit"] == "")
                {
                    args.Add("mbps", "0");
                }
                else
                {
                    args.Add("mbps",vm.other_config["io_limit"]);
                }
            }
            if (vm.power_state == vm_power_state.Running)
            {
                XenAPI.Host.call_plugin(vm.Connection.Session, vm.resident_on.opaque_ref, "vm_io_limit.py", "set_vm_io_limit", args);
            }
            return null;
        }
        public bool IsSrCfq(SR sr)
        {
           if (sr.other_config.ContainsKey("scheduler"))
           {
               if (sr.other_config["scheduler"] == "cfq")
               {
                   return true;
               }
            }
            return false;
         }
        public void SetXenObjects(IXenObject orig, IXenObject clone)
        {
            vm = (VM)clone;
            Limit_Level_Init();
            Limit_Rate_Init();
        }

        public void ShowLocalValidationMessages() { }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;//禁止空格
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符  
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text!=""&&textBox1.Text!=null)
            {
                if (textBox1.Text.Substring(0, 1) == "0")
                {
                    textBox1.Text = "1";
                    label7.Visible = true;
                }
                else
                {
                    label7.Visible = false;
                }
            }           
        }
    }
}
