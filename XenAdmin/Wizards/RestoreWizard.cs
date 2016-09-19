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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalsignLib;
using XenAPI;
using XenAdmin.Core;
using HalsignModel;
using XenAdmin.Controls;
using XenAdmin.Actions.BRActions;
using XenAdmin.Dialogs;
using XenAdmin.Wizards.RestoreWizard_Pages;
using System.Text.RegularExpressions;
using XenAdmin.Wizards.GenericPages;

namespace XenAdmin.Wizards
{
    public partial class RestoreWizard : XenWizardBase
    {
        #region Private Members

        private readonly Page_SetStorage page_SetStorage;
        private readonly Page_SelectVM page_SelectVM;
        private readonly Page_JobSettings page_JobSettings;
        private readonly Page_JobSchedule page_JobSchedule;
        private readonly Page_VMSettings page_VMSettings;
        private readonly Page_Complete page_Complete;
        private readonly RBACWarningPage page_RbacWarning;

        private IXenObject _xenModelObject;
        private Dictionary<string, string> backup_info_list;
        private List<AgentParamDataModel> vmCheckedList;
        private Dictionary<string, List<BackupRestoreConfig.RestoreInfo>> restore_vdi_info_list;
        private RestoreDataModel restoreDataModel;
        private List<BackupRestoreConfig.ResultInfo> restore_info_list;
        private BackupRestoreConfig.RestoreListInfo restoreListInfo;
        private string set_storage_ip;
        private string set_storage_username;
        private string set_storage_password;
        private string restore_job_name;
        private string restore_vm_name;
        private string restore_network_name;
        private string restore_network_uuid;
        private bool _isOnceScheduleChecked;
        private bool _isBackupNetworkSettingChecked;
        private bool _isNewMacAddressChecked;
        private DateTime scheduleDate;
        private DateTime scheduleTime;

        #endregion

        //public RestoreWizard()
        //{
        //    InitializeComponent();
        //}

