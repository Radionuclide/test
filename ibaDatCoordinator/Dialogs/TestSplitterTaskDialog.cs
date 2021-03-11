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
    public partial class TestSplitterTaskDialog : Form, ISplitterTaskProgress
    {

        public TestSplitterTaskDialog(SplitterTaskData data)
        {
            InitializeComponent();
            this.Icon = iba.Properties.Resources.SplitDat;
            m_data = data;
            m_stw = new SplitterTaskWorker(data);
            string to = "";
            Profiler.ProfileString(true, "SplitterTestDlg","OutputFolder", ref to, "");
            m_targetFolderTextBox.Text = to;
            m_btPerform.Enabled = true;
            m_btPerform.Image = iba.Properties.Resources.Stop;
            readyToSplit = false;
            WindowsAPI.SHAutoComplete(m_targetFolderTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            UseWaitCursor = true;
            m_bRunning = true;
            m_bgwCalc.RunWorkerAsync();
        }
        private Image m_btPerformImage;
        private bool readyToSplit;

        private void m_btOK_Click(object sender, EventArgs e)
        {
            UseWaitCursor = false;
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
            if (m_bRunning && readyToSplit) return;
            m_folderBrowserDialog1.ShowNewFolderButton = true;
            m_folderBrowserDialog1.SelectedPath = m_targetFolderTextBox.Text;
            DialogResult result = m_folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_targetFolderTextBox.Text = m_folderBrowserDialog1.SelectedPath;
        }

        private void m_btPerform_Click(object sender, EventArgs e)
        {
            UseWaitCursor = false;
            if (m_bRunning)
            {
                m_bAborted = true;
                return;
            }
            if (!readyToSplit)
            {

            }
            else
            {
                if (!Directory.Exists(m_targetFolderTextBox.Text))
                {
                    try
                    {
                        Directory.CreateDirectory(m_targetFolderTextBox.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK,MessageBoxIcon.Error);
                        return;
                    }
                }
                m_bRunning = true;
                m_btPerform.Image = iba.Properties.Resources.Stop;
                m_toolTip.SetToolTip(m_btPerform, iba.Properties.Resources.CancelStr);
                m_browseFolderButton.Enabled = false;
                m_targetFolderTextBox.Enabled = false;
                m_lblProgress.Visible = true;
                m_progressBar.Visible = true;
                m_bgwSplit.RunWorkerAsync();
            }
        }

        private void m_targetFolderTextBox_TextChanged(object sender, EventArgs e)
        {
           // m_btPerform.Enabled = Directory.Exists(m_targetFolderTextBox.Text);
        }


        public void Update(string message, int progress)
        {
            if (IsDisposed)
                return;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, int>(Update), message, progress);
                return;
            }
            if (readyToSplit)
            {
                m_lblProgress.Text = message;
                m_progressBar.Value = 100 * progress / (m_stw.GetPoints().Count + 1);
            }
            else
            {
                UseWaitCursor = false;

                DateTime startFile = DateTime.Now;
                try
                {
                    startFile = m_stw.GetStartTime();
                }
                catch (Exception ex)
                {
                    m_bAborted = true;
                    this.BeginInvoke(new Action(() => { MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error); Close(); }));
                }
                SplitterTaskWorker.CurrentInfo info = m_stw.CurrentPoints;
                if (info.valid)
                {
                    m_gvEntries.RowCount++;
                    string name = message;
                    DateTime trigStart = startFile + TimeSpan.FromSeconds(info.start);
                    DateTime trigStop = startFile + TimeSpan.FromSeconds(info.stop);
                    m_gvEntries.Rows[progress].Cells[0].Value = name;
                    m_gvEntries.Rows[progress].Cells[1].Value = trigStart;
                    m_gvEntries.Rows[progress].Cells[2].Value = trigStop;
                }
            }
        }

        public bool Aborted
        {
            get { return m_bAborted; }
        }

        private void m_bgwSplit_DoWork(object sender, DoWorkEventArgs e)
        {
            if (IsDisposed)
                return;
            m_stw.Split(null, m_targetFolderTextBox.Text,this);
        }

        private void m_bgwSplit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsDisposed)
                return;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, RunWorkerCompletedEventArgs>(m_bgwSplit_RunWorkerCompleted), sender, e);
                return;
            }
            m_lblProgress.Visible = false;
            m_progressBar.Visible = false;
            m_lblProgress.Text = "";
            m_btPerform.Image = m_btPerformImage;
            m_toolTip.SetToolTip(m_btPerform, iba.Properties.Resources.SplitDialogTestButtonTooltipText);
            m_browseFolderButton.Enabled = true;
            m_targetFolderTextBox.Enabled = true;
            m_progressBar.Value = 0;
            m_bAborted = false;
            m_bRunning = false;
        }
        private void m_bgwCalc_DoWork(object sender, DoWorkEventArgs e)
        {
            if (IsDisposed)
                return;
            List<double> points = m_stw.GetPoints(null,this,m_data.DatFilePassword);
        }

        private void m_bgwCalc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsDisposed)
                return;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object, RunWorkerCompletedEventArgs>(m_bgwCalc_RunWorkerCompleted), sender, e);
                return;
            }
            m_toolTip.SetToolTip(m_btPerform, iba.Properties.Resources.SplitDialogTestButtonTooltipText);
            m_btPerform.Image = m_btPerformImage = Bitmap.FromHicon(iba.Properties.Resources.SplitDat.Handle);
            m_btPerform.Enabled = true;
            m_bRunning = false;
            readyToSplit = true;
        }
    }
}
