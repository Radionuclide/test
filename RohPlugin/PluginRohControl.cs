using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using iba.Plugins;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

namespace Alunorf_roh_plugin
{
    public partial class PluginRohControl : UserControl, IPluginControl
    {
        private IDatCoHost m_datcoHost;
        public PluginRohControl(IDatCoHost host)
        {
            m_datcoHost = host;
            InitializeComponent();
            DataGridView[] grids = { m_datagvStich, m_datagvKopf, m_datagvSchluss };
            foreach (DataGridView grid in grids)
            {
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            }

            ((Bitmap)m_selectButton.Image).MakeTransparent(Color.Magenta);
            m_toolTip.SetToolTip(m_selectButton, Alunorf_roh_plugin.Properties.Resources.tooltipSelect);
            m_toolTip.SetToolTip(m_browseDatFileButton, Alunorf_roh_plugin.Properties.Resources.tooltipBrowse);
            m_toolTip.SetToolTip(m_testRohButton, Alunorf_roh_plugin.Properties.Resources.tooltipTest);
            m_testRohButton.Image = Alunorf_roh_plugin.Properties.Resources.RohTask.ToBitmap();
            m_kurzbezeichner.Font = m_parameter.Font = m_kommentare.Font = new Font(FontFamily.GenericMonospace, 8, FontStyle.Regular);
        }

        #region IPluginControl Members

        PluginRohTask m_data;
        ICommonTaskControl m_control;

