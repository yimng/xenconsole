using System;
using System.Collections.Generic;
using System.Windows.Forms;
using XenAPI;
using XenAdmin.Actions;
using XenAdmin.Network;
using XenAdmin.Core;
using System.Threading;
using XenAdmin.Controls;
using XenAdmin.Dialogs;
using System.Drawing;
using System.Linq;
using XenAdmin.Utils;
using XenAdmin.Wizards.NewSRWizard_Pages;

namespace XenAdmin.Dialogs
{
    public partial class AddMirrorIscsiDialog : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// If an SR already exists on this LUN, this will point to the SR info which indicates that 
        /// the SR should be reattached.
        /// 
        /// If this is null, indicates the LUN should be formatted into a new SR.
        /// </summary>
        private SR.SRInfo _srToIntroduce;
        private IXenConnection Connection;
        private SR _sr;
        private string origin_lun_text;
        private string origin_lun;
        private AsyncAction _repairAction;

        private ISCSIPopulateLunsAction IscsiPopulateLunsAction;
        private ISCSIPopulateIQNsAction IscsiPopulateIqnsAction;

        private readonly Dictionary<String, ISCSIInfo> LunMap = new Dictionary<String, ISCSIInfo>();

        private readonly ToolTip TargetIqnToolTip = new ToolTip();

        private const string TARGET = "target";
        private const string PORT = "port";
        private const string TARGETIQN = "targetIQN";
        private const string LUNSERIAL = "LUNSerial3";
        private const string SCSIID = "SCSIid3";
        private const string LUNSERIAL1 = "LUNSerial1";
        private const string SCSIID1 = "SCSIid1";
        private const string LUNSERIAL2 = "LUNSerial2";
        private const string SCSIID2 = "SCSIid2";
        private const string CHAPUSER = "chapuser";
        private const string CHAPPASSWORD = "chappassword";

        private IEnumerable<Control> ErrorIcons
        {
            get { return new Control[] { errorIconAtCHAPPassword, errorIconAtHostOrIP, errorIconAtTargetLUN }; }
        }

        private IEnumerable<Control> ErrorLabels
        {
            get { return new Control[] { errorLabelAtHostname, errorLabelAtCHAPPassword, errorLabelAtTargetLUN }; }
        }

        private IEnumerable<Control> SpinnerControls
        {
            get { return new Control[] { spinnerIconAtScanTargetHostButton, spinnerIconAtTargetIqn, spinnerIconAtTargetLun, }; }
        }

        private IEnumerable<Control> UserInputControls
        {
            get
            {
                return new Control[]
                           {
                               textBoxIscsiHost, textBoxIscsiPort, IscsiUseChapCheckBox, IScsiChapUserTextBox,
                               IScsiChapSecretTextBox, scanTargetHostButton, comboBoxIscsiIqns, comboBoxLun,
                           };
            }
        }

        private readonly TemporaryDisablerForControls controlDisabler = new TemporaryDisablerForControls();

        public AddMirrorIscsiDialog()
        {
            InitializeComponent();
            PopulatePage();
        }
        public AddMirrorIscsiDialog(SR sr)
        {
            _sr = sr;
            Connection = sr.Connection;
            InitializeComponent();
            PopulatePage();
        }
        public void PageLeave(PageLoadedDirection direction, ref bool cancel)
        {
            if (direction == PageLoadedDirection.Back)
                return;

            // For Miami hosts we need to ensure an SR.probe()
            // has been performed, and that the user has made a decision. Show the iSCSI choices dialog until
            // they click something other than 'Cancel'. For earlier host versions, warn that data loss may
            // occur.

            Host master = Helpers.GetMaster(Connection);
            if (master == null)
            {
                cancel = true;
                return;
            }

            Dictionary<String, String> dconf = DeviceConfig;
            if (dconf == null)
            {
                cancel = true;
                return;
            }

            // Start probe
            // SrProbeAction IscsiProbeAction = new SrProbeAction(Connection, master, SR.SRTypes.lvmomirror_iscsi, dconf);
            SrProbeAction IscsiProbeAction = new SrProbeAction(Connection, master, SR.SRTypes.iscsi, dconf);
            using (var dialog = new ActionProgressDialog(IscsiProbeAction, ProgressBarStyle.Marquee))
            {
                dialog.ShowCancel = true;
                dialog.ShowDialog(this);
            }

            // Probe has been performed. Now ask the user if they want to Reattach/Format/Cancel.
            // Will return false on cancel
            cancel = !ExamineIscsiProbeResults(IscsiProbeAction);
            iscsiProbeError = cancel;
        }

