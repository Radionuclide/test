using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Azure;
using Azure.Storage;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Files.DataLake.Models;
using iba.Data;
using iba.Dialogs;
using iba.Logging;
using iba.Utility;
using WinSCP;

namespace iba.Processing
{
    internal class UploadTaskWorker
    {
        private readonly UploadTaskData m_data;
        private string m_datFile;

        public UploadTaskWorker(string datFile, UploadTaskData data)
        {
            m_datFile = datFile;
            m_data = data;
        }

        /// <summary>
        /// Configure session and upload file
        /// </summary>
        public void UploadFile()
        {
            if (m_data.CreateZipArchive)
            {
                m_datFile = ZipCreator.CreateZipArchive(m_datFile);
            }

            if (m_data.Protocol == UploadTaskData.TransferProtocol.AzureDataLake)
            {
                var service = new AzureDataLakeClient(m_datFile, m_data);
                service.UploadFile();
            }
            else
            {
                using (var session = new Session())
                {
                    var sessionOptions = SetSessionOptions();
                    ConfigureTlsOrSshOptions(sessionOptions);

                    session.Open(sessionOptions);

                    var transferOptions = new TransferOptions
                    {
                        OverwriteMode = OverwriteMode.Overwrite
                    };

                    session.PutFileToDirectory(m_datFile, m_data.RemotePath, false, transferOptions);
                }
            }

            if (m_data.CreateZipArchive)
            {
                File.Delete(m_datFile);
            }
        }

        private SessionOptions SetSessionOptions()
        {
            var sessionOptions = new SessionOptions
            {
                Protocol = SetProtocol(),
                HostName = m_data.Server,
                UserName = m_data.Username,
                Password = m_data.Password,
                PortNumber = SetPort(),
                FtpSecure = SetEncryptionMode(),
                FtpMode = m_data.Mode == UploadTaskData.FtpMode.Passive ? FtpMode.Passive : FtpMode.Active
            };

            return sessionOptions;
        }

        private void ConfigureTlsOrSshOptions(SessionOptions sessionOptions)
        {
            switch (m_data.Protocol)
            {
                case UploadTaskData.TransferProtocol.Ftp:
                    ConfigureTlsOptions(sessionOptions);
                    break;
                case UploadTaskData.TransferProtocol.Sftp:
                case UploadTaskData.TransferProtocol.Scp:
                    ConfigureSshOptions(sessionOptions);
                    break;
                case UploadTaskData.TransferProtocol.AmazonS3:
                    break;
                default:
                    throw new InvalidOperationException("Protocol does not exist");
            }
        }

        private void ConfigureTlsOptions(SessionOptions sessionOptions)
        {
            if (m_data.AcceptAnyTlsCertificate)
            {
                sessionOptions.GiveUpSecurityAndAcceptAnyTlsHostCertificate = true;
                return;
            }

            if (m_data.EncryptionChoice == UploadTaskData.EncryptionChoiceEnum.None) return;

            sessionOptions.TlsHostCertificateFingerprint = m_data.TlsCertificateFingerprint;
            sessionOptions.TlsClientCertificatePath = m_data.PathToCertificate;
        }

        private void ConfigureSshOptions(SessionOptions sessionOptions)
        {
            if (m_data.AcceptAnySshHostKey)
            {
                sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                return;
            }

            if (!string.IsNullOrEmpty(m_data.PathToPrivateKey))
            {
                sessionOptions.SshPrivateKeyPath = m_data.PathToPrivateKey;
                sessionOptions.PrivateKeyPassphrase = m_data.PrivateKeyPassphrase;
            }

            if (string.IsNullOrEmpty(m_data.SshHostKeyFingerprint))
            {
                throw new InvalidOperationException("Fingerprint not set");
            }

            sessionOptions.SshHostKeyFingerprint = m_data.SshHostKeyFingerprint;
        }

        private Protocol SetProtocol()
        {
            switch (m_data.Protocol)
            {
                case UploadTaskData.TransferProtocol.Ftp:
                    return Protocol.Ftp;
                case UploadTaskData.TransferProtocol.Sftp:
                    return Protocol.Sftp;
                case UploadTaskData.TransferProtocol.Scp:
                    return Protocol.Scp;
                case UploadTaskData.TransferProtocol.AmazonS3:
                    return Protocol.S3;
                default:
                    throw new InvalidOperationException("Transfer protocol not available");
            }
        }