        public void LoadData(object datasource, ICommonTaskControl parentcontrol)
        {
            m_data = datasource as PluginRohTask;
            m_control = parentcontrol;

            m_datFileTextBox.Text = m_data.TemplateDatFile;

            m_ftpDirectory.Text = m_data.FtpDirectory;
            m_ftpHost.Text = m_data.FtpHost;
            m_ftpUsername.Text = m_data.FtpUser;
            m_ftpPassword.Text = m_data.FtpPassword;

            m_parameter.Text = m_data.RohInput.Parameter;
            m_kurzbezeichner.Text = m_data.RohInput.Kurzbezeichner;
            m_kommentare.Text = m_data.RohInput.Kommentare;
            m_filePrefix.Text = m_data.FilePrefix;

            iba.RohWriterDataLineInput[][] datasets = { m_data.RohInput.StichDaten, m_data.RohInput.KopfDaten, m_data.RohInput.SchlussDaten};
            DataGridView[] grids = { m_datagvStich, m_datagvKopf, m_datagvSchluss };
            DataGridView grid = null;
            int count;
            for (int j = 0; j < 3; j++)
            {
                grid = grids[j];
                iba.RohWriterDataLineInput[] dataset = datasets[j];
                count = dataset.Length;
                grid.RowCount = count + 1;
                for (int i = 0; i < count; i++)
                {
                    grid.Rows[i].Cells[0].Value = dataset[i].ibaName;
                    grid.Rows[i].Cells[1].Value = dataset[i].Bezeichnung;
                    grid.Rows[i].Cells[2].Value = dataset[i].KurzBezeichnung;
                    grid.Rows[i].Cells[3].Value = dataset[i].Einheit;
                    switch (dataset[i].dataType)
                    {
                        case iba.DataTypeEnum.C:
                            grid.Rows[i].Cells[4].Value = "C";
                            break;
                        case iba.DataTypeEnum.F:
                            grid.Rows[i].Cells[4].Value = "F";
                            break;
                        case iba.DataTypeEnum.F4:
                            grid.Rows[i].Cells[4].Value = "F4";
                            break;
                        case iba.DataTypeEnum.F8:
                            grid.Rows[i].Cells[4].Value = "F8";
                            break;
                        case iba.DataTypeEnum.I:
                            grid.Rows[i].Cells[4].Value = "I";
                            break;
                        case iba.DataTypeEnum.I2:
                            grid.Rows[i].Cells[4].Value = "I2";
                            break;
                        case iba.DataTypeEnum.I4:
                            grid.Rows[i].Cells[4].Value = "I4";
                            break;
                        case iba.DataTypeEnum.T:
                            grid.Rows[i].Cells[4].Value = "T";
                            break;
                        case iba.DataTypeEnum.C2:
                            grid.Rows[i].Cells[4].Value = "C2";
                            break;
                    }
                }
                grid.Rows[count].Cells[0].Value = null;
                grid.Rows[count].Cells[1].Value = null;
                grid.Rows[count].Cells[2].Value = null;
                grid.Rows[count].Cells[3].Value = null;
                grid.Rows[count].Cells[4].Value = null;
            }
            grid = m_datagvKanalbeschreibung;
            iba.RohWriterChannelLineInput[] dataset2 = m_data.RohInput.Kanalen;
            count = dataset2.Length;
            grid.RowCount = count + 1;
            for (int i = 0; i < count; i++)
            {
                grid.Rows[i].Cells[0].Value = dataset2[i].ibaName;
                grid.Rows[i].Cells[1].Value = dataset2[i].Bezeichnung;
                grid.Rows[i].Cells[2].Value = dataset2[i].KurzBezeichnung;
                grid.Rows[i].Cells[3].Value = dataset2[i].Einheit;
                switch (dataset2[i].dataType)
                {
                    case iba.DataTypeEnum.C:
                        grid.Rows[i].Cells[4].Value = "C";
                        break;
                    case iba.DataTypeEnum.F:
                        grid.Rows[i].Cells[4].Value = "F";
                        break;
                    case iba.DataTypeEnum.F4:
                        grid.Rows[i].Cells[4].Value = "F4";
                        break;
                    case iba.DataTypeEnum.F8:
                        grid.Rows[i].Cells[4].Value = "F8";
                        break;
                    case iba.DataTypeEnum.I:
                        grid.Rows[i].Cells[4].Value = "I";
                        break;
                    case iba.DataTypeEnum.I2:
                        grid.Rows[i].Cells[4].Value = "I2";
                        break;
                    case iba.DataTypeEnum.I4:
                        grid.Rows[i].Cells[4].Value = "I4";
                        break;
                    case iba.DataTypeEnum.T:
                        grid.Rows[i].Cells[4].Value = "T";
                        break;
                    case iba.DataTypeEnum.C2:
                        grid.Rows[i].Cells[4].Value = "C2";
                        break;
                }
                grid.Rows[i].Cells[5].Value = dataset2[i].Faktor.ToString();
                grid.Rows[i].Cells[6].Value = dataset2[i].Kennung.ToString();
                grid.Rows[i].Cells[7].Value = dataset2[i].Sollwert;
                grid.Rows[i].Cells[8].Value = dataset2[i].Stutzstellen;
            }
            m_tabControl.SelectedIndex = m_data.SelectedTab;
            m_datFileTextBox_TextChanged(null, null);
        }