        bool iscsiProbeError = false;

        public void PopulatePage()
        {
            HideAllErrorIconsAndLabels();

            // Enable IQN scanning
            comboBoxIscsiIqns.Visible = true;

            // IQN's can be very long, so we will show the value as a mouse over tooltip.
            // Initialize the tooltip here.
            TargetIqnToolTip.Active = true;
            TargetIqnToolTip.AutomaticDelay = 0;
            TargetIqnToolTip.AutoPopDelay = 50000;
            TargetIqnToolTip.InitialDelay = 50;
            TargetIqnToolTip.ReshowDelay = 50;
            TargetIqnToolTip.ShowAlways = true;
        }

        private void textBoxIscsiHost_TextChanged(object sender, EventArgs e)
        {
            HideAllErrorIconsAndLabels();
            HideAllSpinnerIcons();
            IScsiParams_TextChanged(null, null);
        }

        /// <summary>
        /// Called when any of the iSCSI filer params change: resets the IQNs/LUNs.
        /// Must be called on the event thread.
        /// </summary>
        private void IScsiParams_TextChanged(object sender, EventArgs e)
        {
            Program.AssertOnEventThread();

            spinnerIconAtScanTargetHostButton.Visible = false;

            // User has changed filer hostname/username/password - clear IQN/LUN boxes
            comboBoxIscsiIqns.Items.Clear();
            comboBoxIscsiIqns.Enabled = false;
            comboBoxLun.Enabled = false;
            labelIscsiIQN.Enabled = false;
            iSCSITargetGroupBox.Enabled = false;

            // Cancel pending IQN/LUN scans
            if (IscsiPopulateIqnsAction != null)
            {
                IscsiPopulateIqnsAction.Cancel();
            }

            ChapSettings_Changed(null, null);
        }

        private bool IsLunInUse()
        {
            SR sr = UniquenessCheck();

            // LUN is not in use iff sr != null

            if (sr == null)
            {
                HideAllErrorIconsAndLabels();
                return false;
            }

            spinnerIconAtTargetLun.Visible = false;

            Pool pool = Helpers.GetPool(sr.Connection);
            if (pool != null)
            {
                errorIconAtTargetLUN.Visible = true;
                errorLabelAtTargetLUN.Visible = true;
                errorLabelAtTargetLUN.Text = String.Format(Messages.NEWSR_LUN_IN_USE_ON_POOL, sr.Name, pool.Name);
                return true;
            }

            Host master = Helpers.GetMaster(sr.Connection);
            if (master != null)
            {
                errorIconAtTargetLUN.Visible = true;
                errorLabelAtTargetLUN.Visible = true;
                errorLabelAtTargetLUN.Text = String.Format(Messages.NEWSR_LUN_IN_USE_ON_SERVER, sr.Name, master.Name);
                return true;
            }

            errorIconAtTargetLUN.Visible = true;
            errorLabelAtTargetLUN.Visible = true;
            errorLabelAtTargetLUN.Text = Messages.NEWSR_LUN_IN_USE;
            return true;
        }

        private void ChapSettings_Changed(object sender, EventArgs e)
        {
            comboBoxLun.Items.Clear();
            comboBoxLun.Text = "";
            comboBoxLun.Enabled = false;

            if (IscsiPopulateLunsAction != null)
            {
                IscsiPopulateLunsAction.Cancel();
            }

            UpdateButtons();
        }

