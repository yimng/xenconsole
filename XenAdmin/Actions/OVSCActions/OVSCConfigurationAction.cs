using HalsignLib;
using HalsignLib.HalsignModel;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Windows.Forms;
using XenAdmin.Network;
using XenAPI;

namespace XenAdmin.Actions.OVSCActions
{
    public class OVSCConfigurationAction : AsyncAction
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IXenConnection _xenConnection;
        private readonly Pool _Pool;
        private readonly Dictionary<string, string> _Args;
        private readonly int _Flag;
        private readonly bool _Register;
        private const string VMANAGER_PORT = "8443";

        public OVSCConfigurationAction(IXenConnection connection, Pool pool, Dictionary<string, string> args, bool register, int flag)
            : base(connection, Messages.OVSC_CONFIGURE_TITLE, true)
        {
            this._xenConnection = connection;
            this._Pool = pool;
            this._Args = args;
            this._Register = register;
            this._Flag = flag;
        }

        public OVSCConfigurationAction(IXenConnection connection, Pool pool)
            : this(connection, pool, null, false, 1)
        {

        }

        protected override void Run()
        {
            try
            {
                Dictionary<string, string> other_config = _Pool.other_config;

                switch (this._Flag)
                {
                    case 0:
                        if (_Register)
                        {
                            if (other_config.ContainsKey("vswitch_controller_account"))
                            {
                                other_config["vswitch_controller_account"] = _Args["controller_username"];
                            }
                            else
                            {
                                other_config.Add("vswitch_controller_account", _Args["controller_username"]);
                            }
                            string url =
                                string.Format(
                                    @"https://{0}:{1}/client/ws/node?method=registerVgates&vmnUser={2}&vmnPass={3}&vGateUser={4}&vGatePass={5}&vGateIpAddress={6}",
                                    _Args["controller_ip"],
                                    VMANAGER_PORT, HttpUtility.UrlEncode(_Args["controller_username"]),
                                    HttpUtility.UrlEncode(_Args["controller_password"]),
                                    HttpUtility.UrlEncode(_Args["vgate_username"]),
                                    HttpUtility.UrlEncode(_Args["vgate_password"]),
                                    _Args["master_address"]);
                            var result = (OVSCConfig.ConfigResult)HalsignUtil.JsonToObject(this.HttpCall(url), typeof(OVSCConfig.ConfigResult));
                            if (null == result) throw new Exception(string.Format("Exception when call {0}", url));
                            string errMsg = string.Empty;
                            var errcode = (OVSCConfig.OCSC_CONFIG_ERRORCODE)(int.Parse(result.code));
                            if (errcode != OVSCConfig.OCSC_CONFIG_ERRORCODE.ERR_SUCCESS &&
                                errcode != OVSCConfig.OCSC_CONFIG_ERRORCODE.ERR_REGISTERED)
                            {
                                switch (errcode)
                                {
                                    case OVSCConfig.OCSC_CONFIG_ERRORCODE.ERR_INVALID_PARAMS:
                                        errMsg = Messages.ERR_INVALID_PARAMS;break;
                                    case OVSCConfig.OCSC_CONFIG_ERRORCODE.ERR_CONTROLLER_ACCOUNT_INVALID:
                                        errMsg = Messages.ERR_CONTROLLER_ACCOUNT_INVALID;break;
                                    case OVSCConfig.OCSC_CONFIG_ERRORCODE.ERR_VGATE_ACCOUNT_INVALID:
                                        errMsg = Messages.ERR_VGATE_ACCOUNT_INVALID;break;
                                    //case  (int)OVSCConfig.OCSC_CONFIG_ERRORCODE.ERR_REGISTERED:
                                    //    errMsg = Messages.ERR_REGISTERED;break;
                                    default:break;
                                }

                                throw new Exception(errMsg);
                            }
                        }
                        else
                        {
                            if (other_config.ContainsKey("vswitch_controller_account"))
                            {
                                if (_Pool.other_config.ContainsKey("vswitch_controller") &&
                                !string.IsNullOrEmpty(_Pool.other_config["vswitch_controller"]))
                                {
                                    string[] paramslist = _Pool.other_config["vswitch_controller"].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (paramslist.Count() != 3) return;
                                    string url = string.Format("https://{0}:{1}/client/ws/node?method=unregisterVgates&uuid={2}",
                                        paramslist[1], VMANAGER_PORT, _Pool.uuid);
                                    try
                                    {
                                        this.HttpCall(url);
                                    }
                                    catch(Exception){}
                                }
                                other_config.Remove("vswitch_controller_account");
                            }
                        }
                        string paramsValue = string.Format("{0}:{1}:{2}", _Args["protocol"], _Args["controller_ip"], _Args["controller_port"]);
                        //Call set_vswitch_controller
                        Pool.set_vswitch_controller(this._xenConnection.Session, _Args["controller_ip"]);
                        //Write value to pool other_config
                        if (other_config.ContainsKey("vswitch_controller"))
                        {
                            other_config["vswitch_controller"] = paramsValue;
                        }
                        else
                        {
                            other_config.Add("vswitch_controller", paramsValue);
                        }
                        break;
                    case 1:
                        if (other_config.ContainsKey("vswitch_controller_account"))
                        {
                            if (_Pool.other_config.ContainsKey("vswitch_controller") &&
                                !string.IsNullOrEmpty(_Pool.other_config["vswitch_controller"]))
                            {
                                string[] paramslist = _Pool.other_config["vswitch_controller"].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (paramslist.Count() != 3) return;
                                try
                                {
                                    string url =
                                        string.Format("https://{0}:{1}/client/ws/node?method=unregisterVgates&uuid={2}",
                                            paramslist[1], VMANAGER_PORT, _Pool.uuid);
                                    var result =
                                        (OVSCConfig.ConfigResult)
                                            HalsignUtil.JsonToObject(this.HttpCall(url),
                                                typeof (OVSCConfig.ConfigResult));
                                    if (null == result)
                                        throw new Exception(string.Format("Exception when call {0}", url));
                                    if (int.Parse(result.code) != (int) OVSCConfig.OCSC_CONFIG_ERRORCODE.ERR_SUCCESS)
                                    {
                                        throw new Exception(Messages.ERR_INVALID_PARAMS);
                                    }
                                }
                                catch (Exception exception)
                                {
                                    log.Error(exception.Message);
                                }
                                finally
                                {
                                    other_config.Remove("vswitch_controller_account");
                                    if (other_config.ContainsKey("vswitch_controller"))
                                    {
                                        other_config.Remove("vswitch_controller");
                                    }
                                    Pool.set_vswitch_controller(_xenConnection.Session, null);
                                }                               
                            }
                        }
                        break;
                    default: break;
                }

                Pool.set_other_config(_xenConnection.Session, _Pool.opaque_ref, other_config);
            }
            catch (System.Exception ex)
            {
                base.Exception = ex;
                throw;
            }
        }

        private string HttpCall(string url)
        {
            string _result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                X509Certificate cer = X509Certificate.CreateFromCertFile(Application.StartupPath + "/Halsign.cer");
                request.Proxy = WebRequest.DefaultWebProxy;
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                request.ClientCertificates.Add(cer);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                _result = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return _result;
        }
    }
}
