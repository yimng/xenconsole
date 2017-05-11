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
using System.Windows.Forms;
using XenAPI;
using XenAdmin.Controls;
using XenAdmin.Model;
using XenAdmin.Core;
using XenAdmin.Commands;
using XenAdmin.Dialogs;
using XenAdmin.Properties;
using HalsignLib;
using XenAdmin.Actions;
using XenAdmin.Wizards.NewSRWizard_Pages;
using XenAdmin.Wizards.NewSRWizard_Pages.Frontends;
using System.Linq;

namespace XenAdmin.SettingsPanels
{
    public class BrowseStorageGeneralPage : UserControl, IEditPage, VerticalTabs.VerticalTab
    {
        private bool _ValidToSave = true;
        private IContainer components = null;

        private IXenObject xenModelObject;
        private List<GeneralDataStructure> GeneralDataList;
        private Dictionary<string, ListViewGroup> GroupDic;
        private ToolTip toolTip;

        private static string StorageLinkSystemCapabilities = Messages.GENERAL_TAB_CAPABILITIES;
        private static string MultipathBoot = Messages.GENERAL_TAB_MULTIPATH_BOOT;
        private static string StorageLink = Messages.GENERAL_TAB_STORAGELINK;
        private static string Multipathing = Messages.GENERAL_TAB_MULTIPATHING;
        private static string Status = Messages.GENERAL_TAB_STATUS;
        private GeneralListViewEx listViewExGeneral;
        private ColumnHeader ItemName;
        private ColumnHeader ItemValue;
        private static string General = Messages.GENERAL_TAB_GENERAL;

        public BrowseStorageGeneralPage()
        {
            this.InitializeComponent();
            this.Text = Messages.NAME_DESCRIPTION_TAGS;
            GeneralDataList = new List<GeneralDataStructure>();
            GroupDic = new Dictionary<string, ListViewGroup>();
            toolTip = new ToolTip();
        }

        public IXenObject XenModelObject
        {
            get
            {
                return this.xenModelObject;
            }
            set
            {
                if (this.xenModelObject != value)
                {
                    this.xenModelObject = value;
                    this.BuildList();
                }
            }
        }

        public void BuildList()
        {
            this.listViewExGeneral.Items.Clear();
            this.GeneralDataList.Clear();
            this.GroupDic.Clear();

            if (this.xenModelObject.Connection != null && this.xenModelObject.Connection.IsConnected && this.xenModelObject is XenObject<XenAPI.SR>)
            {
                this.generateGeneralBox();
                this.generateStatusBox();
                this.generateMultipathBox();
            }

            foreach (GeneralDataStructure ds in GeneralDataList)
            {
                if (!GroupDic.ContainsKey(ds._Group))
                {
                    ListViewGroup group = new ListViewGroup(ds._Group);
                    GroupDic.Add(ds._Group, group);
                    this.listViewExGeneral.Groups.Add(group);
                }
                ListViewItemEx item = new ListViewItemEx(new string[] { ds._Key, ds._Value }, 0, GroupDic[ds._Group], ds._Color);
                item.Tag = ds._ContextMenuCollection;
                this.listViewExGeneral.AddItem(item);
            }
        }

        private static string FriendlyName(string propertyName)
        {
            return (XenAdmin.Core.PropertyManager.GetFriendlyName(string.Format("Label-{0}", propertyName)) ?? propertyName);
        }

        private static string GetUUID(IXenObject o)
        {
            return (o.Get("uuid") as string);
        }

        private string TagsString()
        {
            string[] tags = Tags.GetTags(this.xenModelObject);
            if ((tags == null) || (tags.Length == 0))
            {
                return Messages.NONE;
            }
            List<string> list = new List<string>(tags);
            list.Sort();
            return string.Join(", ", list.ToArray());
        }

