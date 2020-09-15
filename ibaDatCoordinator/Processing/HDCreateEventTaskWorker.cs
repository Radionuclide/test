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
using System.Threading;
using System.Threading.Tasks;

namespace iba.Processing
{
    class HDCreateEventTaskWorker
    {
        static readonly HdWriterOrigin hdWriterOrigin = new HdWriterOrigin(Guid.NewGuid(), "DatCo");

        HDCreateEventTaskData m_data;

        IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer;
        string m_dataFile;

        public HDCreateEventTaskWorker(HDCreateEventTaskData localData)
        {
            m_ibaAnalyzer = null;
            m_dataFile = null;

            m_data = localData;
        }

        bool TryGetUTCTimes(string filename, out DateTime startTime, out DateTime endTime)
        {
            startTime = endTime = DateTime.MinValue;

            try
            {
                if (Path.GetExtension(filename)?.ToLower() == ".hdq")
                {
                    IniParser parser = new IniParser(filename);
                    if (parser.Read() && !parser.Sections.ContainsKey("HDQ file"))
                        return false;

                    string strStart = "";
                    if (!parser.Sections["HDQ file"].TryGetValue("starttime", out strStart))
                        return false;

                    string strEnd = "";
                    if (!parser.Sections["HDQ file"].TryGetValue("stoptime", out strEnd))
                        return false;

                    startTime = DateTime.ParseExact(strStart, "dd.MM.yyyy HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
                    endTime = DateTime.ParseExact(strEnd, "dd.MM.yyyy HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
                }
                else
                {
                    IbaShortFileInfo sfi = IbaFileReader.ReadShortFileInfo(filename);
                    DateTime dtEnd = sfi.EndTime;
                    DateTime dtStart = new DateTime();
                    int microSeconds = 0;
                    m_ibaAnalyzer.GetStartTime(ref dtStart, ref microSeconds);                    
                    dtStart.AddTicks(microSeconds * 10);

                    if (sfi.UtcOffsetValid)
                    {
                        var currOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                        var fileOffset = TimeSpan.FromMinutes(sfi.UtcOffset);
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

        EventWriterItem GenerateEvent(HDCreateEventTaskData.EventData eventData, int index, IbaAnalyzerMonitor monitor, DateTime startTime, DateTime stopTime, Dictionary<string, Tuple<List<string>, List<double>>> textValues, double from = double.NaN, double to = double.NaN)
        {
            bool bUseSinglePoint = !double.IsNaN(from) && from == to;
            bool bUseRange = !double.IsNaN(from) && !bUseSinglePoint;
            string exprRange = $"XCutRange({{0}},{from},{to})";
            string exprSinglePoint = $"YatX({{0}},{from})";

            float[] floats = new float[eventData.NumericFields.Count];
            for (int i = 0; i < floats.Length; i++)
            {
                string lExpr = eventData.NumericFields[i].Item2;

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

            string[] texts = new string[eventData.TextFields.Count];
            for (int i = 0; i < texts.Length; i++)
            {
                var values = textValues[eventData.TextFields[i].Item1];
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

            byte[][] blobs = new byte[eventData.BlobFields.Count][];
            for (int i = 0; i < blobs.Length; i++)
                blobs[i] = null; //Not supported at the moment

            long stamp = 0;

            if (eventData.TriggerMode == HDCreateEventTaskData.HDEventTriggerEnum.PerFile)
            {
                if (eventData.TimeSignal == HDCreateEventTaskData.StartTime)
                {
                    stamp = startTime.Ticks;
                }
                else if (eventData.TimeSignal == HDCreateEventTaskData.EndTime)
                {
                    stamp = stopTime.Ticks;
                }
                else
                {
                    double seconds = 0;
                    monitor.Execute(() => { seconds = m_ibaAnalyzer.Evaluate(eventData.TimeSignal, 0); });
                    if (double.IsNaN(seconds))
                        throw new ArgumentException("Invalid timeSignal");
                    stamp = startTime.AddSeconds(seconds).Ticks;
                }
            }
            else
            {
                if (double.IsNaN(from))
                    stamp = stopTime.Ticks;
                else
                    stamp = startTime.AddTicks(TimeSpan.FromSeconds(to).Ticks).Ticks;
            }

            long duration = 0;
            if (bUseRange)
                duration = TimeSpan.FromSeconds(to - from).Ticks;
            else if (double.IsNaN(from))
                duration = stamp - startTime.Ticks;

            return new EventWriterItem(index, stamp, duration, true, false, floats, texts, blobs);
        }

        SlimEventWriterConfig CreateSlimHDWriterConfig(HDCreateEventTaskData task, string store)
        {
            HdStoreId storeId = null;
            List<SlimEventWriterSignal> writerSignalConfigs = new List<SlimEventWriterSignal>();
            foreach (var eventData in task.EventSettings)
            {
                if (eventData.StoreName == store && eventData.Active)
                {
                    SlimEventWriterSignal signal = new SlimEventWriterSignal(eventData.ID, eventData.Name);

                    string[] floatFields = new string[eventData.NumericFields.Count];
                    for (int i = 0; i < eventData.NumericFields.Count; i++)
                        floatFields[i] = eventData.NumericFields[i].Item1;
                    signal.FloatFields = floatFields;

                    string[] textFields = new string[eventData.TextFields.Count];
                    for (int i = 0; i < eventData.TextFields.Count; i++)
                        textFields[i] = eventData.TextFields[i].Item1;
                    signal.TextFields = textFields;

                    string[] blobFields = new string[eventData.BlobFields.Count];
                    for (int i = 0; i < eventData.BlobFields.Count; i++)
                        blobFields[i] = eventData.BlobFields[i];
                    signal.BlobFields = blobFields;

                    storeId = task.ServerPort < 0 ? HdStoreId.Empty : new HdStoreId(task.Server, task.ServerPort, eventData.StoreName);
                    writerSignalConfigs.Add(signal);
                }
            }
            SlimEventWriterConfig config = new SlimEventWriterConfig(hdWriterOrigin, storeId, writerSignalConfigs.ToArray(), true);
            config.Password = task.Password;
            config.Username = task.Username;
            return config;
        }

        IEnumerable<HdWriterConfig> CreateHDWriterConfig(HDCreateEventTaskData task, IEnumerable<string> storeNames)
        {
            List<HdWriterConfig> writerConfigs = new List<HdWriterConfig>();
            foreach (string store in storeNames)
            {
                HdStoreId storeId = task.ServerPort < 0 ? HdStoreId.Empty : new HdStoreId(task.Server, task.ServerPort, store);
                List<EventWriterSignal> writerSignalConfigs = new List<EventWriterSignal>();
                if (task?.FullEventConfig?.Count > 0)
                {
                    EventWriterConfig config = new EventWriterConfig(hdWriterOrigin, storeId, new EventWriterSignal[] { }, true);
                    config.Username = task.Username;
                    config.Password = task.Password;
                    config.ServerEventWriterConfig = task.FullEventConfig[store];
                    writerConfigs.Add(config);
                }
                else
                {
                    writerConfigs.Add(CreateSlimHDWriterConfig(task, store));
                }
            }
            return writerConfigs;
        }

        public Dictionary<string, EventWriterData> GenerateEvents(IbaAnalyzer.IbaAnalyzer ibaAnalyzer, string dataFile)
        {
            m_ibaAnalyzer = ibaAnalyzer;
            m_dataFile = dataFile;
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

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
                    m_ibaAnalyzer = new ibaAnalyzerWrapper(new IbaAnalyzer.IbaAnalysisNonInteractive());

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
                    List<EventWriterItem> events = new List<EventWriterItem>();
                    Dictionary<string, EventWriterData> generated = new Dictionary<string, EventWriterData>();
                    Dictionary<string, int> eventIndex = new Dictionary<string, int>();
                    for (int j = 0; j < m_data.EventSettings.Count; j++)
                    {
                        var eventData = m_data.EventSettings[j];

                        // Skip inactive events
                        if (!eventData.Active)
                            continue;

                        if (eventIndex.ContainsKey(eventData.StoreName))
                            eventIndex[eventData.StoreName]++;
                        else
                            eventIndex[eventData.StoreName] = 0;
                        foreach (var textField in eventData.TextFields)
                        {
                            if (string.IsNullOrWhiteSpace(textField.Item2) || textField.Item2 == HDCreateEventTaskData.UnassignedExpression)
                            {
                                textResults[textField.Item1] = Tuple.Create(new List<string>(1) { "" }, new List<double>(1) { 0.0 });
                                continue;
                            }

                            if (textField.Item2 == HDCreateEventTaskData.CurrentFileExpression)
                            {
                                textResults[textField.Item1] = Tuple.Create(new List<string>(1) { Path.GetFullPath(m_dataFile) }, new List<double>(1) { 0.0 });
                                if (Path.GetExtension(m_dataFile)?.ToLower() == ".hdq")
                                {
                                    IniParser parser = new IniParser(m_dataFile);
                                    if (parser.Read() && !parser.Sections.ContainsKey("HDQ file"))
                                        continue;

                                    string server = "";
                                    if (!parser.Sections["HDQ file"].TryGetValue("server", out server))
                                        continue;

                                    string port = "";
                                    if (!parser.Sections["HDQ file"].TryGetValue("portnumber", out port))
                                        continue;
                                    textResults[textField.Item1] = Tuple.Create(new List<string>(1) { $"{server}:{port}/{eventData.StoreName}" }, new List<double>(1) { 0.0 });
                                }
                                continue;
                            }
                            else if (textField.Item2 == HDCreateEventTaskData.ClientIDExpression)
                            {
                                textResults[textField.Item1] = Tuple.Create(new List<string>(1) { $"{Environment.MachineName}/{Environment.UserName}/ibaDatCoordinator" }, new List<double>(1) { 0.0 });
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
                        if (eventData.TriggerMode == HDCreateEventTaskData.HDEventTriggerEnum.PerSignalPulse)
                        {
                            if (string.IsNullOrWhiteSpace(eventData.PulseSignal))
                                throw new HDCreateEventException(Properties.Resources.logHDEventTaskPulseSignalError);

                            double timebase = 0;
                            double xoffset = 0;
                            object data = null;
                            mon.Execute(() => { m_ibaAnalyzer.EvaluateToArray(eventData.PulseSignal, 0, out timebase, out xoffset, out data); });

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
                            {
                                if (generated.ContainsKey(eventData.StoreName))
                                    generated[eventData.StoreName].Items.Add(GenerateEvent(eventData, eventIndex[eventData.StoreName], mon, startTime, endTime, textResults, interval.Item1, interval.Item2));
                                else
                                    generated[eventData.StoreName] = new EventWriterData(new List<EventWriterItem>() { GenerateEvent(eventData, eventIndex[eventData.StoreName], mon, startTime, endTime, textResults, interval.Item1, interval.Item2) });
                            }
                                //events.Add(GenerateEvent(eventData, j, mon, startTime, endTime, textResults, interval.Item1, interval.Item2));
                        }
                        else
                        {
                            // One event for the entire file
                            if (generated.ContainsKey(eventData.StoreName))
                                generated[eventData.StoreName].Items.Add(GenerateEvent(eventData, eventIndex[eventData.StoreName], mon, startTime, endTime, textResults));
                            else
                                generated[eventData.StoreName] = new EventWriterData(new List<EventWriterItem>() { GenerateEvent(eventData, eventIndex[eventData.StoreName], mon, startTime, endTime, textResults) });
                        }
                            //events.Add(GenerateEvent(eventData, j, mon, startTime, endTime, textResults)); // One event for the entire file
                    }
                    return generated;
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

        public IEnumerable<HdValidationMessage> WriteEvents(IEnumerable<string> storeNames, Dictionary<string,EventWriterData> eventData, HdValidatorMulti validator)
        {
            IHdWriterManager writerManager = null;
            IHdWriterSummary summary = null;
            try
            {
                IEnumerable<HdWriterConfig> cfgs = CreateHDWriterConfig(m_data, storeNames);

                writerManager = HdClient.CreateWriterManager();

                foreach (HdWriterConfig cfg in cfgs)
                {
                    writerManager.StartConfig();

                    summary = writerManager.SetConfig(cfg, null, validator);
                    while (summary.Result == WriterConfigResult.Conflict)
                    {
                        foreach (var cflt in summary.Conflicts)
                            cflt.Solution = HdWriterSolution.Append;

                        summary = writerManager.SetConfig(cfg, summary, validator);
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
                    IHdWriter writer = writerManager.CreateWriter(summary, true, validator);
                    writerManager.EndCreate();

                    if (eventData != null && eventData.ContainsKey(cfg.StoreId.StoreName))
                        writer.Write(eventData[cfg.StoreId.StoreName]);
                }

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
            return summary?.Errors;
        }

        // Deprecated do not use. No support for multiple events and multiple stores per task.
        public void WriteEvents(EventWriterData eventData)
        {
            // Write events
            IHdWriterManager writerManager = null;
            try
            {
                HdWriterConfig cfg = CreateSlimHDWriterConfig(m_data, m_data.EventSettings[0].StoreName);

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
