using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using iba.Data;
using iba.Logging;
using iba.Utility;
using IbaAnalyzer;
using Microsoft.Win32;

namespace iba.Remoting
{
    public class ibaAnalyzerExt : MarshalByRefObject, ISponsor, IDisposable, IbaAnalyzer.IbaAnalyzer
    {
        private LifePulse pulse;
        public ibaAnalyzerExt(IbaAnalyzer.IbaAnalyzer analyzer, bool noninteractieve, LifePulse pulse = null)
        {
            this.pulse = pulse;
            m_bAnalyzerOwned = analyzer == null;
            if (m_bAnalyzerOwned)
            {
                if (noninteractieve)
                    this.analyzer = new IbaAnalyzer.IbaAnalysisNonInteractive();
                else
                    this.analyzer = new IbaAnalyzer.IbaAnalysis(); //tree images work with these
            }
            else
                this.analyzer = analyzer;
        }

        bool m_bAnalyzerOwned;
        IbaAnalyzer.IbaAnalyzer analyzer;
        public string GetVersion()
        {
            return analyzer.GetVersion();
        }

        public void OpenAnalysis(string filename)
        {
            analyzer.OpenAnalysis(filename);
        }

        public void CloseAnalysis()
        {
            analyzer.CloseAnalysis();
        }

        public string GetLastError()
        {
            return analyzer.GetLastError();
        }

        public void RunInteractive()
        {
            analyzer.RunInteractive();
        }

        public void GetStartTime(ref DateTime oaStartTime, ref int microSecPart)
        {
            analyzer.GetStartTime(ref oaStartTime, ref microSecPart);
        }

        public void SaveAnalysis(string filename)
        {
            analyzer.SaveAnalysis(filename);
        }

        List<string> SplitHdqFile(string filename)
        {
            List<string> newFiles = new List<string>();
            IniParser parser = new IniParser(filename);
            if (!parser.Read() || !parser.Sections.ContainsKey("HDQ file1"))
            {
                newFiles.Add(filename);
                return newFiles;
            }

            string tempDir = Path.Combine(Path.GetDirectoryName(filename), "temp");
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);
            string baseFileName = Path.Combine(tempDir, Path.GetFileNameWithoutExtension(filename));

            //We have to split the file in multiple files
            foreach (var section in parser.Sections)
            {
                if (!section.Key.StartsWith("HDQ file"))
                    continue;

                //Create new file with single HDQ file section
                string newFileName = baseFileName + "_" + newFiles.Count.ToString() + ".hdq";
                IniParser writer = new IniParser(newFileName);
                writer.Sections.Add("HDQ file", section.Value);
                writer.Write();

                newFiles.Add(newFileName);
            }

            return newFiles;
        }


