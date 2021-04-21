using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using iba.Data;
using iba.Plugins;
using iba.Logging;
using System.Threading;

namespace iba.Utility
{
    public class SharesHandler 
    {
        public static bool TestPath(string path, string userName, string passWord, out string errorMsg, bool createallowed, bool bTestWriteAccess)
        {
	        errorMsg = String.Empty;
            bool doNotDisconnect = false; //set to true to avoid disconnecting the unc path
            //LogData.Data.Log(Logging.Level.Debug, "Attempt to connect to: " + path + " username " + userName + " pasword length: " + passWord.Length);
            if(path.StartsWith(@"\\"))
	        {
		        try
		        {
			        //First add connection
			        String computer = Path.GetPathRoot(path);
                    //LogData.Data.Log(Logging.Level.Debug, "computername " + computer );
                    bool ISDTS = false;
                    if(computer.Contains(".") && !ComputerIsIP(computer)) //contains a dot, assume DFS
                    {
                        computer = path;
                        ISDTS = true;
                    }

			        int error = Shares.ConnectToComputer(computer, userName, passWord);
			        if(error != 0 )
			        {
                        //LogData.Data.Log(Logging.Level.Debug, "ConnectToComputer error code returned " + error.ToString());
                        if(ISDTS && error != 1219)
                        {
                            errorMsg = GetErrorMessage(error);
                            if(error == 59)
                                errorMsg += Environment.NewLine + "DFS was detected, please check if the account is not locked out.";
                            return false;
                        }
                        if (!Directory.Exists(computer))
                        {
                            errorMsg = GetErrorMessage(error);
                            return false;
                        } //in case computer exists, the connect was not necessary
                        else
                        {
                            //LogData.Data.Log(Logging.Level.Debug, "connection to: " + path + " already existed");
                            doNotDisconnect = true;
                        }
			        }
		        }
		        catch (Exception ex)
		        {
                    LogData.Data.Log(Logging.Level.Debug, "Exception: " + ex.Message);
			        return false;
		        }
	        }
	        try
	        {
		        DirectoryInfo dirInfo = new DirectoryInfo(path);
                if(!dirInfo.Exists)
                {
                    dirInfo.Refresh();
                    if(!dirInfo.Exists)
                    {
                        FileInfo fileInfo = new FileInfo(path);
                        if(!fileInfo.Exists) //it's not a file
                        {
                            if(createallowed)
                            {
                                dirInfo.Create();
                            }
                            else
                            {
                                errorMsg = iba.Properties.Resources.FolderDoesNotExist;
                                return false;
                            }
                        }
                    }
                }
                if( bTestWriteAccess )
		        {
			        String fileName = Path.Combine(path, Guid.NewGuid().ToString("N"));
			        FileStream stream = File.Open(fileName, FileMode.Create, FileAccess.Write);
			        stream.Close();
			        try
			        {
				        File.Delete(fileName);
			        }
			        catch
			        {
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

        private static bool ComputerIsIP(string computer)
        {
            if(computer.StartsWith(@"\\"))
                computer = computer.Remove(0, 2);
            int index = computer.IndexOf('\\');
            if(index > 0)
                computer = computer.Substring(0, index);
            System.Net.IPAddress dummy;
            return System.Net.IPAddress.TryParse(computer,out dummy);
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
            int errorCode;
            return AddReferenceInternal(computer, user, pass, out errorCode);
        }
        private int AddReferenceInternal(string computer, string user, string pass, out int errorCode)
        {
            errorCode = 0;
            lock (m_connectedComputers)
            {
                if (m_connectedComputers.ContainsKey(computer))
                    return ++m_connectedComputers[computer];
                else
                {
                    switch (TryToConnectToComputer(computer,user,pass,out errorCode))
                    {
                        case ConnectResult.OK:
                            m_connectedComputers.Add(computer, 1);
                            return 1;
                        case ConnectResult.ACCESSIBLE:
                            LogData.Data.Log(Logging.Level.Warning, "AddReference: connection to: " + computer + " already existed" + errorCode.ToString());
                            return 1;
                        case ConnectResult.FAILED:
                            LogData.Data.Log(Logging.Level.Warning, "AddReference: connection to: " + computer + " failed: " + errorCode.ToString());
                            return 0;
                        case ConnectResult.TIMEDOUT:
                            LogData.Data.Log(Logging.Level.Warning, "AddReference: connection to: " + computer + " timed out");
                            return 0;
                    }
                }
                return 0;
            }
        }
        enum ConnectResult { OK, ACCESSIBLE, FAILED, TIMEDOUT }
        ConnectResult TryToConnectToComputer(string computer, string user, string pass, out int errorCode)
        {
            //TODO: do this on a thread
            TimeSpan timeOut = TimeSpan.FromSeconds(60);

            AutoResetEvent waitConnect = new AutoResetEvent(false);
            int errorCodeLocal=0;
            ThreadPool.QueueUserWorkItem(o =>
            {
               errorCodeLocal = Shares.ConnectToComputer(computer, user, pass);
               waitConnect.Set();
            }
            
            );
            if (!waitConnect.WaitOne(timeOut))
            {
                errorCode = errorCodeLocal;
                return ConnectResult.TIMEDOUT;
            }
            errorCode = errorCodeLocal;
            if (errorCode == 0)
            {
                waitConnect.Dispose();
                return ConnectResult.OK;
            }
            else
            {
                bool exists = false;
                ThreadPool.QueueUserWorkItem(o =>
                {
                    exists = Directory.Exists(computer);
                    waitConnect.Set();
                });
                if (!waitConnect.WaitOne(timeOut))
                {
                    errorCode = errorCodeLocal;
                    return ConnectResult.TIMEDOUT;
                }
                else if (exists) //already available, do not add
                {
                    waitConnect.Dispose();
                    return ConnectResult.ACCESSIBLE;
                }
            }
            return ConnectResult.FAILED;
        }

        public bool TryReconnect(string path, string user, string pass, bool force = false)
        {
            string computer = ComputerName(path);
            lock (m_connectedComputers)
            {
                if (m_connectedComputers.ContainsKey(computer))
                {
                    TimeSpan timeOut = TimeSpan.FromSeconds(60);
                    AutoResetEvent waitConnect = new AutoResetEvent(false);
                    if (!force)
                    {
                        bool exists = false;
                        ThreadPool.QueueUserWorkItem(o =>
                        {
                            exists = Directory.Exists(computer);
                            waitConnect.Set();
                        });
                        if (!waitConnect.WaitOne(timeOut))
                        {
                            return false;
                        }
                        if (exists)
                        {
                            waitConnect.Dispose();
                            return true; //path restored by other means
                        }
                    }
                    bool connected = false;
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        connected =
                        Shares.ConnectToComputer(computer, user, pass) == 0
                        && Directory.Exists(computer);
                    });
                    if (!waitConnect.WaitOne(timeOut))
                    {
                        return false;
                    }
                    waitConnect.Dispose();
                    return connected;
                }
                else
                {
                    return AddReference(computer, user, pass) == 1 && Directory.Exists(computer);
                }
            }
        }

        public void AddReferencesFromConfiguration(ConfigurationData conf, out object error)
        {
            error = null;
            if (!conf.OnetimeJob && IsUNC(conf.DatDirectoryUNC)) //one time jobs may have multiple folders and are handled differently
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
                TaskWithTargetDirData datUNC = dat as TaskWithTargetDirData;
                if (datUNC != null && IsUNC(datUNC.DestinationMapUNC))
                {
                    if (AddReference(ComputerName(datUNC.DestinationMapUNC), datUNC.Username, datUNC.Password) == 0)
                    {
                        foreach (TaskData dat2 in conf.Tasks)
                        {
                            TaskWithTargetDirData datUNC2 = dat2 as TaskWithTargetDirData;
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

        public bool TestTargetDirAvailable(TaskWithTargetDirData tData)
        {
            if (!tData.Enabled) return true;
            if (!IsUNC(tData.DestinationMapUNC)) return true; //no network connection tests, old code to test di
            return TryReconnect(tData.DestinationMapUNC, tData.Username, tData.Password);
        }


        public bool IsUNC(string path)
        {
            return path.StartsWith(@"\\");
        }

        public string ComputerName(string path)
        {
            try
            {
                string computer = Path.GetPathRoot(path);
                return (computer.Contains(".")&&!ComputerIsIP(computer))?path:computer;
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

        public void AddReferenceDirect(string path, string username, string pass, out string errorStr)
        {
            errorStr = String.Empty;
            int error;
            if (IsUNC(path) && AddReferenceInternal(ComputerName(path), username, pass, out error)==0)
                errorStr = GetErrorMessage(error);
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
