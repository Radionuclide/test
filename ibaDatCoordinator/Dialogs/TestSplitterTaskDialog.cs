using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using iba.Data;
using iba.Processing;
using iba.Utility;
using System.IO;



namespace iba.Dialogs
{
    public partial class TestSplitterTaskDialog : Form, SplitterTaskProgress
    {
        public TestSplitterTaskDialog(SplitterTaskData data)
        {
            InitializeComponent();
            this.Icon = iba.Properties.Resources.SplitDat;
            m_toolTip.SetToolTip(m_btPerform, Properties.Resources.SplitDialogTestButtonTooltipText);
            m_btPerform.Image = m_btPerformImage = Bitmap.FromHicon(iba.Properties.Resources.SplitDat.Handle);
            WindowsAPI.SHAutoComplete(m_targetFolderTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            m_data = data;
            m_stw = new SplitterTaskWorker(data);
            List<double> points = m_stw.GetPoints();
            DateTime startFile = DateTime.Now;
            try
            {
                startFile = m_stw.GetStartTime();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            if (points == null) 
            {
                Close();
            }
            else
            {
                m_gvEntries.RowCount = points.Count / 2;
                for (int i = 0; i < points.Count / 2; i++)
                {
                    string name = m_stw.GetName(i, m_data.TestDatFile);
                    DateTime trigStart = startFile + TimeSpan.FromSeconds(points[2 * i]);
                    DateTime trigStop = startFile + TimeSpan.FromSeconds(points[2 * i+1]);
                    m_gvEntries.Rows[i].Cells[0].Value = name;
                    m_gvEntries.Rows[i].Cells[1].Value = trigStart;
                    m_gvEntries.Rows[i].Cells[2].Value = trigStop;
                }
            }
            string to = "";
            Profiler.ProfileString(true, "SplitterTestDlg","OutputFolder", ref to, "");
            m_targetFolderTextBox.Text = to;
            m_btPerform.Enabled = Directory.Exists(m_targetFolderTextBox.Text);
        }

        private Image m_btPerformImage;

        private void m_btOK_Click(object sender, EventArgs e)
        {
            m_bAborted = true;
            string to = m_targetFolderTextBox.Text;
            Profiler.ProfileString(false, "SplitterTestDlg", "OutputFolder", ref to, "");
            Close();
        }

        private SplitterTaskWorker m_stw;
        private SplitterTaskData m_data;
        private bool m_bAborted;
        private bool m_bRunning;

        private void m_browseFolderButton_Click(object sender, EventArgs e)
        {
            if (m_bRunning) return;
            m_folderBrowserDialog1.ShowNewFolderButton = true;
            m_folderBrowserDialog1.SelectedPath = m_targetFolderTextBox.Text;
            DialogResult result = m_folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_targetFolderTextBox.Text = m_folderBrowserDialog1.SelectedPath;
        }

        private void m_btPerform_Click(object sender, EventArgs e)
        {
            if (m_bRunning)
            {
                m_bAborted = true;
                return;
            }
            m_bRunning = true;
            m_btPerform.Image = iba.Properties.Resources.Stop;
            m_toolTip.SetToolTip(m_btPerform, Properties.Resources.CancelStr);
            m_browseFolderButton.Enabled = false;
            m_targetFolderTextBox.Enabled = false;
            m_lblProgress.Visible = true;
            m_progressBar.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void m_targetFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            m_btPerform.Enabled = Directory.Exists(m_targetFolderTextBox.Text);
        }


        public void Update(string message, int progress)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, int>(Update), message, progress);
                return;
            }
            m_lblProgress.Text = message;
            m_progressBar.Value = progress;
        }

        public bool Aborted
        {
            get { return m_bAborted; }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            m_stw.Split(null, m_targetFolderTextBox.Text,this);
            System.Threading.Thread.Sleep(1000);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, RunWorkerCompletedEventArgs>(backgroundWorker1_RunWorkerCompleted), sender, e);
            }
            m_lblProgress.Visible = false;
            m_progressBar.Visible = false;
            m_lblProgress.Text = "";
            m_btPerform.Image = m_btPerformImage;
            m_toolTip.SetToolTip(m_btPerform, Properties.Resources.SplitDialogTestButtonTooltipText);
            m_browseFolderButton.Enabled = true;
            m_targetFolderTextBox.Enabled = true;
            m_progressBar.Value = 0;
            m_bAborted = false;
            m_bRunning = false;
        }
    }
}
