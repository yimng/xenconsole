namespace XenAdmin.Dialogs
{
    partial class VIFDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VIFDialog));
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelNetwork = new System.Windows.Forms.Label();
            this.comboBoxNetwork = new System.Windows.Forms.ComboBox();
            this.promptTextBoxMac = new System.Windows.Forms.TextBox();
            this.radioButtonAutogenerate = new System.Windows.Forms.RadioButton();
            this.radioButtonMac = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanelBody = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkboxQoSDown = new System.Windows.Forms.CheckBox();
            this.promptTextBoxQoSDown = new System.Windows.Forms.TextBox();
            this.labelQoSUnitsDown = new System.Windows.Forms.Label();
            this.tableLayoutPanelMAC = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutpanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.labelBlurb = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxNetworkEncryption = new System.Windows.Forms.TextBox();
            this.checkBoxNetworkEncryption = new System.Windows.Forms.CheckBox();
            this.toolTipContainerOkButton = new XenAdmin.Controls.ToolTipContainer();
            this.panelLicenseRestriction = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelMAC = new System.Windows.Forms.Label();
            this.labelQoS = new System.Windows.Forms.Label();
            this.flowLayoutPanelQoS = new System.Windows.Forms.FlowLayoutPanel();
            this.checkboxQoS = new System.Windows.Forms.CheckBox();
            this.promptTextBoxQoS = new System.Windows.Forms.TextBox();
            this.labelQoSUnits = new System.Windows.Forms.Label();
            this.tableLayoutPanelBody.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelMAC.SuspendLayout();
            this.tableLayoutpanelButtons.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolTipContainerOkButton.SuspendLayout();
            this.panelLicenseRestriction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanelQoS.SuspendLayout();
            this.SuspendLayout();
            // 
            // Cancelbutton
            // 
            resources.ApplyResources(this.Cancelbutton, "Cancelbutton");
            this.Cancelbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.Cancelbutton_Click);
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.Okbutton_Click);
            // 
            // labelNetwork
            // 
            resources.ApplyResources(this.labelNetwork, "labelNetwork");
            this.labelNetwork.Name = "labelNetwork";
            // 
            // comboBoxNetwork
            // 
            resources.ApplyResources(this.comboBoxNetwork, "comboBoxNetwork");
            this.comboBoxNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNetwork.FormattingEnabled = true;
            this.comboBoxNetwork.Name = "comboBoxNetwork";
            // 
            // promptTextBoxMac
            // 
            resources.ApplyResources(this.promptTextBoxMac, "promptTextBoxMac");
            this.promptTextBoxMac.Name = "promptTextBoxMac";
            // 
            // radioButtonAutogenerate
            // 
            resources.ApplyResources(this.radioButtonAutogenerate, "radioButtonAutogenerate");
            this.radioButtonAutogenerate.Name = "radioButtonAutogenerate";
            // 
            // radioButtonMac
            // 
            resources.ApplyResources(this.radioButtonMac, "radioButtonMac");
            this.radioButtonMac.Name = "radioButtonMac";
            this.radioButtonMac.UseVisualStyleBackColor = true;
            this.radioButtonMac.CheckedChanged += new System.EventHandler(this.MacRadioButton_CheckedChanged);
            // 
            // tableLayoutPanelBody
            // 
            resources.ApplyResources(this.tableLayoutPanelBody, "tableLayoutPanelBody");
            this.tableLayoutPanelBody.Controls.Add(this.flowLayoutPanel1, 0, 8);
            this.tableLayoutPanelBody.Controls.Add(this.tableLayoutPanelMAC, 0, 4);
            this.tableLayoutPanelBody.Controls.Add(this.tableLayoutpanelButtons, 0, 1);
            this.tableLayoutPanelBody.Controls.Add(this.labelBlurb, 0, 0);
            this.tableLayoutPanelBody.Controls.Add(this.panelButtons, 0, 8);
            this.tableLayoutPanelBody.Controls.Add(this.labelMAC, 0, 3);
            this.tableLayoutPanelBody.Controls.Add(this.labelQoS, 0, 5);
            this.tableLayoutPanelBody.Controls.Add(this.flowLayoutPanelQoS, 0, 6);
            this.tableLayoutPanelBody.Name = "tableLayoutPanelBody";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.checkboxQoSDown);
            this.flowLayoutPanel1.Controls.Add(this.promptTextBoxQoSDown);
            this.flowLayoutPanel1.Controls.Add(this.labelQoSUnitsDown);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // checkboxQoSDown
            // 
            resources.ApplyResources(this.checkboxQoSDown, "checkboxQoSDown");
            this.checkboxQoSDown.Name = "checkboxQoSDown";
            this.checkboxQoSDown.UseVisualStyleBackColor = true;
            this.checkboxQoSDown.CheckedChanged += new System.EventHandler(this.checkboxQoSDown_CheckedChanged);
            // 
            // promptTextBoxQoSDown
            // 
            resources.ApplyResources(this.promptTextBoxQoSDown, "promptTextBoxQoSDown");
            this.promptTextBoxQoSDown.Name = "promptTextBoxQoSDown";
            // 
            // labelQoSUnitsDown
            // 
            resources.ApplyResources(this.labelQoSUnitsDown, "labelQoSUnitsDown");
            this.labelQoSUnitsDown.Name = "labelQoSUnitsDown";
            // 
            // tableLayoutPanelMAC
            // 
            resources.ApplyResources(this.tableLayoutPanelMAC, "tableLayoutPanelMAC");
            this.tableLayoutPanelMAC.Controls.Add(this.radioButtonMac, 0, 1);
            this.tableLayoutPanelMAC.Controls.Add(this.radioButtonAutogenerate, 0, 0);
            this.tableLayoutPanelMAC.Controls.Add(this.promptTextBoxMac, 1, 1);
            this.tableLayoutPanelMAC.Name = "tableLayoutPanelMAC";
            // 
            // tableLayoutpanelButtons
            // 
            resources.ApplyResources(this.tableLayoutpanelButtons, "tableLayoutpanelButtons");
            this.tableLayoutpanelButtons.Controls.Add(this.labelNetwork, 0, 0);
            this.tableLayoutpanelButtons.Controls.Add(this.comboBoxNetwork, 1, 0);
            this.tableLayoutpanelButtons.Name = "tableLayoutpanelButtons";
            // 
            // labelBlurb
            // 
            resources.ApplyResources(this.labelBlurb, "labelBlurb");
            this.labelBlurb.Name = "labelBlurb";
            // 
            // panelButtons
            // 
            resources.ApplyResources(this.panelButtons, "panelButtons");
            this.panelButtons.Controls.Add(this.tableLayoutPanel1);
            this.panelButtons.Controls.Add(this.toolTipContainerOkButton);
            this.panelButtons.Controls.Add(this.Cancelbutton);
            this.panelButtons.Controls.Add(this.panelLicenseRestriction);
            this.panelButtons.Name = "panelButtons";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.textBoxNetworkEncryption, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxNetworkEncryption, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // textBoxNetworkEncryption
            // 
            resources.ApplyResources(this.textBoxNetworkEncryption, "textBoxNetworkEncryption");
            this.textBoxNetworkEncryption.Name = "textBoxNetworkEncryption";
            this.textBoxNetworkEncryption.TextChanged += new System.EventHandler(this.textBoxNetworkEncryption_TextChanged);
            // 
            // checkBoxNetworkEncryption
            // 
            resources.ApplyResources(this.checkBoxNetworkEncryption, "checkBoxNetworkEncryption");
            this.checkBoxNetworkEncryption.Name = "checkBoxNetworkEncryption";
            this.checkBoxNetworkEncryption.UseVisualStyleBackColor = true;
            this.checkBoxNetworkEncryption.CheckedChanged += new System.EventHandler(this.checkBoxNetworkEncryption_CheckedChanged);
            // 
            // toolTipContainerOkButton
            // 
            this.toolTipContainerOkButton.Controls.Add(this.buttonOk);
            resources.ApplyResources(this.toolTipContainerOkButton, "toolTipContainerOkButton");
            this.toolTipContainerOkButton.Name = "toolTipContainerOkButton";
            // 
            // panelLicenseRestriction
            // 
            this.panelLicenseRestriction.Controls.Add(this.pictureBox1);
            this.panelLicenseRestriction.Controls.Add(this.label1);
            resources.ApplyResources(this.panelLicenseRestriction, "panelLicenseRestriction");
            this.panelLicenseRestriction.Name = "panelLicenseRestriction";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Image = global::XenAdmin.Properties.Resources._000_Info3_h32bit_16;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labelMAC
            // 
            resources.ApplyResources(this.labelMAC, "labelMAC");
            this.labelMAC.Name = "labelMAC";
            // 
            // labelQoS
            // 
            resources.ApplyResources(this.labelQoS, "labelQoS");
            this.labelQoS.Name = "labelQoS";
            // 
            // flowLayoutPanelQoS
            // 
            resources.ApplyResources(this.flowLayoutPanelQoS, "flowLayoutPanelQoS");
            this.flowLayoutPanelQoS.Controls.Add(this.checkboxQoS);
            this.flowLayoutPanelQoS.Controls.Add(this.promptTextBoxQoS);
            this.flowLayoutPanelQoS.Controls.Add(this.labelQoSUnits);
            this.flowLayoutPanelQoS.Name = "flowLayoutPanelQoS";
            // 
            // checkboxQoS
            // 
            resources.ApplyResources(this.checkboxQoS, "checkboxQoS");
            this.checkboxQoS.Name = "checkboxQoS";
            this.checkboxQoS.UseVisualStyleBackColor = true;
            this.checkboxQoS.CheckedChanged += new System.EventHandler(this.checkboxQoS_CheckedChanged);
            // 
            // promptTextBoxQoS
            // 
            resources.ApplyResources(this.promptTextBoxQoS, "promptTextBoxQoS");
            this.promptTextBoxQoS.Name = "promptTextBoxQoS";
            // 
            // labelQoSUnits
            // 
            resources.ApplyResources(this.labelQoSUnits, "labelQoSUnits");
            this.labelQoSUnits.Name = "labelQoSUnits";
            // 
            // VIFDialog
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.Cancelbutton;
            this.Controls.Add(this.tableLayoutPanelBody);
            this.Name = "VIFDialog";
            this.tableLayoutPanelBody.ResumeLayout(false);
            this.tableLayoutPanelBody.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanelMAC.ResumeLayout(false);
            this.tableLayoutPanelMAC.PerformLayout();
            this.tableLayoutpanelButtons.ResumeLayout(false);
            this.tableLayoutpanelButtons.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolTipContainerOkButton.ResumeLayout(false);
            this.panelLicenseRestriction.ResumeLayout(false);
            this.panelLicenseRestriction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanelQoS.ResumeLayout(false);
            this.flowLayoutPanelQoS.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelNetwork;
        private System.Windows.Forms.ComboBox comboBoxNetwork;
        private System.Windows.Forms.TextBox promptTextBoxMac;
        private System.Windows.Forms.RadioButton radioButtonAutogenerate;
        private System.Windows.Forms.RadioButton radioButtonMac;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBody;
        private System.Windows.Forms.Label labelBlurb;
        private System.Windows.Forms.Panel panelButtons;
        private XenAdmin.Controls.ToolTipContainer toolTipContainerOkButton;
        private System.Windows.Forms.Label labelMAC;
        private System.Windows.Forms.Label labelQoS;
        private System.Windows.Forms.Label labelQoSUnits;
        private System.Windows.Forms.CheckBox checkboxQoS;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelQoS;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMAC;
        private System.Windows.Forms.TableLayoutPanel tableLayoutpanelButtons;
        private System.Windows.Forms.TextBox promptTextBoxQoS;
        private System.Windows.Forms.Panel panelLicenseRestriction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox checkboxQoSDown;
        private System.Windows.Forms.TextBox promptTextBoxQoSDown;
        private System.Windows.Forms.Label labelQoSUnitsDown;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxNetworkEncryption;
        private System.Windows.Forms.CheckBox checkBoxNetworkEncryption;
    }
}