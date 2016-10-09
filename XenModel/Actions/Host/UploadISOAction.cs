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
using System.Net;
using System.ComponentModel;
using System.Threading;
using System.IO;
using XenCenterLib.Archive;
using Tamir.SharpSsh;
using XenAPI;
using System.Collections.Generic;

namespace XenAdmin.Actions
{
    internal enum UploadState { InProgress, Cancelled, Completed, Error };

    public class UploadISOAction : AsyncAction
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string outFolder;
        private readonly string fromfile;

        private UploadState isoUploadState;

        SshTransferProtocolBase sshCp;

        public string PatchPath
        {
            get; private set;
        }

        public UploadISOAction(Host host, string uploadfile, string outputFileName)
            : base(null, string.Format(Messages.UPLOAD_ISO_ACTION_TITLE), string.Empty, false)
        {
            Host = host;
            fromfile = uploadfile;
            outFolder = outputFileName;
        }

        private void UploadFile()
        {     
            try
            {
                //sshCp = new Sftp(Host.address, Host.Connection.Username);
                sshCp = new Scp(Host.address, Host.Connection.Username);

                sshCp.Password = Host.Connection.Password;
                sshCp.OnTransferProgress += new FileTransferEvent(sshCp_OnTransferProgress);
                sshCp.OnTransferEnd += new FileTransferEvent(sshCp_OnTransferEnd);
                log.InfoFormat("sshCp Connecting...");
                sshCp.Connect(22);

                log.InfoFormat("sshCp Connecting OK .......");

                sshCp.Put(fromfile, outFolder + "/" + Path.GetFileName(fromfile));

                isoUploadState = UploadState.InProgress;

                sshCp.Close();
            } catch (Exception e)
            {
                log.Error(e);
                throw new Exception(Messages.UPLOAD_ISO_FAILED);
            } 
            
        }        

        protected override void Run()
        {
            log.DebugFormat("Uploading ISO '{0}' (url: {1})", fromfile, outFolder);

            Description = string.Format(Messages.UPLOAD_ISO_ACTION_DESC, fromfile);
            LogDescriptionChanges = false;
            UploadFile();
            LogDescriptionChanges = true;

            if (IsCompleted || Cancelled)
                return;

            if (Cancelling)
                throw new CancelledException();

            Description = Messages.COMPLETED;
            MarkCompleted();
        }

        private void sshCp_OnTransferProgress(string src, string dst, long transferredBytes, long totalBytes, string message)
        {
            if (totalBytes == 0)
            {
                PercentComplete = 100;
                return;
            }
            if (100.0 * transferredBytes / totalBytes < 0)
            {
                log.Error((int)(100.0 * transferredBytes / totalBytes));
            } 
            PercentComplete = (int)(100.0 * transferredBytes / totalBytes);
            Description = string.Format(Messages.UPLOAD_ISO_ACTION_DETAIL_DESC, fromfile,
                                        Util.DiskSizeString(transferredBytes),
                                        Util.DiskSizeString(totalBytes));
        }

        private void sshCp_OnTransferEnd(string src, string dst, long transferredBytes, long totalBytes, string message)
        {
            isoUploadState = UploadState.Completed;
        }

        public override void RecomputeCanCancel()
        {
            CanCancel = !IsCompleted && (isoUploadState == UploadState.InProgress);
        }

        protected override void CancelRelatedTask()
        {
            sshCp.Cancel();
            clean_iso();
        }

        protected override void CleanOnError()
        {
            clean_iso();               
        }

        private void clean_iso()
        {
            try
            {
                Dictionary<String, String> args = new Dictionary<string, string>();
                args.Add("filepath", outFolder + "/" + Path.GetFileName(fromfile));
                XenAPI.Host.async_call_plugin(Host.Connection.Session, Host.opaque_ref, "clean_iso.py", "clean_iso", args);
            }
            catch (Exception e)
            {
                // if the clean up has failed for whatever reason we just log it and give up.
                log.Error(e);
            }
        }
    }
}
