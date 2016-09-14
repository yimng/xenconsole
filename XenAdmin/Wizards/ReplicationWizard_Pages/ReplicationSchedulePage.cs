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
using System.Globalization;
using HalsignLib;
using XenAPI;
using XenAdmin.Controls;
using XenAdmin.Core;
using HalsignModel;


namespace XenAdmin.Wizards.ReplicationWizard_Pages
{
    public class ReplicationSchedulePage : XenTabPage
	{
        private CheckBox DNSACheckBox;
        private CheckBox SaturdayCheckBox;
        private CheckBox FridayCheckBox;
        private CheckBox ThursdayCheckBox;
        private CheckBox WednesdayCheckBox;
        private CheckBox TuesdayCheckBox;
        private CheckBox MondayCheckBox;
        private CheckBox SundayCheckBox;
        private Label RecursLabel;
        private TextBox RecurTextBox;
        private Label RecurLabel;
        private DateTimePicker DNSATimePicker;
        private DateTimePicker StartTimePicker;
        private DateTimePicker StartDatePicker;
        private Label ServerTimeLabel;
        private Label StartTimeLabel;
        private Label StartDateLabel;
        private Label label13;
        private GroupBox groupBox3;
        private RadioButton WeeklyRadioButton;
        private RadioButton DailyRadioButton;
        private RadioButton OnceRadioButton;
        private RadioButton NowRadioButton;
        private IContainer components = null;
        private RadioButton CircleRadioButton;

        private IXenObject _xenModelObject;

