namespace XenAdmin.Dialogs
{
    partial class PciBindDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PciBindDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.VMsComboBox = new XenAdmin.Controls.LongStringComboBox();
            this.BusNamelabel = new System.Windows.Forms.Label();
            this.UsbInfolabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.Bindbutton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.VMsComboBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.BusNamelabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.UsbInfolabel, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 3);
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // VMsComboBox
            // 
            resources.ApplyResources(this.VMsComboBox, "VMsComboBox");
            this.tableLayoutPanel1.SetColumnSpan(this.VMsComboBox, 2);
            this.VMsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VMsComboBox.FormattingEnabled = true;
            this.VMsComboBox.Name = "VMsComboBox";
            // 
            // BusNamelabel
            // 
            resources.ApplyResources(this.BusNamelabel, "BusNamelabel");
            this.BusNamelabel.Name = "BusNamelabel";
            // 
            // UsbInfolabel
            // 
            resources.ApplyResources(this.UsbInfolabel, "UsbInfolabel");
            this.tableLayoutPanel1.SetColumnSpan(this.UsbInfolabel, 2);
            this.UsbInfolabel.Name = "UsbInfolabel";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 3);
            this.flowLayoutPanel1.Controls.Add(this.Cancelbutton);
            this.flowLayoutPanel1.Controls.Add(this.Bindbutton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // Cancelbutton
            // 
            resources.ApplyResources(this.Cancelbutton, "Cancelbutton");
            this.Cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.Cancelbutton_Click);
            // 
            // Bindbutton
            // 
            resources.ApplyResources(this.Bindbutton, "Bindbutton");
            this.Bindbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Bindbutton.Name = "Bindbutton";
            this.Bindbutton.UseVisualStyleBackColor = true;
            this.Bindbutton.Click += new System.EventHandler(this.Bindbutton_Click);
            // 
            // PciBindDialog
            // 
            this.AcceptButton = this.Bindbutton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.Cancelbutton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PciBindDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Controls.LongStringComboBox VMsComboBox;
        private System.Windows.Forms.Label BusNamelabel;
        private System.Windows.Forms.Label UsbInfolabel;
        protected System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        protected System.Windows.Forms.Button Cancelbutton;
        protected System.Windows.Forms.Button Bindbutton;
    }
}