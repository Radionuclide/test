using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Controls;
using iba.Data;
using iba.DatCoordinator.Status.Dialogs;
using iba.DatCoordinator.Status.Utility;
using Microsoft.Win32;

namespace iba.DatCoordinator.Status
{
    public partial class StatusForm : Form, IExternalCommand
    {
        private static Icon ServiceRunningIcon = Properties.Resources.StatusIconRunning;
        private static Icon ServiceStoppedIcon = Properties.Resources.StatusIconStopped;
        private static Icon ServiceDisconnectedIcon = Properties.Resources.StatusIconDisconnected;

        public StatusForm()
        {
            InitializeComponent();
            m_quitForm = new QuitForm(this, "ibaDatCoordinatorStatusCloseForm", true);
            m_quitForm.CreateHandle(new CreateParams());

            WindowState = FormWindowState.Minimized;
            m_miRestoreCoordinator = new ToolStripMenuItem(Properties.Resources.notifyIconMenuItemRestore, null, miRestore_Click);
            m_miRestoreCoordinator.Font = new Font(m_miRestoreCoordinator.Font, FontStyle.Bold);
            m_miStartService = new ToolStripMenuItem(Properties.Resources.notifyIconMenuItemStartService, null, miStartService_Click);
            m_miStopService = new ToolStripMenuItem(Properties.Resources.notifyIconMenuItemStopService, null, miStopService_Click);
            if (!Program.IsAdmin)
            {
                m_miStartService.Image = Properties.Resources.shield;
                m_miStopService.Image = Properties.Resources.shield;
            }


            m_miExit = new ToolStripMenuItem(Properties.Resources.notifyIconMenuItemExit, null, miExit_Click);
            ToolStripItem seperator = new ToolStripSeparator();
            ToolStripItem seperator2 = new ToolStripSeparator();
            m_iconMenu = new ContextMenuStrip();
            m_iconMenu.Items.AddRange(new ToolStripItem[]
                {
                    m_miRestoreCoordinator,
                    seperator,
                    m_miStartService,
                    m_miStopService,
                    seperator2,
                    m_miExit
                });
            m_iconMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
            m_iconMenu.Opening += new CancelEventHandler(m_iconMenu_Opening);

            m_iconEx = new NotifyIcon();
            m_iconEx.ContextMenuStrip = m_iconMenu;
            m_iconEx.DoubleClick += new EventHandler(iconEx_DoubleClick);
            m_iconEx.Visible = false;

            ((Bitmap)m_executeIBAAButton.Image).MakeTransparent(Color.Magenta);
            //m_toolTip.SetToolTip(m_registerButton, iba.Properties.Resources.RegisterIbaAnalyzer);

            Text = Text +  " " + DatCoVersion.GetVersion();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (WindowState == FormWindowState.Minimized)
                Hide();

            UpdateServerStatus(true);
            m_timer.Enabled = true;
            m_iconEx.Visible = true;
            string output = iba.Utility.PathUtil.FindAnalyzerPath();
            m_tbAnalyzerExe.Text = output;
            m_executeIBAAButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);
            //m_registerButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);
        }

