/* Copyright (c) Citrix Systems Inc. 
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using XenAPI;

using XenAdmin.Core;
using XenAdmin.Actions;
using XenAdmin.Network;
using XenAdmin.Controls;

namespace XenAdmin.Dialogs
{
    public partial class HostNetworkTopologyDialog : XenDialogBase
    {
        internal Host host;
        public HostNetworkTopologyDialog(Host host)
        {
            this.host = host;
            InitializeComponent();
            InitNetTopology();
            //host.PropertyChanged +=new PropertyChangedEventHandler(PropertyChangedEventHandler);
            XenAPI.PIF[] pifs = host.Connection.Cache.PIFs;
            foreach (PIF PIF in pifs)
            {
                PIF.PropertyChanged +=
                    new PropertyChangedEventHandler(PIF_PropertyChangedEventHandler);
                if (PIF.PIFMetrics != null)
                    PIF.PIFMetrics.PropertyChanged += new PropertyChangedEventHandler(PropertyChangedEventHandler);
            }
        }
        void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("carrier"))
            {
                Program.Invoke(Program.MainWindow, delegate
                {
                    InitNetTopology();
                });
            }
        }
        void PIF_PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "current_operations" &&
                e.PropertyName != "allowed_operations")
            {
                Program.Invoke(Program.MainWindow, delegate
                {
                    InitNetTopology();
                });
            }
        }

        internal void SetHost(Host host)
        {
            this.host = host;
        }
    }
}
