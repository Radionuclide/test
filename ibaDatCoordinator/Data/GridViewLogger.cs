using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using iba.Logging;
using iba.Utility;
using iba.Controls;
using System.Drawing;

namespace iba.Data
{
    class GridViewLogger : Logger
    {
        public GridViewLogger(DataGridView grid, Control control)
        {
            m_grid = grid;
            m_control = control;
            m_updateDelegate = new UpdateDelegate(update);
            m_updateArrayDelegate = new UpdateArrayDelegate(updateArray);
            //m_readDelegate = new ReadDelegate(readFromFileToBeInvoked);
            m_clearAllRowsDelegate = new ClearDelegate(clearAllRowsToBeInvoked);
            m_clearSomeRowsDelegate = new ClearDelegate(clearSomeRowsToBeInvoked);
            m_updateFilterDelegate = new ClearDelegate(filterRowsToBeInvoked);

            m_maxRows = 50;
            m_logLevel = 0;
            DatCoProfiler.ProfileInt(true, "LastState", "LastMaxRows", ref m_maxRows, 50);
            DatCoProfiler.ProfileInt(true, "LastState", "LastLogLevel", ref m_logLevel, 0);

            m_cacheErrors = new List<Event>();
            m_cacheWarnings = new List<Event>();
            m_cacheInfos = new List<Event>();
        }

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

        private delegate void UpdateDelegate(Event _event);
        private delegate void UpdateArrayDelegate(DatCoEventData[] events);
        private delegate bool ReadDelegate(string filename);
        private delegate void ClearDelegate();

        private UpdateDelegate m_updateDelegate;
        private UpdateArrayDelegate m_updateArrayDelegate;
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
                m_maxRows = Math.Max(1, value);
                DatCoProfiler.ProfileInt(false, "LastState", "LastMaxRows", ref m_maxRows, 50);
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
                DatCoProfiler.ProfileInt(false, "LastState", "LastLogLevel", ref m_logLevel, 0);
                if (m_control != null) //gui present
                    if (prevLevel != m_logLevel) m_control.BeginInvoke(m_updateFilterDelegate);
            }
        }

        private void update(Event _event)
        {
            CacheEvent(_event);

            if ((_event.Level == Logging.Level.Info && m_logLevel > 0) || (_event.Level == Logging.Level.Warning && m_logLevel > 1))
                return;

            lock (m_grid.Rows)
            {
                updateNonLocked(_event);
            }
        }

        private void updateArray(DatCoEventData[] events)
        {
            lock (m_grid.Rows)
            {
                foreach (DatCoEventData datCoEvent in events)
                {
                    Event ev = new Event(datCoEvent.Time, Level.GetLevel(datCoEvent.Level), datCoEvent.Message, datCoEvent.Data, "", "");
                    CacheEvent(ev);

                    if ((ev.Level == Logging.Level.Info && m_logLevel > 0) || (ev.Level == Logging.Level.Warning && m_logLevel > 1))
                        continue;

                    updateNonLocked(ev);
                }
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
            {
                m_grid.Rows.Insert(0, new Object[] {
                    _event.Timestamp,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    _event.Message });
            }
            else
            {
                m_grid.Rows.Insert(0, new Object[] {
                     _event.Timestamp,
                    data.ConfigurationName,
                    data.TaskName,
                    data.DatFile,
                    _event.Message });
            }
            rowpos++;

            DataGridViewCellStyle style = m_grid.Rows[0].Cells[4].Style;
            if (_event.Level == Logging.Level.Warning)
                style.ForeColor = Color.Orange;
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
                if (m_logLevel <= 1)
                    merged.AddRange(m_cacheWarnings);
                if (m_logLevel == 0)
                    merged.AddRange(m_cacheInfos);
                merged.Sort(delegate (Event e1, Event e2) { return e1.Timestamp.CompareTo(e2.Timestamp); });
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

        public void Clear()
        {
            if (m_control != null) //gui present
                m_control.BeginInvoke(m_clearAllRowsDelegate);
        }

        protected override void Write(Event _event)
        {
            try
            {
                if (m_control != null && m_control.IsHandleCreated && !m_control.IsDisposed)
                    m_control.BeginInvoke(m_updateDelegate, new Object[] { _event });
            }
            catch(Exception)
            { }
        }

        protected override void Write(Event[] events, int length)
        {
            try
            {
                if (m_control != null && m_control.IsHandleCreated && !m_control.IsDisposed)
                {
                    for (int i = 0; i < length; i++)
                    {
                        Event _event = events[i];
                        m_control.BeginInvoke(m_updateDelegate, new Object[] { _event });
                    }
                }
            }
            catch(Exception)
            { }
        }

        internal void Write(DatCoEventData[] events)
        {
            try
            {
                if (m_control != null && m_control.IsHandleCreated && !m_control.IsDisposed)
                    m_control.BeginInvoke(m_updateArrayDelegate, new object[] { events });
            }
            catch(Exception)
            { }
        }


    }
}

