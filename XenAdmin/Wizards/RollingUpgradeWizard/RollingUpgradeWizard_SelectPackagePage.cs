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
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using DiscUtils;
using DiscUtils.Wim;
using XenAdmin.Controls;
using XenAdmin.Core;
using XenAdmin.Dialogs;
using XenAdmin.Controls.Common;
using XenCenterLib.Archive;
using XenOvf;
using XenOvf.Definitions;
using XenOvf.Definitions.VMC;
using XenOvf.Utilities;
using XenAPI;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;


namespace XenAdmin.Wizards.RollingUpgradeWizard
{
	internal partial class RollingUpgradeWizardSelectPackagePage : XenTabPage
	{
		#region Private fields
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string m_packageTypes =".tgz";

		/// <summary>
		/// Stores the last valid selected appliance
		/// </summary>
		private string m_lastValidAppliance;
		
		private EnvelopeType m_selectedOvfEnvelope;
        private bool m_buttonNextEnabled;
		#endregion

        public RollingUpgradeWizardSelectPackagePage()
		{
			InitializeComponent();
            m_ctrlError.HideError();
			m_tlpEncryption.Visible = false;
		}

		#region Base class (XenTabPage) overrides

		/// <summary>
		/// Gets the page's title (headline)
		/// </summary>
		public override string PageTitle { get { return Messages.IMPORT_SOURCE_PAGE_TITLE; } }

		/// <summary>
		/// Gets the page's label in the (left hand side) wizard progress panel
		/// </summary>
		public override string Text { get { return Messages.IMPORT_SOURCE_PAGE_TEXT; } }

		/// <summary>
		/// Gets the value by which the help files section for this page is identified
		/// </summary>
		public override string HelpID { get { return "Source"; } }

        protected override bool ImplementsIsDirty()
        {
            return true;
        }

		public override void PageLeave(PageLoadedDirection direction, ref bool cancel)
		{
			if (direction == PageLoadedDirection.Forward && IsDirty)
			{
				if (IsUri() && !CheckDownloadFromUri())
				{
					cancel = true;
					base.PageLeave(direction, ref cancel);
					return;
				}

                if (!PerformCheck(CheckIsSupportedType, CheckPathExists))
				{
					cancel = true;
					base.PageLeave(direction, ref cancel);
					return;
				}

                if (!PerformCheck(CheckIsCompressed))
				{
					cancel = true;
					base.PageLeave(direction, ref cancel);
					return;
				}

				var checks = new List<CheckDelegate>();

				if (!PerformCheck(checks.ToArray()))
				{
					cancel = true;
					base.PageLeave(direction, ref cancel);
					return;
				}
			}
			base.PageLeave(direction, ref cancel);
		}

        public override void PopulatePage()
		{
            lblIntro.Text = Messages.IMPORT_UPGRADE_PAGE_TGZ_ONLY;
		}

        public override bool EnableNext()
        {
            return m_buttonNextEnabled;
        }

		#endregion

		#region Accessors

		public bool OvfModeOnly { private get; set; }

		public string FilePath { get { return m_textBoxFile.Text; } }

		/// <summary>
		/// Maybe null if no valid ovf has been found
		/// </summary>
		public EnvelopeType SelectedOvfEnvelope
		{
			get { return m_selectedOvfEnvelope; }
			private set
			{
				m_selectedOvfEnvelope = value;

				if (m_selectedOvfEnvelope == null)
					_SelectedOvfPackage = null;
				else
					_SelectedOvfPackage = XenOvf.Package.Create(m_textBoxFile.Text);
			}
		}

        /// <summary>
		/// XenOvf.Package is the proper abstraction for the wizard to use for most cases instead of just OVF.
        /// Maintain it along side SelectedOvf until it can be phased out.
		/// </summary>
        public XenOvf.Package SelectedOvfPackage
        {
            get
            {
				if (m_selectedOvfEnvelope == null)
                    return null;

                return _SelectedOvfPackage;
            }
        }
        XenOvf.Package _SelectedOvfPackage;

