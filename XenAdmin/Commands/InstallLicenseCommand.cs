/* Copyright © 2013 Halsign Corporation.
 * All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, 
 * with or without modification, are permitted provided 
 * that the following conditions are met: 
 * 
 * *   Redistributions of source code must retain the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer. 
 * *   Redistributions in binary form must reproduce the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer in the documentation and/or other 
 *     materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalsignTop;
using XenAdmin.Wizards;
using XenAPI;
using System.Windows.Forms;
using XenAdmin.Dialogs;
using XenAdmin;
using XenAdmin.Actions;
using XenAdmin.Commands;

namespace XenAdmin.Commands
{
    internal class InstallLicenseCommand : XenAdmin.Commands.Command
    {
        public InstallLicenseCommand() { }

        public InstallLicenseCommand(IMainWindow mainWindow, IEnumerable<SelectedItem> selection)
            : base(mainWindow, selection) { }

        /// <summary>
        /// Update by dalei.shou on 2013/8/28 for vTop, 2013/12/19 removed
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        protected override bool CanExecuteCore(SelectedItemCollection selection)
        {
            Host hostAncestor = selection.HostAncestor;
            return (selection.Count == 1) && (hostAncestor != null) && hostAncestor.enabled && hostAncestor.IsLive &&
                   selection[0].Connection.IsConnected;
        }

        protected override void ExecuteCore(SelectedItemCollection selection)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = Messages.INSTALL_LICENSE_KEY;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.Filter = string.Format("{0} (*.tslic)|*.tslic|{1} (*.*)|*.*", Messages.XS_LICENSE_FILES, Messages.ALL_FILES);
            //dialog.ShowHelp = true;
            //dialog.HelpRequest += new EventHandler(this.dialog_HelpRequest);
            if ((dialog.ShowDialog(Program.MainWindow) == DialogResult.OK))
            {
                Host hostAncestor = selection.HostAncestor;
                ApplyLicenseAction action = new ApplyLicenseAction(hostAncestor.Connection, hostAncestor, dialog.FileName);
                ActionProgressDialog progressDialog = new ActionProgressDialog(action, ProgressBarStyle.Marquee);
                progressDialog.Text = Messages.INSTALL_LICENSE_KEY;
                progressDialog.ShowDialog(Program.MainWindow);
            }
        }

        public override string MenuText
        {
            get
            {
                return Messages.MENU_INSTALL_LICENSE_KEY;
            }
        }
    }
}
