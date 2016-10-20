namespace XenAdmin.SettingsPanels
{
    partial class PerfmonAlertOptionsPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PerfmonAlertOptionsPage));
            this.EmailAddressLabel = new System.Windows.Forms.Label();
            this.SmtpServerLabel = new System.Windows.Forms.Label();
            this.SmtpServerPortTextBox = new System.Windows.Forms.TextBox();
            this.SmtpPortLabel = new System.Windows.Forms.Label();
            this.SmtpServerAddrTextBox = new System.Windows.Forms.TextBox();
            this.EmailAddressTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new XenAdmin.Controls.DecentGroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CheckButton = new System.Windows.Forms.Button();
            this.groupBox2 = new XenAdmin.Controls.DecentGroupBox();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.MailUserlabel = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.VerifyCheckBox = new System.Windows.Forms.CheckBox();
            this.SendEmailNoteLabel = new System.Windows.Forms.Label();
            this.EmailNotificationCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // EmailAddressLabel
            // 
            resources.ApplyResources(this.EmailAddressLabel, "EmailAddressLabel");
            this.EmailAddressLabel.Name = "EmailAddressLabel";
            // 
            // SmtpServerLabel
            // 
            resources.ApplyResources(this.SmtpServerLabel, "SmtpServerLabel");
            this.SmtpServerLabel.Name = "SmtpServerLabel";
            // 
            // SmtpServerPortTextBox
            // 
            resources.ApplyResources(this.SmtpServerPortTextBox, "SmtpServerPortTextBox");
            this.SmtpServerPortTextBox.Name = "SmtpServerPortTextBox";
            // 
            // SmtpPortLabel
            // 
            resources.ApplyResources(this.SmtpPortLabel, "SmtpPortLabel");
            this.SmtpPortLabel.Name = "SmtpPortLabel";
            // 
            // SmtpServerAddrTextBox
            // 
            resources.ApplyResources(this.SmtpServerAddrTextBox, "SmtpServerAddrTextBox");
            this.SmtpServerAddrTextBox.Name = "SmtpServerAddrTextBox";
            // 
            // EmailAddressTextBox
            // 
            resources.ApplyResources(this.EmailAddressTextBox, "EmailAddressTextBox");
            this.EmailAddressTextBox.Name = "EmailAddressTextBox";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.SmtpPortLabel);
            this.groupBox1.Controls.Add(this.SmtpServerPortTextBox);
            this.groupBox1.Controls.Add(this.EmailAddressTextBox);
            this.groupBox1.Controls.Add(this.EmailAddressLabel);
            this.groupBox1.Controls.Add(this.SmtpServerAddrTextBox);
            this.groupBox1.Controls.Add(this.SmtpServerLabel);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.CheckButton, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.VerifyCheckBox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.SendEmailNoteLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.EmailNotificationCheckBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // CheckButton
            // 
            resources.ApplyResources(this.CheckButton, "CheckButton");
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.verifybutton_click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.UserNameTextBox);
            this.groupBox2.Controls.Add(this.MailUserlabel);
            this.groupBox2.Controls.Add(this.PasswordTextBox);
            this.groupBox2.Controls.Add(this.PasswordLabel);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // UserNameTextBox
            // 
            resources.ApplyResources(this.UserNameTextBox, "UserNameTextBox");
            this.UserNameTextBox.Name = "UserNameTextBox";
            // 
            // MailUserlabel
            // 
            resources.ApplyResources(this.MailUserlabel, "MailUserlabel");
            this.MailUserlabel.Name = "MailUserlabel";
            // 
            // PasswordTextBox
            // 
            resources.ApplyResources(this.PasswordTextBox, "PasswordTextBox");
            this.PasswordTextBox.Name = "PasswordTextBox";
            // 
            // PasswordLabel
            // 
            resources.ApplyResources(this.PasswordLabel, "PasswordLabel");
            this.PasswordLabel.Name = "PasswordLabel";
            // 
            // VerifyCheckBox
            // 
            resources.ApplyResources(this.VerifyCheckBox, "VerifyCheckBox");
            this.VerifyCheckBox.Name = "VerifyCheckBox";
            this.VerifyCheckBox.UseVisualStyleBackColor = false;
            this.VerifyCheckBox.CheckedChanged += new System.EventHandler(this.verifycheckbox_CheckedChanged);
            // 
            // SendEmailNoteLabel
            // 
            resources.ApplyResources(this.SendEmailNoteLabel, "SendEmailNoteLabel");
            this.SendEmailNoteLabel.Name = "SendEmailNoteLabel";
            // 
            // EmailNotificationCheckBox
            // 
            resources.ApplyResources(this.EmailNotificationCheckBox, "EmailNotificationCheckBox");
            this.EmailNotificationCheckBox.Name = "EmailNotificationCheckBox";
            this.EmailNotificationCheckBox.UseVisualStyleBackColor = false;
            this.EmailNotificationCheckBox.CheckedChanged += new System.EventHandler(this.EmailNotificationCheckBox_CheckedChanged);
            // 
            // PerfmonAlertOptionsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PerfmonAlertOptionsPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label EmailAddressLabel;
        private System.Windows.Forms.Label SmtpServerLabel;
        private System.Windows.Forms.TextBox SmtpServerPortTextBox;
        private System.Windows.Forms.Label SmtpPortLabel;
        private System.Windows.Forms.TextBox SmtpServerAddrTextBox;
        private System.Windows.Forms.TextBox EmailAddressTextBox;
        private XenAdmin.Controls.DecentGroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label SendEmailNoteLabel;
        private System.Windows.Forms.CheckBox EmailNotificationCheckBox;
        private System.Windows.Forms.Button CheckButton;
        private Controls.DecentGroupBox groupBox2;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.Label MailUserlabel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.CheckBox VerifyCheckBox;
    }
}
