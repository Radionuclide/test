using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using iba.Plugins;
using System.IO;
using iba.Data;

namespace iba.Utility
{
    public class PluginManager
    {
        private List<PluginTaskInfo> m_pluginInfos;
        public List<PluginTaskInfo> PluginInfos
        {
            get { return m_pluginInfos; }
        }

        private SortedDictionary<string, IDatCoPlugin> m_plugins;

        public SortedDictionary<string, IDatCoPlugin> Plugins
        {
            get { return m_plugins; }
        }

        private PluginManager()
        {
            m_pluginInfos = new List<PluginTaskInfo>();
            m_plugins = new SortedDictionary<string, IDatCoPlugin>();
            m_pluginPath = PathUtil.GetAbsolutePath("Plugins");
        }

        private string m_pluginPath;

        public void LoadPlugins()
        {
            if (!Directory.Exists(m_pluginPath)) return;

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);    
            Type requiredInterface = typeof(IDatCoPlugin);
            String[] assemblies = Directory.GetFiles(m_pluginPath, @"*.dll");
            
            foreach (string assemblyFileName in assemblies)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyFileName);
                    foreach (Type t in assembly.GetTypes())
                    {
                        if (requiredInterface.IsAssignableFrom(t))
                        {
                            IDatCoPlugin plugin = Activator.CreateInstance(t) as IDatCoPlugin;
                            plugin.DatCoordinatorHost = DatCoordinatorHostImpl.Host;
                            if (plugin != null)
                            {
                                foreach (PluginTaskInfo pti in plugin.GetTasks())
                                {
                                    m_pluginInfos.Add(pti);
                                    m_plugins.Add(pti.Name, plugin);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = String.Format(iba.Properties.Resources.logPluginLoadError, assemblyFileName, ex.Message);
                    if (LogData.Data.Logger.IsOpen)
                        LogData.Data.Logger.Log(iba.Logging.Level.Exception, message);
                }
            }
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //extract the simple name from the display name 
            string displayName = args.Name;
            string simpleName = displayName.Split(',')[0] + ".dll";
            //build the filename 
            string fullPath = Path.Combine(m_pluginPath, simpleName);
            //delegate to LoadFrom and return assembly 
            return Assembly.LoadFrom(fullPath); 
        }

        public IPluginTaskData CreateTask(string name, ConfigurationData parentjob)
        {
            if (! m_plugins.ContainsKey(name)) return null;
            else return m_plugins[name].CreateTask(name, parentjob);
        }

        static private PluginManager m_instance = null;
        
        static public PluginManager Manager
        {
            get
            {
                if (m_instance == null)
                    m_instance = new PluginManager();
                return m_instance;
            }
        }

    }
}
