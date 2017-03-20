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
using XenAPI;
using XenAdmin.Dialogs;
using XenAdmin.Controls;
using XenAdmin.Core;
using XenAdmin.Actions.OVSCActions;

namespace XenAdmin.TabPages
{
    public partial class vSwitchControllerPage : BaseTabPage
    {
        public vSwitchControllerPage()
        {
            InitializeComponent();
            base.Text = Messages.OVSC_PAGE_TITLE;
        }

        private void buttonSetController_Click(object sender, EventArgs e)
        {
            if (null != this._pool)
            {
                vSwitchControllerConfigDialog configDialog = new vSwitchControllerConfigDialog(this._pool.Connection, this._pool);
                configDialog.Show(this);
                configDialog.SaveButtonClick += configDialog_SaveButtonClick;
            }
        }

        void configDialog_SaveButtonClick(object sender, EventArgs e)
        {
            this.Rebuild();
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "other_config")
            {
                this.Rebuild();
            }
        }

        public void Rebuild()
        {
            this.customListPanel.BeginUpdate();
            try
            {
                this.customListPanel.ClearRows();
                if (this._pool != null)
                {
                    this.GeneratevSwitchControllerBox();
                }
            }
            finally
            {
                this.customListPanel.EndUpdate();
            }

            this.buttonDeleteConfigure.Enabled = _pool.other_config.ContainsKey("vswitch_controller");
        }

        private void GeneratevSwitchControllerBox()
        {
            CustomListRow row = CreateHeader(Messages.OVSC_CONFIGURE_TITLE);
            this.customListPanel.AddRow(row);
            if (_pool.other_config.ContainsKey("vswitch_controller") && !string.IsNullOrEmpty(_pool.other_config["vswitch_controller"]))
            {
                string params_value = _pool.other_config["vswitch_controller"];
                string[] paramslist = params_value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (paramslist.Count() != 3) return;

                AddRow(row, Messages.OVSC_CONFIGURE_IP, paramslist[1], false);
                AddRow(row, Messages.OVSC_CONFIGURE_PORT, paramslist[2], false);
                AddRow(row, Messages.OVSC_CONFIGURE_PROTOCOL, paramslist[0], false);
                if (_pool.other_config.ContainsKey("vswitch_controller_account") &&
                        !string.IsNullOrEmpty(_pool.other_config["vswitch_controller_account"]))
                {
                    AddRow(row, Messages.OVSC_CONFIGURE_USERNAME, _pool.other_config["vswitch_controller_account"], false);
                }
            }
            else
            {
                AddRow(row, Messages.OVSC_CONFIGURE_IP, Messages.UNKNOWN, false);
                AddRow(row, Messages.OVSC_CONFIGURE_PORT, Messages.UNKNOWN, false);
                AddRow(row, Messages.OVSC_CONFIGURE_PROTOCOL, Messages.UNKNOWN, false);
            }

            AddBottomSpacing(row);
        }

        private CustomListRow CreateHeader(string text)
        {
            return new CustomListRow(text, BaseTabPage.HeaderBackColor, BaseTabPage.HeaderForeColor, BaseTabPage.HeaderBorderColor, Program.DefaultFontHeader);
        }

        private CustomListRow CreateNewRow(string key, string value, bool valueBold)
        {
            CustomListItem item = new CustomListItem(key, BaseTabPage.ItemLabelFont, BaseTabPage.ItemLabelForeColor);
            item.Anchor = AnchorStyles.Top;
            item.itemBorder.Bottom = 4;
            CustomListItem item2 = new CustomListItem(value, valueBold ? BaseTabPage.ItemValueFontBold : BaseTabPage.ItemValueFont, BaseTabPage.ItemValueForeColor);
            item2.Anchor = AnchorStyles.Top;
            item2.itemBorder.Bottom = 4;
            return new CustomListRow(BaseTabPage.ItemBackColor, new CustomListItem[] { item, item2 });
        }

        private void AddBottomSpacing(CustomListRow header)
        {
            if (header.Children.Count != 0)
            {
                header.AddChild(new CustomListRow(header.BackColor, 5));
            }
        }

        private CustomListRow AddRow(CustomListRow parent, string key, string value, bool valueBold)
        {
            CustomListRow row = CreateNewRow(key + ": ", value, valueBold);
            parent.AddChild(row);
            return row;
        }

        private Pool _pool;
        public Pool _Pool
        {
            get { return this._pool; }
            set
            {
                Program.AssertOnEventThread();
                if (value != null)
                {
                    if (this._pool != value)
                    {
                        this._pool = value;
                        this._pool.PropertyChanged += PropertyChanged;
                    }
                }

                this.Rebuild();
            }
        }

        private void buttonDeleteConfigure_Click(object sender, EventArgs e)
        {
            if (new ThreeButtonDialog(
                    new ThreeButtonDialog.Details(null, Messages.MESSAGEBOX_OVSC_DELETE, Messages.MESSAGEBOX_OVSC_DELETE_TITLE),
                    ThreeButtonDialog.ButtonYes,
                    ThreeButtonDialog.ButtonNo).ShowDialog(this) == DialogResult.Yes)
            {
                OVSCConfigurationAction action = new OVSCConfigurationAction(this._pool.Connection, this._pool);
                if (action != null)
                {
                    ActionProgressDialog dialog = new ActionProgressDialog(action, ProgressBarStyle.Blocks);
                    dialog.ShowCancel = true;
                    dialog.ShowDialog(this);
                }
            }
            this.Rebuild();
        }
    }
}
