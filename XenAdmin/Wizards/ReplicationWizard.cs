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
using System.Linq;
using System.Windows.Forms;
using XenAPI;
using XenAdmin.Core;
using HalsignModel;
using XenAdmin.Actions.BRActions;
using XenAdmin.Dialogs;
using XenAdmin.Wizards.GenericPages;
using XenAdmin.Wizards.ReplicationWizard_Pages;
using XenAdmin.Actions;
using XenAdmin.Controls;
using HalsignLib;
using System.Text.RegularExpressions;

namespace XenAdmin.Wizards
{
    public class ReplicationWizard : XenWizardBase
    {
        private IContainer components = null;
        private IXenObject _xenModelObject;
        public AsyncAction FinalAction;
        private readonly string m_text;
        private readonly ReplicationChoseVmPage repChoseVmPage;
        private readonly ReplicationJobSettingPage repJobSettingPage;
        private readonly ReplicationSynchronizationPage synchronizationPage;
        private readonly ReplicationSchedulePage schedulePage;
        private readonly ReplicationVMSettingsPage vmSettingsPage;
        private readonly ReplicationCompletePage completePage;
        private readonly RBACWarningPage page_RbacWarning;
        private string  scheduleDetails;
        private BackupRestoreConfig.Job jobs;
        public static RbacMethodList StaticRBACDependencies = new RbacMethodList(
            // provision VM
            "vm.set_other_config",
            "host.call_plugin"
        );
        internal ReplicationWizard(IXenObject XenObject)
        {
            this.InitializeComponent();
            this._xenModelObject = XenObject;
            this.repChoseVmPage = new ReplicationChoseVmPage(XenObject);
            this.repJobSettingPage = new ReplicationJobSettingPage();
            this.synchronizationPage = new ReplicationSynchronizationPage();
            this.schedulePage = new ReplicationSchedulePage(XenObject);
            this.vmSettingsPage = new ReplicationVMSettingsPage();
            this.completePage = new ReplicationCompletePage();
            this.page_RbacWarning = new RBACWarningPage();
            #region RBAC Warning Page Checks
            if (this._xenModelObject.Connection.Session.IsLocalSuperuser || Helpers.GetMaster(this._xenModelObject.Connection).external_auth_type == Auth.AUTH_TYPE_NONE)
            {
                //page_RbacWarning.DisableStep = true;
            }
            else
            {
                // Check to see if they can even create a VM
                RBACWarningPage.WizardPermissionCheck createCheck = new RBACWarningPage.WizardPermissionCheck(Messages.RBAC_WARNING_VM_WIZARD_REPLICATION);
                foreach (RbacMethod method in StaticRBACDependencies)
                    createCheck.AddApiCheck(method);
                createCheck.Blocking = true;

                page_RbacWarning.AddPermissionChecks(this._xenModelObject.Connection, createCheck);

                AddPage(page_RbacWarning, 0);
            }
            #endregion
            base.AddPage(this.repChoseVmPage);
            base.AddPage(this.repJobSettingPage);
            base.AddPage(this.synchronizationPage);
            base.AddPage(this.schedulePage);
            base.AddPage(this.vmSettingsPage);
            base.AddPage(this.completePage);
            this.m_text = string.Format(Messages.REPLICATION_VM_TITLE, Helpers.GetName(XenObject.Connection));
            
        }

        public void ReplicationEditWizard()
        {
            VM vm = this._xenModelObject as VM;
            this.repChoseVmPage.VmCheckedList.Add(vm);
            BackupRestoreConfig.BrSchedule schedule = (BackupRestoreConfig.BrSchedule)HalsignUtil.JsonToObject(vm.other_config["halsign_rep_rules"], typeof(BackupRestoreConfig.BrSchedule));
            jobs = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(vm.other_config["halsign_br_job_r"], typeof(BackupRestoreConfig.Job));
            base.RemovePage(this.repChoseVmPage);
            base.RemovePage(this.repJobSettingPage);
            base.RemovePage(this.synchronizationPage);
            base.RemovePage(this.vmSettingsPage);
            base.RemovePage(this.completePage);

            this.repJobSettingPage.JobNameText = schedule.jobName;
            string tmpvdistr = "";
            if (schedule.details.EndsWith("halsign_vdi_all"))
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
                this.repChoseVmPage.vdi_expand_list.Add(vm.uuid, tmpvdistr);
            }
            else
            {
                this.repChoseVmPage.vdi_expand_list.Add(vm.uuid, schedule.details);
            }
            this.schedulePage.EditReplicationInitButton(schedule);
            this.repJobSettingPage.EditReplicationInitJobSet(schedule);
            scheduleDetails = schedule.details.Replace("|halsign_vdi_all","");
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
            // ReplicationWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(809, 514);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ReplicationWizard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Text = this.m_text;
        }

