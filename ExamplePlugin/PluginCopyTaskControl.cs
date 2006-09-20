using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using iba.Plugins;

namespace ExamplePlugin
{
    public partial class PluginCopyTaskControl : UserControl, IPluginControl
    {
        private IDatCoHost m_datcoHost;

        public PluginCopyTaskControl(IDatCoHost host)
        {
            m_datcoHost = host;
            InitializeComponent();
            this.tableLayoutPanel3.ColumnStyles[0].SizeType = SizeType.Percent;
            this.tableLayoutPanel3.ColumnStyles[0].Width = 33;
            this.tableLayoutPanel3.ColumnStyles[1].SizeType = SizeType.Percent;
            this.tableLayoutPanel3.ColumnStyles[1].Width = 33;
            this.tableLayoutPanel3.ColumnStyles[2].SizeType = SizeType.Percent;
            this.tableLayoutPanel3.ColumnStyles[2].Width = 33;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            m_datcoHost.EnableAutoComplete(m_targetFolderTextBox.Handle, true);
        }

        #region IPluginControl Members

        PluginCopyTask m_data;
        public void LoadData(object datasource, Control parentcontrol)
        {
            m_data = datasource as PluginCopyTask;
            m_targetFolderTextBox.Text = m_data.DestinationMap;
            m_cbRemoveSource.Checked = m_data.RemoveSource;
            m_folderNumber.Value = m_data.SubfoldersNumber;
            m_rbNONE.Checked = m_data.Subfolder == PluginCopyTask.SubfolderChoiceC.NONE;
            m_rbHour.Checked = m_data.Subfolder == PluginCopyTask.SubfolderChoiceC.HOUR;
            m_rbMonth.Checked = m_data.Subfolder == PluginCopyTask.SubfolderChoiceC.MONTH;
            m_rbDay.Checked = m_data.Subfolder == PluginCopyTask.SubfolderChoiceC.DAY;
            m_rbWeek.Checked = m_data.Subfolder == PluginCopyTask.SubfolderChoiceC.WEEK;
            m_rbOriginal.Checked = m_data.Subfolder == PluginCopyTask.SubfolderChoiceC.SAME;
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;
        }

        public void SaveData()
        {
            m_data.RemoveSource = m_cbRemoveSource.Checked;
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            if (m_rbNONE.Checked) m_data.Subfolder = PluginCopyTask.SubfolderChoiceC.NONE;
            if (m_rbHour.Checked) m_data.Subfolder = PluginCopyTask.SubfolderChoiceC.HOUR;
            if (m_rbDay.Checked) m_data.Subfolder = PluginCopyTask.SubfolderChoiceC.DAY;
            if (m_rbWeek.Checked) m_data.Subfolder = PluginCopyTask.SubfolderChoiceC.WEEK;
            if (m_rbMonth.Checked) m_data.Subfolder = PluginCopyTask.SubfolderChoiceC.MONTH;
            if (m_rbOriginal.Checked) m_data.Subfolder = PluginCopyTask.SubfolderChoiceC.SAME;
            m_data.SubfoldersNumber = (uint)m_folderNumber.Value;

            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();
        }

        #endregion

        private void m_browseFolderButton_Click(object sender, EventArgs e)
        {
            m_folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult result = m_folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_targetFolderTextBox.Text = m_folderBrowserDialog1.SelectedPath;
        }

        private void m_checkPathButton_Click(object sender, EventArgs e)
        {
            SaveData();
            string errormessage = null;
            bool ok = m_datcoHost.TestPath(m_datcoHost.PathToUnc(m_targetFolderTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true);
            if (ok)
            {
                m_checkPathButton.Text = null;
                m_checkPathButton.Image = ExamplePlugin.Properties.Resources.thumbup;
            }
            else
            {
                MessageBox.Show(errormessage, "invalid path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_checkPathButton.Text = null;
                m_checkPathButton.Image = ExamplePlugin.Properties.Resources.thumbdown;
            }
            ((Bitmap)m_checkPathButton.Image).MakeTransparent(Color.Magenta);
        }
    }
}