        /// <summary>
        /// Check the current config of the iSCSI sr in the wizard is unique across
        /// all active connections.
        /// </summary>
        /// <returns>SR that uses this config if not unique, null if unique</returns>
        private SR UniquenessCheck()
        {
            // Check currently selected lun is unique amongst other connected hosts.
            foreach (IXenConnection connection in ConnectionsManager.XenConnectionsCopy)
            {
                foreach (SR sr in connection.Cache.SRs)
                {
                    if (sr.GetSRType(false) != SR.SRTypes.iscsi && sr.GetSRType(false) != SR.SRTypes.lvmomirror)
                        continue;

                    if (sr.PBDs.Count < 1)
                        continue;

                    PBD pbd = connection.Resolve(sr.PBDs[0]);

                    if (pbd == null)
                        continue;

                    if (UniquenessCheckMiami(connection, pbd))
                        return sr;
                }
            }

            return null;
        }

        /// <summary>
        /// Check currently LUN against miami host
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="pbd"></param>
        /// <returns></returns>
        private bool UniquenessCheckMiami(IXenConnection connection, PBD pbd)
        {
            if (!pbd.device_config.ContainsKey(SCSIID))
                return false;

            foreach (string s in pbd.device_config.Keys)
            {
                String scsiID = pbd.device_config[s];
                String myLUN = getIscsiLUN();

                if (!LunMap.ContainsKey(myLUN))
                    return false;

                ISCSIInfo info = LunMap[myLUN];
                if (info.ScsiID == scsiID)
                {
                    return true;
                }
            }
            return false;
        }

        private String getIscsiHost()
        {
            // If the user has selected an IQN, use the host from that IQN (due to multi-homing,
            // this may differ from the host they first entered). Otherwise use the host
            // they first entered,
            ToStringWrapper<IScsiIqnInfo> wrapper = comboBoxIscsiIqns.SelectedItem as ToStringWrapper<IScsiIqnInfo>;
            if (wrapper != null)
                return wrapper.item.IpAddress;
            return textBoxIscsiHost.Text.Trim();
        }

        private UInt16 getIscsiPort()
        {
            ToStringWrapper<IScsiIqnInfo> wrapper = comboBoxIscsiIqns.SelectedItem as ToStringWrapper<IScsiIqnInfo>;
            if (wrapper != null)
                return wrapper.item.Port;

            // No combobox item was selected
            UInt16 port;
            if (UInt16.TryParse(textBoxIscsiPort.Text, out port))
            {
                return port;
            }
            else
            {
                return Util.DEFAULT_ISCSI_PORT;
            }
        }

        private String getIscsiIQN()
        {
            ToStringWrapper<IScsiIqnInfo> wrapper = comboBoxIscsiIqns.SelectedItem as ToStringWrapper<IScsiIqnInfo>;
            if (wrapper == null)
                return "";
            else
                return wrapper.item.TargetIQN;
        }

        private void IScsiTargetIqnComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToStringWrapper<IScsiIqnInfo> wrapper = comboBoxIscsiIqns.SelectedItem as ToStringWrapper<IScsiIqnInfo>;

            ClearLunMapAndCombo();
            HideAllErrorIconsAndLabels();

