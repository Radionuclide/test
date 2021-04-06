using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;
using iba.Processing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using iba.Controls;
using iba.Data;
using IbaAnalyzer;
using iba.Remoting;

namespace iba.Utility
{
	class DatCoordinatorHostImpl : IDatCoHost
	{

		private DatCoordinatorHostImpl() 
		{
			services = new Dictionary<Type, object>();
			services.Add(typeof(IEncryptionService), new EncryptionService(
				new byte[] { 12, 34, 179, 69, 231, 92 },
				new byte[] {
                    0x2F, 0xB8, 0xB4, 0xAB, 0x01, 0xB6, 0xE3, 0xD9,
					0x9D, 0x5C, 0xAB, 0xD2, 0x64, 0xB2, 0x0B, 0xF5,
					0x69, 0x07, 0x14, 0x98, 0x0B, 0x34, 0x94, 0xF8,
					0x9F, 0xD7, 0x6E, 0x46, 0x58, 0xD5, 0x48, 0x3B
                }));
		}

		private static DatCoordinatorHostImpl m_instance = null;

		public string PathToUnc(string fileName, bool convertLocalPaths)
		{
			return Shares.PathToUnc(fileName, convertLocalPaths);
		}

		public bool TryReconnect(string path, string username, string pass)
		{
			return SharesHandler.Handler.TryReconnect(path, username, pass);
		}

		public bool TestPath(string path, string user, string pass, out string errormessage, bool create)
		{
			return TestPath(path, user, pass, out errormessage, create, false);
		}

		public bool TestPath(string path, string user, string pass, out string errormessage, bool create, bool testWrite)
		{
			if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
				return TaskManager.Manager.TestPath(path, user, pass, out errormessage, create, testWrite);
			else
				return SharesHandler.TestPath(path, user, pass, out errormessage, create, testWrite);
		}


