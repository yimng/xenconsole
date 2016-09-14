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


namespace XenAdmin.Wizards.ReplicationWizard_Pages
{
    public class ReplicationChoseVmPage : XenTabPage
	{
        private IContainer components = null;
        private TreeView treeView;
        private IXenObject _xenModelObject;
        private Dictionary<object, TreeNode> _nodes = new Dictionary<object, TreeNode>();
        private bool IsVM;
        private bool switchSnapshot = false;
        private List<VM> vmCheckedList;
        private Dictionary<string, long> vdicheckDictionary; 

        public ReplicationChoseVmPage(IXenObject XenObject)
		{
			InitializeComponent();
            this._xenModelObject = XenObject;
            IsVM = this._xenModelObject is VM;
            this.vmCheckedList = new List<VM>();
            this.vdicheckDictionary = new Dictionary<string, long>();
            this.treeView.ImageList = Images.ImageList16;
            this.treeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.treeView.DrawNode += new DrawTreeNodeEventHandler(TreeView_DrawNode);
            this.treeView.Nodes[0].ImageIndex = 999;
            this.treeView.Nodes[0].SelectedImageIndex = 999;
            this.BuildTreeView();
		}

        public List<VM> VmCheckedList
        {
            get 
            {
                return this.vmCheckedList;
            }
        }

