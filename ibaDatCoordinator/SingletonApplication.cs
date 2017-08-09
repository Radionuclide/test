using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using iba.Utility;

namespace iba
{
	#region Singleton class
	public class SingletonApp 
	{
		static System.Threading.Mutex m_Mutex;

		public static bool CheckIfRunning(string closeFormCaption)
		{
            bool standAlone = Program.RunsWithService == Program.ServiceEnum.NOSERVICE;
            if (standAlone)
            {
                IntPtr handle = FindWindow(null, closeFormCaption);
                if (handle.ToInt32() != 0)
                {
                    iba.Utility.WindowsAPI.SendMessage(handle, 0x8141, 0, 0);
                    return true;
                }
                return false;
            }

			if(IsFirstInstance(closeFormCaption))
			{
				Application.ApplicationExit += new EventHandler(OnExit);
				return false;
			}
			else
			{
				//Activate app that is already running
                IntPtr handle = FindWindow(null, closeFormCaption);
                if (handle.ToInt32() != 0)
                    iba.Utility.WindowsAPI.SendMessage(handle, 0x8141, 0, 0);
                else
                    MessageBox.Show(iba.Properties.Resources.alreadyRunningOtherUser, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return true;
			}
		}

		static bool IsFirstInstance(string closeFormCaption)
		{
            try
            {
                string mutexName = closeFormCaption + " Mutex";
                m_Mutex = new System.Threading.Mutex(false, mutexName);
                bool owned = false;
                owned = m_Mutex.WaitOne(TimeSpan.Zero, false);
                return owned;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception)
            {
                return true;
            }
		}

		static void OnExit(object sender,EventArgs args)
		{
            if (m_Mutex != null)
            {
                m_Mutex.ReleaseMutex();
                m_Mutex.Close();
            }
		}

		[DllImport("user32")] static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
	}
	#endregion

    #region Helper to quit application from installer
    class QuitForm : NativeWindow
    {
        const int WM_GRACEFUL_QUIT = 0x8140;
        const int WM_GRACEFUL_ACTIVATE = 0x8141;
        const int WM_START_SERVICE = 0x8142;

        public QuitForm(IExternalCommand form, string caption, bool bMonitorRegistry)
        {
            this.form = form;
            this.caption = caption;
            if (bMonitorRegistry)
                InitializeRegMon();
        }

        #region Registry monitor

        iba.Utility.RegistryMonitor regMon;
        string uninstallRegKeyName;

        private void InitializeRegMon()
        {
            try
            {
                //if(Environment.Is64BitOperatingSystem)
                //    uninstallRegKeyName = @"SOFTWARE\Wow6432Node\iba\ibaPDA\Uninstall";
                //else
                uninstallRegKeyName = @"SOFTWARE\iba\ibaDatCoordinator\Uninstall";

                regMon = new RegistryMonitor(Microsoft.Win32.RegistryHive.LocalMachine, uninstallRegKeyName);
                regMon.Error += new System.IO.ErrorEventHandler(regMon_Error);
                regMon.RegChanged += new EventHandler(regMon_RegChanged);
                regMon.RegChangeNotifyFilter = RegChangeNotifyFilter.Value;
                regMon.Start();
            }
            catch (Exception)
            {
                //ibaLogger.LogFormat(Level.Exception, "Error starting uninstall registry monitor: {0}", ex);
                CloseRegMon();
            }
        }

        private void CloseRegMon()
        {
            try
            {
                if (regMon == null)
                    return;

                if (regMon.IsMonitoring)
                    regMon.Stop();
                regMon.Dispose();
            }
            catch (Exception)
            {
            }
            regMon = null;
        }

        void regMon_RegChanged(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(uninstallRegKeyName);
                if (key == null)
                    return;

                int isBusy = (int)key.GetValue("isBusy", 0);
                if (isBusy == 1)
                {
                    //ibaLogger.Log(Level.Info, "Uninstall isBusy is set to 1 -> closing server status program");
                    form.OnExternalClose();
                }
            }
            catch (Exception)
            {
            }
        }

        void regMon_Error(object sender, System.IO.ErrorEventArgs e)
        {
            //ibaLogger.LogFormat(Level.Exception, "Received error from uninstall registry monitor: {0}", e.GetException());
        }

        #endregion


        IExternalCommand form;
        string caption;

        public override void CreateHandle(CreateParams cp)
        {
            cp.Caption = caption;
            cp.Style = 0;
            base.CreateHandle(cp);
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GRACEFUL_QUIT)
            {
                form.OnExternalClose();
            }
            else if (m.Msg == WM_GRACEFUL_ACTIVATE)
            {
                form.OnExternalActivate();
            }
            else if (m.Msg == WM_START_SERVICE)
            {
                form.OnStartService();
            }

            base.WndProc(ref m);
        }
    }
    #endregion
}
