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
using XenAPI;
using log4net;
using System.Reflection;
using XenAdmin.TabPages;
using HalsignLib;
using HalsignModel;
using XenAdmin;
using XenAdmin.Actions.BRActions;
using XenAdmin.Dialogs;
using XenAdmin.Wizards;

namespace XenAdmin.Controls.HalsignControls
{
	public partial class BackupJobs: UserControl
    {
        private ToolStrip toolStrip_CancelJobs;
        private TabControl tabControl_Jobs;
        private TabPage tabPage_CurrentJob;
        private DataGridView dataGridView_CurrentJobs;
        private TabPage tabPage_JobHistory;
        private DataGridView dataGridView_JobHistory;
        private ToolStripButton toolStripButton_CancelJob;

        private IXenObject _xenModelObject;
        private List<DataGridViewRow> old_jobs;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewProgressColumn dataGridViewProgressColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn columnHeaderHistoryJobName;
        private DataGridViewTextBoxColumn columnHeaderHistoryType;
        private DataGridViewTextBoxColumn columnHeaderResult;
        private DataGridViewTextBoxColumn columnHeaderDuration;
        private DataGridViewTextBoxColumn columnHeaderStarted;
        private DataGridViewTextBoxColumn columnHeaderFinished;
        private DataGridViewTextBoxColumn columnHeaderError;
        private DataGridViewTextBoxColumn _JobName;
        private DataGridViewTextBoxColumn _Type;
        private DataGridViewTextBoxColumn _StartTime;
        private DataGridViewTextBoxColumn _Status;
        private DataGridViewProgressColumn _Progress;
        private DataGridViewTextBoxColumn _CurrentSpeed;
        private DataGridViewTextBoxColumn _TimeRemaining;
        private ToolStripButton toolStripButton_DeleteJob;
        private ToolStripButton toolStripButton_ViewJob;
        private ToolStripButton toolStripButton_EditJob;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
		public BackupJobs()
		{
			InitializeComponent();
		}

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupJobs));
            this.toolStrip_CancelJobs = new System.Windows.Forms.ToolStrip();
            this.tabControl_Jobs = new System.Windows.Forms.TabControl();
            this.tabPage_CurrentJob = new System.Windows.Forms.TabPage();
            this.dataGridView_CurrentJobs = new System.Windows.Forms.DataGridView();
            this._JobName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._Progress = new XenAdmin.TabPages.DataGridViewProgressColumn();
            this._CurrentSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._TimeRemaining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_JobHistory = new System.Windows.Forms.TabPage();
            this.dataGridView_JobHistory = new System.Windows.Forms.DataGridView();
            this.columnHeaderHistoryJobName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeaderHistoryType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeaderResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeaderDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeaderStarted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeaderFinished = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeaderError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewProgressColumn1 = new XenAdmin.TabPages.DataGridViewProgressColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripButton_ViewJob = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_CancelJob = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_DeleteJob = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_EditJob = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_CancelJobs.SuspendLayout();
            this.tabControl_Jobs.SuspendLayout();
            this.tabPage_CurrentJob.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CurrentJobs)).BeginInit();
            this.tabPage_JobHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_JobHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip_CancelJobs
            // 
            resources.ApplyResources(this.toolStrip_CancelJobs, "toolStrip_CancelJobs");
            this.toolStrip_CancelJobs.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_CancelJobs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_EditJob,
            this.toolStripButton_ViewJob,
            this.toolStripButton_CancelJob,
            this.toolStripButton_DeleteJob});
            this.toolStrip_CancelJobs.Name = "toolStrip_CancelJobs";
            // 
            // tabControl_Jobs
            // 
            resources.ApplyResources(this.tabControl_Jobs, "tabControl_Jobs");
            this.tabControl_Jobs.Controls.Add(this.tabPage_CurrentJob);
            this.tabControl_Jobs.Controls.Add(this.tabPage_JobHistory);
            this.tabControl_Jobs.Name = "tabControl_Jobs";
            this.tabControl_Jobs.SelectedIndex = 0;
            this.tabControl_Jobs.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlJobs_Selected);
            // 
            // tabPage_CurrentJob
            // 
            resources.ApplyResources(this.tabPage_CurrentJob, "tabPage_CurrentJob");
            this.tabPage_CurrentJob.Controls.Add(this.dataGridView_CurrentJobs);
            this.tabPage_CurrentJob.Name = "tabPage_CurrentJob";
            this.tabPage_CurrentJob.UseVisualStyleBackColor = true;
            // 
            // dataGridView_CurrentJobs
            // 
            resources.ApplyResources(this.dataGridView_CurrentJobs, "dataGridView_CurrentJobs");
            this.dataGridView_CurrentJobs.AllowUserToAddRows = false;
            this.dataGridView_CurrentJobs.AllowUserToDeleteRows = false;
            this.dataGridView_CurrentJobs.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_CurrentJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CurrentJobs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._JobName,
            this._Type,
            this._StartTime,
            this._Status,
            this._Progress,
            this._CurrentSpeed,
            this._TimeRemaining});
            this.dataGridView_CurrentJobs.Name = "dataGridView_CurrentJobs";
            this.dataGridView_CurrentJobs.ReadOnly = true;
            this.dataGridView_CurrentJobs.RowHeadersVisible = false;
            this.dataGridView_CurrentJobs.RowTemplate.Height = 23;
            this.dataGridView_CurrentJobs.SelectionChanged += new System.EventHandler(this.dataGridView_CurrentJobs_SelectionChanged);
            // 
            // _JobName
            // 
            resources.ApplyResources(this._JobName, "_JobName");
            this._JobName.Name = "_JobName";
            this._JobName.ReadOnly = true;
            // 
            // _Type
            // 
            resources.ApplyResources(this._Type, "_Type");
            this._Type.Name = "_Type";
            this._Type.ReadOnly = true;
            // 
            // _StartTime
            // 
            resources.ApplyResources(this._StartTime, "_StartTime");
            this._StartTime.Name = "_StartTime";
            this._StartTime.ReadOnly = true;
            // 
            // _Status
            // 
            resources.ApplyResources(this._Status, "_Status");
            this._Status.Name = "_Status";
            this._Status.ReadOnly = true;
            // 
            // _Progress
            // 
            resources.ApplyResources(this._Progress, "_Progress");
            this._Progress.Name = "_Progress";
            this._Progress.ReadOnly = true;
            // 
            // _CurrentSpeed
            // 
            resources.ApplyResources(this._CurrentSpeed, "_CurrentSpeed");
            this._CurrentSpeed.Name = "_CurrentSpeed";
            this._CurrentSpeed.ReadOnly = true;
            // 
            // _TimeRemaining
            // 
            resources.ApplyResources(this._TimeRemaining, "_TimeRemaining");
            this._TimeRemaining.Name = "_TimeRemaining";
            this._TimeRemaining.ReadOnly = true;
            // 
            // tabPage_JobHistory
            // 
            resources.ApplyResources(this.tabPage_JobHistory, "tabPage_JobHistory");
            this.tabPage_JobHistory.Controls.Add(this.dataGridView_JobHistory);
            this.tabPage_JobHistory.Name = "tabPage_JobHistory";
            this.tabPage_JobHistory.UseVisualStyleBackColor = true;
            // 
            // dataGridView_JobHistory
            // 
            resources.ApplyResources(this.dataGridView_JobHistory, "dataGridView_JobHistory");
            this.dataGridView_JobHistory.AllowUserToAddRows = false;
            this.dataGridView_JobHistory.AllowUserToDeleteRows = false;
            this.dataGridView_JobHistory.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_JobHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_JobHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnHeaderHistoryJobName,
            this.columnHeaderHistoryType,
            this.columnHeaderResult,
            this.columnHeaderDuration,
            this.columnHeaderStarted,
            this.columnHeaderFinished,
            this.columnHeaderError});
            this.dataGridView_JobHistory.Name = "dataGridView_JobHistory";
            this.dataGridView_JobHistory.ReadOnly = true;
            this.dataGridView_JobHistory.RowHeadersVisible = false;
            this.dataGridView_JobHistory.RowTemplate.Height = 23;
            // 
            // columnHeaderHistoryJobName
            // 
            resources.ApplyResources(this.columnHeaderHistoryJobName, "columnHeaderHistoryJobName");
            this.columnHeaderHistoryJobName.Name = "columnHeaderHistoryJobName";
            this.columnHeaderHistoryJobName.ReadOnly = true;
            // 
            // columnHeaderHistoryType
            // 
            resources.ApplyResources(this.columnHeaderHistoryType, "columnHeaderHistoryType");
            this.columnHeaderHistoryType.Name = "columnHeaderHistoryType";
            this.columnHeaderHistoryType.ReadOnly = true;
            // 
            // columnHeaderResult
            // 
            resources.ApplyResources(this.columnHeaderResult, "columnHeaderResult");
            this.columnHeaderResult.Name = "columnHeaderResult";
            this.columnHeaderResult.ReadOnly = true;
            // 
            // columnHeaderDuration
            // 
            resources.ApplyResources(this.columnHeaderDuration, "columnHeaderDuration");
            this.columnHeaderDuration.Name = "columnHeaderDuration";
            this.columnHeaderDuration.ReadOnly = true;
            this.columnHeaderDuration.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // columnHeaderStarted
            // 
            resources.ApplyResources(this.columnHeaderStarted, "columnHeaderStarted");
            this.columnHeaderStarted.Name = "columnHeaderStarted";
            this.columnHeaderStarted.ReadOnly = true;
            // 
            // columnHeaderFinished
            // 
            resources.ApplyResources(this.columnHeaderFinished, "columnHeaderFinished");
            this.columnHeaderFinished.Name = "columnHeaderFinished";
            this.columnHeaderFinished.ReadOnly = true;
            // 
            // columnHeaderError
            // 
            resources.ApplyResources(this.columnHeaderError, "columnHeaderError");
            this.columnHeaderError.Name = "columnHeaderError";
            this.columnHeaderError.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewProgressColumn1
            // 
            resources.ApplyResources(this.dataGridViewProgressColumn1, "dataGridViewProgressColumn1");
            this.dataGridViewProgressColumn1.Name = "dataGridViewProgressColumn1";
            // 
            // dataGridViewTextBoxColumn4
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn10
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn12, "dataGridViewTextBoxColumn12");
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // toolStripButton_ViewJob
            // 
            resources.ApplyResources(this.toolStripButton_ViewJob, "toolStripButton_ViewJob");
            this.toolStripButton_ViewJob.Name = "toolStripButton_ViewJob";
            this.toolStripButton_ViewJob.Click += new System.EventHandler(this.toolStripButton_ViewJob_Click);
            // 
            // toolStripButton_CancelJob
            // 
            resources.ApplyResources(this.toolStripButton_CancelJob, "toolStripButton_CancelJob");
            this.toolStripButton_CancelJob.Name = "toolStripButton_CancelJob";
            this.toolStripButton_CancelJob.Click += new System.EventHandler(this.ButtonCancelJob_Click);
            // 
            // toolStripButton_DeleteJob
            // 
            resources.ApplyResources(this.toolStripButton_DeleteJob, "toolStripButton_DeleteJob");
            this.toolStripButton_DeleteJob.Name = "toolStripButton_DeleteJob";
            this.toolStripButton_DeleteJob.Click += new System.EventHandler(this.toolStripButton_DeleteJob_Click);
            // 
            // toolStripButton_EditJob
            // 
            resources.ApplyResources(this.toolStripButton_EditJob, "toolStripButton_EditJob");
            this.toolStripButton_EditJob.Name = "toolStripButton_EditJob";
            this.toolStripButton_EditJob.Click += new System.EventHandler(this.toolStripButton_EditJob_Click);
            // 
            // BackupJobs
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.tabControl_Jobs);
            this.Controls.Add(this.toolStrip_CancelJobs);
            this.Name = "BackupJobs";
            this.toolStrip_CancelJobs.ResumeLayout(false);
            this.toolStrip_CancelJobs.PerformLayout();
            this.tabControl_Jobs.ResumeLayout(false);
            this.tabPage_CurrentJob.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CurrentJobs)).EndInit();
            this.tabPage_JobHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_JobHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void BuildList()
        {
            this.dataGridView_CurrentJobs.Rows.Clear();

            if (this._xenModelObject is Pool)
            {
                Pool pool = this._xenModelObject as Pool;
                this.BuildCurrentList(pool);
                VM[] vMs = this._xenModelObject.Connection.Cache.VMs.Where(vm => vm.uuid != null).ToArray();
                foreach (VM vm in vMs)
                {
                    if (!vm.is_a_snapshot && !vm.is_a_template && HalsignHelpers.IsVMShow(vm))
                    {
                        BuildCurrentList(vm, true);
                    }
                }
            }
            else if (this._xenModelObject is VM)
            {
                VM vm = this._xenModelObject as VM;
                if (!vm.is_a_snapshot && !vm.is_a_template && HalsignHelpers.IsVMShow(vm))
                {
                    BuildCurrentList(vm, false);
                }
            }

            ToolsStripButtonStatusUpdate();
        }

        private void BuildCurrentList(Pool pool)
        {
            int i = this.dataGridView_CurrentJobs.Rows.Count;

            if (pool != null)
            {
                var result_pool = from d in pool.other_config where d.Key.StartsWith("halsign_br_job") select d;
                foreach (var item in result_pool)
                {
                    BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(item.Value, typeof(BackupRestoreConfig.Job));
                    if (job != null)
                    {
                        this.dataGridView_CurrentJobs.Rows.Add();
                        this.dataGridView_CurrentJobs.Rows[i].Cells[4].Value = 0;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[0].Value = job.job_name;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[2].Value = string.IsNullOrEmpty(job.start_time)
                            ? string.Empty
                            : HalsignUtil.FormatSecond(long.Parse(job.start_time), "yyyy-MM-dd HH:mm:ss");

                        if (job.request.StartsWith(BackupRestoreConfig.BACKUP_IMMEDIATELY))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_IMMEDIATELY;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_IMMEDIATELY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP))
                        {
                            switch (job.schedule_type)
                            {
                                case 0: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_NOW; break;
                                case 2: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_DAILY; break;
                                case 3: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_WEEKLY; break;
                                case 4: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_CIRCLE; break;
                            }
                            //this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_ONCE))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_ONCE;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_ONCE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_DAILY))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_DAILY;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_DAILY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_CIRCLE))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_CIRCLE;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_CIRCLE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_WEEKLY))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_WEEKLY;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_WEEKLY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_NOW))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.RESTORE_NOW;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.RESTORE_NOW;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_ONCE))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.RESTORE_ONCE;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.RESTORE_ONCE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_IMMEDIATELY))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_IMMEDIATELY;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_IMMEDIATELY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_ONCE))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_ONCE;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_ONCE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_DAILY))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_DAILY;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_DAILY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_WEEKLY))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_WEEKLY;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_WEEKLY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_CIRCLE))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_CIRCLE;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_CIRCLE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_SYNCH))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_SYNCH;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_SYNCH;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.PUBLISH))
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.VTOP_PUBLISH;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.PUBLISH;
                        }
                        else
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.UNKNOWN;
                        }

                        switch (job.status)
                        {
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INACTIVE:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_INACTIVE;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INQUEUE:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_INQUEUE;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_RUNNING:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_RUNNING;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_PENDING:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_PENDING;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_SUCCESS:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_SUCCESS;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_FAILED:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_FAILED;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELED:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_CANCELED;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CHK_ZFS:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_CHK_ZFS;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_GEN_METADATA:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_GEN_METADATA;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_METADATA:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_TRANS_METADATA;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_DATA:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_TRANS_DATA;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_VERIFY_DATA:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_VERIFY_DATA;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_ZFS_SNAP:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_ZFS_SNAP;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_DELETE_SNAP:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_DELETE_SNAP;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELING:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_CANCELING;
                                break;
                            default:
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.UNKNOWN;
                                break;
                        }

                        if (job.progress <= 0 || job.progress > 100)
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[4].Value = 0;
                        }
                        else
                        {
                            this.dataGridView_CurrentJobs.Rows[i].Cells[4].Value = job.progress;
                        }
                        this.dataGridView_CurrentJobs.Rows[i].Cells[5].Value = (job.speed <= 0 ? "" : HalsignUtil.SpeedString(job.speed) + "/s");
                        this.dataGridView_CurrentJobs.Rows[i].Cells[6].Value = (job.left_time <= 0 ? "" : HalsignUtil.FormatSecond(job.left_time));
                        this.dataGridView_CurrentJobs.Rows[i].Tag = job.pid;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[0].Tag = job.key;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Tag = job.status;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[4].Tag = job.host;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[5].Tag = pool;
                        i++;
                    }
                }
            }
        }

        private void BuildCurrentList(VM vm, bool isPool)
        {
            var result = from d in vm.other_config where d.Key.StartsWith("halsign_br_job") select d;
            int i = 0;

            if (isPool)
            {
                i = this.dataGridView_CurrentJobs.Rows.Count;
            }

            //fill the job list
            foreach (var item in result)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    continue;
                }
                BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(item.Value, typeof(BackupRestoreConfig.Job));
                if (job != null)
                {
                    this.dataGridView_CurrentJobs.Rows.Add();
                    this.dataGridView_CurrentJobs.Rows[i].Cells[4].Value = 0;
                    this.dataGridView_CurrentJobs.Rows[i].Cells[0].Value = job.job_name;
                    this.dataGridView_CurrentJobs.Rows[i].Cells[2].Value = string.IsNullOrEmpty(job.start_time)
                        ? string.Empty
                        : HalsignUtil.FormatSecond(long.Parse(job.start_time), "yyyy-MM-dd HH:mm:ss");

                    if (job.request.StartsWith(BackupRestoreConfig.BACKUP_IMMEDIATELY))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_IMMEDIATELY;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_IMMEDIATELY;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_ONCE))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_ONCE;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_ONCE;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP_ONCE))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_ONCE;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP_ONCE;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP))
                    {
                        switch (job.schedule_type)
                        {
                            case 0: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_NOW; break;
                            case 2: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_DAILY; break;
                            case 3: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_WEEKLY; break;
                            case 4: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_CIRCLE; break;
                        }
                        //this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_DAILY))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_DAILY;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_DAILY;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_CIRCLE))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_CIRCLE;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_CIRCLE;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_WEEKLY))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_WEEKLY;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_WEEKLY;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_NOW))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.RESTORE_NOW;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.RESTORE_NOW;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_ONCE))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.RESTORE_ONCE;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.RESTORE_ONCE;
                    }

                    else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_IMMEDIATELY))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_IMMEDIATELY;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_IMMEDIATELY;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_ONCE))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_ONCE;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_ONCE;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_DAILY))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_DAILY;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_DAILY;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_WEEKLY))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_WEEKLY;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_WEEKLY;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_CIRCLE))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_CIRCLE;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_CIRCLE;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_SYNCH))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_SYNCH;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_SYNCH;
                    }
                    else if (job.request.StartsWith(BackupRestoreConfig.PUBLISH))
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.VTOP_PUBLISH;
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.PUBLISH;
                    }
                    else
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.UNKNOWN;
                    }

                    switch (job.status)
                    {
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INACTIVE:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_INACTIVE;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INQUEUE:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_INQUEUE;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_RUNNING:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_RUNNING;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_PENDING:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_PENDING;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_SUCCESS:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_SUCCESS;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_FAILED:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_FAILED;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELED:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_CANCELED;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CHK_ZFS:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_CHK_ZFS;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_GEN_METADATA:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_GEN_METADATA;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_METADATA:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_TRANS_METADATA;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_DATA:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_TRANS_DATA;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_VERIFY_DATA:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_VERIFY_DATA;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_ZFS_SNAP:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_ZFS_SNAP;
                            break;
                        case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_DELETE_SNAP:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_DELETE_SNAP;
                            break;
                        default:
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.UNKNOWN;
                            break;
                    }

                    if (job.progress <= 0 || job.progress > 100)
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[4].Value = 0;
                    }
                    else
                    {
                        this.dataGridView_CurrentJobs.Rows[i].Cells[4].Value = job.progress;
                    }
                    this.dataGridView_CurrentJobs.Rows[i].Cells[5].Value = (job.speed <= 0 ? "" : HalsignUtil.SpeedString(job.speed) + "/s");
                    this.dataGridView_CurrentJobs.Rows[i].Cells[6].Value = (job.left_time <= 0 ? "" : HalsignUtil.FormatSecond(job.left_time));
                    this.dataGridView_CurrentJobs.Rows[i].Tag = job.pid;
                    this.dataGridView_CurrentJobs.Rows[i].Cells[0].Tag = job.key;
                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Tag = job.status;
                    this.dataGridView_CurrentJobs.Rows[i].Cells[4].Tag = job.host;
                    this.dataGridView_CurrentJobs.Rows[i].Cells[5].Tag = vm;
                    i++;
                }
            }
        }

        private void UpdateList()
        {
            old_jobs = new List<DataGridViewRow>();
            for (int i = 0; i < this.dataGridView_CurrentJobs.RowCount; i++)
            {
                old_jobs.Add(this.dataGridView_CurrentJobs.Rows[i]);
            }
            if (this._xenModelObject is Pool)
            {
                Pool pool = this._xenModelObject as Pool;
                this.UpdateCurrentList(pool);
                VM[] vMs = this._xenModelObject.Connection.Cache.VMs.Where(vm => vm.uuid != null).ToArray();
                foreach (VM vm in vMs)
                {
                    if (!vm.is_a_snapshot && !vm.is_a_template && HalsignHelpers.IsVMShow(vm))
                    {
                        UpdateCurrentList(vm);
                    }
                }
            }
            else if (this._xenModelObject is VM)
            {
                VM vm = this._xenModelObject as VM;
                if (!vm.is_a_snapshot && !vm.is_a_template && HalsignHelpers.IsVMShow(vm))
                {
                    UpdateCurrentList(vm);
                }
            }

            foreach (DataGridViewRow _item in old_jobs)
            {
                this.dataGridView_CurrentJobs.Rows.Remove(_item);
            }

            ToolsStripButtonStatusUpdate();
        }

        private void UpdateCurrentList(Pool pool)
        {
            if (pool != null)
            {
                var result_pool = from d in pool.other_config where d.Key.StartsWith("halsign_br_job") select d;
                foreach (var item in result_pool)
                {
                    BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(item.Value, typeof(BackupRestoreConfig.Job));
                    if (job != null)
                    {
                        bool new_job = true;
                        for (int i = 0; i < this.dataGridView_CurrentJobs.RowCount; i++)
                        {
                            if (this.dataGridView_CurrentJobs.Rows[i].Cells[0].Tag == null)
                            {
                                continue;
                            }

                            if (job.key == this.dataGridView_CurrentJobs.Rows[i].Cells[0].Tag.ToString())
                            {
                                old_jobs.Remove(this.dataGridView_CurrentJobs.Rows[i]);
                                new_job = false;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[0].Value = job.job_name;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[2].Value =
                                string.IsNullOrEmpty(job.start_time)
                                    ? string.Empty
                                    : HalsignUtil.FormatSecond(long.Parse(job.start_time), "yyyy-MM-dd HH:mm:ss");
                                if (job.request.StartsWith(BackupRestoreConfig.BACKUP_IMMEDIATELY))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_IMMEDIATELY;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_IMMEDIATELY;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP))
                                {
                                    switch (job.schedule_type)
                                    {
                                        case 0: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_NOW; break;
                                        case 2: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_DAILY; break;
                                        case 3: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_WEEKLY; break;
                                        case 4: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_CIRCLE; break;
                                    }
                                    //this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_ONCE))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_ONCE;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_ONCE;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP_ONCE))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_ONCE;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP_ONCE;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_DAILY))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_DAILY;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_DAILY;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_CIRCLE))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_CIRCLE;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_CIRCLE;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_WEEKLY))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_WEEKLY;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_WEEKLY;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_NOW))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.RESTORE_NOW;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.RESTORE_NOW;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_ONCE))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.RESTORE_ONCE;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.RESTORE_ONCE;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_IMMEDIATELY))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_IMMEDIATELY;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_IMMEDIATELY;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_ONCE))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_ONCE;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_ONCE;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_DAILY))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_DAILY;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_DAILY;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_WEEKLY))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_WEEKLY;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_WEEKLY;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_CIRCLE))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_CIRCLE;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_CIRCLE;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_SYNCH))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_SYNCH;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_SYNCH;
                                }
                                else if (job.request.StartsWith(BackupRestoreConfig.PUBLISH))
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.VTOP_PUBLISH;
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.PUBLISH;
                                }
                                else
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.UNKNOWN;
                                }

                                switch (job.status)
                                {
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INACTIVE:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_INACTIVE;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INQUEUE:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_INQUEUE;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_RUNNING:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_RUNNING;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_PENDING:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_PENDING;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_SUCCESS:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_SUCCESS;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_FAILED:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_FAILED;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELED:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_CANCELED;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CHK_ZFS:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_CHK_ZFS;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_GEN_METADATA:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_GEN_METADATA;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_METADATA:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_TRANS_METADATA;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_DATA:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_TRANS_DATA;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_VERIFY_DATA:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_VERIFY_DATA;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_ZFS_SNAP:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_ZFS_SNAP;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_DELETE_SNAP:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_DELETE_SNAP;
                                        break;
                                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELING:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_CANCELING;
                                        break;
                                    default:
                                        this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.UNKNOWN;
                                        break;
                                }

                                if (job.progress <= 0 || job.progress > 100)
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[4].Value = 0;
                                }
                                else
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[4].Value = job.progress;
                                }
                                this.dataGridView_CurrentJobs.Rows[i].Cells[5].Value = (job.speed <= 0 ? "" : HalsignUtil.SpeedString(job.speed) + "/s");
                                this.dataGridView_CurrentJobs.Rows[i].Cells[6].Value = (job.left_time <= 0 ? "" : HalsignUtil.FormatSecond(job.left_time));
                                this.dataGridView_CurrentJobs.Rows[i].Tag = job.pid;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[0].Tag = job.key;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[3].Tag = job.status;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[4].Tag = job.host;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[5].Tag = pool;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[6].Tag = job.request;
                                break;
                            }
                        }

                        if (new_job)
                        {
                            int row_number = this.dataGridView_CurrentJobs.RowCount;
                            this.dataGridView_CurrentJobs.Rows.Add();
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[4].Value = 0;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[0].Value = job.job_name;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[2].Value =
                                string.IsNullOrEmpty(job.start_time)
                                    ? string.Empty
                                    : HalsignUtil.FormatSecond(long.Parse(job.start_time), "yyyy-MM-dd HH:mm:ss");

                            if (job.request.StartsWith(BackupRestoreConfig.BACKUP_IMMEDIATELY))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.BACKUP_IMMEDIATELY;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.BACKUP_IMMEDIATELY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP))
                            {
                                switch (job.schedule_type)
                                {
                                    case 0: this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP_NOW; break;
                                    case 2: this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP_DAILY; break;
                                    case 3: this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP_WEEKLY; break;
                                    case 4: this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP_CIRCLE; break;
                                }
                                //this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_ONCE))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.BACKUP_ONCE;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.BACKUP_ONCE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP_ONCE))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP_ONCE;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP_ONCE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_DAILY))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.BACKUP_DAILY;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.BACKUP_DAILY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_CIRCLE))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.BACKUP_CIRCLE;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.BACKUP_CIRCLE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_WEEKLY))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.BACKUP_WEEKLY;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.BACKUP_WEEKLY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_NOW))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.RESTORE_NOW;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.RESTORE_NOW;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_ONCE))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.RESTORE_ONCE;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.RESTORE_ONCE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_IMMEDIATELY))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_IMMEDIATELY;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_IMMEDIATELY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_ONCE))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_ONCE;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_ONCE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_DAILY))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_DAILY;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_DAILY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_WEEKLY))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_WEEKLY;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_WEEKLY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_CIRCLE))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_CIRCLE;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_CIRCLE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_SYNCH))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_SYNCH;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_SYNCH;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.PUBLISH))
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.VTOP_PUBLISH;
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.PUBLISH;
                            }
                            else
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.UNKNOWN;
                            }

                            switch (job.status)
                            {
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INACTIVE:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_INACTIVE;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INQUEUE:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_INQUEUE;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_RUNNING:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_RUNNING;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_PENDING:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_PENDING;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_SUCCESS:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_SUCCESS;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_FAILED:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_FAILED;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELED:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_CANCELED;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CHK_ZFS:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_CHK_ZFS;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_GEN_METADATA:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_GEN_METADATA;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_METADATA:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_TRANS_METADATA;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_DATA:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_TRANS_DATA;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_VERIFY_DATA:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_VERIFY_DATA;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_ZFS_SNAP:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_ZFS_SNAP;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_DELETE_SNAP:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_DELETE_SNAP;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELING:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_CANCELING;
                                    break;
                                default:
                                    this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.UNKNOWN;
                                    break;
                            }

                            if (job.progress <= 0 || job.progress > 100)
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[4].Value = 0;
                            }
                            else
                            {
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[4].Value = job.progress;
                            }
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[5].Value = (job.speed <= 0 ? "" : HalsignUtil.SpeedString(job.speed) + "/s");
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[6].Value = (job.left_time <= 0 ? "" : HalsignUtil.FormatSecond(job.left_time));
                            this.dataGridView_CurrentJobs.Rows[row_number].Tag = job.pid;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[0].Tag = job.key;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Tag = job.status;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[4].Tag = job.host;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[5].Tag = pool;
                        }
                    }
                }
            }
        }

        private void UpdateCurrentList(VM vm)
        {
            var result = from d in vm.other_config where d.Key.StartsWith("halsign_br_job") select d;
            foreach (var item in result)
            {
                BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(item.Value, typeof(BackupRestoreConfig.Job));
                if (job != null)
                {
                    bool new_job = true;
                    for (int i = 0; i < this.dataGridView_CurrentJobs.RowCount; i++)
                    {
                        if (!(this.dataGridView_CurrentJobs.Rows[i].Cells[5].Tag is VM) || this.dataGridView_CurrentJobs.Rows[i].Cells[0].Tag == null)
                        {
                            continue;
                        }

                        VM _vm = this.dataGridView_CurrentJobs.Rows[i].Cells[5].Tag as VM;
                        if (vm.uuid == _vm.uuid && job.key == this.dataGridView_CurrentJobs.Rows[i].Cells[0].Tag.ToString())
                        {
                            old_jobs.Remove(this.dataGridView_CurrentJobs.Rows[i]);
                            new_job = false;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[0].Value = job.job_name;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[2].Value =
                            string.IsNullOrEmpty(job.start_time)
                                ? string.Empty
                                : HalsignUtil.FormatSecond(long.Parse(job.start_time), "yyyy-MM-dd HH:mm:ss");
                            if (job.request.StartsWith(BackupRestoreConfig.BACKUP_IMMEDIATELY))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_IMMEDIATELY;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_IMMEDIATELY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP))
                            {
                                /*if (job.expect_full_count > 0)
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP;
                                }
                                else
                                {
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_NOW;
                                }*/
                                switch (job.schedule_type)
                                {
                                    case 0: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_NOW; break;
                                    case 2: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_DAILY; break;
                                    case 3: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_WEEKLY; break;
                                    case 4: this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_CIRCLE; break;
                                }
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_ONCE))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_ONCE;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_ONCE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP_ONCE))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.FULL_BACKUP_ONCE;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP_ONCE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_DAILY))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_DAILY;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_DAILY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_CIRCLE))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_CIRCLE;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_CIRCLE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_WEEKLY))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.BACKUP_WEEKLY;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.BACKUP_WEEKLY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_NOW))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.RESTORE_NOW;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.RESTORE_NOW;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_ONCE))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.RESTORE_ONCE;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.RESTORE_ONCE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_IMMEDIATELY))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_IMMEDIATELY;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_IMMEDIATELY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_ONCE))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_ONCE;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_ONCE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_DAILY))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_DAILY;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_DAILY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_WEEKLY))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_WEEKLY;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_WEEKLY;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_CIRCLE))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_CIRCLE;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_CIRCLE;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_SYNCH))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.REPLICATION_SYNCH;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.REPLICATION_SYNCH;
                            }
                            else if (job.request.StartsWith(BackupRestoreConfig.PUBLISH))
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.VTOP_PUBLISH;
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Tag = BackupRestoreConfig.PUBLISH;
                            }
                            else
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[1].Value = Messages.UNKNOWN;
                            }

                            switch (job.status)
                            {
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INACTIVE:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_INACTIVE;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INQUEUE:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_INQUEUE;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_RUNNING:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_RUNNING;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_PENDING:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_PENDING;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_SUCCESS:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_SUCCESS;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_FAILED:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_FAILED;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELED:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.BR_STATUS_CANCELED;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CHK_ZFS:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_CHK_ZFS;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_GEN_METADATA:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_GEN_METADATA;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_METADATA:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_TRANS_METADATA;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_DATA:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_TRANS_DATA;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_VERIFY_DATA:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_VERIFY_DATA;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_ZFS_SNAP:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_ZFS_SNAP;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_DELETE_SNAP:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_DELETE_SNAP;
                                    break;
                                case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELING:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.JOB_STATUS_CANCELING;
                                    break;
                                default:
                                    this.dataGridView_CurrentJobs.Rows[i].Cells[3].Value = Messages.UNKNOWN;
                                    break;
                            }

                            if (job.progress <= 0 || job.progress > 100)
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[4].Value = 0;
                            }
                            else
                            {
                                this.dataGridView_CurrentJobs.Rows[i].Cells[4].Value = job.progress;
                            }
                            this.dataGridView_CurrentJobs.Rows[i].Cells[5].Value = (job.speed <= 0 ? "" : HalsignUtil.SpeedString(job.speed) + "/s");
                            this.dataGridView_CurrentJobs.Rows[i].Cells[6].Value = (job.left_time <= 0 ? "" : HalsignUtil.FormatSecond(job.left_time));
                            this.dataGridView_CurrentJobs.Rows[i].Tag = job.pid;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[0].Tag = job.key;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[3].Tag = job.status;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[4].Tag = job.host;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[5].Tag = vm;
                            this.dataGridView_CurrentJobs.Rows[i].Cells[6].Tag = job.request;
                            break;
                        }
                    }

                    if (new_job)
                    {
                        int row_number = this.dataGridView_CurrentJobs.RowCount;
                        this.dataGridView_CurrentJobs.Rows.Add();
                        this.dataGridView_CurrentJobs.Rows[row_number].Cells[4].Value = 0;
                        this.dataGridView_CurrentJobs.Rows[row_number].Cells[0].Value = job.job_name;
                        this.dataGridView_CurrentJobs.Rows[row_number].Cells[2].Value =
                            string.IsNullOrEmpty(job.start_time)
                                ? string.Empty
                                : HalsignUtil.FormatSecond(long.Parse(job.start_time), "yyyy-MM-dd HH:mm:ss");

                        if (job.request.StartsWith(BackupRestoreConfig.BACKUP_IMMEDIATELY))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.BACKUP_IMMEDIATELY;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.BACKUP_IMMEDIATELY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP))
                        {
                            switch (job.schedule_type)
                            {
                                case 0: this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP_NOW; break;
                                case 2: this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP_DAILY; break;
                                case 3: this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP_WEEKLY; break;
                                case 4: this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP_CIRCLE; break;
                            }
                            //this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_ONCE))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.BACKUP_ONCE;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.BACKUP_ONCE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP_ONCE))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.FULL_BACKUP_ONCE;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.FULL_BACKUP_ONCE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_DAILY))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.BACKUP_DAILY;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.BACKUP_DAILY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_CIRCLE))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.BACKUP_CIRCLE;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.BACKUP_CIRCLE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_WEEKLY))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.BACKUP_WEEKLY;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.BACKUP_WEEKLY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_NOW))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.RESTORE_NOW;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.RESTORE_NOW;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.RESTORE_ONCE))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.RESTORE_ONCE;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.RESTORE_ONCE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_IMMEDIATELY))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_IMMEDIATELY;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_IMMEDIATELY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_ONCE))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_ONCE;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_ONCE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_DAILY))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_DAILY;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_DAILY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_WEEKLY))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_WEEKLY;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_WEEKLY;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_CIRCLE))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_CIRCLE;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_CIRCLE;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_SYNCH))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.REPLICATION_SYNCH;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.REPLICATION_SYNCH;
                        }
                        else if (job.request.StartsWith(BackupRestoreConfig.PUBLISH))
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.VTOP_PUBLISH;
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Tag = BackupRestoreConfig.PUBLISH;
                        }
                        else
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[1].Value = Messages.UNKNOWN;
                        }

                        switch (job.status)
                        {
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INACTIVE:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_INACTIVE;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INQUEUE:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_INQUEUE;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_RUNNING:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_RUNNING;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_PENDING:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_PENDING;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_SUCCESS:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_SUCCESS;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_FAILED:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_FAILED;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELED:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.BR_STATUS_CANCELED;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CHK_ZFS:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_CHK_ZFS;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_GEN_METADATA:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_GEN_METADATA;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_METADATA:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_TRANS_METADATA;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_DATA:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_TRANS_DATA;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_VERIFY_DATA:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_VERIFY_DATA;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_ZFS_SNAP:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_ZFS_SNAP;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_DELETE_SNAP:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_DELETE_SNAP;
                                break;
                            case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELING:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.JOB_STATUS_CANCELING;
                                break;
                            default:
                                this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Value = Messages.UNKNOWN;
                                break;
                        }

                        if (job.progress <= 0 || job.progress > 100)
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[4].Value = 0;
                        }
                        else
                        {
                            this.dataGridView_CurrentJobs.Rows[row_number].Cells[4].Value = job.progress;
                        }
                        this.dataGridView_CurrentJobs.Rows[row_number].Cells[5].Value = (job.speed <= 0 ? "" : HalsignUtil.SpeedString(job.speed) + "/s");
                        this.dataGridView_CurrentJobs.Rows[row_number].Cells[6].Value = (job.left_time <= 0 ? "" : HalsignUtil.FormatSecond(job.left_time));
                        this.dataGridView_CurrentJobs.Rows[row_number].Tag = job.pid;
                        this.dataGridView_CurrentJobs.Rows[row_number].Cells[0].Tag = job.key;
                        this.dataGridView_CurrentJobs.Rows[row_number].Cells[3].Tag = job.status;
                        this.dataGridView_CurrentJobs.Rows[row_number].Cells[4].Tag = job.host;
                        this.dataGridView_CurrentJobs.Rows[row_number].Cells[5].Tag = vm;
                    }
                }
            }
        }

        public IXenObject XenModelObject
        {
            set
            {
                Program.AssertOnEventThread();
                if (value != null)
                {
                    if (this._xenModelObject != value)
                    {
                        this._xenModelObject = value;
                        this.tabControl_Jobs.SelectTab(0);
                        if (this._xenModelObject is Pool)
                        {
                            Pool pool = this._xenModelObject as Pool;
                            pool.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
                            VM[] vMs = this._xenModelObject.Connection.Cache.VMs.Where(vm => vm.uuid != null).ToArray();
                            foreach (VM vm in vMs)
                            {
                                if (!vm.is_a_snapshot && !vm.is_a_template && HalsignHelpers.IsVMShow(vm))
                                {
                                    vm.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
                                }
                            }
                        }
                        else if (this._xenModelObject is VM)
                        {
                            VM vm = this._xenModelObject as VM;
                            vm.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
                        }
                    }
                }
                this.BuildList();
            }
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "other_config")
            {
                this.UpdateList();
            }
        }

        private void tabControlJobs_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 1)
            {
                this.toolStripButton_CancelJob.Enabled = false;
                this.toolStripButton_DeleteJob.Enabled = false;
                this.toolStripButton_ViewJob.Enabled = false;
                this.toolStripButton_EditJob.Enabled = false;
                this.dataGridView_JobHistory.Rows.Clear();
                BackupAction action = new BackupAction(Messages.GET_BR_HISTORY, BackupRestoreConfig.BackupActionKind.Histroy, this._xenModelObject);

                if (action != null)
                {
                    ProgressBarStyle progressBarStyle = ProgressBarStyle.Marquee;
                    ActionProgressDialog dialog = new ActionProgressDialog(action, progressBarStyle);
                    dialog.ShowCancel = true;
                    DialogResult result = dialog.ShowDialog(this);

                    if (result != DialogResult.OK)
                    {
                        action.Cancel();
                    }
                    else
                    {
                        try
                        {
                            if (action.historyResultList.Count > 0 && (action.Succeeded || action.Exception == null || string.IsNullOrEmpty(action.Exception.Message)))
                            {
                                this.dataGridView_JobHistory.Rows.Clear();
                                int number = 0;
                                foreach (BackupRestoreConfig.DRResult hisresult in action.historyResultList)
                                {
                                    if (hisresult.items == null)
                                        continue;
                                    foreach (BackupRestoreConfig.DRItemInfo info in hisresult.items)
                                    {
                                        BuildHistoryList(info, number);
                                        number++;
                                    }
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            log.Error("The exception happened when get the history: ", exception);
                        }
                    }
                }
            }
            else if (e.TabPageIndex == 0)
            {
                ToolsStripButtonStatusUpdate();
            }
        }

        private void BuildHistoryList(BackupRestoreConfig.DRItemInfo _history, int number)
        {
            if (_history != null)
            {
                this.dataGridView_JobHistory.Rows.Add();
                this.dataGridView_JobHistory.Rows[number].Cells[0].Value = _history.job_name;
                if (_history.request_type.Equals(BackupRestoreConfig.BACKUP_IMMEDIATELY))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.BACKUP_IMMEDIATELY;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.FULL_BACKUP))
                {
                    switch (_history.schedule_type)
                    {
                        case 2: this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.FULL_BACKUP_DAILY; break;
                        case 3: this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.FULL_BACKUP_WEEKLY; break;
                        case 4: this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.FULL_BACKUP_CIRCLE; break;
                        default: this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.FULL_BACKUP_NOW; break;
                    }
                    //this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.FULL_BACKUP;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.BACKUP_ONCE))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.BACKUP_ONCE;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.FULL_BACKUP_ONCE))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.FULL_BACKUP_ONCE;
                }

                else if (_history.request_type.Equals(BackupRestoreConfig.BACKUP_DAILY))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.BACKUP_DAILY;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.BACKUP_WEEKLY))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.BACKUP_WEEKLY;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.BACKUP_CIRCLE))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.BACKUP_CIRCLE;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.RESTORE_NOW))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.RESTORE_NOW;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.RESTORE_ONCE))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.RESTORE_ONCE;
                }

                else if (_history.request_type.Equals(BackupRestoreConfig.REPLICATION_IMMEDIATELY))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.REPLICATION_IMMEDIATELY;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.REPLICATION_ONCE))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.REPLICATION_ONCE;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.REPLICATION_DAILY))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.REPLICATION_DAILY;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.REPLICATION_WEEKLY))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.REPLICATION_WEEKLY;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.REPLICATION_CIRCLE))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.REPLICATION_CIRCLE;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.REPLICATION_SYNCH))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.REPLICATION_SYNCH;
                }
                else if (_history.request_type.Equals(BackupRestoreConfig.PUBLISH))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.VTOP_PUBLISH;
                }
                else
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[1].Value = Messages.UNKNOWN;
                }

                switch (_history.status)
                {
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INACTIVE:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.BR_STATUS_INACTIVE;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INQUEUE:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.BR_STATUS_INQUEUE;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_RUNNING:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.BR_STATUS_RUNNING;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_PENDING:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.BR_STATUS_PENDING;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_SUCCESS:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.BR_STATUS_SUCCESS;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_FAILED:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.BR_STATUS_FAILED;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELED:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.BR_STATUS_CANCELED;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CHK_ZFS:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.JOB_STATUS_CHK_ZFS;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_GEN_METADATA:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.JOB_STATUS_GEN_METADATA;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_METADATA:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.JOB_STATUS_TRANS_METADATA;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_DATA:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.JOB_STATUS_TRANS_DATA;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_VERIFY_DATA:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.JOB_STATUS_VERIFY_DATA;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_ZFS_SNAP:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.JOB_STATUS_ZFS_SNAP;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_DELETE_SNAP:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.JOB_STATUS_DELETE_SNAP;
                        break;
                    case (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELING:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.JOB_STATUS_CANCELING;
                        break;
                    default:
                        this.dataGridView_JobHistory.Rows[number].Cells[2].Value = Messages.UNKNOWN;
                        break;
                }

                if (!string.IsNullOrEmpty(_history.start_time) && !string.IsNullOrEmpty(_history.end_time))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[3].Value = HalsignUtil.FormatSecond(long.Parse(_history.end_time) - long.Parse(_history.start_time));
                }
                if (!string.IsNullOrEmpty(_history.start_time))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[4].Value = HalsignUtil.FormatSecond(long.Parse(_history.start_time), "yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(_history.end_time))
                {
                    this.dataGridView_JobHistory.Rows[number].Cells[5].Value = HalsignUtil.FormatSecond(long.Parse(_history.end_time), "yyyy-MM-dd HH:mm:ss");
                }
                switch (_history.error_code)
                {
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_SR_INFO:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_SR_INFO;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_SR_CHKZFS:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_SR_CHKZFS;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_SR_ZFS_SNAP:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_SR_ZFS_SNAP;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_LOGIN:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_LOGIN;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_VM_BY_UUID:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_VM_BY_UUID;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_VM_REC:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_VM_REC;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_UPDATE_OTH_CFG:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_UPDATE_OTH_CFG;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_VIFS:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_VIFS;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_VIF:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_VIFS;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_VIF_REC:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_VIFS;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_NETWORK:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_NETWORK;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_NETWORK_REC:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_NETWORK;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_VBDS:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_VBDS;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_VBD:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_VBDS;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_VBD_REC:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_VBDS;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_VDI:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_VDI;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_VDI_REC:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_VDI;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_SR:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_SR;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_SR_REC:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_SR;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_GET_POOLS:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_GET_POOLS;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_NEW_VM:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_NEW_VM;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_NEW_VIF:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_NEW_VIF;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_NEW_VDI:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_NEW_VDI;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_NEW_VBD:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_NEW_VBD;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_AGT_NEW_JOB:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_AGT_NEW_JOB;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_OTH_MAKE_DIR:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_OTH_MAKE_DIR;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_OTH_MAKE_FILE:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_OTH_MAKE_FILE;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_OTH_CHANGE_DIR:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_OTH_CHANGE_DIR;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_OTH_RSYNC:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_OTH_RSYNC;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_NEW_SNAP:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_NEW_SNAP;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_DEL_SNAP:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_DEL_SNAP;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_DEL_VDI_SNAP:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_DEL_SNAP;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_REMOTE_LOGIN:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_REMOTE_LOGIN;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_XEN_DISK_CONFLICT:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_XEN_DISK_CONFLICT;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_OTH_UNKNOWN:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_OTH_UNKNOWN;
                        break;
                    case (int)BackupRestoreConfig.ERROR_CODE.ERR_OTH_REPLICATION_VM_RUN:
                        this.dataGridView_JobHistory.Rows[number].Cells[6].Value = Messages.ERR_OTH_REPLICATION_VM_RUN;
                        break;
                    default:
                        break;
                }
            }
        }

        private void ButtonCancelJob_Click(object sender, EventArgs e)
        {
            //bool _commit = Program.MainWindow.Confirms(this._xenModelObject.Connection, Messages.CONFIRM_CANCEL_BR_JOB, new object[] { this.dataGridView_CurrentJobs.CurrentRow.Cells[0].Value });
            bool _commit = DialogResult.OK == MessageBox.Show(this, string.Format(Messages.CONFIRM_CANCEL_BR_JOB, new object[] { this.dataGridView_CurrentJobs.CurrentRow.Cells[0].Value }), Messages.MESSAGEBOX_CONFIRM, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (_commit && this.dataGridView_CurrentJobs.CurrentRow != null)
            {
                Dictionary<string, string> dconf = new Dictionary<string, string>();
                int pid = Int32.Parse(this.dataGridView_CurrentJobs.CurrentRow.Tag.ToString());
                IXenObject deletedObject = this.dataGridView_CurrentJobs.CurrentRow.Cells[4].Tag as IXenObject;

                dconf.Add("agent_param", this.dataGridView_CurrentJobs.CurrentRow.Tag.ToString());
                dconf.Add("command", "d");
                dconf.Add("job_key", this.dataGridView_CurrentJobs.CurrentRow.Cells[0].Tag.ToString());
                dconf.Add("status", this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag.ToString());
                dconf.Add("job_host", this.dataGridView_CurrentJobs.CurrentRow.Cells[4].Tag.ToString());
                dconf.Add("type", this.dataGridView_CurrentJobs.CurrentRow.Cells[1].Tag.ToString());
                BackupAction action = new BackupAction(Messages.BR_JOB_CANCEL, BackupRestoreConfig.BackupActionKind.Job_Cancel, this._xenModelObject, dconf, deletedObject);
                if (action != null)
                {
                    ProgressBarStyle progressBarStyle = ProgressBarStyle.Marquee;
                    ActionProgressDialog dialog = new ActionProgressDialog(action, progressBarStyle);
                    dialog.ShowCancel = false;
                    dialog.ShowDialog(this);
                    try
                    {
                        if (!string.IsNullOrEmpty(action.Result))
                        {
                            Program.MainWindow.BringToFront();
                        }
                    }
                    catch
                    {
                        Program.MainWindow.BringToFront();
                    }
                }
            }
        }

        private void toolStripButton_DeleteJob_Click(object sender, EventArgs e)
        {
            if (this.dataGridView_CurrentJobs.CurrentRow != null)
            {
                //bool _commit = Program.MainWindow.Confirms(this._xenModelObject.Connection, Messages.CONFIRM_DELETE_BR_JOB, new object[] { this.dataGridView_CurrentJobs.CurrentRow.Cells[0].Value });
                bool _commit = DialogResult.OK == MessageBox.Show(this, string.Format(Messages.CONFIRM_CANCEL_BR_JOB, new object[] { this.dataGridView_CurrentJobs.CurrentRow.Cells[0].Value }), Messages.MESSAGEBOX_CONFIRM, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (_commit)
                {
                    if (!IsJobRunning((int)this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag))
                    {
                        string param_key = this.dataGridView_CurrentJobs.CurrentRow.Cells[0].Tag.ToString();
                        VM vm = this.dataGridView_CurrentJobs.CurrentRow.Cells[5].Tag as VM;
                        if (vm != null)
                        {
                            Dictionary<string, string> other_config = vm.other_config;
                            if (this.dataGridView_CurrentJobs.CurrentRow.Cells[1].Tag != null)
                            {
                                string type = this.dataGridView_CurrentJobs.CurrentRow.Cells[1].Tag.ToString();
                                if (other_config.ContainsKey(param_key))
                                {
                                    other_config.Remove(param_key);
                                }

                                if(type == BackupRestoreConfig.BACKUP_ONCE ||
                                    type == BackupRestoreConfig.BACKUP_DAILY ||
                                    type == BackupRestoreConfig.BACKUP_WEEKLY ||
                                    type == BackupRestoreConfig.BACKUP_CIRCLE ||
                                    type == BackupRestoreConfig.FULL_BACKUP_ONCE ||
                                    type == BackupRestoreConfig.FULL_BACKUP)
                                {
                                    if (other_config.ContainsKey("halsign_br_rules"))
                                    {
                                        other_config.Remove("halsign_br_rules");
                                    }
                                }

                                if (type == BackupRestoreConfig.REPLICATION_ONCE ||
                                    type == BackupRestoreConfig.REPLICATION_DAILY ||
                                    type == BackupRestoreConfig.REPLICATION_WEEKLY ||
                                    type == BackupRestoreConfig.REPLICATION_CIRCLE)
                                {
                                    if (other_config.ContainsKey("halsign_rep_rules"))
                                    {
                                        other_config.Remove("halsign_rep_rules");
                                    }
                                }

                                XenAPI.VM.set_other_config(this._xenModelObject.Connection.Session, vm.opaque_ref, other_config);
                                vm.NotifyPropertyChanged("other_config");
                            }
                        }
                    }
                }
            }
        }

        private void ToolsStripButtonStatusUpdate()
        {
            if (this.dataGridView_CurrentJobs.CurrentRow != null && this.tabControl_Jobs.SelectedIndex == 0)
            {
                if (this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag != null &&
                    ((int)this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELED ||
                    (int)this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_SUCCESS ||
                    (int)this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_FAILED ||
                    (int)this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INACTIVE ||
                    (int)this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INQUEUE))
                {
                    this.toolStripButton_DeleteJob.Enabled = true;
                    if ((int)this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_SUCCESS)
                    {
                        this.toolStripButton_DeleteJob.Enabled = false;
                    }
                    this.toolStripButton_CancelJob.Enabled = false;
                }
                else
                {
                    this.toolStripButton_DeleteJob.Enabled = false;
                    this.toolStripButton_CancelJob.Enabled = true;
                }
                this.toolStripButton_ViewJob.Enabled = true;

                string type = this.dataGridView_CurrentJobs.CurrentRow.Cells[1].Tag == null ? "" : this.dataGridView_CurrentJobs.CurrentRow.Cells[1].Tag.ToString();
                if (type == BackupRestoreConfig.REPLICATION_IMMEDIATELY && 
                    this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag != null&&
                    (int)this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_SUCCESS)
                {
                    this.toolStripButton_DeleteJob.Enabled = true;
                }
                else if (type == BackupRestoreConfig.REPLICATION_IMMEDIATELY || type == BackupRestoreConfig.BACKUP_IMMEDIATELY)
                {
                    this.toolStripButton_ViewJob.Enabled = false;
                }

                if (this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag != null &&
                    (int)this.dataGridView_CurrentJobs.CurrentRow.Cells[3].Tag == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_INACTIVE)
                {
                    this.toolStripButton_EditJob.Enabled = true;
                }
                else
                {
                    this.toolStripButton_EditJob.Enabled = false;
                }
                string key = this.dataGridView_CurrentJobs.CurrentRow.Cells[0].Tag == null ? "" : this.dataGridView_CurrentJobs.CurrentRow.Cells[0].Tag.ToString();
                if (key.Equals("") || key.Length > 17)
                {
                    this.toolStripButton_ViewJob.Enabled = false;
                    this.toolStripButton_EditJob.Enabled = false;
                }
            }
            else
            {
                this.toolStripButton_DeleteJob.Enabled = false;
                this.toolStripButton_CancelJob.Enabled = false;
                this.toolStripButton_ViewJob.Enabled = false;
                this.toolStripButton_EditJob.Enabled = false;
            }
        }

        private bool IsJobRunning(int status)
        {
            return (status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CHK_ZFS
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_GEN_METADATA
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_METADATA
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_DATA
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_VERIFY_DATA
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_RUNNING
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_DELETE_SNAP
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_ZFS_SNAP
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELING);
        }

        private void dataGridView_CurrentJobs_SelectionChanged(object sender, EventArgs e)
        {
            ToolsStripButtonStatusUpdate();
        }

        private void toolStripButton_ViewJob_Click(object sender, EventArgs e)
        {
            string type = this.dataGridView_CurrentJobs.CurrentRow.Cells[1].Tag.ToString();
            VM vm = this.dataGridView_CurrentJobs.CurrentRow.Cells[5].Tag as VM;
            if (vm != null)
            {
                Dictionary<string, string> other_config = vm.other_config;
                if (type == BackupRestoreConfig.BACKUP_ONCE ||
                    type == BackupRestoreConfig.BACKUP_DAILY ||
                    type == BackupRestoreConfig.BACKUP_WEEKLY ||
                    type == BackupRestoreConfig.BACKUP_CIRCLE ||
                    type == BackupRestoreConfig.FULL_BACKUP_ONCE ||
                    type == BackupRestoreConfig.FULL_BACKUP)
                {
                    BackupViewDialog backupViewDialog = new BackupViewDialog();
                    backupViewDialog.otherConfig = other_config;
                    backupViewDialog.VMName = vm.name_label;
                    backupViewDialog.InitBackupViewData();
                    backupViewDialog.ShowDialog(Program.MainWindow);
                }
                else if (type == BackupRestoreConfig.REPLICATION_ONCE ||
                        type == BackupRestoreConfig.REPLICATION_DAILY ||
                        type == BackupRestoreConfig.REPLICATION_WEEKLY ||
                        type == BackupRestoreConfig.REPLICATION_CIRCLE)
                {
                    ReplicationViewDialog replicationViewDialog = new ReplicationViewDialog();
                    replicationViewDialog.otherConfig = other_config;
                    replicationViewDialog.VMName = vm.name_label;
                    replicationViewDialog.InitReplicationViewData();
                    replicationViewDialog.ShowDialog(Program.MainWindow);
                }
            }
        }

        private void toolStripButton_EditJob_Click(object sender, EventArgs e)
        {
            string type = this.dataGridView_CurrentJobs.CurrentRow.Cells[1].Tag.ToString();
            VM vm = this.dataGridView_CurrentJobs.CurrentRow.Cells[5].Tag as VM;
            if (vm != null)
            {
                if (type == BackupRestoreConfig.BACKUP_ONCE ||
                   type == BackupRestoreConfig.BACKUP_DAILY ||
                   type == BackupRestoreConfig.BACKUP_WEEKLY ||
                   type == BackupRestoreConfig.BACKUP_CIRCLE ||
                   type == BackupRestoreConfig.FULL_BACKUP_ONCE ||
                   type == BackupRestoreConfig.FULL_BACKUP)
                {
                    BackupWizard wizard = new BackupWizard(vm);
                    wizard.BackupEditWizard();
                    wizard.ShowDialog();
                } else if (type == BackupRestoreConfig.REPLICATION_ONCE ||
                        type == BackupRestoreConfig.REPLICATION_DAILY ||
                        type == BackupRestoreConfig.REPLICATION_WEEKLY ||
                        type == BackupRestoreConfig.REPLICATION_CIRCLE)
                {
                    ReplicationWizard ReplicationWizard = new ReplicationWizard(vm);
                    ReplicationWizard.ReplicationEditWizard();
                    ReplicationWizard.ShowDialog();
                }
            }
            //this.BuildList();
        }
	}
}