        public void SaveData()
        {
            m_data.TemplateDatFile = m_datFileTextBox.Text;
            m_data.FtpDirectory = m_ftpDirectory.Text;
            m_data.FtpHost = m_ftpHost.Text;
            m_data.FtpUser = m_ftpUsername.Text;
            m_data.FtpPassword = m_ftpPassword.Text;
            m_data.RohInput.Parameter = m_parameter.Text;
            m_data.RohInput.Kommentare = m_kommentare.Text;
            m_data.RohInput.Kurzbezeichner = m_kurzbezeichner.Text;
            m_data.SelectedTab = m_tabControl.SelectedIndex;
            m_data.FilePrefix = m_filePrefix.Text;

            iba.RohWriterDataLineInput[][] datasets = { m_data.RohInput.StichDaten, m_data.RohInput.KopfDaten, m_data.RohInput.SchlussDaten};
            DataGridView[] grids = { m_datagvStich, m_datagvKopf, m_datagvSchluss };
            DataGridView grid = null;
            int count, count2;
            for (int j = 0; j < 3; j++)
            {
                grid = grids[j];
                count = grid.RowCount;
                iba.RohWriterDataLineInput[] dataset = new iba.RohWriterDataLineInput[count];
                count2=0;
                for (int i = 0; i < count; i++)
                {
                    string ibaName = grid.Rows[i].Cells[0].Value as string;
                    string bezeichnung = grid.Rows[i].Cells[1].Value as string;
                    string kurz = grid.Rows[i].Cells[2].Value as string;
                    if (String.IsNullOrEmpty(ibaName) || String.IsNullOrEmpty(bezeichnung) || String.IsNullOrEmpty(kurz)) continue;
                    iba.RohWriterDataLineInput line = new iba.RohWriterDataLineInput();
                    line.ibaName = ibaName.Trim();
                    bezeichnung = bezeichnung.Trim();
                    if (bezeichnung.Length > 30) bezeichnung = bezeichnung.Substring(0, 30);
                    line.Bezeichnung = bezeichnung;
                    kurz = kurz.Trim();
                    if (kurz.Length > 8) kurz = kurz.Substring(0, 8);
                    line.KurzBezeichnung = kurz;
                    string einheit = grid.Rows[i].Cells[3].Value as string;
                    if (einheit == null) einheit = "";
                    einheit = einheit.Trim();
                    if (einheit.Length > 8) einheit = einheit.Substring(0, 8);
                    line.Einheit = einheit;
                    string datatyp = grid.Rows[i].Cells[4].Value as string;
                    if (datatyp == null || datatyp == "C")
                        line.dataType = iba.DataTypeEnum.C;
                    else if (datatyp == "F")
                        line.dataType = iba.DataTypeEnum.F;
                    else if (datatyp == "F4")
                        line.dataType = iba.DataTypeEnum.F4;
                    else if (datatyp == "F8")
                        line.dataType = iba.DataTypeEnum.F8;
                    else if (datatyp == "I")
                        line.dataType = iba.DataTypeEnum.I;
                    else if (datatyp == "I2")
                        line.dataType = iba.DataTypeEnum.I2;
                    else if (datatyp == "I4")
                        line.dataType = iba.DataTypeEnum.I4;
                    else if (datatyp == "T")
                        line.dataType = iba.DataTypeEnum.T;
                    else if (datatyp == "C2")
                        line.dataType = iba.DataTypeEnum.C2;
                    dataset[count2++] = line;
                }
                Array.Resize(ref dataset, count2);
                datasets[j] = dataset;
            }
            m_data.RohInput.StichDaten = datasets[0]; 
            m_data.RohInput.KopfDaten = datasets[1];
            m_data.RohInput.SchlussDaten = datasets[2]; 
            grid = m_datagvKanalbeschreibung;
            count = grid.RowCount;
            iba.RohWriterChannelLineInput[] dataset2 = new iba.RohWriterChannelLineInput[count];
            Array.Resize(ref dataset2, count);
            count2 = 0;
            for (int i = 0; i < count; i++)
            {
                string ibaName = grid.Rows[i].Cells[0].Value as string;
                string bezeichnung = grid.Rows[i].Cells[1].Value as string;
                string kurz = grid.Rows[i].Cells[2].Value as string;
                if (String.IsNullOrEmpty(ibaName) || String.IsNullOrEmpty(bezeichnung) || String.IsNullOrEmpty(kurz)) continue;
                iba.RohWriterChannelLineInput line = new iba.RohWriterChannelLineInput();
                line.ibaName = ibaName.Trim();
                bezeichnung = bezeichnung.Trim();
                if (bezeichnung.Length > 30) bezeichnung = bezeichnung.Substring(0, 30);
                line.Bezeichnung = bezeichnung;
                kurz = kurz.Trim();
                if (kurz.Length > 8) kurz = kurz.Substring(0, 8);
                line.KurzBezeichnung = kurz;
                string einheit = grid.Rows[i].Cells[3].Value as string;
                if (einheit == null) einheit = "";
                einheit = einheit.Trim();
                if (einheit.Length > 8) einheit = einheit.Substring(0, 8);
                line.Einheit = einheit;
                string datatyp = grid.Rows[i].Cells[4].Value as string;
                if (datatyp == null || datatyp == "C")
                    line.dataType = iba.DataTypeEnum.C;
                else if (datatyp == "F")
                    line.dataType = iba.DataTypeEnum.F;
                else if (datatyp == "F4")
                    line.dataType = iba.DataTypeEnum.F4;
                else if (datatyp == "F8")
                    line.dataType = iba.DataTypeEnum.F8;
                else if (datatyp == "I")
                    line.dataType = iba.DataTypeEnum.I;
                else if (datatyp == "I2")
                    line.dataType = iba.DataTypeEnum.I2;
                else if (datatyp == "I4")
                    line.dataType = iba.DataTypeEnum.I4;
                else if (datatyp == "T")
                    line.dataType = iba.DataTypeEnum.T;
                else if (datatyp == "C2")
                    line.dataType = iba.DataTypeEnum.C2;
                string factorstr = grid.Rows[i].Cells[5].Value as string;
                if (factorstr == null || !Double.TryParse(factorstr.Trim(), out line.Faktor))
                    line.Faktor = 1.0;
                string kennungstr = grid.Rows[i].Cells[6].Value as string;
                if (kennungstr == null || !Int32.TryParse(kennungstr.Trim(), out line.Kennung))
                    line.Kennung = 0;
                string sollwert = grid.Rows[i].Cells[7].Value as string;
                if (sollwert == null) sollwert = "";
                sollwert = sollwert.Trim();
                if (sollwert.Length > 8) sollwert = sollwert.Substring(0, 8);
                line.Sollwert = sollwert;
                string stutz = grid.Rows[i].Cells[8].Value as string;
                if (stutz == null) stutz = "";
                stutz = stutz.Trim();
                if (stutz.Length > 8) stutz = stutz.Substring(0, 8);
                line.Stutzstellen = stutz;
                dataset2[count2++] = line;
            }
            Array.Resize(ref dataset2, count2);
            m_data.RohInput.Kanalen = dataset2;
        }

