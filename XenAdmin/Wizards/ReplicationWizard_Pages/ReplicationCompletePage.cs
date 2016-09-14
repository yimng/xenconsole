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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XenAdmin.Controls;

namespace XenAdmin.Wizards.ReplicationWizard_Pages
{
    public partial class ReplicationCompletePage : XenTabPage
	{
        private Label Label_Complete;

        public ReplicationCompletePage()
		{
			InitializeComponent();
		}

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplicationCompletePage));
            this.Label_Complete = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Label_Complete
            // 
            this.Label_Complete.AccessibleDescription = null;
            this.Label_Complete.AccessibleName = null;
            resources.ApplyResources(this.Label_Complete, "Label_Complete");
            this.Label_Complete.Font = null;
            this.Label_Complete.Name = "Label_Complete";
            // 
            // ReplicationCompletePage
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.Label_Complete);
            this.Font = null;
            this.Name = "ReplicationCompletePage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public override string HelpID
        {
            get
            {
                return "Complete";
            }
        }

        public override string PageTitle
        {
            get
            {
                return Messages.REPLICATION_COMPLETE_TEXT;
            }
        }

        public override string Text
        {
            get
            {
                return Messages.REPLICATION_COMPLETE_TEXT;
            }
        }
	}
}
