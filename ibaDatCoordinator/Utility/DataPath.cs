using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Utility
{
    public class DataPath
    {
        public static string Folder(ApplicationState state)
        {
            string rootPath;
            if (state != ApplicationState.SERVICE)
            {
                rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            }
            else
            {

                if (IsAdmin)
                {
                    rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
                }
                else
                {
                    rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                }
            }
            //rootPath = System.IO.Path.Combine(rootPath,@"iba\ibaDatCoordinator");
            return rootPath;
        }


        static bool m_IsAdmin;
        static public bool IsAdmin
        {
            get { return m_IsAdmin; }
        }

        static DataPath()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            m_IsAdmin = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        public static string Folder()
        {
            return Folder(MyState());
        }

        public static ApplicationState MyState()
        {
            if (Program.IsServer)
                return ApplicationState.SERVICE;
            else
            {
                switch (Program.RunsWithService)
                {
                    case Program.ServiceEnum.NOSERVICE:
                        return ApplicationState.CLIENTSTANDALONE;
                    case Program.ServiceEnum.CONNECTED:
                        return ApplicationState.CLIENTCONNECTED;
                    case Program.ServiceEnum.DISCONNECTED:
                        return ApplicationState.CLIENTDISCONNECTED;
                }
            }
            return ApplicationState.SERVICE;
        }
    }

    public enum ApplicationState { CLIENTDISCONNECTED, CLIENTCONNECTED, CLIENTSTANDALONE, SERVICE };
     
}

