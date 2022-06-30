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
using Opc.Ua;
using WinSCP;

namespace iba.Processing
{
    internal interface IUploadTaskWorker
    {
        void UploadFile(string datFile, ConfigurationWorker configurationWorker);
        bool TestConnection(out string errorMessage, UploadTaskData data);
    }

    internal static class UploadTaskWorkerCreator
    {
        public static IUploadTaskWorker CreateUploadTaskWorker(UploadTaskData data)
        {
            return data.Protocol switch
            {
                UploadTaskData.TransferProtocol.Ftp => new WinScpUploadTaskWorker(data),
                UploadTaskData.TransferProtocol.Sftp => new WinScpUploadTaskWorker(data),
                UploadTaskData.TransferProtocol.Scp => new WinScpUploadTaskWorker(data),
                UploadTaskData.TransferProtocol.AmazonS3 => new WinScpUploadTaskWorker(data),
                UploadTaskData.TransferProtocol.AzureDataLake => new AzureDataLakeUploadTaskWorker(data),
                _ => throw new ArgumentOutOfRangeException(nameof(data), $"Not Available: {data.Protocol}")
            };
        }
    }

    internal class WinScpUploadTaskWorker : IUploadTaskWorker
    {
        private readonly UploadTaskData _data;

        internal WinScpUploadTaskWorker(UploadTaskData data)
        {
            _data = data;
        }
        /// <summary>
        /// Configure session and upload file
        /// </summary>
        public void UploadFile(string datFile, ConfigurationWorker configurationWorker)
        {
            using var session = CreateSession(_data);

            var (outputDir, outputFilename) = CreateOutput(datFile, _data, configurationWorker);
            
            if (!_data.OverwriteFiles && session.FileExists(string.Concat(outputDir, outputFilename))) 
                return;

            (outputFilename, datFile) = CreateZipArchiveIfRequired(_data, outputFilename, datFile);

            Upload(session, _data, datFile, outputFilename, outputDir);
            
            DeleteZipArchive(datFile);
        }

        private static void Upload(Session session, UploadTaskData data, string datFile, string outputFilename, string outputDir)
        {
            if (data.UseInfoFieldForOutputFile && !data.CreateZipArchive)
            {
                var tempDatFile = string.Concat(datFile, ".tmp");

                File.Copy(datFile, tempDatFile);
                session.PutFileToDirectory(tempDatFile, outputDir);
                RenameUploadedFile(session, tempDatFile, outputDir, outputFilename);

                File.Delete(tempDatFile);
            }
            else
            {
                session.PutFileToDirectory(datFile, outputDir);
            }
        }

        private static Session CreateSession(UploadTaskData data)
        {
            var session = new Session();
            var sessionOptions = SetSessionOptions(data);
            ConfigureTlsOrSshOptions(sessionOptions, data);
            session.Open(sessionOptions);

            return session;
        }

        private void DeleteZipArchive(string datFile)
        {
            if (_data.CreateZipArchive) 
                File.Delete(datFile);
        }

        public static Tuple<string, string> CreateZipArchiveIfRequired(UploadTaskData data, string outputFilename, string datFile)
        {
            if (data.CreateZipArchive)
            {
                datFile = ZipCreator.CreateZipArchive(datFile,
                    Path.Combine(Path.GetDirectoryName(datFile), outputFilename), outputFilename + ".dat");
            }

            return new Tuple<string, string>(outputFilename, datFile);
        }

        public static Tuple<string, string> CreateOutput(string datFile, UploadTaskData data,
            ConfigurationWorker configurationWorker)
        {
            data.DestinationMap = data.DestinationMapUNC = data.RemotePath;

            var outputDir = configurationWorker?.GetOutputDirectoryName(datFile, data)
                .Replace(configurationWorker.RunningConfiguration.DatDirectory, string.Empty);

            outputDir = string.Concat(outputDir.Replace("\\", "/"), "/");

            var outputFilename = configurationWorker?.GetOutputFileName(data, datFile);

            outputFilename = string.Concat(outputFilename, data.CreateZipArchive ? ".zip" : ".dat");
            
            return new Tuple<string, string>(outputDir, outputFilename);
        }

