using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace iba.Dialogs
{
    public partial class UploadOrDownloadConfigurationsDialog : Form
    {
        public UploadOrDownloadConfigurationsDialog()
        {
            InitializeComponent();
            Upload = false;
        }

        public bool Upload;
        private void m_btDownload_Click(object sender, EventArgs e)
        {
            Upload = false;
            Close();
        }

        private void m_btUpload_Click(object sender, EventArgs e)
        {
            if (iba.Utility.Crypt.CheckPassword(this))
            {
                Upload = true;
                Close();
            }
        }
    }
}