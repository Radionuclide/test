using System;
using System.Windows.Forms;

namespace iba.Controls
{
    public partial class KafkaWriterTaskControlEventHub : UserControl
    {
        public KafkaWriterTaskControlEventHub()
        {
            InitializeComponent();
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
    }
}
