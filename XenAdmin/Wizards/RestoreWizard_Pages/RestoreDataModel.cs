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

namespace XenAdmin.Wizards.RestoreWizard_Pages
{
	public class RestoreDataModel
	{
        private string _choice_sr_uuid;
        public string choice_sr_uuid
        {
            get { return _choice_sr_uuid; }
            set { _choice_sr_uuid = value; }
        }

        private string _choice_sr_ip_name;
        public string choice_sr_ip_name
        {
            get { return _choice_sr_ip_name; }
            set { _choice_sr_ip_name = value; }
        }

        private string _choice_sr_free_space;
        public string choice_sr_free_space
        {
            get { return _choice_sr_free_space; }
            set { _choice_sr_free_space = value; }
        }

        private long _choice_sr_free_space_size;
        public long choice_sr_free_space_size
        {
            get { return _choice_sr_free_space_size; }
            set { _choice_sr_free_space_size = value; }
        }
	}

    /// <summary>
    /// for fullbackup, add root_path param when connecting to agent
    /// so add this struct to avoid big code changes
    /// </summary>
    public class AgentParamDataModel
    {
        public string RootPath { get; set; }
        public string VMRestoreInfo { get; set; }

        public AgentParamDataModel(string root_path, string restore_info)
        {
            RootPath = root_path;
            VMRestoreInfo = restore_info;
        }
    }

    /// <summary>
    /// Backup record in queue
    /// </summary>
    public class RestoreLatestDataModel
    {
        public int timestamp { get; set; }
        public string time_str { get; set; }
        public AgentParamDataModel agent_model { get; set; }

        public RestoreLatestDataModel(int ts, string timestr, AgentParamDataModel param)
        {
            timestamp = ts;
            time_str = timestr;
            agent_model = param;
        }
    }
}
