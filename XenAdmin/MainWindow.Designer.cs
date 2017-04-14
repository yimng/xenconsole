using XenAdmin.Alerts;
using XenAdmin.Network;
using XenAdmin.Commands;
using XenAdmin.Controls;
using XenAdmin.Plugins;
using XenAPI;
using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace XenAdmin
{
    partial class MainWindow
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
            Program.Exiting = true;

            XenAdmin.Core.Clip.UnregisterClipboardViewer();

            pluginManager.PluginsChanged -= pluginManager_PluginsChanged;
            pluginManager.Dispose();

            OtherConfigAndTagsWatcher.DeregisterEventHandlers();
            ConnectionsManager.History.CollectionChanged -= History_CollectionChanged;
            Alert.DeregisterAlertCollectionChanged(XenCenterAlerts_CollectionChanged);
            XenAdmin.Core.Updates.DeregisterCollectionChanged(Updates_CollectionChanged);
            ConnectionsManager.XenConnections.CollectionChanged -= XenConnection_CollectionChanged;
            Properties.Settings.Default.SettingChanging -= new System.Configuration.SettingChangingEventHandler(Default_SettingChanging);
            SearchPage.SearchChanged -= SearchPanel_SearchChanged;

            if (disposing && (components != null))
            {
                components.Dispose();

                log.Debug("MainWindow disoposing license timer");
                if (licenseTimer != null)
                    licenseTimer.Dispose();
            }

            log.Debug("Before MainWindow base.Dispose()");
            base.Dispose(disposing);
            log.Debug("After MainWindow base.Dispose()");
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.navigationPane = new XenAdmin.Controls.MainWindowControls.NavigationPane();
            this.TheTabControl = new System.Windows.Forms.TabControl();
            this.TabPageHome = new System.Windows.Forms.TabPage();
            this.TabPageGeneral = new System.Windows.Forms.TabPage();
            this.TabPageBallooning = new System.Windows.Forms.TabPage();
            this.TabPageBallooningUpsell = new System.Windows.Forms.TabPage();
            this.TabPageConsole = new System.Windows.Forms.TabPage();
            this.TabPageCvmConsole = new System.Windows.Forms.TabPage();
            this.TabPageStorage = new System.Windows.Forms.TabPage();
            this.TabPagePhysicalStorage = new System.Windows.Forms.TabPage();
            this.TabPageSR = new System.Windows.Forms.TabPage();
            this.TabPageNetwork = new System.Windows.Forms.TabPage();
            this.TabPageNICs = new System.Windows.Forms.TabPage();
            this.TabPagePeformance = new System.Windows.Forms.TabPage();
            this.TabPageHA = new System.Windows.Forms.TabPage();
            this.TabPageHAUpsell = new System.Windows.Forms.TabPage();
            this.TabPageSnapshots = new System.Windows.Forms.TabPage();
            this.snapshotPage = new XenAdmin.TabPages.SnapshotsPage();
            this.TabPageWLB = new System.Windows.Forms.TabPage();
            this.TabPageWLBUpsell = new System.Windows.Forms.TabPage();
            this.TabPageAD = new System.Windows.Forms.TabPage();
            this.TabPageADUpsell = new System.Windows.Forms.TabPage();
            this.TabPageGPU = new System.Windows.Forms.TabPage();
            this.TabPagePvs = new System.Windows.Forms.TabPage();
            this.TabPageSearch = new System.Windows.Forms.TabPage();
            this.TabPageDockerProcess = new System.Windows.Forms.TabPage();
            this.TabPageDockerDetails = new System.Windows.Forms.TabPage();
            this.TabPageBackup = new System.Windows.Forms.TabPage();
            this.TabPageBRUpsell = new System.Windows.Forms.TabPage();
            this.TabPageUsbDevice = new System.Windows.Forms.TabPage();
            this.TabPagevSwitchController = new System.Windows.Forms.TabPage();
            this.TabPageSCUpsell = new System.Windows.Forms.TabPage();
            this.alertPage = new XenAdmin.TabPages.AlertSummaryPage();
            this.updatesPage = new XenAdmin.TabPages.ManageUpdatesPage();
            this.eventsPage = new XenAdmin.TabPages.HistoryPage();
            this.TitleBackPanel = new XenAdmin.Controls.GradientPanel.GradientPanel();
            this.TitleIcon = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolTipContainer1 = new XenAdmin.Controls.ToolTipContainer();
            this.loggedInLabel1 = new XenAdmin.Controls.LoggedInLabel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.ToolStrip = new XenAdmin.Controls.ToolStripEx();
            this.backButton = new System.Windows.Forms.ToolStripSplitButton();
            this.forwardButton = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.AddServerToolbarButton = new XenAdmin.Commands.CommandToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.AddPoolToolbarButton = new XenAdmin.Commands.CommandToolStripButton();
            this.newStorageToolbarButton = new XenAdmin.Commands.CommandToolStripButton();
            this.NewVmToolbarButton = new XenAdmin.Commands.CommandToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.shutDownToolStripButton = new XenAdmin.Commands.CommandToolStripButton();
            this.powerOnHostToolStripButton = new XenAdmin.Commands.CommandToolStripButton();
            this.startVMToolStripButton = new XenAdmin.Commands.CommandToolStripButton();
            this.RebootToolbarButton = new XenAdmin.Commands.CommandToolStripButton();
            this.resumeToolStripButton = new XenAdmin.Commands.CommandToolStripButton();
            this.SuspendToolbarButton = new XenAdmin.Commands.CommandToolStripButton();
            this.ForceShutdownToolbarButton = new XenAdmin.Commands.CommandToolStripButton();
            this.ForceRebootToolbarButton = new XenAdmin.Commands.CommandToolStripButton();
            this.stopContainerToolStripButton = new XenAdmin.Commands.CommandToolStripButton();
            this.startContainerToolStripButton = new XenAdmin.Commands.CommandToolStripButton();
            this.restartContainerToolStripButton = new XenAdmin.Commands.CommandToolStripButton();
            this.resumeContainerToolStripButton = new XenAdmin.Commands.CommandToolStripButton();
            this.pauseContainerToolStripButton = new XenAdmin.Commands.CommandToolStripButton();
            this.statusToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ToolBarContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowToolbarMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenuBar = new XenAdmin.Controls.MenuStripEx();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileImportVMToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.importSearchToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
            this.importSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator31 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginItemsPlaceHolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customTemplatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.templatesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.localStorageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowHiddenObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginItemsPlaceHolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.poolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddPoolToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.addServerToolStripMenuItem = new XenAdmin.Commands.AddHostToSelectedPoolToolStripMenuItem();
            this.removeServerToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.poolReconnectAsToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.disconnectPoolToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator27 = new System.Windows.Forms.ToolStripSeparator();
            this.virtualAppliancesToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator30 = new System.Windows.Forms.ToolStripSeparator();
            this.highAvailabilityToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.disasterRecoveryToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.drConfigureToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.DrWizardToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.VMSnapshotScheduleToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.vMProtectionAndRecoveryToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.exportResourceReportPoolToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.wlbReportsToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.wlbDisconnectToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.changePoolPasswordToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator26 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginItemsPlaceHolderToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.PoolPropertiesToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.HostMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddHostToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.RebootHostToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.powerOnToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.ShutdownHostToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.restartToolstackToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.connectDisconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReconnectToolStripMenuItem1 = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.DisconnectToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.reconnectAsToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.connectAllToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.disconnectAllToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.addServerToPoolMenuItem = new XenAdmin.Commands.AddSelectedHostToPoolToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.backupToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.restoreFromBackupToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
            this.maintenanceModeToolStripMenuItem1 = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.controlDomainMemoryToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.installLicenseToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.HostPasswordToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.ChangeRootPasswordToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.forgetSavedPasswordToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator25 = new System.Windows.Forms.ToolStripSeparator();
            this.destroyServerToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.removeHostToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginItemsPlaceHolderToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerPropertiesToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.VMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewVmToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.startShutdownToolStripMenuItem = new XenAdmin.Commands.VMLifeCycleToolStripMenuItem();
            this.resumeOnToolStripMenuItem = new XenAdmin.Commands.ResumeVMOnHostToolStripMenuItem();
            this.relocateToolStripMenuItem = new XenAdmin.Commands.MigrateVMToolStripMenuItem();
            this.startOnHostToolStripMenuItem = new XenAdmin.Commands.StartVMOnHostToolStripMenuItem();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.assignSnapshotScheduleToolStripMenuItem = new XenAdmin.Commands.AssignGroupToolStripMenuItemVMSS();
            this.assignPolicyToolStripMenuItem = new XenAdmin.Commands.AssignGroupToolStripMenuItemVMPP();
            this.assignToVirtualApplianceToolStripMenuItem = new XenAdmin.Commands.AssignGroupToolStripMenuItemVM_appliance();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.copyVMtoSharedStorageMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.MoveVMToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.snapshotToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.convertToTemplateToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.exportToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.enablePVSReadcachingToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.disablePVSReadcachingToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.installToolsToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.sendCtrlAltDelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.uninstallToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginItemsPlaceHolderToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.VMPropertiesToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.StorageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddStorageToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
            this.RepairStorageToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.DefaultSRToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.virtualDisksToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.addVirtualDiskToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.attachVirtualDiskToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.reclaimFreedSpacetripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.DetachStorageToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.ReattachStorageRepositoryToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.ForgetStorageRepositoryToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.DestroyStorageRepositoryToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.ConvertToThinStorageRepositoryToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginItemsPlaceHolderToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.SRPropertiesToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.templatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateVmFromTemplateToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.newVMFromTemplateToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.InstantVmToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator29 = new System.Windows.Forms.ToolStripSeparator();
            this.exportTemplateToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.duplicateTemplateToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.uninstallTemplateToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator28 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginItemsPlaceHolderToolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.templatePropertiesToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bugToolToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.healthCheckToolStripMenuItem1 = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.LicenseManagerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.installNewUpdateToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.rollingUpgradeToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginItemsPlaceHolderToolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginItemsPlaceHolderToolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpTopicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
            this.viewApplicationLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
            this.xenSourceOnTheWebToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xenCenterPluginsOnlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginItemsPlaceHolderToolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutXenSourceAdminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuPanel = new System.Windows.Forms.Panel();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.securityGroupsToolStripMenuItem = new XenAdmin.Commands.CommandToolStripMenuItem();
            this.commandToolStripMenuItem1 = new XenAdmin.Commands.CommandToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.TheTabControl.SuspendLayout();
            this.TabPageSnapshots.SuspendLayout();
            this.TitleBackPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TitleIcon)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolTipContainer1.SuspendLayout();
            this.ToolStrip.SuspendLayout();
            this.ToolBarContextMenu.SuspendLayout();
            this.MainMenuBar.SuspendLayout();
            this.MenuPanel.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.navigationPane);
            this.statusToolTip.SetToolTip(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.ToolTip"));
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.TheTabControl);
            this.splitContainer1.Panel2.Controls.Add(this.alertPage);
            this.splitContainer1.Panel2.Controls.Add(this.updatesPage);
            this.splitContainer1.Panel2.Controls.Add(this.eventsPage);
            this.splitContainer1.Panel2.Controls.Add(this.TitleBackPanel);
            this.statusToolTip.SetToolTip(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.ToolTip"));
            this.statusToolTip.SetToolTip(this.splitContainer1, resources.GetString("splitContainer1.ToolTip"));
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // navigationPane
            // 
            resources.ApplyResources(this.navigationPane, "navigationPane");
            this.navigationPane.Name = "navigationPane";
            this.statusToolTip.SetToolTip(this.navigationPane, resources.GetString("navigationPane.ToolTip"));
            this.navigationPane.NavigationModeChanged += new System.Action<XenAdmin.Controls.MainWindowControls.NavigationPane.NavigationMode>(this.navigationPane_NavigationModeChanged);
            this.navigationPane.NotificationsSubModeChanged += new System.Action<XenAdmin.Controls.MainWindowControls.NotificationsSubModeItem>(this.navigationPane_NotificationsSubModeChanged);
            this.navigationPane.TreeViewSelectionChanged += new System.Action(this.navigationPane_TreeViewSelectionChanged);
            this.navigationPane.TreeNodeBeforeSelected += new System.Action(this.navigationPane_TreeNodeBeforeSelected);
            this.navigationPane.TreeNodeClicked += new System.Action(this.navigationPane_TreeNodeClicked);
            this.navigationPane.TreeNodeRightClicked += new System.Action(this.navigationPane_TreeNodeRightClicked);
            this.navigationPane.TreeViewRefreshed += new System.Action(this.navigationPane_TreeViewRefreshed);
            this.navigationPane.TreeViewRefreshSuspended += new System.Action(this.navigationPane_TreeViewRefreshSuspended);
            this.navigationPane.TreeViewRefreshResumed += new System.Action(this.navigationPane_TreeViewRefreshResumed);
            // 
            // TheTabControl
            // 
            resources.ApplyResources(this.TheTabControl, "TheTabControl");
            this.TheTabControl.Controls.Add(this.TabPageHome);
            this.TheTabControl.Controls.Add(this.TabPageGeneral);
            this.TheTabControl.Controls.Add(this.TabPageBallooning);
            this.TheTabControl.Controls.Add(this.TabPageBallooningUpsell);
            this.TheTabControl.Controls.Add(this.TabPageConsole);
            this.TheTabControl.Controls.Add(this.TabPageCvmConsole);
            this.TheTabControl.Controls.Add(this.TabPageStorage);
            this.TheTabControl.Controls.Add(this.TabPagePhysicalStorage);
            this.TheTabControl.Controls.Add(this.TabPageSR);
            this.TheTabControl.Controls.Add(this.TabPageNetwork);
            this.TheTabControl.Controls.Add(this.TabPageNICs);
            this.TheTabControl.Controls.Add(this.TabPagePeformance);
            this.TheTabControl.Controls.Add(this.TabPageHA);
            this.TheTabControl.Controls.Add(this.TabPageHAUpsell);
            this.TheTabControl.Controls.Add(this.TabPageSnapshots);
            this.TheTabControl.Controls.Add(this.TabPageWLB);
            this.TheTabControl.Controls.Add(this.TabPageWLBUpsell);
            this.TheTabControl.Controls.Add(this.TabPageAD);
            this.TheTabControl.Controls.Add(this.TabPageADUpsell);
            this.TheTabControl.Controls.Add(this.TabPageGPU);
            this.TheTabControl.Controls.Add(this.TabPagePvs);
            this.TheTabControl.Controls.Add(this.TabPageSearch);
            this.TheTabControl.Controls.Add(this.TabPageDockerProcess);
            this.TheTabControl.Controls.Add(this.TabPageDockerDetails);
            this.TheTabControl.Controls.Add(this.TabPageBackup);
            this.TheTabControl.Controls.Add(this.TabPageBRUpsell);
            this.TheTabControl.Controls.Add(this.TabPageUsbDevice);
            this.TheTabControl.Controls.Add(this.TabPagevSwitchController);
            this.TheTabControl.Controls.Add(this.TabPageSCUpsell);
            this.TheTabControl.Name = "TheTabControl";
            this.TheTabControl.SelectedIndex = 4;
            this.statusToolTip.SetToolTip(this.TheTabControl, resources.GetString("TheTabControl.ToolTip"));
            // 
            // TabPageHome
            // 
            resources.ApplyResources(this.TabPageHome, "TabPageHome");
            this.TabPageHome.Name = "TabPageHome";
            this.statusToolTip.SetToolTip(this.TabPageHome, resources.GetString("TabPageHome.ToolTip"));
            this.TabPageHome.UseVisualStyleBackColor = true;
            // 
            // TabPageGeneral
            // 
            resources.ApplyResources(this.TabPageGeneral, "TabPageGeneral");
            this.TabPageGeneral.Name = "TabPageGeneral";
            this.statusToolTip.SetToolTip(this.TabPageGeneral, resources.GetString("TabPageGeneral.ToolTip"));
            this.TabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // TabPageBallooning
            // 
            resources.ApplyResources(this.TabPageBallooning, "TabPageBallooning");
            this.TabPageBallooning.Name = "TabPageBallooning";
            this.statusToolTip.SetToolTip(this.TabPageBallooning, resources.GetString("TabPageBallooning.ToolTip"));
            this.TabPageBallooning.UseVisualStyleBackColor = true;
            // 
            // TabPageBallooningUpsell
            // 
            resources.ApplyResources(this.TabPageBallooningUpsell, "TabPageBallooningUpsell");
            this.TabPageBallooningUpsell.Name = "TabPageBallooningUpsell";
            this.statusToolTip.SetToolTip(this.TabPageBallooningUpsell, resources.GetString("TabPageBallooningUpsell.ToolTip"));
            this.TabPageBallooningUpsell.UseVisualStyleBackColor = true;
            // 
            // TabPageConsole
            // 
            resources.ApplyResources(this.TabPageConsole, "TabPageConsole");
            this.TabPageConsole.Name = "TabPageConsole";
            this.statusToolTip.SetToolTip(this.TabPageConsole, resources.GetString("TabPageConsole.ToolTip"));
            this.TabPageConsole.UseVisualStyleBackColor = true;
            // 
            // TabPageCvmConsole
            // 
            resources.ApplyResources(this.TabPageCvmConsole, "TabPageCvmConsole");
            this.TabPageCvmConsole.Name = "TabPageCvmConsole";
            this.statusToolTip.SetToolTip(this.TabPageCvmConsole, resources.GetString("TabPageCvmConsole.ToolTip"));
            this.TabPageCvmConsole.UseVisualStyleBackColor = true;
            // 
            // TabPageStorage
            // 
            resources.ApplyResources(this.TabPageStorage, "TabPageStorage");
            this.TabPageStorage.Name = "TabPageStorage";
            this.statusToolTip.SetToolTip(this.TabPageStorage, resources.GetString("TabPageStorage.ToolTip"));
            this.TabPageStorage.UseVisualStyleBackColor = true;
            // 
            // TabPagePhysicalStorage
            // 
            resources.ApplyResources(this.TabPagePhysicalStorage, "TabPagePhysicalStorage");
            this.TabPagePhysicalStorage.Name = "TabPagePhysicalStorage";
            this.statusToolTip.SetToolTip(this.TabPagePhysicalStorage, resources.GetString("TabPagePhysicalStorage.ToolTip"));
            this.TabPagePhysicalStorage.UseVisualStyleBackColor = true;
            // 
            // TabPageSR
            // 
            resources.ApplyResources(this.TabPageSR, "TabPageSR");
            this.TabPageSR.Name = "TabPageSR";
            this.statusToolTip.SetToolTip(this.TabPageSR, resources.GetString("TabPageSR.ToolTip"));
            this.TabPageSR.UseVisualStyleBackColor = true;
            // 
            // TabPageNetwork
            // 
            resources.ApplyResources(this.TabPageNetwork, "TabPageNetwork");
            this.TabPageNetwork.Name = "TabPageNetwork";
            this.statusToolTip.SetToolTip(this.TabPageNetwork, resources.GetString("TabPageNetwork.ToolTip"));
            this.TabPageNetwork.UseVisualStyleBackColor = true;
            // 
            // TabPageNICs
            // 
            resources.ApplyResources(this.TabPageNICs, "TabPageNICs");
            this.TabPageNICs.Name = "TabPageNICs";
            this.statusToolTip.SetToolTip(this.TabPageNICs, resources.GetString("TabPageNICs.ToolTip"));
            this.TabPageNICs.UseVisualStyleBackColor = true;
            // 
            // TabPagePeformance
            // 
            resources.ApplyResources(this.TabPagePeformance, "TabPagePeformance");
            this.TabPagePeformance.Name = "TabPagePeformance";
            this.statusToolTip.SetToolTip(this.TabPagePeformance, resources.GetString("TabPagePeformance.ToolTip"));
            this.TabPagePeformance.UseVisualStyleBackColor = true;
            // 
            // TabPageHA
            // 
            resources.ApplyResources(this.TabPageHA, "TabPageHA");
            this.TabPageHA.Name = "TabPageHA";
            this.statusToolTip.SetToolTip(this.TabPageHA, resources.GetString("TabPageHA.ToolTip"));
            this.TabPageHA.UseVisualStyleBackColor = true;
            // 
            // TabPageHAUpsell
            // 
            resources.ApplyResources(this.TabPageHAUpsell, "TabPageHAUpsell");
            this.TabPageHAUpsell.Name = "TabPageHAUpsell";
            this.statusToolTip.SetToolTip(this.TabPageHAUpsell, resources.GetString("TabPageHAUpsell.ToolTip"));
            this.TabPageHAUpsell.UseVisualStyleBackColor = true;
            // 
            // TabPageSnapshots
            // 
            resources.ApplyResources(this.TabPageSnapshots, "TabPageSnapshots");
            this.TabPageSnapshots.Controls.Add(this.snapshotPage);
            this.TabPageSnapshots.Name = "TabPageSnapshots";
            this.statusToolTip.SetToolTip(this.TabPageSnapshots, resources.GetString("TabPageSnapshots.ToolTip"));
            this.TabPageSnapshots.UseVisualStyleBackColor = true;
            // 
            // snapshotPage
            // 
            resources.ApplyResources(this.snapshotPage, "snapshotPage");
            this.snapshotPage.Name = "snapshotPage";
            this.statusToolTip.SetToolTip(this.snapshotPage, resources.GetString("snapshotPage.ToolTip"));
            this.snapshotPage.VM = null;
            // 
            // TabPageWLB
            // 
            resources.ApplyResources(this.TabPageWLB, "TabPageWLB");
            this.TabPageWLB.Name = "TabPageWLB";
            this.statusToolTip.SetToolTip(this.TabPageWLB, resources.GetString("TabPageWLB.ToolTip"));
            this.TabPageWLB.UseVisualStyleBackColor = true;
            // 
            // TabPageWLBUpsell
            // 
            resources.ApplyResources(this.TabPageWLBUpsell, "TabPageWLBUpsell");
            this.TabPageWLBUpsell.Name = "TabPageWLBUpsell";
            this.statusToolTip.SetToolTip(this.TabPageWLBUpsell, resources.GetString("TabPageWLBUpsell.ToolTip"));
            this.TabPageWLBUpsell.UseVisualStyleBackColor = true;
            // 
            // TabPageAD
            // 
            resources.ApplyResources(this.TabPageAD, "TabPageAD");
            this.TabPageAD.Name = "TabPageAD";
            this.statusToolTip.SetToolTip(this.TabPageAD, resources.GetString("TabPageAD.ToolTip"));
            this.TabPageAD.UseVisualStyleBackColor = true;
            // 
            // TabPageADUpsell
            // 
            resources.ApplyResources(this.TabPageADUpsell, "TabPageADUpsell");
            this.TabPageADUpsell.Name = "TabPageADUpsell";
            this.statusToolTip.SetToolTip(this.TabPageADUpsell, resources.GetString("TabPageADUpsell.ToolTip"));
            this.TabPageADUpsell.UseVisualStyleBackColor = true;
            // 
            // TabPageGPU
            // 
            resources.ApplyResources(this.TabPageGPU, "TabPageGPU");
            this.TabPageGPU.Name = "TabPageGPU";
            this.statusToolTip.SetToolTip(this.TabPageGPU, resources.GetString("TabPageGPU.ToolTip"));
            this.TabPageGPU.UseVisualStyleBackColor = true;
            // 
            // TabPagePvs
            // 
            resources.ApplyResources(this.TabPagePvs, "TabPagePvs");
            this.TabPagePvs.Name = "TabPagePvs";
            this.statusToolTip.SetToolTip(this.TabPagePvs, resources.GetString("TabPagePvs.ToolTip"));
            this.TabPagePvs.UseVisualStyleBackColor = true;
            // 
            // TabPageSearch
            // 
            resources.ApplyResources(this.TabPageSearch, "TabPageSearch");
            this.TabPageSearch.Name = "TabPageSearch";
            this.statusToolTip.SetToolTip(this.TabPageSearch, resources.GetString("TabPageSearch.ToolTip"));
            this.TabPageSearch.UseVisualStyleBackColor = true;
            // 
            // TabPageDockerProcess
            // 
            resources.ApplyResources(this.TabPageDockerProcess, "TabPageDockerProcess");
            this.TabPageDockerProcess.Name = "TabPageDockerProcess";
            this.statusToolTip.SetToolTip(this.TabPageDockerProcess, resources.GetString("TabPageDockerProcess.ToolTip"));
            this.TabPageDockerProcess.UseVisualStyleBackColor = true;
            // 
            // TabPageDockerDetails
            // 
            resources.ApplyResources(this.TabPageDockerDetails, "TabPageDockerDetails");
            this.TabPageDockerDetails.Name = "TabPageDockerDetails";
            this.statusToolTip.SetToolTip(this.TabPageDockerDetails, resources.GetString("TabPageDockerDetails.ToolTip"));
            this.TabPageDockerDetails.UseVisualStyleBackColor = true;
            // 
            // TabPageBackup
            // 
            resources.ApplyResources(this.TabPageBackup, "TabPageBackup");
            this.TabPageBackup.Name = "TabPageBackup";
            this.statusToolTip.SetToolTip(this.TabPageBackup, resources.GetString("TabPageBackup.ToolTip"));
            this.TabPageBackup.UseVisualStyleBackColor = true;
            // 
            // TabPageBRUpsell
            // 
            resources.ApplyResources(this.TabPageBRUpsell, "TabPageBRUpsell");
            this.TabPageBRUpsell.Name = "TabPageBRUpsell";
            this.statusToolTip.SetToolTip(this.TabPageBRUpsell, resources.GetString("TabPageBRUpsell.ToolTip"));
            this.TabPageBRUpsell.UseVisualStyleBackColor = true;
            // 
            // TabPageUsbDevice
            // 
            resources.ApplyResources(this.TabPageUsbDevice, "TabPageUsbDevice");
            this.TabPageUsbDevice.Name = "TabPageUsbDevice";
            this.statusToolTip.SetToolTip(this.TabPageUsbDevice, resources.GetString("TabPageUsbDevice.ToolTip"));
            this.TabPageUsbDevice.UseVisualStyleBackColor = true;
            // 
            // TabPagevSwitchController
            // 
            resources.ApplyResources(this.TabPagevSwitchController, "TabPagevSwitchController");
            this.TabPagevSwitchController.Name = "TabPagevSwitchController";
            this.statusToolTip.SetToolTip(this.TabPagevSwitchController, resources.GetString("TabPagevSwitchController.ToolTip"));
            this.TabPagevSwitchController.UseVisualStyleBackColor = true;
            // 
            // TabPageSCUpsell
            // 
            resources.ApplyResources(this.TabPageSCUpsell, "TabPageSCUpsell");
            this.TabPageSCUpsell.Name = "TabPageSCUpsell";
            this.statusToolTip.SetToolTip(this.TabPageSCUpsell, resources.GetString("TabPageSCUpsell.ToolTip"));
            this.TabPageSCUpsell.UseVisualStyleBackColor = true;
            // 
            // alertPage
            // 
            resources.ApplyResources(this.alertPage, "alertPage");
            this.alertPage.BackColor = System.Drawing.SystemColors.Window;
            this.alertPage.Name = "alertPage";
            this.statusToolTip.SetToolTip(this.alertPage, resources.GetString("alertPage.ToolTip"));
            // 
            // updatesPage
            // 
            resources.ApplyResources(this.updatesPage, "updatesPage");
            this.updatesPage.BackColor = System.Drawing.SystemColors.Window;
            this.updatesPage.Name = "updatesPage";
            this.statusToolTip.SetToolTip(this.updatesPage, resources.GetString("updatesPage.ToolTip"));
            // 
            // eventsPage
            // 
            resources.ApplyResources(this.eventsPage, "eventsPage");
            this.eventsPage.BackColor = System.Drawing.SystemColors.Window;
            this.eventsPage.Name = "eventsPage";
            this.statusToolTip.SetToolTip(this.eventsPage, resources.GetString("eventsPage.ToolTip"));
            // 
            // TitleBackPanel
            // 
            resources.ApplyResources(this.TitleBackPanel, "TitleBackPanel");
            this.TitleBackPanel.BackColor = System.Drawing.Color.Transparent;
            this.TitleBackPanel.Controls.Add(this.TitleIcon);
            this.TitleBackPanel.Controls.Add(this.tableLayoutPanel1);
            this.TitleBackPanel.Name = "TitleBackPanel";
            this.TitleBackPanel.Scheme = XenAdmin.Controls.GradientPanel.GradientPanel.Schemes.Title;
            this.statusToolTip.SetToolTip(this.TitleBackPanel, resources.GetString("TitleBackPanel.ToolTip"));
            // 
            // TitleIcon
            // 
            resources.ApplyResources(this.TitleIcon, "TitleIcon");
            this.TitleIcon.Name = "TitleIcon";
            this.TitleIcon.TabStop = false;
            this.statusToolTip.SetToolTip(this.TitleIcon, resources.GetString("TitleIcon.ToolTip"));
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.toolTipContainer1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.TitleLabel, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.statusToolTip.SetToolTip(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.ToolTip"));
            // 
            // toolTipContainer1
            // 
            resources.ApplyResources(this.toolTipContainer1, "toolTipContainer1");
            this.toolTipContainer1.Controls.Add(this.loggedInLabel1);
            this.toolTipContainer1.Name = "toolTipContainer1";
            this.statusToolTip.SetToolTip(this.toolTipContainer1, resources.GetString("toolTipContainer1.ToolTip"));
            // 
            // loggedInLabel1
            // 
            resources.ApplyResources(this.loggedInLabel1, "loggedInLabel1");
            this.loggedInLabel1.BackColor = System.Drawing.Color.Transparent;
            this.loggedInLabel1.Connection = null;
            this.loggedInLabel1.Name = "loggedInLabel1";
            this.statusToolTip.SetToolTip(this.loggedInLabel1, resources.GetString("loggedInLabel1.ToolTip"));
            // 
            // TitleLabel
            // 
            resources.ApplyResources(this.TitleLabel, "TitleLabel");
            this.TitleLabel.AutoEllipsis = true;
            this.TitleLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.TitleLabel.Name = "TitleLabel";
            this.statusToolTip.SetToolTip(this.TitleLabel, resources.GetString("TitleLabel.ToolTip"));
            this.TitleLabel.UseMnemonic = false;
            // 
            // ToolStrip
            // 
            resources.ApplyResources(this.ToolStrip, "ToolStrip");
            this.ToolStrip.ClickThrough = true;
            this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backButton,
            this.forwardButton,
            this.toolStripSeparator17,
            this.AddServerToolbarButton,
            this.toolStripSeparator11,
            this.AddPoolToolbarButton,
            this.newStorageToolbarButton,
            this.NewVmToolbarButton,
            this.toolStripSeparator12,
            this.shutDownToolStripButton,
            this.powerOnHostToolStripButton,
            this.startVMToolStripButton,
            this.RebootToolbarButton,
            this.resumeToolStripButton,
            this.SuspendToolbarButton,
            this.ForceShutdownToolbarButton,
            this.ForceRebootToolbarButton,
            this.stopContainerToolStripButton,
            this.startContainerToolStripButton,
            this.restartContainerToolStripButton,
            this.resumeContainerToolStripButton,
            this.pauseContainerToolStripButton});
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Stretch = true;
            this.ToolStrip.TabStop = true;
            this.statusToolTip.SetToolTip(this.ToolStrip, resources.GetString("ToolStrip.ToolTip"));
            this.ToolStrip.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainMenuBar_MouseClick);
            // 
            // backButton
            // 
            resources.ApplyResources(this.backButton, "backButton");
            this.backButton.Image = global::XenAdmin.Properties.Resources._001_Back_h32bit_24;
            this.backButton.Name = "backButton";
            this.backButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.backButton.ButtonClick += new System.EventHandler(this.backButton_Click);
            this.backButton.DropDownOpening += new System.EventHandler(this.backButton_DropDownOpening);
            // 
            // forwardButton
            // 
            resources.ApplyResources(this.forwardButton, "forwardButton");
            this.forwardButton.Image = global::XenAdmin.Properties.Resources._001_Forward_h32bit_24;
            this.forwardButton.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.forwardButton.ButtonClick += new System.EventHandler(this.forwardButton_Click);
            this.forwardButton.DropDownOpening += new System.EventHandler(this.forwardButton_DropDownOpening);
            // 
            // toolStripSeparator17
            // 
            resources.ApplyResources(this.toolStripSeparator17, "toolStripSeparator17");
            this.toolStripSeparator17.Margin = new System.Windows.Forms.Padding(2, 0, 7, 0);
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // AddServerToolbarButton
            // 
            resources.ApplyResources(this.AddServerToolbarButton, "AddServerToolbarButton");
            this.AddServerToolbarButton.Command = new XenAdmin.Commands.AddHostCommand();
            this.AddServerToolbarButton.Image = global::XenAdmin.Properties.Resources._000_AddApplicationServer_h32bit_24;
            this.AddServerToolbarButton.Name = "AddServerToolbarButton";
            this.AddServerToolbarButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // toolStripSeparator11
            // 
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            this.toolStripSeparator11.Margin = new System.Windows.Forms.Padding(2, 0, 7, 0);
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // AddPoolToolbarButton
            // 
            resources.ApplyResources(this.AddPoolToolbarButton, "AddPoolToolbarButton");
            this.AddPoolToolbarButton.Command = new XenAdmin.Commands.NewPoolCommand();
            this.AddPoolToolbarButton.Image = global::XenAdmin.Properties.Resources._000_PoolNew_h32bit_24;
            this.AddPoolToolbarButton.Name = "AddPoolToolbarButton";
            this.AddPoolToolbarButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // newStorageToolbarButton
            // 
            resources.ApplyResources(this.newStorageToolbarButton, "newStorageToolbarButton");
            this.newStorageToolbarButton.Command = new XenAdmin.Commands.NewSRCommand();
            this.newStorageToolbarButton.Image = global::XenAdmin.Properties.Resources._000_NewStorage_h32bit_24;
            this.newStorageToolbarButton.Name = "newStorageToolbarButton";
            this.newStorageToolbarButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // NewVmToolbarButton
            // 
            resources.ApplyResources(this.NewVmToolbarButton, "NewVmToolbarButton");
            this.NewVmToolbarButton.Command = new XenAdmin.Commands.NewVMCommand();
            this.NewVmToolbarButton.Image = global::XenAdmin.Properties.Resources._000_CreateVM_h32bit_24;
            this.NewVmToolbarButton.Name = "NewVmToolbarButton";
            this.NewVmToolbarButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // toolStripSeparator12
            // 
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            this.toolStripSeparator12.Margin = new System.Windows.Forms.Padding(2, 0, 7, 0);
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            // 
            // shutDownToolStripButton
            // 
            resources.ApplyResources(this.shutDownToolStripButton, "shutDownToolStripButton");
            this.shutDownToolStripButton.Command = new XenAdmin.Commands.ShutDownCommand();
            this.shutDownToolStripButton.Name = "shutDownToolStripButton";
            // 
            // powerOnHostToolStripButton
            // 
            resources.ApplyResources(this.powerOnHostToolStripButton, "powerOnHostToolStripButton");
            this.powerOnHostToolStripButton.Command = new XenAdmin.Commands.PowerOnHostCommand();
            this.powerOnHostToolStripButton.Name = "powerOnHostToolStripButton";
            // 
            // startVMToolStripButton
            // 
            resources.ApplyResources(this.startVMToolStripButton, "startVMToolStripButton");
            this.startVMToolStripButton.Command = new XenAdmin.Commands.StartVMCommand();
            this.startVMToolStripButton.Name = "startVMToolStripButton";
            // 
            // RebootToolbarButton
            // 
            resources.ApplyResources(this.RebootToolbarButton, "RebootToolbarButton");
            this.RebootToolbarButton.Command = new XenAdmin.Commands.RebootCommand();
            this.RebootToolbarButton.Name = "RebootToolbarButton";
            // 
            // resumeToolStripButton
            // 
            resources.ApplyResources(this.resumeToolStripButton, "resumeToolStripButton");
            this.resumeToolStripButton.Command = new XenAdmin.Commands.ResumeVMCommand();
            this.resumeToolStripButton.Image = global::XenAdmin.Properties.Resources._000_Paused_h32bit_24;
            this.resumeToolStripButton.Name = "resumeToolStripButton";
            // 
            // SuspendToolbarButton
            // 
            resources.ApplyResources(this.SuspendToolbarButton, "SuspendToolbarButton");
            this.SuspendToolbarButton.Command = new XenAdmin.Commands.SuspendVMCommand();
            this.SuspendToolbarButton.Image = global::XenAdmin.Properties.Resources._000_Paused_h32bit_24;
            this.SuspendToolbarButton.Name = "SuspendToolbarButton";
            // 
            // ForceShutdownToolbarButton
            // 
            resources.ApplyResources(this.ForceShutdownToolbarButton, "ForceShutdownToolbarButton");
            this.ForceShutdownToolbarButton.Command = new XenAdmin.Commands.ForceVMShutDownCommand();
            this.ForceShutdownToolbarButton.Image = global::XenAdmin.Properties.Resources._001_ForceShutDown_h32bit_24;
            this.ForceShutdownToolbarButton.Name = "ForceShutdownToolbarButton";
            // 
            // ForceRebootToolbarButton
            // 
            resources.ApplyResources(this.ForceRebootToolbarButton, "ForceRebootToolbarButton");
            this.ForceRebootToolbarButton.Command = new XenAdmin.Commands.ForceVMRebootCommand();
            this.ForceRebootToolbarButton.Image = global::XenAdmin.Properties.Resources._001_ForceReboot_h32bit_24;
            this.ForceRebootToolbarButton.Name = "ForceRebootToolbarButton";
            // 
            // stopContainerToolStripButton
            // 
            resources.ApplyResources(this.stopContainerToolStripButton, "stopContainerToolStripButton");
            this.stopContainerToolStripButton.Command = new XenAdmin.Commands.StopDockerContainerCommand();
            this.stopContainerToolStripButton.Image = global::XenAdmin.Properties.Resources._001_ShutDown_h32bit_24;
            this.stopContainerToolStripButton.Name = "stopContainerToolStripButton";
            // 
            // startContainerToolStripButton
            // 
            resources.ApplyResources(this.startContainerToolStripButton, "startContainerToolStripButton");
            this.startContainerToolStripButton.Command = new XenAdmin.Commands.StartDockerContainerCommand();
            this.startContainerToolStripButton.Image = global::XenAdmin.Properties.Resources._001_PowerOn_h32bit_24;
            this.startContainerToolStripButton.Name = "startContainerToolStripButton";
            // 
            // restartContainerToolStripButton
            // 
            resources.ApplyResources(this.restartContainerToolStripButton, "restartContainerToolStripButton");
            this.restartContainerToolStripButton.Command = new XenAdmin.Commands.RestartDockerContainerCommand();
            this.restartContainerToolStripButton.Image = global::XenAdmin.Properties.Resources._001_Reboot_h32bit_24;
            this.restartContainerToolStripButton.Name = "restartContainerToolStripButton";
            // 
            // resumeContainerToolStripButton
            // 
            resources.ApplyResources(this.resumeContainerToolStripButton, "resumeContainerToolStripButton");
            this.resumeContainerToolStripButton.Command = new XenAdmin.Commands.ResumeDockerContainerCommand();
            this.resumeContainerToolStripButton.Image = global::XenAdmin.Properties.Resources._000_Resumed_h32bit_24;
            this.resumeContainerToolStripButton.Name = "resumeContainerToolStripButton";
            // 
            // pauseContainerToolStripButton
            // 
            resources.ApplyResources(this.pauseContainerToolStripButton, "pauseContainerToolStripButton");
            this.pauseContainerToolStripButton.Command = new XenAdmin.Commands.PauseDockerContainerCommand();
            this.pauseContainerToolStripButton.Image = global::XenAdmin.Properties.Resources._000_Paused_h32bit_24;
            this.pauseContainerToolStripButton.Name = "pauseContainerToolStripButton";
            // 
            // ToolBarContextMenu
            // 
            resources.ApplyResources(this.ToolBarContextMenu, "ToolBarContextMenu");
            this.ToolBarContextMenu.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.ToolBarContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowToolbarMenuItem});
            this.ToolBarContextMenu.Name = "ToolBarContextMenu";
            this.statusToolTip.SetToolTip(this.ToolBarContextMenu, resources.GetString("ToolBarContextMenu.ToolTip"));
            // 
            // ShowToolbarMenuItem
            // 
            resources.ApplyResources(this.ShowToolbarMenuItem, "ShowToolbarMenuItem");
            this.ShowToolbarMenuItem.Checked = true;
            this.ShowToolbarMenuItem.CheckOnClick = true;
            this.ShowToolbarMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowToolbarMenuItem.Name = "ShowToolbarMenuItem";
            this.ShowToolbarMenuItem.Click += new System.EventHandler(this.ShowToolbarMenuItem_Click);
            // 
            // MainMenuBar
            // 
            resources.ApplyResources(this.MainMenuBar, "MainMenuBar");
            this.MainMenuBar.ClickThrough = true;
            this.MainMenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.poolToolStripMenuItem,
            this.HostMenuItem,
            this.VMToolStripMenuItem,
            this.StorageToolStripMenuItem,
            this.templatesToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MainMenuBar.Name = "MainMenuBar";
            this.statusToolTip.SetToolTip(this.MainMenuBar, resources.GetString("MainMenuBar.ToolTip"));
            this.MainMenuBar.MenuActivate += new System.EventHandler(this.MainMenuBar_MenuActivate);
            this.MainMenuBar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainMenuBar_MouseClick);
            // 
            // fileToolStripMenuItem
            // 
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            this.fileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileImportVMToolStripMenuItem,
            this.importSearchToolStripMenuItem,
            this.toolStripSeparator21,
            this.importSettingsToolStripMenuItem,
            this.exportSettingsToolStripMenuItem,
            this.toolStripSeparator31,
            this.pluginItemsPlaceHolderToolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            // 
            // FileImportVMToolStripMenuItem
            // 
            resources.ApplyResources(this.FileImportVMToolStripMenuItem, "FileImportVMToolStripMenuItem");
            this.FileImportVMToolStripMenuItem.Command = new XenAdmin.Commands.ImportCommand();
            this.FileImportVMToolStripMenuItem.Name = "FileImportVMToolStripMenuItem";
            // 
            // importSearchToolStripMenuItem
            // 
            resources.ApplyResources(this.importSearchToolStripMenuItem, "importSearchToolStripMenuItem");
            this.importSearchToolStripMenuItem.Command = new XenAdmin.Commands.ImportSearchCommand();
            this.importSearchToolStripMenuItem.Name = "importSearchToolStripMenuItem";
            // 
            // toolStripSeparator21
            // 
            resources.ApplyResources(this.toolStripSeparator21, "toolStripSeparator21");
            this.toolStripSeparator21.Name = "toolStripSeparator21";
            // 
            // importSettingsToolStripMenuItem
            // 
            resources.ApplyResources(this.importSettingsToolStripMenuItem, "importSettingsToolStripMenuItem");
            this.importSettingsToolStripMenuItem.Name = "importSettingsToolStripMenuItem";
            this.importSettingsToolStripMenuItem.Click += new System.EventHandler(this.importSettingsToolStripMenuItem_Click);
            // 
            // exportSettingsToolStripMenuItem
            // 
            resources.ApplyResources(this.exportSettingsToolStripMenuItem, "exportSettingsToolStripMenuItem");
            this.exportSettingsToolStripMenuItem.Name = "exportSettingsToolStripMenuItem";
            this.exportSettingsToolStripMenuItem.Click += new System.EventHandler(this.exportSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator31
            // 
            resources.ApplyResources(this.toolStripSeparator31, "toolStripSeparator31");
            this.toolStripSeparator31.Name = "toolStripSeparator31";
            // 
            // pluginItemsPlaceHolderToolStripMenuItem1
            // 
            resources.ApplyResources(this.pluginItemsPlaceHolderToolStripMenuItem1, "pluginItemsPlaceHolderToolStripMenuItem1");
            this.pluginItemsPlaceHolderToolStripMenuItem1.Name = "pluginItemsPlaceHolderToolStripMenuItem1";
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            this.viewToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customTemplatesToolStripMenuItem,
            this.templatesToolStripMenuItem1,
            this.localStorageToolStripMenuItem,
            this.ShowHiddenObjectsToolStripMenuItem,
            this.toolStripSeparator24,
            this.pluginItemsPlaceHolderToolStripMenuItem,
            this.toolbarToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            // 
            // customTemplatesToolStripMenuItem
            // 
            resources.ApplyResources(this.customTemplatesToolStripMenuItem, "customTemplatesToolStripMenuItem");
            this.customTemplatesToolStripMenuItem.Name = "customTemplatesToolStripMenuItem";
            this.customTemplatesToolStripMenuItem.Click += new System.EventHandler(this.customTemplatesToolStripMenuItem_Click);
            // 
            // templatesToolStripMenuItem1
            // 
            resources.ApplyResources(this.templatesToolStripMenuItem1, "templatesToolStripMenuItem1");
            this.templatesToolStripMenuItem1.Name = "templatesToolStripMenuItem1";
            this.templatesToolStripMenuItem1.Click += new System.EventHandler(this.templatesToolStripMenuItem1_Click);
            // 
            // localStorageToolStripMenuItem
            // 
            resources.ApplyResources(this.localStorageToolStripMenuItem, "localStorageToolStripMenuItem");
            this.localStorageToolStripMenuItem.Name = "localStorageToolStripMenuItem";
            this.localStorageToolStripMenuItem.Click += new System.EventHandler(this.localStorageToolStripMenuItem_Click);
            // 
            // ShowHiddenObjectsToolStripMenuItem
            // 
            resources.ApplyResources(this.ShowHiddenObjectsToolStripMenuItem, "ShowHiddenObjectsToolStripMenuItem");
            this.ShowHiddenObjectsToolStripMenuItem.Name = "ShowHiddenObjectsToolStripMenuItem";
            this.ShowHiddenObjectsToolStripMenuItem.Click += new System.EventHandler(this.ShowHiddenObjectsToolStripMenuItem_Click);
            // 
            // toolStripSeparator24
            // 
            resources.ApplyResources(this.toolStripSeparator24, "toolStripSeparator24");
            this.toolStripSeparator24.Name = "toolStripSeparator24";
            // 
            // pluginItemsPlaceHolderToolStripMenuItem
            // 
            resources.ApplyResources(this.pluginItemsPlaceHolderToolStripMenuItem, "pluginItemsPlaceHolderToolStripMenuItem");
            this.pluginItemsPlaceHolderToolStripMenuItem.Name = "pluginItemsPlaceHolderToolStripMenuItem";
            // 
            // toolbarToolStripMenuItem
            // 
            resources.ApplyResources(this.toolbarToolStripMenuItem, "toolbarToolStripMenuItem");
            this.toolbarToolStripMenuItem.Name = "toolbarToolStripMenuItem";
            this.toolbarToolStripMenuItem.Click += new System.EventHandler(this.ShowToolbarMenuItem_Click);
            // 
            // poolToolStripMenuItem
            // 
            resources.ApplyResources(this.poolToolStripMenuItem, "poolToolStripMenuItem");
            this.poolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddPoolToolStripMenuItem,
            this.toolStripSeparator8,
            this.addServerToolStripMenuItem,
            this.removeServerToolStripMenuItem,
            this.poolReconnectAsToolStripMenuItem,
            this.disconnectPoolToolStripMenuItem,
            this.toolStripSeparator27,
            this.virtualAppliancesToolStripMenuItem,
            this.toolStripSeparator30,
            this.highAvailabilityToolStripMenuItem,
            this.disasterRecoveryToolStripMenuItem,
            this.VMSnapshotScheduleToolStripMenuItem,
            this.vMProtectionAndRecoveryToolStripMenuItem,
            this.exportResourceReportPoolToolStripMenuItem,
            this.wlbReportsToolStripMenuItem,
            this.wlbDisconnectToolStripMenuItem,
            this.toolStripSeparator9,
            this.changePoolPasswordToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator26,
            this.pluginItemsPlaceHolderToolStripMenuItem2,
            this.PoolPropertiesToolStripMenuItem});
            this.poolToolStripMenuItem.Name = "poolToolStripMenuItem";
            // 
            // AddPoolToolStripMenuItem
            // 
            resources.ApplyResources(this.AddPoolToolStripMenuItem, "AddPoolToolStripMenuItem");
            this.AddPoolToolStripMenuItem.Command = new XenAdmin.Commands.NewPoolCommand();
            this.AddPoolToolStripMenuItem.Image = global::XenAdmin.Properties.Resources._000_PoolNew_h32bit_16;
            this.AddPoolToolStripMenuItem.Name = "AddPoolToolStripMenuItem";
            // 
            // toolStripSeparator8
            // 
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            // 
            // addServerToolStripMenuItem
            // 
            resources.ApplyResources(this.addServerToolStripMenuItem, "addServerToolStripMenuItem");
            this.addServerToolStripMenuItem.Name = "addServerToolStripMenuItem";
            // 
            // removeServerToolStripMenuItem
            // 
            resources.ApplyResources(this.removeServerToolStripMenuItem, "removeServerToolStripMenuItem");
            this.removeServerToolStripMenuItem.Command = new XenAdmin.Commands.RemoveHostFromPoolCommand();
            this.removeServerToolStripMenuItem.Name = "removeServerToolStripMenuItem";
            // 
            // poolReconnectAsToolStripMenuItem
            // 
            resources.ApplyResources(this.poolReconnectAsToolStripMenuItem, "poolReconnectAsToolStripMenuItem");
            this.poolReconnectAsToolStripMenuItem.Command = new XenAdmin.Commands.PoolReconnectAsCommand();
            this.poolReconnectAsToolStripMenuItem.Name = "poolReconnectAsToolStripMenuItem";
            // 
            // disconnectPoolToolStripMenuItem
            // 
            resources.ApplyResources(this.disconnectPoolToolStripMenuItem, "disconnectPoolToolStripMenuItem");
            this.disconnectPoolToolStripMenuItem.Command = new XenAdmin.Commands.DisconnectPoolCommand();
            this.disconnectPoolToolStripMenuItem.Name = "disconnectPoolToolStripMenuItem";
            // 
            // toolStripSeparator27
            // 
            resources.ApplyResources(this.toolStripSeparator27, "toolStripSeparator27");
            this.toolStripSeparator27.Name = "toolStripSeparator27";
            // 
            // virtualAppliancesToolStripMenuItem
            // 
            resources.ApplyResources(this.virtualAppliancesToolStripMenuItem, "virtualAppliancesToolStripMenuItem");
            this.virtualAppliancesToolStripMenuItem.Command = new XenAdmin.Commands.VMGroupCommandVM_appliance();
            this.virtualAppliancesToolStripMenuItem.Name = "virtualAppliancesToolStripMenuItem";
            // 
            // toolStripSeparator30
            // 
            resources.ApplyResources(this.toolStripSeparator30, "toolStripSeparator30");
            this.toolStripSeparator30.Name = "toolStripSeparator30";
            // 
            // highAvailabilityToolStripMenuItem
            // 
            resources.ApplyResources(this.highAvailabilityToolStripMenuItem, "highAvailabilityToolStripMenuItem");
            this.highAvailabilityToolStripMenuItem.Command = new XenAdmin.Commands.HACommand();
            this.highAvailabilityToolStripMenuItem.Name = "highAvailabilityToolStripMenuItem";
            // 
            // disasterRecoveryToolStripMenuItem
            // 
            resources.ApplyResources(this.disasterRecoveryToolStripMenuItem, "disasterRecoveryToolStripMenuItem");
            this.disasterRecoveryToolStripMenuItem.Command = new XenAdmin.Commands.DRCommand();
            this.disasterRecoveryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drConfigureToolStripMenuItem,
            this.DrWizardToolStripMenuItem});
            this.disasterRecoveryToolStripMenuItem.Name = "disasterRecoveryToolStripMenuItem";
            // 
            // drConfigureToolStripMenuItem
            // 
            resources.ApplyResources(this.drConfigureToolStripMenuItem, "drConfigureToolStripMenuItem");
            this.drConfigureToolStripMenuItem.Command = new XenAdmin.Commands.DRConfigureCommand();
            this.drConfigureToolStripMenuItem.Name = "drConfigureToolStripMenuItem";
            // 
            // DrWizardToolStripMenuItem
            // 
            resources.ApplyResources(this.DrWizardToolStripMenuItem, "DrWizardToolStripMenuItem");
            this.DrWizardToolStripMenuItem.Command = new XenAdmin.Commands.DisasterRecoveryCommand();
            this.DrWizardToolStripMenuItem.Name = "DrWizardToolStripMenuItem";
            // 
            // VMSnapshotScheduleToolStripMenuItem
            // 
            resources.ApplyResources(this.VMSnapshotScheduleToolStripMenuItem, "VMSnapshotScheduleToolStripMenuItem");
            this.VMSnapshotScheduleToolStripMenuItem.Command = new XenAdmin.Commands.VMGroupCommandVMSS();
            this.VMSnapshotScheduleToolStripMenuItem.Name = "VMSnapshotScheduleToolStripMenuItem";
            // 
            // vMProtectionAndRecoveryToolStripMenuItem
            // 
            resources.ApplyResources(this.vMProtectionAndRecoveryToolStripMenuItem, "vMProtectionAndRecoveryToolStripMenuItem");
            this.vMProtectionAndRecoveryToolStripMenuItem.Command = new XenAdmin.Commands.VMGroupCommandVMPP();
            this.vMProtectionAndRecoveryToolStripMenuItem.Name = "vMProtectionAndRecoveryToolStripMenuItem";
            // 
            // exportResourceReportPoolToolStripMenuItem
            // 
            resources.ApplyResources(this.exportResourceReportPoolToolStripMenuItem, "exportResourceReportPoolToolStripMenuItem");
            this.exportResourceReportPoolToolStripMenuItem.Command = new XenAdmin.Commands.ExportResourceReportCommand();
            this.exportResourceReportPoolToolStripMenuItem.Name = "exportResourceReportPoolToolStripMenuItem";
            // 
            // wlbReportsToolStripMenuItem
            // 
            resources.ApplyResources(this.wlbReportsToolStripMenuItem, "wlbReportsToolStripMenuItem");
            this.wlbReportsToolStripMenuItem.Command = new XenAdmin.Commands.ViewWorkloadReportsCommand();
            this.wlbReportsToolStripMenuItem.Name = "wlbReportsToolStripMenuItem";
            // 
            // wlbDisconnectToolStripMenuItem
            // 
            resources.ApplyResources(this.wlbDisconnectToolStripMenuItem, "wlbDisconnectToolStripMenuItem");
            this.wlbDisconnectToolStripMenuItem.Command = new XenAdmin.Commands.DisconnectWlbServerCommand();
            this.wlbDisconnectToolStripMenuItem.Name = "wlbDisconnectToolStripMenuItem";
            // 
            // toolStripSeparator9
            // 
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            // 
            // changePoolPasswordToolStripMenuItem
            // 
            resources.ApplyResources(this.changePoolPasswordToolStripMenuItem, "changePoolPasswordToolStripMenuItem");
            this.changePoolPasswordToolStripMenuItem.Command = new XenAdmin.Commands.ChangeHostPasswordCommand();
            this.changePoolPasswordToolStripMenuItem.Name = "changePoolPasswordToolStripMenuItem";
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // deleteToolStripMenuItem
            // 
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Command = new XenAdmin.Commands.DeletePoolCommand();
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            // 
            // toolStripSeparator26
            // 
            resources.ApplyResources(this.toolStripSeparator26, "toolStripSeparator26");
            this.toolStripSeparator26.Name = "toolStripSeparator26";
            // 
            // pluginItemsPlaceHolderToolStripMenuItem2
            // 
            resources.ApplyResources(this.pluginItemsPlaceHolderToolStripMenuItem2, "pluginItemsPlaceHolderToolStripMenuItem2");
            this.pluginItemsPlaceHolderToolStripMenuItem2.Name = "pluginItemsPlaceHolderToolStripMenuItem2";
            // 
            // PoolPropertiesToolStripMenuItem
            // 
            resources.ApplyResources(this.PoolPropertiesToolStripMenuItem, "PoolPropertiesToolStripMenuItem");
            this.PoolPropertiesToolStripMenuItem.Command = new XenAdmin.Commands.PoolPropertiesCommand();
            this.PoolPropertiesToolStripMenuItem.Image = global::XenAdmin.Properties.Resources.edit_16;
            this.PoolPropertiesToolStripMenuItem.Name = "PoolPropertiesToolStripMenuItem";
            // 
            // HostMenuItem
            // 
            resources.ApplyResources(this.HostMenuItem, "HostMenuItem");
            this.HostMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddHostToolStripMenuItem,
            this.toolStripMenuItem11,
            this.RebootHostToolStripMenuItem,
            this.powerOnToolStripMenuItem,
            this.ShutdownHostToolStripMenuItem,
            this.restartToolstackToolStripMenuItem,
            this.toolStripSeparator1,
            this.connectDisconnectToolStripMenuItem,
            this.addServerToPoolMenuItem,
            this.toolStripSeparator3,
            this.backupToolStripMenuItem,
            this.restoreFromBackupToolStripMenuItem,
            this.toolStripSeparator23,
            this.maintenanceModeToolStripMenuItem1,
            this.controlDomainMemoryToolStripMenuItem,
            this.commandToolStripMenuItem1,
            this.installLicenseToolStripMenuItem,
            this.HostPasswordToolStripMenuItem,
            this.toolStripSeparator25,
            this.destroyServerToolStripMenuItem,
            this.removeHostToolStripMenuItem,
            this.toolStripSeparator15,
            this.pluginItemsPlaceHolderToolStripMenuItem3,
            this.ServerPropertiesToolStripMenuItem});
            this.HostMenuItem.Name = "HostMenuItem";
            // 
            // AddHostToolStripMenuItem
            // 
            resources.ApplyResources(this.AddHostToolStripMenuItem, "AddHostToolStripMenuItem");
            this.AddHostToolStripMenuItem.Command = new XenAdmin.Commands.AddHostCommand();
            this.AddHostToolStripMenuItem.Image = global::XenAdmin.Properties.Resources._000_AddApplicationServer_h32bit_16;
            this.AddHostToolStripMenuItem.Name = "AddHostToolStripMenuItem";
            // 
            // toolStripMenuItem11
            // 
            resources.ApplyResources(this.toolStripMenuItem11, "toolStripMenuItem11");
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            // 
            // RebootHostToolStripMenuItem
            // 
            resources.ApplyResources(this.RebootHostToolStripMenuItem, "RebootHostToolStripMenuItem");
            this.RebootHostToolStripMenuItem.Command = new XenAdmin.Commands.RebootHostCommand();
            this.RebootHostToolStripMenuItem.Image = global::XenAdmin.Properties.Resources._001_Reboot_h32bit_16;
            this.RebootHostToolStripMenuItem.Name = "RebootHostToolStripMenuItem";
            // 
            // powerOnToolStripMenuItem
            // 
            resources.ApplyResources(this.powerOnToolStripMenuItem, "powerOnToolStripMenuItem");
            this.powerOnToolStripMenuItem.Command = new XenAdmin.Commands.PowerOnHostCommand();
            this.powerOnToolStripMenuItem.Name = "powerOnToolStripMenuItem";
            // 
            // ShutdownHostToolStripMenuItem
            // 
            resources.ApplyResources(this.ShutdownHostToolStripMenuItem, "ShutdownHostToolStripMenuItem");
            this.ShutdownHostToolStripMenuItem.Command = new XenAdmin.Commands.ShutDownHostCommand();
            this.ShutdownHostToolStripMenuItem.Name = "ShutdownHostToolStripMenuItem";
            // 
            // restartToolstackToolStripMenuItem
            // 
            resources.ApplyResources(this.restartToolstackToolStripMenuItem, "restartToolstackToolStripMenuItem");
            this.restartToolstackToolStripMenuItem.Command = new XenAdmin.Commands.RestartToolstackCommand();
            this.restartToolstackToolStripMenuItem.Name = "restartToolstackToolStripMenuItem";
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // connectDisconnectToolStripMenuItem
            // 
            resources.ApplyResources(this.connectDisconnectToolStripMenuItem, "connectDisconnectToolStripMenuItem");
            this.connectDisconnectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ReconnectToolStripMenuItem1,
            this.DisconnectToolStripMenuItem,
            this.reconnectAsToolStripMenuItem,
            this.toolStripSeparator4,
            this.connectAllToolStripMenuItem,
            this.disconnectAllToolStripMenuItem});
            this.connectDisconnectToolStripMenuItem.Name = "connectDisconnectToolStripMenuItem";
            // 
            // ReconnectToolStripMenuItem1
            // 
            resources.ApplyResources(this.ReconnectToolStripMenuItem1, "ReconnectToolStripMenuItem1");
            this.ReconnectToolStripMenuItem1.Command = new XenAdmin.Commands.ReconnectHostCommand();
            this.ReconnectToolStripMenuItem1.Name = "ReconnectToolStripMenuItem1";
            // 
            // DisconnectToolStripMenuItem
            // 
            resources.ApplyResources(this.DisconnectToolStripMenuItem, "DisconnectToolStripMenuItem");
            this.DisconnectToolStripMenuItem.Command = new XenAdmin.Commands.DisconnectHostCommand();
            this.DisconnectToolStripMenuItem.Name = "DisconnectToolStripMenuItem";
            // 
            // reconnectAsToolStripMenuItem
            // 
            resources.ApplyResources(this.reconnectAsToolStripMenuItem, "reconnectAsToolStripMenuItem");
            this.reconnectAsToolStripMenuItem.Command = new XenAdmin.Commands.HostReconnectAsCommand();
            this.reconnectAsToolStripMenuItem.Name = "reconnectAsToolStripMenuItem";
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // connectAllToolStripMenuItem
            // 
            resources.ApplyResources(this.connectAllToolStripMenuItem, "connectAllToolStripMenuItem");
            this.connectAllToolStripMenuItem.Command = new XenAdmin.Commands.ConnectAllHostsCommand();
            this.connectAllToolStripMenuItem.Name = "connectAllToolStripMenuItem";
            // 
            // disconnectAllToolStripMenuItem
            // 
            resources.ApplyResources(this.disconnectAllToolStripMenuItem, "disconnectAllToolStripMenuItem");
            this.disconnectAllToolStripMenuItem.Command = new XenAdmin.Commands.DisconnectAllHostsCommand();
            this.disconnectAllToolStripMenuItem.Name = "disconnectAllToolStripMenuItem";
            // 
            // addServerToPoolMenuItem
            // 
            resources.ApplyResources(this.addServerToPoolMenuItem, "addServerToPoolMenuItem");
            this.addServerToPoolMenuItem.Name = "addServerToPoolMenuItem";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // backupToolStripMenuItem
            // 
            resources.ApplyResources(this.backupToolStripMenuItem, "backupToolStripMenuItem");
            this.backupToolStripMenuItem.Command = new XenAdmin.Commands.BackupHostCommand();
            this.backupToolStripMenuItem.Name = "backupToolStripMenuItem";
            // 
            // restoreFromBackupToolStripMenuItem
            // 
            resources.ApplyResources(this.restoreFromBackupToolStripMenuItem, "restoreFromBackupToolStripMenuItem");
            this.restoreFromBackupToolStripMenuItem.Command = new XenAdmin.Commands.RestoreHostFromBackupCommand();
            this.restoreFromBackupToolStripMenuItem.Name = "restoreFromBackupToolStripMenuItem";
            // 
            // toolStripSeparator23
            // 
            resources.ApplyResources(this.toolStripSeparator23, "toolStripSeparator23");
            this.toolStripSeparator23.Name = "toolStripSeparator23";
            // 
            // maintenanceModeToolStripMenuItem1
            // 
            resources.ApplyResources(this.maintenanceModeToolStripMenuItem1, "maintenanceModeToolStripMenuItem1");
            this.maintenanceModeToolStripMenuItem1.Command = new XenAdmin.Commands.HostMaintenanceModeCommand();
            this.maintenanceModeToolStripMenuItem1.Name = "maintenanceModeToolStripMenuItem1";
            // 
            // controlDomainMemoryToolStripMenuItem
            // 
            resources.ApplyResources(this.controlDomainMemoryToolStripMenuItem, "controlDomainMemoryToolStripMenuItem");
            this.controlDomainMemoryToolStripMenuItem.Command = new XenAdmin.Commands.ChangeControlDomainMemoryCommand();
            this.controlDomainMemoryToolStripMenuItem.Name = "controlDomainMemoryToolStripMenuItem";
            // 
            // installLicenseToolStripMenuItem
            // 
            resources.ApplyResources(this.installLicenseToolStripMenuItem, "installLicenseToolStripMenuItem");
            this.installLicenseToolStripMenuItem.Command = new XenAdmin.Commands.InstallLicenseCommand();
            this.installLicenseToolStripMenuItem.Name = "installLicenseToolStripMenuItem";
            // 
            // HostPasswordToolStripMenuItem
            // 
            resources.ApplyResources(this.HostPasswordToolStripMenuItem, "HostPasswordToolStripMenuItem");
            this.HostPasswordToolStripMenuItem.Command = new XenAdmin.Commands.HostPasswordCommand();
            this.HostPasswordToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangeRootPasswordToolStripMenuItem,
            this.forgetSavedPasswordToolStripMenuItem});
            this.HostPasswordToolStripMenuItem.Name = "HostPasswordToolStripMenuItem";
            // 
            // ChangeRootPasswordToolStripMenuItem
            // 
            resources.ApplyResources(this.ChangeRootPasswordToolStripMenuItem, "ChangeRootPasswordToolStripMenuItem");
            this.ChangeRootPasswordToolStripMenuItem.Command = new XenAdmin.Commands.ChangeHostPasswordCommand();
            this.ChangeRootPasswordToolStripMenuItem.Name = "ChangeRootPasswordToolStripMenuItem";
            // 
            // forgetSavedPasswordToolStripMenuItem
            // 
            resources.ApplyResources(this.forgetSavedPasswordToolStripMenuItem, "forgetSavedPasswordToolStripMenuItem");
            this.forgetSavedPasswordToolStripMenuItem.Command = new XenAdmin.Commands.ForgetSavedPasswordCommand();
            this.forgetSavedPasswordToolStripMenuItem.Name = "forgetSavedPasswordToolStripMenuItem";
            // 
            // toolStripSeparator25
            // 
            resources.ApplyResources(this.toolStripSeparator25, "toolStripSeparator25");
            this.toolStripSeparator25.Name = "toolStripSeparator25";
            // 
            // destroyServerToolStripMenuItem
            // 
            resources.ApplyResources(this.destroyServerToolStripMenuItem, "destroyServerToolStripMenuItem");
            this.destroyServerToolStripMenuItem.Command = new XenAdmin.Commands.DestroyHostCommand();
            this.destroyServerToolStripMenuItem.Name = "destroyServerToolStripMenuItem";
            // 
            // removeHostToolStripMenuItem
            // 
            resources.ApplyResources(this.removeHostToolStripMenuItem, "removeHostToolStripMenuItem");
            this.removeHostToolStripMenuItem.Command = new XenAdmin.Commands.RemoveHostCommand();
            this.removeHostToolStripMenuItem.Name = "removeHostToolStripMenuItem";
            // 
            // toolStripSeparator15
            // 
            resources.ApplyResources(this.toolStripSeparator15, "toolStripSeparator15");
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            // 
            // pluginItemsPlaceHolderToolStripMenuItem3
            // 
            resources.ApplyResources(this.pluginItemsPlaceHolderToolStripMenuItem3, "pluginItemsPlaceHolderToolStripMenuItem3");
            this.pluginItemsPlaceHolderToolStripMenuItem3.Name = "pluginItemsPlaceHolderToolStripMenuItem3";
            // 
            // ServerPropertiesToolStripMenuItem
            // 
            resources.ApplyResources(this.ServerPropertiesToolStripMenuItem, "ServerPropertiesToolStripMenuItem");
            this.ServerPropertiesToolStripMenuItem.Command = new XenAdmin.Commands.HostPropertiesCommand();
            this.ServerPropertiesToolStripMenuItem.Image = global::XenAdmin.Properties.Resources.edit_16;
            this.ServerPropertiesToolStripMenuItem.Name = "ServerPropertiesToolStripMenuItem";
            // 
            // VMToolStripMenuItem
            // 
            resources.ApplyResources(this.VMToolStripMenuItem, "VMToolStripMenuItem");
            this.VMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewVmToolStripMenuItem,
            this.startShutdownToolStripMenuItem,
            this.resumeOnToolStripMenuItem,
            this.relocateToolStripMenuItem,
            this.startOnHostToolStripMenuItem,
            this.toolStripSeparator20,
            this.assignSnapshotScheduleToolStripMenuItem,
            this.assignPolicyToolStripMenuItem,
            this.assignToVirtualApplianceToolStripMenuItem,
            this.toolStripMenuItem9,
            this.copyVMtoSharedStorageMenuItem,
            this.MoveVMToolStripMenuItem,
            this.snapshotToolStripMenuItem,
            this.convertToTemplateToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.enablePVSReadcachingToolStripMenuItem,
            this.disablePVSReadcachingToolStripMenuItem,
            this.toolStripMenuItem12,
            this.installToolsToolStripMenuItem,
            this.sendCtrlAltDelToolStripMenuItem,
            this.toolStripSeparator5,
            this.uninstallToolStripMenuItem,
            this.toolStripSeparator10,
            this.pluginItemsPlaceHolderToolStripMenuItem4,
            this.VMPropertiesToolStripMenuItem});
            this.VMToolStripMenuItem.Name = "VMToolStripMenuItem";
            // 
            // NewVmToolStripMenuItem
            // 
            resources.ApplyResources(this.NewVmToolStripMenuItem, "NewVmToolStripMenuItem");
            this.NewVmToolStripMenuItem.Command = new XenAdmin.Commands.NewVMCommand();
            this.NewVmToolStripMenuItem.Image = global::XenAdmin.Properties.Resources._001_CreateVM_h32bit_16;
            this.NewVmToolStripMenuItem.Name = "NewVmToolStripMenuItem";
            // 
            // startShutdownToolStripMenuItem
            // 
            resources.ApplyResources(this.startShutdownToolStripMenuItem, "startShutdownToolStripMenuItem");
            this.startShutdownToolStripMenuItem.Name = "startShutdownToolStripMenuItem";
            // 
            // resumeOnToolStripMenuItem
            // 
            resources.ApplyResources(this.resumeOnToolStripMenuItem, "resumeOnToolStripMenuItem");
            this.resumeOnToolStripMenuItem.Name = "resumeOnToolStripMenuItem";
            // 
            // relocateToolStripMenuItem
            // 
            resources.ApplyResources(this.relocateToolStripMenuItem, "relocateToolStripMenuItem");
            this.relocateToolStripMenuItem.Name = "relocateToolStripMenuItem";
            // 
            // startOnHostToolStripMenuItem
            // 
            resources.ApplyResources(this.startOnHostToolStripMenuItem, "startOnHostToolStripMenuItem");
            this.startOnHostToolStripMenuItem.Name = "startOnHostToolStripMenuItem";
            // 
            // toolStripSeparator20
            // 
            resources.ApplyResources(this.toolStripSeparator20, "toolStripSeparator20");
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            // 
            // assignSnapshotScheduleToolStripMenuItem
            // 
            resources.ApplyResources(this.assignSnapshotScheduleToolStripMenuItem, "assignSnapshotScheduleToolStripMenuItem");
            this.assignSnapshotScheduleToolStripMenuItem.Name = "assignSnapshotScheduleToolStripMenuItem";
            // 
            // assignPolicyToolStripMenuItem
            // 
            resources.ApplyResources(this.assignPolicyToolStripMenuItem, "assignPolicyToolStripMenuItem");
            this.assignPolicyToolStripMenuItem.Name = "assignPolicyToolStripMenuItem";
            // 
            // assignToVirtualApplianceToolStripMenuItem
            // 
            resources.ApplyResources(this.assignToVirtualApplianceToolStripMenuItem, "assignToVirtualApplianceToolStripMenuItem");
            this.assignToVirtualApplianceToolStripMenuItem.Name = "assignToVirtualApplianceToolStripMenuItem";
            // 
            // toolStripMenuItem9
            // 
            resources.ApplyResources(this.toolStripMenuItem9, "toolStripMenuItem9");
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            // 
            // copyVMtoSharedStorageMenuItem
            // 
            resources.ApplyResources(this.copyVMtoSharedStorageMenuItem, "copyVMtoSharedStorageMenuItem");
            this.copyVMtoSharedStorageMenuItem.Command = new XenAdmin.Commands.CopyVMCommand();
            this.copyVMtoSharedStorageMenuItem.Name = "copyVMtoSharedStorageMenuItem";
            // 
            // MoveVMToolStripMenuItem
            // 
            resources.ApplyResources(this.MoveVMToolStripMenuItem, "MoveVMToolStripMenuItem");
            this.MoveVMToolStripMenuItem.Command = new XenAdmin.Commands.MoveVMCommand();
            this.MoveVMToolStripMenuItem.Name = "MoveVMToolStripMenuItem";
            // 
            // snapshotToolStripMenuItem
            // 
            resources.ApplyResources(this.snapshotToolStripMenuItem, "snapshotToolStripMenuItem");
            this.snapshotToolStripMenuItem.Command = new XenAdmin.Commands.TakeSnapshotCommand();
            this.snapshotToolStripMenuItem.Name = "snapshotToolStripMenuItem";
            // 
            // convertToTemplateToolStripMenuItem
            // 
            resources.ApplyResources(this.convertToTemplateToolStripMenuItem, "convertToTemplateToolStripMenuItem");
            this.convertToTemplateToolStripMenuItem.Command = new XenAdmin.Commands.ConvertVMToTemplateCommand();
            this.convertToTemplateToolStripMenuItem.Name = "convertToTemplateToolStripMenuItem";
            // 
            // exportToolStripMenuItem
            // 
            resources.ApplyResources(this.exportToolStripMenuItem, "exportToolStripMenuItem");
            this.exportToolStripMenuItem.Command = new XenAdmin.Commands.ExportCommand();
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            // 
            // enablePVSReadcachingToolStripMenuItem
            // 
            resources.ApplyResources(this.enablePVSReadcachingToolStripMenuItem, "enablePVSReadcachingToolStripMenuItem");
            this.enablePVSReadcachingToolStripMenuItem.Command = new XenAdmin.Commands.EnablePvsReadCachingCommand();
            this.enablePVSReadcachingToolStripMenuItem.Name = "enablePVSReadcachingToolStripMenuItem";
            // 
            // disablePVSReadcachingToolStripMenuItem
            // 
            resources.ApplyResources(this.disablePVSReadcachingToolStripMenuItem, "disablePVSReadcachingToolStripMenuItem");
            this.disablePVSReadcachingToolStripMenuItem.Command = new XenAdmin.Commands.DisablePvsReadCachingCommand();
            this.disablePVSReadcachingToolStripMenuItem.Name = "disablePVSReadcachingToolStripMenuItem";
            // 
            // toolStripMenuItem12
            // 
            resources.ApplyResources(this.toolStripMenuItem12, "toolStripMenuItem12");
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            // 
            // installToolsToolStripMenuItem
            // 
            resources.ApplyResources(this.installToolsToolStripMenuItem, "installToolsToolStripMenuItem");
            this.installToolsToolStripMenuItem.Command = new XenAdmin.Commands.InstallToolsCommand();
            this.installToolsToolStripMenuItem.Name = "installToolsToolStripMenuItem";
            // 
            // sendCtrlAltDelToolStripMenuItem
            // 
            resources.ApplyResources(this.sendCtrlAltDelToolStripMenuItem, "sendCtrlAltDelToolStripMenuItem");
            this.sendCtrlAltDelToolStripMenuItem.Name = "sendCtrlAltDelToolStripMenuItem";
            this.sendCtrlAltDelToolStripMenuItem.Click += new System.EventHandler(this.sendCtrlAltDelToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // uninstallToolStripMenuItem
            // 
            resources.ApplyResources(this.uninstallToolStripMenuItem, "uninstallToolStripMenuItem");
            this.uninstallToolStripMenuItem.Command = new XenAdmin.Commands.DeleteVMCommand();
            this.uninstallToolStripMenuItem.Name = "uninstallToolStripMenuItem";
            // 
            // toolStripSeparator10
            // 
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            // 
            // pluginItemsPlaceHolderToolStripMenuItem4
            // 
            resources.ApplyResources(this.pluginItemsPlaceHolderToolStripMenuItem4, "pluginItemsPlaceHolderToolStripMenuItem4");
            this.pluginItemsPlaceHolderToolStripMenuItem4.Name = "pluginItemsPlaceHolderToolStripMenuItem4";
            // 
            // VMPropertiesToolStripMenuItem
            // 
            resources.ApplyResources(this.VMPropertiesToolStripMenuItem, "VMPropertiesToolStripMenuItem");
            this.VMPropertiesToolStripMenuItem.Command = new XenAdmin.Commands.VMPropertiesCommand();
            this.VMPropertiesToolStripMenuItem.Image = global::XenAdmin.Properties.Resources.edit_16;
            this.VMPropertiesToolStripMenuItem.Name = "VMPropertiesToolStripMenuItem";
            // 
            // StorageToolStripMenuItem
            // 
            resources.ApplyResources(this.StorageToolStripMenuItem, "StorageToolStripMenuItem");
            this.StorageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddStorageToolStripMenuItem,
            this.toolStripSeparator22,
            this.RepairStorageToolStripMenuItem,
            this.DefaultSRToolStripMenuItem,
            this.toolStripSeparator2,
            this.virtualDisksToolStripMenuItem,
            this.reclaimFreedSpacetripMenuItem,
            this.toolStripSeparator19,
            this.DetachStorageToolStripMenuItem,
            this.ReattachStorageRepositoryToolStripMenuItem,
            this.ForgetStorageRepositoryToolStripMenuItem,
            this.DestroyStorageRepositoryToolStripMenuItem,
            this.ConvertToThinStorageRepositoryToolStripMenuItem,
            this.toolStripSeparator18,
            this.pluginItemsPlaceHolderToolStripMenuItem5,
            this.SRPropertiesToolStripMenuItem});
            this.StorageToolStripMenuItem.Name = "StorageToolStripMenuItem";
            // 
            // AddStorageToolStripMenuItem
            // 
            resources.ApplyResources(this.AddStorageToolStripMenuItem, "AddStorageToolStripMenuItem");
            this.AddStorageToolStripMenuItem.Command = new XenAdmin.Commands.NewSRCommand();
            this.AddStorageToolStripMenuItem.Image = global::XenAdmin.Properties.Resources._000_NewStorage_h32bit_16;
            this.AddStorageToolStripMenuItem.Name = "AddStorageToolStripMenuItem";
            // 
            // toolStripSeparator22
            // 
            resources.ApplyResources(this.toolStripSeparator22, "toolStripSeparator22");
            this.toolStripSeparator22.Name = "toolStripSeparator22";
            // 
            // RepairStorageToolStripMenuItem
            // 
            resources.ApplyResources(this.RepairStorageToolStripMenuItem, "RepairStorageToolStripMenuItem");
            this.RepairStorageToolStripMenuItem.Command = new XenAdmin.Commands.RepairSRCommand();
            this.RepairStorageToolStripMenuItem.Image = global::XenAdmin.Properties.Resources._000_StorageBroken_h32bit_16;
            this.RepairStorageToolStripMenuItem.Name = "RepairStorageToolStripMenuItem";
            // 
            // DefaultSRToolStripMenuItem
            // 
            resources.ApplyResources(this.DefaultSRToolStripMenuItem, "DefaultSRToolStripMenuItem");
            this.DefaultSRToolStripMenuItem.Command = new XenAdmin.Commands.SetAsDefaultSRCommand();
            this.DefaultSRToolStripMenuItem.Image = global::XenAdmin.Properties.Resources._000_StorageDefault_h32bit_16;
            this.DefaultSRToolStripMenuItem.Name = "DefaultSRToolStripMenuItem";
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // virtualDisksToolStripMenuItem
            // 
            resources.ApplyResources(this.virtualDisksToolStripMenuItem, "virtualDisksToolStripMenuItem");
            this.virtualDisksToolStripMenuItem.Command = new XenAdmin.Commands.VirtualDiskCommand();
            this.virtualDisksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addVirtualDiskToolStripMenuItem,
            this.attachVirtualDiskToolStripMenuItem});
            this.virtualDisksToolStripMenuItem.Name = "virtualDisksToolStripMenuItem";
            // 
            // addVirtualDiskToolStripMenuItem
            // 
            resources.ApplyResources(this.addVirtualDiskToolStripMenuItem, "addVirtualDiskToolStripMenuItem");
            this.addVirtualDiskToolStripMenuItem.Command = new XenAdmin.Commands.AddVirtualDiskCommand();
            this.addVirtualDiskToolStripMenuItem.Name = "addVirtualDiskToolStripMenuItem";
            // 
            // attachVirtualDiskToolStripMenuItem
            // 
            resources.ApplyResources(this.attachVirtualDiskToolStripMenuItem, "attachVirtualDiskToolStripMenuItem");
            this.attachVirtualDiskToolStripMenuItem.Command = new XenAdmin.Commands.AttachVirtualDiskCommand();
            this.attachVirtualDiskToolStripMenuItem.Name = "attachVirtualDiskToolStripMenuItem";
            // 
            // reclaimFreedSpacetripMenuItem
            // 
            resources.ApplyResources(this.reclaimFreedSpacetripMenuItem, "reclaimFreedSpacetripMenuItem");
            this.reclaimFreedSpacetripMenuItem.Command = new XenAdmin.Commands.TrimSRCommand();
            this.reclaimFreedSpacetripMenuItem.Name = "reclaimFreedSpacetripMenuItem";
            // 
            // toolStripSeparator19
            // 
            resources.ApplyResources(this.toolStripSeparator19, "toolStripSeparator19");
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            // 
            // DetachStorageToolStripMenuItem
            // 
            resources.ApplyResources(this.DetachStorageToolStripMenuItem, "DetachStorageToolStripMenuItem");
            this.DetachStorageToolStripMenuItem.Command = new XenAdmin.Commands.DetachSRCommand();
            this.DetachStorageToolStripMenuItem.Name = "DetachStorageToolStripMenuItem";
            // 
            // ReattachStorageRepositoryToolStripMenuItem
            // 
            resources.ApplyResources(this.ReattachStorageRepositoryToolStripMenuItem, "ReattachStorageRepositoryToolStripMenuItem");
            this.ReattachStorageRepositoryToolStripMenuItem.Command = new XenAdmin.Commands.ReattachSRCommand();
            this.ReattachStorageRepositoryToolStripMenuItem.Name = "ReattachStorageRepositoryToolStripMenuItem";
            // 
            // ForgetStorageRepositoryToolStripMenuItem
            // 
            resources.ApplyResources(this.ForgetStorageRepositoryToolStripMenuItem, "ForgetStorageRepositoryToolStripMenuItem");
            this.ForgetStorageRepositoryToolStripMenuItem.Command = new XenAdmin.Commands.ForgetSRCommand();
            this.ForgetStorageRepositoryToolStripMenuItem.Name = "ForgetStorageRepositoryToolStripMenuItem";
            // 
            // DestroyStorageRepositoryToolStripMenuItem
            // 
            resources.ApplyResources(this.DestroyStorageRepositoryToolStripMenuItem, "DestroyStorageRepositoryToolStripMenuItem");
            this.DestroyStorageRepositoryToolStripMenuItem.Command = new XenAdmin.Commands.DestroySRCommand();
            this.DestroyStorageRepositoryToolStripMenuItem.Name = "DestroyStorageRepositoryToolStripMenuItem";
            // 
            // ConvertToThinStorageRepositoryToolStripMenuItem
            // 
            resources.ApplyResources(this.ConvertToThinStorageRepositoryToolStripMenuItem, "ConvertToThinStorageRepositoryToolStripMenuItem");
            this.ConvertToThinStorageRepositoryToolStripMenuItem.Command = new XenAdmin.Commands.ConvertToThinSRCommand();
            this.ConvertToThinStorageRepositoryToolStripMenuItem.Name = "ConvertToThinStorageRepositoryToolStripMenuItem";
            // 
            // toolStripSeparator18
            // 
            resources.ApplyResources(this.toolStripSeparator18, "toolStripSeparator18");
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            // 
            // pluginItemsPlaceHolderToolStripMenuItem5
            // 
            resources.ApplyResources(this.pluginItemsPlaceHolderToolStripMenuItem5, "pluginItemsPlaceHolderToolStripMenuItem5");
            this.pluginItemsPlaceHolderToolStripMenuItem5.Name = "pluginItemsPlaceHolderToolStripMenuItem5";
            // 
            // SRPropertiesToolStripMenuItem
            // 
            resources.ApplyResources(this.SRPropertiesToolStripMenuItem, "SRPropertiesToolStripMenuItem");
            this.SRPropertiesToolStripMenuItem.Command = new XenAdmin.Commands.SRPropertiesCommand();
            this.SRPropertiesToolStripMenuItem.Image = global::XenAdmin.Properties.Resources.edit_16;
            this.SRPropertiesToolStripMenuItem.Name = "SRPropertiesToolStripMenuItem";
            // 
            // templatesToolStripMenuItem
            // 
            resources.ApplyResources(this.templatesToolStripMenuItem, "templatesToolStripMenuItem");
            this.templatesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateVmFromTemplateToolStripMenuItem,
            this.toolStripSeparator29,
            this.exportTemplateToolStripMenuItem,
            this.duplicateTemplateToolStripMenuItem,
            this.toolStripSeparator16,
            this.uninstallTemplateToolStripMenuItem,
            this.toolStripSeparator28,
            this.pluginItemsPlaceHolderToolStripMenuItem6,
            this.templatePropertiesToolStripMenuItem});
            this.templatesToolStripMenuItem.Name = "templatesToolStripMenuItem";
            // 
            // CreateVmFromTemplateToolStripMenuItem
            // 
            resources.ApplyResources(this.CreateVmFromTemplateToolStripMenuItem, "CreateVmFromTemplateToolStripMenuItem");
            this.CreateVmFromTemplateToolStripMenuItem.Command = new XenAdmin.Commands.CreateVMFromTemplateCommand();
            this.CreateVmFromTemplateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newVMFromTemplateToolStripMenuItem,
            this.InstantVmToolStripMenuItem});
            this.CreateVmFromTemplateToolStripMenuItem.Image = global::XenAdmin.Properties.Resources._001_CreateVM_h32bit_16;
            this.CreateVmFromTemplateToolStripMenuItem.Name = "CreateVmFromTemplateToolStripMenuItem";
            // 
            // newVMFromTemplateToolStripMenuItem
            // 
            resources.ApplyResources(this.newVMFromTemplateToolStripMenuItem, "newVMFromTemplateToolStripMenuItem");
            this.newVMFromTemplateToolStripMenuItem.Command = new XenAdmin.Commands.NewVMFromTemplateCommand();
            this.newVMFromTemplateToolStripMenuItem.Image = global::XenAdmin.Properties.Resources._001_CreateVM_h32bit_16;
            this.newVMFromTemplateToolStripMenuItem.Name = "newVMFromTemplateToolStripMenuItem";
            // 
            // InstantVmToolStripMenuItem
            // 
            resources.ApplyResources(this.InstantVmToolStripMenuItem, "InstantVmToolStripMenuItem");
            this.InstantVmToolStripMenuItem.Command = new XenAdmin.Commands.InstantVMFromTemplateCommand();
            this.InstantVmToolStripMenuItem.Name = "InstantVmToolStripMenuItem";
            // 
            // toolStripSeparator29
            // 
            resources.ApplyResources(this.toolStripSeparator29, "toolStripSeparator29");
            this.toolStripSeparator29.Name = "toolStripSeparator29";
            // 
            // exportTemplateToolStripMenuItem
            // 
            resources.ApplyResources(this.exportTemplateToolStripMenuItem, "exportTemplateToolStripMenuItem");
            this.exportTemplateToolStripMenuItem.Command = new XenAdmin.Commands.ExportTemplateCommand();
            this.exportTemplateToolStripMenuItem.Name = "exportTemplateToolStripMenuItem";
            // 
            // duplicateTemplateToolStripMenuItem
            // 
            resources.ApplyResources(this.duplicateTemplateToolStripMenuItem, "duplicateTemplateToolStripMenuItem");
            this.duplicateTemplateToolStripMenuItem.Command = new XenAdmin.Commands.CopyTemplateCommand();
            this.duplicateTemplateToolStripMenuItem.Name = "duplicateTemplateToolStripMenuItem";
            // 
            // toolStripSeparator16
            // 
            resources.ApplyResources(this.toolStripSeparator16, "toolStripSeparator16");
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            // 
            // uninstallTemplateToolStripMenuItem
            // 
            resources.ApplyResources(this.uninstallTemplateToolStripMenuItem, "uninstallTemplateToolStripMenuItem");
            this.uninstallTemplateToolStripMenuItem.Command = new XenAdmin.Commands.DeleteTemplateCommand();
            this.uninstallTemplateToolStripMenuItem.Name = "uninstallTemplateToolStripMenuItem";
            // 
            // toolStripSeparator28
            // 
            resources.ApplyResources(this.toolStripSeparator28, "toolStripSeparator28");
            this.toolStripSeparator28.Name = "toolStripSeparator28";
            // 
            // pluginItemsPlaceHolderToolStripMenuItem6
            // 
            resources.ApplyResources(this.pluginItemsPlaceHolderToolStripMenuItem6, "pluginItemsPlaceHolderToolStripMenuItem6");
            this.pluginItemsPlaceHolderToolStripMenuItem6.Name = "pluginItemsPlaceHolderToolStripMenuItem6";
            // 
            // templatePropertiesToolStripMenuItem
            // 
            resources.ApplyResources(this.templatePropertiesToolStripMenuItem, "templatePropertiesToolStripMenuItem");
            this.templatePropertiesToolStripMenuItem.Command = new XenAdmin.Commands.TemplatePropertiesCommand();
            this.templatePropertiesToolStripMenuItem.Image = global::XenAdmin.Properties.Resources.edit_16;
            this.templatePropertiesToolStripMenuItem.Name = "templatePropertiesToolStripMenuItem";
            // 
            // toolsToolStripMenuItem
            // 
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bugToolToolStripMenuItem,
            this.healthCheckToolStripMenuItem1,
            this.toolStripSeparator14,
            this.LicenseManagerMenuItem,
            this.toolStripSeparator13,
            this.installNewUpdateToolStripMenuItem,
            this.rollingUpgradeToolStripMenuItem,
            this.toolStripSeparator6,
            this.pluginItemsPlaceHolderToolStripMenuItem7,
            this.preferencesToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            // 
            // bugToolToolStripMenuItem
            // 
            resources.ApplyResources(this.bugToolToolStripMenuItem, "bugToolToolStripMenuItem");
            this.bugToolToolStripMenuItem.Command = new XenAdmin.Commands.BugToolCommand();
            this.bugToolToolStripMenuItem.Name = "bugToolToolStripMenuItem";
            // 
            // healthCheckToolStripMenuItem1
            // 
            resources.ApplyResources(this.healthCheckToolStripMenuItem1, "healthCheckToolStripMenuItem1");
            this.healthCheckToolStripMenuItem1.Command = new XenAdmin.Commands.HealthCheckCommand();
            this.healthCheckToolStripMenuItem1.Name = "healthCheckToolStripMenuItem1";
            // 
            // toolStripSeparator14
            // 
            resources.ApplyResources(this.toolStripSeparator14, "toolStripSeparator14");
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            // 
            // LicenseManagerMenuItem
            // 
            resources.ApplyResources(this.LicenseManagerMenuItem, "LicenseManagerMenuItem");
            this.LicenseManagerMenuItem.Name = "LicenseManagerMenuItem";
            this.LicenseManagerMenuItem.Click += new System.EventHandler(this.LicenseManagerMenuItem_Click);
            // 
            // toolStripSeparator13
            // 
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            // 
            // installNewUpdateToolStripMenuItem
            // 
            resources.ApplyResources(this.installNewUpdateToolStripMenuItem, "installNewUpdateToolStripMenuItem");
            this.installNewUpdateToolStripMenuItem.Command = new XenAdmin.Commands.InstallNewUpdateCommand();
            this.installNewUpdateToolStripMenuItem.Name = "installNewUpdateToolStripMenuItem";
            // 
            // rollingUpgradeToolStripMenuItem
            // 
            resources.ApplyResources(this.rollingUpgradeToolStripMenuItem, "rollingUpgradeToolStripMenuItem");
            this.rollingUpgradeToolStripMenuItem.Command = new XenAdmin.Commands.RollingUpgradeCommand();
            this.rollingUpgradeToolStripMenuItem.Name = "rollingUpgradeToolStripMenuItem";
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // pluginItemsPlaceHolderToolStripMenuItem7
            // 
            resources.ApplyResources(this.pluginItemsPlaceHolderToolStripMenuItem7, "pluginItemsPlaceHolderToolStripMenuItem7");
            this.pluginItemsPlaceHolderToolStripMenuItem7.Name = "pluginItemsPlaceHolderToolStripMenuItem7";
            // 
            // preferencesToolStripMenuItem
            // 
            resources.ApplyResources(this.preferencesToolStripMenuItem, "preferencesToolStripMenuItem");
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            resources.ApplyResources(this.windowToolStripMenuItem, "windowToolStripMenuItem");
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pluginItemsPlaceHolderToolStripMenuItem9});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            // 
            // pluginItemsPlaceHolderToolStripMenuItem9
            // 
            resources.ApplyResources(this.pluginItemsPlaceHolderToolStripMenuItem9, "pluginItemsPlaceHolderToolStripMenuItem9");
            this.pluginItemsPlaceHolderToolStripMenuItem9.Name = "pluginItemsPlaceHolderToolStripMenuItem9";
            // 
            // helpToolStripMenuItem
            // 
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpTopicsToolStripMenuItem,
            this.helpContextMenuItem,
            this.toolStripMenuItem15,
            this.viewApplicationLogToolStripMenuItem,
            this.toolStripMenuItem17,
            this.xenSourceOnTheWebToolStripMenuItem,
            this.xenCenterPluginsOnlineToolStripMenuItem,
            this.toolStripSeparator7,
            this.pluginItemsPlaceHolderToolStripMenuItem8,
            this.aboutXenSourceAdminToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            // 
            // helpTopicsToolStripMenuItem
            // 
            resources.ApplyResources(this.helpTopicsToolStripMenuItem, "helpTopicsToolStripMenuItem");
            this.helpTopicsToolStripMenuItem.Name = "helpTopicsToolStripMenuItem";
            this.helpTopicsToolStripMenuItem.Click += new System.EventHandler(this.helpTopicsToolStripMenuItem_Click);
            // 
            // helpContextMenuItem
            // 
            resources.ApplyResources(this.helpContextMenuItem, "helpContextMenuItem");
            this.helpContextMenuItem.Image = global::XenAdmin.Properties.Resources._000_HelpIM_h32bit_16;
            this.helpContextMenuItem.Name = "helpContextMenuItem";
            this.helpContextMenuItem.Click += new System.EventHandler(this.helpContextMenuItem_Click);
            // 
            // toolStripMenuItem15
            // 
            resources.ApplyResources(this.toolStripMenuItem15, "toolStripMenuItem15");
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            // 
            // viewApplicationLogToolStripMenuItem
            // 
            resources.ApplyResources(this.viewApplicationLogToolStripMenuItem, "viewApplicationLogToolStripMenuItem");
            this.viewApplicationLogToolStripMenuItem.Name = "viewApplicationLogToolStripMenuItem";
            this.viewApplicationLogToolStripMenuItem.Click += new System.EventHandler(this.viewApplicationLogToolStripMenuItem_Click);
            // 
            // toolStripMenuItem17
            // 
            resources.ApplyResources(this.toolStripMenuItem17, "toolStripMenuItem17");
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            // 
            // xenSourceOnTheWebToolStripMenuItem
            // 
            resources.ApplyResources(this.xenSourceOnTheWebToolStripMenuItem, "xenSourceOnTheWebToolStripMenuItem");
            this.xenSourceOnTheWebToolStripMenuItem.Name = "xenSourceOnTheWebToolStripMenuItem";
            this.xenSourceOnTheWebToolStripMenuItem.Click += new System.EventHandler(this.xenSourceOnTheWebToolStripMenuItem_Click);
            // 
            // xenCenterPluginsOnlineToolStripMenuItem
            // 
            resources.ApplyResources(this.xenCenterPluginsOnlineToolStripMenuItem, "xenCenterPluginsOnlineToolStripMenuItem");
            this.xenCenterPluginsOnlineToolStripMenuItem.Name = "xenCenterPluginsOnlineToolStripMenuItem";
            this.xenCenterPluginsOnlineToolStripMenuItem.Click += new System.EventHandler(this.xenCenterPluginsOnTheWebToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            // 
            // pluginItemsPlaceHolderToolStripMenuItem8
            // 
            resources.ApplyResources(this.pluginItemsPlaceHolderToolStripMenuItem8, "pluginItemsPlaceHolderToolStripMenuItem8");
            this.pluginItemsPlaceHolderToolStripMenuItem8.Name = "pluginItemsPlaceHolderToolStripMenuItem8";
            // 
            // aboutXenSourceAdminToolStripMenuItem
            // 
            resources.ApplyResources(this.aboutXenSourceAdminToolStripMenuItem, "aboutXenSourceAdminToolStripMenuItem");
            this.aboutXenSourceAdminToolStripMenuItem.Name = "aboutXenSourceAdminToolStripMenuItem";
            this.aboutXenSourceAdminToolStripMenuItem.Click += new System.EventHandler(this.aboutXenSourceAdminToolStripMenuItem_Click);
            // 
            // MenuPanel
            // 
            resources.ApplyResources(this.MenuPanel, "MenuPanel");
            this.MenuPanel.Controls.Add(this.MainMenuBar);
            this.MenuPanel.Name = "MenuPanel";
            this.statusToolTip.SetToolTip(this.MenuPanel, resources.GetString("MenuPanel.ToolTip"));
            // 
            // StatusStrip
            // 
            resources.ApplyResources(this.StatusStrip, "StatusStrip");
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusProgressBar});
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.ShowItemToolTips = true;
            this.statusToolTip.SetToolTip(this.StatusStrip, resources.GetString("StatusStrip.ToolTip"));
            // 
            // statusLabel
            // 
            resources.ApplyResources(this.statusLabel, "statusLabel");
            this.statusLabel.AutoToolTip = true;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.statusLabel.Spring = true;
            // 
            // statusProgressBar
            // 
            resources.ApplyResources(this.statusProgressBar, "statusProgressBar");
            this.statusProgressBar.Margin = new System.Windows.Forms.Padding(5);
            this.statusProgressBar.Name = "statusProgressBar";
            this.statusProgressBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // toolStripMenuItem8
            // 
            resources.ApplyResources(this.toolStripMenuItem8, "toolStripMenuItem8");
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            // 
            // securityGroupsToolStripMenuItem
            // 
            resources.ApplyResources(this.securityGroupsToolStripMenuItem, "securityGroupsToolStripMenuItem");
            this.securityGroupsToolStripMenuItem.Name = "securityGroupsToolStripMenuItem";
            // 
            // commandToolStripMenuItem1
            // 
            resources.ApplyResources(this.commandToolStripMenuItem1, "commandToolStripMenuItem1");
            this.commandToolStripMenuItem1.Command = new XenAdmin.Commands.RemoveHostCrashDumpsCommand();
            this.commandToolStripMenuItem1.Name = "commandToolStripMenuItem1";
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.MenuPanel);
            this.Controls.Add(this.StatusStrip);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.MainMenuBar;
            this.Name = "MainWindow";
            this.statusToolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.ResizeEnd += new System.EventHandler(this.MainWindow_ResizeEnd);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.MainWindow_HelpRequested);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.TheTabControl.ResumeLayout(false);
            this.TabPageSnapshots.ResumeLayout(false);
            this.TitleBackPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TitleIcon)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolTipContainer1.ResumeLayout(false);
            this.toolTipContainer1.PerformLayout();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ToolBarContextMenu.ResumeLayout(false);
            this.MainMenuBar.ResumeLayout(false);
            this.MainMenuBar.PerformLayout();
            this.MenuPanel.ResumeLayout(false);
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private XenAdmin.Controls.ToolStripEx ToolStrip;
        private CommandToolStripButton NewVmToolbarButton;
        private CommandToolStripButton AddServerToolbarButton;
        private CommandToolStripButton RebootToolbarButton;
        private CommandToolStripButton SuspendToolbarButton;
        private CommandToolStripButton ForceRebootToolbarButton;
        private CommandToolStripButton ForceShutdownToolbarButton;
        private System.Windows.Forms.ToolTip statusToolTip;
        private CommandToolStripButton AddPoolToolbarButton;
        private CommandToolStripButton newStorageToolbarButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.PictureBox TitleIcon;
        private XenAdmin.Controls.GradientPanel.GradientPanel TitleBackPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        internal System.Windows.Forms.ToolStripSplitButton backButton;
        internal System.Windows.Forms.ToolStripSplitButton forwardButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ContextMenuStrip ToolBarContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ShowToolbarMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private CommandToolStripMenuItem FileImportVMToolStripMenuItem;
        private CommandToolStripMenuItem importSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator21;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem poolToolStripMenuItem;
        private CommandToolStripMenuItem AddPoolToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private CommandToolStripMenuItem PoolPropertiesToolStripMenuItem;
        private CommandToolStripMenuItem highAvailabilityToolStripMenuItem;
        private CommandToolStripMenuItem wlbReportsToolStripMenuItem;
        private CommandToolStripMenuItem wlbDisconnectToolStripMenuItem;
        private AddHostToSelectedPoolToolStripMenuItem addServerToolStripMenuItem;
        private CommandToolStripMenuItem removeServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private CommandToolStripMenuItem deleteToolStripMenuItem;
        private CommandToolStripMenuItem disconnectPoolToolStripMenuItem;
        private CommandToolStripMenuItem exportResourceReportPoolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HostMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
        private CommandToolStripMenuItem ServerPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private CommandToolStripMenuItem RebootHostToolStripMenuItem;
        private CommandToolStripMenuItem ShutdownHostToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private CommandToolStripMenuItem AddHostToolStripMenuItem;
        private CommandToolStripMenuItem removeHostToolStripMenuItem;
        private AddSelectedHostToPoolToolStripMenuItem addServerToPoolMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private CommandToolStripMenuItem installLicenseToolStripMenuItem;
        private CommandToolStripMenuItem maintenanceModeToolStripMenuItem1;
        private CommandToolStripMenuItem backupToolStripMenuItem;
        private CommandToolStripMenuItem restoreFromBackupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem VMToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private CommandToolStripMenuItem VMPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
        private CommandToolStripMenuItem snapshotToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private CommandToolStripMenuItem copyVMtoSharedStorageMenuItem;
        private CommandToolStripMenuItem exportToolStripMenuItem;
        private CommandToolStripMenuItem convertToTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
        private CommandToolStripMenuItem installToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private CommandToolStripMenuItem uninstallToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem StorageToolStripMenuItem;
        private CommandToolStripMenuItem AddStorageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private CommandToolStripMenuItem SRPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator22;
        private CommandToolStripMenuItem RepairStorageToolStripMenuItem;
        private CommandToolStripMenuItem DefaultSRToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private CommandToolStripMenuItem DetachStorageToolStripMenuItem;
        private CommandToolStripMenuItem ReattachStorageRepositoryToolStripMenuItem;
        private CommandToolStripMenuItem ForgetStorageRepositoryToolStripMenuItem;
        private CommandToolStripMenuItem DestroyStorageRepositoryToolStripMenuItem;
        private CommandToolStripMenuItem ConvertToThinStorageRepositoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem templatesToolStripMenuItem;
        private CommandToolStripMenuItem exportTemplateToolStripMenuItem;
        private CommandToolStripMenuItem duplicateTemplateToolStripMenuItem;
        private CommandToolStripMenuItem uninstallTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private CommandToolStripMenuItem bugToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem LicenseManagerMenuItem;
        private CommandToolStripMenuItem installNewUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpTopicsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpContextMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem viewApplicationLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem17;
        private System.Windows.Forms.ToolStripMenuItem xenSourceOnTheWebToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem aboutXenSourceAdminToolStripMenuItem;
        private XenAdmin.Controls.MenuStripEx MainMenuBar;
        private System.Windows.Forms.Panel MenuPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator24;
        private System.Windows.Forms.ToolStripMenuItem toolbarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowHiddenObjectsToolStripMenuItem;
        internal System.Windows.Forms.TabControl TheTabControl;
        private System.Windows.Forms.TabPage TabPageHome;
        internal System.Windows.Forms.TabPage TabPageSearch;
        internal System.Windows.Forms.TabPage TabPageGeneral;
        private System.Windows.Forms.TabPage TabPageBallooning;
        internal System.Windows.Forms.TabPage TabPageConsole;
        private System.Windows.Forms.TabPage TabPageStorage;
        private System.Windows.Forms.TabPage TabPagePhysicalStorage;
        private System.Windows.Forms.TabPage TabPageSR;
        private System.Windows.Forms.TabPage TabPageNetwork;
        private System.Windows.Forms.TabPage TabPageNICs;
        private System.Windows.Forms.TabPage TabPagePeformance;
        private System.Windows.Forms.TabPage TabPageHA;
        private System.Windows.Forms.TabPage TabPageHAUpsell;
        internal System.Windows.Forms.TabPage TabPageWLB;
        private System.Windows.Forms.TabPage TabPageWLBUpsell;
        private System.Windows.Forms.TabPage TabPageSnapshots;
        private System.Windows.Forms.TabPage TabPageDockerProcess;
        internal System.Windows.Forms.TabPage TabPageDockerDetails;
        private XenAdmin.TabPages.SnapshotsPage snapshotPage;
        private System.Windows.Forms.ToolStripMenuItem connectDisconnectToolStripMenuItem;
        private CommandToolStripMenuItem connectAllToolStripMenuItem;
        private CommandToolStripMenuItem DisconnectToolStripMenuItem;
        private CommandToolStripMenuItem disconnectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendCtrlAltDelToolStripMenuItem;
        private CommandToolStripMenuItem virtualDisksToolStripMenuItem;
        private CommandToolStripMenuItem addVirtualDiskToolStripMenuItem;
        private CommandToolStripMenuItem attachVirtualDiskToolStripMenuItem;
        private CommandToolStripMenuItem NewVmToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator26;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator27;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator23;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator25;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator28;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private CommandToolStripMenuItem templatePropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator29;
        private VMLifeCycleToolStripMenuItem startShutdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private CommandToolStripMenuItem ReconnectToolStripMenuItem1;
        private XenAdmin.Controls.ToolTipContainer toolTipContainer1;
        private XenAdmin.Controls.LoggedInLabel loggedInLabel1;
        private CommandToolStripMenuItem reconnectAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private CommandToolStripMenuItem poolReconnectAsToolStripMenuItem;
        internal System.Windows.Forms.TabPage TabPageAD;
        private System.Windows.Forms.TabPage TabPageGPU;
        private CommandToolStripMenuItem powerOnToolStripMenuItem;
        private CommandToolStripButton shutDownToolStripButton;
        private CommandToolStripButton startVMToolStripButton;
        private CommandToolStripButton powerOnHostToolStripButton;
        private CommandToolStripButton resumeToolStripButton;
        private MigrateVMToolStripMenuItem relocateToolStripMenuItem;
        private StartVMOnHostToolStripMenuItem startOnHostToolStripMenuItem;
        private ResumeVMOnHostToolStripMenuItem resumeOnToolStripMenuItem;
        private ToolStripMenuItem pluginItemsPlaceHolderToolStripMenuItem1;
        private ToolStripMenuItem pluginItemsPlaceHolderToolStripMenuItem;
        private ToolStripMenuItem pluginItemsPlaceHolderToolStripMenuItem2;
        private ToolStripMenuItem pluginItemsPlaceHolderToolStripMenuItem3;
        private ToolStripMenuItem pluginItemsPlaceHolderToolStripMenuItem4;
        private ToolStripMenuItem pluginItemsPlaceHolderToolStripMenuItem5;
        private ToolStripMenuItem pluginItemsPlaceHolderToolStripMenuItem6;
        private ToolStripMenuItem pluginItemsPlaceHolderToolStripMenuItem7;
        private ToolStripMenuItem pluginItemsPlaceHolderToolStripMenuItem9;
        private ToolStripMenuItem pluginItemsPlaceHolderToolStripMenuItem8;
        private TabPage TabPageBallooningUpsell;
        private ToolStripMenuItem xenCenterPluginsOnlineToolStripMenuItem;
        private CommandToolStripMenuItem MoveVMToolStripMenuItem;
        private CommandToolStripMenuItem rollingUpgradeToolStripMenuItem;
        private CommandToolStripMenuItem vMProtectionAndRecoveryToolStripMenuItem;
        private AssignGroupToolStripMenuItemVMPP assignPolicyToolStripMenuItem;
        private CommandToolStripMenuItem changePoolPasswordToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator30;
        private CommandToolStripMenuItem virtualAppliancesToolStripMenuItem;
        private AssignGroupToolStripMenuItemVM_appliance assignToVirtualApplianceToolStripMenuItem;
        private CommandToolStripMenuItem disasterRecoveryToolStripMenuItem;
        private CommandToolStripMenuItem DrWizardToolStripMenuItem;
        private CommandToolStripMenuItem drConfigureToolStripMenuItem;
        private CommandToolStripMenuItem securityGroupsToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private CommandToolStripMenuItem HostPasswordToolStripMenuItem;
        private CommandToolStripMenuItem ChangeRootPasswordToolStripMenuItem;
        private CommandToolStripMenuItem forgetSavedPasswordToolStripMenuItem;
        private CommandToolStripMenuItem CreateVmFromTemplateToolStripMenuItem;
        private CommandToolStripMenuItem newVMFromTemplateToolStripMenuItem;
        private CommandToolStripMenuItem InstantVmToolStripMenuItem;
        private ToolStripMenuItem importSettingsToolStripMenuItem;
        private ToolStripMenuItem exportSettingsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator31;
        private CommandToolStripMenuItem destroyServerToolStripMenuItem;
        private CommandToolStripMenuItem restartToolstackToolStripMenuItem;
        private XenAdmin.Controls.MainWindowControls.NavigationPane navigationPane;
        private XenAdmin.TabPages.AlertSummaryPage alertPage;
        private XenAdmin.TabPages.ManageUpdatesPage updatesPage;
        private XenAdmin.TabPages.HistoryPage eventsPage;
        private ToolStripMenuItem customTemplatesToolStripMenuItem;
        private ToolStripMenuItem templatesToolStripMenuItem1;
        private ToolStripMenuItem localStorageToolStripMenuItem;
        private StatusStrip StatusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolStripProgressBar statusProgressBar;
        private CommandToolStripMenuItem reclaimFreedSpacetripMenuItem;
        private CommandToolStripButton startContainerToolStripButton;
        private CommandToolStripButton stopContainerToolStripButton;
        private CommandToolStripButton pauseContainerToolStripButton;
        private CommandToolStripButton resumeContainerToolStripButton;
        private CommandToolStripButton restartContainerToolStripButton;
        private CommandToolStripMenuItem healthCheckToolStripMenuItem1;
        private AssignGroupToolStripMenuItemVMSS assignSnapshotScheduleToolStripMenuItem;
        private CommandToolStripMenuItem VMSnapshotScheduleToolStripMenuItem;
        private TabPage TabPageADUpsell;
        private TabPage TabPageCvmConsole;
        private TabPage TabPagePvs;
        private CommandToolStripMenuItem controlDomainMemoryToolStripMenuItem;
        private CommandToolStripMenuItem enablePVSReadcachingToolStripMenuItem;
        private CommandToolStripMenuItem disablePVSReadcachingToolStripMenuItem;
        private TabPage TabPageBackup;
        private TabPage TabPageBRUpsell;
        private TabPage TabPageUsbDevice;
        private TabPage TabPagevSwitchController;
        private TabPage TabPageSCUpsell;
        private CommandToolStripMenuItem commandToolStripMenuItem1;
    }

}

