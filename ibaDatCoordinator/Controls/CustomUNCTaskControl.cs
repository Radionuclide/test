using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using iba.Plugins;
using iba.Data;

namespace iba.Controls
{
    public partial class CustomUNCTaskControl : UserControl, IPropertyPane, ICommonTaskControl
    {
        public CustomUNCTaskControl(IPluginControlUNC plugin)
        {
            m_plugin = plugin;
            Control pluginControl = plugin as Control;
            InitializeComponent();
            pluginControl.Width = m_pluginPanel.Width;
            m_pluginPanel.Height = pluginControl.Height;
            m_pluginPanel.MinimumSize = new Size(m_pluginPanel.MinimumSize.Width, pluginControl.Height);
            pluginControl.Dock = DockStyle.Fill;
            this.m_pluginPanel.Controls.Add(pluginControl);
            m_gbTarget.Top = m_pluginPanel.Bottom + 5;

            if (plugin.FixedHeight)
            {
                m_pluginPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                m_gbTarget.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            }
            else
            {
                m_pluginPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                m_gbTarget.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }

            m_uncControl = new UNCTaskControl();
            panelOut.Controls.Add(m_uncControl);
            m_uncControl.Dock = DockStyle.Fill;

            int newHeight = m_gbTarget.Bottom + 5;
            this.Height = newHeight;
            //autoscroll seems problematic, add it here with AutoScrollMinSize
            AutoScrollMinSize = new System.Drawing.Size(AutoScrollMinSize.Width, newHeight);
            AutoScroll = true;
        }

        //gui elements
        private IPluginControlUNC m_plugin;
        private UNCTaskControl m_uncControl;

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private CustomTaskDataUNC m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as CustomTaskDataUNC;
            m_uncControl.SetData(m_data);
            m_plugin.LoadData((m_data.Plugin as ICustomTaskData), this);
        }

        public void LeaveCleanup()
        {
            if (m_plugin != null)
                m_plugin.LeaveCleanup();
        }

        public void SaveData()
        {
            m_plugin.SaveData();
            m_uncControl.SaveData();
            m_data.UpdateUNC();
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
        #endregion
    }
}
