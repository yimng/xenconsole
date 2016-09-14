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
using XenAdmin.Core;
using XenAdmin.Network;


namespace XenAdmin.Wizards.ReplicationWizard_Pages
{
    public class ReplicationSynchronizationPage : XenTabPage
	{
        private IContainer components = null;
        private TreeView treeView;

        private List<VM> vmCheckedList;
        private XenConnection selected_xenConnection;
        private Host selected_host;
        private string selected_sr_uuid;
        private Dictionary<object, TreeNode> _nodes;
        private bool isMustNow = false;
        private bool isRunning = false;
        private int repCheckCount = 0;
        private int repSynchCount = 0;
        private int synchCount = 0;
        private List<string> repSelectVmUUidList;
        private List<Dictionary<string, string>> repCheckedList;
        private Dictionary<string, string> vdiCheckedDic;

        public ReplicationSynchronizationPage()
		{
			InitializeComponent();
            this.vmCheckedList = new List<VM>();
            this.vdiCheckedDic = new Dictionary<string, string>();
            this._nodes = new Dictionary<object, TreeNode>();
            this.repSelectVmUUidList = new List<string>();
            this.repCheckedList = new List<Dictionary<string, string>>();
            this.treeView.ImageList = Images.ImageList16;
            this.treeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.treeView.DrawNode += new DrawTreeNodeEventHandler(TreeView_DrawNode);
            this.treeView.Nodes[0].ImageIndex = 999;
            this.treeView.Nodes[0].SelectedImageIndex = 999;
		}

        internal bool IsMustNow
        {
            get
            {
                return this.isMustNow;
            }
        }

        internal  bool Is_VM_Running
        {
            get { return this.isRunning; }
        }