        private bool firstFailure = true;
        private bool bRunning = false;
        private void UpdateServerStatus(bool bForce)
        {
            //Service status

            if (!bForce && !Visible) return; //don't update when not shown

            bool bServiceError = false;
            ServiceControllerStatus status;

            ServiceControllerEx sc = new ServiceControllerEx("ibaDatCoordinatorService");
            try
            {
                status = sc.Status;
            }
            catch (Exception)
            {
                status = ServiceControllerStatus.Stopped;
                if (firstFailure)
                {
                    //if (LogData.Data.Logger != null) LogData.Data.Logger.Log(Logging.Level.Info, "Failure getting service info: " + ex.Message);
                    firstFailure = false;
                }
                bServiceError = true;
            }


            if (status != ServiceControllerStatus.Stopped && !updatingCombo)
            {
                try
                {
                    m_comboPriority.SelectedIndexChanged -= new System.EventHandler(this.m_comboPriority_SelectedIndexChanged);
                    int number = 2;
                    var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(String.Format(@"SOFTWARE\{0}\{1}", "iba", "ibaDatCoordinator"));
                    if (key == null)
                        number = 2;
                    else
                        number = (int)key.GetValue("Priority", 2);
                    m_comboPriority.SelectedIndex = number;
                }
                catch
                {
                }
                finally
                {
                    m_comboPriority.SelectedIndexChanged += new System.EventHandler(this.m_comboPriority_SelectedIndexChanged);
                }
            }


            if (!updatingAutoStart)
            {
                m_cbAutoStart.CheckedChanged -= new System.EventHandler(this.m_cbAutoStart_CheckedChanged);
                if (bServiceError)
                    m_cbAutoStart.Checked = false;
                else
                    m_cbAutoStart.Checked = sc.ServiceStart == ServiceStart.Automatic;
                m_cbAutoStart.CheckedChanged += new System.EventHandler(this.m_cbAutoStart_CheckedChanged);
            }
            sc.Close();


            if (status == ServiceControllerStatus.Running)
            {
                if (bForce || !bRunning)
                {
                    bRunning = true;
                    m_lblServiceStatus.Text = Properties.Resources.serviceStatRunning;
                    m_lblServiceStatus.BackColor = Color.LimeGreen;
                    m_btnStart.Enabled = false;
                    m_btnStop.Enabled = true;
                    m_btnRestart.Enabled = true;
                    m_btnOptimize.Enabled = true;
                    m_btTransferAnalyzerSettings.Enabled = true;
                    m_iconEx.Icon = this.Icon = ServiceRunningIcon;
                    m_iconEx.Text = Properties.Resources.ServiceStatusTooltipRunning;
                }
            }
            else
            {
                if (bForce || bRunning)
                {
                    bRunning = false;
                    m_lblServiceStatus.Text = Properties.Resources.serviceStatStopped;
                    m_lblServiceStatus.BackColor = Color.Red;
                    m_btnStart.Enabled = true;
                    m_btnStop.Enabled = false;
                    m_btnRestart.Enabled = false;
                    m_btnOptimize.Enabled = false;
                    m_btTransferAnalyzerSettings.Enabled = false;
                    m_iconEx.Icon = this.Icon = bServiceError?ServiceDisconnectedIcon:ServiceStoppedIcon;
                    m_iconEx.Text = bServiceError ? Properties.Resources.ServiceStatusTooltipError : Properties.Resources.ServiceStatusTooltipStopped;
                }
            }

            if (!updatingPortNr)
            {
                m_tbPort.Text = Program.ServicePortNr.ToString();
            }
        }

        static public void SetServicePriority(int number)
        { //requires admin ...
            try
            {
                RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(String.Format(@"SOFTWARE\{0}\{1}",
                "iba", "ibaDatCoordinator"));
                if (key != null)
                {
                    key.SetValue("Priority", number);
                    key.Close();
                }

                System.ServiceProcess.ServiceController myController =
                    new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                myController.ExecuteCommand(130);
                myController.Close();
            }
            catch
            {
            }
        }

