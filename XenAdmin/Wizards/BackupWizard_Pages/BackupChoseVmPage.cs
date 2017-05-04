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
using HalsignLib;
using XenAPI;
using XenAdmin.Controls;
using XenAdmin.Network;
using XenAdmin.Core;
using XenAdmin.Dialogs;
using XenAdmin.Actions.BRActions;


namespace XenAdmin.Wizards.BackupWizard_Pages
{
    public class BackupChoseVmPage : XenTabPage
	{
        private IContainer components = null;
        private Button button1;
        private CheckBox checkBox1;
        private TreeView BackupTreeView;

        private IXenObject _xenModelObject;
        private Dictionary<object, TreeNode> _nodes = new Dictionary<object, TreeNode>();
        private bool switchSnapshot = false;
        private bool IsVM;
        //Backup configuration set or not
        private bool checkConfiguration = true;
        //It does not have BR config that Object has been selected
        private IXenObject noSetVM;
        //Selected Count
        private int checkedCount = 0;
        private long storageCount = 0;
        private string jobName;
        private List<VM> vmCheckedList = new List<VM>();
        private Dictionary<string, string> vdi_dictionary = new Dictionary<string, string>();

        public BackupChoseVmPage(IXenObject XenObject)
		{
            this._xenModelObject = XenObject;

			InitializeComponent();
            IsVM = this._xenModelObject is VM;
            this.BackupTreeView.ImageList = Images.ImageList16;
            this.BackupTreeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.BackupTreeView.DrawNode += new DrawTreeNodeEventHandler(TreeView_DrawNode);
            this.BackupTreeView.Nodes[0].ImageIndex = 999;
            this.BackupTreeView.Nodes[0].SelectedImageIndex = 999;
            RefreshTreeViewCore();
		}

        public override string Text
        {
            get
            {
                return Messages.CHOSE_VMS;
            }
        }

        public override string PageTitle
        {
            get
            {
                return Messages.CHOSE_VMS;
            }
        }

        public TreeNodeCollection TreeViewNodes
        {
            get 
            {
                return this.BackupTreeView.Nodes;
            }
        }

        internal bool CheckConfiguration
        {
            get
            {
                return this.checkConfiguration;
            }
        }

        internal string JobName
        {
            get
            {
                return this.jobName;
            }
        }

        internal long StorageCount
        {
            get
            {
                return this.storageCount;
            }
        }

        internal List<VM> VMCheckedList
        {
            get
            {
                return this.vmCheckedList;
            }
        }