        protected override bool RunNextPagePrecheck(XenTabPage senderPage)
        {
            if (senderPage != null && senderPage == this.repChoseVmPage)
            {
                if (this.repChoseVmPage.CheckTreeView() < 1)
                {
                    MessageBox.Show(Messages.REPLICATION_CHECKED_MESSAGE);
                    return false;
                }
                List<VM> removedList = this.repChoseVmPage.VmCheckedList.Where(vm => vm.SRs.Any(sr => sr.IsRemovableStorage() == true)).ToList<VM>();
                if (removedList.Count > 0)
                {
                    MessageBox.Show(string.Format(Messages.DR_WITH_HOT_PLUG, this.ParseVMList(removedList)));
                    return false;
                }
                bool allvmhasvdi = this.repChoseVmPage.VmCheckedList.All(vm => this.repChoseVmPage.vdi_expand_list.ContainsKey(vm.uuid));
                if (!allvmhasvdi)
                {
                    MessageBox.Show(Messages.BR_VM_WITHOUT_VDI);
                    return false;
                }
            }
            if (senderPage != null && senderPage == this.repJobSettingPage)
            {
                if (!this.repJobSettingPage.CheckDestUser())
                {
                    MessageBox.Show(Messages.ADD_NEW_INCORRECT);
                    return false;
                }
                if (!Regex.IsMatch(this.repJobSettingPage.JobNameText, @"^[\w\-\s\(\)]*$"))
                {
                    MessageBox.Show(Messages.BR_JOB_NAME_CHECK);
                    return false;
                }
                this.synchronizationPage.SettingValue(this.repChoseVmPage.VmCheckedList, this.repChoseVmPage.vdi_expand_list, this.repJobSettingPage.Selected_xenConnection, this.repJobSettingPage.Selected_host, this.repJobSettingPage.Choice_sr_uuid);
            }
            if (senderPage != null && senderPage == this.synchronizationPage)
            {
                if (!this.synchronizationPage.CheckCloseFromNext())
                {
                    MessageBox.Show(Messages.SYNCHRONIZATION_CHECKED_MESSAGE);
                    return false;
                }
                if(this.synchronizationPage.Is_VM_Running && this.synchronizationPage.IsMustNow)
                {
                    MessageBox.Show(Messages.REPLICATED_VM_RUNNING_INFO, Messages.XENCENTER, MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return false;
                }
                this.schedulePage.SetPageLoad(this.synchronizationPage.IsMustNow);
            }
            if (senderPage != null && senderPage == this.schedulePage)
            {
                this.vmSettingsPage.SettingValue(this.repChoseVmPage.VmCheckedList, this.repChoseVmPage.VdicheckDictionary, this.repJobSettingPage.VMNameText, this.repJobSettingPage.NetworkText, this.repJobSettingPage.Choice_sr_ip_name, this.repJobSettingPage.Choice_sr_free_space);
            }

            return base.RunNextPagePrecheck(senderPage);
        }

        private string ParseVMList(List<VM> vmList)
        {
            string parseInfo = null;
            for (int i = 1; i < vmList.Count - 1; i++)
            {
                parseInfo += "|" + vmList[i].name_label;
            }
            return vmList[0].name_label + parseInfo;
        }

        protected override void UpdateWizardContent(XenTabPage senderPage)
        {
        }

        protected override void FinishWizard()
        {
            List<Dictionary<string, string>> listParam = new List<Dictionary<string, string>>();
            List<Dictionary<string, List<string>>> listParamTemp = new List<Dictionary<string, List<string>>>();
            BackupRestoreConfig.BrSchedule schedule = new BackupRestoreConfig.BrSchedule();
            BackupRestoreConfig.Job job = new BackupRestoreConfig.Job();
            string str_rules = "";
            String str_job = "";
            string str_command = "";

            schedule.jobName = this.repJobSettingPage.JobNameText;
            job.job_name = this.repJobSettingPage.JobNameText;
            if (this.schedulePage.NowChecked)
            {
                str_command = "f";
                schedule.scheduleType = 0;
                job.schedule_type = 0;
                schedule.scheduleDate = "";
            }
            if (this.schedulePage.OnceChecked)
            {
                str_command = "y";
                schedule.scheduleType = 1;
                job.schedule_type = 1;
                schedule.scheduleDate = this.schedulePage.StartDateText;
                schedule.scheduleTime = this.schedulePage.StartTimeText;
            }
            if (this.schedulePage.DailyChecked)
            {
                str_command = "a";
                schedule.scheduleType = 2;
                job.schedule_type = 2;
                schedule.scheduleDate = this.schedulePage.StartDateText;
                schedule.scheduleTime = this.schedulePage.StartTimeText;
                schedule.recur = this.schedulePage.RecurText;
            }
            if (this.schedulePage.CircleChecked)
            {
                str_command = "u";
                schedule.scheduleType = 4;
                job.schedule_type = 4;
                schedule.scheduleDate = this.schedulePage.StartDateText;
                schedule.scheduleTime = this.schedulePage.StartTimeText;
                schedule.recur = this.schedulePage.RecurText;
            }
            if (this.schedulePage.WeeklyChecked)
            {
                str_command = "z";
                schedule.scheduleType = 3;
                job.schedule_type = 3;
                schedule.scheduleDate = this.schedulePage.StartDateText;
                schedule.scheduleTime = this.schedulePage.StartTimeText;
                schedule.recur = this.schedulePage.RecurText;
                List<int> listDays = new List<int>();
                if (this.schedulePage.MondayChecked)
                {
                    listDays.Add(1);
                }
                if (this.schedulePage.TuesdayChecked)
                {
                    listDays.Add(2);
                }
                if (this.schedulePage.WednesdayChecked)
                {
                    listDays.Add(3);
                }
                if (this.schedulePage.ThursdayChecked)
                {
                    listDays.Add(4);
                }
                if (this.schedulePage.FridayChecked)
                {
                    listDays.Add(5);
                }
                if (this.schedulePage.SaturdayChecked)
                {
                    listDays.Add(6);
                }
                if (this.schedulePage.SundayChecked)
                {
                    listDays.Add(0);
                }
                schedule.weeklyDays = listDays;
            }
            if (scheduleDetails != null && !scheduleDetails.Equals(""))
            {
                schedule.details = scheduleDetails;
            }
            else
            {
                schedule.details = this.repJobSettingPage.Choice_sr_ip + "|"
                                    + this.repJobSettingPage.DestUsernameText + "|" + this.repJobSettingPage.DestPasswordText + "|"
                                    + this.repJobSettingPage.VMNameText + "|" + this.repJobSettingPage.Choice_sr_uuid + "|0|"
                                    + this.repJobSettingPage.MacText + "|" + this.repJobSettingPage.NetworkValue + "|"
                                    + Helpers.GetMaster(this.repJobSettingPage.Selected_xenConnection).address + "|" + this.repJobSettingPage.Selected_xenConnection.Username + "|"
                                    + this.repJobSettingPage.Selected_xenConnection.Password + "|" + this.repJobSettingPage.Selected_host.uuid;
            }
            str_rules = HalsignUtil.ToJson(schedule);
            str_rules = str_rules.Replace("\\/", "/");


            job.host = "";
            job.key = "halsign_br_job_r";
            job.request = str_command + this.repJobSettingPage.JobNameText.TrimEnd().TrimStart() + "|";
            TimeSpan ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            ts = new DateTime(this.schedulePage.StartDateValue.Year, this.schedulePage.StartDateValue.Month,
                           this.schedulePage.StartDateValue.Day, this.schedulePage.StartTimeValue.Hour, this.schedulePage.StartTimeValue.Minute,
                           this.schedulePage.StartTimeValue.Second).Subtract(new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime());
            if (jobs != null)
            {
                jobs.request = str_command + jobs.request.Substring(1, jobs.request.Length-1);
                jobs.schedule_type = job.schedule_type;
                str_job = HalsignUtil.ToJson(jobs);
            }
            else
            {
                job.start_time = "";
                job.progress = -1;
                job.total_storage = -1;
                job.modify_time = "";
                job.pid = -1;
                job.retry = -1;
                job.speed = -1;
                job.status = 0;
                job.current_full_count = 0;
                job.expect_full_count = 0;//this.scheduleBackupPage.IsFullBackup ? 0 : (this.scheduleBackupPage._expectFullCheckBox ? this.scheduleBackupPage._expectFullCountTextBox : 0);
                str_job = HalsignUtil.ToJson(job);
            }


            //string a = this.repJobSettingPage.Selected_xenConnection.Hostname;

            Dictionary<string, string> _dconf = new Dictionary<string, string>();
            _dconf.Add("command", str_command);
            _dconf.Add("is_now", this.schedulePage.NowChecked.ToString());
            _dconf.Add("replication_rules", str_rules);
            _dconf.Add("replication_job", str_job);
            if (jobs != null)
            {
                _dconf.Add("replication_editjob", "true");
            }
            else
            {
                _dconf.Add("replication_editjob", "false");
            }

            foreach (VM item in this.repChoseVmPage.VmCheckedList)
            {
                Dictionary<string, string> _dconf_param = new Dictionary<string, string>();
                Dictionary<string, List<string>> _dconf_param_temp = new Dictionary<string, List<string>>();
                string host_ip = " ";
                if (!HalsignHelpers.VMHome(item).address.Equals(this.repJobSettingPage.Choice_sr_ip))
                {
                    host_ip = this.repJobSettingPage.Choice_sr_ip;
                }
                List<string> syncCheckedList = new List<string>();
                foreach (Dictionary<string, string> dRepChecked in this.synchronizationPage.RepCheckedList)
                {
                    if (dRepChecked.ContainsKey(item.uuid))
                    {
                        string request_expend = "";
                        if (this.repChoseVmPage.vdi_expand_list.ContainsKey(item.uuid))
                        {
                            int vdiNo = 0;
                            foreach (VBD vbd in item.Connection.ResolveAll<VBD>(item.VBDs))
                            {
                                if (HalsignHelpers.IsCDROM(vbd))
                                {
                                    continue;
                                }

                                if (vbd.type.Equals(XenAPI.vbd_type.Disk))
                                {
                                    vdiNo++;
                                }
                            }
                            if (vdiNo == this.repChoseVmPage.vdi_expand_list[item.uuid].Split('@').Length)
                            {
                                request_expend = "|" + "halsign_vdi_all";
                            }
                            else
                            {
                                request_expend = "|" + this.repChoseVmPage.vdi_expand_list[item.uuid];
                            }
                        }
                        if ("new".Equals(dRepChecked[item.uuid]))
                        {
                            syncCheckedList.Add(this.repJobSettingPage.JobNameText + "|" + item.uuid + "|" + host_ip + "|"
                                    + this.repJobSettingPage.DestUsernameText + "|" + this.repJobSettingPage.DestPasswordText + "|"
                                    + this.repJobSettingPage.VMNameText + "|" + this.repJobSettingPage.Choice_sr_uuid + "|0|"
                                + this.repJobSettingPage.MacText + "|" + this.repJobSettingPage.NetworkValue + "|"
                                + Helpers.GetMaster(this.repJobSettingPage.Selected_xenConnection).address + "|" + this.repJobSettingPage.Selected_xenConnection.Username + "|"
                                + this.repJobSettingPage.Selected_xenConnection.Password + "|" + this.repJobSettingPage.Selected_host.uuid + request_expend);
                        }
                        else
                        {
                            syncCheckedList.Add(this.repJobSettingPage.JobNameText + "|" + item.uuid + "|" + host_ip + "|"
                                    + this.repJobSettingPage.DestUsernameText + "|" + this.repJobSettingPage.DestPasswordText + "|"
                                    + dRepChecked[item.uuid] + "|" + Helpers.GetMaster(this.repJobSettingPage.Selected_xenConnection).address + "|"
                                    + this.repJobSettingPage.Selected_xenConnection.Username + "|" + this.repJobSettingPage.Selected_xenConnection.Password + request_expend);
                        }
                    }
                }
                _dconf_param_temp.Add(item.uuid, syncCheckedList);
                listParamTemp.Add(_dconf_param_temp);
            }

            BackupAction action = new BackupAction(Messages.COPY_VM, BackupRestoreConfig.BackupActionKind.Replication, this._xenModelObject, this.repChoseVmPage.VmCheckedList, _dconf, listParamTemp, this.repChoseVmPage.vdi_expand_list);
            if (action != null)
            {
                ProgressBarStyle progressBarStyle = ProgressBarStyle.Marquee;
                ActionProgressDialog dialog = new ActionProgressDialog(action, progressBarStyle);
                dialog.ShowCancel = false;
                dialog.ShowDialog(this);
                if (!action.Succeeded || !string.IsNullOrEmpty(action.Result))
                {
                    base.FinishCanceled();
                    //Program.MainWindow.BringToFront();
                    //e.PageIndex = e.PageIndex - 1;
                }
                else
                {
                    base.FinishWizard();
                }

                if (!(this._xenModelObject is Host))
                {
                    // todo Program.MainWindow.SwitchToTab(MainWindow.Tab.BR);
                    if (this._xenModelObject is VM)
                    {
                        VM vm=this._xenModelObject as VM;
                        vm.NotifyPropertyChanged("other_config");
                    }
                }
            }
        }

        protected override string WizardPaneHelpID()
        {
            if (base.CurrentStepTabPage is RBACWarningPage)
            {
                return base.FormatHelpId("Rbac");
            }
            return base.WizardPaneHelpID();
        }
    }
}

