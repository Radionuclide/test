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
using iba.Dialogs;

namespace iba.Controls
{
    public partial class PermanentFileErrorsControl : UserControl, IPropertyPane, IPluginsUpdatable
    {
        public PermanentFileErrorsControl()
        {
            InitializeComponent();
            this.m_gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            InitializeIcons();
            ((Bitmap)m_refreshDats.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_deleteDats.Image).MakeTransparent(Color.Magenta);
            m_checkedFiles = new Set<string>();
            m_toolTip.SetToolTip(m_deleteDats, iba.Properties.Resources.deletePermanentErrorFilesButton);
            m_toolTip.SetToolTip(m_refreshDats, iba.Properties.Resources.refreshPermanentErrorFilesButton);

        }

       
        #region IPropertyPane Members
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


            m_customtaskIcons = new Dictionary<DatFileStatus.State, Bitmap>[PluginManager.Manager.PluginInfos.Count];
            for (int i = 0; i < m_customtaskIcons.Length; i++)
                m_customtaskIcons[i] = new Dictionary<DatFileStatus.State, Bitmap>();


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
            bFirstLoad = true;
            m_data = datasource as MinimalStatusData;
            m_cd = TaskManager.Manager.GetConfigurationFromWorker(m_data.CorrConfigurationGuid);
            if(m_cd.JobType == ConfigurationData.JobTypeEnum.Scheduled)
            {
                m_infoLabel.Text = Properties.Resources.PermErrorsLabelHDQ;
                DatFiles.HeaderText = Properties.Resources.PermErrorsColumnHeaderHDQ;
            }
            else
            {
                m_infoLabel.Text = Properties.Resources.PermErrorsLabelDat;
                DatFiles.HeaderText = Properties.Resources.PermErrorsColumnHeaderDat;
            }
            m_confNameLinkLabel.Left = m_infoLabel.Right + 5;
            m_confNameLinkLabel.Text = m_cd.Name;
            int count = m_cd.Tasks.Count;
            if (m_gridView.ColumnCount != (count + 3))
            {
                m_gridView.ColumnCount = 3;
                DataGridViewImageColumn [] cols = new DataGridViewImageColumn[count];
                for (int i = 0; i < cols.Length; i++)
                {
                    cols[i] = new DataGridViewImageColumn();
                    cols[i].ReadOnly = true;
                }
                m_gridView.Columns.AddRange(cols);
            }
            foreach (TaskData task in m_cd.Tasks)
                m_gridView.Columns[task.Index + 3].HeaderText = task.Name;

            OnChangedData(null, null);
            m_refreshTimer.Start();
        }

        public void LeaveCleanup() 
        {
            m_refreshTimer.Stop();
            m_checkedFiles.Clear(); //spare some memory
        }

        public void SaveData()
        {
        }

        private Set<String> m_checkedFiles;
        private bool bFirstLoad;
        private void DetermineCheckedFiles()
        {
            m_checkedFiles.Clear();
            if (bFirstLoad) //m_checkedFiles is set by LoadData;
            {
                bFirstLoad = false;
                return;
            }
            foreach (DataGridViewRow row in m_gridView.Rows)
            {
                if (((bool) row.Cells[0].Value))
                {
                    string filename = row.Cells[1].Tag as string;
                    if (!string.IsNullOrEmpty(filename))
                        m_checkedFiles.Add(filename);
                }
            }
        }

        int lastChangedCount = 0;