        internal List<Dictionary<string, string>> RepCheckedList
        {
            get
            {
                return this.repCheckedList;
            }
        }


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplicationSynchronizationPage));
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
            this.treeView.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeCheck);
            this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterCheck);
            this.treeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCollapse);
            // 
            // ReplicationSynchronizationPage
            // 
            this.Controls.Add(this.treeView);
            this.Name = "ReplicationSynchronizationPage";
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
                return "Set Synchronization";
            }
        }

        public override string PageTitle
        {
            get
            {
                return Messages.SET_SYNCHRONIZATION;
            }
        }

        public override string Text
        {
            get
            {
                return Messages.SET_SYNCHRONIZATION;
            }
        }

        public override void PageLoaded(PageLoadedDirection direction)
        {
            base.PageLoaded(direction);
            if (direction == PageLoadedDirection.Forward)
            {
                HelpersGUI.FocusFirstControl(base.Controls);
                this.BuildSynchronizationTreeView();
            }
        }

        internal void SettingValue(List<VM> vmCheckedList, Dictionary<string, string> vdiCheckedList, XenConnection _selected_xenConnection, Host _selected_host, string _selected_sr_uuid)
        {
            this.vmCheckedList = vmCheckedList;
            this.vdiCheckedDic = vdiCheckedList;
            this.selected_xenConnection = _selected_xenConnection;
            this.selected_host = _selected_host;
            this.selected_sr_uuid = _selected_sr_uuid;
        }

        private void BuildSynchronizationTreeView()
        {
            int index = 0;
            int vmIndex = this.vmCheckedList.Count;
            this.treeView.Nodes[0].Nodes.Clear();
            TreeNode parent = this.AddHostNode(this.treeView.Nodes[0], index, this.selected_xenConnection, this.selected_host);
            this.AddVmsToTree(parent, ref vmIndex, this.vmCheckedList);
            this.treeView.ExpandAll();
        }

        private TreeNode AddHostNode(TreeNode parent, int index, XenConnection connection, Host host)
        {
            bool expand;
            bool live;
            TreeNode node = this.AddHostNode(parent, index, connection, host, out expand, out live);
            //node.ForeColor = SystemColors.GrayText;
            node.Checked = true;
            return node;
        }

        private TreeNode AddHostNode(TreeNode parent, int index, XenConnection connection, Host host, out bool expand, out bool live)
        {
            Host_metrics host_metrics = host.Connection.Resolve<Host_metrics>(host.metrics);
            live = (host_metrics != null) && host_metrics.live;
            return this.AddNode(parent, index, host.name_label, Images.GetIconFor(host), false, host, out expand);
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
            
            return node;
        }

        private TreeNode AddVMNodes(TreeNode parent, int index, VM vm)
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
                _icons = Icons.Ubuntu;
            }
            else
            {
                _icons = Icons.XenCenter;
            }
            return this.AddNode(parent, index, vm.name_label, _icons, false, vm, out flag);
        }

        /// <summary>
        /// 1. Check that how many VDIs were checked for replication
        /// 2. Check whether the vms on destination host have the same amount of vdis on destination host
        /// 3. Check whether the vdis have key "src_vdi_uuid"
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="index"></param>
        /// <param name="vms"></param>
        private void AddVmsToTree(TreeNode parent, ref int index, List<VM> vms)
        {
            VM[] vMs =
                this.selected_xenConnection.Cache.VMs.Where(
                    vm =>
                    (vm.uuid != null && vm.is_a_real_vm && HalsignHelpers.VMHome(vm) != null &&
                     HalsignHelpers.VMHome(vm) == this.selected_host)).ToArray();

            foreach (VM vm in vms)
            {
                TreeNode nodeSelectedVM = this.AddVMNodes(parent, index, vm);
                TreeNode newNode = new TreeNode(Messages.NEW_REPLICATION);
                newNode.ImageIndex = 999;
                newNode.SelectedImageIndex = 999;
                newNode.Tag = "new";
                newNode.Checked = true;
                nodeSelectedVM.Nodes.Add(newNode);
                List<string> sourceVdiList = this.vdiCheckedDic[vm.uuid].Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();

                foreach (VM tempVm in vMs)
                {
                    bool isOperationAllowed = (!tempVm.is_a_template && HalsignHelpers.IsVMShow(tempVm) && tempVm.allowed_operations.Count > 0);
                    if (isOperationAllowed)
                    {
                        List<VDI> destVdiList = new List<VDI>();
                        foreach (VBD vbd in tempVm.Connection.ResolveAll<VBD>(tempVm.VBDs).ToArray())
                        {
                            if (HalsignHelpers.IsCDROM(vbd))
                            {
                                continue;
                            }
                            VDI vdi = tempVm.Connection.Resolve(vbd.VDI);
                            SR sr = tempVm.Connection.Resolve(vdi.SR);
                            if (vdi != null && vdi.Show(true) && !string.IsNullOrEmpty(vdi.uuid) && (sr != null && sr.uuid == this.selected_sr_uuid))
                            {
                                if (null != vdi.uuid && !vdi.uuid.Equals(""))
                                {
                                    destVdiList.Add(vdi);
                                }
                            }
                        }
                        if (sourceVdiList.Count == destVdiList.Count)
                        {
                            int vdiiNo = 0;
                            foreach (VDI vdii in destVdiList)
                            {
                                if (vdii.other_config.ContainsKey("src_vdi_uuid"))
                                {
                                    foreach (string uuid in sourceVdiList)
                                    {
                                        if (vdii.other_config["src_vdi_uuid"].Equals(uuid))
                                        {
                                            vdiiNo++;
                                        }
                                    }
                                }
                            }
                            if (vdiiNo == destVdiList.Count)
                            {
                                TreeNode nodeTemp = new TreeNode(tempVm.name_label);
                                nodeTemp.ImageIndex = 999;
                                nodeTemp.SelectedImageIndex = 999;
                                nodeTemp.Tag = new ReplicatedVmState(tempVm.uuid, tempVm.power_state == vm_power_state.Running);
                                nodeSelectedVM.Nodes.Add(nodeTemp);
                            }
                        }
                    }
                }
                index++;
            }
        }

        private void CountRepNodeChecked(TreeNode currNode)
        {
            TreeNodeCollection nodesTemp = currNode.Nodes;
            if (nodesTemp.Count > 0)
            {
                foreach (TreeNode nodeTemp in nodesTemp)
                {
                    if (null != nodeTemp.Tag)
                    {
                        if (nodeTemp.Tag is VM)
                        {
                            repSelectVmUUidList.Add("" + ((VM)(nodeTemp.Tag)).uuid);
                        }
                        if (nodeTemp.Nodes.Count <= 1 && nodeTemp.Checked && (nodeTemp.Tag is string || nodeTemp.Tag is ReplicatedVmState))
                        {
                            Dictionary<string, string> t1 = new Dictionary<string, string>();
                            if (nodeTemp.Tag is string)
                            {
                                t1.Add(((VM)nodeTemp.Parent.Tag).uuid, nodeTemp.Tag.ToString());
                            }
                            else
                            {
                                ReplicatedVmState state = nodeTemp.Tag as ReplicatedVmState;
                                t1.Add(((VM)nodeTemp.Parent.Tag).uuid, state.VmUuid);
                                this.isMustNow = true;
                                this.isRunning = state.IsVMRunning;
                            }
                            this.repSynchCount++;
                            repCheckedList.Add(t1);
                            repCheckCount++;
                        }                      
                    }
                    CountRepNodeChecked(nodeTemp);
                }
            }
        }

        public bool CheckCloseFromNext()
        {
            bool CheckConfiguration = true;
            this.repSelectVmUUidList.Clear();
            this.repCheckedList.Clear();
            this.synchCount = 0;
            this.isMustNow = false;
            this.repCheckCount = 0;
            this.repSynchCount = 0;
            foreach (TreeNode nodeTemp in this.treeView.Nodes)
            {
                CountRepNodeChecked(nodeTemp);
            }
            if (repCheckCount < 1 || !CanGotoNext())
            {
                CheckConfiguration = false;
            }

            return CheckConfiguration;
        }

        private bool CanGotoNext()
        {
            int len = repSelectVmUUidList.Count;
            synchCount = 0;
            foreach (string uuid in this.repSelectVmUUidList)
            {
                foreach (Dictionary<string, string> dic in this.repCheckedList)
                {
                    if (dic.ContainsKey(uuid))
                    {
                        synchCount++;
                        break;
                    }
                }
            }
            if (synchCount == len)
            {
                return true;
            }
            return false;
        }

        private void treeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
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
                if(e.Node.Parent != null && e.Node.Parent.Tag is VM)
                {
                    if(e.Node.Parent.Nodes.Count > 1)
                        foreach (TreeNode node in e.Node.Parent.Nodes)
                        {
                            if (node != e.Node)
                                node.Checked = false;
                        }
                }
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
                if(nodeTemp.Nodes.Count == 0)
                    break;
                SetChildNodeChecked(nodeTemp, state);
            }
        }
    }
}
