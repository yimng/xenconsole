using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using XenAdmin.Actions;
using XenAPI;

namespace XenAdmin.SettingsPanels
{
    public partial class UseSSDCachePage : UserControl, IEditPage
    {
        private VDI vdi;
        private bool currentValue;
        public UseSSDCachePage()
        {
            InitializeComponent();
            Text = Messages.SSD_CACHE;
            ssdCacheWarningImage.Image = SystemIcons.Information.ToBitmap().GetThumbnailImage(16, 16, null, IntPtr.Zero);
        }

        public bool HasChanged
        {
            get { return useSSDCacheCheckBox.Checked != currentValue; }
        }

        public Image Image
        {
            get
            {
                return Properties.Resources._000_SSDCache_h32bit_16;
            }
        }

        public string SubText
        {
            get { return useSSDCacheCheckBox.Checked ? Messages.ENABLED : Messages.DISABLED; }
        }

        public bool ValidToSave
        {
            get
            {
                return true;
            }
        }

        public void Cleanup()
        { }

        public AsyncAction SaveSettings()
        {
            if (!HasChanged)
            {
                return null;
            }
            if (useSSDCacheCheckBox.Checked)
                return new DelegatedAsyncAction(
                    vdi.Connection,
                    Messages.ACTION_ENABLE_SSD_CACHE,
                    "",
                    "",
                    delegate (Session session) {
                        VDI.set_allow_caching(session, this.vdi.opaque_ref, true);
                    },
                    true,
                    "vdi.set_allow_caching"
                );
            else
                return new DelegatedAsyncAction(
                   vdi.Connection,
                   Messages.ACTION_DISABLE_SSD_CACHE,
                   "",
                   "",
                   delegate (Session session) {
                       VDI.set_allow_caching(session, this.vdi.opaque_ref, false);
                   },
                   true,
                   "vdi.set_allow_caching"
               );
        }

        public void SetXenObjects(IXenObject orig, IXenObject clone)
        {
            if (!(clone is VDI))
                return;
            vdi = clone as VDI;
            PopulatePage();
        }

        public void ShowLocalValidationMessages()
        { }

        private void PopulatePage()
        {
            currentValue = VDI.get_allow_caching(this.vdi.Connection.Session, this.vdi.opaque_ref);
            useSSDCacheCheckBox.Checked = currentValue;
            useSSDCacheCheckBox.Enabled = !((vdi.GetVMs()).Any(vm => vm.IsRunning));
        }
    }
}
