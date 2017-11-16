using System;
using XenAPI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XenAdmin.Dialogs.WarningDialogs
{
    public partial class NeedToolsWarningDialog : Form
    {
        private List<VM> no_tools_vms;
        public NeedToolsWarningDialog()
        {
            InitializeComponent();
        }
        public NeedToolsWarningDialog(List<VM> vms)
        {
            no_tools_vms = vms;
            InitializeComponent();
            ComboBoxInit();
        }
        private void ComboBoxInit()
        {
            foreach (VM v in no_tools_vms)
            {
                comboBox1.Items.Add(v.name_label);
            }
            comboBox1.SelectedIndex = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
