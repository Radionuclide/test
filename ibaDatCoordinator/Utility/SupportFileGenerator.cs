using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Processing;
using ICSharpCode.SharpZipLib.Zip;

namespace iba.Utility
{
    class SupportFileGenerator
    {
        internal static void SaveInformation(Form parent, string m_filename)
        {
            ZipFile zip = null;
            try
            {
                string destFile;
                using (SaveFileDialog fd = new SaveFileDialog())
                {
                    fd.DefaultExt = "zip";
                    fd.AddExtension = true;
                    fd.Filter = iba.Properties.Resources.ZipFileFilter;
                    fd.OverwritePrompt = true;
                    fd.FileName = "support.zip";
                    if (fd.ShowDialog(parent) != DialogResult.OK)
                        return;

                    destFile = fd.FileName;
                }

                string destDir = Path.GetDirectoryName(destFile);
                using (WaitCursor wait = new WaitCursor())
                {
                    zip = ZipFile.Create(destFile);
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

                    try
                    {
                        //TODO retrieve log files from server instead

                        //logfiles, server
                        string logdir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
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
                    {
                        //localconf //if exists
                        if (!String.IsNullOrEmpty(m_filename) && File.Exists(m_filename))
                        {
                            zip.BeginUpdate();
                            zip.Add(m_filename, @"localconf\" + Path.GetFileName(m_filename));
                            zip.CommitUpdate();
                        }
                    }
                    catch
                    {
                    }

                    if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                        Program.CommunicationObject.SaveConfigurations();

                    try
                    { //serverconf if exists
                        string file = Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), "lastsaved.xml");
                        if (!String.IsNullOrEmpty(file) && File.Exists(file))
                        {
                            zip.BeginUpdate();
                            zip.Add(file, @"serverconf\" + Path.GetFileName(file));
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
                    }
                    catch
                    {
                    }

                    zip.Close();
                    zip = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (zip != null)
                {
                    try
                    {
                        zip.Close();
                    }
                    catch { }
                }
            }

        }

        private static void GetDongleInfo(StringBuilder sb)
        {
            CDongleInfo licInfo = CDongleInfo.ReadDongle();
            sb.AppendLine("Dongle serial number: " + licInfo.SerialNr);
            sb.AppendLine("Customer: " + licInfo.Customer);
        }
    }


}
