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
            [DataMember(Name = "vendorid")]
            public string vendorid { get; set; }
            [DataMember(Name = "busid")]
            public string busid { get; set; }
            [DataMember(Name = "sysname")]
            public string sysname { get; set; }
            [DataMember(Name = "devid")]
            public string devid { get; set; }
            [DataMember(Name = "vm")]
            public string vm { get; set; }
            [DataMember(Name = "shortname")]
            public string shortname { get; set; }
            [DataMember(Name = "deviceid")]
            public string deviceid { get; set; }
            [DataMember(Name = "longname")]
            public string longname { get; set; }
            [DataMember(Name = "serial")]
            public string serial { get; set; }
            [DataMember(Name = "type")]
            public string type { get; set; }
            [DataMember(Name = "id")]
            public string id { get; set; }
            [DataMember(Name = "pciid")]
            public string pciid { get; set; }

        }

        [DataContract]
        public class PCIsInfo
        {
            [DataMember(Name = "pcis")]
            public List<UsbDeviceInfoConfig.USBInfo> pcis { get; set; }
        }

        [DataContract]
        public class PVUsbListResult
        {
            [DataMember(Name = "returncode")]
            public string returncode { get; set; }
            [DataMember(Name = "returnvalue")]
            public List<UsbDeviceInfoConfig.USBInfo> returnvalue { get; set; }
        }

        [DataContract]
        public class AssingResult
        {
            [DataMember(Name = "returncode")]
            public string returncode { get; set; }
            [DataMember(Name = "returnvalue")]
            public string returnvalue { get; set; }
        }
    }
}