        private int SetPort()
        {
            const int defaultFtpPort = 21;
            var portResult = int.TryParse(m_data.Port, out var port);
            return portResult ? port : defaultFtpPort;
        }

        private FtpSecure SetEncryptionMode()
        {
            if (m_data.Protocol != UploadTaskData.TransferProtocol.Ftp)
                return FtpSecure.None;

            switch (m_data.EncryptionChoice)
            {
                case UploadTaskData.EncryptionChoiceEnum.None:
                    return FtpSecure.None;
                case UploadTaskData.EncryptionChoiceEnum.ExplicitTls:
                    return FtpSecure.Explicit;
                case UploadTaskData.EncryptionChoiceEnum.ImplicitTls:
                    return FtpSecure.Explicit;
                default:
                    throw new InvalidOperationException("Encryption type not available");
            }
        }
        /// <summary>
        /// Create session and test whether file upload is possible
        /// </summary>
        /// <param name="errormessage"></param>
        /// <returns></returns>
        public bool TestConnection(out string errormessage)
        {
            errormessage = string.Empty;

            if (m_data.Protocol != UploadTaskData.TransferProtocol.AzureDataLake)
            {
                using (var session = new Session())
                {
                    SessionOptions sessionOptions = null;
                    try
                    {
                        sessionOptions = SetSessionOptions();
                        ConfigureTlsOrSshOptions(sessionOptions);

                        session.QueryReceived += OnSessionQueryReceived;

                        session.Open(sessionOptions);

                        CheckRemotePath(session);
                    }
                    catch (Exception ex)
                    {
                        errormessage = ex.Message;
                        return HandleException(ref errormessage, sessionOptions, session);
                    }
                }
            }
            else
            {
                try
                {
                    var service = new AzureDataLakeClient(m_datFile, m_data);
                    return service.TestConnection(out errormessage);
                }
                catch (Exception ex)
                {
                    errormessage = ex.Message;
                    return false;
                }
            }

            return true;
        }

        private bool HandleException(ref string errormessage, SessionOptions sessionOptions, Session session)
        {
            if (errormessage.Contains("Peer certificate rejected") && m_tlsCertificateAccepted)
            {
                return true;
            }

            if (errormessage.Contains("Authenticating with public key"))
            {
                return OnPassphraseForPrivateKeyRequired(ref errormessage, sessionOptions, session);
            }

            if (errormessage.Contains("Fingerprint not set"))
            {
                return ScanAndAskForFingerprint(ref errormessage, session, sessionOptions) && 
                       TestFingerprint(ref errormessage, session, sessionOptions);
            }

            return false;
        }

        private bool TestFingerprint(ref string errormessage, Session session, SessionOptions sessionOptions)
        {
            try
            {
                session.Open(sessionOptions);
                return true;
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return HandleException(ref errormessage, sessionOptions, session);
            }
        }

        private void CheckRemotePath(Session session)
        {
            var remoteFileExists = session.FileExists(m_data.RemotePath);

            if (!remoteFileExists)
                throw new InvalidOperationException("Remote path does not exist!");
        }

        private bool ScanAndAskForFingerprint(ref string errormessage, Session session, SessionOptions sessionOptions)
        {
            try
            {
                var sha256 = session.ScanFingerprint(sessionOptions, "SHA-256");
                var md5 = session.ScanFingerprint(sessionOptions, "MD5");

                var message =
                    $"Do you want to continue the connection and save the host key?\n" +
                    $"\nSHA-256: {sha256}" +
                    $"\nMD5: {md5}";

                var dialogResult = MessageBox.Show(message, "Warning", MessageBoxButtons.OKCancel);

                if (dialogResult == DialogResult.OK)
                {
                    m_data.SshHostKeyFingerprint = sha256;
                    sessionOptions.SshHostKeyFingerprint = sha256;
                    return true;
                }
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return false;
            }
            

            return false;
        }