        internal Dictionary<string, string> VDI_Dictionary
        {
            get
            {
                return this.vdi_dictionary;
            }
        }


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupChoseVmPage));
            this.BackupTreeView = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BackupTreeView
            // 
            resources.ApplyResources(this.BackupTreeView, "BackupTreeView");
            this.BackupTreeView.CheckBoxes = true;
            this.BackupTreeView.HideSelection = false;
            this.BackupTreeView.Name = "BackupTreeView";
            this.BackupTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("BackupTreeView.Nodes")))});
            this.BackupTreeView.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.BackupTreeView_BeforeCheck);
            this.BackupTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterCheck);
            this.BackupTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.BackupTreeView_AfterCollapse);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // BackupChoseVmPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BackupTreeView);
            this.Name = "BackupChoseVmPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void RefreshTreeViewCore()
        {
            int index = 0;
            List<IXenConnection> xenConnectionsCopy = ConnectionsManager.XenConnectionsCopy;
            xenConnectionsCopy.Sort();
            foreach (IXenConnection connection in xenConnectionsCopy)
            {
                VM[] vMs = connection.Cache.VMs.Where(vm => vm.uuid != null).ToArray();
                Array.Sort<VM>(vMs);
                XenAPI.SR[] sRs = connection.Cache.SRs;
                Array.Sort<XenAPI.SR>(sRs);
                Pool pool = HalsignHelpers.GetPool(connection);
                if (pool != null)
                {
                    bool flag2;
                    TreeNode parent = this.AddNode(this.BackupTreeView.Nodes[0], index, HalsignHelpers.GetName(pool), Images.GetIconFor(pool), false, pool, out flag2);
                    index++;
                    Host[] hosts = connection.Cache.Hosts;
                    Array.Sort<Host>(hosts);
                    int num2 = 0;
                    foreach (Host obj3 in hosts)
                    {
                        this.AddHostAndChildren(parent, num2, connection, vMs, sRs, obj3, false, true);
                        num2++;
                    }
                    this.AddVMsToTree(parent, ref num2, connection, vMs, null, true);
                    this.PurgeFromHere(parent, num2);
                    if (flag2)
                    {
                        parent.Expand();
                    }
                    continue;
                }
                Pool poolOfOne = HalsignHelpers.GetPoolOfOne(connection);
                Host theHost = (poolOfOne == null) ? null : poolOfOne.Connection.Resolve<Host>(poolOfOne.master);
                if (theHost == null)
                {
                    this.AddDisconnectedNode(this.BackupTreeView.Nodes[0], index, connection);
                }
                else
                {
                    this.AddHostAndChildren(this.BackupTreeView.Nodes[0], index, connection, vMs, sRs, theHost, true, false);
                }
                index++;
            }
            this.PurgeFromHere(this.BackupTreeView.Nodes[0], index);
            if (this.BackupTreeView.Nodes[0].Nodes.Count <= 0)
            {
                this.BackupTreeView.Enabled = false;
            }
            else
            {
                this.BackupTreeView.Nodes[0].Expand();
            }
        }

        private TreeNode AddNode(TreeNode parent, int index, string name, Icons icon, bool grayed, object obj, out bool expand)
        {
            TreeNode node;
            if ((parent.Nodes.Count > index) && (parent.Nodes[index].Tag == obj))
            {
                node = parent.Nodes[index];
                if (node.Text != name)
                {
                    node.Text = name;
                }
                expand = false;
            }
            else
            {
                node = new TreeNode(name);
                if (grayed)
                {
                    node.ForeColor = SystemColors.GrayText;
                }
                node.Tag = obj;
                parent.Nodes.Insert(index, node);
                if (this._nodes.ContainsKey(obj))
                {
                    expand = this._nodes[obj].IsExpanded;
                    this._nodes.Remove(obj);
                }
                else
                {
                    expand = true;
                }
                this._nodes.Add(obj, node);
            }
            int num = (int)icon;
            if (icon == Icons.XenCenter)
            {
                node.ImageIndex = 999;
                node.SelectedImageIndex = 999;
            }
            else if (node.StateImageIndex != num)
            {
                node.ImageIndex = num;
                node.SelectedImageIndex = num;
            }
            if (IsVM && this._xenModelObject == obj)
            {
                this.BackupTreeView.SelectedNode = node;
                node.Checked = true;
            }
            return node;
        }

        private void AddHostAndChildren(TreeNode parent, int index, IXenConnection connection, VM[] vms, XenAPI.SR[] srs, Host host, bool add_all, bool is_pool)
        {
            bool expand;
            bool live;
            TreeNode node = this.AddHostNode(parent, index, connection, host, out expand, out live);
            node.Checked = true;
            int num = 0;
            if (live)
            {
                this.AddVMsToTree(node, ref num, connection, vms, host, is_pool);
                this.PurgeFromHere(node, num);
                if (expand)
                {
                    node.Expand();
                }
            }
            else
            {
                this.PurgeFromHere(node, num);
            }
        }

        private TreeNode AddHostNode(TreeNode parent, int index, IXenConnection connection, Host host, out bool expand, out bool live)
        {
            Host_metrics host_metrics = host.Connection.Resolve<Host_metrics>(host.metrics);
            live = (host_metrics != null) && host_metrics.live;
            return this.AddNode(parent, index, host.name_label, Images.GetIconFor(host), false, host, out expand);
        }

        private void AddVMsToTree(TreeNode parent, ref int index, IXenConnection connection, VM[] vms, Host TheHost, bool IsPool)
        {
            foreach (VM vm in vms)
            {
                bool is_snapshot = (!vm.is_a_template && HalsignHelpers.IsVMShow(vm) && !(vm.snapshots != null && vm.snapshots.Count > 0) && vm.allowed_operations.Count > 0);
                if (switchSnapshot)
                {
                    is_snapshot = (!vm.is_a_template && HalsignHelpers.IsVMShow(vm) && !(vm.snapshots != null && vm.snapshots.Count > 0) && vm.allowed_operations.Count > 0);
                }
                else
                {
                    is_snapshot = (!vm.is_a_template && HalsignHelpers.IsVMShow(vm) && vm.allowed_operations.Count > 0);
                }

                if (is_snapshot)
                {
                    if ((HalsignHelpers.VMHome(vm) == TheHost) || !IsPool)
                    {
                        if (!vm.name_label.StartsWith("__gui__"))
                        {
                            if (switchSnapshot)
                            {
                                bool hasParent = false;
                                foreach (VBD vbd in vm.Connection.ResolveAll<VBD>(vm.VBDs))
                                {
                                    if (HalsignHelpers.IsCDROM(vbd))
                                    {
                                        continue;
                                    }

                                    VDI vdi = vm.Connection.Resolve<VDI>(vbd.VDI);
                                    if (vdi != null) //&& vdi.Server.Show)
                                    {
                                        if (vdi.sm_config.ContainsKey("vhd-parent") && !string.IsNullOrEmpty(vdi.sm_config["vhd-parent"]))
                                        {
                                            hasParent = true;
                                            break;
                                        }
                                    }
                                }
                                if (hasParent)
                                {
                                    continue;
                                }
                            }

                            this.AddVMNode(parent, index, vm);
                            index++;
                            //vmList.Add(vm.uuid, vm);
                        }
                    }
                }
            }
        }

        private void AddDisconnectedNode(TreeNode parent, int index, IXenConnection connection)
        {
            bool flag;
            this.AddNode(parent, index, HalsignHelpers.GetName(connection), Images.GetIconFor(connection), true, connection, out flag);
        }

        private void PurgeFromHere(TreeNode parent, int index)
        {
            while (index < parent.Nodes.Count)
            {
                object tag = parent.Nodes[index].Tag;
                if (this._nodes.ContainsKey(tag) && (this._nodes[tag] == parent.Nodes[index]))
                {
                    this._nodes.Remove(tag);
                }
                parent.Nodes.RemoveAt(index);
            }
        }

        private void AddVMNode(TreeNode parent, int index, VM vm)
        {
            bool flag;

            string os_name = vm.GetOSName().ToLowerInvariant();
            Icons _icons;
            if (os_name.Contains("windows"))
            {
                _icons = Icons.Windows;
            }
            else if (os_name.Contains("debian"))
            {
                _icons = Icons.Debian;
            }
            else if (os_name.Contains("red"))
            {
                _icons = Icons.RHEL;
            }
            else if (os_name.Contains("cent"))
            {
                _icons = Icons.CentOS;
            }
            else if (os_name.Contains("oracle"))
            {
                _icons = Icons.Oracle;
            }
            else if (os_name.Contains("suse"))
            {
                _icons = Icons.SUSE;
            }
            else if (os_name.Contains("ubuntu"))
            {
                _icons = Icons.XenCenter;
            }
            else
            {
                _icons = Icons.XenCenter;
            }
            TreeNode node = this.AddNode(parent, index, vm.name_label, _icons, false, vm, out flag);

            int num = 0;
            foreach (VBD vbd in vm.Connection.ResolveAll<VBD>(vm.VBDs))
            {
                if (HalsignHelpers.IsCDROM(vbd))
                {
                    continue;
                }

                if (vbd.type.Equals(XenAPI.vbd_type.Disk))
                {
                    VDI vdi = vm.Connection.Resolve<VDI>(vbd.VDI);

                    //Add by dalei.shou on 2013/10/28, ignore the removable physical storage shown
                    SR sr = vm.Connection.Resolve<SR>(vdi.SR);
                    if (sr.IsRemovableStorage())
                    {
                        continue;
                    }

                    bool expend;
                    TreeNode n = this.AddNode(node, num, vdi.name_label, Images.GetIconFor(vdi), false, vdi, out expend);
                    if (node.Checked == true)
                    {
                        n.Checked = true;
                    }
                    num++;
                }
            }
            if (node.Checked == true)
            {
                node.Expand();
            }
        
        }

        internal int CheckTreeView()
        {
            this.checkedCount = 0;
            this.storageCount = 0;
            this.vmCheckedList.Clear();
            this.vdi_dictionary.Clear();
            foreach (TreeNode nodeTemp in this.BackupTreeView.Nodes)
            {
                CountNodeChecked(nodeTemp);
            }
            return this.checkedCount;
        }

        private void CountNodeChecked(TreeNode currNode)
        {
            TreeNodeCollection nodesTemp = currNode.Nodes;
            VM vmTemp = null;
            VDI vdiTemp = null;
            if (nodesTemp.Count > 0)
            {
                foreach (TreeNode nodeTemp in nodesTemp)
                {
                    if (nodeTemp.Tag is VDI)
                    {
                        string job_expand = "";

                        if (nodeTemp.Checked)
                        {
                            vdiTemp = nodeTemp.Tag as VDI;
                        }
                        if (vdiTemp != null && nodeTemp.Checked)
                        {
                            if (nodeTemp.Parent.Tag is VM)
                            {
                                vmTemp = nodeTemp.Parent.Tag as VM;
                                if (vmTemp != null)
                                {
                                    if (!this.RetrieveConfiguration(vmTemp))
                                    {
                                        this.checkConfiguration = false;
                                        break;
                                    }
                                }
                                if (!this.vmCheckedList.Contains(vmTemp))
                                {
                                    this.vmCheckedList.Add(vmTemp);
                                    vdi_dictionary.Add(vmTemp.uuid, vdiTemp.uuid);
                                    this.checkedCount++;
                                }
                                else
                                {
                                    if (vdi_dictionary.ContainsKey(vmTemp.uuid))
                                    {
                                        job_expand = vdi_dictionary[vmTemp.uuid] + "@" + vdiTemp.uuid;
                                        vdi_dictionary.Remove(vmTemp.uuid);
                                        vdi_dictionary.Add(vmTemp.uuid, job_expand);
                                    }
                                }
                                this.jobName = nodeTemp.Parent.Text;
                                this.storageCount += vdiTemp.virtual_size;
                            }
                        }
                    }
                    CountNodeChecked(nodeTemp);
                }
            }
        }

        private bool RetrieveConfiguration(VM selectedVM)
        {
            bool result = false;
            if (selectedVM.Connection != null)
            {
                if (selectedVM.other_config.ContainsKey("halsign_br_enabled"))
                {
                    if (selectedVM.other_config["halsign_br_enabled"].Equals("True"))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                        this.noSetVM = selectedVM;
                    }
                }
                else
                {
                    Pool pool = HalsignHelpers.GetPoolOfOne(selectedVM.Connection);
                    if (pool != null && pool.other_config.ContainsKey("halsign_br_enabled") && pool.other_config["halsign_br_enabled"].Equals("True"))
                    {
                        result = true;
                    }
                    else
                    {
                        this.noSetVM = selectedVM;
                    }
                }
            }
            return result;
        }

        internal void ShowConfigurationDialog()
        {
            //if (Program.MainWindow.Confirms(this.noSetVM.Connection, Messages.BACKUP_CHECK_ERROR_MESSAGE, new object[] { (this.noSetVM as VM).name_label }))
            if (DialogResult.OK == MessageBox.Show(this, string.Format(Messages.BACKUP_CHECK_ERROR_MESSAGE, new object[] { (this.noSetVM as VM).name_label }), Messages.MESSAGEBOX_CONFIRM, MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                BackupConfigDialog backupConfig = new BackupConfigDialog();
                backupConfig.ShowDialog();
                if (backupConfig.DialogResult == DialogResult.OK)
                {
                    Dictionary<string, string> _args = new Dictionary<string, string>();
                    _args.Add("halsign_br_ip_address", backupConfig.Address);
                    _args.Add("halsign_br_username", backupConfig.UserName);
                    _args.Add("halsign_br_password", backupConfig.Password);
                    BackupConfigurationAction action = new BackupConfigurationAction(this.noSetVM.Connection, this.noSetVM, _args, 0);
                    if (action != null)
                    {
                        ActionProgressDialog dialog = new ActionProgressDialog(action, ProgressBarStyle.Blocks);
                        dialog.ShowCancel = true;
                        dialog.ShowDialog(this);
                    }
                }
                if (!(this._xenModelObject is Host))
                {
                    Program.MainWindow.TheTabControl.SelectTab("TabPageBackup");
                }
            }
            this.checkConfiguration = true;
        }

        private void BackupConfig_Save(object sender, EventArgs e)
        {
            //The method should be here.
        }

        internal bool CheckSelectedVM()
        {
            foreach (VM vm in this.vmCheckedList)
            {
                if (!vdi_dictionary.ContainsKey(vm.uuid))
                {
                    return false;
                }
            }
            return true;
        }

        private void TreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node != this.BackupTreeView.Nodes[0] && e.Node.Checked)
                {
                    e.Node.Parent.Checked = true;
                }

                SetChildNodeChecked(e.Node, e.Node.Checked);
                SetParentNodeChecked(e.Node);
            }
        }

        private void SetParentNodeChecked(TreeNode currNode)
        {
            if (currNode.Parent != null)
            {
                bool isChecked = false;
                foreach (TreeNode treeViewNode in currNode.Parent.Nodes)
                {
                    if (treeViewNode.Checked)
                        isChecked = true;
                }
                currNode.Parent.Checked = isChecked;
                SetParentNodeChecked(currNode.Parent);
            }
        }

        private void SetChildNodeChecked(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodesTemp = currNode.Nodes;
            foreach (TreeNode nodeTemp in nodesTemp)
            {
                nodeTemp.Checked = state;
                SetChildNodeChecked(nodeTemp, state);
            }
        }

        private void BackupTreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.ForeColor == SystemColors.GrayText)
            {
                e.Cancel = true;
            }
        }

        private void TreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            const int SPACE_IL = 3; // space between Image and Label

            // we only do additional drawing
            e.DrawDefault = true;

            if (this.BackupTreeView.ShowLines && Images.ImageList16 != null &&
            e.Node.ImageIndex >= Images.ImageList16.Images.Count)
            {
                // Image size
                int imgW = Images.ImageList16.ImageSize.Width;
                int imgH = Images.ImageList16.ImageSize.Height;

                // Image center
                int xPos = e.Node.Bounds.Left - SPACE_IL - imgW / 2;
                int yPos = (e.Node.Bounds.Top + e.Node.Bounds.Bottom) / 2;

                using (Pen p = new Pen(this.BackupTreeView.LineColor, 1))
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    // draw the horizontal missing treeline 
                    e.Graphics.DrawLine(p, (xPos - imgW / 2), yPos, (xPos + imgW / 2), yPos);

                    if (!this.BackupTreeView.CheckBoxes && e.Node.IsExpanded)
                    {
                        // draw the vertical missing treeline
                        e.Graphics.DrawLine(p, xPos, yPos, xPos, (yPos + imgW / 2));
                    }
                }
            }
        }

        private void BackupTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (!this.BackupTreeView.CheckBoxes && Images.ImageList16 != null &&
                e.Node.ImageIndex >= Images.ImageList16.Images.Count)
            {
                this.BackupTreeView.Invalidate(e.Node.Bounds);
            }
        }
	}
}