        private static void RenameUploadedFile(Session session, string tempDatFile, string outputDir, string outputFilename)
        {
            var sourcePath = string.Concat(outputDir, Path.GetFileName(tempDatFile));
            var targetPath = string.Concat(outputDir, outputFilename);

            if (session.FileExists(targetPath))
            {
                session.RemoveFile(targetPath);
            }

            session.MoveFile(sourcePath, targetPath);
        }

        private static SessionOptions SetSessionOptions(UploadTaskData data)
        {
            var sessionOptions = new SessionOptions
            {
                Protocol = SetProtocol(data.Protocol),
                HostName = data.Server,
                UserName = data.Username,
                Password = data.Password,
                PortNumber = SetPort(data),
                FtpSecure = SetEncryptionMode(data),
                FtpMode = data.Mode == UploadTaskData.FtpMode.Passive ? FtpMode.Passive : FtpMode.Active
            };

            return sessionOptions;
        }

        private static void ConfigureTlsOrSshOptions(SessionOptions sessionOptions, UploadTaskData data)
        {
            switch (data.Protocol)
            {
                case UploadTaskData.TransferProtocol.Ftp:
                    ConfigureTlsOptions(sessionOptions, data);
                    break;
                case UploadTaskData.TransferProtocol.Sftp:
                case UploadTaskData.TransferProtocol.Scp:
                    ConfigureSshOptions(sessionOptions, data);
                    break;
                case UploadTaskData.TransferProtocol.AmazonS3:
                    break;
                default:
                    throw new InvalidOperationException("Protocol does not exist");
            }
        }

        private static void ConfigureTlsOptions(SessionOptions sessionOptions, UploadTaskData data)
        {
            if (data.AcceptAnyTlsCertificate)
            {
                sessionOptions.GiveUpSecurityAndAcceptAnyTlsHostCertificate = true;
                return;
            }

            if (data.EncryptionChoice == UploadTaskData.EncryptionChoiceEnum.None) return;

            sessionOptions.TlsHostCertificateFingerprint = data.TlsCertificateFingerprint;
            sessionOptions.TlsClientCertificatePath = data.PathToCertificate;
        }

        private static void ConfigureSshOptions(SessionOptions sessionOptions, UploadTaskData data)
        {
            if (data.AcceptAnySshHostKey)
            {
                sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                return;
            }

            if (!string.IsNullOrEmpty(data.PathToPrivateKey))
            {
                sessionOptions.SshPrivateKeyPath = data.PathToPrivateKey;
                sessionOptions.PrivateKeyPassphrase = data.PrivateKeyPassphrase;
            }

            if (string.IsNullOrEmpty(data.SshHostKeyFingerprint))
            {
                throw new InvalidOperationException("Fingerprint not set");
            }

            sessionOptions.SshHostKeyFingerprint = data.SshHostKeyFingerprint;
        }

