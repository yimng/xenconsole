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
using XenAdmin.Controls;
using HalsignModel;

namespace XenAdmin.Wizards.RestoreWizard_Pages
{
    public partial class Page_SelectVM : XenTabPage
	{
        private Label Label_ToRestore;
        private RadioButton RadioButton_Latest;
        private RadioButton RadioButton_ByBackup;
        private TreeView TreeView_Restore;
        private int NodeCheckedCount;
        internal Dictionary<string, List<BackupRestoreConfig.RestoreInfo>> restore_vdi_info_list;
    
		public Page_SelectVM()
		{
			InitializeComponent();
            _vmCheckedList = new List<AgentParamDataModel>();
            _Backup_info_list = new Dictionary<string, string>();
            restore_vdi_info_list = new Dictionary<string, List<BackupRestoreConfig.RestoreInfo>>();
            this.TreeView_Restore.ImageList = Images.ImageList16;
            this.TreeView_Restore.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.TreeView_Restore.DrawNode += new DrawTreeNodeEventHandler(TreeView_DrawNode);
		}

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page_SelectVM));
            this.Label_ToRestore = new System.Windows.Forms.Label();
            this.RadioButton_Latest = new System.Windows.Forms.RadioButton();
            this.RadioButton_ByBackup = new System.Windows.Forms.RadioButton();
            this.TreeView_Restore = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // Label_ToRestore
            // 
            this.Label_ToRestore.AccessibleDescription = null;
            this.Label_ToRestore.AccessibleName = null;
            resources.ApplyResources(this.Label_ToRestore, "Label_ToRestore");
            this.Label_ToRestore.Font = null;
            this.Label_ToRestore.Name = "Label_ToRestore";
            // 
            // RadioButton_Latest
            // 
            this.RadioButton_Latest.AccessibleDescription = null;
            this.RadioButton_Latest.AccessibleName = null;
            resources.ApplyResources(this.RadioButton_Latest, "RadioButton_Latest");
            this.RadioButton_Latest.BackgroundImage = null;
            this.RadioButton_Latest.Font = null;
            this.RadioButton_Latest.Name = "RadioButton_Latest";
            this.RadioButton_Latest.UseVisualStyleBackColor = true;
            this.RadioButton_Latest.CheckedChanged += new System.EventHandler(this.RadioButton_Latest_CheckedChanged);
            // 
            // RadioButton_ByBackup
            // 
            this.RadioButton_ByBackup.AccessibleDescription = null;
            this.RadioButton_ByBackup.AccessibleName = null;
            resources.ApplyResources(this.RadioButton_ByBackup, "RadioButton_ByBackup");
            this.RadioButton_ByBackup.BackgroundImage = null;
            this.RadioButton_ByBackup.Checked = true;
            this.RadioButton_ByBackup.Font = null;
            this.RadioButton_ByBackup.Name = "RadioButton_ByBackup";
            this.RadioButton_ByBackup.TabStop = true;
            this.RadioButton_ByBackup.UseVisualStyleBackColor = true;
            this.RadioButton_ByBackup.CheckedChanged += new System.EventHandler(this.RadioButton_ByBackup_CheckedChanged);
            // 
            // TreeView_Restore
            // 
            this.TreeView_Restore.AccessibleDescription = null;
            this.TreeView_Restore.AccessibleName = null;
            resources.ApplyResources(this.TreeView_Restore, "TreeView_Restore");
            this.TreeView_Restore.BackgroundImage = null;
            this.TreeView_Restore.CheckBoxes = true;
            this.TreeView_Restore.Font = null;
            this.TreeView_Restore.Name = "TreeView_Restore";
            this.TreeView_Restore.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("TreeView_Restore.Nodes")))});
            this.TreeView_Restore.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_Restore_AfterCheck);
            this.TreeView_Restore.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_Restore_AfterCollapse);
            this.TreeView_Restore.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeView_Restore_BeforeCheck);
            // 
            // Page_SelectVM
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.TreeView_Restore);
            this.Controls.Add(this.RadioButton_ByBackup);
            this.Controls.Add(this.RadioButton_Latest);
            this.Controls.Add(this.Label_ToRestore);
            this.Font = null;
            this.Name = "Page_SelectVM";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public override string PageTitle
        {
            get
            {
                return Messages.RESTORE_SELECT_VM_TITLE;
            }
        }

        public override string Text
        {
            get
            {
                return Messages.RESTORE_SELECT_VM_TEXT;
            }
        }

        public override bool EnableNext()
        {
            return (NodeCheckedCount > 0);
        }

        public override void PageLeave(PageLoadedDirection direction, ref bool cancel)
        {
            if (direction == PageLoadedDirection.Back || cancel)
            {
                this.NodeCheckedCount = 0;
                this.RadioButton_ByBackup.Checked = true;
            }
            base.PageLeave(direction, ref cancel);
        }

        private void TreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            const int SPACE_IL = 3; // space between Image and Label

            // we only do additional drawing
            e.DrawDefault = true;

            if (this.TreeView_Restore.ShowLines && Images.ImageList16 != null &&
            e.Node.ImageIndex >= Images.ImageList16.Images.Count)
            {
                // Image size
                int imgW = Images.ImageList16.ImageSize.Width;
                int imgH = Images.ImageList16.ImageSize.Height;

                // Image center
                int xPos = e.Node.Bounds.Left - SPACE_IL - imgW / 2;
                int yPos = (e.Node.Bounds.Top + e.Node.Bounds.Bottom) / 2;

                using (Pen p = new Pen(this.TreeView_Restore.LineColor, 1))
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    // draw the horizontal missing treeline 
                    e.Graphics.DrawLine(p, (xPos - imgW / 2), yPos, (xPos + imgW / 2), yPos);

                    if (!this.TreeView_Restore.CheckBoxes && e.Node.IsExpanded)
                    {
                        // draw the vertical missing treeline
                        e.Graphics.DrawLine(p, xPos, yPos, xPos, (yPos + imgW / 2));
                    }
                }
            }
        }

        private void AddNodeImage(TreeNode node, string os_version)
        {
            if (os_version.ToLower().Contains("windows"))
            {
                node.ImageKey = "windows_h32bit_16.png";
                node.SelectedImageKey = "windows_h32bit_16.png";
            }
            else if (os_version.ToLower().Contains("debian"))
            {
                node.ImageKey = "debian_16x.png";
                node.SelectedImageKey = "debian_16x.png";
            }
            else if (os_version.ToLower().Contains("red"))
            {
                node.ImageKey = "redhat_16x.png";
                node.SelectedImageKey = "redhat_16x.png";
            }
            else if (os_version.ToLower().Contains("cent"))
            {
                node.ImageKey = "centos_16x.png";
                node.SelectedImageKey = "centos_16x.png";
            }
            else if (os_version.ToLower().Contains("oracle"))
            {
                node.ImageKey = "oracle_16x.png";
                node.SelectedImageKey = "oracle_16x.png";
            }
            else if (os_version.ToLower().Contains("suse"))
            {
                node.ImageKey = "suse_16x.png";
                node.SelectedImageKey = "suse_16x.png";
            }
            else if (os_version.ToLower().Contains("ubuntu"))
            {
                node.ImageKey = "ubuntu_16x.png";
                node.SelectedImageKey = "ubuntu_16x.png";
            }
            else
            {
                node.ImageIndex = 999;
                node.SelectedImageIndex = 999;
            }
        }

        public void BuildRestoreTree()
        {
            this._Backup_info_list.Clear();
            this._restore_vdi_info_list.Clear();

            this.TreeView_Restore.BeginUpdate();
            this.TreeView_Restore.Enabled = true;
            this.TreeView_Restore.Nodes.Clear();

            TreeNode rootNode = new TreeNode(Messages.RESTORE_LIST);
            rootNode.ImageIndex = rootNode.SelectedImageIndex = 999;
            this.TreeView_Restore.Nodes.Add(rootNode);

            foreach (var vmitem in this.restoreList.items)
            {
                TreeNode vmnode = new TreeNode(vmitem.name);
                AddNodeImage(vmnode, vmitem.osversion);
                rootNode.Nodes.Add(vmnode);
                if (vmitem.children == null)
                    continue;
                foreach (var fullitem in vmitem.children)
                {
                    TreeNode fullnode = new TreeNode(string.Format("{0} ({1})", fullitem.name, fullitem.time_str));
                    fullnode.SelectedImageIndex = fullnode.ImageIndex = 999;                   
                    vmnode.Nodes.Add(fullnode);
                    string fullitemKey = fullitem.parentuuid + "/" + fullitem.uuid + "/" +
                                         fullitem.timestamp;
                    fullnode.Tag = new AgentParamDataModel(fullitem.root_path, fullitemKey); ;
                    this._Backup_info_list.Add(fullitemKey, fullitem.content + "|" + fullitem.name);
                    if (fullitem.content.Contains("UUID"))
                    {
                        CheckUuid(fullitemKey, fullnode, fullitem.content);
                    }

                    if(fullitem.children == null)
                        continue;
                    foreach (var incrementitem in fullitem.children)
                    {
                        TreeNode incrementnode = new TreeNode(incrementitem.time_str);
                        incrementnode.SelectedImageIndex = incrementnode.ImageIndex = 999;
                        fullnode.Nodes.Add(incrementnode);
                        string incrementitemKey = incrementitem.parentuuid + "/" +
                                                  incrementitem.uuid + "/" + incrementitem.timestamp;
                        incrementnode.Tag = new AgentParamDataModel(incrementitem.root_path, incrementitemKey);
                        this._Backup_info_list.Add(incrementitemKey, incrementitem.content + "|" + incrementitem.name);
                        if (incrementitem.content.Contains("UUID"))
                        {
                            CheckUuid(incrementitemKey, incrementnode, incrementitem.content);
                        }
                    }
                }
            }

            this.TreeView_Restore.EndUpdate();

            if (rootNode.Nodes.Count == 0)
            {
                this.TreeView_Restore.Enabled = false;
            }
            else
            {
                this.TreeView_Restore.ExpandAll();
            }
        }

        public void CheckTreeViewStatus()
        {
            this._vmCheckedList.Clear();

            foreach (TreeNode nodeTemp in this.TreeView_Restore.Nodes)
            {
                CheckTreeViewStatus(nodeTemp);
            }
        }

        private void CheckUuid(string uuidKey, TreeNode node, string content)
        {
            if (!content.Contains("UUID"))
            {
                return;
            }
            string[] strList = content.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < strList.Length - 1; i += 4)
            {
                List<BackupRestoreConfig.RestoreInfo> list = null;
                BackupRestoreConfig.RestoreInfo restoreInfo = new BackupRestoreConfig.RestoreInfo();

                int nameIndex = strList[i].IndexOf("Name", System.StringComparison.OrdinalIgnoreCase);
                string name = strList[i].Substring(nameIndex + 5);

                int storageIndex = strList[i + 1].IndexOf("Storage", System.StringComparison.OrdinalIgnoreCase);
                string storage = strList[i + 1].Substring(storageIndex + 8);

                int uuidIndex = strList[i + 2].IndexOf("UUID", System.StringComparison.OrdinalIgnoreCase);
                string uuid = strList[i + 2].Substring(uuidIndex + 5);

                restoreInfo.name = name;
                restoreInfo.storage = storage;
                restoreInfo.uuid = uuid;

                if (restore_vdi_info_list.ContainsKey(uuidKey))
                {
                    list = restore_vdi_info_list[uuidKey];
                    list.Add(restoreInfo);
                    restore_vdi_info_list[uuidKey] = list;
                }
                else
                {
                    list = new List<BackupRestoreConfig.RestoreInfo> {restoreInfo};
                    restore_vdi_info_list.Add(uuidKey, list);
                }
            }
        }

        private void BuildRestoreTreeByLatest()
        {
            this.TreeView_Restore.Enabled = true;
            this.TreeView_Restore.Nodes.Clear();
            TreeNode rootNode = new TreeNode(Messages.RESTORE_LIST) {ImageIndex = 999, SelectedImageIndex = 999};
            this.TreeView_Restore.Nodes.Add(rootNode);

            foreach (var vmitem in this.restoreList.items)
            {
                List<RestoreLatestDataModel> items = new List<RestoreLatestDataModel>();

                TreeNode vmnode = new TreeNode(vmitem.name);
                AddNodeImage(vmnode, vmitem.osversion);
                rootNode.Nodes.Add(vmnode);
                if (vmitem.children == null)
                    continue;
                foreach (var fullitem in vmitem.children)
                {
                    items.Add(new RestoreLatestDataModel(fullitem.timestamp, fullitem.time_str,
                                                         new AgentParamDataModel(fullitem.root_path,
                                                                                 fullitem.parentuuid + "/" +
                                                                                 fullitem.uuid + "/" +
                                                                                 fullitem.timestamp)));

                    if (fullitem.children != null)
                    {
                        items.AddRange(
                            fullitem.children.Select(
                                incrementitem =>
                                new RestoreLatestDataModel(incrementitem.timestamp, incrementitem.time_str,
                                                           new AgentParamDataModel(incrementitem.root_path,
                                                                                   incrementitem.parentuuid + "/" +
                                                                                   incrementitem.uuid + "/" +
                                                                                   incrementitem.timestamp))));
                    }
                }

                if(items.Count > 0)
                {
                    var sortedItems = from restoreLatestDataModel in items
                                      orderby restoreLatestDataModel.timestamp descending
                                      select restoreLatestDataModel;
                    foreach (var restoreLatestDataModel in sortedItems)
                    {
                        TreeNode node = new TreeNode(restoreLatestDataModel.time_str)
                        {
                            SelectedImageIndex = 999,
                            ImageIndex = 999,
                            Tag = restoreLatestDataModel.agent_model
                        };
                        vmnode.Nodes.Add(node);
                    }
                }

                if (this.TreeView_Restore.Nodes[0].Nodes.Count <= 0)
                {
                    this.TreeView_Restore.Enabled = false;
                }
                else
                {
                    this.TreeView_Restore.ExpandAll();
                }
            }
        }

        private void CheckTreeViewStatus(TreeNode currNode)
        {
            TreeNodeCollection nodesTemp = currNode.Nodes;
            if (nodesTemp.Count > 0)
            {
                foreach (TreeNode nodeTemp in nodesTemp)
                {
                    if (nodeTemp.Checked)
                    {
                        if (nodeTemp.Tag == null)
                        {
                            continue;
                        }
                        if (nodeTemp.Tag is AgentParamDataModel)
                        {
                            this._vmCheckedList.Add((nodeTemp.Tag as AgentParamDataModel));
                        }
                    }
                    CheckTreeViewStatus(nodeTemp);
                }
            }
        }

        private BackupRestoreConfig.RestoreListInfo restoreList;
        public  BackupRestoreConfig.RestoreListInfo RestoreList
        {
            get { return this.restoreList; }
            set { this.restoreList = value; }
        }

        private Dictionary<string, string> _Backup_info_list;
        public Dictionary<string, string> Backup_info_list
        {
            get { return _Backup_info_list; }
            set { _Backup_info_list = value; }
        }

        private List<AgentParamDataModel> _vmCheckedList;
        public List<AgentParamDataModel> vmCheckedList
        {
            get { return _vmCheckedList; }
            set { _vmCheckedList = value; }
        }

        public Dictionary<string, List<BackupRestoreConfig.RestoreInfo>> _restore_vdi_info_list
        {
            get { return restore_vdi_info_list; }
            set { _restore_vdi_info_list = value; }
        }

        #region Events

        private void RadioButton_ByBackup_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButton_ByBackup.Checked)
            {
                this.NodeCheckedCount = 0;
                this.vmCheckedList.Clear();
                this.BuildRestoreTree();
                base.OnPageUpdated();
            }
        }

        private void RadioButton_Latest_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioButton_Latest.Checked)
            {
                this.NodeCheckedCount = 0;
                this.vmCheckedList.Clear();
                this.BuildRestoreTreeByLatest();
                base.OnPageUpdated();
            }
        }

        private void TreeView_Restore_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.ForeColor == SystemColors.GrayText)
                {
                    e.Cancel = true;
                }

                if (e.Node.Tag != null)
                {
                    TreeNode parent = (e.Node.Parent.Tag == null) ? e.Node.Parent : e.Node.Parent.Parent;
                    CheckNodeState(e.Node, parent);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void CheckNodeState(TreeNode current, TreeNode parent)
        {
            foreach (TreeNode fullitemnode in parent.Nodes)
            {
                if (current != fullitemnode)
                {
                    if (fullitemnode.Checked)
                    {
                        this.NodeCheckedCount--;
                    }
                    fullitemnode.Checked = false;
                }
                foreach (TreeNode incrementnode in fullitemnode.Nodes)
                {
                    if (incrementnode.Checked)
                    {
                        this.NodeCheckedCount--;
                    }
                    incrementnode.Checked = false;
                }
            }
        }

        private void TreeView_Restore_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Checked && e.Node.Tag != null)
                {
                    this.NodeCheckedCount++;
                }
                if (!e.Node.Checked && e.Node.Tag != null)
                {
                    this.NodeCheckedCount--;
                }

                base.OnPageUpdated();
            }
        }

        #endregion

        private void TreeView_Restore_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (!this.TreeView_Restore.CheckBoxes && Images.ImageList16 != null &&
                e.Node.ImageIndex >= Images.ImageList16.Images.Count)
            {
                this.TreeView_Restore.Invalidate(e.Node.Bounds);
            }
        }
    }
}
