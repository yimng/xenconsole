namespace XenAdmin.Dialogs.WarningDialogs
{
    partial class LVMoMirrorChooseLogWarningDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LVMoMirrorChooseLogWarningDialog));
            this.labelSelectOption = new System.Windows.Forms.Label();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelLUNDetails = new System.Windows.Forms.Label();
            this.pictureBoxArrowFormat = new System.Windows.Forms.PictureBox();
            this.buttonFormat = new System.Windows.Forms.Button();
            this.panelFormat = new XenAdmin.Controls.FlickerFreePanel();
            this.labelFormatInfo = new System.Windows.Forms.Label();
            this.panelReattach = new XenAdmin.Controls.FlickerFreePanel();
            this.pictureBoxArrowReattach = new System.Windows.Forms.PictureBox();
            this.buttonReattach = new System.Windows.Forms.Button();
            this.labelReattachInfo = new System.Windows.Forms.Label();
            this.labelWarning = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxRepeat = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArrowFormat)).BeginInit();
            this.panelFormat.SuspendLayout();
            this.panelReattach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArrowReattach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSelectOption
            // 
            this.labelSelectOption.AutoSize = true;
            this.labelSelectOption.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelSelectOption.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelSelectOption.Location = new System.Drawing.Point(12, 227);
            this.labelSelectOption.Name = "labelSelectOption";
            this.labelSelectOption.Size = new System.Drawing.Size(222, 17);
            this.labelSelectOption.TabIndex = 15;
            this.labelSelectOption.Text = "Click the option below to add the SR";
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.labelHeader.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelHeader.Location = new System.Drawing.Point(9, 203);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(296, 17);
            this.labelHeader.TabIndex = 14;
            this.labelHeader.Text = "An existing SR was found on the selected LUN.";
            // 
            // labelLUNDetails
            // 
            this.labelLUNDetails.AutoSize = true;
            this.labelLUNDetails.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelLUNDetails.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelLUNDetails.Location = new System.Drawing.Point(9, 9);
            this.labelLUNDetails.Name = "labelLUNDetails";
            this.labelLUNDetails.Size = new System.Drawing.Size(95, 68);
            this.labelLUNDetails.TabIndex = 13;
            this.labelLUNDetails.Text = "Vendor:\r\nSerial Number:\r\nSCSI ID:\r\nSize:\r\n";
            // 
            // pictureBoxArrowFormat
            // 
            this.pictureBoxArrowFormat.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.pictureBoxArrowFormat.Image = global::XenAdmin.Properties.Resources._112_RightArrowLong_Blue_24x24_72;
            this.pictureBoxArrowFormat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBoxArrowFormat.Location = new System.Drawing.Point(5, 3);
            this.pictureBoxArrowFormat.Name = "pictureBoxArrowFormat";
            this.pictureBoxArrowFormat.Size = new System.Drawing.Size(24, 24);
            this.pictureBoxArrowFormat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxArrowFormat.TabIndex = 13;
            this.pictureBoxArrowFormat.TabStop = false;
            this.pictureBoxArrowFormat.Click += new System.EventHandler(this.panel_Click);
            this.pictureBoxArrowFormat.MouseEnter += new System.EventHandler(this.panelFormat_MouseEnter);
            // 
            // buttonFormat
            // 
            this.buttonFormat.AutoSize = true;
            this.buttonFormat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonFormat.FlatAppearance.BorderSize = 0;
            this.buttonFormat.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.buttonFormat.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.buttonFormat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFormat.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.buttonFormat.ForeColor = System.Drawing.Color.Navy;
            this.buttonFormat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonFormat.Location = new System.Drawing.Point(29, 4);
            this.buttonFormat.Name = "buttonFormat";
            this.buttonFormat.Size = new System.Drawing.Size(62, 27);
            this.buttonFormat.TabIndex = 0;
            this.buttonFormat.Text = "&Format";
            this.buttonFormat.UseVisualStyleBackColor = true;
            this.buttonFormat.Click += new System.EventHandler(this.buttonFormat_Click);
            this.buttonFormat.MouseEnter += new System.EventHandler(this.panelFormat_MouseEnter);
            // 
            // panelFormat
            // 
            this.panelFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFormat.BorderColor = System.Drawing.Color.Black;
            this.panelFormat.BorderWidth = 1;
            this.panelFormat.Controls.Add(this.pictureBoxArrowFormat);
            this.panelFormat.Controls.Add(this.buttonFormat);
            this.panelFormat.Controls.Add(this.labelFormatInfo);
            this.panelFormat.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.panelFormat.Location = new System.Drawing.Point(15, 309);
            this.panelFormat.Name = "panelFormat";
            this.panelFormat.Size = new System.Drawing.Size(393, 58);
            this.panelFormat.TabIndex = 17;
            this.panelFormat.Click += new System.EventHandler(this.panel_Click);
            this.panelFormat.MouseEnter += new System.EventHandler(this.panelFormat_MouseEnter);
            // 
            // labelFormatInfo
            // 
            this.labelFormatInfo.AutoSize = true;
            this.labelFormatInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelFormatInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelFormatInfo.Location = new System.Drawing.Point(33, 32);
            this.labelFormatInfo.Name = "labelFormatInfo";
            this.labelFormatInfo.Size = new System.Drawing.Size(347, 17);
            this.labelFormatInfo.TabIndex = 1;
            this.labelFormatInfo.Text = "Destroy any data present on the disk and create a new SR";
            this.labelFormatInfo.Click += new System.EventHandler(this.panel_Click);
            this.labelFormatInfo.MouseEnter += new System.EventHandler(this.panelFormat_MouseEnter);
            // 
            // panelReattach
            // 
            this.panelReattach.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelReattach.BackColor = System.Drawing.SystemColors.Control;
            this.panelReattach.BorderColor = System.Drawing.Color.Black;
            this.panelReattach.BorderWidth = 1;
            this.panelReattach.Controls.Add(this.pictureBoxArrowReattach);
            this.panelReattach.Controls.Add(this.buttonReattach);
            this.panelReattach.Controls.Add(this.labelReattachInfo);
            this.panelReattach.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.panelReattach.Location = new System.Drawing.Point(15, 245);
            this.panelReattach.Name = "panelReattach";
            this.panelReattach.Size = new System.Drawing.Size(393, 58);
            this.panelReattach.TabIndex = 22;
            this.panelReattach.Click += new System.EventHandler(this.panel_Click);
            this.panelReattach.MouseEnter += new System.EventHandler(this.panelReattach_MouseEnter);
            // 
            // pictureBoxArrowReattach
            // 
            this.pictureBoxArrowReattach.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.pictureBoxArrowReattach.Image = global::XenAdmin.Properties.Resources._112_RightArrowLong_Blue_24x24_72;
            this.pictureBoxArrowReattach.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBoxArrowReattach.Location = new System.Drawing.Point(5, 3);
            this.pictureBoxArrowReattach.Name = "pictureBoxArrowReattach";
            this.pictureBoxArrowReattach.Size = new System.Drawing.Size(24, 24);
            this.pictureBoxArrowReattach.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxArrowReattach.TabIndex = 14;
            this.pictureBoxArrowReattach.TabStop = false;
            this.pictureBoxArrowReattach.Click += new System.EventHandler(this.panel_Click);
            this.pictureBoxArrowReattach.MouseEnter += new System.EventHandler(this.panelReattach_MouseEnter);
            // 
            // buttonReattach
            // 
            this.buttonReattach.AutoSize = true;
            this.buttonReattach.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonReattach.FlatAppearance.BorderSize = 0;
            this.buttonReattach.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.buttonReattach.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.buttonReattach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReattach.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.buttonReattach.ForeColor = System.Drawing.Color.Navy;
            this.buttonReattach.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonReattach.Location = new System.Drawing.Point(29, 4);
            this.buttonReattach.Name = "buttonReattach";
            this.buttonReattach.Size = new System.Drawing.Size(71, 27);
            this.buttonReattach.TabIndex = 0;
            this.buttonReattach.Text = "&Reattach";
            this.buttonReattach.UseVisualStyleBackColor = true;
            this.buttonReattach.Click += new System.EventHandler(this.buttonReattach_Click);
            this.buttonReattach.MouseEnter += new System.EventHandler(this.panelReattach_MouseEnter);
            // 
            // labelReattachInfo
            // 
            this.labelReattachInfo.AutoSize = true;
            this.labelReattachInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelReattachInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelReattachInfo.Location = new System.Drawing.Point(33, 32);
            this.labelReattachInfo.Name = "labelReattachInfo";
            this.labelReattachInfo.Size = new System.Drawing.Size(119, 17);
            this.labelReattachInfo.TabIndex = 1;
            this.labelReattachInfo.Text = "Use the existing SR";
            this.labelReattachInfo.Click += new System.EventHandler(this.panel_Click);
            this.labelReattachInfo.MouseEnter += new System.EventHandler(this.panelReattach_MouseEnter);
            // 
            // labelWarning
            // 
            this.labelWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWarning.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelWarning.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelWarning.Location = new System.Drawing.Point(54, 379);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(354, 54);
            this.labelWarning.TabIndex = 23;
            this.labelWarning.Text = "To prevent data loss you must ensure that the LUN is not in use by any other syst" +
    "em, including vGate hosts that are not connected to vGate Console.";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox2.Location = new System.Drawing.Point(15, 379);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 24;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.Location = new System.Drawing.Point(14, 379);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(53, 379);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 54);
            this.label1.TabIndex = 23;
            this.label1.Text = "To prevent data loss you must ensure that the LUN is not in use by any other syst" +
    "em, including vGate hosts that are not connected to vGate Console.";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCancel.Location = new System.Drawing.Point(333, 481);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 26;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxRepeat
            // 
            this.checkBoxRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxRepeat.AutoSize = true;
            this.checkBoxRepeat.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBoxRepeat.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.checkBoxRepeat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxRepeat.Location = new System.Drawing.Point(15, 450);
            this.checkBoxRepeat.Name = "checkBoxRepeat";
            this.checkBoxRepeat.Size = new System.Drawing.Size(322, 21);
            this.checkBoxRepeat.TabIndex = 25;
            this.checkBoxRepeat.Text = "Do this for all remaining LUNs without existing SRs";
            this.checkBoxRepeat.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBoxRepeat.UseVisualStyleBackColor = true;
            // 
            // LVMoMirrorChooseLogWarningDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 516);
            this.Controls.Add(this.checkBoxRepeat);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panelReattach);
            this.Controls.Add(this.labelSelectOption);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.labelLUNDetails);
            this.Controls.Add(this.panelFormat);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "LVMoMirrorChooseLogWarningDialog";
            this.Text = "LVMoMirrorChooseLogWarningDialog";
            this.MouseEnter += new System.EventHandler(this.ExistingSRsWarningDialog_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArrowFormat)).EndInit();
            this.panelFormat.ResumeLayout(false);
            this.panelFormat.PerformLayout();
            this.panelReattach.ResumeLayout(false);
            this.panelReattach.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxArrowReattach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSelectOption;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelLUNDetails;
        private System.Windows.Forms.PictureBox pictureBoxArrowFormat;
        private System.Windows.Forms.Button buttonFormat;
        private Controls.FlickerFreePanel panelFormat;
        private System.Windows.Forms.Label labelFormatInfo;
        private Controls.FlickerFreePanel panelReattach;
        private System.Windows.Forms.PictureBox pictureBoxArrowReattach;
        private System.Windows.Forms.Button buttonReattach;
        private System.Windows.Forms.Label labelReattachInfo;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox checkBoxRepeat;
    }
}