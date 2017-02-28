using System;
using System.Collections.Generic;
using XenAPI;
using System.IO;
using System.Windows.Forms;

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
            StreamWriter mystream = null;
            String filename = null;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "All files(*.*)|*.*";
            sfd.RestoreDirectory = true;
            Dictionary<String, String> mac_version=new Dictionary<string,string>();
            if (selection[0].XenObject is Pool&&selection.Count==1) 
            {
                Pool p = (Pool)selection[0].XenObject;
                List<Host> hostList = new List<Host>(p.Connection.Cache.Hosts);
                foreach(Host h in hostList)
                {
                    List<PIF> pifList = h.Connection.ResolveAll(h.PIFs);
                    mac_version.Add(pifList[0].MAC, h.ProductVersionText); 
                }
            }
            foreach(SelectedItem s in selection)
            {
                if (s.XenObject is Host)
                {
                    Host host = (Host)s.XenObject;
                    List<PIF> pifList = host.Connection.ResolveAll(host.PIFs);
                    mac_version.Add(pifList[0].MAC, host.ProductVersionText);
                }    
            }
            sfd.ShowDialog();
            filename = sfd.FileName;
            if(!string.IsNullOrEmpty(filename))
            {              
                mystream = new StreamWriter(filename);
                foreach(KeyValuePair<String,String> pair in mac_version)
                {
                    mystream.WriteLine(pair);
                }
                mystream.Flush();
                mystream.Close();
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
    }
}
