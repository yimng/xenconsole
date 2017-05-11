/* Copyright © 2013 Halsign Corporation.
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using XenAPI;
using XenAdmin.Controls;
using XenAdmin.Network;
using XenAdmin.Core;


namespace XenAdmin.Wizards.RestoreWizard_Pages
{
    public partial class Page_SetStorage : XenTabPage
	{
        private Label Label_IP;
        private Label Label_UserName;
        private ComboBox ComboBox_IP;
        private TextBox TextBox_UserName;
        private TextBox TextBox_Password;
        private Label Label_UserPrompt;
        private Label Label_Password;
        private IXenObject _xenModelObject;

        public Page_SetStorage(IXenObject xenModelObject)
		{
			InitializeComponent();
            this._xenModelObject = xenModelObject;
		}

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page_SetStorage));
            this.Label_IP = new System.Windows.Forms.Label();
            this.Label_UserName = new System.Windows.Forms.Label();
            this.Label_Password = new System.Windows.Forms.Label();
            this.ComboBox_IP = new System.Windows.Forms.ComboBox();
            this.TextBox_UserName = new System.Windows.Forms.TextBox();
            this.TextBox_Password = new System.Windows.Forms.TextBox();
            this.Label_UserPrompt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Label_IP
            // 
            this.Label_IP.AccessibleDescription = null;
            this.Label_IP.AccessibleName = null;
            resources.ApplyResources(this.Label_IP, "Label_IP");
            this.Label_IP.Font = null;
            this.Label_IP.Name = "Label_IP";
            // 
            // Label_UserName
            // 
            this.Label_UserName.AccessibleDescription = null;
            this.Label_UserName.AccessibleName = null;
            resources.ApplyResources(this.Label_UserName, "Label_UserName");
            this.Label_UserName.Font = null;
            this.Label_UserName.Name = "Label_UserName";
            // 
            // Label_Password
            // 
            this.Label_Password.AccessibleDescription = null;
            this.Label_Password.AccessibleName = null;
            resources.ApplyResources(this.Label_Password, "Label_Password");
            this.Label_Password.Font = null;
            this.Label_Password.Name = "Label_Password";
            // 
            // ComboBox_IP
            // 
            this.ComboBox_IP.AccessibleDescription = null;
            this.ComboBox_IP.AccessibleName = null;
            resources.ApplyResources(this.ComboBox_IP, "ComboBox_IP");
            this.ComboBox_IP.BackgroundImage = null;
            this.ComboBox_IP.Font = null;
            this.ComboBox_IP.FormattingEnabled = true;
            this.ComboBox_IP.Name = "ComboBox_IP";
            this.ComboBox_IP.TextChanged += new System.EventHandler(this.SetStorage_Controls_TextChanged);
            // 
            // TextBox_UserName
            // 
            this.TextBox_UserName.AccessibleDescription = null;
            this.TextBox_UserName.AccessibleName = null;
            resources.ApplyResources(this.TextBox_UserName, "TextBox_UserName");
            this.TextBox_UserName.BackgroundImage = null;
            this.TextBox_UserName.Font = null;
            this.TextBox_UserName.Name = "TextBox_UserName";
            this.TextBox_UserName.TextChanged += new System.EventHandler(this.SetStorage_Controls_TextChanged);
            this.TextBox_UserName.Validated += new System.EventHandler(this.TextBox_UserName_Validated);
            // 
            // TextBox_Password
            // 
            this.TextBox_Password.AccessibleDescription = null;
            this.TextBox_Password.AccessibleName = null;
            resources.ApplyResources(this.TextBox_Password, "TextBox_Password");
            this.TextBox_Password.BackgroundImage = null;
            this.TextBox_Password.Font = null;
            this.TextBox_Password.Name = "TextBox_Password";
            this.TextBox_Password.UseSystemPasswordChar = true;
            this.TextBox_Password.TextChanged += new System.EventHandler(this.SetStorage_Controls_TextChanged);
            // 
            // Label_UserPrompt
            // 
            this.Label_UserPrompt.AccessibleDescription = null;
            this.Label_UserPrompt.AccessibleName = null;
            resources.ApplyResources(this.Label_UserPrompt, "Label_UserPrompt");
            this.Label_UserPrompt.Font = null;
            this.Label_UserPrompt.Name = "Label_UserPrompt";
            // 
            // Page_SetStorage
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.Label_UserPrompt);
            this.Controls.Add(this.TextBox_Password);
            this.Controls.Add(this.TextBox_UserName);
            this.Controls.Add(this.ComboBox_IP);
            this.Controls.Add(this.Label_Password);
            this.Controls.Add(this.Label_UserName);
            this.Controls.Add(this.Label_IP);
            this.Font = null;
            this.Name = "Page_SetStorage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public override bool EnableNext()
        {
            return !string.IsNullOrEmpty(this.ComboBox_IP.Text) && !string.IsNullOrEmpty(this.TextBox_UserName.Text) && !string.IsNullOrEmpty(this.TextBox_Password.Text);
        }

        public override bool EnablePrevious()
        {
            return false;
        }

        public override void PopulatePage()
        {
            InitStorageComboBox();
        }

        public override string Text
        {
            get
            {
                return Messages.RESTORE_SET_STORAGE;
            }
        }

        public override string PageTitle
        {
            get
            {
                return Messages.RESTORE_SET_STORAGE;
            }
        }

        #region Private Function Methods

        /// <summary>
        /// Update combox item whith vStorage server ip address
        /// </summary>
        private void InitStorageComboBox()
        {
            List<IXenConnection> xenConnectionsCopy = ConnectionsManager.XenConnectionsCopy;
            xenConnectionsCopy.Sort();
            Hashtable items = new Hashtable();
            int i = 0;
            foreach (IXenConnection connection in xenConnectionsCopy)
            {
                VM[] vMs = connection.Cache.VMs.Where(vm => vm.uuid != null).ToArray();
                Array.Sort<VM>(vMs);
                Pool pool = Helpers.GetPool(connection);
                if (pool != null && pool.other_config.ContainsKey("halsign_br_ip_address"))
                {
                    if (!items.ContainsValue(pool.other_config["halsign_br_ip_address"]))
                    {
                        items.Add(i, pool.other_config["halsign_br_ip_address"]);
                        i++;
                    }
                }
                foreach (VM vm in vMs)
                {
                    if (vm != null && vm.other_config.ContainsKey("halsign_br_ip_address"))
                    {
                        if (!items.ContainsValue(vm.other_config["halsign_br_ip_address"]))
                        {
                            items.Add(i, vm.other_config["halsign_br_ip_address"]);
                            i++;
                        }
                    }
                }
            }
            this.ComboBox_IP.BeginUpdate();
            foreach (DictionaryEntry item in items)
            {
                this.ComboBox_IP.Items.Add(item.Value);
            }
            this.ComboBox_IP.EndUpdate();
        }

        #endregion

        #region Events

        private void SetStorage_Controls_TextChanged(object sender, EventArgs e)
        {
            base.OnPageUpdated();
        }

        private void TextBox_UserName_Validated(object sender, EventArgs e)
        {
            if (this.TextBox_UserName.Text.ToString().ToLower().Equals("root"))
            {
                this.Label_UserPrompt.Visible = true;
            }
            else
            {
                this.Label_UserPrompt.Visible = false;
            }
        }

        #endregion

        #region Properties

        public string UserName
        {
            get { return this.TextBox_UserName.Text; }
            set { this.TextBox_UserName.Text = value; }
        }

        public string Password
        {
            get { return this.TextBox_Password.Text; }
            set { this.TextBox_Password.Text = value; }
        }

        public string StorageIP
        {
            get { return this.ComboBox_IP.Text; }
        }

        #endregion

        #region Public Functions Methods
        public string CheckAuthValidate()
        {
            string url = "https://" + this.ComboBox_IP.Text + "/halsign/user/auth/";
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                X509Certificate cer = X509Certificate.CreateFromCertFile(Application.StartupPath + "/Halsign.cer");
                request.Proxy = WebRequest.DefaultWebProxy;
                request.Method = "GET";
                request.ContentType = "application/xml";
                request.Accept = "application/xml";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                request.Headers["CLIENT-API-VERSION"] = "5.2.0";
                //NetworkCredential credentials = new NetworkCredential(this.TextBox_UserName.Text, this.TextBox_Password.Text);
                //CredentialCache cache = new CredentialCache {{uri, "Basic", credentials}};
                //request.Credentials = cache;
                request.ClientCertificates.Add(cer);
                string authInfo = this.TextBox_UserName.Text + ":" + this.TextBox_Password.Text;
                authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
                request.Headers.Add("Authorization", "Basic " + authInfo);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), encoding: Encoding.Default);

                XmlDocument document = new XmlDocument();
                document.LoadXml(reader.ReadToEnd());

                foreach (XmlNode node in document.GetElementsByTagName("Result"))
                {
                    XmlNode errorCodeNode = node.SelectSingleNode("errorCode");
                    XmlNode itemsNode = node.SelectSingleNode("items");
                    if (errorCodeNode == null || errorCodeNode.InnerText.Trim() != "0" || itemsNode == null)
                    {
                        XmlNode errormessageNode = node.SelectSingleNode("errorMessage");
                        if (errormessageNode == null || string.IsNullOrEmpty(errormessageNode.InnerText.Trim()))
                        {
                            //return string.Format("Unexpected exception when connecting to Halsign MC.");
                            return Messages.BACKUP_UNEXPECTED_CONNECTING;
                        }
                        else
                        {
                            if (errorCodeNode.InnerText.Trim().Equals("1503"))
                            {
                                return string.Format(Messages.BACKUP_ERROR_1503, this.TextBox_UserName.Text);
                            }
                            else if (errorCodeNode.InnerText.Trim().Equals("1504"))
                            {
                                return Messages.BACKUP_ERROR_1504;
                            }
                            else
                            {
                                return errormessageNode.InnerText.Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }
        #endregion
    }
}
