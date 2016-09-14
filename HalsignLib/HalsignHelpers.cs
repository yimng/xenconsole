using System.Linq;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using XenAPI;
using XenAdmin.Network;
using XenAdmin;
using XenAdmin.Core;

namespace HalsignLib
{
    public static class HalsignHelpers
    {
        public static NumberFormatInfo _nfi = new CultureInfo("en-US", false).NumberFormat;
        private static bool _unrecognisedVersionWarned = false;
        private static Regex CpuRegex = new Regex("^cpu([0-9]+)$");
        public const int CUSTOM_BUILD_NUMBER = 0x1a0a;
        public const int DEFAULT_NAME_TRIM_LENGTH = 50;
        private static Regex DiskRegex = new Regex("^vbd_((xvd|hd)[a-z]+)_(read|write)((_latency)?)$");
        public const string GuiTempObjectPrefix = "__gui__";
        public static Regex HostnameOrIpRegex = new Regex(@"[\w.]+");
        private static readonly Regex IqnRegex = new Regex(@"^iqn\.\d{4}-\d{2}\.([a-zA-Z0-9][-_a-zA-Z0-9]*(\.[a-zA-Z0-9][-_a-zA-Z0-9]*)*)(:.+)?$", RegexOptions.ECMAScript);
        private static Regex LoadAvgRegex = new Regex("loadavg");
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex MacRegex = new Regex("^([0-9a-fA-F]{2}:){5}[0-9a-fA-F]{2}$");
        private static Regex NetworkLatencyRegex = new Regex("^network/latency$");
        private static Regex PifBondRegex = new Regex("^pif_(bond[0-9]+)_(tx|rx)((_errors)?)$");
        private static Regex PifBrRegex = new Regex("^pif_xenbr([0-9]+)_(tx|rx)((_errors)?)$");
        private static Regex PifEthRegex = new Regex("^pif_eth([0-9]+)_(tx|rx)((_errors)?)$");
        private static Regex PifLoRegex = new Regex("^pif_lo_(tx|rx)((_errors)?)$");
        private static Regex PifTapRegex = new Regex("^pif_tap([0-9]+)_(tx|rx)((_errors)?)$");
        private static Regex PifVlanRegex = new Regex("^pif_eth([0-9]+).([0-9]+)_(tx|rx)((_errors)?)$");
        private static Regex PifXapiRegex = new Regex("^pif_xapi([0-9]+)_(tx|rx)((_errors)?)$");
        public static readonly Regex SessionRefRegex = new Regex("OpaqueRef:[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}");
        private static Regex SrRegex = new Regex("^sr_[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}_cache_(size|hits|misses)");
        private static Regex StatefileLatencyRegex = new Regex("^statefile/[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}/latency$");
        private static Regex VifRegex = new Regex("^vif_([0-9]+)_(tx|rx)((_errors)?)$");
        private static Regex XapiLatencyRegex = new Regex("^xapi_healthcheck/latency$");

