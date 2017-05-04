using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XenAdmin.Actions;
using XenAdmin.Actions.VMActions;
using XenAdmin.Commands;
using XenAPI;
using XenAdmin.Dialogs.WarningDialogs;

namespace XenAdmin.Wizards.InstantVMsWizard
{
    public partial class InstantVMsWizard : Form
    {

        private int Number;
        private SelectedItem selectedItem;

        public InstantVMsWizard()
        {
            InitializeComponent();
        }

        public InstantVMsWizard(SelectedItem selectedItem)
        {
            this.selectedItem = selectedItem;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!(Char.IsNumber(e.KeyChar))&&e.KeyChar!=(char)8)
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null||textBox1.Text=="")
            {
                Number = 0;
            }
            else 
            {
                Number = int.Parse(textBox1.Text);
                RunCreate(selectedItem);
            }
            
            this.Close();
        }

        public void RunCreate(SelectedItem selection)
        {
            //判断是否有足够空间创建输入数量的虚拟机
            VM temp = selection.XenObject as VM;
            List<VBD> vbds = selection.Connection.ResolveAll<VBD>(temp.VBDs);
            List<VDI> vdis = new List<VDI>();
            Dictionary<VDI,SR> vdi_sr=new Dictionary<VDI,SR>(); 
            if(Number>0)
            {
                //取所有的vdi
                foreach(VBD vbd in vbds)
                {
                    if(selection.Connection.Resolve<VDI>(vbd.VDI)!=null)
                    {
                        vdis.Add(selection.Connection.Resolve<VDI>(vbd.VDI));
                    }
                    
                }
                //用Dictionary保存每个vdi对应的sr，vdi-sr
                foreach(VDI vdi in vdis)
                {
                    vdi_sr.Add(vdi,selection.Connection.Resolve<SR>(vdi.SR));
                }
                //用List保存所有相同SR的vdi，计算出能创建的最多个数后保存到Dictionary中,然后清空List
                long allvdisize = 0;
                long free_sr_size = 0;
                long storage_overhead = 0;
                List<VDI> samevdis=new List<VDI>();
                Dictionary<SR, int> sr_vdinum = new Dictionary<SR, int>();
                for (int i = 0; i < vdis.Count;i++ )
                {
                    for (int j = i; j < vdis.Count;j++ )
                    {
                        if(vdi_sr[vdis[i]]==vdi_sr[vdis[j]])
                        {
                            samevdis.Add(vdis[j]);                        
                        }
                    }
                    foreach(VDI vdi in samevdis)
                    {
                        allvdisize += vdi.virtual_size;
                    }
                    foreach(VDI vdi in selection.Connection.ResolveAll<VDI>(vdi_sr[samevdis[0]].VDIs))
                    {
                        storage_overhead += vdi.virtual_size;   
                    }
                    free_sr_size = vdi_sr[samevdis[0]].FreeSpace;
                    sr_vdinum.Add(vdi_sr[samevdis[0]],Convert.ToInt32(free_sr_size/allvdisize));
                    foreach (VDI vdi in samevdis)
                    {
                        vdi_sr.Remove(vdi);
                        vdis.Remove(vdi);
                    }
                    samevdis.Clear();
                    i = -1;
                }
                if(Number>sr_vdinum.Values.Min())
                {
                    new NotEnoughStorageWarningDialog().ShowDialog();
                    return;
                }
            }
            List<AsyncAction> actions = new List<AsyncAction>();
            for (int i = 0; i < Number; i++)
            {
                var createAction = new CreateVMFastAction(selection.Connection, selection.XenObject as VM);
//                createAction.Completed += createAction_Completed;
                actions.Add(createAction);
            }
            MultipleAction multiAction = new MultipleAction(selectedItem.Connection, Messages.INSTANT_CREATE_VMS, Messages.INSTANT_CREATE_VMS_START,Messages.INSTANT_CREATE_VMS_FINISH ,actions);
            multiAction.RunAsync();
        }

        private void createAction_Completed(ActionBase sender)
        {
            CreateVMFastAction action = (CreateVMFastAction)sender;
            var startAction = new VMStartAction(action.Connection.Resolve(new XenRef<VM>(action.Result)), VMOperationCommand.WarningDialogHAInvalidConfig, VMOperationCommand.StartDiagnosisForm);
            startAction.RunAsync();
        }
    }
}
