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
using System.Runtime.Serialization;

namespace HalsignTop
{
	public class Manager
	{
        [DataContract]
        public class DeviceList
        {
            [DataMember(Name = "code")]
            public int code { get; set; }
            [DataMember(Name = "message")]
            public string message { get; set; }
            [DataMember(Name = "result")]
            public List<ResultInfo> result { get; set; }
        }

        [DataContract]
        public class ResultInfo
        {
            [DataMember(Name = "connected")]
            public bool connected { get; set; }
            [DataMember(Name = "host")]
            public string host { get; set; }
            [DataMember(Name = "nodeName")]
            public string nodeName { get; set; }
            [DataMember(Name = "password")]
            public string password { get; set; }
            [DataMember(Name = "type")]
            public int type { get; set; }
            [DataMember(Name = "userName")]
            public string userName { get; set; }
            [DataMember(Name = "uuid")]
            public string uuid { get; set; }

        }

        [DataContract]
        public class IPList
        {
            [DataMember(Name = "code")]
            public int code { get; set; }
            [DataMember(Name = "message")]
            public string message { get; set; }
            [DataMember(Name = "result")]
            public List<IPInfo> result { get; set; }
        }

        [DataContract]
        public class IPInfo
        {
            [DataMember(Name = "ipAddress")]
            public string ipAddress { get; set; }
            [DataMember(Name = "password")]
            public string password { get; set; }
            [DataMember(Name = "userName")]
            public string userName { get; set; }
        }

        public enum HalsignTypeList
        {
            SERVER = 0,
            DESKTOP = 1
        }
	}
}
