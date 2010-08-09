using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace iba
{
	#region Singleton class
	public class SingletonApp 
	{
		static System.Threading.Mutex m_Mutex;

		public static bool CheckIfRunning()
		{
			if(IsFirstInstance())
			{
				Application.ApplicationExit += new EventHandler(OnExit);
				return false;
			}
			else
			{
				//Activate app that is already running
                IntPtr handle = FindWindow(null, "ibaDatCoordinatorStatusCloseForm");
                if (handle.ToInt32() != 0)
                    iba.Utility.WindowsAPI.SendMessage(handle, 0x8141, 0, 0);
                else
                    MessageBox.Show(iba.Properties.Resources.AllReadyRunningOtherUser, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return true;
			}
		}

		static bool IsFirstInstance()
		{
            try
            {
                m_Mutex = new System.Threading.Mutex(false, "Global\\ibaDatCoordinatorServerStatus Mutex");
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
			m_Mutex.ReleaseMutex();
			m_Mutex.Close();
		}

		[DllImport("user32")] static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
	}
	#endregion

    #region Helper to quit application from installer
    internal class QuitForm : NativeWindow
    {
        const int WM_GRACEFUL_QUIT = 0x8140;
        const int WM_GRACEFUL_ACTIVATE = 0x8141;
        const int WM_START_SERVICE = 0x8142;

        public QuitForm(IExternalCommand form)
        {
            this.form = form;
        }

        IExternalCommand form;

        public override void CreateHandle(CreateParams cp)
        {
            cp.Caption = "ibaDatCoordinatorStatusCloseForm";
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
