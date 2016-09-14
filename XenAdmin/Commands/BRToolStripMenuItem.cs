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
using System.ComponentModel;
using HalsignTop;
using XenAPI;
using XenAdmin;

namespace XenAdmin.Commands
{
    internal class BRToolStripMenuItem : CommandToolStripMenuItem
    {
        public BRToolStripMenuItem() : base(new BRCommand()) { }

        public BRToolStripMenuItem(IMainWindow mainwindow) : base(new BRCommand(mainwindow)) { }

        public BRToolStripMenuItem(IMainWindow mainwindow, IEnumerable<SelectedItem> selection) : base(new BRCommand(mainwindow, selection)) { }

        protected override void Update()
        {
            base.Update();

            IMainWindow mainWindowCommandInterface = this.Command.MainWindowCommandInterface;
            SelectedItemCollection selection = this.Command.GetSelection();
            if (mainWindowCommandInterface != null)
            {
                base.DropDownItems.Clear();
                base.DropDownItems.Add(new CommandToolStripMenuItem(new BRBackupCommand(mainWindowCommandInterface, selection)));
                base.DropDownItems.Add(new CommandToolStripMenuItem(new BRRestoreCommand(mainWindowCommandInterface, selection)));
                base.DropDownItems.Add(new CommandToolStripMenuItem(new BRReplicationCommand(mainWindowCommandInterface, selection)));
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public new Command Command
        {
            get
            {
                return base.Command;
            }
        }

        private class BRCommand : Command
        {
            /// <summary>
            /// Update by dalei.shou on 2013/8/27 for vTop, 2013/12/18 removed
            /// </summary>
            /// <param name="selection"></param>
            /// <returns></returns>
            protected override bool CanExecuteCore(SelectedItemCollection selection)
            {
                var conn = selection.GetConnectionOfFirstItem();
                if (conn == null)
                    return false;

                Host tempHost = Pool.getLowestLicenseHost(XenAdmin.Core.Helpers.GetPool(conn));
                if (tempHost != null)
                {
                    //standard
                    if (tempHost.edition.ToLowerInvariant().EndsWith("express"))
                    {
                        return false;
                    }
                }

                if (selection.Count == 1)
                {
                    if (selection[0].Value is Host)
                    {
                        return (selection[0].Value as Host).IsLive;
                    }

                    if (selection[0].Value is VM)
                    {
                        VM vm = selection[0].Value as VM;
                        return vm.other_config.ContainsKey("halsign_br_ready")
                                   ? vm.other_config["halsign_br_ready"].Equals("true")
                                   : vm.current_operations.Count == 0;
                    }
                }
                return false;
            }

            public BRCommand() { }

            public BRCommand(IMainWindow mainwindow) : base(mainwindow) { }

            public BRCommand(IMainWindow mainwindow, IEnumerable<SelectedItem> selection) : base(mainwindow, selection) { }
        }

        public override string Text
        {
            get
            {
                return Messages.BR_MENU;
            }
        }
    }
}
