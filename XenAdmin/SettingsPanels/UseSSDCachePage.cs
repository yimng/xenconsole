using System;
using System.Collections.Generic;
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
            var vms = vdi.GetVMs();
            useSSDCacheCheckBox.Enabled = !(vms.Any(vm => vm.IsRunning)) && ShowSSDCache(vdi);
        }

        private bool ShowSSDCache(VDI vdi)
        {
            var vms = vdi.GetVMs();
            SR vmsr = vdi.Connection.Resolve<SR>(vdi.SR);
            if (!vmsr.IsLocalSR && vmsr.GetSRType(true) != SR.SRTypes.nfs)
            {
                return false;
            }
            var Affinity = vms.Count == 0 ? null : (vms[0] == null ? null : vms[0].GetStorageHost(true));
            List<SR> AllSRs = new List<SR>(vdi.Connection.Cache.SRs);
            List<SR> srs;
            if (Affinity != null)
            {
                srs = new List<SR>();
                foreach (SR sr in AllSRs)
                {
                    if (sr.GetStorageHost() == Affinity)
                    {
                        srs.Add(sr);
                    }
                }
            }
            else
            {
                srs = AllSRs;
            }
            foreach (SR sr in srs)
            {
                if (sr == null || sr.IsToolsSR || !sr.Show(Properties.Settings.Default.ShowHiddenVMs))
                    continue;
                if (sr.GetSRType(true) == SR.SRTypes.ext && SR.get_local_cache_enabled(vdi.Connection.Session, sr.opaque_ref))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
