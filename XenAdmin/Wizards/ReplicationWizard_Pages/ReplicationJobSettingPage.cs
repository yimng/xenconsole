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
using System.Windows.Forms;
using HalsignLib;
using XenAPI;
using XenAdmin.Controls;
using XenAdmin.Network;
using XenAdmin.Core;
using HalsignModel;

namespace XenAdmin.Wizards.ReplicationWizard_Pages
{
    public class ReplicationJobSettingPage : XenTabPage
	{
        private TextBox DestPasswordTextbox;
        private TextBox DestUsernameTextbox;
        private Label label3;
        private Label label1;
        private DataGridView DefaultStorageDataGridView;
        private GroupBox groupBox2;
        private ComboBox NetworkComboBox;
        private CheckBox NewMacAddressCheckBox;
        private Label label12;
        private Label label11;
        private CheckBox VMNameCheckBox;
        private Label LabelJobName;
        private TextBox VMNameTextBox;
        private TextBox JobNameTextBox;
        private Label LabelRestoreOptions;
        private IContainer components = null;

        private XenConnection selected_xenConnection;
        private Host selected_host;
        private string _choice_sr_uuid;
        private string _choice_sr_name;
        private string _choice_sr_ip_name;
        private string _choice_sr_free_space;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private string _choice_sr_ip;

        public ReplicationJobSettingPage()
		{
			InitializeComponent();
		}

