namespace XenAdmin.TabPages
{
    partial class vSwitchControllerPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(vSwitchControllerPage));
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonDeleteConfigure = new System.Windows.Forms.Button();
            this.buttonSetController = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.customListPanel = new XenAdmin.Controls.CustomListPanel();
            this.pageContainerPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageContainerPanel
            // 
            resources.ApplyResources(this.pageContainerPanel, "pageContainerPanel");
            this.pageContainerPanel.Controls.Add(this.tableLayoutPanel2);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.buttonDeleteConfigure);
            this.panel1.Controls.Add(this.buttonSetController);
            this.panel1.Name = "panel1";
            // 
            // buttonDeleteConfigure
            // 
            resources.ApplyResources(this.buttonDeleteConfigure, "buttonDeleteConfigure");
            this.buttonDeleteConfigure.Name = "buttonDeleteConfigure";
            this.buttonDeleteConfigure.UseVisualStyleBackColor = true;
            this.buttonDeleteConfigure.Click += new System.EventHandler(this.buttonDeleteConfigure_Click);
            // 
            // buttonSetController
            // 
            resources.ApplyResources(this.buttonSetController, "buttonSetController");
            this.buttonSetController.Name = "buttonSetController";
            this.buttonSetController.UseVisualStyleBackColor = true;
            this.buttonSetController.Click += new System.EventHandler(this.buttonSetController_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.customListPanel);
            this.panel2.Name = "panel2";
            // 
            // customListPanel
            // 
            resources.ApplyResources(this.customListPanel, "customListPanel");
            this.customListPanel.Name = "customListPanel";
            // 
            // vSwitchControllerPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Name = "vSwitchControllerPage";
            this.pageContainerPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonDeleteConfigure;
        private System.Windows.Forms.Button buttonSetController;
        private System.Windows.Forms.Panel panel2;
        private Controls.CustomListPanel customListPanel;

    }
}