        private bool issciFaulty(string scsiid)
        {
            XenAPI.SR sr = this.xenModelObject as XenAPI.SR;

            foreach (PBD pdb in sr.Connection.ResolveAll<PBD>(sr.PBDs))
            {
                if (pdb.other_config.ContainsKey("LUN2-status") && pdb.other_config["LUN2-status"].Contains("faulty") && pdb.other_config.ContainsKey("LUN2-scsiid") && pdb.other_config["LUN2-scsiid"] == scsiid ||
                    pdb.other_config.ContainsKey("LUN1-status") && pdb.other_config["LUN1-status"].Contains("faulty") && pdb.other_config.ContainsKey("LUN1-scsiid") && pdb.other_config["LUN1-scsiid"] == scsiid)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CanAddLUN(string scsiid)
        {
            XenAPI.SR sr = this.xenModelObject as XenAPI.SR;
            List<PBD> pbds = sr.Connection.ResolveAll<PBD>(sr.PBDs);
            if (pbds.Any<PBD>(pbd => pbd.other_config.ContainsKey("LUN1-status") && pbd.other_config["LUN1-status"].Contains("spare rebuilding") ||
                    pbd.other_config.ContainsKey("LUN2-status") && pbd.other_config["LUN2-status"].Contains("spare rebuilding")))
            {
                return false;
            }
            if (pbds.Any<PBD>(pdb => pdb.other_config.ContainsKey("LUN1-status") && pdb.other_config["LUN1-status"].Contains("removed") && pdb.other_config.ContainsKey("LUN1-scsiid") && pdb.other_config["LUN1-scsiid"] == scsiid ||
                    pdb.other_config.ContainsKey("LUN2-status") && pdb.other_config["LUN2-status"].Contains("removed") && pdb.other_config.ContainsKey("LUN2-scsiid") && pdb.other_config["LUN2-scsiid"] == scsiid ||
                    !pdb.other_config.ContainsKey("LUN1-status") || !pdb.other_config.ContainsKey("LUN2-status")))
            {
                return true;
            }
            
            return false;
        }

        private bool canissciRemove()
        {
            XenAPI.SR sr = this.xenModelObject as XenAPI.SR;
            List<PBD> pbds = sr.Connection.ResolveAll<PBD>(sr.PBDs);

            if (pbds.Any<PBD>(pbd => pbd.other_config.ContainsKey("LUN1-status") && pbd.other_config["LUN1-status"].Contains("spare rebuilding") ||
                    pbd.other_config.ContainsKey("LUN2-status") && pbd.other_config["LUN2-status"].Contains("spare rebuilding")))
            {
                return false;
            }
            if (pbds.Any(pbd => pbd.other_config.ContainsKey("LUN1-status") && pbd.other_config["LUN1-status"].Contains("removed") ||
                    pbd.other_config.ContainsKey("LUN2-status") && pbd.other_config["LUN2-status"].Contains("removed")))
            {
                return false;
            }
            
            return true;
        }

        private bool isHAenableOfSR(SR sr)
        {
            Pool pool = Helpers.GetPoolOfOne(sr.Connection);
            List<SR> srs = pool.GetHAHeartbeatSRs();
            if (!pool.ha_enabled)
            {
                return false;
            }
            foreach (SR hasr in srs)
            {
                if (hasr.Equals(sr))
                {
                    return true;
                }
            }
            return false;
        }
        
        private void generateGeneralBox()
        {
            GeneralDataList.Add(new GeneralDataStructure(FriendlyName("host.name_label"), Helpers.GetName(this.xenModelObject), General));
            GeneralDataList.Add(new GeneralDataStructure(FriendlyName("host.name_description"), this.xenModelObject.Description, General));
            GeneralDataList.Add(new GeneralDataStructure(Messages.FOLDER, this.xenModelObject.Path, General));
            GeneralDataList.Add(new GeneralDataStructure(Messages.TAGS, this.TagsString(), General));
            if (this.xenModelObject is XenObject<XenAPI.SR>)
            {
                XenAPI.SR xenObject = this.xenModelObject as XenAPI.SR;
                GeneralDataList.Add(new GeneralDataStructure(Messages.TYPE, xenObject.FriendlyTypeName, General));
                if ((xenObject.content_type != "iso") && (xenObject.GetSRType(false) != XenAPI.SR.SRTypes.udev))
                {
                    GeneralDataList.Add(new GeneralDataStructure(FriendlyName("SR.size"), xenObject.SizeString, General));
                }
                if (xenObject.GetScsiID() != null && xenObject.GetScsiID().Count > 0)
                {
                    foreach (String scsiid in xenObject.GetScsiID())
                    {
                        if (xenObject.GetSRType(true) == SR.SRTypes.lvmobond)
                        {
                            if (CanAddLUN(scsiid))
                            {
                                List<ToolStripMenuItem> ctxMenuItems = new List<ToolStripMenuItem>();                              
                                ToolStripMenuItem itm = new ToolStripMenuItem(Messages.ADD) { Image = Resources._000_StorageBroken_h32bit_16 };
                                itm.Click += delegate
                                {
                                    List<FibreChannelDevice> devices;
                                    var success = LVMoBond.FiberChannelScan(this, xenObject.Connection, out devices);
                                    Program.MainWindow.ShowPerConnectionWizard(this.xenModelObject.Connection, new AddLUNDialog(xenObject, devices));
                                };

                                ctxMenuItems.Add(itm);
                                GeneralDataList.Add(new GeneralDataStructure(FriendlyName("SR.scsiid"), scsiid ?? Messages.UNKNOWN, General, Color.Red, ctxMenuItems));
                            }
                            else if (canissciRemove())
                            {
                                List<ToolStripMenuItem> ctxMenuItems = new List<ToolStripMenuItem>();                               
                                ToolStripMenuItem itm = new ToolStripMenuItem(Messages.REMOVE) { Image = Resources._000_StorageBroken_h32bit_16 };
                                itm.Click += delegate
                                {
                                    AsyncAction Action = new SrRemoveLUNAction(xenObject.Connection, xenObject, scsiid);
                                    ActionProgressDialog dialog = new ActionProgressDialog(Action, ProgressBarStyle.Marquee) { ShowCancel = true };
                                    dialog.ShowDialog(this);

                                };
                                ctxMenuItems.Add(itm);
                                GeneralDataList.Add(new GeneralDataStructure(FriendlyName("SR.scsiid"), scsiid ?? Messages.UNKNOWN, General, ctxMenuItems));
                            }
                            else
                            {
                                GeneralDataList.Add(new GeneralDataStructure(FriendlyName("SR.scsiid"), scsiid ?? Messages.UNKNOWN, General));
                            }
                        }
                        else if (xenObject.GetSRType(true) == SR.SRTypes.lvmomirror)
                        {
                            if (CanAddLUN(scsiid))
                            {
                                List<ToolStripMenuItem> ctxMenuItems = new List<ToolStripMenuItem>();
                                ToolStripMenuItem itm = new ToolStripMenuItem(Messages.ADD) { Image = Resources._000_StorageBroken_h32bit_16 };
                                itm.Click += delegate
                                {
                                    List<FibreChannelDevice> devices;
                                    var success = LVMoMirror.FiberChannelScan(this, xenObject.Connection, out devices);
                                    Program.MainWindow.ShowPerConnectionWizard(this.xenModelObject.Connection, new AddMirrorLUNDialog(xenObject, devices));
                                };

                                ctxMenuItems.Add(itm);
                                GeneralDataList.Add(new GeneralDataStructure(FriendlyName("SR.scsiid"), scsiid ?? Messages.UNKNOWN, General, Color.Red, ctxMenuItems));
                            }
                            else if (canissciRemove())
                            {
                                List<ToolStripMenuItem> ctxMenuItems = new List<ToolStripMenuItem>();
                                ToolStripMenuItem itm = new ToolStripMenuItem(Messages.REMOVE) { Image = Resources._000_StorageBroken_h32bit_16 };
                                itm.Click += delegate
                                {
                                    AsyncAction Action = new SrRemoveMirrorLUNAction(xenObject.Connection, xenObject, scsiid);
                                    ActionProgressDialog dialog = new ActionProgressDialog(Action, ProgressBarStyle.Marquee) { ShowCancel = true };
                                    dialog.ShowDialog(this);

                                };
                                ctxMenuItems.Add(itm);
                                GeneralDataList.Add(new GeneralDataStructure(FriendlyName("SR.scsiid"), scsiid ?? Messages.UNKNOWN, General, ctxMenuItems));
                            }
                            else
                            {
                                GeneralDataList.Add(new GeneralDataStructure(FriendlyName("SR.scsiid"), scsiid ?? Messages.UNKNOWN, General));
                            }
                        }
                        else
                        {
                            GeneralDataList.Add(new GeneralDataStructure(FriendlyName("SR.scsiid"), scsiid ?? Messages.UNKNOWN, General));
                        }
                    }
                }

                if ((Program.MainWindow.SelectionManager.Selection.HostAncestor == null) && (Program.MainWindow.SelectionManager.Selection.PoolAncestor == null))
                {
                    IXenObject o = Helpers.GetPool(xenObject.Connection);
                    if (o != null)
                    {
                        GeneralDataList.Add(new GeneralDataStructure(Messages.POOL, Helpers.GetName(o), General));
                    }
                    else
                    {
                        o = Helpers.GetMaster(xenObject.Connection);
                        if (o != null)
                        {
                            GeneralDataList.Add(new GeneralDataStructure(Messages.SERVER, Helpers.GetName(o), General));
                        }
                    }
                }
            }

            GeneralDataList.Add(new GeneralDataStructure(FriendlyName("host.uuid"), GetUUID(this.xenModelObject), General));
        }

        private void generateStatusBox()
        {
            XenAPI.SR sr = this.xenModelObject as XenAPI.SR;
            if (sr != null)
            {
                bool flag = (sr.IsBroken() || !sr.MultipathAOK);
                bool isDetached = sr.IsDetached;
                List<ToolStripMenuItem> contextMenuItems = new List<ToolStripMenuItem>();
                
                ToolStripMenuItem item = new ToolStripMenuItem(Messages.GENERAL_SR_CONTEXT_REPAIR) { Image = Resources._000_StorageBroken_h32bit_16 };
                item.Click += delegate
                {
                    Program.MainWindow.ShowPerConnectionWizard(this.xenModelObject.Connection, new RepairSRDialog(sr));
                };
                contextMenuItems.Add(item);
                if (flag && !isDetached)
                {
                    GeneralDataList.Add(new GeneralDataStructure(FriendlyName("SR.state"), sr.StatusString, Status, contextMenuItems));
                }
                else
                {
                    GeneralDataList.Add(new GeneralDataStructure(FriendlyName("SR.state"), sr.StatusString, Status));
                }
                foreach (Host host in this.xenModelObject.Connection.Cache.Hosts)
                {
                    PBD pbd = null;
                    foreach (PBD pbd2 in this.xenModelObject.Connection.ResolveAll<PBD>(host.PBDs))
                    {
                        if (!(pbd2.SR.opaque_ref != this.xenModelObject.opaque_ref))
                        {
                            pbd = pbd2;
                            break;
                        }
                    }
                    if (pbd == null)
                    {
                        if (sr.shared)
                        {
                            if (!isDetached)
                            {
                                GeneralDataList.Add(new GeneralDataStructure("  " + HalsignHelpers.TrimStringIfRequired(Helpers.GetName(host), 30), Messages.REPAIR_SR_DIALOG_CONNECTION_MISSING, Status, Color.Red, contextMenuItems));
                            }
                            else
                            {
                                GeneralDataList.Add(new GeneralDataStructure("  " + HalsignHelpers.TrimStringIfRequired(Helpers.GetName(host), 30), Messages.REPAIR_SR_DIALOG_CONNECTION_MISSING, Status, Color.Red));
                            }
                        }
                    }
                    else
                    {
                        pbd.PropertyChanged -= new PropertyChangedEventHandler(this.PropertyChanged);
                        pbd.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
                        if (!pbd.currently_attached)
                        {
                            if (!isDetached)
                            {
                                GeneralDataList.Add(new GeneralDataStructure(HalsignHelpers.TrimStringIfRequired(Helpers.GetName(host), 30), pbd.StatusString, Status, Color.Red, contextMenuItems));
                            }
                            else
                            {
                                GeneralDataList.Add(new GeneralDataStructure(HalsignHelpers.TrimStringIfRequired(Helpers.GetName(host), 30), pbd.StatusString, Status, Color.Red));
                            }
                        }
                        else
                        {
                            GeneralDataList.Add(new GeneralDataStructure(HalsignHelpers.TrimStringIfRequired(Helpers.GetName(host), 30), pbd.StatusString, Status));
                        }
                    }
                    if (sr.GetSRType(true) == SR.SRTypes.lvmobond)
                    {
                        if (pbd != null)
                        {
                            String status;
                            if (pbd.other_config.ContainsKey("LUN1-status"))
                            {
                                if (pbd.other_config.TryGetValue("LUN1-status", out status))
                                {
                                    GeneralDataList.Add(new GeneralDataStructure(FriendlyName("LUN1"), status, Status));
                                }
                            }
                            if (pbd.other_config.ContainsKey("LUN2-status"))
                            {
                                if (pbd.other_config.TryGetValue("LUN2-status", out status))
                                {
                                    GeneralDataList.Add(new GeneralDataStructure(FriendlyName("LUN2"), status, Status));
                                }
                            }
                        }
                    }                    
                }//foreach
                /**
                if (sr.GetSRType(true) == SR.SRTypes.lvmobond)
                {
                    if (sr.sm_config.ContainsKey("state"))
                    {
                        GeneralDataList.Add(new GeneralDataStructure(FriendlyName("RAID.state"), sr.sm_config["state"], Status));
                    }
                    if (sr.sm_config.ContainsKey("LUN0-status"))
                    {
                        
                        if (sr.sm_config["LUN0-status"].Contains("removed"))
                        {
                            List<ToolStripMenuItem> ctxMenuItems = new List<ToolStripMenuItem>();
                            ToolStripMenuItem itm = MainWindow.NewToolStripMenuItem("Add", Resources._000_StorageBroken_h32bit_16, delegate(object sender, EventArgs e)
                            {
                                List<FibreChannelDevice> devices;
                                var success = LVMoBond.FiberChannelScan(this, sr.Connection, out devices);
                                Program.MainWindow.ShowPerConnectionWizard(this.xenModelObject.Connection, new AddLUNDialog(sr, devices));
                            });

                            ctxMenuItems.Add(itm);
                            GeneralDataList.Add(new GeneralDataStructure(FriendlyName("LUN1"), sr.sm_config["LUN0-status"], Status, Color.Red, ctxMenuItems));
                        }
                        else
                        {
                            String iscsiid = sr.sm_config["LUN0-scsiid"];
                            String mpath_enable = sr.sm_config["multipathable"];
                            String boundsr_dev = sr.sm_config["md_device"];
                            Dictionary<String, String> args = new Dictionary<string, string>();
                            args.Add("scsiid", iscsiid);
                            args.Add("mpath_enable", mpath_enable);
                            args.Add("boundsr_dev", boundsr_dev);
                            List<ToolStripMenuItem> ctxMenuItems = new List<ToolStripMenuItem>();
                            ToolStripMenuItem itm = MainWindow.NewToolStripMenuItem("remove", Resources._000_StorageBroken_h32bit_16, delegate(object sender, EventArgs e)
                            {
                                try
                                {
                                    AsyncAction Action = new SrRemoveLUNAction(sr.Connection, sr, args, false);
                                    Action.RunAsync();
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                            ctxMenuItems.Add(itm);
                            GeneralDataList.Add(new GeneralDataStructure(FriendlyName("LUN1"), sr.sm_config["LUN0-status"], Status, ctxMenuItems));
                        }
                    }
                    if (sr.sm_config.ContainsKey("LUN1-status"))
                    {                        
                        if (sr.sm_config["LUN1-status"].Contains("removed"))
                        {
                            List<ToolStripMenuItem> ctxMenuItems = new List<ToolStripMenuItem>();
                            ToolStripMenuItem itm = MainWindow.NewToolStripMenuItem("Add", Resources._000_StorageBroken_h32bit_16, delegate(object sender, EventArgs e)
                            {
                                List<FibreChannelDevice> devices;
                                var success = LVMoBond.FiberChannelScan(this, sr.Connection, out devices);
                                Program.MainWindow.ShowPerConnectionWizard(this.xenModelObject.Connection, new AddLUNDialog(sr, devices));
                            });

                            ctxMenuItems.Add(itm);
                            GeneralDataList.Add(new GeneralDataStructure(FriendlyName("LUN2"), sr.sm_config["LUN1-status"], Status, Color.Red, ctxMenuItems));
                        }
                        else
                        {
                            String iscsiid = sr.sm_config["LUN1-scsiid"];
                            String mpath_enable = sr.sm_config["multipathable"];
                            String boundsr_dev = sr.sm_config["md_device"];
                            Dictionary<String, String> args = new Dictionary<string, string>();
                            args.Add("scsiid", iscsiid);
                            args.Add("mpath_enable", mpath_enable);
                            args.Add("boundsr_dev", boundsr_dev);
                            List<ToolStripMenuItem> ctxMenuItems = new List<ToolStripMenuItem>();
                            ToolStripMenuItem itm = MainWindow.NewToolStripMenuItem("remove", Resources._000_StorageBroken_h32bit_16, delegate(object sender, EventArgs e)
                            {
                                try
                                {
                                    AsyncAction Action = new SrRemoveLUNAction(sr.Connection, sr, args, false);
                                    Action.RunAsync();
                                    
                                }
                                catch(Exception ex)
                                {
                                }
                            });
                            ctxMenuItems.Add(itm);
                            GeneralDataList.Add(new GeneralDataStructure(FriendlyName("LUN2"), sr.sm_config["LUN1-status"], Status, ctxMenuItems));
                        }
                    }
                }**/


            }
        }

        private void generateMultipathBootBox()
        {
            int num;
            int num2;
            Host xenObject = this.xenModelObject as Host;
            if ((xenObject != null) && xenObject.GetBootPathCounts(out num, out num2))
            {
                string str = string.Format(Messages.MULTIPATH_STATUS, num, num2);
                GeneralDataList.Add(new GeneralDataStructure(Messages.STATUS, str, MultipathBoot));
            }
        }

        private void generateMultipathBox()
        {
            XenAPI.SR xenObject = this.XenModelObject as XenAPI.SR;
            if (xenObject != null)
            {
                if (!xenObject.MultipathCapable)
                {
                    GeneralDataList.Add(new GeneralDataStructure(Messages.MULTIPATH_CAPABLE, Messages.NO, Multipathing));
                }
                else if (xenObject.LunPerVDI)
                {
                    Dictionary<VM, Dictionary<VDI, string>> multiPathStatusLunPerVDI = xenObject.GetMultiPathStatusLunPerVDI();
                    foreach (Host host in this.XenModelObject.Connection.Cache.Hosts)
                    {
                        PBD pBDFor = xenObject.GetPBDFor(host);
                        if ((pBDFor == null) || !pBDFor.MultipathActive)
                        {
                            GeneralDataList.Add(new GeneralDataStructure(host.Name, Messages.MULTIPATH_NOT_ACTIVE, Multipathing));
                        }
                        else
                        {
                            GeneralDataList.Add(new GeneralDataStructure(host.Name, Messages.MULTIPATH_ACTIVE, Multipathing));
                            foreach (KeyValuePair<VM, Dictionary<VDI, string>> pair in multiPathStatusLunPerVDI)
                            {
                                VM key = pair.Key;
                                if ((key.resident_on != null) && (key.resident_on.opaque_ref == host.opaque_ref))
                                {
                                    bool flag = false;
                                    int max = -1;
                                    int current = -1;
                                    foreach (KeyValuePair<VDI, string> pair2 in pair.Value)
                                    {
                                        int num3;
                                        int num4;
                                        if (PBD.ParsePathCounts(pair2.Value, out num3, out num4))
                                        {
                                            if (!flag)
                                            {
                                                max = num4;
                                                current = num3;
                                                flag = true;
                                            }
                                            else if ((max != num4) || (current != num3))
                                            {
                                                flag = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (flag)
                                    {
                                        this.AddMultipathLine(string.Format("    {0}", key.Name), current, max, pBDFor.ISCSISessions);
                                    }
                                    else
                                    {
                                        GeneralDataList.Add(new GeneralDataStructure(string.Format("    {0}", key.Name), "", Multipathing));
                                        foreach (KeyValuePair<VDI, string> pair3 in pair.Value)
                                        {
                                            int num5;
                                            int num6;
                                            if (PBD.ParsePathCounts(pair3.Value, out num5, out num6))
                                            {
                                                this.AddMultipathLine(string.Format("        {0}", pair3.Key.Name), num5, num6, pBDFor.ISCSISessions);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Dictionary<PBD, Dictionary<String, String>> multiPathStatusLunPerSR = xenObject.GetMultiPathStatusLunPerSR();
                    foreach (Host host2 in this.XenModelObject.Connection.Cache.Hosts)
                    {
                        PBD pbd2 = xenObject.GetPBDFor(host2);
                        if ((pbd2 == null) || !multiPathStatusLunPerSR.ContainsKey(pbd2))
                        {
                            GeneralDataList.Add(new GeneralDataStructure(host2.Name, ((pbd2 != null) && pbd2.MultipathActive) ? Messages.MULTIPATH_ACTIVE : Messages.MULTIPATH_NOT_ACTIVE, Multipathing));
                        }
                        else
                        {
                            GeneralDataList.Add(new GeneralDataStructure(host2.Name, "", Multipathing));
                            foreach (String k in multiPathStatusLunPerSR[pbd2].Keys)
                            {
                                int num7;
                                int num8;
                                string input = multiPathStatusLunPerSR[pbd2][k];
                                PBD.ParsePathCounts(input, out num7, out num8);
                                this.AddMultipathLine(k, num7, num8, pbd2.ISCSISessions);
                            }                            
                        }
                    }
                }
            }
        }

        private void AddMultipathLine(string k, int current, int max, int iscsiSessions)
        {
            bool flag = (current < max) || ((iscsiSessions != -1) && (max < iscsiSessions));
            string str = string.Format(Messages.MULTIPATH_STATUS, k, current, max);
            if (iscsiSessions != -1)
            {
                str = str + string.Format(Messages.MULTIPATH_STATUS_ISCSI_SESSIONS, iscsiSessions);
            }
            if (flag)
            {
                GeneralDataList.Add(new GeneralDataStructure("SCSI ID", str, Multipathing, Color.Red));
            }
            else
            {
                GeneralDataList.Add(new GeneralDataStructure("SCSI ID", str, Multipathing));
            }
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName != "state") && (e.PropertyName != "last_updated"))
            {
                Program.Invoke(this, delegate
                {
                    if (e.PropertyName == "PBDs")
                    {
                        XenAPI.SR xenObject = this.xenModelObject as XenAPI.SR;
                        if (xenObject != null)
                        {
                            foreach (PBD pbd in this.xenModelObject.Connection.ResolveAll<PBD>(xenObject.PBDs))
                            {
                                pbd.PropertyChanged -= new PropertyChangedEventHandler(this.PropertyChanged);
                                pbd.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
                            }
                            this.BuildList();
                        }
                    }
                    else
                    {
                        this.BuildList();
                        //this.EnableDisableEdit();
                    }
                });
            }
        }

        public void Cleanup()
        {
            if (this.toolTip != null)
            {
                this.toolTip.Dispose();
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
            this.listViewExGeneral = new GeneralListViewEx();
            this.ItemName = new System.Windows.Forms.ColumnHeader();
            this.ItemValue = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // listViewExGeneral
            // 
            this.listViewExGeneral.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewExGeneral.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ItemName,
            this.ItemValue});
            this.listViewExGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewExGeneral.FullRowSelect = true;
            this.listViewExGeneral.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewExGeneral.Location = new System.Drawing.Point(0, 0);
            this.listViewExGeneral.MultiSelect = false;
            this.listViewExGeneral.Name = "listViewExGeneral";
            this.listViewExGeneral.OwnerDraw = true;
            this.listViewExGeneral.Size = new System.Drawing.Size(490, 316);
            this.listViewExGeneral.TabIndex = 2;
            this.listViewExGeneral.UseCompatibleStateImageBehavior = false;
            this.listViewExGeneral.View = System.Windows.Forms.View.Details;
            // 
            // ItemName
            // 
            this.ItemName.Text = "Item Name";
            this.ItemName.Width = 150;
            // 
            // ItemValue
            // 
            this.ItemValue.Text = "Item Value";
            this.ItemValue.Width = 246;
            // 
            // BrowseStorageGeneralPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.listViewExGeneral);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BrowseStorageGeneralPage";
            this.Size = new System.Drawing.Size(490, 316);
            this.ResumeLayout(false);

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
                return Messages.NAME_SUB_DESCRIPTION_BROWSE_GENERAL;
            }
        }

        public bool ValidToSave
        {
            get
            {
                return this._ValidToSave;
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
                XenModelObject = clone;
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
    }
}

