﻿using System;
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
            dtTable.Columns.Add("pciid", typeof (string));
            dtTable.Columns.Add("usbmode", typeof (string));
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
                        PcisdataGridViewExs.Rows[i].Cells[0].Tag = dtTable.Rows[i]["pciid"];
                        PcisdataGridViewExs.Rows[i].Cells[1].Value = dtTable.Rows[i]["devices"];
                        PcisdataGridViewExs.Rows[i].Cells[2].Value = dtTable.Rows[i]["vm_name"];
                        PcisdataGridViewExs.Rows[i].Cells[2].Tag = dtTable.Rows[i]["vm_uuid"];
                        PcisdataGridViewExs.Rows[i].Cells[3].Value = dtTable.Rows[i]["usbmode"];
                        usbmode.Frozen = !(bool)(dtTable.Rows[i]["bind"]);
                        
                        ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[4])).Bind = (bool)(dtTable.Rows[i]["bind"]);           
                        ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[4])).Enabled = (bool)(dtTable.Rows[i]["enabled"]);
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
                    string result = XenAPI.Host.call_plugin(home.Connection.Session, home.opaque_ref, "pvusbinfo.py", "list", null);
                    var ret = (UsbDeviceInfoConfig.PVUsbResult)HalsignUtil.JsonToObject(result, typeof(UsbDeviceInfoConfig.PVUsbResult));
                    foreach (UsbDeviceInfoConfig.USBInfo usbinfo in ret.returnvalue)
                    {
                        DataRow row = dtTable.NewRow();
                        row["id"] = usbinfo.id;
                        row["pciid"] = usbinfo.pciid;
                        row["bus"] = string.Concat("Bus ", usbinfo.busid);

                        row["devices"] = usbinfo.longname != null ? usbinfo.longname : usbinfo.shortname;
                        if (usbinfo.vm != null)
                        {
                            XenRef<VM> vmRef = VM.get_by_uuid(home.Connection.Session, usbinfo.vm);
                            VM bindvm = VM.get_record(home.Connection.Session, vmRef);

                            row["vm_name"] = bindvm.name_label;
                            row["vm_uuid"] = usbinfo.vm;
                            row["usbmode"] = bindvm.other_config["usbmode"];
                            row["bind"] = false;
                            row["enabled"] = true;
                        }
                        else
                        {
                            VM findvm = home.Connection.Cache.VMs.FirstOrDefault(vm => vm.uuid != null
                                && vm.is_a_real_vm
                                && HalsignHelpers.IsVMShow(vm)
                                //&& vm.Home().Equals(home) 
                                && vm.other_config.ContainsKey("pci")
                                && vm.other_config["pci"].Contains(string.Concat("0000:", usbinfo.pciid)));

                            if (findvm == null)
                            {
                                row["usbmode"] = "pvusb";
                                row["bind"] = true;
                                row["enabled"] = true;
                            }
                            else
                            {
                                row["vm_name"] = findvm.name_label;
                                row["vm_uuid"] = findvm.uuid;
                                row["usbmode"] = "vt-d";
                                row["bind"] = false;
                                row["enabled"] = true;
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
                string result = XenAPI.Host.call_plugin(home.Connection.Session, home.opaque_ref, "pvusbinfo.py", "list", null);
                var pvusbresult = (UsbDeviceInfoConfig.PVUsbResult)HalsignUtil.JsonToObject(result, typeof(UsbDeviceInfoConfig.PVUsbResult));
                for (int i = 0; i < pvusbresult.returnvalue.Count; i++)
                {
                    PcisdataGridViewExs.Rows.Add();
                    PcisdataGridViewExs.Rows[i].Tag = pvusbresult.returnvalue[i].id;
                    PcisdataGridViewExs.Rows[i].Cells[0].Value = string.Concat("Bus ", pvusbresult.returnvalue[i].busid);
                    PcisdataGridViewExs.Rows[i].Cells[0].Tag = pvusbresult.returnvalue[i].pciid;
                    PcisdataGridViewExs.Rows[i].Cells[1].Value = pvusbresult.returnvalue[i].longname != null ? pvusbresult.returnvalue[i].longname : pvusbresult.returnvalue[i].shortname;
                    if (pvusbresult.returnvalue[i].vm != null)
                    {
                        XenRef<VM> vmRef = VM.get_by_uuid(home.Connection.Session, pvusbresult.returnvalue[i].vm);
                        VM bindvm = VM.get_record(home.Connection.Session, vmRef);
                        PcisdataGridViewExs.Rows[i].Cells[2].Value = bindvm.name_label;
                        PcisdataGridViewExs.Rows[i].Cells[2].Tag = pvusbresult.returnvalue[i].vm;
                        PcisdataGridViewExs.Rows[i].Cells[3].Value = bindvm.other_config["usbmode"];
                        ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[4])).Bind = false;
                        ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[4 ])).Enabled = true;
                    }
                    else
                    {
                        VM findvm = home.Connection.Cache.VMs.FirstOrDefault(vm => vm.uuid != null
                            && vm.is_a_real_vm
                            && HalsignHelpers.IsVMShow(vm)
                            //&& vm.Home().Equals(home) 
                            && vm.other_config.ContainsKey("pci")
                            && vm.other_config["pci"].Contains(string.Concat("0000:", pvusbresult.returnvalue[i].pciid)));
                        if (findvm == null)
                        {
                            PcisdataGridViewExs.Rows[i].Cells[3].Value = "pvusb";
                            ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[4])).Bind = true;
                            ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[4])).Enabled = true;
                        }
                        else
                        {
                            PcisdataGridViewExs.Rows[i].Cells[2].Value = findvm.name_label;
                            PcisdataGridViewExs.Rows[i].Cells[2].Tag = findvm.uuid;
                            PcisdataGridViewExs.Rows[i].Cells[3].Value = "vt-d";
                            ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[4])).Bind = false;
                            ((DataGridViewButtonCellEx)(PcisdataGridViewExs.Rows[i].Cells[4])).Enabled = true;
                        }
                        
                    }
                }
            }
        }        

        private void PcisdataGridViewExs_CellButtonClicked(object sender, Controls.DataGridViewExs.DataGridViewButtonClickEventArgs e)
        {
            if (e.Bind)
            {
                if(PcisdataGridViewExs.Rows[e.RowIndex].Cells[0].Value != null &&
                    PcisdataGridViewExs.Rows[e.RowIndex].Tag != null &&
                    PcisdataGridViewExs.Rows[e.RowIndex].Cells[1].Value != null)
                {
                    string id = "";
                    string mode = "pvusb";
                    if (PcisdataGridViewExs.Rows[e.RowIndex].Cells[3].Value.ToString() == "pvusb")
                    {
                        string value = usbmode.Selected.ToString();
                        mode = "pvusb";
                        id = PcisdataGridViewExs.Rows[e.RowIndex].Tag.ToString(); // pass id
                    }
                    else if (PcisdataGridViewExs.Rows[e.RowIndex].Cells[3].Value.ToString() == "vt-d")
                    {
                        mode = "vt-d";
                        id = PcisdataGridViewExs.Rows[e.RowIndex].Cells[0].Tag.ToString();// pass pciid
                    }
                    PciBindDialog dialog = new PciBindDialog(home, PcisdataGridViewExs.Rows[e.RowIndex].Cells[0].Value.ToString(), id, mode, PcisdataGridViewExs.Rows[e.RowIndex].Cells[1].Value.ToString());
                    dialog.Show(Program.MainWindow);
                }
            }
            else
            {
                string vmuuid = PcisdataGridViewExs.Rows[e.RowIndex].Cells[2].Tag.ToString();
                var vmref = VM.get_by_uuid(this.home.Connection.Session, vmuuid);
                VM selectedvm = VM.get_record(this.home.Connection.Session, vmref);
                Dictionary<string, string> other_config = selectedvm.other_config;
                string mode = other_config["usbmode"];
                other_config.Remove("usbmode");
                if (mode == "vt-d")
                {
                    string pcistr = string.Concat("0/0000:", PcisdataGridViewExs.Rows[e.RowIndex].Cells[0].Tag.ToString());
                    string pci_val = other_config["pci"];
                    string[] devices = pci_val.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (other_config["pci"] == pcistr)
                    {
                        other_config.Remove("pci");
                    }
                    else if (other_config["pci"].StartsWith(pcistr) && devices.Length > 1)
                    {
                        pci_val = pci_val.Remove(0, pcistr.Length + 1);
                        other_config["pci"] = pci_val;
                    }
                    else
                    {
                        int position = other_config["pci"].IndexOf(pcistr, StringComparison.CurrentCultureIgnoreCase);
                        if (position != -1)
                        {
                            pci_val = pci_val.Remove(position - 1, pcistr.Length + 1);
                            other_config["pci"] = pci_val;
                        }
                    }
                } else if (mode == "pvusb")
                {
                    Dictionary<string, string> args = new Dictionary<string, string>();
                    args.Add("id", PcisdataGridViewExs.Rows[e.RowIndex].Tag.ToString());
                    string result = XenAPI.Host.call_plugin(home.Connection.Session, home.opaque_ref, "pvusbinfo.py", "unassign", args);
                }
                XenAPI.VM.set_other_config(home.Connection.Session, vmref, other_config);
                selectedvm.NotifyPropertyChanged("other_config");
            }
        }
    }
}
