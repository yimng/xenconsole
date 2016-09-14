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
using XenAdmin.Controls;

namespace XenAdmin.Wizards.BackupWizard_Pages
{
	public class BackupOptionsPage: XenTabPage
	{
        private Label label1;
        private TextBox jobNameTextBox;
        private GroupBox groupBox1;
        private CheckBox checkBox1;
        private ComboBox VerifyBackupComboBox;
        private CheckBox checkBox4;
        private CheckBox checkBox3;
        private CheckBox checkBox2;
        private GroupBox groupBoxOptions;
        private RadioButton radioButtonIncrement;
        private RadioButton radioButtonFull;
        private Label label2;

        public override string Text
        {
            get
            {
                return Messages.OPTION_PAGE;
            }
        }

        public override string PageTitle
        {
            get
            {
                return Messages.OPTION_PAGE;
            }
        }

		public BackupOptionsPage()
		{
			InitializeComponent();
		}

        public void JobNameTextBox(String text)
        {
            this.jobNameTextBox.Text = text;

        }

        public String _JobNameTextBox 
        {
            set
            {
                this.jobNameTextBox.Text = value;
            }
            get
            {
                return this.jobNameTextBox.Text;
            }            
        }

	    public bool IsFullBackup
	    {
	        get { return this.radioButtonFull.Checked; }
	    }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupOptionsPage));
            this.label1 = new System.Windows.Forms.Label();
            this.jobNameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.VerifyBackupComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.radioButtonIncrement = new System.Windows.Forms.RadioButton();
            this.radioButtonFull = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // jobNameTextBox
            // 
            resources.ApplyResources(this.jobNameTextBox, "jobNameTextBox");
            this.jobNameTextBox.Name = "jobNameTextBox";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.VerifyBackupComboBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // VerifyBackupComboBox
            // 
            resources.ApplyResources(this.VerifyBackupComboBox, "VerifyBackupComboBox");
            this.VerifyBackupComboBox.FormattingEnabled = true;
            this.VerifyBackupComboBox.Name = "VerifyBackupComboBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBoxOptions
            // 
            resources.ApplyResources(this.groupBoxOptions, "groupBoxOptions");
            this.groupBoxOptions.Controls.Add(this.radioButtonIncrement);
            this.groupBoxOptions.Controls.Add(this.radioButtonFull);
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
            // 
            // BackupOptionsPage
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupBoxOptions);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.jobNameTextBox);
            this.Controls.Add(this.label1);
            this.Name = "BackupOptionsPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
	}
}
