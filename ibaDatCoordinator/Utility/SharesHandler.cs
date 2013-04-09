using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using iba.Data;
using iba.Plugins;
using iba.Logging;

namespace iba.Utility
{
    public class SharesHandler 
    {
        public static bool TestPath(string path, string userName, string passWord, out string errorMsg, bool createallowed)
        {
	        errorMsg = String.Empty;
            bool doNotDisconnect = false; //set to true to avoid disconnecting the unc path
            if(path.StartsWith(@"\\"))
	        {
		        try
		        {
			        //First add connection
			        String computer = Path.GetPathRoot(path);
			        int error = Shares.ConnectToComputer(computer, userName, passWord);
			        if(error != 0 )
			        {
                        if (!Directory.Exists(computer))
                        {
                            errorMsg = GetErrorMessage(error);
                            return false;
                        } //in case computer exists, the connect was not necessary
                        else
                        {
                            doNotDisconnect = true;
                        }
			        }
		        }
		        catch
		        {
			        return false;
		        }
	        }
	        try
	        {
		        DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    FileInfo fileInfo = new FileInfo(path);
                    if (!fileInfo.Exists) //it's not a file
                    {
                        if (createallowed)
                            dirInfo.Create();
                        else
                        {
                            errorMsg = iba.Properties.Resources.FolderDoesNotExist;
                            return false;
                        }
                    }
                }
		        return true;
	        }
	        catch(Exception ex)
	        {
		        errorMsg = ex.Message;
		        return false;
	        }
	        finally
	        {
		        if(path.StartsWith(@"\\") && !doNotDisconnect)
		        {
			        //Cancel connection
			        String computer = Path.GetPathRoot(path);
			        Shares.DisconnectFromComputer(computer, false);
		        }
	        }
        }

        private SortedDictionary<string, int> m_connectedComputers;
        private SharesHandler()
        {
            m_connectedComputers = new SortedDictionary<string, int>();
        }

        ~SharesHandler() //should not do much, but just in case
        {
            lock (m_connectedComputers)
            {
                foreach (string comp in m_connectedComputers.Keys)
                {
                    Shares.DisconnectFromComputer(comp, true);
                }
                m_connectedComputers.Clear();
            }
        }

        private int m_error;
        private int AddReference(string computer, string user, string pass)
        {
            lock (m_connectedComputers)
            {
                if (m_connectedComputers.ContainsKey(computer))
                    return ++m_connectedComputers[computer];
                else
                {
                    m_error = Shares.ConnectToComputer(computer, user, pass);
                    if (m_error == 0)
                    {
                        m_connectedComputers.Add(computer, 1);
                        return 1;
                    }
                    else if (Directory.Exists(computer)) //already available, do not add
                        return 1;
                    else return 0;
                }
            }
        }

        public bool TryReconnect(string path, string user, string pass)
        {
            return TryReconnectCommon(path, user, pass, false);
        }

        public bool TryReconnectForce(string path, string user, string pass)
        {
            return TryReconnectCommon(path, user, pass, true);
        }

        public bool TryReconnectCommon(string path, string user, string pass, bool force)
        {
            string computer = ComputerName(path);
            lock (m_connectedComputers)
            {
                if (m_connectedComputers.ContainsKey(computer))
                {
                    if (!force && Directory.Exists(path)) return true; //path restored by other means
                    return Shares.ConnectToComputer(computer, user, pass) == 0
                        && Directory.Exists(computer);
                }
                else
                {
                    //return false;
                    return AddReference(computer, user, pass) == 1 && Directory.Exists(computer);
                }
            }
        }

