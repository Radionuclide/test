using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Processing;
using iba.Remoting;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace iba.Utility
{
    class SupportFileGenerator
    {
        public SupportFileGenerator(Form parent, string m_filename)
        {
            this.parent = parent;
            this.fileNameClientConfiguration = m_filename;
        }

        public void SaveInformation()
        {
            ZipFile zip = null;
            string host = iba.Properties.Resources.ThisSystem;
            string supportFileName = "";

            try
            {
                if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
                {
                    //Warn that ibaDatCoordinator server is not connected
                    if (MessageBox.Show(parent, String.Format(iba.Properties.Resources.SupportFileServerNotConnected, Program.ServiceHost),
                        Program.MainForm.saveInformationToolStripMenuItem.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                        return;
                }

                supportFileName = GetSupportFileName();
                if (String.IsNullOrEmpty(supportFileName))
                    return;

                destDir = Path.GetDirectoryName(supportFileName);

                string clientFile = null;
                if (Program.RunsWithService != Program.ServiceEnum.NOSERVICE)
                {
                    clientFile = Path.Combine(destDir, "client.zip");
                    GenerateClientZipFile(clientFile);
                }


                string serverFile = null;
                if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
                {
                    serverFile = Path.Combine(destDir, "standalone.zip");
                    GenerateServerZipFile(serverFile, fileNameClientConfiguration);
                }
                else if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && Program.CommunicationObject.TestConnection())
                {
                    serverFile = Path.Combine(destDir, "server.zip");
                    GetServerZipFile(serverFile);
                    host = Program.ServiceHost;
                }

                using (WaitCursor wait = new WaitCursor())
                {
                    zip = ZipFile.Create(supportFileName);
                    zip.BeginUpdate();

                    if (clientFile != null && File.Exists(clientFile))
                        zip.Add(clientFile, Path.GetFileName(clientFile));

                    if (serverFile != null && File.Exists(serverFile))
                        zip.Add(serverFile, Path.GetFileName(serverFile));

                    zip.CommitUpdate();
                    zip.Close();

                    if (clientFile != null && File.Exists(clientFile))
                        File.Delete(clientFile);

                    if (serverFile != null && File.Exists(serverFile))
                        File.Delete(serverFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(parent, String.Format(iba.Properties.Resources.SupportFilesError, supportFileName, ex.Message),
                    Program.MainForm.saveInformationToolStripMenuItem.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (zip != null)
                    zip.Close();
            }

            if (MessageBox.Show(parent, String.Format(Properties.Resources.SupportFilesOk, supportFileName),
                Program.MainForm.saveInformationToolStripMenuItem.Text, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Regex cleanupRegex = new Regex(@"[\\/]+");
                string filePath = cleanupRegex.Replace(supportFileName, @"\");
                Process.Start("explorer.exe", String.Format("/select, \"{0}\"", filePath));
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
                Profiler.ProfileString(true, "Client", "SupportFileDir", ref initialPath, "");
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

        static private void GetDongleInfo(StringBuilder sb)
        {
            CDongleInfo licInfo = CDongleInfo.ReadDongle();
            sb.AppendLine("Dongle serial number: " + licInfo.SerialNr);
            sb.AppendLine("Customer: " + licInfo.Customer);
        }

        static private void GenerateInfo(ZipFile zip, string targetDir, string targetFile)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Append("ibaDatCoordinator Version: ");
                sb.AppendLine(DatCoVersion.GetVersion());
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
                iba.ibaFilesLiteDotNet.IbaFileReader myIbaFile = new iba.ibaFilesLiteDotNet.IbaFileReader();
                sb.AppendLine(myIbaFile.GetType().Assembly.GetName().Version.ToString());
                sb.Append("ibaFiles Version (GetVersion): ");
                sb.AppendLine(myIbaFile.GetVersion());
                myIbaFile.Dispose();
            }
            catch
            { }


            try
            {
                GetDongleInfo(sb);
            }
            catch
            { }

            try
            {
                string fullPath = Path.Combine(targetDir, targetFile);
                SystemInfoCollector.SaveSystemInfo(sb.ToString(), "", fullPath);
                zip.BeginUpdate();
                zip.Add(fullPath, targetFile);
                zip.CommitUpdate();
                File.Delete(fullPath);
            }
            catch
            { }
        }

        private void GenerateClientZipFile(string target)
        {
            using (WaitCursor wait = new WaitCursor())
            {
                ZipFile zip = ZipFile.Create(target);

                GenerateInfo(zip, Path.GetDirectoryName(target), "clientinfo.txt");

                try
                {
                    string outFile = Path.Combine(destDir, "ibaAnalyzer.reg");
                    if (RegistryExporter.ExportIbaAnalyzerKey(outFile))
                    {
                        zip.BeginUpdate();
                        zip.Add(outFile, "ibaAnalyzer.reg");
                        zip.CommitUpdate();

                        File.Delete(outFile);
                    }
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
                                zip.Add(file, @"logging\" + Path.GetFileName(file));
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

                // Create list of installed software
                try
                {
                    string softwareInfoFile = Path.Combine(destDir, "software_info.txt");
                    SystemInfoCollector.SaveInstalledSoftware(softwareInfoFile);
                    
                    zip.BeginUpdate();
                    zip.Add(softwareInfoFile, Path.GetFileName(softwareInfoFile));
                    zip.CommitUpdate();

                    File.Delete(softwareInfoFile);
                }
                catch
                { }

                //Export installation history files
                try
                {
                    string installHistoryX86;
                    string installHistoryX64;
                    SystemInfoCollector.GetInstallHistoryFiles(out installHistoryX86, out installHistoryX64);
                    
                    zip.BeginUpdate();
                    if (installHistoryX86 != null && File.Exists(installHistoryX86))
                        zip.Add(installHistoryX86, "InstallHistory_x86.txt");
                    if (installHistoryX64 != null && File.Exists(installHistoryX64))
                        zip.Add(installHistoryX64, "InstallHistory_x64.txt");
                    zip.CommitUpdate();
                }
                catch
                { }

                zip.Close();
                zip = null;
            }
        }

        private void GetServerZipFile(string target)
        {
            ServerFileInfo inf;
            using (WaitCursor wait = new WaitCursor())
            {
                inf = Program.CommunicationObject.GenerateSupportZipFile();
                if (inf == null)
                {
                    return;
                };
            }
            using (FilesDownloaderForm downloadForm = new FilesDownloaderForm(new ServerFileInfo[] { inf }, target, Program.CommunicationObject.GetServerSideFileHandler(), true))
            {
                downloadForm.ShowDialog(parent);
            }
            Program.CommunicationObject.DeleteFile(inf.LocalFileName);
        }

        //This is running on the server!
        public static void GenerateServerZipFile(string target, string serverConfigFile)
        {
            ZipFile zip = ZipFile.Create(target);
            string destDir = Path.GetDirectoryName(target);

            GenerateInfo(zip, destDir, "serverinfo.txt");

            // Create list of installed software
            try
            {
                string softwareInfoFile = Path.Combine(destDir, "software_info.txt");
                SystemInfoCollector.SaveInstalledSoftware(softwareInfoFile);

                zip.BeginUpdate();
                zip.Add(softwareInfoFile, Path.GetFileName(softwareInfoFile));
                zip.CommitUpdate();

                File.Delete(softwareInfoFile);
            }
            catch
            { }

            //Export installation history files
            try
            {
                string installHistoryX86;
                string installHistoryX64;
                SystemInfoCollector.GetInstallHistoryFiles(out installHistoryX86, out installHistoryX64);

                zip.BeginUpdate();
                if (installHistoryX86 != null && File.Exists(installHistoryX86))
                    zip.Add(installHistoryX86, "InstallHistory_x86.txt");
                if (installHistoryX64 != null && File.Exists(installHistoryX64))
                    zip.Add(installHistoryX64, "InstallHistory_x64.txt");
                zip.CommitUpdate();
            }
            catch
            { }

            try
            {
                //logfiles, server
                string logdir = DataPath.Folder();
                if (Directory.Exists(logdir))
                {
                    string[] logFiles = Directory.GetFiles(logdir, "ibaDatCoordinatorLog*.txt");
                    foreach (string file in logFiles)
                    {
                        try
                        {
                            zip.BeginUpdate();
                            zip.Add(file, @"logging\" + Path.GetFileName(file));
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
                string tempDir = Path.Combine(DataPath.Folder(), "Temp");
                if(!Directory.Exists(tempDir))
                    Directory.CreateDirectory(tempDir);

                //ibaAnalyzer registry settings
                string analyzerRegFile = Path.Combine(tempDir, "ibaAnalyzer.reg");
                RegistryExporter.ExportIbaAnalyzerKey(analyzerRegFile);

                //System registry settings
                RegistryOptimizer.RegExport(tempDir, false);

                //Zip all generated .reg files
                string[] regFiles = Directory.GetFiles(tempDir, "*.reg");
                foreach (string file in regFiles)
                {
                    try
                    {
                        zip.BeginUpdate();
                        zip.Add(file, Path.GetFileName(file));
                        zip.CommitUpdate();

                        File.Delete(file);
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
                if(String.IsNullOrEmpty(serverConfigFile))
                    serverConfigFile = Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), "lastsaved.xml");

                if (File.Exists(serverConfigFile))
                {
                    zip.BeginUpdate();
                    zip.Add(serverConfigFile, Path.GetFileName(serverConfigFile));
                    zip.CommitUpdate();
                }
            }
            catch
            {
            }

            var myList = TaskManager.Manager.AdditionalFileNames();
            foreach (var p in myList)
            {
                try
                {
                    zip.BeginUpdate();
                    zip.Add(p.Key, p.Value);
                    zip.CommitUpdate();
                }
                catch
                //catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }

            //Save dongle info
            string dongleFile = Path.Combine(destDir, "dongle.vwr");
            if (SystemInfoCollector.ExportDongleInfo(dongleFile))
            {
                try
                {
                    zip.BeginUpdate();
                    zip.Add(dongleFile, Path.GetFileName(dongleFile));
                    zip.CommitUpdate();

                    File.Delete(dongleFile);
                }
                catch
                { }
            }

            zip.Close();
            zip = null;
        }

    }
}
