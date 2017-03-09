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
        public PciBindDialog(Host home, string busname, string usbidx, string usbinfo)
        {
            InitializeComponent();
            BuildVMList(home);
            this.BusNamelabel.Text = string.Concat(busname, ":");
            this.UsbInfolabel.Text = usbinfo;
            this.m_host = home;
            this.m_usbidx = usbidx;
        }

        private void BuildVMList(Host home)
        {
            VM[] vms = home.Connection.Cache.VMs.Where<VM>(vm => vm.uuid != null && vm.is_a_real_vm && HalsignHelpers.IsVMShow(vm) && vm.Home() == home).ToArray();
            this.VMsComboBox.DataSource = vms;
            this.VMsComboBox.DisplayMember = "name_label";
            this.VMsComboBox.ValueMember = "uuid";
        }

        private string WrapStrings(string[] strs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string str in strs)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    sb.AppendLine(str);
                }
            }

            return sb.ToString();
        }

        private Host m_host;
        private string m_usbidx;

        private void Bindbutton_Click(object sender, EventArgs e)
        {
            try
            {
                VM selected = m_host.Connection.Cache.VMs.First(vm => vm.uuid != null &&
                   vm.is_a_real_vm && HalsignHelpers.IsVMShow(vm) && 
                   vm.uuid == this.VMsComboBox.SelectedValue.ToString() && vm.Home() == m_host);
                Dictionary<string, string> other_config = selected.other_config;
                if (!other_config.ContainsKey("pci"))
                {
                    other_config.Add("pci", string.Concat("0/0000:", this.m_usbidx));
                }
                else
                {
                    string pci_value = other_config["pci"];
                    if (!pci_value.Contains(this.m_usbidx))
                    {
                        pci_value = string.Format("{0},{1}", pci_value, string.Concat("0/0000:", this.m_usbidx));
                        other_config["pci"] = pci_value;
                    }
                }
                XenAPI.VM.set_other_config(m_host.Connection.Session, selected.opaque_ref, other_config);
                selected.NotifyPropertyChanged("other_config");
                string msg = selected.power_state == vm_power_state.Halted ? Messages.BOND_USB_DEVICE_VM_HALT : Messages.BOND_USB_DEVICE_VM_RUNNING;
                //MessageBox.Show(this, msg);
                this.Close();
            }
            catch (System.InvalidOperationException)
            {
                //Ignore:VM was not found.
            }
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VMsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
