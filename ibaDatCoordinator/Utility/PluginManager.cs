using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using iba.Plugins;
using System.IO;
using iba.Data;
using iba.Remoting;
using System.Windows.Forms;

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
            if (Program.IsServer || Program.RunsWithService == Program.ServiceEnum.NOSERVICE )
                m_pluginPath = PathUtil.GetAbsolutePath("Plugins");
            else
            {
                string rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                m_cachePath = System.IO.Path.Combine(rootPath, @"iba\ibaDatCoordinator\PluginCache");
                rootPath = System.IO.Path.Combine(rootPath, @"iba\ibaDatCoordinator\Plugins");
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }
                m_pluginPath = rootPath;
            }
            if (Directory.Exists(m_pluginPath))
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        private string m_pluginPath;
        private string m_cachePath;

        public void LoadPlugins(CommunicationObjectWrapper wrapper = null)
        {
            if (!Directory.Exists(m_pluginPath)) return;

            m_plugins.Clear();
            m_pluginInfos.Clear();
            Type requiredInterface = typeof(IDatCoPlugin);
            String[] assemblies = Directory.GetFiles(m_pluginPath, @"*plugin.dll");
            
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
                                    if (!Program.IsServer && wrapper!= null && !wrapper.HasPlugin(pti.Name))
                                        continue;
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
                    if (LogData.Data != null && LogData.Data.Logger != null && LogData.Data.Logger.IsOpen)
                        LogData.Data.Logger.Log(iba.Logging.Level.Exception, message);
                }
            }
        }

        internal bool CopyPluginCache()
        {
            var files = Directory.GetFiles(m_cachePath, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string cpy = file.Replace(m_cachePath, m_pluginPath);
                try
                {
                    string dir = Path.GetDirectoryName(cpy);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    File.Copy(file, cpy, true);
                    
                }
                catch
                {
                    return false;
                }
            }
            try
            {
                Directory.Delete(m_cachePath, true);
            }
            catch
            {
                return false;
            }

            return true;
        }

        internal bool CopyCacheNeeded()
        {
            return (Directory.Exists(m_cachePath));
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
		    //Extract dll filename
		    string dllName = args.Name.Split(',')[0];
            if(dllName.EndsWith("resources"))
                return null;
		    dllName = dllName + ".dll";
    		
		    //Try to load the dll from the plugin path
            string fullPath = Path.Combine(m_pluginPath, dllName);
            if (File.Exists(fullPath))
                return Assembly.LoadFrom(fullPath);
            else
                return null;
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

        public string PluginPath
        {
            get
            {
                return m_pluginPath;
            }
        }

        public string CachePath
        {
            get
            {
                return m_cachePath;
            }
        }

        public ServerFileInfo[] GetPluginFiles() //called from server side
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(m_pluginPath);
                FileInfo[] files = dirInfo.GetFiles("*.*",SearchOption.AllDirectories);
                if (files == null || files.Length==0)
                    return new ServerFileInfo[0];
                ServerFileInfo[] result = new ServerFileInfo[files.Length];
                for (int i = 0; i < files.Length; i++)
                    result[i] = new ServerFileInfo(files[i].FullName, files[i].LastWriteTimeUtc,files[i].Length);
                return result;
            }
            catch
            {
                return new ServerFileInfo[0];
            }
        }

        //called from client side, filter plugins to installed plugins
        public ServerFileInfo[] FilterPlugins(ServerFileInfo[] onServer, string basePath) 
        {
            List<ServerFileInfo> result = new List<ServerFileInfo>();

            string serverPluginPath = basePath;
            string clientPluginPath = m_pluginPath;
            if (!clientPluginPath.EndsWith("\\")) clientPluginPath += "\\";
            foreach (ServerFileInfo serverInfo in onServer)
            {
                string clientFile = serverInfo.LocalFileName.Replace(serverPluginPath, clientPluginPath);
                if (!File.Exists(clientFile))
                    result.Add(serverInfo);
                else
                {
                    FileInfo clientInfo = new FileInfo(clientFile);
                    if (clientInfo.LastWriteTimeUtc != serverInfo.LastWriteTimeUtc || clientInfo.Length != serverInfo.FileSize)
                    {
                        result.Add(serverInfo);
                    }
                }
            }
            return result.ToArray();
        }

        //returns true if restart is required.
        public bool PluginActionsOnConnect(CommunicationObjectWrapper wrapper)
        {
            var list = wrapper.GetPluginFiles();
            string basePath = wrapper.GetPluginPath();
            if (list == null || string.IsNullOrEmpty(basePath)) return false; //error in connecton, no need to restart
            if (!basePath.EndsWith("\\")) basePath += "\\";
            list = FilterPlugins(list, basePath);
            bool failed = false;
            MethodInvoker m = delegate ()
            {
                if (list != null && list.Length > 0)
                {

                    if (!Directory.Exists(m_cachePath))
                        Directory.CreateDirectory(m_cachePath);
                    using (FilesDownloaderForm downloadForm = new FilesDownloaderForm(list, basePath, m_cachePath, wrapper.GetServerSideFileHandler()))
                    {
                        downloadForm.ShowDialog(Program.MainForm);
                    }
                    if (!CopyPluginCache())
                    {
                        MessageBox.Show(iba.Properties.Resources.RestartPluginsRequired, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        failed = true;
                        return;
                    }
                }
                if (!failed)
                {
                    PluginManager.Manager.LoadPlugins(wrapper);
                    Program.MainForm.UpdatePluginGUIElements();
                }
            };
            Program.MainForm.Invoke(m);
            return failed;
        }

        public bool HasPlugin(string name)
        {
            return (m_plugins != null && m_plugins.ContainsKey(name));
        }
    }
}
