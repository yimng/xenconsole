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
using XenAPI;
using System.Threading;
using System.Text.RegularExpressions;
using XenAdmin.Controls;
using HalsignModel;

namespace XenAdmin.Wizards.BackupWizard_Pages
{
    public class BackupSchedulePage : XenTabPage
	{
        private RadioButton OnceRadioButton;
        private RadioButton DailyRadioButton;
        private RadioButton WeeklyRadioButton;
        private RadioButton NowRadioButton;
        private Label StartDateLabel;
        private Label StartTimeLabel;
        private Label RecurLabel;
        private DateTimePicker StartDatePicker;
        private DateTimePicker StartTimePicker;
        private MaskedTextBox RecurTextBox;
        private CheckBox SundayCheckBox;
        private CheckBox MondayCheckBox;
        private CheckBox TuesdayCheckBox;
        private CheckBox WednesdayCheckBox;
        private CheckBox ThursdayCheckBox;
        private CheckBox FridayCheckBox;
        private CheckBox SaturdayCheckBox;
        private Label ServerTimeLabel;
        private Label ServerDateLabel;
        private RadioButton CircleRadioButton;
        private Label RecursLabel;
        private GroupBox groupBoxOptions;
        private RadioButton radioButtonIncrement;
        private RadioButton radioButtonFull;
        private CheckBox expectFullCheckBox;
        private MaskedTextBox expectFullCountTextBox;
        private Label countlabel;

        private string StrScheduleType = "";

        public override string Text
        {
            get
            {
                return Messages.SCHEDULE_JOB;
            }
        }

        public override string PageTitle
        {
            get
            {
                return Messages.SCHEDULE_JOB;
            }
        }

    
		public BackupSchedulePage()
		{
			InitializeComponent();
            SetGroupBoxOptions(new Point(123, 86), new Size(295, 46),false);
		}

        public string _StrScheduleType
        {
            get
            {
                return StrScheduleType;
            }
        }

        public Boolean _MondayCheckBoxIsChecked()
        {
            return MondayCheckBox.Checked;
        }

        public Boolean _SundayCheckBoxIsChecked()
        {
            return SundayCheckBox.Checked;
        }

        public Boolean _TuesdayCheckBoxIsChecked()
        {
            return TuesdayCheckBox.Checked;
        }

        public Boolean _WednesdayCheckBoxIsChecked()
        {
            return WednesdayCheckBox.Checked;
        }

        public Boolean _ThursdayCheckBoxIsChecked()
        {
            return ThursdayCheckBox.Checked;
        }

        public Boolean _FridayCheckBoxIsChecked()
        {
            return FridayCheckBox.Checked;
        }

        public Boolean _SaturdayCheckBoxIsChecked()
        {
            return SaturdayCheckBox.Checked;
        }

        public string ServerDateText
        {
            set
            {
                this.ServerDateLabel.Text = value;
            }
        }

        public String _RecurTextBox
        {
            get
            {
                return this.RecurTextBox.Text;
            }
        }

        public String StartDatePickerText
        {
            get 
            {
                return this.StartDatePicker.Text;
            }
        }

        public String StartTimePickerText
        {
            get 
            {
                return this.StartTimePicker.Text;
            }
        }

        public DateTime StartTimePickerValue
        {
            get 
            {
                return this.StartTimePicker.Value;
            }
        }

        public DateTime StartDatePickerValue
        {
            get 
            {
                return this.StartDatePicker.Value;
            }
        }

