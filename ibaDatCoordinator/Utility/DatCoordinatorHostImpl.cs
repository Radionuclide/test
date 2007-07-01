using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;
using iba.Processing;
using System.IO;

namespace iba.Utility
{
    [Serializable]
    class DatCoordinatorHostImpl : IDatCoHost
    {

        private DatCoordinatorHostImpl() { }

        private static DatCoordinatorHostImpl m_instance=null;

        public string PathToUnc(string fileName, bool convertLocalPaths)
        {
            return Shares.PathToUnc(fileName,convertLocalPaths);
        }

        public bool TryReconnect(string path, string username, string pass)
        {
            return SharesHandler.Handler.TryReconnect(path, username, pass);
        }

        public bool TestPath(string path, string user, string pass, out string errormessage, bool create)
        {
            if (  Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                return TaskManager.Manager.TestPath(path, user, pass, out errormessage, create);
            else
                return SharesHandler.TestPath(path, user, pass, out errormessage, create);
        }


        public void EnableAutoComplete(IntPtr handle, bool directory)
        {
            if (directory)
                WindowsAPI.SHAutoComplete(handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
                SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            else
                WindowsAPI.SHAutoComplete(handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
                SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }


        public static DatCoordinatorHostImpl Host
        {
            get
            {
                if (m_instance == null) m_instance = new DatCoordinatorHostImpl();
                return m_instance;
            }
        }

        public PluginTaskWorkerStatus GetStatusPlugin(Guid guid, int taskindex)
        {
            return TaskManager.Manager.GetStatusPlugin(guid, taskindex);
        }

        public string FindSuitableFileName(string filename)
        {
            string candidate = filename;
            try
            {
                for (int index = 0; File.Exists(candidate); index++)
                {
                    int pos = filename.LastIndexOf('.');
                    string indexstr = index.ToString("d2");
                    candidate = filename.Substring(0, pos) + '_' + indexstr + filename.Substring(pos);
                }
            }
            catch { }
            return candidate;
        }
    }
}
