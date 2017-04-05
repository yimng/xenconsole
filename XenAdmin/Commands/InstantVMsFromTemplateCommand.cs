using System;
using System.Collections.Generic;
using System.Text;

using XenAdmin.Actions;
using XenAdmin.Wizards.InstantVMsWizard;
using XenAdmin.Network;
using XenAPI;
using XenAdmin.Actions.VMActions;
using System.Collections.ObjectModel;


namespace XenAdmin.Commands
{
    /// <summary>
    /// Creates VMs from the specified template.
    /// </summary>
    internal class InstantVMsFromTemplateCommand : Command
    {
        public InstantVMsFromTemplateCommand()
        {
        }

        public InstantVMsFromTemplateCommand(IMainWindow mainWindow, IEnumerable<SelectedItem> selection)
            : base(mainWindow, selection)
        {
        }

        public InstantVMsFromTemplateCommand(IMainWindow mainWindow, VM template)
            : base(mainWindow, template)
        {
        }

        protected override void ExecuteCore(SelectedItemCollection selection)
        {
            GetNumber(selection);
        }
        
        protected void GetNumber(SelectedItemCollection selection) 
        {
            MainWindowCommandInterface.ShowPerConnectionWizard(selection[0].Connection, new InstantVMsWizard(selection[0]));
        }
        

        protected override bool CanExecuteCore(SelectedItemCollection selection)
        {
            return selection.ContainsOneItemOfType<VM>() && selection.AtLeastOneXenObjectCan<VM>(CanExecute);
        }

        private static bool CanExecute(VM vm)
        {
            return vm != null && vm.is_a_template && !vm.Locked && !vm.is_a_snapshot && vm.InstantTemplate;
        }

        public override string MenuText
        {
            get
            {
                return Messages.MAINWINDOW_INSTANT_VMS_FROM_TEMPLATE;
            }
        }
    }
}