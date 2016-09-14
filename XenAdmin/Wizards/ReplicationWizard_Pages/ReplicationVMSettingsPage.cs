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
using XenAdmin.Controls;
using XenAdmin.Core;

namespace XenAdmin.Wizards.ReplicationWizard_Pages
{
    public partial class ReplicationVMSettingsPage : XenTabPage
	{
        private Label Label_Notice;
        private DataGridView DataGridViewRestoreInfo;
        private Label Label_Warnning;

        private List<VM> vmCheckedList;
        private Dictionary<string, long> vdicheckDictionary; 
        private string vmNameText;
        private string networkText;
        private string _choice_sr_ip_name;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewTextBoxColumn Column11;
        private DataGridViewTextBoxColumn Column12;
        private TableLayoutPanel tableLayoutPanel1;
        private string _choice_sr_free_space;

        public ReplicationVMSettingsPage()
		{
			InitializeComponent();
		}

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplicationVMSettingsPage));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Label_Notice = new System.Windows.Forms.Label();
            this.DataGridViewRestoreInfo = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Label_Warnning = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewRestoreInfo)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label_Notice
            // 
            resources.ApplyResources(this.Label_Notice, "Label_Notice");
            this.Label_Notice.Name = "Label_Notice";
            // 
            // DataGridViewRestoreInfo
            // 
            this.DataGridViewRestoreInfo.AllowUserToAddRows = false;
            this.DataGridViewRestoreInfo.AllowUserToDeleteRows = false;
            this.DataGridViewRestoreInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DataGridViewRestoreInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12});
            resources.ApplyResources(this.DataGridViewRestoreInfo, "DataGridViewRestoreInfo");
            this.DataGridViewRestoreInfo.Name = "DataGridViewRestoreInfo";
            this.DataGridViewRestoreInfo.ReadOnly = true;
            this.DataGridViewRestoreInfo.RowHeadersVisible = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.DataGridViewRestoreInfo.RowsDefaultCellStyle = dataGridViewCellStyle1;
            // 
            // Column6
            // 
            resources.ApplyResources(this.Column6, "Column6");
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column7
            // 
            resources.ApplyResources(this.Column7, "Column7");
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column8
            // 
            resources.ApplyResources(this.Column8, "Column8");
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column9
            // 
            resources.ApplyResources(this.Column9, "Column9");
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column10
            // 
            resources.ApplyResources(this.Column10, "Column10");
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column10.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column11
            // 
            resources.ApplyResources(this.Column11, "Column11");
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column12
            // 
            this.Column12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.Column12, "Column12");
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            this.Column12.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Label_Warnning
            // 
            resources.ApplyResources(this.Label_Warnning, "Label_Warnning");
            this.Label_Warnning.Name = "Label_Warnning";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.Label_Notice, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Label_Warnning, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.DataGridViewRestoreInfo, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // ReplicationVMSettingsPage
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ReplicationVMSettingsPage";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewRestoreInfo)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        public override string HelpID
        {
            get
            {
                return "View VM Settings";
            }
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

        public override void PageLoaded(PageLoadedDirection direction)
        {
            base.PageLoaded(direction);
            if (direction == PageLoadedDirection.Forward)
            {
                HelpersGUI.FocusFirstControl(base.Controls);
                this.BuildRepliactionInfoList();
            }
        }

        private void BuildRepliactionInfoList()
        {
            this.DataGridViewRestoreInfo.Rows.Clear();

            int i = 0;

            foreach (VM vmTemp in this.vmCheckedList)
            {
                //long StorageCount = 0;
                //foreach (VBD vbd in vmTemp.Connection.ResolveAll<VBD>(vmTemp.VBDs))
                //{
                //    if (HalsignHelpers.IsCDROM(vbd))
                //    {
                //        continue;
                //    }
                //    VDI vdi = vmTemp.Connection.Resolve<VDI>(vbd.VDI);
                //    if ((vdi != null) && vdi.Show(true))
                //    {
                //        SR sr = vmTemp.Connection.Resolve<SR>(vdi.SR);
                //        if ((sr != null) && !sr.IsToolsSR)
                //        {
                //            StorageCount += vdi.virtual_size;
                //        }
                //    }
                //}

                this.DataGridViewRestoreInfo.Rows.Add();
                this.DataGridViewRestoreInfo.Rows[i].Cells[1].Value = vmTemp.name_label.ToString().Trim();
                this.DataGridViewRestoreInfo.Rows[i].Cells[2].Value = this.vmNameText;
                this.DataGridViewRestoreInfo.Rows[i].Cells[3].Value = Util.DiskSizeString(vdicheckDictionary[vmTemp.uuid]);
                this.DataGridViewRestoreInfo.Rows[i].Cells[4].Value = this.networkText;
                this.DataGridViewRestoreInfo.Rows[i].Cells[5].Value = this._choice_sr_ip_name;
                this.DataGridViewRestoreInfo.Rows[i].Cells[6].Value = this._choice_sr_free_space;
                i++;
            }
        }

        internal void SettingValue(List<VM> vmCheckedList, Dictionary<string, long> vdicheckDictionary, string vmNameText, string networkText, string _choice_sr_ip_name, string _choice_sr_free_space)
        {
            this.vmCheckedList = vmCheckedList;
            this.vdicheckDictionary = vdicheckDictionary;
            this.vmNameText = vmNameText;
            this.networkText = networkText;
            this._choice_sr_ip_name = _choice_sr_ip_name;
            this._choice_sr_free_space = _choice_sr_free_space;
        }
	}
}
