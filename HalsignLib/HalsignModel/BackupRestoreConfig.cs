using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HalsignModel
{
    public class BackupRestoreConfig
    {
        [DataContract]
        public class BrSchedule
        {
            [DataMember(Name = "jobName")]
            public string jobName { get; set; }
            [DataMember(Name = "scheduleType")]
            public int scheduleType { get; set; }
            [DataMember(Name = "scheduleDate")]
            public string scheduleDate { get; set; }
            [DataMember(Name = "scheduleTime")]
            public string scheduleTime { get; set; }
            [DataMember(Name = "recur")]
            public int recur { get; set; }
            [DataMember(Name = "weeklyDays")]
            public List<int> weeklyDays { get; set; }
            [DataMember(Name = "details")]
            public string details { get; set; }
            [DataMember(Name = "current_full_count")]
            public int current_full_count { get; set; }
            [DataMember(Name = "expect_full_count")]
            public int expect_full_count { get; set; }
        }

        [DataContract]
        public class RestoreJob
        {
            [DataMember(Name = "key")]
            public string key { get; set; }
            [DataMember(Name = "request")]
            public string request { get; set; }
            [DataMember(Name = "job_name")]
            public string job_name { get; set; }
            [DataMember(Name = "host")]
            public string host { get; set; }
            [DataMember(Name = "start_time")]
            public string start_time { get; set; }
            [DataMember(Name = "modify_time")]
            public string modify_time { get; set; }
            [DataMember(Name = "status")]
            public int status { get; set; }
            [DataMember(Name = "pid")]
            public int pid { get; set; }
            [DataMember(Name = "progress")]
            public int progress { get; set; }
            [DataMember(Name = "total_storage")]
            public int total_storage { get; set; }
            [DataMember(Name = "speed")]
            public string speed { get; set; }
            [DataMember(Name = "retry")]
            public int retry { get; set; }
        }

        [DataContract]
        public class Job
        {
            [DataMember(Name = "key")]
            public string key { get; set; }
            [DataMember(Name = "request")]
            public string request { get; set; }
            [DataMember(Name = "job_name")]
            public string job_name { get; set; }
            [DataMember(Name = "host")]
            public string host { get; set; }
            [DataMember(Name = "start_time")]
            public string start_time { get; set; }
            [DataMember(Name = "modify_time")]
            public string modify_time { get; set; }
            [DataMember(Name = "status")]
            public int status { get; set; }
            [DataMember(Name = "pid")]
            public int pid { get; set; }
            [DataMember(Name = "progress")]
            public int progress { get; set; }
            [DataMember(IsRequired = false)]
            public int total_storage { get; set; }
            [DataMember(Name = "speed")]
            public int speed { get; set; }
            [DataMember(Name = "left_time")]
            public int left_time { get; set; }
            [DataMember(Name = "retry")]
            public int retry { get; set; }
            [DataMember(Name = "current_full_count")]
            public int current_full_count { get; set; }
            [DataMember(Name = "expect_full_count")]
            public int expect_full_count { get; set; }
            [DataMember(Name = "schedule_type")]
            public int schedule_type { get; set; }
        }

        [DataContract]
        public class DRItemInfo
        {
            [DataMember(Name = "src_vm_uuid")]
            public string src_vm_uuid { get; set; }
            [DataMember(Name = "dest_vm_uuid")]
            public string dest_vm_uuid { get; set; }
            [DataMember(Name = "vm_name")]
            public string vm_name { get; set; }
            [DataMember(Name = "start_time")]
            public string start_time { get; set; }
            [DataMember(Name = "end_time")]
            public string end_time { get; set; }
            [DataMember(Name = "error_code")]
            public int error_code { get; set; }
            [DataMember(Name = "status")]
            public int status { get; set; }
            [DataMember(Name = "schedule_type")]
            public int schedule_type { get; set; }
            [DataMember(Name = "job_name")]
            public string job_name { get; set; }
            [DataMember(Name = "request_type")]
            public string request_type { get; set; }
        }

        [DataContract]
        public class DRResult
        {
            [DataMember(Name = "errorCode")]
            public string error_code { get; set; }
            [DataMember(Name = "items")]
            public List<DRItemInfo> items { get; set; }
            [DataMember(Name = "errorMessage")]
            public string error_message { get; set; }
        }

        [DataContract]
        public class IncrementalBackupItem
        {
            [DataMember(Name = "comment")]
            public string comment { get; set; }

            [DataMember(Name = "timestamp")]
            public int timestamp { get; set; }

            [DataMember(Name = "backup_no")]
            public int backup_no { get; set; }

            [DataMember(Name = "size")]
            public string size { get; set; }

            [DataMember(Name = "desc")]
            public string desc { get; set; }

            [DataMember(Name = "user")]
            public string user { get; set; }

            [DataMember(Name = "exportable")]
            public bool exportable { get; set; }

            [DataMember(Name = "uuid")]
            public string uuid { get; set; }

            [DataMember(Name = "parentuuid")]
            public string parentuuid { get; set; }

            [DataMember(Name = "name")]
            public string name { get; set; }

            [DataMember(Name = "osversion")]
            public string osversion { get; set; }

            [DataMember(Name = "time_str")]
            public string time_str { get; set; }

            [DataMember(Name = "root_path")]
            public string root_path { get; set; }

            [DataMember(Name = "content")]
            public string content { get; set; }
        }

        [DataContract]
        public class FullBackupItem
        {
            [DataMember(Name = "comment")]
            public string comment { get; set; }

            [DataMember(Name = "timestamp")]
            public int timestamp { get; set; }

            [DataMember(Name = "backup_no")]
            public int backup_no { get; set; }

            [DataMember(Name = "size")]
            public string size { get; set; }

            [DataMember(Name = "desc")]
            public string desc { get; set; }

            [DataMember(Name = "user")]
            public string user { get; set; }

            [DataMember(Name = "exportable")]
            public bool exportable { get; set; }

            [DataMember(Name = "uuid")]
            public string uuid { get; set; }

            [DataMember(Name = "parentuuid")]
            public string parentuuid { get; set; }

            [DataMember(Name = "name")]
            public string name { get; set; }

            [DataMember(Name = "osversion")]
            public string osversion { get; set; }

            [DataMember(Name = "time_str")]
            public string time_str { get; set; }

            [DataMember(Name = "root_path")]
            public string root_path { get; set; }

            [DataMember(Name = "content")]
            public string content { get; set; }

            [DataMember(Name = "children")]
            public List<IncrementalBackupItem> children { get; set; } 
        }

        [DataContract]
        public class RestoreListItem
        {
            [DataMember(Name = "comment")]
            public string comment { get; set; }

            [DataMember(Name = "timestamp")]
            public int timestamp { get; set; }

            [DataMember(Name = "backup_no")]
            public int backup_no { get; set; }

            [DataMember(Name = "size")]
            public string size { get; set; }

            [DataMember(Name = "desc")]
            public string desc { get; set; }

            [DataMember(Name = "user")]
            public string user { get; set; }

            [DataMember(Name = "exportable")]
            public bool exportable { get; set; }

            [DataMember(Name = "uuid")]
            public string uuid { get; set; }

            [DataMember(Name = "parentuuid")]
            public string parentuuid { get; set; }

            [DataMember(Name = "name")]
            public string name { get; set; }

            [DataMember(Name = "osversion")]
            public string osversion { get; set; }

            [DataMember(Name = "time_str")]
            public string time_str { get; set; }

            [DataMember(Name = "root_path")]
            public string root_path { get; set; }

            [DataMember(Name = "children")]
            public List<FullBackupItem> children { get; set; }
        }

        [DataContract]
        public class RestoreListInfo
        {
            [DataMember(Name = "errorCode")]
            public int error_code { get; set; }

            [DataMember(Name = "items")]
            public List<RestoreListItem> items { get; set; }

            [DataMember(Name = "errorMessage")]
            public string error_message { get; set; }
        }

        public class ResultInfo
        {
            public string vm_uuid { get; set; }
            public string vm_name { get; set; }
            public bool is_complete { get; set; }
            public List<string> content_list { get; set; }
            public Dictionary<string, string> backup_info_list { get; set; }
            public Dictionary<string, string> vm_info_list { get; set; }
            public string item_content { get; set; }
            public string comp_uuid { get; set; }
        }

        public class RestoreInfo
        {
            public string name { get; set; }
            public string storage { get; set; }
            public string uuid { get; set; }
        }

        public enum ERROR_CODE
        {
            ERR_SR_INFO = 1001,
            ERR_SR_CHKZFS = 1002,
            ERR_SR_ZFS_SNAP = 1003,
            ERR_XEN_LOGIN = 2001,
            ERR_XEN_GET_VM_BY_UUID = 2002,
            ERR_XEN_GET_VM_REC = 2003,
            ERR_XEN_UPDATE_OTH_CFG = 2004,
            ERR_XEN_GET_VIFS = 2005,
            ERR_XEN_GET_VIF = 2006,
            ERR_XEN_GET_VIF_REC = 2007,
            ERR_XEN_GET_NETWORK = 2008,
            ERR_XEN_GET_NETWORK_REC = 2009,
            ERR_XEN_GET_VBDS = 2010,
            ERR_XEN_GET_VBD = 2011,
            ERR_XEN_GET_VBD_REC = 2012,
            ERR_XEN_GET_VDI = 2013,
            ERR_XEN_GET_VDI_REC = 2014,
            ERR_XEN_GET_SR = 2015,
            ERR_XEN_GET_SR_REC = 2016,
            ERR_XEN_GET_POOLS = 2017,
            ERR_XEN_NEW_VM = 2018,
            ERR_XEN_NEW_VIF = 2019,
            ERR_XEN_NEW_VDI = 2020,
            ERR_XEN_NEW_VBD = 2021,
            ERR_AGT_NEW_JOB = 3001,
            ERR_OTH_MAKE_DIR = 4001,
            ERR_OTH_MAKE_FILE = 4002,
            ERR_OTH_CHANGE_DIR = 4003,
            ERR_OTH_RSYNC = 4004,
            ERR_OTH_UNKNOWN = 4005,
            ERR_OTH_REPLICATION_VM_RUN = 4007,
            ERR_XEN_NEW_SNAP = 2022,
            ERR_XEN_DEL_SNAP = 2023,
            ERR_XEN_DEL_VDI_SNAP = 2024,
            ERR_XEN_REMOTE_LOGIN = 2025,
            ERR_XEN_DISK_CONFLICT = 2026
        }

        public readonly static string BACKUP_IMMEDIATELY = "i";
        public readonly static string BACKUP_ONCE = "s";
        public readonly static string BACKUP_DAILY = "t";
        public readonly static string BACKUP_WEEKLY = "w";
        public readonly static string BACKUP_CIRCLE = "h";
        public readonly static string RESTORE_NOW = "r";
        public readonly static string RESTORE_ONCE = "o";
        public readonly static string REPLICATION_IMMEDIATELY = "f";
        public readonly static string REPLICATION_SYNCH = "g";
        public readonly static string REPLICATION_ONCE = "y";
        public readonly static string REPLICATION_DAILY = "a";
        public readonly static string REPLICATION_WEEKLY = "z";
        public readonly static string REPLICATION_CIRCLE = "u";
        public readonly static string PUBLISH = "j";
        public readonly static string FULL_BACKUP = "b";
        public readonly static string FULL_BACKUP_ONCE = "q";

        public enum JOB_STATUS
        {
            JOB_STATUS_INACTIVE = 0,
            JOB_STATUS_INQUEUE = 1,
            JOB_STATUS_RUNNING = 2,
            JOB_STATUS_PENDING = 3,
            JOB_STATUS_SUCCESS = 4,
            JOB_STATUS_FAILED = 5,
            JOB_STATUS_CANCELED = 6,
            JOB_STATUS_CHK_ZFS = 7,
            JOB_STATUS_GEN_METADATA = 8,
            JOB_STATUS_TRANS_METADATA = 9,
            JOB_STATUS_TRANS_DATA = 10,
            JOB_STATUS_VERIFY_DATA = 11,
            JOB_STATUS_ZFS_SNAP = 12,
            JOB_STATUS_DELETE_SNAP = 13,
            JOB_STATUS_CANCELING = 16
        }

        public enum BackupActionKind
        {
            Backup,
            Restore,
            Replication,
            RestoreFileList,
            Histroy,
            Job_Cancel,
            Publish,
            IPList
        }

    }
}
