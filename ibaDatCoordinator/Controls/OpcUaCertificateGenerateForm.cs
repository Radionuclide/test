using System;
using System.Windows.Forms;
using iba.Data;

namespace iba.Controls
{
    /// <summary> (copied from ibaPda project) </summary>
    public partial class OpcUaCertificateGenerateForm : Form
    {
        public OpcUaCertificateGenerateForm()
        {
            InitializeComponent();

            cbCertAlgorithm.Items.Add("SHA-1");
            cbCertAlgorithm.Items.Add("SHA-256");

            cbCertAlgorithm.SelectedItem = "SHA-256";
        }

        public void LoadDataSource(OpcUaData.CGenerateCertificateArgs args)
        {
            tbCertName.Text = args.ApplicationName;
            tbAppUri.Text = args.ApplicationUri;
            numLifeTime.Value = Math.Min(Math.Max(numLifeTime.Minimum, args.Lifetime), numLifeTime.Maximum);
            cbCertAlgorithm.SelectedItem = args.UseSha256 ? "SHA-256" : "SHA-1";
        }

        public void ApplySettings(OpcUaData.CGenerateCertificateArgs args)
        {
            args.ApplicationName = tbCertName.Text;
            args.ApplicationUri = tbAppUri.Text;
            args.Lifetime = (int)numLifeTime.Value;
            args.UseSha256 = ((string)cbCertAlgorithm.SelectedItem == "SHA-256");
			args.KeySize = args.UseSha256 ? 2048 : 1024;
        }
    }
}
