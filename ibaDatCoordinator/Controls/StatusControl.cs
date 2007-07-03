using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.IO;
using iba;
using iba.Data;
using iba.Utility;
using iba.Processing;
using iba.Plugins;

namespace iba.Controls
{
    public partial class StatusControl : UserControl, IPropertyPane
    {
        public StatusControl()
        {
            InitializeComponent();
            InitializeIcons();
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        StatusData m_data;

        private enum TaskChoice {REPORT,EXTRACT,BATCHFILE}

        Dictionary<DatFileStatus.State, Bitmap> m_reportIcons, m_extractIcons, m_batchfileIcons, m_copydatIcons, m_conditionIcons;
        Dictionary<DatFileStatus.State, Bitmap>[] m_customtaskIcons;
        
        Dictionary<DatFileStatus.State, String> m_taskTexts;
        
        Bitmap m_blankIcon;

        private void InitializeIcons()
        {
            m_reportIcons = new Dictionary<DatFileStatus.State, Bitmap>();
            m_extractIcons = new Dictionary<DatFileStatus.State, Bitmap>();
            m_batchfileIcons = new Dictionary<DatFileStatus.State, Bitmap>();
            m_copydatIcons = new Dictionary<DatFileStatus.State, Bitmap>();
            m_conditionIcons = new Dictionary<DatFileStatus.State, Bitmap>();
            m_taskTexts = new Dictionary<DatFileStatus.State, String>();


            m_customtaskIcons = new Dictionary<DatFileStatus.State, Bitmap>[PluginManager.Manager.PluginInfos.Count];
            for (int i = 0; i < m_customtaskIcons.Length; i++)
                m_customtaskIcons[i] = new Dictionary<DatFileStatus.State, Bitmap>();


            Bitmap blankBitmap = Bitmap.FromHicon(iba.Properties.Resources.blank.Handle);

            m_reportIcons.Add(DatFileStatus.State.NOT_STARTED, blankBitmap);
            m_reportIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle));
            m_reportIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS,Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));
            m_reportIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));
            m_reportIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));
            m_reportIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));
            m_reportIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));
            m_reportIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));

            m_extractIcons.Add(DatFileStatus.State.NOT_STARTED, blankBitmap);
            m_extractIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle));
            m_extractIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));
            m_extractIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));
            m_extractIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));
            m_extractIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));
            m_extractIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));
            m_extractIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));

            m_batchfileIcons.Add(DatFileStatus.State.NOT_STARTED, blankBitmap);
            m_batchfileIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle));
            m_batchfileIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));
            m_batchfileIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));
            m_batchfileIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));
            m_batchfileIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));
            m_batchfileIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));
            m_batchfileIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));

            m_copydatIcons.Add(DatFileStatus.State.NOT_STARTED, blankBitmap);
            m_copydatIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle));
            m_copydatIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));
            m_copydatIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));
            m_copydatIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));
            m_copydatIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));
            m_copydatIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));
            m_copydatIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));

            m_conditionIcons.Add(DatFileStatus.State.NOT_STARTED, blankBitmap);
            m_conditionIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle));
            m_conditionIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));
            m_conditionIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));
            m_conditionIcons.Add(DatFileStatus.State.COMPLETED_TRUE, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle))); 
            m_conditionIcons.Add(DatFileStatus.State.COMPLETED_FALSE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));
            m_conditionIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));
            m_conditionIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));
            m_conditionIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));

            for (int i = 0; i < m_customtaskIcons.Length; i++)
            {
                IntPtr handle = PluginManager.Manager.PluginInfos[i].Icon.Handle;
                m_customtaskIcons[i].Add(DatFileStatus.State.NOT_STARTED, blankBitmap);
                m_customtaskIcons[i].Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(handle));
                m_customtaskIcons[i].Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(handle)));
                m_customtaskIcons[i].Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(handle)));
                m_customtaskIcons[i].Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(handle)));
                m_customtaskIcons[i].Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(handle)));
                m_customtaskIcons[i].Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(handle)));
                m_customtaskIcons[i].Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(handle)));
            }


            m_taskTexts.Add(DatFileStatus.State.NOT_STARTED, String.Empty);
            m_taskTexts.Add(DatFileStatus.State.RUNNING, iba.Properties.Resources.Running);
            m_taskTexts.Add(DatFileStatus.State.NO_ACCESS, iba.Properties.Resources.Noaccess);
            m_taskTexts.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, iba.Properties.Resources.Success);
            m_taskTexts.Add(DatFileStatus.State.COMPLETED_FAILURE, iba.Properties.Resources.Failure);
            m_taskTexts.Add(DatFileStatus.State.TIMED_OUT, iba.Properties.Resources.Timeout);
            m_taskTexts.Add(DatFileStatus.State.MEMORY_EXCEEDED, iba.Properties.Resources.MemoryExceededTaskText);
            m_taskTexts.Add(DatFileStatus.State.COMPLETED_FALSE, iba.Properties.Resources.logIfTaskEvaluatedFalse);
            m_taskTexts.Add(DatFileStatus.State.COMPLETED_TRUE, iba.Properties.Resources.logIfTaskEvaluatedTrue);
            m_taskTexts.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, iba.Properties.Resources.AttemptsExceeded);
            m_blankIcon = Bitmap.FromHicon(iba.Properties.Resources.blank.Handle);
        
        }

        private Bitmap MergeIcons(DatFileStatus.State stat, Bitmap original)
        {
            Bitmap overlayBitmap = null;
            switch (stat)
            {
                case DatFileStatus.State.COMPLETED_FAILURE:
                    overlayBitmap = Bitmap.FromHicon(iba.Properties.Resources.error.Handle);
                    break;
                case DatFileStatus.State.COMPLETED_SUCCESFULY:
                    overlayBitmap = Bitmap.FromHicon(iba.Properties.Resources.success1.Handle);
                    break;
                case DatFileStatus.State.NO_ACCESS:
                    overlayBitmap = Bitmap.FromHicon(iba.Properties.Resources.noaccess1.Handle);
                    break;
                case DatFileStatus.State.TIMED_OUT:
                    overlayBitmap = Bitmap.FromHicon(iba.Properties.Resources.timeout1.Handle);
                    break;
                case DatFileStatus.State.MEMORY_EXCEEDED:
                    overlayBitmap = Bitmap.FromHicon(iba.Properties.Resources.memoryexceeded.Handle);
                    break;
                case DatFileStatus.State.TRIED_TOO_MANY_TIMES:
                    overlayBitmap = Bitmap.FromHicon(iba.Properties.Resources.tomanytimestried.Handle);
                    break;
            }
            Bitmap combinedBitmap = (Bitmap) original.Clone();
            Graphics g = Graphics.FromImage(combinedBitmap);
            g.DrawImageUnscaled(overlayBitmap, 0, 0);
            g.Dispose();
            return combinedBitmap;
        }

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as StatusData;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED) //refresh
                m_data = TaskManager.Manager.GetStatus(m_data.CorrConfigurationData.Guid);

            m_confNameLinkLabel.Text = m_data.CorrConfigurationData.Name;
            int count = m_data.CorrConfigurationData.Tasks.Count;
            if (m_gridView.ColumnCount != (count + 2))
            {
                m_gridView.ColumnCount = 2;
                DataGridViewImageColumn [] cols = new DataGridViewImageColumn[count];
                for(int i = 0; i < cols.Length;i++) cols[i] = new DataGridViewImageColumn();
                m_gridView.Columns.AddRange(cols);
            }
            foreach (TaskData task in m_data.CorrConfigurationData.Tasks)
                m_gridView.Columns[task.Index + 2].HeaderText = task.Name;

            OnChangedData(null, null);
            m_refreshTimer.Start();
        }

        public void LeaveCleanup() {
            m_refreshTimer.Stop();
        }

        public void SaveData()
        {
            //nothing to be saved as status is purely an output control
        }

        public void OnChangedData(object sender, EventArgs e)
        {
            m_refreshTimer.Enabled = false;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED) //refresh
            {
                try
                {
                    m_data = TaskManager.Manager.GetStatusCopy(m_data.CorrConfigurationData.Guid);
                }
                catch (SocketException) //shouldn't happen but just in case
                {
                    m_refreshTimer.Enabled = true;
                    return;
                }
            }
            if (!m_data.Changed && sender != null)
            {
                m_refreshTimer.Enabled = true;
                return;
            }
            m_data.Changed = false;
            //wait cursor
            if (m_data.UpdatingFileList) this.Cursor = Cursors.WaitCursor;
            else this.Cursor = Cursors.Default;
            string text = String.Format(iba.Properties.Resources.statText1, m_data.Started ? iba.Properties.Resources.statText2 : iba.Properties.Resources.statText3) + Environment.NewLine;
            //text += "processedFiles: " + m_data.ProcessedFiles.Count.ToString() + Environment.NewLine;
            //text += "readFiles: " + m_data.ReadFiles.Count.ToString() + Environment.NewLine;
            m_infoText.Text = text;
            SortedDictionary<DateTime, KeyValuePair<string, DatFileStatus>> contents = new SortedDictionary<DateTime, KeyValuePair<string, DatFileStatus>>();
            lock (m_data.ProcessedFilesCopy)
            {
                foreach (string file in m_data.ProcessedFilesCopy)
                    lock (m_data.DatFileStates)
                    {
                        try
                        {
                            if (File.Exists(file))
                            {
                                FileInfo f = new FileInfo(file);
                                DatFileStatus dfs = null;
                                if (m_data.DatFileStates.ContainsKey(file))
                                    dfs = (DatFileStatus)m_data.DatFileStates[file].Clone();
                                contents[f.LastWriteTime] =
                                    new KeyValuePair<string, DatFileStatus>(file, dfs);
                            }
                        }
                        catch //if network disconnection shouldh happen
                        {
                        }
                    }
            }
            lock (m_data.ReadFilesCopy)
            {
                foreach (string file in m_data.ReadFilesCopy)
                    lock (m_data.DatFileStates)
                    {
                        try
                        {
                            if (File.Exists(file))
                            {
                                FileInfo f = new FileInfo(file);
                                DatFileStatus dfs = null;
                                if (m_data.DatFileStates.ContainsKey(file))
                                    dfs = (DatFileStatus)m_data.DatFileStates[file].Clone();
                                contents[f.LastWriteTime] =
                                    new KeyValuePair<string, DatFileStatus>(file, dfs);
                            }
                        }
                        catch //if network disconnection should happen
                        { 
                        }
                    }
            }

            m_gridView.RowCount = contents.Count;
            int count = 0;
            foreach (KeyValuePair<DateTime, KeyValuePair<string, DatFileStatus>> it in contents)
            {
                m_gridView.Rows[count].Cells[0].Value = it.Value.Key;
                List<bool> blank = new List<bool>();
                for (int i = 0; i < m_gridView.Rows[count].Cells.Count; i++)
                    blank.Add(true);
                if (it.Value.Value != null)
                {
                    m_gridView.Rows[count].Cells[1].Value = it.Value.Value.TimesTried;
                    Bitmap bitmap = null;
                    text = String.Empty;
                    foreach (KeyValuePair<TaskData, DatFileStatus.State> pair in it.Value.Value.States)
                    {
                        if (pair.Key is ReportData)
                            bitmap = m_reportIcons[pair.Value];
                        else if (pair.Key is ExtractData)
                            bitmap = m_extractIcons[pair.Value];
                        else if (pair.Key is BatchFileData)
                            bitmap = m_batchfileIcons[pair.Value];
                        else if (pair.Key is CopyMoveTaskData)
                            bitmap = m_copydatIcons[pair.Value];
                        else if (pair.Key is IfTaskData)
                            bitmap = m_conditionIcons[pair.Value];
                        else if (pair.Key is CustomTaskData)
                        {
                            CustomTaskData cust = (CustomTaskData) pair.Key;
                            string name = cust.Plugin.NameInfo;
                            int index = PluginManager.Manager.PluginInfos.FindIndex(delegate(PluginTaskInfo i) { return i.Name == name; });
                            bitmap = m_customtaskIcons[index][pair.Value];
                        }
                        text = m_taskTexts[pair.Value];
                        DataGridViewImageCell cell = m_gridView.Rows[count].Cells[pair.Key.Index + 2] as DataGridViewImageCell;
                        blank[pair.Key.Index + 2] = false;
                        if ((cell.Value as Bitmap) != bitmap)
                        {
                            cell.Value = bitmap;
                            if (pair.Value == DatFileStatus.State.RUNNING)
                                m_gridView.CurrentCell = cell;
                        }
                        if (cell.ToolTipText != text)
                            cell.ToolTipText = text;
                    }
                }

                for (int i = 2; i < m_gridView.Rows[count].Cells.Count; i++)
                    if (blank[i] && (m_gridView.Rows[count].Cells[i] as DataGridViewImageCell).Value != m_blankIcon)
                        (m_gridView.Rows[count].Cells[i] as DataGridViewImageCell).Value = m_blankIcon;
                count++;
            }
            m_refreshTimer.Enabled = true;
        }
        #endregion

        private void m_confNameLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (m_manager as MainForm).fromStatusToConfiguration();
        }
    }
}
