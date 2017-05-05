/* Copyright © 2013 Halsign Corporation.
 * All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, 
 * with or without modification, are permitted provided 
 * that the following conditions are met: 
 * 
 * *   Redistributions of source code must retain the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer. 
 * *   Redistributions in binary form must reproduce the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer in the documentation and/or other 
 *     materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XenAPI;
using XenAdmin.Controls.HalsignControls;
using XenAdmin.Controls;
using XenAdmin.Core;
using XenAdmin.Dialogs;
using XenAdmin.Actions.BRActions;

namespace XenAdmin.TabPages
{
	public partial class BackupPage: BaseTabPage
    {
        private IContainer components = null;
        private Panel panel_Buttons;
        private Button button_ConfigBackup;
        private Button button_DisableBackup;
        private Button button_DeleteConfig;
        private BackupJobs backupJobs;
        private Panel panelTop;
        private CustomListPanel customListPanel;
        private IXenObject xenModelObject;
	    private BackupConfigDialog backupConfig;
    
		public BackupPage()
		{
			InitializeComponent();
            backupConfig = new BackupConfigDialog();
            base.Text = Messages.BR_PAGE_TITLE;
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupPage));
            this.panel_Buttons = new System.Windows.Forms.Panel();
            this.button_ConfigBackup = new System.Windows.Forms.Button();
            this.button_DisableBackup = new System.Windows.Forms.Button();
            this.button_DeleteConfig = new System.Windows.Forms.Button();
            this.backupJobs = new XenAdmin.Controls.HalsignControls.BackupJobs();
            this.panelTop = new System.Windows.Forms.Panel();
            this.customListPanel = new XenAdmin.Controls.CustomListPanel();
            this.pageContainerPanel.SuspendLayout();
            this.panel_Buttons.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageContainerPanel
            // 
            resources.ApplyResources(this.pageContainerPanel, "pageContainerPanel");
            this.pageContainerPanel.Controls.Add(this.panelTop);
            this.pageContainerPanel.Controls.Add(this.backupJobs);
            this.pageContainerPanel.Controls.Add(this.panel_Buttons);
            // 
            // panel_Buttons
            // 
            resources.ApplyResources(this.panel_Buttons, "panel_Buttons");
            this.panel_Buttons.BackColor = System.Drawing.Color.Transparent;
            this.panel_Buttons.Controls.Add(this.button_ConfigBackup);
            this.panel_Buttons.Controls.Add(this.button_DisableBackup);
            this.panel_Buttons.Controls.Add(this.button_DeleteConfig);
            this.panel_Buttons.Name = "panel_Buttons";
            // 
            // button_ConfigBackup
            // 
            resources.ApplyResources(this.button_ConfigBackup, "button_ConfigBackup");
            this.button_ConfigBackup.Name = "button_ConfigBackup";
            this.button_ConfigBackup.UseVisualStyleBackColor = true;
            this.button_ConfigBackup.Click += new System.EventHandler(this.button_ConfigBackup_Click);
            // 
            // button_DisableBackup
            // 
            resources.ApplyResources(this.button_DisableBackup, "button_DisableBackup");
            this.button_DisableBackup.Name = "button_DisableBackup";
            this.button_DisableBackup.UseVisualStyleBackColor = true;
            this.button_DisableBackup.Click += new System.EventHandler(this.button_DisableBackup_Click);
            // 
            // button_DeleteConfig
            // 
            resources.ApplyResources(this.button_DeleteConfig, "button_DeleteConfig");
            this.button_DeleteConfig.Name = "button_DeleteConfig";
            this.button_DeleteConfig.UseVisualStyleBackColor = true;
            this.button_DeleteConfig.Click += new System.EventHandler(this.button_DeleteConfig_Click);
            // 
            // backupJobs
            // 
            resources.ApplyResources(this.backupJobs, "backupJobs");
            this.backupJobs.BackColor = System.Drawing.SystemColors.Control;
            this.backupJobs.Name = "backupJobs";
            // 
            // panelTop
            // 
            resources.ApplyResources(this.panelTop, "panelTop");
            this.panelTop.Controls.Add(this.customListPanel);
            this.panelTop.Name = "panelTop";
            // 
            // customListPanel
            // 
            resources.ApplyResources(this.customListPanel, "customListPanel");
            this.customListPanel.Name = "customListPanel";
            // 
            // BackupPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.DoubleBuffered = true;
            this.Name = "BackupPage";
            this.pageContainerPanel.ResumeLayout(false);
            this.panel_Buttons.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private string GetUserName(IXenObject xenObject)
        {
            string result = Messages.UNKNOWN;
            if (xenObject is Pool)
            {
                Pool pool = xenObject as Pool;
                if (pool.other_config.ContainsKey("halsign_br_username"))
                {
                    result = pool.other_config["halsign_br_username"];
                }
            }
            else if (xenObject is VM)
            {
                VM vm = xenObject as VM;
                if (vm.other_config.ContainsKey("halsign_br_username"))
                {
                    result = vm.other_config["halsign_br_username"];
                }
            }
            return result;
        }

        private string GetStatus(IXenObject xenObject)
        {
            string result = Messages.UNKNOWN;
            if (xenObject is Pool)
            {
                Pool pool = xenObject as Pool;
                if (pool.other_config.ContainsKey("halsign_br_enabled"))
                {
                    result = pool.other_config["halsign_br_enabled"];
                }
            }
            else if (xenObject is VM)
            {
                VM vm = xenObject as VM;
                if (vm.other_config.ContainsKey("halsign_br_enabled"))
                {
                    result = vm.other_config["halsign_br_enabled"];
                }
            }

            if ("True".Equals(result))
            {
                result = Messages.BR_ENABLED;
                this.button_DisableBackup.Enabled = true;
                this.button_DisableBackup.Text = Messages.DISABLE_BACKUP;
                this.button_DeleteConfig.Enabled = true;
            }
            else if ("False".Equals(result))
            {
                result = Messages.BR_DISABLED;
                this.button_DisableBackup.Enabled = true;
                this.button_DisableBackup.Text = Messages.ENABLE_BACKUP;
                this.button_DeleteConfig.Enabled = true;
            }
            else
            {
                this.button_DisableBackup.Text = Messages.DISABLE_BACKUP;
                this.button_DisableBackup.Enabled = false;
                this.button_DeleteConfig.Enabled = false;
            }
            return result;
        }

        private string GetStoragePathIP(IXenObject xenObject)
        {
            string result = Messages.UNKNOWN;
            if (xenObject is Pool)
            {
                Pool pool = xenObject as Pool;
                if (pool.other_config.ContainsKey("halsign_br_ip_address"))
                {
                    result = pool.other_config["halsign_br_ip_address"];
                }
            }
            else if (xenObject is VM)
            {
                VM vm = xenObject as VM;
                if (vm.other_config.ContainsKey("halsign_br_ip_address"))
                {
                    result = vm.other_config["halsign_br_ip_address"];
                }
            }
            return result;
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "other_config")
            {
                this.Rebuild();
            }
        }

        public void Rebuild()
        {
            this.customListPanel.BeginUpdate();
            try
            {
                this.customListPanel.ClearRows();
                if (this.xenModelObject != null)
                {
                    this.GenerateBackupBox();
                }
            }
            finally
            {
                this.customListPanel.EndUpdate();
            }
        }

        private void GenerateBackupBox()
        {
            CustomListRow row = CreateHeader(Messages.BR_CONFIGURATION_TITLE);
            this.customListPanel.AddRow(row);
            if (this.xenModelObject is Pool)
            {
                Pool pool = this.xenModelObject as Pool;
                AddRow<Pool>(row, Messages.BR_CONFIG_STATUS, pool, new ToStringDelegate<Pool>(this.GetStatus), false);
                AddRow<Pool>(row, Messages.BR_STORAGE_IP, pool, new ToStringDelegate<Pool>(this.GetStoragePathIP), false);
                AddRow<Pool>(row, Messages.BR_STORAGE_USERNAME, pool, new ToStringDelegate<Pool>(this.GetUserName), false);
            }
            else if (this.xenModelObject is VM)
            {
                VM vm = this.xenModelObject as VM;
                AddRow<VM>(row, Messages.BR_CONFIG_STATUS, vm, new ToStringDelegate<VM>(this.GetStatus), false);
                AddRow<VM>(row, Messages.BR_STORAGE_IP, vm, new ToStringDelegate<VM>(this.GetStoragePathIP), false);
                AddRow<VM>(row, Messages.BR_STORAGE_USERNAME, vm, new ToStringDelegate<VM>(this.GetUserName), false);
            }

            AddBottomSpacing(row);
        }

        private CustomListRow CreateHeader(string text)
        {
            return new CustomListRow(text, BaseTabPage.HeaderBackColor, BaseTabPage.HeaderForeColor, BaseTabPage.HeaderBorderColor, Program.DefaultFontHeader);
        }

        private CustomListRow CreateNewRow<T>(string key, ToStringWrapper<T> value, bool valueBold) where T : IEquatable<T>
        {
            CustomListItem item = new CustomListItem(key, BaseTabPage.ItemLabelFont, BaseTabPage.ItemLabelForeColor);
            item.Anchor = AnchorStyles.Top;
            item.itemBorder.Bottom = 4;
            CustomListItem item2 = new CustomListItem(value, valueBold ? BaseTabPage.ItemValueFontBold : BaseTabPage.ItemValueFont, BaseTabPage.ItemValueForeColor);
            item2.Anchor = AnchorStyles.Top;
            item2.itemBorder.Bottom = 4;
            return new CustomListRow(BaseTabPage.ItemBackColor, new CustomListItem[] { item, item2 });
        }

        private void AddBottomSpacing(CustomListRow header)
        {
            if (header.Children.Count != 0)
            {
                header.AddChild(new CustomListRow(header.BackColor, 5));
            }
        }

        private CustomListRow AddRow<T>(CustomListRow parent, string key, T obj, ToStringDelegate<T> del, bool valueBold) where T : IEquatable<T>
        {
            CustomListRow row = CreateNewRow<T>(key + ": ", new ToStringWrapper<T>(obj, del), valueBold);
            parent.AddChild(row);
            return row;
        }

        public IXenObject XenModelObject
        {
            set
            {
                Program.AssertOnEventThread();
                if (value != null)
                {
                    this.backupJobs.XenModelObject = value;
                    if (this.xenModelObject != value)
                    {
                        this.xenModelObject = value;
                        if (this.xenModelObject is Pool)
                        {
                            Pool pool = this.xenModelObject as Pool;
                            pool.PropertyChanged += new PropertyChangedEventHandler(PropertyChanged);
                        }
                        else if (this.xenModelObject is VM)
                        {
                            VM vm = this.xenModelObject as VM;
                            vm.PropertyChanged += new PropertyChangedEventHandler(PropertyChanged);
                        }
                    }
                    this.Rebuild();
                }
            }
        }

        private void button_ConfigBackup_Click(object sender, EventArgs e)
        {
            Program.AssertOnEventThread();
            backupConfig.ShowDialog();

            // Blocking for a long time, check we haven't had the dialog disposed under us
            if (Disposing || IsDisposed)
                return;

            if (backupConfig.DialogResult == DialogResult.Cancel)
            {
                backupConfig.ClearPassword();
                return;
            }

            if (backupConfig.DialogResult == DialogResult.OK)
            {
                Dictionary<string, string> _args = new Dictionary<string, string>();
                _args.Add("halsign_br_ip_address", backupConfig.Address);
                _args.Add("halsign_br_username", backupConfig.UserName);
                _args.Add("halsign_br_password", backupConfig.Password);
                BackupConfigurationAction action = new BackupConfigurationAction(this.xenModelObject.Connection, this.xenModelObject, _args, 0);
                if (action != null)
                {
                    ActionProgressDialog dialog = new ActionProgressDialog(action, ProgressBarStyle.Blocks);
                    dialog.ShowCancel = true;
                    dialog.ShowDialog(this);
                    if (action.Exception == null)
                    {
                        this.Rebuild();
                    }
                }
            }
        }

        private void button_DeleteConfig_Click(object sender, EventArgs e)
        {
            string str_name = string.Empty;
            if (this.xenModelObject is Pool)
            {
                str_name = (this.xenModelObject as Pool).name_label;
            }
            else if (this.xenModelObject is VM)
            {
                str_name = (this.xenModelObject as VM).name_label;
            }
            //bool _commit = Program.MainWindow.Confirms(this.xenModelObject.Connection, Messages.CONFIRM_DELETE_BACKUP, new object[] { str_name });
            bool _commit = DialogResult.OK == MessageBox.Show(this, string.Format(Messages.CONFIRM_DELETE_BACKUP, new object[] { str_name }), Messages.MESSAGEBOX_CONFIRM, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (_commit)
            {
                BackupConfigurationAction action = new BackupConfigurationAction(this.xenModelObject.Connection, this.xenModelObject, null, 1);
                if (action != null)
                {
                    ActionProgressDialog dialog = new ActionProgressDialog(action, ProgressBarStyle.Blocks);
                    dialog.ShowCancel = true;
                    dialog.ShowDialog(this);
                }
            }
        }

        private void button_DisableBackup_Click(object sender, EventArgs e)
        {
            bool configure_enabled = false;
            if (this.xenModelObject is Pool)
            {
                configure_enabled = (this.xenModelObject as Pool).other_config["halsign_br_enabled"].Equals("True") ? true : false;
            }
            if (this.xenModelObject is VM)
            {
                if (this.xenModelObject is VM)
                {
                    configure_enabled = (this.xenModelObject as VM).other_config["halsign_br_enabled"].Equals("True") ? true : false;
                }
            }
            BackupConfigurationAction action = new BackupConfigurationAction(this.xenModelObject.Connection, this.xenModelObject, configure_enabled);
            if (action != null)
            {
                ActionProgressDialog dialog = new ActionProgressDialog(action, ProgressBarStyle.Blocks);
                dialog.ShowCancel = true;
                dialog.ShowDialog(this);
            }
        }

	}
}