        public void LeaveCleanup()
        {//nothing to clean up
        }

        #endregion

        private void m_datagv_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int row = 0;
                int col = 0;

                DataGridView grid = sender as DataGridView;
                if (grid == null) return;

                if (grid.CurrentCell != null)
                {
                    row = grid.CurrentCell.RowIndex;
                    col = grid.CurrentCell.ColumnIndex;
                }
                if (grid.SelectedCells.Count > 2) //multiselect, only take first
                {
                    row = Algorithms.min<DataGridViewCell>(grid.SelectedCells, delegate(DataGridViewCell a, DataGridViewCell b) { return a.RowIndex - b.RowIndex; }).RowIndex;
                    col = Algorithms.min<DataGridViewCell>(grid.SelectedCells, delegate(DataGridViewCell a, DataGridViewCell b) { return a.ColumnIndex - b.ColumnIndex; }).ColumnIndex;
                }

                if (e.Control && e.KeyCode == Keys.V)
                {
                    object o = Clipboard.GetData(DataFormats.CommaSeparatedValue);
                    if (o == null)
                        o = Clipboard.GetData(DataFormats.UnicodeText);
                    if (o == null)
                        o = Clipboard.GetData(DataFormats.Text);
                    if (o == null) return;
                    Stream stream = o as Stream;
                    String asstring = o as String;
                    TextReader sr = null;
                    if (stream != null) sr = new StreamReader(stream);
                    else if (asstring != null) sr = new StringReader(asstring);
                    else return;
                    try
                    {
                        for (string s = sr.ReadLine(); s != null; s = sr.ReadLine())
                        {
                            if (string.IsNullOrEmpty(s) || s == "\0")
                            {
                                row++;
                                continue;
                            }
                            while (row + 1 >= grid.RowCount) //always leav
                                grid.RowCount++;
                            string[] sr_array = Algorithms.SplitPaste(s);

                            if (sr_array.Length == grid.ColumnCount+1 && string.IsNullOrEmpty(sr_array[0])) //when copying entire row unfortunately the header seems to be copied as well
                            {
                                string[] sr_arrayCopy = new string[sr_array.Length - 1];
                                Array.Copy(sr_array, 1, sr_arrayCopy, 0, sr_array.Length - 1);
                                sr_array = sr_arrayCopy;
                            }
                            for (int col2 = col; col2 < sr_array.Length + col; col2++)
                            {
                                if (col2 >= grid.ColumnCount) break;
                                string temp = sr_array[col2 - col].Trim();
                                switch (col2)
                                {
                                    case 0: //ibaName
                                        break;
                                    case 1: //beschreibung -> max 30 characters
                                        if (temp.Length > 30) temp = temp.Substring(0,30);
                                        break;
                                    case 2: //kurz -> max 8 characters
                                        if (temp.Length > 8) temp = temp.Substring(0, 8);
                                        break;
                                    case 3: //einheit -> same as kurz
                                        goto case 2;
                                    case 4: //datatyp
                                        DataGridViewComboBoxCell cell = (grid.Rows[row].Cells[col2] as DataGridViewComboBoxCell);
                                        if (!cell.Items.Contains(temp))
                                            temp = "C";
                                        break;
                                    case 5: //kennung ->needs to be an int
                                        int dummy;
                                        if (!Int32.TryParse(temp, out dummy))
                                            temp = "0";
                                        break;
                                    case 6:
                                        goto case 2;
                                    case 7:
                                        goto case 2;
                                    default:
                                        break;
                                }
                                (grid.Rows[row].Cells[col2]).Value = temp;
                            }
                            row++;
                        }
                    }
                    finally
                    {
                        sr.Dispose();
                    }
                }
                else if (e.Alt && e.KeyCode == Keys.Up)
                {
                    if (grid.SelectedRows != null && grid.SelectedRows.Count == 1 && grid.SelectedRows[0].Index > 0)
                    {
                        int index = grid.SelectedRows[0].Index;
                        for (int i = 0; i < grid.ColumnCount; i++)
                        {
                            object temp = grid.Rows[index].Cells[i].Value;
                            grid.Rows[index].Cells[i].Value = grid.Rows[index - 1].Cells[i].Value;
                            grid.Rows[index - 1].Cells[i].Value = temp;

                        }
                    }
                }
                else if (e.Alt && e.KeyCode == Keys.Down)
                {
                    if (grid.SelectedRows != null && grid.SelectedRows.Count == 1 && grid.SelectedRows[0].Index < grid.RowCount - 2)
                    {
                        int index = grid.SelectedRows[0].Index;
                        for (int i = 0; i < grid.ColumnCount; i++)
                        {
                            object temp = grid.Rows[index].Cells[i].Value;
                            grid.Rows[index].Cells[i].Value = grid.Rows[index + 1].Cells[i].Value;
                            grid.Rows[index + 1].Cells[i].Value = temp;
                        }
                    }
                }
            }
            catch 
            {
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataGridView[] grids = { m_datagvStich, m_datagvKopf, m_datagvSchluss };
            foreach (DataGridView grid in grids)
            {
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            }
            WindowsAPI.SHAutoComplete(m_datFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        private void m_datagv_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null) return;
            //store a string representation of the row number in 'strRowNumber'
            string strRowNumber = (e.RowIndex + 1).ToString();

            //prepend leading zeros to the string if necessary to improve
            //appearance. For example, if there are ten rows in the grid,
            //row seven will be numbered as "07" instead of "7". Similarly, if 
            //there are 100 rows in the grid, row seven will be numbered as "007".
            while (strRowNumber.Length < grid.RowCount.ToString().Length) strRowNumber = "0" + strRowNumber;

            //determine the display size of the row number string using
            //the DataGridView's current font.
            SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);

