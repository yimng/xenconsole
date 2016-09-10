namespace XenAdmin.TabPages
{
	partial class HomePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomePage));
            this.LinkLabelHomePage = new System.Windows.Forms.LinkLabel();
            this.LabelLinkInfo = new System.Windows.Forms.Label();
            this.LabelVM = new System.Windows.Forms.Label();
            this.LabelServer = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.gradientPanel1 = new XenAdmin.Controls.GradientPanel.GradientPanel();
            this.labelInformation = new System.Windows.Forms.Label();
            this.PictureBoxVM = new System.Windows.Forms.PictureBox();
            this.PictureBoxHost = new System.Windows.Forms.PictureBox();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxVM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxHost)).BeginInit();
            this.SuspendLayout();
            // 
            // LinkLabelHomePage
            // 
            resources.ApplyResources(this.LinkLabelHomePage, "LinkLabelHomePage");
            this.LinkLabelHomePage.Name = "LinkLabelHomePage";
            this.LinkLabelHomePage.TabStop = true;
            this.LinkLabelHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelHomePage_LinkClicked);
            // 
            // LabelLinkInfo
            // 
            resources.ApplyResources(this.LabelLinkInfo, "LabelLinkInfo");
            this.LabelLinkInfo.Name = "LabelLinkInfo";
            // 
            // LabelVM
            // 
            resources.ApplyResources(this.LabelVM, "LabelVM");
            this.LabelVM.Name = "LabelVM";
            // 
            // LabelServer
            // 
            resources.ApplyResources(this.LabelServer, "LabelServer");
            this.LabelServer.Name = "LabelServer";
            // 
            // labelInfo
            // 
            resources.ApplyResources(this.labelInfo, "labelInfo");
            this.labelInfo.Name = "labelInfo";
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.Controls.Add(this.labelInformation);
            resources.ApplyResources(this.gradientPanel1, "gradientPanel1");
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Scheme = XenAdmin.Controls.GradientPanel.GradientPanel.Schemes.Tab;
            // 
            // labelInformation
            // 
            this.labelInformation.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelInformation, "labelInformation");
            this.labelInformation.ForeColor = System.Drawing.SystemColors.InfoText;
            this.labelInformation.Name = "labelInformation";
            // 
            // PictureBoxVM
            // 
            this.PictureBoxVM.BackgroundImage = global::XenAdmin.Properties.Resources._000_CreateVM_h32bit_32;
            resources.ApplyResources(this.PictureBoxVM, "PictureBoxVM");
            this.PictureBoxVM.Name = "PictureBoxVM";
            this.PictureBoxVM.TabStop = false;
            // 
            // PictureBoxHost
            // 
            this.PictureBoxHost.BackgroundImage = global::XenAdmin.Properties.Resources.addhost_48;
            resources.ApplyResources(this.PictureBoxHost, "PictureBoxHost");
            this.PictureBoxHost.Name = "PictureBoxHost";
            this.PictureBoxHost.TabStop = false;
            // 
            // HomePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gradientPanel1);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.LinkLabelHomePage);
            this.Controls.Add(this.LabelLinkInfo);
            this.Controls.Add(this.LabelVM);
            this.Controls.Add(this.PictureBoxVM);
            this.Controls.Add(this.LabelServer);
            this.Controls.Add(this.PictureBoxHost);
            this.Name = "HomePage";
            this.gradientPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxVM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxHost)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.LinkLabel LinkLabelHomePage;
		private System.Windows.Forms.Label LabelLinkInfo;
		private System.Windows.Forms.Label LabelVM;
		private System.Windows.Forms.PictureBox PictureBoxVM;
		private System.Windows.Forms.Label LabelServer;
		private System.Windows.Forms.PictureBox PictureBoxHost;
		private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelInformation;
        private XenAdmin.Controls.GradientPanel.GradientPanel gradientPanel1;
	}
}