        private static Protocol SetProtocol(UploadTaskData.TransferProtocol protocol)
        {
            switch (protocol)
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

        private static int SetPort(UploadTaskData data)
        {
            const int defaultFtpPort = 21;
            var portResult = int.TryParse(data.Port, out var port);
            return portResult ? port : defaultFtpPort;
        }

        private static FtpSecure SetEncryptionMode(UploadTaskData data)
        {
            if (data.Protocol != UploadTaskData.TransferProtocol.Ftp)
                return FtpSecure.None;

            switch (data.EncryptionChoice)
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
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool TestConnection(out string errorMessage, UploadTaskData data)
        {
            errorMessage = string.Empty;

            using var session = new Session();
            SessionOptions sessionOptions = null;
            try
            {
                sessionOptions = SetSessionOptions(data);
                ConfigureTlsOrSshOptions(sessionOptions, data);

                session.QueryReceived += OnSessionQueryReceived;

                session.Open(sessionOptions);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return HandleException(ref errorMessage, sessionOptions, session, _data);
            }

            return true;
        }

        private static bool HandleException(ref string errormessage, SessionOptions sessionOptions, Session session, UploadTaskData data)
        {
            if (errormessage.Contains("Peer certificate rejected") && m_tlsCertificateAccepted)
            {
                return true;
            }

            if (errormessage.Contains("Authenticating with public key"))
            {
                return OnPassphraseForPrivateKeyRequired(ref errormessage, sessionOptions, session, data);
            }

            if (errormessage.Contains("Fingerprint not set"))
            {
                return ScanAndAskForFingerprint(ref errormessage, session, sessionOptions, data) && 
                       TestFingerprint(ref errormessage, session, sessionOptions, data);
            }

            return false;
        }

        private static bool TestFingerprint(ref string errormessage, Session session, SessionOptions sessionOptions, UploadTaskData data)
        {
            try
            {
                session.Open(sessionOptions);
                return true;
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return HandleException(ref errormessage, sessionOptions, session, data);
            }
        }

        private static bool ScanAndAskForFingerprint(ref string errormessage, Session session, SessionOptions sessionOptions, UploadTaskData data)
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
                    data.SshHostKeyFingerprint = sha256;
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

        private static bool OnPassphraseForPrivateKeyRequired(ref string errormessage, SessionOptions sessionOptions,
            Session session, UploadTaskData data)
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
                data.PrivateKeyPassphrase = privateKeyPassphrase;
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return false;
            }
            
            return true;
        }

        private static bool m_tlsCertificateAccepted = false;

        private void OnSessionQueryReceived(object sender, QueryReceivedEventArgs args)
        {
            var dialogResult = MessageBox.Show(args.Message, "Warning", MessageBoxButtons.OKCancel);

            if (dialogResult != DialogResult.OK) return;

            var fingerprint = ParseFingerprint(args);

            _data.TlsCertificateFingerprint = fingerprint;
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

    internal class AzureDataLakeUploadTaskWorker : IUploadTaskWorker
    {
        private readonly UploadTaskData _data;

        public AzureDataLakeUploadTaskWorker(UploadTaskData data)
        {
            _data = data;

        }

        public void UploadFile(string datFile, ConfigurationWorker configurationWorker)
        {
            var sharedKeyCredential = new StorageSharedKeyCredential(_data.Username, _data.Password);
            var serviceUri = new Uri("https://" + _data.Username + ".dfs.core.windows.net");
            var dataLakeServiceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            var (outputDir, outputFilename) = WinScpUploadTaskWorker.CreateOutput(datFile, _data, configurationWorker);

            (outputFilename, datFile) = WinScpUploadTaskWorker.CreateZipArchiveIfRequired(_data, outputFilename, datFile);

            var dataLakeFileSystemClient = dataLakeServiceClient.GetFileSystemClient(GetFilesystemPath(outputDir));
            
            dataLakeFileSystemClient.CreateIfNotExists();

            var fileClient = dataLakeFileSystemClient.GetFileClient(outputDir + outputFilename);

            try
            {
                fileClient.Upload(datFile, _data.OverwriteFiles);
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == "PathAlreadyExists")
            {
                //Do not overwrite file
            }

            if (_data.CreateZipArchive)
            {
                File.Delete(datFile);
            }
        }

        public bool TestConnection(out string errorMessage, UploadTaskData data)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
            {
                errorMessage = "Please enter your Account name and Shared key";
                return false;
            }

            if (GetFilesystemPath(data.RemotePath).Length < 3)
            {
                errorMessage = "The specified Remote path length is not within the permissible limits";
                return false;
            }

            var sharedKeyCredential = new StorageSharedKeyCredential(data.Username, data.Password);
            var serviceUri = new Uri("https://" + data.Username + ".dfs.core.windows.net");
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
                errorMessage = $"Unexpected error: {ex.Message}";
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

        private static string FormatPath(string remotePath)
        {
            return remotePath.Replace(@"\", "/").Trim('/');
        }
    }
}