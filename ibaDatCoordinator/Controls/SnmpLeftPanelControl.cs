using System;
using System.Windows.Forms;
using iba.Processing;

namespace iba.Controls
{
    public partial class SnmpLeftPanelControl : UserControl 
    {
        //private readonly MainForm _mainForm;
        public SnmpLeftPanelControl(MainForm mainForm)
        {
            //_mainForm = mainForm;
            InitializeComponent();
        }

        private void SnmpLeftPanelControl_Load(object sender, EventArgs e)
        {
            SnmpWorker snmpWorker = TaskManager.Manager?.SnmpWorker;
            if (snmpWorker == null)
            {
                return;
            }
            //subscribe to status change
            snmpWorker.StatusChanged += SnmpWorker_StatusChanged;
            //set initial status
            snmpWorker.ApplyStatusToTextBox(tbStatus);
        }

        private void SnmpWorker_StatusChanged(object sender, SnmpWorkerStatusChangedEventArgs e)
        {
            if (tbStatus.InvokeRequired)
            {
                Invoke(new EventHandler<SnmpWorkerStatusChangedEventArgs>(SnmpWorker_StatusChanged), sender, e);
            }
            else
            {
                tbStatus.BackColor = e.Color;
                tbStatus.Text = e.Message;
                cbEnabled.Checked = e.Status != SnmpWorkerStatus.Stopped;
            }
        }

        private void buttonShowDebug_Click(object sender, EventArgs e)
        {
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            // todo remove timer

            var worker = TaskManager.Manager?.SnmpWorker;
            if (worker == null) return;

            string str = "";
            lock (worker.LockObject)
            {
                var od = worker.ObjectsData;
                if (od != null)
                {
                    //str += $"LastUpdated: {od.TimeStamp.ToLongTimeString()}\r\n";
                    str += "\r\n";
                    str += $"Cleanup: {od.GlobalCleanup.Count}\r\n";
                    str += $"StandardJobs: {od.StandardJobs.Count}\r\n";
                    str += $"ScheduledJobs: {od.ScheduledJobs.Count}\r\n";
                    str += $"OneTimeJobs: {od.OneTimeJobs.Count}\r\n";
                    str += "\r\n";
                    str += $"Total SNMP Objects: {worker.IbaSnmp?.GetListOfAllOids().Count}\r\n";
                }

                tbGeneralInfo.Text = str;
            }

            tbLog.Text = SnmpWorker._tmpLog;
        }

        private void buttonCleanLog_Click(object sender, EventArgs e)
        {
            SnmpWorker._tmpLog = "";
        }
    }
}
