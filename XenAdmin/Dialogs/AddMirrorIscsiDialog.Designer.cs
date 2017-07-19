namespace XenAdmin.Dialogs
{
    partial class AddMirrorIscsiDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddMirrorIscsiDialog));
            this.placeHolderLabel2 = new System.Windows.Forms.Label();
            this.placeholderLabel = new System.Windows.Forms.Label();
            this.errorIconAtHostOrIP = new System.Windows.Forms.PictureBox();
            this.scanTargetHostButton = new System.Windows.Forms.Button();
            this.labelIscsiTargetHost = new System.Windows.Forms.Label();
            this.textBoxIscsiHost = new System.Windows.Forms.TextBox();
            this.labelColon = new System.Windows.Forms.Label();
            this.IScsiChapSecretLabel = new System.Windows.Forms.Label();
            this.IscsiUseChapCheckBox = new System.Windows.Forms.CheckBox();
            this.errorLabelAtHostname = new System.Windows.Forms.Label();
            this.textBoxIscsiPort = new System.Windows.Forms.TextBox();
            this.IScsiChapUserTextBox = new System.Windows.Forms.TextBox();
            this.IScsiChapSecretTextBox = new System.Windows.Forms.TextBox();
            this.errorLabelAtCHAPPassword = new System.Windows.Forms.Label();
            this.labelCHAPuser = new System.Windows.Forms.Label();
            this.errorIconAtCHAPPassword = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.spinnerIconAtScanTargetHostButton = new XenAdmin.Controls.SpinnerIcon();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolTipContainerIQNscan = new XenAdmin.Controls.ToolTipContainer();
            this.lunInUseLabel = new System.Windows.Forms.Label();
            this.iSCSITargetGroupBox = new XenAdmin.Controls.DecentGroupBox();
            this.Lun_Size_Different = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelIscsiIQN = new System.Windows.Forms.Label();
            this.comboBoxIscsiIqns = new System.Windows.Forms.ComboBox();
            this.spinnerIconAtTargetIqn = new XenAdmin.Controls.SpinnerIcon();
            this.spinnerIconAtTargetLun = new XenAdmin.Controls.SpinnerIcon();
            this.comboBoxLun = new System.Windows.Forms.ComboBox();
            this.errorLabelAtTargetLUN = new System.Windows.Forms.Label();
            this.errorIconAtTargetLUN = new System.Windows.Forms.PictureBox();
            this.Lun = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorIconAtHostOrIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorIconAtCHAPPassword)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtScanTargetHostButton)).BeginInit();
            this.toolTipContainerIQNscan.SuspendLayout();
            this.iSCSITargetGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtTargetIqn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtTargetLun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorIconAtTargetLUN)).BeginInit();
            this.SuspendLayout();
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
            // errorIconAtHostOrIP
            // 
            resources.ApplyResources(this.errorIconAtHostOrIP, "errorIconAtHostOrIP");
            this.errorIconAtHostOrIP.Name = "errorIconAtHostOrIP";
            this.errorIconAtHostOrIP.TabStop = false;
            // 
            // scanTargetHostButton
            // 
            resources.ApplyResources(this.scanTargetHostButton, "scanTargetHostButton");
            this.tableLayoutPanel1.SetColumnSpan(this.scanTargetHostButton, 2);
            this.scanTargetHostButton.Name = "scanTargetHostButton";
            this.scanTargetHostButton.Click += new System.EventHandler(this.scanTargetHostButton_Click);
            // 
            // labelIscsiTargetHost
            // 
            resources.ApplyResources(this.labelIscsiTargetHost, "labelIscsiTargetHost");
            this.labelIscsiTargetHost.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.labelIscsiTargetHost, 2);
            this.labelIscsiTargetHost.Name = "labelIscsiTargetHost";
            // 
            // textBoxIscsiHost
            // 
            resources.ApplyResources(this.textBoxIscsiHost, "textBoxIscsiHost");
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxIscsiHost, 2);
            this.textBoxIscsiHost.Name = "textBoxIscsiHost";
            this.textBoxIscsiHost.TextChanged += new System.EventHandler(this.textBoxIscsiHost_TextChanged);
            // 
            // labelColon
            // 
            resources.ApplyResources(this.labelColon, "labelColon");
            this.labelColon.Name = "labelColon";
            // 
            // IScsiChapSecretLabel
            // 
            resources.ApplyResources(this.IScsiChapSecretLabel, "IScsiChapSecretLabel");
            this.IScsiChapSecretLabel.BackColor = System.Drawing.Color.Transparent;
            this.IScsiChapSecretLabel.Name = "IScsiChapSecretLabel";
            // 
            // IscsiUseChapCheckBox
            // 
            resources.ApplyResources(this.IscsiUseChapCheckBox, "IscsiUseChapCheckBox");
            this.tableLayoutPanel1.SetColumnSpan(this.IscsiUseChapCheckBox, 2);
            this.IscsiUseChapCheckBox.Name = "IscsiUseChapCheckBox";
            this.IscsiUseChapCheckBox.UseVisualStyleBackColor = true;
            this.IscsiUseChapCheckBox.CheckedChanged += new System.EventHandler(this.IscsiUseChapCheckBox_CheckedChanged);
            // 
            // errorLabelAtHostname
            // 
            resources.ApplyResources(this.errorLabelAtHostname, "errorLabelAtHostname");
            this.tableLayoutPanel1.SetColumnSpan(this.errorLabelAtHostname, 3);
            this.errorLabelAtHostname.ForeColor = System.Drawing.Color.Red;
            this.errorLabelAtHostname.Name = "errorLabelAtHostname";
            // 
            // textBoxIscsiPort
            // 
            resources.ApplyResources(this.textBoxIscsiPort, "textBoxIscsiPort");
            this.textBoxIscsiPort.Name = "textBoxIscsiPort";
            // 
            // IScsiChapUserTextBox
            // 
            resources.ApplyResources(this.IScsiChapUserTextBox, "IScsiChapUserTextBox");
            this.IScsiChapUserTextBox.AllowDrop = true;
            this.tableLayoutPanel1.SetColumnSpan(this.IScsiChapUserTextBox, 2);
            this.IScsiChapUserTextBox.Name = "IScsiChapUserTextBox";
            // 
            // IScsiChapSecretTextBox
            // 
            resources.ApplyResources(this.IScsiChapSecretTextBox, "IScsiChapSecretTextBox");
            this.tableLayoutPanel1.SetColumnSpan(this.IScsiChapSecretTextBox, 2);
            this.IScsiChapSecretTextBox.Name = "IScsiChapSecretTextBox";
            this.IScsiChapSecretTextBox.UseSystemPasswordChar = true;
            // 
            // errorLabelAtCHAPPassword
            // 
            resources.ApplyResources(this.errorLabelAtCHAPPassword, "errorLabelAtCHAPPassword");
            this.errorLabelAtCHAPPassword.Name = "errorLabelAtCHAPPassword";
            // 
            // labelCHAPuser
            // 
            resources.ApplyResources(this.labelCHAPuser, "labelCHAPuser");
            this.labelCHAPuser.BackColor = System.Drawing.Color.Transparent;
            this.labelCHAPuser.Name = "labelCHAPuser";
            // 
            // errorIconAtCHAPPassword
            // 
            resources.ApplyResources(this.errorIconAtCHAPPassword, "errorIconAtCHAPPassword");
            this.errorIconAtCHAPPassword.Name = "errorIconAtCHAPPassword";
            this.errorIconAtCHAPPassword.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.placeHolderLabel2, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.placeholderLabel, 0, 2);
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
            this.tableLayoutPanel1.Controls.Add(this.errorLabelAtCHAPPassword, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.errorIconAtCHAPPassword, 2, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // spinnerIconAtScanTargetHostButton
            // 
            resources.ApplyResources(this.spinnerIconAtScanTargetHostButton, "spinnerIconAtScanTargetHostButton");
            this.spinnerIconAtScanTargetHostButton.Name = "spinnerIconAtScanTargetHostButton";
            this.spinnerIconAtScanTargetHostButton.SucceededImage = global::XenAdmin.Properties.Resources._000_Tick_h32bit_16;
            this.spinnerIconAtScanTargetHostButton.TabStop = false;
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // toolTipContainerIQNscan
            // 
            resources.ApplyResources(this.toolTipContainerIQNscan, "toolTipContainerIQNscan");
            this.toolTipContainerIQNscan.Controls.Add(this.lunInUseLabel);
            this.toolTipContainerIQNscan.Name = "toolTipContainerIQNscan";
            // 
            // lunInUseLabel
            // 
            resources.ApplyResources(this.lunInUseLabel, "lunInUseLabel");
            this.lunInUseLabel.Name = "lunInUseLabel";
            // 
            // iSCSITargetGroupBox
            // 
            resources.ApplyResources(this.iSCSITargetGroupBox, "iSCSITargetGroupBox");
            this.iSCSITargetGroupBox.Controls.Add(this.Lun_Size_Different);
            this.iSCSITargetGroupBox.Controls.Add(this.label1);
            this.iSCSITargetGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.iSCSITargetGroupBox.Controls.Add(this.errorLabelAtTargetLUN);
            this.iSCSITargetGroupBox.Controls.Add(this.errorIconAtTargetLUN);
            this.iSCSITargetGroupBox.Name = "iSCSITargetGroupBox";
            this.iSCSITargetGroupBox.TabStop = false;
            // 
            // Lun_Size_Different
            // 
            resources.ApplyResources(this.Lun_Size_Different, "Lun_Size_Different");
            this.Lun_Size_Different.ForeColor = System.Drawing.Color.Red;
            this.Lun_Size_Different.Name = "Lun_Size_Different";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Name = "label1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.labelIscsiIQN, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxIscsiIqns, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.spinnerIconAtTargetIqn, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.spinnerIconAtTargetLun, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxLun, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.Lun, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // labelIscsiIQN
            // 
            resources.ApplyResources(this.labelIscsiIQN, "labelIscsiIQN");
            this.labelIscsiIQN.BackColor = System.Drawing.Color.Transparent;
            this.labelIscsiIQN.Name = "labelIscsiIQN";
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
            // comboBoxLun
            // 
            resources.ApplyResources(this.comboBoxLun, "comboBoxLun");
            this.tableLayoutPanel2.SetColumnSpan(this.comboBoxLun, 2);
            this.comboBoxLun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLun.FormattingEnabled = true;
            this.comboBoxLun.Name = "comboBoxLun";
            this.comboBoxLun.SelectedIndexChanged += new System.EventHandler(this.comboBoxLun_SelectedIndexChanged);
            // 
            // errorLabelAtTargetLUN
            // 
            resources.ApplyResources(this.errorLabelAtTargetLUN, "errorLabelAtTargetLUN");
            this.errorLabelAtTargetLUN.ForeColor = System.Drawing.Color.Red;
            this.errorLabelAtTargetLUN.Name = "errorLabelAtTargetLUN";
            // 
            // errorIconAtTargetLUN
            // 
            resources.ApplyResources(this.errorIconAtTargetLUN, "errorIconAtTargetLUN");
            this.errorIconAtTargetLUN.Name = "errorIconAtTargetLUN";
            this.errorIconAtTargetLUN.TabStop = false;
            // 
            // Lun
            // 
            resources.ApplyResources(this.Lun, "Lun");
            this.Lun.Name = "Lun";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.tableLayoutPanel1.SetColumnSpan(this.label11, 6);
            this.label11.Name = "label11";
            // 
            // AddMirrorIscsiDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.toolTipContainerIQNscan);
            this.Controls.Add(this.iSCSITargetGroupBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AddMirrorIscsiDialog";
            ((System.ComponentModel.ISupportInitialize)(this.errorIconAtHostOrIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorIconAtCHAPPassword)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtScanTargetHostButton)).EndInit();
            this.toolTipContainerIQNscan.ResumeLayout(false);
            this.iSCSITargetGroupBox.ResumeLayout(false);
            this.iSCSITargetGroupBox.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtTargetIqn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinnerIconAtTargetLun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorIconAtTargetLUN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label placeHolderLabel2;
        private System.Windows.Forms.Label placeholderLabel;
        private System.Windows.Forms.PictureBox errorIconAtHostOrIP;
        private System.Windows.Forms.Button scanTargetHostButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelIscsiTargetHost;
        private System.Windows.Forms.TextBox textBoxIscsiHost;
        private System.Windows.Forms.Label labelColon;
        private System.Windows.Forms.TextBox textBoxIscsiPort;
        private System.Windows.Forms.TextBox IScsiChapUserTextBox;
        private System.Windows.Forms.TextBox IScsiChapSecretTextBox;
        private System.Windows.Forms.Label labelCHAPuser;
        private System.Windows.Forms.Label IScsiChapSecretLabel;
        private System.Windows.Forms.CheckBox IscsiUseChapCheckBox;
        private System.Windows.Forms.Label errorLabelAtHostname;
        private Controls.SpinnerIcon spinnerIconAtScanTargetHostButton;
        private System.Windows.Forms.Label errorLabelAtCHAPPassword;
        private System.Windows.Forms.PictureBox errorIconAtCHAPPassword;
        private Controls.ToolTipContainer toolTipContainerIQNscan;
        private Controls.SpinnerIcon spinnerIconAtTargetIqn;
        private System.Windows.Forms.Label Lun_Size_Different;
        private Controls.DecentGroupBox iSCSITargetGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelIscsiIQN;
        private System.Windows.Forms.ComboBox comboBoxIscsiIqns;
        private System.Windows.Forms.Label lunInUseLabel;
        private Controls.SpinnerIcon spinnerIconAtTargetLun;
        private System.Windows.Forms.ComboBox comboBoxLun;
        private System.Windows.Forms.Label errorLabelAtTargetLUN;
        private System.Windows.Forms.PictureBox errorIconAtTargetLUN;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label Lun;
        private System.Windows.Forms.Label label11;
    }
}