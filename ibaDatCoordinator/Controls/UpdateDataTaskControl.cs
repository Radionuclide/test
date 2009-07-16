using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using iba;
using iba.Data;
using iba.Utility;
using iba.Processing;


namespace iba.Controls
{
    public partial class UpdateDataTaskControl : UserControl, IPropertyPane
    {
        public UpdateDataTaskControl()
        {
            InitializeComponent();
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        UpdateDataTask m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as UpdateDataTask;
            
            
            m_targetFolderTextBox.Text = m_data.DestinationMap;
            m_rbLimitDirectories.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories;
            m_rbQuota.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_nudDirs.Value = m_data.SubfoldersNumber;
            m_nudQuota.Value = m_data.Quota;

            m_rbNONE.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.NONE;
            m_rbHour.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.HOUR;
            m_rbMonth.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.MONTH;
            m_rbDay.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.DAY;
            m_rbWeek.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.WEEK;
            m_rbOriginal.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.SAME;
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;
            m_cbOverwrite.Checked = m_data.OverwriteFiles;
        }

        public void LeaveCleanup()
        {
            return; 
        }

        public void SaveData()
        {
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            if (m_rbNONE.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.NONE;
            if (m_rbHour.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.HOUR;
            if (m_rbDay.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.DAY;
            if (m_rbWeek.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.WEEK;
            if (m_rbMonth.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.MONTH;
            if (m_rbOriginal.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.SAME;
            m_data.SubfoldersNumber = (uint)m_nudDirs.Value;
            m_data.Quota = (uint)m_nudQuota.Value;
            m_data.OutputLimitChoice = m_rbLimitDirectories.Checked ? TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories : TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_data.OverwriteFiles = m_cbOverwrite.Checked;

            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        #endregion
    }
}
