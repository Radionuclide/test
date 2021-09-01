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
using System.Linq;

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

                if (!VersionCheck.CheckVersion(ibaAnalyzerExe, "7.1.0"))
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

                    if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal) //check for corresponding .lst files
                    {
                        var infos  = Program.CommunicationObject.GetFileInfos2(Path.GetDirectoryName(pdoFile), "*.lst")?.OrderBy(File => File.LastWriteTimeUtc);
                        if (infos.Count() > 0)
                        {
                            var sameName = infos.FirstOrDefault(File => Path.GetFileNameWithoutExtension(File.LocalFileName).ToLower() == Path.GetFileNameWithoutExtension(pdoFile).ToLower());
                            string lstfile = (sameName??infos.Last()).LocalFileName;
                            if (!Program.RemoteFileLoader.DownloadFile(lstfile, out string localLstFile, out string error2))
                            {
                                MessageBox.Show(error2, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
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
            localPath = null; //obsolete...
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
                return true;
            }
            return false;
        }


        void IDatCoHost.UploadPdoFile(bool messageOnNoChanges, Control form, string pdoFilePath, IAnalyzerManagerUpdateSource analyzerManager, IJobData m_data)
        {
            UploadPdoFileWithReturnValue(messageOnNoChanges, form, pdoFilePath, analyzerManager, m_data);
        }

        public bool UploadPdoFileWithReturnValue(bool messageOnNoChanges, Control form, string pdoFilePath, IAnalyzerManagerUpdateSource analyzerManager, IJobData jobData)
        {
            if (form.Disposing || form.IsDisposed)
                return false;

            if (form.InvokeRequired)
            {
                return (bool)form.Invoke(new Func<bool>(() => UploadPdoFileWithReturnValue(messageOnNoChanges, form, pdoFilePath, analyzerManager, jobData)));
            }

            if (!(Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.RunsWithService == Program.ServiceEnum.CONNECTED))
                return false;

            string localFile = Program.RemoteFileLoader.GetLocalPath(pdoFilePath);
            localFile = localFile.Replace(".pdo", "_(ibaDatCoordinator).pdo");

            string lstFileLocal = null;
            string lstFileRemote = null;
            bool lstFileNewOrModified = false;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
            {
                try
                {
                    DateTime newest = DateTime.MinValue;
                    string[] lstFiles = Directory.GetFiles(Path.GetDirectoryName(localFile), "*.lst");
                    foreach (string lstFile in lstFiles)
                    {
                        if (File.Exists(lstFile))
                        {
                            if (Path.GetFileNameWithoutExtension(lstFile).ToLower() == Path.GetFileNameWithoutExtension(localFile).ToLower())
                            {
                                lstFileLocal = lstFile;
                                break;
                            }
                            DateTime writeTime = File.GetLastWriteTime(lstFile);
                            if (writeTime > newest)
                            {
                                lstFileLocal = lstFile;
                                newest = writeTime;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(lstFileLocal))
                    {
                        lstFileRemote = Path.Combine(Path.GetDirectoryName(pdoFilePath), Path.GetFileName(lstFileLocal));
                        lstFileNewOrModified = !Program.CommunicationObject.FileExists(lstFileRemote) || Program.RemoteFileLoader.IsFileChangedLocally(lstFileLocal, lstFileRemote);
                    }
                }
                catch
                {

                }
            }

            if (Program.RemoteFileLoader.IsFileChangedLocally(localFile, pdoFilePath) || lstFileNewOrModified)
            {
                if (MessageBox.Show(form, Program.RunsWithService == Program.ServiceEnum.NOSERVICE ? Properties.Resources.FileChanged_UploadStandalone : Properties.Resources.FileChanged_Upload, "ibaDatCoordinator", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return false;

                form.Cursor = Cursors.WaitCursor;

                //unload ibaAnalyzer from trees
                analyzerManager?.UnLoadAnalysis();


                try
                {
                    bool bStarted = jobData != null && TaskManager.Manager.IsJobStarted(jobData.Guid);
                    if (bStarted)
                        TaskManager.Manager.StopAndWaitForConfiguration((ConfigurationData)jobData);

                    if (!Program.RemoteFileLoader.UploadFile(localFile, pdoFilePath, true, out string error))
                        throw new Exception(error);
                    if (lstFileNewOrModified && !Program.RemoteFileLoader.UploadFile(lstFileLocal, lstFileRemote, true, out string error2))
                        throw new Exception(error2);
                    if (bStarted)
                        TaskManager.Manager.StartConfiguration((ConfigurationData)jobData);

                    form.Cursor = Cursors.Default;
                }
                catch (Exception ex)
                {
                    form.Cursor = Cursors.Default;
                    MessageBox.Show(form, ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //unload ibaAnalyzer from trees
                analyzerManager?.ReopenAnalysis();
                return true;

            }
            else if (messageOnNoChanges)
            {
                MessageBox.Show(form, Properties.Resources.FileChanged_UploadNoChanges, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }            
            return false;
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

        public bool FileExists(string file)
        {
            return DataPath.FileExists(file);
        }

    }
}
