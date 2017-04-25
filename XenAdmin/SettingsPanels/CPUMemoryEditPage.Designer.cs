namespace XenAdmin.SettingsPanels
{
    partial class CPUMemoryEditPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CPUMemoryEditPage));
            this.lblSliderHighest = new System.Windows.Forms.Label();
            this.lblSliderNormal = new System.Windows.Forms.Label();
            this.lblSliderLowest = new System.Windows.Forms.Label();
            this.lblPriority = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxInitialVCPUs = new System.Windows.Forms.ComboBox();
            this.labelInitialVCPUs = new System.Windows.Forms.Label();
            this.labelInvalidVCPUWarning = new System.Windows.Forms.Label();
            this.labelTopology = new System.Windows.Forms.Label();
            this.MemWarningLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblMB = new System.Windows.Forms.Label();
            this.nudMemory = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblVCPUs = new System.Windows.Forms.Label();
            this.lblVcpuWarning = new System.Windows.Forms.LinkLabel();
            this.lblMemory = new System.Windows.Forms.Label();
            this.VCPUWarningLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxVCPUs = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxTopology = new XenAdmin.Controls.CPUTopologyComboBox();
            this.transparentTrackBar1 = new XenAdmin.Controls.TransparentTrackBar();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMemory)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSliderHighest
            // 
            resources.ApplyResources(this.lblSliderHighest, "lblSliderHighest");
            this.lblSliderHighest.Name = "lblSliderHighest";
            // 
            // lblSliderNormal
            // 
            resources.ApplyResources(this.lblSliderNormal, "lblSliderNormal");
            this.lblSliderNormal.Name = "lblSliderNormal";
            // 
            // lblSliderLowest
            // 
            resources.ApplyResources(this.lblSliderLowest, "lblSliderLowest");
            this.lblSliderLowest.Name = "lblSliderLowest";
            // 
            // lblPriority
            // 
            resources.ApplyResources(this.lblPriority, "lblPriority");
            this.tableLayoutPanel1.SetColumnSpan(this.lblPriority, 2);
            this.lblPriority.Name = "lblPriority";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.Controls.Add(this.comboBoxInitialVCPUs, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.labelInitialVCPUs, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.labelInvalidVCPUWarning, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxTopology, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelTopology, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.MemWarningLabel, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblPriority, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblVCPUs, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblVcpuWarning, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblMemory, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.VCPUWarningLabel, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxVCPUs, 1, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // comboBoxInitialVCPUs
            // 
            resources.ApplyResources(this.comboBoxInitialVCPUs, "comboBoxInitialVCPUs");
            this.comboBoxInitialVCPUs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInitialVCPUs.FormattingEnabled = true;
            this.comboBoxInitialVCPUs.Name = "comboBoxInitialVCPUs";
            // 
            // labelInitialVCPUs
            // 
            resources.ApplyResources(this.labelInitialVCPUs, "labelInitialVCPUs");
            this.labelInitialVCPUs.Name = "labelInitialVCPUs";
            // 
            // labelInvalidVCPUWarning
            // 
            resources.ApplyResources(this.labelInvalidVCPUWarning, "labelInvalidVCPUWarning");
            this.tableLayoutPanel1.SetColumnSpan(this.labelInvalidVCPUWarning, 2);
            this.labelInvalidVCPUWarning.ForeColor = System.Drawing.Color.Red;
            this.labelInvalidVCPUWarning.Name = "labelInvalidVCPUWarning";
            // 
            // labelTopology
            // 
            resources.ApplyResources(this.labelTopology, "labelTopology");
            this.labelTopology.Name = "labelTopology";
            // 
            // MemWarningLabel
            // 
            resources.ApplyResources(this.MemWarningLabel, "MemWarningLabel");
            this.MemWarningLabel.ForeColor = System.Drawing.Color.Red;
            this.MemWarningLabel.Name = "MemWarningLabel";
            this.tableLayoutPanel1.SetRowSpan(this.MemWarningLabel, 2);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.lblMB);
            this.panel2.Controls.Add(this.nudMemory);
            this.panel2.Name = "panel2";
            // 
            // lblMB
            // 
            resources.ApplyResources(this.lblMB, "lblMB");
            this.lblMB.Name = "lblMB";
            // 
            // nudMemory
            // 
            resources.ApplyResources(this.nudMemory, "nudMemory");
            this.nudMemory.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.nudMemory.Minimum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.nudMemory.Name = "nudMemory";
            this.nudMemory.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.nudMemory.ValueChanged += new System.EventHandler(this.nudMemory_ValueChanged);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 3);
            this.panel1.Controls.Add(this.lblSliderHighest);
            this.panel1.Controls.Add(this.lblSliderNormal);
            this.panel1.Controls.Add(this.lblSliderLowest);
            this.panel1.Controls.Add(this.transparentTrackBar1);
            this.panel1.Name = "panel1";
            // 
            // lblVCPUs
            // 
            resources.ApplyResources(this.lblVCPUs, "lblVCPUs");
            this.lblVCPUs.Name = "lblVCPUs";
            // 
            // lblVcpuWarning
            // 
            resources.ApplyResources(this.lblVcpuWarning, "lblVcpuWarning");
            this.tableLayoutPanel1.SetColumnSpan(this.lblVcpuWarning, 2);
            this.lblVcpuWarning.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblVcpuWarning.Name = "lblVcpuWarning";
            this.lblVcpuWarning.TabStop = true;
            this.lblVcpuWarning.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblVcpuWarning_LinkClicked);
            // 
            // lblMemory
            // 
            resources.ApplyResources(this.lblMemory, "lblMemory");
            this.lblMemory.Name = "lblMemory";
            // 
            // VCPUWarningLabel
            // 
            resources.ApplyResources(this.VCPUWarningLabel, "VCPUWarningLabel");
            this.VCPUWarningLabel.ForeColor = System.Drawing.Color.Red;
            this.VCPUWarningLabel.Name = "VCPUWarningLabel";
            this.tableLayoutPanel1.SetRowSpan(this.VCPUWarningLabel, 2);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 3);
            this.label1.Name = "label1";
            // 
            // comboBoxVCPUs
            // 
            resources.ApplyResources(this.comboBoxVCPUs, "comboBoxVCPUs");
            this.comboBoxVCPUs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVCPUs.FormattingEnabled = true;
            this.comboBoxVCPUs.Name = "comboBoxVCPUs";
            this.comboBoxVCPUs.SelectedIndexChanged += new System.EventHandler(this.comboBoxVCPUs_SelectedIndexChanged);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.trackBar1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Name = "panel3";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // trackBar1
            // 
            resources.ApplyResources(this.trackBar1, "trackBar1");
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.label2.Name = "label2";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // comboBoxTopology
            // 
            resources.ApplyResources(this.comboBoxTopology, "comboBoxTopology");
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxTopology, 2);
            this.comboBoxTopology.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTopology.FormattingEnabled = true;
            this.comboBoxTopology.Name = "comboBoxTopology";
            this.comboBoxTopology.SelectedIndexChanged += new System.EventHandler(this.comboBoxTopology_SelectedIndexChanged);
            // 
            // transparentTrackBar1
            // 
            resources.ApplyResources(this.transparentTrackBar1, "transparentTrackBar1");
            this.transparentTrackBar1.BackColor = System.Drawing.Color.Transparent;
            this.transparentTrackBar1.Name = "transparentTrackBar1";
            this.transparentTrackBar1.TabStop = false;
            // 
            // CPUMemoryEditPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "CPUMemoryEditPage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMemory)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.NumericUpDown nudMemory;
        private System.Windows.Forms.Label lblMB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSliderHighest;
        private System.Windows.Forms.Label lblSliderNormal;
        private System.Windows.Forms.Label lblSliderLowest;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.Label lblVCPUs;
        private System.Windows.Forms.Label lblMemory;
        private System.Windows.Forms.LinkLabel lblVcpuWarning;
        private XenAdmin.Controls.TransparentTrackBar transparentTrackBar1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label VCPUWarningLabel;
        private System.Windows.Forms.Label MemWarningLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTopology;
        private XenAdmin.Controls.CPUTopologyComboBox comboBoxTopology;
        private System.Windows.Forms.Label labelInvalidVCPUWarning;
        private System.Windows.Forms.ComboBox comboBoxVCPUs;
        private System.Windows.Forms.ComboBox comboBoxInitialVCPUs;
        private System.Windows.Forms.Label labelInitialVCPUs;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
    }
}