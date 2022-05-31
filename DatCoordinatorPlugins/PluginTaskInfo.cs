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
        public PluginTaskInfo(string name, string description, Image icon)
        {
            m_name = name;
            m_description = description;
            m_image = icon;
			m_isOutdated = false;
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
        private Image m_image;
        public Image Icon
        {
            get { return m_image; }
            set { m_image = value; }
        }

		/// <summary>
		/// This plugin cannot be used and must be updated to newer version
		/// </summary>
		private bool m_isOutdated;
		public bool IsOutdated
		{
			get { return m_isOutdated; }
			set { m_isOutdated = value; }
		}
	}

    /// <summary>
    /// Class containing information about a taskdata plugin is the case it is an UNC plugin
    /// </summary>
    public class PluginTaskInfoUNC : PluginTaskInfo
    {
        //currently no new methods
        
        /// <summary>
        /// Constructs an information object for the plugin
        /// </summary>
        /// <param name="name">The name of the plugin</param>
        /// <param name="description">A short description of the plugin</param>
        /// <param name="icon">An appropriate icon for the plugin</param>
        public PluginTaskInfoUNC(string name, string description, Image icon)
        : base(name,description,icon)
        {

        }
    }
}

