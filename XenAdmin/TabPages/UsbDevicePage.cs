using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XenAPI;
using XenAdmin;
using HalsignLib;
using HalsignModel;
using XenAdmin.Controls;
using XenAdmin.Controls.DataGridViewExs;
using XenAdmin.Dialogs;

namespace XenAdmin.TabPages
{
    public partial class UsbDevicePage : BaseTabPage
    {
        public UsbDevicePage()
        {
            InitializeComponent();
            Initdatatable();
            base.Text = Messages.USBDEVICE_TAB_TITLE;
        }

        private Host home;
        private DataTable dtTable;
        private static object lockobj = new object();

        private delegate void BuildDataListHandler();

        private void Initdatatable()
        {
            dtTable = new DataTable();
            dtTable.Columns.Add("id", typeof (string));
            dtTable.Columns.Add("bus", typeof (string));
            dtTable.Columns.Add("devices", typeof(string));
            dtTable.Columns.Add("vm_name", typeof (string));
            dtTable.Columns.Add("vm_uuid", typeof (string));
            dtTable.Columns.Add("bind", typeof(bool));
            dtTable.Columns.Add("enabled", typeof(bool));
        }

        public IXenObject XenObject
        {
            set
            {
                Program.AssertOnEventThread();

                if (value != null)
                {
                    home = value as Host;
                    if (home != null)
                    {
                        VM[] vms = home.Connection.Cache.VMs.Where<VM>(vm => vm.uuid != null && vm.is_a_real_vm && HalsignHelpers.IsVMShow(vm) && vm.Home() == home).ToArray();
                        foreach (VM vm in vms)
                        {
                            vm.PropertyChanged -= new PropertyChangedEventHandler(vm_PropertyChanged);
                            vm.PropertyChanged += new PropertyChangedEventHandler(vm_PropertyChanged);
                        }

                        //BuildList();
                    }

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                    worker.RunWorkerAsync(this);
                }
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (lockobj)
            {
                this.PcisdataGridViewExs.Rows.Clear();
                this.BeginInvoke(new BuildDataListHandler(() =>
                {
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        PcisdataGridViewExs.Rows.Add();
                        PcisdataGridViewExs.Rows[i].Tag = dtTable.Rows[i]["id"];
                        PcisdataGridViewExs.Rows[i].Cells[0].Value = dtTable.Rows[i]["bus"];
                        PcisdataGridViewExs.Rows[i].Cells[1].Value = dtTable.Rows[i]["devices"];
                        PcisdataGridViewExs.Rows[i].Cells[2].Value = dtTable.Rows[i]["vm_name"];
                        PcisdataGridViewExs.Rows[i].Cells[2].Tag = dtTable.Rows[i]["vm_uuid"];
                        ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[3])).Bind = (bool)(dtTable.Rows[i]["bind"]);           
                        ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[3])).Enabled = (bool)(dtTable.Rows[i]["enabled"]);
                    }
                }));
            }
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (lockobj)
            {
                dtTable.Rows.Clear();
                if (this.home != null)
                {
                    string result = XenAPI.Host.call_plugin(home.Connection.Session, home.opaque_ref, "usbinfo.py", "usbinfo", null);
                    var pcis = (UsbDeviceInfoConfig.PCIsInfo)HalsignUtil.JsonToObject(result.Replace('\'', '"'), typeof(UsbDeviceInfoConfig.PCIsInfo));
                    foreach (UsbDeviceInfoConfig.USBInfo usbinfo in pcis.pcis)
                    {
                        DataRow row = dtTable.NewRow();
                        row["id"] = usbinfo.id;
                        row["bus"] = string.Concat("Bus ", usbinfo.idx);

                        try
                        {
                            VM findvm =
                                home.Connection.Cache.VMs.First(
                                    vm => vm.uuid != null && vm.is_a_real_vm && HalsignHelpers.IsVMShow(vm) &&
                                          vm.Home() != null && vm.Home().Equals(home) &&
                                          vm.other_config.ContainsKey("pci") &&
                                          vm.other_config["pci"].Contains(string.Concat("0000:", usbinfo.id)));
                            row["vm_name"] = findvm.name_label;
                            row["vm_uuid"] = findvm.uuid;
                            row["bind"] = false;
                        }
                        catch (System.InvalidOperationException)
                        {
                            row["bind"] = true;
                        }
                        finally
                        {
                            if (usbinfo.devices != null)
                            {
                                row["devices"] = WrapStrings(usbinfo.devices);
                                row["enabled"] = true;
                            }
                            else
                            {
                                row["enabled"] = !string.IsNullOrEmpty(row["vm_name"].ToString());
                            }
                        }

                        dtTable.Rows.Add(row);
                    }
                }
            }
        }

        void vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "other_config")
            {
                Program.Invoke(this, BuildList);
            }
        }

        public void BuildList()
        {
            PcisdataGridViewExs.Rows.Clear();
            if(this.home != null)
            {
                string result = XenAPI.Host.call_plugin(home.Connection.Session, home.opaque_ref, "usbinfo.py", "usbinfo", null);
                var pcis = (UsbDeviceInfoConfig.PCIsInfo)HalsignUtil.JsonToObject(result.Replace('\'', '"'), typeof(UsbDeviceInfoConfig.PCIsInfo));
                for (int i = 0; i < pcis.pcis.Count; i++)
                {
                    PcisdataGridViewExs.Rows.Add();
                    PcisdataGridViewExs.Rows[i].Tag = pcis.pcis[i].id;
                    PcisdataGridViewExs.Rows[i].Cells[0].Value = string.Concat("Bus ", pcis.pcis[i].idx);

                    try
                    {
                        VM findvm = home.Connection.Cache.VMs.First(vm => vm.uuid != null 
                            && vm.is_a_real_vm 
                            && HalsignHelpers.IsVMShow(vm) 
                            //&& vm.Home().Equals(home) 
                            && vm.other_config.ContainsKey("pci") 
                            && vm.other_config["pci"].Contains(string.Concat("0000:", pcis.pcis[i].id)));
                        PcisdataGridViewExs.Rows[i].Cells[2].Value = findvm.name_label;
                        PcisdataGridViewExs.Rows[i].Cells[2].Tag = findvm.uuid;
                        ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[3])).Bind = false;
                    }
                    catch (System.InvalidOperationException)
                    {
                        ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[3])).Bind = true;
                    }
                    finally
                    {
                        if (pcis.pcis[i].devices != null)
                        {
                            PcisdataGridViewExs.Rows[i].Cells[1].Value = WrapStrings(pcis.pcis[i].devices);
                            ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[3])).Enabled = true;
                        }
                        else
                        {
                            ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[3])).Enabled =
                                (PcisdataGridViewExs.Rows[i].Cells[2].Value != null);
                        }                        
                    }                    
                }
            }
        }

        private string WrapStrings(string[] strs)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strs.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(strs[i].Trim()))
                {
                    sb.AppendLine(strs[i]);
                }
            }
            if (!string.IsNullOrEmpty(strs[strs.Length - 1].Trim()))
                sb.Append(strs[strs.Length - 1]);

            return sb.ToString();
        }

        private void PcisdataGridViewExs_CellButtonClicked(object sender, Controls.DataGridViewExs.DataGridViewButtonClickEventArgs e)
        {
            if (e.Bind)
            {
                if(PcisdataGridViewExs.Rows[e.RowIndex].Cells[0].Value != null &&
                    PcisdataGridViewExs.Rows[e.RowIndex].Tag != null &&
                    PcisdataGridViewExs.Rows[e.RowIndex].Cells[1].Value != null)
                {
                    PciBindDialog dialog = new PciBindDialog(home, PcisdataGridViewExs.Rows[e.RowIndex].Cells[0].Value.ToString(),
                        PcisdataGridViewExs.Rows[e.RowIndex].Tag.ToString(), PcisdataGridViewExs.Rows[e.RowIndex].Cells[1].Value.ToString());
                    dialog.Show(Program.MainWindow);
                }
            }
            else
            {
                string usbidx = string.Concat("0/0000:", PcisdataGridViewExs.Rows[e.RowIndex].Tag.ToString());
                try
                {
                    VM selected = home.Connection.Cache.VMs.First(vm => vm.uuid != null && vm.is_a_real_vm && HalsignHelpers.IsVMShow(vm) &&
                        vm.Home() == home && vm.other_config.ContainsKey("pci") && vm.other_config["pci"].Contains(usbidx));
                    Dictionary<string, string> other_config = selected.other_config;
                    string pci_val = other_config["pci"];
                    string[] devices = pci_val.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (other_config["pci"] == usbidx)
                    {
                        other_config.Remove("pci");
                    }
                    else if (other_config["pci"].StartsWith(usbidx) && devices.Length > 1)
                    {
                        pci_val = pci_val.Remove(0, usbidx.Length + 1);
                        other_config["pci"] = pci_val;
                    }
                    else
                    {
                        int position = other_config["pci"].IndexOf(usbidx, StringComparison.CurrentCultureIgnoreCase);
                        if (position != -1)
                        {
                            pci_val = pci_val.Remove(position - 1, usbidx.Length + 1);
                            other_config["pci"] = pci_val;
                        }
                    }

                    XenAPI.VM.set_other_config(home.Connection.Session, selected.opaque_ref, other_config);
                    selected.NotifyPropertyChanged("other_config");

                    //if (PcisdataGridViewExs.Rows[e.RowIndex].Cells[1].Value == null)
                    //{
                    //    MessageBox.Show(Messages.UBOND_USB_DEIVCE);
                    //}
                }
                catch(System.InvalidOperationException)
                {
                    //Ignore, VM was not found
                }
            }
        }
    }
}
