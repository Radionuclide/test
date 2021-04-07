using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.Remoting
{
    //TODO test compatibility client/server (newly added methods in CommunicationObject)
    class RemoteFileLoader : IDisposable
    {
        const string LocalDir = @"iba\ibaDatCoordinator\Temp";

        #region Initialize
        public RemoteFileLoader()
        {
            Dispose();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), LocalDir);

                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                    return;

                foreach (var file in dirInfo.EnumerateFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    { }
                }

                dirInfo.Delete();
            }
            catch
            { }
        }
        #endregion

        public string GetLocalPath(string remoteFile)
        {
            try
            {
                if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.ServiceIsLocal)
                    return remoteFile;

                string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), LocalDir);
                string filename = Path.GetFileName(remoteFile);
                return Path.Combine(dirPath, filename);
            }
            catch (Exception ex)
            {
                Logging.ibaLogger.LogFormat(Logging.Level.Exception, "RemotFileLoader GetLocalPath: {0}", ex.Message);
                return null;
            }
        }

        public bool DownloadFile(string remoteFile, out string localFile, out string error)
        {
            localFile = error = "";

            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || localFile == remoteFile)
            {
                localFile = remoteFile;
                return true;
            }

            string filename = "";

            try
            {
                filename = Path.GetFileName(remoteFile);

                if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
                {
                    error = string.Format(Properties.Resources.Download_ErrorDisconnected, filename);
                    return false;
                }

                string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), LocalDir);
                localFile = Path.Combine(dirPath, filename);

                ServerFileInfo[] remoteInfos = Program.CommunicationObject.GetFileInfos(new string[1] { remoteFile });
                if (remoteInfos == null || remoteInfos.Length == 0 || remoteInfos[0] == null)
                {
                    localFile = "";
                    error = string.Format(Properties.Resources.Download_ErrorUnknown, filename);
                    return false;
                }

                ServerFileInfo remoteInfo = remoteInfos[0];

                FileInfo fileInfo = new FileInfo(localFile);
                if (fileInfo.Exists && fileInfo.LastWriteTimeUtc >= remoteInfo.LastWriteTimeUtc)
                    return true;

                DialogResult res = DialogResult.OK;
                using (var dlg = new FilesDownloaderForm(new ServerFileInfo[1] { remoteInfo }, localFile, Program.CommunicationObject.GetServerSideFileHandler(), true))
                {
                    if (Program.MainForm.InvokeRequired)
                       res = (DialogResult) Program.MainForm.Invoke(new Func<DialogResult>(() => {return dlg.ShowDialog(Program.MainForm); }));
                    else
                        res = dlg.ShowDialog(Program.MainForm);
                }

                if (res == DialogResult.OK)
                    return true;

                localFile = "";
                if (res == DialogResult.Abort)
                    error = string.Format(Properties.Resources.Download_ErrorException, filename);
                else if (res == DialogResult.Cancel)
                    error = string.Format(Properties.Resources.Download_ErrorCancelled, filename);
                else
                    error = string.Format(Properties.Resources.Download_ErrorUnknown, filename);

                return false;
            }
            catch(Exception ex)
            {
                localFile = "";
                error = string.Format(Properties.Resources.Download_ErrorException, filename) + ": " + ex.Message;
                return false;
            }
        }

        public bool IsFileChangedLocally(string localFile, string remoteFile)
        {
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.ServiceIsLocal)
                return File.Exists(localFile) && File.Exists(remoteFile) && File.GetLastWriteTime(localFile) > File.GetLastWriteTime(remoteFile);

            string filename = "";

            try
            {
                filename = Path.GetFileName(localFile);

                if (Program.ServiceIsLocal)
                    return false;

                if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
                {
                    Logging.ibaLogger.Log(Logging.Level.Exception, string.Format(Properties.Resources.FileChanged_ErrorDisconnected, filename));
                    return false;
                }

                FileInfo fileInfo = new FileInfo(localFile);
                if (!fileInfo.Exists)
                    return false;

                ServerFileInfo[] remoteInfos = Program.CommunicationObject.GetFileInfos(new string[1] { remoteFile });
                if (remoteInfos != null && remoteInfos.Length > 0)
                {
                    ServerFileInfo remoteInfo = remoteInfos[0];
                    if (remoteInfo != null && fileInfo.LastWriteTimeUtc <= remoteInfo.LastWriteTimeUtc)
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logging.ibaLogger.Log(Logging.Level.Exception, string.Format(Properties.Resources.FileChanged_ErrorException, filename, ex.Message));
                return false;
            }
        }

        public bool UploadFile(string localFile, string remoteFile, bool ifExists, out string error)
        {
            error = "";
			string filename = "";

			if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.ServiceIsLocal)
			{ //just copy

				try
				{
					File.Copy(localFile, remoteFile, true);
					return true;
				}
				catch (Exception ex)
				{
					filename = Path.GetFileName(localFile);
					error = string.Format(Properties.Resources.Upload_ErrorException, filename) + ": " + ex.Message;
					return false;
				}
			}


            try
            {
                filename = Path.GetFileName(localFile);

                if (Program.ServiceIsLocal)
                    return true;

                if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
                {
                    error = string.Format(Properties.Resources.Upload_ErrorDisconnected, filename);
                    return false;
                }

                FileInfo fileInfo = new FileInfo(localFile);
                if (!fileInfo.Exists)
                {
                    if (ifExists)
                        return true;

                    error = string.Format(Properties.Resources.Upload_ErrorNotFound, filename);
                    return false;
                }

                ServerFileInfo[] remoteInfos = Program.CommunicationObject.GetFileInfos(new string[1] { remoteFile });
                if (remoteInfos != null && remoteInfos.Length > 0)
                {
                    ServerFileInfo remoteInfo = remoteInfos[0];
                    if (remoteInfo != null && fileInfo.LastWriteTimeUtc <= remoteInfo.LastWriteTimeUtc)
                        return true;
                }

                DialogResult res = DialogResult.OK;
                using (var dlg = new FilesUploaderForm(new string[1] { localFile }, Path.GetDirectoryName(remoteFile), Program.CommunicationObject.GetServerSideFileHandler()))
                {
                    if (Program.MainForm.InvokeRequired)
                        res = (DialogResult)Program.MainForm.Invoke(new Func<DialogResult>(() => { return dlg.ShowDialog(Program.MainForm); }));
                    else
                        res = dlg.ShowDialog(Program.MainForm);
                }

                if (res == DialogResult.OK)
                    return true;

                if (res == DialogResult.Abort)
                    error = string.Format(Properties.Resources.Upload_ErrorException, filename);
                else if (res == DialogResult.Cancel)
                    error = string.Format(Properties.Resources.Upload_ErrorCancelled, filename);
                else
                    error = string.Format(Properties.Resources.Upload_ErrorUnknown, filename);

                return false;
            }
            catch (Exception ex)
            {
                error = string.Format(Properties.Resources.Upload_ErrorException, filename) + ": " + ex.Message;
                return false;
            }
        }

        public bool UploadFiles(string[] localFiles, string[] remoteFiles, bool ifExists, out Dictionary<string, string> errors)
        {
            int length = Math.Min(localFiles.Length, remoteFiles.Length);
            errors = new Dictionary<string, string>();
            bool bSuccess = true;

            for (int i = 0; i < length; i++)
            {
                if (!UploadFile(localFiles[i], remoteFiles[i], ifExists, out string error))
                {
                    errors[Path.GetFileName(localFiles[i])] = error;
                    bSuccess = false;
                }
            }

            return bSuccess;
        }
    }
}