        public static bool ArrayElementsEqual<T>(T[] a, T[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if ((a[i] != null) || (b[i] != null))
                {
                    if (a[i] == null)
                    {
                        return false;
                    }
                    if (!a[i].Equals(b[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int BondSizeLimit(IXenConnection connection)
        {
            Host master = GetMaster(connection);
            if (((master != null) && TampaOrGreater(master)) && master.vSwitchNetworkBackend)
            {
                return 4;
            }
            return 2;
        }

        public static string BoolToString(bool b)
        {
            if (!b)
            {
                return Messages.NO;
            }
            return Messages.YES;
        }

        public static bool BostonOrGreater(IXenConnection conn)
        {
            if (conn != null)
            {
                return BostonOrGreater(GetMaster(conn));
            }
            return true;
        }

        public static bool BostonOrGreater(Host host)
        {
            if (!TampaOrGreater(host) && (productVersionCompare(HostProductVersion(host), "5.6.199") < 0))
            {
                return (HostBuildNumber(host) == 0x1a0a);
            }
            return true;
        }

        public static bool CDsExist(IXenConnection connection)
        {
            if (connection != null)
            {
                foreach (XenAPI.SR sr in connection.Cache.SRs)
                {
                    if (!(sr.content_type != "iso") && (sr.VDIs.Count > 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CompareLists<T>(List<T> l1, List<T> l2)
        {
            if (l1 == l2)
            {
                return true;
            }
            if ((l1 == null) || (l2 == null))
            {
                return false;
            }
            bool flag = l1.Count == l2.Count;
            foreach (T local in l1)
            {
                if (!l2.Contains(local))
                {
                    return false;
                }
            }
            return flag;
        }

        public static bool CowleyOrGreater(IXenConnection conn)
        {
            if (conn != null)
            {
                return CowleyOrGreater(GetMaster(conn));
            }
            return true;
        }

        public static bool CowleyOrGreater(Host host)
        {
            if (!TampaOrGreater(host) && (productVersionCompare(HostProductVersion(host), "5.6.1") < 0))
            {
                return (HostBuildNumber(host) == 0x1a0a);
            }
            return true;
        }

        public static bool CustomWithNoDVD(VM template)
        {
            return (((template != null) && !template.DefaultTemplate) && (template.FindVMCDROM() == null));
        }

        public static string DefaultVMName(string p, IXenConnection connection)
        {
            int num = 1;
            while (true)
            {
                bool flag = true;
                string str = string.Format(Messages.NEWVM_DEFAULTNAME, p, num);
                string str2 = "__gui__" + str;
                foreach (VM vm in connection.Cache.VMs)
                {
                    if ((vm.name_label == str) || (vm.name_label == str2))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return str;
                }
                num++;
            }
        }

        public static string EscapeAmpersands(string s)
        {
            if (s == null)
            {
                return null;
            }
            return s.Replace("&", "&&");
        }

        public static bool FeatureForbidden(IXenConnection xenConnection, Predicate<Host> restrictionTest)
        {
            if (xenConnection != null)
            {
                //foreach (Host host in xenConnection.Cache.Hosts)
                //{
                //    if (restrictionTest(host))
                //    {
                //        return true;
                //    }
                //}
                return false;
            }
            return true;
        }

        public static bool FeatureForbidden(IXenObject iXenObject, Predicate<Host> restrictionTest)
        {
            IXenConnection xenConnection = (iXenObject == null) ? null : iXenObject.Connection;
            return FeatureForbidden(xenConnection, restrictionTest);
        }

        private static Network FindNetworkOfPIF(IXenObject iXenObject, string device)
        {
            PIF pif = FindPIF(iXenObject, device, true);
            if (pif != null)
            {
                Network network = iXenObject.Connection.Resolve<Network>(pif.network);
                if (network != null)
                {
                    return network;
                }
            }
            return null;
        }

        private static Network FindNetworkOfVIF(IXenObject iXenObject, string device)
        {
            foreach (VIF vif in iXenObject.Connection.Cache.VIFs)
            {
                if ((vif.device == device) && (((iXenObject is Host) && ((Host) iXenObject).resident_VMs.Contains(vif.VM)) || ((iXenObject is VM) && (vif.VM.opaque_ref == iXenObject.opaque_ref))))
                {
                    Network network = iXenObject.Connection.Resolve<Network>(vif.network);
                    if (network != null)
                    {
                        return network;
                    }
                }
            }
            return null;
        }

        public static PIF FindPIF(Network network, Host owner)
        {
            foreach (PIF pif in network.Connection.ResolveAll<PIF>(network.PIFs))
            {
                if ((owner == null) || (pif.host == owner.opaque_ref))
                {
                    return pif;
                }
            }
            return null;
        }

        private static PIF FindPIF(IXenObject iXenObject, string device, bool physical)
        {
            foreach (PIF pif in iXenObject.Connection.Cache.PIFs)
            {
                if ((!physical || pif.IsPhysical) && (pif.device == device))
                {
                    if ((iXenObject is Host) && (pif.host.opaque_ref == iXenObject.opaque_ref))
                    {
                        return pif;
                    }
                    if ((iXenObject is VM) && (pif.host.opaque_ref == ((VM) iXenObject).resident_on.opaque_ref))
                    {
                        return pif;
                    }
                }
            }
            return null;
        }

        private static VBD FindVBD(IXenObject iXenObject, string device)
        {
            if (iXenObject is VM)
            {
                VM vm = (VM) iXenObject;
                foreach (VBD vbd in vm.Connection.ResolveAll<VBD>(vm.VBDs))
                {
                    if (vbd.device == device)
                    {
                        return vbd;
                    }
                }
            }
            return null;
        }

        private static Network FindVlan(IXenObject iXenObject, string device, string tag)
        {
            foreach (PIF pif in iXenObject.Connection.Cache.PIFs)
            {
                if (((pif.device == device) && (((iXenObject is Host) && (pif.host.opaque_ref == iXenObject.opaque_ref)) || ((iXenObject is VM) && (pif.host.opaque_ref == ((VM) iXenObject).resident_on.opaque_ref)))) && (pif.VLAN == long.Parse(tag)))
                {
                    return iXenObject.Connection.Resolve<Network>(pif.network);
                }
            }
            return null;
        }

        public static string FirstLine(string s)
        {
            if (s == null)
            {
                return "";
            }
            s = s.Split(new char[] { '\n' })[0];
            return s.Split(new char[] { '\r' })[0];
        }

        private static string FormatFriendly(string key, params string[] args)
        {
            return string.Format(PropertyManager.GetFriendlyName(key), (object[]) args);
        }

        private static string FromHostOrMaster(Host host, HostToStr fn)
        {
            if (host != null)
            {
                string str = fn(host);
                if (str != null)
                {
                    return str;
                }
                Host master = GetMaster(host.Connection);
                if (master != null)
                {
                    return fn(master);
                }
            }
            return null;
        }

        public static bool GeorgeOrGreater(IXenConnection conn)
        {
            if (conn != null)
            {
                return GeorgeOrGreater(GetMaster(conn));
            }
            return true;
        }

        public static bool GeorgeOrGreater(Host host)
        {
            if (!TampaOrGreater(host) && (productVersionCompare(HostProductVersion(host), "3.1.0") < 0))
            {
                return (HostBuildNumber(host) == 0x1a0a);
            }
            return true;
        }

        public static bool? GetBoolXmlAttribute(XmlNode Node, string AttributeName)
        {
            bool flag;
            if ((Node.Attributes[AttributeName] != null) && bool.TryParse(Node.Attributes[AttributeName].Value, out flag))
            {
                return new bool?(flag);
            }
            return null;
        }

        public static bool GetBoolXmlAttribute(XmlNode Node, string AttributeName, bool Default)
        {
            bool? boolXmlAttribute = GetBoolXmlAttribute(Node, AttributeName);
            if (!boolXmlAttribute.HasValue)
            {
                return Default;
            }
            return boolXmlAttribute.Value;
        }

        public static string GetCPUProperties(Host_cpu cpu)
        {
            return string.Format("{0}\n{1}\n{2}", string.Format(Messages.GENERAL_CPU_VENDOR, cpu.vendor), string.Format(Messages.GENERAL_CPU_MODEL, cpu.modelname), string.Format(Messages.GENERAL_CPU_SPEED, cpu.speed));
        }

        public static long GetDirSize(DirectoryInfo dir)
        {
            long num = 0L;
            foreach (FileInfo info in dir.GetFiles())
            {
                num += info.Length;
            }
            foreach (DirectoryInfo info2 in dir.GetDirectories())
            {
                num += GetDirSize(info2);
            }
            return num;
        }

        public static T GetEnumXmlAttribute<T>(XmlNode Node, string Attribute, T Default)
        {
            if (Node.Attributes[Attribute] == null)
            {
                return Default;
            }
            Trace.Assert(typeof(T).IsEnum, "Supplied type to GetEnumXmlAttribute is not an enum");
            try
            {
                return (T) Enum.Parse(typeof(T), Node.Attributes[Attribute].Value);
            }
            catch
            {
                return Default;
            }
        }

        public static float? GetFloatXmlAttribute(XmlNode Node, string AttributeName)
        {
            float num;
            if ((Node.Attributes[AttributeName] != null) && float.TryParse(Node.Attributes[AttributeName].Value, out num))
            {
                return new float?(num);
            }
            return null;
        }

        public static float GetFloatXmlAttribute(XmlNode Node, string AttributeName, float Default)
        {
            float? floatXmlAttribute = GetFloatXmlAttribute(Node, AttributeName);
            if (!floatXmlAttribute.HasValue)
            {
                return Default;
            }
            return floatXmlAttribute.Value;
        }

        public static string GetFriendlyDataSourceName(string name, IXenObject iXenObject)
        {
            if (iXenObject != null)
            {
                string str = GetFriendlyDataSourceName_(name, iXenObject);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }
            return name;
        }

        private static string GetFriendlyDataSourceName_(string name, IXenObject iXenObject)
        {
            Match match = CpuRegex.Match(name);
            if (match.Success)
            {
                return FormatFriendly("Label-performance.cpu", new string[] { match.Groups[1].Value });
            }
            match = VifRegex.Match(name);
            if (match.Success)
            {
                string device = match.Groups[1].Value;
                Network network = FindNetworkOfVIF(iXenObject, device);
                if (network != null)
                {
                    return FormatFriendly(string.Format("Label-performance.vif_{0}{1}", match.Groups[2].Value, match.Groups[3].Value), new string[] { network.Name });
                }
            }
            match = PifEthRegex.Match(name);
            if (match.Success)
            {
                return FormatFriendly(string.Format("Label-performance.nic_{0}{1}", match.Groups[2].Value, match.Groups[3].Value), new string[] { match.Groups[1].Value });
            }
            match = PifVlanRegex.Match(name);
            if (match.Success)
            {
                string str2 = string.Format("eth{0}", match.Groups[1].Value);
                Network network2 = FindVlan(iXenObject, str2, match.Groups[2].Value);
                if (network2 != null)
                {
                    return FormatFriendly(string.Format("Label-performance.vlan_{0}{1}", match.Groups[3].Value, match.Groups[4].Value), new string[] { network2.Name });
                }
            }
            match = PifBrRegex.Match(name);
            if (match.Success)
            {
                string str3 = string.Format("eth{0}", match.Groups[1].Value);
                Network network3 = FindNetworkOfPIF(iXenObject, str3);
                if (network3 != null)
                {
                    return FormatFriendly(string.Format("Label-performance.xenbr_{0}{1}", match.Groups[2].Value, match.Groups[3].Value), new string[] { network3.Name });
                }
                return null;
            }
            match = PifXapiRegex.Match(name);
            if (match.Success)
            {
                return FormatFriendly(string.Format("Label-performance.xapi_{0}{1}", match.Groups[2].Value, match.Groups[3].Value), new string[] { match.Groups[1].Value });
            }
            match = PifBondRegex.Match(name);
            if (match.Success)
            {
                PIF pif = FindPIF(iXenObject, match.Groups[1].Value, false);
                if (pif != null)
                {
                    return FormatFriendly(string.Format("Label-performance.bond_{0}{1}", match.Groups[2].Value, match.Groups[3].Value), new string[] { pif.Name });
                }
                return null;
            }
            match = PifLoRegex.Match(name);
            if (match.Success)
            {
                return FormatFriendly(string.Format("Label-performance.lo_{0}{1}", match.Groups[1].Value, match.Groups[2].Value), new string[0]);
            }
            match = PifTapRegex.Match(name);
            if (match.Success)
            {
                return FormatFriendly(string.Format("Label-performance.tap_{0}{1}", match.Groups[2].Value, match.Groups[3].Value), new string[] { match.Groups[1].Value });
            }
            match = DiskRegex.Match(name);
            if (match.Success)
            {
                VBD vbd = FindVBD(iXenObject, match.Groups[1].Value);
                if (vbd != null)
                {
                    return FormatFriendly(string.Format("Label-performance.vbd_{0}{1}", match.Groups[3].Value, match.Groups[4].Value), new string[] { vbd.userdevice });
                }
                return null;
            }
            match = SrRegex.Match(name);
            if (match.Success)
            {
                return FormatFriendly(string.Format("Label-performance.sr_cache_{0}", match.Groups[1].Value), new string[0]);
            }
            if (NetworkLatencyRegex.IsMatch(name))
            {
                return PropertyManager.GetFriendlyName("Label-performance.network_latency");
            }
            if (XapiLatencyRegex.IsMatch(name))
            {
                return PropertyManager.GetFriendlyName("Label-performance.xapi_latency");
            }
            if (StatefileLatencyRegex.IsMatch(name))
            {
                return PropertyManager.GetFriendlyName("Label-performance.statefile_latency");
            }
            if (LoadAvgRegex.IsMatch(name))
            {
                return PropertyManager.GetFriendlyName("Label-performance.loadavg");
            }
            return PropertyManager.GetFriendlyName(string.Format("Label-performance.{0}", name));
        }

        
        public static Dictionary<string, string> GetGuiConfig(IXenObject o)
        {
            return (o.Get("gui_config") as Dictionary<string, string>);
        }

        public static Host GetHostAncestor(IXenObject xenObject)
        {
            if ((xenObject != null) && (xenObject.Connection != null))
            {
                if (xenObject is Host)
                {
                    return (Host) xenObject;
                }
                if (xenObject is XenAPI.SR)
                {
                    return ((XenAPI.SR) xenObject).Home;
                }
                if (xenObject is VM)
                {
                    VM vm = (VM) xenObject;
                    return vm.Home();
                }
            }
            return null;
        }

        public static string GetHostRestrictions(Host host)
        {
            string str = "";
            List<string> list = new List<string>();
            foreach (string str2 in host.license_params.Keys)
            {
                if (host.license_params[str2] == "true")
                {
                    list.Add(str2);
                }
            }
            bool flag = true;
            list.Sort();
            foreach (string str3 in list)
            {
                string str4 = Messages.ResourceManager.GetString(str3);
                if (str4 != null)
                {
                    if (flag)
                    {
                        str = str + str4;
                        flag = false;
                    }
                    else
                    {
                        str = str + "\n" + str4;
                    }
                }
            }
            return str;
        }

        public static int? GetIntXmlAttribute(XmlNode Node, string AttributeName)
        {
            int num;
            if ((Node.Attributes[AttributeName] != null) && int.TryParse(Node.Attributes[AttributeName].Value, out num))
            {
                return new int?(num);
            }
            return null;
        }

        public static int GetIntXmlAttribute(XmlNode Node, string AttributeName, int Default)
        {
            int? intXmlAttribute = GetIntXmlAttribute(Node, AttributeName);
            if (!intXmlAttribute.HasValue)
            {
                return Default;
            }
            return intXmlAttribute.Value;
        }

        public static object GetListOfNames(List<Host> list)
        {
            List<string> list2 = new List<string>();
            foreach (Host host in list)
            {
                list2.Add(host.Name);
            }
            return string.Join(", ", list2.ToArray());
        }

        public static string GetMacString(string mac)
        {
            if (!(mac == ""))
            {
                return mac;
            }
            return Messages.MAC_AUTOGENERATE;
        }

        public static Host GetMaster(IXenConnection connection)
        {
            Pool poolOfOne = GetPoolOfOne(connection);
            if (poolOfOne != null)
            {
                return connection.Resolve<Host>(poolOfOne.master);
            }
            return null;
        }

        public static Host GetMaster(Pool pool)
        {
            if (pool != null)
            {
                return pool.Connection.Resolve<Host>(pool.master);
            }
            return null;
        }

        public static string GetName(IXenConnection connection)
        {
            if (connection != null)
            {
                return connection.Name;
            }
            return "";
        }

        public static string GetName(IXenObject o)
        {
            if (o == null)
            {
                return "";
            }
            return o.Name;
        }

        public static string GetName(Pool pool)
        {
            if (pool == null)
            {
                return "";
            }
            return pool.Name;
        }

        public static string GetNameAndObject(IXenObject XenObject)
        {
            if (XenObject is Pool)
            {
                return string.Format(Messages.POOL_X, GetName(XenObject));
            }
            if (XenObject is Host)
            {
                return string.Format(Messages.SERVER_X, GetName(XenObject));
            }
            if (XenObject is VM)
            {
                VM vm = (VM) XenObject;
                if (vm.is_control_domain)
                {
                    return string.Format(Messages.SERVER_X, GetName(XenObject.Connection.Resolve<Host>(vm.resident_on)));
                }
                return string.Format(Messages.VM_X, GetName(XenObject));
            }
            if (XenObject is XenAPI.SR)
            {
                return string.Format(Messages.STORAGE_REPOSITORY_X, GetName(XenObject));
            }
            return Messages.UNKNOWN_OBJECT;
        }

        public static Dictionary<string, string> GetOtherConfig(IXenObject o)
        {
            return (o.Get("other_config") as Dictionary<string, string>);
        }

        public static Pool GetPool(IXenConnection connection)
        {
            if (connection != null)
            {
                foreach (Pool pool in connection.Cache.Pools)
                {
                    if ((pool != null) && pool.IsVisible)
                    {
                        return pool;
                    }
                }
            }
            return null;
        }

        public static Pool GetPoolOfOne(IXenConnection connection)
        {
            Trace.Assert(connection != null);
            Pool[] pools = connection.Cache.Pools;
            int index = 0;
            while (index < pools.Length)
            {
                return pools[index];
            }
            return null;
        }

        public static string GetStringXmlAttribute(XmlNode Node, string AttributeName)
        {
            if (Node.Attributes[AttributeName] == null)
            {
                return null;
            }
            return Node.Attributes[AttributeName].Value;
        }

        public static string GetStringXmlAttribute(XmlNode Node, string AttributeName, string Default)
        {
            if (Node.Attributes[AttributeName] == null)
            {
                return Default;
            }
            return Node.Attributes[AttributeName].Value;
        }

        public static string GetUrl(IXenConnection connection)
        {
            UriBuilder builder = new UriBuilder(connection.UriScheme, connection.Hostname) {
                Port = connection.Port
            };
            return builder.ToString();
        }

        public static string GetUuid(IXenObject iXenObject)
        {
            if (iXenObject == null)
            {
                return "";
            }
            PropertyInfo property = iXenObject.GetType().GetProperty("uuid", BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                return "";
            }
            return (string) property.GetValue(iXenObject, null);
        }

        public static Dictionary<VM, VM.HA_Restart_Priority> GetVmHaRestartPriorities(IXenConnection connection, bool showHiddenVMs)
        {
            Dictionary<VM, VM.HA_Restart_Priority> dictionary = new Dictionary<VM, VM.HA_Restart_Priority>();
            foreach (VM vm in connection.Cache.VMs.Where(vm => vm.uuid != null).ToArray())
            {
                if (vm.HaCanProtect(showHiddenVMs))
                {
                    dictionary[vm] = vm.HARestartPriority;
                }
            }
            return dictionary;
        }

        
        public static Dictionary<VM, VMStartupOptions> GetVmStartupOptions(IXenConnection connection, bool showHiddenVMs)
        {
            Dictionary<VM, VMStartupOptions> dictionary = new Dictionary<VM, VMStartupOptions>();
            foreach (VM vm in connection.Cache.VMs.Where(vm => vm.uuid != null).ToArray())
            {
                if (vm.HaCanProtect(showHiddenVMs))
                {
                    dictionary[vm] = new VMStartupOptions(vm.order, vm.start_delay, vm.HARestartPriority);
                }
            }
            return dictionary;
        }

        public static string GetXmlAttribute(XmlNode Node, string Attribute)
        {
            if (Node.Attributes[Attribute] == null)
            {
                throw new I18NException(I18NExceptionType.XmlAttributeMissing, new string[] { Attribute, Node.Name });
            }
            return Node.Attributes[Attribute].Value;
        }

        public static bool HAEnabled(IXenConnection connection)
        {
            Pool poolOfOne = GetPoolOfOne(connection);
            if (poolOfOne == null)
            {
                return false;
            }
            return poolOfOne.ha_enabled;
        }

        public static bool HaIgnoreStartupOptions(IXenConnection connection)
        {
            return !BostonOrGreater(connection);
        }

        public static bool HasFullyConnectedSharedStorage(IXenConnection connection)
        {
            foreach (XenAPI.SR sr in connection.Cache.SRs)
            {
                if (((sr.content_type != "iso") && sr.shared) && sr.CanCreateVmOn())
                {
                    return true;
                }
            }
            return false;
        }

        public static int HostBuildNumber(Host host)
        {
            if (host.BuildNumber > 0)
            {
                return host.BuildNumber;
            }
            Host master = GetMaster(host.Connection);
            if (master != null)
            {
                return master.BuildNumber;
            }
            return -1;
        }

        public static bool HostIsMaster(Host host)
        {
            Pool poolOfOne = GetPoolOfOne(host.Connection);
            if (poolOfOne == null)
            {
                return false;
            }
            return (host.opaque_ref == poolOfOne.master.opaque_ref);
        }

        public static string HostnameFromLocation(string p)
        {
            foreach (Match match in HostnameOrIpRegex.Matches(p))
            {
                return match.Value;
            }
            return "";
        }

        public static string HostPlatformVersion(Host host)
        {
            if (host == null)
            {
                return null;
            }
            return host.PlatformVersion;
        }

        public static string HostProductVersion(Host host)
        {
            return FromHostOrMaster(host, h => h.ProductVersion);
        }

        public static string HostProductVersionText(Host host)
        {
            return FromHostOrMaster(host, h => h.ProductVersionText);
        }

        public static string HostProductVersionTextShort(Host host)
        {
            return FromHostOrMaster(host, h => h.ProductVersionTextShort);
        }

        public static string HostProductVersionWithOEM(Host host)
        {
            if (string.IsNullOrEmpty(OEMName(host)))
            {
                return HostProductVersion(host);
            }
            return string.Format("{0}.{1}", HostProductVersion(host), OEMName(host));
        }

        public static bool IsOlderThanMaster(Host host)
        {
            Host master = GetMaster(host.Connection);
            if ((master == null) || (master.opaque_ref == host.opaque_ref))
            {
                return false;
            }
            if (productVersionCompare(HostProductVersion(host), HostProductVersion(master)) >= 0)
            {
                return false;
            }
            return true;
        }

        public static bool IsPool(IXenConnection connection)
        {
            return (GetPool(connection) != null);
        }

        public static bool IsValidMAC(string macString)
        {
            return MacRegex.IsMatch(macString);
        }

        public static bool IsWLBEnabled(IXenConnection connection)
        {
            Pool poolOfOne = GetPoolOfOne(connection);
            if (poolOfOne == null)
            {
                return false;
            }
            return poolOfOne.wlb_enabled;
        }

        public static TimeSpan LicenceExpiresIn(Host host, DateTime serverTime)
        {
            return host.LicenseExpiryUTC.ToLocalTime().Subtract(serverTime);
        }

        public static List<T> ListsCommonItems<T>(List<T> lst1, List<T> lst2)
        {
            List<T> list = new List<T>();
            foreach (T local in lst1)
            {
                if (lst2.Contains(local) && !list.Contains(local))
                {
                    list.Add(local);
                }
            }
            foreach (T local2 in lst2)
            {
                if (lst1.Contains(local2) && !list.Contains(local2))
                {
                    list.Add(local2);
                }
            }
            return list;
        }

        public static bool ListsContentsEqual<T>(List<T> lst1, List<T> lst2)
        {
            if (lst1.Count != lst2.Count)
            {
                return false;
            }
            foreach (T local in lst1)
            {
                if (!lst2.Contains(local))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ListsIntersect<T>(List<T> l1, List<T> l2)
        {
            foreach (T local in l1)
            {
                if (l2.Contains(local))
                {
                    return true;
                }
            }
            return false;
        }

        public static XmlDocument LoadXmlDocument(Stream xmlStream)
        {
            XmlDocument document = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings {
                IgnoreComments = true,
                IgnoreWhitespace = true,
                IgnoreProcessingInstructions = true
            };
            document.Load(XmlReader.Create(xmlStream, settings));
            return document;
        }

        public static string MakeUniqueName(string stub, List<string> compareAgainst)
        {
            string str = stub;
            for (int i = 1; compareAgainst.Contains(stub); i++)
            {
                stub = string.Format(Messages.NEWVM_DEFAULTNAME, str, i);
            }
            return stub;
        }

        public static string MakeUniqueNameFromPattern(string pattern, List<string> compareAgainst, int startAt)
        {
            string str;
            int num = startAt;
            do
            {
                str = string.Format(pattern, num++);
            }
            while (compareAgainst.Contains(str));
            return str;
        }

        public static int Max(params int[] arr)
        {
            int num = -2147483648;
            foreach (int num2 in arr)
            {
                if (num2 > num)
                {
                    num = num2;
                }
            }
            return num;
        }

        public static bool MidnightRideOrGreater(IXenConnection conn)
        {
            if (conn != null)
            {
                return MidnightRideOrGreater(GetMaster(conn));
            }
            return true;
        }

        public static bool MidnightRideOrGreater(Host host)
        {
            if (!TampaOrGreater(host) && (productVersionCompare(HostProductVersion(host), "4.1.0") < 0))
            {
                return (HostBuildNumber(host) == 0x1a0a);
            }
            return true;
        }

        public static string OEMName(Host host)
        {
            if (!host.software_version.ContainsKey("oem_manufacturer"))
            {
                return "";
            }
            return host.software_version["oem_manufacturer"].ToLowerInvariant();
        }

        internal static bool OrderOfMagnitudeDifference(long MinSize, long MaxSize)
        {
            if ((MinSize < 0x400L) && (MaxSize < 0x400L))
            {
                return false;
            }
            if ((MinSize < 0x400L) && (MaxSize > 0x400L))
            {
                return true;
            }
            if ((MinSize > 0x400L) && (MaxSize < 0x400L))
            {
                return true;
            }
            if ((MinSize < 0x100000L) && (MaxSize < 0x100000L))
            {
                return false;
            }
            if ((MinSize < 0x100000L) && (MaxSize > 0x100000L))
            {
                return true;
            }
            if ((MinSize > 0x100000L) && (MaxSize < 0x100000L))
            {
                return true;
            }
            if ((MinSize < 0x40000000L) && (MaxSize < 0x40000000L))
            {
                return false;
            }
            return (((MinSize < 0x40000000L) && (MaxSize > 0x40000000L)) || ((MinSize > 0x40000000L) && (MaxSize < 0x40000000L)));
        }

        public static double ParseStringToDouble(string toParse, double defaultValue)
        {
            double num;
            if (!double.TryParse(toParse, NumberStyles.Any, (IFormatProvider) _nfi, out num))
            {
                num = defaultValue;
            }
            return num;
        }

        public static bool PoolHasEnabledHosts(Pool pool)
        {
            foreach (Host host in pool.Connection.Cache.Hosts)
            {
                if (host.enabled)
                {
                    return true;
                }
            }
            return false;
        }

        public static string PrettyFingerprint(string p)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < p.Length; i += 2)
            {
                list.Add(p.Substring(i, 2));
            }
            return string.Join(":", list.ToArray());
        }

        public static int productVersionCompare(string version1, string version2)
        {
            int num = 0x63;
            int num2 = 0x63;
            int num3 = 0x63;
            int num4 = 0x63;
            int num5 = 0x63;
            int num6 = 0x63;
            string[] strArray = null;
            if (version1 != null)
            {
                strArray = version1.Split(new char[] { '.' });
            }
            if ((strArray != null) && (strArray.Length == 3))
            {
                num = int.Parse(strArray[0]);
                num2 = int.Parse(strArray[1]);
                num3 = int.Parse(strArray[2]);
            }
            else if (!_unrecognisedVersionWarned)
            {
                log.DebugFormat("Unrecognised version format {0} - treating as developer version", version1);
                _unrecognisedVersionWarned = true;
            }
            strArray = null;
            if (version2 != null)
            {
                strArray = version2.Split(new char[] { '.' });
            }
            if ((strArray != null) && (strArray.Length == 3))
            {
                num4 = int.Parse(strArray[0]);
                num5 = int.Parse(strArray[1]);
                num6 = int.Parse(strArray[2]);
            }
            else if (!_unrecognisedVersionWarned)
            {
                log.DebugFormat("Unrecognised version format {0} - treating as developer version", version2);
                _unrecognisedVersionWarned = true;
            }
            if (num4 > num)
            {
                return -1;
            }
            if (num4 == num)
            {
                if (num5 > num2)
                {
                    return -1;
                }
                if (num5 == num2)
                {
                    if (num6 > num3)
                    {
                        return -1;
                    }
                    if (num3 == num6)
                    {
                        return 0;
                    }
                }
            }
            return 1;
        }

        public static void RemoveFromGuiConfig(Session session, IXenObject o, string key)
        {
            o.Do("remove_from_gui_config", new object[] { session, o.opaque_ref, key });
        }

        public static void RemoveFromOtherConfig(Session session, IXenObject o, string key)
        {
            o.Do("remove_from_other_config", new object[] { session, o.opaque_ref, key });
        }

        public static string RestartPriorityDescription(VM.HA_Restart_Priority? priority)
        {
            if (!priority.HasValue)
            {
                return "";
            }
            return (PropertyManager.GetFriendlyName("Description-VM.ha_restart_priority." + priority.ToString()) ?? priority.ToString());
        }

        public static string RestartPriorityI18n(VM.HA_Restart_Priority? priority)
        {
            if (!priority.HasValue)
            {
                return "";
            }
            return (PropertyManager.GetFriendlyName("Label-VM.ha_restart_priority." + priority.ToString()) ?? priority.ToString());
        }

        public static bool SameServerVersion(Host host, string productVersion, int buildNumber)
        {
            return ((host.ProductVersion == productVersion) && (host.BuildNumber == buildNumber));
        }

        public static bool SanibelOrGreater(IXenConnection conn)
        {
            if (conn != null)
            {
                return SanibelOrGreater(GetMaster(conn));
            }
            return true;
        }

        public static bool SanibelOrGreater(Host host)
        {
            if (!TampaOrGreater(host) && (productVersionCompare(HostProductVersion(host), "6.0.1") < 0))
            {
                return (HostBuildNumber(host) == 0x1a0a);
            }
            return true;
        }

        public static void SetGuiConfig(Session session, IXenObject o, string key, string value)
        {
            o.Do("remove_from_gui_config", new object[] { session, o.opaque_ref, key });
            o.Do("add_to_gui_config", new object[] { session, o.opaque_ref, key, value });
        }

        public static void SetOtherConfig(Session session, IXenObject o, string key, string value)
        {
            o.Do("remove_from_other_config", new object[] { session, o.opaque_ref, key });
            o.Do("add_to_other_config", new object[] { session, o.opaque_ref, key, value });
        }

        public static void ShuffleList<T>(List<T> listToShuffle)
        {
            Random random = new Random();
            for (int i = listToShuffle.Count - 1; i > 1; i--)
            {
                int num2 = random.Next(i);
                T local = listToShuffle[i];
                listToShuffle[i] = listToShuffle[num2];
                listToShuffle[num2] = local;
            }
        }

        
        public static string StringifyList<T>(List<T> list)
        {
            if (list == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                builder.Append(list[i].ToString());
                if (i < (list.Count - 2))
                {
                    builder.Append(Messages.STRINGIFY_LIST_INNERSEP);
                }
                else if (i == (list.Count - 2))
                {
                    builder.Append(Messages.STRINGIFY_LIST_LASTSEP);
                }
            }
            return builder.ToString();
        }

        public static double StringToDouble(string str)
        {
            if (str == "NaN")
            {
                return double.NaN;
            }
            if (str == "Infinity")
            {
                return double.PositiveInfinity;
            }
            if (str == "-Infinity")
            {
                return double.NegativeInfinity;
            }
            return Convert.ToDouble(str, CultureInfo.InvariantCulture);
        }

        public static bool SupportsLinkAggregationBond(IXenConnection connection)
        {
            Host master = GetMaster(connection);
            return (((master != null) && TampaOrGreater(master)) && master.vSwitchNetworkBackend);
        }

        public static bool TampaOrGreater(IXenConnection conn)
        {
            if (conn != null)
            {
                return TampaOrGreater(GetMaster(conn));
            }
            return true;
        }

        public static bool TampaOrGreater(Host host)
        {
            if (host == null)
            {
                return true;
            }
            string str = HostPlatformVersion(host);
            return (((str != null) && (productVersionCompare(str, "1.5.50") >= 0)) || (HostBuildNumber(host) == 0x1a0a));
        }

        public static string ToWindowsLineEndings(string input)
        {
            return Regex.Replace(input, "\r?\n", "\r\n");
        }

        public static string TrimStringIfRequired(string input, int maxLength)
        {
            if (input == null)
            {
                return "";
            }
            if (input.Length < maxLength)
            {
                return input;
            }
            return (input.Substring(0, maxLength - Messages.ELLIPSIS.Length) + Messages.ELLIPSIS);
        }

        public static bool ValidateIscsiIQN(string iqn)
        {
            return IqnRegex.IsMatch(iqn);
        }

        public static string VlanString(PIF pif)
        {
            if ((pif != null) && (pif.VLAN != -1L))
            {
                return pif.VLAN.ToString();
            }
            return Messages.SPACED_HYPHEN;
        }

        public static List<VM> VMsRunningOn(List<Host> hosts)
        {
            List<VM> list = new List<VM>();
            foreach (Host host in hosts)
            {
                list.AddRange(host.Connection.ResolveAll<VM>(host.resident_VMs));
            }
            return list;
        }

        public static bool WlbConfigured(IXenConnection conn)
        {
            Pool pool = GetPool(conn);
            return ((pool != null) && !string.IsNullOrEmpty(pool.wlb_url));
        }

        public static bool WlbEnabled(IXenConnection conn)
        {
            Pool pool = GetPool(conn);
            return (((pool != null) && pool.wlb_enabled) && WlbConfigured(conn));
        }

        public static IXenObject XenObjectFromMessage(Message message)
        {
            switch (message.cls)
            {
                case cls.VM:
                    {
                        VM vm = message.Connection.Cache.Find_By_Uuid<VM>(message.obj_uuid);
                        if (vm == null)
                        {
                            break;
                        }
                        return vm;
                    }
                case cls.Host:
                    {
                        Host host = message.Connection.Cache.Find_By_Uuid<Host>(message.obj_uuid);
                        if (host == null)
                        {
                            break;
                        }
                        return host;
                    }
                case cls.SR:
                    {
                        XenAPI.SR sr = message.Connection.Cache.Find_By_Uuid<XenAPI.SR>(message.obj_uuid);
                        if (sr == null)
                        {
                            break;
                        }
                        return sr;
                    }
                case cls.Pool:
                    {
                        Pool pool = message.Connection.Cache.Find_By_Uuid<Pool>(message.obj_uuid);
                        if (pool == null)
                        {
                            break;
                        }
                        return pool;
                    }
                case cls.VMPP:
                    {
                        VMPP vmpp = message.Connection.Cache.Find_By_Uuid<VMPP>(message.obj_uuid);
                        if (vmpp == null)
                        {
                            break;
                        }
                        return vmpp;
                    }
            }
            return null;
        }

        public static bool CommonCriteriaCertificationRelease
        {
            get
            {
                return false;
            }
        }

        private delegate string HostToStr(Host host);

        public static Host VMHome(VM vm)
        {
            if (vm.power_state == vm_power_state.Running)
            {
                return vm.Connection.Resolve<Host>(vm.resident_on);
            }
            Host storageHost = GetStorageHost(vm, vm.Connection);
            if (storageHost != null)
            {
                return storageHost;
            }
            return vm.Connection.Resolve(vm.affinity);
        }

        public static Host GetStorageHost(VM vm, IXenConnection connection)
        {
            return GetStorageHost(vm, connection, false);
        }

        public static Host GetStorageHost(VM vm, IXenConnection connection, bool ignoreCDs)
        {
            foreach (VBD vbd in connection.ResolveAll<VBD>(vm.VBDs))
            {
                if (!ignoreCDs || (vbd.type != vbd_type.CD))
                {
                    VDI vdi = connection.Resolve<VDI>(vbd.VDI);
                    if (vdi != null)
                    {
                        XenAPI.SR sr = connection.Resolve<XenAPI.SR>(vdi.SR);
                        if (sr != null)
                        {
                            Host storageHost = GetStorageHost(sr, connection);
                            if (storageHost != null)
                            {
                                return storageHost;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static Host GetStorageHost(SR sr, IXenConnection connection)
        {
            if (!sr.shared && (sr.PBDs.Count == 1))
            {
                PBD pbd = connection.Resolve<PBD>(sr.PBDs[0]);
                if (pbd != null)
                {
                    return connection.Resolve<Host>(pbd.host);
                }
            }
            return null;
        }

        public static bool IsVMShow(VM vm)
        {
            if (vm.name_label.StartsWith("__gui__"))
                return false;

            if (vm.is_control_domain)
                return false;

            if (vm.InternalTemplate)
                return false;

            return true;
        }

        public static bool IsCDROM(VBD vbd)
        {
            return (vbd.type == vbd_type.CD);
        }

        public static bool hostCanSeeNetwork(Host host, XenAPI.Network network)
        {
            Trace.Assert(host != null);
            Trace.Assert(network != null);
            if (network.PIFs.Count == 0)
            {
                return true;
            }
            foreach (PIF objPIF in network.Connection.ResolveAll<PIF>(network.PIFs))
            {
                if ((objPIF.host != null) && (objPIF.host.opaque_ref == host.opaque_ref))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool allHostsCanSeeNetwork(XenAPI.Network network)
        {
            foreach (Host objHost in network.Connection.Cache.Hosts)
            {
                if (!hostCanSeeNetwork(objHost, network))
                {
                    return false;
                }
            }
            return true;
        }

        public static Host GetHostFromUuid(IXenConnection xc, string p)
        {
            foreach (Host objHost in xc.Cache.Hosts)
            {
                if (objHost.uuid == p)
                {
                    return objHost;
                }
            }
            return null;
        }
    }
}