        public void EditBackupInitButton(BackupRestoreConfig.BrSchedule schedule)
        {
            this.NowRadioButton.Enabled=false;

            switch (schedule.scheduleType)
            {
                case 1: this.OnceRadioButton.Checked = true;break;
                case 2: this.DailyRadioButton.Checked = true; break;
                case 3: this.WeeklyRadioButton.Checked = true; break;
                case 4: this.CircleRadioButton.Checked = true; break;
                case 5: this.OnceRadioButton.Checked = true; this.radioButtonFull.Checked = true; break;
            }
            this.StartDatePicker.Text = schedule.scheduleDate;
            this.StartTimePicker.Text = schedule.scheduleTime;
            this.RecurTextBox.Text = schedule.recur+"";
            if (schedule.expect_full_count > 0)
            {
                this.expectFullCheckBox.Checked = true;
                this.expectFullCountTextBox.Text = schedule.expect_full_count+"";
            }

            this.SundayCheckBox.Checked = false; 
            this.MondayCheckBox.Checked = false;
            this.TuesdayCheckBox.Checked = false;
            this.WednesdayCheckBox.Checked = false;
            this.ThursdayCheckBox.Checked = false;
            this.FridayCheckBox.Checked = false;
            this.SaturdayCheckBox.Checked = false; 
            if (schedule.scheduleType==3)
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

        public Boolean NowRadioButtonIsChecked()
        {
            return this.NowRadioButton.Checked;
        }

        public Boolean OnceRadioButtonIsChecked()
        {
            return this.OnceRadioButton.Checked;
        }

        public Boolean DailyRadioButtonIsChecked()
        {
            return this.DailyRadioButton.Checked;
        }

        public Boolean CircleRadioButtonIsChecked()
        {
            return this.CircleRadioButton.Checked;
        }

        public Boolean WeeklyRadioButtonIsChecked()
        {
            return this.WeeklyRadioButton.Checked;
        }
        public bool IsFullBackup
        {
            get { return this.radioButtonFull.Checked; }
        }
        public int _expectFullCountTextBox
        {
            get
            {
                return this.expectFullCountTextBox.Text == "" ? 0 : System.Int32.Parse(this.expectFullCountTextBox.Text);
            }
        }
        public bool _expectFullCheckBox
        {
            get
            {
                return this.expectFullCheckBox.Checked;
            }
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupSchedulePage));
            this.NowRadioButton = new System.Windows.Forms.RadioButton();
            this.OnceRadioButton = new System.Windows.Forms.RadioButton();
            this.DailyRadioButton = new System.Windows.Forms.RadioButton();
            this.WeeklyRadioButton = new System.Windows.Forms.RadioButton();
            this.StartDateLabel = new System.Windows.Forms.Label();
            this.StartTimeLabel = new System.Windows.Forms.Label();
            this.RecurLabel = new System.Windows.Forms.Label();
            this.StartDatePicker = new System.Windows.Forms.DateTimePicker();
            this.StartTimePicker = new System.Windows.Forms.DateTimePicker();
            this.RecurTextBox = new System.Windows.Forms.MaskedTextBox();
            this.SundayCheckBox = new System.Windows.Forms.CheckBox();
            this.MondayCheckBox = new System.Windows.Forms.CheckBox();
            this.TuesdayCheckBox = new System.Windows.Forms.CheckBox();
            this.WednesdayCheckBox = new System.Windows.Forms.CheckBox();
            this.ThursdayCheckBox = new System.Windows.Forms.CheckBox();
            this.FridayCheckBox = new System.Windows.Forms.CheckBox();
            this.SaturdayCheckBox = new System.Windows.Forms.CheckBox();
            this.ServerTimeLabel = new System.Windows.Forms.Label();
            this.ServerDateLabel = new System.Windows.Forms.Label();
            this.CircleRadioButton = new System.Windows.Forms.RadioButton();
            this.RecursLabel = new System.Windows.Forms.Label();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.radioButtonIncrement = new System.Windows.Forms.RadioButton();
            this.radioButtonFull = new System.Windows.Forms.RadioButton();
            this.expectFullCheckBox = new System.Windows.Forms.CheckBox();
            this.expectFullCountTextBox = new System.Windows.Forms.MaskedTextBox();
            this.countlabel = new System.Windows.Forms.Label();
            this.groupBoxOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // NowRadioButton
            // 
            resources.ApplyResources(this.NowRadioButton, "NowRadioButton");
            this.NowRadioButton.Checked = true;
            this.NowRadioButton.Name = "NowRadioButton";
            this.NowRadioButton.TabStop = true;
            this.NowRadioButton.UseVisualStyleBackColor = true;
            this.NowRadioButton.CheckedChanged += new System.EventHandler(this.NowButtonCheckedChanged);
            // 
            // OnceRadioButton
            // 
            resources.ApplyResources(this.OnceRadioButton, "OnceRadioButton");
            this.OnceRadioButton.Name = "OnceRadioButton";
            this.OnceRadioButton.UseVisualStyleBackColor = true;
            this.OnceRadioButton.CheckedChanged += new System.EventHandler(this.OnceButtonCheckedChanged);
            // 
            // DailyRadioButton
            // 
            resources.ApplyResources(this.DailyRadioButton, "DailyRadioButton");
            this.DailyRadioButton.Name = "DailyRadioButton";
            this.DailyRadioButton.UseVisualStyleBackColor = true;
            this.DailyRadioButton.CheckedChanged += new System.EventHandler(this.DailyButtonCheckedChanged);
            // 
            // WeeklyRadioButton
            // 
            resources.ApplyResources(this.WeeklyRadioButton, "WeeklyRadioButton");
            this.WeeklyRadioButton.Name = "WeeklyRadioButton";
            this.WeeklyRadioButton.UseVisualStyleBackColor = true;
            this.WeeklyRadioButton.CheckedChanged += new System.EventHandler(this.WeeklyButtonCheckedChanged);
            // 
            // StartDateLabel
            // 
            resources.ApplyResources(this.StartDateLabel, "StartDateLabel");
            this.StartDateLabel.Name = "StartDateLabel";
            // 
            // StartTimeLabel
            // 
            resources.ApplyResources(this.StartTimeLabel, "StartTimeLabel");
            this.StartTimeLabel.Name = "StartTimeLabel";
            // 
            // RecurLabel
            // 
            resources.ApplyResources(this.RecurLabel, "RecurLabel");
            this.RecurLabel.Name = "RecurLabel";
            // 
            // StartDatePicker
            // 
            resources.ApplyResources(this.StartDatePicker, "StartDatePicker");
            this.StartDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDatePicker.Name = "StartDatePicker";
            // 
            // StartTimePicker
            // 
            resources.ApplyResources(this.StartTimePicker, "StartTimePicker");
            this.StartTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.StartTimePicker.Name = "StartTimePicker";
            this.StartTimePicker.ShowUpDown = true;
            // 
            // RecurTextBox
            // 
            this.RecurTextBox.HidePromptOnLeave = true;
            resources.ApplyResources(this.RecurTextBox, "RecurTextBox");
            this.RecurTextBox.Name = "RecurTextBox";
            this.RecurTextBox.ValidatingType = typeof(int);
            this.RecurTextBox.TextChanged += new System.EventHandler(this.RecurTextBox_TextChanged);
            // 
            // SundayCheckBox
            // 
            resources.ApplyResources(this.SundayCheckBox, "SundayCheckBox");
            this.SundayCheckBox.Checked = true;
            this.SundayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SundayCheckBox.Name = "SundayCheckBox";
            this.SundayCheckBox.UseVisualStyleBackColor = true;
            // 
            // MondayCheckBox
            // 
            resources.ApplyResources(this.MondayCheckBox, "MondayCheckBox");
            this.MondayCheckBox.Checked = true;
            this.MondayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MondayCheckBox.Name = "MondayCheckBox";
            this.MondayCheckBox.UseVisualStyleBackColor = true;
            // 
            // TuesdayCheckBox
            // 
            resources.ApplyResources(this.TuesdayCheckBox, "TuesdayCheckBox");
            this.TuesdayCheckBox.Checked = true;
            this.TuesdayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TuesdayCheckBox.Name = "TuesdayCheckBox";
            this.TuesdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // WednesdayCheckBox
            // 
            resources.ApplyResources(this.WednesdayCheckBox, "WednesdayCheckBox");
            this.WednesdayCheckBox.Checked = true;
            this.WednesdayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.WednesdayCheckBox.Name = "WednesdayCheckBox";
            this.WednesdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // ThursdayCheckBox
            // 
            resources.ApplyResources(this.ThursdayCheckBox, "ThursdayCheckBox");
            this.ThursdayCheckBox.Checked = true;
            this.ThursdayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ThursdayCheckBox.Name = "ThursdayCheckBox";
            this.ThursdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // FridayCheckBox
            // 
            resources.ApplyResources(this.FridayCheckBox, "FridayCheckBox");
            this.FridayCheckBox.Checked = true;
            this.FridayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FridayCheckBox.Name = "FridayCheckBox";
            this.FridayCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaturdayCheckBox
            // 
            resources.ApplyResources(this.SaturdayCheckBox, "SaturdayCheckBox");
            this.SaturdayCheckBox.Checked = true;
            this.SaturdayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SaturdayCheckBox.Name = "SaturdayCheckBox";
            this.SaturdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // ServerTimeLabel
            // 
            resources.ApplyResources(this.ServerTimeLabel, "ServerTimeLabel");
            this.ServerTimeLabel.Name = "ServerTimeLabel";
            // 
            // ServerDateLabel
            // 
            resources.ApplyResources(this.ServerDateLabel, "ServerDateLabel");
            this.ServerDateLabel.Name = "ServerDateLabel";
            // 
            // CircleRadioButton
            // 
            resources.ApplyResources(this.CircleRadioButton, "CircleRadioButton");
            this.CircleRadioButton.Name = "CircleRadioButton";
            this.CircleRadioButton.TabStop = true;
            this.CircleRadioButton.UseVisualStyleBackColor = true;
            this.CircleRadioButton.CheckedChanged += new System.EventHandler(this.CircleButtonCheckedChanged);
            // 
            // RecursLabel
            // 
            resources.ApplyResources(this.RecursLabel, "RecursLabel");
            this.RecursLabel.Name = "RecursLabel";
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.radioButtonIncrement);
            this.groupBoxOptions.Controls.Add(this.radioButtonFull);
            resources.ApplyResources(this.groupBoxOptions, "groupBoxOptions");
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.TabStop = false;
            // 
            // radioButtonIncrement
            // 
            resources.ApplyResources(this.radioButtonIncrement, "radioButtonIncrement");
            this.radioButtonIncrement.Checked = true;
            this.radioButtonIncrement.Name = "radioButtonIncrement";
            this.radioButtonIncrement.TabStop = true;
            this.radioButtonIncrement.UseVisualStyleBackColor = true;
            // 
            // radioButtonFull
            // 
            resources.ApplyResources(this.radioButtonFull, "radioButtonFull");
            this.radioButtonFull.Name = "radioButtonFull";
            this.radioButtonFull.UseVisualStyleBackColor = true;
            this.radioButtonFull.CheckedChanged += new System.EventHandler(this.radioButtonFull_CheckedChanged);
            // 
            // expectFullCheckBox
            // 
            resources.ApplyResources(this.expectFullCheckBox, "expectFullCheckBox");
            this.expectFullCheckBox.Name = "expectFullCheckBox";
            this.expectFullCheckBox.UseVisualStyleBackColor = true;
            this.expectFullCheckBox.CheckedChanged += new System.EventHandler(this.expectFullCheckBox_CheckedChanged);
            // 
            // expectFullCountTextBox
            // 
            resources.ApplyResources(this.expectFullCountTextBox, "expectFullCountTextBox");
            this.expectFullCountTextBox.HidePromptOnLeave = true;
            this.expectFullCountTextBox.Name = "expectFullCountTextBox";
            this.expectFullCountTextBox.TextChanged += new System.EventHandler(this.expectFullCountTextBox_TextChanged);
            this.expectFullCountTextBox.Leave += new System.EventHandler(this.expectFullCountTextBox_Leave);
            // 
            // countlabel
            // 
            resources.ApplyResources(this.countlabel, "countlabel");
            this.countlabel.Name = "countlabel";
            // 
            // BackupSchedulePage
            // 
            this.Controls.Add(this.countlabel);
            this.Controls.Add(this.expectFullCountTextBox);
            this.Controls.Add(this.expectFullCheckBox);
            this.Controls.Add(this.groupBoxOptions);
            this.Controls.Add(this.CircleRadioButton);
            this.Controls.Add(this.ServerDateLabel);
            this.Controls.Add(this.ServerTimeLabel);
            this.Controls.Add(this.SaturdayCheckBox);
            this.Controls.Add(this.FridayCheckBox);
            this.Controls.Add(this.ThursdayCheckBox);
            this.Controls.Add(this.WednesdayCheckBox);
            this.Controls.Add(this.TuesdayCheckBox);
            this.Controls.Add(this.MondayCheckBox);
            this.Controls.Add(this.SundayCheckBox);
            this.Controls.Add(this.RecursLabel);
            this.Controls.Add(this.RecurTextBox);
            this.Controls.Add(this.StartTimePicker);
            this.Controls.Add(this.StartDatePicker);
            this.Controls.Add(this.RecurLabel);
            this.Controls.Add(this.StartTimeLabel);
            this.Controls.Add(this.StartDateLabel);
            this.Controls.Add(this.WeeklyRadioButton);
            this.Controls.Add(this.DailyRadioButton);
            this.Controls.Add(this.OnceRadioButton);
            this.Controls.Add(this.NowRadioButton);
            this.Name = "BackupSchedulePage";
            resources.ApplyResources(this, "$this");
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void OnceButtonCheckedChanged(object sender, EventArgs e)
        {
            if (this.OnceRadioButton.Checked)
            {
                this.StrScheduleType = "Once";
                this.ServerTimeLabel.Visible = true;
                this.ServerDateLabel.Visible = true;
                this.StartDateLabel.Visible = true;
                this.StartDatePicker.Visible = true;
                this.StartTimeLabel.Visible = true;
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

                SetGroupBoxOptions(new Point(123, 158), new Size(295, 46), false);
            }
        }

