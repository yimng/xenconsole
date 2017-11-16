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
using HalsignLib;
using XenAPI;
using System.Globalization;
using XenAdmin.Controls;
using XenAdmin.Core;

namespace XenAdmin.Wizards.RestoreWizard_Pages
{
    public partial class Page_JobSettings : XenTabPage
	{
        private Label Label_RestoreOptions;
        private TextBox TextBox_JobName;
        private CheckBox CheckBox_VMName;
        private TextBox TextBox_VMName;
        private Label Label_DefaultStorage;
        private DataGridView DataGridView_DefaultStorage;
        private GroupBox GroupBox_NetworkSettings;
        private CheckBox CheckBox_BackupSettings;
        private CheckBox CheckBox_NewMACAddress;
        private ComboBox ComboBox_Network;
        private Label Label_DefaultNetwork;
        private Label Label_JobName;
        private IXenObject _xenModelObject;
        private Panel panel1;
        private DataGridViewTextBoxColumn _Server;
        private DataGridViewTextBoxColumn _NetworkAddress;
        private DataGridViewTextBoxColumn _VMStorage;
        private DataGridViewTextBoxColumn _FreeSpace;
        private RestoreDataModel resotreDataModel;
    
		public Page_JobSettings(IXenObject XenModelObject)
		{
			InitializeComponent();
            this.resotreDataModel = new RestoreDataModel();
            this._xenModelObject = XenModelObject;
            this.InitStorageGridList();
            this.InitNetworkListBox();
		}

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page_JobSettings));
            this.Label_RestoreOptions = new System.Windows.Forms.Label();
            this.Label_JobName = new System.Windows.Forms.Label();
            this.TextBox_JobName = new System.Windows.Forms.TextBox();
            this.CheckBox_VMName = new System.Windows.Forms.CheckBox();
            this.TextBox_VMName = new System.Windows.Forms.TextBox();
            this.Label_DefaultStorage = new System.Windows.Forms.Label();
            this.DataGridView_DefaultStorage = new System.Windows.Forms.DataGridView();
            this._Server = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._NetworkAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._VMStorage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._FreeSpace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupBox_NetworkSettings = new System.Windows.Forms.GroupBox();
            this.CheckBox_NewMACAddress = new System.Windows.Forms.CheckBox();
            this.ComboBox_Network = new System.Windows.Forms.ComboBox();
            this.Label_DefaultNetwork = new System.Windows.Forms.Label();
            this.CheckBox_BackupSettings = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_DefaultStorage)).BeginInit();
            this.GroupBox_NetworkSettings.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label_RestoreOptions
            // 
            resources.ApplyResources(this.Label_RestoreOptions, "Label_RestoreOptions");
            this.Label_RestoreOptions.Name = "Label_RestoreOptions";
            // 
            // Label_JobName
            // 
            resources.ApplyResources(this.Label_JobName, "Label_JobName");
            this.Label_JobName.Name = "Label_JobName";
            // 
            // TextBox_JobName
            // 
            resources.ApplyResources(this.TextBox_JobName, "TextBox_JobName");
            this.TextBox_JobName.Name = "TextBox_JobName";
            this.TextBox_JobName.TextChanged += new System.EventHandler(this.TextBox_JobName_TextChanged);
            this.TextBox_JobName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_JobName_KeyPress);
            // 
            // CheckBox_VMName
            // 
            resources.ApplyResources(this.CheckBox_VMName, "CheckBox_VMName");
            this.CheckBox_VMName.Name = "CheckBox_VMName";
            this.CheckBox_VMName.UseVisualStyleBackColor = true;
            this.CheckBox_VMName.CheckedChanged += new System.EventHandler(this.CheckBox_VMName_CheckedChanged);
            // 
            // TextBox_VMName
            // 
            resources.ApplyResources(this.TextBox_VMName, "TextBox_VMName");
            this.TextBox_VMName.Name = "TextBox_VMName";
            this.TextBox_VMName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_VMName_KeyPress);
            // 
            // Label_DefaultStorage
            // 
            resources.ApplyResources(this.Label_DefaultStorage, "Label_DefaultStorage");
            this.Label_DefaultStorage.Name = "Label_DefaultStorage";
            // 
            // DataGridView_DefaultStorage
            // 
            this.DataGridView_DefaultStorage.AllowUserToAddRows = false;
            this.DataGridView_DefaultStorage.AllowUserToDeleteRows = false;
            this.DataGridView_DefaultStorage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_DefaultStorage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._Server,
            this._NetworkAddress,
            this._VMStorage,
            this._FreeSpace});
            resources.ApplyResources(this.DataGridView_DefaultStorage, "DataGridView_DefaultStorage");
            this.DataGridView_DefaultStorage.Name = "DataGridView_DefaultStorage";
            this.DataGridView_DefaultStorage.ReadOnly = true;
            this.DataGridView_DefaultStorage.RowHeadersVisible = false;
            this.DataGridView_DefaultStorage.RowTemplate.Height = 23;
            this.DataGridView_DefaultStorage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView_DefaultStorage.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_DefaultStorage_RowEnter);
            // 
            // _Server
            // 
            resources.ApplyResources(this._Server, "_Server");
            this._Server.Name = "_Server";
            this._Server.ReadOnly = true;
            this._Server.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _NetworkAddress
            // 
            resources.ApplyResources(this._NetworkAddress, "_NetworkAddress");
            this._NetworkAddress.Name = "_NetworkAddress";
            this._NetworkAddress.ReadOnly = true;
            this._NetworkAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _VMStorage
            // 
            resources.ApplyResources(this._VMStorage, "_VMStorage");
            this._VMStorage.Name = "_VMStorage";
            this._VMStorage.ReadOnly = true;
            this._VMStorage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _FreeSpace
            // 
            resources.ApplyResources(this._FreeSpace, "_FreeSpace");
            this._FreeSpace.Name = "_FreeSpace";
            this._FreeSpace.ReadOnly = true;
            this._FreeSpace.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // GroupBox_NetworkSettings
            // 
            this.GroupBox_NetworkSettings.Controls.Add(this.CheckBox_NewMACAddress);
            this.GroupBox_NetworkSettings.Controls.Add(this.ComboBox_Network);
            this.GroupBox_NetworkSettings.Controls.Add(this.Label_DefaultNetwork);
            this.GroupBox_NetworkSettings.Controls.Add(this.CheckBox_BackupSettings);
            resources.ApplyResources(this.GroupBox_NetworkSettings, "GroupBox_NetworkSettings");
            this.GroupBox_NetworkSettings.Name = "GroupBox_NetworkSettings";
            this.GroupBox_NetworkSettings.TabStop = false;
            // 
            // CheckBox_NewMACAddress
            // 
            resources.ApplyResources(this.CheckBox_NewMACAddress, "CheckBox_NewMACAddress");
            this.CheckBox_NewMACAddress.Checked = true;
            this.CheckBox_NewMACAddress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBox_NewMACAddress.Name = "CheckBox_NewMACAddress";
            this.CheckBox_NewMACAddress.UseVisualStyleBackColor = true;
            // 
            // ComboBox_Network
            // 
            this.ComboBox_Network.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_Network.FormattingEnabled = true;
            resources.ApplyResources(this.ComboBox_Network, "ComboBox_Network");
            this.ComboBox_Network.Name = "ComboBox_Network";
            this.ComboBox_Network.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Network_SelectedIndexChanged);
            // 
            // Label_DefaultNetwork
            // 
            resources.ApplyResources(this.Label_DefaultNetwork, "Label_DefaultNetwork");
            this.Label_DefaultNetwork.Name = "Label_DefaultNetwork";
            // 
            // CheckBox_BackupSettings
            // 
            resources.ApplyResources(this.CheckBox_BackupSettings, "CheckBox_BackupSettings");
            this.CheckBox_BackupSettings.Name = "CheckBox_BackupSettings";
            this.CheckBox_BackupSettings.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Label_RestoreOptions);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // Page_JobSettings
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.GroupBox_NetworkSettings);
            this.Controls.Add(this.DataGridView_DefaultStorage);
            this.Controls.Add(this.Label_DefaultStorage);
            this.Controls.Add(this.TextBox_VMName);
            this.Controls.Add(this.CheckBox_VMName);
            this.Controls.Add(this.TextBox_JobName);
            this.Controls.Add(this.Label_JobName);
            this.Name = "Page_JobSettings";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_DefaultStorage)).EndInit();
            this.GroupBox_NetworkSettings.ResumeLayout(false);
            this.GroupBox_NetworkSettings.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public override string PageTitle
        {
            get
            {
                return Messages.RESTORE_JOB_SETTINGS_TITLE;
            }
        }

        public override string Text
        {
            get
            {
                return Messages.RESTORE_JOB_SETTINGS_TEXT;
            }
        }

        public override bool EnableNext()
        {
            return !string.IsNullOrEmpty(this.TextBox_JobName.Text) && this.ComboBox_Network.SelectedItem != null && this.resotreDataModel.choice_sr_uuid != null;
        }

        public void PopulateData()
        {
            if (this.DataGridView_DefaultStorage.RowCount > 0)
            {
                DataGridViewRow _CurrentRow = this.DataGridView_DefaultStorage.Rows[0];
                this.resotreDataModel.choice_sr_uuid = _CurrentRow.Tag.ToString();
                this.resotreDataModel.choice_sr_ip_name = _CurrentRow.Cells[1].Value.ToString() + ":" + _CurrentRow.Cells[2].Value.ToString();
                this.resotreDataModel.choice_sr_free_space = _CurrentRow.Cells[3].Value.ToString();
                this.resotreDataModel.choice_sr_free_space_size = long.Parse(_CurrentRow.Cells[3].Tag.ToString());
            }
        }

        private void InitStorageGridList()
        {
            VM _xenVM = null;
            Host _xenHost = null;
            if (this._xenModelObject is VM)
            {
                _xenVM = this._xenModelObject as VM;
                _xenHost = HalsignHelpers.VMHome(_xenVM);
            }
            else
            {
                _xenHost = _xenModelObject as Host;
            }
            this.DataGridView_DefaultStorage.Rows.Clear();
            PBD[] objArray = _xenHost.Connection.ResolveAll<PBD>(_xenHost.PBDs).ToArray();
            List<string> list = new List<string>();
            int i = 0;
            foreach (PBD pbd in objArray)
            {
                SR sr = pbd.Connection.Resolve<SR>(pbd.SR);
                if ((sr != null) && !sr.IsToolsSR)
                {
                    int index = list.BinarySearch(sr.opaque_ref);
                    if (index < 0)
                    {
                        index = ~index;
                        list.Insert(index, sr.opaque_ref);
                        if (sr.GetSRType(false) == SR.SRTypes.lvm 
                            || sr.GetSRType(false) == SR.SRTypes.ext 
                            || sr.GetSRType(false) == SR.SRTypes.lvmoiscsi
                            || sr.GetSRType(false) == SR.SRTypes.lvmohba
                            || sr.GetSRType(false) == SR.SRTypes.lvmobond
                            || sr.GetSRType(false) == SR.SRTypes.nfs
                            || sr.GetSRType(false) == SR.SRTypes.lvmomirror)
                        {
                            String[] rowTemp = { _xenHost.name_label, _xenHost.address, sr.name_label, Util.DiskSizeString(sr.FreeSpace) };
                            this.DataGridView_DefaultStorage.Rows.Add(rowTemp);
                            this.DataGridView_DefaultStorage.Rows[i].Tag = sr.uuid;
                            this.DataGridView_DefaultStorage.Rows[i].Cells[3].Tag = sr.FreeSpace;
                            i++;
                        }
                    }
                }
            }
        }

        private void InitNetworkListBox()
        {
            this.ComboBox_Network.Items.Clear();
            this.ComboBox_Network.BeginUpdate();
            foreach (XenAPI.Network network in this._xenModelObject.Connection.Cache.Networks)
            {
                if (this.ShowNetwork(network) && network.AutoPlug)
                {
                    NetworkItem item = new NetworkItem();
                    item.text = network.Name;
                    item.value = network.uuid;
                    this.ComboBox_Network.Items.Add(item);
                }
            }
            this.ComboBox_Network.EndUpdate();

            if(this.ComboBox_Network.Items.Count > 0)
            {
                this.ComboBox_Network.SelectedIndex = 0;
            }
        }

        private bool ShowNetwork(XenAPI.Network network)
        {
            if (network.IsGuestInstallerNetwork)
            {
                return false;
            }
            if (network.IsInUseBondSlave)
            {
                return false;
            }

            Host temp_host = null;
            if (this._xenModelObject is VM)
            {
                VM _xenVM = this._xenModelObject as VM;
                temp_host = HalsignHelpers.VMHome(_xenVM);
            }
            else if (this._xenModelObject is Host)
            {
                temp_host = _xenModelObject as Host;
            }
            else
            {
                temp_host = Helpers.GetMaster(this._xenModelObject.Connection);
            }

            if ((temp_host != null) && !HalsignHelpers.hostCanSeeNetwork(temp_host, network))
            {
                return false;
            }
            if ((temp_host == null) && !HalsignHelpers.allHostsCanSeeNetwork(network))
            {
                return false;
            }
            return true;
        }

        private class NetworkItem
        {
            public string text { get; set; }
            public string value { get; set; }
            public override string ToString()
            {
                return text;
            }
        }

        private void CheckBox_VMName_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckBox_VMName.Checked)
            {
                this.TextBox_VMName.Enabled = true;
            }
            else
            {
                this.TextBox_VMName.Enabled = false;
            }
        }

        private void ComboBox_Network_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.OnPageUpdated();
        }

        private void DataGridView_DefaultStorage_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.DataGridView_DefaultStorage.CurrentRow != null)
            {
                int _rowIndex = e.RowIndex;
                this.resotreDataModel.choice_sr_uuid = this.DataGridView_DefaultStorage.Rows[_rowIndex].Tag.ToString();
                this.resotreDataModel.choice_sr_ip_name = this.DataGridView_DefaultStorage.Rows[_rowIndex].Cells[1].Value.ToString() + ":" + this.DataGridView_DefaultStorage.Rows[_rowIndex].Cells[2].Value.ToString();
                this.resotreDataModel.choice_sr_free_space = this.DataGridView_DefaultStorage.Rows[_rowIndex].Cells[3].Value.ToString();
                this.resotreDataModel.choice_sr_free_space_size = long.Parse(this.DataGridView_DefaultStorage.Rows[_rowIndex].Cells[3].Tag.ToString());
                base.OnPageUpdated();
            }
        }

        public bool isBackupNetworkSettingChecked
        {
            get { return CheckBox_BackupSettings.Checked; }
        }

        public bool isNewMacAddressChecked
        {
            get { return CheckBox_NewMACAddress.Checked; }
        }

        public string vm_Name
        {
            get { return (this.CheckBox_VMName.Checked ? this.TextBox_VMName.Text : " "); }
        }

        public string network_Name
        {
            get { return (ComboBox_Network.SelectedItem as NetworkItem).text; }
        }

        public string job_Name
        {
            get { return TextBox_JobName.Text; }
        }

        public string network_UUID
        {
            get { return (ComboBox_Network.SelectedItem as NetworkItem).value; }
        }

        public RestoreDataModel restoreDataModel
        {
            get { return this.resotreDataModel; }
        }

        private void TextBox_JobName_TextChanged(object sender, EventArgs e)
        {
            base.OnPageUpdated();
        }

        private void TextBox_JobName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().Equals("|"))
            {
                e.Handled = true;
                return;
            }
        }

        private void TextBox_VMName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().Equals("|"))
            {
                e.Handled = true;
                return;
            }
        }
	}
}
