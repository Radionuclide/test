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
            m_datFile = datFile;
            m_cd = cd;
            m_task = task;
        }
    }

    public class LogExtraDataFormatter : DataFormatter
    {
        private static string EmptyResult = "[]\t[]\t[]" + Environment.NewLine;
        public override string Format(object o)
        {
            if (o == null) return EmptyResult;
            LogExtraData ld = o as LogExtraData;
            if (ld == null) return EmptyResult;
            string taskname = "";
            if (ld.TaskData != null) taskname = ld.TaskData.Name;
            string confname = "";
            if (ld.ConfigurationData != null) confname = ld.ConfigurationData.Name;
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

    public class EventForwarder : MarshalByRefObject
    {
        public void Forward(int priority, string message, LogExtraData dat)
        {
            try
            {
                LogData.Data.Logger.Log(Level.GetLevel(priority), message, dat);
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

        public bool ForwardReadFromFileCommand(string filename)
        {
            return (LogData.Data.Logger as GridViewLogger).readFromFile(filename); 
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

        private EventForwarder m_ef;
        public EventForwarder Forwarder
        {
            get { return m_ef; }
            set { m_ef = value; }
        }

        private bool m_isForwarding;
        private bool m_isForwarding2;
        public bool IsForwarding
        {
            get { return m_isForwarding2; }
            set 
            {   m_isForwarding = value;
                if (value) m_isForwarding2 = true;    
            }
        }

        private delegate void UpdateDelegate(Event _event);
        private delegate bool ReadDelegate(string filename);
        private delegate void ClearDelegate();

        private UpdateDelegate m_updateDelegate;
        private ReadDelegate m_readDelegate;
        private ClearDelegate m_clearAllRowsDelegate;
        private ClearDelegate m_clearSomeRowsDelegate;

        private int m_maxRows;
        public int MaxRows
        {
            get { return m_maxRows; }
            set
            {
                m_maxRows = value;
                Profiler.ProfileInt(false, "LastState", "LastMaxRows", ref m_maxRows, 50);
                if (m_control != null) //gui present
                    if (m_grid.Rows.Count > m_maxRows) m_control.BeginInvoke(m_clearSomeRowsDelegate);
            }
        }

        private void update(Event _event)
        {
            int index;

            LogControl lc = (m_control as LogControl);
            
            bool freeze = lc!= null && lc.Freeze;

            int rowpos = freeze ? m_grid.FirstDisplayedScrollingRowIndex:-1;

            while (m_grid.Rows.Count >= m_maxRows)
            {
                m_grid.Rows.RemoveAt(0);
                rowpos -= 1;
            }
            
            LogExtraData data = _event.Data as LogExtraData;
            lock (m_grid.Rows)
            {
                if (data == null)
                    index = m_grid.Rows.Add(new Object[] { 
                    _event.Timestamp, 
                    String.Empty,
                    String.Empty, 
                    String.Empty, 
                    _event.Message });
                else
                    index = m_grid.Rows.Add(new Object[] { 
                     _event.Timestamp, 
                    data.ConfigurationData==null?String.Empty:data.ConfigurationData.Name, 
                    data.TaskData == null?String.Empty:data.TaskData.Name, 
                    data.DatFile, 
                    _event.Message });
                DataGridViewCellStyle style = m_grid.Rows[index].Cells[4].Style;
                if (_event.Level == Logging.Level.Warning) style.ForeColor = Color.Orange;
                else if (_event.Level == Logging.Level.Info)
                {
                    style.ForeColor = Color.Green;
                    Program.MainForm.StatusBarLabel.Text = "";
                }
                else if (_event.Level == Logging.Level.Exception)
                {
                    style.ForeColor = Color.Red;
                    string text = _event.Message;
                    int tindex = text.IndexOf(Environment.NewLine);
                    if (tindex >= 0)
                        text = text.Remove(tindex);
                    Program.MainForm.StatusBarLabel.Text = String.Format(iba.Properties.Resources.StatusBarError, text);
                }
                if (!freeze)
                {
                    m_grid.Rows[index].Selected = true;
                    if (lc != null) lc.Freeze = false;
                }
            }
            try
            {
                if (freeze && rowpos > 0)
                    m_grid.FirstDisplayedScrollingRowIndex = rowpos;
            }
            catch
            {
            }
        }

        private void clearAllRowsToBeInvoked()
        {
            lock (m_grid.Rows)
            {
                m_grid.Rows.Clear();
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

        private bool readFromFileToBeInvoked(string filename)
        {
            List<List<string>> readEvents = new List<List<string>>();
            try
            {
                FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (StreamReader logfile = new StreamReader(stream))
                {
                    char[] sep = { '\t' };
                    char[] totrim = {'[',']'};
                    string str;
                    while ((str = logfile.ReadLine()) != null)
                    {
                        if (String.IsNullOrEmpty(str)) continue;
                        string[] pieces = str.Split(sep);
                        pieces[3] = pieces[3].Trim(totrim);
                        pieces[4] = pieces[4].Trim(totrim);
                        pieces[5] = pieces[5].Trim(totrim);
                        readEvents.Add(new List<string>(pieces));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(iba.Properties.Resources.OpenFileProblem + " " + ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            lock (m_grid.Rows)
            {
                m_grid.Rows.Clear();
                int index = 0;
                foreach (List<string> readEvent in readEvents)
                {
                    while (m_grid.Rows.Count >= m_maxRows)
                        m_grid.Rows.RemoveAt(0);
                    index = m_grid.Rows.Add(new Object[] { readEvent[0], readEvent[3], readEvent[4], readEvent[5], readEvent[2]});
                    DataGridViewCellStyle style = m_grid.Rows[index].Cells[4].Style;
                    if (readEvent[1] == Logging.Level.Warning.ToString()) style.ForeColor = Color.Orange;
                    else if (readEvent[1] == Logging.Level.Info.ToString()) style.ForeColor = Color.Green;
                    else if (readEvent[1] == Logging.Level.Exception.ToString()) style.ForeColor = Color.Red;
                }
                if (m_grid.Rows.Count > index)
                    m_grid.Rows[index].Selected=true;
            }
            return true;
        }

        public bool readFromFile(string filename)
        {
            if (m_control == null && (m_ef == null || !m_isForwarding)) //we are at te server side, but there is no client to forward to
            {
                return true;
            }
            bool result = false;
            if (m_ef != null && m_isForwarding)
            {
                result = m_ef.ForwardReadFromFileCommand(filename);
            }
            m_isForwarding2 = m_isForwarding; //ok to close GUI
            if (m_ef != null) return result;
            if (m_control.InvokeRequired)
                return (bool)m_control.Invoke(m_readDelegate, new object[] { filename });
            else
                return readFromFileToBeInvoked(filename);
        }

        public void clear()
        {
            if (m_ef != null && m_isForwarding)
            {
                m_ef.ForwardClearCommand();
            }
            m_isForwarding2 = m_isForwarding; //ok to close GUI
            if (m_ef != null) return;
            if (m_control != null) //gui present
                m_control.BeginInvoke(m_clearAllRowsDelegate);
        }

        public GridViewLogger(DataGridView grid, Control control)
        {
            m_grid = grid;
            m_control = control;
            m_updateDelegate = new UpdateDelegate(update);
            m_readDelegate = new ReadDelegate(readFromFileToBeInvoked);
            m_clearAllRowsDelegate = new ClearDelegate(clearAllRowsToBeInvoked);
            m_clearSomeRowsDelegate = new ClearDelegate(clearSomeRowsToBeInvoked);
            m_maxRows = 50;
            Profiler.ProfileInt(true, "LastState", "LastMaxRows", ref m_maxRows, 50);
            m_isForwarding = m_isForwarding2 = false;
        }

        protected override void Write(Event _event)
        {
            if (m_ef != null && m_isForwarding)
            {
                try
                {
                    m_ef.Forward(_event.Level.Priority, _event.Message, _event.Data as LogExtraData);
                    m_isForwarding2 = m_isForwarding; //ok to close GUI
                }
                catch (Exception)
                {
                    m_isForwarding2 = m_isForwarding = false;
                    m_ef = null;
                }
            }
            m_isForwarding2 = m_isForwarding; //ok to close GUI
            if (m_ef != null) return;
            if (m_control != null && m_control.IsHandleCreated && !m_control.IsDisposed)
                m_control.BeginInvoke(m_updateDelegate, new Object[] { _event });
        }

        protected override void Write(Event[] events, int length)
        {
            if (m_ef != null && m_isForwarding )
            {
                try
                {
                    for (int i = 0; i < length; i++)
                    {
                        Event _event = events[i];
                        m_ef.Forward(_event.Level.Priority, _event.Message, _event.Data as LogExtraData);
                    }
                }
                catch (Exception)
                {
                    m_isForwarding2 = m_isForwarding = false;
                    m_ef = null;
                }
            }
            m_isForwarding2 = m_isForwarding; //ok to close GUI
            if (m_ef != null) return;
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

        public int MaxRows
        {
            get 
            {
                if (m_data.Logger.ChildCount > 0)
                    return (m_data.Logger.Children[0] as GridViewLogger).MaxRows;
                else
                    return (m_data.Logger as GridViewLogger).MaxRows;
            }
            set
            {
                if (m_data.Logger.ChildCount > 0)
                    (m_data.Logger.Children[0] as GridViewLogger).MaxRows = value;
                else
                    (m_data.Logger as GridViewLogger).MaxRows = value;
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

        static public void StopLogger()
        {
            m_data.Logger.Close();
        }

        public enum ApplicationState {CLIENTDISCONNECTED,CLIENTCONNECTED,CLIENTSTANDALONE, SERVICE};

        static public void InitializeLogger(DataGridView grid, Control control, ApplicationState appState)
        {
            if (m_data == null)
                m_data = new LogData();

            GridViewLogger gvLogger = new GridViewLogger(grid,control);
            gvLogger.EventFormatter = new Logging.EventFormatters.SimpleEventFormatter();
            gvLogger.Level = Level.All;

            if (appState != ApplicationState.CLIENTCONNECTED)
            {
                string rootPath = Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
                string filename = "";
                if (appState == ApplicationState.CLIENTDISCONNECTED) //other file name as not to overwrite possible filename from service
                    filename = m_data.m_filename = Path.Combine(rootPath, @"iba\ibaDatCoordinator\ibaDatCoordinaterLog_disconnected.txt");
                else
                    filename = m_data.m_filename = Path.Combine(rootPath, @"iba\ibaDatCoordinator\ibaDatCoordinaterLog.txt");;
                FileBackup.Backup(filename, Path.GetDirectoryName(filename), appState == ApplicationState.CLIENTDISCONNECTED?"ibaDatCoordinatorLog_disconnected":"ibaDatCoordinatorLog", 10);
                FileLogger fileLogger = Logger.CreateFileLogger(filename, "{ts}\t{ln}\t{msg}\t{data}");
                fileLogger.IsBufferingEnabled = false;
                fileLogger.IsContextEnabled = true;
                fileLogger.AutoFlushInterval = 1000;
                fileLogger.BufferSize = 1000;
                fileLogger.Level = Level.All;
                fileLogger.MakeDailyArchive = true;
                fileLogger.MaximumArchiveFiles = 10;
                fileLogger.EventFormatter.DataFormatter = new LogExtraDataFormatter();

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
            m_data.Logger.Open();
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
