using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using XenAdmin.Core;
using XenAdmin.Wizards.NewSRWizard_Pages;
using XenAdmin.Wizards.NewSRWizard_Pages.Frontends;

namespace XenAdmin.Dialogs.WarningDialogs
{
    public partial class LVMoMirrorWarningDialog : XenDialogBase
    {
        private Panel highlightedPanel;
        private List<FibreChannelDevice> currentDevice;
        private int remainingDevicesCount;
        private bool foundExistingSR;

        public LVMoMirror.UserSelectedOption SelectedOption { get; private set; }
        public LVMoMirrorChooseLogPage.UserSelectedOption SelectedLogOption { get; private set; }
        public bool RepeatForRemainingLUNs { get { return checkBoxRepeat.Checked; } }

        public LVMoMirrorWarningDialog(List<FibreChannelDevice> currentDevice, int remainingDevicesCount,
            bool foundExistingSR)
        {
            InitializeComponent();
            this.currentDevice = currentDevice;
            this.remainingDevicesCount = remainingDevicesCount;
            this.foundExistingSR = foundExistingSR;
            PopulateControls();
            ActiveControl = buttonCancel;
        }

        private object HighlightedPanel
        {
            get { return highlightedPanel; }
            set
            {
                Panel panel = value as Panel;
                if (panel == highlightedPanel) return;

                SetPanelColor(highlightedPanel, false);
                SetPanelColor(panel, true);
                highlightedPanel = panel;
            }
        }

        private void SetPanelColor(Panel panel, bool highlighted)
        {
            if (panel == null)
                return;

            Color color = highlighted ? SystemColors.ControlLight : SystemColors.Control;
            panel.BackColor = color;

            foreach (var control in panel.Controls)
            {
                if (control is Button)
                {
                    var button = control as Button;
                    button.FlatAppearance.MouseOverBackColor = color;
                    button.FlatAppearance.MouseDownBackColor = color;
                }
            }
        }

        private void PanelClicked()
        {
            Panel panel = HighlightedPanel as Panel;
            if (panel == null)
                return;

            SelectedOption = panel == panelFormat
                                 ? LVMoMirror.UserSelectedOption.Format
                                 : LVMoMirror.UserSelectedOption.Reattach;

            DialogResult = DialogResult.OK;
        }

        private void panelReattach_MouseEnter(object sender, EventArgs e)
        {
            HighlightedPanel = panelReattach;
        }

        private void panelFormat_MouseEnter(object sender, EventArgs e)
        {
            HighlightedPanel = panelFormat;
        }

        private void ExistingSRsWarningDialog_MouseEnter(object sender, EventArgs e)
        {
            HighlightedPanel = null;
        }

        private void panel_Click(object sender, EventArgs e)
        {
            PanelClicked();
        }

        private void PopulateControls()
        {
            labelHeader.Text = foundExistingSR
                                   ? Messages.LVMOHBA_WARNING_DIALOG_HEADER_FOUND_EXISTING_SR
                                   : Messages.LVMOHBA_WARNING_DIALOG_HEADER_NO_EXISTING_SRS;

            checkBoxRepeat.Text = foundExistingSR
                                      ? Messages.LVMOHBA_WARNING_DIALOG_REPEAT_FOR_REMAINING_WITH_SR
                                      : Messages.LVMOHBA_WARNING_DIALOG_REPEAT_FOR_REMAINING_NO_SR;
            checkBoxRepeat.Visible = remainingDevicesCount > 0;

            labelLUNDetails.Text = string.Format(Messages.LVMOBOND_WARNING_DIALOG_LUN_DETAILS, currentDevice[0].Vendor,
                                                 currentDevice[0].Serial,
                                                 string.IsNullOrEmpty(currentDevice[0].SCSIid)
                                                     ? currentDevice[0].Path
                                                     : currentDevice[0].SCSIid,
                                                 Util.DiskSizeString(currentDevice[0].Size),
                                                 currentDevice[1].Vendor,
                                                 currentDevice[1].Serial,
                                                 string.IsNullOrEmpty(currentDevice[1].SCSIid)
                                                     ? currentDevice[1].Path
                                                     : currentDevice[1].SCSIid,
                                                 Util.DiskSizeString(currentDevice[1].Size));

            panelReattach.Enabled = foundExistingSR;
            if (!panelReattach.Enabled)
            {
                labelReattachInfo.Text = Messages.LVMOHBA_WARNING_DIALOG_REATTACH_LABEL_TEXT;
                pictureBoxArrowReattach.Image = Drawing.ConvertToGreyScale(pictureBoxArrowReattach.Image);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            SelectedOption = LVMoMirror.UserSelectedOption.Cancel;
        }

        private void buttonReattach_Click(object sender, EventArgs e)
        {
            HighlightedPanel = panelReattach;
            PanelClicked();
        }

        private void buttonFormat_Click(object sender, EventArgs e)
        {
            HighlightedPanel = panelFormat;
            PanelClicked();
        }

        private void labelLUNDetails_Click(object sender, EventArgs e)
        {

        }
    }
}