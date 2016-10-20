using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XenAPI;
using XenAdmin.Actions;
using XenAdmin.Actions.VMActions;
using XenAdmin.Dialogs;
using System.Drawing;
using System.Windows.Forms;

namespace XenAdmin.Commands
{
    internal class VMResetVDICommand : Command
    {
        public VMResetVDICommand()
        {
        }

        public VMResetVDICommand(IMainWindow mainWindow, IEnumerable<SelectedItem> selection)
            : base(mainWindow, selection)
        {
        }

        public VMResetVDICommand(IMainWindow mainWindow, VM vm)
            : base(mainWindow, vm)
        {
        }

        protected override void ExecuteCore(SelectedItemCollection selection)
        {
            Host host = selection[0].HostAncestor;
            VM vm = (VM)selection[0].XenObject;
            using (var dlg = new ThreeButtonDialog(
                        new ThreeButtonDialog.Details(
                            SystemIcons.Warning,
                            string.Format(Messages.RESET_VDI_DIALOG_TEXT, vm.Name.Ellipsise(25)),
                            Messages.RESET_VM_VDI),
                        new ThreeButtonDialog.TBDButton(Messages.RESET_VM_VDI, DialogResult.OK, ThreeButtonDialog.ButtonType.ACCEPT, true),
                        ThreeButtonDialog.ButtonCancel))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    new ResetVirtualDiskAction(host, vm).RunAsync();
                }
            }
                
        }

        protected override bool CanExecuteCore(SelectedItemCollection selection)
        {
            return selection.ContainsOneItemOfType<VM>() && selection.AtLeastOneXenObjectCan<VM>(CanExecute);
        }

        private static bool CanExecute(VM vm)
        {
            /**
            if (vm != null && !vm.is_a_template)
            {
                if (vm.allowed_operations != null && vm.allowed_operations.Contains(vm_operations.power_state_reset))
                {
                    return true;
                }
            }

            return false;
            */
            return true;
        }

        public override string MenuText
        {
            get
            {
                return Messages.MAINWINDOW_RESET_VDI_VM;
            }
        }
    }
}
