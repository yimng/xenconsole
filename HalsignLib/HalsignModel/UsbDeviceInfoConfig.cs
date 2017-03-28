using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HalsignModel
{
    public class UsbDeviceInfoConfig
    {
        [DataContract]
        public class USBInfo
        {
            [DataMember(Name = "devices")]
            public string[] devices { get; set; }
            [DataMember(Name = "id")]
            public string id { get; set; }
            [DataMember(Name = "idx")]
            public string idx { get; set; }
        }

        [DataContract]
        public class PCIsInfo
        {
            [DataMember(Name = "pcis")]
            public List<UsbDeviceInfoConfig.USBInfo> pcis { get; set; }
        }
    }
}
