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
        public KernelCrashDumpPage()
        {
            InitializeComponent();
            Text = Messages.KERNEL_CRASH_DUMP_SUBTEXT;
        }
        private void CrashDumpInit()
        {
            Host host = this.host;
        }
        public bool HasChanged
        {
            get
            {
                return false;
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
            return null;
        }

        public void SetXenObjects(IXenObject orig, IXenObject clone)
        {
            host = (Host)clone;
        }

        public void ShowLocalValidationMessages(){  }
    }
}