            if (wrapper != null)
            {
                TargetIqnToolTip.SetToolTip(comboBoxIscsiIqns, wrapper.ToString());
                IscsiPopulateLUNs();
            }
            else
            {
                TargetIqnToolTip.SetToolTip(comboBoxIscsiIqns, Messages.SELECT_TARGET_IQN);
            }
            UpdateButtons();
        }

        private String getIscsiLUN()
        {
            return comboBoxLun.Text;
        }

        private void IscsiUseChapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = IscsiUseChapCheckBox.Checked;

            IScsiChapUserTextBox.Enabled = enabled;
            IScsiChapSecretTextBox.Enabled = enabled;
            labelCHAPuser.Enabled = enabled;
            IScsiChapSecretLabel.Enabled = enabled;

            HideAllErrorIconsAndLabels();
            ChapSettings_Changed(null, null);
        }

        private void scanTargetHostButton_Click(object sender, EventArgs e)
        {
            HideAllErrorIconsAndLabels();
            spinnerIconAtTargetIqn.Visible = false;
            spinnerIconAtTargetLun.Visible = false;

            spinnerIconAtScanTargetHostButton.StartSpinning();

            scanTargetHostButton.Enabled = false;
            // For this button to be enabled, we must be Miami or newer
            comboBoxIscsiIqns.Items.Clear();
            // Clear LUNs as they may no longer be valid
            ClearLunMapAndCombo();
            // Cancel any LUN scan in progress, as it is no longer meaningful
            if (IscsiPopulateLunsAction != null)
            {
                IscsiPopulateLunsAction.Cancel();
            }

            UpdateButtons();

            if (IscsiUseChapCheckBox.Checked)
            {
                IscsiPopulateIqnsAction = new ISCSIPopulateIQNsAction(Connection,
                    getIscsiHost(), getIscsiPort(), IScsiChapUserTextBox.Text, IScsiChapSecretTextBox.Text);
            }
            else
            {
                IscsiPopulateIqnsAction = new ISCSIPopulateIQNsAction(Connection,
                    getIscsiHost(), getIscsiPort(), null, null);
            }

            IscsiPopulateIqnsAction.Completed += IscsiPopulateIqnsAction_Completed;

            controlDisabler.Reset();
            controlDisabler.SaveOrUpdateEnabledStates(UserInputControls);
            controlDisabler.DisableAllControls();

            scanTargetHostButton.Enabled = false;
            IscsiPopulateIqnsAction.RunAsync();
        }

        private void ClearLunMapAndCombo()
        {
            // Clear LUNs as they may no longer be valid
            comboBoxLun.Items.Clear();
            comboBoxLun.Text = "";
            comboBoxLun.Enabled = false;
            LunMap.Clear();
            spinnerIconAtTargetIqn.Visible = false;
            spinnerIconAtTargetLun.Visible = false;

            UpdateButtons();
        }

        private void IscsiPopulateIqnsAction_Completed(ActionBase sender)
        {
            Program.Invoke(this, (System.Threading.WaitCallback)IscsiPopulateIqnsAction_Completed_, sender);
        }

        private void IscsiPopulateIqnsAction_Completed_(object o)
        {
            Program.AssertOnEventThread();
            ISCSIPopulateIQNsAction action = (ISCSIPopulateIQNsAction)o;

            controlDisabler.RestoreEnabledOnAllControls();

            if (action.Succeeded)
            {
                if (action.IQNs.Length == 0)
                {
                    // Do nothing: ActionProgressDialog will show Messages.NEWSR_NO_IQNS_FOUND
                }
                else
                {
                    int width = comboBoxIscsiIqns.Width;

                    comboBoxIscsiIqns.Items.Add(Messages.SELECT_TARGET_IQN);

                    foreach (Actions.IScsiIqnInfo iqnInfo in action.IQNs)
                    {
                        if (!String.IsNullOrEmpty(iqnInfo.TargetIQN))
                        {
                            String toString = String.Format("{0} ({1}:{2})", iqnInfo.TargetIQN, iqnInfo.IpAddress, iqnInfo.Port);
                            comboBoxIscsiIqns.Items.Add(new ToStringWrapper<IScsiIqnInfo>(iqnInfo, toString));
                            width = Math.Max(width, Drawing.MeasureText(toString, comboBoxIscsiIqns.Font).Width);
                        }
                    }
                    // Set the combo box dropdown width to accommodate the widest item (within reason)
                    comboBoxIscsiIqns.DropDownWidth = Math.Min(width, Int16.MaxValue);

                    if (comboBoxIscsiIqns.Items.Count > 0)
                    {
                        comboBoxIscsiIqns.SelectedItem = Messages.SELECT_TARGET_IQN;
                        comboBoxIscsiIqns.Enabled = true;
                        labelIscsiIQN.Enabled = true;
                        iSCSITargetGroupBox.Enabled = true;
                    }

                    spinnerIconAtScanTargetHostButton.DisplaySucceededImage();

                    comboBoxIscsiIqns.Focus();
                }
            }
            else
            {
                spinnerIconAtScanTargetHostButton.Visible = false;

                Failure failure = action.Exception as Failure;
                if (failure != null && failure.ErrorDescription[0] == "SR_BACKEND_FAILURE_140")
                {
                    errorIconAtHostOrIP.Visible = true;
                    errorLabelAtHostname.Text = Messages.INVALID_HOST;
                    errorLabelAtHostname.Visible = true;
                    textBoxIscsiHost.Focus();
                }
                else if (failure != null && failure.ErrorDescription[0] == "SR_BACKEND_FAILURE_141")
                {
                    errorIconAtHostOrIP.Visible = true;
                    errorLabelAtHostname.Text = Messages.SR_UNABLE_TO_CONNECT_TO_SCSI_TARGET;
                    errorLabelAtHostname.Visible = true;
                    textBoxIscsiHost.Focus();
                }
                else if (failure != null && failure.ErrorDescription[0] == "SR_BACKEND_FAILURE_68")
                {
                    errorIconAtHostOrIP.Visible = true;
                    errorLabelAtHostname.Text = Messages.LOGGING_IN_TO_THE_ISCSI_TARGET_FAILED;
                    errorLabelAtHostname.Visible = true;
                    textBoxIscsiHost.Focus();
                }
                else
                {
                    errorIconAtHostOrIP.Visible = true;
                    errorLabelAtHostname.Text = failure.ErrorDescription.Count > 2 ? failure.ErrorDescription[2] : failure.ErrorDescription[0];
                    errorLabelAtHostname.Visible = true;
                    textBoxIscsiHost.Focus();

                }
            }
            scanTargetHostButton.Enabled = true;
        }

        private void IscsiPopulateLUNs()
        {
            spinnerIconAtTargetIqn.StartSpinning();

            comboBoxLun.Items.Clear();
            LunMap.Clear();

            if (IscsiUseChapCheckBox.Checked)
            {
                IscsiPopulateLunsAction = new Actions.ISCSIPopulateLunsAction(Connection,
                    getIscsiHost(), getIscsiPort(), getIscsiIQN(), IScsiChapUserTextBox.Text, IScsiChapSecretTextBox.Text);
            }
            else
            {
                IscsiPopulateLunsAction = new Actions.ISCSIPopulateLunsAction(Connection,
                    getIscsiHost(), getIscsiPort(), getIscsiIQN(), null, null);
            }

            IscsiPopulateLunsAction.Completed += IscsiPopulateLunsAction_Completed;

            controlDisabler.Reset();
            controlDisabler.SaveOrUpdateEnabledStates(UserInputControls);
            controlDisabler.DisableAllControls();

            IscsiPopulateLunsAction.RunAsync();
        }

        private void IscsiPopulateLunsAction_Completed(ActionBase sender)
        {
            Program.Invoke(this, (WaitCallback)IscsiPopulateLunsAction_Completed_, sender);
        }

        private void IscsiPopulateLunsAction_Completed_(object o)
        {
            Program.AssertOnEventThread();

            controlDisabler.RestoreEnabledOnAllControls();

            ISCSIPopulateLunsAction action = (ISCSIPopulateLunsAction)o;

            if (!action.Succeeded)
            {
                spinnerIconAtTargetIqn.Visible = false;

                Failure failure = action.Exception as Failure;

                if (failure != null && failure.ErrorDescription != null && failure.ErrorDescription.Count > 0)
                {
                    if (failure.ErrorDescription[0] == "SR_BACKEND_FAILURE_140")
                    {
                        errorIconAtHostOrIP.Visible = true;
                        errorLabelAtHostname.Text = Messages.INVALID_HOST;
                        errorLabelAtHostname.Visible = true;
                        textBoxIscsiHost.Focus();
                    }
                    else if (failure.ErrorDescription[0] == "SR_BACKEND_FAILURE_68")
                    {
                        errorIconAtCHAPPassword.Visible = true;
                        errorLabelAtCHAPPassword.Text = Messages.LOGGING_IN_TO_THE_ISCSI_TARGET_FAILED;
                        errorLabelAtCHAPPassword.Visible = true;
                        IScsiChapUserTextBox.Focus();
                    }
                    else
                    {
                        errorIconAtTargetLUN.Visible = true;
                        errorIconAtTargetLUN.Text = failure.ErrorDescription.Count > 2 ? failure.ErrorDescription[2] : failure.ErrorDescription[0];
                        errorIconAtTargetLUN.Visible = true;
                        textBoxIscsiHost.Focus();
                    }
                }
                return;
            }

            if (action.LUNs.Length == 0)
            {
                // Do nothing: ActionProgressDialog will show Messages.NEWSR_NO_LUNS_FOUND
            }
            else
            {

                foreach (Actions.ISCSIInfo i in action.LUNs)
                {
                    String label = "LUN";
                    if (i.LunID != -1)
                        label += String.Format(" {0}", i.LunID);
                    if (i.Serial != "")
                        label += String.Format(": {0}", i.Serial);
                    if (i.Vendor != "")
                        label += String.Format(" ({0})", i.Vendor);
                    if (i.ScsiID != "")
                        label += String.Format(" ({0})", i.ScsiID);
                    if (i.Size >= 0)
                        label += String.Format(": {0}", Util.DiskSizeString(i.Size));
                    comboBoxLun.Items.Add(label);
                    LunMap.Add(label, i);
                }
                comboBoxLun.Enabled = true;
                Lun.Enabled = true;

                spinnerIconAtTargetIqn.DisplaySucceededImage();
            }
            comboBoxLun.Enabled = true;
            comboBoxIscsiIqns.Enabled = true;

            UpdateButtons();
        }

        /// <summary>
        /// Called with the results of an iSCSI SR.probe(), either immediately after the scan, or after the
        /// user has performed a scan, clicked 'cancel' on a dialog, and then clicked 'next' again (this
        /// avoids duplicate probing if none of the settings have changed).
        /// </summary>
        /// <returns>
        /// Whether to continue or not - wheter to format or not is stored in 
        /// iScsiFormatLUN.
        /// </returns>
        private bool ExamineIscsiProbeResults(SrProbeAction action)
        {
            _srToIntroduce = null;

            if (!action.Succeeded)
            {
                Exception exn = action.Exception;
                log.Warn(exn, exn);
                Failure failure = exn as Failure;
                if (failure != null && failure.ErrorDescription[0] == "SR_BACKEND_FAILURE_140")
                {
                    errorIconAtHostOrIP.Visible = true;
                    errorLabelAtHostname.Visible = true;
                    errorLabelAtHostname.Text = Messages.INVALID_HOST;
                    textBoxIscsiHost.Focus();
                }
                else if (failure != null)
                {
                    errorIconAtHostOrIP.Visible = true;
                    errorLabelAtHostname.Visible = true;
                    errorLabelAtHostname.Text = failure.ErrorDescription.Count > 2 ? failure.ErrorDescription[2] : failure.ErrorDescription[0];
                    textBoxIscsiHost.Focus();
                }
                return false;
            }

            try
            {
                List<SR.SRInfo> SRs = SR.ParseSRListXML(action.Result);

                if (!String.IsNullOrEmpty(SrWizardType.UUID))
                {
                    // Check LUN contains correct SR
                    if (SRs.Count == 1 && SRs[0].UUID == SrWizardType.UUID)
                    {
                        _srToIntroduce = SRs[0];
                        return true;
                    }

                    errorIconAtTargetLUN.Visible = true;
                    errorLabelAtTargetLUN.Visible = true;
                    errorLabelAtTargetLUN.Text = String.Format(Messages.INCORRECT_LUN_FOR_SR, SrWizardType.SrName);

                    return false;
                }
                else if (SRs.Count == 0)
                {
                    // No existing SRs were found on this LUN. If allowed to create new SR, ask the user if they want to proceed and format.
                    if (!SrWizardType.AllowToCreateNewSr)
                    {
                        using (var dlg = new ThreeButtonDialog(
                           new ThreeButtonDialog.Details(SystemIcons.Error, Messages.NEWSR_LUN_HAS_NO_SRS, Messages.XENCENTER)))
                        {
                            dlg.ShowDialog(this);
                        }

                        return false;
                    }
                    DialogResult result = DialogResult.Yes;
                    if (!Program.RunInAutomatedTestMode)
                    {
                        using (var dlg = new ThreeButtonDialog(
                            new ThreeButtonDialog.Details(SystemIcons.Warning, Messages.NEWSR_ISCSI_FORMAT_WARNING, this.Text),
                            ThreeButtonDialog.ButtonYes,
                            new ThreeButtonDialog.TBDButton(Messages.NO_BUTTON_CAPTION, DialogResult.No, ThreeButtonDialog.ButtonType.CANCEL, true)))
                        {
                            result = dlg.ShowDialog(this);
                        }
                    }

                    return result == DialogResult.Yes;
                }
                else
                {
                    // There should be 0 or 1 SRs on the LUN
                    System.Diagnostics.Trace.Assert(SRs.Count == 1);

                    // CA-17230
                    // Check this isn't a detached SR
                    SR.SRInfo info = SRs[0];
                    SR sr = SrWizardHelpers.SrInUse(info.UUID);
                    if (sr != null)
                    {
                        DialogResult res;
                        using (var d = new ThreeButtonDialog(
                            new ThreeButtonDialog.Details(null, string.Format(Messages.DETACHED_ISCI_DETECTED, Helpers.GetName(sr.Connection))),
                            new ThreeButtonDialog.TBDButton(Messages.ATTACH_SR, DialogResult.OK),
                            ThreeButtonDialog.ButtonCancel))
                        {
                            res = d.ShowDialog(Program.MainWindow);
                        }

                        if (res == DialogResult.Cancel)
                            return false;

                        _srToIntroduce = info;
                        return true;
                    }

                    // An SR exists on this LUN. Ask the user if they want to attach it, format it and
                    // create a new SR, or cancel.
                    DialogResult result = Program.RunInAutomatedTestMode ? DialogResult.Yes :
                        new IscsiChoicesDialog(Connection, info).ShowDialog(this);

                    switch (result)
                    {
                        case DialogResult.Yes:
                            // Reattach
                            _srToIntroduce = SRs[0];
                            return true;

                        case DialogResult.No:
                            // Format - SrToIntroduce is already null
                            return true;

                        default:
                            return false;
                    }
                }
            }
            catch
            {
                // We really want to prevent the user getting to the next step if there is any kind of
                // exception here, since clicking 'finish' might destroy data: require another probe.
                return false;
            }
        }

        private void HideAllErrorIconsAndLabels()
        {
            foreach (var c in ErrorIcons.Concat(ErrorLabels))
                c.Visible = false;
        }

        private void HideAllSpinnerIcons()
        {

            foreach (var c in SpinnerControls)
                c.Visible = false;
        }

        #region Accessors

        public SrWizardType SrWizardType { private get; set; }

        public string UUID { get { return _srToIntroduce == null ? null : _srToIntroduce.UUID; } }

        public long SRSize
        {
            get
            {
                ISCSIInfo info = LunMap[getIscsiLUN()];
                return info.Size;
            }
        }

        public Dictionary<String, String> DeviceConfig
        {
            get
            {
                Dictionary<String, String> dconf = new Dictionary<String, String>();
                ToStringWrapper<IScsiIqnInfo> iqn = comboBoxIscsiIqns.SelectedItem as ToStringWrapper<IScsiIqnInfo>;
                if (iqn == null)
                    return null;

                // Reset target IP address to home address specified in IQN scan.
                // Allows multi-homing - see CA-11607
                dconf[TARGET] = iqn.item.IpAddress;
                dconf[PORT] = iqn.item.Port.ToString();
                dconf[TARGETIQN] = getIscsiIQN();

                if (!LunMap.ContainsKey(getIscsiLUN()))
                    return null;

                ISCSIInfo info = LunMap[getIscsiLUN()];
                if (info.LunID == -1)
                {
                    dconf[LUNSERIAL] = info.Serial;
                }
                else
                {
                    dconf[SCSIID] = info.ScsiID;
                }

                if (IscsiUseChapCheckBox.Checked)
                {
                    dconf[CHAPUSER] = IScsiChapUserTextBox.Text;
                    dconf[CHAPPASSWORD] = IScsiChapSecretTextBox.Text;
                }

                return dconf;
            }
        }

        public string SrDescription
        {
            get
            {
                ToStringWrapper<IScsiIqnInfo> iqn = comboBoxIscsiIqns.SelectedItem as ToStringWrapper<IScsiIqnInfo>;
                return iqn == null ? null : string.Format(Messages.NEWSR_ISCSI_DESCRIPTION, iqn.item.IpAddress, getIscsiIQN(), getIscsiLUN());
            }
        }

        #endregion

        private void comboBoxLun_SelectedIndexChanged(object sender, EventArgs e)
        {
            iscsiProbeError = false;
            if (comboBoxLun.SelectedItem as string != Messages.SELECT_TARGET_LUN)
            {
                spinnerIconAtTargetLun.DisplaySucceededImage();
            }
            else
            {
                spinnerIconAtTargetLun.Visible = false;
                HideAllErrorIconsAndLabels();
            }

            UpdateButtons();
        }
        private void UpdateButtons()
        {
            label1.Visible = false;
            Lun_Size_Different.Visible = false;
            if (comboBoxLun.Text!=""&&comboBoxLun.Text!=null)
            {
                origin_lun_text = comboBoxLun.Text.Substring(comboBoxLun.Text.LastIndexOf(":") + 1);
                if (_sr.other_config["mirror_iscsi_size"] != origin_lun_text)
                {
                    Lun_Size_Different.Visible = true;
                    buttonAdd.Enabled = false;
                }
            }
            if (!IsLunInUse()&& _sr.other_config["mirror_iscsi_size"] == origin_lun_text)
            {
                buttonAdd.Enabled = true;
            }
            foreach (XenRef<PBD> p in _sr.PBDs)
            {
                if (Connection.Resolve<PBD>(p).device_config.ContainsKey("SCSIid1"))
                {
                    if (Connection.Resolve<PBD>(p).device_config["SCSIid1"] != "")
                    {
                        origin_lun = Connection.Resolve<PBD>(p).device_config["SCSIid1"];
                    }
                }
                if (Connection.Resolve<PBD>(p).device_config.ContainsKey("SCSIid2"))
                {
                    if (Connection.Resolve<PBD>(p).device_config["SCSIid2"] != "")
                    {
                        origin_lun = Connection.Resolve<PBD>(p).device_config["SCSIid2"];
                    }
                }
            }
            if (comboBoxLun.Text!="")
            {
                int start = comboBoxLun.Text.LastIndexOf("(");
                int end = comboBoxLun.Text.LastIndexOf(")");
                if (comboBoxLun.Text.Substring(start + 1, end - start-1) == origin_lun)
                {
                    buttonAdd.Enabled = false;
                    label1.Visible = true;
                }
            }
            UInt16 i;
            bool portValid = UInt16.TryParse(textBoxIscsiPort.Text, out i);
            scanTargetHostButton.Enabled =
                !String.IsNullOrEmpty(getIscsiHost())
                 && portValid;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            ToStringWrapper<IScsiIqnInfo> iqn = comboBoxIscsiIqns.SelectedItem as ToStringWrapper<IScsiIqnInfo>;
            buttonAdd.Enabled = false;
            buttonCancel.Text = Messages.CLOSE;
            int start = comboBoxLun.Text.LastIndexOf("(");
            int end = comboBoxLun.Text.LastIndexOf(")");
            _repairAction = new SrAddMirrorLUNAction(_sr.Connection, _sr, comboBoxLun.Text.Substring(start + 1,end-start-1),
                                                     iqn.item.IpAddress,iqn.item.TargetIQN, iqn.item.Port.ToString(),IScsiChapUserTextBox.Text,IScsiChapSecretTextBox.Text);
            _repairAction.RunAsync();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
