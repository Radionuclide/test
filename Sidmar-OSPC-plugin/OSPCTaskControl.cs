using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iba.Plugins;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace AM_OSPC_plugin
{
    public partial class OSPCTaskControl : UserControl, IPluginControl
    {
        private IDatCoHost m_datcoHost;
        public OSPCTaskControl(IDatCoHost datcoHost)
        {
            m_datcoHost = datcoHost;
            InitializeComponent();
            ((Bitmap)m_testButton.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_executeIBAAButton.Image).MakeTransparent(Color.Magenta);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_datFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPluginControl Members

        public void LoadData(object datasource, ICommonTaskControl parentcontrol)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                object o = key.GetValue("");
                ibaAnalyzerExe = Path.GetFullPath(o.ToString());
            }
            catch
            {
                ibaAnalyzerExe = Properties.Resources.noIbaAnalyser;
            }
        }
        private string ibaAnalyzerExe;
        
        public void SaveData()
        {
            throw new NotImplementedException();
        }

        public void LeaveCleanup()
        {
            //nothing to do
            //throw new NotImplementedException();
        }

        #endregion

        private void m_browsePDOFileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog.CheckFileExists = true;
            m_openFileDialog.Filter = "ibaAnalyzer PDO files (*.pdo)|*.pdo";
            DialogResult result = m_openFileDialog.ShowDialog();
            if(result == DialogResult.OK)
                m_pdoFileTextBox.Text = m_openFileDialog.FileName;
        }

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
        {
            try
            {
                using(Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = ibaAnalyzerExe;
                    bool d = !string.IsNullOrEmpty(m_datFileTextBox.Text);
                    bool p = !string.IsNullOrEmpty(m_pdoFileTextBox.Text);
                    if (p&d)
                        ibaProc.StartInfo.Arguments = "\" " + m_datFileTextBox.Text + "\" \"" + m_pdoFileTextBox.Text + "\"";
                    else if (p)
                        ibaProc.StartInfo.Arguments = "\" " + m_pdoFileTextBox.Text + "\"";
                    else if (d)
                        ibaProc.StartInfo.Arguments = "\" " + m_datFileTextBox.Text + "\"";
                    ibaProc.Start();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_browseDatFileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog.CheckFileExists = true;
            m_openFileDialog.FileName = "";
            m_openFileDialog.Filter = "All files (*.*)|*.*";
            if(m_openFileDialog.ShowDialog(this) == DialogResult.OK)
                m_datFileTextBox.Text = m_openFileDialog.FileName;
        }

        private void m_testButton_Click(object sender, EventArgs e)
        {

        }
    }

    /// </summary>
    internal class WindowsAPI
    {
        #region Shlwapi.dll functions
        [DllImport("Shlwapi.dll")]
        public static extern int SHAutoComplete(IntPtr handle, SHAutoCompleteFlags flags);
        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    }



    #region SHAutoCompleteFlags
    [Flags]
    internal enum SHAutoCompleteFlags : uint
    {
        SHACF_DEFAULT = 0x00000000,  // Currently (SHACF_FILESYSTEM | SHACF_URLALL)
        SHACF_FILESYSTEM = 0x00000001,  // This includes the File System as well as the rest of the shell (Desktop\My Computer\Control Panel\)
        SHACF_URLHISTORY = 0x00000002,  // URLs in the User's History
        SHACF_URLMRU = 0x00000004,  // URLs in the User's Recently Used list.
        SHACF_USETAB = 0x00000008,  // Use the tab to move thru the autocomplete possibilities instead of to the next dialog/window control.
        SHACF_FILESYS_ONLY = 0x00000010,  // This includes the File System
        SHACF_FILESYS_DIRS = 0x00000020,  // Same as SHACF_FILESYS_ONLY except it only includes directories, UNC servers, and UNC server shares.
        SHACF_URLALL = (SHACF_URLHISTORY | SHACF_URLMRU),
        SHACF_AUTOSUGGEST_FORCE_ON = 0x10000000,  // Ignore the registry default and force the feature on.
        SHACF_AUTOSUGGEST_FORCE_OFF = 0x20000000,  // Ignore the registry default and force the feature off.
        SHACF_AUTOAPPEND_FORCE_ON = 0x40000000,  // Ignore the registry default and force the feature on. (Also know as AutoComplete)
        SHACF_AUTOAPPEND_FORCE_OFF = 0x80000000   // Ignore the registry default and force the feature off. (Also know as AutoComplete)
    }
    #endregion
}
