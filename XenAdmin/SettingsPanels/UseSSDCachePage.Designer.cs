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
            this.useSSDCacheCheckBox = new System.Windows.Forms.CheckBox();
            this.labelUseSSDCache = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.useSSDCacheCheckBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelUseSSDCache, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // useSSDCacheCheckBox
            // 
            resources.ApplyResources(this.useSSDCacheCheckBox, "useSSDCacheCheckBox");
            this.useSSDCacheCheckBox.Name = "useSSDCacheCheckBox";
            this.useSSDCacheCheckBox.UseVisualStyleBackColor = false;
            // 
            // labelUseSSDCache
            // 
            resources.ApplyResources(this.labelUseSSDCache, "labelUseSSDCache");
            this.labelUseSSDCache.Name = "labelUseSSDCache";
            // 
            // UseSSDCachePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UseSSDCachePage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox useSSDCacheCheckBox;
        private System.Windows.Forms.Label labelUseSSDCache;
    }
}
