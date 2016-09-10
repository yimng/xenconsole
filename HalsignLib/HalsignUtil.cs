using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using XenAdmin;
using HalsignModel;

namespace HalsignLib
{
    public class HalsignUtil
    {
        public const long BINARY_GIGA = 0x40000000L;
        public const long BINARY_KILO = 0x400L;
        public const long BINARY_MEGA = 0x100000L;
        public const long DEC_GIGA = 0x3b9aca00L;
        public const long DEC_KILO = 0x3e8L;
        public const long DEC_MEGA = 0xf4240L;
        public const ushort DEFAULT_ISCSI_PORT = 0xcbc;

        public static string DataRateString(long bitsPerSec)
        {
            return string.Format(Messages.VAL_FORMAT, DataRateValue(bitsPerSec), DataRateUnits(bitsPerSec));
        }

        public static string DataRateStringWithoutUnits(long bitsPerSec)
        {
            if (bitsPerSec >= 0x3b9aca00L)
            {
                return Math.Round((double) (((double) bitsPerSec) / 1000000000.0), 1).ToString();
            }
            if (bitsPerSec >= 0xf4240L)
            {
                return Math.Round((double) (((double) bitsPerSec) / 1000000.0), 1).ToString();
            }
            if (bitsPerSec >= 0x3e8L)
            {
                return Math.Round((double) (((double) bitsPerSec) / 1000.0), 1).ToString();
            }
            return bitsPerSec.ToString();
        }

        public static string DataRateUnits(long bitsPerSec)
        {
            if (bitsPerSec >= 0x3b9aca00L)
            {
                return Messages.VAL_GIGRATE;
            }
            if (bitsPerSec >= 0xf4240L)
            {
                return Messages.VAL_MEGRATE;
            }
            if (bitsPerSec >= 0x3e8L)
            {
                return Messages.VAL_KILRATE;
            }
            return Messages.VAL_RATE;
        }

        public static string DataRateValue(long bitsPerSec)
        {
            if (bitsPerSec >= 0x3b9aca00L)
            {
                return Math.Round((double) (((double) bitsPerSec) / 1000000000.0), 1).ToString(Messages.VAL_RATE_NUM_FORMAT);
            }
            if (bitsPerSec >= 0xf4240L)
            {
                return Math.Round((double) (((double) bitsPerSec) / 1000000.0), 1).ToString(Messages.VAL_RATE_NUM_FORMAT);
            }
            if (bitsPerSec > 0x3e8L)
            {
                return Math.Round((double) (((double) bitsPerSec) / 1000.0), 1).ToString(Messages.VAL_RATE_NUM_FORMAT);
            }
            return bitsPerSec.ToString();
        }

        public static string DiskSizeString(long bytes)
        {
            return DiskSizeString(bytes, 1);
        }

        public static string DiskSizeString(ulong bytes)
        {
            return DiskSizeString(bytes, 1);
        }

        public static string DiskSizeString(long bytes, int dp)
        {
            return DiskSizeString((ulong) Math.Abs(bytes), dp);
        }

        public static string DiskSizeString(ulong bytes, int dp)
        {
            if (bytes >= 0x40000000L)
            {
                return string.Format(Messages.VAL_GB, Math.Round((double) (((double) bytes) / 1073741824.0), dp));
            }
            if (bytes >= 0x100000L)
            {
                return string.Format(Messages.VAL_MB, Math.Round((double) (((double) bytes) / 1048576.0), dp));
            }
            if (bytes > 0x400L)
            {
                return string.Format(Messages.VAL_KB, Math.Round((double) (((double) bytes) / 1024.0), dp));
            }
            return string.Format(Messages.VAL_B, bytes);
        }

        public static string DiskSizeStringWithoutUnits(long bytes)
        {
            if (bytes >= 0x40000000L)
            {
                return Math.Round((double) (((double) bytes) / 1073741824.0), 1).ToString();
            }
            if (bytes >= 0x100000L)
            {
                return Math.Round((double) (((double) bytes) / 1048576.0), 1).ToString();
            }
            if (bytes >= 0x400L)
            {
                return Math.Round((double) (((double) bytes) / 1024.0), 1).ToString();
            }
            return bytes.ToString();
        }

        public static DateTime FromUnixTime(double time)
        {
            DateTime time2 = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return time2.AddSeconds(time);
        }

        public static string GetContentsOfValueNode(string xml)
        {
            ThrowIfStringParameterNullOrEmpty(xml, "xml");
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            foreach (XmlNode node in document.GetElementsByTagName("value"))
            {
                return node.InnerText;
            }
            return null;
        }

        public static List<T> GetList<T>(IEnumerable<T> input)
        {
            if (input == null)
            {
                return null;
            }
            List<T> list = input as List<T>;
            if (list != null)
            {
                return list;
            }
            return new List<T>(input);
        }

