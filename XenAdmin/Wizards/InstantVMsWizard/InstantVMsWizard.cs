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
            List<AsyncAction> actions = new List<AsyncAction>();
            for (int i = 0; i < Number; i++)
            {
                var createAction = new CreateVMFastAction(selection.Connection, selection.XenObject as VM);
                //createAction.Completed += createAction_Completed;
                actions.Add(createAction);
            }
            MultipleAction multiAction = new MultipleAction(selectedItem.Connection, Messages.INSTANT_CREATE_VMS, Messages.INSTANT_CREATE_VMS_START,Messages.INSTANT_CREATE_VMS_FINISH ,actions);
            multiAction.RunAsync();
        }
        private void InstantVMsWizard_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void createAction_Completed(ActionBase sender)
        {
            CreateVMFastAction action = (CreateVMFastAction)sender;
            var startAction = new VMStartAction(action.Connection.Resolve(new XenRef<VM>(action.Result)), VMOperationCommand.WarningDialogHAInvalidConfig, VMOperationCommand.StartDiagnosisForm);
            startAction.RunAsync();
        }
    }
}
