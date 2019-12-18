using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iba.Utility;
using iba.Data;
using iba.Logging;

namespace iba.Processing
{
    /// <summary>
    /// Wrapper around the ibaAnalyzer COM object. 
    /// This is used in the ibaAnalyzerCollection.
    /// </summary>
    class ibaAnalyzerWrapper : IbaAnalyzer.IbaAnalyzer
    {
        public ibaAnalyzerWrapper(IbaAnalyzer.IbaAnalyzer analyzer)
        {
            this.analyzer = analyzer;
        }

        IbaAnalyzer.IbaAnalyzer analyzer;

        public void Release()
        {
            if (analyzer != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(analyzer);
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

        public void AddLogical(string name, string expression, int XType)
        {
            analyzer.AddLogical(name, expression, XType);
        }

        public void AddToFileGroup(string filename, int select)
        {
            analyzer.AddToFileGroup(filename, select);
        }

        public void ClearFileGroup()
        {
            analyzer.ClearFileGroup();
        }

        public void CloseAnalysis()
        {
            analyzer.CloseAnalysis();
        }

        public float Evaluate(string expression, int XType)
        {
            return analyzer.Evaluate(expression, XType);
        }

        public double EvaluateDouble(string expression, int XType)
        {
            return analyzer.EvaluateDouble(expression, XType);
        }

        public void Extract(int fileExtract, string output)
        {
            analyzer.Extract(fileExtract, output);
        }

        public string GetFileExtractParameters()
        {
            return analyzer.GetFileExtractParameters();
        }

        public int GetGraphCount()
        {
            return analyzer.GetGraphCount();
        }

        public string GetLastError()
        {
            return analyzer.GetLastError();
        }

        public int GetProcessID()
        {
            return analyzer.GetProcessID();
        }

        public string GetQueryResult(int resultIndex)
        {
            return analyzer.GetQueryResult(resultIndex);
        }

        public void GetStartTime(ref DateTime oaStartTime, ref int microSecPart)
        {
            analyzer.GetStartTime(ref oaStartTime, ref microSecPart);
        }

        public string GetVersion()
        {
            return analyzer.GetVersion();
        }

        public void OpenAnalysis(string filename)
        {
            analyzer.OpenAnalysis(filename);
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

        class TempHdqFile
        {
            public string FileName;
            public int OriginalIndex; //Original dat file index in ibaAnalyzer
            public int ActualIndex;   //New dat file index in ibaAnalyzer
        }
        List<TempHdqFile> tempFiles;

        public void OpenDataFile(int index, string filename)
        {
            if(filename.EndsWith(".hdq"))
            {
                //Check if the file contains multiple sections that need to be split into multiple hdq files
                var newFiles = SplitHdqFile(filename);
                if(newFiles.Count > 1)
                {
                    //There are multiple files generated -> let's open them all in ibaAnalyzer
                    if (tempFiles == null)
                        tempFiles = new List<TempHdqFile>();

                    int newIndex = index;
                    foreach(var newFileName in newFiles)
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
            if(tempFiles != null)
            {
                for(int i=tempFiles.Count-1; i>=0; i--)
                {
                    var tempFile = tempFiles[i];
                    if(tempFile.OriginalIndex == index)
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

            if(tempFiles != null)
            {
                foreach(var tempFile in tempFiles)
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

        public void Print()
        {
            analyzer.Print();
        }

        public bool RemoveLogical(string name)
        {
            return analyzer.RemoveLogical(name);
        }

        public bool ReplaceExpression(string name, string expr)
        {
            return analyzer.ReplaceExpression(name, expr);
        }

        public void Report(string output)
        {
            analyzer.Report(output);
        }

        public void RunInteractive()
        {
            analyzer.RunInteractive();
        }

        public int RunSqlQuery(string sql, string sync)
        {
            return analyzer.RunSqlQuery(sql, sync);
        }

        public int RunSqlQueryFromFile(string filename, string sync)
        {
            return analyzer.RunSqlQueryFromFile(filename, sync);
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

        public int GetZoomAreaBegin(int XType)
        {
           return analyzer.GetZoomAreaBegin(XType);
        }

        public int GetZoomAreaEnd(int XType)
        {
            return analyzer.GetZoomAreaEnd(XType);
        }

        public void SetFilePassword(string filename, string password )
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

		public void EvaluateToArray(string expression, int XType, out double pTimebase, out double xoffset, out object pData)
		{
			analyzer.EvaluateToArray(expression, XType, out pTimebase, out xoffset, out pData);
		}

		public uint GetMarkerColor(int index)
		{
			return analyzer.GetMarkerColor(index);
		}

		public void UpdateOverlay()
		{
			analyzer.UpdateOverlay();
		}

		public dynamic GetSignalTree(int filter)
		{
			return analyzer.GetSignalTree(filter);
		}

		public void SignalTreeImageData(int index, out object pData)
		{
			analyzer.SignalTreeImageData(index, out pData);
		}

		public int SignalTreeImageCount
		{ get => analyzer.SignalTreeImageCount; set => throw new NotImplementedException(); }

		public void EvaluateToStringArray(string expression, int XType, out object pTimeStamps, out object pStrings)
		{
			analyzer.EvaluateToStringArray(expression, XType, out pTimeStamps, out pStrings);
		}

		public void EvaluateToNEArray(string expression, int XType, out object pTimeStamps, out object pValues)
		{
			analyzer.EvaluateToNEArray(expression, XType, out pTimeStamps, out pValues);
		}

		public void SaveAnalysis(string file)
		{
			analyzer.SaveAnalysis(file);
		}
	}
}
