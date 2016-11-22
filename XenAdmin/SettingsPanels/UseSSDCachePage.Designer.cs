namespace XenAdmin.SettingsPanels
{
    partial class UseSSDCachePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UseSSDCachePage));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ssdCacheWarningLabel = new System.Windows.Forms.Label();
            this.ssdCacheWarningImage = new System.Windows.Forms.PictureBox();
            this.labelUseSSDCache = new System.Windows.Forms.Label();
            this.useSSDCacheCheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssdCacheWarningImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.ssdCacheWarningLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ssdCacheWarningImage, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelUseSSDCache, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.useSSDCacheCheckBox, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // ssdCacheWarningLabel
            // 
            resources.ApplyResources(this.ssdCacheWarningLabel, "ssdCacheWarningLabel");
            this.ssdCacheWarningLabel.Name = "ssdCacheWarningLabel";
            // 
            // ssdCacheWarningImage
            // 
            resources.ApplyResources(this.ssdCacheWarningImage, "ssdCacheWarningImage");
            this.ssdCacheWarningImage.Name = "ssdCacheWarningImage";
            this.ssdCacheWarningImage.TabStop = false;
            // 
            // labelUseSSDCache
            // 
            resources.ApplyResources(this.labelUseSSDCache, "labelUseSSDCache");
            this.tableLayoutPanel1.SetColumnSpan(this.labelUseSSDCache, 2);
            this.labelUseSSDCache.Name = "labelUseSSDCache";
            // 
            // useSSDCacheCheckBox
            // 
            resources.ApplyResources(this.useSSDCacheCheckBox, "useSSDCacheCheckBox");
            this.tableLayoutPanel1.SetColumnSpan(this.useSSDCacheCheckBox, 2);
            this.useSSDCacheCheckBox.Name = "useSSDCacheCheckBox";
            this.useSSDCacheCheckBox.UseVisualStyleBackColor = false;
            // 
            // UseSSDCachePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UseSSDCachePage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssdCacheWarningImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox useSSDCacheCheckBox;
        private System.Windows.Forms.Label labelUseSSDCache;
        private System.Windows.Forms.PictureBox ssdCacheWarningImage;
        private System.Windows.Forms.Label ssdCacheWarningLabel;
    }
}
