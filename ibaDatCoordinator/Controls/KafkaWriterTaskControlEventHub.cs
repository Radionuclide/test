using iba.CertificateStore;
using iba.CertificateStore.Forms;
using iba.CertificateStore.Proxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.Controls
{
    public partial class KafkaWriterTaskControlEventHub : UserControl, ICertificatesControlHost
    {
        private CertificatesComboBox CACertCBox;
        private CertificateInfo CACertParams;
        class CertificateInfo : ICertificateInfo
        {
            public string Thumbprint { get; set; }

            public CertificateRequirement CertificateRequirements { get; } =
                CertificateRequirement.Trusted |
                CertificateRequirement.Valid;

            public string DisplayName { get; } = "Cert for Kafka";
        }
        public KafkaWriterTaskControlEventHub()
        {
            InitializeComponent();
            CACertCBox = CertificatesComboBox.ReplaceCombobox(CACertPlaceholder, "", false);
            CACertParams = new CertificateInfo();
            CACertCBox.UnsetEnvironment();
            CACertCBox.SetEnvironment(this, CACertParams);
        }

        public string connectionString
        {
            get
            {
                return addressTextBox.Text;
            }
            set
            {
                addressTextBox.Text = value;
            }
        }

        public bool enableSSLVerification
        {
            get
            {
                return enableSSLVerificationCb.Checked;
            }
            set
            {
                enableSSLVerificationCb.Checked = value;
                // call it to make cert box visible/unvisible
                //enableSSLVerificationCb_CheckedChanged(null, null);
            }
        }

        public string ceThumbprint
        {
            get
            {
                return CACertParams.Thumbprint;
            }
            set
            {
                CACertParams.Thumbprint = value;
                // refresh CACertCBox to apply certificate change
                CACertCBox.UnsetEnvironment();
                CACertCBox.SetEnvironment(this, CACertParams);
                CACertCBox.Enabled = enableSSLVerificationCb.Checked;
            }
        }

        public double messageTimeout
        {
            get
            {
                return Decimal.ToDouble(timeoutNumericUpDown.Value);
            }
            set
            {
                timeoutNumericUpDown.Value =
                   new Decimal(
                   Math.Min(
                       Math.Max(value, Decimal.ToDouble(timeoutNumericUpDown.Minimum)),
                       Decimal.ToDouble(timeoutNumericUpDown.Maximum))
               );
            }
        }

        #region ICertificatesControlHost
        public bool IsLocalHost { get; }
        public string ServerAddress { get; }
        public ICertificateManagerProxy CertificateManagerProxy { get; } = new CertificateManagerProxyJsonAdapter(new AppCertificateManagerJsonProxy());
        public bool IsCertificatesReadonly => false;
        public bool IsReadOnly => false; // set to true in case of user restriction
        public string UsagePart { get; } = "EX"; // IO and DS are used in PDA
        public IWin32Window Instance => this;
        public ContextMenuStrip PopupMenu { get; } = new ContextMenuStrip(); // or reuse the context menu of an other control

        public void OnSaveDataSource()
        {
            throw new NotImplementedException();
        }

        public ICertifiable GetCertifiableRootNode()
        {
            throw new NotImplementedException();
        }

        public void ManageCertificates()
        {
            throw new NotImplementedException();
        }

        public void JumpToCertificateInfoNode(string displayName)
        {
            throw new NotImplementedException();
        }
        #endregion

        private void enableSSLVerificationCb_CheckedChanged(object sender, EventArgs e)
        {
            CACertificateLabel.Enabled = enableSSLVerificationCb.Checked;
            CACertCBox.Enabled = enableSSLVerificationCb.Checked;
        }
    }
}
