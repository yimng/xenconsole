/* Copyright (c) Citrix Systems Inc. 
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
using HalsignModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using XenAdmin.Core;


namespace XenAdmin.Dialogs
{
    public partial class BackupViewDialog : XenDialogBase
    {

        private static Dictionary<string, string> other_config;

        private static string vm_name;

        public Dictionary<string, string> otherConfig
        {
            set
            {
                other_config = value;
            }
        }
        public string VMName
        {
            set
            {
                vm_name = value;
            }
        }
        public BackupViewDialog()
        {
            InitializeComponent();
            
        }

        public void InitBackupViewData()
        {
            BackupRestoreConfig.BrSchedule schedule = (BackupRestoreConfig.BrSchedule)HalsignUtil.JsonToObject(other_config["halsign_br_rules"], typeof(BackupRestoreConfig.BrSchedule));
            BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(other_config["halsign_br_job_s"], typeof(BackupRestoreConfig.Job));
            VMNameDetailLabel.Text =vm_name;
            this.Text =schedule.jobName + this.Text;
            if (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP_ONCE) || 
                (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP) && schedule.scheduleType==1))
            {
                JobScheduleTypeDetailLabel.Text = Messages.FULL_BACKUP_NOW;
            }
            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_ONCE) ||
                (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP) && schedule.scheduleType == 1))
            {
                JobScheduleTypeDetailLabel.Text = Messages.BACKUP_ONCE;
            }
            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_DAILY) ||
                (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP) && schedule.scheduleType == 2))
            {
                JobScheduleTypeDetailLabel.Text = Messages.BACKUP_DAILY;
                RecurDetailLabel.Text = schedule.recur + Messages.BACKUP_RECUR_TEXT_DAYS;
            }
            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_WEEKLY) ||
                (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP) && schedule.scheduleType == 3))
            {
                JobScheduleTypeDetailLabel.Text = Messages.BACKUP_WEEKLY;
                List<int> WeeklyDays = schedule.weeklyDays;
                WeeklyDays.Sort();
                string WeeklyDayStr="";
                if (WeeklyDays != null && WeeklyDays.Count>0)
                {
                    foreach (var weeklyDay in WeeklyDays)
                    {
                        switch (weeklyDay)
                        {
                            case 0: WeeklyDayStr += Messages.SUNDAY + ","; break;
                            case 1: WeeklyDayStr += Messages.MONDAY + ","; break;
                            case 2: WeeklyDayStr += Messages.TUESDAY + ","; break;
                            case 3: WeeklyDayStr += Messages.WEDNESDAY + ","; break;
                            case 4: WeeklyDayStr += Messages.THURSDAY + ","; break;
                            case 5: WeeklyDayStr += Messages.FRIDAY + ","; break;
                            case 6: WeeklyDayStr += Messages.SATURDAY + ","; break;
                        }
                    }
                }
                WeeklyDaysDetailLabel.Text += WeeklyDayStr.Substring(0, WeeklyDayStr.Length-1);
                RecurDetailLabel.Text = schedule.recur + Messages.BACKUP_RECUR_TEXT_WEEKS;
            }
            else if (job.request.StartsWith(BackupRestoreConfig.BACKUP_CIRCLE) ||
                (job.request.StartsWith(BackupRestoreConfig.FULL_BACKUP) && schedule.scheduleType == 4))
            {
                JobScheduleTypeDetailLabel.Text = Messages.BACKUP_CIRCLE;
                RecurDetailLabel.Text = schedule.recur + Messages.BACKUP_RECUR_TEXT_HOURS;
            }

            JobNameDetailLable.Text = schedule.jobName;
            StartDateDetailLabel.Text = schedule.scheduleDate;
            StartTimeDetailLabel.Text = schedule.scheduleTime;
            OptionDetailLabel.Text = schedule.expect_full_count == -1 ? "" : string.Format(Messages.FULL_BACKUP_RECURS_EVERY, schedule.expect_full_count);
        }
        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