        private bool OnPassphraseForPrivateKeyRequired(ref string errormessage, SessionOptions sessionOptions,
            Session session)
        {
            string privateKeyPassphrase;

            using (var dlg = new AskForPassphrase())
            {
                privateKeyPassphrase = dlg.ShowDialog("Passphrase:", "Warning");
            }

            sessionOptions.PrivateKeyPassphrase = privateKeyPassphrase;

            try
            {
                session.Open(sessionOptions);
                m_data.PrivateKeyPassphrase = privateKeyPassphrase;
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return false;
            }

            return true;
        }

        private bool m_tlsCertificateAccepted = false;

        private void OnSessionQueryReceived(object sender, QueryReceivedEventArgs args)
        {
            var dialogResult = MessageBox.Show(args.Message, "Warning", MessageBoxButtons.OKCancel);

            if (dialogResult != DialogResult.OK) return;

            var fingerprint = ParseFingerprint(args);

            m_data.TlsCertificateFingerprint = fingerprint;
            m_tlsCertificateAccepted = true;
        }

        private static string ParseFingerprint(QueryReceivedEventArgs args)
        {
            var lineWithFingerprint = args.Message
                .Split(new[] { "\n" }, StringSplitOptions.None)
                .First(line => line.StartsWith("Fingerprint"));

            var fingerprint = lineWithFingerprint
                .Substring(lineWithFingerprint
                .IndexOf(":", StringComparison.Ordinal))
                .Trim(' ', ':');

            return fingerprint;
        }
    }

    internal class AzureDataLakeClient
    {
        private readonly UploadTaskData m_data;
        private readonly string m_datFile;

        public AzureDataLakeClient(string datFile, UploadTaskData data)
        {
            m_datFile = datFile;
            m_data = data;
        }

        public void UploadFile()
        {
            var sharedKeyCredential = new StorageSharedKeyCredential(m_data.Username, m_data.Password);

            var serviceUri = new Uri("https://" + m_data.Username + ".dfs.core.windows.net");

            DataLakeServiceClient dataLakeServiceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            DataLakeFileSystemClient dataLakeFileSystemClient = dataLakeServiceClient.GetFileSystemClient(GetFilesystemPath(m_data.RemotePath));
            
            dataLakeFileSystemClient.CreateIfNotExists();

            var fileClient = dataLakeFileSystemClient.GetFileClient(
                $"{GetFilePath(m_data.RemotePath)}/{Path.GetFileName(m_datFile)}");

            try
            {
                fileClient.Upload(m_datFile, m_data.Overwrite);
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == "PathAlreadyExists")
            {
                //Do not overwrite file
            }
        }

        public bool TestConnection(out string errormessage)
        {
            errormessage = string.Empty;

            if (string.IsNullOrEmpty(m_data.Username) || string.IsNullOrEmpty(m_data.Password))
            {
                errormessage = "Please enter your Account name and Shared key";
                return false;
            }

            if (GetFilesystemPath(m_data.RemotePath).Length < 3)
            {
                errormessage = "The specified Remote path length is not within the permissible limits";
                return false;
            }

            var sharedKeyCredential = new StorageSharedKeyCredential(m_data.Username, m_data.Password);
            var serviceUri = new Uri("https://" + m_data.Username + ".dfs.core.windows.net");
            var serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            try
            {
                var response = serviceClient.GetProperties();
                if (response.GetRawResponse().Status == 200)
                {
                    return true;
                }
            }
            catch (RequestFailedException ex)
            {
                errormessage = $"Unexpected error: {ex.Message}";
                throw;
            }

            return false;
        }

        private static string GetFilesystemPath(string remotePath)
        {
            remotePath = FormatPath(remotePath);

            if (!remotePath.Contains("/"))
            {
                return remotePath;
            }

            return remotePath.Substring(0, remotePath.IndexOf("/"));
        }

        private static string GetFilePath(string remotePath)
        {
            remotePath = FormatPath(remotePath);
            
            if (!remotePath.Contains("/"))
            {
                return string.Empty;
            }

            return remotePath.Substring(remotePath.IndexOf("/"));
        }

        private static string FormatPath(string remotePath)
        {
            return remotePath.Replace(@"\", "/").Trim('/');
        }
    }
}