        public bool CheckVersion(string v)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
            object o = key.GetValue("");
            string ibaAnalyzerExe = Path.GetFullPath(o.ToString());
            return VersionCheck.CheckVersion(ibaAnalyzerExe, v);
        }

        public static bool CheckVersion(IbaAnalyzer.IbaAnalyzer ana, string v)
        {
            return ((ibaAnalyzerExt)ana).CheckVersion(v);
        }

        class TempHdqFile
        {
            public string FileName;
            public int OriginalIndex; //Original dat file index in ibaAnalyzer
            public int ActualIndex;   //New dat file index in ibaAnalyzer
        }
        List<TempHdqFile> tempFiles;

        public void OpenDataFile(int index, string filename)
        {
            if (filename.EndsWith(".hdq"))
            {
                //Check if the file contains multiple sections that need to be split into multiple hdq files
                var newFiles = SplitHdqFile(filename);
                if (newFiles.Count > 1)
                {
                    //There are multiple files generated -> let's open them all in ibaAnalyzer
                    if (tempFiles == null)
                        tempFiles = new List<TempHdqFile>();

                    int newIndex = index;
                    foreach (var newFileName in newFiles)
                    {
                        TempHdqFile tempFile = new TempHdqFile
                        {
                            FileName = newFileName,
                            OriginalIndex = index,
                            ActualIndex = newIndex
                        };
                        tempFiles.Add(tempFile);

                        //Open the new file in Analyzer
                        analyzer.OpenDataFile(newIndex, newFileName);
                        newIndex++;
                    }

                    return;
                }
            }

            analyzer.OpenDataFile(index, filename);
        }

        public void AppendDataFile(int index, string filename)
        {
            if (filename.EndsWith(".hdq"))
            {
                //Check if the file contains multiple sections that need to be split into multiple hdq files
                var newFiles = SplitHdqFile(filename);
                if (newFiles.Count > 1)
                {
                    //There are multiple files generated -> let's open them all in ibaAnalyzer
                    if (tempFiles == null)
                        tempFiles = new List<TempHdqFile>();

                    foreach (var newFileName in newFiles)
                    {
                        TempHdqFile tempFile = new TempHdqFile
                        {
                            FileName = newFileName,
                            OriginalIndex = index,
                            ActualIndex = index
                        };
                        tempFiles.Add(tempFile);

                        //Open the new file in Analyzer
                        analyzer.AppendDataFile(index, newFileName);
                    }

                    return;
                }
            }

            analyzer.AppendDataFile(index, filename);
        }

        public void CloseDataFile(int index)
        {
            if (tempFiles != null)
            {
                for (int i = tempFiles.Count - 1; i >= 0; i--)
                {
                    var tempFile = tempFiles[i];
                    if (tempFile.OriginalIndex == index)
                    {
                        analyzer.CloseDataFile(tempFile.ActualIndex);
                        try
                        {
                            File.Delete(tempFile.FileName);
                        }
                        catch (System.Exception ex)
                        {
                            LogData.Data.Log(Level.Debug, String.Format("Failed to delete temporary hdq file {0}: {1}", tempFile.FileName, ex.Message));
                        }
                        tempFiles.RemoveAt(i);
                    }
                }

                if (tempFiles.Count == 0)
                    tempFiles = null;
            }

            analyzer.CloseDataFile(index);
        }

        public void CloseDataFiles()
        {
            analyzer.CloseDataFiles();

            if (tempFiles != null)
            {
                foreach (var tempFile in tempFiles)
                {
                    try
                    {
                        File.Delete(tempFile.FileName);
                    }
                    catch (System.Exception ex)
                    {
                        LogData.Data.Log(Level.Debug, String.Format("Failed to delete temporary hdq file {0}: {1}", tempFile.FileName, ex.Message));
                    }
                }
                tempFiles = null;
            }
        }

        public void AddToFileGroup(string filename, int select)
        {
            analyzer.AddToFileGroup(filename, select);
        }

        public void ClearFileGroup()
        {
            analyzer.ClearFileGroup();
        }

        public int RunSqlQuery(string sql, string sync)
        {
            return analyzer.RunSqlQuery(sql, sync);
        }

        public int RunSqlQueryFromFile(string filename, string sync)
        {
            return analyzer.RunSqlQueryFromFile(filename, sync);
        }

        public string GetQueryResult(int resultIndex)
        {
            return analyzer.GetQueryResult(resultIndex);
        }

        public void Extract(int fileExtract, string output)
        {
            analyzer.Extract(fileExtract, output);
        }

        public void Report(string output)
        {
            analyzer.Report(output);
        }

        public void Print()
        {
            analyzer.Print();
        }

        public float Evaluate(string expression, int XType)
        {
            return analyzer.Evaluate(expression, XType);
        }

        public int GetProcessID()
        {
            return analyzer.GetProcessID();
        }

        public void AddLogical(string name, string expression, int XType)
        {
            analyzer.AddLogical(name, expression, XType);
        }

        public bool RemoveLogical(string name)
        {
            return analyzer.RemoveLogical(name);
        }

        public bool ReplaceExpression(string name, string expr)
        {
            return analyzer.ReplaceExpression(name, expr);
        }

        public static IbaAnalyzer.IbaAnalyzer Create(bool noninteractive) //factory method
        {
            if (!Program.IsServer && Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
                return new ibaAnalylerClientWrapper(Program.CommunicationObject.GetRemoteIbaAnalyzer(noninteractive));
            else
                return new ibaAnalyzerExt(null, noninteractive);
        }

        public double EvaluateDouble(string expression, int XType)
        {
            return analyzer.EvaluateDouble(expression, XType);
        }

        public int EvaluateDataType(string expression, int XType)
        {
            try
            {
                return analyzer.EvaluateDataType(expression, XType);
            }
            catch //might be older version than analyzer V7.3.0;
            {
                return 0; //analog
            }
        }

        public int RunSqlTrendQuery(string sql, string sync, int append, int microSecond, int overview)
        {
            return analyzer.RunSqlTrendQuery(sql, sync, append, microSecond, overview);
        }

        public int RunSqlTrendQueryFromFile(string filename, string sync, int append, int microSecond, int overview)
        {
            return analyzer.RunSqlTrendQueryFromFile(filename, sync, append, microSecond, overview);
        }

        public void SaveGraphImage(int graphIndex, string filename, int width, int height)
        {
            analyzer.SaveGraphImage(graphIndex, filename, width, height);
        }

        public int GetGraphCount()
        {
            return analyzer.GetGraphCount();
        }

        public int GetZoomAreaBegin(int XType)
        {
            return analyzer.GetZoomAreaBegin(XType);
        }

        public int GetZoomAreaEnd(int XType)
        {
            return analyzer.GetZoomAreaEnd(XType);
        }

        public string GetFileExtractParameters()
        {
            return analyzer.GetFileExtractParameters();
        }


        public void SetFilePassword(string filename, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(password))
                    analyzer.SetFilePassword(filename, password);
            }
            catch //might be other version than analyzer V7;
            {

            }
        }

        public void SetHDCredentials(string username, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(username))
                    analyzer.SetHDCredentials(username, password);
            }
            catch //might be other version than analyzer V7;
            {

            }

        }

        public void SetHDCredentialsEx(int useCurrentWindowsUser, string username, string password)
        {
            try
            {
                analyzer.SetHDCredentialsEx(useCurrentWindowsUser, username, password);
            }
            catch //might be older version than analyzer V7.3.0;
            {

            }
        }

        public uint GetMarkerColor(int index)
        {
            return analyzer.GetMarkerColor(index);
        }

        public void UpdateOverlay()
        {
            analyzer.UpdateOverlay();
        }

        public void EvaluateToStringArray(string expression, int XType, out object pTimeStamps, out object pStrings)
        {
            analyzer.EvaluateToStringArray(expression, XType, out pTimeStamps, out pStrings);
        }

        public void EvaluateToNEArray(string expression, int XType, out object pTimeStamps, out object pValues)
        {
            analyzer.EvaluateToNEArray(expression, XType, out pTimeStamps, out pValues);
        }

        public dynamic GetSignalTree(int filter)
        {
            return new AnalyzerSignalTreeExt(this.analyzer.GetSignalTree(filter));
        }

        public void SignalTreeImageData(int index, out object pData)
        {
            analyzer.SignalTreeImageData(index, out pData);
        }

        public bool IsVisible
        {
            get
            {
                return analyzer.IsVisible;
            }

            set
            {
                analyzer.IsVisible = value;
            }
        }

        public int SignalTreeImageCount
        {
            get
            {
                return analyzer.SignalTreeImageCount;
            }

            set
            {
                analyzer.SignalTreeImageCount = value;
            }
        }

        public void EvaluateToArray(string expression, int XType, out double pTimebase, out double xoffset, out object pData)
        {
            analyzer.EvaluateToArray(expression, XType, out pTimebase, out xoffset, out pData);
        }

        #region IDisposable Support

        ~ibaAnalyzerExt() => Dispose(false);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            pulse = null;
            if (analyzer == null) return; //redundant call;
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }
            if (m_bAnalyzerOwned && analyzer != null)
            {
                try
                {
                    analyzer.CloseAnalysis();
                    analyzer.CloseDataFiles();
                }
                catch
                {
                }
                Marshal.ReleaseComObject(analyzer);
            }
            analyzer = null;
        }

        #endregion

        #region remoting

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(100);
#if DEBUG
                lease.SponsorshipTimeout = TimeSpan.FromSeconds(300);
