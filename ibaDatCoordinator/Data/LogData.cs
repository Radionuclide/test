using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using iba;
using iba.Utility;
using iba.Logging;
using iba.Logging.Loggers;
using iba.Processing;
using iba.Controls;

namespace iba.Data
{
    [Serializable]
    public class LogExtraData
    {
        private string m_datFile;
        public String DatFile
        {
            get { return m_datFile; }
            set { m_datFile = value; }
        }

        private ConfigurationData m_cd;
        public ConfigurationData ConfigurationData
        {
            get { return m_cd; }
            set { m_cd = value; }
        }

        private TaskData m_task;
        public TaskData TaskData
        {
            get { return m_task; }
            set { m_task = value; }
        }

        public LogExtraData(string datFile, TaskData task, ConfigurationData cd)
        {
            if(cd.JobType == ConfigurationData.JobTypeEnum.Scheduled && !string.IsNullOrEmpty(datFile)) m_datFile = cd.CreateHDQFileDescription(datFile);
            else
                m_datFile = datFile;
            m_cd = cd;
            m_task = task;
        }
    }

    public class LogExtraDataFormatter : DataFormatter
    {
        private static string EmptyResult = "[]\t[]\t[]";
        public override string Format(object o)
        {
            if (o == null) return EmptyResult;
            LogExtraData ld = o as LogExtraData;
            if (ld == null) return EmptyResult;
            string taskname = "";
            if (ld.TaskData != null) taskname = ld.TaskData.Name;
            string confname = "";
            if (ld.ConfigurationData != null) confname = ld.ConfigurationData.Name;
            string filename = "";
            if (ld.DatFile != null) filename = ld.DatFile;
            return "[" + confname + "]\t[" + taskname + "]\t[" + ld.DatFile + "]";
        }

        public override void Format(object o, StringBuilder sb)
        {
            sb.Append(Format(o));
        }

        public override DataFormatter Copy()
        {
            return new LogExtraDataFormatter();
        }
    }