		public ulong ImageLength { get; private set; }

		public bool IsWIM { get; private set; }

		public bool IsXvaVersion1 { get; private set; }

		public ulong DiskCapacity { get; private set; }

		#endregion

		public void SetFileName(string fileName)
		{
			m_textBoxFile.Text = fileName;
		}
        public String GetFileName()
        {
            return m_textBoxFile.Text;
        }

		#region Private methods

        /// <summary>
        /// Performs certain checks on the pages's input data and shows/hides an error accordingly
        /// </summary>
        /// <param name="checks">The checks to perform</param>
        private bool PerformCheck(params CheckDelegate[] checks)
        {
            m_buttonNextEnabled = m_ctrlError.PerformCheck(checks);
            OnPageUpdated();
            return m_buttonNextEnabled;
        }

		private bool GetDiskCapacityXva(out string error)
		{
			error = string.Empty;

			try
			{
				FileInfo info = new FileInfo(m_textBoxFile.Text);
				ImageLength = info.Length > 0 ? (ulong)info.Length : 0;

				DiskCapacity = IsXvaVersion1
				               	? GetTotalSizeFromXmlGeneva() //Geneva style
				               	: GetTotalSizeFromXmlXva(GetXmlStringFromTarXVA()); //xva style
			}
			catch (Exception)
			{
				DiskCapacity = ImageLength;
			}
			return true;
		}

		private ulong GetTotalSizeFromXmlGeneva()
		{
			ulong totalSize = 0;
			XmlDocument xmlMetadata = new XmlDocument();
			xmlMetadata.Load(m_textBoxFile.Text);
			XPathNavigator nav = xmlMetadata.CreateNavigator();
			XPathNodeIterator nodeIterator = nav.Select(".//vdi");

			while (nodeIterator.MoveNext())
				totalSize += UInt64.Parse(nodeIterator.Current.GetAttribute("size", ""));

			return totalSize;
		}

		private string GetXmlStringFromTarXVA()
		{
			using (Stream stream = new FileStream(m_textBoxFile.Text, FileMode.Open, FileAccess.Read))
			{
			    ArchiveIterator iterator = ArchiveFactory.Reader(ArchiveFactory.Type.Tar, stream);
                if( iterator.HasNext() )
                {
                    Stream ofs = new MemoryStream();
                    iterator.ExtractCurrentFile(ofs);
                    return new StreamReader(ofs).ReadToEnd();
                }

			    return String.Empty;
			}
		}

		private ulong GetTotalSizeFromXmlXva(string xmlString)
		{
			ulong totalSize = 0;
			XmlDocument xmlMetadata = new XmlDocument();
			xmlMetadata.LoadXml(xmlString);
			XPathNavigator nav = xmlMetadata.CreateNavigator();
			XPathNodeIterator nodeIterator = nav.Select(".//name[. = \"virtual_size\"]");

			while (nodeIterator.MoveNext())
			{
				XPathNavigator vdiNavigator = nodeIterator.Current;

				if (vdiNavigator.MoveToNext())
					totalSize += UInt64.Parse(vdiNavigator.Value);
			}
			return totalSize;
		}

