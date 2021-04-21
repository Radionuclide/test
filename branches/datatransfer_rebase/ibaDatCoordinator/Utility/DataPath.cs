using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace iba.Utility
{
    class DataPath
    {
        public static string Folder(Program.ApplicationState state)
        {
            string rootPath;
            if (Program.IsServer && IsAdmin)
                rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            else
                rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            rootPath = System.IO.Path.Combine(rootPath, @"iba\ibaDatCoordinator");
            return rootPath;
            //string rootPath;
            //if (state != ApplicationState.SERVICE)
            //{
            //    rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            //}
            //else
            //{
            //    if (IsAdmin)
            //    {
            //        rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            //    }
            //    else
            //    {
            //        rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            //    }
            //}
            ////rootPath = System.IO.Path.Combine(rootPath,@"iba\ibaDatCoordinator");
            //return rootPath;
        }


        static bool m_IsAdmin;
        public static bool IsAdmin
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
            return Folder(Program.MyState());
        }

        public static bool FileExists(string file)
        {
            if (Program.MyState() == Program.ApplicationState.CLIENTCONNECTED)
            {
                return Program.CommunicationObject.FileExists(file);
            }
            else
                return System.IO.File.Exists(file);
        }

        public static string ReadFile(string filename)
        {
            string btext = "";
            if (Program.MyState() == Program.ApplicationState.CLIENTCONNECTED)
            {
                btext = Program.CommunicationObject.ReadFile(filename);
            }
            else
            {
                using (StreamReader bfile = File.OpenText(filename))
                {
                    string str;
                    while ((str = bfile.ReadLine()) != null)
                        btext += str + Environment.NewLine;
                }
            }
            return btext;
        }

        public static void WriteFile(string filename, string text)
        {
            if (Program.MyState() == Program.ApplicationState.CLIENTCONNECTED)
            {
                Program.CommunicationObject.WriteFile(filename,text);
            }
            else
            {
                using (StreamWriter bfile = File.CreateText(filename))
                {
                    bfile.Write(text);
                }
            }
        }

        public static bool IsReadOnly(string filename)
        {
            if (Program.MyState() == Program.ApplicationState.CLIENTCONNECTED)
            {
                return Program.CommunicationObject.IsReadOnly(filename);
            }
            else
                return (new FileInfo(filename)).IsReadOnly;
        }
    }

}