    public class EventForwarder : MarshalByRefObject, IEquatable<EventForwarder>
    {
        public void Forward(int priority, string message, LogExtraData dat)
        {
            try
            {
                LogData.Data.Log(Level.GetLevel(priority), message, dat);
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void ForwardClearCommand()
        {
            (LogData.Data.Logger as GridViewLogger).clear(); 
        }

        public bool Equals(EventForwarder other)
        {
            return other != null && this.Guid == other.Guid;
        }

        private Guid m_guid;

        public Guid Guid
        {
            get
            {
                return m_guid;
            }
        }

        public EventForwarder()
        {
            m_guid = Guid.NewGuid();
        }
    }

    public class GridViewLogger : Logger
    {
        private DataGridView m_grid;
        public DataGridView Grid
        {
            get { return m_grid; }
        }

        private Control m_control;
        public Control LogControl
        {
            get { return m_control; }
        }

        private System.Collections.Concurrent.ConcurrentDictionary<Guid, EventForwarder> m_efDict = new System.Collections.Concurrent.ConcurrentDictionary<Guid, EventForwarder>();
        
        public void AddForwarder(EventForwarder ev, Guid g)
        {
            m_efDict.TryAdd(g, ev);
            //if (m_efDict.TryAdd(g, ev))
            //{
            //    //LogData.Data.FileLog(Level.Info, String.Format("EXTRALOG added logger with guid g {0} Dict count {1}", g, m_efDict.Count));
            //}
            //else 
            //{
            //    //LogData.Data.FileLog(Level.Info, String.Format("EXTRALOG failed adding logger with guid g {0} Dict count {1}", g, m_efDict.Count));
            //}
        }

        public void RemoveForwarder(Guid g)
        {
            EventForwarder ev;
            //if (m_efDict.TryRemove(g, out ev))
            //{
            //    LogData.Data.FileLog(Level.Info, String.Format("EXTRALOG removed logger with guid g {0} Dict count {1}", g, m_efDict.Count));
            //}
            //else
            //{
            //    LogData.Data.FileLog(Level.Info, String.Format("EXTRALOG failed removing logger with guid g {0} Dict count {1}", g, m_efDict.Count));
            //}
            m_efDict.TryRemove(g, out ev);
        }

        public bool IsForwarding
        {
            get { return m_efDict.Count > 0; }
        }

        private delegate void UpdateDelegate(Event _event);
        private delegate bool ReadDelegate(string filename);
        private delegate void ClearDelegate();

        private UpdateDelegate m_updateDelegate;
        //private ReadDelegate m_readDelegate;
        private ClearDelegate m_clearAllRowsDelegate;
        private ClearDelegate m_clearSomeRowsDelegate;
        private ClearDelegate m_updateFilterDelegate;

        private int m_maxRows;
        public int MaxRows
        {
            get { return m_maxRows; }
            set
            {
                m_maxRows = Math.Max(1,value);
                Profiler.ProfileInt(false, "LastState", "LastMaxRows", ref m_maxRows, 50);
                if (m_control != null) //gui present
                    if (m_grid.Rows.Count > m_maxRows) m_control.BeginInvoke(m_clearSomeRowsDelegate);
            }
        }

        //loglevel 0 = all. 1 = warnings,errors. 2 = only errors
        private int m_logLevel;
        public int LogLevel
        {
            get { return m_logLevel; }
            set 
            { 
                int prevLevel = m_logLevel;
                m_logLevel = value;
                Profiler.ProfileInt(false, "LastState", "LastLogLevel", ref m_logLevel, 0);
                if (m_control != null) //gui present
                    if (prevLevel != m_logLevel) m_control.BeginInvoke(m_updateFilterDelegate);
            }
        }

        public void ClearForwarders()
        {
            //LogData.Data.FileLog(Level.Info, "EXTRALOG removing logger");
            m_efDict.Clear();
        }

        private void update(Event _event)
        {
            if ((_event.Level == Logging.Level.Info && m_logLevel > 0) || (_event.Level == Logging.Level.Warning && m_logLevel > 1))
                return;
            lock (m_grid.Rows)
            {
                CacheEvent(_event);
                updateNonLocked(_event);
            }
        }

        private void updateNonLocked(Event _event)
        {
            LogControl lc = (m_control as LogControl);
            bool freeze = lc != null && lc.Freeze;

            int rowpos = freeze ? m_grid.FirstDisplayedScrollingRowIndex : -1;

            while (m_grid.Rows.Count >= m_maxRows && m_grid.Rows.Count > 0)
            {
                m_grid.Rows.RemoveAt(m_grid.Rows.Count - 1);
            }
            
            LogExtraData data = _event.Data as LogExtraData;
            if (data == null)
                m_grid.Rows.Insert(0, new Object[] { 
                    _event.Timestamp, 
                    String.Empty,
                    String.Empty, 
                    String.Empty, 
                    _event.Message });
            else
                m_grid.Rows.Insert(0, new Object[] { 
                     _event.Timestamp, 
                    data.ConfigurationData==null?String.Empty:data.ConfigurationData.Name, 
                    data.TaskData == null?String.Empty:data.TaskData.Name, 
                    data.DatFile, 
                    _event.Message });
            rowpos++;
            DataGridViewCellStyle style = m_grid.Rows[0].Cells[4].Style;
            if (_event.Level == Logging.Level.Warning) style.ForeColor = Color.Orange;
            else if (_event.Level == Logging.Level.Info)
            {
                style.ForeColor = Color.Green;
                Program.MainForm.StatusBarLabelErrors.Text = "";//any positive message clears the status bar
            }
            else if (_event.Level == Logging.Level.Exception)
            {
                style.ForeColor = Color.Red;
                string text = _event.Message;
                int tindex = text.IndexOf(Environment.NewLine);
                if (tindex >= 0)
                    text = text.Remove(tindex);
                Program.MainForm.StatusBarLabelErrors.Text = String.Format(iba.Properties.Resources.StatusBarError, text);
            }
            if (!freeze)
            {
                m_grid.Rows[0].Selected = true;
                if (lc != null) lc.Freeze = false;
            }

            try
            {
                if (freeze && rowpos > 0 && rowpos < m_grid.Rows.Count)
                    m_grid.FirstDisplayedScrollingRowIndex = rowpos;
            }
            catch
            {
            }
        }


        private List<Event> m_cacheErrors;
        private List<Event> m_cacheWarnings;
        private List<Event> m_cacheInfos;

        //cache of events to restore in case filter is changed
        private void CacheEvent(Event _event)
        {
            List<Event> cache = m_cacheInfos;
            if (_event.Level > Level.Info)
                cache = m_cacheWarnings;
            if (_event.Level > Level.Warning)
                cache = m_cacheErrors;
            cache.Add(_event);
            int maxsize = Math.Max(1, m_maxRows);
            while (cache.Count > maxsize)
                cache.RemoveAt(0);
        }

        private void filterRowsToBeInvoked()
        {
            lock (m_grid.Rows)
            {
                m_grid.Rows.Clear();
                List<Event> merged = new List<Event>(3 * m_maxRows);
                merged.AddRange(m_cacheErrors);
                if (m_logLevel <=1)
                    merged.AddRange(m_cacheWarnings);
                if (m_logLevel == 0)
                    merged.AddRange(m_cacheInfos);
                merged.Sort(delegate(Event e1,Event e2){return e1.Timestamp.CompareTo(e2.Timestamp);});
                //oldest are first, remove oldest
                while (merged.Count > m_maxRows)
                    merged.RemoveAt(0);
                foreach (Event _event in merged)
                {
                    updateNonLocked(_event);
                }
            }
        }


        private void clearAllRowsToBeInvoked()
        {
            lock (m_grid.Rows)
            {
                m_grid.Rows.Clear();
                m_cacheErrors.Clear();
                m_cacheInfos.Clear();
                m_cacheWarnings.Clear();
            }
        }

        private void clearSomeRowsToBeInvoked()
        {
            lock (m_grid.Rows)
            {
                while (m_grid.Rows.Count > MaxRows)
                    m_grid.Rows.RemoveAt(0);
            }
        }

        public void clear()
        {
            foreach (EventForwarder ef in m_efDict.Values)
            { 
                if (ef != null)
                {
                    ef.ForwardClearCommand();
                }
            }
            if (m_efDict.Count > 0) return;
            if (m_control != null) //gui present
                m_control.BeginInvoke(m_clearAllRowsDelegate);
        }

        public GridViewLogger(DataGridView grid, Control control)
        {
            m_grid = grid;
            m_control = control;
            m_updateDelegate = new UpdateDelegate(update);
            //m_readDelegate = new ReadDelegate(readFromFileToBeInvoked);
            m_clearAllRowsDelegate = new ClearDelegate(clearAllRowsToBeInvoked);
            m_clearSomeRowsDelegate = new ClearDelegate(clearSomeRowsToBeInvoked);
            m_updateFilterDelegate = new ClearDelegate(filterRowsToBeInvoked);
            m_maxRows = 50;
            m_logLevel = 0;
            Profiler.ProfileInt(true, "LastState", "LastMaxRows", ref m_maxRows, 50);
            Profiler.ProfileInt(true, "LastState", "LastLogLevel", ref m_logLevel, 0);
            m_cacheErrors = new List<Event>();
            m_cacheWarnings = new List<Event>();
            m_cacheInfos = new List<Event>();
        }

        protected override void Write(Event _event)
        {
            List<Guid> guidsToDelete = new List<Guid>();

            foreach (var kvp in m_efDict)
            {
                try
                {
                    kvp.Value.Forward(_event.Level.Priority, _event.Message, _event.Data as LogExtraData);
                    //LogData.Data.FileLog(Level.Info, String.Format("EXTRALOG forwarded to logger with guid g {0} Dict count {1}", kvp.Key, m_efDict.Count));
                }
                catch (Exception /*ex*/)
                {
                    //LogData.Data.FileLog(Level.Info, String.Format("EXTRALOG failed forwarding  to logger with guid g {0} Dict count {1}, ex: {2} ", kvp.Key, m_efDict.Count,ex.Message));
                    guidsToDelete.Add(kvp.Key);
                }

            }
            EventForwarder ef;
            foreach (Guid g in guidsToDelete)
                m_efDict.TryRemove(g, out ef);
            if (m_efDict.Count > 0 ) return;
            if (m_control != null && m_control.IsHandleCreated && !m_control.IsDisposed)
                m_control.BeginInvoke(m_updateDelegate, new Object[] { _event });
        }

        protected override void Write(Event[] events, int length)
        {
            List<Guid> guidsToDelete = new List<Guid>();
            foreach (var kvp in m_efDict)
            {
                try
                {
                    for (int i = 0; i < length; i++)
                    {
                        Event _event = events[i];
                        kvp.Value.Forward(_event.Level.Priority, _event.Message, _event.Data as LogExtraData);
                        //LogData.Data.FileLog(Level.Info, String.Format("EXTRALOG forwarded to logger with guid g {0} Dict count {1}", kvp.Key, m_efDict.Count));
                    }
                }
                catch (Exception ex)
                {
                    //LogData.Data.FileLog(Level.Info, String.Format("EXTRALOG failed forwarding to logger with guid g {0} Dict count {1}, ex: {2} ", kvp.Key, m_efDict.Count, ex.Message));
                    guidsToDelete.Add(kvp.Key);
                }
            }
            EventForwarder ef;
            foreach (Guid g in guidsToDelete)
                m_efDict.TryRemove(g, out ef);
            if (m_efDict.Count > 0) return;
            if (m_control == null || !m_control.IsHandleCreated || m_control.IsDisposed)
                return;
            for (int i = 0; i < length; i++)
            {
                Event _event = events[i];
                m_control.BeginInvoke(m_updateDelegate, new Object[] { _event });
                string eventmessage = _event.Message;
            }
        }
    }
    
    public class LogData
    {
        private string m_filename;
        public string FileName
        {
            get {
                if (AppState == ApplicationState.CLIENTCONNECTED)
                    return Program.CommunicationObject.Logging_fileName;
                else
                    return m_filename; 
            }
            set 
            {
                m_filename = value;
            }
        }

        public void ClearGrid()
        {
            if (m_data != null && m_data.Logger != null)
            {
                if (m_data.Logger.ChildCount > 0)
                    (m_data.Logger.Children[0] as GridViewLogger).clear();
                else
                    (m_data.Logger as GridViewLogger).clear();
            }
        }

        public int MaxRows
        {
            get 
            {
                if (m_data == null || m_data.Logger == null) return 50;
                if (m_data.Logger.ChildCount > 0)
                    return (m_data.Logger.Children[0] as GridViewLogger).MaxRows;
                else
                    return (m_data.Logger as GridViewLogger).MaxRows;
            }
            set
            {
                if (m_data != null && m_data.Logger != null)
                {
                    if (m_data.Logger.ChildCount > 0)
                        (m_data.Logger.Children[0] as GridViewLogger).MaxRows = value;
                    else
                        (m_data.Logger as GridViewLogger).MaxRows = value;
                }
            }
        }

        //loglevel 0 = all, 1 = warnings,errors, 2 = only errors
        public int LogLevel
        {
            get
            {
                if (m_data == null || m_data.Logger == null) return 0;
                if (m_data.Logger.ChildCount > 0)
                    return (m_data.Logger.Children[0] as GridViewLogger).LogLevel;
                else
                    return (m_data.Logger as GridViewLogger).LogLevel;
            }
            set 
            {
                if (m_data != null && m_data.Logger != null)
                {
                    if (m_data.Logger.ChildCount > 0)
                        (m_data.Logger.Children[0] as GridViewLogger).LogLevel = value;
                    else
                        (m_data.Logger as GridViewLogger).LogLevel = value;
                }
            }
        }


        private Logger m_logger; 
        public Logger Logger
        {
            get {return m_logger;}
            set { m_logger = value; }
        }
        
        //singleton construction
        private LogData()
        {
            m_logger = null; //expect to be set in Logger
        }

        public void Log(Level level, string message, object data)
        {
            if (m_logger != null && m_logger.IsOpen)
            m_logger.Log(level, message, data);
        }

        public void Log(Level level, string message)
        {
            if (m_logger != null && m_logger.IsOpen)
                m_logger.Log(level, message);
        }

        public void FileLog(Level level, string message)
        {
            if (m_fileLogger != null && m_fileLogger.IsOpen)
                m_fileLogger.Log(level, message);
        }

        static public void StopLogger()
        {
            if (m_data != null && m_data.m_logger != null && m_data.m_logger.IsOpen)
                m_data.Logger.Close();
        }

        static private FileLogger m_fileLogger;

        static public void InitializeLogger(DataGridView grid, Control control, ApplicationState appState)
        {
            if (m_data == null)
                m_data = new LogData();

            GridViewLogger gvLogger = new GridViewLogger(grid,control);
            gvLogger.EventFormatter = new Logging.EventFormatters.SimpleEventFormatter();
            gvLogger.Level = Level.Info;
            FileLogger fileLogger=null;
            string VersionLine = null;
            if (appState != ApplicationState.CLIENTCONNECTED)
            {
                string rootPath = Utility.DataPath.Folder(appState);
                string filename = "";
                if(appState == ApplicationState.SERVICE)
                    filename = m_data.m_filename = Path.Combine(rootPath, @"iba\ibaDatCoordinator\ibaDatCoordinatorLog_service.txt");
                else if (appState == ApplicationState.CLIENTDISCONNECTED) //other file name as not to overwrite possible filename from service
                    filename = m_data.m_filename = Path.Combine(rootPath, @"iba\ibaDatCoordinator\ibaDatCoordinatorLog_disconnected.txt");
                else
                    filename = m_data.m_filename = Path.Combine(rootPath, @"iba\ibaDatCoordinator\ibaDatCoordinatorLog.txt");
                if (File.Exists(filename))
                {
                    try
                    {
                        FileBackup.Backup(filename, Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename), 10);
                        File.Delete(filename);
                    }
                    catch (Exception)
                    {
                        rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
                        filename = "";
                        if (appState == ApplicationState.SERVICE)
                            filename = m_data.m_filename = Path.Combine(rootPath, @"iba\ibaDatCoordinator\ibaDatCoordinatorLog_service.txt");
                        else if (appState == ApplicationState.CLIENTDISCONNECTED) //other file name as not to overwrite possible filename from service
                            filename = m_data.m_filename = Path.Combine(rootPath, @"iba\ibaDatCoordinator\ibaDatCoordinatorLog_disconnected.txt");
                        else
                            filename = m_data.m_filename = Path.Combine(rootPath, @"iba\ibaDatCoordinator\ibaDatCoordinatorLog.txt"); ;
                    }
                }

                fileLogger = Logger.CreateFileLogger(filename, "{ts}\t{ln}\t{msg}\t{data}");
                fileLogger.IsBufferingEnabled = false;
                fileLogger.IsContextEnabled = true;
                fileLogger.AutoFlushInterval = 1000;
                fileLogger.BufferSize = 1000;
                fileLogger.Level = Level.All;
                fileLogger.MakeDailyArchive = true;
                fileLogger.MaximumArchiveFiles = 10;
                fileLogger.EventFormatter.DataFormatter = new LogExtraDataFormatter();
                m_fileLogger = fileLogger;

                string logWhat = "";
                switch(appState)
                {
                    case ApplicationState.CLIENTCONNECTED: logWhat = "(client-connected)"; break;
                    case ApplicationState.CLIENTDISCONNECTED: logWhat = "(client-disconnected)"; break;
                    case ApplicationState.CLIENTSTANDALONE: logWhat = "(standalone)"; break;
                    case ApplicationState.SERVICE: logWhat = "(service)"; break;
                }
                VersionLine = "ibaDatCoordinator v" + DatCoVersion.GetVersion() + " " + logWhat;
                fileLogger.DailyString = VersionLine + "\r\n";
                if(Utility.DataPath.IsAdmin)
                {
                    WindowsEventLogger eventLogger = Logger.CreateWindowsEventLogger("ibaDatCoordinator");
                    eventLogger.EventFormatter = new iba.Logging.EventFormatters.PatternEventFormatter("{msg}\t{data}");
                    eventLogger.EventFormatter.DataFormatter = new LogExtraDataFormatter();
                    eventLogger.IsBufferingEnabled = false;
                    eventLogger.IsContextEnabled = true;
                    eventLogger.Level = Level.Info;
                    m_data.Logger = Logger.CreateCompositeLogger(gvLogger, fileLogger, eventLogger);
                }
                else //no admin, no event logger
                    m_data.Logger = Logger.CreateCompositeLogger(gvLogger, fileLogger);
            }
            else
            {
                m_data.Logger = gvLogger;
            }
            m_data.AppState = appState;
            m_data.Logger.IsBufferingEnabled = true;
            m_data.Logger.IsContextEnabled = true;
            m_data.Logger.AutoFlushInterval = 1000;
            m_data.Logger.BufferSize = 1000;
            m_data.Logger.Level = Level.All;
            try
            {
                m_data.Logger.Open();
                if (fileLogger != null && VersionLine != null) fileLogger.Log(VersionLine); //so it is inserted in the first logfile
            }
            catch
            {
                m_data.Logger = null;
            }
        }

        private ApplicationState m_appState;
        public ApplicationState AppState
        {
            get { return m_appState; }
            set { m_appState = value; }
        }

        static private LogData m_data = null;
        static LogData()
        {
            m_data = new LogData();
        }
        static public LogData Data
        {
            get 
            {
                return m_data;
            }
        }
    }
}
