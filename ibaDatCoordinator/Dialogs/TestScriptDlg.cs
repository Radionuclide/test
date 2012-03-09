using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace iba.Dialogs
{
    public partial class TestScriptDlg : Form
    {
        public TestScriptDlg(iba.Data.BatchFileData data)
        {
            InitializeComponent();
            string cmdLineArgs = data.ParsedArguments(data.TestDatFile);
            m_lblFileName.Text = data.BatchFile;
            m_bClientSide = data.TestOnClientSide;
            if (string.IsNullOrEmpty(cmdLineArgs))
            {
                m_lblArguments.Text = "";
            }
            else
            {
                string entirecmdline = "\"" + data.BatchFile + "\" " + cmdLineArgs;
                try
                {
                    string[] argsarray = CommandLineToArgs(entirecmdline);
                    if (argsarray.Length <= 1)
                        m_lblArguments.Text = "";
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 1; i < argsarray.Length; i++) //skips first
                        {
                            sb.Append('%');
                            sb.Append(i);
                            sb.Append(":\t");
                            sb.AppendLine(argsarray[i]);
                        }
                        m_lblArguments.Text = sb.ToString();
                    }
                }
                catch 
                {
                    m_lblArguments.Text = "";
                }
            }
         
            try
            {
                m_lblTimeElapsed.Text = TimeSpan.FromSeconds(0).ToString();
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !m_bClientSide)
                    Program.CommunicationObject.TestScript(data.BatchFile,data.ParsedArguments(data.TestDatFile),new MyScriptTester(this));
                else
                    StartScriptClientSide(data);
                m_started = DateTime.Now;
                m_timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void StartScriptClientSide(iba.Data.BatchFileData data)
        {
            m_scriptProc = new System.Diagnostics.Process();
            m_scriptProc.EnableRaisingEvents = true;
            m_scriptProc.StartInfo.FileName = data.BatchFile;
            m_scriptProc.StartInfo.Arguments = data.ParsedArguments(data.TestDatFile);
            m_scriptProc.Exited += new EventHandler(ScriptFinishedEventHandler);
            m_scriptProc.Start();
        }

        private bool m_bClientSide;
        private DateTime m_started;
        private Process m_scriptProc;

        private void m_btCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !m_bClientSide)
                    Program.CommunicationObject.KillScript();
                else
                {
                    m_scriptProc.Exited -= new EventHandler(ScriptFinishedEventHandler);
                    m_scriptProc.Kill();
                    m_scriptProc.Dispose();
                    m_scriptProc = null;
                }
            }
            catch { }
            Close();
            m_res = ScriptResult.CANCELED;
        }

        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        public static string[] CommandLineToArgs(string commandLine)
        {
            int argc;
            var argv = CommandLineToArgvW(commandLine, out argc);
            if (argv == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception();
            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }
                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }

        private void m_timer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - m_started;
            if (ts > TimeSpan.FromMinutes(15))
                HandleScriptTimeOut();
            m_lblTimeElapsed.Text = String.Format("{0:00}:{1:00}:{2:00}", (int) ts.TotalHours, ts.Minutes, ts.Seconds);
        }

        private void HandleScriptTimeOut()
        {
            m_btCancel_Click(null, null);
            m_res =  ScriptResult.TIMEOUT;
        }

        public void ScriptFinishedEventHandler(object sender, System.EventArgs e)
        {
            if (m_scriptProc != null)
                ScriptEnded(m_scriptProc.ExitCode);
        }

        public enum ScriptResult { SUCCESS, ERROR, TIMEOUT, CANCELED}

        private ScriptResult m_res;
        public ScriptResult Result
        {
            get { return m_res; }
        }

        private int m_exitCode;
        public int ExitCode
        {
            get { return m_exitCode; }
        }

        public void ScriptEnded(int exitCode)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(ScriptEnded), exitCode);
                return;
            }
            m_timer.Stop();
            if (m_scriptProc != null)
            {
                m_scriptProc.Exited -= new EventHandler(ScriptFinishedEventHandler);
                m_scriptProc.Dispose();
                m_scriptProc = null;
            }
            Close();
            m_exitCode = exitCode;
            m_res = (exitCode == 0) ? ScriptResult.SUCCESS : ScriptResult.ERROR;
            //{
            //    MessageBox.Show(string.Format(iba.Properties.Resources.logBatchfileFailed,exitCode), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //else
            //    MessageBox.Show(iba.Properties.Resources.logBatchfileSuccess, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }




    public class MyScriptTester : ScriptTester
    {
        TestScriptDlg m_parent;
        public MyScriptTester(TestScriptDlg parent)
        {
            m_parent = parent;
        }
        public override void NotifyScriptEnd(int errorCode)
        {
            m_parent.ScriptEnded(errorCode);
        }
    }
}