#else
                lease.SponsorshipTimeout = TimeSpan.FromSeconds(30);
#endif
                lease.RenewOnCallTime = TimeSpan.FromSeconds(100);
                lease.Register(this);

            }
            return lease;

        }

        public TimeSpan Renewal(ILease lease)
        {
            try
            {
                if (pulse != null && pulse.isAlife())
                    return lease.InitialLeaseTime;
            }
            catch (Exception)
            { }
            //Something went wrong --> Disconnect
            return new TimeSpan(0);
        }
#endregion
    }

    public class AnalyzerSignalTreeExt : MarshalByRefObject, IDisposable, IbaAnalyzer.ISignalTree
    {
        IbaAnalyzer.ISignalTree tree;

        public AnalyzerSignalTreeExt(IbaAnalyzer.ISignalTree tree)
        {
            this.tree = tree;
        }

        public dynamic GetRootNode()
        {
            object root = tree.GetRootNode();
            if (root == null) return null;
            return new AnalyzerSignalTreeNodeExt(root as IbaAnalyzer.ISignalTreeNode);
        }

        public dynamic FindNodeWithID(string channelId)
        {
            object node = tree.FindNodeWithID(channelId);
            if (node == null) return null;
            return new AnalyzerSignalTreeNodeExt(node as IbaAnalyzer.ISignalTreeNode);
        }

#region IDisposable Support

        ~AnalyzerSignalTreeExt() => Dispose(false);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (tree == null) return; //redundant call;
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }
            if (tree != null)
            {
                Marshal.ReleaseComObject(tree);
            }
            tree = null;
        }
