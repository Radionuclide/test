using iba.Data;
using iba.HD.Client;
using iba.HD.Client.Interfaces;
using iba.HD.Common;
using iba.ibaFilesLiteDotNet;
using iba.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Processing
{
    class HDCreateEventTaskWorker
    {
        static readonly HdWriterOrigin hdWriterOrigin = new HdWriterOrigin(Guid.NewGuid(), "DatCo");

        HDCreateEventTaskData m_data;

        IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer;
        string m_dataFile;

        public HDCreateEventTaskWorker(HDCreateEventTaskData data)
        {
            m_ibaAnalyzer = null;
            m_dataFile = null;

            m_data = data;
        }

        bool TryGetUTCTimes(string filename, out DateTime startTime, out DateTime endTime)
        {
            startTime = endTime = DateTime.MinValue;

            try
            {
                if (Path.GetExtension(filename)?.ToLower() == ".hdq")
                {
                    IniParser parser = new IniParser(filename);
                    if (parser.Read() && parser.Sections.ContainsKey("HDQ file"))
                        return false;

                    string strStart = "";
                    if (!parser.Sections["HDQ file"].TryGetValue("starttime", out strStart))
                        return false;

                    string strEnd = "";
                    if (!parser.Sections["HDQ file"].TryGetValue("stoptime", out strEnd))
                        return false;

                    startTime = DateTime.ParseExact(strStart, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
                    endTime = DateTime.ParseExact(strEnd, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
                }
                else
                {
                    IbaShortFileInfo sfi = IbaFileReader.ReadShortFileInfo(filename);
                    DateTime dtEnd = sfi.EndTime;
                    DateTime dtStart = sfi.StartTime;
                    if (sfi.UtcOffsetValid)
                    {
                        var currOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                        var fileOffset = TimeSpan.FromMinutes(sfi.UtcOffset);
                        dtStart = dtStart.AddTicks((currOffset - fileOffset).Ticks);
                        dtEnd = dtEnd.AddTicks((currOffset - fileOffset).Ticks);
                    }

                    startTime = dtStart;
                    endTime = dtEnd;
                }
            }
            catch
            {
                return false;
            }

            startTime = startTime.ToUniversalTime();
            endTime = endTime.ToUniversalTime();
            return true;
        }

        EventWriterItem GenerateEvent(HDCreateEventTaskData task, IbaAnalyzerMonitor monitor, DateTime startTime, DateTime stopTime, Dictionary<string, Tuple<List<string>, List<double>>> textValues, double from = double.NaN, double to = double.NaN) //TODO add text coll args
        {
            bool bUseSinglePoint = !double.IsNaN(from) && from == to;
            bool bUseRange = !double.IsNaN(from) && !bUseSinglePoint;
            string exprRange = $"XCutRange({{0}},{from},{to})";
            string exprSinglePoint = $"YatX({{0}},{from})";

            float[] floats = new float[task.EventSettings.NumericFields.Count];
            for (int i = 0; i < floats.Length; i++)
            {
                string lExpr = task.EventSettings.NumericFields[i].Item2;

                if (string.IsNullOrWhiteSpace(lExpr) || lExpr == HDCreateEventTaskData.UnassignedExpression)
                {
                    floats[i] = float.NaN;
                    continue;
                }

                if (bUseRange)
                    lExpr = string.Format(exprRange, lExpr);
                else if (bUseSinglePoint)
                    lExpr = string.Format(exprSinglePoint, lExpr);

                monitor.Execute(() => { floats[i] = m_ibaAnalyzer.Evaluate(lExpr, 0); });
            }

            string[] texts = new string[task.EventSettings.TextFields.Count];
            for (int i = 0; i < texts.Length; i++)
            {
                var values = textValues[task.EventSettings.TextFields[i].Item1];
                List<string> strings = values.Item1;
                List<double> stamps = values.Item2;

                if (stamps.Count != 0)
                {
                    if (double.IsNaN(from))
                    {
                        texts[i] = stamps[0] > 0.0 ? "" : strings[0];
                        continue;
                    }

                    int j = 0;
                    while (j < stamps.Count && stamps[j] <= from)
                        j++;

                    texts[i] = j == 0 ? "" : strings[j - 1];
                }
                else
                    texts[i] = "";
            }

            byte[][] blobs = new byte[task.EventSettings.BlobFields.Count][];
            for (int i = 0; i < blobs.Length; i++)
                blobs[i] = null; //Not supported at the moment

            long duration = 0;
            if (bUseRange)
                duration = TimeSpan.FromSeconds(to - from).Ticks;
            else if (double.IsNaN(from))
                duration = (stopTime - startTime).Ticks;

            long stamp = 0;
            if (double.IsNaN(from))
                stamp = stopTime.Ticks;
            else
                stamp = startTime.AddTicks(TimeSpan.FromSeconds(to).Ticks).Ticks;

            return new EventWriterItem(0, stamp, duration, true, false, floats, texts, blobs);
        }

        SlimEventWriterConfig CreateHDWriterConfig(HDCreateEventTaskData task)
        {
            SlimEventWriterSignal signal = new SlimEventWriterSignal(HdId.GetSubId(task.EventSettings.ID), task.EventSettings.Name);

            string[] floatFields = new string[task.EventSettings.NumericFields.Count];
            for (int i = 0; i < task.EventSettings.NumericFields.Count; i++)
                floatFields[i] = task.EventSettings.NumericFields[i].Item1;
            signal.FloatFields = floatFields;

            string[] textFields = new string[task.EventSettings.TextFields.Count];
            for (int i = 0; i < task.EventSettings.TextFields.Count; i++)
                textFields[i] = task.EventSettings.TextFields[i].Item1;
            signal.TextFields = textFields;

            string[] blobFields = new string[task.EventSettings.BlobFields.Count];
            for (int i = 0; i < task.EventSettings.BlobFields.Count; i++)
                blobFields[i] = task.EventSettings.BlobFields[i];
            signal.BlobFields = blobFields;

            HdStoreId storeId = task.EventSettings.ServerPort < 0 ? HdStoreId.Empty : new HdStoreId(task.EventSettings.Server, task.EventSettings.ServerPort, task.EventSettings.StoreName);
            return new SlimEventWriterConfig(hdWriterOrigin, storeId, new SlimEventWriterSignal[1] { signal }, true);
        }

        public EventWriterData GenerateEvents(IbaAnalyzer.IbaAnalyzer ibaAnalyzer, string dataFile)
        {
            m_ibaAnalyzer = ibaAnalyzer;
            m_dataFile = dataFile;

            Version versionIbaAnalyzer = null;
            string ibaAnalyzerExe = "";
            try
            {
                Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                object o = key.GetValue("");
                ibaAnalyzerExe = Path.GetFullPath(o.ToString());
            }
            catch
            {
                ibaAnalyzerExe = iba.Properties.Resources.noIbaAnalyser;
            }

            versionIbaAnalyzer = VersionCheck.GetVersion(ibaAnalyzerExe);

            if (versionIbaAnalyzer == null || versionIbaAnalyzer < new Version(7, 1, 0))
                throw new HDCreateEventException(string.Format(Properties.Resources.logAnalyzerVersionError, "7.1.0"));

            // Generate events
            bool bDisposeAnalyzer = false;
            try
            {
                string pdoFile = m_data.AnalysisFile;
                if (m_ibaAnalyzer == null)
                {
                    if (!Program.RemoteFileLoader.DownloadFile(m_data.AnalysisFile, out string localFile, out string error))
                        throw new HDCreateEventException(error);
                    else
                        pdoFile = localFile;

                    if (m_data.DatFileHost != Environment.MachineName || !File.Exists(m_data.DatFile))
                        throw new HDCreateEventException(Properties.Resources.logHDEventTaskDATError);

                    bDisposeAnalyzer = true;
                    m_ibaAnalyzer = new IbaAnalyzer.IbaAnalysisNonInteractive();

                    if (!string.IsNullOrEmpty(m_data.DatFilePassword))
                        m_ibaAnalyzer.SetFilePassword("", m_data.DatFilePassword);

                    m_ibaAnalyzer.OpenDataFile(0, m_data.DatFile);
                    m_dataFile = m_data.DatFile;
                }
                
                if (!File.Exists(pdoFile))
                    throw new HDCreateEventException(iba.Properties.Resources.AnalysisFileNotFound + pdoFile);

                if (!TryGetUTCTimes(m_dataFile, out DateTime startTime, out DateTime endTime))
                    throw new HDCreateEventException(Properties.Resources.logHDEventTaskTimeError);

                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_data.MonitorData))
                {
                    mon.Execute(delegate () { m_ibaAnalyzer.OpenAnalysis(pdoFile); });

                    Dictionary<string, Tuple<List<string>, List<double>>> textResults = new Dictionary<string, Tuple<List<string>, List<double>>>();
                    foreach (var textField in m_data.EventSettings.TextFields)
                    {
                        if (string.IsNullOrWhiteSpace(textField.Item2) || textField.Item2 == HDCreateEventTaskData.UnassignedExpression)
                        {
                            textResults[textField.Item1] = Tuple.Create(new List<string>(1) { "" }, new List<double>(1) { 0.0 });
                            continue;
                        }

                        if (textField.Item2 == HDCreateEventTaskData.CurrentFileExpression)
                        {
                            textResults[textField.Item1] = Tuple.Create(new List<string>(1) { Path.GetFileName(m_dataFile) }, new List<double>(1) { 0.0 });
                            continue;
                        }

                        object oStamps = null;
                        object oValues = null;
                        mon.Execute(delegate () { m_ibaAnalyzer.EvaluateToStringArray(textField.Item2, 0, out oStamps, out oValues); });

                        double[] stamps = oStamps as double[];
                        string[] values = oValues as string[];

                        List<double> lStamps = null;
                        List<string> lValues = null;
                        if (stamps == null || values == null || stamps.Length != values.Length)
                        {
                            lStamps = new List<double>(1) { 0.0 };
                            lValues = new List<string>(1) { "" };
                        }
                        else
                        {
                            lStamps = new List<double>(stamps);
                            lValues = new List<string>(values);
                        }

                        textResults[textField.Item1] = Tuple.Create(lValues, lStamps);
                    }

                    List<EventWriterItem> events = new List<EventWriterItem>();
                    if (m_data.TriggerMode == HDCreateEventTaskData.HDEventTriggerEnum.PerSignalPulse)
                    {
                        if (string.IsNullOrWhiteSpace(m_data.PulseSignal))
                            throw new HDCreateEventException(Properties.Resources.logHDEventTaskPulseSignalError);

                        double timebase = 0;
                        double xoffset = 0;
                        object data = null;
                        mon.Execute(() => { m_ibaAnalyzer.EvaluateToArray(m_data.PulseSignal, 0, out timebase, out xoffset, out data); });

                        double[] pulseValues = data as double[];
                        if (pulseValues == null || pulseValues.Length == 0)
                            throw new HDCreateEventException(Properties.Resources.logHDEventTaskPulseSignalError);

                        // Determine intervals
                        bool bInPulse = false;
                        double currStart = 0.0;
                        List<Tuple<double, double>> intervals = new List<Tuple<double, double>>();

                        for (int i = 0; i < pulseValues.Length; i++)
                        {
                            bool bAboveZero = pulseValues[i] > 0.0;
                            if (bInPulse == bAboveZero)
                                continue;

                            if (bAboveZero)
                                currStart = xoffset + i * timebase;
                            else
                                intervals.Add(Tuple.Create(currStart, xoffset + (i - 1) * timebase));

                            bInPulse = bAboveZero;
                        }

                        if (bInPulse)
                            intervals.Add(Tuple.Create(currStart, xoffset + (pulseValues.Length - 1) * timebase));

                        // Create events
                        foreach (var interval in intervals)
                            events.Add(GenerateEvent(m_data, mon, startTime, endTime, textResults, interval.Item1, interval.Item2));
                    }
                    else
                        events.Add(GenerateEvent(m_data, mon, startTime, endTime, textResults)); // One event for the entire file

                    return new EventWriterData(events);
                }
            }
            finally
            {
                if (bDisposeAnalyzer && m_ibaAnalyzer != null)
                {
                    try
                    {
                        m_ibaAnalyzer.CloseDataFiles();
                        m_ibaAnalyzer.CloseAnalysis();
                    }
                    catch
                    { }

                    try
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(m_ibaAnalyzer);
                    }
                    catch
                    { }

                    m_ibaAnalyzer = null;
                }
            }
        }

        public void WriteEvents(EventWriterData eventData)
        {
            // Write events
            IHdWriterManager writerManager = null;
            try
            {
                HdWriterConfig cfg = CreateHDWriterConfig(m_data);

                writerManager = HdClient.CreateWriterManager();

                writerManager.StartConfig();

                IHdWriterSummary summary = writerManager.SetConfig(cfg, null, HdValidationMessage.Ignore);
                while (summary.Result == WriterConfigResult.Conflict)
                {
                    foreach (var cflt in summary.Conflicts)
                        cflt.Solution = HdWriterSolution.Append;

                    summary = writerManager.SetConfig(cfg, summary, HdValidationMessage.Ignore);
                }

                writerManager.EndConfig();

                if (summary.Result != WriterConfigResult.Valid)
                {
                    StringBuilder sb = new StringBuilder();
                    if (summary.Errors != null && summary.Errors.Count > 0)
                    {
                        foreach (var err in summary.Errors)
                            sb.Append(" ").Append(err.Text).Append(",");

                        sb.Remove(sb.Length - 1, 1);
                    }

                    throw new HDCreateEventException(string.Format(Properties.Resources.logHDEventTaskConfigError, sb.ToString()));
                }

                writerManager.StartCreate();
                IHdWriter writer = writerManager.CreateWriter(summary, true, HdValidationMessage.Ignore);
                writerManager.EndCreate();

                if (writer == null || writer.Status != HdWriterStatus.Open)
                    throw new HDCreateEventException(Properties.Resources.logHDEventTaskActivateError);

                writer.Write(eventData);
            }
            finally
            {
                if (writerManager != null)
                {
                    try
                    {
                        writerManager.Dispose();
                    }
                    catch
                    { }
                }
            }
        }
    }

    internal class HDCreateEventException : Exception
    {
        public HDCreateEventException(string message)
            :base(message)
        { }
    }
}
