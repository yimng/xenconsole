using HalsignLib;

namespace XenAdmin.Actions.BRActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using log4net;
    using System.Reflection;
    using XenAPI;
    using System.Threading;
    using System.Net;
    using System.IO;
    using System.Xml;
    using System.Web;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;
    using System.Windows.Forms;
    using HalsignModel;
    using XenAdmin.Core;

    public class BackupAction : AsyncAction
    {
        private Dictionary<string, string> _dconf;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IXenObject xenModelObject;
        private readonly BackupRestoreConfig.BackupActionKind kind;
        private readonly List<VM> vmCheckedList;
        private readonly List<string> restoreAgentParams;
        private readonly List<Dictionary<string, string>> listSchedule;
        private readonly List<Dictionary<string, List<string>>> listScheduleTemp;
        private readonly IXenObject deletedObject;
        private Dictionary<string, string> _vdiconf;
        private Dictionary<string, string> _vdi_expand_list;
        private List<BackupRestoreConfig.DRResult> _histotyresultList;
        private BackupRestoreConfig.RestoreListInfo restoreList;

        public BackupAction(string _show_message, BackupRestoreConfig.BackupActionKind kind, IXenObject _xenModelObject, List<VM> _vmCheckedList, Dictionary<string, string> args, Dictionary<string, string> vargs)
            : base(_xenModelObject.Connection, GetTitle(kind, _xenModelObject), _show_message)
        {
            this.xenModelObject = _xenModelObject;
            if (_xenModelObject is VM)
            {
                base.VM = _xenModelObject as VM;
                base.Host = HalsignHelpers.VMHome(base.VM);
            }
            else if (_xenModelObject is Host)
            {
                base.Host = _xenModelObject as Host;
            }
            this.kind = kind;
            this.vmCheckedList = _vmCheckedList;
            this._dconf = args;
            this._vdiconf = vargs;
            this._histotyresultList = new List<BackupRestoreConfig.DRResult>();
            this.restoreList = new BackupRestoreConfig.RestoreListInfo();
        }

        public BackupAction(string _show_message, BackupRestoreConfig.BackupActionKind kind, IXenObject _xenModelObject, Dictionary<string, string> args)
            : base(_xenModelObject.Connection, GetTitle(kind, _xenModelObject), _show_message)
        {
            //base.CanCancel = true;
            this.xenModelObject = _xenModelObject;
            if (_xenModelObject is VM)
            {
                base.VM = _xenModelObject as VM;
                base.Host = HalsignHelpers.VMHome(base.VM);
            }
            else if (_xenModelObject is Host)
            {
                base.Host = _xenModelObject as Host;
            }
            else if (_xenModelObject is Pool)
            {
                base.Host = Helpers.GetMaster(_xenModelObject.Connection);
            }
            this.kind = kind;
            this._dconf = args;
            this._histotyresultList = new List<BackupRestoreConfig.DRResult>();
            this.restoreList = new BackupRestoreConfig.RestoreListInfo();
            this.CanCancel = true;
        }

        public BackupAction(string _show_message, BackupRestoreConfig.BackupActionKind kind, IXenObject _xenModelObject, List<string> _restoreAgentParams, List<Dictionary<string, string>> _listSchedule)
            : base(_xenModelObject.Connection, GetTitle(kind, _xenModelObject), _show_message)
        {
            this.xenModelObject = _xenModelObject;
            if (_xenModelObject is VM)
            {
                base.VM = _xenModelObject as VM;
                base.Host = HalsignHelpers.VMHome(base.VM);
            }
            else if (_xenModelObject is Host)
            {
                base.Host = _xenModelObject as Host;
            }
            this.kind = kind;
            this.listSchedule = _listSchedule;
            this._histotyresultList = new List<BackupRestoreConfig.DRResult>();
            this.restoreList = new BackupRestoreConfig.RestoreListInfo();
            this.restoreAgentParams = _restoreAgentParams;
        }

        public BackupAction(string _show_message, BackupRestoreConfig.BackupActionKind kind, IXenObject _xenModelObject)
            : base(_xenModelObject.Connection, GetTitle(kind, _xenModelObject), _show_message)
        {
            this.xenModelObject = _xenModelObject;
            if (_xenModelObject is VM)
            {
                base.VM = _xenModelObject as VM;
                base.Host = HalsignHelpers.VMHome(base.VM);
            }
            else if (_xenModelObject is Host)
            {
                base.Host = _xenModelObject as Host;
            }
            else if (_xenModelObject is Pool)
            {
                base.Pool = _xenModelObject as Pool;
            }
            this.kind = kind;
            this._histotyresultList = new List<BackupRestoreConfig.DRResult>();
            this.restoreList = new BackupRestoreConfig.RestoreListInfo();
            this.CanCancel = true;
        }

        public BackupAction(string _show_message, BackupRestoreConfig.BackupActionKind kind, IXenObject _xenModelObject, List<VM> _vmCheckedList, Dictionary<string, string> args, List<Dictionary<string, string>> _listSchedule)
            : base(_xenModelObject.Connection, GetTitle(kind, _xenModelObject), _show_message)
        {
            this.xenModelObject = _xenModelObject;
            if (_xenModelObject is VM)
            {
                base.VM = _xenModelObject as VM;
                base.Host = HalsignHelpers.VMHome(base.VM);
            }
            else if (_xenModelObject is Host)
            {
                base.Host = _xenModelObject as Host;
            }
            this.kind = kind;
            this.vmCheckedList = _vmCheckedList;
            this._dconf = args;
            this.listSchedule = _listSchedule;
            this._histotyresultList = new List<BackupRestoreConfig.DRResult>();
            this.restoreList = new BackupRestoreConfig.RestoreListInfo();
        }

        public BackupAction(string _show_message, BackupRestoreConfig.BackupActionKind kind, IXenObject _xenModelObject, List<VM> _vmCheckedList, Dictionary<string, string> args, List<Dictionary<string, List<string>>> _listSchedule, Dictionary<string, string> vdi_expand_list)
            : base(_xenModelObject.Connection, GetTitle(kind, _xenModelObject), _show_message)
        {
            this.xenModelObject = _xenModelObject;
            if (_xenModelObject is VM)
            {
                base.VM = _xenModelObject as VM;
                base.Host = HalsignHelpers.VMHome(base.VM);
            }
            else if (_xenModelObject is Host)
            {
                base.Host = _xenModelObject as Host;
            }
            this.kind = kind;
            this.vmCheckedList = _vmCheckedList;
            this._dconf = args;
            this.listScheduleTemp = _listSchedule;
            this._vdi_expand_list = vdi_expand_list;
            this._histotyresultList = new List<BackupRestoreConfig.DRResult>();
            this.restoreList = new BackupRestoreConfig.RestoreListInfo();
        }

        public BackupAction(string _show_message, BackupRestoreConfig.BackupActionKind kind, IXenObject _xenModelObject, Dictionary<string, string> args, IXenObject _deletedObject)
            : base(_xenModelObject.Connection, GetTitle(kind, _xenModelObject), _show_message)
        {
            this.xenModelObject = _xenModelObject;
            if (_xenModelObject is VM)
            {
                base.VM = _xenModelObject as VM;
                base.Host = HalsignHelpers.VMHome(base.VM);
            }
            else if (_xenModelObject is Host)
            {
                base.Host = _xenModelObject as Host;
            }
            else if (_xenModelObject is Pool)
            {
                base.Host = Helpers.GetMaster(_xenModelObject.Connection);
            }
            this.kind = kind;
            this._dconf = args;
            this.deletedObject = _deletedObject;
            this._histotyresultList = new List<BackupRestoreConfig.DRResult>();
            this.restoreList = new BackupRestoreConfig.RestoreListInfo();
            this.CanCancel = true;
        }

        private static string GetTitle(BackupRestoreConfig.BackupActionKind kind, IXenObject xenObject)
        {
            string connnectionName = Helpers.GetName(xenObject.Connection);

            switch (kind)
            {
                case BackupRestoreConfig.BackupActionKind.Backup:
                    return string.Format(Messages.BACKUP_VM_TITLE, connnectionName);
                case BackupRestoreConfig.BackupActionKind.Restore:
                    return string.Format(Messages.RESTORE_VM_TITLE, connnectionName);
                case BackupRestoreConfig.BackupActionKind.Replication:
                    return string.Format(Messages.REPLICATION_VM_TITLE,connnectionName);
                case BackupRestoreConfig.BackupActionKind.RestoreFileList:
                    return string.Format(Messages.GET_RESTORE_LIST, connnectionName);
                case BackupRestoreConfig.BackupActionKind.Histroy:
                    return Messages.GET_BR_HISTORY;
                case BackupRestoreConfig.BackupActionKind.Job_Cancel:
                    return Messages.BR_JOB_CANCEL;
                case BackupRestoreConfig.BackupActionKind.Publish:
                    return string.Format(Messages.VTOP_PUBLISH, connnectionName);
                default:
                    return "";
            }
        }

        protected override void Run()
        {
            bool isActionSucceed = true;
            try
            {
                switch (this.kind)
                {
                    case BackupRestoreConfig.BackupActionKind.Backup:
                        foreach (VM vmTemp in this.vmCheckedList)
                        {
                            if (this._dconf.ContainsKey("agent_param"))
                            {
                                this._dconf.Remove("agent_param");
                            }
                            string job_expend = null;
                            if (this._vdiconf.ContainsKey(vmTemp.uuid))
                            {
                                job_expend = this._dconf["job_name"] + "|" + vmTemp.uuid + "|" + _vdiconf[vmTemp.uuid];
                                int vdiNo = 0;
                                foreach (VBD vbd in vmTemp.Connection.ResolveAll<VBD>(vmTemp.VBDs))
                                {
                                    if (HalsignHelpers.IsCDROM(vbd))
                                    {
                                        continue;
                                    }

                                    if (vbd.type.Equals(XenAPI.vbd_type.Disk))
                                    {
                                        vdiNo++;
                                    }
                                }
                                if (_vdiconf[vmTemp.uuid].Split('@').Length == vdiNo)
                                {
                                    job_expend = this._dconf["job_name"] + "|" + vmTemp.uuid + "|" + "halsign_vdi_all";
                                }

                                if (this._dconf.ContainsKey("command"))
                                {
                                    string backupOption = this._dconf["command"];
                                    backupOption = backupOption.Substring(backupOption.Length - 1); 
                                    this._dconf.Remove("command");
                                    if ((getStringLength(job_expend) + 1) < 100)
                                    {
                                        this._dconf.Add("command", "00" + (getStringLength(job_expend) + 1) + backupOption);
                                    }
                                    else if ((getStringLength(job_expend) + 1) > 99 && (getStringLength(job_expend) + 1) < 1000)
                                    {
                                        this._dconf.Add("command", "0" + (getStringLength(job_expend) + 1) + backupOption);
                                    }
                                    else if ((getStringLength(job_expend) + 1) > 999 && (getStringLength(job_expend) + 1) < 10000)
                                    {
                                        this._dconf.Add("command", "" + (getStringLength(job_expend) + 1) + backupOption);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                this._dconf.Add("agent_param", job_expend);
                            }
                            else
                            {
                                continue;
                            }
                            if ("False".Equals(this._dconf["is_now"]))
                            {
                                if (vmTemp.other_config.ContainsKey("halsign_br_rules"))
                                {
                                    BackupRestoreConfig.BrSchedule schedule = (BackupRestoreConfig.BrSchedule)HalsignUtil.JsonToObject(this._dconf["backup_rules"], typeof(BackupRestoreConfig.BrSchedule));
                                    BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(this._dconf["backup_job"], typeof(BackupRestoreConfig.Job));
                                    if (!job_expend.Contains("halsign_vdi_all"))
                                    {
                                        schedule.details = _vdiconf[vmTemp.uuid];
                                        job.request = job.request + vmTemp.uuid + "|" + _vdiconf[vmTemp.uuid];
                                    }
                                    else
                                    {
                                        schedule.details = "halsign_vdi_all";
                                        job.request = job.request + vmTemp.uuid + "|" + "halsign_vdi_all";
                                    }
                                    String str_rules = HalsignUtil.ToJson(schedule);
                                    String str_jobs = HalsignUtil.ToJson(job);
                                    String set_rules = HalsignUtil.ToJson(schedule, str_rules);
                                    String set_jobs = HalsignUtil.ToJson(schedule, str_jobs);

                                    this._dconf.Remove("backup_rules");
                                    this._dconf.Add("backup_rules", set_rules);//str_rules);

                                    vmTemp.other_config["halsign_br_rules"] = this._dconf["backup_rules"];

                                    XenAPI.VM.set_other_config(base.Session, vmTemp.opaque_ref, vmTemp.other_config);
                                    if (vmTemp.other_config.ContainsKey("halsign_br_job_s"))
                                    {
                                        vmTemp.other_config["halsign_br_job_s"] = set_jobs;//HalsignUtil.ToJson(job);
                                        XenAPI.VM.set_other_config(base.Session, vmTemp.opaque_ref, vmTemp.other_config);
                                    }
                                    else
                                    {
                                        XenAPI.VM.set_other_config(base.Session, vmTemp.opaque_ref, vmTemp.other_config);
                                        XenAPI.VM.add_to_other_config(base.Session, vmTemp.opaque_ref, "halsign_br_job_s", set_jobs);//HalsignUtil.ToJson(job));
                                    }
                                    //XenAPI.VM.add_to_other_config(base.Session, vmTemp.ServerOpaqueRef, "halsign_br_job_s", Util.ToJson(job))
                                }
                                else
                                {
                                    BackupRestoreConfig.BrSchedule schedule = (BackupRestoreConfig.BrSchedule)HalsignUtil.JsonToObject(this._dconf["backup_rules"], typeof(BackupRestoreConfig.BrSchedule));
                                    BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(this._dconf["backup_job"], typeof(BackupRestoreConfig.Job));
                                    if (!job_expend.Contains("halsign_vdi_all"))
                                    {
                                        schedule.details = _vdiconf[vmTemp.uuid];
                                        job.request = job.request + vmTemp.uuid + "|" + _vdiconf[vmTemp.uuid];
                                    }
                                    else
                                    {
                                        schedule.details = "halsign_vdi_all";
                                        job.request = job.request + vmTemp.uuid + "|" + "halsign_vdi_all";
                                    }
                                    String str_rules = HalsignUtil.ToJson(schedule);
                                    if (this._dconf.ContainsKey("backup_rules"))
                                    {
                                        this._dconf.Remove("backup_rules");
                                    }
                                    String str_jobs = HalsignUtil.ToJson(job);
                                    String set_rules = HalsignUtil.ToJson(schedule, str_rules);
                                    String set_jobs = HalsignUtil.ToJson(schedule, str_jobs);

                                    this._dconf.Add("backup_rules", set_rules);//str_rules);
                                    //vmTemp.Server.other_config["halsign_br_rules"] = this._dconf["backup_rules"];                                               

                                    if (vmTemp.other_config.ContainsKey("halsign_br_job_s"))
                                    {
                                        vmTemp.other_config["halsign_br_job_s"] = set_jobs; //HalsignUtil.ToJson(job);
                                        XenAPI.VM.set_other_config(base.Session, vmTemp.opaque_ref, vmTemp.other_config);
                                    }
                                    else
                                    {
                                        XenAPI.VM.set_other_config(base.Session, vmTemp.opaque_ref, vmTemp.other_config);
                                        XenAPI.VM.add_to_other_config(base.Session, vmTemp.opaque_ref, "halsign_br_job_s", set_jobs);//HalsignUtil.ToJson(job));
                                    }
                                    XenAPI.VM.add_to_other_config(base.Session, vmTemp.opaque_ref, "halsign_br_rules", this._dconf["backup_rules"]);
                                    //XenAPI.VM.add_to_other_config(base.Session, vmTemp.ServerOpaqueRef, "halsign_br_rules", this._dconf["backup_rules"]);                                                                        
                                    //XenAPI.VM.add_to_other_config(base.Session, vmTemp.ServerOpaqueRef, "halsign_br_job_s", Util.ToJson(job));
                                }
                            }

                            /*
                             * Check whether the vm can be handled by the current agent.
                             * 1) Check the vm's affinity host, if the affinity host is not empty, that host will be the target host.
                             * 2) If the host is slave, the host uuid must be the same with the target host uuid.
                             * 3) if the host is master, the host uuid can be the same with the target host uuid,
                             * or the target host is down and the master host it one of the VM's possible host.
                             * 4) Sometimes the VM's affinity is empty, then we have to use one of the possible host as its target host.
                             */
                            if ("True".Equals(this._dconf["is_now"]))
                            {

                                BackupRestoreConfig.BrSchedule schedule = (BackupRestoreConfig.BrSchedule)HalsignUtil.JsonToObject(this._dconf["backup_rules"], typeof(BackupRestoreConfig.BrSchedule));
                                String set_rules = HalsignUtil.ToJson(schedule, this._dconf["backup_rules"]);
                                if (this._dconf.ContainsKey("backup_rules"))
                                {
                                    this._dconf.Remove("backup_rules");
                                }
                                this._dconf.Add("backup_rules", set_rules);
                                if (vmTemp.IsRunning)
                                {
                                    Host host = base.Connection.Resolve<Host>(vmTemp.resident_on);
                                    base.Result = XenAPI.Host.call_plugin(base.Session, host.opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                                } else
                                {
                                    Host _host_affinity = base.Connection.Resolve<Host>(vmTemp.affinity);
                                    List<XenRef<Host>> _list_host = XenAPI.VM.get_possible_hosts(base.Session, vmTemp.opaque_ref);
                                    if (_host_affinity != null && _host_affinity.IsLive)
                                    {
                                        base.Result = XenAPI.Host.call_plugin(base.Session, _host_affinity.opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                                    }
                                    else if (_list_host != null && _list_host.Count > 0 && base.Connection.Resolve<Host>(_list_host[0]).IsLive)
                                    {
                                        base.Result = XenAPI.Host.call_plugin(base.Session, _list_host[0].opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                                    }
                                    else if (_list_host == null || _list_host.Count <= 0)
                                    {
                                        base.Result = XenAPI.Host.call_plugin(base.Session, HalsignHelpers.VMHome(vmTemp).opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                                    }
                                    else
                                    {
                                        bool have_master = false;
                                        foreach (XenRef<Host> item in _list_host)
                                        {
                                            if (item != null && item.opaque_ref == Helpers.GetMaster(base.Connection).opaque_ref)
                                            {
                                                have_master = true;
                                            }
                                        }
                                        if (have_master)
                                        {
                                            base.Result = XenAPI.Host.call_plugin(base.Session, Helpers.GetMaster(base.Connection).opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                                        }
                                        else
                                        {
                                            throw new Exception(Messages.BR_CALL_AGENT_FROM_MASTER);
                                        }
                                    }
                                }
                                
                                if (!string.IsNullOrEmpty(base.Result))
                                {
                                    throw new Exception(Messages.BR_CALL_AGENT_EXCEPTION + base.Result);
                                }
                            }                            
                        }
                        break;
                    case BackupRestoreConfig.BackupActionKind.Restore:
                        if (this.listSchedule != null && listSchedule.Count > 0)
                        {
                            Pool pool = Helpers.GetPoolOfOne(base.Connection);
                            if (pool == null)
                            {
                                throw new Exception(Messages.BR_NO_POOL_EXCEPTION);
                            }
                            foreach (Dictionary<string, string> dconfTemp in this.listSchedule)
                            {
                                XenAPI.Pool.add_to_other_config(base.Session, pool.opaque_ref, dconfTemp["config_name"], dconfTemp["schedule"]);
                            }
                        }
                        else
                        {
                            this._dconf = new Dictionary<string, string>();
                            this._dconf.Add("command", "r");
                            foreach (string param in this.restoreAgentParams)
                            {
                                if (this._dconf.ContainsKey("agent_param"))
                                {
                                    this._dconf.Remove("agent_param");
                                }
                                if (this._dconf.ContainsKey("command"))
                                {
                                    this._dconf.Remove("command");
                                }
                                if ((getStringLength(param) + 1) < 100)
                                {
                                    this._dconf.Add("command", "00" + (getStringLength(param) + 1) + "r");
                                }
                                else if ((getStringLength(param) + 1) > 99 && (getStringLength(param) + 1) < 1000)
                                {
                                    this._dconf.Add("command", "0" + (getStringLength(param) + 1) + "r");
                                }
                                else if ((getStringLength(param) + 1) > 999 && (getStringLength(param) + 1) < 10000)
                                {
                                    this._dconf.Add("command", "" + (getStringLength(param) + 1) + "r");
                                }
                                else
                                {
                                    continue;
                                }

                                this._dconf.Add("agent_param", param);
                                base.Result = XenAPI.Host.call_plugin(base.Session, base.Host.opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                                if (!string.IsNullOrEmpty(base.Result))
                                {
                                    throw new Exception(Messages.BR_CALL_AGENT_EXCEPTION + base.Result);
                                }
                            }
                        }
                        break;
                    case BackupRestoreConfig.BackupActionKind.RestoreFileList:
                        ParseRestoreList();
                        if (base.Cancelling)
                        {
                            throw new CancelledException();
                        }
                        break;
                    case BackupRestoreConfig.BackupActionKind.Histroy:
                        if (this.xenModelObject is VM)
                        {
                            VM vm = this.xenModelObject as VM;
                            if (vm.other_config.ContainsKey("halsign_br_username") && vm.other_config.ContainsKey("halsign_br_password"))
                            {
                                queryHistory(vm.uuid, vm.other_config["halsign_br_ip_address"], vm.other_config["halsign_br_username"], vm.other_config["halsign_br_password"]);
                            }
                            else
                            {
                                Pool pool = Helpers.GetPool(vm.Connection);
                                if (pool != null && pool.other_config.ContainsKey("halsign_br_username") && pool.other_config.ContainsKey("halsign_br_password"))
                                {
                                    queryHistory(vm.uuid, pool.other_config["halsign_br_ip_address"], pool.other_config["halsign_br_username"], pool.other_config["halsign_br_password"]);
                                }
                            }
                        }
                        else if (this.xenModelObject is Pool)
                        {
                            Pool pool = this.xenModelObject as Pool;
                            VM[] vMs = this.xenModelObject.Connection.Cache.VMs.Where(vm => vm.uuid != null && vm.is_a_real_vm).ToArray();
                            if (pool != null && pool.other_config.ContainsKey("halsign_br_username") && pool.other_config.ContainsKey("halsign_br_password"))
                            {
                                foreach (VM vmTemp in vMs)
                                {
                                    if (!vmTemp.is_a_snapshot && !vmTemp.is_a_template && HalsignHelpers.IsVMShow(vmTemp))
                                    {
                                        if (vmTemp.other_config.ContainsKey("halsign_br_username") && vmTemp.other_config.ContainsKey("halsign_br_password"))
                                        {
                                            queryHistory(vmTemp.uuid, vmTemp.other_config["halsign_br_ip_address"], vmTemp.other_config["halsign_br_username"], vmTemp.other_config["halsign_br_password"]);
                                        }
                                        else
                                        {
                                            queryHistory(vmTemp.uuid, pool.other_config["halsign_br_ip_address"], pool.other_config["halsign_br_username"], pool.other_config["halsign_br_password"]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (VM vmTemp in vMs)
                                {
                                    if (!vmTemp.is_a_snapshot && !vmTemp.is_a_template && HalsignHelpers.IsVMShow(vmTemp))
                                    {
                                        if (vmTemp.other_config.ContainsKey("halsign_br_username") && vmTemp.other_config.ContainsKey("halsign_br_password"))
                                        {
                                            queryHistory(vmTemp.uuid, vmTemp.other_config["halsign_br_ip_address"], vmTemp.other_config["halsign_br_username"], vmTemp.other_config["halsign_br_password"]);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case BackupRestoreConfig.BackupActionKind.Job_Cancel:
                        int status = Int32.Parse(this._dconf["status"]);
                        string job_key = this._dconf["job_key"];
                        string temp_param = this._dconf["agent_param"];
                        if (this._dconf.ContainsKey("agent_param"))
                        {
                            this._dconf.Remove("agent_param");
                        }
                        if (this._dconf.ContainsKey("command"))
                        {
                            this._dconf.Remove("command");
                        }
                        if ((getStringLength(temp_param) + 1) > 0 && (getStringLength(temp_param) + 1) < 10)
                        {
                            this._dconf.Add("command", "000" + (getStringLength(temp_param) + 1) + "d");
                        }
                        else if ((getStringLength(temp_param) + 1) > 9 && (getStringLength(temp_param) + 1) < 100)
                        {
                            this._dconf.Add("command", "00" + (temp_param.Length + 1) + "d");
                        }
                        else if ((getStringLength(temp_param) + 1) > 99 && (getStringLength(temp_param) + 1) < 1000)
                        {
                            this._dconf.Add("command", "0" + (getStringLength(temp_param) + 1) + "d");
                        }
                        else if ((getStringLength(temp_param) + 1) > 999 && (getStringLength(temp_param) + 1) < 10000)
                        {
                            this._dconf.Add("command", "" + (getStringLength(temp_param) + 1) + "d");
                        }
                        this._dconf.Add("agent_param", temp_param);     

                        if (BeRunning(status))
                        {
                            if (string.IsNullOrEmpty(this._dconf["job_host"]))
                            {
                                base.Result = XenAPI.Host.call_plugin(base.Session, base.Host.opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                            }
                            else
                            {
                                base.Result = XenAPI.Host.call_plugin(base.Session, HalsignHelpers.GetHostFromUuid(base.Connection, this._dconf["job_host"]).opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                            }

                            if (!string.IsNullOrEmpty(base.Result))
                            {
                                throw new Exception(Messages.BR_CALL_AGENT_EXCEPTION + base.Result);
                            }

                            if (this.deletedObject is VM)
                            {
                                VM vm = this.deletedObject as VM;
                                if (vm != null && this._dconf["type"] != BackupRestoreConfig.BACKUP_IMMEDIATELY)
                                {
                                    XenAPI.VM.remove_from_other_config(base.Session, vm.opaque_ref, "halsign_br_rules");
                                }
                            }
                        }
                        else
                        {
                            if (this.deletedObject is Pool)
                            {
                                Pool pool = this.deletedObject as Pool;
                                if (pool != null && pool.other_config.ContainsKey(job_key))
                                {
                                    BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(pool.other_config[job_key], typeof(BackupRestoreConfig.Job));
                                    job.status = (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELED;
                                    pool.other_config[job_key] = HalsignUtil.ToJson(job);
                                    XenAPI.Pool.set_other_config(base.Session, pool.opaque_ref, pool.other_config);
                                }
                            }
                            else if (this.deletedObject is VM)
                            {
                                VM vm = this.deletedObject as VM;
                                if (vm != null)
                                {
                                    if (vm.other_config.ContainsKey(job_key))
                                    {
                                        BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(vm.other_config[job_key], typeof(BackupRestoreConfig.Job));
                                        job.status = (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELED;
                                        vm.other_config[job_key] = HalsignUtil.ToJson(job);
                                        XenAPI.VM.set_other_config(base.Session, vm.opaque_ref, vm.other_config);
                                    }
                                    if (this._dconf["type"] != BackupRestoreConfig.BACKUP_IMMEDIATELY)
                                    {
                                        XenAPI.VM.remove_from_other_config(base.Session, vm.opaque_ref, "halsign_br_rules");
                                    }
                                }
                            }
                        }
                        break;
                    case BackupRestoreConfig.BackupActionKind.Replication:
                        foreach (VM vmTemp in this.vmCheckedList)
                        {
                            if ("False".Equals(this._dconf["is_now"]))
                            {
                                BackupRestoreConfig.Job job = (BackupRestoreConfig.Job)HalsignUtil.JsonToObject(this._dconf["replication_job"], typeof(BackupRestoreConfig.Job));
                                if (this._vdi_expand_list.ContainsKey(vmTemp.uuid))
                                {
                                    string request_expend = "";
                                    BackupRestoreConfig.BrSchedule schedule = (BackupRestoreConfig.BrSchedule)HalsignUtil.JsonToObject(this._dconf["replication_rules"], typeof(BackupRestoreConfig.BrSchedule));
                                    int vdiNo = 0;
                                    foreach (VBD vbd in vmTemp.Connection.ResolveAll<VBD>(vmTemp.VBDs))
                                    {
                                        if (HalsignHelpers.IsCDROM(vbd))
                                        {
                                            continue;
                                        }

                                        if (vbd.type.Equals(XenAPI.vbd_type.Disk))
                                        {
                                            vdiNo++;
                                        }
                                    }
                                    if (vdiNo == _vdi_expand_list[vmTemp.uuid].Split('@').Length)
                                    {
                                        request_expend = "|" + "halsign_vdi_all";
                                    }
                                    else
                                    {
                                        request_expend = "|" + _vdi_expand_list[vmTemp.uuid];
                                    }
                                    schedule.details = schedule.details + request_expend;
                                    if (this._dconf.ContainsKey("replication_rules"))
                                    {
                                        string str_rules =  HalsignUtil.ReplicationJsonFormat(HalsignUtil.ToJson(schedule));
                                        _dconf.Remove("replication_rules");
                                        _dconf.Add("replication_rules", str_rules);
                                    }
                                    if ("false".Equals(this._dconf["replication_editjob"]))
                                    {
                                        job.request += vmTemp.uuid + "|" + schedule.details;
                                    }
                                    this._dconf["replication_job"] = HalsignUtil.ReplicationJsonFormat(HalsignUtil.ToJson(job));
                                    if (vmTemp.other_config.ContainsKey("halsign_br_job_r"))
                                    {
                                        vmTemp.other_config["halsign_br_job_r"] = this._dconf["replication_job"];//HalsignUtil.ToJson(job);
                                        XenAPI.VM.set_other_config(base.Session, vmTemp.opaque_ref, vmTemp.other_config);
                                    }
                                    else
                                    {
                                        XenAPI.VM.set_other_config(base.Session, vmTemp.opaque_ref, vmTemp.other_config);
                                        XenAPI.VM.add_to_other_config(base.Session, vmTemp.opaque_ref, "halsign_br_job_r", this._dconf["replication_job"]);//HalsignUtil.ToJson(job));
                                    }
                                }
                                if (vmTemp.other_config.ContainsKey("halsign_rep_rules"))
                                {
                                    vmTemp.other_config["halsign_rep_rules"] =  HalsignUtil.ReplicationJsonFormat(this._dconf["replication_rules"]);
                                    XenAPI.VM.set_other_config(base.Session, vmTemp.opaque_ref, vmTemp.other_config);
                                }
                                else
                                {
                                    XenAPI.VM.add_to_other_config(base.Session, vmTemp.opaque_ref, "halsign_rep_rules", this._dconf["replication_rules"]);
                                }
                            }
                            else if ("True".Equals(this._dconf["is_now"]))
                            {
                                foreach (Dictionary<string, List<string>> item in listScheduleTemp)
                                {
                                    if (item.ContainsKey(vmTemp.uuid))
                                    {
                                        foreach (string itemList in item[vmTemp.uuid])
                                        {
                                            if (this._dconf.ContainsKey("agent_param"))
                                            {
                                                this._dconf.Remove("agent_param");
                                            }
                                            if (this._dconf.ContainsKey("command"))
                                            {
                                                this._dconf.Remove("command");
                                            }

                                            if (itemList.Split('|').Length == 10)
                                            {
                                                if ((getStringLength(itemList) + 1) < 100)
                                                {
                                                    this._dconf.Add("command", "00" + (getStringLength(itemList) + 1) + "g");
                                                }
                                                else if ((getStringLength(itemList) + 1) < 1000 && (getStringLength(itemList) + 1) > 99)
                                                {
                                                    this._dconf.Add("command", "0" + (getStringLength(itemList) + 1) + "g");
                                                }
                                                else if ((getStringLength(itemList) + 1) < 10000 && (getStringLength(itemList) + 1) > 999)
                                                {
                                                    this._dconf.Add("command", "" + (getStringLength(itemList) + 1) + "g");
                                                }
                                            }
                                            else if (itemList.Split('|').Length == 15)
                                            {
                                                if ((getStringLength(itemList) + 1) < 100)
                                                {
                                                    this._dconf.Add("command", "00" + (getStringLength(itemList) + 1) + "f");
                                                }
                                                else if ((getStringLength(itemList) + 1) < 1000 && (getStringLength(itemList) + 1) > 99)
                                                {
                                                    this._dconf.Add("command", "0" + (getStringLength(itemList) + 1) + "f");
                                                }
                                                else if ((getStringLength(itemList) + 1) < 10000 && (getStringLength(itemList) + 1) > 999)
                                                {
                                                    this._dconf.Add("command", "" + (getStringLength(itemList) + 1) + "f");
                                                }
                                            }
                                            this._dconf.Add("agent_param", itemList);
                                            base.Result = XenAPI.Host.call_plugin(base.Session, HalsignHelpers.VMHome(vmTemp).opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                                            if (!string.IsNullOrEmpty(base.Result))
                                            {
                                                throw new Exception(Messages.BR_CALL_AGENT_EXCEPTION + base.Result);
                                            }
                                        }
                                        continue;
                                    }
                                }
                            }
                        }
                        break;
                    case BackupRestoreConfig.BackupActionKind.Publish:
                        foreach (VM vmTemp in this.vmCheckedList)
                        {
                            if (this._dconf.ContainsKey("agent_param"))
                            {
                                this._dconf.Remove("agent_param");
                            }
                            if (this._dconf.ContainsKey("command"))
                            {
                                this._dconf.Remove("command");
                            }
                            string job_expend = null;
                            if (this._vdiconf.ContainsKey(vmTemp.uuid))
                            {
                                job_expend = this._dconf["job_name"] + "|" + vmTemp.uuid + "|" + _vdiconf[vmTemp.uuid] + "|" + this._dconf["ip"] + "|" + this._dconf["username"] + "|" + this._dconf["password"];
                                int vdiNo = 0;
                                foreach (VBD vbd in vmTemp.Connection.ResolveAll<VBD>(vmTemp.VBDs))
                                {
                                    if (HalsignHelpers.IsCDROM(vbd))
                                    {
                                        continue;
                                    }

                                    if (vbd.type.Equals(XenAPI.vbd_type.Disk))
                                    {
                                        vdiNo++;
                                    }
                                }
                                if (_vdiconf[vmTemp.uuid].Split('@').Length == vdiNo)
                                {
                                    job_expend = this._dconf["job_name"] + "|" + vmTemp.uuid + "|halsign_vdi_all|" + this._dconf["ip"] + "|" + this._dconf["username"] + "|" + this._dconf["password"];
                                }

                                if ((getStringLength(job_expend) + 1) < 100)
                                {
                                    this._dconf.Add("command", "00" + (getStringLength(job_expend) + 1) + "j");
                                }
                                else if ((getStringLength(job_expend) + 1) > 99 && (getStringLength(job_expend) + 1) < 1000)
                                {
                                    this._dconf.Add("command", "0" + (getStringLength(job_expend) + 1) + "j");
                                }
                                else if ((getStringLength(job_expend) + 1) > 999 && (getStringLength(job_expend) + 1) < 10000)
                                {
                                    this._dconf.Add("command", "" + (getStringLength(job_expend) + 1) + "j");
                                }
                                else
                                {
                                    continue;
                                }
                                this._dconf.Add("agent_param", job_expend);
                            }
                            else
                            {
                                continue;
                            }

                            /*
                             * Check whether the vm can be handled by the current agent.
                             * 1) Check the vm's affinity host, if the affinity host is not empty, that host will be the target host.
                             * 2) If the host is slave, the host uuid must be the same with the target host uuid.
                             * 3) if the host is master, the host uuid can be the same with the target host uuid,
                             * or the target host is down and the master host it one of the VM's possible host.
                             * 4) Sometimes the VM's affinity is empty, then we have to use one of the possible host as its target host.
                             */
                            Host _host_affinity = base.Connection.Resolve<Host>(vmTemp.affinity);
                            List<XenRef<Host>> _list_host = XenAPI.VM.get_possible_hosts(base.Session, vmTemp.opaque_ref);
                            if (_host_affinity != null && _host_affinity.IsLive)
                            {
                                base.Result = XenAPI.Host.call_plugin(base.Session, _host_affinity.opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                            }
                            else if (_list_host != null && _list_host.Count > 0 && base.Connection.Resolve<Host>(_list_host[0]).IsLive)
                            {
                                base.Result = XenAPI.Host.call_plugin(base.Session, _list_host[0].opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                            }
                            else if (_list_host == null || _list_host.Count <= 0)
                            {
                                base.Result = XenAPI.Host.call_plugin(base.Session, HalsignHelpers.VMHome(vmTemp).opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                            }
                            else
                            {
                                bool have_master = false;
                                foreach (XenRef<Host> item in _list_host)
                                {
                                    if (item != null && item.opaque_ref == Helpers.GetMaster(base.Connection).opaque_ref)
                                    {
                                        have_master = true;
                                    }
                                }
                                if (have_master)
                                {
                                    base.Result = XenAPI.Host.call_plugin(base.Session, Helpers.GetMaster(base.Connection).opaque_ref, "halsign-backup-restore", "callAgent", this._dconf);
                                }
                                else
                                {
                                    throw new Exception(Messages.BR_CALL_AGENT_FROM_MASTER);
                                }
                            }
                            if (!string.IsNullOrEmpty(base.Result))
                            {
                                throw new Exception(Messages.BR_CALL_AGENT_EXCEPTION + base.Result);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception exception)
            {
                isActionSucceed = false;
                base.Exception = exception;
                log.Error("Backup&restore exception happened: ", exception);
            }
            base.Description = isActionSucceed ? Messages.BR_ACTION_SUCCESS : Messages.BR_ACTION_FAILED;
        }

        private void queryHistory(string _vm_uuid, string _ip, string _username, string _password)
        {
            string url = "https://" + _ip + @"/halsign/log/list/";
            byte[] data = UTF8Encoding.GetEncoding("utf-8").GetBytes(string.Format("LOG_TYPE={0}&uuid={1}", "dr", _vm_uuid));
            try
            {
                this._histotyresultList.Add((BackupRestoreConfig.DRResult)HalsignUtil.JsonToObject(this.HttpPostCall(url, _username, _password, data), typeof(BackupRestoreConfig.DRResult)));
            }
            catch (Exception)
            {
                throw new Exception(Messages.BR_PARSE_HISTORY);
            }
            if (base.Cancelling)
            {
                throw new CancelledException();
            }
        }

        private void ParseRestoreList()
        {
            string url = "https://" + this._dconf["ip"] + @"/halsign/dr/lsvms/vms/";
            BackupRestoreConfig.RestoreListInfo listInfo =
                (BackupRestoreConfig.RestoreListInfo)
                HalsignUtil.JsonToObject(this.HttpCall(url, this._dconf["username"], this._dconf["password"], "json"),
                                         typeof (BackupRestoreConfig.RestoreListInfo));
            if(listInfo.error_code == 0)
            {
                this.restoreList = listInfo;
            }
        }

        private string HttpCall(string url, string _username, string _password, string type)
        {
            string _result = "";
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                X509Certificate cer = X509Certificate.CreateFromCertFile(Application.StartupPath + "/Halsign.cer");
                request.Proxy = WebRequest.DefaultWebProxy;
                request.Method = "GET";
                request.ContentType = request.Accept = string.Format("application/{0}", type);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                request.Headers["CLIENT-API-VERSION"] = "5.2.0";
                //NetworkCredential credentials = new NetworkCredential(_username, _password);
                //CredentialCache cache = new CredentialCache();
                //cache.Add(uri, "Basic", credentials);
                //request.Credentials = cache;
                request.ClientCertificates.Add(cer);
                string authInfo = _username + ":" + _password;
                authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
                request.Headers.Add("Authorization", "Basic " + authInfo);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                _result = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw new Exception(Messages.BR_CALL_STORAGE_EXCEPTION + "(" + url + ") " + e.Message);
            }

            return _result;
        }

        private string HttpPostCall(string url, string _username, string _password, byte[] data)
        {
            string _result = "";
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                X509Certificate cer = X509Certificate.CreateFromCertFile(Application.StartupPath + "/Halsign.cer");
                request.Proxy = WebRequest.DefaultWebProxy;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "application/json";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                request.Headers["CLIENT-API-VERSION"] = "5.2.0";
                //NetworkCredential credentials = new NetworkCredential(_username, _password);
                //CredentialCache cache = new CredentialCache();
                //cache.Add(uri, "Basic", credentials);
                //request.Credentials = cache;
                string authInfo = _username + ":" + _password;
                authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
                request.Headers.Add("Authorization", "Basic " + authInfo);
                request.ClientCertificates.Add(cer);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                request.ContentLength = data.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }               

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                _result = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw new Exception(Messages.BR_CALL_STORAGE_EXCEPTION + "(" + url + ") " + e.Message);
            }

            return _result;
        }

        private static string TestPostRequest()
        {
            string[] paramName = { "src_vm_uuid", "dest_vm_uuid", "vm_name", "start_time", "end_time", "error_code", "request", "status", "schedule_type", "job_name" };
            string[] paramValue = { "test3", "test3", "vm-name", "111", "222", "0", "test", "0", "0", "bug151515中文测试" };
            string _result = "";
            string url = "http://192.168.0.38:8888/sr/insertlog";
            //Encoding myEncoding = Encoding.GetEncoding("UTF8");
            // 编辑并Encoding提交的数据
            StringBuilder sbuilder = new StringBuilder(paramName[0] + "=" + paramValue[0]);
            for (int i = 1; i < paramName.Length; i++)
                sbuilder.Append("&" + HttpUtility.UrlEncode(paramName[i], System.Text.Encoding.UTF8) + "=" + HttpUtility.UrlEncode(paramValue[i], System.Text.Encoding.UTF8));
            byte[] data = System.Text.Encoding.UTF8.GetBytes(sbuilder.ToString());

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                request.Accept = "application/xml";
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes("br1" + ":" + "qwer1234")));
                request.ContentLength = data.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                _result = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw new Exception(Messages.BR_CALL_STORAGE_EXCEPTION + "(" + url + ") " + e.Message);
            }

            return _result;
        }

        private Boolean BeRunning(int status)
        {
            return (status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CHK_ZFS
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_GEN_METADATA
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_METADATA
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_TRANS_DATA
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_VERIFY_DATA
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_RUNNING
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_DELETE_SNAP
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_ZFS_SNAP
                || status == (int)BackupRestoreConfig.JOB_STATUS.JOB_STATUS_CANCELING);
        }

        protected void Cancel_()
        {
            try
            {
                throw new CancelledException();
            }
            catch (CancelledException e)
            {
                base.Exception = e;
                //base.Cancel_();
            }

            //this.DestroyTask();
        }

        public override void RecomputeCanCancel()
        {
            this.CanCancel = true;
        }

        public BackupRestoreConfig.RestoreListInfo RestoreList
        {
            get { return this.restoreList; }
        }

        public List<BackupRestoreConfig.DRResult> historyResultList
        {
            get { return this._histotyresultList; }
        }

        private int getStringLength(string str)
        {
            System.Text.ASCIIEncoding n = new System.Text.ASCIIEncoding();
            byte[] b = n.GetBytes(str);
            int l = 0;
            for (int i = 0; i <= b.Length - 1; i++)
            {
                if (b[i] == 63)
                {
                    l += 2;
                }
                l++;
            }
            return l;
        }
    }
}
