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
    public partial class StatusControl : UserControl, IPropertyPane, IPluginsUpdatable
    {
        public StatusControl()
        {
            InitializeComponent();
            this.m_gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            InitializeIcons();
        }
       
        IPropertyPaneManager m_manager;
        MinimalStatusData m_data;


        Dictionary<DatFileStatus.State, Bitmap> m_reportIcons, m_extractIcons, m_batchfileIcons, m_copydatIcons, m_conditionIcons, m_updateIcons, m_pauseIcons, m_cleanupIcons, m_splitIcons;
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
            m_updateIcons = new Dictionary<DatFileStatus.State, Bitmap>();
            m_pauseIcons = new Dictionary<DatFileStatus.State, Bitmap>();
            m_cleanupIcons = new Dictionary<DatFileStatus.State, Bitmap>();
            m_splitIcons = new Dictionary<DatFileStatus.State, Bitmap>();
            m_taskTexts = new Dictionary<DatFileStatus.State, String>();



            m_blankIcon = Bitmap.FromHicon(iba.Properties.Resources.blank.Handle);

            m_reportIcons.Add(DatFileStatus.State.NOT_STARTED, m_blankIcon);
            m_reportIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle));
            m_reportIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS,Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));
            m_reportIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));
            m_reportIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));
            m_reportIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));
            m_reportIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));
            m_reportIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle)));

            m_extractIcons.Add(DatFileStatus.State.NOT_STARTED, m_blankIcon);
            m_extractIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle));
            m_extractIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));
            m_extractIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));
            m_extractIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));
            m_extractIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));
            m_extractIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));
            m_extractIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle)));

            m_batchfileIcons.Add(DatFileStatus.State.NOT_STARTED, m_blankIcon);
            m_batchfileIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle));
            m_batchfileIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));
            m_batchfileIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));
            m_batchfileIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));
            m_batchfileIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));
            m_batchfileIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));
            m_batchfileIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle)));

            m_copydatIcons.Add(DatFileStatus.State.NOT_STARTED, m_blankIcon);
            m_copydatIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle));
            m_copydatIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));
            m_copydatIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));
            m_copydatIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));
            m_copydatIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));
            m_copydatIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));
            m_copydatIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle)));

            m_conditionIcons.Add(DatFileStatus.State.NOT_STARTED, m_blankIcon);
            m_conditionIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle));
            m_conditionIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));
            m_conditionIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));
            m_conditionIcons.Add(DatFileStatus.State.COMPLETED_TRUE, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle))); 
            m_conditionIcons.Add(DatFileStatus.State.COMPLETED_FALSE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));
            m_conditionIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));
            m_conditionIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));
            m_conditionIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle)));

            m_updateIcons.Add(DatFileStatus.State.NOT_STARTED, m_blankIcon);
            m_updateIcons.Add(DatFileStatus.State.RUNNING, iba.Properties.Resources.updatedatatask);
            m_updateIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, iba.Properties.Resources.updatedatatask));
            m_updateIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, iba.Properties.Resources.updatedatatask));
            m_updateIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, iba.Properties.Resources.updatedatatask));
            m_updateIcons.Add(DatFileStatus.State.COMPLETED_FALSE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, iba.Properties.Resources.updatedatatask));
            m_updateIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, iba.Properties.Resources.updatedatatask));
            m_updateIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, iba.Properties.Resources.updatedatatask));
            m_updateIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, iba.Properties.Resources.updatedatatask));

            m_pauseIcons.Add(DatFileStatus.State.NOT_STARTED, m_blankIcon);
            m_pauseIcons.Add(DatFileStatus.State.RUNNING, iba.Properties.Resources.pausetask);
            m_pauseIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, iba.Properties.Resources.pausetask));
            m_pauseIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, iba.Properties.Resources.pausetask));
            m_pauseIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, iba.Properties.Resources.pausetask));
            m_pauseIcons.Add(DatFileStatus.State.COMPLETED_FALSE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, iba.Properties.Resources.pausetask));
            m_pauseIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, iba.Properties.Resources.pausetask));
            m_pauseIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, iba.Properties.Resources.pausetask));
            m_pauseIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, iba.Properties.Resources.pausetask));

            m_cleanupIcons.Add(DatFileStatus.State.NOT_STARTED, m_blankIcon);
            m_cleanupIcons.Add(DatFileStatus.State.RUNNING, iba.Properties.Resources.broom);
            m_cleanupIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, iba.Properties.Resources.broom));
            m_cleanupIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, iba.Properties.Resources.broom));
            m_cleanupIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, iba.Properties.Resources.broom));
            m_cleanupIcons.Add(DatFileStatus.State.COMPLETED_FALSE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, iba.Properties.Resources.broom));
            m_cleanupIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, iba.Properties.Resources.broom));
            m_cleanupIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, iba.Properties.Resources.broom));
            m_cleanupIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, iba.Properties.Resources.broom));

            m_splitIcons.Add(DatFileStatus.State.NOT_STARTED, m_blankIcon);
            m_splitIcons.Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(iba.Properties.Resources.SplitDat.Handle));
            m_splitIcons.Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(iba.Properties.Resources.SplitDat.Handle)));
            m_splitIcons.Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.SplitDat.Handle)));
            m_splitIcons.Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(iba.Properties.Resources.SplitDat.Handle)));
            m_splitIcons.Add(DatFileStatus.State.COMPLETED_FALSE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(iba.Properties.Resources.SplitDat.Handle)));
            m_splitIcons.Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(iba.Properties.Resources.SplitDat.Handle)));
            m_splitIcons.Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(iba.Properties.Resources.SplitDat.Handle)));
            m_splitIcons.Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(iba.Properties.Resources.SplitDat.Handle)));

            UpdatePlugins();

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

        private ConfigurationData m_cd;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as MinimalStatusData;
            m_cd = TaskManager.Manager.GetConfigurationFromWorker(m_data.CorrConfigurationGuid);
            if(m_cd.JobType == ConfigurationData.JobTypeEnum.Scheduled)
                DatFiles.HeaderText = Properties.Resources.PermErrorsColumnHeaderHDQ;

            m_confNameLinkLabel.Text = m_cd.Name;
            int count = m_cd.Tasks.Count;
            if (m_gridView.ColumnCount != (count + 2))
            {
                m_gridView.ColumnCount = 2;
                DataGridViewImageColumn [] cols = new DataGridViewImageColumn[count];
                for(int i = 0; i < cols.Length;i++) cols[i] = new DataGridViewImageColumn();
                m_gridView.Columns.AddRange(cols);
            }
            foreach (TaskData task in m_cd.Tasks)
                m_gridView.Columns[task.Index + 2].HeaderText = task.Name;

            OnChangedData(null, null);
            m_refreshTimer.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.m_gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        public void LeaveCleanup() 
        {
            m_refreshTimer.Stop();
        }

        public void SaveData()
        {
            //nothing to be saved as status is purely an output control
        }

        int lastChangedCount;
        public void OnChangedData(object sender, EventArgs e)
        {
            m_refreshTimer.Enabled = false;
            if (sender != null) //refresh
               m_data = TaskManager.Manager.GetMinimalStatus(m_data.CorrConfigurationGuid,false);
            if (m_data==null) //returned zero, try again other time
            {
                m_refreshTimer.Enabled = true;
                return;
            }
            if(m_data.UpdatingFileList) this.Cursor = Cursors.WaitCursor;
            else
            {
                this.Cursor = Cursors.Default;
                m_gridView.Cursor = this.Cursor; //workaround for cursor in gridview not changing
            }
            if (m_data.ChangedCount == lastChangedCount && sender != null)
            {
                m_refreshTimer.Enabled = true;
                return;
            }
            lastChangedCount = m_data.ChangedCount;
            //wait cursor
            string text = String.Format(iba.Properties.Resources.statText1, m_data.Started ? iba.Properties.Resources.statText2 : iba.Properties.Resources.statText3) + Environment.NewLine;
            //text += "processedFiles: " + m_data.ProcessedFiles.Count.ToString() + Environment.NewLine;
            //text += "readFiles: " + m_data.ReadFiles.Count.ToString() + Environment.NewLine;
            m_statusRunningText.Text = text;
            m_gridView.RowCount = m_data.Files.Count;
            int count = 0;
            foreach (MinimalDatFileStatus dfs in m_data.Files)
            {
                m_gridView.Rows[count].Cells[0].Value = dfs.Description;
                List<bool> blank = new List<bool>();
                for (int i = 0; i < m_gridView.Rows[count].Cells.Count; i++)
                    blank.Add(true);
                if (dfs.TaskStates != null)
                {
                    m_gridView.Rows[count].Cells[1].Value = dfs.TimesTried;
                    Bitmap bitmap = null;
                    text = String.Empty;
                    for (int i = 0; i < dfs.TaskStates.Length; i++)
                    {
                        TaskData task = m_cd.Tasks[i];
                        DatFileStatus.State value = dfs.TaskStates[i];
                        if (task is ReportData)
                            bitmap = m_reportIcons[value];
                        else if (task is ExtractData)
                            bitmap = m_extractIcons[value];
                        else if (task is BatchFileData)
                            bitmap = m_batchfileIcons[value];
                        else if (task is CopyMoveTaskData)
                            bitmap = m_copydatIcons[value];
                        else if (task is IfTaskData)
                            bitmap = m_conditionIcons[value];
                        else if (task is UpdateDataTaskData)
                            bitmap = m_updateIcons[value];
                        else if (task is PauseTaskData)
                            bitmap = m_pauseIcons[value];
                        else if (task is SplitterTaskData)
                            bitmap = m_splitIcons[value];
                        else if (task is TaskWithTargetDirData) // have this last, as UNCTask derives from cleanupTask and many derive from unc
                            bitmap = m_cleanupIcons[value];
                        else if (task is ICustomTaskData)
                        {
                            ICustomTaskData cust = (ICustomTaskData)task;
                            string name = cust.Plugin.NameInfo;
                            int index = PluginManager.Manager.PluginInfos.FindIndex(delegate(PluginTaskInfo ii) { return ii.Name == name; });
                            bitmap = m_customtaskIcons[index][value];
                        }

                        text = m_taskTexts[value];
                        DataGridViewImageCell cell = m_gridView.Rows[count].Cells[i + 2] as DataGridViewImageCell;
                        blank[i + 2] = false;
                        if ((cell.Value as Bitmap) != bitmap)
                        {
                            cell.Value = bitmap;
                            if (value == DatFileStatus.State.RUNNING)
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

        private void m_confNameLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (m_manager as MainForm).fromStatusToConfiguration();
        }

        public void UpdatePlugins()
        {
            m_customtaskIcons = new Dictionary<DatFileStatus.State, Bitmap>[PluginManager.Manager.PluginInfos.Count];
            for (int i = 0; i < m_customtaskIcons.Length; i++)
            {
                m_customtaskIcons[i] = new Dictionary<DatFileStatus.State, Bitmap>();
                IntPtr handle = PluginManager.Manager.PluginInfos[i].Icon.Handle;
                m_customtaskIcons[i].Add(DatFileStatus.State.NOT_STARTED, m_blankIcon);
                m_customtaskIcons[i].Add(DatFileStatus.State.RUNNING, Bitmap.FromHicon(handle));
                m_customtaskIcons[i].Add(DatFileStatus.State.NO_ACCESS, MergeIcons(DatFileStatus.State.NO_ACCESS, Bitmap.FromHicon(handle)));
                m_customtaskIcons[i].Add(DatFileStatus.State.COMPLETED_FAILURE, MergeIcons(DatFileStatus.State.COMPLETED_FAILURE, Bitmap.FromHicon(handle)));
                m_customtaskIcons[i].Add(DatFileStatus.State.COMPLETED_SUCCESFULY, MergeIcons(DatFileStatus.State.COMPLETED_SUCCESFULY, Bitmap.FromHicon(handle)));
                m_customtaskIcons[i].Add(DatFileStatus.State.TIMED_OUT, MergeIcons(DatFileStatus.State.TIMED_OUT, Bitmap.FromHicon(handle)));
                m_customtaskIcons[i].Add(DatFileStatus.State.MEMORY_EXCEEDED, MergeIcons(DatFileStatus.State.MEMORY_EXCEEDED, Bitmap.FromHicon(handle)));
                m_customtaskIcons[i].Add(DatFileStatus.State.TRIED_TOO_MANY_TIMES, MergeIcons(DatFileStatus.State.TRIED_TOO_MANY_TIMES, Bitmap.FromHicon(handle)));
            }
        }
    }
}