        public void AddReferencesFromConfiguration(ConfigurationData conf, out object error)
        {
            error = null;
            if (!conf.OnetimeJob && IsUNC(conf.DatDirectoryUNC)) //onetimejobs may have multiple folders and are handled differently
            {
                if (AddReference(ComputerName(conf.DatDirectoryUNC), conf.Username, conf.Password) == 0)
                {
                    error = conf;
                    return;
                }
            }
            
            foreach (TaskData dat in conf.Tasks)
            {
                if (!dat.Enabled) continue;
                TaskDataUNC datUNC = dat as TaskDataUNC;
                if (datUNC != null && IsUNC(datUNC.DestinationMapUNC))
                {
                    if (AddReference(ComputerName(datUNC.DestinationMapUNC), datUNC.Username, datUNC.Password) == 0)
                    {
                        foreach (TaskData dat2 in conf.Tasks)
                        {
                            TaskDataUNC datUNC2 = dat2 as TaskDataUNC;
                            if (datUNC == datUNC2) break;
                            if (datUNC2 != null)
                                Release(ComputerName(datUNC2.DestinationMapUNC));
                        }
                        error = datUNC;
                        return;
                    }
                }
                IPluginTaskDataUNC plugin = null;
                if ((dat is CustomTaskData) && ((dat as CustomTaskData).Plugin is IPluginTaskDataUNC))
                    plugin = (dat as CustomTaskData).Plugin as IPluginTaskDataUNC;
                if (plugin != null)
                {
                    foreach (string[] folder in plugin.UNCPaths())
                    {
                        if (AddReference(ComputerName(folder[0]), folder[1], folder[2]) == 0)
                        {
                            foreach (TaskData dat2 in conf.Tasks)
                            {
                                TaskDataUNC datUNC2 = dat2 as TaskDataUNC;
                                if (datUNC == datUNC2) break;
                                if (datUNC2 != null)
                                    Release(ComputerName(datUNC2.DestinationMapUNC));
                            }
                            error = datUNC;
                            return;
                        }
                    }
                }
            }
        }

        public bool IsUNC(string path)
        {
            return path.StartsWith(@"\\");
        }

        public string ComputerName(string path)
        {
            try
            {
                return Path.GetPathRoot(path);
            }
            catch 
            {
                return "";
            }
        }

        public void ReleaseFromConfiguration(ConfigurationData conf)
        {
            if (IsUNC(conf.DatDirectoryUNC))
                Release(ComputerName(conf.DatDirectoryUNC));
            foreach (TaskData dat in conf.Tasks)
            {
                TaskDataUNC datUNC = dat as TaskDataUNC;
                if (datUNC != null && IsUNC(datUNC.DestinationMapUNC))
                    Release(ComputerName(datUNC.DestinationMapUNC));
                IPluginTaskDataUNC plugin = null;
                if ((dat is CustomTaskData) && ((dat as CustomTaskData).Plugin is IPluginTaskDataUNC))
                    plugin = (dat as CustomTaskData).Plugin as IPluginTaskDataUNC;
                if (plugin != null)
                {
                    foreach (string[] folder in plugin.UNCPaths())
                    {
                        if (IsUNC(folder[0]))
                            Release(ComputerName(folder[0]));
                    }
                }
            }
        }

        private void Release(string computer)
        {
            lock (m_connectedComputers)
            {
                if (m_connectedComputers.ContainsKey(computer))
                {
                    int count = --m_connectedComputers[computer];
                    if (count <= 0)
                    {
                        Shares.DisconnectFromComputer(computer, true);
                        m_connectedComputers.Remove(computer);
                    }
                }
            }
        }

        static private SharesHandler m_handler=null;

        static public SharesHandler Handler
        {
            get
            {
                if (m_handler != null) return m_handler;
                else return m_handler = new SharesHandler();
            }
        }
        
        public void ReleaseReferenceDirect(string path)
        {
             if (IsUNC(path))
                 Release(ComputerName(path));
        }

        public void AddReferenceDirect(string path, string username, string pass, out string error)
        {
            error = String.Empty;
            if (IsUNC(path) && AddReference(ComputerName(path), username, pass)==0)
                error = GetErrorMessage(m_error);
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, System.Text.StringBuilder lpBuffer, int nSize, IntPtr arguments);
        
        public static string GetErrorMessage(int error)
        {
            System.Text.StringBuilder lpBuffer = new System.Text.StringBuilder(256);
            while (FormatMessage(0x3200, IntPtr.Zero, error, 0, lpBuffer, lpBuffer.Capacity + 1, IntPtr.Zero) == 0)
            {
                if (lpBuffer.Capacity < 64 * 1024)
                    lpBuffer.Capacity *= 2;
                else
                    return ("Unknown error (0x" + Convert.ToString(error, 0x10) + ")");
            }

            //Remove the new line characters from the end
            int length = lpBuffer.Length;
            while (length > 0)
            {
                char ch = lpBuffer[length - 1];
                if (ch > ' ')
                {
                    break;
                }
                length--;
            }
            return lpBuffer.ToString(0, length);
        }
    }
}
