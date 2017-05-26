using System;
using System.Collections.Generic;
using XenAPI;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace XenAdmin.Commands
{
    internal class ExportMacCommand : Command
    {
        public ExportMacCommand() 
        {
        }
        public ExportMacCommand(IMainWindow mainWindow, IEnumerable<SelectedItem> selection)
            : base(mainWindow, selection)
        {
        }
        public ExportMacCommand(IMainWindow mainWindow, SelectedItem selection)
            : base(mainWindow, selection)
        {
        }
        
        protected override void ExecuteCore(SelectedItemCollection selection)
        {
            List<String> dic1=new List<String>();
            List<String> dic2 =new List<String>();
            Dictionary<String, String> dics = new Dictionary<string, string>();
            //StreamWriter mystream = null;
            String filename = null;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "All files(*.*)|*.*";
            sfd.RestoreDirectory = true;
            Dictionary<String, String> mac_version=new Dictionary<string,string>();
            Dictionary<String, int> host_cpus = new Dictionary<string, int>();
            if (selection[0].XenObject is Pool&&selection.Count==1) 
            {
                Pool p = (Pool)selection[0].XenObject;
                List<Host> hostList = new List<Host>(p.Connection.Cache.Hosts);
                foreach(Host h in hostList)
                {
                    int a = 0;
                    List<PIF> pifList = h.Connection.ResolveAll(h.PIFs);
                    foreach(PIF pi in pifList)
                    {
                        if (pi.LinkStatus == PIF.LinkState.Connected) 
                        {
                            mac_version.Add(pi.MAC, h.ProductVersionText);
                            host_cpus.Add(h.name_label,h.CpuSockets);
                            a = 1;
                            break;
                        }
                    }
                    if(a==0)
                    {
                        mac_version.Add(pifList[0].MAC,h.ProductVersionText);
                        host_cpus.Add(h.name_label, h.CpuSockets);
                    }
                                        
                }
            }
            foreach(SelectedItem s in selection)
            {
                if (s.XenObject is Host)
                {
                    int a = 0;
                    Host host = (Host)s.XenObject;
                    List<PIF> pifList = host.Connection.ResolveAll(host.PIFs);
                    foreach (PIF pi in pifList)
                    {
                        if (pi.LinkStatus == PIF.LinkState.Connected)
                        {
                            mac_version.Add(pi.MAC, host.ProductVersionText);
                            host_cpus.Add(host.name_label, host.CpuSockets);
                            a = 1;
                            break;
                        }                     
                    }
                    if(a == 0)
                    {
                        mac_version.Add(pifList[0].MAC, host.ProductVersionText);
                        host_cpus.Add(host.name_label, host.CpuSockets);
                    }                     
                }    
            }
            sfd.Title = Messages.SAVE_PATH;
            sfd.ShowDialog();            
            filename = sfd.FileName;
            if(!string.IsNullOrEmpty(filename))
            {
                String path = filename.ToString();
                FileStream fsOut = File.Create(@path);
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                CryptoStream cs = new CryptoStream(fsOut, tdes.CreateEncryptor(), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);
                //mystream = new StreamWriter(filename);
                foreach(KeyValuePair<String,String> pair in mac_version)
                {
                    dic1.Add(Messages.MEDIA_ACCESS_CONTROL+":"+pair.Key.ToUpper()+","+Messages.VGATE_VERSION+pair.Value);
                }
                foreach (KeyValuePair<String, int> pair in host_cpus) 
                {
                    dic2.Add(Messages.HOST+":"+pair.Key +","+Messages.CPUS+":"+pair.Value);
                }
                for (int i = 0; i < dic1.Count; i++ )
                {
                    dics.Add(dic2[i], dic1[i]);
                }
                foreach (KeyValuePair<String, String> pair in dics) 
                {
                    sw.WriteLine(pair);
                    //allData += pair.ToString();
                    //mystream.WriteLine(pair);
                }
               // data = Encoding.Unicode.GetBytes(allData);
               // StringBuilder result = new StringBuilder(data.Length * 8);
                //foreach (byte b in data)
                //{
                //    result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
                //}
               // mystream.Write(result);
                //mystream.Flush();
                //mystream.Close();
                sw.Flush();
                sw.Close();
                FileStream fsKeyOut = File.Create(@path+".pt");
                BinaryWriter bw = new BinaryWriter(fsKeyOut);
                bw.Write(tdes.Key);
                bw.Write(tdes.IV);
                bw.Flush();
                bw.Close();
            }           
        }

        protected override bool CanExecuteCore(SelectedItemCollection selection)
        {   
            if(selection.Count==1&&selection[0].XenObject is Pool)
            {
                return true;
            }
            foreach(SelectedItem s in selection)
            {
                if (s.XenObject is Host)
                {
                    continue;
                }
                else 
                {
                    return false;
                }                
            }
            return true;
        }
        public override string MenuText
        {
            get
            {
                return Messages.MAINWINDOW_EXPORT_MAC;
            }
        }
        public override string ContextMenuText
        {
            get
            {
                return Messages.MAINWINDOW_EXPORT_MAC_CONTEXT_MENU;
            }
        }
        public override Keys ShortcutKeys
        {
            get
            {
                return Keys.Control | Keys.L;
            }
        }

        public override string ShortcutKeyDisplayString
        {
            get
            {
                return Messages.MAINWINDOW_CTRL_L;
            }
        }

    }
}
