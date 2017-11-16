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
            resources.ApplyResources(this.labelSelectOption, "labelSelectOption");
            this.labelSelectOption.Name = "labelSelectOption";
            // 
            // labelHeader
            // 
            resources.ApplyResources(this.labelHeader, "labelHeader");
            this.labelHeader.Name = "labelHeader";
            // 
            // labelLUNDetails
            // 
            resources.ApplyResources(this.labelLUNDetails, "labelLUNDetails");
            this.labelLUNDetails.Name = "labelLUNDetails";
            // 
            // pictureBoxArrowFormat
            // 
            resources.ApplyResources(this.pictureBoxArrowFormat, "pictureBoxArrowFormat");
            this.pictureBoxArrowFormat.Image = global::XenAdmin.Properties.Resources._112_RightArrowLong_Blue_24x24_72;
            this.pictureBoxArrowFormat.Name = "pictureBoxArrowFormat";
            this.pictureBoxArrowFormat.TabStop = false;
            this.pictureBoxArrowFormat.Click += new System.EventHandler(this.panel_Click);
            this.pictureBoxArrowFormat.MouseEnter += new System.EventHandler(this.panelFormat_MouseEnter);
            // 
            // buttonFormat
            // 
            resources.ApplyResources(this.buttonFormat, "buttonFormat");
            this.buttonFormat.FlatAppearance.BorderSize = 0;
            this.buttonFormat.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.buttonFormat.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.buttonFormat.ForeColor = System.Drawing.Color.Navy;
            this.buttonFormat.Name = "buttonFormat";
            this.buttonFormat.UseVisualStyleBackColor = true;
            this.buttonFormat.Click += new System.EventHandler(this.buttonFormat_Click);
            this.buttonFormat.MouseEnter += new System.EventHandler(this.panelFormat_MouseEnter);
            // 
            // panelFormat
            // 
            resources.ApplyResources(this.panelFormat, "panelFormat");
            this.panelFormat.BorderColor = System.Drawing.Color.Black;
            this.panelFormat.BorderWidth = 1;
            this.panelFormat.Controls.Add(this.pictureBoxArrowFormat);
            this.panelFormat.Controls.Add(this.buttonFormat);
            this.panelFormat.Controls.Add(this.labelFormatInfo);
            this.panelFormat.Name = "panelFormat";
            this.panelFormat.Click += new System.EventHandler(this.panel_Click);
            this.panelFormat.MouseEnter += new System.EventHandler(this.panelFormat_MouseEnter);
            // 
            // labelFormatInfo
            // 
            resources.ApplyResources(this.labelFormatInfo, "labelFormatInfo");
            this.labelFormatInfo.Name = "labelFormatInfo";
            this.labelFormatInfo.Click += new System.EventHandler(this.panel_Click);
            this.labelFormatInfo.MouseEnter += new System.EventHandler(this.panelFormat_MouseEnter);
            // 
            // panelReattach
            // 
            resources.ApplyResources(this.panelReattach, "panelReattach");
            this.panelReattach.BackColor = System.Drawing.SystemColors.Control;
            this.panelReattach.BorderColor = System.Drawing.Color.Black;
            this.panelReattach.BorderWidth = 1;
            this.panelReattach.Controls.Add(this.pictureBoxArrowReattach);
            this.panelReattach.Controls.Add(this.buttonReattach);
            this.panelReattach.Controls.Add(this.labelReattachInfo);
            this.panelReattach.Name = "panelReattach";
            this.panelReattach.Click += new System.EventHandler(this.panel_Click);
            this.panelReattach.MouseEnter += new System.EventHandler(this.panelReattach_MouseEnter);
            // 
            // pictureBoxArrowReattach
            // 
            resources.ApplyResources(this.pictureBoxArrowReattach, "pictureBoxArrowReattach");
            this.pictureBoxArrowReattach.Image = global::XenAdmin.Properties.Resources._112_RightArrowLong_Blue_24x24_72;
            this.pictureBoxArrowReattach.Name = "pictureBoxArrowReattach";
            this.pictureBoxArrowReattach.TabStop = false;
            this.pictureBoxArrowReattach.Click += new System.EventHandler(this.panel_Click);
            this.pictureBoxArrowReattach.MouseEnter += new System.EventHandler(this.panelReattach_MouseEnter);
            // 
            // buttonReattach
            // 
            resources.ApplyResources(this.buttonReattach, "buttonReattach");
            this.buttonReattach.FlatAppearance.BorderSize = 0;
            this.buttonReattach.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.buttonReattach.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.buttonReattach.ForeColor = System.Drawing.Color.Navy;
            this.buttonReattach.Name = "buttonReattach";
            this.buttonReattach.UseVisualStyleBackColor = true;
            this.buttonReattach.Click += new System.EventHandler(this.buttonReattach_Click);
            this.buttonReattach.MouseEnter += new System.EventHandler(this.panelReattach_MouseEnter);
            // 
            // labelReattachInfo
            // 
            resources.ApplyResources(this.labelReattachInfo, "labelReattachInfo");
            this.labelReattachInfo.Name = "labelReattachInfo";
            this.labelReattachInfo.Click += new System.EventHandler(this.panel_Click);
            this.labelReattachInfo.MouseEnter += new System.EventHandler(this.panelReattach_MouseEnter);
            // 
            // labelWarning
            // 
            resources.ApplyResources(this.labelWarning, "labelWarning");
            this.labelWarning.Name = "labelWarning";
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxRepeat
            // 
            resources.ApplyResources(this.checkBoxRepeat, "checkBoxRepeat");
            this.checkBoxRepeat.Name = "checkBoxRepeat";
            this.checkBoxRepeat.UseVisualStyleBackColor = true;
            // 
            // LVMoMirrorChooseLogWarningDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
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
            this.Name = "LVMoMirrorChooseLogWarningDialog";
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