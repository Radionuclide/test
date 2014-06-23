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
using iba.Plugins;

namespace iba.Controls
{
    public partial class CommonTaskControl : UserControl, IPropertyPane, ICommonTaskControl
    {
        public CommonTaskControl(Control pluginControl)
        {
            m_plugin = pluginControl as IPluginControl;
            m_regularControl = pluginControl as IPropertyPane;
            InitializeComponent();
            pluginControl.Width = m_pluginPanel.Width;
            //m_pluginPanel.Height = pluginControl.Height;
            //m_pluginPanel.MinimumSize = new Size(m_pluginPanel.MinimumSize.Width, pluginControl.Height);
            pluginControl.Dock = DockStyle.Fill;
            //m_pluginPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.m_pluginPanel.Controls.Add(pluginControl);
            //int newHeight = m_pluginPanel.Bottom + 5;
            //this.Height = newHeight;
            this.MinimumSize = new Size(m_pluginPanel.Left + pluginControl.MinimumSize.Width + 5, m_pluginPanel.Top + pluginControl.MinimumSize.Height + 10);
            //AutoScrollMinSize = new System.Drawing.Size(AutoScrollMinSize.Width, newHeight);
            //AutoScrollMinSize = this.MinimumSize;
            AutoScroll = true;
        }

        private void m_nameTextBox_TextChanged(object sender, EventArgs e)
        {
            TreeNode node = m_manager.LeftTree.SelectedNode;
            if (node != null)
                node.Text = m_nameTextBox.Text;
            m_manager.AdjustRightPaneControlTitle();
        }

        private void m_whenRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            TreeNode taskNode = m_manager.LeftTree.SelectedNode;
            MainForm.strikeOutNodeText(taskNode, m_rbDisabled.Checked);
            if (m_rb1stFailure.Checked)
                taskNode.StateImageIndex = 2;
            else if (m_rbFailure.Checked)
                taskNode.StateImageIndex = 1;
            else if (m_rbSucces.Checked)
                taskNode.StateImageIndex = 0;
            else if (m_rbDisabled.Checked)
            {
                m_data.WhenToExecute = TaskData.WhenToDo.DISABLED;
                taskNode.StateImageIndex = -1;
                if (m_data.ParentConfigurationData.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data.ParentConfigurationData);
            }
            else
                taskNode.StateImageIndex = -1;
        }


        private IPluginControl m_plugin;
        private IPropertyPane m_regularControl;

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private TaskData m_data;

        public TaskData Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as TaskData;
            m_nameTextBox.Text = m_data.Name;

            m_rbAlways.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
            m_rbDisabled.Checked = m_data.WhenToExecute == TaskData.WhenToDo.DISABLED;


            bool firsttask = (m_data.Index == 0 || 
                m_data.ParentConfigurationData.Tasks[m_data.Index-1].WhenToExecute == TaskData.WhenToDo.DISABLED);
            
            if (!firsttask)
            {
                m_rbSucces.Enabled = m_rbFailure.Enabled = m_rb1stFailure.Enabled = true;
                m_rbSucces.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES;
                m_rbFailure.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_FAILURE;
                m_rb1stFailure.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_1st_FAILURE;
            }
            else
            {
                m_rbSucces.Checked = m_rbSucces.Enabled = m_rbFailure.Checked = m_rbFailure.Enabled
                                   = m_rb1stFailure.Checked = m_rb1stFailure.Enabled = false;
            }

            m_whenRadioButton_CheckedChanged(null, null);

            m_rbNotAlways.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
            m_rbNotSuccess.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES;
            m_rbNotFailure.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_FAILURE;
            m_rbNot1stFailure.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_1st_FAILURE;
            m_rbNotDisabled.Checked = m_data.WhenToNotify == TaskData.WhenToDo.DISABLED;

            m_cbResourceCritical.Checked = m_data.ResourceIntensive;

            if (m_plugin != null)
                m_plugin.LoadData((m_data as ICustomTaskData).Plugin, this);
            else if (m_regularControl != null)
                m_regularControl.LoadData(datasource, manager);

        }

        public void SaveData()
        {
            if (m_rbAlways.Checked)
                m_data.WhenToExecute = TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
            else if (m_rbSucces.Checked)
                m_data.WhenToExecute = TaskData.WhenToDo.AFTER_SUCCES;
            else if (m_rbFailure.Checked)
                m_data.WhenToExecute = TaskData.WhenToDo.AFTER_FAILURE;
            else if (m_rb1stFailure.Checked)
                m_data.WhenToExecute = TaskData.WhenToDo.AFTER_1st_FAILURE;
            else
                m_data.WhenToExecute = TaskData.WhenToDo.DISABLED;

            if (m_rbNotAlways.Checked)
                m_data.WhenToNotify = TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
            else if (m_rbNotSuccess.Checked)
                m_data.WhenToNotify = TaskData.WhenToDo.AFTER_SUCCES;
            else if (m_rbNotFailure.Checked)
                m_data.WhenToNotify = TaskData.WhenToDo.AFTER_FAILURE;
            else if (m_rbNot1stFailure.Checked)
                m_data.WhenToNotify = TaskData.WhenToDo.AFTER_1st_FAILURE;
            else
                m_data.WhenToNotify = TaskData.WhenToDo.DISABLED;

            m_data.Name = m_nameTextBox.Text;

            m_data.ResourceIntensive = m_cbResourceCritical.Checked;

            if (m_plugin != null)
                m_plugin.SaveData();
            else if (m_regularControl != null)
                m_regularControl.SaveData();
            
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        public void LeaveCleanup() {
            if (m_plugin != null)
                m_plugin.LeaveCleanup();
            else if (m_regularControl != null)
                m_regularControl.LeaveCleanup();
        }

        #endregion

        #region ICommonTaskControl Members

        public Guid ParentConfigurationGuid()
        {
            return m_data.ParentConfigurationData.Guid;
        }

        public int TaskIndex()
        {
            return m_data.Index;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        #endregion
    }
}