        public void OnChangedData(object sender, EventArgs e)
        {
            m_refreshTimer.Enabled = false;
            if (sender != null) //refresh
                m_data = TaskManager.Manager.GetMinimalStatus(m_data.CorrConfigurationGuid, true);
            if (m_data == null) //returned zero, try again other time
            {
                m_refreshTimer.Enabled = true;
                return;
            }
            //wait cursor
            if (m_data.UpdatingFileList)
            {
                this.Cursor = Cursors.WaitCursor;
                m_gridView.Cursor = this.Cursor;
                m_deleteDats.Enabled = m_refreshDats.Enabled = false;
            }
            else
            {
                m_deleteDats.Enabled = m_refreshDats.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            if (m_data.ChangedCount == lastChangedCount && sender != null)
            {
                m_refreshTimer.Enabled = true;
                return;
            }
            lastChangedCount = m_data.ChangedCount;

            DetermineCheckedFiles();
            m_gridView.RowCount = m_data.Files.Count; 
            int count = 0;
            foreach (MinimalDatFileStatus dfs in m_data.Files)
            {
                m_gridView.Rows[count].Cells[1].Value = dfs.Description;
                m_gridView.Rows[count].Cells[1].Tag = dfs.Filename;
                m_gridView.Rows[count].Cells[0].Value = m_checkedFiles.Contains(dfs.Filename);
                List<bool> blank = new List<bool>();
                for (int i = 0; i < m_gridView.Rows[count].Cells.Count; i++)
                    blank.Add(true);
                if (dfs.TaskStates != null)
                {
                    m_gridView.Rows[count].Cells[2].Value = dfs.TimesTried;
                    Bitmap bitmap = null;
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
                        String text = m_taskTexts[value];
                        DataGridViewImageCell cell = m_gridView.Rows[count].Cells[i + 3] as DataGridViewImageCell;
                        blank[i + 3] = false;
                        if ((cell.Value as Bitmap) != bitmap)
                            cell.Value = bitmap;
                        if (cell.ToolTipText != text)
                            cell.ToolTipText = text;
                    }
                }

                for (int i = 3; i < m_gridView.Rows[count].Cells.Count; i++)
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

        private void m_deleteDats_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show(this, iba.Properties.Resources.deletePermanentErrorFilesWarning,
            iba.Properties.Resources.deletePermanentErrorFilesButton, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (res != DialogResult.Yes)
                return;
            DetermineCheckedFiles();
            using (DeleteDatFilesDialog dlg = new DeleteDatFilesDialog(m_cd.DatDirectoryUNC, m_cd.Username, m_cd.Password, m_checkedFiles))
            {
                if (m_cd.JobType == ConfigurationData.JobTypeEnum.Scheduled)
                    dlg.Text = iba.Properties.Resources.DeleteDialogTitleHDQ;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
                TaskManager.Manager.AlterPermanentFileErrorList(TaskManager.AlterPermanentFileErrorListWhatToDo.AFTERDELETE, m_data.CorrConfigurationGuid, m_checkedFiles);
            }
        }

        private void m_refreshDats_Click(object sender, EventArgs e)
        {

            if(!TaskManager.Manager.IsJobStarted(m_cd.Guid) && !m_cd.InitialScanEnabled && !m_cd.RescanEnabled)
            {
                MessageBox.Show(this, iba.Properties.Resources.refreshPermanentErrorFilesError,
                    iba.Properties.Resources.refreshDatButton, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            DialogResult res = MessageBox.Show(this, iba.Properties.Resources.refreshPermanentErrorFilesWarning,
                iba.Properties.Resources.refreshDatButton, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if(res != DialogResult.Yes)
                return;
            DetermineCheckedFiles();
            using (RemoveMarkingsDialog dlg = new RemoveMarkingsDialog(m_cd.DatDirectoryUNC, m_cd.Username, m_cd.Password, m_checkedFiles))
            {
                if (m_cd.JobType == ConfigurationData.JobTypeEnum.Scheduled)
                    dlg.Text = iba.Properties.Resources.UnmarkDialogTitleHDQ;

                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
                TaskManager.Manager.AlterPermanentFileErrorList(TaskManager.AlterPermanentFileErrorListWhatToDo.AFTERREFRESH, m_data.CorrConfigurationGuid, m_checkedFiles);
            }
        }

        private void m_gridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_gridView.EndEdit();
            if (e.ColumnIndex == 0 && m_gridView.Rows.Count > 0) //checkBox
            {
                int startPos = m_gridView.Rows.Count;
                if (m_gridView.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in m_gridView.SelectedCells)
                        if (cell.RowIndex < startPos) 
                            startPos = cell.RowIndex;
                }
                else startPos = 0;
                bool toset = (bool) m_gridView.Rows[startPos].Cells[0].Value;
                for (int i = startPos; i < m_gridView.Rows.Count; i++)
                    m_gridView.Rows[i].Cells[0].Value = toset;
            }
        }

        //auto commit column 1
        private void m_gridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (m_gridView.IsCurrentCellDirty && m_gridView.CurrentCell != null && m_gridView.CurrentCell.ColumnIndex == 0)
            {
                m_gridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.m_gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DatFiles.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            m_attempts.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
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
