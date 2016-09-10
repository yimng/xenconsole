using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HalsignLib.HalsignModel
{
    public class OVSCConfig
    {
        [DataContract]
        public class ConfigResult
        {
            [DataMember(Name = "code")]
            public string code { get; set; }
            [DataMember(Name = "message")]
            public string message { get; set; }
            [DataMember(Name = "result")]
            public string result { get; set; }
        }

        public enum OCSC_CONFIG_ERRORCODE
        {
            ERR_SUCCESS = 0,
            ERR_INVALID_PARAMS = 800203,
            ERR_CONTROLLER_ACCOUNT_INVALID = 800204,
            ERR_VGATE_ACCOUNT_INVALID = 800205,
            ERR_REGISTERED = 501005 
        }
    }
}