        private Dictionary<string, string> restore_vdi_list;
        private string _Message;
        public static RbacMethodList StaticRBACDependencies = new RbacMethodList(
            // provision VM
            "vm.set_other_config"
        );
        public RestoreWizard(IXenObject xenModelObject)
        {
            this.InitializeComponent();
            this._Message =
                base.Text = string.Format(Messages.RESTORE_VM_TITLE, Helpers.GetName(xenModelObject.Connection));
            this.page_SetStorage = new Page_SetStorage(xenModelObject);
            this.page_SelectVM = new Page_SelectVM();
            this.page_JobSettings = new Page_JobSettings(xenModelObject);
            this.page_JobSchedule = new Page_JobSchedule();
            this.page_VMSettings = new Page_VMSettings();
            this.page_Complete = new Page_Complete();
            this.page_RbacWarning = new RBACWarningPage();
            this._xenModelObject = xenModelObject;
            this.backup_info_list = new Dictionary<string, string>();
            this.vmCheckedList = new List<AgentParamDataModel>();
            this.restoreDataModel = new RestoreDataModel();
            this.restore_info_list = new List<BackupRestoreConfig.ResultInfo>();
            this.restoreListInfo = new BackupRestoreConfig.RestoreListInfo();
            this.restore_vdi_list = new Dictionary<string, string>();
            #region RBAC Warning Page Checks
            if (this._xenModelObject.Connection.Session.IsLocalSuperuser || Helpers.GetMaster(this._xenModelObject.Connection).external_auth_type == Auth.AUTH_TYPE_NONE)
            {
                //page_RbacWarning.DisableStep = true;
            }
            else
            {
                // Check to see if they can even create a VM
                RBACWarningPage.WizardPermissionCheck createCheck = new RBACWarningPage.WizardPermissionCheck(Messages.RBAC_WARNING_VM_WIZARD_RESTORE);
                foreach (RbacMethod method in StaticRBACDependencies)
                    createCheck.AddApiCheck(method);
                createCheck.Blocking = true;

                page_RbacWarning.AddPermissionChecks(this._xenModelObject.Connection, createCheck);

                AddPage(page_RbacWarning, 0);
            }
            #endregion
            base.AddPages(new XenTabPage[] { this.page_SetStorage, this.page_SelectVM, this.page_JobSettings, this.page_JobSchedule, this.page_VMSettings, this.page_Complete });
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RestoreWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(809, 514);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "RestoreWizard";
            this.Text = "Restore VM";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override bool RunNextPagePrecheck(XenTabPage senderPage)
        {
            if (senderPage is Page_SetStorage)
            {
                if (!this.page_SetStorage.UserName.ToLower().Equals("root"))
                {
                    if (string.IsNullOrEmpty(this.page_SetStorage.CheckAuthValidate()))
                    {
                        if (this.InitRestoreTree(ref restoreListInfo))
                        {
                            this.set_storage_ip = this.page_SetStorage.StorageIP;
                            this.set_storage_username = this.page_SetStorage.UserName;
                            this.set_storage_password = this.page_SetStorage.Password;
                            this.page_SelectVM.RestoreList = this.restoreListInfo;
                            //this.page_SelectVM.BuildTree();
                            this.page_SelectVM.BuildRestoreTree();
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this.page_SetStorage.CheckAuthValidate());
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show(Messages.RESTORE_ROOT_PROMPT);
                    this.page_SetStorage.UserName = string.Empty;
                    this.page_SetStorage.Password = string.Empty;
                    return false;
                }
            }
            else if (senderPage is Page_SelectVM)
            {
                this.page_SelectVM.CheckTreeViewStatus();
                this.backup_info_list = this.page_SelectVM.Backup_info_list;
                this.vmCheckedList = this.page_SelectVM.vmCheckedList;
                this.page_JobSettings.PopulateData();
            }
            else if (senderPage is Page_JobSettings)
            {
                this.restoreDataModel = this.page_JobSettings.restoreDataModel;
                this.restore_job_name = this.page_JobSettings.job_Name;
                this.restore_vm_name = this.page_JobSettings.vm_Name;
                this.restore_network_name = this.page_JobSettings.network_Name;
                this.restore_network_uuid = this.page_JobSettings.network_UUID;
                this._isBackupNetworkSettingChecked = this.page_JobSettings.isBackupNetworkSettingChecked;
                this._isNewMacAddressChecked = this.page_JobSettings.isNewMacAddressChecked;
                this.page_JobSchedule.UpdateServerTime(this._xenModelObject);
                if (!Regex.IsMatch(this.page_JobSettings.job_Name, @"^[\w\-\s\(\)]*$"))
                {
                    MessageBox.Show(Messages.BR_JOB_NAME_CHECK);
                    return false;
                }
            }
            else if (senderPage is Page_JobSchedule)
            {
                this._isOnceScheduleChecked = this.page_JobSchedule.isOnceScheduleChecked;
                this.scheduleDate = this.page_JobSchedule.scheduleDate;
                this.scheduleTime = this.page_JobSchedule.scheduleTime;
                this.restore_vdi_info_list = this.page_SelectVM._restore_vdi_info_list;
                this.page_VMSettings.BuildRestoreInfoList(vmCheckedList, backup_info_list, restore_vm_name, restore_network_name, restoreDataModel, restore_vdi_info_list);
            }
            else if (senderPage is Page_VMSettings)
            {
                this.ScheduleRestoreJob();
            }

            return true;
        }

        /// <summary>
        /// Get machine backups list
        /// </summary>
        /// <returns></returns>
        private bool InitRestoreTree(ref BackupRestoreConfig.RestoreListInfo restoreList)
        {
            bool isActionSucceed = false;

            Dictionary<string, string> dconf = new Dictionary<string, string>
                                                   {
                                                       {"ip", this.page_SetStorage.StorageIP},
                                                       {"username", this.page_SetStorage.UserName},
                                                       {"password", this.page_SetStorage.Password}
                                                   };
            BackupAction action = new BackupAction(Messages.GET_RESTORE_LIST, BackupRestoreConfig.BackupActionKind.RestoreFileList, this._xenModelObject, dconf);

            const ProgressBarStyle progressBarStyle = ProgressBarStyle.Marquee;
            ActionProgressDialog dialog = new ActionProgressDialog(action, progressBarStyle);
            dialog.ShowCancel = true;
            dialog.ShowDialog(this);
            
            try
            {
                if (!(action.Succeeded || ((action.Exception == null) || (action.Exception.Message.Equals("")))))
                {
                    Program.MainWindow.BringToFront();
                }
                else
                {
                    restoreList = action.RestoreList;
                    isActionSucceed = true;
                }
            }
            catch
            {
                Program.MainWindow.BringToFront();
            }
            

            return isActionSucceed;
        }

        /// <summary>
        /// Update by Dalei.Shou on 2013/8/16, remove the last "|0" parameter
        /// </summary>
        private void ScheduleRestoreJob()
        {
            int rowCount = this.page_VMSettings.DataGridViewRestoreInfo.RowCount;
            for (int i = 0; i < this.page_VMSettings.DataGridViewRestoreInfo.RowCount; i++)
            {
                if (this.page_VMSettings.DataGridViewRestoreInfo.Rows[i].Cells[0].Value.ToString().ToLowerInvariant() == "true")
                {
                    string[] uuidTag = this.page_VMSettings.DataGridViewRestoreInfo.Rows[i].Tag.ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    if(!restore_vdi_list.ContainsKey(uuidTag[0]))
                    {
                        restore_vdi_list.Add(uuidTag[0], uuidTag[1]);
                    }
                    else
                    {
                        string temp = string.Concat(restore_vdi_list[uuidTag[0]], "@", uuidTag[1]);
                        restore_vdi_list[uuidTag[0]] = temp;
                    }
                }
            }
            List<string> list = new List<string>();
            List<Dictionary<string, string>> listSchedule = new List<Dictionary<string, string>>();
            string schedule = "";
            int milliseconds = 0;
            foreach (AgentParamDataModel item in this.vmCheckedList)
            {
                string uuid_spend = "|";
                if (restore_vdi_list.ContainsKey(item.VMRestoreInfo.Substring(37, 36)))
                {
                    int l = restore_vdi_list[item.VMRestoreInfo.Substring(37, 36)].Split('@').Length;
                    if (l == 1)
                    {
                        uuid_spend += "halsign_vdi_all";
                    }
                    else
                    {
                        if (l - 1 == restore_vdi_info_list[item.VMRestoreInfo].Count)
                        {
                            uuid_spend += "halsign_vdi_all";
                        }
                        else if (l - 1 < restore_vdi_info_list[item.VMRestoreInfo].Count)
                        {
                            for (int y = 1; y < l; y++)
                            {
                                if (y == 1)
                                {
                                    uuid_spend = uuid_spend + restore_vdi_list[item.VMRestoreInfo.Substring(37, 36)].Split('@')[y];
                                }
                                else
                                {
                                    uuid_spend = uuid_spend + "@" +
                                                 restore_vdi_list[item.VMRestoreInfo.Substring(37, 36)].Split('@')[y];
                                }
                            }
                        }
                    }
                }

                list.Add(this.restore_job_name.TrimEnd().TrimStart() + "|" + this.set_storage_ip.Trim() + "|"
                         + this.set_storage_username + "|" + this.set_storage_password + "|" + item.RootPath + "|" +
                         item.VMRestoreInfo + "|"
                         + this.restore_vm_name + "|" + this.restoreDataModel.choice_sr_uuid + "|"
                         + (this._isBackupNetworkSettingChecked ? "1" : "0") + "|" +
                         (this._isNewMacAddressChecked ? "1" : "0")
                         + "|" + this.restore_network_uuid + uuid_spend);

                if (this._isOnceScheduleChecked)
                {
                    Dictionary<string, string> dconfTemp = new Dictionary<string, string>();
                    BackupRestoreConfig.Job job = new BackupRestoreConfig.Job();
                    job.job_name = this.restore_job_name.TrimEnd().TrimStart();

                    if (this._xenModelObject is VM)
                    {
                        VM _xenVM = this._xenModelObject as VM;
                        job.host = HalsignHelpers.VMHome(_xenVM).uuid;
                    }
                    else if (this._xenModelObject is Host)
                    {
                        Host _xenHost = _xenModelObject as Host;
                        job.host = _xenHost.uuid;
                    }
                    else
                    {
                        job.host = Helpers.GetMaster(this._xenModelObject.Connection).uuid;
                    }

                    TimeSpan ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0));
                    job.key = "halsign_br_job_s_" + (Math.Floor(ts.TotalMilliseconds) + milliseconds++);

                    job.request = "o" + this.restore_job_name.TrimEnd().TrimStart() + "|" + this.set_storage_ip.Trim() +
                                  "|" + this.set_storage_username + "|" + this.set_storage_password + "|" +
                                  item.RootPath +
                                  "|" + item.VMRestoreInfo + "|" + restore_vm_name + "|" +
                                  this.restoreDataModel.choice_sr_uuid + "|"
                                  + (this._isBackupNetworkSettingChecked ? "1" : "0") + "|" +
                                  (this._isNewMacAddressChecked ? "1" : "0") + "|" + this.restore_network_uuid +
                                  uuid_spend;
                    ts = new DateTime(this.scheduleDate.Year, this.scheduleDate.Month,this.scheduleDate.Day, this.scheduleTime.Hour, this.scheduleTime.Minute,
                                this.scheduleTime.Second).Subtract(new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime());
                    job.start_time = Math.Floor(ts.TotalSeconds).ToString();
                    job.progress = -1;
                    job.total_storage = -1;
                    job.modify_time = "";
                    job.pid = -1;
                    job.retry = -1;
                    job.speed = -1;
                    job.status = 0;
                    schedule = HalsignUtil.ToJson(job);
                    schedule = schedule.Replace("\\/", "/");
                    dconfTemp.Add("schedule", schedule);
                    dconfTemp.Add("config_name", job.key);
                    listSchedule.Add(dconfTemp);
                }
            }

            BackupAction action = new BackupAction(_Message, BackupRestoreConfig.BackupActionKind.Restore, this._xenModelObject, list, listSchedule);
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
}
