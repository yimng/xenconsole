using Microsoft.VisualBasic.PowerPacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using XenAPI;
namespace XenAdmin.Dialogs
{
    partial class HostNetworkTopologyDialog
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HostNetworkTopologyDialog));
            this.CancelBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // CancelBtn
            // 
            resources.ApplyResources(this.CancelBtn, "CancelBtn");
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Name = "panel1";
            // 
            // HostNetworkTopologyDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.CancelBtn;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CancelBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "HostNetworkTopologyDialog";
            this.ResumeLayout(false);

        }

        #endregion
        private void InitNetTopology()
        {
            this.panel1.Controls.Clear();
            XenAPI.PIF[] pifs = host.Connection.Cache.PIFs;
            XenAPI.Network[] networks = host.Connection.Cache.Networks;
            XenAPI.SR[] srs = host.Connection.Cache.SRs;
            XenAPI.PBD[] pbds = host.Connection.Cache.PBDs;
            XenAPI.VIF[] vifs = host.Connection.Cache.VIFs;
            //XenAPI.VM[] vms = host.Connection.ResolveAll(host.resident_VMs);
            XenAPI.VM[] vms = host.Connection.Cache.VMs;
            int tmpi = 0;
            foreach (IXenObject o in host.Connection.Cache.XenSearchableObjects)
            {
                tmpi++;
            }
            //List<VM> vms = host.Connection.ResolveAll(host.resident_VMs);
            XenAPI.Bond[] bonds = host.Connection.Cache.Bonds;

            //组装bonds的pif
            HashSet<string> bondSets = new HashSet<string>();
            for (int bondi = 0; bondi < bonds.Length; bondi++)
            {
                foreach (var pifop in bonds[bondi].slaves)
                {
                    bondSets.Add(pifop);
                }
            }
            //组装network 与 pif 关系
            Hashtable networkpif = new Hashtable();
            Hashtable bridgedevice = new Hashtable();
            foreach (var netob  in networks)
            {
                if (netob.PIFs.Count > 0)
                {
                    foreach (var pifob in pifs)
                    {
                        if (pifob.opaque_ref.Equals(netob.PIFs[0].opaque_ref))
                        {
                            networkpif.Add(netob, pifob);
                        }
                    }
                }
                else
                {
                    networkpif.Add(netob, null);
                }
            }

            IDictionaryEnumerator enumerator = networkpif.GetEnumerator();
            while (enumerator.MoveNext())
            {
                XenAPI.Network network = (XenAPI.Network)enumerator.Key;
                XenAPI.PIF pif = (XenAPI.PIF)enumerator.Value;

                //if (!network.bridge.StartsWith("xenapi") &&!bondSets.Contains(pif.opaque_ref) && pif.VLAN==-1)
                if (network.bridge.StartsWith("xenapi"))
                {
                    continue;
                }
                if (network.PIFs.Count == 0)
                {
                    bridgedevice.Add(network.bridge, "");
                }
                else if (!bondSets.Contains(pif.opaque_ref) && pif.VLAN == -1)
                {
                    //bridgeSets.Add(network.bridge);
                    bridgedevice.Add(network.bridge, pif.device);
                }
            }
            int totaly = 0;
            ArrayList switchesArray = new ArrayList();
            int i = 0;
            foreach (DictionaryEntry bd in bridgedevice)
            {
                totaly += 20;
                this.panel1.Controls.Add(newLable(40, totaly, 430, 30, "lable" + i, Messages.NETWORKTOPOLOGY_VSWITCH+"：" + bd.Key));
                totaly += 30;
                this.panel1.Controls.Add(newLineShape(40, totaly, 730, totaly, "xenbrline", "", DashStyle.Dot, 2));

                //遍历network 匹配当前相同虚拟交换机
                int nowy = 0;
                totaly += 10;
                for (int l1 = 0; l1 < networks.Length; l1++)
                {
                    int vmsize = 0;
                    if (networks[l1].bridge.Equals("xenapi"))
                    {
                        continue;
                    }
                    else
                    {
                        if (!bd.Key.Equals(networks[l1].bridge))
                        {
                            if (networkpif[networks[l1]] !=null && ((XenAPI.PIF)networkpif[networks[l1]]).device.Equals(bd.Value))
                            { }
                            else
                            {
                                continue;
                            }
                        }

                        HashSet<string> vmSets = new HashSet<string>();
                        //筛选vm
                        for (int vift = 0; vift < vifs.Length; vift++)
                        {
                            if (vifs[vift].network.opaque_ref.Equals(networks[l1].opaque_ref))
                            {
                                vmSets.Add(vifs[vift].VM.opaque_ref);
                            }
                        }
                        ArrayList vmlist = new ArrayList();
                        //for (int vmt = 0; vmt < vms.Length; vmt++)
                        foreach(var vmob in vms)
                        {
                            //if (vmSets.Contains(vms[vmt].opaque_ref) && vms[vmt].TemplateType == XenAPI.VM.VmTemplateType.NoTemplate )
                            if (vmSets.Contains(vmob.opaque_ref) && vmob.TemplateType == XenAPI.VM.VmTemplateType.NoTemplate && host.Equals(vmob.Home()))
                            {
                                vmlist.Add(vmob);
                            }
                        }

                        //groupBox1
                        System.Windows.Forms.GroupBox groupBox1 = new System.Windows.Forms.GroupBox();
                        //totaly += 10;
                        //nowy +=20;
                        groupBox1.SuspendLayout();
                        groupBox1.Size = new Size(250 ,70 + (vmlist.Count * 20 + 12));
                        groupBox1.Controls.Add(newLable(10, 20, 150, 20, "lable21" + i, networks[l1].name_label.Replace("Pool-wide network associated with", "Network")));
                        groupBox1.Controls.Add(newLable(10, 40, 150, 20, "lable22" + i, vmlist.Count + Messages.NETWORKTOPOLOGY_VMS));
                        groupBox1.Controls.Add(newPictureBox(220, 20, 20, 20, "groupBox1PictureBox", global::XenAdmin.Properties.Resources._000_ManagementInterface_h32bit_16));//networkinterfacecard 
                        this.panel1.Controls.Add(newOvalShape(297, totaly + 27, 6, 6, "groupBox1Ovalshape", Color.Lime));
                        this.panel1.Controls.Add(newLineShape(280, totaly + 30, 297, totaly + 30, "", "", DashStyle.Solid, 1));
                        groupBox1.Controls.Add(newLineShape(0, 50, 250, 50, "lineShape2", "", DashStyle.Dot, 2));
                        //遍历虚拟机
                        int groupboxY = 80;
                        vmsize = vmlist.Count;
                        for (int j = 0; j < vmsize; j++)
                        {
                            groupBox1.Controls.Add(newLable(10, groupboxY + (j * 20), 200, 20, "lable24" + i, ((VM)vmlist[j]).name_label));

                            if (((VM)vmlist[j]).power_state == vm_power_state.Running)
                            {
                                groupBox1.Controls.Add(newPictureBox(220, groupboxY + (j * 20), 20, 20, "pictureBoxvm" + j, global::XenAdmin.Properties.Resources._000_StartVM_h32bit_16));
                                this.panel1.Controls.Add(newOvalShape(297, totaly + groupboxY + (j * 20) + 7, 6, 6, "shapeContainer1", Color.Lime));
                                this.panel1.Controls.Add(newLineShape(280, totaly + groupboxY + (j * 20) + 10, 297, totaly + groupboxY + (j * 20) + 10, "", "", DashStyle.Solid, 1));
                            }
                            else
                            {
                                groupBox1.Controls.Add(newPictureBox(220, groupboxY + (j * 20), 20, 20, "pictureBoxvm" + j, global::XenAdmin.Properties.Resources._000_StoppedVM_h32bit_16));
                            }
                        }
                        
                        this.panel1.Controls.Add(groupBox1);
                        groupBox1.Name = "groupBox1";
                        groupBox1.TabStop = false;
                        groupBox1.Location = new Point(40, totaly);
                        //groupBox1.Text = networks[l1].name_label.StartsWith("Pool-wide network associated with ")?"":"";
                        groupBox1.Text = Messages.NETWORKTOPOLOGY_VMS_GROUP;
                        groupBox1.ResumeLayout(false);
                    }
                    totaly += 70 + (vmsize * 20 + 10);
                    nowy += 70 + (vmsize * 20 + 10);
                }
                //遍历硬件

                if (!bd.Value.Equals(""))
                {
                    for (int pift = 0; pift < pifs.Length; pift++)
                    {
                        if (pifs[pift].device.Equals(bd.Value) && pifs[pift].currently_attached && pifs[pift].VLAN==-1)
                        {
                            ArrayList nicArray = new ArrayList();
                            if (pifs[pift].management)
                            {
                                System.Windows.Forms.GroupBox groupBox2 = new System.Windows.Forms.GroupBox();
                                groupBox2.SuspendLayout();
                                groupBox2.Size = new Size(250, 70);
                                groupBox2.Controls.Add(newLable(10, 20, 200, 20, "nic", pifs[pift].ManagementInterfaceNameOrUnknown));
                                groupBox2.Controls.Add(newLable(10, 40, 200, 20, "ip", Messages.NETWORKPANEL_IP+"：" + pifs[pift].IP));
                                groupBox2.Controls.Add(newPictureBox(220, 20, 20, 20, "pictureBox1", global::XenAdmin.Properties.Resources._000_ManagementInterface_h32bit_16));
                                this.panel1.Controls.Add(newOvalShape(297, totaly + 27, 6, 6, "shapeContainer1", Color.Lime));
                                this.panel1.Controls.Add(newLineShape(280, totaly + 30, 297, totaly + 30, "", "", DashStyle.Solid, 1));
                                this.panel1.Controls.Add(groupBox2);
                                groupBox2.Name = "groupBox2" + i;
                                groupBox2.TabStop = false;
                                groupBox2.Location = new Point(40, totaly);
                                groupBox2.Text = Messages.NETWORKTOPOLOGY_MANAGEMENTINTERFACE;
                                groupBox2.ResumeLayout(false);

                                totaly += 70;
                                nowy += 70;
                            }

                            //网卡线路
                            System.Windows.Forms.GroupBox groupBox3 = new System.Windows.Forms.GroupBox();
                            groupBox3.SuspendLayout();
                            groupBox3.Size = new Size(300, 70);
                            groupBox3.AutoSize = true;

                            if (((string)bd.Value).StartsWith("bond"))
                            {
                                foreach (var bondob in bonds)
                                {
                                    if (bondob.master.opaque_ref.Equals(pifs[pift].opaque_ref))
                                    {
                                        int slavei = 0;
                                        foreach (var slaveob in bondob.slaves)
                                        {
                                            foreach (var pifob in pifs)
                                            {
                                                if (slaveob.opaque_ref.Equals(pifob.opaque_ref))
                                              {
                                                  groupBox3.Controls.Add(newPictureBox(35, 40 * (1 + slavei) -20, 20, 20, "pictureBoxnic", global::XenAdmin.Properties.Resources.networkinterfacecard));
                                                  groupBox3.Controls.Add(newLable(60, 40 * (1 + slavei) -20, 220, 20, "nic", pifob.Name + " " + pifob.Speed + ";duplex:" + pifob.Duplex));
                                                  groupBox3.Controls.Add(newLable(60, 40 * (1 + slavei), 240, 20, "ipaddress", Messages.NETWORKPANEL_IP + ":" + pifob.IP + ";" + Messages.NETWORKTOPOLOGY_GATEWAY + ":" + pifob.gateway));

                                                  if (pifob.Carrier)
                                                  {
                                                      groupBox3.Controls.Add(newOvalShape(20, 27 + 40* (slavei), 6, 6, "shapeContainer1", Color.Lime));
                                                      groupBox3.Controls.Add(newOvalShape(290, 27 + 40 * (slavei), 6, 6, "shapeContainer1", Color.Lime));
                                                      string[] ss = new string[2];
                                                      ss[0] = totaly - nowy + 30 + 40 * (slavei)+"";
                                                      nicArray.Add(ss);
                                                  }
                                                  else
                                                  {
                                                      groupBox3.Controls.Add(newOvalShape(20, 27 +40 * (slavei), 6, 6, "shapeContainer1", Color.Red));
                                                  }
                                                  if (slavei == 0)
                                                  {
                                                      this.panel1.Controls.Add(newOvalShape(312, totaly - nowy + 27 , 6, 6, "shapeContainer1", Color.Lime));
                                                      this.panel1.Controls.Add(newLineShape(318, totaly - nowy + 30 , 355, totaly - nowy + 30 + 40 * (slavei), "", "", DashStyle.Solid, 1));
                                                  }
                                                  else
                                                  {
                                                      if (slavei > 1 && nowy < (30 * (slavei+1)))
                                                      { 
                                                          totaly+=40;
                                                          nowy += 40;
                                                      }
                                                      groupBox3.Controls.Add(newLineShape(10, 14, 10, 14 + 40 * (slavei), "Lline1" + slavei, "", DashStyle.Solid, 1));
                                                      groupBox3.Controls.Add(newLineShape(10, 14 + 40 * (slavei), 17, 14 + 40 * (slavei), "Lline2" + slavei, "", DashStyle.Solid, 1));
                                                  }
                                                  slavei++;
                                              }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                groupBox3.Controls.Add(newPictureBox(35, 20, 20, 20, "pictureBoxnic", global::XenAdmin.Properties.Resources.networkinterfacecard));
                                groupBox3.Controls.Add(newLable(60, 20, 220, 20, "nic", pifs[pift].Name + " " + pifs[pift].Speed + ";duplex:" + pifs[pift].Duplex));
                                groupBox3.Controls.Add(newLable(60, 40, 240, 20, "ipaddress", "ip:" + pifs[pift].IP + /*"netmask:" + pifs[pift].netmask +*/ ";gateway:" + pifs[pift].gateway));
                                this.panel1.Controls.Add(newOvalShape(312, totaly - nowy + 27, 6, 6, "shapeContainer1", Color.Lime));
                                if (pifs[pift].Carrier)
                                {
                                    groupBox3.Controls.Add(newOvalShape(20, 27, 6, 6, "shapeContainer1", Color.Lime));
                                    groupBox3.Controls.Add(newOvalShape(290, 27, 6, 6, "shapeContainer1", Color.Lime));
                                    
                                    this.panel1.Controls.Add(newLineShape(318, totaly - nowy + 30, 355, totaly - nowy + 30, "", "", DashStyle.Solid, 1));
                                    string[] ss = new string[1];
                                    ss[0] = totaly - nowy + 30 + "";
                                    nicArray.Add(ss);
                                }
                                else
                                {
                                    groupBox3.Controls.Add(newOvalShape(20, 27, 6, 6, "shapeContainer1", Color.Red));
                                    this.panel1.Controls.Add(newLineShape(318, totaly - nowy + 30, 355, totaly - nowy + 30, "", "", DashStyle.Solid, 1));
                                }
                            }
                            /*
                            this.panel1.Controls.Add(groupBox3);
                            groupBox3.Name = "groupBox3" + i;
                            groupBox3.TabStop = false;
                            groupBox3.Location = new Point(335, totaly - nowy);
                            groupBox3.Text = "物理适配器";
                            groupBox3.ResumeLayout(false);
                            */
                            
                            if (nicArray.Count>0)
                            {
                                //路由或交换机
                                System.Windows.Forms.GroupBox groupBox4 = new System.Windows.Forms.GroupBox();
                                groupBox4.SuspendLayout();
                                groupBox4.Size = new Size(150, 70);
                                groupBox4.Controls.Add(newPictureBox(25, 20, 105, 28, "pictureBoxnic", global::XenAdmin.Properties.Resources.switches));
                                groupBox4.Controls.Add(newOvalShape(10, 27, 6, 6, "shapeContainer1", Color.Lime));
                                //groupBox4.Controls.Add(newOvalShape(140, 27, 6, 6, "shapeContainer1", Color.Lime));

                                foreach (var nicob in nicArray)
                                {
                                    this.panel1.Controls.Add(newLineShape(631, System.Int32.Parse(((string[])nicob)[0]), 665, totaly - nowy + 30, "", "", DashStyle.Solid, 1));
                                }

                                this.panel1.Controls.Add(groupBox4);
                                groupBox4.Name = "groupBox4" + i;
                                groupBox4.TabStop = false;
                                groupBox4.Location = new Point(655, totaly - nowy);
                                groupBox4.Text = Messages.NETWORKTOPOLOGY_SWITCH_ROUTER;
                                groupBox4.ResumeLayout(false);

                                string[] ss = new string[4];
                                ss[0] = pifs[pift].IP;
                                ss[1] = pifs[pift].netmask;
                                ss[2] = 805 + "";
                                ss[3] = totaly - nowy + 27 + "";
                                switchesArray.Add(ss);
                            }
                            this.panel1.Controls.Add(groupBox3);
                            groupBox3.Name = "groupBox3" + i;
                            groupBox3.TabStop = false;
                            groupBox3.Location = new Point(335, totaly - nowy);
                            groupBox3.Text = Messages.NETWORKTOPOLOGY_PHYSICAL_ADAPTER;
                            groupBox3.ResumeLayout(false);

                            break;
                        }
                    }
                }
                else
                {
                    //无物理网卡
                    System.Windows.Forms.GroupBox groupBox3 = new System.Windows.Forms.GroupBox();
                    groupBox3.SuspendLayout();
                    groupBox3.Size = new Size(300, 70);
                    groupBox3.Controls.Add(newLable(60, 20, 200, 20, "nic", Messages.NETWORKTOPOLOGY_NO_PHYSICAL_NIC));
                    groupBox3.Controls.Add(newOvalShape(20, 27, 6, 6, "shapeContainer1", Color.Lime));
                    this.panel1.Controls.Add(newOvalShape(312, totaly - nowy + 27, 6, 6, "shapeContainer1", Color.Lime));
                    this.panel1.Controls.Add(newLineShape(318, totaly - nowy + 30, 355, totaly - nowy + 30, "", "", DashStyle.Solid, 1));
                    this.panel1.Controls.Add(groupBox3);
                    groupBox3.Name = "groupBox3" + i;
                    groupBox3.TabStop = false;
                    groupBox3.Location = new Point(335, totaly - nowy);
                    groupBox3.Text = Messages.NETWORKTOPOLOGY_PHYSICAL_ADAPTER;
                    groupBox3.ResumeLayout(false);
                }
                //vswicth 
                this.panel1.Controls.Add(newRectangleShape(300, totaly - nowy + 10, 15, nowy - 10, "rectangleShape1", 2));
                this.panel1.Controls.Add(newLable(0, totaly + 10, 0, 10, "labelend", ""));
                i++;
            }
            //存储
            System.Windows.Forms.GroupBox groupBox5 = new System.Windows.Forms.GroupBox();
            groupBox5.SuspendLayout();
            groupBox5.AutoSize = true;
            groupBox5.AutoScrollOffset = new Point(250, 70);
            groupBox5.Size = new Size(150, 70);
            int srui = 0;
            for (int sri = 0; sri < srs.Length; sri++)
            {
                if (srs[sri].shared && !srs[sri].name_label.Equals("vGateServer Tools"))
                {
                    for (int pbdi = 0; pbdi < pbds.Length; pbdi++)
                    {
                        if (pbds[pbdi].SR.opaque_ref.Equals(srs[sri].opaque_ref))
                        {
                            if (pbds[pbdi].currently_attached)
                            {
                                groupBox5.Controls.Add(newPictureBox(25, 40 * (1 + srui) - 20, 16, 16, "pictureBoxnic", global::XenAdmin.Properties.Resources._000_Storage_h32bit_16));
                                groupBox5.Controls.Add(newOvalShape(10, 27 + (40 * srui), 6, 6, "shapeContainer1", Color.Lime));
                                break;
                            }
                            else
                            {
                                groupBox5.Controls.Add(newPictureBox(25, 40 * (1 + srui) - 20, 16, 16, "pictureBoxnic", global::XenAdmin.Properties.Resources._000_StorageBroken_h32bit_16));
                                groupBox5.Controls.Add(newOvalShape(10, 27 + (40 * srui), 6, 6, "shapeContainer1", Color.Red));
                                break;
                            }
                        }
                    }
                    groupBox5.Controls.Add(newLable(40, 40 * (1 + srui)-20, 200, 20, "sr", srs[sri].name_label));
                    groupBox5.Controls.Add(newLable(40, 40 * (1 + srui), 200, 20, "srdescription", srs[sri].name_description));
                    /*
                    for(int swi = 0; swi < switchesArray.Count; swi++)
                    {
                        string[] ss = (string[])switchesArray[swi];
                        this.panel1.Controls.Add(newLineShape(System.Int32.Parse(ss[2]), System.Int32.Parse(ss[3]), 865, 50 + 27 + (40 *srui), "", "", DashStyle.Solid, 1));
                    }*/
                    srui++;
                }
            }
            //this.panel1.Controls.Add(newLineShape(800, totaly - nowy + 30, 840, totaly - nowy + 30, "", "", DashStyle.Solid, 1));
            this.panel1.Controls.Add(groupBox5);
            groupBox5.Name = "groupBox5";
            groupBox5.TabStop = false;
            groupBox5.Location = new Point(855, 70);
            groupBox5.Text = Messages.CONFIGURATION_HARDWARE_STORAGE;
            groupBox5.ResumeLayout(false);

            this.panel1.Controls.Add(newLable(1115,0, 20, 20, "labelrend", ""));
        }
        private System.Windows.Forms.Label newLable(int x,int y,int sh,int sw,string name ,string text)
        {
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            label.Location = new Point(x, y);
            label.Size = new Size(sh, sw);
            label.Name = name;
            label.Text = text;
            return label;
        }

        private Microsoft.VisualBasic.PowerPacks.ShapeContainer newLineShape(int x1, int y1, int x2, int y2, string name, string text, DashStyle dashStyle, int borderWidth)
        {
            Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            Microsoft.VisualBasic.PowerPacks.LineShape lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            shapeContainer1.Location = new System.Drawing.Point(3, 17);
            shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            shapeContainer1.Name = "shapeContainer1";
            shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            lineShape1});
            shapeContainer1.Size = new System.Drawing.Size(244, 230);
            shapeContainer1.TabIndex = 0;
            shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            lineShape1.BorderStyle = dashStyle;
            lineShape1.BorderWidth = borderWidth;
            lineShape1.Name = "lineShape1";
            lineShape1.X1 = x1;
            lineShape1.X2 = x2;
            lineShape1.Y1 = y1;
            lineShape1.Y2 = y2;
            return shapeContainer1;
        }

        private System.Windows.Forms.PictureBox newPictureBox(int x, int y, int sh, int sw, string name, Image image)
        {
            System.Windows.Forms.PictureBox pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            pictureBox1.Image = image;
            pictureBox1.Name = name;
            pictureBox1.TabStop = false;
            pictureBox1.Location = new Point(x, y);//220,20
            pictureBox1.Size = new Size(sh, sw);//20,20
            return pictureBox1;
        }

        private Microsoft.VisualBasic.PowerPacks.ShapeContainer newOvalShape(int x, int y, int sh, int sw, string name,Color color)
        {
            Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContaineroval = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape1 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
            ovalShape1.Location = new System.Drawing.Point(x, y);
            ovalShape1.Name = name;
            ovalShape1.Size = new System.Drawing.Size(sh, sw);
            ovalShape1.BackStyle = BackStyle.Opaque;
            ovalShape1.BackColor = color;//Color.Lime;

            shapeContaineroval.Location = new System.Drawing.Point(0, 0);
            shapeContaineroval.Margin = new System.Windows.Forms.Padding(0);
            shapeContaineroval.Name = "shapeContainer1";
            shapeContaineroval.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] { ovalShape1 });
            shapeContaineroval.Size = new System.Drawing.Size(645, 614);
            shapeContaineroval.TabIndex = 0;
            shapeContaineroval.TabStop = false;
            return shapeContaineroval;
        }

        private Microsoft.VisualBasic.PowerPacks.ShapeContainer newRectangleShape(int x, int y, int sh, int sw, string name, int borderWidth)
        {
            Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainervs = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShapevs = new Microsoft.VisualBasic.PowerPacks.RectangleShape();

            shapeContainervs.Location = new System.Drawing.Point(0, 0);
            shapeContainervs.Margin = new System.Windows.Forms.Padding(0);
            shapeContainervs.Name = "shapeContainer1";
            shapeContainervs.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] { rectangleShapevs });
            shapeContainervs.Size = new Size(645, 614);
            shapeContainervs.TabIndex = 0;
            shapeContainervs.TabStop = false;
            rectangleShapevs.Location = new Point(x, y);
            rectangleShapevs.Name = name;
            rectangleShapevs.Size = new Size(sh, sw);
            rectangleShapevs.BorderWidth = borderWidth;
            return shapeContainervs;
        }

        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Panel panel1;
    }
}