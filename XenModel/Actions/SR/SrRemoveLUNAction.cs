﻿/* Copyright (c) Citrix Systems Inc. 
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
    public class SrRemoveLUNAction : AsyncAction
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private String scsiid;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sr">Must not be null.</param>
        public SrRemoveLUNAction(IXenConnection connection, SR sr, String id)
            : base(connection, string.Format(Messages.ACTION_SR_LUN_REMOVING, sr.NameWithoutHost))
        {

            if (sr == null)
                throw new ArgumentNullException("sr");

            this.SR = sr;
            this.scsiid = id;
            
            #region RBAC Dependencies
            ApiMethodsToRoleCheck.Add("pbd.plug");
            ApiMethodsToRoleCheck.Add("pbd.create");
            ApiMethodsToRoleCheck.AddRange(Role.CommonSessionApiList);
            #endregion
        }
        
        protected override void Run()
        {
            int max = Connection.Cache.Hosts.Length * 2;
            int delta = 100 / max;
            foreach (Host host in Connection.Cache.Hosts)
            {
                Dictionary<String, String> args = new Dictionary<string, string>();
                args.Add("sr_uuid", SR.uuid);
                args.Add("scsiid", scsiid);
                args.Add("mpath_enable", this.SR.sm_config["multipathable"]);
                args.Add("boundsr_dev", this.SR.sm_config["md_device"]);
                args.Add("host_uuid", host.uuid);
                RelatedTask = XenAPI.Host.async_call_plugin(host.Connection.Session, host.opaque_ref, "ManageLun.py", "removeLUN", args);
                this.Description = string.Format(Messages.ACTION_SR_LUN_REMOVING, Helpers.GetName(host));
                if (PercentComplete + delta <= 100)
                {
                    PollToCompletion(PercentComplete, PercentComplete + delta);
                }
                else
                {
                    PollToCompletion(PercentComplete, 100);
                    PercentComplete = 100;
                }
                PercentComplete += delta;
            }
            
            Description = Messages.ACTION_SR_LUN_REMOVE_SUCCESSFUL;
        }        

        protected override void CancelRelatedTask()
        {
            this.Description = Messages.ACTION_SR_REPAIR_CANCELLED;

            base.CancelRelatedTask();
        }
    }
}