		public void EnableAutoComplete(IntPtr handle, bool directory)
		{
			if (directory)
				WindowsAPI.SHAutoComplete(handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
				SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
			else
				WindowsAPI.SHAutoComplete(handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
				SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
		}


		public static DatCoordinatorHostImpl Host
		{
			get
			{
				if (m_instance == null) m_instance = new DatCoordinatorHostImpl();
				return m_instance;
			}
		}

		public PluginTaskWorkerStatus GetStatusPlugin(Guid guid, int taskindex)
		{
			return TaskManager.Manager.GetStatusPlugin(guid, taskindex);
		}

		public string FindSuitableFileName(string filename)
		{
			string candidate = filename;
			try
			{
				for (int index = 0; File.Exists(candidate); index++)
				{
					int pos = filename.LastIndexOf('.');
					string indexstr = index.ToString("d2");
					candidate = filename.Substring(0, pos) + '_' + indexstr + filename.Substring(pos);
				}
			}
			catch { }
			return candidate;
		}

		public bool BrowseForDatFile(ref string datFile, IJobData data)
		{
			if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
			{
				using (iba.Controls.ServerFolderBrowser fd = new iba.Controls.ServerFolderBrowser(true))
				{
					fd.FixedDrivesOnly = false;
					fd.ShowFiles = true;
					fd.SelectedPath = datFile;
					bool isDat = data?.DatTriggered ?? true;
					fd.Filter = isDat ? Properties.Resources.DatFileFilter : Properties.Resources.HdqDatFileFilter;
					if (fd.ShowDialog() == DialogResult.OK)
					{
						datFile = fd.SelectedPath;
						return true;
					}
					else return false;
				}
			}
			else
			{
				OpenFileDialog dlg = new OpenFileDialog();
				dlg.CheckFileExists = true;
				if (!string.IsNullOrEmpty(datFile))
					dlg.FileName = datFile;
				bool isDat = data?.DatTriggered ?? true;
				dlg.Filter = isDat ? Properties.Resources.DatFileFilter : Properties.Resources.HdqDatFileFilter;
				if (!string.IsNullOrEmpty(datFile) && System.IO.File.Exists(datFile))
					dlg.FileName = datFile;
				else if (System.IO.Directory.Exists(datFile))
					dlg.InitialDirectory = datFile;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					datFile = dlg.FileName;
					return true;
				}
				else return false;
			}
		}
		public void OpenPDO(string pdoFile, string datFile = "")
		{
			try
			{
				RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
				object o = key.GetValue("");
				string ibaAnalyzerExe = Path.GetFullPath(o.ToString());

				if (!Utility.VersionCheck.CheckVersion(ibaAnalyzerExe, "7.1.0"))
				{
					MessageBox.Show(string.Format(Properties.Resources.logAnalyzerVersionError, "7.1.0"), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				string localFile = "";
				if (!string.IsNullOrEmpty(pdoFile))
				{

					if (!Program.RemoteFileLoader.DownloadFile(pdoFile, out localFile, out string error))
					{
						MessageBox.Show(error, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					if (!File.Exists(localFile))
					{
						DialogResult res = MessageBox.Show(Properties.Resources.AnalysisDoesNotExist, "ibaDatCoordinator", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
						if (res == DialogResult.No)
							return;
					}
					else
					{
						string copyForInteractiveEdit = localFile.Replace(".pdo", "_(ibaDatCoordinator).pdo");

						if (!File.Exists(copyForInteractiveEdit) || File.GetLastWriteTime(copyForInteractiveEdit) < File.GetLastWriteTime(localFile))
							File.Copy(localFile, copyForInteractiveEdit, true);
						if (File.Exists(copyForInteractiveEdit))
							localFile = copyForInteractiveEdit;
					}
				}
				using (Process ibaProc = new Process())
				{
					ibaProc.EnableRaisingEvents = false;
					ibaProc.StartInfo.FileName = ibaAnalyzerExe;
					ibaProc.StartInfo.Arguments = "\"" + localFile + "\"";
                    if (!String.IsNullOrEmpty(datFile) && File.Exists(datFile))
                        ibaProc.StartInfo.Arguments += " \"" + datFile;
                    ibaProc.Start();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public bool BrowseForPdoFile(ref string m_pdoFilePath, out string localPath)
		{
			bool bLocal = false;
			DialogResult result = DialogResult.Abort;
			string path = m_pdoFilePath;
			if (Program.RunsWithService != Program.ServiceEnum.NOSERVICE && !Program.ServiceIsLocal)
			{
				if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
				{
					using (iba.Controls.ServerFolderBrowser fd = new iba.Controls.ServerFolderBrowser(true))
					{
						fd.FixedDrivesOnly = false;
						fd.ShowFiles = true;
						fd.SelectedPath = path;
						fd.Filter = Properties.Resources.PdoFileFilter;
						result = fd.ShowDialog();
						path = fd.SelectedPath;
					}
				}
				else
					MessageBox.Show(Properties.Resources.HDEventTask_PDOConnectionRequired, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				bLocal = true;
				using (var dlg = new OpenFileDialog())
				{
					dlg.CheckFileExists = true;
					dlg.FileName = "";
					dlg.Filter = Properties.Resources.PdoFileFilter;
					if (File.Exists(path))
						dlg.FileName = Path.GetFileName(path);
					if (!string.IsNullOrEmpty(path))
					{
						string dir = Path.GetDirectoryName(path);
						if (Directory.Exists(dir))
							dlg.InitialDirectory = dir;
					}
					result = dlg.ShowDialog();
					path = dlg.FileName;
				}
			}
			if (result == DialogResult.OK)
			{
				m_pdoFilePath = path;
				localPath = bLocal ? path : Path.GetFileName(path);
				return true;
			}
			localPath = "";
			return false;
		}

		public void UploadPdoFile(bool messageOnNoChanges, Control form, string m_pdoFilePath, IAnalyzerManagerUpdateSource analyzerManager, IJobData jobData)
		{
			if (form.Disposing || form.IsDisposed)
				return;

			if (form.InvokeRequired)
			{
				form.Invoke(new Action<bool, Control, string, AnalyzerManager, IJobData>(UploadPdoFile), messageOnNoChanges, form, m_pdoFilePath, analyzerManager, jobData);
				return;
			}

			if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.RunsWithService == Program.ServiceEnum.CONNECTED)
			{
				string localFile = Program.RemoteFileLoader.GetLocalPath(m_pdoFilePath);
				string copyForInteractiveEdit = localFile.Replace(".pdo", "_(ibaDatCoordinator).pdo");
				string toDelete = "";
				if (File.Exists(copyForInteractiveEdit) && (!File.Exists(localFile) || File.GetLastWriteTime(copyForInteractiveEdit) > File.GetLastWriteTime(localFile)))
				{
					if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
					{ //copy locally
						analyzerManager?.UnLoadAnalysis();
						try
						{
							File.Copy(copyForInteractiveEdit, localFile, true);
							toDelete = copyForInteractiveEdit;
						}
						catch
						{

						}
					}
					else
					{
						localFile = copyForInteractiveEdit;
						toDelete = copyForInteractiveEdit;
					}
				}

				if (Program.RemoteFileLoader.IsFileChangedLocally(localFile, m_pdoFilePath))
				{
					if (MessageBox.Show(form, Program.RunsWithService == Program.ServiceEnum.NOSERVICE ? Properties.Resources.FileChanged_UploadStandalone : Properties.Resources.FileChanged_Upload, "ibaDatCoordinator", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
						return;

					form.Cursor = Cursors.WaitCursor;

					if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.ServiceIsLocal)
					{ //unload ibaAnalyzer from trees
						analyzerManager?.UnLoadAnalysis();
					}

					try
					{
						bool bStarted = jobData != null && TaskManager.Manager.IsJobStarted(jobData.Guid);
						if (bStarted)
							TaskManager.Manager.StopAndWaitForConfiguration((ConfigurationData)jobData);

						if (!Program.RemoteFileLoader.UploadFile(localFile, m_pdoFilePath, true, out string error))
							throw new Exception(error);

						if (bStarted)
							TaskManager.Manager.StartConfiguration((ConfigurationData)jobData);

						form.Cursor = Cursors.Default;
					}
					catch (Exception ex)
					{
						form.Cursor = Cursors.Default;
						MessageBox.Show(form, ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.ServiceIsLocal)
					{ //unload ibaAnalyzer from trees
						analyzerManager?.ReopenAnalysis();
					}

				}
				else if (messageOnNoChanges)
				{
					MessageBox.Show(form, Properties.Resources.FileChanged_UploadNoChanges, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
					toDelete = "";
				}
				if (!String.IsNullOrEmpty(toDelete))
				{
					try
					{
						File.Delete(toDelete);
					}
					catch
					{
					}
				}
			}
		}

		Dictionary<Type, object> services;

		public T GetService<T>() where T : class
        {
			if (services.TryGetValue(typeof(T), out object service))
				return service as T;
			else
				return null;
        }

        public IbaAnalyzer.IbaAnalyzer CreateIbaAnalyzer()
        {
            return ibaAnalyzerExt.Create(false);
        }
    }
}
