namespace XenAdmin.Wizards.NewSRWizard_Pages.Frontends
{
    partial class LVMoMirrorSetIscsi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LVMoMirrorSetIscsi));
            this.textBoxIscsiPort = new System.Windows.Forms.TextBox();
            this.labelColon = new System.Windows.Forms.Label();
            this.toolTipContainerIQNscan = new XenAdmin.Controls.ToolTipContainer();
            this.scanTargetHostButton = new System.Windows.Forms.Button();
            this.IscsiUseChapCheckBox = new System.Windows.Forms.CheckBox();
            this.comboBoxIscsiIqns = new System.Windows.Forms.ComboBox();
            this.comboBoxIscsiLuns = new System.Windows.Forms.ComboBox();
            this.errorLabelAtHostname = new System.Windows.Forms.Label();
            this.lunInUseLabel = new System.Windows.Forms.Label();
            this.targetLunLabel = new System.Windows.Forms.Label();
            this.IScsiChapSecretLabel = new System.Windows.Forms.Label();
            this.IScsiChapSecretTextBox = new System.Windows.Forms.TextBox();
            this.labelCHAPuser = new System.Windows.Forms.Label();
            this.IScsiChapUserTextBox = new System.Windows.Forms.TextBox();
            this.labelIscsiTargetHost = new System.Windows.Forms.Label();
            this.labelIscsiIQN = new System.Windows.Forms.Label();
            this.textBoxIscsiHost = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.placeHolderLabel2 = new System.Windows.Forms.Label();
            this.placeholderLabel = new System.Windows.Forms.Label();
            this.errorLabelAtCHAPPassword = new System.Windows.Forms.Label();
            this.errorIconAtCHAPPassword = new System.Windows.Forms.PictureBox();
            this.errorIconAtHostOrIP = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.spinnerIconAtScanTargetHostButton = new XenAdmin.Controls.SpinnerIcon();
            this.iSCSITargetGroupBox = new XenAdmin.Controls.DecentGroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.spinnerIconAtTargetIqn = new XenAdmin.Controls.SpinnerIcon();
            this.spinnerIconAtTargetLun = new XenAdmin.Controls.SpinnerIcon();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.spinnerIcon1 = new XenAdmin.Controls.SpinnerIcon();
            this.spinnerIcon2 = new XenAdmin.Controls.SpinnerIcon();
            this.errorLabelAtTargetLUN = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorIconAtCHAPPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorIconAtHostOrIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtScanTargetHostButton)).BeginInit();
            this.iSCSITargetGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtTargetIqn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtTargetLun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIcon1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIcon2)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxIscsiPort
            // 
            resources.ApplyResources(this.textBoxIscsiPort, "textBoxIscsiPort");
            this.textBoxIscsiPort.Name = "textBoxIscsiPort";
            this.textBoxIscsiPort.TextChanged += new System.EventHandler(this.textBoxIscsiHost_TextChanged);
            // 
            // labelColon
            // 
            resources.ApplyResources(this.labelColon, "labelColon");
            this.labelColon.Name = "labelColon";
            // 
            // toolTipContainerIQNscan
            // 
            resources.ApplyResources(this.toolTipContainerIQNscan, "toolTipContainerIQNscan");
            this.toolTipContainerIQNscan.Name = "toolTipContainerIQNscan";
            // 
            // scanTargetHostButton
            // 
            resources.ApplyResources(this.scanTargetHostButton, "scanTargetHostButton");
            this.tableLayoutPanel1.SetColumnSpan(this.scanTargetHostButton, 2);
            this.scanTargetHostButton.Name = "scanTargetHostButton";
            this.scanTargetHostButton.Click += new System.EventHandler(this.scanTargetHostButton_Click);
            // 
            // IscsiUseChapCheckBox
            // 
            resources.ApplyResources(this.IscsiUseChapCheckBox, "IscsiUseChapCheckBox");
            this.tableLayoutPanel1.SetColumnSpan(this.IscsiUseChapCheckBox, 2);
            this.IscsiUseChapCheckBox.Name = "IscsiUseChapCheckBox";
            this.IscsiUseChapCheckBox.UseVisualStyleBackColor = true;
            this.IscsiUseChapCheckBox.CheckedChanged += new System.EventHandler(this.IscsiUseChapCheckBox_CheckedChanged);
            // 
            // comboBoxIscsiIqns
            // 
            resources.ApplyResources(this.comboBoxIscsiIqns, "comboBoxIscsiIqns");
            this.tableLayoutPanel2.SetColumnSpan(this.comboBoxIscsiIqns, 2);
            this.comboBoxIscsiIqns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIscsiIqns.FormattingEnabled = true;
            this.comboBoxIscsiIqns.Name = "comboBoxIscsiIqns";
            this.comboBoxIscsiIqns.SelectedIndexChanged += new System.EventHandler(this.IScsiTargetIqnComboBox_SelectedIndexChanged);
            // 
            // comboBoxIscsiLuns
            // 
            resources.ApplyResources(this.comboBoxIscsiLuns, "comboBoxIscsiLuns");
            this.tableLayoutPanel2.SetColumnSpan(this.comboBoxIscsiLuns, 2);
            this.comboBoxIscsiLuns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIscsiLuns.FormattingEnabled = true;
            this.comboBoxIscsiLuns.Name = "comboBoxIscsiLuns";
            this.comboBoxIscsiLuns.SelectedIndexChanged += new System.EventHandler(this.comboBoxIscsiLuns_SelectedIndexChanged);
            // 
            // errorLabelAtHostname
            // 
            resources.ApplyResources(this.errorLabelAtHostname, "errorLabelAtHostname");
            this.tableLayoutPanel1.SetColumnSpan(this.errorLabelAtHostname, 3);
            this.errorLabelAtHostname.ForeColor = System.Drawing.Color.Red;
            this.errorLabelAtHostname.Name = "errorLabelAtHostname";
            // 
            // lunInUseLabel
            // 
            resources.ApplyResources(this.lunInUseLabel, "lunInUseLabel");
            this.lunInUseLabel.Name = "lunInUseLabel";
            // 
            // targetLunLabel
            // 
            resources.ApplyResources(this.targetLunLabel, "targetLunLabel");
            this.targetLunLabel.Name = "targetLunLabel";
            // 
            // IScsiChapSecretLabel
            // 
            resources.ApplyResources(this.IScsiChapSecretLabel, "IScsiChapSecretLabel");
            this.IScsiChapSecretLabel.BackColor = System.Drawing.Color.Transparent;
            this.IScsiChapSecretLabel.Name = "IScsiChapSecretLabel";
            // 
            // IScsiChapSecretTextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.IScsiChapSecretTextBox, 2);
            resources.ApplyResources(this.IScsiChapSecretTextBox, "IScsiChapSecretTextBox");
            this.IScsiChapSecretTextBox.Name = "IScsiChapSecretTextBox";
            this.IScsiChapSecretTextBox.UseSystemPasswordChar = true;
            this.IScsiChapSecretTextBox.TextChanged += new System.EventHandler(this.ChapSettings_Changed);
            // 
            // labelCHAPuser
            // 
            resources.ApplyResources(this.labelCHAPuser, "labelCHAPuser");
            this.labelCHAPuser.BackColor = System.Drawing.Color.Transparent;
            this.labelCHAPuser.Name = "labelCHAPuser";
            // 
            // IScsiChapUserTextBox
            // 
            this.IScsiChapUserTextBox.AllowDrop = true;
            this.tableLayoutPanel1.SetColumnSpan(this.IScsiChapUserTextBox, 2);
            resources.ApplyResources(this.IScsiChapUserTextBox, "IScsiChapUserTextBox");
            this.IScsiChapUserTextBox.Name = "IScsiChapUserTextBox";
            this.IScsiChapUserTextBox.TextChanged += new System.EventHandler(this.ChapSettings_Changed);
            // 
            // labelIscsiTargetHost
            // 
            resources.ApplyResources(this.labelIscsiTargetHost, "labelIscsiTargetHost");
            this.labelIscsiTargetHost.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.labelIscsiTargetHost, 2);
            this.labelIscsiTargetHost.Name = "labelIscsiTargetHost";
            // 
            // labelIscsiIQN
            // 
            resources.ApplyResources(this.labelIscsiIQN, "labelIscsiIQN");
            this.labelIscsiIQN.BackColor = System.Drawing.Color.Transparent;
            this.labelIscsiIQN.Name = "labelIscsiIQN";
            // 
            // textBoxIscsiHost
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxIscsiHost, 2);
            resources.ApplyResources(this.textBoxIscsiHost, "textBoxIscsiHost");
            this.textBoxIscsiHost.Name = "textBoxIscsiHost";
            this.textBoxIscsiHost.TextChanged += new System.EventHandler(this.textBoxIscsiHost_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.placeHolderLabel2, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.placeholderLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.errorLabelAtCHAPPassword, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.errorIconAtCHAPPassword, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.errorIconAtHostOrIP, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.scanTargetHostButton, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.labelIscsiTargetHost, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxIscsiHost, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelColon, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxIscsiPort, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.IScsiChapUserTextBox, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.IScsiChapSecretTextBox, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.labelCHAPuser, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.IScsiChapSecretLabel, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.IscsiUseChapCheckBox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.errorLabelAtHostname, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.spinnerIconAtScanTargetHostButton, 2, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // placeHolderLabel2
            // 
            resources.ApplyResources(this.placeHolderLabel2, "placeHolderLabel2");
            this.placeHolderLabel2.ForeColor = System.Drawing.Color.Red;
            this.placeHolderLabel2.Name = "placeHolderLabel2";
            // 
            // placeholderLabel
            // 
            resources.ApplyResources(this.placeholderLabel, "placeholderLabel");
            this.placeholderLabel.ForeColor = System.Drawing.Color.Red;
            this.placeholderLabel.Name = "placeholderLabel";
            // 
            // errorLabelAtCHAPPassword
            // 
            resources.ApplyResources(this.errorLabelAtCHAPPassword, "errorLabelAtCHAPPassword");
            this.tableLayoutPanel1.SetColumnSpan(this.errorLabelAtCHAPPassword, 3);
            this.errorLabelAtCHAPPassword.ForeColor = System.Drawing.Color.Red;
            this.errorLabelAtCHAPPassword.Name = "errorLabelAtCHAPPassword";
            // 
            // errorIconAtCHAPPassword
            // 
            resources.ApplyResources(this.errorIconAtCHAPPassword, "errorIconAtCHAPPassword");
            this.errorIconAtCHAPPassword.Name = "errorIconAtCHAPPassword";
            this.errorIconAtCHAPPassword.TabStop = false;
            // 
            // errorIconAtHostOrIP
            // 
            resources.ApplyResources(this.errorIconAtHostOrIP, "errorIconAtHostOrIP");
            this.errorIconAtHostOrIP.Name = "errorIconAtHostOrIP";
            this.errorIconAtHostOrIP.TabStop = false;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.tableLayoutPanel1.SetColumnSpan(this.label11, 6);
            this.label11.Name = "label11";
            // 
            // spinnerIconAtScanTargetHostButton
            // 
            resources.ApplyResources(this.spinnerIconAtScanTargetHostButton, "spinnerIconAtScanTargetHostButton");
            this.spinnerIconAtScanTargetHostButton.Name = "spinnerIconAtScanTargetHostButton";
            this.spinnerIconAtScanTargetHostButton.SucceededImage = global::XenAdmin.Properties.Resources._000_Tick_h32bit_16;
            this.spinnerIconAtScanTargetHostButton.TabStop = false;
            // 
            // iSCSITargetGroupBox
            // 
            resources.ApplyResources(this.iSCSITargetGroupBox, "iSCSITargetGroupBox");
            this.iSCSITargetGroupBox.Controls.Add(this.errorLabelAtTargetLUN);
            this.iSCSITargetGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.iSCSITargetGroupBox.Name = "iSCSITargetGroupBox";
            this.iSCSITargetGroupBox.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.spinnerIcon2, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.labelIscsiIQN, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxIscsiIqns, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxIscsiLuns, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.targetLunLabel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.spinnerIconAtTargetIqn, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.spinnerIconAtTargetLun, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboBox2, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.comboBox1, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.spinnerIcon1, 3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // spinnerIconAtTargetIqn
            // 
            resources.ApplyResources(this.spinnerIconAtTargetIqn, "spinnerIconAtTargetIqn");
            this.spinnerIconAtTargetIqn.Name = "spinnerIconAtTargetIqn";
            this.spinnerIconAtTargetIqn.SucceededImage = global::XenAdmin.Properties.Resources._000_Tick_h32bit_16;
            this.spinnerIconAtTargetIqn.TabStop = false;
            // 
            // spinnerIconAtTargetLun
            // 
            resources.ApplyResources(this.spinnerIconAtTargetLun, "spinnerIconAtTargetLun");
            this.spinnerIconAtTargetLun.Name = "spinnerIconAtTargetLun";
            this.spinnerIconAtTargetLun.SucceededImage = global::XenAdmin.Properties.Resources._000_Tick_h32bit_16;
            this.spinnerIconAtTargetLun.TabStop = false;
            // 
            // comboBox1
            // 
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.tableLayoutPanel2.SetColumnSpan(this.comboBox1, 2);
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Name = "comboBox1";
            // 
            // comboBox2
            // 
            resources.ApplyResources(this.comboBox2, "comboBox2");
            this.tableLayoutPanel2.SetColumnSpan(this.comboBox2, 2);
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Name = "comboBox2";
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
            // spinnerIcon1
            // 
            resources.ApplyResources(this.spinnerIcon1, "spinnerIcon1");
            this.spinnerIcon1.Name = "spinnerIcon1";
            this.spinnerIcon1.SucceededImage = global::XenAdmin.Properties.Resources._000_Tick_h32bit_16;
            this.spinnerIcon1.TabStop = false;
            // 
            // spinnerIcon2
            // 
            resources.ApplyResources(this.spinnerIcon2, "spinnerIcon2");
            this.spinnerIcon2.Name = "spinnerIcon2";
            this.spinnerIcon2.SucceededImage = global::XenAdmin.Properties.Resources._000_Tick_h32bit_16;
            this.spinnerIcon2.TabStop = false;
            // 
            // errorLabelAtTargetLUN
            // 
            resources.ApplyResources(this.errorLabelAtTargetLUN, "errorLabelAtTargetLUN");
            this.errorLabelAtTargetLUN.ForeColor = System.Drawing.Color.Red;
            this.errorLabelAtTargetLUN.Name = "errorLabelAtTargetLUN";
            // 
            // LVMoMirrorSetIscsi
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.iSCSITargetGroupBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolTipContainerIQNscan);
            this.Name = "LVMoMirrorSetIscsi";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorIconAtCHAPPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorIconAtHostOrIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtScanTargetHostButton)).EndInit();
            this.iSCSITargetGroupBox.ResumeLayout(false);
            this.iSCSITargetGroupBox.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtTargetIqn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtTargetLun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIcon1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIcon2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxIscsiPort;
        private System.Windows.Forms.Label labelColon;
        private XenAdmin.Controls.ToolTipContainer toolTipContainerIQNscan;
        private System.Windows.Forms.Button scanTargetHostButton;
        private System.Windows.Forms.CheckBox IscsiUseChapCheckBox;
        private System.Windows.Forms.ComboBox comboBoxIscsiIqns;
        private System.Windows.Forms.ComboBox comboBoxIscsiLuns;
        private System.Windows.Forms.Label errorLabelAtHostname;
        private System.Windows.Forms.Label lunInUseLabel;
        private System.Windows.Forms.Label targetLunLabel;
        private System.Windows.Forms.Label IScsiChapSecretLabel;
        private System.Windows.Forms.TextBox IScsiChapSecretTextBox;
        private System.Windows.Forms.Label labelCHAPuser;
        private System.Windows.Forms.TextBox IScsiChapUserTextBox;
        private System.Windows.Forms.Label labelIscsiTargetHost;
        private System.Windows.Forms.Label labelIscsiIQN;
        private System.Windows.Forms.TextBox textBoxIscsiHost;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private XenAdmin.Controls.DecentGroupBox iSCSITargetGroupBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox errorIconAtHostOrIP;
        private System.Windows.Forms.PictureBox errorIconAtCHAPPassword;
        private System.Windows.Forms.Label errorLabelAtCHAPPassword;
        private XenAdmin.Controls.SpinnerIcon spinnerIconAtTargetIqn;
        private XenAdmin.Controls.SpinnerIcon spinnerIconAtTargetLun;
        private XenAdmin.Controls.SpinnerIcon spinnerIconAtScanTargetHostButton;
        private System.Windows.Forms.Label placeholderLabel;
        private System.Windows.Forms.Label placeHolderLabel2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Controls.SpinnerIcon spinnerIcon2;
        private Controls.SpinnerIcon spinnerIcon1;
        private System.Windows.Forms.Label errorLabelAtTargetLUN;
    }
}
