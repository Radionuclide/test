namespace iba
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.m_statusStrip = new System.Windows.Forms.StatusStrip();
            this.m_statusBarStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.m_navBar = new TD.Eyefinder.NavigationBar();
            this.m_settingsPane = new TD.Eyefinder.NavigationPane();
            this.m_watchdogPane = new TD.Eyefinder.NavigationPane();
            this.m_loggingPane = new TD.Eyefinder.NavigationPane();
            this.label2 = new System.Windows.Forms.Label();
            this.m_rbOnlyErrors = new System.Windows.Forms.RadioButton();
            this.m_rbErrorsWarnings = new System.Windows.Forms.RadioButton();
            this.m_rbAllLog = new System.Windows.Forms.RadioButton();
            this.m_EntriesNumericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.m_statusPane = new TD.Eyefinder.NavigationPane();
            this.m_statusTreeView = new System.Windows.Forms.TreeView();
            this.m_configPane = new TD.Eyefinder.NavigationPane();
            this.m_configTreeView = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_stopButton = new System.Windows.Forms.Button();
            this.m_startButton = new System.Windows.Forms.Button();
            this.m_rightPane = new TD.Eyefinder.HeaderControl();
            this.m_menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.watchdogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VersionHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_btnClearLogging = new System.Windows.Forms.Button();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.m_statusStrip.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.m_navBar.SuspendLayout();
            this.m_loggingPane.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_EntriesNumericUpDown1)).BeginInit();
            this.m_statusPane.SuspendLayout();
            this.m_configPane.SuspendLayout();
            this.panel1.SuspendLayout();
            this.m_menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.m_statusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer2);
            resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.m_menuStrip);
            // 
            // m_statusStrip
            // 
            resources.ApplyResources(this.m_statusStrip, "m_statusStrip");
            this.m_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_statusBarStripLabel});
            this.m_statusStrip.Name = "m_statusStrip";
            // 
            // m_statusBarStripLabel
            // 
            this.m_statusBarStripLabel.Name = "m_statusBarStripLabel";
            resources.ApplyResources(this.m_statusBarStripLabel, "m_statusBarStripLabel");
            this.m_statusBarStripLabel.Spring = true;
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.m_navBar);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.m_rightPane);
            // 
            // m_navBar
            // 
            this.m_navBar.Controls.Add(this.m_settingsPane);
            this.m_navBar.Controls.Add(this.m_watchdogPane);
            this.m_navBar.Controls.Add(this.m_loggingPane);
            this.m_navBar.Controls.Add(this.m_statusPane);
            this.m_navBar.Controls.Add(this.m_configPane);
            this.m_navBar.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.m_navBar, "m_navBar");
            this.m_navBar.HeaderFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.m_navBar.Name = "m_navBar";
            this.m_navBar.PaneFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.m_navBar.SelectedPane = this.m_loggingPane;
            this.m_navBar.ShowPanes = 5;
            this.m_navBar.SelectedPaneChanged += new System.EventHandler(this.navbar_SelectedPaneChanged);
            // 
            // m_settingsPane
            // 
            resources.ApplyResources(this.m_settingsPane, "m_settingsPane");
            this.m_settingsPane.Name = "m_settingsPane";
            // 
            // m_watchdogPane
            // 
            resources.ApplyResources(this.m_watchdogPane, "m_watchdogPane");
            this.m_watchdogPane.Name = "m_watchdogPane";
            // 
            // m_loggingPane
            // 
            this.m_loggingPane.Controls.Add(this.m_btnClearLogging);
            this.m_loggingPane.Controls.Add(this.label2);
            this.m_loggingPane.Controls.Add(this.m_rbOnlyErrors);
            this.m_loggingPane.Controls.Add(this.m_rbErrorsWarnings);
            this.m_loggingPane.Controls.Add(this.m_rbAllLog);
            this.m_loggingPane.Controls.Add(this.m_EntriesNumericUpDown1);
            this.m_loggingPane.Controls.Add(this.label1);
            resources.ApplyResources(this.m_loggingPane, "m_loggingPane");
            this.m_loggingPane.Name = "m_loggingPane";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_rbOnlyErrors
            // 
            resources.ApplyResources(this.m_rbOnlyErrors, "m_rbOnlyErrors");
            this.m_rbOnlyErrors.Name = "m_rbOnlyErrors";
            this.m_rbOnlyErrors.TabStop = true;
            this.m_rbOnlyErrors.UseVisualStyleBackColor = true;
            this.m_rbOnlyErrors.CheckedChanged += new System.EventHandler(this.m_rbLogLevelRbCheckedChanged);
            // 
            // m_rbErrorsWarnings
            // 
            resources.ApplyResources(this.m_rbErrorsWarnings, "m_rbErrorsWarnings");
            this.m_rbErrorsWarnings.Name = "m_rbErrorsWarnings";
            this.m_rbErrorsWarnings.TabStop = true;
            this.m_rbErrorsWarnings.UseVisualStyleBackColor = true;
            this.m_rbErrorsWarnings.CheckedChanged += new System.EventHandler(this.m_rbLogLevelRbCheckedChanged);
            // 
            // m_rbAllLog
            // 
            resources.ApplyResources(this.m_rbAllLog, "m_rbAllLog");
            this.m_rbAllLog.Name = "m_rbAllLog";
            this.m_rbAllLog.TabStop = true;
            this.m_rbAllLog.UseVisualStyleBackColor = true;
            this.m_rbAllLog.CheckedChanged += new System.EventHandler(this.m_rbLogLevelRbCheckedChanged);
            // 
            // m_EntriesNumericUpDown1
            // 
            resources.ApplyResources(this.m_EntriesNumericUpDown1, "m_EntriesNumericUpDown1");
            this.m_EntriesNumericUpDown1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.m_EntriesNumericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_EntriesNumericUpDown1.Name = "m_EntriesNumericUpDown1";
            this.m_EntriesNumericUpDown1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.m_EntriesNumericUpDown1.ValueChanged += new System.EventHandler(this.m_EntriesNumericUpDown1_ValueChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_statusPane
            // 
            this.m_statusPane.Controls.Add(this.m_statusTreeView);
            resources.ApplyResources(this.m_statusPane, "m_statusPane");
            this.m_statusPane.Name = "m_statusPane";
            // 
            // m_statusTreeView
            // 
            resources.ApplyResources(this.m_statusTreeView, "m_statusTreeView");
            this.m_statusTreeView.HideSelection = false;
            this.m_statusTreeView.Name = "m_statusTreeView";
            this.m_statusTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnStatusTreeViewAfterSelect);
            // 
            // m_configPane
            // 
            this.m_configPane.Controls.Add(this.m_configTreeView);
            this.m_configPane.Controls.Add(this.panel1);
            resources.ApplyResources(this.m_configPane, "m_configPane");
            this.m_configPane.Name = "m_configPane";
            // 
            // m_configTreeView
            // 
            this.m_configTreeView.AllowDrop = true;
            resources.ApplyResources(this.m_configTreeView, "m_configTreeView");
            this.m_configTreeView.HideSelection = false;
            this.m_configTreeView.Name = "m_configTreeView";
            this.m_configTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.m_configTreeView_DragDrop);
            this.m_configTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnConfigurationTreeViewAfterSelect);
            this.m_configTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_configTreeView_MouseDown);
            this.m_configTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_configTreeView_KeyDown);
            this.m_configTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.m_configTreeView_ItemDrag);
            this.m_configTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.m_configTreeView_DragOver);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_stopButton);
            this.panel1.Controls.Add(this.m_startButton);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // m_stopButton
            // 
            resources.ApplyResources(this.m_stopButton, "m_stopButton");
            this.m_stopButton.Image = global::iba.Properties.Resources.Stop;
            this.m_stopButton.Name = "m_stopButton";
            this.m_stopButton.UseVisualStyleBackColor = true;
            this.m_stopButton.Click += new System.EventHandler(this.m_stopButton_Click);
            // 
            // m_startButton
            // 
            resources.ApplyResources(this.m_startButton, "m_startButton");
            this.m_startButton.Image = global::iba.Properties.Resources.Start;
            this.m_startButton.Name = "m_startButton";
            this.m_startButton.UseVisualStyleBackColor = true;
            this.m_startButton.Click += new System.EventHandler(this.m_startButton_Click);
            // 
            // m_rightPane
            // 
            resources.ApplyResources(this.m_rightPane, "m_rightPane");
            this.m_rightPane.HeaderFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.m_rightPane.Name = "m_rightPane";
            this.m_rightPane.Leave += new System.EventHandler(this.m_rightPane_Leave);
            this.m_rightPane.Enter += new System.EventHandler(this.m_rightPane_Enter);
            // 
            // m_menuStrip
            // 
            resources.ApplyResources(this.m_menuStrip, "m_menuStrip");
            this.m_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.serviceToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.m_menuStrip.Name = "m_menuStrip";
            this.m_menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // newToolStripMenuItem
            // 
            resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
            // 
            // saveToolStripMenuItem
            // 
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // cutToolStripMenuItem
            // 
            resources.ApplyResources(this.cutToolStripMenuItem, "cutToolStripMenuItem");
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.statusToolStripMenuItem,
            this.loggingToolStripMenuItem,
            this.watchdogToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            resources.ApplyResources(this.configurationToolStripMenuItem, "configurationToolStripMenuItem");
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            resources.ApplyResources(this.statusToolStripMenuItem, "statusToolStripMenuItem");
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // loggingToolStripMenuItem
            // 
            this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            resources.ApplyResources(this.loggingToolStripMenuItem, "loggingToolStripMenuItem");
            this.loggingToolStripMenuItem.Click += new System.EventHandler(this.loggingToolStripMenuItem_Click);
            // 
            // watchdogToolStripMenuItem
            // 
            this.watchdogToolStripMenuItem.Name = "watchdogToolStripMenuItem";
            resources.ApplyResources(this.watchdogToolStripMenuItem, "watchdogToolStripMenuItem");
            this.watchdogToolStripMenuItem.Click += new System.EventHandler(this.watchdogToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // serviceToolStripMenuItem
            // 
            this.serviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startServiceToolStripMenuItem,
            this.stopServiceToolStripMenuItem});
            this.serviceToolStripMenuItem.Name = "serviceToolStripMenuItem";
            resources.ApplyResources(this.serviceToolStripMenuItem, "serviceToolStripMenuItem");
            this.serviceToolStripMenuItem.DropDownOpening += new System.EventHandler(this.serviceToolStripMenuItem_DropDownOpening);
            // 
            // startServiceToolStripMenuItem
            // 
            this.startServiceToolStripMenuItem.Name = "startServiceToolStripMenuItem";
            resources.ApplyResources(this.startServiceToolStripMenuItem, "startServiceToolStripMenuItem");
            this.startServiceToolStripMenuItem.Click += new System.EventHandler(this.miStartService_Click);
            // 
            // stopServiceToolStripMenuItem
            // 
            this.stopServiceToolStripMenuItem.Name = "stopServiceToolStripMenuItem";
            resources.ApplyResources(this.stopServiceToolStripMenuItem, "stopServiceToolStripMenuItem");
            this.stopServiceToolStripMenuItem.Click += new System.EventHandler(this.miStopService_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveInformationToolStripMenuItem,
            this.VersionHistoryToolStripMenuItem,
            this.toolStripSeparator1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // saveInformationToolStripMenuItem
            // 
            this.saveInformationToolStripMenuItem.Name = "saveInformationToolStripMenuItem";
            resources.ApplyResources(this.saveInformationToolStripMenuItem, "saveInformationToolStripMenuItem");
            this.saveInformationToolStripMenuItem.Click += new System.EventHandler(this.saveInformationToolStripMenuItem_Click);
            // 
            // VersionHistoryToolStripMenuItem
            // 
            this.VersionHistoryToolStripMenuItem.Name = "VersionHistoryToolStripMenuItem";
            resources.ApplyResources(this.VersionHistoryToolStripMenuItem, "VersionHistoryToolStripMenuItem");
            this.VersionHistoryToolStripMenuItem.Click += new System.EventHandler(this.VersionHistoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // m_openFileDialog
            // 
            this.m_openFileDialog.FileName = "openFileDialog1";
            // 
            // m_btnClearLogging
            // 
            resources.ApplyResources(this.m_btnClearLogging, "m_btnClearLogging");
            this.m_btnClearLogging.Name = "m_btnClearLogging";
            this.m_btnClearLogging.UseVisualStyleBackColor = true;
            this.m_btnClearLogging.Click += new System.EventHandler(this.m_btnClearLogging_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.m_statusStrip.ResumeLayout(false);
            this.m_statusStrip.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.m_navBar.ResumeLayout(false);
            this.m_loggingPane.ResumeLayout(false);
            this.m_loggingPane.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_EntriesNumericUpDown1)).EndInit();
            this.m_statusPane.ResumeLayout(false);
            this.m_configPane.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.m_menuStrip.ResumeLayout(false);
            this.m_menuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private TD.Eyefinder.NavigationBar m_navBar;
        private TD.Eyefinder.NavigationPane m_statusPane;
        private TD.Eyefinder.NavigationPane m_configPane;
        private TD.Eyefinder.HeaderControl m_rightPane;
        private System.Windows.Forms.TreeView m_configTreeView;
        private System.Windows.Forms.TreeView m_statusTreeView;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip m_menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog m_saveFileDialog;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private TD.Eyefinder.NavigationPane m_loggingPane;
        private System.Windows.Forms.ToolStripMenuItem loggingToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown m_EntriesNumericUpDown1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button m_startButton;
        private System.Windows.Forms.Button m_stopButton;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.StatusStrip m_statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel m_statusBarStripLabel;
        private System.Windows.Forms.ToolStripMenuItem serviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private TD.Eyefinder.NavigationPane m_watchdogPane;
        private System.Windows.Forms.ToolStripMenuItem watchdogToolStripMenuItem;
        private TD.Eyefinder.NavigationPane m_settingsPane;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripMenuItem saveInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem VersionHistoryToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton m_rbOnlyErrors;
        private System.Windows.Forms.RadioButton m_rbErrorsWarnings;
        private System.Windows.Forms.RadioButton m_rbAllLog;
        private System.Windows.Forms.Button m_btnClearLogging;
    }
}

