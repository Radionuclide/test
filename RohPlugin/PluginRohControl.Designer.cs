namespace Alunorf_roh_plugin
{
    partial class PluginRohControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.m_tabControl = new System.Windows.Forms.TabControl();
            this.m_stichTab = new System.Windows.Forms.TabPage();
            this.m_datagvStich = new System.Windows.Forms.DataGridView();
            this.m_stichColumnInfoField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_stichColumnBezeichnung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_stichColumnKurz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_stichColumnEinheit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_stichColumnDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.m_kommentareTab = new System.Windows.Forms.TabPage();
            this.m_kommentare = new System.Windows.Forms.TextBox();
            this.m_kopfTab = new System.Windows.Forms.TabPage();
            this.m_datagvKopf = new System.Windows.Forms.DataGridView();
            this.m_kopfColumnInfoField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kopfColumnBezeichnung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kopfColumnKurz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kopfColumnEinheit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kopfColumnDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.m_schlussTab = new System.Windows.Forms.TabPage();
            this.m_datagvSchluss = new System.Windows.Forms.DataGridView();
            this.m_schlussColumnInfoField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_schlussColumnBezeichnung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_schlussColumnKurz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_schlussColumnEinheit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_schlussColumnDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.m_kurzbezeichnerTab = new System.Windows.Forms.TabPage();
            this.m_kurzbezeichner = new System.Windows.Forms.TextBox();
            this.m_parameterTab = new System.Windows.Forms.TabPage();
            this.m_parameter = new System.Windows.Forms.TextBox();
            this.m_kanalTab = new System.Windows.Forms.TabPage();
            this.m_datagvKanalbeschreibung = new System.Windows.Forms.DataGridView();
            this.m_kanalColumnInfoField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kanalColumnBezeichnung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kanalColumnKurz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kanalColumnEinheit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kanalColumnDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.m_kanalColumnFaktor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kanalColumnKennung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kanalColumnSollwert = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_kanalColumnStutz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_ftpTab = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.m_filePrefix = new System.Windows.Forms.TextBox();
            this.m_ftpPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.m_ftpUsername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_ftpDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_ftpHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_Panel = new System.Windows.Forms.Panel();
            this.m_testRohButton = new System.Windows.Forms.Button();
            this.m_browseDatFileButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_datFileTextBox = new System.Windows.Forms.TextBox();
            this.m_selectButton = new System.Windows.Forms.Button();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_tabControl.SuspendLayout();
            this.m_stichTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvStich)).BeginInit();
            this.m_kommentareTab.SuspendLayout();
            this.m_kopfTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvKopf)).BeginInit();
            this.m_schlussTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvSchluss)).BeginInit();
            this.m_kurzbezeichnerTab.SuspendLayout();
            this.m_parameterTab.SuspendLayout();
            this.m_kanalTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvKanalbeschreibung)).BeginInit();
            this.m_ftpTab.SuspendLayout();
            this.m_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tabControl
            // 
            this.m_tabControl.Controls.Add(this.m_stichTab);
            this.m_tabControl.Controls.Add(this.m_kommentareTab);
            this.m_tabControl.Controls.Add(this.m_kopfTab);
            this.m_tabControl.Controls.Add(this.m_schlussTab);
            this.m_tabControl.Controls.Add(this.m_kurzbezeichnerTab);
            this.m_tabControl.Controls.Add(this.m_parameterTab);
            this.m_tabControl.Controls.Add(this.m_kanalTab);
            this.m_tabControl.Controls.Add(this.m_ftpTab);
            this.m_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabControl.Location = new System.Drawing.Point(0, 0);
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            this.m_tabControl.Size = new System.Drawing.Size(565, 358);
            this.m_tabControl.TabIndex = 0;
            this.m_tabControl.SelectedIndexChanged += new System.EventHandler(this.m_tabControl_SelectedIndexChanged);
            // 
            // m_stichTab
            // 
            this.m_stichTab.Controls.Add(this.m_datagvStich);
            this.m_stichTab.Location = new System.Drawing.Point(4, 22);
            this.m_stichTab.Name = "m_stichTab";
            this.m_stichTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_stichTab.Size = new System.Drawing.Size(557, 332);
            this.m_stichTab.TabIndex = 0;
            this.m_stichTab.Text = "Stichdaten";
            this.m_stichTab.UseVisualStyleBackColor = true;
            // 
            // m_datagvStich
            // 
            this.m_datagvStich.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_datagvStich.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_stichColumnInfoField,
            this.m_stichColumnBezeichnung,
            this.m_stichColumnKurz,
            this.m_stichColumnEinheit,
            this.m_stichColumnDataType});
            this.m_datagvStich.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_datagvStich.Location = new System.Drawing.Point(3, 3);
            this.m_datagvStich.Name = "m_datagvStich";
            this.m_datagvStich.Size = new System.Drawing.Size(551, 326);
            this.m_datagvStich.TabIndex = 1;
            this.m_datagvStich.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.m_datagv_RowPostPaint);
            this.m_datagvStich.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_datagv_KeyDown);
            // 
            // m_stichColumnInfoField
            // 
            this.m_stichColumnInfoField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_stichColumnInfoField.FillWeight = 25F;
            this.m_stichColumnInfoField.HeaderText = "iba Infofeld";
            this.m_stichColumnInfoField.Name = "m_stichColumnInfoField";
            this.m_stichColumnInfoField.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_stichColumnBezeichnung
            // 
            this.m_stichColumnBezeichnung.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_stichColumnBezeichnung.FillWeight = 30F;
            this.m_stichColumnBezeichnung.HeaderText = "Bezeichnung";
            this.m_stichColumnBezeichnung.MaxInputLength = 30;
            this.m_stichColumnBezeichnung.Name = "m_stichColumnBezeichnung";
            this.m_stichColumnBezeichnung.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_stichColumnKurz
            // 
            this.m_stichColumnKurz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_stichColumnKurz.FillWeight = 20F;
            this.m_stichColumnKurz.HeaderText = "KurzBezeichnung";
            this.m_stichColumnKurz.MaxInputLength = 8;
            this.m_stichColumnKurz.Name = "m_stichColumnKurz";
            this.m_stichColumnKurz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_stichColumnEinheit
            // 
            this.m_stichColumnEinheit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_stichColumnEinheit.FillWeight = 15F;
            this.m_stichColumnEinheit.HeaderText = "Einheit";
            this.m_stichColumnEinheit.MaxInputLength = 8;
            this.m_stichColumnEinheit.Name = "m_stichColumnEinheit";
            this.m_stichColumnEinheit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_stichColumnDataType
            // 
            this.m_stichColumnDataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_stichColumnDataType.FillWeight = 10F;
            this.m_stichColumnDataType.HeaderText = "Datentyp";
            this.m_stichColumnDataType.Items.AddRange(new object[] {
            "F",
            "F4",
            "F8",
            "I",
            "I2",
            "I4",
            "C",
            "T",
            "C2"});
            this.m_stichColumnDataType.MaxDropDownItems = 10;
            this.m_stichColumnDataType.Name = "m_stichColumnDataType";
            // 
            // m_kommentareTab
            // 
            this.m_kommentareTab.Controls.Add(this.m_kommentare);
            this.m_kommentareTab.Location = new System.Drawing.Point(4, 22);
            this.m_kommentareTab.Name = "m_kommentareTab";
            this.m_kommentareTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_kommentareTab.Size = new System.Drawing.Size(557, 332);
            this.m_kommentareTab.TabIndex = 1;
            this.m_kommentareTab.Text = "Kommentare";
            this.m_kommentareTab.UseVisualStyleBackColor = true;
            // 
            // m_kommentare
            // 
            this.m_kommentare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_kommentare.Location = new System.Drawing.Point(3, 3);
            this.m_kommentare.Multiline = true;
            this.m_kommentare.Name = "m_kommentare";
            this.m_kommentare.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.m_kommentare.Size = new System.Drawing.Size(551, 326);
            this.m_kommentare.TabIndex = 0;
            // 
            // m_kopfTab
            // 
            this.m_kopfTab.Controls.Add(this.m_datagvKopf);
            this.m_kopfTab.Location = new System.Drawing.Point(4, 22);
            this.m_kopfTab.Name = "m_kopfTab";
            this.m_kopfTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_kopfTab.Size = new System.Drawing.Size(557, 332);
            this.m_kopfTab.TabIndex = 2;
            this.m_kopfTab.Text = "Kopfdaten";
            this.m_kopfTab.UseVisualStyleBackColor = true;
            // 
            // m_datagvKopf
            // 
            this.m_datagvKopf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_datagvKopf.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_kopfColumnInfoField,
            this.m_kopfColumnBezeichnung,
            this.m_kopfColumnKurz,
            this.m_kopfColumnEinheit,
            this.m_kopfColumnDataType});
            this.m_datagvKopf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_datagvKopf.Location = new System.Drawing.Point(3, 3);
            this.m_datagvKopf.Name = "m_datagvKopf";
            this.m_datagvKopf.Size = new System.Drawing.Size(551, 326);
            this.m_datagvKopf.TabIndex = 2;
            this.m_datagvKopf.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.m_datagv_RowPostPaint);
            this.m_datagvKopf.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_datagv_KeyDown);
            // 
            // m_kopfColumnInfoField
            // 
            this.m_kopfColumnInfoField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kopfColumnInfoField.FillWeight = 25F;
            this.m_kopfColumnInfoField.HeaderText = "iba Infofeld";
            this.m_kopfColumnInfoField.Name = "m_kopfColumnInfoField";
            this.m_kopfColumnInfoField.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_kopfColumnBezeichnung
            // 
            this.m_kopfColumnBezeichnung.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kopfColumnBezeichnung.FillWeight = 30F;
            this.m_kopfColumnBezeichnung.HeaderText = "Bezeichnung";
            this.m_kopfColumnBezeichnung.MaxInputLength = 30;
            this.m_kopfColumnBezeichnung.Name = "m_kopfColumnBezeichnung";
            this.m_kopfColumnBezeichnung.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_kopfColumnKurz
            // 
            this.m_kopfColumnKurz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kopfColumnKurz.FillWeight = 20F;
            this.m_kopfColumnKurz.HeaderText = "KurzBezeichnung";
            this.m_kopfColumnKurz.MaxInputLength = 8;
            this.m_kopfColumnKurz.Name = "m_kopfColumnKurz";
            this.m_kopfColumnKurz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_kopfColumnEinheit
            // 
            this.m_kopfColumnEinheit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kopfColumnEinheit.FillWeight = 15F;
            this.m_kopfColumnEinheit.HeaderText = "Einheit";
            this.m_kopfColumnEinheit.MaxInputLength = 8;
            this.m_kopfColumnEinheit.Name = "m_kopfColumnEinheit";
            this.m_kopfColumnEinheit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_kopfColumnDataType
            // 
            this.m_kopfColumnDataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kopfColumnDataType.FillWeight = 10F;
            this.m_kopfColumnDataType.HeaderText = "Datentyp";
            this.m_kopfColumnDataType.Items.AddRange(new object[] {
            "F",
            "F4",
            "F8",
            "I",
            "I2",
            "I4",
            "C",
            "T",
            "C2"});
            this.m_kopfColumnDataType.MaxDropDownItems = 10;
            this.m_kopfColumnDataType.Name = "m_kopfColumnDataType";
            // 
            // m_schlussTab
            // 
            this.m_schlussTab.Controls.Add(this.m_datagvSchluss);
            this.m_schlussTab.Location = new System.Drawing.Point(4, 22);
            this.m_schlussTab.Name = "m_schlussTab";
            this.m_schlussTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_schlussTab.Size = new System.Drawing.Size(557, 332);
            this.m_schlussTab.TabIndex = 3;
            this.m_schlussTab.Text = "Schlussdaten";
            this.m_schlussTab.UseVisualStyleBackColor = true;
            // 
            // m_datagvSchluss
            // 
            this.m_datagvSchluss.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_datagvSchluss.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_schlussColumnInfoField,
            this.m_schlussColumnBezeichnung,
            this.m_schlussColumnKurz,
            this.m_schlussColumnEinheit,
            this.m_schlussColumnDataType});
            this.m_datagvSchluss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_datagvSchluss.Location = new System.Drawing.Point(3, 3);
            this.m_datagvSchluss.Name = "m_datagvSchluss";
            this.m_datagvSchluss.Size = new System.Drawing.Size(551, 326);
            this.m_datagvSchluss.TabIndex = 2;
            this.m_datagvSchluss.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.m_datagv_RowPostPaint);
            this.m_datagvSchluss.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_datagv_KeyDown);
            // 
            // m_schlussColumnInfoField
            // 
            this.m_schlussColumnInfoField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_schlussColumnInfoField.FillWeight = 25F;
            this.m_schlussColumnInfoField.HeaderText = "iba Infofeld";
            this.m_schlussColumnInfoField.Name = "m_schlussColumnInfoField";
            this.m_schlussColumnInfoField.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_schlussColumnBezeichnung
            // 
            this.m_schlussColumnBezeichnung.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_schlussColumnBezeichnung.FillWeight = 30F;
            this.m_schlussColumnBezeichnung.HeaderText = "Bezeichnung";
            this.m_schlussColumnBezeichnung.MaxInputLength = 30;
            this.m_schlussColumnBezeichnung.Name = "m_schlussColumnBezeichnung";
            this.m_schlussColumnBezeichnung.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_schlussColumnKurz
            // 
            this.m_schlussColumnKurz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_schlussColumnKurz.FillWeight = 20F;
            this.m_schlussColumnKurz.HeaderText = "KurzBezeichnung";
            this.m_schlussColumnKurz.MaxInputLength = 8;
            this.m_schlussColumnKurz.Name = "m_schlussColumnKurz";
            this.m_schlussColumnKurz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_schlussColumnEinheit
            // 
            this.m_schlussColumnEinheit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_schlussColumnEinheit.FillWeight = 15F;
            this.m_schlussColumnEinheit.HeaderText = "Einheit";
            this.m_schlussColumnEinheit.MaxInputLength = 8;
            this.m_schlussColumnEinheit.Name = "m_schlussColumnEinheit";
            this.m_schlussColumnEinheit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_schlussColumnDataType
            // 
            this.m_schlussColumnDataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_schlussColumnDataType.FillWeight = 10F;
            this.m_schlussColumnDataType.HeaderText = "Datentyp";
            this.m_schlussColumnDataType.Items.AddRange(new object[] {
            "F",
            "F4",
            "F8",
            "I",
            "I2",
            "I4",
            "C",
            "T",
            "C2"});
            this.m_schlussColumnDataType.MaxDropDownItems = 10;
            this.m_schlussColumnDataType.Name = "m_schlussColumnDataType";
            // 
            // m_kurzbezeichnerTab
            // 
            this.m_kurzbezeichnerTab.Controls.Add(this.m_kurzbezeichner);
            this.m_kurzbezeichnerTab.Location = new System.Drawing.Point(4, 22);
            this.m_kurzbezeichnerTab.Name = "m_kurzbezeichnerTab";
            this.m_kurzbezeichnerTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_kurzbezeichnerTab.Size = new System.Drawing.Size(557, 332);
            this.m_kurzbezeichnerTab.TabIndex = 4;
            this.m_kurzbezeichnerTab.Text = "Kurzbezeichner";
            this.m_kurzbezeichnerTab.UseVisualStyleBackColor = true;
            // 
            // m_kurzbezeichner
            // 
            this.m_kurzbezeichner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_kurzbezeichner.Location = new System.Drawing.Point(3, 3);
            this.m_kurzbezeichner.Multiline = true;
            this.m_kurzbezeichner.Name = "m_kurzbezeichner";
            this.m_kurzbezeichner.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.m_kurzbezeichner.Size = new System.Drawing.Size(551, 326);
            this.m_kurzbezeichner.TabIndex = 1;
            // 
            // m_parameterTab
            // 
            this.m_parameterTab.Controls.Add(this.m_parameter);
            this.m_parameterTab.Location = new System.Drawing.Point(4, 22);
            this.m_parameterTab.Name = "m_parameterTab";
            this.m_parameterTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_parameterTab.Size = new System.Drawing.Size(557, 332);
            this.m_parameterTab.TabIndex = 5;
            this.m_parameterTab.Text = "Parameter";
            this.m_parameterTab.UseVisualStyleBackColor = true;
            // 
            // m_parameter
            // 
            this.m_parameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_parameter.Location = new System.Drawing.Point(3, 3);
            this.m_parameter.Multiline = true;
            this.m_parameter.Name = "m_parameter";
            this.m_parameter.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.m_parameter.Size = new System.Drawing.Size(551, 326);
            this.m_parameter.TabIndex = 2;
            // 
            // m_kanalTab
            // 
            this.m_kanalTab.Controls.Add(this.m_datagvKanalbeschreibung);
            this.m_kanalTab.Location = new System.Drawing.Point(4, 22);
            this.m_kanalTab.Name = "m_kanalTab";
            this.m_kanalTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_kanalTab.Size = new System.Drawing.Size(557, 332);
            this.m_kanalTab.TabIndex = 6;
            this.m_kanalTab.Text = "Kanalbeschreibung";
            this.m_kanalTab.UseVisualStyleBackColor = true;
            // 
            // m_datagvKanalbeschreibung
            // 
            this.m_datagvKanalbeschreibung.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_datagvKanalbeschreibung.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_kanalColumnInfoField,
            this.m_kanalColumnBezeichnung,
            this.m_kanalColumnKurz,
            this.m_kanalColumnEinheit,
            this.m_kanalColumnDataType,
            this.m_kanalColumnFaktor,
            this.m_kanalColumnKennung,
            this.m_kanalColumnSollwert,
            this.m_kanalColumnStutz});
            this.m_datagvKanalbeschreibung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_datagvKanalbeschreibung.Location = new System.Drawing.Point(3, 3);
            this.m_datagvKanalbeschreibung.Name = "m_datagvKanalbeschreibung";
            this.m_datagvKanalbeschreibung.Size = new System.Drawing.Size(551, 326);
            this.m_datagvKanalbeschreibung.TabIndex = 3;
            this.m_datagvKanalbeschreibung.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.m_datagvKanalbeschreibung_CellValidating);
            this.m_datagvKanalbeschreibung.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.m_datagv_RowPostPaint);
            this.m_datagvKanalbeschreibung.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_datagv_KeyDown);
            // 
            // m_kanalColumnInfoField
            // 
            this.m_kanalColumnInfoField.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kanalColumnInfoField.FillWeight = 25F;
            this.m_kanalColumnInfoField.HeaderText = "iba Kanal";
            this.m_kanalColumnInfoField.Name = "m_kanalColumnInfoField";
            this.m_kanalColumnInfoField.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_kanalColumnBezeichnung
            // 
            this.m_kanalColumnBezeichnung.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kanalColumnBezeichnung.FillWeight = 30F;
            this.m_kanalColumnBezeichnung.HeaderText = "Bezeichnung";
            this.m_kanalColumnBezeichnung.MaxInputLength = 30;
            this.m_kanalColumnBezeichnung.Name = "m_kanalColumnBezeichnung";
            this.m_kanalColumnBezeichnung.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_kanalColumnKurz
            // 
            this.m_kanalColumnKurz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kanalColumnKurz.FillWeight = 20F;
            this.m_kanalColumnKurz.HeaderText = "KurzBezeichnung";
            this.m_kanalColumnKurz.MaxInputLength = 8;
            this.m_kanalColumnKurz.Name = "m_kanalColumnKurz";
            this.m_kanalColumnKurz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_kanalColumnEinheit
            // 
            this.m_kanalColumnEinheit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kanalColumnEinheit.FillWeight = 15F;
            this.m_kanalColumnEinheit.HeaderText = "Einheit";
            this.m_kanalColumnEinheit.MaxInputLength = 8;
            this.m_kanalColumnEinheit.Name = "m_kanalColumnEinheit";
            this.m_kanalColumnEinheit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_kanalColumnDataType
            // 
            this.m_kanalColumnDataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kanalColumnDataType.FillWeight = 10F;
            this.m_kanalColumnDataType.HeaderText = "Datentyp";
            this.m_kanalColumnDataType.Items.AddRange(new object[] {
            "F",
            "F4",
            "F8",
            "I",
            "I2",
            "I4",
            "C",
            "T",
            "C2"});
            this.m_kanalColumnDataType.MaxDropDownItems = 10;
            this.m_kanalColumnDataType.Name = "m_kanalColumnDataType";
            // 
            // m_kanalColumnFaktor
            // 
            this.m_kanalColumnFaktor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kanalColumnFaktor.FillWeight = 15F;
            this.m_kanalColumnFaktor.HeaderText = "Faktor";
            this.m_kanalColumnFaktor.Name = "m_kanalColumnFaktor";
            // 
            // m_kanalColumnKennung
            // 
            this.m_kanalColumnKennung.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kanalColumnKennung.FillWeight = 15F;
            this.m_kanalColumnKennung.HeaderText = "Kennung";
            this.m_kanalColumnKennung.Name = "m_kanalColumnKennung";
            this.m_kanalColumnKennung.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_kanalColumnSollwert
            // 
            this.m_kanalColumnSollwert.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kanalColumnSollwert.FillWeight = 20F;
            this.m_kanalColumnSollwert.HeaderText = "Sollwert";
            this.m_kanalColumnSollwert.Name = "m_kanalColumnSollwert";
            this.m_kanalColumnSollwert.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_kanalColumnStutz
            // 
            this.m_kanalColumnStutz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_kanalColumnStutz.FillWeight = 20F;
            this.m_kanalColumnStutz.HeaderText = "Stützstellen";
            this.m_kanalColumnStutz.Name = "m_kanalColumnStutz";
            this.m_kanalColumnStutz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_ftpTab
            // 
            this.m_ftpTab.Controls.Add(this.label4);
            this.m_ftpTab.Controls.Add(this.m_filePrefix);
            this.m_ftpTab.Controls.Add(this.m_ftpPassword);
            this.m_ftpTab.Controls.Add(this.label6);
            this.m_ftpTab.Controls.Add(this.m_ftpUsername);
            this.m_ftpTab.Controls.Add(this.label5);
            this.m_ftpTab.Controls.Add(this.m_ftpDirectory);
            this.m_ftpTab.Controls.Add(this.label2);
            this.m_ftpTab.Controls.Add(this.m_ftpHost);
            this.m_ftpTab.Controls.Add(this.label1);
            this.m_ftpTab.Location = new System.Drawing.Point(4, 22);
            this.m_ftpTab.Name = "m_ftpTab";
            this.m_ftpTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_ftpTab.Size = new System.Drawing.Size(557, 332);
            this.m_ftpTab.TabIndex = 7;
            this.m_ftpTab.Text = "FTP";
            this.m_ftpTab.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 36);
            this.label4.TabIndex = 11;
            this.label4.Text = "Dateinamen Prefix:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // m_filePrefix
            // 
            this.m_filePrefix.Location = new System.Drawing.Point(91, 181);
            this.m_filePrefix.Name = "m_filePrefix";
            this.m_filePrefix.Size = new System.Drawing.Size(189, 20);
            this.m_filePrefix.TabIndex = 10;
            // 
            // m_ftpPassword
            // 
            this.m_ftpPassword.Location = new System.Drawing.Point(91, 129);
            this.m_ftpPassword.Name = "m_ftpPassword";
            this.m_ftpPassword.Size = new System.Drawing.Size(189, 20);
            this.m_ftpPassword.TabIndex = 9;
            this.m_ftpPassword.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 132);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Password:";
            // 
            // m_ftpUsername
            // 
            this.m_ftpUsername.Location = new System.Drawing.Point(91, 103);
            this.m_ftpUsername.Name = "m_ftpUsername";
            this.m_ftpUsername.Size = new System.Drawing.Size(189, 20);
            this.m_ftpUsername.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Username:";
            // 
            // m_ftpDirectory
            // 
            this.m_ftpDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ftpDirectory.Location = new System.Drawing.Point(91, 51);
            this.m_ftpDirectory.Name = "m_ftpDirectory";
            this.m_ftpDirectory.Size = new System.Drawing.Size(424, 20);
            this.m_ftpDirectory.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Directory:";
            // 
            // m_ftpHost
            // 
            this.m_ftpHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ftpHost.Location = new System.Drawing.Point(91, 25);
            this.m_ftpHost.Name = "m_ftpHost";
            this.m_ftpHost.Size = new System.Drawing.Size(424, 20);
            this.m_ftpHost.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host:";
            // 
            // m_Panel
            // 
            this.m_Panel.Controls.Add(this.m_testRohButton);
            this.m_Panel.Controls.Add(this.m_browseDatFileButton);
            this.m_Panel.Controls.Add(this.label3);
            this.m_Panel.Controls.Add(this.m_datFileTextBox);
            this.m_Panel.Controls.Add(this.m_selectButton);
            this.m_Panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_Panel.Location = new System.Drawing.Point(0, 358);
            this.m_Panel.Name = "m_Panel";
            this.m_Panel.Size = new System.Drawing.Size(565, 52);
            this.m_Panel.TabIndex = 1;
            // 
            // m_testRohButton
            // 
            this.m_testRohButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_testRohButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_testRohButton.Location = new System.Drawing.Point(515, 6);
            this.m_testRohButton.Name = "m_testRohButton";
            this.m_testRohButton.Size = new System.Drawing.Size(40, 40);
            this.m_testRohButton.TabIndex = 15;
            this.m_testRohButton.UseVisualStyleBackColor = true;
            this.m_testRohButton.Click += new System.EventHandler(this.m_testRohButton_Click);
            // 
            // m_browseDatFileButton
            // 
            this.m_browseDatFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseDatFileButton.Image = global::Alunorf_roh_plugin.Properties.Resources.open;
            this.m_browseDatFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseDatFileButton.Location = new System.Drawing.Point(423, 6);
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_browseDatFileButton.Size = new System.Drawing.Size(40, 40);
            this.m_browseDatFileButton.TabIndex = 13;
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(7, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Vorlage .dat Datei:";
            // 
            // m_datFileTextBox
            // 
            this.m_datFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_datFileTextBox.Location = new System.Drawing.Point(108, 17);
            this.m_datFileTextBox.Name = "m_datFileTextBox";
            this.m_datFileTextBox.Size = new System.Drawing.Size(309, 20);
            this.m_datFileTextBox.TabIndex = 12;
            this.m_datFileTextBox.TextChanged += new System.EventHandler(this.m_datFileTextBox_TextChanged);
            // 
            // m_selectButton
            // 
            this.m_selectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_selectButton.Image = global::Alunorf_roh_plugin.Properties.Resources.select;
            this.m_selectButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_selectButton.Location = new System.Drawing.Point(469, 6);
            this.m_selectButton.Name = "m_selectButton";
            this.m_selectButton.Size = new System.Drawing.Size(40, 40);
            this.m_selectButton.TabIndex = 14;
            this.m_selectButton.UseVisualStyleBackColor = true;
            this.m_selectButton.Click += new System.EventHandler(this.m_selectButton_Click);
            // 
            // PluginRohControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_tabControl);
            this.Controls.Add(this.m_Panel);
            this.MinimumSize = new System.Drawing.Size(0, 410);
            this.Name = "PluginRohControl";
            this.Size = new System.Drawing.Size(565, 410);
            this.m_tabControl.ResumeLayout(false);
            this.m_stichTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvStich)).EndInit();
            this.m_kommentareTab.ResumeLayout(false);
            this.m_kommentareTab.PerformLayout();
            this.m_kopfTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvKopf)).EndInit();
            this.m_schlussTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvSchluss)).EndInit();
            this.m_kurzbezeichnerTab.ResumeLayout(false);
            this.m_kurzbezeichnerTab.PerformLayout();
            this.m_parameterTab.ResumeLayout(false);
            this.m_parameterTab.PerformLayout();
            this.m_kanalTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvKanalbeschreibung)).EndInit();
            this.m_ftpTab.ResumeLayout(false);
            this.m_ftpTab.PerformLayout();
            this.m_Panel.ResumeLayout(false);
            this.m_Panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl m_tabControl;
        private System.Windows.Forms.TabPage m_stichTab;
        private System.Windows.Forms.TabPage m_kommentareTab;
        private System.Windows.Forms.Panel m_Panel;
        private System.Windows.Forms.Button m_browseDatFileButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_datFileTextBox;
        private System.Windows.Forms.Button m_selectButton;
        private System.Windows.Forms.TabPage m_kopfTab;
        private System.Windows.Forms.TabPage m_schlussTab;
        private System.Windows.Forms.TabPage m_kurzbezeichnerTab;
        private System.Windows.Forms.TabPage m_parameterTab;
        private System.Windows.Forms.TabPage m_kanalTab;
        private System.Windows.Forms.TabPage m_ftpTab;
        private System.Windows.Forms.TextBox m_ftpHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_ftpDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_ftpPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox m_ftpUsername;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView m_datagvStich;
        private System.Windows.Forms.DataGridView m_datagvKopf;
        private System.Windows.Forms.DataGridView m_datagvSchluss;
        private System.Windows.Forms.DataGridView m_datagvKanalbeschreibung;
        private System.Windows.Forms.TextBox m_kommentare;
        private System.Windows.Forms.TextBox m_kurzbezeichner;
        private System.Windows.Forms.TextBox m_parameter;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.Button m_testRohButton;
        private System.Windows.Forms.SaveFileDialog m_saveFileDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_stichColumnInfoField;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_stichColumnBezeichnung;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_stichColumnKurz;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_stichColumnEinheit;
        private System.Windows.Forms.DataGridViewComboBoxColumn m_stichColumnDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kopfColumnInfoField;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kopfColumnBezeichnung;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kopfColumnKurz;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kopfColumnEinheit;
        private System.Windows.Forms.DataGridViewComboBoxColumn m_kopfColumnDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_schlussColumnInfoField;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_schlussColumnBezeichnung;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_schlussColumnKurz;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_schlussColumnEinheit;
        private System.Windows.Forms.DataGridViewComboBoxColumn m_schlussColumnDataType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox m_filePrefix;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kanalColumnInfoField;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kanalColumnBezeichnung;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kanalColumnKurz;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kanalColumnEinheit;
        private System.Windows.Forms.DataGridViewComboBoxColumn m_kanalColumnDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kanalColumnFaktor;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kanalColumnKennung;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kanalColumnSollwert;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_kanalColumnStutz;
    }
}
