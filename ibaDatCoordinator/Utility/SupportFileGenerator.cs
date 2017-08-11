using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Processing;
using iba.Remoting;
using ICSharpCode.SharpZipLib.Zip;

namespace iba.Utility
{
    class SupportFileGenerator
    {
        public SupportFileGenerator(Form parent, string m_filename)
        {
            this.parent = parent;
            this.fileNameClientConfiguration = m_filename;
            SaveInformation();
        }

        private void SaveInformation()
        {
            ZipFile zip = null;
            try
            {
                if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
                {
                    //Warn that ibaPDA server is not connected
                    if (MessageBox.Show(parent, String.Format(iba.Properties.Resources.SupportFileServerNotConnected, Program.ServiceHost),
                        Program.MainForm.saveInformationToolStripMenuItem.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                        return;
                }

                string supportFileName = GetSupportFileName();
                if (String.IsNullOrEmpty(supportFileName))
                    return;
                destDir = Path.GetDirectoryName(supportFileName);
                string clientFile = Path.Combine(destDir, "client.zip");
                string serverFile = Path.Combine(destDir, "server.zip");
                GenerateClientZipFile(Program.RunsWithService == Program.ServiceEnum.NOSERVICE ? supportFileName : clientFile);
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && Program.CommunicationObject.TestConnection())
                {
                    GenerateServerZipFile(Path.Combine(destDir, "server.zip"));
                }

                if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE) return;
                else
                    using (WaitCursor wait = new WaitCursor())
                    {
                        zip = ZipFile.Create(supportFileName);
                        zip.BeginUpdate();
                        zip.Add(clientFile);
                        zip.Add(serverFile);
                        zip.CommitUpdate();
                        zip.Close();
                        File.Delete(clientFile);
                        File.Delete(serverFile);
                    }
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(parent, String.Format(iba.Properties.Resources.SupportFileServerNotConnected, Program.ServiceHost),
                    Program.MainForm.saveInformationToolStripMenuItem.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                    return;
            }
            finally
            {
                if (zip != null)
                    zip.Close();
            }
        }

        Form parent;
        string destDir;
        string fileNameClientConfiguration;

        private string GetSupportFileName()
        {
            //Ask user for filename
            using (SaveFileDialog fd = new SaveFileDialog())
            {
                fd.DefaultExt = "zip";
                fd.AddExtension = true;
                fd.Filter = Properties.Resources.ZipFileFilter;
                fd.OverwritePrompt = true;

                DateTime dtNow = DateTime.Now;
                string initialPath = "";
                Profiler.ProfileString(true,"Client", "SupportFileDir", ref initialPath, "");
                if (!String.IsNullOrEmpty(initialPath))
                    fd.InitialDirectory = initialPath;

                fd.FileName = String.Format("ibaDatCoSupport_{0}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}", dtNow.Year, dtNow.Month, dtNow.Day, dtNow.Hour, dtNow.Minute, dtNow.Second);

                if (fd.ShowDialog(parent) == DialogResult.OK)
                {
                    initialPath = Path.GetDirectoryName(fd.FileName);
                    Profiler.ProfileString(false, "Client", "SupportFileDir", ref initialPath, "");
                    return fd.FileName;
                }
                return null;
            }
        }

        private void GetDongleInfo(StringBuilder sb)
        {
            CDongleInfo licInfo = CDongleInfo.ReadDongle();
            sb.AppendLine("Dongle serial number: " + licInfo.SerialNr);
            sb.AppendLine("Customer: " + licInfo.Customer);
        }

        private void GenerateClientZipFile(string target)
        {
            using (WaitCursor wait = new WaitCursor())
            {
                ZipFile zip = ZipFile.Create(target);
                StringBuilder sb = new StringBuilder();

                try
                {
                    sb.Append("ibaDatCoordinator Version: ");
                    sb.AppendLine(parent.GetType().Assembly.GetName().Version.ToString(3));
                }
                catch
                { }

                try
                {
                    sb.Append("ibaAnalyzer Version: ");
                    IbaAnalyzer.IbaAnalysis MyIbaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                    sb.AppendLine(MyIbaAnalyzer.GetVersion().Remove(0, 12));
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(MyIbaAnalyzer);
                }
                catch
                { }

                try
                {
                    sb.Append("ibaFiles Version: ");
                    ibaFilesLiteLib.IbaFileClass myIbaFile = new ibaFilesLiteLib.IbaFileClass();
                    sb.AppendLine(myIbaFile.GetType().Assembly.GetName().Version.ToString());
                    sb.Append("ibaFiles Version (GetVersion): ");
                    sb.AppendLine(myIbaFile.GetVersion());
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(myIbaFile);
                }
                catch
                { }


                try
                {
                    GetDongleInfo(sb);
                }
                catch
                {
                }

                try
                {
                    string clientInfoFile = Path.Combine(destDir, "info.txt");
                    SystemInfoCollector.SaveSystemInfo(sb.ToString(), clientInfoFile);
                    zip.BeginUpdate();
                    zip.Add(clientInfoFile, @"client_info.txt");
                    zip.CommitUpdate();
                    File.Delete(clientInfoFile);
                }
                catch
                {
                }

                try
                {
                    string outFile;
                    if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && Program.CommunicationObject.TestConnection())
                    {
                        outFile = Program.CommunicationObject.GetIbaAnalyzerRegKey();
                    }
                    else
                    {
                        outFile = Path.Combine(destDir, "ibaAnalyzer.reg");
                        Utility.RegistryExporter.ExportIbaAnalyzerKey(outFile);
                    }
                    zip.BeginUpdate();
                    zip.Add(outFile, "ibaAnalyzer.reg");
                    zip.CommitUpdate();
                    if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                        Program.CommunicationObject.DeleteFile(outFile);
                    else
                        File.Delete(outFile);
                }
                catch
                {
                }

                try
                {
                    //logfiles, local
                    string logdir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    logdir = Path.Combine(logdir, @"iba\ibaDatCoordinator");
                    if (Directory.Exists(logdir))
                    {
                        string[] logFiles = Directory.GetFiles(logdir, "ibaDatCoordinatorLog*.txt");
                        foreach (string file in logFiles)
                        {
                            try
                            {
                                zip.BeginUpdate();
                                zip.Add(file, @"logging\local\" + Path.GetFileName(file));
                                zip.CommitUpdate();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                catch
                {
                }


                if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
                {
                    string programdir = Path.GetDirectoryName(typeof(MainForm).Assembly.Location);
                    try
                    { //exception.txt
                        string file = Path.Combine(programdir, "exception.txt");
                        if (File.Exists(file))
                        {
                            zip.BeginUpdate();
                            zip.Add(file, "exception.txt");
                            zip.CommitUpdate();
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        RegistryOptimizer.RegExport(false);
                        string[] regFiles = Directory.GetFiles(programdir, "*.reg");
                        foreach (string file in regFiles)
                        {
                            try
                            {
                                zip.BeginUpdate();
                                zip.Add(file, Path.GetFileName(file));
                                zip.CommitUpdate();
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                try
                {
                    //localconf //if exists
                    if (!String.IsNullOrEmpty(fileNameClientConfiguration) && File.Exists(fileNameClientConfiguration))
                    {
                        zip.BeginUpdate();
                        zip.Add(fileNameClientConfiguration, Path.GetFileName(fileNameClientConfiguration));
                        zip.CommitUpdate();
                    }
                }
                catch
                {
                }
                zip.Close();
                zip = null;
            }
        }

        private void GetServerZipFile(string target)
        {
            if (Program.ServiceIsLocal)
            {
                using (WaitCursor wait = new WaitCursor())
                {
                    GenerateServerZipFile(target);
                }
                return;
            }
            else
            {
                string onServer;
                using (WaitCursor wait = new WaitCursor())
                {
                    onServer = Program.CommunicationObject.GenerateSupportZipFile();
                }
                using (FilesDownloaderForm downloadForm = new FilesDownloaderForm(new ServerFileInfo[] {new ServerFileInfo(onServer) }, target, Program.CommunicationObject.GetServerSideFileHandler(), true))
                {
                    downloadForm.ShowDialog();
                }
                Program.CommunicationObject.DeleteFile(onServer);
            }
        }

        public static void GenerateServerZipFile(string target)
        {
            ZipFile zip = ZipFile.Create(target);
            try
            {
                //TODO retrieve log files from server instead

                //logfiles, server
                string logdir = DataPath.Folder();
                logdir = Path.Combine(logdir, @"iba\ibaDatCoordinator");
                if (Directory.Exists(logdir))
                {
                    string[] logFiles = Directory.GetFiles(logdir, "ibaDatCoordinatorLog*.txt");
                    foreach (string file in logFiles)
                    {
                        try
                        {
                            zip.BeginUpdate();
                            zip.Add(file, @"logging\server\" + Path.GetFileName(file));
                            zip.CommitUpdate();
                        }
                        catch
                        { }
                    }
                }
            }
            catch
            {
            }

            string programdir = Path.GetDirectoryName(typeof(MainForm).Assembly.Location);
            try
            { //exception.txt
                string file = Path.Combine(programdir, "exception.txt");
                if (File.Exists(file))
                {
                    zip.BeginUpdate();
                    zip.Add(file, "exception.txt");
                    zip.CommitUpdate();
                }
            }
            catch
            {
            }

            try
            {
                RegistryOptimizer.RegExport(false);
                string[] regFiles = Directory.GetFiles(programdir, "*.reg");
                foreach (string file in regFiles)
                {
                    try
                    {
                        zip.BeginUpdate();
                        zip.Add(file, Path.GetFileName(file));
                        zip.CommitUpdate();
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            try
            { //serverconf if exists
                string file = Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), "lastsaved.xml");
                if (!String.IsNullOrEmpty(file) && File.Exists(file))
                {
                    zip.BeginUpdate();
                    zip.Add(file,Path.GetFileName(file));
                    zip.CommitUpdate();
                }
            }
            catch
            {
            }

            try
            {
                List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>();
                TaskManager.Manager.AdditionalFileNames(myList);
                foreach(var p in myList)
                {
                    zip.BeginUpdate();
                    zip.Add(p.Key, p.Value);
                    zip.CommitUpdate();
                }
            }
            catch
            {
            }

            zip.Close();
            zip = null;
        }

    }
}
