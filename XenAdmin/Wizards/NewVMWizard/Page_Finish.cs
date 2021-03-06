﻿/* Copyright (c) Citrix Systems, Inc. 
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
using System.Windows.Forms;
using XenAdmin.Controls;
using XenAPI;

namespace XenAdmin.Wizards.NewVMWizard
{
    public partial class Page_Finish : XenTabPage
    {
        public Page_Finish()
        {
            InitializeComponent();
            richTextBox1.Text = Messages.NEWVMWIZARD_FINISHPAGE;
        }

        public override string Text
        {
            get { return Messages.NEWVMWIZARD_FINISHPAGE_NAME; }
        }

        public override string PageTitle
        {
            get { return Messages.NEWVMWIZARD_FINISHPAGE_TITLE; }
        }

        public override string HelpID
        {
            get { return "Finish"; }
        }

        public override string NextText(bool isLastPage)
        {
            return Messages.NEWVMWIZARD_FINISHPAGE_CREATE;
        }

        public bool StartImmediately
        {
            get
            {
                return AutoStartCheckBox.Checked;
            }
        }
        public bool UseIntelliCache
        {
            get
            {
                return UseSSDCache.Checked;
            }
        }
        public override void PageLoaded(PageLoadedDirection direction)
        {
            base.PageLoaded(direction);
            SummaryGridView.Rows.Clear();
            
            if (SummaryRetreiver == null)
                return;

            var entries = SummaryRetreiver.Invoke();
            foreach (KeyValuePair<string, string> pair in entries)
                SummaryGridView.Rows.Add(pair.Key, pair.Value);
        }

        public override void SelectDefaultControl()
        {
            AutoStartCheckBox.Select();
            UseSSDCache.Enabled = EnableSSDCache();
        }

        public Func<IEnumerable<KeyValuePair<string, string>>> SummaryRetreiver { private get; set; }
        public Host Affinity { get; set; }
        public SR sysSR { get; set; }

        private bool EnableSSDCache()
        {
            List<SR> AllSRs = new List<SR>(Connection.Cache.SRs);
            if (Affinity != null)
            {
                foreach(PBD pbd in Connection.ResolveAll<PBD>(Affinity.PBDs))
                {
                    SR sr = Connection.Resolve<SR>(pbd.SR);
                    if (sr.GetSRType(true) == SR.SRTypes.ext && SR.get_local_cache_enabled(Connection.Session, sr.opaque_ref))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (!sysSR.IsLocalSR && sysSR.GetSRType(true) != SR.SRTypes.nfs)
                {
                    return false;
                }
                else
                {
                    foreach(SR sr in AllSRs)
                    {
                        if (sr.GetSRType(true) == SR.SRTypes.ext && SR.get_local_cache_enabled(Connection.Session, sr.opaque_ref))
                        {
                            Affinity = sr.GetStorageHost();
                            return true;// pool has ssd cache
                        }
                    }
                }
            }
            return false;        
        }
    }
}