        private void NowButtonCheckedChanged(object sender, EventArgs e)
        {
            if (this.NowRadioButton.Checked)
            {
                this.StrScheduleType = "Now";
                this.ServerTimeLabel.Visible = true;
                this.ServerDateLabel.Visible = true;
                this.StartDateLabel.Visible = false;
                this.StartDatePicker.Visible = false;
                this.StartTimeLabel.Visible = false;
                this.StartTimePicker.Visible = false;
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
                SetGroupBoxOptions(new Point(123, 86), new Size(295, 46), false);
            }
        }

        

        private void DailyButtonCheckedChanged(object sender, EventArgs e)
        {
            if (this.DailyRadioButton.Checked)
            {
                this.RecursLabel.Text = Messages.BACKUP_RECUR_TEXT_DAYS;
                this.StrScheduleType = "Daily";
                this.ServerTimeLabel.Visible = true;
                this.ServerDateLabel.Visible = true;
                this.StartDateLabel.Visible = true;
                this.StartDatePicker.Visible = true;
                this.StartTimeLabel.Visible = true;
                this.StartTimePicker.Visible = true;
                //this.DNSACheckBox.Visible = true;
                //this.DNSATimePicker.Visible = true;
                this.RecurLabel.Visible = true;
                this.RecurTextBox.Visible = true;
                this.RecursLabel.Visible = true;
                //this.RecursLabel.Text = "Days";
                this.SundayCheckBox.Visible = false;
                this.MondayCheckBox.Visible = false;
                this.TuesdayCheckBox.Visible = false;
                this.WednesdayCheckBox.Visible = false;
                this.ThursdayCheckBox.Visible = false;
                this.FridayCheckBox.Visible = false;
                this.SaturdayCheckBox.Visible = false;
                SetGroupBoxOptions(new Point(123, 193), new Size(295, 46), true);

            }
        }

