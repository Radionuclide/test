using System;
using System.Xml.Serialization;
using DevExpress.XtraSplashScreen;
using iba.Utility;

namespace iba.Data
{
    [Serializable]
    public class UploadTaskData : TaskDataUNC
    {
        private string m_server;

        public string Server
        {
            get => m_server; 
            set => m_server = value;
        }

        private string m_port;
        public string Port
        {
            get => m_port;
            set => m_port = value;
        }
        private string m_privateKeyPassphrase = string.Empty;
        
        [XmlIgnore]
        public string PrivateKeyPassphrase
        {
            get => m_privateKeyPassphrase;
            set => m_privateKeyPassphrase = value;
        }

        public string PrivateKeyPassphraseCrypted
        {
            get => Crypt.Encrypt(m_privateKeyPassphrase);
            set => m_privateKeyPassphrase = Crypt.Decrypt(value);
        }

        private string m_remotePath;
        public string RemotePath
        {
            get => m_remotePath; 
            set => m_remotePath = value;
        }

        private string m_tlsCertificateFingerprint;
        public string TlsCertificateFingerprint
        {
            get => m_tlsCertificateFingerprint;
            set => m_tlsCertificateFingerprint = value;
        }

        private string m_sshHostKeyFingerprint;
        public string SshHostKeyFingerprint
        {
            get => m_sshHostKeyFingerprint;
            set => m_sshHostKeyFingerprint = value;
        }

        public UploadTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.UploadTaskTitle;
            Port = "21";
            RemotePath = "/";
        }

        public enum WhatFileUploadEnum { DATFILE, PREVOUTPUT }

        private WhatFileUploadEnum m_whatFileUpload;

        public WhatFileUploadEnum WhatFileUpload
        {
            get => m_whatFileUpload;
            set => m_whatFileUpload = value;
        }

        public enum EncryptionChoiceEnum { None, ExplicitTls, ImplicitTls }

        private EncryptionChoiceEnum m_encryptionChoice;

        public EncryptionChoiceEnum EncryptionChoice
        {
            get => m_encryptionChoice;
            set => m_encryptionChoice = value;
        }

        public enum TransferProtocol { Ftp, Sftp, Scp, AmazonS3, AzureDataLake }

        private TransferProtocol m_transferProtocol;

        public TransferProtocol Protocol
        {
            get => m_transferProtocol;
            set => m_transferProtocol = value;
        }
        public enum FtpMode { Passive, Active}

        private FtpMode m_ftpMode;
        public FtpMode Mode
        {
            get => m_ftpMode;
            set => m_ftpMode = value;
        }

        private bool m_anonymous;

        public bool Anonymous
        {
            get => m_anonymous;
            set => m_anonymous = value;
        }
        private string m_pathToPrivateKey;

        public string PathToPrivateKey
        {
            get => m_pathToPrivateKey;
            set => m_pathToPrivateKey = value;
        }
        private bool m_acceptAnySshHostKey;

        public bool AcceptAnySshHostKey
        {
            get => m_acceptAnySshHostKey;
            set => m_acceptAnySshHostKey = value;
        }

        private bool m_acceptAnyTlsCertificate;

        public bool AcceptAnyTlsCertificate
        {
            get => m_acceptAnyTlsCertificate;
            set => m_acceptAnyTlsCertificate = value;
        }

        private string m_pathToCertificate;

        public string PathToCertificate
        {
            get => m_pathToCertificate;
            set => m_pathToCertificate = value;
        }

        public bool CreateZipArchive { get; set; }

        public UploadTaskData()
            : this(null)
        {
        }

        public override TaskData CloneInternal()
        {
            UploadTaskData cd = new UploadTaskData(null);
            cd.m_server = m_server;
            cd.m_privateKeyPassphrase = m_privateKeyPassphrase;
            cd.m_port = m_port;
            cd.m_remotePath = m_remotePath;
            cd.m_tlsCertificateFingerprint = m_tlsCertificateFingerprint;
            cd.m_encryptionChoice = m_encryptionChoice;
            cd.m_transferProtocol = m_transferProtocol;
            cd.m_ftpMode = m_ftpMode;
            cd.m_whatFileUpload = m_whatFileUpload;
            cd.m_anonymous = m_anonymous;
            cd.m_acceptAnySshHostKey = m_acceptAnySshHostKey;
            cd.m_acceptAnyTlsCertificate = m_acceptAnyTlsCertificate;
            cd.SshHostKeyFingerprint = SshHostKeyFingerprint;
            cd.TlsCertificateFingerprint = TlsCertificateFingerprint;
            cd.m_pathToCertificate = m_pathToCertificate;
            cd.m_pathToPrivateKey = m_pathToPrivateKey;
            cd.CreateZipArchive = CreateZipArchive;

            CopyUNCData(cd);

            return cd;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            UploadTaskData other = taskData as UploadTaskData;
            if (other == null) return false;
            if (other == this) return true;
            if (!UNCDataIsSame(other)) return false;
            return
                other.m_whatFileUpload == m_whatFileUpload &&
                other.m_server == m_server &&
                other.m_privateKeyPassphrase == m_privateKeyPassphrase &&
                other.m_port == m_port &&
                other.m_remotePath == m_remotePath &&
                other.m_tlsCertificateFingerprint == m_tlsCertificateFingerprint &&
                other.m_encryptionChoice == m_encryptionChoice &&
                other.m_transferProtocol == m_transferProtocol &&
                other.m_ftpMode == m_ftpMode &&
                other.m_anonymous == m_anonymous &&
                other.m_acceptAnySshHostKey == m_acceptAnySshHostKey &&
                other.m_acceptAnyTlsCertificate == m_acceptAnyTlsCertificate &&
                other.m_sshHostKeyFingerprint == m_sshHostKeyFingerprint &&
                other.m_tlsCertificateFingerprint == m_tlsCertificateFingerprint &&
                other.m_pathToCertificate == m_pathToCertificate &&
                other.m_pathToPrivateKey == m_pathToPrivateKey &&
                other.CreateZipArchive == CreateZipArchive;
        }
    }
}
