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
using System.Linq;
using System.Windows.Forms;
using XenAdmin.Actions;
using XenAdmin.Controls;
using XenAdmin.Core;
using XenAdmin.Dialogs;
using XenAdmin.Dialogs.WarningDialogs;
using XenAdmin.Network;
using XenAPI;
using System.Drawing;

namespace XenAdmin.Wizards.NewSRWizard_Pages.Frontends
{
    public partial class LVMoMirror : XenTabPage
    {
        //用List存放用于做mirror的两个相同大小的LUN
        public static List<FibreChannelDevice> two_device = new List<FibreChannelDevice>();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<FibreChannelDevice> _selectedDevices = new List<FibreChannelDevice>();
        public LVMoMirror()
        {
            InitializeComponent();
        }
        public override string PageTitle { get { return Messages.NEWSR_SELECT_LUN; } }

        public override string Text { get { return Messages.NEWSR_LOCATION; } }
        public override string HelpID { get { return "Location_Mirror"; } }
        public override void PageLeave(PageLoadedDirection direction, ref bool cancel)
        {
            //将选中的两个LUN放入List
                two_device.Add(_selectedDevices[0]);
                two_device.Add(_selectedDevices[1]);
            if (direction == PageLoadedDirection.Back)
                return;

            Host master = Helpers.GetMaster(Connection);
            if (master == null)
            {
                cancel = true;
                return;
            }
            var descr = new LVMoMirrorSrDescriptor(_selectedDevices, Connection);

            SrDescriptors = new List<LVMoMirrorSrDescriptor>();

            var existingSrDescriptors = new List<LVMoMirrorSrDescriptor>();
            var formatDiskDescriptors = new List<LVMoMirrorSrDescriptor>();

            var action = new SrProbeAction(Connection, master, SR.SRTypes.lvmomirror, descr.DeviceConfig);
            new ActionProgressDialog(action, ProgressBarStyle.Marquee).ShowDialog(this);

            if (!action.Succeeded)
            {
                cancel = true;
                return;
            }

            descr.UUID = SrWizardHelpers.ExtractUUID(action.Result);

            if (!string.IsNullOrEmpty(SrWizardType.UUID))
            {
                // Check LUN contains correct SR
                if (descr.UUID == SrWizardType.UUID)
                {
                    SrDescriptors.Add(descr);
                    return;
                }

                using (var dlog = new ThreeButtonDialog(
                    new ThreeButtonDialog.Details(SystemIcons.Error,
                        String.Format(Messages.INCORRECT_LUN_FOR_SR, SrWizardType.SrName), Messages.XENCENTER)))
                {
                    dlog.ShowDialog(this);
                }

                cancel = true;
                return;
            }

            if (string.IsNullOrEmpty(descr.UUID))
            {
                // No existing SRs were found on this LUN. If allowed to create
                // a new SR, ask the user if they want to proceed and format.
                if (!SrWizardType.AllowToCreateNewSr)
                {
                    using (var dlog = new ThreeButtonDialog(
                        new ThreeButtonDialog.Details(SystemIcons.Error,
                            Messages.NEWSR_LUN_HAS_NO_SRS, Messages.XENCENTER)))
                    {
                        dlog.ShowDialog(this);
                    }

                    cancel = true;
                    return;
                }

                if (!Program.RunInAutomatedTestMode)
                    formatDiskDescriptors.Add(descr);
            }
            else
            {
                // CA-17230: Check this isn't a detached SR. If it is then just continue
                SR sr = SrWizardHelpers.SrInUse(descr.UUID);
                if (sr != null)
                {
                    //SrDescriptors.Add(descr);
                    //return;
                    formatDiskDescriptors.Add(descr);
                }
                else
                {
                    // We found a SR on this LUN. Will ask user for choice later.
                    existingSrDescriptors.Add(descr);
                }
            }

            if (!cancel && existingSrDescriptors.Count > 0)
            {
                var launcher = new LVMoMIRRORWarningDialogLauncher(this, existingSrDescriptors, true);
                launcher.ShowWarnings();
                cancel = launcher.Cancelled;
                if (!cancel && launcher.SrDescriptors.Count > 0)
                    SrDescriptors.AddRange(launcher.SrDescriptors);
            }

            if (!cancel && formatDiskDescriptors.Count > 0)
            {
                var launcher = new LVMoMIRRORWarningDialogLauncher(this, formatDiskDescriptors, false);
                launcher.ShowWarnings();
                cancel = launcher.Cancelled;
                if (!cancel && launcher.SrDescriptors.Count > 0)
                    SrDescriptors.AddRange(launcher.SrDescriptors);
            }

            base.PageLeave(direction, ref cancel);
        }
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_srWizardType.SrToReattach != null)
                return;

