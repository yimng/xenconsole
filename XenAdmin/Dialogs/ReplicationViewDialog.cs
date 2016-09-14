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
using XenAdmin.Network;
using XenAPI;


namespace XenAdmin.Dialogs
{
    public partial class ReplicationViewDialog : XenDialogBase
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
        public ReplicationViewDialog()
        {
            InitializeComponent();
           
        }

        public void InitReplicationViewData()
        {
            BackupRestoreConfig.BrSchedule schedule = (BackupRestoreConfig.BrSchedule)HalsignUtil.JsonToObject(other_config["halsign_rep_rules"], typeof(BackupRestoreConfig.BrSchedule));
            BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(other_config["halsign_br_job_r"], typeof(BackupRestoreConfig.Job));
            this.Text = schedule.jobName + this.Text;
            string[] details = schedule.details.Split('|');
            VMNameDetailLabel.Text =vm_name;

            if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_ONCE))
            {
                JobScheduleTypeDetailLabel.Text = Messages.REPLICATION_ONCE;
            }
            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_DAILY))
            {
                JobScheduleTypeDetailLabel.Text = Messages.REPLICATION_DAILY;
                RecurDetailLabel.Text = schedule.recur + Messages.BACKUP_RECUR_TEXT_DAYS;
            }
            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_WEEKLY))
            {
                JobScheduleTypeDetailLabel.Text = Messages.REPLICATION_WEEKLY;
                List<int> WeeklyDays = schedule.weeklyDays;
                WeeklyDays.Sort();
                string WeeklyDayStr="";
                if (WeeklyDays != null && WeeklyDays.Count > 0)
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
            else if (job.request.StartsWith(BackupRestoreConfig.REPLICATION_CIRCLE))
            {
                JobScheduleTypeDetailLabel.Text = Messages.REPLICATION_CIRCLE;
                RecurDetailLabel.Text = schedule.recur + Messages.BACKUP_RECUR_TEXT_HOURS;
            }

            JobNameDetailLable.Text = schedule.jobName;
            StartDateDetailLabel.Text = schedule.scheduleDate;
            StartTimeDetailLabel.Text = schedule.scheduleTime;
            AppendDetailLabel.Text = details[3];
            DestServerDetailLabel.Text = details[0];
            DestSRDetailLabel.Text = details[4];
            NetworkDetailLabel.Text = details[7];
            string hostname = details[0];
            //int port = 80; // default
            string user = details[1];
            string password = details[2];
            Session session=null;
            try
            {
                XenConnection XenConnection = new XenConnection(hostname, "");
                session = SessionFactory.CreateSession(connection, hostname, 443);
                session.login_with_password(user, password, API_Version.LATEST); 
                /*XenRef<SR> sr = SR.get_by_uuid(session, details[4]);
                DestSRDetailLabel.Text = SR.get_name_label(session, sr);
                XenRef<XenAPI.Network> network = XenAPI.Network.get_by_uuid(session, details[7]);
                NetworkDetailLabel.Text = XenAPI.Network.get_name_label(session, network).Replace("Pool-wide network associated with eth", Messages.NETWORK);
                */
                setSrNetwork(session, details);
            }catch (Failure exn)
            {
                if ("HOST_IS_SLAVE".Equals(exn.ErrorDescription[0]))
                {
                    session = SessionFactory.CreateSession(connection, exn.ErrorDescription[1], 443);
                    session.login_with_password(user, password, API_Version.LATEST);
                    setSrNetwork(session, details);
                }
            }
            /*catch (Failure f)
            {
                if (f.ErrorDescription.Count > 0)
                    throw new CancelledException();
            }*/
            finally
            {
                if (session != null)
                {
                    session.logout();
                }
            }

            OptionDetailLabel.Text = "";
        }
        private void setSrNetwork(Session session, string[] details)
        {
            XenRef<SR> sr = SR.get_by_uuid(session, details[4]);
            DestSRDetailLabel.Text = SR.get_name_label(session, sr);
            XenRef<XenAPI.Network> network = XenAPI.Network.get_by_uuid(session, details[7]);
            NetworkDetailLabel.Text = XenAPI.Network.get_name_label(session, network).Replace("Pool-wide network associated with eth", Messages.NETWORK);
        }


        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
