using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using iba.Data;

namespace iba.Utility
{
    public class SharesHandler 
    {
        public static bool TestPath(string path, string userName, string passWord, out string errorMsg, bool createallowed)
        {
	        errorMsg = String.Empty;
            if(path.StartsWith(@"\\"))
	        {
		        try
		        {
			        //First add connection
			        String computer = Path.GetPathRoot(path);
			        int error = Shares.ConnectToComputer(computer, userName, passWord);
			        if(error != 0)
			        {
				        errorMsg = (new System.ComponentModel.Win32Exception(error)).Message;
				        return false;
			        }
		        }
		        catch(Exception ex)
		        {
			        errorMsg= ex.Message;
			        return false;
		        }
	        }
	        try
	        {
		        DirectoryInfo dirInfo = new DirectoryInfo(path);
		        if(!dirInfo.Exists)
                    if (createallowed)
                        dirInfo.Create();
                    else
                    {
                        errorMsg = iba.Properties.Resources.FolderDoesNotExist;
                        return false;
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
		        if(path.StartsWith(@"\\"))
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

        private int AddReference(string computer, string user, string pass)
        {
            lock (m_connectedComputers)
            {
                if (m_connectedComputers.ContainsKey(computer))
                    return ++m_connectedComputers[computer];
                else
                {
                    int error = Shares.ConnectToComputer(computer, user, pass);
                    if (error == 0)
                    {
                        m_connectedComputers.Add(computer, 1);
                        return 1;
                    }
                    else return 0;
                }
            }
        }

        public bool TryReconnect(string path, string user, string pass)
        {
            string computer = ComputerName(path);
            lock (m_connectedComputers)
            {
                if (Directory.Exists(path)) return true; //path restored by other means
                if (m_connectedComputers.ContainsKey(computer))
                {
                    return Shares.ConnectToComputer(computer, user, pass) == 0
                        && Directory.Exists(computer);
                }
                else return false;
            }
        }

        public void AddReferencesFromConfiguration(ConfigurationData conf, out object error)
        {
            error = null;
            if (IsUNC(conf.DatDirectoryUNC))
            {
                if (AddReference(ComputerName(conf.DatDirectoryUNC), conf.Username, conf.Password) == 0)
                {
                    error = conf;
                    return;
                }
            }
            
            foreach (TaskData dat in conf.Tasks)
            {
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
            }
        }

        public bool IsUNC(string path)
        {
            return path.StartsWith(@"\\");
        }

        public string ComputerName(string path)
        {
            return Path.GetPathRoot(path);
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
    }
}
