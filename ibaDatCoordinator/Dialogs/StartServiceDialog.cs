using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace iba.Dialogs
{
    public partial class StartServiceDialog : Form
    {
        public StartServiceDialog()
        {
            InitializeComponent();
            m_result = false;
        }

        private bool m_result;
        public bool Result
        {
            get {return m_result;}
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                System.ServiceProcess.ServiceController myController =
                new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                myController.Start();
                myController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running,TimeSpan.FromMinutes(1.0));
                if (myController.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    MessageBox.Show(String.Format(iba.Properties.Resources.ServiceConnectProblem, iba.Properties.Resources.ServiceConnectProblem2, Environment.NewLine), iba.Properties.Resources.ServiceConnectProblemCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);                            
                    m_result = false;
                    return;
                }
                else
                {
                    m_result = true;
                }
                myController.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format(iba.Properties.Resources.ServiceConnectProblem, ex.Message, Environment.NewLine), iba.Properties.Resources.ServiceConnectProblemCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_result = false;
                return;
            }
            CommunicationObject com = (CommunicationObject)Activator.GetObject(typeof(CommunicationObject), "tcp://localhost:8800/IbaDatCoordinatorCommunicationObject");
            Program.CommunicationObject = new CommunicationObjectWrapper(com);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_progressBar.PerformStep();
            if (m_progressBar.Value >= m_progressBar.Maximum) m_progressBar.Value = m_progressBar.Minimum;
        }
    }
}