/* Copyright © 2013 Halsign Corporation.
 * All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, 
 * with or without modification, are permitted provided 
 * that the following conditions are met: 
 * 
 * *   Redistributions of source code must retain the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer. 
 * *   Redistributions in binary form must reproduce the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer in the documentation and/or other 
 *     materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XenAdmin.Controls;
using HalsignModel;

namespace XenAdmin.Wizards.RestoreWizard_Pages
{
    public partial class Page_VMSettings : XenTabPage
	{
        private Label Label_Notice;
        private Label Label_Warnning;
        private DataGridViewCheckBoxColumn _Selected;
        private DataGridViewTextBoxColumn _Name;
        private DataGridViewTextBoxColumn _Suffix;
        private DataGridViewTextBoxColumn _DiskSize;
        private DataGridViewTextBoxColumn _Network;
        private DataGridViewTextBoxColumn _DataStore;
        private DataGridViewTextBoxColumn _FreeSpace;
        private DataGridView DataGridView_RestoreInfo;

        public DataGridView DataGridViewRestoreInfo
        {
            get { return DataGridView_RestoreInfo; }
            set { DataGridView_RestoreInfo = value; }
        }

        public Page_VMSettings()
		{
			InitializeComponent();
		}

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page_VMSettings));
            this.Label_Notice = new System.Windows.Forms.Label();
            this.DataGridView_RestoreInfo = new System.Windows.Forms.DataGridView();
            this.Label_Warnning = new System.Windows.Forms.Label();
            this._Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Suffix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._DiskSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Network = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._DataStore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._FreeSpace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_RestoreInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // Label_Notice
            // 
            this.Label_Notice.AccessibleDescription = null;
            this.Label_Notice.AccessibleName = null;
            resources.ApplyResources(this.Label_Notice, "Label_Notice");
            this.Label_Notice.Font = null;
            this.Label_Notice.Name = "Label_Notice";
            // 
            // DataGridView_RestoreInfo
            // 
            this.DataGridView_RestoreInfo.AccessibleDescription = null;
            this.DataGridView_RestoreInfo.AccessibleName = null;
            this.DataGridView_RestoreInfo.AllowUserToAddRows = false;
            this.DataGridView_RestoreInfo.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.DataGridView_RestoreInfo, "DataGridView_RestoreInfo");
            this.DataGridView_RestoreInfo.BackgroundImage = null;
            this.DataGridView_RestoreInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_RestoreInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._Selected,
            this._Name,
            this._Suffix,
            this._DiskSize,
            this._Network,
            this._DataStore,
            this._FreeSpace});
            this.DataGridView_RestoreInfo.Font = null;
            this.DataGridView_RestoreInfo.Name = "DataGridView_RestoreInfo";
            this.DataGridView_RestoreInfo.ReadOnly = true;
            this.DataGridView_RestoreInfo.RowHeadersVisible = false;
            this.DataGridView_RestoreInfo.RowTemplate.Height = 23;
            this.DataGridView_RestoreInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewRestoreInfo_RowEnter);
            // 
            // Label_Warnning
            // 
            this.Label_Warnning.AccessibleDescription = null;
            this.Label_Warnning.AccessibleName = null;
            resources.ApplyResources(this.Label_Warnning, "Label_Warnning");
            this.Label_Warnning.Font = null;
            //this.Label_Warnning.Image = global::Properties.Resources.alerts_16;
            this.Label_Warnning.Name = "Label_Warnning";
            // 
            // _Selected
            // 
            resources.ApplyResources(this._Selected, "_Selected");
            this._Selected.Name = "_Selected";
            this._Selected.ReadOnly = true;
            this._Selected.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // _Name
            // 
            resources.ApplyResources(this._Name, "_Name");
            this._Name.Name = "_Name";
            this._Name.ReadOnly = true;
            this._Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _Suffix
            // 
            resources.ApplyResources(this._Suffix, "_Suffix");
            this._Suffix.Name = "_Suffix";
            this._Suffix.ReadOnly = true;
            this._Suffix.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _DiskSize
            // 
            resources.ApplyResources(this._DiskSize, "_DiskSize");
            this._DiskSize.Name = "_DiskSize";
            this._DiskSize.ReadOnly = true;
            this._DiskSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _Network
            // 
            resources.ApplyResources(this._Network, "_Network");
            this._Network.Name = "_Network";
            this._Network.ReadOnly = true;
            this._Network.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _DataStore
            // 
            resources.ApplyResources(this._DataStore, "_DataStore");
            this._DataStore.Name = "_DataStore";
            this._DataStore.ReadOnly = true;
            this._DataStore.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _FreeSpace
            // 
            resources.ApplyResources(this._FreeSpace, "_FreeSpace");
            this._FreeSpace.Name = "_FreeSpace";
            this._FreeSpace.ReadOnly = true;
            this._FreeSpace.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Page_VMSettings
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.Label_Warnning);
            this.Controls.Add(this.DataGridView_RestoreInfo);
            this.Controls.Add(this.Label_Notice);
            this.Font = null;
            this.Name = "Page_VMSettings";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_RestoreInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public override string PageTitle
        {
            get
            {
                return Messages.RESTORE_VM_SETTINGS_TITLE;
            }
        }

        public override string Text
        {
            get
            {
                return Messages.RESTORE_VM_SETTINGS_TEXT;
            }
        }

        public override bool EnableNext()
        {
            return !this.Label_Warnning.Visible;
        }

        /// <summary>
        /// update by dalei.shou on 2013/10/29, remove the code that does not make sense
        /// Each row with tag: vm.uuid|vdi.uuid
        /// </summary>
        /// <param name="vmCheckedList"></param>
        /// <param name="backup_info_list"></param>
        /// <param name="vm_name"></param>
        /// <param name="network_name"></param>
        /// <param name="restoreDataModel"></param>
        /// <param name="restore_vdi_info_list"></param>
        public void BuildRestoreInfoList(List<AgentParamDataModel> vmCheckedList, Dictionary<string, string> backup_info_list, string vm_name, string network_name, RestoreDataModel restoreDataModel, Dictionary<string, List<BackupRestoreConfig.RestoreInfo>> restore_vdi_info_list)
        {
            long restore_size = 0;
            this.DataGridView_RestoreInfo.Rows.Clear();

            int i = 0;
            foreach (AgentParamDataModel _vm in vmCheckedList)
            {
                if (!backup_info_list.ContainsKey(_vm.VMRestoreInfo))
                {
                    continue;
                }
                this.DataGridView_RestoreInfo.Rows.Add();
                this.DataGridView_RestoreInfo.Rows[i].Cells[0].Value = true;
                this.DataGridView_RestoreInfo.Rows[i].Cells[0].Tag = "vm";
                string[] strTemp = backup_info_list[_vm.VMRestoreInfo].Split('|');
                this.DataGridView_RestoreInfo.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                string _vm_name;
                string _os_version;
                this.ParseVMInfo(strTemp[1], out _vm_name, out _os_version);
                this.DataGridView_RestoreInfo.Rows[i].Cells[1].Value = _vm_name;
                this.DataGridView_RestoreInfo.Rows[i].Cells[2].Value = vm_name;
                this.DataGridView_RestoreInfo.Rows[i].Cells[4].Value = network_name; ;
                this.DataGridView_RestoreInfo.Rows[i].Cells[5].Value = restoreDataModel.choice_sr_ip_name;
                this.DataGridView_RestoreInfo.Rows[i].Cells[6].Value = restoreDataModel.choice_sr_free_space;
                this.DataGridView_RestoreInfo.Rows[i].Tag = string.Format("{0}|{1}", _vm.VMRestoreInfo.Substring(37, 36), _vm.VMRestoreInfo.Substring(37, 36));

                string[] strDisk = strTemp[0].Split('\n');
                List<DiskInfo> diskList = new List<DiskInfo>();
                DiskInfo _disk_info;
                foreach (string item in strDisk)
                {
                    if (item.IndexOf(".Name") > 0)
                    {
                        _disk_info = new DiskInfo();
                        _disk_info.name = item.Split('=')[1];
                        foreach (string _storage in strDisk)
                        {
                            if (_storage.IndexOf(item.Split('.')[0] + ".Storage") >= 0)
                            {
                                _disk_info.value = _storage.Split('=')[1];
                            }
                        }
                        diskList.Add(_disk_info);
                    }
                }

                if (!restore_vdi_info_list.ContainsKey(_vm.VMRestoreInfo))
                {
                    long count = 0;
                    foreach (DiskInfo _value in diskList)
                    {
                        count = count + long.Parse(_value.value);
                    }
                    this.DataGridView_RestoreInfo.Rows[i].Cells[3].Value = Util.DiskSizeString(count);
                    i++;
                    continue;
                }

                i++;

                foreach (BackupRestoreConfig.RestoreInfo info in restore_vdi_info_list[_vm.VMRestoreInfo])
                {
                    this.DataGridView_RestoreInfo.Rows.Add();
                    this.DataGridView_RestoreInfo.Rows[i].Cells[0].Value = true;
                    this.DataGridView_RestoreInfo.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                    this.DataGridView_RestoreInfo.Rows[i].Cells[1].Value = info.name;
                    this.DataGridView_RestoreInfo.Rows[i].Cells[3].Value = Util.DiskSizeString(long.Parse(info.storage));
                    this.DataGridView_RestoreInfo.Rows[i].Cells[5].Value = restoreDataModel.choice_sr_ip_name;
                    this.DataGridView_RestoreInfo.Rows[i].Cells[6].Value = restoreDataModel.choice_sr_free_space;
                    this.DataGridView_RestoreInfo.Rows[i].Tag = string.Format("{0}|{1}", _vm.VMRestoreInfo.Substring(37, 36), info.uuid);
                    i++;
                }
            }

            if (restore_size > restoreDataModel.choice_sr_free_space_size)
            {
                this.Label_Warnning.Visible = true;
                base.OnPageUpdated();
            }
            else
            {
                this.Label_Warnning.Visible = false;
                base.OnPageUpdated();
            }
        }

        private class DiskInfo
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        private void DataGridViewRestoreInfo_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;

            if (columnIndex == 0)
            {
                DataGridViewCheckBoxCell dgvCheckBoxCell = this.DataGridView_RestoreInfo.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                if (!(this.DataGridView_RestoreInfo.Rows[e.RowIndex].Cells[0].Tag != null && this.DataGridView_RestoreInfo.Rows[e.RowIndex].Cells[0].Tag is string && this.DataGridView_RestoreInfo.Rows[e.RowIndex].Cells[0].Tag.ToString() == "vm"))
                {
                    if (this.DataGridView_RestoreInfo.Rows[e.RowIndex].Cells[0].Value.ToString().ToLowerInvariant() == "true")
                    {
                        this.DataGridView_RestoreInfo.Rows[e.RowIndex].Cells[0].Value = false;
                    }
                    else if (this.DataGridView_RestoreInfo.Rows[e.RowIndex].Cells[0].Value.ToString().ToLowerInvariant() == "false")
                    {
                        this.DataGridView_RestoreInfo.Rows[e.RowIndex].Cells[0].Value = true;
                    }
                }
            }
        }

        private void ParseVMInfo(string vm_info, out string vm_name, out string os_version)
        {
            vm_name = "";
            os_version = "";

            foreach (string info in vm_info.Split('\n'))
            {
                string[] items = info.Split('=');
                if (items != null && items.Length > 0 && "vm.name".Equals(items[0]))
                {
                    vm_name = items[1];
                }
                if (items != null && items.Length > 0 && "os.version".Equals(items[0]))
                {
                    os_version = items[1];
                }
            }
        }
	}
}
