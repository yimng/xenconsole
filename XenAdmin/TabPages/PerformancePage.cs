/* Copyright (c) Citrix Systems, Inc. 
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
using System.Windows.Forms;
using XenAPI;
using XenAdmin.Core;
using XenAdmin.Controls.CustomDataGraph;
using XenAdmin.Dialogs;
using XenAdmin.Actions;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data;
using System.Text;
namespace XenAdmin.TabPages
{
    public partial class PerformancePage : BaseTabPage
    {
        private IXenObject _xenObject;
        private bool _disposed;
        private readonly CollectionChangeEventHandler Message_CollectionChangedWithInvoke;
        private readonly ArchiveMaintainer ArchiveMaintainer = new ArchiveMaintainer();

        public PerformancePage()
        {
            InitializeComponent();

            buttonPrint.Text = Messages.BUTTON_PRINT_TEXT;
            ArchiveMaintainer.ArchivesUpdated += ArchiveMaintainer_ArchivesUpdated;
            Message_CollectionChangedWithInvoke = Program.ProgramInvokeHandler(Message_CollectionChanged);
            GraphList.ArchiveMaintainer = ArchiveMaintainer;
            GraphList.SelectedGraphChanged += GraphList_SelectedGraphChanged;
            GraphList.MouseDown += GraphList_MouseDown;
            DataPlotNav.ArchiveMaintainer = ArchiveMaintainer;
            this.DataEventList.SetPlotNav(this.DataPlotNav);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //set text colour for gradient bar
            EventsLabel.ForeColor = Program.TitleBarForeColor;
            base.Text = Messages.PERFORMANCE_TAB_TITLE;
            UpdateMoveButtons();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            
            ArchiveMaintainer.Stop();

            if (disposing)
            {
                ArchiveMaintainer.ArchivesUpdated -= ArchiveMaintainer_ArchivesUpdated;

                if (components != null)
                    components.Dispose();

                _disposed = true;
            }
            base.Dispose(disposing);
        }

        private void GraphList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(GraphList, e.X, e.Y);
            }
        }

        private bool CanEnableGraphButtons()
        {
            return XenObject != null && GraphList.SelectedGraph != null;
        }

        private void UpdateMoveButtons()
        {
            bool canEnable = CanEnableGraphButtons();
            moveUpButton.Enabled = canEnable && GraphList.SelectedGraphIndex != 0;
            moveUpToolStripMenuItem.Enabled = moveUpButton.Enabled;

            moveDownButton.Enabled = canEnable && GraphList.SelectedGraphIndex != GraphList.Count - 1;
            moveDownToolStripMenuItem.Enabled = moveDownButton.Enabled;
        }

        private void UpdateActionButtons()
        {
            bool canEnable = CanEnableGraphButtons();

            VM vm = XenObject as VM;
            bool isRunning = XenObject != null && (vm != null)
                                 ? vm.current_operations.Count == 0 && vm.IsRunning
                                 : (XenObject is Host);
            
            string newText = (vm != null && !isRunning)
                                 ? string.Format(Messages.GRAPHS_CANNOT_ADD_VM_HALTED, vm.Name)
                                 : string.Empty;

            newGraphToolStripMenuItem.Enabled = isRunning;
            newGraphToolStripMenuItem.ToolTipText = newText;

            newGraphToolStripContextMenuItem.Enabled = isRunning;
            newGraphToolStripContextMenuItem.ToolTipText = newText;

            string editText=(vm != null && !isRunning)
                                 ? string.Format(Messages.GRAPHS_CANNOT_EDIT_VM_HALTED, vm.Name)
                                 : string.Empty;

            editGraphToolStripMenuItem.Enabled = canEnable && isRunning;
            editGraphToolStripMenuItem.ToolTipText = editText;

            editGraphToolStripContextMenuItem.Enabled = canEnable && isRunning;
            editGraphToolStripContextMenuItem.ToolTipText = editText;

            deleteGraphToolStripMenuItem.Enabled = canEnable && GraphList.Count > 1;
            deleteGraphToolStripContextMenuItem.Enabled = canEnable && GraphList.Count > 1;

            restoreDefaultGraphsToolStripMenuItem.Enabled = XenObject != null && !GraphList.ShowingDefaultGraphs;
            restoreDefaultGraphsToolStripContextMenuItem.Enabled = XenObject != null && !GraphList.ShowingDefaultGraphs;
        }

        private void UpdateAllButtons()
        {
            UpdateMoveButtons();
            UpdateActionButtons();
        }

        private void GraphList_SelectedGraphChanged()
        {
            UpdateAllButtons();
        }

        public IXenObject XenObject
        {
            get
            {
                return _xenObject;
            }
            set
            {
                ArchiveMaintainer.Pause();
                ArchiveMaintainer.XenObject = null;
                DataEventList.Clear();

                DeregEvents();
                _xenObject = value;
                RegEvents();

                if (_xenObject != null)
                {
                    GraphList.LoadGraphs(XenObject);
                    LoadEvents();
                    ArchiveMaintainer.XenObject = value;
                    ArchiveMaintainer.Start(); 
                }
            }
        }

        private bool FeatureForbidden
        {
            get { return Helpers.FeatureForbidden(XenObject, Host.RestrictPerformanceGraphs); }
        }

        private void LoadEvents()
        {
            foreach(XenAPI.Message m in XenObject.Connection.Cache.Messages)
            {
                CheckMessage(m, CollectionChangeAction.Add);    
            }
        }

        private void CheckMessage(XenAPI.Message m, CollectionChangeAction a)
        {
            if (!m.ShowOnGraphs || m.cls != cls.VM)
                return;

            Host h = XenObject as Host;
            if (h != null)
            {
                List<VM> resVMs = h.Connection.ResolveAll<VM>(h.resident_VMs);
                foreach (VM v in resVMs)
                {
                    if (v.uuid == m.obj_uuid)
                    {
                        if (a == CollectionChangeAction.Add)
                            DataEventList.AddEvent(new DataEvent(m.timestamp.ToLocalTime().Ticks, 0, m));
                        else
                            DataEventList.RemoveEvent(new DataEvent(m.timestamp.ToLocalTime().Ticks, 0, m));

                        break;
                    }
                }

            }
            else if (XenObject is VM)
            {
                if (m.obj_uuid != Helpers.GetUuid(XenObject))
                    return;

                if (a == CollectionChangeAction.Add)
                    DataEventList.AddEvent(new DataEvent(m.timestamp.ToLocalTime().Ticks, 0, m));
                else
                    DataEventList.RemoveEvent(new DataEvent(m.timestamp.ToLocalTime().Ticks, 0, m));
            }
        }

        private void RegEvents()
        {
            if (XenObject == null)
                return;

            Pool pool = Helpers.GetPoolOfOne(XenObject.Connection);

            if(pool != null)
                pool.PropertyChanged += pool_PropertyChanged;

            XenObject.Connection.Cache.RegisterCollectionChanged<XenAPI.Message>(Message_CollectionChangedWithInvoke);
        }

        private void Message_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            Program.AssertOnEventThread();
            XenAPI.Message m = ((XenAPI.Message)e.Element);
            CheckMessage(m, e.Action);
        }

        private void pool_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "gui_config")
                Program.Invoke(this, () => GraphList.LoadGraphs(XenObject));
        }

        private void DeregEvents()
        {
            if (XenObject == null)
                return;

            Pool pool = Helpers.GetPoolOfOne(XenObject.Connection);

            if (pool != null)
                pool.PropertyChanged -= pool_PropertyChanged;

            XenObject.Connection.Cache.DeregisterCollectionChanged<XenAPI.Message>(Message_CollectionChangedWithInvoke);
        }

        public override void PageHidden()
        {
            DeregEvents();
            if (ArchiveMaintainer != null && XenObject != null)
            {
                ArchiveMaintainer.Pause();
                ArchiveMaintainer.DeregEvents();
            }
        }
        
        private void ArchiveMaintainer_ArchivesUpdated(object sender, EventArgs args)
        {
            Program.Invoke(this, RefreshAll);
        }

        private void RefreshAll()
        {
            DataPlotNav.RefreshXRange(false);
        }

        private void ShowUpsell()
        {
            using (var upsellDialog = new UpsellDialog(HiddenFeatures.LinkLabelHidden ? Messages.UPSELL_BLURB_PERFORMANCE : Messages.UPSELL_BLURB_PERFORMANCE + Messages.UPSELL_BLURB_PERFORMANCE_MORE,
                                                        InvisibleMessages.UPSELL_LEARNMOREURL_PERFORMANCE))
                upsellDialog.ShowDialog(this);
        }

        private void MoveGraphUp()
        {
            if (XenObject == null)
                return;

            int index = GraphList.SelectedGraphIndex;
            if (GraphList.AuthorizedRole)
            {
                GraphList.ExchangeGraphs(index, index - 1);
                GraphList.SaveGraphs(null);
            }
        }

        private void MoveGraphDown()
        {
            if (XenObject == null)
                return;

            int index = GraphList.SelectedGraphIndex;
            if (GraphList.AuthorizedRole)
            {
                GraphList.ExchangeGraphs(index, index + 1);
                GraphList.SaveGraphs(null);
            }
        }

        private void NewGraph()
        {
            if (XenObject == null)
                return;
            
            if (GraphList.AuthorizedRole)
            {
                GraphDetailsDialog dialog = new GraphDetailsDialog(GraphList, null);
                dialog.ShowDialog();
            }
        }

        private void EditGraph()
        {
            if (XenObject == null)
                return;

            if (GraphList.AuthorizedRole)
            {
                GraphDetailsDialog dialog = new GraphDetailsDialog(GraphList, GraphList.SelectedGraph);
                dialog.ShowDialog();
            }
        }

        private void DeleteGraph()
        {
            using (ThreeButtonDialog dlog = new ThreeButtonDialog(
                new ThreeButtonDialog.Details(SystemIcons.Warning,
                    string.Format(Messages.DELETE_GRAPH_MESSAGE, GraphList.SelectedGraph.DisplayName.EscapeAmpersands()),
                    Messages.XENCENTER),
                ThreeButtonDialog.ButtonYes,
                ThreeButtonDialog.ButtonNo))
            {
                if (dlog.ShowDialog(this) == DialogResult.Yes)
                    if (GraphList.AuthorizedRole)
                    {
                        GraphList.DeleteGraph(GraphList.SelectedGraph);
                        GraphList.LoadDataSources(SaveGraphs);
                    }
            }
        }

        private void RestoreDefaultGraphs()
        {
            using (ThreeButtonDialog dlog = new ThreeButtonDialog(
                    new ThreeButtonDialog.Details(SystemIcons.Warning,
                        Messages.GRAPHS_RESTORE_DEFAULT_MESSAGE,
                        Messages.XENCENTER),
                    ThreeButtonDialog.ButtonYes,
                    ThreeButtonDialog.ButtonNo))
            {
                if (dlog.ShowDialog(this) == DialogResult.Yes)
                    if (GraphList.AuthorizedRole)
                    {
                        GraphList.RestoreDefaultGraphs();
                        GraphList.LoadDataSources(SaveGraphs);
                    }
            }
        }

        private void SaveGraphs(ActionBase sender)
        {
            Program.Invoke(Program.MainWindow, delegate
            {
                var action = sender as GetDataSourcesAction;
                if (action != null)
                {
                    var dataSources = DataSourceItemList.BuildList(action.IXenObject, action.DataSources);
                    GraphList.SaveGraphs(dataSources);
                }
            });
        }

        private void LastYearZoom()
        {
            if (FeatureForbidden)
                ShowUpsell();
            else
                DataPlotNav.ZoomToRange(TimeSpan.Zero, TimeSpan.FromDays(366) - TimeSpan.FromSeconds(1));
        }

        private void LastMonthZoom()
        {
            if (FeatureForbidden)
                ShowUpsell();
            else
                DataPlotNav.ZoomToRange(TimeSpan.Zero, TimeSpan.FromDays(30) - TimeSpan.FromSeconds(1));
        }

        private void LastWeekZoom()
        {
            if (FeatureForbidden)
                ShowUpsell();
            else
                DataPlotNav.ZoomToRange(TimeSpan.Zero, TimeSpan.FromDays(7) - TimeSpan.FromSeconds(1));
        }

        private void LastDayZoom()
        {
            DataPlotNav.ZoomToRange(TimeSpan.Zero, TimeSpan.FromDays(1) - TimeSpan.FromSeconds(1));
        }

        private void LastHourZoom()
        {
            DataPlotNav.ZoomToRange(TimeSpan.Zero, TimeSpan.FromHours(1) - TimeSpan.FromSeconds(1));
        }

        private void LastTenMinutesZoom()
        {
            DataPlotNav.ZoomToRange(TimeSpan.Zero, TimeSpan.FromMinutes(10) - TimeSpan.FromSeconds(1));
        }

        #region Control event handlers

        private void addGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGraph();
        }

        private void editGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditGraph();
        }

        private void deleteGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteGraph();
        }

        private void restoreDefaultGraphsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreDefaultGraphs();
        }

        
        private void moveUpButton_Click(object sender, EventArgs e)
        {
            MoveGraphUp();
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            MoveGraphDown();
        }


        private void graphActionsButton_Click(object sender, EventArgs e)
        {
            graphActionsMenuStrip.Show(graphActionsButton, 0, graphActionsButton.Height);
        }

        private void graphActionsMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            UpdateActionButtons();
        }

        private void zoomButton_Click(object sender, EventArgs e)
        {
            zoomMenuStrip.Show(zoomButton, 0, zoomButton.Height);
        }

        
        private void lastYearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LastYearZoom();
        }

        private void lastMonthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LastMonthZoom();
        }

        private void lastWeekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LastWeekZoom();
        }

        private void lastDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LastDayZoom();
        }

        private void lastHourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LastHourZoom();
        }

        private void lastTenMinutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LastTenMinutesZoom();
        }

        
        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            UpdateActionButtons();
        }
        
        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveGraphUp();
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveGraphDown();
        }

        
        private void newGraphToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            NewGraph();
        }

        private void editGraphToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            EditGraph();
        }

        private void deleteGraphToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            DeleteGraph();
        }

        private void restoreDefaultGraphsToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            RestoreDefaultGraphs();
        }

        
        private void lastYearToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            LastYearZoom();
        }

        private void lastMonthToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            LastMonthZoom();
        }

        private void lastWeekToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            LastWeekZoom();
        }

        private void lastDayToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            LastDayZoom();
        }

        private void lastHourToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            LastHourZoom();
        }

        private void lastTenMinutesToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            LastTenMinutesZoom();
        }

        
        private void GraphList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            UpdateActionButtons();
            if (editGraphToolStripMenuItem.Enabled)
                EditGraph();
        }

        #endregion
        void AddDataSource(string uuid, List<string> dsuuids, DesignedGraph dg)
        {
            dsuuids.Add(uuid);
            dg.DataSources.Add(new DataSourceItem(new Data_source(), "", Palette.GetColour(uuid), uuid));
        }
        private void buttonPrint_Click(object sender, EventArgs e)
        {
            object misValue = System.Reflection.Missing.Value;
            FormWindowState fws = new FormWindowState();
            fws = Program.MainWindow.WindowState;
            SaveFileDialog sfd = new SaveFileDialog();
            string localFilePath = null;
            sfd.Filter = "PNG|*.png|JPEG(*.JPG *.JPEG *.JPE)|*.jpg|BMP|*.bmp";
            if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName != null && sfd.FileName != "")
            {
                localFilePath = sfd.FileName.ToString();
            }
            else
            {
                return;
            }
            FileStream performanceStream = new FileStream(localFilePath, FileMode.Create);
            try
            {
                //生成性能图表截图
                Program.MainWindow.WindowState = FormWindowState.Maximized;
                Program.MainWindow.Refresh();
                Bitmap bit = new Bitmap(GraphList.Width-30, GraphList.Height-55);
                Graphics g = Graphics.FromImage(bit);
                g.CopyFromScreen(GraphList.PointToScreen(System.Drawing.Point.Empty), System.Drawing.Point.Empty, GraphList.Size);
                bit.Save(performanceStream, System.Drawing.Imaging.ImageFormat.Png);
                Program.MainWindow.WindowState = fws;
                //取数据               
                List<string> dsuuids = new List<string>();
                Pool pool = Helpers.GetPoolOfOne(XenObject.Connection);
                List<DesignedGraph> dglist = new List<DesignedGraph>();
                if (pool != null)
                {
                    if (XenObject == null)
                        return;

//                    int index = GraphList.Count - 1;
//                    if (GraphList.AuthorizedRole)
//                    {
//                        GraphList.ExchangeGraphs(1, 0);
 //                       GraphList.ExchangeGraphs(0, 1);
                        //GraphList.ExchangeGraphs(index-1, index);
                        //GraphList.SaveGraphs(null);
                   // }
                    Dictionary<string, string> gui_config = Helpers.GetGuiConfig(pool);
                    int i = 0;
                    while (true)
                    {
                        DesignedGraph dg = new DesignedGraph();
                        string key = Palette.GetLayoutKey(i, XenObject);
                        if (!gui_config.ContainsKey(key))
                            break;

                        string[] dslist = gui_config[key].Split(',');
                        foreach (string ds in dslist)
                        {
                            AddDataSource(string.Format("{0}:{1}:{2}", XenObject is Host ? "host" : "vm", Helpers.GetUuid(XenObject), ds), dsuuids, dg);
                        }

                        key = Palette.GetGraphNameKey(i, XenObject);
                        if (gui_config.ContainsKey(key))
                        {
                            dg.DisplayName = gui_config[key];
                        }

                        dglist.Add(dg);
                        i++;
                    }
                }
                //导出到Excel
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook xlWorkBook = xlApp.Workbooks.Add();
                Worksheet xlWorkSheet;
                Dictionary<string, string> config = Helpers.GetGuiConfig(pool);
                for (int j = 0; j < dglist.Count; j++)
                {
                    xlWorkBook.Sheets.Add(misValue, misValue, 1, XlSheetType.xlWorksheet);
                    xlWorkSheet = xlWorkBook.Worksheets.get_Item(1);
                    //每个图表创建一个对应的表单（CPU性能,内存性能...）
                    xlWorkSheet.Name = dglist[j].DisplayName;
                    //取每个表单的标签名称（cpu0，cpu1...）
                    string key = Palette.GetLayoutKey(j, XenObject);
                    String[] dslist = config[key].Split(',');

                    for (int i = 0; i < dslist.Length; i++)
                    {
                   //    xlWorkSheet.Cells[i + 2, 1] = dslist[i];
                        xlWorkSheet.Cells[i + 2, 1] = DataKey.items[i];
                        Microsoft.Office.Interop.Excel.Range allColumn = xlWorkSheet.Columns;
                        allColumn.AutoFit();
                    }
                }
                try
                {
                    xlWorkBook.SaveAs(localFilePath.Substring(0,localFilePath.LastIndexOf("."))+".xls", XlFileFormat.xlWorkbookNormal, "", "", false, false, XlSaveAsAccessMode.xlExclusive, true, true);
                    xlWorkBook.Close(true, localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1) + ".xls", false);
                    xlApp.Quit();
                }
                catch{ }                
                ExportSuccessfullyDialog esd = new ExportSuccessfullyDialog();
                esd.ShowDialog();
            }
            finally
            {
                performanceStream.Close();
            }
        }
    }
}