        private void WeeklyButtonCheckedChanged(object sender, EventArgs e)
        {
            if (this.WeeklyRadioButton.Checked)
            {
                this.RecursLabel.Text = Messages.BACKUP_RECUR_TEXT_WEEKS;
                this.StrScheduleType = "Weekly";
                this.ServerTimeLabel.Visible = true;
                this.ServerDateLabel.Visible = true;
                this.StartDateLabel.Visible = true;
                this.StartDatePicker.Visible = true;
                this.StartTimeLabel.Visible = true;
                this.StartTimePicker.Visible = true;
                //this.DNSACheckBox.Visible = true;
                //this.DNSATimePicker.Visible = true;
                this.RecurLabel.Visible = true;
                this.RecurTextBox.Visible = true;
                this.RecursLabel.Visible = true;
                //this.RecursLabel.Text = "Weeks";
                this.SundayCheckBox.Visible = true;
                this.MondayCheckBox.Visible = true;
                this.TuesdayCheckBox.Visible = true;
                this.WednesdayCheckBox.Visible = true;
                this.ThursdayCheckBox.Visible = true;
                this.FridayCheckBox.Visible = true;
                this.SaturdayCheckBox.Visible = true;
                SetGroupBoxOptions(new Point(123, 263), new Size(295, 46), true);
            }
        }