        public static string GetXmlNodeInnerText(XmlNode node, string xPath)
        {
            ThrowIfParameterNull(node, "node");
            ThrowIfStringParameterNullOrEmpty(xPath, "xPath");
            XmlNodeList list = node.SelectNodes(xPath);
            if ((list == null) || (list.Count == 0))
            {
                throw new InvalidOperationException("Node not found: " + xPath);
            }
            return list[0].InnerText;
        }

        internal static string GThanSize(long min)
        {
            return string.Format(Messages.GREATER_THAN, DiskSizeString(min));
        }

        internal static string GThanTime(long min)
        {
            return string.Format(Messages.GREATER_THAN, TimeString(min));
        }

        public static bool IsValidPort(string s)
        {
            int num;
            if (!int.TryParse(s, out num))
            {
                return false;
            }
            return ((0 < num) && (num <= 0xffff));
        }

        public static string LoadAverageString(long p)
        {
            return string.Format(Messages.VAL_FORMAT, LoadAverageStringWithoutUnits(p), Messages.LOADAVG_UNIT);
        }

        public static string LoadAverageStringWithoutUnits(long value)
        {
            double num = ((double) value) / 1000.0;
            return num.ToString(Messages.VAL_LOAD_NUM_FORMAT);
        }

        internal static string LThanSize(long max)
        {
            return string.Format(Messages.LESS_THAN, DiskSizeString(max));
        }

        internal static string LThanTime(long max)
        {
            return string.Format(Messages.LESS_THAN, TimeString(max));
        }

        public static string MemorySizeString(long bytes)
        {
            return string.Format(Messages.VAL_MB, Math.Round((double) (((double) bytes) / 1048576.0)).ToString());
        }

        public static string MemorySizeStringWithoutUnits(long bytes)
        {
            return Math.Round((double) (((double) bytes) / 1048576.0)).ToString();
        }

        public static string MemorySizeUnits(long bytes)
        {
            return Messages.VAL_MEGB;
        }

        public static string MilliSecondsString(long t)
        {
            return string.Format(Messages.VAL_FORMAT, MilliSecondsStringWithoutUnits(t), MilliSecondsUnits(t));
        }

        public static string MilliSecondsStringWithoutUnits(long t)
        {
            if (t <= 0x3e8L)
            {
                return t.ToString();
            }
            long num = t / 0x3e8L;
            return num.ToString();
        }

        public static string MilliSecondsUnits(long t)
        {
            if (t <= 0x3e8L)
            {
                return Messages.VAL_MILSEC;
            }
            return Messages.VAL_SEC;
        }

        public static string NanoSecondsString(long t)
        {
            return string.Format(Messages.VAL_FORMAT, NanoSecondsStringWithoutUnits(t), NanoSecondsUnits(t));
        }

        public static string NanoSecondsStringWithoutUnits(long t)
        {
            long num = (t > 0xf4240L) ? ((t > 0x3b9aca00L) ? (t / 0x3b9aca00L) : (t / 0xf4240L)) : (t / 0x3e8L);
            return num.ToString();
        }

        public static string NanoSecondsUnits(long t)
        {
            if (t <= 0xf4240L)
            {
                return Messages.VAL_MICSEC;
            }
            if (t <= 0x3b9aca00L)
            {
                return Messages.VAL_MILSEC;
            }
            return Messages.VAL_SEC;
        }

        public static string PercentageString(double TriggerLevel)
        {
            double num = TriggerLevel * 100.0;
            return string.Format("{0}%", num.ToString("0.0"));
        }

        public static List<T> PopulateList<T>(IEnumerable input)
        {
            ThrowIfParameterNull(input, "input");
            List<T> list = new List<T>();
            foreach (T local in input)
            {
                list.Add(local);
            }
            return list;
        }

        public static string SuperiorSizeString(long bytes, int dp)
        {
            if (bytes >= 0x280000000L)
            {
                return string.Format(Messages.VAL_GB, Math.Round((double) (((double) bytes) / 1073741824.0), dp).ToString());
            }
            if (bytes >= 0xa00000L)
            {
                return string.Format(Messages.VAL_MB, Math.Round((double) (((double) bytes) / 1048576.0), dp).ToString());
            }
            if (bytes > 0x2800L)
            {
                return string.Format(Messages.VAL_KB, Math.Round((double) (((double) bytes) / 1024.0), dp).ToString());
            }
            return string.Format(Messages.VAL_B, bytes);
        }

        private static void ThrowBecauseZeroLength(string name)
        {
            throw new ArgumentException(string.Format("{0} cannot have 0 length.", name), name);
        }

        public static void ThrowIfEnumerableParameterNullOrEmpty(IEnumerable value, string name)
        {
            ThrowIfParameterNull(value, name);
            //using (IEnumerator enumerator = value.GetEnumerator())
            //{
            //    while (enumerator.MoveNext())
            //    {
            //        object current = enumerator.Current;
            //        return;
            //    }
            //}
            IEnumerator enumerator = value.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                return;
            }
            ThrowBecauseZeroLength(name);
        }