		private bool GetDiskCapacityImage(out string error)
		{
			error = string.Empty;
			string filename = m_textBoxFile.Text;

			FileInfo info = new FileInfo(filename);
			ImageLength = info.Length > 0 ? (ulong)info.Length : 0;

			if (IsWIM)
			{
				try
				{
					using (FileStream wimstream = new FileStream(filename, FileMode.Open, FileAccess.Read))
					{
						WimFile wimDisk = new WimFile(wimstream);
						string manifest = wimDisk.Manifest;
						Wim_Manifest wimManifest = (Wim_Manifest)Tools.Deserialize(manifest, typeof(Wim_Manifest));
						DiskCapacity = wimManifest.Image[wimDisk.BootImage].TotalBytes; //image data size
						return true;
					}
				}
				catch (Exception)
				{
					error = Messages.IMAGE_SELECTION_PAGE_ERROR_CORRUPT_FILE;
					return false;
				}
			}

			try
			{
				using (VirtualDisk vd = VirtualDisk.OpenDisk(filename, FileAccess.Read))
				{
					DiskCapacity = (ulong)vd.Capacity;
					return true;
				}
			}
			catch (IOException ioe)
			{
				error = ioe.Message.Contains("Invalid VMDK descriptor file")
				        	? Messages.IMAGE_SELECTION_PAGE_ERROR_INVALID_VMDK_DESCRIPTOR
				        	: Messages.IMAGE_SELECTION_PAGE_ERROR_INVALID_FILE_TYPE;
				return false;
			}
			catch (Exception)
			{
				error = Messages.IMAGE_SELECTION_PAGE_ERROR_CORRUPT_FILE;
				return false;
			}
		}

		private bool LoadAppliance(out string error)
		{
			error = string.Empty;

			if (m_lastValidAppliance == m_textBoxFile.Text)
				return true;

			SelectedOvfEnvelope = GetOvfEnvelope(out error);
			if (SelectedOvfEnvelope != null)
			{
				m_lastValidAppliance = m_textBoxFile.Text;
				return true;
			}

			return false;
		}

		private EnvelopeType GetOvfEnvelope(out string error)
		{
			error = string.Empty;
			string path = m_textBoxFile.Text;
			string extension = Path.GetExtension(path).ToLower();

			if (extension == ".ovf")
			{
			    EnvelopeType env = null;

                try
                {
                    env = OVF.Load(path);
                }
                catch(OutOfMemoryException ex)
                {
                    log.ErrorFormat("Failed to load OVF {0} as we ran out of memory: {1}", path, ex.Message);
                    env = null;
                }

				if (env == null)
				{
					error = Messages.INVALID_OVF;
					return null;
				}

				return env;
			}

			if (extension == ".ova")
			{
                if (!CheckSourceIsWritable(out error))
                    return null;

				try
				{
					string x = OVF.ExtractOVFXml(path);
					var env = Tools.DeserializeOvfXml(x);
					if (env == null)
					{
						error = Messages.IMPORT_SELECT_APPLIANCE_PAGE_ERROR_CORRUPT_OVA;
						return null;
					}                   
					return env;
				}
				catch (Exception)
				{
					error = Messages.IMPORT_SELECT_APPLIANCE_PAGE_ERROR_CORRUPT_OVA;
					return null;
				}
			}

			return null;
		}

		private bool IsUri()
		{
			Regex regex = new Regex(Messages.URI_REGEX, RegexOptions.IgnoreCase);
			return regex.Match(m_textBoxFile.Text).Success;
		}

		/// <summary>
		/// Check on the fly
		/// </summary>
		private bool CheckPathValid(out string error)
		{
			error = string.Empty;

			if (String.IsNullOrEmpty(m_textBoxFile.Text))
				return false;

			//if it's URI ignore
			if (IsUri())
				return true;

			if (!PathValidator.IsPathValid(m_textBoxFile.Text))
			{
				error = Messages.IMPORT_SELECT_APPLIANCE_PAGE_ERROR_INVALID_PATH;
				return false;
			}

			return true;
		}

		/// <summary>
		/// Check when we leave the page
		/// </summary>
		private bool CheckIsSupportedType(out string error)
		{
			error = string.Empty;
			IsWIM = false;

			string filepath = m_textBoxFile.Text;

            if (filepath.ToLower().EndsWith(m_packageTypes))
              {
                      return true;
              }

			error = Messages.ERROR_UNSUPPORTED_FILE_TYPE;
			return false;
		}

		/// <summary>
		/// Check when we leave the page
		/// </summary>
		private bool CheckPathExists(out string error)
		{
			error = string.Empty;

			if (File.Exists(m_textBoxFile.Text))
				return true;

			error = Messages.IMPORT_SELECT_APPLIANCE_PAGE_ERROR_NONE_EXIST_PATH;
			return false;
		}

