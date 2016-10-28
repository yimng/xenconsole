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

namespace XenAdmin.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using XenAPI;
    using XenAdmin.Model;
    using XenAdmin;
    using XenAdmin.Properties;
    using XenAdmin.Dialogs;

    internal class BrowseStorageCommand : Command
    {

        public BrowseStorageCommand()
        {
            //
        }

        public BrowseStorageCommand(IMainWindow mainWindow, IEnumerable<SelectedItem> selection) : base(mainWindow, selection)
        {
            //
        }

        protected override bool CanExecuteCore(SelectedItemCollection selection)
        {
            if (!selection.ContainsOneItemOfType<IXenObject>())
            {
                return false;
            }
            IXenObject xenObject = selection[0].XenObject;
            return ((!(xenObject is Folder) && (xenObject.Connection != null)) && xenObject.Connection.IsConnected);
        }

        protected void Execute(IXenObject xenObject)
        {
            BrowseStorageDialog dialog = new BrowseStorageDialog(xenObject);
            dialog.SelectTab("BrowseStorageGeneralPage");
            dialog.ShowDialog(base.Parent);
        }

        protected override void ExecuteCore(SelectedItemCollection selection)
        {
            this.Execute(selection[0].XenObject);
        }

        public override string ContextMenuText
        {
            get
            {
                return Messages.BROWSE_STORAGE;
            }
        }

        public override Image MenuImage
        {
            get
            {
                return Resources._000_VirtualStorage_h32bit_16;
            }
        }

        public override string MenuText
        {
            get
            {
                return Messages.BROWSE_STORAGE;
            }
        }
    }
}

