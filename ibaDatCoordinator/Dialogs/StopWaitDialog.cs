using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using iba.Data;
using iba.Processing;

namespace iba.Dialogs
{
    public partial class StopWaitDialog : Form
    {
        ConfigurationData m_dataToStop;

        public StopWaitDialog() : this(null)
        {
        }

        public StopWaitDialog(ConfigurationData datatostop)
        {
            InitializeComponent();
            m_dataToStop = datatostop;
            if (m_dataToStop != null)
                this.Text = iba.Properties.Resources.stopConfiguration + ": " + m_dataToStop.Name;
            else
                this.Text = iba.Properties.Resources.stopAllConfigurations;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (m_dataToStop != null)
                TaskManager.Manager.StopAndWaitForConfiguration(m_dataToStop.Guid);
            else
                TaskManager.Manager.StopAndWaitForAllConfigurations();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            if (progressBar1.Value >= progressBar1.Maximum) progressBar1.Value = progressBar1.Minimum;
        }
    }
}