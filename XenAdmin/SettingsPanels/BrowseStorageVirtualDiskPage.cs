namespace XenAdmin.SettingsPanels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using XenAPI;
    using XenAdmin.Controls;
    using XenAdmin.Commands;
    using XenAdmin.Dialogs;
    using XenAdmin.Actions;
    using XenAdmin.Core;
    using XenAdmin.Properties;

    public class BrowseStorageVirtualDiskPage : UserControl, IEditPage, VerticalTabs.VerticalTab
    {
        private ContextMenuStrip addContextMenuStrip;
        private DropDownButton addDropDownButton;
        private Button addVirtualDiskButton;
        private Button buttonMove;
        private Button buttonRefresh;
        private IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private DataGridView dataGridViewVDIs;
        private ToolStripMenuItem deleteVirtualDiskToolStripMenuItem;
        private Button EditButton;
        private ToolTipContainer EditButtonContainer;
        private ToolStripMenuItem editVirtualDiskToolStripMenuItem;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox groupBox1;
        private ToolStripMenuItem moveVirtualDiskToolStripMenuItem;
        private Panel panel1;
        private bool rebuildRequired;
        private Button RemoveButton;
        private ToolTipContainer RemoveButtonContainer;
        private ContextMenuStrip removeContextMenuStrip;
        private DropDownButton removeDropDownButton;
        private readonly DataGridViewColumn sizeColumn;
        private XenAPI.SR sr;
        private readonly DataGridViewColumn storageLinkVolumeColumn;
        private ToolStripSeparator toolStripSeparator1;
        private ToolTipContainer toolTipContainerMove;
        private DataGridViewTextBoxColumn ColumnName;
        private DataGridViewTextBoxColumn ColumnVolume;
        private DataGridViewTextBoxColumn ColumnDesc;
        private DataGridViewTextBoxColumn ColumnSize;
        private DataGridViewTextBoxColumn ColumnVM;
        private ToolTipContainer toolTipContainerRescan;

        public BrowseStorageVirtualDiskPage()
        {
            this.InitializeComponent();
            this.Text = Messages.NAME_DESCRIPTION_BROWSE_DISK;
            this.storageLinkVolumeColumn = this.ColumnVolume;
            this.sizeColumn = this.ColumnSize;
            for (int i = 0; i < 5; i++)
            {
                this.dataGridViewVDIs.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
            }
            this.dataGridViewVDIs.SortCompare += new DataGridViewSortCompareEventHandler(this.DataGridViewObject_SortCompare);
            this.dataGridViewVDIs.SelectionChanged += new EventHandler(this.dataGridViewVDIs_SelectedIndexChanged);
            this.dataGridViewVDIs.MouseUp += new MouseEventHandler(this.dataGridViewVDIs_MouseUp);
            this.dataGridViewVDIs.KeyUp += new KeyEventHandler(this.dataGridViewVDIs_KeyUp);
            ConnectionsManager.History.CollectionChanged += new CollectionChangeEventHandler(this.History_CollectionChanged);
            base.Text = Messages.VIRTUAL_DISKS;
            XenAdmin.Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(this.Default_PropertyChanged);
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowseStorageVirtualDiskPage));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.moveVirtualDiskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteVirtualDiskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridViewVDIs = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTipContainerRescan = new ToolTipContainer();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.addVirtualDiskButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EditButtonContainer = new ToolTipContainer();
            this.EditButton = new System.Windows.Forms.Button();
            this.toolTipContainerMove = new ToolTipContainer();
            this.buttonMove = new System.Windows.Forms.Button();
            this.RemoveButtonContainer = new ToolTipContainer();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addDropDownButton = new XenAdmin.Controls.DropDownButton();
            this.removeDropDownButton = new XenAdmin.Controls.DropDownButton();
            this.editVirtualDiskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVDIs)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.toolTipContainerRescan.SuspendLayout();
            this.EditButtonContainer.SuspendLayout();
            this.toolTipContainerMove.SuspendLayout();
            this.RemoveButtonContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AccessibleDescription = null;
            this.contextMenuStrip1.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.BackgroundImage = null;
            this.contextMenuStrip1.Font = null;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveVirtualDiskToolStripMenuItem,
            this.deleteVirtualDiskToolStripMenuItem,
            this.toolStripSeparator1,
            this.editVirtualDiskToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // moveVirtualDiskToolStripMenuItem
            // 
            this.moveVirtualDiskToolStripMenuItem.AccessibleDescription = null;
            this.moveVirtualDiskToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.moveVirtualDiskToolStripMenuItem, "moveVirtualDiskToolStripMenuItem");
            this.moveVirtualDiskToolStripMenuItem.BackgroundImage = null;
            this.moveVirtualDiskToolStripMenuItem.Name = "moveVirtualDiskToolStripMenuItem";
            this.moveVirtualDiskToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.moveVirtualDiskToolStripMenuItem.Click += new System.EventHandler(this.moveVirtualDiskToolStripMenuItem_Click);
            // 
            // deleteVirtualDiskToolStripMenuItem
            // 
            this.deleteVirtualDiskToolStripMenuItem.AccessibleDescription = null;
            this.deleteVirtualDiskToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.deleteVirtualDiskToolStripMenuItem, "deleteVirtualDiskToolStripMenuItem");
            this.deleteVirtualDiskToolStripMenuItem.BackgroundImage = null;
            this.deleteVirtualDiskToolStripMenuItem.Name = "deleteVirtualDiskToolStripMenuItem";
            this.deleteVirtualDiskToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.deleteVirtualDiskToolStripMenuItem.Click += new System.EventHandler(this.removeVirtualDisk_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleDescription = null;
            this.toolStripSeparator1.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // addContextMenuStrip
            // 
            this.addContextMenuStrip.AccessibleDescription = null;
            this.addContextMenuStrip.AccessibleName = null;
            resources.ApplyResources(this.addContextMenuStrip, "addContextMenuStrip");
            this.addContextMenuStrip.BackgroundImage = null;
            this.addContextMenuStrip.Font = null;
            this.addContextMenuStrip.Name = "addContextMenuStrip";
            this.addContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.addContextMenuStrip_Opening);
            // 
            // removeContextMenuStrip
            // 
            this.removeContextMenuStrip.AccessibleDescription = null;
            this.removeContextMenuStrip.AccessibleName = null;
            resources.ApplyResources(this.removeContextMenuStrip, "removeContextMenuStrip");
            this.removeContextMenuStrip.BackgroundImage = null;
            this.removeContextMenuStrip.Font = null;
            this.removeContextMenuStrip.Name = "removeContextMenuStrip";
            this.removeContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.removeContextMenuStrip_Opening);
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.dataGridViewVDIs);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Font = null;
            this.panel1.MaximumSize = new System.Drawing.Size(900, 400);
            this.panel1.Name = "panel1";
            // 
            // dataGridViewVDIs
            // 
            this.dataGridViewVDIs.AccessibleDescription = null;
            this.dataGridViewVDIs.AccessibleName = null;
            this.dataGridViewVDIs.AllowUserToAddRows = false;
            this.dataGridViewVDIs.AllowUserToDeleteRows = false;
            this.dataGridViewVDIs.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dataGridViewVDIs, "dataGridViewVDIs");
            this.dataGridViewVDIs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewVDIs.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewVDIs.BackgroundImage = null;
            this.dataGridViewVDIs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewVDIs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVDIs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnVolume,
            this.ColumnDesc,
            this.ColumnSize,
            this.ColumnVM});
            this.dataGridViewVDIs.Font = null;
            this.dataGridViewVDIs.Name = "dataGridViewVDIs";
            this.dataGridViewVDIs.ReadOnly = true;
            this.dataGridViewVDIs.RowHeadersVisible = false;
            this.dataGridViewVDIs.RowTemplate.Height = 23;
            this.dataGridViewVDIs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AccessibleDescription = null;
            this.flowLayoutPanel1.AccessibleName = null;
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackgroundImage = null;
            this.flowLayoutPanel1.Controls.Add(this.toolTipContainerRescan);
            this.flowLayoutPanel1.Controls.Add(this.addVirtualDiskButton);
            this.flowLayoutPanel1.Controls.Add(this.addDropDownButton);
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Controls.Add(this.EditButtonContainer);
            this.flowLayoutPanel1.Controls.Add(this.toolTipContainerMove);
            this.flowLayoutPanel1.Controls.Add(this.RemoveButtonContainer);
            this.flowLayoutPanel1.Controls.Add(this.removeDropDownButton);
            this.flowLayoutPanel1.Font = null;
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // toolTipContainerRescan
            // 
            this.toolTipContainerRescan.AccessibleDescription = null;
            this.toolTipContainerRescan.AccessibleName = null;
            resources.ApplyResources(this.toolTipContainerRescan, "toolTipContainerRescan");
            this.toolTipContainerRescan.BackgroundImage = null;
            this.toolTipContainerRescan.Controls.Add(this.buttonRefresh);
            this.toolTipContainerRescan.Font = null;
            this.toolTipContainerRescan.Name = "toolTipContainerRescan";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.AccessibleDescription = null;
            this.buttonRefresh.AccessibleName = null;
            resources.ApplyResources(this.buttonRefresh, "buttonRefresh");
            this.buttonRefresh.BackgroundImage = null;
            this.buttonRefresh.Font = null;
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // addVirtualDiskButton
            // 
            this.addVirtualDiskButton.AccessibleDescription = null;
            this.addVirtualDiskButton.AccessibleName = null;
            resources.ApplyResources(this.addVirtualDiskButton, "addVirtualDiskButton");
            this.addVirtualDiskButton.BackgroundImage = null;
            this.addVirtualDiskButton.Font = null;
            this.addVirtualDiskButton.Name = "addVirtualDiskButton";
            this.addVirtualDiskButton.UseVisualStyleBackColor = true;
            this.addVirtualDiskButton.Click += new System.EventHandler(this.addVirtualDiskButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // EditButtonContainer
            // 
            this.EditButtonContainer.AccessibleDescription = null;
            this.EditButtonContainer.AccessibleName = null;
            resources.ApplyResources(this.EditButtonContainer, "EditButtonContainer");
            this.EditButtonContainer.BackgroundImage = null;
            this.EditButtonContainer.Controls.Add(this.EditButton);
            this.EditButtonContainer.Font = null;
            this.EditButtonContainer.Name = "EditButtonContainer";
            // 
            // EditButton
            // 
            this.EditButton.AccessibleDescription = null;
            this.EditButton.AccessibleName = null;
            resources.ApplyResources(this.EditButton, "EditButton");
            this.EditButton.BackgroundImage = null;
            this.EditButton.Font = null;
            this.EditButton.Name = "EditButton";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // toolTipContainerMove
            // 
            this.toolTipContainerMove.AccessibleDescription = null;
            this.toolTipContainerMove.AccessibleName = null;
            resources.ApplyResources(this.toolTipContainerMove, "toolTipContainerMove");
            this.toolTipContainerMove.BackgroundImage = null;
            this.toolTipContainerMove.Controls.Add(this.buttonMove);
            this.toolTipContainerMove.Font = null;
            this.toolTipContainerMove.Name = "toolTipContainerMove";
            // 
            // buttonMove
            // 
            this.buttonMove.AccessibleDescription = null;
            this.buttonMove.AccessibleName = null;
            resources.ApplyResources(this.buttonMove, "buttonMove");
            this.buttonMove.BackgroundImage = null;
            this.buttonMove.Font = null;
            this.buttonMove.Name = "buttonMove";
            this.buttonMove.UseVisualStyleBackColor = true;
            this.buttonMove.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // RemoveButtonContainer
            // 
            this.RemoveButtonContainer.AccessibleDescription = null;
            this.RemoveButtonContainer.AccessibleName = null;
            resources.ApplyResources(this.RemoveButtonContainer, "RemoveButtonContainer");
            this.RemoveButtonContainer.BackgroundImage = null;
            this.RemoveButtonContainer.Controls.Add(this.RemoveButton);
            this.RemoveButtonContainer.Font = null;
            this.RemoveButtonContainer.Name = "RemoveButtonContainer";
            // 
            // RemoveButton
            // 
            this.RemoveButton.AccessibleDescription = null;
            this.RemoveButton.AccessibleName = null;
            resources.ApplyResources(this.RemoveButton, "RemoveButton");
            this.RemoveButton.BackgroundImage = null;
            this.RemoveButton.Font = null;
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // ColumnName
            // 
            this.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColumnName, "ColumnName");
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // ColumnVolume
            // 
            this.ColumnVolume.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColumnVolume, "ColumnVolume");
            this.ColumnVolume.Name = "ColumnVolume";
            this.ColumnVolume.ReadOnly = true;
            // 
            // ColumnDesc
            // 
            this.ColumnDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColumnDesc, "ColumnDesc");
            this.ColumnDesc.Name = "ColumnDesc";
            this.ColumnDesc.ReadOnly = true;
            // 
            // ColumnSize
            // 
            this.ColumnSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.ColumnSize, "ColumnSize");
            this.ColumnSize.Name = "ColumnSize";
            this.ColumnSize.ReadOnly = true;
            // 
            // ColumnVM
            // 
            this.ColumnVM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColumnVM, "ColumnVM");
            this.ColumnVM.Name = "ColumnVM";
            this.ColumnVM.ReadOnly = true;
            // 
            // addDropDownButton
            // 
            this.addDropDownButton.AccessibleDescription = null;
            this.addDropDownButton.AccessibleName = null;
            resources.ApplyResources(this.addDropDownButton, "addDropDownButton");
            this.addDropDownButton.BackgroundImage = null;
            this.addDropDownButton.Font = null;
            this.addDropDownButton.Name = "addDropDownButton";
            this.addDropDownButton.UseVisualStyleBackColor = true;
            // 
            // removeDropDownButton
            // 
            this.removeDropDownButton.AccessibleDescription = null;
            this.removeDropDownButton.AccessibleName = null;
            resources.ApplyResources(this.removeDropDownButton, "removeDropDownButton");
            this.removeDropDownButton.BackgroundImage = null;
            this.removeDropDownButton.ContextMenuStrip = this.removeContextMenuStrip;
            this.removeDropDownButton.Font = null;
            this.removeDropDownButton.Name = "removeDropDownButton";
            this.removeDropDownButton.UseVisualStyleBackColor = true;
            // 
            // editVirtualDiskToolStripMenuItem
            // 
            this.editVirtualDiskToolStripMenuItem.AccessibleDescription = null;
            this.editVirtualDiskToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.editVirtualDiskToolStripMenuItem, "editVirtualDiskToolStripMenuItem");
            this.editVirtualDiskToolStripMenuItem.BackgroundImage = null;
            this.editVirtualDiskToolStripMenuItem.Name = "editVirtualDiskToolStripMenuItem";
            this.editVirtualDiskToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.editVirtualDiskToolStripMenuItem.Click += new System.EventHandler(this.editVirtualDiskToolStripMenuItem_Click);
            // 
            // BrowseStorageVirtualDiskPage
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = null;
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Font = null;
            this.Name = "BrowseStorageVirtualDiskPage";
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVDIs)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.toolTipContainerRescan.ResumeLayout(false);
            this.EditButtonContainer.ResumeLayout(false);
            this.toolTipContainerMove.ResumeLayout(false);
            this.RemoveButtonContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void BuildList()
        {
            DataGridViewSelectedRowCollection selectedRows = this.dataGridViewVDIs.SelectedRows;
            int firstDisplayedScrollingRowIndex = this.dataGridViewVDIs.FirstDisplayedScrollingRowIndex;
            this.dataGridViewVDIs.SuspendLayout();
            try
            {
                this.dataGridViewVDIs.Rows.Clear();
                if (this.sr == null)
                {
                    return;
                }
                List<VDI> list = this.sr.Connection.ResolveAll<VDI>(this.sr.VDIs);
                this.storageLinkVolumeColumn.Visible = list.Find(v => v.sm_config.ContainsKey("SVID")) != null;
                foreach (VDI vdi in list)
                {
                    if (vdi.is_a_snapshot)
                    {
                        bool isNotShow = vdi.GetVMs().Where(vm => vm.Show(true)).Any(vm => vm.is_a_snapshot && vm.other_config.ContainsKey("halsign_snapshot"));
                        if(isNotShow)
                            continue;
                    }
                    if (vdi.Show(XenAdmin.Properties.Settings.Default.ShowHiddenVMs) && !vdi.IsAnIntermediateStorageMotionSnapshot)
                    {
                        VDIRow dataGridViewRow = new VDIRow(vdi, this.storageLinkVolumeColumn.Visible);
                        this.dataGridViewVDIs.Rows.Add(dataGridViewRow);
                    }
                }
                IEnumerable<VDI> source = from row in selectedRows.Cast<VDIRow>() select row.VDI;
                foreach (VDIRow row2 in (IEnumerable)this.dataGridViewVDIs.Rows)
                {
                    row2.Selected = source.Contains<VDI>(row2.VDI);
                }
            }
            finally
            {
                if ((this.dataGridViewVDIs.SortedColumn != null) && (this.dataGridViewVDIs.SortOrder != SortOrder.None))
                {
                    this.dataGridViewVDIs.Sort(this.dataGridViewVDIs.SortedColumn, (this.dataGridViewVDIs.SortOrder == SortOrder.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending);
                }
                this.dataGridViewVDIs.ResumeLayout();
            }
            if (this.dataGridViewVDIs.Rows.Count > 0)
            {
                this.dataGridViewVDIs.FirstDisplayedScrollingRowIndex = ((firstDisplayedScrollingRowIndex < 0) || (firstDisplayedScrollingRowIndex >= this.dataGridViewVDIs.Rows.Count)) ? 0 : firstDisplayedScrollingRowIndex;
            }
            this.RefreshButtons();
        }

        private void a_Completed(ActionBase sender)
        {
            Program.Invoke(Program.MainWindow, this.RefreshButtons);
        }

        private void addContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this.addContextMenuStrip.Items.Clear();
            Command command = new AddVirtualDiskCommand(Program.MainWindow, this.sr);
            this.addContextMenuStrip.Items.Add(new CommandToolStripMenuItem(command, Messages.ADD_VIRTUAL_DISK));
            //command = new ImportStorageLinkVolumeCommand(Program.MainWindow, this.sr.StorageLinkRepository(Program.StorageLinkConnections));
            //this.addContextMenuStrip.Items.Add(new CommandToolStripMenuItem(command, Messages.ADD_SL_VOLUME));
        }

        private void addVirtualDiskButton_Click(object sender, EventArgs e)
        {
            if (this.sr != null)
            {
                Program.MainWindow.ShowPerConnectionWizard(this.sr.Connection, new NewDiskDialog(this.sr.Connection, this.sr));
            }
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            this.moveVirtualDiskToolStripMenuItem_Click(sender, e);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            new SrRefreshAction(this.sr).RunAsync();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            SelectedItemCollection selectedVDIs = this.SelectedVDIs;
            if (selectedVDIs.Count != 0)
            {
                this.contextMenuStrip1.Items.Clear();
                DeleteVirtualDiskCommand command = new DeleteVirtualDiskCommand(Program.MainWindow, selectedVDIs)
                {
                    AllowRunningVMDelete = true
                };
                this.contextMenuStrip1.Items.Add(new CommandToolStripMenuItem(command, true));
                this.contextMenuStrip1.Items.Add(new CommandToolStripMenuItem(this.MoveMigrateCommand(selectedVDIs), true));
                this.contextMenuStrip1.Items.Add(this.editVirtualDiskToolStripMenuItem);
                this.editVirtualDiskToolStripMenuItem.Enabled = (selectedVDIs.Count == 1) && !selectedVDIs.AsXenObjects<VDI>()[0].is_a_snapshot;
            }
        }

        private void dataGridViewVDIs_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (((e.Button == MouseButtons.Right) && (e.RowIndex >= 0)) && !this.dataGridViewVDIs.Rows[e.RowIndex].Selected)
            {
                if (this.dataGridViewVDIs.CurrentCell == this.dataGridViewVDIs[e.ColumnIndex, e.RowIndex])
                {
                    this.dataGridViewVDIs.Rows[e.RowIndex].Selected = true;
                }
                else
                {
                    this.dataGridViewVDIs.CurrentCell = this.dataGridViewVDIs[e.ColumnIndex, e.RowIndex];
                }
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            this.editVirtualDiskToolStripMenuItem_Click(sender, e);
        }

        private void editVirtualDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedItemCollection selectedVDIs = this.SelectedVDIs;
            if (selectedVDIs.Count == 1)
            {
                VDI xenObject = selectedVDIs.AsXenObjects<VDI>()[0];
                if (!xenObject.is_a_snapshot)
                {
                    new PropertiesDialog(xenObject).ShowDialog(this);
                }
            }
        }

        private void DataGridViewObject_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == this.sizeColumn.Index)
            {
                VDI vDI = ((VDIRow)this.dataGridViewVDIs.Rows[e.RowIndex1]).VDI;
                VDI vdi2 = ((VDIRow)this.dataGridViewVDIs.Rows[e.RowIndex2]).VDI;
                long num = vDI.virtual_size - vdi2.virtual_size;
                e.SortResult = (num > 0L) ? 1 : ((num < 0L) ? -1 : 0);
                e.Handled = true;
            }
        }

        private void dataGridViewVDIs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshButtons();
        }

        private void dataGridViewVDIs_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo info = this.dataGridViewVDIs.HitTest(e.X, e.Y);
                if ((info.Type == DataGridViewHitTestType.Cell) && (this.dataGridViewVDIs.Rows.Count >= 0))
                {
                    if (!this.dataGridViewVDIs.Rows[info.RowIndex].Selected)
                    {
                        this.dataGridViewVDIs.CurrentCell = this.dataGridViewVDIs[info.ColumnIndex, info.RowIndex];
                    }
                    this.contextMenuStrip1.Show(this.dataGridViewVDIs, new Point(e.X, e.Y));
                }
            }
        }

        private void dataGridViewVDIs_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Apps) && (this.dataGridViewVDIs.SelectedRows.Count > 0))
            {
                DataGridViewRow row = this.dataGridViewVDIs.SelectedRows[0];
                this.contextMenuStrip1.Show(this.dataGridViewVDIs, 5, row.Height * (row.Index + 2));
            }
        }

        private void History_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            Program.BeginInvoke(Program.MainWindow, delegate
            {
                SrRefreshAction element = e.Element as SrRefreshAction;
                if (element != null)
                {
                    if (e.Action == CollectionChangeAction.Add)
                    {
                        element.Completed += this.a_Completed;
                    }
                    if (e.Action == CollectionChangeAction.Remove)
                    {
                        element.Completed -= this.a_Completed;
                    }
                    this.RefreshButtons();
                }
            });
        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ShowHiddenVMs")
            {
                Program.Invoke(this, new MethodInvoker(this.BuildList));
            }
        }

        private void RefreshButtons()
        {
            bool flag = (this.sr == null) || this.sr.Locked;
            SelectedItemCollection selectedVDIs = this.SelectedVDIs;
            DeleteVirtualDiskCommand command = new DeleteVirtualDiskCommand(Program.MainWindow, selectedVDIs)
            {
                AllowMultipleVBDDelete = true,
                AllowRunningVMDelete = true
            };
            if (command.CanExecute())
            {
                this.RemoveButton.Enabled = true;
                this.RemoveButtonContainer.RemoveAll();
            }
            else
            {
                this.RemoveButton.Enabled = false;
                this.RemoveButtonContainer.SetToolTip(command.ToolTipText);
            }
            Command command2 = this.MoveMigrateCommand(selectedVDIs);
            if (command2.CanExecute())
            {
                this.buttonMove.Enabled = true;
                this.toolTipContainerMove.RemoveAll();
            }
            else
            {
                this.buttonMove.Enabled = false;
                this.toolTipContainerMove.SetToolTip(command2.ToolTipText);
            }
            if (flag)
            {
                this.buttonRefresh.Enabled = false;
            }
            else if (HelpersGUI.BeingScanned(this.sr))
            {
                this.buttonRefresh.Enabled = false;
                this.toolTipContainerRescan.SetToolTip(Messages.SCAN_IN_PROGRESS_TOOLTIP);
            }
            else
            {
                this.buttonRefresh.Enabled = true;
                this.toolTipContainerRescan.RemoveAll();
            }
            this.addVirtualDiskButton.Enabled = !flag;
            if (selectedVDIs.Count == 1)
            {
                VDI vdi = selectedVDIs.AsXenObjects<VDI>()[0];
                this.EditButton.Enabled = (!flag && !vdi.is_a_snapshot) && !vdi.Locked;
            }
            else
            {
                this.EditButton.Enabled = false;
            }
            this.removeDropDownButton.Enabled = command.CanExecute();
        }

        private Command MoveMigrateCommand(IEnumerable<SelectedItem> selection)
        {
            MoveVirtualDiskCommand command = new MoveVirtualDiskCommand(Program.MainWindow, selection);
            if (command.CanExecute())
            {
                return command;
            }
            return new MigrateVirtualDiskCommand(Program.MainWindow, selection);
        }

        private void moveVirtualDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedItemCollection selectedVDIs = this.SelectedVDIs;
            Command command = this.MoveMigrateCommand(selectedVDIs);
            if (command.CanExecute())
            {
                command.Execute();
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            this.removeVirtualDisk_Click(sender, e);
        }

        private void removeContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this.removeContextMenuStrip.Items.Clear();
            SelectedItemCollection selectedVDIs = this.SelectedVDIs;
            if (selectedVDIs.Count > 0)
            {
                Command command = new DeleteVirtualDiskCommand(Program.MainWindow, selectedVDIs);
                this.removeContextMenuStrip.Items.Add(new CommandToolStripMenuItem(command, Messages.DELETE_VIRTUAL_DISK));
                //command = new RemoveStorageLinkVolumeCommand(Program.MainWindow, this.sr.StorageLinkRepository(Program.StorageLinkConnections), selectedVDIs);
                //this.removeContextMenuStrip.Items.Add(new CommandToolStripMenuItem(command, Messages.DELETE_SL_VOLUME_CONTEXT_MENU_ELIPS));
            }
        }

        private void removeVirtualDisk_Click(object sender, EventArgs e)
        {
            SelectedItemCollection selectedVDIs = this.SelectedVDIs;
            DeleteVirtualDiskCommand command = new DeleteVirtualDiskCommand(Program.MainWindow, selectedVDIs)
            {
                AllowMultipleVBDDelete = true,
                AllowRunningVMDelete = true
            };
            if (command.CanExecute())
            {
                command.Execute();
            }
        }

        private void sr_PropertyChanged(object sender1, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VDIs")
            {
                this.rebuildRequired = true;
            }
        }

        private void VDI_BatchCollectionChanged(object sender, EventArgs e)
        {
            this.rebuildRequired = true;
        }

        private SelectedItemCollection SelectedVDIs
        {
            get
            {
                List<SelectedItem> items = new List<SelectedItem>();
                foreach (DataGridViewRow row in this.dataGridViewVDIs.SelectedRows)
                {
                    VDIRow row2 = row as VDIRow;
                    items.Add(new SelectedItem(row2.VDI));
                }
                return new SelectedItemCollection(items);
            }
        }

        private void Connection_XenObjectsUpdated(object sender, EventArgs e)
        {
            if (this.rebuildRequired)
            {
                this.BuildList();
            }
            this.rebuildRequired = false;
        }

        public XenAPI.SR SR
        {
            set
            {
                Program.AssertOnEventThread();
                if (this.sr != null)
                {
                    this.sr.PropertyChanged -= new PropertyChangedEventHandler(this.sr_PropertyChanged);
                    this.sr.Connection.Cache.DeregisterBatchCollectionChanged<VDI>(new EventHandler(this.VDI_BatchCollectionChanged));
                    this.sr.Connection.XenObjectsUpdated -= new EventHandler<EventArgs>(this.Connection_XenObjectsUpdated);
                }
                this.sr = value;
                if (this.sr != null)
                {
                    this.sr.PropertyChanged += new PropertyChangedEventHandler(this.sr_PropertyChanged);
                    this.sr.Connection.Cache.RegisterBatchCollectionChanged<VDI>(new EventHandler(this.VDI_BatchCollectionChanged));
                    this.addVirtualDiskButton.Visible = this.sr.SupportsVdiCreate();
                    this.sr.Connection.XenObjectsUpdated += new EventHandler<EventArgs>(this.Connection_XenObjectsUpdated);
                }
                
                this.addDropDownButton.Visible = false;
                //this.addVirtualDiskButton.Visible &= !this.addDropDownButton.Visible;
                this.removeDropDownButton.Visible = this.addDropDownButton.Visible;
                this.RemoveButtonContainer.Visible = !this.removeDropDownButton.Visible;
                this.BuildList();
                
            }
        }

        public class VDIRow : DataGridViewRow
        {
            private bool showStorageLink;
            public XenAPI.VDI VDI;

            public VDIRow(XenAPI.VDI vdi, bool showStorageLink)
            {
                this.showStorageLink = showStorageLink;
                this.VDI = vdi;
                for (int i = 0; i < 5; i++)
                {
                    base.Cells.Add(new DataGridViewTextBoxCell());
                    base.Cells[i].Value = this.GetCellText(i);
                }
            }

            private string GetCellText(int cellIndex)
            {
                switch (cellIndex)
                {
                    case 0:
                        return this.VDI.Name;

                    case 1:
                        /**
                        string str;                        
                        if (!Helpers.BostonOrGreater(this.VDI.Connection))
                        {
                            StorageLinkVolume volume = this.showStorageLink ? this.VDI.StorageLinkVolume(Program.StorageLinkConnections.GetCopy()) : null;
                            if (volume != null)
                            {
                                return volume.Name;
                            }
                            return string.Empty;
                        }
                        if (!this.VDI.sm_config.TryGetValue("displayname", out str))
                        {
                            return string.Empty;
                        }
                        return str;
                        **/

                    case 2:
                        return this.VDI.Description;

                    case 3:
                        return this.VDI.SizeText;

                    case 4:
                        return this.VDI.VMsOfVDI;
                }
                return "";
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return Resources._000_VirtualStorage_h32bit_16;
            }
        }

        public string SubText
        {
            get
            {
                return Messages.NAME_SUB_DESCRIPTION_BROWSE_DISK;
            }
        }

        public bool ValidToSave
        {
            get
            {
                return true;
            }
        }

        #region IEditPage Members


        public AsyncAction SaveSettings()
        {
            return null;
        }

        public void SetXenObjects(IXenObject orig, IXenObject clone)
        {
            if (clone != null)
            {
                //XenModelObject = clone;
            }
        }

        public void ShowLocalValidationMessages()
        {
            //
        }

        public bool HasChanged
        {
            get { return true; }
        }

        #endregion

        #region IEditPage Members

        public void Cleanup()
        {
            //if (this.toolTip != null)
            //{
            //    this.toolTip.Dispose();
            //}
        }

        #endregion
    }
}