#endregion

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromMinutes(5);
                //lease.SponsorshipTimeout = TimeSpan.FromMinutes(2);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(2);
            }
            return lease;
        }
    }

    public class AnalyzerSignalTreeNodeExt : MarshalByRefObject, IDisposable, IbaAnalyzer.ISignalTreeNode
    {
        IbaAnalyzer.ISignalTreeNode node;

        public AnalyzerSignalTreeNodeExt(IbaAnalyzer.ISignalTreeNode node)
        {
            this.node = node;

        }

        public dynamic GetFirstChildNode()
        {
            object newnode = this.node.GetFirstChildNode();
            if (newnode == null) return null;
            return new AnalyzerSignalTreeNodeExt(newnode as IbaAnalyzer.ISignalTreeNode);
        }

        public dynamic GetSiblingNode()
        {
            object newnode = this.node.GetSiblingNode();
            if (newnode == null) return null;
            return new AnalyzerSignalTreeNodeExt(newnode as IbaAnalyzer.ISignalTreeNode);
        }

        public dynamic GetParentNode()
        {
            object newnode = this.node.GetParentNode();
            if (newnode == null) return null;
            return new AnalyzerSignalTreeNodeExt(newnode as IbaAnalyzer.ISignalTreeNode);
        }

        public void Expand()
        {
            node.Expand();
        }

        public string Text
        {
            get
            {
                return node.Text;
            }
        }

        public string channelId
        {
            get
            {
                return node.channelId;
            }
        }

        public int ImageIndex
        {
            get
            {
                return node.ImageIndex;
            }
        }

        public int IndexInCollection
        {
            get
            {
                return node.IndexInCollection;
            }
        }

#region IDisposable Support

        ~AnalyzerSignalTreeNodeExt() => Dispose(false);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (node == null) return; //redundant call;
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }
            if (node != null)
            {
                Marshal.ReleaseComObject(node);
            }
            node = null;
        }
