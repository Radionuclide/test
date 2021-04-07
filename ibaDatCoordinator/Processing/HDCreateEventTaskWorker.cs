using iba.Data;
using iba.HD.Client;
using iba.HD.Client.Interfaces;
using iba.HD.Common;
using iba.ibaFilesLiteDotNet;
using iba.Remoting;
using iba.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        IHdWriterManager writerManager = null;
        IHdWriter writer = null;
        IEnumerable<HdWriterConfig> cfgs = null;
        ConfigurationWorker m_configWorker;
        string m_dataFile;

        public HDCreateEventTaskWorker(HDCreateEventTaskData localData, ConfigurationWorker configWorker)
        {
            m_ibaAnalyzer = null;
            m_dataFile = null;
            m_configWorker = configWorker;
            m_data = localData;
        }

        public void Dispose()
        {
            try
            {
                writerManager?.Dispose();
                writerManager = null;
                writer = null;
            }
            catch (Exception e)
            {
                Logging.ibaLogger.Log(e);
            }
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
                    dtStart = dtStart.AddTicks(microSeconds * 10);

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

        float EvaluateFloatField(string numericField, IbaAnalyzerMonitor monitor, double from, double to)
        {
            bool bUseSinglePoint = !double.IsNaN(from) && from == to;
            bool bUseRange = !double.IsNaN(from) && !bUseSinglePoint;
            string exprRange = $"XCutRange({{0}},{from},{to})";
            string exprSinglePoint = $"YatX({{0}},{from})";

            string lExpr = numericField;

            if (string.IsNullOrWhiteSpace(lExpr) || lExpr == HDCreateEventTaskData.UnassignedExpression)
                return float.NaN;

            if (bUseRange)
                lExpr = string.Format(exprRange, lExpr);
            else if (bUseSinglePoint)
                lExpr = string.Format(exprSinglePoint, lExpr);

            float result = float.NaN;
            monitor.Execute(() => { result = m_ibaAnalyzer.Evaluate(lExpr, 0); });
            return result;
        }

        string EvaluateTextField(string textField, IbaAnalyzerMonitor monitor, double from, double to, object oStamps = null, object oValues = null)
        {
            if (oStamps == null || oValues == null)
                monitor.Execute(delegate () { m_ibaAnalyzer.EvaluateToStringArray(textField, 0, out oStamps, out oValues); });

            double[] stamps = oStamps as double[];
            string[] strings = oValues as string[];

            if (strings == null || stamps == null || stamps.Length != strings.Length)
                return null;

            for (int j = 0; j < stamps.Length; j++)
            {
                if (!double.IsNaN(from) && stamps[j] < from) continue;
                if (!double.IsNaN(to) && stamps[j] > to) break;
                if (string.IsNullOrEmpty(strings[j])) continue;
                return strings[j];
            }
            return "";
        }

        EventWriterItem GenerateEvent(HDCreateEventTaskData.EventData eventData, int index, IbaAnalyzerMonitor monitor, DateTime startTime, DateTime stopTime, Dictionary<string, Tuple<string[], double[]>> textValues, double stamp = double.NaN, double from = double.NaN, double to = double.NaN, bool bIncoming = true)
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
                
                floats[i] = EvaluateFloatField(eventData.NumericFields[i].Item2, monitor, from, to);

                if (float.IsNaN(floats[i]))
                {
                    string text = EvaluateTextField(eventData.NumericFields[i].Item2, monitor, from, to, null, null);
                    if (!float.TryParse(text, out floats[i]))
                        floats[i] = float.NaN;
                }
            }

            string[] texts = new string[eventData.TextFields.Count];
            for (int i = 0; i < texts.Length; i++)
            {
                var values = textValues[eventData.TextFields[i].Item1];
                string[] strings = values.Item1;
                double[] stamps = values.Item2;

                if (stamps != null && stamps.Length != 0)
                {
                    if (double.IsNaN(from))
                    {
                        texts[i] = stamps[0] > 0.0 ? "" : strings[0];
                        continue;
                    }

                    texts[i] = EvaluateTextField("", monitor, from, to, stamps, strings);
                }
                else
                {
                    float numeric = EvaluateFloatField(eventData.TextFields[i].Item2, monitor, from, to);
                    texts[i] = float.IsNaN(numeric) ? "" : numeric.ToString(CultureInfo.InvariantCulture);
                }
            }

            byte[][] blobs = new byte[eventData.BlobFields.Count][];
            for (int i = 0; i < blobs.Length; i++)
                blobs[i] = null; //Not supported at the moment

            long eventStamp = 0;
            if (double.IsNaN(stamp))
            {
                if (eventData.TriggerMode == HDCreateEventTaskData.HDEventTriggerEnum.PerFile)
                {
                    string perFileTimeSignal = bIncoming ? eventData.TimeSignal : eventData.TimeSignalOutgoing;

                    if (perFileTimeSignal == HDCreateEventTaskData.StartTime)
                    {
                        eventStamp = startTime.Ticks;
                    }
                    else if (perFileTimeSignal == HDCreateEventTaskData.EndTime)
                    {
                        eventStamp = stopTime.Ticks;
                    }
                    else
                    {
                        double seconds = 0;
                        monitor.Execute(() => { seconds = m_ibaAnalyzer.Evaluate(perFileTimeSignal, 0); });
                        if (double.IsNaN(seconds))
                            throw new ArgumentException("Invalid timeSignal");
                        eventStamp = startTime.AddTicks((long)(TimeSpan.TicksPerSecond * seconds)).Ticks;
                    }
                }
                else
                {
                    if (double.IsNaN(from))
                        eventStamp = stopTime.Ticks;
                    else
                        eventStamp = startTime.AddTicks((long)(TimeSpan.TicksPerSecond * to)).Ticks;
                }
            }
            else
            {
                eventStamp = startTime.AddTicks((long)(TimeSpan.TicksPerSecond * stamp)).Ticks;
            }

            long duration = 0;

            if (bUseRange)
                duration = (long)(to - from) * TimeSpan.TicksPerSecond;
            else if (double.IsNaN(from))
                duration = eventStamp - startTime.Ticks;

            return new EventWriterItem(index, eventStamp, duration, bIncoming, !bIncoming, floats, texts, blobs);
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
            config.Password = task.HDPassword;
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
                    config.Password = task.HDPassword;
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
            m_ibaAnalyzer = (ibaAnalyzer)??(ibaAnalyzerExt.Create(true));
            m_dataFile = dataFile;
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            if (!VersionCheck.CheckIbaAnalyzerVersion("7.1.0"))
                throw new HDCreateEventException(string.Format(Properties.Resources.logAnalyzerVersionError, "7.1.0"));

            // Generate events
            bool bDisposeAnalyzer = false;
            try
            {
                string pdoFile = m_data.AnalysisFile;
                if (m_ibaAnalyzer == null)
                {
                    if (!DataPath.FileExists(m_data.DatFile))
                        throw new HDCreateEventException(Properties.Resources.logHDEventTaskDATError);

                    bDisposeAnalyzer = true;
                    m_ibaAnalyzer = ibaAnalyzerExt.Create(true);

                    if (Path.GetExtension(m_data.DatFile)==".hdq")
                    {
                        IbaAnalyzerCollection.TrySetHDCredentials(m_ibaAnalyzer, m_data.ParentConfigurationData);
                    }
                    else 
                    if (!string.IsNullOrEmpty(m_data.DatFilePassword))
                        m_ibaAnalyzer.SetFilePassword("", m_data.DatFilePassword);

                    try
                    {
                        m_ibaAnalyzer.OpenDataFile(0, m_data.DatFile);
                    }
                    catch
                    {
                        throw new HDCreateEventException(string.Format(Properties.Resources.ErrorDatFileOpen, m_data.DatFile));
                    }
                    m_dataFile = m_data.DatFile;
                }
                
                if (!DataPath.FileExists(pdoFile))
                    throw new HDCreateEventException(iba.Properties.Resources.AnalysisFileNotFound + pdoFile);

                if (!TryGetUTCTimes(m_dataFile, out DateTime startTime, out DateTime endTime))
                    throw new HDCreateEventException(Properties.Resources.logHDEventTaskTimeError);

                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_data.MonitorData))
                {
                    try
                    {
                        mon.Execute(delegate () { m_ibaAnalyzer.OpenAnalysis(pdoFile); });
                    }
                    catch (Exception e)
                    {
                        throw new HDCreateEventException(string.Format(Properties.Resources.PDOOpenFailed, pdoFile) + e.Message);
                    }

                    Dictionary<string, Tuple<string[], double[]>> textResults = new Dictionary<string, Tuple<string[], double[]>>();
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
                                textResults[textField.Item1] = Tuple.Create(new string[] { "" }, new double[] { 0.0 });
                                continue;
                            }

                            if (textField.Item2 == HDCreateEventTaskData.CurrentFileExpression)
                            {
                                textResults[textField.Item1] = Tuple.Create(new string[] { Path.GetFullPath(m_dataFile) }, new double[] { 0.0 });
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
                                    textResults[textField.Item1] = Tuple.Create(new string[] { $"{server}:{port}/{eventData.StoreName}" }, new double[] { 0.0 });
                                }
                                continue;
                            }
                            else if (textField.Item2 == HDCreateEventTaskData.ClientIDExpression)
                            {
                                textResults[textField.Item1] = Tuple.Create(new string[] { $"{Environment.MachineName}/{Environment.UserName}/ibaDatCoordinator" }, new double[] { 0.0 });
                                continue;
                            }

                            object oStamps = null;
                            object oValues = null;

                            try
                            {
                                mon.Execute(delegate () { m_ibaAnalyzer.EvaluateToStringArray(textField.Item2, 0, out oStamps, out oValues); });
                            }
                            catch (Exception e)
                            {
                                m_configWorker?.Log(Logging.Level.Warning, string.Format(Properties.Resources.GenerateTextFieldFailed, textField.Item2, e), dataFile);
                            }                            

                            double[] stamps = oStamps as double[];
                            string[] values = oValues as string[];

                            textResults[textField.Item1] = Tuple.Create(values, stamps);
                        }
                        if (eventData.TriggerMode == HDCreateEventTaskData.HDEventTriggerEnum.PerSignalPulse)
                        {
                            if (string.IsNullOrWhiteSpace(eventData.PulseSignal))
                                throw new HDCreateEventException(Properties.Resources.logHDEventTaskPulseSignalError);

                            double timebase = 0;
                            double xoffset = 0;
                            object data = null;
                            try
                            {
                                mon.Execute(() => { m_ibaAnalyzer.EvaluateToArray(eventData.PulseSignal, 0, out timebase, out xoffset, out data); });
                            }
                            catch (Exception e)
                            {
                                throw new HDCreateEventException(string.Format(Properties.Resources.logHDEventTaskPulseSignalError + $": {e}", eventData.Name));
                            }

                            double[] pulseValues = data as double[];
                            if (pulseValues == null || pulseValues.Length == 0)
                                throw new HDCreateEventException(string.Format(Properties.Resources.logHDEventTaskPulseSignalError, eventData.Name));
                            // Determine intervals
                            bool bInPulse = false;
                            bool bGenerateIncomming = false;

                            if (pulseValues.Length > 0)
                                bInPulse = pulseValues[0] > 0.0;

                            double currStart = 0.0;
                            List<Tuple<double, double, bool, bool>> intervals = new List<Tuple<double, double, bool, bool>>();

                            for (int i = 0; i < pulseValues.Length; i++)
                            {
                                bool bAboveZero = pulseValues[i] > 0.0;
                                if (bInPulse == bAboveZero)
                                    continue;

                                if (bAboveZero)
                                {
                                    currStart = xoffset + i * timebase;
                                    bGenerateIncomming = true;
                                }
                                else
                                    intervals.Add(Tuple.Create(currStart, xoffset + i * timebase, bGenerateIncomming, true));

                                bInPulse = bAboveZero;
                            }

                            if (bInPulse)
                                intervals.Add(Tuple.Create(currStart, xoffset + (pulseValues.Length - 1) * timebase, bGenerateIncomming, false));

                            // Create events
                            foreach (var interval in intervals)
                            {
                                EventWriterItem incoming;
                                EventWriterItem outgoing;

                                double stamp = (eventData.Slope == HDCreateEventTaskData.TriggerSlope.Rising || eventData.Slope == HDCreateEventTaskData.TriggerSlope.RisingFalling) ? interval.Item1 : interval.Item2;

                                double from = stamp;
                                double to = stamp;

                                //Generate the incoming event
                                if (interval.Item3)
                                {
                                    incoming = GenerateEvent(eventData, eventIndex[eventData.StoreName], mon, startTime, endTime, textResults, stamp, from, to, true);

                                    // Add the incoming event to the list of generated events
                                    if (generated.ContainsKey(eventData.StoreName))
                                        generated[eventData.StoreName].Items.Add(incoming);
                                    else
                                        generated[eventData.StoreName] = new EventWriterData(new List<EventWriterItem>() { incoming });
                                }

                                if (interval.Item4)
                                {
                                    // Generate and add the outgoing event if it exists
                                    if (eventData.Slope == HDCreateEventTaskData.TriggerSlope.RisingFalling)
                                        stamp = interval.Item2;
                                    else if (eventData.Slope == HDCreateEventTaskData.TriggerSlope.FallingRising)
                                        stamp = interval.Item1;
                                    else
                                        stamp = double.NaN;

                                    from = stamp;
                                    to = stamp;

                                    if (eventData.Slope == HDCreateEventTaskData.TriggerSlope.RisingFalling || eventData.Slope == HDCreateEventTaskData.TriggerSlope.FallingRising)
                                    {
                                        outgoing = GenerateEvent(eventData, eventIndex[eventData.StoreName], mon, startTime, endTime, textResults, stamp, from, to, false);

                                        // Add the outgoing event to the list of generated events
                                        if (generated.ContainsKey(eventData.StoreName))
                                            generated[eventData.StoreName].Items.Add(outgoing);
                                        else
                                            generated[eventData.StoreName] = new EventWriterData(new List<EventWriterItem>() { outgoing });
                                    }
                                }
                                //events.Add(GenerateEvent(eventData, j, mon, startTime, endTime, textResults, interval.Item1, interval.Item2));
                            }
                        }
                        else
                        {
                            // One event for the entire file
                            if (generated.ContainsKey(eventData.StoreName))
                                generated[eventData.StoreName].Items.Add(GenerateEvent(eventData, eventIndex[eventData.StoreName], mon, startTime, endTime, textResults, double.NaN, double.NaN, double.NaN, true));
                            else
                                generated[eventData.StoreName] = new EventWriterData(new List<EventWriterItem>() { GenerateEvent(eventData, eventIndex[eventData.StoreName], mon, startTime, endTime, textResults, double.NaN, double.NaN, double.NaN, true) });

                            if (!string.IsNullOrEmpty(eventData.TimeSignalOutgoing) && eventData.TimeSignalOutgoing != HDCreateEventTaskData.UnassignedExpression)
                                generated[eventData.StoreName].Items.Add(GenerateEvent(eventData, eventIndex[eventData.StoreName], mon, startTime, endTime, textResults, double.NaN, double.NaN, double.NaN, false));

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
            IHdWriterSummary summary = null;
            try
            {
                IEnumerable<HdWriterConfig> newCfgs = CreateHDWriterConfig(m_data, storeNames);

                bool same = cfgs != null && cfgs.Count() == newCfgs.Count();

                if (same)
                {
                    foreach (HdWriterConfig config in cfgs)
                    {
                        bool found = false;
                        foreach (HdWriterConfig newConfig in newCfgs)
                        {
                            if (config.Equals(newConfig))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            same = false;
                            break;
                        }
                    }
                }

                cfgs = newCfgs;

                // Check the connection status of the existing writer and try to reconnect if needed
                if (writer != null && writer.Status != HdWriterStatus.Open)
                {
                    Logging.ibaLogger.Log(Logging.Level.Debug, $"Connection the the ibaHD-server was lost: Reconnecting");
                    writer.Reconnect();

                    if (writer.Status != HdWriterStatus.Open)
                    {
                        Logging.ibaLogger.Log(Logging.Level.Debug, $"Failed to reconnect the ibaHD writer. Status: {writer.Status}");
                        writer.Dispose();
                        writer = null;
                    }
                }

                if (writerManager == null)
                    writerManager = HdClient.CreateWriterManager();

                if (!same || writer == null)
                {
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
                        writer = writerManager.CreateWriter(summary, true, validator);
                        writerManager.EndCreate();

                        if (eventData == null)
                        {
                            writer.Dispose();
                        }

                    }

                    if (writer.Status != HdWriterStatus.Open)
                    {
                        m_configWorker?.Log(Logging.Level.Exception, Properties.Resources.ErrorConnectIbaHD);
                        writer.Dispose();
                        writer = null;
                        return summary?.Errors;
                    }
                }

                foreach (HdWriterConfig cfg in cfgs)
                {
                    if (eventData != null && eventData.ContainsKey(cfg.StoreId.StoreName))
                        writer.Write(eventData[cfg.StoreId.StoreName]);
                }
            }
            catch (Exception e)
            {
                m_configWorker?.Log(Logging.Level.Exception, Properties.Resources.ErrorWriteEvents);
                Logging.ibaLogger.Log(e);
                writerManager.Dispose();
                writerManager = null;
            }

            return summary?.Errors;
        }

        public string Test(string datFile)
        {
            string error = "";
            try
            {

                bool ok = true;
                if (string.IsNullOrEmpty(datFile) || !File.Exists(datFile))
                {
                    error = Properties.Resources.logHDEventTaskDATError;
                    ok = false;
                }

                if (ok)
                    GenerateEvents(null, datFile);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return error;
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
