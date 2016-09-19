﻿/* Copyright (c) Citrix Systems Inc. 
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
using System.Drawing;
using System.Windows.Forms;
using XenAdmin.Core;
using XenAdmin.Wizards.NewSRWizard_Pages;
using XenAdmin.Wizards.NewSRWizard_Pages.Frontends;

namespace XenAdmin.Dialogs.WarningDialogs
{
    public partial class LVMoBondWarningDialog : XenDialogBase
    {
        private Panel highlightedPanel;
        private List<FibreChannelDevice> currentDevice;
        private int remainingDevicesCount;
        private bool foundExistingSR;

        public LVMoBond.UserSelectedOption SelectedOption { get; private set; }
        public bool RepeatForRemainingLUNs { get { return checkBoxRepeat.Checked; } }

        public LVMoBondWarningDialog(List<FibreChannelDevice> currentDevice, int remainingDevicesCount,
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
                                 ? LVMoBond.UserSelectedOption.Format
                                 : LVMoBond.UserSelectedOption.Reattach;

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
            SelectedOption = LVMoBond.UserSelectedOption.Cancel;
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