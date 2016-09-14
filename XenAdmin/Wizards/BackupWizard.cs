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

using HalsignLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XenAPI;
using XenAdmin.Wizards.BackupWizard_Pages;
using XenAdmin.Controls;
using HalsignModel;
using XenAdmin.Dialogs;
using XenAdmin.Actions.BRActions;
using System.Text.RegularExpressions;
using System.Globalization;
using XenAdmin.Wizards.GenericPages;
using XenAdmin.Core;

namespace XenAdmin.Wizards
{
	public class BackupWizard : XenWizardBase
	{
        private IContainer components = null;
        private IXenObject _xenModelObject;

        private readonly BackupChoseVmPage xenChoseVmBackupPage;
        private readonly BackupSchedulePage scheduleBackupPage;
        private readonly BackupSummaryPage summaryBackupPage;
        private readonly BackupOptionsPage optionsBackupPage;
        private readonly BackupCompletePage completeBackupPage;
        private readonly RBACWarningPage page_RbacWarning;

        private List<VM> vmCheckedList = new List<VM>();
        private Dictionary<string, string> vdi_dictionary = new Dictionary<string, string>();
        public static RbacMethodList StaticRBACDependencies = new RbacMethodList(
            // provision VM
            "vm.set_other_config",
            // backup and restore need to call host.call_plugin
            "host.call_plugin"
        );

