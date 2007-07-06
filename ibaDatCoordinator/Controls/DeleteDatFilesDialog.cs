using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iba.Data;

namespace iba.Controls
{
    public partial class DeleteDatFilesDialog : Form
    {
        List<string> m_files;
        public DeleteDatFilesDialog(List<string> files)
        {
            InitializeComponent();
            m_files = files;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int count = 0; count < m_files.Count; count++)
            {
                string filename = m_files[count];
                try
                {
                    if (File.Exists(filename))
                        File.Delete(filename);
                    backgroundWorker1.ReportProgress(0, new ProgressData(count, filename));
                }
                catch { }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressdata is the same from RemoveMarkingsDialog
            ProgressData dat = (e.UserState as ProgressData);
            int progress = (int)((double)(dat.Count * 100) / (double)(m_files.Count));
            m_fileLabel.Text = dat.FileName;
            m_progressBar.Value = progress;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
    }
}