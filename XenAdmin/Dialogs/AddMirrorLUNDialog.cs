using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XenAdmin.Actions;
using XenAdmin.Wizards.NewSRWizard_Pages;
using XenAPI;

namespace XenAdmin.Dialogs
{
    public partial class AddMirrorLUNDialog : DialogWithProgress
    {
        private readonly SR sr;
        public List<FibreChannelDevice> FCDevices { private get; set; }
        private List<FibreChannelDevice> _selectedDevices = new List<FibreChannelDevice>();
        private AsyncAction _repairAction;

        public AddMirrorLUNDialog(SR sr, List<FibreChannelDevice> list)
        {            
            InitializeComponent();
            this.sr = sr;
            FCDevices = list;
            Shrink();
            PopulatePage();
        }

        private void PopulatePage()
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
            addButton.Enabled = false;
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

        private void replace_Click(object sender, EventArgs e)
        {

            addButton.Enabled = false;
            cancelButton.Text = Messages.CLOSE;
            _repairAction = new SrAddMirrorLUNAction(sr.Connection, sr, _selectedDevices[0].SCSIid);
            DoAction(_repairAction);            
        }

        private void DoAction(AsyncAction action)
        {
            Program.AssertOnEventThread();

            action.Changed += action_Changed;
            action.Completed += action_Completed;

            Grow(action.RunAsync);
        }

        private void action_Changed(ActionBase action)
        {
            Program.Invoke(this, () => UpdateProgressControls(action));
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.ColumnIndex != colCheck.Index || e.RowIndex < 0 || e.RowIndex > dataGridView.RowCount - 1)
                return;

            var deviceRow = dataGridView.Rows[e.RowIndex] as FCDeviceRow;
            if (deviceRow == null)
                return;

            deviceRow.Cells[colCheck.Index].Value = !(bool)deviceRow.Cells[colCheck.Index].Value;

            UpdateSelectedDevices();
        }

        private void UpdateSelectedDevices()
        {          
            //when reattaching SR the checkbox column is hidden
            _selectedDevices = (from DataGridViewRow row in dataGridView.Rows
                                let deviceRow = row as FCDeviceRow
                                where deviceRow != null && deviceRow.Cells.Count > 0
                                          && (bool)(deviceRow.Cells[colCheck.Index].Value)
                                select deviceRow.Device).ToList();
            addButton.Enabled = _selectedDevices.Count == 1 ? true : false;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void action_Completed(ActionBase sender)
        {
            Program.Invoke(this, () =>
            {
                FinalizeProgressControls(sender);
            });
        }
    }
}
