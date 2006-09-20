using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace iba.Plugins
{
    /// <summary>
    /// Class containing information about a taskdata plugin
    /// </summary>
    public class PluginTaskInfo
    {
        /// <summary>
        /// Constructs an information object for the plugin
        /// </summary>
        /// <param name="name">The name of the plugin</param>
        /// <param name="description">A short description of the plugin</param>
        /// <param name="icon">An appropriate icon for the plugin</param>
        public PluginTaskInfo(string name, string description, Icon icon)
        {
            m_name = name;
            m_description = description;
            m_icon = icon;
        }

        private string m_name;

        /// <summary>
        /// Name of this plugin
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private string m_description;
        /// <summary>
        /// Description of this plugin
        /// </summary>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        /// <summary>
        /// Icon for this plugin
        /// </summary>
        private Icon m_icon;
        public Icon Icon
        {
            get { return m_icon; }
            set { m_icon = value; }
        }
    }
}

