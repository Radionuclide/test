using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using iba.Utility;
using iba.Processing;

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
            bool bPostpone = TaskManager.Manager.DoPostponeProcessing;
            m_cbPostpone.Checked = bPostpone;
            int minutes = TaskManager.Manager.PostponeMinutes;
            m_nudPostponeTime.Value = (decimal)minutes;
            int iPc = TaskManager.Manager.ProcessPriority;
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
        }

        public void LeaveCleanup()
        {
        }

        public void SaveData()
        {
            if (Program.RunsWithService != Program.ServiceEnum.NOSERVICE)
            {
                ServiceControllerEx service = new ServiceControllerEx("ibaDatCoordinatorService");
                try
                {
                    if (m_cbAutoStart.Checked) 
                        service.ServiceStart = ServiceStart.Automatic;
                    else
                        service.ServiceStart = ServiceStart.Manual;
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
            bool bPostpone = m_cbPostpone.Checked;
            TaskManager.Manager.DoPostponeProcessing = bPostpone;
            int minutes = (int) m_nudPostponeTime.Value;
            TaskManager.Manager.PostponeMinutes = minutes;
            
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
            //set actual priority;
        }

        #endregion
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
