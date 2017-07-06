using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XenAdmin.Actions;
using XenAPI;

namespace XenAdmin.SettingsPanels
{
    public partial class KernelCrashDumpPage : UserControl,IEditPage
    {
        private bool _ValidToSave = true;
        private Host host;
        private String result;
        private bool orig_check=false;
        Dictionary<String, String> args = new Dictionary<string, string>();
        public KernelCrashDumpPage()
        {
            InitializeComponent();
            Text = Messages.KERNEL_CRASH_DUMP_SUBTEXT;
        }
        private void CrashDumpInit()
        {
            Host host = this.host;
            if (host.other_config.ContainsKey("dump_status"))
            {
                result = XenAPI.Host.call_plugin(host.Connection.Session, host.opaque_ref, "kdump-trigger.py", "status", args);
                if (result == true.ToString())
                {
                    checkBox1.Checked = true;
                    orig_check = true;
                }
                else if (result == false.ToString())
                {
                    checkBox1.Checked = false;
                    orig_check = false;
                }
                else if (result == "inconsistent")
                {
                    checkBox1.Enabled = false;
                    label1.Visible = true;

                }
            }     
        }
        public bool HasChanged
        {
            get { return HasStatusChanged(); }
        }
        public bool HasStatusChanged()
        {
            if (checkBox1.Checked == orig_check)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public Image Image
        {
            get
            {
                return Properties.Resources._000_ExportMessages_h32bit_16;
            }
        }

        public string SubText
        {
            get
            {
                return Messages.KERNEL_CRASH_DUMP_SUBTEXT;
            }
        }

        public bool ValidToSave
        {
            get
            {
                return _ValidToSave;
            }
        }

        public void Cleanup(){ }

        public AsyncAction SaveSettings()
        {
            if (checkBox1.Checked==true)
            {
                XenAPI.Host.call_plugin(host.Connection.Session, host.opaque_ref, "kdump-trigger.py", "enable", args);
                if (host.other_config.ContainsKey("dump_status"))
                {
                    XenAPI.Host.remove_from_other_config(host.Connection.Session,host.opaque_ref,"dump_status");
                }
                XenAPI.Host.add_to_other_config(host.Connection.Session,host.opaque_ref,"dump_status","dump_true");
            }
            if (checkBox1.Checked == false)
            {
                XenAPI.Host.call_plugin(host.Connection.Session, host.opaque_ref, "kdump-trigger.py", "disable", args);
                if (host.other_config.ContainsKey("dump_status"))
                {
                    XenAPI.Host.remove_from_other_config(host.Connection.Session, host.opaque_ref, "dump_status");
                }
            }
            return null;
        }

        public void SetXenObjects(IXenObject orig, IXenObject clone)
        {
            host = (Host)clone;
            CrashDumpInit();
        }

        public void ShowLocalValidationMessages(){  }
    }
}
