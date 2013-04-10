using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;
using System.IO;
using System.Net;

namespace Alunorf_roh_plugin
{
    class PluginRohWorker : IPluginTaskWorker
    {
        #region IPluginTaskWorker Members

        private bool m_started;
        public bool OnStart()
        {
            m_started = true;
            return true;
        }

        public bool OnStop()
        {
            m_started = false;
            return true;
        }

        public bool OnApply(IPluginTaskData newtask, IJobData newParentJob)
        {
            m_dataToApply = newtask as PluginRohTask;
            return true;
        }

        public bool ExecuteTask(string datFile)
        {
            if (m_dataToApply != null)
            {
                m_data = m_dataToApply;
                m_dataToApply = null;
            }
            m_error = "";

            try
            {
                string root = Path.GetFileNameWithoutExtension(datFile);
                string rohFileName = (m_data.FilePrefix == null ? "" : m_data.FilePrefix) + root + ".roh";
                int currentFileNr = System.Threading.Interlocked.Increment(ref filenamecount);
                string tempFilePath = Path.Combine(Path.GetTempPath(), currentFileNr.ToString() + ".roh");
                iba.RohWriter rw = new iba.RohWriter();
                int res = rw.Write(m_data.RohInput, datFile, tempFilePath);
                if (res != 0)
                {
                    switch (res)
                    {
                        case 1:
                            m_error = string.Format(Properties.Resources.StichDataNotFound, rw.errorDataLineInput.ibaName, PluginRohTask.FindDataLine(m_data.RohInput.StichDaten, rw.errorDataLineInput));
                            break;
                        case 2:
                            m_error = string.Format(Properties.Resources.KopfDataNotFound, rw.errorDataLineInput.ibaName, PluginRohTask.FindDataLine(m_data.RohInput.KopfDaten, rw.errorDataLineInput));
                            break;
                        case 3:
                            m_error = string.Format(Properties.Resources.SchlussDataNotFound, rw.errorDataLineInput.ibaName, PluginRohTask.FindDataLine(m_data.RohInput.SchlussDaten, rw.errorDataLineInput));
                            break;
                        case 4:
                            m_error = string.Format(Properties.Resources.KanalDataNotFound, rw.errorChannelLineInput.ibaName, PluginRohTask.FindChannelLine(m_data.RohInput.Kanalen, rw.errorChannelLineInput));
                            break;
                        case 5:
                            m_error = string.Format(Properties.Resources.ErrorDatUnexpected, rw.errorMessage);
                            break;
                        case 6:
                            m_error = string.Format(Properties.Resources.ErrorIbaFiles, rw.errorMessage);
                            break;
                        case 7:
                            m_error = string.Format(Properties.Resources.ErrorIbaFilesOpen, rw.errorMessage);
                            break;
                        case 8:
                            m_error = string.Format(Properties.Resources.ErrorRohUnexpected, rw.errorMessage);
                            break;
                        case 9:
                            m_error = string.Format(Properties.Resources.KanalDataCouldNotBeLoaded, rw.errorChannelLineInput.ibaName, PluginRohTask.FindChannelLine(m_data.RohInput.Kanalen, rw.errorChannelLineInput));
                            break;
                        case 10:
                            m_error = string.Format(Properties.Resources.ErrorRohFileCreate, rw.errorMessage);
                            break;
                        default:
                            m_error = string.Format(Properties.Resources.ErrorUnexpected, rw.errorMessage);
                            break;
                    }
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch
                    {
                    }
                    return false;
                }

                //ftp stuff
                try
                {
                    StringBuilder sb = new StringBuilder();
                    string dir = m_data.FtpDirectory.Replace('\\' , '/');
                    sb.Append("ftp://");
                    if (m_data.FtpHost.StartsWith("ftp://"))
                        sb.Append(m_data.FtpHost.Substring(6));
                    else
                        sb.Append(m_data.FtpHost);
                    if (m_data.FtpHost.EndsWith("/") && dir.StartsWith("/"))
                        sb.Append(dir.Substring(1));
                    else if (!m_data.FtpHost.EndsWith("/") && !dir.StartsWith("/"))
                    {
                        sb.Append("/");
                        sb.Append(dir);
                    }
                    else
                        sb.Append(dir);
                    if (!dir.EndsWith("/"))
                        sb.Append("/");
                    sb.Append(rohFileName);
                    Uri uri = new Uri(sb.ToString());
                    FtpWebRequest request = FtpWebRequest.Create(uri) as FtpWebRequest;
                    request.Credentials = new NetworkCredential(m_data.FtpUser, m_data.FtpPassword);
                    request.KeepAlive = false;
                    request.UsePassive = true;
                    request.UseBinary = true;
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    FileInfo finf = new FileInfo(tempFilePath);
                    request.ContentLength = (finf).Length;
                    Stream strm = request.GetRequestStream();

                    int bufferSize = 64 * 1024;
                    int readCount;
                    byte[] buff = new byte[bufferSize];
                    
                    FileStream fs = finf.OpenRead();
                    readCount = fs.Read(buff, 0, bufferSize);

                    while (readCount != 0)
                    {
                        strm.Write(buff, 0, readCount);
                        readCount = fs.Read(buff, 0, bufferSize);
                    }
                    strm.Close();
                    fs.Close();
                }
                catch (Exception ftpex)
                {
                    m_error = string.Format(Properties.Resources.ErrorFtp, ftpex.Message);
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch
                    {
                    }
                    return false;
                }
                try
                {
                    File.Delete(tempFilePath);
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                m_error = String.Format(Properties.Resources.ErrorUnexpected, ex.Message);
            }

            return true;
        }

        private string m_error;
        public string GetLastError()
        {
            return m_error;
        }

        public PluginTaskWorkerStatus GetWorkerStatus()
        {
            PluginTaskWorkerStatus res = new PluginTaskWorkerStatus();
            res.started = m_started;
            res.extraData = null; 
            return res;
        }

        #endregion

        private PluginRohTask m_data;
        private PluginRohTask m_dataToApply;

        static int filenamecount;
        public PluginRohWorker(PluginRohTask data)
        {
            m_data = data;
            m_dataToApply = null;
        }
    }
}