        public  void EditReplicationInitJobSet(BackupRestoreConfig.BrSchedule schedule)
        {
            try
            {
                string[] details = schedule.details.Split('|');
                this._choice_sr_ip = details[0];
                this.DestUsernameTextbox.Text = details[1];
                this.DestPasswordTextbox.Text = details[2];
                if (!details[3].Equals(""))
                {
                    this.VMNameCheckBox.Checked = true;
                    this.VMNameTextBox.Text = details[3];
                }
                this._choice_sr_uuid = details[4];
                if (details[5].Equals("1"))
                {
                    this.NewMacAddressCheckBox.Checked = true;
                }
                NetworkItem item = new NetworkItem();
                item.text = "";
                item.value = details[6];
                this.NetworkComboBox.Items.Add(item);
                this.NetworkComboBox.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplicationJobSettingPage));
            this.DestPasswordTextbox = new System.Windows.Forms.TextBox();
            this.DestUsernameTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DefaultStorageDataGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.NetworkComboBox = new System.Windows.Forms.ComboBox();
            this.NewMacAddressCheckBox = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.VMNameCheckBox = new System.Windows.Forms.CheckBox();
            this.LabelJobName = new System.Windows.Forms.Label();
            this.VMNameTextBox = new System.Windows.Forms.TextBox();
            this.JobNameTextBox = new System.Windows.Forms.TextBox();
            this.LabelRestoreOptions = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DefaultStorageDataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DestPasswordTextbox
            // 
            resources.ApplyResources(this.DestPasswordTextbox, "DestPasswordTextbox");
            this.DestPasswordTextbox.Name = "DestPasswordTextbox";
            this.DestPasswordTextbox.UseSystemPasswordChar = true;
            this.DestPasswordTextbox.TextChanged += new System.EventHandler(this.DestPasswordTextbox_TextChanged);
            // 
            // DestUsernameTextbox
            // 
            resources.ApplyResources(this.DestUsernameTextbox, "DestUsernameTextbox");
            this.DestUsernameTextbox.Name = "DestUsernameTextbox";
            this.DestUsernameTextbox.TextChanged += new System.EventHandler(this.DestUsernameTextbox_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // DefaultStorageDataGridView
            // 
            resources.ApplyResources(this.DefaultStorageDataGridView, "DefaultStorageDataGridView");
            this.DefaultStorageDataGridView.AllowUserToAddRows = false;
            this.DefaultStorageDataGridView.AllowUserToDeleteRows = false;
            this.DefaultStorageDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DefaultStorageDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.DefaultStorageDataGridView.MultiSelect = false;
            this.DefaultStorageDataGridView.Name = "DefaultStorageDataGridView";
            this.DefaultStorageDataGridView.ReadOnly = true;
            this.DefaultStorageDataGridView.RowHeadersVisible = false;
            this.DefaultStorageDataGridView.RowTemplate.Height = 23;
            this.DefaultStorageDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DefaultStorageDataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DefualtStorageDataGridView_RowEnter);
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            resources.ApplyResources(this.Column4, "Column4");
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.NetworkComboBox);
            this.groupBox2.Controls.Add(this.NewMacAddressCheckBox);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // NetworkComboBox
            // 
            resources.ApplyResources(this.NetworkComboBox, "NetworkComboBox");
            this.NetworkComboBox.DropDownHeight = 100;
            this.NetworkComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NetworkComboBox.FormattingEnabled = true;
            this.NetworkComboBox.Name = "NetworkComboBox";
            this.NetworkComboBox.SelectedIndexChanged += new System.EventHandler(this.NetworkComboBox_SelectedIndexChanged);
            // 
            // NewMacAddressCheckBox
            // 
            resources.ApplyResources(this.NewMacAddressCheckBox, "NewMacAddressCheckBox");
            this.NewMacAddressCheckBox.Checked = true;
            this.NewMacAddressCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NewMacAddressCheckBox.Name = "NewMacAddressCheckBox";
            this.NewMacAddressCheckBox.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // VMNameCheckBox
            // 
            resources.ApplyResources(this.VMNameCheckBox, "VMNameCheckBox");
            this.VMNameCheckBox.Name = "VMNameCheckBox";
            this.VMNameCheckBox.UseVisualStyleBackColor = true;
            this.VMNameCheckBox.CheckedChanged += new System.EventHandler(this.VMNameCheckBox_CheckedChanged);
            // 
            // LabelJobName
            // 
            resources.ApplyResources(this.LabelJobName, "LabelJobName");
            this.LabelJobName.Name = "LabelJobName";
            // 
            // VMNameTextBox
            // 
            resources.ApplyResources(this.VMNameTextBox, "VMNameTextBox");
            this.VMNameTextBox.Name = "VMNameTextBox";
            this.VMNameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.VMNameTextBox_KeyPress);
            // 
            // JobNameTextBox
            // 
            resources.ApplyResources(this.JobNameTextBox, "JobNameTextBox");
            this.JobNameTextBox.Name = "JobNameTextBox";
            this.JobNameTextBox.TextChanged += new System.EventHandler(this.JobNameTextBox_TextChanged);
            // 
            // LabelRestoreOptions
            // 
            resources.ApplyResources(this.LabelRestoreOptions, "LabelRestoreOptions");
            this.LabelRestoreOptions.Name = "LabelRestoreOptions";
            // 
            // ReplicationJobSettingPage
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.DestPasswordTextbox);
            this.Controls.Add(this.DestUsernameTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DefaultStorageDataGridView);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.VMNameCheckBox);
            this.Controls.Add(this.LabelJobName);
            this.Controls.Add(this.VMNameTextBox);
            this.Controls.Add(this.JobNameTextBox);
            this.Controls.Add(this.LabelRestoreOptions);
            this.Name = "ReplicationJobSettingPage";
            ((System.ComponentModel.ISupportInitialize)(this.DefaultStorageDataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public override string HelpID
        {
            get
            {
                return "Set Job Settings";
            }
        }

        public override string PageTitle
        {
            get
            {
                return Messages.REPLICATION_JOB_SETTINGS_TITLE;
            }
        }

        public override string Text
        {
            get
            {
                return Messages.REPLICATION_JOB_SETTINGS_TEXT;
            }
        }

        private void InitStorageGridList()
        {
            this.DefaultStorageDataGridView.Rows.Clear();
            List<string> list = new List<string>();
            int i = 0;

            foreach (XenConnection connection in ConnectionsManager.XenConnectionsCopy)
            {
                if (Helpers.GetPoolOfOne(connection) != null)
                {
                    foreach (Host objHost in connection.Cache.Hosts)
                    {
                        PBD[] objArray = objHost.Connection.ResolveAll<PBD>(objHost.PBDs).ToArray();
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
                                    if (sr.GetSRType(false) == SR.SRTypes.lvm || sr.GetSRType(false) == SR.SRTypes.ext 
                                        || sr.GetSRType(false) == SR.SRTypes.lvmoiscsi 
                                        || sr.GetSRType(false) == SR.SRTypes.lvmohba 
                                        || sr.GetSRType(false) == SR.SRTypes.lvmobond
                                        || sr.GetSRType(false) == SR.SRTypes.nfs)
                                    {
                                        object[] rowTemp = { objHost.name_label, objHost.address, sr.name_label, Util.DiskSizeString(sr.FreeSpace) };
                                        this.DefaultStorageDataGridView.Rows.Add(rowTemp);
                                        this.DefaultStorageDataGridView.Rows[i].Tag = sr.uuid;
                                        this.DefaultStorageDataGridView.Rows[i].Cells[0].Tag = connection;
                                        this.DefaultStorageDataGridView.Rows[i].Cells[1].Tag = objHost;
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
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

        private void InitNetworkListBox()
        {
            this.NetworkComboBox.Items.Clear();
            this.NetworkComboBox.BeginUpdate();
            this.NetworkComboBox.Text = Messages.REPLICATION_NETWORK_TEXT;
            this.NetworkComboBox.SelectedItem = null;

            foreach (XenAPI.Network network in this.selected_xenConnection.Cache.Networks)
            {
                if (this.ShowNetwork(network) && network.AutoPlug)
                {
                    NetworkItem item = new NetworkItem();
                    item.text = network.Name;
                    item.value = network.uuid;
                    this.NetworkComboBox.Items.Add(item);
                }
            }
            this.NetworkComboBox.EndUpdate();

            if(this.NetworkComboBox.Items.Count > 0)
            {
                this.NetworkComboBox.SelectedIndex = 0;
            }
        }

        private  void InitCredentials()
        {
            this.DestUsernameTextbox.Text = this.selected_xenConnection.Username;
            this.DestPasswordTextbox.Text = this.selected_xenConnection.Password;
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
            if ((this.selected_host != null) && !HalsignHelpers.hostCanSeeNetwork(selected_host, network))
            {
                return false;
            }
            if ((this.selected_host == null) && !HalsignHelpers.allHostsCanSeeNetwork(network))
            {
                return false;
            }
            return true;
        }

        private void SettingValue()
        {
            if (this.DefaultStorageDataGridView.CurrentRow != null)
            {
                this._choice_sr_uuid = this.DefaultStorageDataGridView.CurrentRow.Tag.ToString();
                this._choice_sr_ip = this.DefaultStorageDataGridView.CurrentRow.Cells[1].Value.ToString().TrimEnd().TrimStart();
                this._choice_sr_name = this.DefaultStorageDataGridView.CurrentRow.Cells[0].Value.ToString();
                this._choice_sr_ip_name = this.DefaultStorageDataGridView.CurrentRow.Cells[1].Value.ToString() + ":"
                                        + this.DefaultStorageDataGridView.CurrentRow.Cells[2].Value.ToString();
                this._choice_sr_free_space = this.DefaultStorageDataGridView.CurrentRow.Cells[3].Value.ToString();
                this.selected_xenConnection = this.DefaultStorageDataGridView.CurrentRow.Cells[0].Tag as XenConnection;
                this.selected_host = this.DefaultStorageDataGridView.CurrentRow.Cells[1].Tag as Host;
            }
        }

        public override void PageLoaded(PageLoadedDirection direction)
        {
            base.PageLoaded(direction);
            if (direction == PageLoadedDirection.Forward)
            {
                HelpersGUI.FocusFirstControl(base.Controls);
                this.InitStorageGridList();
                this.SettingValue();
                this.InitNetworkListBox();
                this.InitCredentials();
            }
        }

        public override bool EnableNext()
        {
            return !string.IsNullOrEmpty(this.JobNameTextBox.Text) && this.NetworkComboBox.SelectedItem != null
                    && this._choice_sr_uuid != null && !string.IsNullOrEmpty(this.DestUsernameTextbox.Text)
                    && !string.IsNullOrEmpty(this.DestPasswordTextbox.Text);
        }

        private void JobNameTextBox_TextChanged(object sender, EventArgs e)
        {
            base.OnPageUpdated();
        }

        private void VMNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().Equals("|"))
            {
                e.Handled = true;
            }
        }

        private void VMNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.VMNameCheckBox.Checked)
            {
                this.VMNameTextBox.Enabled = true;
            }
            else
            {
                this.VMNameTextBox.Enabled = false;
            }
        }

        private void DestUsernameTextbox_TextChanged(object sender, EventArgs e)
        {
            base.OnPageUpdated();
        }

        private void DestPasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            base.OnPageUpdated();
        }

        private void NetworkComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.OnPageUpdated();
        }

        internal bool CheckDestUser()
        {
            return !(!string.IsNullOrEmpty(this.selected_xenConnection.Username) &&
                     !string.IsNullOrEmpty(this.selected_xenConnection.Password)) || this.selected_xenConnection.Username.Equals(this.DestUsernameTextbox.Text) &&
                   this.selected_xenConnection.Password.Equals(this.DestPasswordTextbox.Text);
        }

        private void DefualtStorageDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.DefaultStorageDataGridView.CurrentRow != null)
            {
                int _rowIndex = e.RowIndex;
                this._choice_sr_uuid = this.DefaultStorageDataGridView.Rows[_rowIndex].Tag.ToString();
                this._choice_sr_ip = this.DefaultStorageDataGridView.Rows[_rowIndex].Cells[1].Value.ToString().TrimEnd().TrimStart();
                this._choice_sr_name = this.DefaultStorageDataGridView.Rows[_rowIndex].Cells[0].Value.ToString();
                this._choice_sr_ip_name = this.DefaultStorageDataGridView.Rows[_rowIndex].Cells[1].Value.ToString() + ":" + this.DefaultStorageDataGridView.Rows[_rowIndex].Cells[2].Value.ToString();
                this._choice_sr_free_space = this.DefaultStorageDataGridView.Rows[_rowIndex].Cells[3].Value.ToString();
                this.selected_xenConnection = this.DefaultStorageDataGridView.Rows[_rowIndex].Cells[0].Tag as XenConnection;
                this.selected_host = this.DefaultStorageDataGridView.Rows[_rowIndex].Cells[1].Tag as Host;
                this.InitNetworkListBox();
                this.InitCredentials();
                base.OnPageUpdated();
            }
        }

        internal XenConnection Selected_xenConnection
        {
            get 
            {
                return this.selected_xenConnection;
            }
        }

        internal Host Selected_host
        {
            get
            {
                return this.selected_host;
            }
        }

        internal string Choice_sr_uuid
        {
            get
            {
                return this._choice_sr_uuid;
            }
        }

        internal string Choice_sr_name
        {
            get
            {
                return this._choice_sr_name;
            }
        }

        internal string Choice_sr_ip_name
        {
            get
            {
                return this._choice_sr_ip_name;
            }
        }

        internal string Choice_sr_free_space
        {
            get
            {
                return this._choice_sr_free_space;
            }
        }

        internal string Choice_sr_ip
        {
            get
            {
                return this._choice_sr_ip;
            }
        }

        internal string VMNameText
        {
            get
            {
                return this.VMNameCheckBox.Checked ? this.VMNameTextBox.Text.TrimEnd().TrimStart() : " ";
            }
        }

        internal string NetworkText
        {
            get
            {
                return (this.NetworkComboBox.SelectedItem as NetworkItem).text;
            }
        }

        internal string NetworkValue
        {
            get
            {
                return (this.NetworkComboBox.SelectedItem as NetworkItem).value;
            }
        }

        internal string JobNameText
        {
            set
            {
                this.JobNameTextBox.Text = value;
            }
            get
            {
                return this.JobNameTextBox.Text.TrimEnd().TrimStart();
            }
        }

        internal string DestUsernameText
        {
            get
            {
                return this.DestUsernameTextbox.Text.TrimEnd().TrimStart();
            }
        }

        internal string DestPasswordText
        {
            get
            {
                return this.DestPasswordTextbox.Text.TrimEnd().TrimStart();
            }
        }

        internal string MacText
        {
            get
            {
                return this.NewMacAddressCheckBox.Checked ? "1" : "0";
            }
        }
        
	}
}