        public static void ThrowIfParameterNull(object obj, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                ThrowBecauseZeroLength("name");
            }
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfStringParameterNullOrEmpty(string value, string name)
        {
            ThrowIfParameterNull(value, name);
            if (value.Length == 0)
            {
                ThrowBecauseZeroLength(name);
            }
        }

        public static string TimeRangeString(long t1, long t2)
        {
            if ((t1 > 60L) && (t2 > 60L))
            {
                return string.Format(Messages.TIME_RANGE_MINUTES, t1 / 60L, t2 / 60L);
            }
            return string.Format(Messages.TIME_RANGE_SECONDS, t1, t2);
        }

        public static string TimeString(long t)
        {
            if (t >= 120L)
            {
                return string.Format(Messages.TIME_MINUTES, t / 60L);
            }
            if (t > 0L)
            {
                return string.Format(Messages.TIME_SECONDS, t);
            }
            return Messages.TIME_NEGLIGIBLE;
        }

        public static long ToMB(long bytes)
        {
            return ToMB(bytes, RoundingBehaviour.Nearest);
        }

        public static long ToMB(long bytes, RoundingBehaviour rounding)
        {
            switch (rounding)
            {
                case RoundingBehaviour.Up:
                    return (long) Math.Ceiling((double) (((double) bytes) / 1048576.0));

                case RoundingBehaviour.Down:
                    return (long) Math.Floor((double) (((double) bytes) / 1048576.0));
            }
            return (long) Math.Round((double) (((double) bytes) / 1048576.0), MidpointRounding.AwayFromZero);
        }

        public static double ToUnixTime(DateTime time)
        {
            TimeSpan span = (TimeSpan) (time - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            return span.TotalSeconds;
        }

        public static string ToJson(BackupRestoreConfig.BrSchedule schedule)
        {
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(BackupRestoreConfig.BrSchedule));
            MemoryStream ms = new MemoryStream();
            ds.WriteObject(ms, schedule);
            string strReturn = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return strReturn;
        }

        public static string ToJson(BackupRestoreConfig.Job job)
        {
            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(BackupRestoreConfig.Job));
            MemoryStream ms = new MemoryStream();
            ds.WriteObject(ms, job);
            string strReturn = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return strReturn;
        }
        public static string ToJson(BackupRestoreConfig.BrSchedule schedule , String str)
        {  
            if (schedule.scheduleType == 0 || schedule.scheduleType == 1|| schedule.expect_full_count==0)
            {
                String rs = "{";
                String tempstr = str.Substring(1, str.Length - 2);
                String[] rulesParament = tempstr.Split(',');
                foreach (var rp in rulesParament)
                {
                    if (rp.IndexOf("current_full_count") > -1 || rp.IndexOf("expect_full_count") > -1 )
                    { }
                    else
                    {
                        rs += rp + ",";
                    }
                }
                str = rs.Substring(0, rs.Length - 1) + "}";
            }
            return str;
        }

        public static string ReplicationJsonFormat(String str)
        {
                String rs = "{";
                String tempstr = str.Substring(1, str.Length - 2);
                String[] rulesParament = tempstr.Split(',');
                foreach (var rp in rulesParament)
                {
                    if (rp.IndexOf("current_full_count") > -1 || rp.IndexOf("expect_full_count") > -1)
                    { }
                    else
                    {
                        rs += rp + ",";
                    }
                }
                return rs.Substring(0, rs.Length - 1) + "}";
        }

        public static string FormatSecond(long second, string format)
        {
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddSeconds(second);
            dt = dt.ToLocalTime();
            return dt.ToString(format);
        }

        public static Object JsonToObject(string strJson, Type type)
        {
            Object job;
            DataContractJsonSerializer ds = new DataContractJsonSerializer(type);
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJson));
            job = ds.ReadObject(ms);
            ms.Close();
            return job;
        }

        public static string SpeedString(long bytes)
        {
            return SpeedString(bytes, 1);
        }

        public static string SpeedString(long bytes, int dp)
        {
            if (bytes >= 0x100000L)
            {
                return string.Format(Messages.VAL_GB, Math.Round((double)(((double)bytes) / 1048576.0), dp).ToString());
            }
            if (bytes >= 0x400L)
            {
                return string.Format(Messages.VAL_MB, Math.Round((double)(((double)bytes) / 1024.0), dp).ToString());
            }
            return string.Format(Messages.VAL_KB, bytes);
        }

        public static string FormatSecond(long secondCount)
        {
            long second = secondCount % 60;
            long minute = ((secondCount - second) / 60) % 60;
            long hour = (secondCount - second) / 3600;
            return string.Format("{0}:{1}:{2}", hour.ToString("00"), minute.ToString("00"), second.ToString("00"));
        }
    }
}