#endregion

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromMinutes(5);
                //lease.SponsorshipTimeout = TimeSpan.FromMinutes(2);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(2);
            }
            return lease;
        }
    }

    public class ibaAnalylerClientWrapper : IDisposable, IbaAnalyzer.IbaAnalyzer
    {
        ibaAnalyzerExt remoteIbaAnalyzer;
        public ibaAnalylerClientWrapper(ibaAnalyzerExt ibaAnalyzer)
        {
            remoteIbaAnalyzer = ibaAnalyzer;
        }

        public string GetVersion()
        {
            try
            {
                return remoteIbaAnalyzer.GetVersion();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return "";
            }
        }

        public void OpenAnalysis(string filename)
        {
            try
            {
                remoteIbaAnalyzer.OpenAnalysis(filename);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void CloseAnalysis()
        {
            try
            {
                remoteIbaAnalyzer.CloseAnalysis();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }


        public string GetLastError()
        {
            try
            {
                return remoteIbaAnalyzer.GetLastError();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return ex.Message;
            }
        }


        public void RunInteractive()
        {
            try
            {
                remoteIbaAnalyzer.RunInteractive();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void GetStartTime(ref DateTime oaStartTime, ref int microSecPart)
        {
            try
            {
                remoteIbaAnalyzer.GetStartTime(ref oaStartTime, ref microSecPart);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void SaveAnalysis(string filename)
        {
            try
            {
                remoteIbaAnalyzer.SaveAnalysis(filename);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void OpenDataFile(int index, string filename)
        {
            try
            {
                remoteIbaAnalyzer.OpenDataFile(index, filename);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void AppendDataFile(int index, string filename)
        {
            try
            {
                remoteIbaAnalyzer.AppendDataFile(index, filename);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void CloseDataFile(int index)
        {
            try
            {
                remoteIbaAnalyzer.CloseDataFile(index);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void CloseDataFiles()
        {
            try
            {
                remoteIbaAnalyzer.CloseDataFiles();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void AddToFileGroup(string filename, int select)
        {
            try
            {
                remoteIbaAnalyzer.AddToFileGroup(filename, select);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void ClearFileGroup()
        {
            try
            {
                remoteIbaAnalyzer.ClearFileGroup();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public int RunSqlQuery(string sql, string sync)
        {
            try
            {
                return remoteIbaAnalyzer.RunSqlQuery(sql, sync);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return 0;
            }
        }

        public int RunSqlQueryFromFile(string filename, string sync)
        {
            try
            {
                return remoteIbaAnalyzer.RunSqlQueryFromFile(filename, sync);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return 0;
            }
        }

        public string GetQueryResult(int resultIndex)
        {
            try
            {
                return remoteIbaAnalyzer.GetQueryResult(resultIndex);

            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return "";
            }
        }

        public void Extract(int fileExtract, string output)
        {
            try
            {
                remoteIbaAnalyzer.Extract(fileExtract, output);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void Report(string output)
        {
            try
            {
                remoteIbaAnalyzer.Report(output);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void Print() {
            try
            {
                remoteIbaAnalyzer.Print();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public float Evaluate(string expression, int XType)
        {
            try
            {
                return remoteIbaAnalyzer.Evaluate(expression, XType);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return float.NaN;
            }
        }

        public int GetProcessID()
        {

            try
            {
                return remoteIbaAnalyzer.GetProcessID();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return 0;
            }
        }

        public void AddLogical(string name, string expression, int XType)
        {
            try
            {
                remoteIbaAnalyzer.AddLogical(name, expression, XType);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public bool RemoveLogical(string name)
        {
            try
            {
                return remoteIbaAnalyzer.RemoveLogical(name);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return false;
            }
        }

        public bool ReplaceExpression(string name, string expr)
        {
            try
            {
                return remoteIbaAnalyzer.ReplaceExpression(name, expr);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return false;
            }
        }

        public double EvaluateDouble(string expression, int XType)
        {
            try
            {
                return remoteIbaAnalyzer.EvaluateDouble(expression, XType);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return double.NaN;
            }
        }

        public int EvaluateDataType(string expression, int XType)
        {
            try
            {
                return remoteIbaAnalyzer.EvaluateDataType(expression, XType);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return -1;
            }
        }

        public int RunSqlTrendQuery(string sql, string sync, int append, int microSecond, int overview)
        {
            try
            {
                return remoteIbaAnalyzer.RunSqlTrendQuery(sql, sync, append, microSecond, overview);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return 0;
            }
        }


        public int RunSqlTrendQueryFromFile(string filename, string sync, int append, int microSecond, int overview)
        {
            try
            {
                return remoteIbaAnalyzer.RunSqlTrendQueryFromFile(filename, sync, append, microSecond, overview);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return 0;
            }
        }

        public void SaveGraphImage(int graphIndex, string filename, int width, int height)
        {
            try {
                remoteIbaAnalyzer.SaveGraphImage(graphIndex, filename, width, height);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public int GetGraphCount()
        {
            try {
                return remoteIbaAnalyzer.GetGraphCount();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return 0;
            }
        }

        public int GetZoomAreaBegin(int XType)
        {
            try
            {
                return remoteIbaAnalyzer.GetZoomAreaBegin(XType);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return 0;
            }
        }

        public int GetZoomAreaEnd(int XType)
        {
            try
            {
                return remoteIbaAnalyzer.GetZoomAreaEnd(XType);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return 0;
            }
        }

        public string GetFileExtractParameters()
        {
            try
            {
                return remoteIbaAnalyzer.GetFileExtractParameters();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return "";
            }
        }

        public void SetFilePassword(string filename, string password)
        {
            try
            {
                remoteIbaAnalyzer.SetFilePassword(filename, password);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void SetHDCredentials(string userName, string password)
        {
            try
            {
                remoteIbaAnalyzer.SetHDCredentials(userName, password);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void SetHDCredentialsEx(int useCurrentWindowsUser, string userName, string password)
        {
            try
            {
                remoteIbaAnalyzer.SetHDCredentialsEx(useCurrentWindowsUser, userName, password);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void EvaluateToArray(string expression, int XType, out double pTimebase, out double xoffset, out object pData)
        {
            try
            {
                remoteIbaAnalyzer.EvaluateToArray(expression, XType, out pTimebase, out xoffset, out pData);
            }
            catch (Exception ex)
            {
                pTimebase = xoffset = 0;
                pData = null;
                HandleBrokenConnection(ex);
            }
        }

        public uint GetMarkerColor(int index)
        {
            try
            {
                return remoteIbaAnalyzer.GetMarkerColor(index);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return 0;
            }
        }

        public void UpdateOverlay()
        {
            try {
                remoteIbaAnalyzer.UpdateOverlay();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void EvaluateToStringArray(string expression, int XType, out object pTimeStamps, out object pStrings)
        {
            try
            {
                remoteIbaAnalyzer.EvaluateToStringArray(expression, XType, out pTimeStamps, out pStrings);
            }
            catch (Exception ex)
            {
                pTimeStamps = pStrings = null;
                HandleBrokenConnection(ex);
            }
        }

        public void EvaluateToNEArray(string expression, int XType, out object pTimeStamps, out object pValues)
        {
            try
            {
                remoteIbaAnalyzer.EvaluateToNEArray(expression, XType, out pTimeStamps, out pValues);
            }
            catch (Exception ex)
            {
                pTimeStamps = pValues = null;
                HandleBrokenConnection(ex);
            }
        }

        public dynamic GetSignalTree(int filter)
        {
            try {
                return remoteIbaAnalyzer.GetSignalTree(filter);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return null;
            }
        }

        public void SignalTreeImageData(int index, out object pData)
        {
            try
            {
                remoteIbaAnalyzer.SignalTreeImageData(index, out pData);
            }
            catch (Exception ex)
            {
                pData = null;
                HandleBrokenConnection(ex);
            }
        }

        public void Dispose()
        {
            try
            {
                ((IDisposable)remoteIbaAnalyzer).Dispose();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public bool IsVisible
        {
            get
            {
                try
                {
                    return remoteIbaAnalyzer.IsVisible;
                }
                catch (Exception ex)
                {
                    HandleBrokenConnection(ex);
                    return false;
                }
            }


            set
            {
                throw new NotImplementedException();
            }
        }


        public int SignalTreeImageCount
        {
            get
            {
                try
                {
                    return remoteIbaAnalyzer.SignalTreeImageCount;
                }
                catch (Exception ex)
                {
                    HandleBrokenConnection(ex);
                    return 0;
                }
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        private void HandleBrokenConnection(Exception ex)
        {
            if (ex.Message.Contains("E_FAIL")) //ordinary ibaAnalyzer excpetion -> rethrow
                throw ex;
            else
                Program.CommunicationObject.HandleBrokenConnection(ex);
        }

    }

}