            if (e.ColumnIndex != colCheck.Index || e.RowIndex < 0 || e.RowIndex > dataGridView.RowCount - 1)
                return;

            var deviceRow = dataGridView.Rows[e.RowIndex] as FCDeviceRow;
            if (deviceRow == null)
                return;

            deviceRow.Cells[colCheck.Index].Value = !(bool)deviceRow.Cells[colCheck.Index].Value;

            UpdateSelectedDevices();
            OnPageUpdated();
        }
        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (_srWizardType.SrToReattach == null)
                return;

            UpdateSelectedDevices();
            OnPageUpdated();
        }
        private void UpdateSelectedDevices()
        {
            if (SrWizardType.SrToReattach == null)
            {
                //when creating a new SR the checkbox column is visible
                _selectedDevices = (from DataGridViewRow row in dataGridView.Rows
                                    let deviceRow = row as FCDeviceRow
                                    where deviceRow != null
                                          && deviceRow.Cells.Count > 0
                                          && (bool)(deviceRow.Cells[colCheck.Index].Value)
                                    select deviceRow.Device).ToList();
            }
            else
            {
                //when reattaching SR the checkbox column is hidden
                _selectedDevices = (from DataGridViewRow row in dataGridView.Rows
                                    let deviceRow = row as FCDeviceRow
                                    where deviceRow != null && deviceRow.Selected
                                    select deviceRow.Device).ToList();
            }
            if (_selectedDevices.Count == 2)
            {
                if (_selectedDevices[0].Size != _selectedDevices[1].Size)
                {
                   MessageBox.Show(Messages.SELECT_LUN_WARNING,
                                     Messages.MESSAGEBOX_CONFIRM,
                                     MessageBoxButtons.OK);
                }
            }
        }
        public static bool FiberChannelScan(IWin32Window owner, IXenConnection connection, out List<FibreChannelDevice> devices)
        {
            devices = new List<FibreChannelDevice>();

            Host master = Helpers.GetMaster(connection);
            if (master == null)
                return false;

            FibreChannelProbeAction action = new FibreChannelProbeAction(master);
            ActionProgressDialog dialog = new ActionProgressDialog(action, ProgressBarStyle.Marquee);
            dialog.ShowDialog(owner); //Will block until dialog closes, action completed

            if (!action.Succeeded)
                return false;

            try
            {
                devices=FibreChannelProbeParsing.ProcessXML(action.Result);

                if (devices.Count == 0)
                {
                    new ThreeButtonDialog(
                        new ThreeButtonDialog.Details(SystemIcons.Warning, Messages.FIBRECHANNEL_NO_RESULTS, Messages.XENCENTER)).ShowDialog();

                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                log.Debug("Exception parsing result of fibre channel scan", e);
                log.Debug(e, e);
                new ThreeButtonDialog(
                    new ThreeButtonDialog.Details(SystemIcons.Warning, Messages.FIBRECHANNEL_XML_ERROR, Messages.XENCENTER)).ShowDialog();

                return false;
            }
        }
        public List<FibreChannelDevice> FCDevices { private get; set; }
        private SrWizardType _srWizardType;
        public SrWizardType SrWizardType
        {
            private get
            {
                return _srWizardType;
            }
            set
            {
                _srWizardType = value;

                bool creatingNew = _srWizardType.SrToReattach == null;

                colCheck.Visible = creatingNew;
                dataGridView.MultiSelect = creatingNew;
            }
        }
        private class VendorRow : DataGridViewRow
        {
            public VendorRow(string vendor)
            {
                Cells.AddRange(new DataGridViewCheckBoxCellVendor(),
                    new DataGridViewTextBoxCell { Value = vendor },
                    new DataGridViewTextBoxCell(),
                    new DataGridViewTextBoxCell(),
                    new DataGridViewTextBoxCell());
            }

            private class DataGridViewCheckBoxCellVendor : DataGridViewCheckBoxCell
            {
                protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
                {
                    using (var normalBrush = new SolidBrush(OwningRow.DefaultCellStyle.BackColor))
                    using (var selectedBrush = new SolidBrush(OwningRow.DefaultCellStyle.SelectionBackColor))
                    {
                        graphics.FillRectangle(
                            (elementState & DataGridViewElementStates.Selected) != 0 ? selectedBrush : normalBrush,
                            cellBounds.X, cellBounds.Y, cellBounds.Width, cellBounds.Height);
                    }
                }
            }
        }
        private class FCDeviceRow : DataGridViewRow
        {
            public FibreChannelDevice Device { get; private set; }

            public FCDeviceRow(FibreChannelDevice device)
            {
                Device = device;

                string id = string.IsNullOrEmpty(device.SCSIid) ? device.Path : device.SCSIid;
                string details = String.Format("{0}:{1}:{2}:{3}", device.adapter, device.channel, device.id, device.lun);

                Cells.AddRange(new DataGridViewCheckBoxCell { ThreeState = false, Value = false },
                    new DataGridViewTextBoxCell { Value = Util.DiskSizeString(device.Size) },
                    new DataGridViewTextBoxCell { Value = device.Serial },
                    new DataGridViewTextBoxCell { Value = id },
                    new DataGridViewTextBoxCell { Value = details });
            }
        }
        public enum UserSelectedOption { Cancel, Reattach, Format, Skip }
        public override void PopulatePage()
        {
            dataGridView.Rows.Clear();

            var vendorGroups = from device in FCDevices
                               group device by device.Vendor into g
                               orderby g.Key
                               select new { VendorName = g.Key, Devices = g.OrderBy(x => x.Serial) };

            foreach (var vGroup in vendorGroups)
            {
                var vendorRow = new VendorRow(vGroup.VendorName);
                dataGridView.Rows.Add(vendorRow);

                using (var font = new Font(dataGridView.DefaultCellStyle.Font, FontStyle.Bold))
                    vendorRow.DefaultCellStyle = new DataGridViewCellStyle(dataGridView.DefaultCellStyle)
                    {
                        Font = font,
                        SelectionBackColor = dataGridView.DefaultCellStyle.BackColor,
                        SelectionForeColor = dataGridView.DefaultCellStyle.ForeColor
                    };

                var deviceRows = from device in vGroup.Devices select new FCDeviceRow(device);
                dataGridView.Rows.AddRange(deviceRows.ToArray());
            }
        }

        public List<LVMoMirrorSrDescriptor> SrDescriptors { get; private set; }
        public override bool EnableNext()
        {
            return _selectedDevices.Count == 2 && _selectedDevices[0].Size == _selectedDevices[1].Size;
        }

        public override bool EnablePrevious()
        {
            if (SrWizardType.DisasterRecoveryTask && SrWizardType.SrToReattach == null)
                return false;

            return true;
        }
        private class LVMoMIRRORWarningDialogLauncher
        {
            private readonly List<LVMoMirrorSrDescriptor> inputSrDescriptors;
            private readonly bool foundExistingSRs;
            private readonly IWin32Window owner;

            public List<LVMoMirrorSrDescriptor> SrDescriptors { get; private set; }
            public bool Cancelled { get; private set; }

            public LVMoMIRRORWarningDialogLauncher(IWin32Window owner, List<LVMoMirrorSrDescriptor> srDescriptors,
                bool foundExistingSRs)
            {
                this.owner = owner;
                this.foundExistingSRs = foundExistingSRs;
                inputSrDescriptors = srDescriptors;
                SrDescriptors = new List<LVMoMirrorSrDescriptor>();
            }

            private UserSelectedOption GetSelectedOption(LVMoMirrorSrDescriptor lvmOmirrorSrDescriptor,
                out bool repeatForRemainingLUNs)
            {
                int remainingCount = inputSrDescriptors.Count - 1 - inputSrDescriptors.IndexOf(lvmOmirrorSrDescriptor);

                using (var dialog = new LVMoMirrorWarningDialog(lvmOmirrorSrDescriptor.Device, remainingCount, foundExistingSRs))
                {
                    dialog.ShowDialog(owner);
                    repeatForRemainingLUNs = dialog.RepeatForRemainingLUNs;
                    return dialog.SelectedOption;
                }
            }

            public void ShowWarnings()
            {
                bool repeatForRemainingLUNs = false;
                UserSelectedOption selectedOption = UserSelectedOption.Cancel;

                foreach (LVMoMirrorSrDescriptor lvmOmirrorSrDescriptor in inputSrDescriptors)
                {
                    if (!repeatForRemainingLUNs)
                    {
                        selectedOption = GetSelectedOption(lvmOmirrorSrDescriptor, out repeatForRemainingLUNs);
                    }

                    switch (selectedOption)
                    {
                        case UserSelectedOption.Format:
                            lvmOmirrorSrDescriptor.UUID = null;
                            SrDescriptors.Add(lvmOmirrorSrDescriptor);
                            break;
                        case UserSelectedOption.Reattach:
                            SrDescriptors.Add(lvmOmirrorSrDescriptor);
                            break;
                        case UserSelectedOption.Cancel:
                            SrDescriptors.Clear();
                            Cancelled = true;
                            return;
                    }
                }
            }
        }


    }

}