        private void CircleButtonCheckedChanged(object sender, EventArgs e)
        {
            this.StrScheduleType = "Circle";
            this.ServerTimeLabel.Visible = true;
            this.ServerDateLabel.Visible = true;
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
            SetGroupBoxOptions(new Point(123, 193), new Size(295, 46), true);
        }
        /*
         * ONLY Increment Increment FULL , NO  FULL FULL FULL 
         */
        private void SetGroupBoxOptions(Point point,Size size, bool visible)
        {
            if (visible)
            {
                this.groupBoxOptions.Visible = false;
                this.groupBoxOptions.Location = point;
                this.groupBoxOptions.Size = size;
                this.radioButtonIncrement.Checked = true;
                this.radioButtonFull.Enabled = false;
                this.expectFullCheckBox.Visible = true;
                this.expectFullCountTextBox.Visible = true;
                this.countlabel.Visible = true;
                this.expectFullCheckBox.Location = new Point(this.expectFullCheckBox.Location.X, point.Y); ;
                this.expectFullCountTextBox.Location = new Point(this.expectFullCountTextBox.Location.X, point.Y);
                this.countlabel.Location = new Point(this.countlabel.Location.X, point.Y); 
            }
            else {
                this.groupBoxOptions.Location = point;
                this.groupBoxOptions.Size = size;
                this.groupBoxOptions.Visible = true;
                this.expectFullCountTextBox.Visible = false;
                this.expectFullCheckBox.Visible = false;
                this.countlabel.Visible = false;
                this.radioButtonFull.Enabled = true;
                
            }
        }
        string pattern = @"^[0-9]*[1-9][0-9]*$";
        private void RecurTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.CircleRadioButton.Checked == true)
            {
                if (Regex.IsMatch(RecurTextBox.Text.ToString(), pattern))
                {
                    if (Int32.Parse(this.RecurTextBox.Text) / 24 > 0)
                    {
                        this.RecursLabel.Text = Int32.Parse(this.RecurTextBox.Text) / 24 + Messages.BACKUP_RECUR_TEXT_DAYS + Int32.Parse(this.RecurTextBox.Text) % 24 + Messages.BACKUP_RECUR_TEXT_HOURS;
                    }
                    else
                    {
                        this.RecursLabel.Text = Int32.Parse(this.RecurTextBox.Text) % 24 + Messages.BACKUP_RECUR_TEXT_HOURS;
                    }
                }
            }
        }

        private void radioButtonFull_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void expectFullCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.expectFullCheckBox.Checked)
            {
                this.expectFullCountTextBox.Enabled = true;
            }
            else
            {
                this.expectFullCountTextBox.Enabled = false;
            }
        }

        private void expectFullCountTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.expectFullCountTextBox.Text!="")
            {
                this.expectFullCountTextBox.Text = this.expectFullCountTextBox.Text.Replace(" ", "");
                if(Int32.Parse(this.expectFullCountTextBox.Text) < 1)
                {
                    this.expectFullCountTextBox.Text = "1";
                }
            }
        }

        private void expectFullCountTextBox_Leave(object sender, EventArgs e)
        {
            if (this.expectFullCountTextBox.Text == "" || Int32.Parse(this.expectFullCountTextBox.Text) < 1)
            {
                this.expectFullCountTextBox.Text = "1";
            }
        }

        public override void PageLoaded(PageLoadedDirection direction)
        {
            base.PageLoaded(direction);
            if (direction == PageLoadedDirection.Forward)
            {
                this.ServerDateLabel.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
            }
        }
	}
}
