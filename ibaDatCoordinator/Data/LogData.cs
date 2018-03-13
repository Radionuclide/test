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
    [System.Reflection.Obfuscation]
    public class LogExtraData
    {
        private string m_datFile;
        public string DatFile
        {
            get { return m_datFile; }
        }

        private string m_configName;
        public string ConfigurationName
        {
            get { return m_configName; }
        }

        private string m_taskName;
        public string TaskName
        {
            get { return m_taskName; }
        }

        public LogExtraData(string datFile, TaskData task, ConfigurationData cd)
        {
            if(cd != null && cd.JobType == ConfigurationData.JobTypeEnum.Scheduled && !String.IsNullOrEmpty(datFile))
                m_datFile = cd.CreateHDQFileDescription(datFile);
            else
                m_datFile = datFile;

            m_configName = cd != null ? cd.Name : "";
            m_taskName = task != null ? task.Name : "";
        }
    }

    class LogExtraDataFormatter : DataFormatter
    {
        private static string EmptyResult = "[]\t[]\t[]";
        public override string Format(object o)
        {
            LogExtraData ld = o as LogExtraData;
            if (ld == null)
                return EmptyResult;

            string filename = "";
            if (ld.DatFile != null)
                filename = ld.DatFile;

            return "[" + ld.ConfigurationName + "]\t[" + ld.TaskName + "]\t[" + filename + "]";
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

    class EventForwarder : MarshalByRefObject, IEventForwarder, IEquatable<EventForwarder>, IDisposable
    {
        #region IEventForwarder implementation

        public void Forward(DatCoEventData[] events)
        {
            try
            {
                LogData.GridViewLogger?.Write(events);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Lifetime

        public override object InitializeLifetimeService()
        {
            //Use an infinite lifetime for remoting. The object will be removed from remoting via the Dispose call
            return null;
        }

        public void Dispose()
        {
            System.Runtime.Remoting.RemotingServices.Disconnect(this);
        }

        #endregion

        //public void ForwardClearCommand()
        //{
        //    (LogData.Data.Logger as GridViewLogger).clear(); 
        //}

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
   
    public class LogData
    {
        private string m_filename;
        public string FileName
        {
            get
            {
                return m_filename; 
            }
            set 
            {
                m_filename = value;
            }
        }

        public void ClearGrid()
        {
            if (m_gvLogger != null)
                m_gvLogger.Clear();
        }

        public int MaxRows
        {
            get 
            {
                if (m_gvLogger != null)
                    return m_gvLogger.MaxRows;
                else
                    return 50;
            }
            set
            {
                if (m_gvLogger != null)
                    m_gvLogger.MaxRows = value;
            }
        }

        //loglevel 0 = all, 1 = warnings,errors, 2 = only errors
        public int LogLevel
        {
            get
            {
                if (m_gvLogger != null)
                    return m_gvLogger.LogLevel;
                else
                    return 0;
            }
            set 
            {
                if (m_gvLogger != null)
                    m_gvLogger.LogLevel = value;
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

        static public void StopLogger()
        {
            if (m_data.m_logger != null && m_data.m_logger.IsOpen)
                m_data.Logger.Close();

            //Close gridview logger separately for client
            if ((m_gvLogger != null) && m_gvLogger.IsOpen)
                m_gvLogger.Close();
        }

        static private FileLogger m_fileLogger;
        static private RemoteLogger m_remoteLogger;
        static private GridViewLogger m_gvLogger;

        static internal RemoteLogger RemoteLogger
        {
            get { return m_remoteLogger; }
        }

        static internal GridViewLogger GridViewLogger
        {
            get { return m_gvLogger; }
        }

        static public void InitializeServerLogger()
        {
            string versionLine = "ibaDatCoordinator v" + DatCoVersion.GetVersion() + " (service)";

            string rootPath = Utility.DataPath.Folder(ApplicationState.SERVICE);
            string filename = m_data.m_filename = Path.Combine(rootPath, "ibaDatCoordinatorLog_service.txt");
            FileBackup.Backup(filename, Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename), 10, 10);

            //File logger
            FileLogger fileLogger = Logger.CreateFileLogger(filename, "{ts}\t{ln}\t{msg}\t{data}");
            fileLogger.IsBufferingEnabled = false;
            fileLogger.IsContextEnabled = true;
            fileLogger.AutoFlushInterval = 1000;
            fileLogger.BufferSize = 1000;
            fileLogger.Level = Level.All;
            fileLogger.MakeDailyArchive = true;
            fileLogger.MaximumArchiveFiles = 10;
            fileLogger.MaximumZipFiles = 10;
            fileLogger.DailyString = versionLine;
            fileLogger.EventFormatter.DataFormatter = new LogExtraDataFormatter();
            m_fileLogger = fileLogger;

            //Logger that forwards events to the clients to display in their logging gridview
            RemoteLogger remoteLogger = new iba.Data.RemoteLogger();
            remoteLogger.EventFormatter = new Logging.EventFormatters.SimpleEventFormatter();
            remoteLogger.IsBufferingEnabled = false;
            remoteLogger.Level = Level.Info;
            m_remoteLogger = remoteLogger;

            //Windows event log logger
            if (Utility.DataPath.IsAdmin)
            {
                WindowsEventLogger eventLogger = Logger.CreateWindowsEventLogger("ibaDatCoordinator");
                eventLogger.EventFormatter = new iba.Logging.EventFormatters.PatternEventFormatter("{msg}\t{data}");
                eventLogger.EventFormatter.DataFormatter = new LogExtraDataFormatter();
                eventLogger.IsBufferingEnabled = false;
                eventLogger.IsContextEnabled = true;
                eventLogger.Level = Level.Info;

                m_data.Logger = Logger.CreateCompositeLogger(fileLogger, remoteLogger, eventLogger);
            }
            else
                m_data.Logger = Logger.CreateCompositeLogger(fileLogger, remoteLogger);

            m_data.Logger.IsBufferingEnabled = true;
            m_data.Logger.IsContextEnabled = true;
            m_data.Logger.AutoFlushInterval = 1000;
            m_data.Logger.BufferSize = 1000;
            m_data.Logger.Level = Level.All;
            try
            {
                m_data.Logger.Open();
                ibaLogger.Logger = m_data.Logger;

                m_data.Logger.Log(Level.Debug, versionLine); //so it is inserted in the first logfile
            }
            catch
            {
                m_data.Logger = null;
            }
        }

        static public void InitializeClientLogger(DataGridView grid, Control control)
        {
            string versionLine = "ibaDatCoordinator v" + DatCoVersion.GetVersion() + " (client)";

            string rootPath = Utility.DataPath.Folder(ApplicationState.CLIENTDISCONNECTED);
            string filename = m_data.m_filename = Path.Combine(rootPath, "ibaDatCoordinatorLog_client.txt");
            FileBackup.Backup(filename, Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename), 10, 10);

            //File logger
            FileLogger fileLogger = Logger.CreateFileLogger(filename, "{ts}\t{ln}\t{msg}\t{data}");
            fileLogger.IsBufferingEnabled = true;
            fileLogger.IsContextEnabled = true;
            fileLogger.AutoFlushInterval = 1000;
            fileLogger.BufferSize = 1000;
            fileLogger.Level = Level.All;
            fileLogger.MakeDailyArchive = true;
            fileLogger.MaximumArchiveFiles = 10;
            fileLogger.MaximumZipFiles = 10;
            fileLogger.DailyString = versionLine;
            fileLogger.EventFormatter.DataFormatter = new LogExtraDataFormatter();
            m_fileLogger = fileLogger;

            m_data.Logger = m_fileLogger;

            try
            {
                m_data.Logger.Open();
                ibaLogger.Logger = m_data.Logger;

                m_fileLogger.Log(versionLine); //so it is inserted in the first logfile
            }
            catch
            {
                m_data.Logger = null;
            }

            //Create a gridview logger that is only called directly from the event forwarder. 
            //The gridview will only show messages coming from the server. No client messages are shown!
            m_gvLogger = new iba.Data.GridViewLogger(grid, control);
            m_gvLogger.EventFormatter = new Logging.EventFormatters.SimpleEventFormatter();
            m_gvLogger.IsBufferingEnabled = false;
            m_gvLogger.Level = Level.Info;
            m_gvLogger.Open();
        }

        static public void InitializeStandAloneLogger(DataGridView grid, Control control)
        {
            string versionLine = "ibaDatCoordinator v" + DatCoVersion.GetVersion() + " (standalone)";

            string rootPath = Utility.DataPath.Folder(ApplicationState.CLIENTSTANDALONE);
            string filename = m_data.m_filename = Path.Combine(rootPath, "ibaDatCoordinatorLog.txt");
            FileBackup.Backup(filename, Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename), 10, 10);

            //File logger
            FileLogger fileLogger = Logger.CreateFileLogger(filename, "{ts}\t{ln}\t{msg}\t{data}");
            fileLogger.IsBufferingEnabled = false;
            fileLogger.IsContextEnabled = true;
            fileLogger.AutoFlushInterval = 1000;
            fileLogger.BufferSize = 1000;
            fileLogger.Level = Level.All;
            fileLogger.MakeDailyArchive = true;
            fileLogger.MaximumArchiveFiles = 10;
            fileLogger.MaximumZipFiles = 10;
            fileLogger.DailyString = versionLine;
            fileLogger.EventFormatter.DataFormatter = new LogExtraDataFormatter();
            m_fileLogger = fileLogger;

            //Gridview logger that shows log messages in (local) gridview
            GridViewLogger gvLogger = new iba.Data.GridViewLogger(grid, control);
            gvLogger.EventFormatter = new Logging.EventFormatters.SimpleEventFormatter();
            gvLogger.IsBufferingEnabled = false;
            gvLogger.Level = Level.Info;
            m_gvLogger = gvLogger;
 
            //Windows event log logger
            if (Utility.DataPath.IsAdmin)
            {
                WindowsEventLogger eventLogger = Logger.CreateWindowsEventLogger("ibaDatCoordinator");
                eventLogger.EventFormatter = new iba.Logging.EventFormatters.PatternEventFormatter("{msg}\t{data}");
                eventLogger.EventFormatter.DataFormatter = new LogExtraDataFormatter();
                eventLogger.IsBufferingEnabled = false;
                eventLogger.IsContextEnabled = true;
                eventLogger.Level = Level.Info;

                m_data.Logger = Logger.CreateCompositeLogger(fileLogger, gvLogger, eventLogger);
            }
            else
                m_data.Logger = Logger.CreateCompositeLogger(fileLogger, gvLogger);

            m_data.Logger.IsBufferingEnabled = true;
            m_data.Logger.IsContextEnabled = true;
            m_data.Logger.AutoFlushInterval = 1000;
            m_data.Logger.BufferSize = 1000;
            m_data.Logger.Level = Level.All;
            try
            {
                m_data.Logger.Open();
                ibaLogger.Logger = m_data.Logger;

                m_fileLogger.Log(versionLine); //so it is inserted in the first logfile
            }
            catch
            {
                m_data.Logger = null;
            }
        }

        static private LogData m_data;

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
