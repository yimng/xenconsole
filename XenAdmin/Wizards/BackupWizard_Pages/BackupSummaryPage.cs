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
	public partial class BackupSummaryPage: XenTabPage
	{
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label JobNameLabel;
        private Label ScheduleTypeLabel;
        private Label DataSizeLabel;
        private Label OptionsLabel;
    
		public BackupSummaryPage()
		{
			InitializeComponent();
		}

        public override string Text
        {
            get
            {
                return Messages.SUMMARY_PAGE;
            }
        }

        public override string PageTitle
        {
            get
            {
                return Messages.SUMMARY_PAGE;
            }
        }


        public void _JobNameLable(String text)
        {
            this.JobNameLabel.Text = text;
        }

        public void _ScheduleTypeLabel(String text)
        {
            this.ScheduleTypeLabel.Text = text;
        }

	    public string DataSize
	    {
	        set { this.DataSizeLabel.Text = value; }
	    }

	    public  string Options
	    {
            set { this.OptionsLabel.Text = value; }
	    }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupSummaryPage));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.JobNameLabel = new System.Windows.Forms.Label();
            this.ScheduleTypeLabel = new System.Windows.Forms.Label();
            this.DataSizeLabel = new System.Windows.Forms.Label();
            this.OptionsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // JobNameLabel
            // 
            resources.ApplyResources(this.JobNameLabel, "JobNameLabel");
            this.JobNameLabel.Name = "JobNameLabel";
            // 
            // ScheduleTypeLabel
            // 
            resources.ApplyResources(this.ScheduleTypeLabel, "ScheduleTypeLabel");
            this.ScheduleTypeLabel.Name = "ScheduleTypeLabel";
            // 
            // DataSizeLabel
            // 
            resources.ApplyResources(this.DataSizeLabel, "DataSizeLabel");
            this.DataSizeLabel.Name = "DataSizeLabel";
            // 
            // OptionsLabel
            // 
            resources.ApplyResources(this.OptionsLabel, "OptionsLabel");
            this.OptionsLabel.Name = "OptionsLabel";
            // 
            // BackupSummaryPage
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.OptionsLabel);
            this.Controls.Add(this.DataSizeLabel);
            this.Controls.Add(this.ScheduleTypeLabel);
            this.Controls.Add(this.JobNameLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BackupSummaryPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
	}
}