            //adjust the width of the column that contains the row header cells 
            //if necessary
            if (grid.RowHeadersWidth < (int)(size.Width + 20)) grid.RowHeadersWidth = (int)(size.Width + 20);

            //this brush will be used to draw the row number string on the
            //row header cell using the system's current ControlText color
            Brush b = SystemBrushes.ControlText;

            //draw the row number string on the current row header cell using
            //the brush defined above and the DataGridView's default font
            e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        private void m_browseDatFileButton_Click(object sender, EventArgs e)
        {
			string datFile = m_datFileTextBox.Text;
			if (m_datcoHost.BrowseForDatFile(ref datFile, m_data.m_parentJob))
			{
				m_datFileTextBox.Text = datFile;
			}
		}

        private void m_selectButton_Click(object sender, EventArgs e)
        {
            using (SelectInfoOrChannels dlg = new SelectInfoOrChannels())
            {
                DataGridView grid;
                if (m_tabControl.SelectedTab == m_stichTab)
                {
                    dlg.Text = Alunorf_roh_plugin.Properties.Resources.SelectStich;
                    grid = m_datagvStich;
                    dlg.SelectChannels = false;
                }
                else if (m_tabControl.SelectedTab == m_kopfTab)
                {
                    dlg.Text = Alunorf_roh_plugin.Properties.Resources.SelectKopf;
                    grid = m_datagvKopf;
                    dlg.SelectChannels = false;
                }
                else if (m_tabControl.SelectedTab == m_schlussTab)
                {
                    dlg.Text = Alunorf_roh_plugin.Properties.Resources.SelectSchluss;
                    grid = m_datagvSchluss;
                    dlg.SelectChannels = false;
                }
                else if (m_tabControl.SelectedTab == m_kanalTab)
                {
                    dlg.Text = Alunorf_roh_plugin.Properties.Resources.SelectKanal;
                    grid = m_datagvKanalbeschreibung;
                    dlg.SelectChannels = true;
                }
                else return;
                dlg.DatFile = m_datFileTextBox.Text;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    int startrow = grid.Rows.Count - 1;
                    while (startrow >= 0 && (grid.Rows[startrow].Cells[0].Value == null || (grid.Rows[startrow].Cells[0].Value as string) == ""))
                        startrow--;
                    startrow++;
                    string[] results = dlg.SelectedItems();
                    if (results.Length == 0) return;
                    while (startrow + results.Length >= grid.RowCount) //always leav
                        grid.RowCount++;
                    for (int i = 0; i < results.Length; i++)
                    {
                        grid.Rows[startrow + i].Cells[0].Value = results[i];
                        if (dlg.AdditionalInfos.ContainsKey(results[i]))
                        {
                            ExtraData ed = dlg.AdditionalInfos[results[i]];
                            grid.Rows[startrow + i].Cells[1].Value = ed.description;
                            grid.Rows[startrow + i].Cells[2].Value = ed.kurz;
                            grid.Rows[startrow + i].Cells[3].Value = ed.unit;
                            grid.Rows[startrow + i].Cells[4].Value = ed.dt;
                        }
                    }
                }
            }
        }

        private bool m_bDatFileExists;
        private void m_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_selectButton.Enabled = m_bDatFileExists &&
                (
                m_tabControl.SelectedTab == m_stichTab || m_tabControl.SelectedTab == m_kopfTab || 
                m_tabControl.SelectedTab == m_schlussTab || m_tabControl.SelectedTab == m_kanalTab);
        }

        private void m_datFileTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                m_testRohButton.Enabled = m_bDatFileExists = File.Exists(m_datFileTextBox.Text);
                m_tabControl_SelectedIndexChanged(sender, e);
            }
            catch
            {
            }
        }

        private void m_testRohButton_Click(object sender, EventArgs e)
        {
            m_saveFileDialog.CreatePrompt = false;
            m_saveFileDialog.OverwritePrompt = true;
            m_saveFileDialog.FileName = "test";
            m_saveFileDialog.DefaultExt = "roh";
            m_saveFileDialog.Filter = "ROH Datei (*.roh)|*.roh";
            if (m_saveFileDialog.ShowDialog() != DialogResult.OK) return;
            string filename = m_saveFileDialog.FileName;
            iba.RohWriter rw = new iba.RohWriter();
            SaveData();
            int res = rw.Write(m_data.RohInput,m_data.TemplateDatFile,filename);
            if (res == 0)
                MessageBox.Show(this, Properties.Resources.TestSuccess, "ROH plugin test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                string errormessage = "";
                switch (res)
                {
                    case 1:
                        errormessage = string.Format(Properties.Resources.StichDataNotFound, rw.errorDataLineInput.ibaName, PluginRohTask.FindDataLine(m_data.RohInput.StichDaten, rw.errorDataLineInput));
                        break;
                    case 2:
                        errormessage = string.Format(Properties.Resources.KopfDataNotFound, rw.errorDataLineInput.ibaName, PluginRohTask.FindDataLine(m_data.RohInput.KopfDaten, rw.errorDataLineInput));
                        break;
                    case 3:
                        errormessage = string.Format(Properties.Resources.SchlussDataNotFound, rw.errorDataLineInput.ibaName, PluginRohTask.FindDataLine(m_data.RohInput.SchlussDaten, rw.errorDataLineInput));
                        break;
                    case 4:
                        errormessage = string.Format(Properties.Resources.KanalDataNotFound, rw.errorChannelLineInput.ibaName, PluginRohTask.FindChannelLine(m_data.RohInput.Kanalen, rw.errorChannelLineInput));
                        break;
                    case 5:
                        errormessage = string.Format(Properties.Resources.ErrorDatUnexpected, rw.errorMessage);
                        break;
                    case 6:
                        errormessage = string.Format(Properties.Resources.ErrorIbaFiles, rw.errorMessage);
                        break;
                    case 7:
                        errormessage = string.Format(Properties.Resources.ErrorIbaFilesOpen, rw.errorMessage);
                        break;
                    case 8:
                        errormessage = string.Format(Properties.Resources.ErrorRohUnexpected, rw.errorMessage);
                        break;
                    case 9:
                        errormessage = string.Format(Properties.Resources.KanalDataCouldNotBeLoaded, rw.errorChannelLineInput.ibaName, PluginRohTask.FindChannelLine(m_data.RohInput.Kanalen, rw.errorChannelLineInput));
                        break;                  
                    case 10:
                        errormessage = string.Format(Properties.Resources.ErrorRohFileCreate, rw.errorMessage);
                        break;
                    case 11:
                        errormessage = string.Format(Properties.Resources.StichDataNotRead, rw.errorDataLineInput.ibaName, PluginRohTask.FindDataLine(m_data.RohInput.StichDaten, rw.errorDataLineInput));
                        break;
                    case 12:
                        errormessage = string.Format(Properties.Resources.KopfDataNotRead, rw.errorDataLineInput.ibaName, PluginRohTask.FindDataLine(m_data.RohInput.KopfDaten, rw.errorDataLineInput));
                        break;
                    case 13:
                        errormessage = string.Format(Properties.Resources.SchlussDataNotRead, rw.errorDataLineInput.ibaName, PluginRohTask.FindDataLine(m_data.RohInput.SchlussDaten, rw.errorDataLineInput));
                        break;
                    default:
                        errormessage = string.Format(Properties.Resources.ErrorUnexpected, rw.errorMessage);
                        break;
                }
                MessageBox.Show(this, errormessage, "ROH plugin test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_datagvKanalbeschreibung_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (m_datagvKanalbeschreibung.Columns[e.ColumnIndex] == m_kanalColumnKennung)
            {
                int d;
                if (e.FormattedValue != null && !string.IsNullOrEmpty(e.FormattedValue.ToString()) && !Int32.TryParse(e.FormattedValue.ToString(), out d))
                {
                    e.Cancel = true;
                }
            }
            else if (m_datagvKanalbeschreibung.Columns[e.ColumnIndex] == m_kanalColumnFaktor)
            {
                double d;
                if (e.FormattedValue != null && !string.IsNullOrEmpty(e.FormattedValue.ToString()) && !Double.TryParse(e.FormattedValue.ToString(), out d))
                {
                    e.Cancel = true;
                }
            }
        }
    }

    class Algorithms
    {
        static public T max<T>(IEnumerable<T> container) where T : IComparable<T>
        {
            IEnumerator<T> iter = container.GetEnumerator();
            iter.MoveNext();
            T max = iter.Current;
            while (iter.MoveNext())
            {
                if (iter.Current.CompareTo(max) > 0) max = iter.Current;
            }
            return max;
        }

        static public T min<T>(IEnumerable<T> container) where T : IComparable<T>
        {
            IEnumerator<T> iter = container.GetEnumerator();
            iter.MoveNext();
            T min = iter.Current;
            while (iter.MoveNext())
            {
                if (iter.Current.CompareTo(min) < 0) min = iter.Current;
            }
            return min;
        }

        public delegate int CompareDelegate<T>(T t1, T t2);

        static public T max<T>(IEnumerable<T> container, CompareDelegate<T> comp)
        {
            IEnumerator<T> iter = container.GetEnumerator();
            iter.MoveNext();
            T max = iter.Current;
            while (iter.MoveNext())
            {
                if (comp(iter.Current, max) > 0) max = iter.Current;
            }
            return max;
        }

        static public T min<T>(IEnumerable<T> container, CompareDelegate<T> comp)
        {
            IEnumerator<T> iter = container.GetEnumerator();
            iter.MoveNext();
            T min = iter.Current;
            while (iter.MoveNext())
            {
                if (comp(iter.Current, min) < 0) min = iter.Current;
            }
            return min;
        }

        static public T max<T>(IEnumerable container, CompareDelegate<T> comp)
        {
            IEnumerator iter = container.GetEnumerator();
            iter.MoveNext();
            T max = (T)iter.Current;
            while (iter.MoveNext())
            {
                if (comp((T)iter.Current, max) > 0) max = (T)iter.Current;
            }
            return max;
        }

        static public T min<T>(IEnumerable container, CompareDelegate<T> comp)
        {
            IEnumerator iter = container.GetEnumerator();
            iter.MoveNext();
            T min = (T)iter.Current;
            while (iter.MoveNext())
            {
                if (comp((T)iter.Current, min) < 0) min = (T)iter.Current;
            }
            return min;
        }

        static public string[] SplitPaste(string s)
        {
            char[] delims = { ',', ';', '\t' };
            string[] sr_array = s.Split(delims);
            List<string> myList = new List<string>();
            bool concat = false;
            string laststring  = "";
            for (int i = 0; i < sr_array.Length; i++)
            {
                if (concat)
                {
                    laststring += ",";
                    if (sr_array[i].EndsWith("\""))
                    {
                        laststring += sr_array[i].Substring(0, sr_array[i].Length - 1);
                        concat = false;
                        myList.Add(laststring);
                    }
                    else
                        laststring += sr_array[i];
                }
                else if (sr_array[i].StartsWith("\""))
                {
                    concat = true;
                    laststring = sr_array[i].Substring(1);
                }
                else
                    myList.Add(sr_array[i]);
            }
            return myList.ToArray();
        }
    }
    #region SHAutoCompleteFlags
    [Flags]
    internal enum SHAutoCompleteFlags : uint
    {
        SHACF_DEFAULT = 0x00000000,  // Currently (SHACF_FILESYSTEM | SHACF_URLALL)
        SHACF_FILESYSTEM = 0x00000001,  // This includes the File System as well as the rest of the shell (Desktop\My Computer\Control Panel\)
        SHACF_URLHISTORY = 0x00000002,  // URLs in the User's History
        SHACF_URLMRU = 0x00000004,  // URLs in the User's Recently Used list.
        SHACF_USETAB = 0x00000008,  // Use the tab to move thru the autocomplete possibilities instead of to the next dialog/window control.
        SHACF_FILESYS_ONLY = 0x00000010,  // This includes the File System
        SHACF_FILESYS_DIRS = 0x00000020,  // Same as SHACF_FILESYS_ONLY except it only includes directories, UNC servers, and UNC server shares.
        SHACF_URLALL = (SHACF_URLHISTORY | SHACF_URLMRU),
        SHACF_AUTOSUGGEST_FORCE_ON = 0x10000000,  // Ignore the registry default and force the feature on.
        SHACF_AUTOSUGGEST_FORCE_OFF = 0x20000000,  // Ignore the registry default and force the feature off.
        SHACF_AUTOAPPEND_FORCE_ON = 0x40000000,  // Ignore the registry default and force the feature on. (Also know as AutoComplete)
        SHACF_AUTOAPPEND_FORCE_OFF = 0x80000000   // Ignore the registry default and force the feature off. (Also know as AutoComplete)
    }
    #endregion

    internal class WindowsAPI
    {
        #region Shlwapi.dll functions
        [DllImport("Shlwapi.dll")]
        public static extern int SHAutoComplete(IntPtr handle, SHAutoCompleteFlags flags);
        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    }
}
