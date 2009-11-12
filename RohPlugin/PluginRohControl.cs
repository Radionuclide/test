using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using iba.Plugins;

namespace Alunorf_roh_plugin
{
    public partial class PluginRohControl : UserControl, IPluginControl
    {
        private IDatCoHost m_datcoHost;
        public PluginRohControl(IDatCoHost host)
        {
            m_datcoHost = host;
            InitializeComponent();
        }

        #region IPluginControl Members

        PluginRohTask m_data;
        ICommonTaskControl m_control;

        public void LoadData(object datasource, ICommonTaskControl parentcontrol)
        {
            m_data = datasource as PluginRohTask;
            m_control = parentcontrol;

            m_ftpDirectory.Text = m_data.FtpDirectory;
            m_ftpHost.Text = m_data.FtpDirectory;
            m_nudFtpPort.Value = m_data.FtpPort;
            m_ftpUsername.Text = m_data.FtpUser;
            m_ftpPassword.Text = m_data.FtpPassword;

            m_parameter.Text = m_data.RohInput.Parameter;
            m_kurzbezeichner.Text = m_data.RohInput.Kurzbezeichner;
            m_kommentare.Text = m_data.RohInput.Kommentare;

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
                }
                grid.Rows[i].Cells[5].Value = dataset2[i].Kennung.ToString();
                grid.Rows[i].Cells[6].Value = dataset2[i].Sollwert;
                grid.Rows[i].Cells[7].Value = dataset2[i].Stutzstellen;
            }
        }

        public void SaveData()
        {
            m_data.FtpDirectory = m_ftpDirectory.Text;
            m_data.FtpHost = m_ftpHost.Text;
            m_data.FtpPort = (int)m_nudFtpPort.Value;
            m_data.FtpUser = m_ftpUsername.Text;
            m_data.FtpPassword = m_ftpPassword.Text;
            m_data.RohInput.Parameter = m_parameter.Text;
            m_data.RohInput.Kommentare = m_kommentare.Text;
            m_data.RohInput.Kurzbezeichner = m_kurzbezeichner.Text;

            iba.RohWriterDataLineInput[][] datasets = { m_data.RohInput.StichDaten, m_data.RohInput.KopfDaten, m_data.RohInput.SchlussDaten};
            DataGridView[] grids = { m_datagvStich, m_datagvKopf, m_datagvSchluss };
            DataGridView grid = null;
            int count;
            for (int j = 0; j < 3; j++)
            {
                grid = grids[j];
                iba.RohWriterDataLineInput[] dataset = datasets[j];
                count = grid.RowCount;
                if (dataset.Length!=0)
                    Array.Clear(dataset,0,dataset.Length);
                for (int i = 0; i < count; i++)
                {
                    string ibaName = grid.Rows[i].Cells[0].Value as string;
                    string bezeichnung = grid.Rows[i].Cells[1].Value as string;
                    string kurz = grid.Rows[i].Cells[2].Value as string;
                    if (String.IsNullOrEmpty(ibaName) || String.IsNullOrEmpty(bezeichnung) || String.IsNullOrEmpty(kurz)) continue;
                    iba.RohWriterDataLineInput line = new iba.RohWriterDataLineInput();
                    line.ibaName = ibaName.Trim();
                    bezeichnung = bezeichnung.Trim();
                    if (bezeichnung.Length > 8) bezeichnung = bezeichnung.Substring(0, 8);
                    line.Bezeichnung = bezeichnung;
                    kurz = kurz.Trim();
                    if (kurz.Length > 8) kurz = bezeichnung.Substring(0, 8);
                    line.KurzBezeichnung = kurz;
                    string einheit = grid.Rows[i].Cells[3] as string;
                    if (einheit == null) einheit = "";
                    einheit = einheit.Trim();

                    line.Einheit = einheit;

                }
            }
        }

        public void LeaveCleanup()
        {//nothing to clean up
        }

        #endregion

        private void m_datagvKanalbeschreibung_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if ((int)(((System.Windows.Forms.DataGridView)(sender)).CurrentCell.ColumnIndex) == 5) //Kennung column
            {
                e.Control.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextboxNumeric_KeyPress);

            }   
        }

        private void TextboxNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            Boolean nonNumberEntered;

            nonNumberEntered = true;

            if ((e.KeyChar >= 48 && e.KeyChar <= 57))
            {
                nonNumberEntered = false;
            }

            if (nonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }

        }
    }
}
