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
    public partial class CustomTaskControl : UserControl, IPropertyPane
    {
        public CustomTaskControl(Control pluginControl)
        {
            m_plugin = pluginControl as IPluginControl;
            InitializeComponent();
            //this.tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
            //this.tableLayoutPanel1.ColumnStyles[0].Width = 33;
            //this.tableLayoutPanel1.ColumnStyles[1].SizeType = SizeType.Percent;
            //this.tableLayoutPanel1.ColumnStyles[1].Width = 33;
            //this.tableLayoutPanel1.ColumnStyles[2].SizeType = SizeType.Percent;
            //this.tableLayoutPanel1.ColumnStyles[2].Width = 33;

            //this.tableLayoutPanel6.ColumnStyles[0].SizeType = SizeType.Percent;
            //this.tableLayoutPanel6.ColumnStyles[0].Width = 33;
            //this.tableLayoutPanel6.ColumnStyles[1].SizeType = SizeType.Percent;
            //this.tableLayoutPanel6.ColumnStyles[1].Width = 33;
            //this.tableLayoutPanel6.ColumnStyles[2].SizeType = SizeType.Percent;
            //this.tableLayoutPanel6.ColumnStyles[2].Width = 33;

            pluginControl.Dock = DockStyle.Fill;
            this.m_pluginPanel.Controls.Add(pluginControl);

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
            else
                taskNode.StateImageIndex = -1;
        }


        private IPluginControl m_plugin;

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private CustomTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as CustomTaskData;
            m_nameTextBox.Text = m_data.Name;

            if (m_data.Index == 0)
            {
                m_rbAlways.Checked = true;
                m_rbSucces.Checked = false;
                m_rbFailure.Checked = false;
                m_rb1stFailure.Checked = false;
                m_rbDisabled.Checked = false;
                groupBox4.Enabled = false;
            }
            else
            {
                groupBox4.Enabled = true;
                m_rbAlways.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
                m_rbSucces.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES;
                m_rbFailure.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_FAILURE;
                m_rb1stFailure.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_1st_FAILURE;
                m_rbDisabled.Checked = m_data.WhenToExecute == TaskData.WhenToDo.DISABLED;
            }
            m_rbNotAlways.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
            m_rbNotSuccess.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES;
            m_rbNotFailure.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_FAILURE;
            m_rbNot1stFailure.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_1st_FAILURE;
            m_rbNotDisabled.Checked = m_data.WhenToNotify == TaskData.WhenToDo.DISABLED;

            m_plugin.LoadData(m_data.Plugin,this);
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
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);

            m_plugin.SaveData();
        }
        #endregion
    }
}
