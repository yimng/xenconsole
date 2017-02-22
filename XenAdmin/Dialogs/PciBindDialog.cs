using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XenAPI;
using HalsignLib;
using XenAdmin.Commands;

namespace XenAdmin.Dialogs
{
    public partial class PciBindDialog : XenDialogBase
    {
        public PciBindDialog(Host home, string busname, string id, string mode, string usbinfo)
        {
            InitializeComponent();
            BuildVMList(home);
            this.BusNamelabel.Text = string.Concat(busname, ":");
            this.UsbInfolabel.Text = usbinfo;
            this.m_host = home;
            this.m_id = id;
            this.mode = mode;
        }

        private void BuildVMList(Host home)
        {
            VM[] vms = home.Connection.Cache.VMs.Where<VM>(vm => vm.uuid != null && vm.is_a_real_vm && HalsignHelpers.IsVMShow(vm) && vm.Home() == home).ToArray();
            this.VMsComboBox.DataSource = vms;
            this.VMsComboBox.DisplayMember = "name_label";
            this.VMsComboBox.ValueMember = "uuid";
        }

        private Host m_host;
        private string m_id;
        private string mode;

        private void Bindbutton_Click(object sender, EventArgs e)
        {
            var vmref = VM.get_by_uuid(m_host.Connection.Session, this.VMsComboBox.SelectedValue.ToString());
            VM selectedvm = VM.get_record(m_host.Connection.Session, vmref);
            Dictionary<string, string> other_config = selectedvm.other_config;
            if (mode == "pvusb")
            {                
                Dictionary<string, string> args = new Dictionary<string, string>();
                args.Add("id", this.m_id);
                args.Add("vm_uuid", selectedvm.uuid);
                string result = XenAPI.Host.call_plugin(m_host.Connection.Session, m_host.opaque_ref, "pvusbinfo.py", "assign", args);
                
                if (!other_config.ContainsKey("usbmode"))
                {
                    other_config.Add("usbmode", "pvusb");
                } else
                {
                    other_config["usbmode"] = "pvusb";
                }
            }
            else
            {
                if (!other_config.ContainsKey("usbmode"))
                {
                    other_config.Add("usbmode", "vt-d");
                }
                else
                {
                    other_config["usbmode"] = "vt-d";
                }

                if (!other_config.ContainsKey("pci"))
                {
                    other_config.Add("pci", string.Concat("0/0000:", this.m_id));
                }
                else
                {
                    string pci_value = other_config["pci"];
                    if (!pci_value.Contains(this.m_id))
                    {
                        pci_value = string.Format("{0},{1}", pci_value, string.Concat("0/0000:", this.m_id));
                        other_config["pci"] = pci_value;
                    }
                }
                //string msg = selectedvm.power_state == vm_power_state.Halted ? Messages.BOND_USB_DEVICE_VM_HALT : Messages.BOND_USB_DEVICE_VM_RUNNING;
                //MessageBox.Show(this, msg);                
            }
            XenAPI.VM.set_other_config(m_host.Connection.Session, vmref, other_config);
            selectedvm.NotifyPropertyChanged("other_config");
            this.Close();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
