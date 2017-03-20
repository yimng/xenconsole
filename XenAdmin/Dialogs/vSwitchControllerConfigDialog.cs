using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using XenAdmin.Actions.OVSCActions;
using XenAdmin.Network;
using XenAPI;

namespace XenAdmin.Dialogs
{
    public partial class vSwitchControllerConfigDialog : XenDialogBase
    {
        private IXenConnection _xenConnection;
        private IXenObject _xenObject;
        public event EventHandler SaveButtonClick;

        public vSwitchControllerConfigDialog(IXenConnection connection, IXenObject xenobject)
        {
            InitializeComponent();
            this._xenConnection = connection;
            this._xenObject = xenobject;
            FillConfigureInformation();
        }

        private void FillConfigureInformation()
        {
            Pool pool = this._xenObject as Pool;
            if (pool == null) return;
            Dictionary<string, string> other_config = pool.other_config;
            //this.checkBoxRegister.Checked = other_config.ContainsKey("vswitch_controller_account");
            if (other_config.ContainsKey("vswitch_controller") && !string.IsNullOrEmpty(pool.other_config["vswitch_controller"]))
            {
                string params_value = pool.other_config["vswitch_controller"];
                string[] paramslist = params_value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (paramslist.Count() != 3) return;
                this.textBoxvSwitchControllerIP.Text = paramslist[1];
                this.textBoxPort.Text = paramslist[2];
                if (paramslist[0].ToLowerInvariant() == "ssl")
                {
                    this.radioButtonSSL.Checked = true;
                } 
                else
                {
                    this.radioButtonTCPIP.Checked = true;
                }
            }
            if (other_config.ContainsKey("vswitch_controller_account") && !string.IsNullOrEmpty(pool.other_config["vswitch_controller_account"]))
            {
                this.textBoxUserName.Text = other_config["vswitch_controller_account"];
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Pool pool = this._xenObject as Pool;
            if (pool == null) return;

            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("protocol", this.radioButtonTCPIP.Checked ? this.radioButtonTCPIP.Tag.ToString() : this.radioButtonSSL.Tag.ToString());
            args.Add("controller_ip", this.textBoxvSwitchControllerIP.Text);
            args.Add("controller_port", this.textBoxPort.Text);

            if (checkBoxRegister.Checked)
            {
                args.Add("controller_username", textBoxUserName.Text);
                args.Add("controller_password", textBoxPassword.Text);
                args.Add("vgate_username", this._xenConnection.Username);
                args.Add("vgate_password", this._xenConnection.Password);
                args.Add("master_address", GetMasterAddress(pool));
            }

            OVSCConfigurationAction action = new OVSCConfigurationAction(_xenConnection, pool, args, checkBoxRegister.Checked, 0);
            if (action != null)
            {
                ActionProgressDialog dialog = new ActionProgressDialog(action, ProgressBarStyle.Blocks);
                dialog.ShowCancel = true;
                dialog.ShowDialog(this);
                this.SaveButtonClick(sender, e);
                if (action.Exception == null)
                {
                    this.Close();
                }
            }
        }

        private void textBoxvSwitchControllerIP_TextChanged(object sender, EventArgs e)
        {
            RefreshSaveBtnStatus();
        }

        private void TextBoxvSwitchControllerPort_TextChanged(object sender, EventArgs e)
        {
            RefreshSaveBtnStatus();
        }

        private const string ipPattern = @"(?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))";
        private const string portPattern = @"^(\d{1,4}|([1-5]\d{4})|([1-6][1-4]\d{3})|([1-6][1-4][1-4]{2})|([1-6][1-4][1-4][1-2]\d)|([1-6][1-5][1-5][1-3][1-5]))$";
        private void RefreshSaveBtnStatus()
        {
            bool isIPEmpty = string.IsNullOrEmpty(textBoxvSwitchControllerIP.Text);
            bool isUserNameEmpty = string.IsNullOrEmpty(textBoxUserName.Text);
            bool isPasswordEmpty = string.IsNullOrEmpty(textBoxPassword.Text);
            bool isPortEmpty = string.IsNullOrEmpty(textBoxPort.Text);
            this.buttonSave.Enabled = (checkBoxRegister.Checked
                ? (isIPEmpty || isUserNameEmpty || isPasswordEmpty || isPortEmpty)
                : (isIPEmpty || isPortEmpty))
                ? false
                : (Regex.IsMatch(textBoxvSwitchControllerIP.Text, ipPattern) &&
                   Regex.IsMatch(textBoxPort.Text, portPattern));
        }

        private void checkBoxRegister_CheckStateChanged(object sender, EventArgs e)
        {
            textBoxUserName.Enabled = textBoxPassword.Enabled = checkBoxRegister.Checked;
        }

        private void textBoxUserName_TextChanged(object sender, EventArgs e)
        {
            RefreshSaveBtnStatus();
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            RefreshSaveBtnStatus();
        }

        private string GetMasterAddress(Pool pool)
        {
            Host master = this._xenConnection.Resolve(pool.master);
            if (master == null) return null;
            foreach (PIF pif in this._xenConnection.ResolveAll<PIF>(master.PIFs))
            {
                if (pif.management)
                {
                    return pif.FriendlyIPAddress;
                }
            }

            return null;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
