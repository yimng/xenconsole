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
using System.Collections;
using System.Collections.Generic;
using XenAdmin.Network;
using XenAdmin.Core;
using XenAPI;

namespace XenAdmin.Actions
{
    public class SrRepairLUNAction : AsyncAction
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly SR sr;
        private readonly Dictionary<String, String> dconf;

        /// <summary>
        /// RBAC dependencies needed to reattach SR.
        /// </summary>
        public static RbacMethodList StaticRBACDependencies = new RbacMethodList("sr.set_name_label",
                                                                                 "sr.set_name_description",
                                                                                 "pbd.async_create",
                                                                                 "pbd.async_plug");
        public SrRepairLUNAction(IXenConnection connection, SR sr, string id)
            : base(connection, string.Format(Messages.ACTION_SR_REPAIRING, sr.NameWithoutHost))
        {
            if (sr == null)
                throw new ArgumentNullException("sr");

            this.sr = sr;
            PBD pbd = this.sr.Connection.ResolveAll<PBD>(this.sr.PBDs)[0];
            this.dconf = pbd.device_config;
            foreach (var item in this.dconf)
            {
                if (item.Value == id)
                {
                    this.dconf[item.Key] = "";
                    break;
                }
            }               

            #region RBAC Dependencies
            ApiMethodsToRoleCheck.Add("pbd.plug");
            ApiMethodsToRoleCheck.Add("pbd.create");
            ApiMethodsToRoleCheck.AddRange(Role.CommonSessionApiList);
            #endregion
        }
        

        protected override void Run()
        {
            foreach (XenRef<PBD> pbd in sr.PBDs)
            {
                RelatedTask = PBD.async_destroy(Session, pbd.opaque_ref);
                PollToCompletion();
            }

            Description = Messages.ACTION_SR_ATTACHING;

            // Now repair the SR with new PBDs for each host in the pool
            PBD pbdTemplate = new PBD();
            pbdTemplate.currently_attached = false;
            pbdTemplate.device_config = dconf;
            pbdTemplate.SR = new XenRef<SR>(sr.opaque_ref);

            int delta = 100 / (Connection.Cache.HostCount * 2);
            List<Host> _listHost = new List<Host>(Connection.Cache.Hosts);
            masterFirst(_listHost);
            foreach (Host host in _listHost)
            {
                // Create the PBD
                log.DebugFormat("Creating PBD for host {0}", host.Name);
                this.Description = String.Format(Messages.ACTION_SR_REPAIR_CREATE_PBD, Helpers.GetName(host));
                pbdTemplate.host = new XenRef<Host>(host.opaque_ref);
                RelatedTask = PBD.async_create(this.Session, pbdTemplate);
                PollToCompletion(PercentComplete, PercentComplete + delta);
                XenRef<PBD> pbdRef = new XenRef<PBD>(this.Result);

                // Now plug the PBD
                log.DebugFormat("Plugging PBD for host {0}", host.Name);
                this.Description = String.Format(Messages.ACTION_SR_REPAIR_PLUGGING_PBD, Helpers.GetName(host));
                RelatedTask = PBD.async_plug(this.Session, pbdRef);
                PollToCompletion(PercentComplete, PercentComplete + delta);
            }

            Description = Messages.ACTION_SR_ATTACH_SUCCESSFUL;
        }

        protected override void CancelRelatedTask()
        {
            this.Description = Messages.ACTION_SR_REPAIR_CANCELLED;

            base.CancelRelatedTask();
        }

        private void masterFirst(List<Host> l)
        {
            Host master = null;
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].IsMaster())
                {
                    master = l[i];
                    l.RemoveAt(i);
                    break;
                }
            }
            if (master != null)
            {
                l.Add(master);
                l.Reverse();
            }
        }
    }
}
