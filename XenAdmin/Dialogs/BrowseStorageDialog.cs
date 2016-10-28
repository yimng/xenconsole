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
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Forms;
using XenAPI;
using XenAdmin.Dialogs;
using XenAdmin.Core;
using XenAdmin.SettingsPanels;
using XenAdmin.Wizards.NewPolicyWizard;
using XenAdmin.Wizards.NewVMApplianceWizard;

namespace XenAdmin.Dialogs
{
    public class BrowseStorageDialog : VerticallyTabbedDialog
    {
        private int _selectedTab;
        private IContainer components = null;
        private System.Timers.Timer timer;
        private IXenObject xenObject;
        private IXenObject xenObjectBefore;
        private IXenObject xenObjectCopy;

        private BrowseStorageGeneralPage BrowseStorageGeneralPage;
        private BrowseStorageVirtualDiskPage BrowseStorageVirtualDiskPage;

        public BrowseStorageDialog(IXenObject xenObject)
            : base(xenObject.Connection)
        {
            this.timer = new System.Timers.Timer();
            this._selectedTab = -1;
            this.InitializeComponent();
            this.xenObject = xenObject;
            this.xenObjectBefore = xenObject.Clone();
            this.xenObjectCopy = xenObject.Clone();
            base.Name = string.Format("Browse {0}", xenObject.GetType().Name);
            this.Text = string.Format(Messages.BROWSE_STORAGE_DIALOG_TITLE, Helpers.GetName(xenObject));
            this.okButton.Text = Messages.OK;
            if (!Application.RenderWithVisualStyles)
            {
                base.ContentPanel.BackColor = SystemColors.Control;
            }
            this.Build();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Build()
        {
            Pool poolOfOne = Helpers.GetPoolOfOne(base.connection);
            bool flag7 = Helpers.GetPool(this.xenObjectCopy.Connection) != null;
            base.ContentPanel.SuspendLayout();
            base.verticalTabs.BeginUpdate();
            try
            {
                base.verticalTabs.Items.Clear();
                this.ShowTab(this.BrowseStorageGeneralPage = new BrowseStorageGeneralPage());
                this.ShowTab(this.BrowseStorageVirtualDiskPage = new BrowseStorageVirtualDiskPage());
                this.BrowseStorageVirtualDiskPage.SR = this.xenObject as XenAPI.SR;
                
            }
            finally
            {
                base.ContentPanel.ResumeLayout();
                base.verticalTabs.EndUpdate();
                base.verticalTabs.SelectedIndex = (this._selectedTab > 0) ? this._selectedTab : 0;
            }
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PropertiesDialog));
            base.splitContainer.Panel1.SuspendLayout();
            base.splitContainer.SuspendLayout();
            base.SuspendLayout();
            resources.ApplyResources(base.ContentPanel, "ContentPanel");
            base.ContentPanel.MinimumSize = new Size(500, 500);
            resources.ApplyResources(base.verticalTabs, "verticalTabs");
            base.verticalTabs.SelectedIndexChanged += new EventHandler(this.verticalTabs_SelectedIndexChanged);
            resources.ApplyResources(base.cancelButton, "cancelButton");
            base.cancelButton.Click += new EventHandler(this.btnCancel_Click);
            resources.ApplyResources(base.okButton, "okButton");
            base.okButton.Click += new EventHandler(this.btnOK_Click);
            resources.ApplyResources(base.splitContainer, "splitContainer");
            resources.ApplyResources(base.splitContainer.Panel1, "splitContainer.Panel1");
            resources.ApplyResources(base.splitContainer.Panel2, "splitContainer.Panel2");
            resources.ApplyResources(this, "$this");
            base.Name = "PropertiesDialog";
            base.FormClosed += new FormClosedEventHandler(this.BrowseStorageDialog_FormClosed);
            base.splitContainer.Panel1.ResumeLayout(false);
            base.splitContainer.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected sealed override void OnFormClosing(FormClosingEventArgs e)
        {
            foreach (IEditPage page in base.verticalTabs.Items)
            {
                page.Cleanup();
            }
        }

        private void BrowseStorageDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.timer.Stop();
        }

        public void SetSelectedTab(int selectedTabIndex)
        {
            this._selectedTab = selectedTabIndex;
        }

        private void ShowTab(IEditPage editPage)
        {
            Control control = (Control) editPage;
            base.ContentPanel.Controls.Add(control);
            control.BackColor = Color.Transparent;
            control.Dock = DockStyle.Fill;
            editPage.SetXenObjects(this.xenObject, this.xenObjectCopy);
            base.verticalTabs.Items.Add(editPage);
        }

        private void verticalTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            NewPolicyArchivePage selectedItem = base.verticalTabs.SelectedItem as NewPolicyArchivePage;
            if (selectedItem != null)
            {
            }
            else
            {
                NewVMApplianceVMOrderAndDelaysPage page2 = base.verticalTabs.SelectedItem as NewVMApplianceVMOrderAndDelaysPage;
            }
        }

        private bool HasChanged
        {
            get
            {
                foreach (IEditPage page in base.verticalTabs.Items)
                {
                    if (page.HasChanged)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private bool ValidToSave
        {
            get
            {
                foreach (IEditPage page in base.verticalTabs.Items)
                {
                    if (!page.ValidToSave)
                    {
                        base.SelectPage(page);
                        page.ShowLocalValidationMessages();
                        return false;
                    }
                }
                return true;
            }
        }
    }
}

