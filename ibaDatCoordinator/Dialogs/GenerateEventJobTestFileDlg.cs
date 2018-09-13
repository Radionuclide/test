using iba.Data;
using iba.Logging;
using iba.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.Dialogs
{
    public partial class GenerateEventJobTestFileDlg : Form
    {
        #region Members
        EventJobTestGenerator m_generator;
        bool m_bGeneratorFinished;

        public List<string> GeneratedFiles { get { return m_generator?.GeneratedFiles; } }
        #endregion

        #region Initialize
        public GenerateEventJobTestFileDlg(ConfigurationData confData, string fileName)
        {
            InitializeComponent();
            m_generator = new EventJobTestGenerator(confData, fileName);
            m_generator.StatusChanged += OnStatusChanged;
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_generator != null)
                {
                    m_generator.Dispose();
                    m_generator = null;
                }

                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GenerateEventJobTestFileDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_bGeneratorFinished)
                m_generator.StatusChanged -= OnStatusChanged;
            else
            {
                e.Cancel = true;
                m_generator.Cancel();
            }
        }
        #endregion

        #region Actions
        public new DialogResult ShowDialog()
        {
            m_generator.Start();
            return base.ShowDialog();
        }

        void OnStatusChanged(EventJobTestStatus status)
        {
            if (status == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<EventJobTestStatus>(OnStatusChanged), status);
                return;
            }

            AppendText(status);

            if (!status.Finished)
                return;

            m_bGeneratorFinished = true;

            if (status.StatusLevel == Level.Exception)
                m_btClose.Text = Properties.Resources.OK;
            else
            {
                if (status.StatusMessage != Properties.Resources.EventJob_TestFile_Canceled)
                    DialogResult = DialogResult.OK;

                Close();
            }
        }

        void AppendText(EventJobTestStatus status)
        {
            Color lColor = m_rtbStatus.ForeColor;
            if (status.StatusLevel == Level.Exception)
                lColor = Color.Red;
            if (status.StatusLevel == Level.Warning)
                lColor = Color.Orange;

            m_rtbStatus.SelectionStart = m_rtbStatus.TextLength;
            m_rtbStatus.SelectionLength = 0;

            m_rtbStatus.SelectionColor = lColor;
            m_rtbStatus.AppendText(status.StatusMessage);
            m_rtbStatus.SelectionColor = m_rtbStatus.ForeColor;
            m_rtbStatus.AppendText(Environment.NewLine);
        }
        #endregion
    }
}
