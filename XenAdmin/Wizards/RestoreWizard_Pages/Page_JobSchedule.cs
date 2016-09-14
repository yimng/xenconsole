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

namespace XenAdmin.Wizards.RestoreWizard_Pages
{
    public partial class Page_JobSchedule : XenTabPage
	{
        private GroupBox GroupBox_ScheduleType;
        private RadioButton RadioButton_Once;
        private RadioButton RadioButton_Now;
        private Label Label_ServerTime;
        private Label Label_StartDate;
        private Label Label_StartTime;
        private DateTimePicker DateTimePicker_StartDate;
        private Label Label_Server_ScheduleTime;
        private DateTimePicker DateTimePicker_StartTime;

		public Page_JobSchedule()
		{
			InitializeComponent();
		}

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page_JobSchedule));
            this.GroupBox_ScheduleType = new System.Windows.Forms.GroupBox();
            this.RadioButton_Once = new System.Windows.Forms.RadioButton();
            this.RadioButton_Now = new System.Windows.Forms.RadioButton();
            this.Label_ServerTime = new System.Windows.Forms.Label();
            this.Label_StartDate = new System.Windows.Forms.Label();
            this.Label_StartTime = new System.Windows.Forms.Label();
            this.DateTimePicker_StartDate = new System.Windows.Forms.DateTimePicker();
            this.DateTimePicker_StartTime = new System.Windows.Forms.DateTimePicker();
            this.Label_Server_ScheduleTime = new System.Windows.Forms.Label();
            this.GroupBox_ScheduleType.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox_ScheduleType
            // 
            this.GroupBox_ScheduleType.AccessibleDescription = null;
            this.GroupBox_ScheduleType.AccessibleName = null;
            resources.ApplyResources(this.GroupBox_ScheduleType, "GroupBox_ScheduleType");
            this.GroupBox_ScheduleType.BackgroundImage = null;
            this.GroupBox_ScheduleType.Controls.Add(this.RadioButton_Once);
            this.GroupBox_ScheduleType.Controls.Add(this.RadioButton_Now);
            this.GroupBox_ScheduleType.Font = null;
            this.GroupBox_ScheduleType.Name = "GroupBox_ScheduleType";
            this.GroupBox_ScheduleType.TabStop = false;
            // 
            // RadioButton_Once
            // 
            this.RadioButton_Once.AccessibleDescription = null;
            this.RadioButton_Once.AccessibleName = null;
            resources.ApplyResources(this.RadioButton_Once, "RadioButton_Once");
            this.RadioButton_Once.BackgroundImage = null;
            this.RadioButton_Once.Font = null;
            this.RadioButton_Once.Name = "RadioButton_Once";
            this.RadioButton_Once.UseVisualStyleBackColor = true;
            this.RadioButton_Once.CheckedChanged += new System.EventHandler(this.RadioButton_Once_CheckedChanged);
            // 
            // RadioButton_Now
            // 
            this.RadioButton_Now.AccessibleDescription = null;
            this.RadioButton_Now.AccessibleName = null;
            resources.ApplyResources(this.RadioButton_Now, "RadioButton_Now");
            this.RadioButton_Now.BackgroundImage = null;
            this.RadioButton_Now.Checked = true;
            this.RadioButton_Now.Font = null;
            this.RadioButton_Now.Name = "RadioButton_Now";
            this.RadioButton_Now.TabStop = true;
            this.RadioButton_Now.UseVisualStyleBackColor = true;
            this.RadioButton_Now.CheckedChanged += new System.EventHandler(this.RadioButton_Now_CheckedChanged);
            // 
            // Label_ServerTime
            // 
            this.Label_ServerTime.AccessibleDescription = null;
            this.Label_ServerTime.AccessibleName = null;
            resources.ApplyResources(this.Label_ServerTime, "Label_ServerTime");
            this.Label_ServerTime.Font = null;
            this.Label_ServerTime.Name = "Label_ServerTime";
            // 
            // Label_StartDate
            // 
            this.Label_StartDate.AccessibleDescription = null;
            this.Label_StartDate.AccessibleName = null;
            resources.ApplyResources(this.Label_StartDate, "Label_StartDate");
            this.Label_StartDate.Font = null;
            this.Label_StartDate.Name = "Label_StartDate";
            // 
            // Label_StartTime
            // 
            this.Label_StartTime.AccessibleDescription = null;
            this.Label_StartTime.AccessibleName = null;
            resources.ApplyResources(this.Label_StartTime, "Label_StartTime");
            this.Label_StartTime.Font = null;
            this.Label_StartTime.Name = "Label_StartTime";
            // 
            // DateTimePicker_StartDate
            // 
            this.DateTimePicker_StartDate.AccessibleDescription = null;
            this.DateTimePicker_StartDate.AccessibleName = null;
            resources.ApplyResources(this.DateTimePicker_StartDate, "DateTimePicker_StartDate");
            this.DateTimePicker_StartDate.BackgroundImage = null;
            this.DateTimePicker_StartDate.CalendarFont = null;
            this.DateTimePicker_StartDate.Font = null;
            this.DateTimePicker_StartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePicker_StartDate.Name = "DateTimePicker_StartDate";
            // 
            // DateTimePicker_StartTime
            // 
            this.DateTimePicker_StartTime.AccessibleDescription = null;
            this.DateTimePicker_StartTime.AccessibleName = null;
            resources.ApplyResources(this.DateTimePicker_StartTime, "DateTimePicker_StartTime");
            this.DateTimePicker_StartTime.BackgroundImage = null;
            this.DateTimePicker_StartTime.CalendarFont = null;
            this.DateTimePicker_StartTime.Font = null;
            this.DateTimePicker_StartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.DateTimePicker_StartTime.Name = "DateTimePicker_StartTime";
            this.DateTimePicker_StartTime.ShowUpDown = true;
            // 
            // Label_Server_ScheduleTime
            // 
            this.Label_Server_ScheduleTime.AccessibleDescription = null;
            this.Label_Server_ScheduleTime.AccessibleName = null;
            resources.ApplyResources(this.Label_Server_ScheduleTime, "Label_Server_ScheduleTime");
            this.Label_Server_ScheduleTime.Font = null;
            this.Label_Server_ScheduleTime.Name = "Label_Server_ScheduleTime";
            // 
            // Page_JobSchedule
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.Label_Server_ScheduleTime);
            this.Controls.Add(this.DateTimePicker_StartTime);
            this.Controls.Add(this.DateTimePicker_StartDate);
            this.Controls.Add(this.Label_StartTime);
            this.Controls.Add(this.Label_StartDate);
            this.Controls.Add(this.Label_ServerTime);
            this.Controls.Add(this.GroupBox_ScheduleType);
            this.Font = null;
            this.Name = "Page_JobSchedule";
            this.GroupBox_ScheduleType.ResumeLayout(false);
            this.GroupBox_ScheduleType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public override string PageTitle
        {
            get
            {
                return Messages.RESTORE_JOB_SCHEDULE_TITLE;
            }
        }

        public override string Text
        {
            get
            {
                return Messages.RESTORE_JOB_SCHEDULE_TEXT;
            }
        }

        public void UpdateServerTime(IXenObject XenModelObject)
        {
            Host _host = null;
            if (XenModelObject is VM)
            {
                _host = HalsignHelpers.VMHome(XenModelObject as VM);
            }
            else if (XenModelObject is Host)
            {
                _host = XenModelObject as Host;
            }
            if (_host != null)
            {
                this.Label_Server_ScheduleTime.Text = Host.get_servertime(XenModelObject.Connection.Session, _host.opaque_ref).ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
            }
            else
            {
                this.Label_Server_ScheduleTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
            }
        }

        private void RadioButton_Now_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButton_Now.Checked)
            {
                this.DateTimePicker_StartDate.Enabled = false;
                this.DateTimePicker_StartTime.Enabled = false;
            }
        }

        private void RadioButton_Once_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButton_Once.Checked)
            {
                this.DateTimePicker_StartDate.Enabled = true;
                this.DateTimePicker_StartTime.Enabled = true;
            }
        }

        public DateTime scheduleDate
        {
            get { return DateTimePicker_StartDate.Value; }
        }

        public DateTime scheduleTime
        {
            get { return DateTimePicker_StartTime.Value; }
        }

        public bool isOnceScheduleChecked
        {
            get { return RadioButton_Once.Checked; }
        }
	}
}