        private bool m_actualClose = false;
        protected override void OnClosing(CancelEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                iba.Utility.FormStateSerializer.SaveSettings(this, "StatusForm");

            if (!m_actualClose)
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Minimized;
                    ShowInTaskbar = false;
                    Hide();
                    e.Cancel = true;
                }
                return;
            }

            //if (LogData.Data.Logger != null) LogData.Data.Logger.Close();
            base.OnClosing(e);
        }

        private QuitForm m_quitForm;
        private NotifyIcon m_iconEx;
        public NotifyIcon NotifyIcon
        {
            get { return m_iconEx; }
        }

        public int PortNr {
            get
            {
                return int.Parse(m_tbPort.Text);
            }
            set
            {
                m_tbPort.Text = value.ToString();
            }
        }

        private ContextMenuStrip m_iconMenu;
        private ToolStripMenuItem m_miRestoreCoordinator;
        private ToolStripMenuItem m_miStartService;
        private ToolStripMenuItem m_miStopService;
        private ToolStripMenuItem m_miExit;

        public void OnExternalActivate()
        {
            Show();
            Activate();
            WindowState = FormWindowState.Normal;
            iba.Utility.FormStateSerializer.LoadSettings(this, "StatusForm", true);
            ShowInTaskbar = true;
        }

        #region menuOptions
        void m_iconMenu_Opening(object sender, CancelEventArgs e)
        {
            ServiceController service = new ServiceController("IbaDatCoordinatorService");
            try
            {
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    m_miStartService.Enabled = true;
                    m_miStopService.Enabled = false;
                }
                else
                {
                    m_miStartService.Enabled = false;
                    m_miStopService.Enabled = true;
                }
            }
            catch (Exception)
            {
                m_miStartService.Enabled = false;
                m_miStopService.Enabled = false;
            }
            service.Close();
        }

        private void miRestore_Click(object sender, System.EventArgs e)
        {
            OnExternalActivate();
        }

        private void miExit_Click(object sender, System.EventArgs e)
        {
            OnExternalClose();
        }

        private void miStartService_Click(object sender, System.EventArgs e)
        {
            //if (!Utility.Crypt.CheckPassword(this)) return;
            OnStartService();
        }

        private void miStopService_Click(object sender, System.EventArgs e)
        {
            //if (!Utility.Crypt.CheckPassword(this)) return;
            OnStopService();
        }

        private void iconEx_DoubleClick(object sender, System.EventArgs e)
        {
            miRestore_Click(null, null);
        }

        #endregion

        public void OnExternalClose()
        {
            m_actualClose = true;
            if (m_iconEx != null)
                m_iconEx.Dispose();
            Close();
        }

        #region Service related methods

        public void OnStopService()
        {
            bool bTimerEnabled = m_timer.Enabled;
            m_timer.Enabled = false;
            bool result = false;
            using (StopServiceDialog ssd = new StopServiceDialog())
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ssd.StartPosition = FormStartPosition.CenterScreen;
                    ssd.ShowDialog();
                    result = ssd.Result;
                }
                else
                {
                    ssd.StartPosition = FormStartPosition.CenterParent;
                    ssd.ShowDialog(this);
                    result = ssd.Result;
                }
            }
            if (result)
            {
                //if (LogData.Data.Logger != null) LogData.Data.Logger.Log(Logging.Level.Info, iba.Properties.Resources.logServiceStopped);

            }
            UpdateServerStatus(true);
            m_timer.Enabled = bTimerEnabled;
        }

        public void OnStartService()
        {
            bool bTimerEnabled = m_timer.Enabled;
            m_timer.Enabled = false;
            //startservice dialog
            bool result = false;
            using (StartServiceDialog ssd = new StartServiceDialog())
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ssd.StartPosition = FormStartPosition.CenterScreen;
                    ssd.ShowDialog();
                    result = ssd.Result; 
                }
                else
                {
                    ssd.StartPosition = FormStartPosition.CenterParent;
                    ssd.ShowDialog(this);
                    result = ssd.Result;
                }
            }
            if (result)
            {
                //if (LogData.Data.Logger != null) LogData.Data.Logger.Log(Logging.Level.Info, iba.Properties.Resources.logServiceStarted);

            }
            UpdateServerStatus(true);
            m_timer.Enabled = bTimerEnabled;
        }

        public void OnRestartService()
        {
            bool bTimerEnabled = m_timer.Enabled;
            m_timer.Enabled = false;
            using (RestartServiceDialog ssd = new RestartServiceDialog())
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ssd.StartPosition = FormStartPosition.CenterScreen;
                    ssd.ShowDialog();
                }
                else
                {
                    ssd.StartPosition = FormStartPosition.CenterParent;
                    ssd.ShowDialog(this);
                }
            }
            UpdateServerStatus(true);
            m_timer.Enabled = bTimerEnabled;
        }

        #endregion

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_QUERYENDSESSION = 0x011;
            if (m.Msg == WM_QUERYENDSESSION)
            {
                m_actualClose = true;
            }
            base.WndProc(ref m);

        } //WndProc 

        private void m_btnStart_Click(object sender, EventArgs e)
        {
            OnStartService();
        }

        private void m_btnStop_Click(object sender, EventArgs e)
        {
            OnStopService();
        }


        private void m_btnRestart_Click(object sender, EventArgs e)
        {
            OnRestartService();
        }

        private void m_btTransferAnalyzerSettings_Click(object sender, EventArgs e)
        {
            iba.Utility.Pair<String, String> res = GetFilesForTransfer();
            //if (!Program.IsAdmin) //elevated process start the service
            //{
                if (System.Environment.OSVersion.Version.Major < 6)
                {
                    MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                procInfo.UseShellExecute = true;
                procInfo.ErrorDialog = true;

                procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                procInfo.FileName = Application.ExecutablePath;

                procInfo.Arguments = string.Format("/transfersettings: \"{0}\" \"{1}\"",res.First,res.Second);
                procInfo.Verb = "runas";

                try
                {
                    System.Diagnostics.Process.Start(procInfo);
                }
                catch
                {
                    MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            //}
            //else
              //  OnTransferSettings(res.First, res.Second);
        }

        private iba.Utility.Pair<string, string> GetFilesForTransfer()
        {
            string IbaAnalyzerPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            IbaAnalyzerPath = Path.Combine(IbaAnalyzerPath, "iba", "ibaAnalyzer");
            string tempDir = System.IO.Path.GetTempPath();
            string outFile = Path.Combine(tempDir, "ibaAnalyzer.reg");
            iba.Utility.RegistryExporter.ExportIbaAnalyzerKey(outFile);
            return new iba.Utility.Pair<string, string>(IbaAnalyzerPath, outFile);
        }

        public static void OnTransferSettings(string AnalyzerFolder, string regFile)
        {
            try
            {
                var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(String.Format(@"SOFTWARE\{0}\{1}", "iba", "ibaDatCoordinator"));
                if (key != null)
                {
                    key.SetValue("RegFile", regFile);
                    key.SetValue("AnalyzerFolder", AnalyzerFolder);
                    key.Close();
                }
                else
                    return;
                System.ServiceProcess.ServiceController myController =
                    new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                //instruct service to register
                myController.ExecuteCommand(128);//instruct service to connect to the named pipe
                myController.Close();
                File.Delete(regFile);
                MessageBox.Show(Properties.Resources.TransferIbaAnalyzerSettingsSuccess, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PortNrValidate()
        {
            int prevNr = Program.ServicePortNr;
            int newNr = int.Parse(m_tbPort.Text);
            if (prevNr != newNr)
            {
                string text = Properties.Resources.RestartServerQuestion;
                DialogResult res = DialogResult.No;
                res = MessageBox.Show(this, text, this.Text,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                try
                {
                    if (res == DialogResult.Yes)
                    {
                        SetPortNumber(newNr);
                        m_btnStop_Click(this, EventArgs.Empty);
                        m_btnStart_Click(this, EventArgs.Empty);
                    }
                    else if (res == DialogResult.No)
                    {
                        SetPortNumber(newNr);
                    }
                    else
                    {
                        m_tbPort.Text = prevNr.ToString();
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (ex.InnerException != null)
                        msg += "\r\n" + ex.InnerException.Message;

                    MessageBox.Show(this, msg, "", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        void SetPortNumber(int number)
        {
            if (!Program.IsAdmin) //elevated process start the service
            {
                if (System.Environment.OSVersion.Version.Major < 6)
                {
                    MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                procInfo.UseShellExecute = true;
                procInfo.ErrorDialog = true;

                procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                procInfo.FileName = Application.ExecutablePath;

                procInfo.Arguments = "/setportnumber:" + number.ToString();
                procInfo.Verb = "runas";

                try
                {
                    System.Diagnostics.Process.Start(procInfo);
                }
                catch
                {
                    MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Program.ServicePortNr = number;
            }
        }

        private bool updatingAutoStart;
        private void m_cbAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            ServiceControllerEx service = new ServiceControllerEx("ibaDatCoordinatorService");
            updatingAutoStart = true;
            if (!Program.IsAdmin) //elevated process start the service
            {
                if (System.Environment.OSVersion.Version.Major < 6)
                {
                    MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    service.Close();
                    return;
                }
                System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                procInfo.UseShellExecute = true;
                procInfo.ErrorDialog = true;

                procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                procInfo.FileName = Application.ExecutablePath;

                procInfo.Arguments = "/toggleservicestart";
                procInfo.Verb = "runas";

                try
                {
                    Process p = System.Diagnostics.Process.Start(procInfo);
                    p.WaitForExit(1000);
                }
                catch
                {
                    MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                service.ServiceStart = service.ServiceStart == ServiceStart.Manual ? ServiceStart.Automatic : ServiceStart.Manual;
            }
            updatingAutoStart = false;
        }

        private void m_registerButton_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                procInfo.UseShellExecute = true;
                procInfo.ErrorDialog = true;

                procInfo.FileName = m_tbAnalyzerExe.Text;

                procInfo.Arguments = "/register";
                procInfo.Verb = "runas";

                try
                {
                    System.Diagnostics.Process.Start(procInfo);
                }
                catch
                {
                    MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_tbAnalyzerExe.Text;
                    ibaProc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_browseIbaAnalyzerButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog.Filter = "ibaAnalyzer Executable (*.exe)|*.exe";
            try
            {
                if (File.Exists(m_tbAnalyzerExe.Text))
                    m_openFileDialog.FileName = m_tbAnalyzerExe.Text;
            }
            catch
            {
            }
            DialogResult result = m_openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                m_tbAnalyzerExe.Text = m_openFileDialog.FileName;
        }

        //private void m_tbAnalyzerExe_TextChanged(object sender, EventArgs e)
        //{
        //    m_registerButton.Enabled = m_executeIBAAButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);
        //}

        private void m_btnOptimize_Click(object sender, EventArgs e)
        {
            if (!Program.IsAdmin) //elevated process start the service
            {
                if (System.Environment.OSVersion.Version.Major < 6)
                {
                    MessageBox.Show(this, Properties.Resources.UACTextRegistrySettings, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                procInfo.UseShellExecute = true;
                procInfo.ErrorDialog = true;

                procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                procInfo.FileName = Application.ExecutablePath;

                procInfo.Arguments = "/optimizeregistry";
                procInfo.Verb = "runas";

                try
                {
                    System.Diagnostics.Process.Start(procInfo);
                }
                catch
                {
                    MessageBox.Show(this, Properties.Resources.UACTextRegistrySettings, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                RegistryOptimizer.DoWork();
            }
        }

        private void m_timer_Tick(object sender, EventArgs e)
        {
            m_timer.Enabled = false;

            try
            {
                UpdateServerStatus(false);
            }
            catch (Exception)
            {
            }

            m_timer.Enabled = true;
        }

        private bool updatingCombo;
        private void m_comboPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_comboPriority.SelectedIndex < 0)
                return;

            updatingCombo = true;

            if (!Program.IsAdmin) //elevated process start the service
            {
                if (System.Environment.OSVersion.Version.Major < 6)
                {
                    MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                procInfo.UseShellExecute = true;
                procInfo.ErrorDialog = true;

                procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                procInfo.FileName = Application.ExecutablePath;

                procInfo.Arguments = "/setServicePriority:" + m_comboPriority.SelectedIndex.ToString();
                procInfo.Verb = "runas";

                try
                {
                    System.Diagnostics.Process.Start(procInfo);
                }
                catch
                {
                    MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                SetServicePriority(m_comboPriority.SelectedIndex);
            }
            updatingCombo = false;
        }

        private bool updatingPortNr;
        private void btChangePort_Click(object sender, EventArgs e)
        {
            using (ChangeServerPortForm form = new ChangeServerPortForm())
            {
                form.PortNr = this.PortNr;
                form.Icon = Icon;
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    updatingPortNr = true;
                    this.PortNr = form.PortNr;
                    PortNrValidate();
                    updatingPortNr = false;
                }
            }
        }
    }
}
