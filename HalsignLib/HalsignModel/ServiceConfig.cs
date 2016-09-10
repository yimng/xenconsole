using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HalsignModel
{
    public class ServiceConfig
    {
        [DataContract]
        public class serviceinfolist
        {
            [DataMember(Name = "servicekey")]
            public string servicekey { get; set; }
            [DataMember(Name = "servicevalue")]
            public string[] servicevalue { get; set; }
        }

    }
}
