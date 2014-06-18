using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using iba.Utility;
using iba.Processing;
using iba.Data;

namespace iba.Controls
{
    public partial class ServiceSettingsControl : UserControl, IPropertyPane
    {
        public ServiceSettingsControl()
        {
            InitializeComponent();
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                m_gbApp.Text = iba.Properties.Resources.Application;
                m_lblPriority.Text = iba.Properties.Resources.PriorityApp;
                m_cbAutoStart.Visible = false;
            }
            m_toolTip.SetToolTip(m_registerButton,iba.Properties.Resources.RegisterIbaAnalyzer);
            
            m_globalCleanupWorker = new Dictionary<string,Timer>();
            m_globalCleanupData = new Dictionary<string, GlobalCleanupData>();

        }

        #region IPropertyPane Members

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            if (Program.RunsWithService != Program.ServiceEnum.NOSERVICE)
            {
                ServiceControllerEx service = new ServiceControllerEx("ibaDatCoordinatorService");
                try
                {
                    m_cbAutoStart.Checked = service.ServiceStart == ServiceStart.Automatic;
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (ex.InnerException != null)
                        msg += "\r\n" + ex.InnerException.Message;

                    MessageBox.Show(this, msg, "ibaDatCoordinator", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                service.Close();
            }
    
            m_cbRestartIbaAnalyzer.Checked = TaskManager.Manager.IsIbaAnalyzerCallsLimited;
            m_nudRestartIbaAnalyzer.Value = TaskManager.Manager.MaxIbaAnalyzerCalls;
            m_nudRestartIbaAnalyzer.Enabled = m_cbRestartIbaAnalyzer.Checked;
            m_nudMaxIbaAnalyzers.Value = TaskManager.Manager.MaxSimultaneousIbaAnalyzers;
            m_cbPostpone.Checked = TaskManager.Manager.DoPostponeProcessing;
            m_nudPostponeTime.Value = TaskManager.Manager.PostponeMinutes;
            m_nudPostponeTime.Enabled = m_cbPostpone.Checked;

            int iPc = TaskManager.Manager.ProcessPriority;
            m_nudResourceCritical.Value = (decimal)TaskManager.Manager.MaxResourceIntensiveTasks;
            System.Diagnostics.ProcessPriorityClass pc = (System.Diagnostics.ProcessPriorityClass) iPc;
            switch (pc)
            {
                case System.Diagnostics.ProcessPriorityClass.Idle:
                    m_comboPriority.SelectedIndex = 0;
                    break;
                case System.Diagnostics.ProcessPriorityClass.BelowNormal:
                    m_comboPriority.SelectedIndex = 1;
                    break;
                case System.Diagnostics.ProcessPriorityClass.Normal:
                    m_comboPriority.SelectedIndex = 2;
                    break;
                case System.Diagnostics.ProcessPriorityClass.AboveNormal:
                    m_comboPriority.SelectedIndex = 3;
                    break;
                case System.Diagnostics.ProcessPriorityClass.High:
                    m_comboPriority.SelectedIndex = 4;
                    break;
                case System.Diagnostics.ProcessPriorityClass.RealTime:
                    m_comboPriority.SelectedIndex = 5;
                    break;
            }
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                object o = key.GetValue("");
                m_tbAnalyzerExe.Text = Path.GetFullPath(o.ToString());
            }
            catch
            {
                m_tbAnalyzerExe.Text = iba.Properties.Resources.noIbaAnalyser;
            }
            m_executeIBAAButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);
            m_registerButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);

            m_pass = "";
            try
            {
                m_pass = TaskManager.Manager.Password;
                m_cbRememberPassword.Checked = TaskManager.Manager.RememberPassEnabled;
                m_nudRememberTime.Value = (decimal)TaskManager.Manager.RememberPassTime.TotalMinutes;
                m_nudRememberTime.Enabled = m_cbRememberPassword.Checked;
            }
            catch { }
            UpdatePassControls();

            m_globalCleanupData = TaskManager.Manager.GlobalCleanupDataList.ToDictionary(gc => gc.DriveName, gc => gc);
            InitializeDrives();
        }

        private void UpdatePassControls()
        {
            if (string.IsNullOrEmpty(m_pass))
            {
                m_SetChangePassBtn.Text = iba.Properties.Resources.SetStr;
                m_ClearPassBtn.Enabled = false;
                m_passwordStatusLabel.Text = iba.Properties.Resources.PassNotSet;
            }
            else
            {
                m_SetChangePassBtn.Text = iba.Properties.Resources.ChangeStr;
                m_ClearPassBtn.Enabled = true;
                m_passwordStatusLabel.Text = iba.Properties.Resources.PassSet;
            }
            TaskManager.Manager.Password = m_pass;
        }

        private Dictionary<string, Timer> m_globalCleanupWorker;
        private Dictionary<string, GlobalCleanupData> m_globalCleanupData;
        private string m_pass;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_tbAnalyzerExe.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        } 

        public void LeaveCleanup()
        {
            foreach (KeyValuePair<string, Timer> kvp in m_globalCleanupWorker)
            {
                kvp.Value.Stop();
            }
        }

        public void SaveData()
        {
            if (Program.RunsWithService != Program.ServiceEnum.NOSERVICE)
            {
                ServiceControllerEx service = new ServiceControllerEx("ibaDatCoordinatorService");
                try
                {
                    if (m_cbAutoStart.Checked && service.ServiceStart != ServiceStart.Automatic)
                    {
                        if (!iba.Utility.DataPath.IsAdmin) //elevated process start the service
                        {
                            if (System.Environment.OSVersion.Version.Major < 6)
                            {
                                MessageBox.Show(this, iba.Properties.Resources.UACText, iba.Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                service.Close();
                                return;
                            }
                            System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                            procInfo.UseShellExecute = true;
                            procInfo.ErrorDialog = true;

                            procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                            procInfo.FileName = Application.ExecutablePath;

                            procInfo.Arguments = "/setautomaticservicestart";
                            procInfo.Verb = "runas";

                            try
                            {
                                System.Diagnostics.Process.Start(procInfo);
                            }
                            catch
                            {
                                MessageBox.Show(this, iba.Properties.Resources.UACText, iba.Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            service.ServiceStart = ServiceStart.Automatic;
                        }
                    }
                    else if (!m_cbAutoStart.Checked && service.ServiceStart == ServiceStart.Automatic)
                    {
                        if (!iba.Utility.DataPath.IsAdmin) //elevated process start the service
                        {
                            if (System.Environment.OSVersion.Version.Major < 6)
                            {
                                MessageBox.Show(this, iba.Properties.Resources.UACText, iba.Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                service.Close();
                                return;
                            }
                            System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                            procInfo.UseShellExecute = true;
                            procInfo.ErrorDialog = true;

                            procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                            procInfo.FileName = Application.ExecutablePath;

                            procInfo.Arguments = "/setmanualservicestart";
                            procInfo.Verb = "runas";

                            try
                            {
                                System.Diagnostics.Process.Start(procInfo);
                            }
                            catch
                            {
                                MessageBox.Show(this, iba.Properties.Resources.UACText, iba.Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            service.ServiceStart = ServiceStart.Manual;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (ex.InnerException != null)
                        msg += "\r\n" + ex.InnerException.Message;

                    MessageBox.Show(this, msg, "ibaDatCoordinator", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                service.Close();
            }
            TaskManager.Manager.DoPostponeProcessing = m_cbPostpone.Checked;
            TaskManager.Manager.PostponeMinutes = (int)m_nudPostponeTime.Value;
            TaskManager.Manager.MaxIbaAnalyzerCalls = (int)m_nudRestartIbaAnalyzer.Value;
            TaskManager.Manager.IsIbaAnalyzerCallsLimited = m_cbRestartIbaAnalyzer.Checked;
            TaskManager.Manager.MaxSimultaneousIbaAnalyzers = (int)m_nudMaxIbaAnalyzers.Value;
            TaskManager.Manager.RememberPassEnabled = m_cbRememberPassword.Checked;
            TaskManager.Manager.RememberPassTime = TimeSpan.FromMinutes((double)m_nudRememberTime.Value);

            TaskManager.Manager.MaxResourceIntensiveTasks = (int) m_nudResourceCritical.Value;
            int iPc = 2;
            switch (m_comboPriority.SelectedIndex)
            {
                case 0: iPc = (int)System.Diagnostics.ProcessPriorityClass.Idle; break;
                case 1: iPc = (int)System.Diagnostics.ProcessPriorityClass.BelowNormal; break;
                case 2: iPc = (int)System.Diagnostics.ProcessPriorityClass.Normal; break;
                case 3: iPc = (int)System.Diagnostics.ProcessPriorityClass.AboveNormal; break;
                case 4: iPc = (int)System.Diagnostics.ProcessPriorityClass.High; break;
                case 5: iPc = (int)System.Diagnostics.ProcessPriorityClass.RealTime; break;
            }
            TaskManager.Manager.ProcessPriority = iPc;

            TaskManager.Manager.GlobalCleanupDataList.Clear();
            TaskManager.Manager.GlobalCleanupDataList.AddRange(m_globalCleanupData.Values);
        }

        #endregion

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

        private void m_registerButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_tbAnalyzerExe.Text;
                    ibaProc.StartInfo.Arguments = "/regserver";
                    ibaProc.Start();
                    ibaProc.WaitForExit();
                    ibaProc.StartInfo.Arguments = "-noninteractive /regserver";
                    ibaProc.Start();
                    ibaProc.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_tbAnalyzerExe_TextChanged(object sender, EventArgs e)
        {
            m_executeIBAAButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);
            m_registerButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);
        }

        private void m_SetChangePassBtn_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword()) return;
            iba.Dialogs.SpecifyPasswordDialog dlg = new iba.Dialogs.SpecifyPasswordDialog();
            dlg.Pass = m_pass;
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog(this);
            if (!dlg.Cancelled)
            {
                m_pass = dlg.Pass;
                UpdatePassControls();
            }
        }

        private void m_ClearPassBtn_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword()) return;
            m_pass = "";
            UpdatePassControls();
        }

        private void m_cbPostpone_CheckedChanged(object sender, EventArgs e)
        {
            m_nudPostponeTime.Enabled = m_cbPostpone.Checked;
        }

        private void m_cbRestartIbaAnalyzer_CheckedChanged(object sender, EventArgs e)
        {
            m_nudRestartIbaAnalyzer.Enabled = m_cbRestartIbaAnalyzer.Checked;
        }

        private void m_cbRememberPassword_CheckedChanged(object sender, EventArgs e)
        {
            m_nudRememberTime.Enabled = m_cbRememberPassword.Checked;
        }

        private void m_btnOptimize_Click(object sender, EventArgs e)
        {
            if (!iba.Utility.DataPath.IsAdmin) //elevated process start the service
            {
                if (System.Environment.OSVersion.Version.Major < 6)
                {
                    MessageBox.Show(this, iba.Properties.Resources.UACTextRegistrySettings, iba.Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show(this, iba.Properties.Resources.UACTextRegistrySettings, iba.Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                RegistryOptimizer.DoWork();
            }
        }


        private void InitializeDrives()
        {

            var rowIdx = 0;
            GlobalCleanupData gcd; 
            foreach (var drive in DriveUtil.LocalDrives())
            {
                try
                {
                    if (!m_globalCleanupData.TryGetValue(drive.Name, out gcd))
                    {
                        gcd = new GlobalCleanupData() { DriveName = drive.Name };
                        m_globalCleanupData.Add(drive.Name, gcd);
                    }

                    gcd.IsSystemDrive = drive.IsSystemDrive();
                    gcd.TotalSize = drive.TotalSize;

                    string driveName = String.Format("{0} - [{1}]", drive.Name, drive.VolumeLabel);
                    if (gcd.IsSystemDrive)
                    {
                        gcd.Active = false;
                        driveName = driveName + " - (System)";
                    }

                    Debug.WriteLine(driveName);

                    rowIdx = tbl_GlobalCleanup.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                    AddTextToTableLayout(driveName, 0, rowIdx, ContentAlignment.TopLeft);
                    if (drive.IsReady)
                    {
                        AddTextToTableLayout(PathUtil.GetSizeReadable(drive.TotalSize), 1, rowIdx, ContentAlignment.TopRight);
                        AddTextToTableLayout(PathUtil.GetSizeReadable(drive.TotalFreeSpace), 2, rowIdx, ContentAlignment.TopRight);

                        AddQuotaToTableLayout(gcd, 3, rowIdx);
                        AddRescanTimeToTableLayout(gcd, 4, rowIdx);
                        AddIsActiveToTableLayout(gcd, 5, rowIdx);
                    }
                }
                catch
                { }

            }
            gb_GlobalCleanup.Height = (rowIdx + 1) * 32;

        }
        private void AddTextToTableLayout(string displayValue, int colIdx, int rowIdx, ContentAlignment textAlign)
        {

            Panel pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.AutoSize = true;
            pnl.Margin = new Padding(0, 5, 0, 0);

            Label lbl = new Label();
            lbl.Dock = DockStyle.Fill;
            lbl.Text = displayValue;
            lbl.TextAlign = textAlign;

            pnl.Controls.Add(lbl);

            tbl_GlobalCleanup.Controls.Add(pnl, colIdx, rowIdx);
        }

        private void AddQuotaToTableLayout(GlobalCleanupData data, int colIdx, int rowIdx)
        {
            Panel pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.AutoSize = true;

            PercentUpDown nud = new PercentUpDown();
            nud.Location = new Point(5, 0);
            nud.Margin = new Padding(0);
            nud.Size = new Size(54, 20);
            nud.Value = (decimal)data.PercentageFree;

            Label lbl = new Label();
            lbl.Margin = new Padding(0);
            lbl.Text = PathUtil.GetSizeReadable((long)(data.TotalSize * (data.PercentageFree / 100.0)));
            lbl.Location = new Point(nud.Location.X + nud.Size.Width + 6, nud.Location.Y + 2);

            nud.ValueChanged += (s, e) => {
                data.PercentageFree = (int)nud.Value;
                lbl.Text = PathUtil.GetSizeReadable((long)(data.TotalSize * (data.PercentageFree / 100.0))); 
            };

            pnl.Controls.Add(nud);
            pnl.Controls.Add(lbl);

            tbl_GlobalCleanup.Controls.Add(pnl, colIdx, rowIdx);

        }

        private void AddRescanTimeToTableLayout(GlobalCleanupData data, int colIdx, int rowIdx)
        {
            var lblHeader = tbl_GlobalCleanup.GetControlFromPosition(colIdx, 0);

            Panel pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.AutoSize = true;

            NumericUpDown nud = new NumericUpDown();
            nud.Location = new Point(0, 0);
            nud.Margin = new Padding(0);
            nud.Size = new Size(54, 20);
            nud.Maximum = 86400; // one day 86400 min
            nud.Minimum = 1;
            nud.Value = (decimal)data.RescanTime;

            Label lbl = new Label();
            lbl.Margin = new Padding(0);
            lbl.Text = "min";

            CenterControls(lblHeader.Width, nud, lbl);

            pnl.Controls.Add(nud);
            pnl.Controls.Add(lbl);

            tbl_GlobalCleanup.Controls.Add(pnl, colIdx, rowIdx);

        }

        private static void CenterControls(int headerWidth, NumericUpDown nud, Label lbl)
        {
            var width = nud.Size.Width + 5 + lbl.PreferredWidth;
            var left = headerWidth / 2 - width / 2;

            nud.Left = left;
            lbl.Location = new Point(nud.Location.X + nud.Size.Width + 5, nud.Location.Y + 2);
        }

        private void AddIsActiveToTableLayout(GlobalCleanupData data, int colIdx, int rowIdx)
        {
            var lblHeader = tbl_GlobalCleanup.GetControlFromPosition(colIdx, 0);
            
            Panel pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.AutoSize = true;

            CheckBox cb = new CheckBox();
            cb.Location = new Point(lblHeader.Width / 2 - 15 / 2, 0);
            cb.Checked = data.Active;
            cb.Enabled = !data.IsSystemDrive;

            cb.CheckedChanged += (s, e) => {
                data.Active = cb.Checked;
                StartStopCleanup(rowIdx, data); 
            };

            pnl.Controls.Add(cb);

            tbl_GlobalCleanup.Controls.Add(pnl, colIdx, rowIdx);
        }

        private void StartStopCleanup(int rowIdx, GlobalCleanupData data)
        {
            for (int i = 0; i < tbl_GlobalCleanup.ColumnCount - 1; i++)
            {
                tbl_GlobalCleanup.GetControlFromPosition(i, rowIdx).Enabled = !data.Active;
            }

            if (data.IsSystemDrive)
            {
                data.Active = false;
                return;
            }

            if (data.Active)
                Start(data);
            else
                Stop(data);
        }

        private void Start(GlobalCleanupData data)
        {
            Stop(data);

            var drive = new DriveInfo(data.DriveName);
            try
            {
                if (!drive.IsReady)
                    return;
            }
            catch (Exception)
            {
                return;
            }




            var t = new GlobalCleanupTaskData(new ConfigurationData(data.DriveName, false));
#if DEBUG
            t.DestinationMapUNC = @"Y:\Cleanup";
#else
            t.DestinationMapUNC = data.DriveName;
#endif

            t.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            double factor = 1 - data.PercentageFree / 100.0;
            double result = (data.TotalSize / 1024 / 1024) * factor;

            t.Quota = (uint)result;

            var quota = new FileQuotaCleanup(t, ".dat");
            quota.FastSearch = true;
            quota.NewLogMessage += (s, e) => { Debug.WriteLine(e.Message); };

            CleanupFiles(quota, t.DestinationMapUNC);

            var cleanupTimer = new System.Windows.Forms.Timer();
            cleanupTimer.Interval = (int)(data.RescanTime * 1000 * 10);

            m_globalCleanupWorker.Add(data.DriveName, cleanupTimer);

            cleanupTimer.Tick += (s, e) => { TimerTick(s as Timer, quota, t.DestinationMapUNC); };

            cleanupTimer.Tag = true;
            cleanupTimer.Start();
        }
        
        private void Stop(GlobalCleanupData data)
        {
            Timer tmr;
            if (!m_globalCleanupWorker.TryGetValue(data.DriveName, out tmr))
                return;
            m_globalCleanupWorker.Remove(data.DriveName);

            tmr.Tag = false;
            tmr.Stop();

        }

        private void TimerTick(Timer tmr, FileQuotaCleanup quota, string cleanupPath)
        {
            tmr.Stop();

            CleanupFiles(quota, cleanupPath);

            if ((bool)tmr.Tag)
                tmr.Start();

        }

        private void CleanupFiles(FileQuotaCleanup quota, string cleanupPath)
        {
            var sw = Stopwatch.StartNew();
            Log(cleanupPath + " Cleanup start");
            quota.Init();
            sw.Stop();
            Log(cleanupPath + " Cleanup init finished " + (sw.ElapsedMilliseconds / 1000.0).ToString("0.000") + "s");
            sw.Restart();
            quota.Clean("Cleanup");
            sw.Stop();
            Log(cleanupPath + " Cleanup finished " + (sw.ElapsedMilliseconds / 1000.0).ToString("0.000") + "s");
        }

        private void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }

    internal enum ServiceStart
    {
        Boot = 0,
        System = 1,
        Automatic = 2,
        Manual = 3,
        Disabled = 4
    }

    internal class ServiceControllerEx : System.ServiceProcess.ServiceController
    {
        public ServiceControllerEx() : base() { }
        public ServiceControllerEx(string serviceName) : base(serviceName) { }
        public ServiceControllerEx(string serviceName, string machineName) : base(serviceName, machineName) { }

        public ServiceStart ServiceStart
        {
            get
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(
                    "SYSTEM\\CurrentControlSet\\Services\\" + this.ServiceName);
                if (key == null)
                    return ServiceStart.Automatic;

                ServiceStart start = (ServiceStart)key.GetValue("Start");
                key.Close();
                key = null;
                return (start);
            }
            set
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(
                    "SYSTEM\\CurrentControlSet\\Services\\" + this.ServiceName, true);
                if (key == null)
                    return;

                key.SetValue("Start", (int)value);
                key.Close();
                key = null;
            }
        }
    }
}