        public BackupWizard(IXenObject XenObject)
        {
            this._xenModelObject = XenObject;
            InitializeComponent();
            base.Text = string.Format(Messages.BACKUP_VM_TITLE, HalsignHelpers.GetName(this._xenModelObject.Connection));
            this.xenChoseVmBackupPage = new BackupChoseVmPage(XenObject);
            this.scheduleBackupPage = new BackupSchedulePage();
            this.summaryBackupPage = new BackupSummaryPage();
            this.optionsBackupPage = new BackupOptionsPage();
            this.completeBackupPage = new BackupCompletePage();
            this.page_RbacWarning = new RBACWarningPage();
            #region RBAC Warning Page Checks
            if (this._xenModelObject.Connection.Session.IsLocalSuperuser || Helpers.GetMaster(this._xenModelObject.Connection).external_auth_type == Auth.AUTH_TYPE_NONE)
            {
                //page_RbacWarning.DisableStep = true;
            }
            else
            {
                // Check to see if they can even create a VM
                RBACWarningPage.WizardPermissionCheck createCheck = new RBACWarningPage.WizardPermissionCheck(Messages.RBAC_WARNING_VM_WIZARD_BACKUP);
                foreach (RbacMethod method in StaticRBACDependencies)
                    createCheck.AddApiCheck(method);
                createCheck.Blocking = true;

                page_RbacWarning.AddPermissionChecks(this._xenModelObject.Connection, createCheck);

                AddPage(page_RbacWarning, 0);
            }
            #endregion
            base.AddPage(this.xenChoseVmBackupPage);
            base.AddPage(this.scheduleBackupPage);
            base.AddPage(this.optionsBackupPage);
            base.AddPage(this.summaryBackupPage);
            base.AddPage(this.completeBackupPage);

            
        }
        public void BackupEditWizard()
        {
            VM vm = this._xenModelObject as VM;
            this.vmCheckedList.Add(vm);
            BackupRestoreConfig.BrSchedule schedule = (BackupRestoreConfig.BrSchedule)HalsignUtil.JsonToObject(vm.other_config["halsign_br_rules"], typeof(BackupRestoreConfig.BrSchedule));
            BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(vm.other_config["halsign_br_job_s"], typeof(BackupRestoreConfig.Job));
            base.RemovePage(this.xenChoseVmBackupPage);
            base.RemovePage(this.optionsBackupPage);
            base.RemovePage(this.summaryBackupPage);
            base.RemovePage(this.completeBackupPage);

            this.optionsBackupPage._JobNameTextBox=schedule.jobName;
            string tmpvdistr="";
            if (schedule.details.Equals("halsign_vdi_all"))
            {
                foreach (VBD vbd in vm.Connection.ResolveAll<VBD>(vm.VBDs))
                {
                    if (HalsignHelpers.IsCDROM(vbd))
                    {
                        continue;
                    }
                    if (vbd.type.Equals(XenAPI.vbd_type.Disk))
                    {
                        tmpvdistr += vm.Connection.Resolve<VDI>(vbd.VDI).uuid + "@"; ;
                    }
                }
                tmpvdistr = tmpvdistr.Substring(0, tmpvdistr.Length - 1);
                this.vdi_dictionary.Add(vm.uuid, tmpvdistr);
            }
            else
            {
                this.vdi_dictionary.Add(vm.uuid, schedule.details);
            }
            this.scheduleBackupPage.EditBackupInitButton(schedule);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BackupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(809, 544);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "BackupWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BackupWizard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override bool RunNextPagePrecheck(XenTabPage senderPage)
        {
            if (senderPage != null && senderPage == this.xenChoseVmBackupPage)
            {
                int checkedCount = this.xenChoseVmBackupPage.CheckTreeView();
                if (!this.xenChoseVmBackupPage.CheckConfiguration)
                {
                    this.xenChoseVmBackupPage.ShowConfigurationDialog();
                    return false;
                }
                else if (checkedCount > 1)
                {
                    if (!this.xenChoseVmBackupPage.CheckSelectedVM())
                    {
                        MessageBox.Show(Messages.BACKUP_CHECKED_MESSAGE);
                    }
                    this.optionsBackupPage.JobNameTextBox("Backup");
                }
                else if (checkedCount < 1)
                {
                    MessageBox.Show(Messages.BACKUP_CHECKED_MESSAGE);
                    return false;
                }
                else if (checkedCount == 1)
                {
                    this.optionsBackupPage.JobNameTextBox("Backup " + this.xenChoseVmBackupPage.JobName);
                }
                List<VM> removedList = this.xenChoseVmBackupPage.VMCheckedList.Where(vm => vm.SRs.Any(sr => sr.IsRemovableStorage() == true)).ToList<VM>();
                if (removedList.Count > 0)
                {
                    MessageBox.Show(string.Format(Messages.DR_WITH_HOT_PLUG, this.ParseVMList(removedList)));
                    return false;
                }
                this.summaryBackupPage.DataSize = Util.DiskSizeString(this.xenChoseVmBackupPage.StorageCount);
                this.vmCheckedList = this.xenChoseVmBackupPage.VMCheckedList;
                this.vdi_dictionary = this.xenChoseVmBackupPage.VDI_Dictionary;
            }

            if (senderPage != null && senderPage == this.optionsBackupPage)
            {
                if (!Regex.IsMatch(this.optionsBackupPage._JobNameTextBox, @"^[\w\-\s\(\)]*$"))
                {
                    MessageBox.Show(Messages.BR_JOB_NAME_CHECK);
                    return false;
                }
            }
            
            return base.RunNextPagePrecheck(senderPage);
        }

        private string ParseVMList(List<VM> vmList)
        {
            string parseInfo = null;
            for (int i = 1; i < vmList.Count - 1; i++ )
            {
                parseInfo += "|" + vmList[i].name_label;
            }
            return vmList[0].name_label + parseInfo;
        }

        protected override void UpdateWizardContent(XenTabPage senderPage)
        {

            System.Type type = senderPage.GetType();
            if (type == typeof(BackupChoseVmPage))
            {
                Host _host = null;
                if (this._xenModelObject is VM)
                {
                    _host = HalsignHelpers.VMHome(this._xenModelObject as VM);
                }
                else if (this._xenModelObject is Host)
                {
                    _host = this._xenModelObject as Host;
                }
                if (_host != null)
                {
                    this.scheduleBackupPage.ServerDateText = Host.get_servertime(this._xenModelObject.Connection.Session, _host.opaque_ref).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
                }
                else
                {
                    this.scheduleBackupPage.ServerDateText = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
                }
            }
            else if(type == typeof(BackupOptionsPage))
            {
                this.summaryBackupPage._JobNameLable(this.optionsBackupPage._JobNameTextBox);
                if (String.IsNullOrEmpty(this.scheduleBackupPage._StrScheduleType))
                {
                    if (this.scheduleBackupPage.IsFullBackup)
                    {
                        this.summaryBackupPage._ScheduleTypeLabel(Messages.FULL_BACKUP_NOW);
                    }
                    else
                    {
                        this.summaryBackupPage._ScheduleTypeLabel(Messages.BACKUP_IMMEDIATELY);
                    }
                }
                else
                {
                    if (this.scheduleBackupPage.IsFullBackup)
                    {
                        this.summaryBackupPage._ScheduleTypeLabel(Messages.FULL_BACKUP_ONCE);
                    }
                    else if (this.scheduleBackupPage._StrScheduleType.Equals("Once"))
                    {
                        this.summaryBackupPage._ScheduleTypeLabel(Messages.BACKUP_ONCE);
                    }
                    else
                    {
                        if (this.scheduleBackupPage._StrScheduleType.Equals("Daily"))
                        {
                            this.summaryBackupPage._ScheduleTypeLabel(Messages.BACKUP_DAILY);
                        }
                        else if (this.scheduleBackupPage._StrScheduleType.Equals("Weekly"))
                        {
                            this.summaryBackupPage._ScheduleTypeLabel(Messages.BACKUP_WEEKLY);
                        }
                        else
                        {
                            this.summaryBackupPage._ScheduleTypeLabel(Messages.BACKUP_CIRCLE);
                        }

                        if (this.scheduleBackupPage._expectFullCheckBox)
                        {
                            this.summaryBackupPage.Options = string.Format(Messages.FULL_BACKUP_RECURS_EVERY, this.scheduleBackupPage._expectFullCountTextBox);
                        }
                    }
                    //this.summaryBackupPage._ScheduleTypeLabel(this.scheduleBackupPage._StrScheduleType);
                }
            }
        }

        protected override void FinishWizard()
        {
            this.SummeryInfoCheck();
        }

        private void SummeryInfoCheck()
        {
            String str_rules = "";
            String str_job = "";
            String c = "";
            BackupRestoreConfig.BrSchedule schedule = new BackupRestoreConfig.BrSchedule();
            schedule.current_full_count = 0;
            if (this.scheduleBackupPage.NowRadioButtonIsChecked())
            {
                schedule.jobName = this.optionsBackupPage._JobNameTextBox;
                schedule.scheduleType = 0;
                schedule.scheduleDate = "";
                str_rules = HalsignUtil.ToJson(schedule);
            }
            if (this.scheduleBackupPage.OnceRadioButtonIsChecked() && !this.scheduleBackupPage.IsFullBackup)
            {
                schedule.jobName = this.optionsBackupPage._JobNameTextBox;
                schedule.scheduleType = 1;
                schedule.scheduleDate = this.scheduleBackupPage.StartDatePickerText;
                schedule.scheduleTime = this.scheduleBackupPage.StartTimePickerText;
                str_rules = HalsignUtil.ToJson(schedule);
                c = "s";
            }

            if (this.scheduleBackupPage.OnceRadioButtonIsChecked() && this.scheduleBackupPage.IsFullBackup)
            {
                schedule.jobName = this.optionsBackupPage._JobNameTextBox;
                schedule.scheduleType = 5;
                schedule.scheduleDate = this.scheduleBackupPage.StartDatePickerText;
                schedule.scheduleTime = this.scheduleBackupPage.StartTimePickerText;
                str_rules = HalsignUtil.ToJson(schedule);
                c = "q";
            }

            if (this.scheduleBackupPage.DailyRadioButtonIsChecked())
            {
                schedule.jobName = this.optionsBackupPage._JobNameTextBox;
                schedule.scheduleType = 2;
                schedule.scheduleDate = this.scheduleBackupPage.StartDatePickerText;
                schedule.scheduleTime = this.scheduleBackupPage.StartTimePickerText;
                schedule.recur = this.scheduleBackupPage._RecurTextBox == "" ? 0 : Int32.Parse(this.scheduleBackupPage._RecurTextBox);
                schedule.expect_full_count = this.scheduleBackupPage._expectFullCheckBox?this.scheduleBackupPage._expectFullCountTextBox:0;
                str_rules = HalsignUtil.ToJson(schedule);
                c = "t";
            }
            if (this.scheduleBackupPage.CircleRadioButtonIsChecked())
            {
                schedule.jobName = this.optionsBackupPage._JobNameTextBox;
                schedule.scheduleType = 4;
                schedule.scheduleDate = this.scheduleBackupPage.StartDatePickerText;
                schedule.scheduleTime = this.scheduleBackupPage.StartTimePickerText;
                schedule.recur = this.scheduleBackupPage._RecurTextBox == "" ? 1 : Int32.Parse(this.scheduleBackupPage._RecurTextBox);
                schedule.expect_full_count = this.scheduleBackupPage._expectFullCheckBox ? this.scheduleBackupPage._expectFullCountTextBox : 0;
                str_rules = HalsignUtil.ToJson(schedule);
                c = "h";
            }
            if (this.scheduleBackupPage.WeeklyRadioButtonIsChecked())
            {
                schedule.jobName = this.optionsBackupPage._JobNameTextBox;
                schedule.scheduleType = 3;
                schedule.scheduleDate = this.scheduleBackupPage.StartDatePickerText;
                schedule.scheduleTime = this.scheduleBackupPage.StartTimePickerText;
                c = "w";
                schedule.recur = this.scheduleBackupPage._RecurTextBox == "" ? 0 : Int32.Parse(this.scheduleBackupPage._RecurTextBox);
                schedule.expect_full_count = this.scheduleBackupPage._expectFullCheckBox ? this.scheduleBackupPage._expectFullCountTextBox : 0;
                List<int> list = new List<int>();
                if (this.scheduleBackupPage._MondayCheckBoxIsChecked())
                {
                    list.Add(1);
                }
                if (this.scheduleBackupPage._TuesdayCheckBoxIsChecked())
                {
                    list.Add(2);
                }
                if (this.scheduleBackupPage._WednesdayCheckBoxIsChecked())
                {
                    list.Add(3);
                }
                if (this.scheduleBackupPage._ThursdayCheckBoxIsChecked())
                {
                    list.Add(4);
                }
                if (this.scheduleBackupPage._FridayCheckBoxIsChecked())
                {
                    list.Add(5);
                }
                if (this.scheduleBackupPage._SaturdayCheckBoxIsChecked())
                {
                    list.Add(6);
                }
                if (this.scheduleBackupPage._SundayCheckBoxIsChecked())
                {
                    list.Add(0);
                }
                schedule.weeklyDays = list;
                str_rules = HalsignUtil.ToJson(schedule);
            }
            str_rules = str_rules.Replace("\\/", "/");

            if (!c.Equals(""))
            {
                BackupRestoreConfig.Job job = new BackupRestoreConfig.Job();
                job.job_name = this.optionsBackupPage._JobNameTextBox;
                job.host = "";
                job.key = "halsign_br_job_s";
                job.request = c + this.optionsBackupPage._JobNameTextBox.TrimEnd().TrimStart() + "|";
                TimeSpan ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
                ts = new DateTime(this.scheduleBackupPage.StartDatePickerValue.Year, this.scheduleBackupPage.StartDatePickerValue.Month,
                               this.scheduleBackupPage.StartDatePickerValue.Day, this.scheduleBackupPage.StartTimePickerValue.Hour, this.scheduleBackupPage.StartTimePickerValue.Minute,
                               this.scheduleBackupPage.StartTimePickerValue.Second).Subtract(new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime());
                job.start_time = "";
                job.progress = -1;
                job.total_storage = -1;
                job.modify_time = "";
                job.pid = -1;
                job.retry = -1;
                job.speed = -1;
                job.status = 0;
                job.current_full_count = 0;
                if (c.Equals("s"))
                {
                    job.schedule_type = 1;
                }else if(c.Equals("q"))
                {
                    job.schedule_type = 5;
                }
                else if (c.Equals("t"))
                {
                    job.schedule_type = 2;
                }
                else if (c.Equals("h"))
                {
                    job.schedule_type = 4;
                }
                else if (c.Equals("w"))
                {
                    job.schedule_type = 3;
                }
                job.expect_full_count = this.scheduleBackupPage.IsFullBackup ? 0 : (this.scheduleBackupPage._expectFullCheckBox ? this.scheduleBackupPage._expectFullCountTextBox : 0);
                str_job = HalsignUtil.ToJson(job);
            }
            Dictionary<string, string> _dconf = new Dictionary<string, string>();
            _dconf.Add("job_name", this.optionsBackupPage._JobNameTextBox.TrimEnd().TrimStart());
            //_dconf.Add("command", this.optionsBackupPage.IsFullBackup ? "b" : "i");
            _dconf.Add("command", this.scheduleBackupPage.IsFullBackup ? "b" : "i");
            _dconf.Add("is_now", this.scheduleBackupPage.NowRadioButtonIsChecked().ToString());
            _dconf.Add("backup_rules", str_rules);
            _dconf.Add("backup_job", str_job);

            BackupAction action = new BackupAction(Messages.BACKUP_VM, BackupRestoreConfig.BackupActionKind.Backup, this._xenModelObject, this.vmCheckedList, _dconf, this.vdi_dictionary);
            if (action != null)
            {
                ProgressBarStyle progressBarStyle = ProgressBarStyle.Marquee;
                ActionProgressDialog dialog = new ActionProgressDialog(action, progressBarStyle);
                dialog.ShowCancel = false;
                dialog.ShowDialog(this);
                if (!action.Succeeded || !string.IsNullOrEmpty(action.Result))
                {
                    base.FinishCanceled();
                }
                else
                {
                    base.FinishWizard();
                }
            }

            if (!(this._xenModelObject is Host))
            {
                // todo Program.MainWindow.SwitchToTab(MainWindow.Tab.BR);
                if (this._xenModelObject is VM)
                {
                    VM vm = this._xenModelObject as VM;
                    vm.NotifyPropertyChanged("other_config");
                }
            }
        }
	}
}
