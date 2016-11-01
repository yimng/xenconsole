/* Copyright (c) Citrix Systems Inc. 
 * All rights reserved. 
 * 
 * Redistribution and use in source and binary forms, 
 * with or without modification, are permitted provided 
 * that the following conditions are met: 
 * 
 * *   Redistributions of source code must retain the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer. 
 * *   Redistributions in binary form must reproduce the above 
 *     copyright notice, this list of conditions and the 
 *     following disclaimer in the documentation and/or other 
 *     materials provided with the distribution. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND 
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using XenAPI;
using Tamir.SharpSsh;
using System.IO;
using System.Text;
using log4net;
using System.Reflection;
using System.Threading;

namespace XenAdmin.Wizards.PatchingWizard.PlanActions
{
    class PatchUpgradeHostPlanAction : UpgradeManualHostPlanAction
    {
        //private static global::System.Globalization.CultureInfo resourceCulture;
        //private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, string> _arguments = new Dictionary<string, string>();
        string _fileName;
        public PatchUpgradeHostPlanAction(Host host, Dictionary<string, string> arguments, string FileName)
            : base(host)
        {
            _arguments = arguments;
            _fileName = FileName;
        }


        protected override void RunWithSession(ref Session session)
        {
            string toDirectory = "/tmp";
            Status = Messages.IN_PROGRESS;
            log.InfoFormat("{0} Upgrade...", Host.address);
            int sshdstate = 1;
            int sshport = 22;
            Dictionary<string, string> _servicearguments = new Dictionary<string, string>();
            _servicearguments.Add("servicename", "sshd");
            try
            {
                string rsvalue = XenAPI.Host.call_plugin(Host.Connection.Session, Host.opaque_ref, "serviceinfo.py", "getserviceinfo", _servicearguments);
                rsvalue = rsvalue.Replace("[", "").Replace("]", "").Replace("'", "").Replace("\\n", "");
                string[] rsvalues = rsvalue.Split(',');
                if (rsvalues.Length > 1 && "0".Equals(rsvalues[1].Trim()))
                {
                    log.InfoFormat("{0} sshd is running ...", Host.address);
                }
                if (rsvalues.Length > 1 && "3".Equals(rsvalues[1].Trim()))
                {
                    log.InfoFormat("{0} sshd is stop, start sshd ...", Host.address);
                    rsvalue = XenAPI.Host.call_plugin(Host.Connection.Session, Host.opaque_ref, "serviceinfo.py", "startserviceinfo", _servicearguments);
                    rsvalue = rsvalue.Replace("[", "").Replace("]", "").Replace("'", "").Replace("\\n", "");
                    rsvalues = rsvalue.Split(',');
                    if (rsvalues.Length > 1 && "0".Equals(rsvalues[1].Trim()))
                    {
                        sshdstate = 0;
                    }
                    else
                    {
                        throw new Exception(Messages.START_SSH_FAILED);
                    }
                }
                //sshport = 7443;
            }catch(Exception)
            {}
            FileStream file = new FileStream(_fileName, FileMode.Open);
            string topfilename = Path.GetFileNameWithoutExtension(_fileName);
            
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            log.InfoFormat("md5.......{0}", sb);

            SshTransferProtocolBase sshCp;
            //sshCp = new Sftp(Host.address, Host.Connection.Username);
            sshCp = new Scp(Host.address, Host.Connection.Username);

            sshCp.Password = Host.Connection.Password;

            log.InfoFormat("sshCp Connecting...");
            sshCp.Connect(sshport);

            log.InfoFormat("sshCp Connecting OK .......");

            String Programurl = Program.AssemblyDir;

            sshCp.Put(_fileName, toDirectory+"/"+Path.GetFileName(_fileName));

            log.InfoFormat("cp Installation package ok......");

            sshCp.Put(Programurl + "\\halsign_host_upgrade.py", "/etc/xapi.d/plugins/halsign_host_upgrade.py");

            log.InfoFormat("cp halsign_host_upgrade.py ok......");
            
            SshShell ssh = new SshShell(Host.address, Host.Connection.Username);
            
            ssh.Password = Host.Connection.Password;
            string productVersion = Host.ProductVersion;

            log.InfoFormat("sshshell Connecting...");
            ssh.Connect(sshport);

            log.InfoFormat("sshshell Connecting ok...");

            ssh.ExpectPattern = "#";
            ssh.RemoveTerminalEmulationCharacters = true;

            //System.Console.WriteLine();
            //Thread.Sleep(1000);
            //ssh.WriteLine("chmod 777 /etc/xapi.d/plugins/halsign_host_upgrade.py;echo $?");
            //string output = ssh.Expect("#");

            //log.InfoFormat(output);

            while (true)
            {

                ssh.WriteLine("chmod 777 /etc/xapi.d/plugins/halsign_host_upgrade.py;echo $?");
                string output = ssh.Expect("#");
                log.InfoFormat(output);

                 if (output.IndexOf("rwxrwxrwx") > -1)
                {
                    break;
                }
                ssh.WriteLine("ls -ld /etc/xapi.d/plugins/halsign_host_upgrade.py |awk '{print $1}'|sed 's/^[a-zA-Z-]//'");
                output = ssh.Expect("#");
                log.InfoFormat(output);

                if (output.IndexOf("rwxrwxrwx") > -1)
                {
                    break;
                }
            }

            ssh.WriteLine("exit");
            ssh.Close();
            log.InfoFormat("sshshell Disconnecting OK.....");
            
            sshCp.Close();

            log.InfoFormat("sshCp Disconnecting OK.....");

            if (sshdstate == 0)
            {
                XenAPI.Host.call_plugin(Host.Connection.Session, Host.opaque_ref, "serviceinfo.py", "stopserviceinfo", _servicearguments);
            }

            _arguments= new Dictionary<string, string>();
            _arguments.Add("md5", sb.ToString());
            _arguments.Add("filename", topfilename);
            _arguments.Add("toDirectory", toDirectory);
            Status = Messages.PLAN_ACTION_STATUS_HOST_UPGRADED;
            string value = XenAPI.Host.call_plugin(session, Host.opaque_ref, "halsign_host_upgrade.py", "main", _arguments);

            if (value.ToLower() == "true")
            {
                //base.RunWithSession(ref session);
                Status = Messages.PLAN_ACTION_STATUS_HOST_UPGRADED;
            }
            else if ( ",1,2,5,6,7,10,11,12,13,1001,1002,1003,1004,1005,1006,".IndexOf( ","+value+",")>-1)
            {
                value = value == "2" ? "1" : value;

                string Ms = "PATCH_UPGRADE_" + value;

                if (value.Equals("1"))
                {
                    //Status = (string)XenAdmin.Messages.ResourceManager.GetString(Ms, resourceCulture);
                    throw new Exception("PATCHOK_"+(string)XenAdmin.Messages.ResourceManager.GetString(Ms));
                }
                else
                {
                    throw new Exception((string)XenAdmin.Messages.ResourceManager.GetString(Ms));
                }
            }
            else
            {
                throw new Exception(Messages.ERROR + ":" + value);
            }
        }
    }
}