        public Dictionary<string, long> VdicheckDictionary
        {
            get { return this.vdicheckDictionary; }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplicationChoseVmPage));
            this.treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.CheckBoxes = true;
            resources.ApplyResources(this.treeView, "treeView");
            this.treeView.HideSelection = false;
            this.treeView.Name = "treeView";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeView.Nodes")))});
            this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterCheck);
            this.treeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCollapse);
            this.treeView.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.BackupTreeView_BeforeCheck);
            // 
            // ReplicationChoseVmPage
            // 
            this.Controls.Add(this.treeView);
            this.Name = "ReplicationChoseVmPage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public override string HelpID
        {
            get
            {
                return "Select VM";
            }
        }

        public override string PageTitle
        {
            get
            {
                return Messages.SELECT_VM_REPLICATE;
            }
        }

        public override string Text
        {
            get
            {
                return Messages.CHOSE_VMS;
            }
        }

        private void BuildTreeView()
        {
            int index = 0;
            List<IXenConnection> xenConnectionsCopy = ConnectionsManager.XenConnectionsCopy;
            xenConnectionsCopy.Sort();
            foreach (XenConnection connection in xenConnectionsCopy)
            {
                VM[] vMs = connection.Cache.VMs.Where(vm => vm.uuid != null).ToArray();
                Array.Sort<VM>(vMs);
                Pool pool = Helpers.GetPool(connection);
                if (pool != null)
                {
                    bool flag2;
                    TreeNode parent = this.AddNode(this.treeView.Nodes[0], index, Helpers.GetName(pool), Images.GetIconFor(pool), false, pool, out flag2);
                    index++;
                    Host[] hosts = connection.Cache.Hosts;
                    Array.Sort<Host>(hosts);
                    int num2 = 0;
                    foreach (Host obj3 in hosts)
                    {
                        this.AddHostAndChildren(parent, num2, connection, vMs, obj3, false, true);
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
                Pool poolOfOne = Helpers.GetPoolOfOne(connection);
                Host theHost = (poolOfOne == null) ? null : poolOfOne.Connection.Resolve<Host>(poolOfOne.master);
                if (theHost == null)
                {
                    this.AddDisconnectedNode(this.treeView.Nodes[0], index, connection);
                }
                else
                {
                    this.AddHostAndChildren(this.treeView.Nodes[0], index, connection, vMs, theHost, true, false);
                }
                index++;
            }
            this.PurgeFromHere(this.treeView.Nodes[0], index);

            if (this.treeView.Nodes[0].Nodes.Count <= 0)
            {
                this.treeView.Enabled = false;
            }
            else
            {
                this.treeView.Nodes[0].Expand();
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
                this.treeView.SelectedNode = node;
                node.Checked = true;
            }
            return node;
        }

        private void AddDisconnectedNode(TreeNode parent, int index, IXenConnection connection)
        {
            bool flag;
            this.AddNode(parent, index, Helpers.GetName(connection), Images.GetIconFor(connection), true, connection, out flag);
        }

        private void AddHostAndChildren(TreeNode parent, int index, IXenConnection connection, VM[] vms, Host host, bool add_all, bool is_pool)
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
                                    if (vdi != null && vdi.Show(true))
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

        private void AddVMNode(TreeNode parent, int index, VM vm)
        {
            bool flag;

            string os_name = vm.GetOSName().ToLowerInvariant();
            Icons _icons = Icons.XenCenter;
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

        private int CheckedCount = 0;
        internal Dictionary<string, string> vdi_expand_list = new Dictionary<string, string>();

        private void CountNodeChecked(TreeNode currNode)
        {
            TreeNodeCollection nodesTemp = currNode.Nodes;
            VM vmTemp = null;
            VDI vdiTemp = null;
            if (nodesTemp.Count > 0)
            {
                foreach (TreeNode nodeTemp in nodesTemp)
                {
                    if (nodeTemp.Checked)
                    {
                        if (nodeTemp.Tag is VM)
                        {
                            vmTemp = nodeTemp.Tag as VM;
                        }
                        if (nodeTemp.Tag is VDI)
                        {
                            vdiTemp = nodeTemp.Tag as VDI;
                            string vdi_uuid = null;
                            if (nodeTemp.Parent.Tag is VM)
                            {
                                if (vdi_expand_list.ContainsKey(((VM)nodeTemp.Parent.Tag).uuid))
                                {
                                    vdi_uuid = vdi_expand_list[((VM)nodeTemp.Parent.Tag).uuid];
                                    vdi_expand_list[((VM)nodeTemp.Parent.Tag).uuid] = vdi_uuid + "@" + vdiTemp.uuid;
                                }
                                else
                                {
                                    vdi_expand_list.Add(((VM)nodeTemp.Parent.Tag).uuid, vdiTemp.uuid);
                                }

                                if (vdicheckDictionary.ContainsKey(((VM) nodeTemp.Parent.Tag).uuid))
                                {
                                    vdicheckDictionary[((VM) nodeTemp.Parent.Tag).uuid] += vdiTemp.virtual_size;
                                }
                                else
                                {
                                    vdicheckDictionary.Add(((VM)nodeTemp.Parent.Tag).uuid, vdiTemp.virtual_size);
                                }
                            }
                        }

                        if (vmTemp != null)
                        {
                            this.vmCheckedList.Add(vmTemp);
                            this.CheckedCount++;
                        }
                    }
                    CountNodeChecked(nodeTemp);
                }
            }
        }

        internal int CheckTreeView()
        {
            this.vmCheckedList.Clear();
            this.vdi_expand_list.Clear();
            this.vdicheckDictionary.Clear();
            foreach (TreeNode nodeTemp in this.treeView.Nodes)
            {
                CountNodeChecked(nodeTemp);
            }
            return CheckedCount;
        }

        private void TreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            const int SPACE_IL = 3; // space between Image and Label

            // we only do additional drawing
            e.DrawDefault = true;

            if (this.treeView.ShowLines && Images.ImageList16 != null &&
            e.Node.ImageIndex >= Images.ImageList16.Images.Count)
            {
                // Image size
                int imgW = Images.ImageList16.ImageSize.Width;
                int imgH = Images.ImageList16.ImageSize.Height;

                // Image center
                int xPos = e.Node.Bounds.Left - SPACE_IL - imgW / 2;
                int yPos = (e.Node.Bounds.Top + e.Node.Bounds.Bottom) / 2;

                using (Pen p = new Pen(this.treeView.LineColor, 1))
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    // draw the horizontal missing treeline 
                    e.Graphics.DrawLine(p, (xPos - imgW / 2), yPos, (xPos + imgW / 2), yPos);

                    if (!this.treeView.CheckBoxes && e.Node.IsExpanded)
                    {
                        // draw the vertical missing treeline
                        e.Graphics.DrawLine(p, xPos, yPos, xPos, (yPos + imgW / 2));
                    }
                }
            }
        }

        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (!this.treeView.CheckBoxes && Images.ImageList16 != null &&
                e.Node.ImageIndex >= Images.ImageList16.Images.Count)
            {
                this.treeView.Invalidate(e.Node.Bounds);
            }
        }

        private void TreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node != this.treeView.Nodes[0] && e.Node.Checked)
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
	}
}
