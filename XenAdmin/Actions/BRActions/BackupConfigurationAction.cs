namespace XenAdmin.Actions.BRActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using log4net;
    using System.Reflection;
    using XenAPI;
    using System.Net;
    using System.IO;
    using System.Xml;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;
    using System.Windows.Forms;
    using XenAdmin.Network;
    using System.Web;

    public class BackupConfigurationAction : AsyncAction
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IXenObject xenModelObject;
        private readonly Dictionary<string, string> _dconf;
        private readonly int _configFlag;
        private readonly bool _enabled;

        public BackupConfigurationAction(IXenConnection connection, IXenObject _xenModelObject, Dictionary<string, string> args, int configFlag)
            : base(connection, string.Format(Messages.PLUGIN_TITLE, Messages.CONFIGURE_TITLE), true)
        {
            this.xenModelObject = _xenModelObject;
            this._dconf = args;
            this._configFlag = configFlag;
        }

        public BackupConfigurationAction(IXenConnection connection, IXenObject xenObject, bool enabled)
            : base(connection, string.Format(Messages.PLUGIN_TITLE, Messages.CONFIGURE_TITLE), true)
        {
            this._configFlag = 2;
            this.xenModelObject = xenObject;
            this._enabled = enabled;
        }

        protected override void Run()
        {
            try
            {
                switch (this._configFlag)
                {
                    case 0:
                    #region Add BR Configuration

                    TestConnection();
                    if (this.xenModelObject is Pool)
                    {
                        Pool pool = this.xenModelObject as Pool;
                        if (pool.other_config.ContainsKey("halsign_br_ip_address"))
                        {
                            pool.other_config["halsign_br_ip_address"] = _dconf["halsign_br_ip_address"];
                            XenAPI.Pool.set_other_config(base.Session, pool.opaque_ref, pool.other_config);
                        }
                        else
                        {
                            XenAPI.Pool.add_to_other_config(base.Session, pool.opaque_ref, "halsign_br_ip_address", _dconf["halsign_br_ip_address"]);
                        }
                        if (pool.other_config.ContainsKey("halsign_br_username"))
                        {
                            pool.other_config["halsign_br_username"] = _dconf["halsign_br_username"];
                            XenAPI.Pool.set_other_config(base.Session, pool.opaque_ref, pool.other_config);
                        }
                        else
                        {
                            XenAPI.Pool.add_to_other_config(base.Session, pool.opaque_ref, "halsign_br_username", _dconf["halsign_br_username"]);
                        }
                        if (pool.other_config.ContainsKey("halsign_br_password"))
                        {
                            pool.other_config["halsign_br_password"] = _dconf["halsign_br_password"];
                            XenAPI.Pool.set_other_config(base.Session, pool.opaque_ref, pool.other_config);
                        }
                        else
                        {
                            XenAPI.Pool.add_to_other_config(base.Session, pool.opaque_ref, "halsign_br_password", _dconf["halsign_br_password"]);
                        }
                        if (pool.other_config.ContainsKey("halsign_br_enabled"))
                        {
                            pool.other_config["halsign_br_enabled"] = "True";
                            XenAPI.Pool.set_other_config(base.Session, pool.opaque_ref, pool.other_config);
                        }
                        else
                        {
                            XenAPI.Pool.add_to_other_config(base.Session, pool.opaque_ref, "halsign_br_enabled", "True");
                        }
                    }
                    if (this.xenModelObject is VM)
                    {
                        VM vm = this.xenModelObject as VM;
                        if (vm.other_config.ContainsKey("halsign_br_ip_address"))
                        {
                            vm.other_config["halsign_br_ip_address"] = _dconf["halsign_br_ip_address"];
                            XenAPI.VM.set_other_config(base.Session, vm.opaque_ref, vm.other_config);
                        }
                        else
                        {
                            XenAPI.VM.add_to_other_config(base.Session, vm.opaque_ref, "halsign_br_ip_address", _dconf["halsign_br_ip_address"]);
                        }
                        if (vm.other_config.ContainsKey("halsign_br_username"))
                        {
                            vm.other_config["halsign_br_username"] = _dconf["halsign_br_username"];
                            XenAPI.VM.set_other_config(base.Session, vm.opaque_ref, vm.other_config);
                        }
                        else
                        {
                            XenAPI.VM.add_to_other_config(base.Session, vm.opaque_ref, "halsign_br_username", _dconf["halsign_br_username"]);
                        }
                        if (vm.other_config.ContainsKey("halsign_br_password"))
                        {
                            vm.other_config["halsign_br_password"] = _dconf["halsign_br_password"];
                            XenAPI.VM.set_other_config(base.Session, vm.opaque_ref, vm.other_config);
                        }
                        else
                        {
                            XenAPI.VM.add_to_other_config(base.Session, vm.opaque_ref, "halsign_br_password", _dconf["halsign_br_password"]);
                        }
                        if (vm.other_config.ContainsKey("halsign_br_enabled"))
                        {
                            vm.other_config["halsign_br_enabled"] = "True";
                            XenAPI.VM.set_other_config(base.Session, vm.opaque_ref, vm.other_config);
                        }
                        else
                        {
                            XenAPI.VM.add_to_other_config(base.Session, vm.opaque_ref, "halsign_br_enabled", "True");
                        }
                    }
                    break;

                    #endregion
                    case 1:
                    #region Delete BR Configuration

                        if (this.xenModelObject is Pool)
                        {
                            Pool pool = this.xenModelObject as Pool;
                            if (pool.other_config.ContainsKey("halsign_br_ip_address"))
                            {
                                XenAPI.Pool.remove_from_other_config(base.Session, pool.opaque_ref,
                                                                     "halsign_br_ip_address");
                            }
                            if (pool.other_config.ContainsKey("halsign_br_username"))
                            {
                                XenAPI.Pool.remove_from_other_config(base.Session, pool.opaque_ref,
                                                                     "halsign_br_username");
                            }
                            if (pool.other_config.ContainsKey("halsign_br_password"))
                            {
                                XenAPI.Pool.remove_from_other_config(base.Session, pool.opaque_ref,
                                                                     "halsign_br_password");
                            }
                            if (pool.other_config.ContainsKey("halsign_br_enabled"))
                            {
                                XenAPI.Pool.remove_from_other_config(base.Session, pool.opaque_ref, "halsign_br_enabled");
                            }
                        }
                        if (this.xenModelObject is VM)
                        {
                            VM vm = this.xenModelObject as VM;
                            if (vm.other_config.ContainsKey("halsign_br_ip_address"))
                            {
                                XenAPI.VM.remove_from_other_config(base.Session, vm.opaque_ref, "halsign_br_ip_address");
                            }
                            if (vm.other_config.ContainsKey("halsign_br_username"))
                            {
                                XenAPI.VM.remove_from_other_config(base.Session, vm.opaque_ref, "halsign_br_username");
                            }
                            if (vm.other_config.ContainsKey("halsign_br_password"))
                            {
                                XenAPI.VM.remove_from_other_config(base.Session, vm.opaque_ref, "halsign_br_password");
                            }
                            if (vm.other_config.ContainsKey("halsign_br_enabled"))
                            {
                                XenAPI.VM.remove_from_other_config(base.Session, vm.opaque_ref, "halsign_br_enabled");
                            }
                        }
                        break;

                    #endregion
                    case 2:
                    #region Update BR Configuration

                        if (this.xenModelObject is Pool)
                        {
                            Pool pool = this.xenModelObject as Pool;
                            if (this._enabled)
                            {
                                XenAPI.Pool.remove_from_other_config(base.Session, pool.opaque_ref, "halsign_br_enabled");
                                XenAPI.Pool.add_to_other_config(base.Session, pool.opaque_ref, "halsign_br_enabled", "False");
                                //pool.other_config["halsign_br_enabled"] = "False";
                            }
                            else
                            {
                                XenAPI.Pool.remove_from_other_config(base.Session, pool.opaque_ref, "halsign_br_enabled");
                                XenAPI.Pool.add_to_other_config(base.Session, pool.opaque_ref, "halsign_br_enabled", "True");
                                //pool.other_config["halsign_br_enabled"] = "True";
                            }
                            //XenAPI.Pool.set_other_config(base.Session, pool.opaque_ref, pool.other_config);
                        }
                        if (this.xenModelObject is VM)
                        {
                            VM vm = this.xenModelObject as VM;
                            if (this._enabled)
                            {
                                XenAPI.VM.remove_from_other_config(base.Session, vm.opaque_ref, "halsign_br_enabled");
                                XenAPI.VM.add_to_other_config(base.Session, vm.opaque_ref, "halsign_br_enabled", "False");
                                //vm.other_config["halsign_br_enabled"] = "False";
                            }
                            else
                            {
                                XenAPI.VM.remove_from_other_config(base.Session, vm.opaque_ref, "halsign_br_enabled");
                                XenAPI.VM.add_to_other_config(base.Session, vm.opaque_ref, "halsign_br_enabled", "True");
                                //vm.other_config["halsign_br_enabled"] = "True";
                            }
                            //XenAPI.VM.set_other_config(base.Session, vm.opaque_ref, vm.other_config);
                        }
                        break;

                    #endregion
                    default:break;
                }
            }
            catch (Exception exception)
            {
                log.Error("Exception happens when send backup configuration: " + exception.StackTrace);
                throw;
            }
        }

        private void TestConnection()
        {
            string url = "https://" + this._dconf["halsign_br_ip_address"] + "/halsign/user/auth/";
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                X509Certificate cer = X509Certificate.CreateFromCertFile(Application.StartupPath + "/Halsign.cer");
                request.Proxy = WebRequest.DefaultWebProxy;
                request.Method = WebRequestMethods.Http.Get;
                request.ContentType = "application/xml";
                request.Accept = "application/xml";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                request.Headers["CLIENT-API-VERSION"] = "5.2.0";
                //NetworkCredential credentials = new NetworkCredential(this._dconf["halsign_br_username"], this._dconf["halsign_br_password"]);
                //CredentialCache cache = new CredentialCache();
                //cache.Add(uri, "Basic", credentials);
                //request.Credentials = cache;
                request.ClientCertificates.Add(cer);
                string authInfo = this._dconf["halsign_br_username"] + ":" + this._dconf["halsign_br_password"];
                authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
                request.Headers.Add("Authorization", "Basic " + authInfo);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);

                XmlDocument document = new XmlDocument();
                document.LoadXml(reader.ReadToEnd());
                XmlNode errorCodeNode = null;
                XmlNode itemsNode = null;
                XmlNode errormessageNode = null;

                foreach (XmlNode node in document.GetElementsByTagName("Result"))
                {
                    errorCodeNode = node.SelectSingleNode("errorCode");
                    itemsNode = node.SelectSingleNode("items");
                    if (errorCodeNode == null || errorCodeNode.InnerText.Trim() != "0" || itemsNode == null)
                    {
                        errormessageNode = node.SelectSingleNode("errorMessage");
                        if (errormessageNode == null || string.IsNullOrEmpty(errormessageNode.InnerText.Trim()))
                        {
                            base.Exception = new Exception(Messages.BACKUP_UNEXPECTED_CONNECTING + "\r\n" + url);
                        }
                        else
                        {
                            if (errorCodeNode.InnerText.Trim().Equals("1503"))
                            {
                                base.Exception = new Exception(string.Format(Messages.BACKUP_ERROR_1503, this._dconf["halsign_br_username"]));
                            }
                            else if (errorCodeNode.InnerText.Trim().Equals("1504"))
                            {
                                base.Exception = new Exception(Messages.BACKUP_ERROR_1504);
                            }
                            else
                            {
                                base.Exception = new Exception(errormessageNode.InnerText.Trim());
                            }
                        }
                        throw base.Exception;
                    }
                }
            }
            catch (Exception e)
            {
                base.Exception = e;
                throw;
            }
        }
    }
}
