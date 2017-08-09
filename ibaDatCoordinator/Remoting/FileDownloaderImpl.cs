using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iba.Logging;

namespace iba.Remoting
{
    class FileDownloaderImpl
    {
        public FileDownloaderImpl(FileStream file, Int64 fileSize, IFileDownload download)
        {
            this.file = file;
            this.fileSize = fileSize;
            this.download = download;
            this.copyPaths = null;
            StartThread();
        }
        public FileDownloaderImpl(String[] copyPaths, Int64 fileSize, IFileDownload download)
        {
            this.fileSize = fileSize;
            this.download = download;
            this.copyPaths = copyPaths;

            String destPath = PdaServerFiles.NormalizePath(copyPaths[0]);
            String dir = Path.GetDirectoryName(destPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            this.file = File.Create(destPath);

            StartThread();
        }

        private void DownloadFile()
        {
            try
            {
                int receivedSize = 0;
                while (receivedSize < fileSize)
                {
                    Byte[] buffer = download.GetNextPortion();
                    if ((buffer == null) || (buffer.Length == 0))
                        break;

                    file.Write(buffer, 0, buffer.Length);
                    receivedSize += buffer.Length;
                }

                file.Flush();

                // Copy the file to other locations if necessary
                if (copyPaths != null)
                {
                    for (int i = 1; i < copyPaths.Length; i++)
                    {
                        file.Seek(0, SeekOrigin.Begin);

                        String destPath = PdaServerFiles.NormalizePath(copyPaths[i]);
                        String dir = Path.GetDirectoryName(destPath);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        FileStream copy = File.Create(destPath);
                        file.CopyTo(copy);
                        copy.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ibaLogger.DebugFormat("Error receiving file from client : {0}", ex.Message);
                IFileDownload2 download2 = download as IFileDownload2;
                if (download2 != null)
                {
                    try
                    {
                        download2.ErrorOccurred(ex.Message);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            finally
            {
                file.Close();
            }
        }

        private void StartThread()
        {
            Thread thread = new Thread(new ThreadStart(DownloadFile));
            thread.SetApartmentState(ApartmentState.MTA);
            thread.IsBackground = true;
            thread.Start();
        }

        private FileStream file;
        private IFileDownload download;
        Int64 fileSize;
        string[] copyPaths;

    }
}