        /// <summary>
        /// Check the source folder is writable
        /// </summary>
        private bool CheckSourceIsWritable(out string error)
        {
            error = string.Empty;

            var directory = Path.GetDirectoryName(m_textBoxFile.Text);
            var touchFile = Path.Combine(directory, "_");

            try
            {
                //attempt writing in the destination directory
                using (FileStream fs = File.OpenWrite(touchFile))
                {
                    //it works
                }
                File.Delete(touchFile);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                error = Messages.IMPORT_SELECT_APPLIANCE_PAGE_ERROR_SOURCE_IS_READONLY;
                return false;
            }
        }

		/// <summary>
		/// Check when we leave the page
		/// </summary>
		private bool CheckIsCompressed(out string error)
		{
		    error = string.Empty;
			string fileName = m_textBoxFile.Text;
			string extension = Path.GetExtension(fileName).ToLower();

			if (extension == ".gz")
			{
                if (!CheckSourceIsWritable(out error))
                    return false;

				string outfile = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName));

				using (var dlog = new DecompressApplianceDialog(fileName, outfile))
				{
					if (dlog.ShowDialog(ParentForm) == DialogResult.Yes)
					{
						m_textBoxFile.Text = outfile;
						return true;
					}
					return false; //user cancelled or error
				}
			}
			return true;//it's not compressed, ok to continue
		}

		private bool CheckDownloadFromUri()
		{
			using (var dlog = new DownloadApplianceDialog(new Uri(m_textBoxFile.Text)))
			{
				if (dlog.ShowDialog(ParentForm) == DialogResult.Yes && !string.IsNullOrEmpty(dlog.DownloadedPath))
				{
					m_textBoxFile.Text = dlog.DownloadedPath;
					return true;
				}
				return false; //an error happened during download or the user cancelled
			}
		}
		
		#endregion

		#region Control event handlers

        /// <summary>
        /// get patch package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void m_buttonBrowse_Click(object sender, EventArgs e)
		{
			using (FileDialog ofd = new OpenFileDialog
			                        	{
			                        		CheckFileExists = true,
			                        		CheckPathExists = true,
			                        		DereferenceLinks = true,
                                            Filter = Messages.PATCH_PACKAGE_HALSIGNFILETYPES,
			                        		RestoreDirectory = true,
			                        		Multiselect = false,
			                        	})
			{
				if (ofd.ShowDialog() == DialogResult.OK)
					m_textBoxFile.Text = ofd.FileName;
			}
		}

		private void m_textBoxImage_TextChanged(object sender, EventArgs e)
		{
			m_tlpEncryption.Visible = false;
			PerformCheck(CheckPathValid);
			IsDirty = true;
		}
        public IEnumerable<Host> SelectedMasters { private get; set; }

		#endregion

        private void ExtractTGZ(String gzArchiveName, String destFolder)
        {

            Stream inStream = File.OpenRead(gzArchiveName);
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(destFolder);
            tarArchive.Close();

            gzipStream.Close();
            inStream.Close();
        }

        public bool isPrecheck()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            bool isprecheck = true;
            ExtractTGZ(m_textBoxFile.Text, folder);
            string[] lines = File.ReadAllLines(folder + 
                Path.DirectorySeparatorChar + 
                Path.GetFileNameWithoutExtension(m_textBoxFile.Text) +
                Path.DirectorySeparatorChar + 
                "patch.cfg");
            foreach (string line in lines)
            {
                if (line.StartsWith("pre_check"))
                {
                    if (line.Substring(line.IndexOf("=") + 1) == "false")
                    {
                        isprecheck = false;
                    }
                }
            }

            Directory.Delete(folder +
                Path.DirectorySeparatorChar +
                Path.GetFileNameWithoutExtension(m_textBoxFile.Text), true);
            return isprecheck;
        }
    }
}