        public ReplicationSchedulePage(IXenObject XenObject)
		{
			InitializeComponent();
            this._xenModelObject = XenObject;
		}
        public void EditReplicationInitButton(BackupRestoreConfig.BrSchedule schedule)
        {
            this.NowRadioButton.Enabled = false;
            switch (schedule.scheduleType)
            {
                case 1: this.OnceRadioButton.Checked = true; break;
                case 2: this.DailyRadioButton.Checked = true; break;
                case 3: this.WeeklyRadioButton.Checked = true; break;
                case 4: this.CircleRadioButton.Checked = true; break;
                case 5: this.OnceRadioButton.Checked = true;  break;
            }
             
            this.StartDatePicker.Text = schedule.scheduleDate;
            this.StartTimePicker.Text = schedule.scheduleTime;
            this.RecurTextBox.Text = schedule.recur + "";

            this.SundayCheckBox.Checked = false;
            this.MondayCheckBox.Checked = false;
            this.TuesdayCheckBox.Checked = false;
            this.WednesdayCheckBox.Checked = false;
            this.ThursdayCheckBox.Checked = false;
            this.FridayCheckBox.Checked = false;
            this.SaturdayCheckBox.Checked = false;
            if (schedule.scheduleType == 3)
            {
                foreach (var weeklyDay in schedule.weeklyDays)
                {
                    switch (weeklyDay)
                    {
                        case 0: this.SundayCheckBox.Checked = true; break;
                        case 1: this.MondayCheckBox.Checked = true; break;
                        case 2: this.TuesdayCheckBox.Checked = true; break;
                        case 3: this.WednesdayCheckBox.Checked = true; break;
                        case 4: this.ThursdayCheckBox.Checked = true; break;
                        case 5: this.FridayCheckBox.Checked = true; break;
                        case 6: this.SaturdayCheckBox.Checked = true; break;
                    }
                }
            }
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplicationSchedulePage));
            this.DNSACheckBox = new System.Windows.Forms.CheckBox();
            this.SaturdayCheckBox = new System.Windows.Forms.CheckBox();
            this.FridayCheckBox = new System.Windows.Forms.CheckBox();
            this.ThursdayCheckBox = new System.Windows.Forms.CheckBox();
            this.WednesdayCheckBox = new System.Windows.Forms.CheckBox();
            this.TuesdayCheckBox = new System.Windows.Forms.CheckBox();
            this.MondayCheckBox = new System.Windows.Forms.CheckBox();
            this.SundayCheckBox = new System.Windows.Forms.CheckBox();
            this.RecursLabel = new System.Windows.Forms.Label();
            this.RecurTextBox = new System.Windows.Forms.TextBox();
            this.RecurLabel = new System.Windows.Forms.Label();
            this.DNSATimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartDatePicker = new System.Windows.Forms.DateTimePicker();
            this.ServerTimeLabel = new System.Windows.Forms.Label();
            this.StartTimeLabel = new System.Windows.Forms.Label();
            this.StartDateLabel = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.WeeklyRadioButton = new System.Windows.Forms.RadioButton();
            this.DailyRadioButton = new System.Windows.Forms.RadioButton();
            this.OnceRadioButton = new System.Windows.Forms.RadioButton();
            this.NowRadioButton = new System.Windows.Forms.RadioButton();
            this.CircleRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // DNSACheckBox
            // 
            resources.ApplyResources(this.DNSACheckBox, "DNSACheckBox");
            this.DNSACheckBox.Name = "DNSACheckBox";
            this.DNSACheckBox.UseVisualStyleBackColor = true;
            // 
            // SaturdayCheckBox
            // 
            resources.ApplyResources(this.SaturdayCheckBox, "SaturdayCheckBox");
            this.SaturdayCheckBox.Checked = true;
            this.SaturdayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SaturdayCheckBox.Name = "SaturdayCheckBox";
            this.SaturdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // FridayCheckBox
            // 
            resources.ApplyResources(this.FridayCheckBox, "FridayCheckBox");
            this.FridayCheckBox.Checked = true;
            this.FridayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FridayCheckBox.Name = "FridayCheckBox";
            this.FridayCheckBox.UseVisualStyleBackColor = true;
            // 
            // ThursdayCheckBox
            // 
            resources.ApplyResources(this.ThursdayCheckBox, "ThursdayCheckBox");
            this.ThursdayCheckBox.Checked = true;
            this.ThursdayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ThursdayCheckBox.Name = "ThursdayCheckBox";
            this.ThursdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // WednesdayCheckBox
            // 
            resources.ApplyResources(this.WednesdayCheckBox, "WednesdayCheckBox");
            this.WednesdayCheckBox.Checked = true;
            this.WednesdayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WednesdayCheckBox.Name = "WednesdayCheckBox";
            this.WednesdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // TuesdayCheckBox
            // 
            resources.ApplyResources(this.TuesdayCheckBox, "TuesdayCheckBox");
            this.TuesdayCheckBox.Checked = true;
            this.TuesdayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TuesdayCheckBox.Name = "TuesdayCheckBox";
            this.TuesdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // MondayCheckBox
            // 
            resources.ApplyResources(this.MondayCheckBox, "MondayCheckBox");
            this.MondayCheckBox.Checked = true;
            this.MondayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MondayCheckBox.Name = "MondayCheckBox";
            this.MondayCheckBox.UseVisualStyleBackColor = true;
            // 
            // SundayCheckBox
            // 
            resources.ApplyResources(this.SundayCheckBox, "SundayCheckBox");
            this.SundayCheckBox.Checked = true;
            this.SundayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SundayCheckBox.Name = "SundayCheckBox";
            this.SundayCheckBox.UseVisualStyleBackColor = true;
            // 
            // RecursLabel
            // 
            resources.ApplyResources(this.RecursLabel, "RecursLabel");
            this.RecursLabel.Name = "RecursLabel";
            // 
            // RecurTextBox
            // 
            resources.ApplyResources(this.RecurTextBox, "RecurTextBox");
            this.RecurTextBox.Name = "RecurTextBox";
            this.RecurTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RecurTextBox_KeyPress);
            // 
            // RecurLabel
            // 
            resources.ApplyResources(this.RecurLabel, "RecurLabel");
            this.RecurLabel.Name = "RecurLabel";
            // 
            // DNSATimePicker
            // 
            resources.ApplyResources(this.DNSATimePicker, "DNSATimePicker");
            this.DNSATimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.DNSATimePicker.Name = "DNSATimePicker";
            this.DNSATimePicker.ShowUpDown = true;
            this.DNSATimePicker.Value = new System.DateTime(2012, 5, 15, 4, 0, 0, 0);
            // 
            // StartTimePicker
            // 
            resources.ApplyResources(this.StartTimePicker, "StartTimePicker");
            this.StartTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.StartTimePicker.Name = "StartTimePicker";
            this.StartTimePicker.ShowUpDown = true;
            // 
            // StartDatePicker
            // 
            resources.ApplyResources(this.StartDatePicker, "StartDatePicker");
            this.StartDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDatePicker.Name = "StartDatePicker";
            // 
            // ServerTimeLabel
            // 
            resources.ApplyResources(this.ServerTimeLabel, "ServerTimeLabel");
            this.ServerTimeLabel.Name = "ServerTimeLabel";
            // 
            // StartTimeLabel
            // 
            resources.ApplyResources(this.StartTimeLabel, "StartTimeLabel");
            this.StartTimeLabel.Name = "StartTimeLabel";
            // 
            // StartDateLabel
            // 
            resources.ApplyResources(this.StartDateLabel, "StartDateLabel");
            this.StartDateLabel.Name = "StartDateLabel";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.CircleRadioButton);
            this.groupBox3.Controls.Add(this.WeeklyRadioButton);
            this.groupBox3.Controls.Add(this.DailyRadioButton);
            this.groupBox3.Controls.Add(this.OnceRadioButton);
            this.groupBox3.Controls.Add(this.NowRadioButton);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // WeeklyRadioButton
            // 
            resources.ApplyResources(this.WeeklyRadioButton, "WeeklyRadioButton");
            this.WeeklyRadioButton.Name = "WeeklyRadioButton";
            this.WeeklyRadioButton.UseVisualStyleBackColor = true;
            this.WeeklyRadioButton.CheckedChanged += new System.EventHandler(this.WeeklyRadioButton_CheckedChanged);
            // 
            // DailyRadioButton
            // 
            resources.ApplyResources(this.DailyRadioButton, "DailyRadioButton");
            this.DailyRadioButton.Name = "DailyRadioButton";
            this.DailyRadioButton.UseVisualStyleBackColor = true;
            this.DailyRadioButton.CheckedChanged += new System.EventHandler(this.DailyRadioButton_CheckedChanged);
            // 
            // OnceRadioButton
            // 
            resources.ApplyResources(this.OnceRadioButton, "OnceRadioButton");
            this.OnceRadioButton.Name = "OnceRadioButton";
            this.OnceRadioButton.UseVisualStyleBackColor = true;
            this.OnceRadioButton.CheckedChanged += new System.EventHandler(this.OnceRadioButton_CheckedChanged);
            // 
            // NowRadioButton
            // 
            resources.ApplyResources(this.NowRadioButton, "NowRadioButton");
            this.NowRadioButton.Checked = true;
            this.NowRadioButton.Name = "NowRadioButton";
            this.NowRadioButton.TabStop = true;
            this.NowRadioButton.UseVisualStyleBackColor = true;
            this.NowRadioButton.CheckedChanged += new System.EventHandler(this.NowRadioButton_CheckedChanged);
            // 
            // CircleRadioButton
            // 
            resources.ApplyResources(this.CircleRadioButton, "CircleRadioButton");
            this.CircleRadioButton.Name = "CircleRadioButton";
            this.CircleRadioButton.UseVisualStyleBackColor = true;
            this.CircleRadioButton.CheckedChanged += new System.EventHandler(this.CircleRadioButton_CheckedChanged);
            // 
            // ReplicationSchedulePage
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.DNSACheckBox);
            this.Controls.Add(this.SaturdayCheckBox);
            this.Controls.Add(this.FridayCheckBox);
            this.Controls.Add(this.ThursdayCheckBox);
            this.Controls.Add(this.WednesdayCheckBox);
            this.Controls.Add(this.TuesdayCheckBox);
            this.Controls.Add(this.MondayCheckBox);
            this.Controls.Add(this.SundayCheckBox);
            this.Controls.Add(this.RecursLabel);
            this.Controls.Add(this.RecurTextBox);
            this.Controls.Add(this.RecurLabel);
            this.Controls.Add(this.DNSATimePicker);
            this.Controls.Add(this.StartTimePicker);
            this.Controls.Add(this.StartDatePicker);
            this.Controls.Add(this.ServerTimeLabel);
            this.Controls.Add(this.StartTimeLabel);
            this.Controls.Add(this.StartDateLabel);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.groupBox3);
            this.Name = "ReplicationSchedulePage";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
                return "Set Job Schedule";
            }
        }

        public override string PageTitle
        {
            get
            {
                return Messages.SCHEDULE_REPLICATION;
            }
        }

        public override string Text
        {
            get
            {
                return Messages.RESTORE_JOB_SCHEDULE_TEXT;
            }
        }

        public override void PageLoaded(PageLoadedDirection direction)
        {
            base.PageLoaded(direction);
            if (direction == PageLoadedDirection.Forward)
            {
                //HelpersGUI.FocusFirstControl(base.Controls);
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
                    this.ServerTimeLabel.Text = Host.get_servertime(this._xenModelObject.Connection.Session, _host.opaque_ref).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
                }
                else
                {
                    this.ServerTimeLabel.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
                }
            }
        }

        public void SetPageLoad(bool isMustNow)
        {
            if (isMustNow)
            {
                this.OnceRadioButton.Visible = false;
                this.DailyRadioButton.Visible = false;
                this.WeeklyRadioButton.Visible = false;
                this.CircleRadioButton.Visible = false;
            }
            else
            {
                this.OnceRadioButton.Visible = true;
                this.DailyRadioButton.Visible = true;
                this.WeeklyRadioButton.Visible = true;
            }
        }

        private void NowRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (this.NowRadioButton.Checked)
            {
                this.StartDateLabel.Visible = false;
                this.StartTimeLabel.Visible = false;
                this.StartDatePicker.Visible = false;
                this.StartTimePicker.Visible = false;
                this.StartDatePicker.Visible = false;
                this.StartTimePicker.Visible = false;
                this.RecurLabel.Visible = false;
                this.RecurTextBox.Visible = false;
                this.RecursLabel.Visible = false;
                this.SundayCheckBox.Visible = false;
                this.MondayCheckBox.Visible = false;
                this.TuesdayCheckBox.Visible = false;
                this.WednesdayCheckBox.Visible = false;
                this.ThursdayCheckBox.Visible = false;
                this.FridayCheckBox.Visible = false;
                this.SaturdayCheckBox.Visible = false;
            }
        }

        private void OnceRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (this.OnceRadioButton.Checked)
            {
                this.StartDateLabel.Visible = true;
                this.StartTimeLabel.Visible = true;
                this.StartDatePicker.Visible = true;
                this.StartTimePicker.Visible = true;
                //this.DNSACheckBox.Visible = false;
                //this.DNSATimePicker.Visible = false;
                this.RecurLabel.Visible = false;
                this.RecurTextBox.Visible = false;
                this.RecursLabel.Visible = false;
                this.SundayCheckBox.Visible = false;
                this.MondayCheckBox.Visible = false;
                this.TuesdayCheckBox.Visible = false;
                this.WednesdayCheckBox.Visible = false;
                this.ThursdayCheckBox.Visible = false;
                this.FridayCheckBox.Visible = false;
                this.SaturdayCheckBox.Visible = false;
            }
        }

        private void DailyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (this.DailyRadioButton.Checked)
            {
                this.RecursLabel.Text = Messages.BACKUP_RECUR_TEXT_DAYS;
                this.StartDateLabel.Visible = true;
                this.StartDatePicker.Visible = true;
                this.StartTimeLabel.Visible = true;
                this.StartTimePicker.Visible = true;
                //this.DNSACheckBox.Visible = true;
                //this.DNSATimePicker.Visible = true;
                this.RecurLabel.Visible = true;
                this.RecurTextBox.Visible = true;
                this.RecursLabel.Visible = true;
                this.SundayCheckBox.Visible = false;
                this.MondayCheckBox.Visible = false;
                this.TuesdayCheckBox.Visible = false;
                this.WednesdayCheckBox.Visible = false;
                this.ThursdayCheckBox.Visible = false;
                this.FridayCheckBox.Visible = false;
                this.SaturdayCheckBox.Visible = false;
            }
        }

        private void WeeklyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (this.WeeklyRadioButton.Checked)
            {
                this.RecursLabel.Text = Messages.BACKUP_RECUR_TEXT_WEEKS;
                this.ServerTimeLabel.Visible = true;
                this.StartDateLabel.Visible = true;
                this.StartDatePicker.Visible = true;
                this.StartTimeLabel.Visible = true;
                this.StartTimePicker.Visible = true;
                //this.DNSACheckBox.Visible = true;
                //this.DNSATimePicker.Visible = true;
                this.RecurLabel.Visible = true;
                this.RecurTextBox.Visible = true;
                this.RecursLabel.Visible = true;
                this.SundayCheckBox.Visible = true;
                this.MondayCheckBox.Visible = true;
                this.TuesdayCheckBox.Visible = true;
                this.WednesdayCheckBox.Visible = true;
                this.ThursdayCheckBox.Visible = true;
                this.FridayCheckBox.Visible = true;
                this.SaturdayCheckBox.Visible = true;
            }
        }

        private void RecurTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "[0-9]"))
            {
                e.Handled = true;
                return;
            }
        }

        internal bool NowChecked
        {
            get
            {
                return this.NowRadioButton.Checked;
            }
        }

        internal bool OnceChecked
        {
            get
            {
                return this.OnceRadioButton.Checked;
            }
        }

        internal bool DailyChecked
        {
            get
            {
                return this.DailyRadioButton.Checked;
            }
        }

        internal bool CircleChecked
        {
            get
            {
                return this.CircleRadioButton.Checked;
            }
        }

        internal bool WeeklyChecked
        {
            get
            {
                return this.WeeklyRadioButton.Checked;
            }
        }

        internal string StartDateText
        {
            get
            {
                return this.StartDatePicker.Text;
            }
        }

        internal DateTime StartDateValue
        {
            get
            {
                return this.StartDatePicker.Value;
            }
        }

        internal string StartTimeText
        {
            get
            {
                return this.StartTimePicker.Text;
            }
        }

        internal DateTime StartTimeValue
        {
            get
            {
                return this.StartTimePicker.Value;
            }
        }

        internal int RecurText
        {
            get
            {
                return this.RecurTextBox.Text == "" ? 0 : Int32.Parse(this.RecurTextBox.Text);
            }
        }

        internal bool MondayChecked
        {
            get
            {
                return this.MondayCheckBox.Checked;
            }
        }

        internal bool TuesdayChecked
        {
            get
            {
                return this.TuesdayCheckBox.Checked;
            }
        }

        internal bool WednesdayChecked
        {
            get
            {
                return this.WednesdayCheckBox.Checked;
            }
        }

        internal bool ThursdayChecked
        {
            get
            {
                return this.ThursdayCheckBox.Checked;
            }
        }

        internal bool FridayChecked
        {
            get
            {
                return this.FridayCheckBox.Checked;
            }
        }

        internal bool SaturdayChecked
        {
            get
            {
                return this.SaturdayCheckBox.Checked;
            }
        }

        internal bool SundayChecked
        {
            get
            {
                return this.SundayCheckBox.Checked;
            }
        }

        /*
        internal bool CircleChecked()
        {
            throw new NotImplementedException();
        }
        */
        private void CircleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CircleRadioButton.Checked)
            {
                this.StartDateLabel.Visible = true;
                this.StartDatePicker.Visible = true;
                this.StartTimeLabel.Visible = true;
                this.StartTimePicker.Visible = true;
                //this.DNSACheckBox.Visible = true;
                //this.DNSATimePicker.Visible = true;
                this.RecurLabel.Visible = true;
                this.RecurTextBox.Visible = true;
                this.RecursLabel.Visible = true;
                this.RecursLabel.Text = Messages.BACKUP_RECUR_TEXT_HOURS;
                this.SundayCheckBox.Visible = false;
                this.MondayCheckBox.Visible = false;
                this.TuesdayCheckBox.Visible = false;
                this.WednesdayCheckBox.Visible = false;
                this.ThursdayCheckBox.Visible = false;
                this.FridayCheckBox.Visible = false;
                this.SaturdayCheckBox.Visible = false;
            }
        }
    }
}
